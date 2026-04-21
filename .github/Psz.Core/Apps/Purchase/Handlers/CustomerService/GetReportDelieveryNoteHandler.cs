using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.CustomerService
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using SkiaSharp;
	using System.Data;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using ZXing;
	using ZXing.Common;
	using ZXing.SkiaSharp;

	public class GetReportDelieveryNoteHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.CustomerService.OrderReportRequestModel _data { get; set; }
		public GetReportDelieveryNoteHandler(Identity.Models.UserModel user, Models.CustomerService.OrderReportRequestModel data)
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
							new ResponseModel<byte[]>.ResponseError{ Key="", Value = "Delivery Note not found"}
						}
					};
				}
				int languageId = 1;
				if(order.Kunden_Nr.HasValue)
				{
					var sprachentity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer((int)order.Kunden_Nr);
					if(sprachentity.Sprache.HasValue)
						languageId = sprachentity.Sprache.Value;
					else
						languageId = this._data.LanguageId;
				}
				//var languageId = order.Kunden_Nr.HasValue
				//    ? (int)Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer((int)order.Kunden_Nr)?.Sprache
				//    : this._data.LanguageId;
				var languageEntities = Infrastructure.Data.Access.Tables.STG.SprachenAccess.Get();
				var language = new Infrastructure.Data.Entities.Tables.STG.SprachenEntity
				{
					ID = languageId,
					Sprache = languageEntities.FirstOrDefault(x => x.ID == 1).Sprache,
				};

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

				// get order template data
				var reportTemplateResponse = new CustomerService.GetSingleDelieveryNoteHandler(this._user, new Models.RequestModel { TypeId = this._data.TypeId, LanguageId = language.ID }).Handle();
				if(!reportTemplateResponse.Success || reportTemplateResponse.Body == null)
				{
					return new ResponseModel<byte[]>()
					{
						Success = false,
						Errors = new List<ResponseModel<byte[]>.ResponseError>
						{
							new ResponseModel<byte[]>.ResponseError{ Key="", Value = "Report template not found"}
						}
					};
				}

				var customer = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(order.Kunden_Nr ?? -1);
				var orderItems = (Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.OrderId)
						?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()).ToList();
				var reportentity = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.GetByLanguageAndType(language.ID, this._data.TypeId);
				Psz.Core.CustomerService.Helpers.ReportHelper.SetBanksFooterByCustomerFactoring(reportentity, customer.Factoring ?? false);
				var reportModel = new Psz.Core.CustomerService.Reporting.Models.CTS_LSReportModel(reportentity, order);
				reportModel.Items = orderItems?
					.Where(x => x.erledigt_pos != true)
					.OrderBy(x => x.Position)
					.Select(x => new Psz.Core.CustomerService.Reporting.Models.CTC_LSReportItemsModel(x))
					.ToList();
				var footerData = new Psz.Core.CustomerService.Reporting.Models.DocFooterModel(reportentity);

				var Sumweight = 0m;
				foreach(var item in orderItems)
				{
					var artikelItem = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(item.ArtikelNr ?? -1) ?? new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity();
					Sumweight += Math.Round(((item.Anzahl ?? 0) * (artikelItem.Größe ?? 0) / 1000), 2);
				}
				reportModel.SummaryWeightValue = Sumweight.ToString("0.00");
				reportModel.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
				reportModel.Top100Logo = Program.CTS.Top100Logo;
				reportModel.Top100Description = Program.CTS.Top100Description;
				reportModel.Top100_2026Logo = Program.CTS.Top100_2026Logo;
				var reportType = (Infrastructure.Services.Reporting.Helpers.ReportType)this._data.TypeId;
				reportModel.DocumentType = getOrderTypeI18N(reportType, language?.Sprache);
				//
				string Unser_Zeichen1 = string.Empty;
				string Unser_Zeichen2 = string.Empty;
				switch(order?.Unser_Zeichen)
				{
					case "10014":
						Unser_Zeichen1 = "Abteilung FR-F3 / BK 301";
						break;
					case "10096":
						Unser_Zeichen1 = "Rechnungsprüfung";
						break;
					case "10064":
						Unser_Zeichen1 = "Abteilung FR-F3 / BK 22";
						break;
					case "10367":
						Unser_Zeichen1 = "Abteilung FR-F3 / BK316 ";
						break;
					case "10358":
						Unser_Zeichen1 = "Abteilung FR-F3 / BK 22";
						break;
					case "10150":
						Unser_Zeichen1 = "Abteilung FR-F3 / BK 84";
						break;
					default:
						Unser_Zeichen1 = "Buchhaltung";
						break;
				}
				var testlist = new List<string> { "10014", "10064", "10367", "10358", "10150" };
				if(testlist.Contains(order?.Unser_Zeichen))
					Unser_Zeichen2 = "Postfach 700941";
				else
					Unser_Zeichen2 = order?.Straße_Postfach?.Trim();
				reportModel.Unser_Zeichen1 = Unser_Zeichen1;
				reportModel.Address3 = Unser_Zeichen2;
				if(!string.IsNullOrEmpty(order?.Typ) && !string.IsNullOrWhiteSpace(order?.Typ) && order?.Typ == "Lieferschein")
				{
					//if(order?.Vorname_NameFirma == "Sirona Dental Systems GmbH" || order?.Vorname_NameFirma == "Hamm AG")
					if(customer.CodeTypeInLSId.HasValue && customer.CodeTypeInLSId.Value == (int)Psz.Core.BaseData.Enums.AddressEnums.LSCodeTypes.Barcode)
					{
						reportModel.Barcode = $"data:image/png;base64,{GenerateBarcodeBase64($"{order?.Angebot_Nr}")}";
					}
					else
					{
						// - 2024-02-08 - QrCodes for LS-Nummer, LS-Datum & PO-Nummer
						reportModel.QrCodeDocLsNumberDatum = $"data:image/png;base64,{GenerateQrcodeBase64($"{order?.Bezug};;{order?.Angebot_Nr};;{order?.Datum?.ToString("dd.MM.yyyy")}")}";
					}
					if(customer.LsBarCodeDocumentNumber == true)
					{
						reportModel.BarcodeDocumentNumber = $"data:image/png;base64,{GenerateBarcodeBase64($"{order?.Bezug}")}";
					}
				}
				responseBody = await Psz.Core.CustomerService.Reporting.IText.GetItextPDF(new Psz.Core.CustomerService.Reporting.Models.ITextHeaderFooterProps
				{
					BodyData = reportModel,
					DocumentTitle = $" von {reportModel.DocumentType} {order.Angebot_Nr}",
					BodyTemplate = "CTS_LS_Body",
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
				//Psz.Core.CustomerService.Reporting.IText.GetLSReport(reportModel, footerData);
				//await Infrastructure.Services.Reporting.IText.CTS.GetLS(reportModel, footerData);
				return ResponseModel<byte[]>.SuccessResponse(responseBody);
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
				case "französisch":
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
				case "englisch":
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
		internal static string GenerateBarcodeBase64(string content)
		{
			BarcodeWriter barcodeWriter = new BarcodeWriter();
			barcodeWriter.Format = BarcodeFormat.CODE_128;
			barcodeWriter.Options = new EncodingOptions
			{
				Width = 140,
				Height = 30,
				PureBarcode = true,
			};
			var barcodeBitmap = barcodeWriter.Write(content);
			using(var data = barcodeBitmap.Encode(SKEncodedImageFormat.Png, 80))
			{
				using(MemoryStream stream = new MemoryStream())
				{
					return Convert.ToBase64String(data.ToArray());
				}
			}
		}
		internal static string GenerateQrcodeBase64(string content)
		{
			BarcodeWriter barcodeWriter = new BarcodeWriter();
			barcodeWriter.Format = BarcodeFormat.QR_CODE;
			barcodeWriter.Options = new EncodingOptions
			{
				Width = 70,
				Height = 70,
				PureBarcode = true,
				Margin = 0,
			};
			var barcodeBitmap = barcodeWriter.Write(content);
			using(var data = barcodeBitmap.Encode(SKEncodedImageFormat.Png, 80))
			{
				using(MemoryStream stream = new MemoryStream())
				{
					return Convert.ToBase64String(data.ToArray());
				}
			}
		}
	}
}
