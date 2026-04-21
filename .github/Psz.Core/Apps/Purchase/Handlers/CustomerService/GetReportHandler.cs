using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.CustomerService
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Data;
	using System.Linq;
	using System.Threading.Tasks;

	public class GetReportHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.CustomerService.OrderReportRequestModel _data { get; set; }
		public GetReportHandler(Identity.Models.UserModel user, Models.CustomerService.OrderReportRequestModel data)
		{
			_user = user;
			_data = data;
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

				// get order content data
				var order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.OrderId);
				if(order == null)
				{
					return new ResponseModel<byte[]>()
					{
						Success = false,
						Errors = new List<ResponseModel<byte[]>.ResponseError>
						{
							new ResponseModel<byte[]>.ResponseError{ Key="", Value = "Order not found"}
						}
					};
				}

				var languageId = order.Kunden_Nr.HasValue
					? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer((int)order.Kunden_Nr)?.Sprache ?? this._data.LanguageId
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


				//new Logic
				var customer = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(order.Kunden_Nr ?? -1);
				var reportType = (Infrastructure.Services.Reporting.Helpers.ReportType)this._data.TypeId;
				var reportEntity = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.GetByLanguageAndType(language.ID, this._data.TypeId);
				var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.OrderId);
				var itemsEntity = (Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.OrderId)
						?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>())
						.Where(x => x.erledigt_pos != true).OrderBy(x => x.Position).ToList();
				var buyerEntity = Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.GetByOrderType(orderEntity.Nr, (int)Apps.EDI.Enums.OrderEnums.OrderTypes.Order);
				var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderEntity.Kunden_Nr ?? -1);
				Psz.Core.CustomerService.Helpers.ReportHelper.SetBanksFooterByCustomerFactoring(reportEntity, customer.Factoring ?? false);
				var reportModel = new Infrastructure.Services.Reporting.Models.Itext.CTS_ABReportModel(reportEntity, orderEntity, buyerEntity, addressEntity, itemsEntity?.Count > 0 ? itemsEntity?[0].USt ?? 0m : 0);
				var itemsReportModel = itemsEntity?.Select(x => new Infrastructure.Services.Reporting.Models.Itext.CTS_ABReportItemsModel(x)).ToList();
				reportModel.Items = itemsReportModel;
				reportModel.DocumentType = getOrderTypeI18N(reportType, language?.Sprache);

				reportModel.Logo = Program.CTS.Logo;
				reportModel.Top100Description = Program.CTS.Top100Description;
				reportModel.Top100Logo = Program.CTS.Top100Logo;
				reportModel.Top100_2026Logo = Program.CTS.Top100_2026Logo;
				//sums

				var summarySum = itemsEntity?.Sum(x => Convert.ToDecimal(x.Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture));
				var summaryUST = itemsEntity?.Sum(x => Convert.ToDecimal(x.Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture) * Convert.ToDecimal(x.USt, System.Globalization.CultureInfo.InvariantCulture));
				reportModel.SummarySumValue = $"{Infrastructure.Services.ExtensionsClass.FormatDecimal(summarySum.Value, 2)} €";
				reportModel.SummaryUSTValue = $"{Infrastructure.Services.ExtensionsClass.FormatDecimal(summaryUST.Value, 2)} €";
				reportModel.SummaryTotalValue = $"{Infrastructure.Services.ExtensionsClass.FormatDecimal(summarySum.Value + summaryUST.Value, 2)} €";
				var footerData = new Infrastructure.Services.Reporting.Models.Itext.DocFooterModel(reportEntity);
				
				responseBody = await Infrastructure.Services.Reporting.IText.CTS.GetAB(reportModel, footerData);
				return ResponseModel<byte[]>.SuccessResponse(responseBody);
				//

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e, e.StackTrace);
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

		internal string getOrderTypeI18N(Infrastructure.Services.Reporting.Helpers.ReportType orderType, string language)
		{
			switch(language.ToLower())
			{
				case "deutsch":
					{
						switch(orderType)
						{
							case Infrastructure.Services.Reporting.Helpers.ReportType.INVOICE:
								return "Rechnung";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_CONFIRMATION:
								return "Auftragsbestätigung";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_FORECAST:
								return "Bedarfsprognose / Bedarfsvorschau";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_KANBAN:
								return "Kanban";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_CONTRACT:
								return "Rahmenauftrag";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_DELIVERY:
								return "Lieferschein";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_CREDIT:
								return "Gutschrift";
							default:
								return "";
						}
					}
				case "français":
					{
						switch(orderType)
						{
							case Infrastructure.Services.Reporting.Helpers.ReportType.INVOICE:
								return "Facture";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_CONFIRMATION:
								return "Confirmation de commande";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_FORECAST:
								return "Prévision des besoins";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_KANBAN:
								return "Kanban";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_CONTRACT:
								return "Contrat-cadre";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_DELIVERY:
								return "Bon de livraison";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_CREDIT:
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
							case Infrastructure.Services.Reporting.Helpers.ReportType.INVOICE:
								return "Invoice";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_CONFIRMATION:
								return "Order confirmation";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_FORECAST:
								return "Forecast";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_KANBAN:
								return "Kanban";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_CONTRACT:
								return "Blanket order";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_DELIVERY:
								return "Delivery note";
							case Infrastructure.Services.Reporting.Helpers.ReportType.ORDER_CREDIT:
								return "Credit";
							default:
								return "";
						}
					}
			}
		}

	}
}
