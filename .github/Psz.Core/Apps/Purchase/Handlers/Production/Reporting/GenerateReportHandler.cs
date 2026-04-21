using Infrastructure.Data.Entities.Tables.PRS;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Psz.Core.Apps.Purchase.Handlers.Production.Reporting
{
	public class GenerateReportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _productionNumber { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public GenerateReportHandler(int productionNumber,
			Identity.Models.UserModel user)
		{
			this._productionNumber = productionNumber;
			this._user = user;
		}

		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var productionEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._productionNumber);

				var orderItemEntity = productionEntity.Angebot_Artikel_Nr.HasValue
					? Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(productionEntity.Angebot_Artikel_Nr.Value)
					: null;

				var itemEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(productionEntity.Artikel_Nr ?? -1);

				var productionItemsDb = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByIdFertigung(productionEntity.ID);

				var itemsIds = productionItemsDb.Where(e => e.Artikel_Nr.HasValue).Select(e => e.Artikel_Nr.Value).ToList();
				var itemsOfProductionItemsEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(itemsIds);

				var storagePositionsIds = productionItemsDb.Where(e => e.Lagerort_ID.HasValue).Select(e => e.Lagerort_ID.Value).ToList();
				var storageLocationsEntities = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(storagePositionsIds);

				var articleClassifiersIds = itemsOfProductionItemsEntities.Select(x => x.ID_Klassifizierung).Distinct().ToList();
				var articleClassifierItems = articleClassifiersIds == null
					? null
					: Infrastructure.Data.Access.Tables.PRS.ArtikelstammKlassifizierungAccess.Get()?.FindAll(x => articleClassifiersIds.Contains(x.ID));

				byte[] fileBytes;
				if(productionEntity.Artikel_Nr == 16584)
				{
					fileBytes = generateTechnicalReport(productionEntity,
						itemEntity,
						productionItemsDb,
						itemsOfProductionItemsEntities,
						storageLocationsEntities,
						articleClassifierItems);
				}
				else
				{
					fileBytes = generateStandardReport(productionEntity,
						orderItemEntity,
						itemEntity,
						productionItemsDb,
						itemsOfProductionItemsEntities,
						storageLocationsEntities,
						articleClassifierItems);
				}

				return ResponseModel<byte[]>.SuccessResponse(fileBytes);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private byte[] generateStandardReport(FertigungEntity productionEntity,
			AngeboteneArtikelEntity orderItemEntity,
			ArtikelEntity itemEntity,
			List<FertigungPositionenEntity> productionItemsEntities,
			List<ArtikelEntity> itemsOfProductionItemsEntities,
			List<Infrastructure.Data.Entities.Tables.INV.LagerorteEntity> storageLocationsEntities,
			List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> articleClassifierItems)
		{
			try
			{
				var productionsData = new List<Models.Production.Reporting.StandardReportModel.ProductionModel>()
				{
					new Models.Production.Reporting.StandardReportModel.ProductionModel()
					{
						Id = 1,

						L01 = orderItemEntity?.Typ != null
							? Enums.OrderElementEnums.TypeToString((Enums.OrderElementEnums.Types)orderItemEntity.Typ.Value)
							: string.Empty,
						L02 = $"{productionEntity.Fertigungsnummer ?? 0}",
						L03 = itemEntity?.Halle,

						L04 = productionEntity.Datum?.ToString("dd/MM/yyyy"),
						L05 = productionEntity.Mandant,
						L06 = productionEntity.Lagerort_id_zubuchen?.ToString(),
						L07 = itemEntity?.ArtikelNummer,
						L08 = itemEntity?.Bezeichnung1,
						L09 = itemEntity?.Artikelfamilie_Kunde,
						L10 = productionEntity.KundenIndex,
						L11 = productionEntity.Anzahl?.ToString(),

						L12 = productionEntity.Bemerkung,
						L13 = productionEntity.Techniker,
						L14 = itemEntity?.Artikelfamilie_Kunde_Detail1,
						L15 = productionEntity.Termin_Bestatigt1?.ToString("dd/MM/yyyy"),
						L16 = itemEntity?.Artikelfamilie_Kunde_Detail2,
						L17 = productionEntity.Zeit?.ToString(),

						L18 = (productionEntity.Anzahl ?? 0) * (productionEntity.Zeit ?? 0) > 0
							? ((productionEntity.Anzahl ?? 0) * (productionEntity.Zeit ?? 0) / 60).ToString("0.0")
							: "0",
						L19 = itemEntity?.Freigabestatus,
						L20 = itemEntity?.FreigabestatusTNIntern,
						L21 = productionEntity.Fertigungsnummer?.ToString(),
						L22 = itemEntity?.Verpackungsart,
						L23 = itemEntity?.Verpackungsmenge?.ToString(),
						L24 = itemEntity?.Losgroesse?.ToString(),

						Date = DateTime.Today.ToString("dd/MM/yyyy"),
						User = this._user.Username,

						R01 = productionEntity.Fertigungsnummer?.ToString(),
						R02 = itemEntity?.ArtikelNummer,
						R03 = productionEntity.Anzahl?.ToString(),
						R04 = itemEntity?.Bezeichnung1,
						R05 = productionEntity.Bemerkung,
						R06 = productionEntity.Termin_Bestatigt1?.ToString("dd/MM/yyyy"),

						S01 = getS01(productionEntity),
						S02 = productionEntity.Fertigungsnummer?.ToString(),
						S03 = itemEntity?.ArtikelNummer,
						S04 = productionEntity.Anzahl?.ToString(),
						S05 = productionEntity.Termin_Bestatigt1?.ToString("dd/MM/yyyy"),
					}
				};

				var itemsData = new List<Models.Production.Reporting.StandardReportModel.ItemModel>();
				foreach(var productionItemEntity in productionItemsEntities)
				{
					var itemOfProductionItemEntity = itemsOfProductionItemsEntities.Find(e => e.ArtikelNr == productionItemEntity.Artikel_Nr);
					var storageLocationEntity = storageLocationsEntities.Find(e => e.LagerortId == productionEntity.Lagerort_id);

					itemsData.Add(new Models.Production.Reporting.StandardReportModel.ItemModel()
					{
						ProductionId = 1,

						ItemNumber = itemOfProductionItemEntity?.ArtikelNummer,
						Requirement = "",
						Prepared = productionItemEntity.Anzahl?.ToString(),
						Name = itemOfProductionItemEntity?.Bezeichnung1,
						StorageLocation = storageLocationEntity?.Lagerort,
						Odpad = "",
					});
				}

				//
				var Gewerks = new List<Models.Production.Reporting.StandardReportModel.Gewerk>();
				var GewerkItems = new List<Models.Production.Reporting.StandardReportModel.GewerkItem>();
				if(articleClassifierItems != null)
				{
					var uniqueAClassifiers = articleClassifierItems.Select(x => x.Gewerk).Distinct().ToList();
					for(int i = 0; i < uniqueAClassifiers.Count; i++)
					{
						if(!string.IsNullOrEmpty(uniqueAClassifiers[i]) && !string.IsNullOrWhiteSpace(uniqueAClassifiers[i]))
						{
							Gewerks.Add(new Models.Production.Reporting.StandardReportModel.Gewerk { Id = i, Name = uniqueAClassifiers[i] });
							var gewerks = articleClassifierItems.FindAll(p => p.Gewerk.ToLower() == uniqueAClassifiers[i].ToLower());
							foreach(var item in gewerks)
							{
								GewerkItems.Add(new Models.Production.Reporting.StandardReportModel.GewerkItem { GewerkId = i, Name = item.Bezeichnung });
							}
						}
					}
				}

				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(newTable("Prd", productionsData));
				dataSet.Tables.Add(newTable("Itm", itemsData));
				dataSet.Tables.Add(newTable("Gew", Gewerks));
				dataSet.Tables.Add(newTable("Pos", GewerkItems));

				return new Infrastructure.Services.Reporting.ReportGenerator("ProductionStandard.frx", dataSet).GeneratePdfFile();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private byte[] generateTechnicalReport(FertigungEntity productionEntity,
			ArtikelEntity itemEntity,
			List<FertigungPositionenEntity> productionItemsEntities,
			List<ArtikelEntity> itemsOfProductionItemsEntities,
			List<Infrastructure.Data.Entities.Tables.INV.LagerorteEntity> storageLocationsEntities,
			List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> articleClassifierItems)
		{
			try
			{
				var productionsData = new List<Models.Production.Reporting.TechnicalReportModel.ProductionModel>()
				{
					new Models.Production.Reporting.TechnicalReportModel.ProductionModel()
					{
						Id = 1,

						L01 = getPreReference(productionEntity),
						L02 = $"{productionEntity.Fertigungsnummer ?? 0}",
						L03 = itemEntity?.Halle,

						L04 = itemEntity?.Freigabestatus,
						L05 = productionEntity.Datum?.ToString("dd/MM/yyyy"),
						L06 = itemEntity?.ArtikelNummer,
						L07 = productionEntity.Urs_Artikelnummer,
						L26 = itemEntity?.Artikelfamilie_Kunde,
						L08 = itemEntity?.Bezeichnung1,
						L09 = productionEntity.Anzahl?.ToString() ?? "",
						L10 = productionEntity.Bemerkung,
						L11 = productionEntity.Techniker,

						L19 = productionEntity.Mandant,
						L20 = productionEntity.Fertigungsnummer?.ToString(),

						L12 = getF12(productionEntity),
						L13 = productionEntity.Lagerort_id_zubuchen?.ToString(),
						L14 =  productionEntity.Termin_Bestatigt1?.ToString("dd/MM/yyyy"),
						L15 = itemEntity?.Artikelfamilie_Kunde_Detail2,
						L16 = productionEntity.Zeit?.ToString(),
						L17 = (productionEntity.Anzahl ?? 0) * (productionEntity.Zeit ?? 0) > 0
							? ((productionEntity.Anzahl ?? 0) * (productionEntity.Zeit ?? 0) / 60).ToString("0.0")
							: "0",
						L18 = itemEntity?.Artikelfamilie_Kunde_Detail1,

						User = _user.Name,
					}
				};

				var itemsData = new List<Models.Production.Reporting.TechnicalReportModel.ItemModel>();
				foreach(var productionItemEntity in productionItemsEntities)
				{
					var itemOfProductionItemEntity = itemsOfProductionItemsEntities.Find(e => e.ArtikelNr == productionItemEntity.Artikel_Nr);
					var storageLocationEntity = storageLocationsEntities.Find(e => e.LagerortId == productionEntity.Lagerort_id);

					itemsData.Add(new Models.Production.Reporting.TechnicalReportModel.ItemModel()
					{
						ProductionId = 1,

						ItemNumber = itemOfProductionItemEntity?.ArtikelNummer,
						RequiredQuantity = productionItemEntity.Anzahl?.ToString(),
						PreparedQuantity = "",
						Name1 = itemOfProductionItemEntity?.Bezeichnung1,
						Name2 = itemOfProductionItemEntity?.Bezeichnung2,
						StorageLocation = storageLocationEntity?.Lagerort,
						Scrap = "",
					});
				}


				//
				var Gewerks = new List<Models.Production.Reporting.StandardReportModel.Gewerk>();
				var GewerkItems = new List<Models.Production.Reporting.StandardReportModel.GewerkItem>();
				if(articleClassifierItems != null)
				{
					var uniqueAClassifiers = articleClassifierItems.Select(x => x.Gewerk).Distinct().ToList();
					for(int i = 0; i < uniqueAClassifiers.Count; i++)
					{
						if(!string.IsNullOrEmpty(uniqueAClassifiers[i]) && !string.IsNullOrWhiteSpace(uniqueAClassifiers[i]))
						{
							Gewerks.Add(new Models.Production.Reporting.StandardReportModel.Gewerk { Id = i, Name = uniqueAClassifiers[i] });
							var gewerks = articleClassifierItems.FindAll(p => p.Gewerk.ToLower() == uniqueAClassifiers[i].ToLower());
							foreach(var item in gewerks)
							{
								GewerkItems.Add(new Models.Production.Reporting.StandardReportModel.GewerkItem { GewerkId = i, Name = item.Bezeichnung });
							}
						}
					}
				}

				var dataSet = new DataSet("Data");
				dataSet.Tables.Add(newTable("Prd", productionsData));
				dataSet.Tables.Add(newTable("Itm", itemsData));
				dataSet.Tables.Add(newTable("Gew", Gewerks));
				dataSet.Tables.Add(newTable("Pos", GewerkItems));

				return new Infrastructure.Services.Reporting.ReportGenerator("ProductionTechnical.frx", dataSet).GeneratePdfFile();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null || (!this._user.Access.Purchase.ModuleActivated && !this._user.Access.CustomerService.ModuleActivated))
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			var errors = new List<ResponseModel<byte[]>.ResponseError>();

			var productionEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._productionNumber);
			if(productionEntity == null)
			{
				errors.Add(new ResponseModel<byte[]>.ResponseError()
				{
					Key = "",
					Value = "Production not found"
				});
			}

			if(errors.Count > 0)
			{
				return new ResponseModel<byte[]>()
				{
					Errors = errors,
				};
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}

		private string getPreReference(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity productionEntity)
		{
			switch(productionEntity.Lagerort_id)
			{
				default:
					return string.Empty;
				case 6:
					return "CZ";
				case 7:
					return "TN";
				case 60:
					return "BETN";
				case 42:
					return "WS";
				case 26:
					return "AL";
				case 15:
					return "D";
			}
		}


		private string getS01(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity productionEntity)
		{
			switch(productionEntity.Lagerort_id)
			{
				default:
					return string.Empty;
				case 7:
					return "P S Z  Tunisie S.a.r.l";
				case 6:
					return "PSZ/SC Czech";
				case 26:
					return "PSZ Albania";
				case 15:
					return "PSZ D";
			}
		}

		private string getF12(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity productionEntity)
		{
			switch(productionEntity.Lagerort_id)
			{
				default:
					return string.Empty;
				case 6:
					return "Czech Republic";
				case 7:
					return "Tunisie sarl";
				case 60:
					return "Tunisie sarl";
				case 42:
					return "Tunisie sarl";
				case 26:
					return "Albania";
				case 15:
					return "electronic GmbH";
			}
		}

		#region > Helpers
		private static DataTable newTable<T>(string name, IEnumerable<T> list)
		{
			var propInfos = typeof(T).GetProperties();
			var table = table<T>(name, list, propInfos);
			var enumerator = list.GetEnumerator();
			while(enumerator.MoveNext())
			{
				table.Rows.Add(createRow<T>(table.NewRow(), enumerator.Current, propInfos));
			}
			return table;
		}

		private static DataRow createRow<T>(DataRow row, T listItem, PropertyInfo[] propInfos)
		{
			foreach(var propInfo in propInfos)
			{
				row[propInfo.Name.ToString()] = propInfo.GetValue(listItem, null);
			}
			return row;
		}

		private static DataTable table<T>(string name, IEnumerable<T> list, PropertyInfo[] propInfos)
		{
			var table = new DataTable(name);
			foreach(var propInfo in propInfos)
			{
				table.Columns.Add(propInfo.Name, propInfo.PropertyType);
			}
			return table;
		}
		#endregion
	}
}
