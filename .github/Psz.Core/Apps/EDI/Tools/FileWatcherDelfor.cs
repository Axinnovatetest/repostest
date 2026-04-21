using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using File = System.IO.File;

namespace Psz.Core.Apps.EDI.Tools
{
	public class FileWatcherDelfor
	{
		private FileSystemWatcher fileSystemWatcher;
		private string _newOrdersDirectory { get; set; }
		private string _errorOrderDirectory { get; set; }
		private string _processedOrdersDirectory { get; set; }
		private string _archiveOrdersDirectory { get; set; }

		public FileWatcherDelfor(
			string newOrdersDirectory,
			string errorOrderDirectory,
			string processedOrdersDirectory,
			string archiveOrdersDirectory
			)
		{
			try
			{
				_errorOrderDirectory = errorOrderDirectory;
				_newOrdersDirectory = newOrdersDirectory;
				_processedOrdersDirectory = processedOrdersDirectory;
				_archiveOrdersDirectory = archiveOrdersDirectory;

				Task.Run(() => processFiles());

				fileSystemWatcher = new FileSystemWatcher(newOrdersDirectory);

				// Watch all files.
				fileSystemWatcher.Filter = "";
				// -
				fileSystemWatcher.Created += new FileSystemEventHandler(fileCreated);
				fileSystemWatcher.Changed += new FileSystemEventHandler(fileCreated);
				fileSystemWatcher.Renamed += new RenamedEventHandler(fileCreated);

				// Start watching
				fileSystemWatcher.EnableRaisingEvents = true;

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, e.StackTrace);
				throw;
			}
		}

		private void fileCreated(object sender, FileSystemEventArgs e)
		{
			//lock (Locks.DocumentsLock)
			{
				Task.Run(() => processFiles());
			}
		}

		#region > New
		private void processFiles()
		{
			lock(Locks.DocumentsLock)
			{
				Thread.Sleep(1100);
				var moveFileToError = false;
				foreach(var file in new DirectoryInfo(_newOrdersDirectory).GetFiles()?.OrderBy(x => x.LastWriteTime))
				{
					try
					{
						var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Models.Delfor.ErpelIndustryMessage));
						Models.Delfor.ErpelIndustryMessage erpelIndustryMessage = null;

						using(var stream = new FileStream(file.FullName, FileMode.Open))
						{
							try
							{
								erpelIndustryMessage = (Models.Delfor.ErpelIndustryMessage)xmlSerializer.Deserialize(stream);
							} catch(Exception e)
							{
								Infrastructure.Data.Access.Tables.CTS.ErrorAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.ErrorEntity
								{
									BuyerDuns = null,
									Documentnumber = "", // not null
									ErrorMessage = e.Message,
									ErrorTrace = e.StackTrace,
									FileName = GetDestinationFileName(file.Name, _errorOrderDirectory),
									Id = -1,
									ProcessTime = System.DateTime.Now,
									RecipientId = "-1",
									SenderId = "-1",
									Validated = false,
									ValidationTime = null,
									ValidationUserId = null,
								});
								Infrastructure.Data.Access.Tables.CTS.ProcessedFileAccess.Insert(
									new Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity
									{
										Id = -1,
										FileName = file.FullName,
										ProcessStatus = (int)Enums.DelforEnums.FileProcessStatus.Error,
										ProcessStatusName = Enums.DelforEnums.FileProcessStatus.Error.GetDescription(),
										ProcessStatusTrace = $"{e.Message}\\{e.StackTrace}",
										ProcessTime = System.DateTime.Now
									});

								moveFileToError = true;
								Infrastructure.Services.Logging.Logger.Log(e);
							}
						}

						if(moveFileToError)
						{
							moveFromNewFolder(file.FullName, _errorOrderDirectory);
							moveFileToError = false;
							// -
							continue;
						}

						// - process file xml data
						Models.Delfor.XMLHeaderModel header = null;
						List<Models.Delfor.XMLLineItemModel> lineItems = null;
						List<Models.Delfor.XMLLineItemPlanModel> lineItemPlans = null;
						getFormattedData(erpelIndustryMessage, ref header, ref lineItems, ref lineItemPlans);

						#region Header integrity check
						var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
						botransaction.beginTransaction();
						var validData = ValidateData(header, lineItems, lineItemPlans, botransaction.connection, botransaction.transaction);
						if(validData != null && validData.Count > 0)
						{
							var dest = moveFromNewFolder(file.FullName, _errorOrderDirectory);
							validData.ForEach(x => x.FileName = dest);
							Infrastructure.Data.Access.Tables.CTS.ErrorAccess.Insert(validData.GroupBy(x => new { x.FileName, x.SenderId, x.RecipientId, x.Documentnumber, x.ErrorMessage }).Select(x => x.First()).ToList());
							continue;
						}
						#endregion
						if(header != null && !string.IsNullOrWhiteSpace(header.DocumentNumber))
						{

							try
							{
								#region // -- transaction-based logic -- //

								header = updateConsigneeLocation(header, botransaction);
								// - 2023-03-23 - Set Consignee from Customer.LS_ADR if XML section empty
								header = updateBuyerNConsigneeData(header, botransaction);
								var adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByDunsNumber(header?.BuyerDUNS, botransaction.connection, botransaction.transaction);
								header.PszCustomerNumber = adressDb?.Nr ?? -1;

								// - 2025-03-25 - exit if customer is not edi active
								if(adressDb.EDI_Aktiv is null || adressDb.EDI_Aktiv == false)
								{
									Infrastructure.Data.Access.Tables.CTS.ErrorAccess.Insert(
										new Infrastructure.Data.Entities.Tables.CTS.ErrorEntity
										{
											FileName = file.FullName,
											SenderId = header.SenderId,
											RecipientId = header.RecipientId,
											Documentnumber = header.DocumentNumber,
											ErrorMessage = $"Customer [{header.BuyerPartyName}] is not EDI active",
											ProcessTime = System.DateTime.Now,
										});
									moveFromNewFolder(file.FullName, _errorOrderDirectory);
									moveFileToError = false;
									continue;
								}

								var headerEntity = header.ToEntity();
								if(headerEntity != null &&
									(string.IsNullOrWhiteSpace(headerEntity.SupplierDUNS) ||
									string.IsNullOrWhiteSpace(headerEntity.SupplierPartyName) ||
									string.IsNullOrWhiteSpace(headerEntity.SupplierPostCode) ||
									string.IsNullOrWhiteSpace(headerEntity.SupplierStreet) ||
									string.IsNullOrWhiteSpace(headerEntity.SupplierCity) ||
									string.IsNullOrWhiteSpace(headerEntity.SupplierCountryName) ||
									string.IsNullOrWhiteSpace(headerEntity.SupplierContactTelephone) ||
									string.IsNullOrWhiteSpace(headerEntity.SupplierContactFax)))
								{
									var pszAdressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetPsz();
									if(string.IsNullOrWhiteSpace(headerEntity.SupplierDUNS))
									{
										headerEntity.SupplierDUNS = pszAdressEntity.Duns ?? string.Empty;
									}
									if(string.IsNullOrWhiteSpace(headerEntity.SupplierPartyName))
									{
										headerEntity.SupplierPartyName = pszAdressEntity.Name1 ?? string.Empty;
									}
									if(string.IsNullOrWhiteSpace(headerEntity.SupplierPostCode))
									{
										headerEntity.SupplierPostCode = pszAdressEntity.PLZ_StraBe ?? string.Empty;
									}
									if(string.IsNullOrWhiteSpace(headerEntity.SupplierStreet))
									{
										headerEntity.SupplierStreet = pszAdressEntity.StraBe ?? string.Empty;
									}
									if(string.IsNullOrWhiteSpace(headerEntity.SupplierCity))
									{
										headerEntity.SupplierCity = pszAdressEntity.Ort ?? string.Empty;
									}
									if(string.IsNullOrWhiteSpace(headerEntity.SupplierCountryName))
									{
										headerEntity.SupplierCountryName = pszAdressEntity.Land ?? string.Empty;
									}
									if(string.IsNullOrWhiteSpace(headerEntity.SupplierContactTelephone))
									{
										headerEntity.SupplierContactTelephone = pszAdressEntity.Telefon ?? string.Empty;
									}
									if(string.IsNullOrWhiteSpace(headerEntity.SupplierContactFax))
									{
										headerEntity.SupplierContactFax = pszAdressEntity.Fax ?? string.Empty;
									}
								}
								var headerId = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.InsertWithTransaction(headerEntity, botransaction.connection, botransaction.transaction);
								if(headerId > 0 && lineItems != null && lineItems.Count > 0)
								{
									// - Unique DocunemtNumber
									if(Infrastructure.Data.Access.Tables.CTS.HeaderAccess.ExistsDocumentNumber(header.DocumentNumber, botransaction.connection, botransaction.transaction))
									{
										if(lineItems != null && lineItems.Count > 0)
										{
											// - get Article by Customer, first
											var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(adressDb?.Kundennummer ?? -1, botransaction.connection, botransaction.transaction);
											var articleXmlEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(lineItems.Select(x => x.SuppliersItemMaterialNumber ?? "").ToList(), botransaction.connection, botransaction.transaction);
											var articleConcernEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(
												 Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetCustomerNumbersInSameConcern(adressDb?.Kundennummer ?? -1)
												 ?.ToList());
											var lItems = new List<Models.Delfor.XMLLineItemPlanModel>();
											foreach(var item in lineItems)
											{
												item.HeaderId = headerId;
												item.HeaderVersion = header.ReferenceVersionNumber;
												item.HeaderPreviousVersion = header.PreviousReferenceVersionNumber;
												item.PSZArtikelNr = item.PSZArtikelNr <= 0 ? articleEntities.FirstOrDefault(x => x.ArtikelNummer?.Trim()?.ToLower() == item.SuppliersItemMaterialNumber?.Trim()?.ToLower())?.ArtikelNr ?? -1 : item.PSZArtikelNr;
												item.PSZArtikelNr = item.PSZArtikelNr <= 0 ? articleConcernEntities.FirstOrDefault(x => x.ArtikelNummer?.Trim()?.ToLower() == item.SuppliersItemMaterialNumber?.Trim()?.ToLower())?.ArtikelNr ?? -1 : item.PSZArtikelNr;
												item.PSZArtikelNr = item.PSZArtikelNr <= 0 ? articleXmlEntities.FirstOrDefault(x => x.ArtikelNummer?.Trim()?.ToLower() == item.SuppliersItemMaterialNumber?.Trim()?.ToLower())?.ArtikelNr ?? -1 : item.PSZArtikelNr;

												////// - replace XML PSZ# by default EDI, if any
												////var defaultEdiArticle = articleEntities?.FirstOrDefault(x => x.CustomerItemNumber?.ToLower()?.Trim() == item.CustomersItemMaterialNumber?.ToLower()?.Trim());
												////if(defaultEdiArticle != null && item.SuppliersItemMaterialNumber?.ToLower()?.Trim() != defaultEdiArticle.ArtikelNummer?.ToLower()?.Trim())
												////{
												////	item.SuppliersItemMaterialNumber = defaultEdiArticle.ArtikelNummer;
												////	item.PSZArtikelNr = defaultEdiArticle.ArtikelNr;
												////}
												////else
												////{
												////	// - if XML article is Obsolete, chose any other article from the same CustomerItemNumber (Bz1 is used here)
												////	var xmlArticle = articleXmlEntities.FirstOrDefault(x => x.CustomerItemNumber?.ToLower()?.Trim() == item.CustomersItemMaterialNumber?.ToLower()?.Trim());
												////	if(xmlArticle?.Freigabestatus?.ToLower()?.Trim() == "o")
												////	{
												////		var sp = item.SuppliersItemMaterialNumber?.ToLower()?.Trim().Split("-");
												////		var sizeKreis = (sp != null && sp.Length > 1 ? sp[0] : item.SuppliersItemMaterialNumber?.ToLower()?.Trim()).Length;
												////		var validArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumbersOrBz(new List<string> { item.CustomersItemMaterialNumber })
												////			?.OrderByDescending(x => x.ArtikelNr)
												////			?.FirstOrDefault(x => x.Freigabestatus?.ToLower()?.Trim() != "o" && x.ArtikelNummer?.ToLower()?.Trim().Substring(0, sizeKreis) == item.SuppliersItemMaterialNumber?.ToLower()?.Trim().Substring(0, sizeKreis));
												////		if(validArticle != null)
												////		{
												////			item.SuppliersItemMaterialNumber = validArticle.ArtikelNummer;
												////			item.PSZArtikelNr = validArticle.ArtikelNr;
												////		}
												////	}
												////}

												var itemId = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.InsertWithTransaction(item.ToEntity(), botransaction.connection, botransaction.transaction);
												var itemPlans = lineItemPlans.Where(x => x.PositionNumber == item.PositionNumber)?.ToList();
												if(itemId > 0 && itemPlans != null && itemPlans.Count > 0)
												{
													itemPlans.ForEach(x => x.LineItemId = itemId);
													lItems.AddRange(itemPlans);
												}
											}
											Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.InsertWithTransaction(lItems.Select(x => x.ToEntity())?.ToList(), botransaction.connection, botransaction.transaction);
										}
									}
									else
									{
										// - get Article by Customer, first
										var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(adressDb?.Kundennummer ?? -1, botransaction.connection, botransaction.transaction);
										var articleXmlEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(lineItems.Select(x => x.SuppliersItemMaterialNumber ?? "").ToList(), botransaction.connection, botransaction.transaction);
										var articleConcernEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(
											 Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetCustomerNumbersInSameConcern(adressDb?.Kundennummer ?? -1)
											 ?.ToList());
										var lItems = new List<Models.Delfor.XMLLineItemPlanModel>();
										foreach(var item in lineItems)
										{
											item.HeaderId = headerId;
											item.PSZArtikelNr = item.PSZArtikelNr <= 0 ? articleEntities.FirstOrDefault(x => x.ArtikelNummer?.Trim()?.ToLower() == item.SuppliersItemMaterialNumber?.Trim()?.ToLower())?.ArtikelNr ?? -1 : item.PSZArtikelNr;
											item.PSZArtikelNr = item.PSZArtikelNr <= 0 ? articleConcernEntities.FirstOrDefault(x => x.ArtikelNummer?.Trim()?.ToLower() == item.SuppliersItemMaterialNumber?.Trim()?.ToLower())?.ArtikelNr ?? -1 : item.PSZArtikelNr;
											item.PSZArtikelNr = item.PSZArtikelNr <= 0 ? articleXmlEntities.FirstOrDefault(x => x.ArtikelNummer?.Trim()?.ToLower() == item.SuppliersItemMaterialNumber?.Trim()?.ToLower())?.ArtikelNr ?? -1 : item.PSZArtikelNr;
											////// - replace XML PSZ# by default EDI, if any
											////var defaultEdiArticle = articleEntities?.FirstOrDefault(x => x.CustomerItemNumber?.ToLower()?.Trim() == item.CustomersItemMaterialNumber?.ToLower()?.Trim());
											////if(defaultEdiArticle != null && item.SuppliersItemMaterialNumber?.ToLower()?.Trim() != defaultEdiArticle.ArtikelNummer?.ToLower()?.Trim())
											////{
											////	item.SuppliersItemMaterialNumber = defaultEdiArticle.ArtikelNummer;
											////	item.PSZArtikelNr = defaultEdiArticle.ArtikelNr;
											////}
											////else
											////{
											////	// - if XML article is Obsolete, chose any other article from the same CustomerItemNumber (Bz1 is used here)
											////	var xmlArticle = articleXmlEntities.FirstOrDefault(x => x.ArtikelNummer?.ToLower()?.Trim() == item.SuppliersItemMaterialNumber?.ToLower()?.Trim());
											////	if(xmlArticle.Freigabestatus?.ToLower()?.Trim() == "o")
											////	{
											////		var sp = item.SuppliersItemMaterialNumber?.ToLower()?.Trim().Split("-");
											////		var sizeKreis = (sp != null && sp.Length > 1 ? sp[0] : item.SuppliersItemMaterialNumber?.ToLower()?.Trim()).Length;
											////		var validArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumbersOrBz(new List<string> { item.CustomersItemMaterialNumber })
											////			?.OrderByDescending(x => x.ArtikelNr)
											////			?.FirstOrDefault(x => x.Freigabestatus?.ToLower()?.Trim() != "o" && x.ArtikelNummer?.ToLower()?.Trim().Substring(0, sizeKreis) == item.SuppliersItemMaterialNumber?.ToLower()?.Trim().Substring(0, sizeKreis));
											////		if(validArticle != null)
											////		{
											////			item.SuppliersItemMaterialNumber = validArticle.ArtikelNummer;
											////			item.PSZArtikelNr = validArticle.ArtikelNr;
											////		}
											////	}
											////}

											var itemId = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.InsertWithTransaction(item.ToEntity(), botransaction.connection, botransaction.transaction);
											var itemPlans = lineItemPlans.Where(x => x.PositionNumber == item.PositionNumber)?.ToList();
											if(itemId > 0 && itemPlans != null && itemPlans.Count > 0)
											{
												itemPlans.ForEach(x => x.LineItemId = itemId);
												lItems.AddRange(itemPlans);
											}
										}
										// - 
										Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.InsertWithTransaction(lItems.Select(x => x.ToEntity())?.ToList(), botransaction.connection, botransaction.transaction);
									}
								}

								Infrastructure.Data.Access.Tables.CTS.ProcessedFileAccess.InsertWithTransaction(
									new Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity
									{
										Id = -1,
										FileName = file.FullName,
										ProcessStatus = (int)Enums.DelforEnums.FileProcessStatus.Success,
										ProcessStatusName = Enums.DelforEnums.FileProcessStatus.Success.GetDescription(),
										ProcessStatusTrace = null,
										ProcessTime = System.DateTime.Now
									}, botransaction.connection, botransaction.transaction);

								#endregion // -- transaction-based logic -- //

								//TODO: handle transaction state (success or failure)
								if(botransaction.commit())
								{
									moveFromNewFolder(file.FullName, _processedOrdersDirectory);
								}
								else
								{
									throw new Exception("Transaction Error");
								}
							} catch(Exception e)
							{
								botransaction.rollback();
								Infrastructure.Services.Logging.Logger.Log(e);
								throw;
							}
						}
						else
						{
							// - 
							Infrastructure.Data.Access.Tables.CTS.ProcessedFileAccess.Insert(
								new Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity
								{
									Id = -1,
									FileName = file.FullName,
									ProcessStatus = (int)Enums.DelforEnums.FileProcessStatus.Error,
									ProcessStatusName = Enums.DelforEnums.FileProcessStatus.Error.GetDescription(),
									ProcessStatusTrace = $"XML Header Null",
									ProcessTime = System.DateTime.Now
								});

							moveFromNewFolder(file.FullName, _errorOrderDirectory);
						}
					} catch(Exception externalEx)
					{
						Infrastructure.Services.Logging.Logger.Log(externalEx);
					}
				}
			}
		}

		private Models.Delfor.XMLHeaderModel updateBuyerNConsigneeData(Models.Delfor.XMLHeaderModel xMLHeader, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			// - Customer
			var customerAddressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(int.TryParse(xMLHeader.BuyerPartyIdentification, out var n) ? n : -1);
			if(string.IsNullOrWhiteSpace(xMLHeader.BuyerPartyName)
				|| string.IsNullOrEmpty(xMLHeader.BuyerPartyName))
			{
				if(customerAddressEntity != null)
				{
					var contactEntities = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(customerAddressEntity.Nr);
					xMLHeader.BuyerContactName = contactEntities != null && contactEntities.Count > 0 ? contactEntities[0].Ansprechpartner : "";
					xMLHeader.BuyerContactTelephone = contactEntities != null && contactEntities.Count > 0 ? contactEntities[0].Telefon : customerAddressEntity.Telefon;
					xMLHeader.BuyerDUNS = customerAddressEntity.Duns;
					xMLHeader.BuyerPartyName = customerAddressEntity.Name1;
					xMLHeader.BuyerPurchasingDepartment = contactEntities != null && contactEntities.Count > 0 ? (!string.IsNullOrWhiteSpace(contactEntities[0].Abteilung) ? contactEntities[0].Abteilung : customerAddressEntity.Abteilung) : customerAddressEntity.Abteilung;
				}
			}


			// - Consignee
			var customerEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(customerAddressEntity?.Nr ?? -1);
			var deliveryAddressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerEntity?.LSADR ?? -1);
			if(string.IsNullOrWhiteSpace(xMLHeader.ConsigneePartyName)
				|| string.IsNullOrEmpty(xMLHeader.ConsigneePartyName))
			{
				// -  2023-06-28 - init
				xMLHeader.ConsigneeAddress = "";
				xMLHeader.ConsigneeCity = "";
				xMLHeader.ConsigneeContactFax = "";
				xMLHeader.ConsigneeContactTelephone = "";
				xMLHeader.ConsigneeCountryName = "";
				xMLHeader.ConsigneeDUNS = "";
				xMLHeader.ConsigneePartyIdentification = "";
				xMLHeader.ConsigneePartyName = "";
				xMLHeader.ConsigneePostCode = "";
				xMLHeader.ConsigneeStorageLocation = "";
				xMLHeader.ConsigneeStreet = "";
				xMLHeader.ConsigneeUnloadingPoint = "";

				if(deliveryAddressEntity != null)
				{
					xMLHeader.ConsigneeAddress = "";
					xMLHeader.ConsigneeCity = deliveryAddressEntity.Ort;
					xMLHeader.ConsigneeContactFax = deliveryAddressEntity.Fax;
					xMLHeader.ConsigneeContactTelephone = deliveryAddressEntity.Telefon;
					xMLHeader.ConsigneeCountryName = deliveryAddressEntity.Land;
					xMLHeader.ConsigneeDUNS = deliveryAddressEntity.Duns;
					xMLHeader.ConsigneePartyIdentification = deliveryAddressEntity.Kundennummer?.ToString() ?? "";
					xMLHeader.ConsigneePartyName = deliveryAddressEntity.Name1;
					xMLHeader.ConsigneePostCode = deliveryAddressEntity.PLZ_StraBe;
					xMLHeader.ConsigneeStorageLocation = deliveryAddressEntity.StorageLocation;
					xMLHeader.ConsigneeStreet = deliveryAddressEntity.StraBe;
					xMLHeader.ConsigneeUnloadingPoint = deliveryAddressEntity.UnloadingPoint;
				}
			}

			// - 2024-09-18 - complete data if missing parts
			if(!string.IsNullOrWhiteSpace(xMLHeader.BuyerPartyName) && customerAddressEntity != null && xMLHeader.BuyerPartyName.Trim().ToLower() == customerAddressEntity.Name1?.Trim()?.ToLower())
			{
				var contactEntities = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(customerAddressEntity.Nr);
				if(string.IsNullOrWhiteSpace(xMLHeader.BuyerContactName))
				{
					xMLHeader.BuyerContactName = contactEntities != null && contactEntities.Count > 0 ? contactEntities[0].Ansprechpartner : "";
				}
				if(string.IsNullOrWhiteSpace(xMLHeader.BuyerContactTelephone))
				{
					xMLHeader.BuyerContactTelephone = contactEntities != null && contactEntities.Count > 0 ? contactEntities[0].Telefon : customerAddressEntity.Telefon;
				}
				if(string.IsNullOrWhiteSpace(xMLHeader.BuyerDUNS))
				{
					xMLHeader.BuyerDUNS = customerAddressEntity.Duns;
				}

				if(string.IsNullOrWhiteSpace(xMLHeader.BuyerPurchasingDepartment))
				{
					xMLHeader.BuyerPurchasingDepartment = contactEntities != null && contactEntities.Count > 0 ? (!string.IsNullOrWhiteSpace(contactEntities[0].Abteilung) ? contactEntities[0].Abteilung : customerAddressEntity.Abteilung) : customerAddressEntity.Abteilung;
				}
			}
			// - consignee
			if(!string.IsNullOrWhiteSpace(xMLHeader.ConsigneePartyName) && deliveryAddressEntity != null && xMLHeader.ConsigneePartyName.Trim().ToLower() == deliveryAddressEntity.Name1?.Trim()?.ToLower())
			{
				if(string.IsNullOrWhiteSpace(xMLHeader.ConsigneeCity))
				{
					xMLHeader.ConsigneeCity = deliveryAddressEntity.Ort;
				}
				if(string.IsNullOrWhiteSpace(xMLHeader.ConsigneeContactFax))
				{
					xMLHeader.ConsigneeContactFax = deliveryAddressEntity.Fax;
				}
				if(string.IsNullOrWhiteSpace(xMLHeader.ConsigneeContactTelephone))
				{
					xMLHeader.ConsigneeContactTelephone = deliveryAddressEntity.Telefon;
				}
				if(string.IsNullOrWhiteSpace(xMLHeader.ConsigneeCountryName))
				{
					xMLHeader.ConsigneeCountryName = deliveryAddressEntity.Land;
				}
				if(string.IsNullOrWhiteSpace(xMLHeader.ConsigneeDUNS))
				{
					xMLHeader.ConsigneeDUNS = deliveryAddressEntity.Duns;
				}
				if(string.IsNullOrWhiteSpace(xMLHeader.ConsigneePartyIdentification))
				{
					xMLHeader.ConsigneePartyIdentification = deliveryAddressEntity.Kundennummer?.ToString() ?? "";
				}
				if(string.IsNullOrWhiteSpace(xMLHeader.ConsigneePostCode))
				{
					xMLHeader.ConsigneePostCode = deliveryAddressEntity.PLZ_StraBe;
				}
				if(string.IsNullOrWhiteSpace(xMLHeader.ConsigneeStreet))
				{
					xMLHeader.ConsigneeStreet = deliveryAddressEntity.StraBe;
				}
			}

			// - Supplier - Set PSZ data, if empty
			if(string.IsNullOrWhiteSpace(xMLHeader.SupplierPartyName)
				|| string.IsNullOrWhiteSpace(xMLHeader.SupplierDUNS)
				|| string.IsNullOrWhiteSpace(xMLHeader.SupplierPartyIdentification)
				|| string.IsNullOrWhiteSpace(xMLHeader.SupplierStreet)
				|| string.IsNullOrWhiteSpace(xMLHeader.SupplierPostCode))
			{
				var supplierEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetPsz();
				if(supplierEntity != null)
				{
					xMLHeader.SupplierCity = supplierEntity.Ort;
					xMLHeader.SupplierContactFax = supplierEntity.Fax;
					xMLHeader.SupplierContactTelephone = supplierEntity.Telefon;
					xMLHeader.SupplierCountryName = supplierEntity.Land;
					xMLHeader.SupplierDUNS = supplierEntity.Duns;
					xMLHeader.SupplierPartyIdentification = supplierEntity.Lieferantennummer?.ToString();
					xMLHeader.SupplierPartyName = supplierEntity.Name1;
					xMLHeader.SupplierPostCode = supplierEntity.PLZ_StraBe;
					xMLHeader.SupplierStreet = supplierEntity.StraBe;
				}
			}
			// - 
			return xMLHeader;
		}
		private Models.Delfor.XMLHeaderModel updateConsigneeLocation(Models.Delfor.XMLHeaderModel xMLHeader, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			if(xMLHeader == null)
				return xMLHeader;

			// - 2025-06-11 replace weird chars in string // KION e.g WÃ¶hlerstrasse
			if(Psz.Core.Helpers.StringHelpers.ContainsMojibake(xMLHeader.ConsigneeStreet))
			{
				xMLHeader.ConsigneeStreet = Psz.Core.Helpers.StringHelpers.FixMojibake(xMLHeader.ConsigneeStreet);
			}

			if(string.IsNullOrWhiteSpace(xMLHeader.ConsigneeDUNS)
				|| (!string.IsNullOrWhiteSpace(xMLHeader.ConsigneeStorageLocation) && (!string.IsNullOrWhiteSpace(xMLHeader.ConsigneeUnloadingPoint))))
				return xMLHeader;

			// - 2022-11-17
			var __duns = long.TryParse(xMLHeader.ConsigneeDUNS, out var _duns) ? _duns : -1;
			var isECOSIO = Infrastructure.Data.Access.Tables.PRS.AdresseECOSIOAccess.IsECOSIOByDuns(xMLHeader.ConsigneeDUNS ?? "", botransaction.connection, botransaction.transaction); // Only HORSCH
			if(isECOSIO)
			{
				var ecosioAddress = Infrastructure.Data.Access.Tables.PRS.AdresseECOSIOAccess.GetAllByDuns(__duns, botransaction.connection, botransaction.transaction);
				if(ecosioAddress == null || ecosioAddress.Count <= 0 || ecosioAddress.Count > 1)
					return xMLHeader;

				xMLHeader.ConsigneeStorageLocation = ecosioAddress[0].AnlieferLagerort?.ToString();
				xMLHeader.ConsigneeUnloadingPoint = ecosioAddress[0].Werksnummer?.ToString();

			}
			var adress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByDunsNumber(xMLHeader.BuyerDUNS, botransaction.connection, botransaction.transaction);
			if(adress == null)
			{
				var extensionAdress = Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.GetByDuns(xMLHeader.BuyerDUNS, botransaction.connection, botransaction.transaction);
				if(extensionAdress == null)
					return xMLHeader;
				else
					xMLHeader.PszCustomerNumber = extensionAdress?.AdressenNr;
			}
			else
				xMLHeader.PszCustomerNumber = adress?.Nr;


			return xMLHeader;
		}

		private void getFormattedData(Models.Delfor.ErpelIndustryMessage xmlMessage, ref Models.Delfor.XMLHeaderModel headerData, ref List<Models.Delfor.XMLLineItemModel> lineItems, ref List<Models.Delfor.XMLLineItemPlanModel> lineItemPlans)
		{
			headerData = new Models.Delfor.XMLHeaderModel(xmlMessage.ErpelBusinessDocumentHeader?.InterchangeHeader, xmlMessage.Document.Header);

			if(xmlMessage.Document?.Details?.ForecastData?.ForecastLineItem != null)
			{
				lineItems = new List<Models.Delfor.XMLLineItemModel>();
				lineItemPlans = new List<Models.Delfor.XMLLineItemPlanModel>();
				foreach(var lineItem in xmlMessage.Document?.Details?.ForecastData?.ForecastLineItem)
				{
					lineItems.Add(new Models.Delfor.XMLLineItemModel(lineItem, -1, headerData.ReferenceVersionNumber ?? -1, headerData.PreviousReferenceVersionNumber ?? -1, headerData.DocumentNumber));
					if(lineItem.PlanningQuantity != null)
					{
						var planningQuantities = new List<Models.Delfor.XMLLineItemPlanModel>();
						foreach(var planningQuantityItem in lineItem.PlanningQuantity)
						{
							planningQuantities.Add(new Models.Delfor.XMLLineItemPlanModel(planningQuantityItem, -1, lineItem.PositionNumber));
						}
						lineItemPlans.AddRange(planningQuantities);
					}
				}
			}
		}
		private bool validateDataHeader(Models.Delfor.XMLHeaderModel headerData, out List<string> errors)
		{
			errors = new List<string>();
			if(headerData == null)
				return true;


			if(string.IsNullOrWhiteSpace(headerData.SenderId) || string.IsNullOrEmpty(headerData.SenderId))
				errors.Add($"Invalid SenderId: [{headerData.SenderId}]");

			if(string.IsNullOrWhiteSpace(headerData.RecipientId) || string.IsNullOrEmpty(headerData.RecipientId))
				errors.Add($"Invalid RecipientId: [{headerData.RecipientId}]");

			if(string.IsNullOrWhiteSpace(headerData.MessageType) || string.IsNullOrEmpty(headerData.MessageType) || headerData.MessageType.Trim().ToLower() != "delfor")
				errors.Add($"Invalid MessageType: [{headerData.MessageType}]");

			if(string.IsNullOrWhiteSpace(headerData.DocumentNumber) || string.IsNullOrEmpty(headerData.DocumentNumber))
				errors.Add($"Invalid DocumentNumber: [{headerData.DocumentNumber}]");

			if(string.IsNullOrWhiteSpace(headerData.ReferenceNumber) || string.IsNullOrEmpty(headerData.ReferenceNumber))
				errors.Add($"Invalid ReferenceNumber: [{headerData.ReferenceNumber}]");

			if(!headerData.ReferenceVersionNumber.HasValue)
				errors.Add($"Invalid ReferenceVersionNumber: [{headerData.ReferenceVersionNumber}]");

			// - 2023-03-23 - if PartyIdentification is empty, then following fields are required
			if(string.IsNullOrWhiteSpace(headerData.BuyerPartyIdentification) || string.IsNullOrEmpty(headerData.BuyerPartyIdentification))
			{
				// - 2023-03-23 - Take these fields from PartyIdentification (CustomerNumber)
				if(string.IsNullOrWhiteSpace(headerData.BuyerDUNS) || string.IsNullOrEmpty(headerData.BuyerDUNS))
					errors.Add($"Invalid BuyerDUNS: [{headerData.BuyerDUNS}]");
				if(string.IsNullOrWhiteSpace(headerData.BuyerPartyName) || string.IsNullOrEmpty(headerData.BuyerPartyName))
					errors.Add($"Invalid BuyerPartyName: [{headerData.BuyerPartyName}]");
				if(string.IsNullOrWhiteSpace(headerData.BuyerContactName) || string.IsNullOrEmpty(headerData.BuyerContactName))
					errors.Add($"Invalid BuyerContactName: [{headerData.BuyerContactName}]");
				//if(string.IsNullOrWhiteSpace(headerData.BuyerContactTelephone) || string.IsNullOrEmpty(headerData.BuyerContactTelephone))
				//	errors.Add($"Invalid BuyerContactTelephone: {headerData.BuyerContactTelephone}");
			}
			else
			{
				var kundenNr = int.TryParse(headerData.BuyerPartyIdentification, out var n) ? n : -1;
				var adress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(kundenNr);
				if(adress == null)
				{
					errors.Add($"Customer [{headerData.BuyerPartyIdentification}] not found");
				}
				var lastDocumentVersion = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetLastVersion(adress?.Nr ?? -1, headerData.DocumentNumber);
				if(lastDocumentVersion != null && lastDocumentVersion.ReferenceVersionNumber >= headerData.ReferenceVersionNumber)
					errors.Add($"Wrong Header version | Document: [{headerData.DocumentNumber}], LastVersionNumber: [{lastDocumentVersion.ReferenceVersionNumber}], XMLVersionNumber: [{headerData.ReferenceVersionNumber}]");
			}

			if(!string.IsNullOrWhiteSpace(headerData.BuyerDUNS) && !string.IsNullOrEmpty(headerData.BuyerDUNS))
			{
				var adress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByDunsNumber(headerData.BuyerDUNS);
				if(adress == null)
				{
					var extensionAdress = Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.GetByDuns(headerData.BuyerDUNS);
					if(extensionAdress == null)
						errors.Add($"Customer Duns not found: [{headerData.BuyerDUNS}]");
				}
				var lastDocumentVersion = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetLastVersion(headerData.BuyerDUNS, headerData.DocumentNumber);
				if(lastDocumentVersion != null && lastDocumentVersion.ReferenceVersionNumber >= headerData.ReferenceVersionNumber)
					errors.Add($"Wrong Header version | Document: [{headerData.DocumentNumber}], LastVersionNumber: [{lastDocumentVersion.ReferenceVersionNumber}], XMLVersionNumber: [{headerData.ReferenceVersionNumber}]");
			}

			//if(string.IsNullOrWhiteSpace(headerData.SupplierDUNS) || string.IsNullOrEmpty(headerData.SupplierDUNS))
			//	errors.Add($"Invalid SupplierDUNS: {headerData.SupplierDUNS}");

			// -- 2023-03-23 - Consignee is OPTIONAL -> Take Customer.LS_ADR if empty
			/*
			if(string.IsNullOrWhiteSpace(headerData.ConsigneeDUNS) || string.IsNullOrEmpty(headerData.ConsigneeDUNS))
				errors.Add($"Invalid ConsigneeDUNS: [{headerData.ConsigneeDUNS}]");
			if(string.IsNullOrWhiteSpace(headerData.ConsigneePartyName) || string.IsNullOrEmpty(headerData.ConsigneePartyName))
				errors.Add($"Invalid ConsigneePartyName: [{headerData.ConsigneePartyName}]");
			if(string.IsNullOrWhiteSpace(headerData.ConsigneeStreet) || string.IsNullOrEmpty(headerData.ConsigneeStreet))
				errors.Add($"Invalid ConsigneeStreet: [{headerData.ConsigneeStreet}]");
			if(string.IsNullOrWhiteSpace(headerData.ConsigneeCity) || string.IsNullOrEmpty(headerData.ConsigneeCity))
				errors.Add($"Invalid ConsigneeCity: [{headerData.ConsigneeCity}]");
			if(string.IsNullOrWhiteSpace(headerData.ConsigneePostCode) || string.IsNullOrEmpty(headerData.ConsigneePostCode))
				errors.Add($"Invalid ConsigneePostCode: [{headerData.ConsigneePostCode}]");
			if(string.IsNullOrWhiteSpace(headerData.ConsigneePartyIdentification) || string.IsNullOrEmpty(headerData.ConsigneePartyIdentification))
				errors.Add($"Invalid ConsigneePartyIdentification: [{headerData.ConsigneePartyIdentification}]");
			*/


			if(errors.Count > 0)
				return false;

			return true;
		}
		private bool validateDataLineItem(List<Models.Delfor.XMLLineItemModel> lineItems, Models.Delfor.XMLHeaderModel headerData,
			SqlConnection connection, SqlTransaction transaction, out List<string> errors)
		{
			errors = new List<string>();

			var adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByDunsNumber(headerData?.BuyerDUNS)
				?? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(int.TryParse(headerData?.BuyerPartyIdentification, out var n) ? n : -1);

			var concernItemEntity = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetByCustomerNumber(adressDb?.Kundennummer ?? -1);
			var concernEntity = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.Get(concernItemEntity?.ConcernId ?? -1);
			var includeBz1 = concernEntity?.IncludeDescription ?? false;
			var trimLeadingZeros = concernEntity?.TrimLeadingZeros ?? false;

			var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(adressDb?.Kundennummer ?? -1);
			var articleConcernEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(
			 Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetCustomerNumbersInSameConcern(adressDb?.Kundennummer ?? -1)
			 ?.ToList());
			var allCustomerArticleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetAllCustomersEdi(true);

			// - 2023-08-09
			for(int i = 0; i < allCustomerArticleEntities.Count; i++)
			{
				allCustomerArticleEntities[i].CustomerItemNumber = allCustomerArticleEntities[i].CustomerItemNumber?.ToLower()?.Trim();
				// - 
				if(trimLeadingZeros)
				{
					allCustomerArticleEntities[i].CustomerItemNumber = allCustomerArticleEntities[i].CustomerItemNumber?.TrimStart('0');
				}
			}
			for(int i = 0; i < articleEntities.Count(); i++)
			{
				articleEntities[i].CustomerItemNumber = articleEntities[i].CustomerItemNumber?.ToLower()?.Trim();
				articleEntities[i].Bezeichnung1 = articleEntities[i].Bezeichnung1?.ToLower()?.Trim();
				// - 
				if(trimLeadingZeros)
				{
					articleEntities[i].CustomerItemNumber = articleEntities[i].CustomerItemNumber?.TrimStart('0');
					articleEntities[i].Bezeichnung1 = articleEntities[i].Bezeichnung1?.TrimStart('0');
				}
			}
			for(int i = 0; i < articleConcernEntities.Count; i++)
			{
				articleConcernEntities[i].CustomerItemNumber = articleConcernEntities[i].CustomerItemNumber?.ToLower()?.Trim();
				articleConcernEntities[i].Bezeichnung1 = articleConcernEntities[i].Bezeichnung1?.ToLower()?.Trim();
				// - 
				if(trimLeadingZeros)
				{
					articleConcernEntities[i].CustomerItemNumber = articleConcernEntities[i].CustomerItemNumber?.TrimStart('0');
					articleConcernEntities[i].Bezeichnung1 = articleConcernEntities[i].Bezeichnung1?.TrimStart('0');
				}
			}

			var foundByCustomerItemNumber = false;
			foreach(var item in lineItems)
			{
				foundByCustomerItemNumber = true;
				if(item.PositionNumber <= 0)
					errors.Add($"Invalid PositionNumber [{item.PositionNumber}]");
				if(!item.CallOffNumber.HasValue)
					errors.Add($"Invalid CallOffNumber [{item.CallOffNumber}]");
				if(!item.CallOffDateTime.HasValue)
					errors.Add($"Invalid CallOffDateTime [{item.CallOffDateTime}]");
				if(!item.CumulativeReceivedQuantity.HasValue || item.CumulativeReceivedQuantity.Value < 0)
					errors.Add($"Invalid CumulativeReceivedQuantity [{item.CumulativeReceivedQuantity}]");
				if(!item.CumulativeScheduledQuantity.HasValue || item.CumulativeScheduledQuantity.Value < 0)
					errors.Add($"Invalid CumulativeScheduledQuantity [{item.CumulativeScheduledQuantity}]");
				if(!item.LastReceivedQuantity.HasValue || item.LastReceivedQuantity.Value < 0)
					errors.Add($"Invalid LastReceivedQuantity [{item.LastReceivedQuantity}]");

				if(string.IsNullOrEmpty(item.CustomersItemMaterialNumber) || string.IsNullOrWhiteSpace(item.CustomersItemMaterialNumber))
					errors.Add($"Invalid CustomersItemMaterialNumber [{item.CustomersItemMaterialNumber}]");
				else
				{
					// - 2023-08-09
					item.CustomersItemMaterialNumber = item.CustomersItemMaterialNumber?.ToLower()?.Trim();
					if(trimLeadingZeros)
					{
						item.CustomersItemMaterialNumber = item.CustomersItemMaterialNumber?.TrimStart('0');
					}

					// - 2023-08-09 - not all customer article
					var _articles = allCustomerArticleEntities.Where(x => x.CustomerItemNumber == item.CustomersItemMaterialNumber)?.ToList();
					if(_articles != null && _articles.Count > 0)
					{
						item.CustomersItemMaterialNumber = _articles[0].CustomerItemNumber;
						item.PSZArtikelNr = _articles[0].ArtikelNr;
						item.SuppliersItemMaterialNumber = _articles[0].ArtikelNummer;
					}
					else
					{
						var defaultEdiArticle = articleEntities?.FirstOrDefault(x => x.CustomerItemNumber == item.CustomersItemMaterialNumber
						|| (includeBz1 == true && x.Bezeichnung1 == item.CustomersItemMaterialNumber));
						if(defaultEdiArticle != null)
						{
							item.CustomersItemMaterialNumber = defaultEdiArticle.CustomerItemNumber;
							item.PSZArtikelNr = defaultEdiArticle.ArtikelNr;
							item.SuppliersItemMaterialNumber = defaultEdiArticle.ArtikelNummer;
						}
						else
						{
							var defaultEdiArticles_concern = articleConcernEntities?.Where(x => x.CustomerItemNumber == item.CustomersItemMaterialNumber
							|| (includeBz1 == true && x.Bezeichnung1 == item.CustomersItemMaterialNumber))?.ToList();
							if(defaultEdiArticles_concern != null && defaultEdiArticles_concern.Count > 0)
							{
								item.CustomersItemMaterialNumber = defaultEdiArticles_concern[0].CustomerItemNumber;
								item.PSZArtikelNr = defaultEdiArticles_concern[0].ArtikelNr;
								item.SuppliersItemMaterialNumber = defaultEdiArticles_concern[0].ArtikelNummer;
							}
							else
							{
								foundByCustomerItemNumber = false;
								errors.Add($"Invalid CustomersItemMaterialNumber [{item.CustomersItemMaterialNumber}] not found in EDI-Standart articles for [{adressDb?.Name1}]");
							}
						}
					}
				}

				// - add following error only when previous was added
				if((string.IsNullOrEmpty(item.CustomersItemMaterialNumber) || string.IsNullOrWhiteSpace(item.CustomersItemMaterialNumber)) && (string.IsNullOrEmpty(item.SuppliersItemMaterialNumber) || string.IsNullOrWhiteSpace(item.SuppliersItemMaterialNumber)))
					errors.Add($"Invalid SuppliersItemMaterialNumber [{item.SuppliersItemMaterialNumber}]");
				var lastVersionHeader = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetLastVersion(adressDb?.Nr ?? 0, item.DocumentNumber);
				if(lastVersionHeader != null)
				{
					var lastLineItemVersion = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetByDocumentLastVersionNumber(item.DocumentNumber, lastVersionHeader?.Id);
					if(lastLineItemVersion != null && lastLineItemVersion.CallOffNumber >= (item.CallOffNumber ?? 0))
						errors.Add($"Wrong LineItem version | Document: [{item.DocumentNumber}], LastVersionNumber: [{lastLineItemVersion.CallOffNumber}], XMLVersionNumber: [{item.CallOffNumber}]");
				}

				if(!foundByCustomerItemNumber && string.IsNullOrEmpty(item.CustomersItemMaterialNumber)  && !string.IsNullOrEmpty(item.SuppliersItemMaterialNumber) && !string.IsNullOrWhiteSpace(item.SuppliersItemMaterialNumber))
				{
					// - get Article by Customer, first
					if(adressDb == null)
						errors.Add($"Customer [{headerData?.BuyerDUNS}] not found");
					var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(item.SuppliersItemMaterialNumber);
					if(article == null)
						errors.Add($"Article {item.SuppliersItemMaterialNumber} not found");
					else
					{
						item.CustomersItemMaterialNumber = article.CustomerItemNumber;
						item.PSZArtikelNr = article.ArtikelNr;
					}

				}
				#region > Check Items
				var itemsSuppliersNumbers = lineItems
							.Where(e => !string.IsNullOrWhiteSpace(e.SuppliersItemMaterialNumber))
							.Select(e => e.SuppliersItemMaterialNumber?.Trim())
							.ToList();
				var itemsCustomersNumbers = lineItems
					.Where(e => !string.IsNullOrWhiteSpace(e.CustomersItemMaterialNumber))
					.Select(e => e.CustomersItemMaterialNumber?.Trim()?.TrimStart('0'))
					.ToList();

				var itemsDbBySuppliersNumbers = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(itemsSuppliersNumbers, connection, transaction);
				var itemsDbByCustomerNumbers = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByBezeichnung1(itemsCustomersNumbers, connection, transaction);
				var stdEdiArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(adressDb.Kundennummer ?? -1, connection, transaction)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity>();
				var customerConcernItem = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetByCustomerNumber(adressDb?.Kundennummer ?? -1, connection, transaction);
				var customerConcern = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.GetWithTransaction(customerConcernItem?.ConcernId ?? -1, connection, transaction);

				var stdEdiConcernArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(
					Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetCustomerNumbersInSameConcern(adressDb?.Kundennummer ?? -1), connection, transaction)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity>();
				var customersConcernItems = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetListByConcernId(customerConcern.Id, connection, transaction);

				// - 
				var customerIndependentArticles = Infrastructure.Data.Access.Joins.BSD.DashboardAccess.GetUniversalArticles();

				var itemsErrors = new List<string>();
				var errortracks = new List<Tuple<int, int>>();

				foreach(var lineItem in lineItems)
				{
					// - 2025-06-03 - customer-independent articles - anyone can order them.
					if(!string.IsNullOrEmpty(lineItem.CustomersItemMaterialNumber))
					{
						if(customerIndependentArticles?.Exists(x =>
							(x.CustomerItemNumber.TrimStart('0') == lineItem.CustomersItemMaterialNumber.TrimStart('0')) ||
							(x.Bezeichnung1.TrimStart('0') == lineItem.CustomersItemMaterialNumber.TrimStart('0'))) == true)
						{
							// - The ordered article is Universal
							continue;
						}
					}
					else
					{
						if(!string.IsNullOrEmpty(lineItem.SuppliersItemMaterialNumber))
						{
							if(customerIndependentArticles?.Exists(x => x.ArtikelNummer?.ToLower() == lineItem.SuppliersItemMaterialNumber.ToLower()) == true)
							{
								// - The ordered article is Universal
								continue;
							}
						}
					}

					// 2023-01-20 - check StdEdi
					var stdEdi = stdEdiArticles.FirstOrDefault(x => (x.CustomerItemNumber.TrimStart('0') == lineItem.CustomersItemMaterialNumber.TrimStart('0')) ||
					(x.Bezeichnung1.TrimStart('0') == lineItem.CustomersItemMaterialNumber.TrimStart('0')));
					if(stdEdi != null)
					{
						// - we're good to go
						// - 2025-05-02 - but first set matching PSZ number if not the case - Khelil/Groetsch (ticket #)
						var lineItemNumber = lineItem.SuppliersItemMaterialNumber?.Trim()?.ToLower();
						if(!string.IsNullOrEmpty(lineItemNumber))
						{
							if(stdEdi.ArtikelNummer?.Trim()?.ToLower() != lineItemNumber)
							{
								lineItem.SuppliersItemMaterialNumber = stdEdi.ArtikelNummer;
							}
						}
						continue;
					}
					// 2023-08-09 - check StdEdi in Concern
					stdEdi = stdEdiConcernArticles.FirstOrDefault(x => FoundArticle(x, lineItem, customerConcern.IncludeDescription ?? false, customerConcern.TrimLeadingZeros ?? false));
					if(stdEdi != null)
					{
						// - we're good to go, here too
						// - 2025-05-02 - but first set matching PSZ number if not the case - Khelil/Groetsch (ticket #)
						var lineItemNumber = lineItem.SuppliersItemMaterialNumber?.Trim()?.ToLower();
						if(!string.IsNullOrEmpty(lineItemNumber))
						{
							if(stdEdi.ArtikelNummer?.Trim()?.ToLower() != lineItemNumber)
							{
								lineItem.SuppliersItemMaterialNumber = stdEdi.ArtikelNummer;
							}
						}
						continue;
					}
					if(!string.IsNullOrEmpty(lineItem.CustomersItemMaterialNumber))
					{
						var includeDesignation1 = true;
						var trimTrailingZeros = true;
						var customerNumbers = new List<int> { adressDb.Kundennummer ?? -1 };
						if(customersConcernItems?.Count > 0)
						{
							customerNumbers = customersConcernItems.Select(c => c.CustomerNumber ?? -1).ToList();
							if(customerConcern.IncludeDescription != true)
								includeDesignation1 = false;
							if(customerConcern.TrimLeadingZeros != true)
								trimTrailingZeros = false;
						}

						var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(customerNumbers, lineItem.CustomersItemMaterialNumber, connection, transaction, includeDesignation1, trimTrailingZeros);
						if(articles != null && articles.Count > 0)
						{
							errors.Add("Position " + lineItem.PositionNumber + $" CustomerItemNumber [{lineItem.CustomersItemMaterialNumber}] is not EDI standard.");
							continue;
						}
					}
					List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> itemDbs = null;
					// - Search Article by PSZ number, if any
					if(!string.IsNullOrWhiteSpace(lineItem.SuppliersItemMaterialNumber))
					{
						itemDbs = !string.IsNullOrWhiteSpace(lineItem.SuppliersItemMaterialNumber)
								? itemsDbBySuppliersNumbers.FindAll(e => e.ArtikelNummer == lineItem.SuppliersItemMaterialNumber).ToList()
								: null;
						if(!string.IsNullOrEmpty(lineItem.CustomersItemMaterialNumber) && itemDbs != null && itemDbs.Count > 0)
						{
							errors.Add("Position " + lineItem.PositionNumber + $" [{lineItem.SuppliersItemMaterialNumber}] : CustomerItemNumber [{lineItem.CustomersItemMaterialNumber}] does not match PSZ number [{lineItem.SuppliersItemMaterialNumber}]");
						}
					}

					if(itemDbs == null || itemDbs.Count <= 0)
					{
						if(!errortracks.Exists(x => x.Item1 == lineItem.PositionNumber && x.Item2 == 0))
						{
							errortracks.Add(new Tuple<int, int>(lineItem.PositionNumber, 0));
							errors.Add("Position " + lineItem.PositionNumber + $" [{lineItem.SuppliersItemMaterialNumber} | {lineItem.CustomersItemMaterialNumber}]: Article not found.");
						}

						continue;
					}
				}
				#endregion

			}
			if(errors != null && errors.Count > 0)
				return false;

			return true;
		}
		private bool validateDataLineItemPlan(List<Models.Delfor.XMLLineItemPlanModel> lineItemPlans, out List<string> errors)
		{
			errors = new List<string>();
			foreach(var item in lineItemPlans)
			{
				if(item.PositionNumber <= 0)
					errors.Add($"Invalid PositionNumber {item.PositionNumber}");
				if(string.IsNullOrEmpty(item.PlanningQuantityUnitOfMeasure) || string.IsNullOrWhiteSpace(item.PlanningQuantityUnitOfMeasure))
					errors.Add($"Invalid PlanningQuantityUnitOfMeasure {item.PlanningQuantityUnitOfMeasure}");
				if(string.IsNullOrEmpty(item.PlanningQuantityFrequencyIdentifier) || string.IsNullOrWhiteSpace(item.PlanningQuantityFrequencyIdentifier))
					errors.Add($"Invalid PlanningQuantityFrequencyIdentifier {item.PlanningQuantityFrequencyIdentifier}");

				if(!item.PlanningQuantityRequestedShipmentDate.HasValue)
				{
					errors.Add($"Invalid PlanningQuantityRequestedShipmentDate {item.PlanningQuantityRequestedShipmentDate}");
					//if(item.PlanningQuantityFrequencyIdentifier.ToLower() != "d" && item.PlanningQuantityFrequencyIdentifier.ToLower() != "t")
					//{
					//	if(!item.PlanningQuantityWeeklyPeriodEndDate.HasValue)
					//		errors.Add($"Invalid PlanningQuantityWeeklyPeriodEndDate {item.PlanningQuantityWeeklyPeriodEndDate}");
					//}
				}
				else
				{
					// - 2023-04-05 - auto-update values
					if(!item.PlanningQuantityWeeklyPeriodEndDate.HasValue)
					{
						var rsd = item.PlanningQuantityRequestedShipmentDate.Value;
						switch(item.PlanningQuantityFrequencyIdentifier?.ToLower())
						{
							case "w":
								item.PlanningQuantityWeeklyPeriodEndDate = Helpers.DateHelper.EndOfWeek(rsd);
								break;
							case "m":
								item.PlanningQuantityWeeklyPeriodEndDate = new DateTime(rsd.Year, rsd.Month, DateTime.DaysInMonth(rsd.Year, rsd.Month));
								break;
							case "y":
								item.PlanningQuantityWeeklyPeriodEndDate = new DateTime(rsd.Year, 12, 31);
								break;
							default:
								break;
						}
					}
				}
			}
			if(errors != null && errors.Count > 0)
				return false;

			return true;
		}
		private Tuple<string, Psz.Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage, OportedOrdersHandler> deserializeFile(string fileName, OportedOrdersHandler deseralizer)
		{
			try
			{
				Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage erpelIndustryMessage = null;
				string documentName = null;

				while(isFileLocked(fileName))
				{
					//Thread.Sleep(300);
				}

				var wrongFormatDetected = false;
				using(var stream = new FileStream(fileName, FileMode.Open))
				{
					var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Models.OrderResponse.ErpelIndustryMessage));

					try
					{
						erpelIndustryMessage = (Models.OrderResponse.ErpelIndustryMessage)xmlSerializer.Deserialize(stream);

						var position = stream.Name.LastIndexOf('\\');
						documentName = stream.Name.Substring(position, stream.Name.Length - position);
					} catch(Exception e)
					{
						Infrastructure.Services.Logging.Logger.Log(e);
						wrongFormatDetected = true;
					}

					stream.Close();
				}

				if(wrongFormatDetected)
				{
					deseralizer.HandleError("\\" + System.IO.Path.GetFileName(fileName),
						"Wrong Document Format",
						"Other",
						-1);
					return null;
				}

				return new Tuple<string, Models.OrderResponse.ErpelIndustryMessage, OportedOrdersHandler>(documentName, erpelIndustryMessage, deseralizer);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return null;
			}
		}
		#endregion

		private bool isFileLocked(string fileName)
		{
			FileStream stream = null;

			try
			{
				stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
			} catch(IOException e)
			{
				if(e.HResult != -2147024864)
				{
					return false;
				}
				return true;
			} finally
			{
				if(stream != null)
				{
					stream.Close();
				}
			}

			return false;
		}


		public void moveErrorToNewFile(string fileName)
		{
			var moveTo = Path.Combine(_newOrdersDirectory, Path.GetFileName(fileName));
			var moveFrom = fileName; // Path.Combine(_errorOrderDirectory, Path.GetFileName(fileName));
			lock(Locks.DocumentsLock)
			{
				try
				{
					moveTo = OportedOrdersHandler.checkAndFixFileName(moveTo);
					OportedOrdersHandler.createIfNotExists(moveTo);

					File.Move(moveFrom, moveTo);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
		public string moveFromNewFolder(string fileName, string destDirectory)
		{
			var moveTo = GetDestinationFileName(fileName, destDirectory); // - 2023-03-20 Path.Combine(destDirectory, System.DateTime.Today.ToString("dd.MM.yyyy"), Path.GetFileName(fileName));
			var moveFrom = fileName;
			lock(Locks.DocumentsLock)
			{
				try
				{
					moveTo = OportedOrdersHandler.checkAndFixFileName(moveTo);
					OportedOrdersHandler.createIfNotExists(moveTo);

					File.Move(moveFrom, moveTo);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
				}
			}
			return moveTo;
		}

		public static string GetDestinationFileName(string fileName, string destDirectory)
		{
			return Path.Combine(destDirectory, System.DateTime.Today.ToString("dd.MM.yyyy"), Path.GetFileName(fileName));
		}
		public List<Infrastructure.Data.Entities.Tables.CTS.ErrorEntity> ValidateData(Models.Delfor.XMLHeaderModel headerData,
			List<Models.Delfor.XMLLineItemModel> lineItems,
			List<Models.Delfor.XMLLineItemPlanModel> lineItemPlans,
			SqlConnection connection,
			SqlTransaction transaction
			)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.CTS.ErrorEntity>();
			var validHeader = validateDataHeader(headerData, out List<string> errors_header);
			var validLineItems = validateDataLineItem(lineItems, headerData, connection, transaction, out List<string> errors_lineItems);
			var validLineItemPlans = validateDataLineItemPlan(lineItemPlans, out List<string> errors_lineItemPlans);

			if(!validHeader)
				foreach(var item in errors_header)
				{
					result.Add(new Infrastructure.Data.Entities.Tables.CTS.ErrorEntity
					{
						Id = -1,
						ErrorMessage = item,
						ErrorTrace = $"HeaderData: {Newtonsoft.Json.JsonConvert.SerializeObject(lineItems)}\\{Newtonsoft.Json.JsonConvert.SerializeObject(lineItems)}",
						ProcessTime = System.DateTime.Now,
						RecipientId = headerData.RecipientId,
						SenderId = headerData.SenderId,
						Documentnumber = headerData.DocumentNumber,
						BuyerDuns = headerData.BuyerDUNS
					});
				}
			if(!validLineItems)
				foreach(var item in errors_lineItems)
				{
					result.Add(new Infrastructure.Data.Entities.Tables.CTS.ErrorEntity
					{
						Id = -1,
						ErrorMessage = item,
						ErrorTrace = $"HeaderData: {Newtonsoft.Json.JsonConvert.SerializeObject(lineItems)}\\{Newtonsoft.Json.JsonConvert.SerializeObject(lineItems)}",
						ProcessTime = System.DateTime.Now,
						RecipientId = headerData.RecipientId,
						SenderId = headerData.SenderId,
						Documentnumber = headerData.DocumentNumber,
						BuyerDuns = headerData.BuyerDUNS ?? headerData.SenderId,

					});
				}
			if(!validLineItemPlans)
				foreach(var item in errors_lineItemPlans)
				{
					result.Add(new Infrastructure.Data.Entities.Tables.CTS.ErrorEntity
					{
						Id = -1,
						ErrorMessage = item,
						ErrorTrace = $"HeaderData: {Newtonsoft.Json.JsonConvert.SerializeObject(lineItems)}\\{Newtonsoft.Json.JsonConvert.SerializeObject(lineItems)}",
						ProcessTime = System.DateTime.Now,
						RecipientId = headerData.RecipientId,
						SenderId = headerData.SenderId,
						Documentnumber = headerData.DocumentNumber,
						BuyerDuns = headerData.BuyerDUNS ?? headerData.SenderId,
					});
				}
			return result;
		}
		private static bool FoundArticle(Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity articleEntity, Models.Delfor.XMLLineItemModel lineItem,
			bool includeDescription1, bool trimLeadingZeros)
		{
			//true - true
			if(includeDescription1 && trimLeadingZeros)
			{
				return articleEntity.CustomerItemNumber.TrimStart('0') == lineItem.CustomersItemMaterialNumber.TrimStart('0')
					|| articleEntity.Bezeichnung1.TrimStart('0') == lineItem.CustomersItemMaterialNumber.TrimStart('0');
			}
			// true - false
			if(includeDescription1 && !trimLeadingZeros)
			{
				return articleEntity.CustomerItemNumber == lineItem.CustomersItemMaterialNumber
					|| articleEntity.Bezeichnung1 == lineItem.CustomersItemMaterialNumber;
			}
			// false - true
			if(!includeDescription1 && trimLeadingZeros)
			{
				return articleEntity.CustomerItemNumber.TrimStart('0') == lineItem.CustomersItemMaterialNumber.TrimStart('0');
			}
			// false - false
			if(!includeDescription1 && !trimLeadingZeros)
			{
				return articleEntity.CustomerItemNumber == lineItem.CustomersItemMaterialNumber;
			}

			return false;
		}
	}
}
