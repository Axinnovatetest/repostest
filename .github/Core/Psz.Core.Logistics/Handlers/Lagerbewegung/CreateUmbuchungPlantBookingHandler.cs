namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	using Psz.Core.Common.Models;
	using Psz.Core.Logistics.Models.Lagebewegung.PDFReports;
	using Psz.Core.SharedKernel.Interfaces;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class CreateUmbuchungPlantBookingHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private List<Models.Lagebewegung.LagerbewegungDetailsPlantBookingModel> _data { get; set; }


		public CreateUmbuchungPlantBookingHandler(Identity.Models.UserModel user, List<Models.Lagebewegung.LagerbewegungDetailsPlantBookingModel> listPosition)
		{
			this._user = user;
			this._data = listPosition;

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
				long idLagerbewegung = -1;
				long response = -1;
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
					string bezeichnung1 = null;
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(item.artikelNr);
					if(articleEntity != null)
					{
						bezeichnung1 = articleEntity.Bezeichnung1;
					}
					var insertedNr = Infrastructure.Data.Access.Tables.Logistics.LagerBewgungPlantBookingAccess.AddlistUmbuchungPositionWithTransaction(new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsPlantBookingEntity
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
						WereingangId = item.WereingangId,
						WereingangIdNach = item.WereingangIdNach,
						gebuchtVon = "Gebucht durch " + this._user.Username + " " + DateTime.Now.ToString("dd-MM-yyyy hh:mm"),
						//receivedQuantity=item.TransferableQuantity

						receivedQuantity = item.receivedQuantity ?? 0
					}, botransaction.connection, botransaction.transaction);
					listIDPosition.Add(insertedNr);
					//idLagerbewegung = item.idLagerbewegung;
					//------------Update Menge
					//------------Update Menge

					//Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.updateMengeArtikelImLagerWithTransaction(item.artikelNr, item.lagerVon, -item.anzahl, botransaction.connection, botransaction.transaction);
					
					Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.updateMengeArtikelImLagerWithTransaction(item.artikelNrNach, item.lagerNach, item.TransferableQuantity, botransaction.connection, botransaction.transaction);

					var resultGet = Infrastructure.Data.Access.Joins.Logistics.LagerArtikelAccess.GetListFilteredArtikelLagerPlantBooking(item.lagerVon);

					//var resultGet = Infrastructure.Data.Access.Tables.Logistics.LagerArtikelAccess.GetListFilteredArtikelLagerPlantBooking(item.lagerVon);
					var FullArticle = resultGet.Where(x => x.wereingangId == item.WereingangId).FirstOrDefault();
					//if(FullArticle.Anzahl >= FullArticle.UbertrageneMenge + item.TransferableQuantity)
					if(FullArticle.Anzahl >= 0)
					{
						if(item.FromRealOrder ?? false)
						{
							var nr = FullArticle.wereingangId;
							Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.UpdateTransferedQuantityWithTransaction(nr ?? -1, item.TransferableQuantity, botransaction.connection, botransaction.transaction);
						}
					}
					else
					{
						return ResponseModel<byte[]>.FailureResponse("Quantity to be transfered exceeded the actual booked quantity !");
					}
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
					response = idLagerbewegung;
				}
				else
				{
					response = -1;
				}


				byte[] responsebyte = null;

				var ReportData = new ReportPlantBookingLagerbewegungModel();
				ReportData.Headers = new List<HeaderReportLagerbewegungModel> { };
				var lagerbwegungHeaderEntity = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetLagerbewegungById(response);
				if(lagerbwegungHeaderEntity != null && lagerbwegungHeaderEntity.id > 0)
				{
					ReportData.Headers.Add(new HeaderReportLagerbewegungModel(lagerbwegungHeaderEntity.id, lagerbwegungHeaderEntity.typ, lagerbwegungHeaderEntity.datum != null ? lagerbwegungHeaderEntity.datum.Value.ToString("dd-MM-yyyy") : "", lagerbwegungHeaderEntity.gebuchtVon));
				}

				var lagerbewegungDetailsEntity0 = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetLagerbewegungDetailsByID(response);

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
				if(lagerbewegungDetailsEntity != null && lagerbewegungDetailsEntity.Count > 0)
				{
					//filling details list
					ReportData.Details = lagerbewegungDetailsEntity.Select(a => new DetailsReportPlantBookingLagerbewegungModel(a)).ToList();

					//var _Headers1 = bestandEntity.Select(a => new
					//{
					//	a.versanddatum_Auswahl,
					//	a.versandarten_Auswahl,
					//	a.lVornameNameFirma,
					//	a.lStrassePostfach,
					//	a.lName2,
					//	a.lName3,
					//	a.lAnsprechpartner,
					//	a.lAbteilung,
					//	a.lLandPLZORT
					//}
					//	  ).Distinct().ToList();

					////filling lager list
					//foreach(var Header in _Headers1)
					//{
					//	ReportData.Headers.Add(new HeaderReportModel(Header.versanddatum_Auswahl != null ? Header.versanddatum_Auswahl.Value.ToString("dd-MM-yyyy") : ""
					//		, Header.versandarten_Auswahl, Header.lVornameNameFirma, Header.lStrassePostfach
					//		, Header.lName2, Header.lName3, Header.lAnsprechpartner, Header.lAbteilung, Header.lLandPLZORT));

					//}
				}

				responsebyte = Module.Logistic_ReportingService.GeneratePlantBookingLagerbewegungReport(Enums.ReportingEnums.ReportType.LagerbewegungPlantBooking, ReportData);

				return ResponseModel<byte[]>.SuccessResponse(responsebyte);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null

			)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}



			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}