using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung.PDFReports;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
				//var validationResponse = this.Validate();
				//if(!validationResponse.Success)
				//{
				//	return validationResponse;
				//}

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
				if(lagerbewegungDetailsEntity != null && lagerbewegungDetailsEntity.Count > 0)
				{
					//filling details list
					ReportData.Details = lagerbewegungDetailsEntity.Select(a => new DetailsReportLagerbewegungModel(a)).ToList();

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

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}