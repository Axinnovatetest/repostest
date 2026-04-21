using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public partial class Change
		{
			public static Core.Models.ResponseModel<object> AcceptGlobal(int globalChangeId,
				Core.Identity.Models.UserModel user)
			{
				lock(Locks.OrdersLock)
				{
					var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
					try
					{
						botransaction.beginTransaction();

						#region // -- transaction-based logic -- //

						if(user == null
							|| !user.Access.Purchase.ModuleActivated)
						{
							throw new Core.Exceptions.UnauthorizedException();
						}

						var globalChangeDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.Get(globalChangeId, botransaction.connection, botransaction.transaction);
						if(globalChangeDb == null)
						{
							throw new Core.Exceptions.NotFoundException();
						}

						if((Enums.OrderEnums.GlobalOrderChangeStatus)globalChangeDb.GlobalStatus
							!= Enums.OrderEnums.GlobalOrderChangeStatus.Pending)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Global Change already used or overwritten" }
							};
						}

						var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(globalChangeDb.OrderId, botransaction.connection, botransaction.transaction);
						if(orderDb == null)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Order not found" }
							};
						}

						var orderExtensionEntity = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrderId(orderDb.Nr, botransaction.connection, botransaction.transaction);
						var isECOSIO = Infrastructure.Data.Access.Tables.PRS.AdresseECOSIOAccess.IsECOSIOByDuns(orderExtensionEntity?.SenderDuns ?? "", botransaction.connection, botransaction.transaction); // Only HORSCH
						#region > Update Order
						if(globalChangeDb.CustomerName != null)
						{ orderDb.ABSENDER = globalChangeDb.CustomerName; }
						if(globalChangeDb.DocumentName != null && (!string.IsNullOrWhiteSpace(orderDb.EDI_Dateiname_CSV) && orderDb.EDI_Dateiname_CSV.ToLower().Trim().Substring(orderDb.EDI_Dateiname_CSV.ToLower().Trim().Length - 4, 4) != ".csv"))
						{ orderDb.EDI_Dateiname_CSV = globalChangeDb.DocumentName; }
						if(globalChangeDb.CustomerName != null)
						{ orderDb.Vorname_NameFirma = globalChangeDb.CustomerName; }
						if(globalChangeDb.CustomerContactName != null)
						{ orderDb.Ansprechpartner = isECOSIO ? "" : globalChangeDb.CustomerContactName; } // >>>>>  -- FIXME: IGNORE Contact & Department on HORSCH
						if(globalChangeDb.CustomerPurchasingDepartment != null)
						{ orderDb.Abteilung = isECOSIO ? "" : globalChangeDb.CustomerPurchasingDepartment; }
						if(globalChangeDb.CustomerStreetPostalCode != null)
						{ orderDb.Straße_Postfach = globalChangeDb.CustomerStreetPostalCode; }
						if(globalChangeDb.CustomerStreetCityPostalCode != null)
						{ orderDb.Land_PLZ_Ort = globalChangeDb.CustomerStreetCityPostalCode; }
						if(globalChangeDb.Notes != null)
						{ orderDb.Freitext = globalChangeDb.Notes; }
						if(globalChangeDb.ConsigneeName != null)
						{ orderDb.LVorname_NameFirma = globalChangeDb.ConsigneeName; }
						if(globalChangeDb.ConsigneeContactName != null)
						{ orderDb.LAnsprechpartner = globalChangeDb.ConsigneeContactName; }
						if(globalChangeDb.ConsigneePurchasingDepartment != null)
						{ orderDb.LAbteilung = globalChangeDb.ConsigneePurchasingDepartment; }
						if(globalChangeDb.ConsigneeStreetPostalCode != null)
						{ orderDb.LStraße_Postfach = globalChangeDb.ConsigneeStreetPostalCode; }

						orderDb.Gebucht = true;
						Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(orderDb, botransaction.connection, botransaction.transaction);
						#endregion

						#region Update Order Buyer
						var buyerEntity = Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.GetByOrderType(orderDb.Nr, (int)Enums.OrderEnums.OrderTypes.Order, botransaction.connection, botransaction.transaction)
							?? new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity
							{
								Id = -1,
								OrderId = orderDb.Nr,
								OrderType = (int)Enums.OrderEnums.OrderTypes.Order
							};

						if(globalChangeDb.CustomerName != null)
						{ buyerEntity.Name = globalChangeDb.CustomerName; }
						if(globalChangeDb.CustomerContactName != null)
						{ buyerEntity.ContactName = globalChangeDb.CustomerContactName; }
						if(globalChangeDb.CustomerPurchasingDepartment != null)
						{ buyerEntity.PurchasingDepartment = globalChangeDb.CustomerPurchasingDepartment; }
						//if (globalChangeDb.CustomerStreetPostalCode != null) { buyerEntity.PostalCode = globalChangeDb.CustomerStreetPostalCode; }
						if(globalChangeDb.CustomerPartyIdentification != null)
						{ buyerEntity.PartyIdentification = globalChangeDb.CustomerPartyIdentification; }
						if(globalChangeDb.CustomerPartyIdentificationCodeListQualifier != null)
						{ buyerEntity.PartyIdentificationCodeListQualifier = globalChangeDb.CustomerPartyIdentificationCodeListQualifier; }
						if(globalChangeDb.CustomerName2 != null)
						{ buyerEntity.Name2 = globalChangeDb.CustomerName2; }
						if(globalChangeDb.CustomerName3 != null)
						{ buyerEntity.Name3 = globalChangeDb.CustomerName3; }
						if(globalChangeDb.CustomerStreet != null)
						{ buyerEntity.Street = globalChangeDb.CustomerStreet; }
						if(globalChangeDb.CustomerCity != null)
						{ buyerEntity.City = globalChangeDb.CustomerCity; }
						if(globalChangeDb.CustomerPostalCode != null)
						{ buyerEntity.PostalCode = globalChangeDb.CustomerPostalCode; }
						if(globalChangeDb.CustomerCountryName != null)
						{ buyerEntity.CountryName = globalChangeDb.CustomerCountryName; }
						if(globalChangeDb.CustomerContactTelephone != null)
						{ buyerEntity.ContactTelephone = globalChangeDb.CustomerContactTelephone; }
						if(globalChangeDb.CustomerContactFax != null)
						{ buyerEntity.ContactFax = globalChangeDb.CustomerContactFax; }
						if(buyerEntity.Id == -1)
						{
							Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.Insert(buyerEntity, botransaction.connection, botransaction.transaction);
						}
						else
						{
							Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.Update(buyerEntity, botransaction.connection, botransaction.transaction);
						}
						#endregion

						#region Update Order Consignee
						var consigneeEntity = Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.GetByOrderType(orderDb.Nr, (int)Enums.OrderEnums.OrderTypes.Order, null, botransaction.connection, botransaction.transaction)
							?? new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity
							{
								Id = -1,
								OrderId = orderDb.Nr,
								OrderType = (int)Enums.OrderEnums.OrderTypes.Order
							};

						if(globalChangeDb.ConsigneeName != null)
						{ consigneeEntity.Name = globalChangeDb.ConsigneeName; }
						if(globalChangeDb.ConsigneeContactName != null)
						{ consigneeEntity.ContactName = globalChangeDb.ConsigneeContactName; }
						if(globalChangeDb.ConsigneePurchasingDepartment != null)
						{ consigneeEntity.PurchasingDepartment = globalChangeDb.ConsigneePurchasingDepartment; }
						if(globalChangeDb.ConsigneeStreetPostalCode != null)
						{ consigneeEntity.Street = globalChangeDb.ConsigneeStreetPostalCode; }
						if(globalChangeDb.ConsigneePartyIdentification != null)
						{ consigneeEntity.PartyIdentification = globalChangeDb.ConsigneePartyIdentification; }
						if(globalChangeDb.ConsigneePartyIdentificationCodeListQualifier != null)
						{ consigneeEntity.PartyIdentificationCodeListQualifier = globalChangeDb.ConsigneePartyIdentificationCodeListQualifier; }
						if(globalChangeDb.ConsigneeName2 != null)
						{ consigneeEntity.Name2 = globalChangeDb.ConsigneeName2; }
						if(globalChangeDb.ConsigneeName3 != null)
						{ consigneeEntity.Name3 = globalChangeDb.ConsigneeName3; }
						if(globalChangeDb.ConsigneeStreet != null)
						{ consigneeEntity.Street = globalChangeDb.ConsigneeStreet; }
						if(globalChangeDb.ConsigneeCity != null)
						{ consigneeEntity.City = globalChangeDb.ConsigneeCity; }
						if(globalChangeDb.ConsigneePostalCode != null)
						{ consigneeEntity.PostalCode = globalChangeDb.ConsigneePostalCode; }
						if(globalChangeDb.ConsigneeCountryName != null)
						{ consigneeEntity.CountryName = globalChangeDb.ConsigneeCountryName; }
						if(globalChangeDb.ConsigneeStorageLocation != null)
						{ consigneeEntity.StorageLocation = globalChangeDb.ConsigneeStorageLocation; }
						if(globalChangeDb.ConsigneeContactTelephone != null)
						{ consigneeEntity.ContatTelephone = globalChangeDb.ConsigneeContactTelephone; }
						if(globalChangeDb.ConsigneeContactFax != null)
						{ consigneeEntity.ContactFax = globalChangeDb.ConsigneeContactFax; }
						if(consigneeEntity.Id == -1)
						{
							Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.Insert(consigneeEntity, botransaction.connection, botransaction.transaction);
						}
						else
						{
							Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.Update(consigneeEntity, botransaction.connection, botransaction.transaction);
						}
						#endregion

						#region > Update Order Change
						globalChangeDb.GlobalStatus = (int)Enums.OrderEnums.GlobalOrderChangeStatus.Accepted;
						globalChangeDb.ActionTime = DateTime.Now;
						globalChangeDb.ActionUserId = user.Id;
						globalChangeDb.ActionUsername = user.Username;

						Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.Update(globalChangeDb, botransaction.connection, botransaction.transaction);
						#endregion


						#endregion // -- transaction-based logic -- //

						//
						if(botransaction.commit())
						{
							return Core.Models.ResponseModel<object>.SuccessResponse();
						}
						else
						{
							return Core.Models.ResponseModel<object>.FailureResponse("Transaction error");
						}
					} catch(Exception e)
					{
						botransaction.rollback();
						Infrastructure.Services.Logging.Logger.Log(e);
						throw;
					}
				}
			}
		}
	}
}
