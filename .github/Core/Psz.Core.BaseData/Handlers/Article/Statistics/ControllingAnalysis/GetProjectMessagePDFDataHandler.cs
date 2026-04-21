using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetProjectMessagePDFDataHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		private List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemModel> _data { get; set; }
		public GetProjectMessagePDFDataHandler(UserModel user, List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemModel> data)
		{
			this._user = user;
			this._data = data;
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

				return ResponseModel<byte[]>.SuccessResponse(GetData(this._data));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}

		public static byte[] GetData(List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemModel> data, int? pmeldung = 1)
		{
			var updatedResults = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetProjectMessage_PDFData(data?.Select(x => x.ID)?.ToList(), pmeldung)
				?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessagePDF>();

			var results = new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessagePDF>();

			results.AddRange(
				updatedResults.Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessagePDF
				{
					AB_Datum = x.AB_Datum,
					Arbeitszeit_Serien_Pro_Kabesatz = x.Arbeitszeit_Serien_Pro_Kabesatz,
					Artikelnummer = x.Artikelnummer,
					Bemerkungen = x.Bemerkungen,
					EAU = x.EAU,
					EMPB = x.EMPB,
					Erstanlage = x.Erstanlage,
					FA_Datum = x.FA_Datum,
					ID = x.ID,
					Kontakt_AV_PSZ = x.Kontakt_AV_PSZ,
					Kontakt_CS_PSZ = x.Kontakt_CS_PSZ,
					Kontakt_Technik_Kunde = x.Kontakt_Technik_Kunde,
					Kontakt_Technik_PSZ = x.Kontakt_Technik_PSZ,
					Kosten = x.Kosten,
					Krimp_WKZ = x.Krimp_WKZ,
					Material_Eskalation_AV = x.Material_Eskalation_AV,
					Material_Eskalation_Termin = x.Material_Eskalation_Termin,
					Material_Komplett = x.Material_Komplett,
					Menge = x.Menge,
					MOQ = x.MOQ,
					Projekt_betreung = x.Projekt_betreung,
					Projekt_Start = x.Projekt_Start,
					Projektmeldung = x.Projektmeldung,
					Projekt_Nr = x.Projekt_Nr,
					Rapid_Prototyp = x.Rapid_Prototyp,
					Serie_PSZ = x.Serie_PSZ,
					SG_WKZ = x.SG_WKZ,
					Standort_Muster = x.Standort_Muster,
					Standort_Serie = x.Standort_Serie,
					Summe_Arbeitszeit = $"{(decimal.TryParse(x.Summe_Arbeitszeit, out var v) ? v.ToString("0.####") + "Std." : "")}",
					Termin_mit_Technik_abgesprochen = x.Termin_mit_Technik_abgesprochen,
					TSP_Kunden = x.TSP_Kunden,
					Typ = x.Typ,
					UL_Verpackung = x.UL_Verpackung,
					Wunschtermin_Kunde = x.Wunschtermin_Kunde,
					Zuschlag = x.Zuschlag,
					//
					Kunde = x.Kunde,
					Kundenschlussel = x.Kundenschlussel
				}));
			return SaveToPdfFile(results);
		}

		internal static byte[] SaveToPdfFile(List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessagePDF> dataEntities)
		{
			try
			{
				var data = new Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsProjectMessage();

				// -
				data.ReportData = new List<Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsProjectMessage.Header> {
					new Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsProjectMessage.Header {Title= $"'N' Meldung" }
				};

				// - Items
				data.Items = dataEntities
					//.Where(x => data.Suppliers.Exists(y => y.Name1 == x.Name1))
					.Select(x => new Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsProjectMessage.Item
					{
						AB_Datum = x.AB_Datum.HasValue ? x.AB_Datum.Value.ToString("dd/MM/yyyy") : "",
						Arbeitszeit_Serien_Pro_Kabesatz = x.Arbeitszeit_Serien_Pro_Kabesatz ?? 0,
						Artikelnummer = x.Artikelnummer,
						Bemerkungen = x.Bemerkungen?.Trim(),
						EAU = x.EAU ?? 0,
						EMPB = x.EMPB,
						Erstanlage = x.Erstanlage ?? DateTime.MinValue,
						FA_Datum = x.FA_Datum.HasValue ? x.FA_Datum.Value.ToString("dd/MM/yyyy") : "",
						ID = x.ID,
						Kontakt_AV_PSZ = x.Kontakt_AV_PSZ,
						Kontakt_CS_PSZ = x.Kontakt_CS_PSZ,
						Kontakt_Technik_Kunde = x.Kontakt_Technik_Kunde,
						Kontakt_Technik_PSZ = x.Kontakt_Technik_PSZ,
						Kosten = x.Kosten ?? 0,
						Krimp_WKZ = x.Krimp_WKZ,
						Material_Eskalation_AV = x.Material_Eskalation_AV,
						Material_Eskalation_Termin = x.Material_Eskalation_Termin,
						Material_Komplett = x.Material_Komplett,
						Menge = x.Menge ?? 0,
						MOQ = x.MOQ ?? 0,
						Projekt_betreung = x.Projekt_betreung,
						Projekt_Start = x.Projekt_Start,
						Projektmeldung = x.Projektmeldung ?? false,
						Projekt_Nr = x.Projekt_Nr,
						Rapid_Prototyp = x.Rapid_Prototyp,
						Serie_PSZ = x.Serie_PSZ,
						SG_WKZ = x.SG_WKZ,
						Standort_Muster = x.Standort_Muster,
						Standort_Serie = x.Standort_Serie,
						Summe_Arbeitszeit = x.Summe_Arbeitszeit,
						Termin_mit_Technik_abgesprochen = x.Termin_mit_Technik_abgesprochen,
						TSP_Kunden = x.TSP_Kunden,
						Typ = x.Typ,
						UL_Verpackung = x.UL_Verpackung,
						Wunschtermin_Kunde = x.Wunschtermin_Kunde.HasValue ? x.Wunschtermin_Kunde.Value.ToString("dd/MM/yyyy") : "",
						Zuschlag = x.Zuschlag,
						//
						Kunde = x.Kunde,
						Kundenschlussel = x.Kundenschlussel
					})?.OrderByDescending(x => x.Artikelnummer)?.ThenBy(x => x.Projekt_Nr).ToList();

				// -
				return Module.ReportingService.Generate_BSD_ArticlesStatisticsProjectMessage(Infrastructure.Services.Reporting.Helpers.ReportType.BSD_ART_STATS_PROJECTMSG, data);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}
