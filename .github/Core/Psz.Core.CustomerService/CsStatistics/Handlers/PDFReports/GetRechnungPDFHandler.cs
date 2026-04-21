using Infrastructure.Data.Entities.Joins.CTS;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetRechnungPDFHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private RechnungEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRechnungPDFHandler(Identity.Models.UserModel user, RechnungEntryModel data)
		{
			this._user = user;
			this._data = data;
		}
		public async Task<ResponseModel<byte[]>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				byte[] response = null;
				var ReportData = new Psz.Core.CustomerService.Reporting.Models.RechnungStatsReportModel();
				var _lager = GetWarehouse(_data);
				var rechnungEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRechnung(_data.From, _data.To, _lager);
				if(_data.Lager == (int)Enums.OrderEnums.KapazitatLager.WS)
				{
					var tnLager = GetWarehouse(new RechnungEntryModel { Lager = (int)Enums.OrderEnums.KapazitatLager.TN });
					rechnungEntity.AddRange(Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRechnung(_data.From, _data.To, tnLager));
				}
				var _lagerNummer = Enums.OrderEnums.GetLagerNumber((Enums.OrderEnums.KapazitatLager)_data.Lager);

				// - 2024-01-10 - no data for BETN starting Year 2024
				if(_data.Lager == 4)
				{
					_lagerNummer = Enums.OrderEnums.GetLagerNumber(Enums.OrderEnums.KapazitatLager.WS);
					if(_data.From < new DateTime(2024, 1, 1) || _data.To < new DateTime(2024, 1, 1))
					{
						rechnungEntity = new List<Infrastructure.Data.Entities.Joins.CTS.RechnungEntity>();
					}
				}
				var _zollatarif = rechnungEntity?.Select(z => z.Zolltarif_nr)?.Distinct()?.ToList() ?? new List<string>();

				if(rechnungEntity != null)
				{
					// - 2025-01-09 - Khleil: RG TN = RG TN * 0.8 also RG WS = RG TN + RG WS
					if(_data.Lager == (int)Enums.OrderEnums.KapazitatLager.TN)
					{
						var percentage = 0.8m;
						rechnungEntity.ForEach(x =>
						{
							x.Ausdr3 = (x.Ausdr3 ?? 0) * percentage;
							x.Preis = (x.Preis ?? 0) * percentage;
							x.PREIS_Mit_Zusatzkosten_Pro_Stück = (x.PREIS_Mit_Zusatzkosten_Pro_Stück ?? 0) * percentage;
							x.Preis_Total_Mit_Zusatzkosten = (x.Preis_Total_Mit_Zusatzkosten ?? 0) * percentage;
						});
					}

					var RechnungReportParametersEntity = Infrastructure.Data.Access.Tables.CTS.RechnungReportingAccess.GetByLagerIdAndType(_lagerNummer, "Rechnung");
					ReportData = new Psz.Core.CustomerService.Reporting.Models.RechnungStatsReportModel
					{
						ReportParameters = new Psz.Core.CustomerService.Reporting.Models.RechnungStatsReportHeaderModel(RechnungReportParametersEntity, new Psz.Core.CustomerService.Reporting.Models.RechnungStatsReportEntryModel
						{
							From = _data.From,
							To = _data.To,
							Lager = _data.Lager,
							RechnungsDatum = _data.RechnungsDatum
						},
						PrepareRechnungNummer(_data)),

						Zollatarif = _zollatarif.OrderBy(s => s).Select(z => new Psz.Core.CustomerService.Reporting.Models.RechnungStatsReportGroupedZollaTarif
						{
							Zolltarif_nummer = z,
							Gewicht = Helpers.FormatHelper.FormatNumber(rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.Sum(a => a.Gesamtgewicht / 1000) ?? 0, "."),
							Positionen = rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.ToList()?.Count ?? 0,
							Lohnleistun = Helpers.FormatHelper.FormatNumber(rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.Sum(a => a.Ausdr3) ?? 0, "."),
							Zusatzkosten = Helpers.FormatHelper.FormatNumber(rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.Sum(a => a.Zusatzkosten_Produktion) ?? 0, "."),
							Material = Helpers.FormatHelper.FormatNumber(rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.Sum(a => a.Material1) ?? 0, "."),
							Stat_Wert = Helpers.FormatHelper.FormatNumber(rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.Sum(a => a.Material1 + a.Ausdr3 + a.Zusatzkosten_Produktion) ?? 0, "."),
						}).ToList(),
					};

					// - 2025-01-10 - TN bills WS not PSZ GmbH - Khelil/Kechiche
					if(_data.Lager == (int)Enums.OrderEnums.KapazitatLager.TN)
					{
						var wsEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetTNWS();
						var countryEntity = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(wsEntity?.CountryId ?? 0);
						ReportData.ReportParameters.Header2 = $"{wsEntity.Name}";
						ReportData.ReportParameters.Header3 = $"{wsEntity.Address}";
						ReportData.ReportParameters.Header4 = $"{wsEntity.PostalCode} {wsEntity.City}".Trim();
						ReportData.ReportParameters.Header5 = $"{(!string.IsNullOrEmpty(countryEntity?.Name) ? countryEntity?.Name : wsEntity.Country)}".Trim();
					}
				}
				var Details = rechnungEntity?.Select(r => new Psz.Core.CustomerService.Reporting.Models.RechnungStatsReportDetailsModel(r)).ToList()
					?? new List<Psz.Core.CustomerService.Reporting.Models.RechnungStatsReportDetailsModel>();
				ReportData.Details = SetRepeated(Details);
				var typ = new Enums.ReportingEnums.ReportType();
				if(_data.Lager == (int)Enums.OrderEnums.KapazitatLager.CZ)
				{
					typ = Enums.ReportingEnums.ReportType.CTS_RECHNUNGCZ;
					ReportData.ReportParameters.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetCZ()?.Logo)}";
				}
				if(_data.Lager == (int)Enums.OrderEnums.KapazitatLager.TN) // - 2025-01-10 TN is missing logo
				{
					typ = Enums.ReportingEnums.ReportType.CTS_RECHNUNGTN;
					ReportData.ReportParameters.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetTN()?.Logo)}";
				}
				if(_data.Lager == 5 || _data.Lager == 4) // - 2024-01-10 BETN show Header from WS
				{
					typ = Enums.ReportingEnums.ReportType.CTS_RECHNUNGTN;
					//if(ReportData.ReportParameters?.Count > 0)
					//{
					ReportData.ReportParameters.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetTNWS()?.Logo)}";
					//Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetTNWS()?.Logo ?? ReportData.ReportParameters.Logo;
					//}
				}
				if(_data.Lager == 6)
				{
					typ = Enums.ReportingEnums.ReportType.CTS_RECHNUNGTN;
					//if(ReportData.ReportParameters?.Count > 0)
					//{
					ReportData.ReportParameters.Logo = ReportData.ReportParameters.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetTNGZ()?.Logo)}";

					//Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetTNGZ()?.Logo ?? ReportData.ReportParameters[0].Logo;
					//}
				}
				if(_data.Lager == (int)Enums.OrderEnums.KapazitatLager.AL)
				{
					typ = Enums.ReportingEnums.ReportType.CTS_RECHNUNGAL;
					ReportData.ReportParameters.Logo = ReportData.ReportParameters.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetAL()?.Logo)}";
				}

				if(ReportData.Details != null && ReportData.Details.Count > 0)
				{
					ReportData.TotalAusdr3 = Helpers.FormatHelper.FormatNumber(ReportData.Details.Sum(x => x.Ausdr3_decimal), ".");
					ReportData.TotalZusatzkosten_Produktion = Helpers.FormatHelper.FormatNumber(ReportData.Details.Sum(x => x.Zusatzkosten_Produktion_Decimal), ".");
					ReportData.TotalPreis_Total_Mit_Zusatzkosten = Helpers.FormatHelper.FormatNumber(ReportData.Details.Sum(x => x.Preis_Total_Mit_Zusatzkosten_Decimal), ".");
				}
				if(ReportData.Zollatarif != null && ReportData.Zollatarif.Count > 0)
				{
					var ZollatarifDecimal = _zollatarif.OrderBy(s => s).Select(z => new Infrastructure.Services.Reporting.Models.CTS.RechnungGroupedZollaTarifDecmial
					{
						Zolltarif_nummer = z,
						Gewicht = rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.Sum(a => a.Gesamtgewicht / 1000) ?? 0,
						Positionen = rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.ToList()?.Count ?? 0,
						Lohnleistun = rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.Sum(a => a.Ausdr3) ?? 0,
						Zusatzkosten = rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.Sum(a => a.Zusatzkosten_Produktion) ?? 0,
						Material = rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.Sum(a => a.Material1) ?? 0,
						Stat_Wert = rechnungEntity?.Where(x => x.Zolltarif_nr == z)?.Sum(a => a.Material1 + a.Ausdr3 + a.Zusatzkosten_Produktion) ?? 0,
					}).ToList();
					ReportData.TotalGewecht = Helpers.FormatHelper.FormatNumber(ZollatarifDecimal.Sum(x => x.Gewicht), ".");
					ReportData.TotalPositionen = ZollatarifDecimal.Sum(x => x.Positionen).ToString();
					ReportData.TotalLohnleistun = Helpers.FormatHelper.FormatNumber(ZollatarifDecimal.Sum(x => x.Lohnleistun), ".");
					ReportData.TotalZusatkosten = Helpers.FormatHelper.FormatNumber(ZollatarifDecimal.Sum(x => x.Zusatzkosten), ".");
					ReportData.TotalMaterial = Helpers.FormatHelper.FormatNumber(ZollatarifDecimal.Sum(x => x.Material), ".");
					ReportData.TotalStatWert = Helpers.FormatHelper.FormatNumber(ZollatarifDecimal.Sum(x => x.Stat_Wert), ".");
				}
				/// -
				var reportEntity = Infrastructure.Data.Access.Tables.CTS.RechnungReportingAccess.GetByLagerIdAndType(_lager.Key, "Rechnung");
				var footerData = new Psz.Core.CustomerService.Reporting.Models.CTS_StatsRG_DocFooterModel(reportEntity, _data.Lager);

				response = await Reporting.IText.GetItextPDF(new ITextHeaderFooterProps
				{
					BodyData = ReportData,
					BodyTemplate = "CTS_STATS_RG_Body",
					DocumentTitle = "",
					FooterCenterText = "",
					FooterData = footerData,
					FooterLeftText = "",
					FooterTemplate = "CTS_STATS_RG_Footer",
					FooterWithCounter = false,
					HasFooter = true,
					HasHeader = true,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = true,
					HeaderText = "",
					Logo = null,
					HeaderFirstPageOnly = false,

				});
				//await Psz.Core.CustomerService.Reporting.IText.GetCTSRechnungReport(ReportData, footerData);
				//await Infrastructure.Services.Reporting.IText.CTS.GetCTSRG(ReportData, footerData);
				//Module.CS_ReportingService.GenerateRechnungReport(typ, ReportData);

				return ResponseModel<byte[]>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public Task<ResponseModel<byte[]>> ValidateAsync()
		{
			if(this._user == null/*this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponseAsync();
			}
			return ResponseModel<byte[]>.SuccessResponseAsync();
		}
		public static KeyValuePair<int, int> GetWarehouse(RechnungEntryModel _data)
		{
			switch(_data.Lager)
			{
				case (int)Enums.OrderEnums.KapazitatLager.CZ:
					return new KeyValuePair<int, int>(6, 3);
				case (int)Enums.OrderEnums.KapazitatLager.TN:
					return new KeyValuePair<int, int>(7, 4);
				case (int)Enums.OrderEnums.KapazitatLager.BETN:
					return new KeyValuePair<int, int>(60, 62);
				case (int)Enums.OrderEnums.KapazitatLager.GZTN:
					return new KeyValuePair<int, int>(102, 106);
				case (int)Enums.OrderEnums.KapazitatLager.WS:
					return new KeyValuePair<int, int>(42, 40);
				case (int)Enums.OrderEnums.KapazitatLager.AL:
					return new KeyValuePair<int, int>(26, 25);
				default:
					return new KeyValuePair<int, int>();
			}
		}
		public static string PrepareRechnungNummer(RechnungEntryModel _data)
		{
			var Suffix = $"{_data.From.Day.ToString()}-{_data.To.Day.ToString()}-{_data.To.Month.ToString()}-{_data.To.Year.ToString()}";
			switch(_data.Lager)
			{
				case (int)Enums.OrderEnums.KapazitatLager.CZ:
					return $"CZ-{Suffix}";
				case (int)Enums.OrderEnums.KapazitatLager.TN:
					return $"TN-{Suffix}";
				case (int)Enums.OrderEnums.KapazitatLager.GZTN:
					return $"GZTN-{Suffix}";
				case (int)Enums.OrderEnums.KapazitatLager.BETN:
					return $"WS/BETN-{Suffix}"; // - 2024-01-11 BETN show in WS
				case (int)Enums.OrderEnums.KapazitatLager.WS:
					return $"WS-{Suffix}";
				case (int)Enums.OrderEnums.KapazitatLager.AL:
					return $"AL-{Suffix}";
				default:
					return "";
			}
		}
		public string PrepareLagerText()
		{
			switch(_data.Lager)
			{

				case (int)Enums.OrderEnums.KapazitatLager.TN:
					return $"Kasr Hellal";
				case (int)Enums.OrderEnums.KapazitatLager.BETN: // return $"Bennane"; // - 2024-01-11 BETN show in WS
				case (int)Enums.OrderEnums.KapazitatLager.WS:
					return $"WS";
				case (int)Enums.OrderEnums.KapazitatLager.GZTN:
					return $"Ghezala";
				default:
					return "";
			}
		}
		public string PrepareAdress()
		{
			switch(_data.Lager)
			{

				case (int)Enums.OrderEnums.KapazitatLager.TN:
					return $"PSZ Tunisie sarl - Zone industrielle Hadj Ali Soua - Ksar Hellal";
				case (int)Enums.OrderEnums.KapazitatLager.BETN: // return $"PSZ Tunisie sarl - Zone industrielle Hadj Ali Soua - Bennane"; // - 2024-01-11 BETN show in WS
				case (int)Enums.OrderEnums.KapazitatLager.WS:
					return $"PSZ Tunisie sarl - Zone industrielle Bouhjar - 5015 Bouhjar";
				case (int)Enums.OrderEnums.KapazitatLager.GZTN:
					return $"PSZ Tunisie sarl - Zone industrielle - Ghezala";
				default:
					return "";
			}
		}
		public static List<Psz.Core.CustomerService.Reporting.Models.RechnungStatsReportDetailsModel> SetRepeated(List<Psz.Core.CustomerService.Reporting.Models.RechnungStatsReportDetailsModel> data)
		{
			var result = new List<Psz.Core.CustomerService.Reporting.Models.RechnungStatsReportDetailsModel>();
			var FAList = data.Select(f => f.Fertigungsnummer).ToList();
			var _repeatedFA = FAList.GroupBy(x => x)
			   .Where(g => g.Count() > 1)
			   .Select(y => y.Key)
			   .ToList();
			result = data.Select(d => new Psz.Core.CustomerService.Reporting.Models.RechnungStatsReportDetailsModel
			{
				Ausdr3 = d.Ausdr3,
				Datum = d.Datum,
				Fertigungsnummer = d.Fertigungsnummer,
				Originalanzahl = d.Originalanzahl,
				Anzahl_erledigt = d.Anzahl_erledigt,
				Anzahl = d.Anzahl,
				Artikelnummer = d.Artikelnummer,
				Bezeichnung1 = d.Bezeichnung1,
				Betrag = d.Betrag,
				Preis = d.Preis,
				Bemerkung = d.Bemerkung,
				Bezfeld = d.Bezfeld,
				Erstmuster = d.Erstmuster,
				Zolltarif_nr = d.Zolltarif_nr,
				Material1 = d.Material1,
				Größe = d.Größe,
				Gesamtgewicht = d.Gesamtgewicht,
				Stundensatz = d.Stundensatz,
				MinutenKosten = d.MinutenKosten,
				Zusatzkosten_FA_Basis_30_Min = d.Zusatzkosten_FA_Basis_30_Min,
				PREIS_Mit_Zusatzkosten_Pro_Stück = d.PREIS_Mit_Zusatzkosten_Pro_Stück,
				Zusatzkosten_Produktion = d.Zusatzkosten_Produktion,
				Preis_Total_Mit_Zusatzkosten = d.Preis_Total_Mit_Zusatzkosten,
				Zusatzkosten_FA_Basis_30_Min_Decimal = d.Zusatzkosten_FA_Basis_30_Min_Decimal,
				Originalanzahl_Decimal = d.Originalanzahl_Decimal,
				Repeated = _repeatedFA != null && _repeatedFA.Count > 0 && _repeatedFA.Contains(d.Fertigungsnummer) ? 1 : 0,
				Ausdr3_decimal = d.Ausdr3_decimal,
				Zusatzkosten_Produktion_Decimal = d.Zusatzkosten_Produktion_Decimal,
				Preis_Total_Mit_Zusatzkosten_Decimal = d.Preis_Total_Mit_Zusatzkosten_Decimal,
			}).ToList();

			return result;
		}
	}
}