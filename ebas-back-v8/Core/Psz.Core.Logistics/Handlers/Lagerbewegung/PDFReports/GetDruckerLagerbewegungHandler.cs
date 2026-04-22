using Psz.Core.Logistics.Models.Lagebewegung.PDFReports;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung.PDFReports
{
	public class GetDruckerLagerbewegungHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private List<Models.Lagebewegung.LagerbewegungDetailsModel> _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetDruckerLagerbewegungHandler(Identity.Models.UserModel user, List<Models.Lagebewegung.LagerbewegungDetailsModel> data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				//---------------------------------------------
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				long idLagerbewegung = -1;
				long response0 = -1;
				List<int> listIDPosition = new List<int>();
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				botransaction.beginTransaction();
				var insertedNrHeader = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.AddLagebewegungHeaderWithTransaction(new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity
				{
					typ = "Umbuchung",
				}, botransaction.connection, botransaction.transaction
			   );
				idLagerbewegung = (long)insertedNrHeader;
				var deletePosition = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.DeletelistPositionByIdLagerbewegungWithTransaction(idLagerbewegung, botransaction.connection, botransaction.transaction);

				foreach(var item in this._data)
				{
					//-----------------------------------------------------
					var position0 = Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.GetBestandArtikelBy(item.lagerVon, item.artikelNr, botransaction.connection, botransaction.transaction);
					if(position0 != null && position0.artikelNr > 0)
					{
						item.bestandLagervonVor = (decimal)position0.bestand;
						item.bestandLagervonNach = (decimal)position0.bestand - (decimal)item.anzahl;
						item.lagerortVon = position0.lagerort;


					}
					else
					{
						item.bestandLagervonVor = 0;
						item.bestandLagervonNach = 0;
						item.lagerortVon = "";


					}
					var position1 = Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.GetBestandArtikelBy(item.lagerNach, item.artikelNrNach, botransaction.connection, botransaction.transaction);
					if(position1 != null && position1.artikelNr > 0)
					{
						item.bestandLagernachVor = (decimal)position1.bestand;
						item.bestandLagernachNach = (decimal)position1.bestand + (decimal)item.anzahlNach;
						item.lagerortNach = position1.lagerort;
					}
					else
					{
						item.bestandLagernachVor = 0;
						item.bestandLagernachNach = 0;
						item.lagerortNach = "";
					}
					//-----------------------------------------------------
					string bezeichnung1 = null;
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(item.artikelNr);
					if(articleEntity != null)
					{
						bezeichnung1 = articleEntity.Bezeichnung1;
					}
					var insertedNr = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.AddlistUmbuchungPositionWithTransaction(new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity
					{

						idLagerbewegung = idLagerbewegung,
						artikelNr = item.artikelNr,
						bezeichnung1 = bezeichnung1,
						einheit = item.einheit,
						anzahl = item.anzahl,
						lagerVon = item.lagerVon,
						artikelNrNach = item.artikelNrNach,
						bezeichnung1Nach = bezeichnung1,
						anzahlNach = item.anzahlNach,
						lagerNach = item.lagerNach,
						gebuchtVon = "Gebucht durch " + this._user.Username + " " + DateTime.Now.ToString("dd-MM-yyyy hh:mm"),

					}, botransaction.connection, botransaction.transaction);
					listIDPosition.Add(insertedNr);
					//idLagerbewegung = item.idLagerbewegung;
					//------------Update Menge
					if(item.changed == true || Math.Round((position0.bestand - item.anzahl), 4) != 0)
					{
						Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.updateMengeArtikelImLagerWithTransaction(item.artikelNr, item.lagerVon, -item.anzahl, botransaction.connection, botransaction.transaction);
					}
					else
					{
						Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.updateMengeArtikelToZeroImLagerWithTransaction(item.artikelNr, item.lagerVon, botransaction.connection, botransaction.transaction);
					}



					Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.updateMengeArtikelImLagerWithTransaction(item.artikelNrNach, item.lagerNach, item.anzahlNach, botransaction.connection, botransaction.transaction);

				}

				//------------Upadte Header Lage bewegung
				var header = new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity
				{
					id = idLagerbewegung,
					gebucht = true,
					gebuchtVon = "Gebucht durch " + this._user.Username + " " + DateTime.Now.ToString("dd-MM-yyyy hh:mm")
				};

				var updateNr = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.updateLagebewegungHeaderWithTransaction(header, botransaction.connection, botransaction.transaction);


				if(botransaction.commit())
				{
					response0 = idLagerbewegung;
				}
				else
				{
					response0 = -1;
				}
				//---------------------------------------------

				byte[] response = null;

				var ReportData = new ReportLagerbewegungModel();
				ReportData.Headers = new List<HeaderReportLagerbewegungModel> { };
				var lagerbwegungHeaderEntity = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetLagerbewegungById(response0);
				if(lagerbwegungHeaderEntity != null && lagerbwegungHeaderEntity.id > 0)
				{
					ReportData.Headers.Add(new HeaderReportLagerbewegungModel(lagerbwegungHeaderEntity.id, lagerbwegungHeaderEntity.typ, lagerbwegungHeaderEntity.datum != null ? lagerbwegungHeaderEntity.datum.Value.ToString("dd-MM-yyyy") : "", lagerbwegungHeaderEntity.gebuchtVon));
				}

				var lagerbewegungDetailsEntity0 = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetLagerbewegungDetailsByID(response0);

				//var lagerbewegungDetailsEntity = this._data;
				foreach(var item0 in this._data)
				{
					var x = from e in lagerbewegungDetailsEntity0 where e.artikelNr == item0.artikelNr select e;
					item0.artikelnummer = x.First().artikelnummer;
					item0.artikelnummerNach = x.First().artikelnummerNach;
					item0.einheit = x.First().einheit;
				}
			;
				var lagerbewegungDetailsEntity = this._data;
				lagerbewegungDetailsEntity = lagerbewegungDetailsEntity.OrderBy(x => x.artikelnummer).ToList();

				if(lagerbewegungDetailsEntity != null && lagerbewegungDetailsEntity.Count > 0)
				{
					//filling details list
					ReportData.Details = lagerbewegungDetailsEntity.Select(a => new DetailsReportLagerbewegungModel(a)).ToList();
				}

				response = Module.Logistic_ReportingService.GenerateLagerbewegungReport(Enums.ReportingEnums.ReportType.Lagerbewegung, ReportData);

				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			var ids = new List<int>();
			if(_data?.Count > 0)
			{
				ids.AddRange(_data.Select(x => x.lagerNach));
				ids.AddRange(_data.Select(x => x.lagerVon));
			}
			var warehouseInInventory = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.GetBlockedWarehouses(ids.Distinct());
			if(warehouseInInventory?.Count > 0)
			{
				return ResponseModel<byte[]>.FailureResponse($"Warehouse(s) [{string.Join(",", warehouseInInventory.Take(5)?.Select(x => x.Lagerort_id))}] are currently in Inventory. Movements are restricted.");
			}

			//var hl = Module.BSD.ProductionLagerIds;
			//var pl = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetPLLagers()?.Select(x => x.Lagerort_id)?.ToList();
			//if(hl.Count > 0 && pl.Count > 0)
			//{
			//	var warehouses = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.GetInventoryActive(ids.Distinct());
			//	var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetMinimal(this._data.Select(x => x.artikelNr)?.ToList())?.Where(x => x.Warengruppe?.ToLower() == "roh");
			//	if(articles.Count() > 0 && warehouses?.Count()>0)
			//	{
			//		var errors = new List<string>();
			//		foreach(var item in this._data)
			//		{
			//			// - T1
			//			var article = articles.FirstOrDefault(a => a.ArtikelNr == item.artikelNr);
			//			if(article.Warentyp == 1)
			//			{
			//				// - commission - lager_von = HL or Lager_nach = PL
			//				if(hl.Exists(x => x == item.lagerVon) || pl.Exists(x => x == item.lagerNach))
			//				{
			//					errors.Add($"Warehouses [{item.lagerVon}, {item.lagerNach}] are in inventory, movement is not allowed for Typ1 articles [{article.ArtikelNummer}].");
			//					continue;
			//				}

			//				// - retour
			//				if(hl.Exists(x => x == item.lagerNach) || pl.Exists(x => x == item.lagerVon))
			//				{
			//					errors.Add($"Warehouses [{item.lagerVon}, {item.lagerNach}] are in inventory, movement is not allowed for Typ1 articles [{article.ArtikelNummer}].");
			//					continue;
			//				}
			//			}

			//			// - T2
			//			if(article.Warentyp == 2)
			//			{
			//				// - commission - lager_von = HL or Lager_nach = PL
			//				if(hl.Exists(x => x == item.lagerVon) || pl.Exists(x => x == item.lagerNach))
			//				{
			//					errors.Add($"Warehouses [{item.lagerVon}, {item.lagerNach}] are in inventory, movement is not allowed for Typ2 articles [{article.ArtikelNummer}].");
			//					continue;
			//				}

			//				// - retour
			//				if(hl.Exists(x => x == item.lagerNach) || pl.Exists(x => x == item.lagerVon))
			//				{
			//					errors.Add($"Warehouses [{item.lagerVon}, {item.lagerNach}] are in inventory, movement is not allowed for Typ2 articles [{article.ArtikelNummer}].");
			//					continue;
			//				}
			//			}
			//		}
			//		if(errors.Any())
			//		{
			//			return ResponseModel<byte[]>.FailureResponse(errors);
			//		}
			//	}
			//}
			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}