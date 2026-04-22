using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Gutshrift;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class GetGutshriftReportHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private GutshriftReportRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetGutshriftReportHandler(Identity.Models.UserModel user, GutshriftReportRequestModel data)
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

				byte[] responseBody = null;

				var gutshrift = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.GutshriftId);
				if(gutshrift == null)
				{
					return new ResponseModel<byte[]>()
					{
						Success = false,
						Errors = new List<ResponseModel<byte[]>.ResponseError>
						{
							new ResponseModel<byte[]>.ResponseError{ Key="", Value = "gutshrift not found"}
						}
					};
				}
				var languageId = gutshrift.Kunden_Nr.HasValue
				   ? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer((int)gutshrift.Kunden_Nr)?.Sprache ?? this._data.LanguageId
				   : this._data.LanguageId;

				var language = new Infrastructure.Data.Entities.Tables.STG.SprachenEntity
				{
					ID = languageId,
					Sprache = "deutsch"
				};

				var languageEntities = Infrastructure.Data.Access.Tables.STG.SprachenAccess.Get();
				if(languageEntities != null || languageEntities.Count > 0)
				{
					var idx = languageEntities.FindIndex(l => l.ID == languageId);
					if(idx >= 0)
						language = languageEntities[idx];
					else
					{
						idx = languageEntities.FindIndex(l => l.Sprache.ToLower() == "deutsch");
						if(idx >= 0)
							language = languageEntities[idx];
						else
							language = languageEntities[0];
					}
				}

				var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.GutshriftId);
				var customer = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(orderEntity.Kunden_Nr ?? -1);
				var reportType = (Enums.ReportingEnums.ReportType)this._data.TypeId;
				var reportEntity = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.GetByLanguageAndType(language.ID, this._data.TypeId);
				Psz.Core.CustomerService.Helpers.ReportHelper.SetBanksFooterByCustomerFactoring(reportEntity, customer.Factoring ?? false);
				var itemsEntity = (Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.GutshriftId)
						?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>())
						.Where(x => x.erledigt_pos != true).OrderBy(x => x.Position).ToList();
				var buyerEntity = Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.GetByOrderType(gutshrift.Nr, (int)Enums.ReportingEnums.OrderTypes.Order);
				var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderEntity.Kunden_Nr ?? -1);
				var reportModel = new Psz.Core.CustomerService.Reporting.Models.CTS_GSReportModel(
					reportEntity,
					orderEntity,
					buyerEntity,
					addressEntity,
					itemsEntity?[0].USt ?? 0m
					);
				;
				var itemsReportModel = itemsEntity?.Select(x => new Psz.Core.CustomerService.Reporting.Models.CTS_GSReportItemsModel(x)).ToList();
				reportModel.Items = itemsReportModel;
				reportModel.DocumentType = getOrderTypeI18N(reportType, language?.Sprache);

				reportModel.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
				reportModel.Top100Logo = Module.CTS.Top100Logo;
				reportModel.Top100Description = Module.CTS.Top100Description;
				reportModel.Top100_2026Logo = Module.CTS.Top100_2026Logo;
				//sums
				var summarySum = itemsEntity?.Sum(x => Convert.ToDecimal(x.Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture));
				var summaryUST = itemsEntity?.Sum(x => Convert.ToDecimal(x.Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture) * Convert.ToDecimal(x.USt, System.Globalization.CultureInfo.InvariantCulture));
				reportModel.SummarySumValue = $"{summarySum.Value.ToString("0.00")} €";
				reportModel.SummaryUSTValue = $"{summaryUST.Value.ToString("0.00")} €";
				reportModel.SummaryTotalValue = $"{(summarySum + summaryUST).Value.ToString("0.00")} €";
				var footerData = new Psz.Core.CustomerService.Reporting.Models.DocFooterModel(reportEntity);

				responseBody = await Reporting.IText.GetItextPDF(new Reporting.Models.ITextHeaderFooterProps
				{
					BodyData = reportModel,
					DocumentTitle = $" von {reportModel.DocumentType} {orderEntity.Angebot_Nr}",
					BodyTemplate = "CTS_GS_Body",
					FooterCenterText = null,
					FooterData = footerData,
					FooterLeftText = null,
					FooterTemplate = "CTS_Footer",
					HasFooter = true,
					FooterWithCounter = false,
					HasHeader = true,
					HeaderLogoWithCounter = true,
					HeaderLogoWithText = false,
					HeaderText = null,
					Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}",
					HeaderFirstPageOnly = false
				});
				//Psz.Core.CustomerService.Reporting.IText.GetGSReport(reportModel, footerData);
				//await Infrastructure.Services.Reporting.IText.CTS.GetGS(reportModel, footerData);
				return ResponseModel<byte[]>.SuccessResponse(responseBody);

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
		public static string cleanArticleSuffix(string articlenumber)
		{
			// - 2022-04-26 - Khelil remove only Site Suffixes (TN, AL, DE)
			articlenumber = articlenumber.Trim();
			if(string.IsNullOrWhiteSpace(articlenumber) || articlenumber.Length < 2)
			{
				return articlenumber;
			}
			// -
			if(articlenumber.ToLower().EndsWith("al") || articlenumber.ToLower().EndsWith("tn") || articlenumber.ToLower().EndsWith("de"))
			{
				return articlenumber.Substring(0, articlenumber.Length - 2);
			}
			// -
			return articlenumber;
		}
		internal string getOrderTypeI18N(Enums.ReportingEnums.ReportType orderType, string language)
		{
			switch(language.ToLower())
			{
				case "deutsch":
					{
						switch(orderType)
						{
							case Enums.ReportingEnums.ReportType.INVOICE:
								return "Rechnung";
							case Enums.ReportingEnums.ReportType.ORDER_CONFIRMATION:
								return "Auftragsbestätigung";
							case Enums.ReportingEnums.ReportType.ORDER_FORECAST:
								return "Bedarfsprognose / Bedarfsvorschau";
							case Enums.ReportingEnums.ReportType.ORDER_KANBAN:
								return "Kanban";
							case Enums.ReportingEnums.ReportType.ORDER_CONTRACT:
								return "Rahmenauftrag";
							case Enums.ReportingEnums.ReportType.ORDER_DELIVERY:
								return "Lieferschein";
							case Enums.ReportingEnums.ReportType.ORDER_CREDIT:
								return "Gutschrift";
							default:
								return "";
						}
					}
				case "français":
					{
						switch(orderType)
						{
							case Enums.ReportingEnums.ReportType.INVOICE:
								return "Facture";
							case Enums.ReportingEnums.ReportType.ORDER_CONFIRMATION:
								return "Confirmation de commande";
							case Enums.ReportingEnums.ReportType.ORDER_FORECAST:
								return "Prévision des besoins";
							case Enums.ReportingEnums.ReportType.ORDER_KANBAN:
								return "Kanban";
							case Enums.ReportingEnums.ReportType.ORDER_CONTRACT:
								return "Contrat-cadre";
							case Enums.ReportingEnums.ReportType.ORDER_DELIVERY:
								return "Bon de livraison";
							case Enums.ReportingEnums.ReportType.ORDER_CREDIT:
								return "Crédit";
							default:
								return "";
						}
					}
				case "english":
				default:
					{
						switch(orderType)
						{
							case Enums.ReportingEnums.ReportType.INVOICE:
								return "Invoice";
							case Enums.ReportingEnums.ReportType.ORDER_CONFIRMATION:
								return "Order confirmation";
							case Enums.ReportingEnums.ReportType.ORDER_FORECAST:
								return "Forecast";
							case Enums.ReportingEnums.ReportType.ORDER_KANBAN:
								return "Kanban";
							case Enums.ReportingEnums.ReportType.ORDER_CONTRACT:
								return "Blanket order";
							case Enums.ReportingEnums.ReportType.ORDER_DELIVERY:
								return "Delivery note";
							case Enums.ReportingEnums.ReportType.ORDER_CREDIT:
								return "Credit";
							default:
								return "";
						}
					}
			}
		}
	}
}