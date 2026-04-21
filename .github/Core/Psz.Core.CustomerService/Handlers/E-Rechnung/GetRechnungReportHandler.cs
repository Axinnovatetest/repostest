using Psz.Core.Common.Helpers;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class GetRechnungReportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private RechnungReportRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRechnungReportHandler(Identity.Models.UserModel user, RechnungReportRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<byte[]> Handle()
		{
			var invoiceData = new List<RechnungReportingModel>();
			var invoiceItemData = new List<RechnungReportingItemModel>();
			Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity rechnungReportEntity = null;
			List<RechnungReportingModel> invoiceFields = null;
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				byte[] responseBody = null;
				var order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.RechnungId);
				if(order == null)
				{
					return new ResponseModel<byte[]>()
					{
						Success = false,
						Errors = new List<ResponseModel<byte[]>.ResponseError>
						{
							new ResponseModel<byte[]>.ResponseError{ Key="", Value = "Invoice not found"}
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
				// get order template data
				rechnungReportEntity = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.GetByLanguageAndType(languageId, this._data.TypeId);
				if(rechnungReportEntity == null)
				{
					rechnungReportEntity = new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity
					{
						Id = -1,
						CompanyLogoImageId = -1,
						ImportLogoImageId = -1,
						Header = string.Empty,
						ItemsHeader = string.Empty,
						ItemsFooter1 = string.Empty,
						ItemsFooter2 = string.Empty,
						Footer11 = string.Empty,
						Footer12 = string.Empty,
						Footer13 = string.Empty,
						Footer14 = string.Empty,
						Footer15 = string.Empty,
						Footer16 = string.Empty,
						Footer17 = string.Empty,
						Footer21 = string.Empty,
						Footer22 = string.Empty,
						Footer23 = string.Empty,
						Footer24 = string.Empty,
						Footer25 = string.Empty,
						Footer26 = string.Empty,
						Footer27 = string.Empty,
						Footer31 = string.Empty,
						Footer32 = string.Empty,
						Footer33 = string.Empty,
						Footer34 = string.Empty,
						Footer35 = string.Empty,
						Footer36 = string.Empty,
						Footer37 = string.Empty,
						Footer41 = string.Empty,
						Footer42 = string.Empty,
						Footer43 = string.Empty,
						Footer44 = string.Empty,
						Footer45 = string.Empty,
						Footer46 = string.Empty,
						Footer47 = string.Empty,
						Footer51 = string.Empty,
						Footer52 = string.Empty,
						Footer53 = string.Empty,
						Footer54 = string.Empty,
						Footer55 = string.Empty,
						Footer56 = string.Empty,
						Footer57 = string.Empty,
						Footer61 = string.Empty,
						Footer62 = string.Empty,
						Footer63 = string.Empty,
						Footer64 = string.Empty,
						Footer65 = string.Empty,
						Footer66 = string.Empty,
						Footer67 = string.Empty,
						Footer71 = string.Empty,
						Footer72 = string.Empty,
						Footer73 = string.Empty,
						Footer74 = string.Empty,
						Footer75 = string.Empty,
						Footer76 = string.Empty,
						Footer77 = string.Empty,
						// PSZ Address
						Address1 = string.Empty,
						Address2 = string.Empty,
						Address3 = string.Empty,
						Address4 = string.Empty,
						// Document
						OrderNumberPO = string.Empty,
						DocumentType = string.Empty,
						OrderNumber = string.Empty,
						OrderDate = string.Empty,
						// Client
						ClientNumber = string.Empty,
						InternalNumber = string.Empty,
						ShippingMethod = string.Empty,
						PaymentMethod = string.Empty,
						PaymentTarget = string.Empty,
						UST_ID = string.Empty,
						// Items
						Position = string.Empty,
						Article = string.Empty,
						Description = string.Empty,
						Amount = string.Empty,
						PE = string.Empty,
						BasisPrice150 = string.Empty,
						Cu_G = string.Empty,
						Cu_Surcharge = string.Empty,
						UnitPrice = string.Empty,
						Designation = string.Empty,
						Unit = string.Empty,
						TotalPrice150 = string.Empty,
						DEL = string.Empty,
						Cu_Total = string.Empty,
						UnitTotal = string.Empty,
						//
						Bestellt = string.Empty,
						Geliefert = string.Empty,
						Liefertermin = string.Empty,
						Offen = string.Empty,
						Abladestelle = string.Empty,
						LastPageText1 = string.Empty,
						LastPageText2 = string.Empty,
						LastPageText3 = string.Empty,
						LastPageText4 = string.Empty,
						LastPageText5 = string.Empty,
						LastPageText6 = string.Empty,
						LastPageText7 = string.Empty,
						LastPageText8 = string.Empty,
						LastPageText9 = string.Empty,
						SummarySum = string.Empty,
						SummaryTotal = string.Empty,
						SummaryUST = string.Empty,
						LanguageId = this._data.LanguageId,
						OrderTypeId = this._data.TypeId,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserId = this._user.Id,
						Lieferadresse = string.Empty,
						Index_Kunde = string.Empty
					};
					rechnungReportEntity.Id = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.Insert(rechnungReportEntity);
				}
				var customer = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(nummer: order.Kunden_Nr ?? -1);
				Psz.Core.CustomerService.Helpers.ReportHelper.SetBanksFooterByCustomerFactoring(rechnungReportEntity, customer.Factoring ?? false);
				var rechnungReportModel = new CreateModel(rechnungReportEntity);
				var orders = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>
				{
					order
				};
				var buyerEntity = Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.GetByOrderType(order.Nr, (int)Enums.ReportingEnums.OrderTypes.Order);
				var customerConditions = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customer.Konditionszuordnungs_Nr ?? -1);

				var orderItems = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
				var kundenRechnungType = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.GetByKundennummer(order.Kunden_Nr ?? -1);
				if(order.Typ == Enums.OrderEnums.Types.Invoice.GetDescription())
				{
					if(kundenRechnungType != null && kundenRechnungType.Typ == Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung.GetDescription())
					{
						var sammelInvoices = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByAngebotNr(order.Angebot_Nr.ToString());
						orderItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyAngeboteNrs(sammelInvoices?.Select(x => x.Nr).ToList())?
							.Where(y => y.erledigt_pos != true).ToList();
					}
					else
						orderItems = (Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.RechnungId)
							?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>())
							.Where(x => x.erledigt_pos != true).ToList();
				}
				else
					orderItems = (Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.RechnungId)
						?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>())
						.Where(x => x.erledigt_pos != true).ToList();

				var reportType = order.Typ == Enums.OrderEnums.Types.Invoice.GetDescription() ? Enums.ReportingEnums.ReportType.CTS_RECHNUNG :
					Enums.ReportingEnums.ReportType.ORDER_FORECAST;

				invoiceFields = new List<RechnungReportingModel> { rechnungReportModel.ToInvoiceFields() };
				setBigFooter(invoiceFields, customer.Factoring ?? false);
				NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
				nfi.NumberGroupSeparator = " ";
				nfi.NumberDecimalSeparator = ",";
				var hasRGAbweichendeAdr = false;

				for(int i = 0; i < orders.Count; i++)
				{
					var liefersheinItem = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orders[i].Nr_lie ?? -1);
					var address = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orders[i].Kunden_Nr ?? -1);
					var sumSum = 0m;
					var sumUST = 0m;
					var ust = 0m;
					hasRGAbweichendeAdr = !string.IsNullOrWhiteSpace(customer.RG_Abteilung) || !string.IsNullOrWhiteSpace(customer.RG_Strasse_Postfach) || !string.IsNullOrWhiteSpace(customer.RG_Land_PLZ_ORT);
					for(int j = 0; j < orderItems.Count; j++)
					{
						var rg = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderItems[j].AngebotNr ?? -1);
						var ls = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(rg?.Nr_lie ?? -1);
						if(j == 0)
						{
							ust = orderItems[j].USt ?? 0;
						}
						else
						{
							if(orderItems[j].USt != ust)
								return ResponseModel<byte[]>.FailureResponse("problem Ust value not compatible");
						}
						sumSum += Convert.ToDecimal(orderItems[j].Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture);
						sumUST += Convert.ToDecimal(orderItems[j].Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture) * Convert.ToDecimal(orderItems[j].USt, System.Globalization.CultureInfo.InvariantCulture);
						var _articleItem = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItems[j].ArtikelNr ?? -1);
						invoiceItemData.Add(new RechnungReportingItemModel()
						{
							InvoiceId = i,
							PositionNumber = orderItems[j].Position?.ToString(),
							ItemNumber = _articleItem?.ArtikelNummer?.cleanArticleSuffix(),
							Description = orderItems[j].Bezeichnung1?.ToString(),
							Designation = orderItems[j].Bezeichnung2?.ToString(),
							Amount = orderItems[j].OriginalAnzahl?.ToString(),
							PE = orderItems[j].Preiseinheit?.ToString(),
							Unit = orderItems[j].Einheit?.ToString(),
							BasePrice = orderItems[j].VKEinzelpreis ?? 0m, // basispreis
							TotalPrice = orderItems[j].VKGesamtpreis ?? 0m, // Gesamppreis
							TotalCopper = $"{(orderItems[j].EinzelCuGewicht ?? 0m).FormatDecimal(3)}kg",
							DEL = orderItems[j].DEL?.ToString(),
							SurchargeCopper = orderItems[j].Einzelkupferzuschlag ?? 0m, // Cu-Zuschlag
							TotalSurchargeCopper = orderItems[j].Gesamtkupferzuschlag ?? 0m, // Cu-Zuschlag Gesamt
							UnitPrice = orderItems[j].Einzelpreis ?? 0m, // Einzelpreis
							TotalUnitPrice = orderItems[j].Gesamtpreis ?? 0m, // Einzelpreis Gesamt
							AB_Pos_zu_RA_Pos = $"{orderItems[j]?.ABPoszuRAPos}",
							Liefertermin = $"{orderItems[j]?.Liefertermin?.ToString("dd.MM.yyyy")}",
							Geliefert = $"{orderItems[j]?.Geliefert}",
							Anzahl = orderItems[j]?.Anzahl ?? 0m,
							Bestellt = $"{orderItems[j]?.Bestellnummer}",
							Offen = $"{orderItems[j]?.RA_Offen}",
							Abladestelle = !string.IsNullOrEmpty(orderItems[j]?.Abladestelle) && !string.IsNullOrWhiteSpace(orderItems[j]?.Abladestelle) ?
							$"{rechnungReportEntity?.Abladestelle} {orderItems[j]?.Abladestelle}" : "",
							Postext = kundenRechnungType != null && !kundenRechnungType.Typ.StringIsNullOrEmptyOrWhiteSpaces() && kundenRechnungType.Typ == Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung.GetDescription()
							? $"{orderItems[j].POSTEXT?.ToString()?.Trim()}<br/>{(ls is not null && ls.Angebot_Nr is not null ? $"{rechnungReportEntity?.ForPosDeliveryNote} {ls.Angebot_Nr}" : "")}"
							: orderItems[j].POSTEXT?.ToString()?.Trim(),
							DELFixiert = orderItems[j].DELFixiert == true ? "1" : "0",
							Index_Kunde = orderItems[j].Index_Kunde?.Trim() ?? "",
							DelFixedText = orderItems[j].DELFixiert.HasValue && orderItems[j].DELFixiert.Value ? "DEL fixiert lauf Angebot" : null,
							ExternComment = orderItems[j].GSExternComment,
							TotalUnitSurcharge = $"{(orderItems[j].Zuschlag_VK ?? 0 * orderItems[j].Einzelpreis ?? 0).ToString("n", nfi)} €",
							TotalSurcharge = $"{(orderItems[j].Zuschlag_VK ?? 0 * orderItems[j].Gesamtpreis ?? 0).ToString("n", nfi)} €",
							LSBezug = kundenRechnungType != null && !kundenRechnungType.Typ.StringIsNullOrEmptyOrWhiteSpaces() && kundenRechnungType.Typ == Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung.GetDescription()
							? $"{rechnungReportEntity.OrderNumberPO} {rg?.Bezug}"
							: "",
							Factoring = customer.Factoring ?? false,
							Ursprungsland = _articleItem.Ursprungsland,
						});
					}
					var kundenType = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.GetByKundennummer(orders[i].Kunden_Nr ?? -1)?.Typ;
					invoiceData.Add(new RechnungReportingModel
					{
						Id = i,

						Header = orders[i].Typ,
						ItemsHeader = string.Empty,
						ItemsFooter1 = orders[i].Anrede,
						ItemsFooter2 = orders[i].Anrede,
						Footer11 = string.Empty,
						Footer12 = string.Empty,
						Footer13 = string.Empty,
						Footer14 = string.Empty,
						Footer15 = string.Empty,
						Footer16 = string.Empty,
						Footer17 = string.Empty,
						Footer21 = string.Empty,
						Footer22 = string.Empty,
						Footer23 = string.Empty,
						Footer24 = string.Empty,
						Footer25 = string.Empty,
						Footer26 = string.Empty,
						Footer27 = string.Empty,
						Footer31 = string.Empty,
						Footer32 = string.Empty,
						Footer33 = string.Empty,
						Footer34 = string.Empty,
						Footer35 = string.Empty,
						Footer36 = string.Empty,
						Footer37 = string.Empty,
						Footer41 = string.Empty,
						Footer42 = string.Empty,
						Footer43 = string.Empty,
						Footer44 = string.Empty,
						Footer45 = string.Empty,
						Footer46 = string.Empty,
						Footer47 = string.Empty,
						Footer51 = string.Empty,
						Footer52 = string.Empty,
						Footer53 = string.Empty,
						Footer54 = string.Empty,
						Footer55 = string.Empty,
						Footer56 = string.Empty,
						Footer57 = string.Empty,
						Footer61 = string.Empty,
						Footer62 = string.Empty,
						Footer63 = string.Empty,
						Footer64 = string.Empty,
						Footer65 = string.Empty,
						Footer66 = string.Empty,
						Footer67 = string.Empty,
						Footer71 = string.Empty,
						Footer72 = string.Empty,
						Footer73 = string.Empty,
						Footer74 = string.Empty,
						Footer75 = string.Empty,
						Footer76 = string.Empty,
						Footer77 = string.Empty,
						//
						DocumentType = getOrderTypeI18N(reportType, language?.Sprache),
						OrderNumberPO = orders[i].Bezug,
						OrderNumber = orders[i].Angebot_Nr?.ToString(),
						OrderDate = orders[i].Datum?.ToString("dd.MM.yyyy"),
						//
						ClientNumber = orders[i].Ihr_Zeichen,
						InternalNumber = orders[i].Unser_Zeichen.ToString(),
						ShippingMethod = orders[i].Versandart,
						PaymentMethod = orders[i].Zahlungsweise,
						PaymentTarget = orders[i].Konditionen,
						RechnungNummer = !kundenType.StringIsNullOrEmptyOrWhiteSpaces() && kundenType == Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung.GetDescription()
						? ""
						: $"{liefersheinItem?.Angebot_Nr.ToString()}",

						Address1 = orders[i].Anrede?.Trim(),
						Address2 = orders[i].Vorname_NameFirma?.Trim(),
						Address2_2 = hasRGAbweichendeAdr ? "" : orders[i].Name2?.Trim(),
						Address2_3 = hasRGAbweichendeAdr ? "" : orders[i].Name3?.Trim(),
						Address2_4 = orders[i].Ansprechpartner?.Trim(),
						Address2_5 = !string.IsNullOrWhiteSpace(customer.RG_Abteilung) ? "" : orders[i].Abteilung?.Trim(),
						Address2_6 = !string.IsNullOrWhiteSpace(customer.RG_Abteilung) ? customer?.RG_Abteilung?.Trim() : "Buchhaltung",
						Address3 = !string.IsNullOrWhiteSpace(customer.RG_Strasse_Postfach) ? customer?.RG_Strasse_Postfach?.Trim() :
									(orders[i].Land_PLZ_Ort.Contains(orders[i].Straße_Postfach?.Trim()) == true
									  ? buyerEntity?.Name2.Trim()
										: ((string.IsNullOrWhiteSpace(buyerEntity?.Name2) || orders[i].Straße_Postfach?.Contains(buyerEntity?.Name2?.Trim()) == true)
											? orders[i].Straße_Postfach?.Trim()
											: orders[i].Straße_Postfach?.Trim() + " " + buyerEntity?.Name2)?.Trim()),
						Address4 = !string.IsNullOrWhiteSpace(customer.RG_Land_PLZ_ORT) ? customer?.RG_Land_PLZ_ORT?.Trim() : orders[i].Land_PLZ_Ort?.Trim(),
						Address5 = "", // - 2023-05-22 - Heidenreich no need for Fax // hasRGAbweichendeAdr ? "" : address?.Fax?.Trim(),
						Address6 = hasRGAbweichendeAdr ? "" : (address.Land?.Trim()?.ToLower() == "d" ? "" : address.Land?.Trim()),
						UST_ID = orders[i]?.Freitext?.Trim(),
						SummarySumValue = sumSum,
						SummaryUSTValue = sumUST,
						SummaryTotalValue = sumSum + sumUST,
						Ust = ust == 0 ? "0%" : $"{Convert.ToInt32((ust * 100))}%",
						LAnrede = orders[i]?.LAnrede ?? "",
						LVorname = orders[i]?.LVorname_NameFirma ?? "",
						LName2 = orders[i]?.LName2 ?? "",
						LName3 = orders[i]?.LName3 ?? "",
						Labteilung = orders[i]?.LAbteilung ?? "",
						Lansprechpartner = orders[i]?.LAnsprechpartner ?? "",
						LStrabe = orders[i]?.LStraße_Postfach ?? "",
						LLandPLZOrt = orders[i]?.LLand_PLZ_Ort ?? "",
						FooterText1 = !customerConditions.Skonto.HasValue || customerConditions.Skonto.Value == 0 ? "" :
						getFooterText((sumSum + sumUST), orders[i]?.Datum, customerConditions, rechnungReportEntity),
						FooterText2 = customer.Factoring.HasValue && customer.Factoring.Value
						? $"{rechnungReportEntity.ItemsFooter1}\n{rechnungReportEntity.ItemsFooter2}"/*"Wir haben unsere sämtlichen gegenwärtigen und zukünftigen – auch die dieser Rechnung zugrundeliegenden\nForderungen – an die VR Factoring GmbH, Hauptstraße 131-137, 65760 Eschborn abgetreten.\nAuch unseren Eigentumsvorbehalt haben wir an die VR Factoring GmbH übertragen. Zahlungen mit\nschuldbefreiender Wirkung erbitten wir deshalb ausschließlich durch Überweisung an die VR Factroring GmbH:\nIBAN: DE59 5006 0419 7406 4440 05 SWIFT-/BIC-Code: GENODEF1VK9, Kreditinstitut: DZ BANK AG unter \nAngabe des"*/ : "",
						FooterText3 = customer.Factoring.HasValue && customer.Factoring.Value
						? $"{rechnungReportEntity?.FactoringText1} F-6444  {rechnungReportEntity?.FactoringText2} {orders[i].Unser_Zeichen}  {rechnungReportEntity?.FactoringText3} {orders[i].Angebot_Nr}  {rechnungReportEntity?.FactoringText4} {orders[i].Datum.Value.ToString("dd/MM/yyyy")}"
						: "",
						FooterText4 = !customer.Umsatzsteuer_berechnen.HasValue || !customer.Umsatzsteuer_berechnen.Value
						? rechnungReportEntity.Footer24
						: "",
					});

					#region convertHtml
					string header = HtmlPdfHelper.Template("Header");
					RechnungReportingModel oneInvoiceData = invoiceData.FirstOrDefault();
					string body = string.Empty;
					string Logo = string.Empty;
					string invoiceHeader = string.Empty;
					string invoiceNumber = string.Empty;
					if(oneInvoiceData is not null)
					{
						if(invoiceFields is not null && invoiceFields.Count > 0)
						{
							RechnungReportingModel invoiceField = invoiceFields.FirstOrDefault();
							if(invoiceField is not null)
							{
								var companyLogo = Module.FilesManager.GetFile(invoiceField.ImportLogoImageId);
								if(companyLogo != null && companyLogo.FileBytes is not null)
								{
									Logo = $"data:image/{companyLogo.FileExtension.Substring(1)};base64,{Convert.ToBase64String(companyLogo?.FileBytes)}";
								}
								if(string.IsNullOrEmpty(Logo))
								{
									// UNDONE : Fix logo
									Logo = "data:image/png;base64,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";
								}
								invoiceHeader = oneInvoiceData.Header;
								invoiceNumber = oneInvoiceData.OrderNumber;
								string invoiceDate = oneInvoiceData.OrderDate;
								string positionLines = string.Empty;
								foreach(var itemData in invoiceItemData.OrderBy(x => int.TryParse(x.PositionNumber, out var _x) ? _x : 0))
								{
									positionLines +=
													(!string.IsNullOrEmpty(itemData.Postext) ? "<tr><td colspan='10'><b>" + itemData.Postext + "</b></td></tr>" : string.Empty) +
													(!string.IsNullOrEmpty(itemData.LSBezug) ? "<tr><td colspan='10'><b>" + itemData.LSBezug + "</b></td></tr>" : string.Empty) +

													"<tr class='RowInvoice'>" +
														"<td>" + itemData.PositionNumber + "</td>" +
														"<td>" + itemData.ItemNumber + "</td>" +
														"<td>" + itemData.Description + "</td>" +
														"<td colspan='2'>" + itemData.Liefertermin + "</td>" +
														"<td>" + itemData.PE + "</td>" +
														$"<td align='right'>{itemData.BasePrice.FormatDecimal(2)} €</td>" +
														"<td align='right' class='nobr'>" + itemData.TotalCopper + "</td>" +
														$"<td align='right'>{itemData.SurchargeCopper.FormatDecimal(2)} €</td>" +
														$"<td align='right'>{itemData.UnitPrice.FormatDecimal(2)} €</td>" +
													"</tr>" +
													"<tr class='RowInvoice'>" +
														"<td colspan='3' class='firstItemPadding'>" + itemData.Designation + "</td>" +
														$"<td colspan=2>{itemData.Anzahl.FormatDecimal(0)} {itemData.Unit}</td>" +
														"<td>" + itemData.Index_Kunde + "</td>" +
														$"<td align='right'>{itemData.TotalPrice.FormatDecimal(2)} €</td>" +
														"<td align='right'>" + itemData.DEL + "</td>" +
														$"<td align='right'>{itemData.TotalSurchargeCopper.FormatDecimal(2)} €</td>" +
														$"<td align='right'>{itemData.TotalUnitPrice.FormatDecimal(2)} €</td>" +
													"</tr>" +
													(!string.IsNullOrEmpty(itemData.DelFixedText) ? "<tr class='RowInvoice'><td colspan='10' align='right'>" + itemData.DelFixedText + "</td></tr>" : string.Empty) +

													"<tr class='RowInvoice'><td colspan='10'><hr class='hr'></td></tr>";
								}

								body = HtmlPdfHelper.Template("Invoice", new List<PdfTag> {
						new PdfTag("<%invoice.header%>", rechnungReportEntity.DocumentType/*invoiceHeader*/),
						new PdfTag("<%invoice.number%>", invoiceNumber),
						new PdfTag("<%invoice.date%>", invoiceDate),
						new PdfTag("<%invoice.logo%>", Logo),
						new PdfTag("<%invoice.logoTop100%>", Module.CTS.Top100Logo),
						new PdfTag("<%invoice.Top100Description%>", Module.CTS.Top100Description),
						new PdfTag("<%invoice.Top100_2026Logo%>", Module.CTS.Top100_2026Logo),
						new PdfTag("<%invoice.pos%>", rechnungReportEntity.Position),
						new PdfTag("<%invoice.DesignationLabel%>", rechnungReportEntity.Designation),
						new PdfTag("<%invoice.Lieferadresse%>", rechnungReportEntity.Lieferadresse),

						new PdfTag("<%invoice.DeliveryNoteNumberLabel%>", string.IsNullOrWhiteSpace( oneInvoiceData.RechnungNummer) ? "": $"{rechnungReportEntity.ForDeliveryNote}"),
						new PdfTag("<%invoice.DeliveryNoteNumber%>", oneInvoiceData.RechnungNummer),
						new PdfTag("<%invoice.rechnungNummer%>", rechnungReportEntity.OrderNumber),
						new PdfTag("<%invoice.artikel%>", rechnungReportEntity.Article),
						new PdfTag("<%invoice.basispreis150%>", rechnungReportEntity.BasisPrice150),
						new PdfTag("<%invoice.designation%>", rechnungReportEntity.Designation),
						new PdfTag("<%invoice.liefertermin%>", rechnungReportEntity.Liefertermin),
						new PdfTag("<%invoice.Cu_G%>", rechnungReportEntity.Cu_G),
						new PdfTag("<%invoice.pe%>", rechnungReportEntity.PE),
						new PdfTag("<%invoice.Cu_Surcharge%>", rechnungReportEntity.Cu_Surcharge),
						new PdfTag("<%invoice.UnitPrice%>", rechnungReportEntity.UnitPrice),
						new PdfTag("<%invoice.Amount%>", rechnungReportEntity.Amount),
						new PdfTag("<%invoice.Unit%>", rechnungReportEntity.Unit),
						new PdfTag("<%invoice.TotalPrice150%>", rechnungReportEntity.TotalPrice150),
						new PdfTag("<%invoice.DEL%>", rechnungReportEntity.DEL),
						new PdfTag("<%invoice.Cu_Total%>", rechnungReportEntity.Cu_Total),
						new PdfTag("<%invoice.UnitTotal%>", rechnungReportEntity.UnitTotal),
						new PdfTag("<%invoice.Description%>", rechnungReportEntity.Description),
						new PdfTag("<%invoice.ClientNumberLabel%>", rechnungReportEntity.ClientNumber),
						new PdfTag("<%invoice.InternalNumberLabel%>", rechnungReportEntity.InternalNumber),
						new PdfTag("<%invoice.ShippingMethodLabel%>", rechnungReportEntity.ShippingMethod),
						new PdfTag("<%invoice.PaymentMethodLabel%>", rechnungReportEntity.PaymentMethod),
						new PdfTag("<%invoice.PaymentTargetLabel%>", rechnungReportEntity.PaymentTarget),
						new PdfTag("<%invoice.OrderDate%>", rechnungReportEntity.OrderDate),
						new PdfTag("<%invoice.ItemsHeader%>", rechnungReportEntity.ItemsHeader),
						new PdfTag("<%invoice.SummarySumLabel%>", rechnungReportEntity.SummarySum),
						new PdfTag("<%invoice.SummaryTotalLabel%>", rechnungReportEntity.SummaryTotal),
						new PdfTag("<%invoice.SummaryUSTLabel%>", rechnungReportEntity.SummaryUST),
						new PdfTag("<%invoice.TopHeader%>", rechnungReportEntity.Header),
						new PdfTag("<%invoice.Title%>", rechnungReportEntity.OrderNumberPO+" "+oneInvoiceData.OrderNumberPO),
						new PdfTag("<%invoice.HeaderAddress%>",
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address1)?"":$"<br>{oneInvoiceData.Address1}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2)?"":$"<br>{oneInvoiceData.Address2}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2_2)?"":$"<br>{oneInvoiceData.Address2_2}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2_3)?"":$"<br>{oneInvoiceData.Address2_3}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2_4)?"":$"<br>{oneInvoiceData.Address2_4}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2_5)?"":$"<br>{oneInvoiceData.Address2_5}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2_6)?"":$"<br>{oneInvoiceData.Address2_6}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address3)?"":$"<br>{oneInvoiceData.Address3}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address4)?"":$"<br>{oneInvoiceData.Address4}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address5)?"":$"<br>{oneInvoiceData.Address5}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address6)?"":$"<br>{oneInvoiceData.Address6}")}"
						),
						new PdfTag("<%invoice.LastPageText1%>", rechnungReportEntity.LastPageText1),
						new PdfTag("<%invoice.LastPageText2%>", rechnungReportEntity.LastPageText2),
						new PdfTag("<%invoice.LastPageText3%>", rechnungReportEntity.LastPageText3),
						new PdfTag("<%invoice.LastPageText4%>", rechnungReportEntity.LastPageText4),
						new PdfTag("<%invoice.LastPageText5%>",!string.IsNullOrEmpty(rechnungReportEntity.LastPageText5)?rechnungReportEntity.LastPageText5: "https://www.psz-electronic.com/assets/public/images/dsgvo/19-02-04 PSZ_Informationspflicht Datenschutz Art.13_Gesch%C3%A4ftspartnerBewerber.pdf"),
						new PdfTag("<%invoice.LastPageText6%>", rechnungReportEntity.LastPageText6),
						new PdfTag("<%invoice.LastPageText7%>", rechnungReportEntity.LastPageText7),
						new PdfTag("<%invoice.LastPageText8%>", rechnungReportEntity.LastPageText8),
						new PdfTag("<%invoice.LastPageText10%>", rechnungReportEntity.LastPageText10),
						new PdfTag("<%invoice.LastPageText11%>", rechnungReportEntity.LastPageText11),

						new PdfTag("<%invoice.positionLines%>", positionLines),
						new PdfTag("<%invoice.FooterText1%>", oneInvoiceData.FooterText1),
						new PdfTag("<%invoice.FooterText2%>", oneInvoiceData.FooterText2),
						new PdfTag("<%invoice.FooterText3%>", oneInvoiceData.FooterText3),
						new PdfTag("<%invoice.FooterText4%>", oneInvoiceData.FooterText4),
						new PdfTag("<%invoice.Footer11%>", oneInvoiceData.Footer11),
						new PdfTag("<%invoice.LAnrede%>", oneInvoiceData.LAnrede),
						new PdfTag("<%invoice.LVorname%>", oneInvoiceData.LVorname),
						new PdfTag("<%invoice.LName2%>", oneInvoiceData.LName2),
						new PdfTag("<%invoice.LStrabe%>", oneInvoiceData.LStrabe),
						new PdfTag("<%invoice.LLandPLZOrt%>", oneInvoiceData.LLandPLZOrt),
						new PdfTag("<%invoice.ClientNumber%>", oneInvoiceData.ClientNumber),
						new PdfTag("<%invoice.InternalNumber%>", oneInvoiceData.InternalNumber),
						new PdfTag("<%invoice.ShippingMethod%>", oneInvoiceData.ShippingMethod),
						new PdfTag("<%invoice.PaymentMethod%>", oneInvoiceData.PaymentMethod),
						new PdfTag("<%invoice.PaymentTarget%>", oneInvoiceData.PaymentTarget),
						// HT
						new PdfTag("<%invoice.SummarySum%>", $"{oneInvoiceData.SummarySumValue.FormatDecimal(2)} €"),
						// Montant TVA
						new PdfTag("<%invoice.SummaryUST%>", $"{oneInvoiceData.SummaryUSTValue.FormatDecimal(2)} €"),
						// Taux TVA
						new PdfTag("<%invoice.Ust%>", oneInvoiceData.Ust),
						// TTC
						new PdfTag("<%invoice.SummaryTotal%>", $"{oneInvoiceData.SummaryTotalValue.FormatDecimal(2)} €"),
						new PdfTag("<%invoice.UST_ID%>", oneInvoiceData.UST_ID),
					});
							}
							// Footer
							string footer = HtmlPdfHelper.Template("Footer", new List<PdfTag> {
						new PdfTag("<%invoice.Footer11%>", rechnungReportEntity.Footer11),
						new PdfTag("<%invoice.Footer12%>", rechnungReportEntity.Footer12),
						new PdfTag("<%invoice.Footer13%>", rechnungReportEntity.Footer13),
						new PdfTag("<%invoice.Footer14%>", rechnungReportEntity.Footer14),
						new PdfTag("<%invoice.Footer15%>", rechnungReportEntity.Footer15),
						new PdfTag("<%invoice.Footer16%>", rechnungReportEntity.Footer16),
						new PdfTag("<%invoice.Footer17%>", rechnungReportEntity.Footer17),

						new PdfTag("<%invoice.Footer21%>", rechnungReportEntity.Footer21),
						new PdfTag("<%invoice.Footer22%>", rechnungReportEntity.Footer22),
						new PdfTag("<%invoice.Footer23%>", rechnungReportEntity.Footer23),
						new PdfTag("<%invoice.Footer26%>", rechnungReportEntity.Footer26),
						new PdfTag("<%invoice.Footer27%>", rechnungReportEntity.Footer27),

						new PdfTag("<%invoice.Footer31%>", rechnungReportEntity.Footer31),
						new PdfTag("<%invoice.Footer32%>", rechnungReportEntity.Footer32),
						new PdfTag("<%invoice.Footer33%>", rechnungReportEntity.Footer33),
						new PdfTag("<%invoice.Footer34%>", rechnungReportEntity.Footer34),
						new PdfTag("<%invoice.Footer35%>", rechnungReportEntity.Footer35),
						new PdfTag("<%invoice.Footer36%>", rechnungReportEntity.Footer36),
						new PdfTag("<%invoice.Footer37%>", rechnungReportEntity.Footer37),

						new PdfTag("<%invoice.Footer41%>", rechnungReportEntity.Footer41),
						new PdfTag("<%invoice.Footer43%>", rechnungReportEntity.Footer43),
						new PdfTag("<%invoice.Footer44%>", rechnungReportEntity.Footer44),
						new PdfTag("<%invoice.Footer45%>", rechnungReportEntity.Footer45),
						new PdfTag("<%invoice.Footer47%>", rechnungReportEntity.Footer47),

						new PdfTag("<%invoice.Footer51%>", $"{invoiceField.Footer51} {(string.IsNullOrWhiteSpace(invoiceField.Footer61)?"":$"<br/>{invoiceField.Footer61}")} {(string.IsNullOrWhiteSpace(invoiceField.Footer71)?"":$"<br/>{invoiceField.Footer71}")} {(string.IsNullOrWhiteSpace(invoiceField?.Footer78)?"":$"<br/>{invoiceField?.Footer78}")}"),
						new PdfTag("<%invoice.Footer53%>", $"{invoiceField.Footer53} {(string.IsNullOrWhiteSpace(invoiceField.Footer63)?"":$"<br/>{invoiceField.Footer63}")} {(string.IsNullOrWhiteSpace(invoiceField.Footer73)?"":$"<br/>{invoiceField.Footer73}")} {(string.IsNullOrWhiteSpace(invoiceField?.Footer79)?"":$"<br/>{invoiceField?.Footer79}")}"),
						new PdfTag("<%invoice.Footer54%>", $"{invoiceField.Footer54} {(string.IsNullOrWhiteSpace(invoiceField.Footer64)?"":$"<br/>{invoiceField.Footer64}")} {(string.IsNullOrWhiteSpace(invoiceField.Footer74)?"":$"<br/>{invoiceField.Footer74}")} {(string.IsNullOrWhiteSpace(invoiceField?.Footer80)?"":$"<br/>{invoiceField?.Footer80}")}"),
						new PdfTag("<%invoice.Footer55%>", $"{invoiceField.Footer55} {(string.IsNullOrWhiteSpace(invoiceField.Footer65)?"":$"<br/>{invoiceField.Footer65}")} {(string.IsNullOrWhiteSpace(invoiceField.Footer75)?"":$"<br/>{invoiceField.Footer75}")} {(string.IsNullOrWhiteSpace(invoiceField?.Footer81)?"":$"<br/>{invoiceField?.Footer81}")}"),
						new PdfTag("<%invoice.Footer57%>", $"{invoiceField.Footer57} {(string.IsNullOrWhiteSpace(invoiceField.Footer67)?"":$"<br/>{invoiceField.Footer67}")} {(string.IsNullOrWhiteSpace(invoiceField.Footer77)?"":$"<br/>{invoiceField.Footer77}")} {(string.IsNullOrWhiteSpace(invoiceField?.Footer82)?"":$"<br/>{invoiceField?.Footer82}")}")
							});
							responseBody = HtmlPdfHelper.Convert(body, header, footer, " von " + invoiceHeader + " " + invoiceNumber + " ");
						}
					}
					#endregion
					//responseBody = Module.CS_ReportingService.GenerateRechnungReport(reportType, invoiceFields, invoiceData, invoiceItemData.OrderBy(x => int.TryParse(x.PositionNumber, out var p) ? p : 0).ToList());
					return ResponseModel<byte[]>.SuccessResponse(responseBody);
				}
				return ResponseModel<byte[]>.SuccessResponse();
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

		public ResponseModel<byte[]> Handle(Infrastructure.Services.Utils.TransactionsManager botransaction)
		{

			Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity rechnungReportEntity = null;

			try
			{
				#region // -- transaction-based logic -- //

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				byte[] responseBody = null;

				var order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(this._data.RechnungId, botransaction.connection, botransaction.transaction);
				if(order == null)
				{
					return new ResponseModel<byte[]>()
					{
						Success = false,
						Errors = new List<ResponseModel<byte[]>.ResponseError>
						{
							new ResponseModel<byte[]>.ResponseError{ Key="", Value = "Invoice not found"}
						}
					};
				}
				var languageId = order.Kunden_Nr.HasValue
				   ? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer((int)order.Kunden_Nr, botransaction.connection, botransaction.transaction)?.Sprache ?? this._data.LanguageId
				   : this._data.LanguageId;

				var language = new Infrastructure.Data.Entities.Tables.STG.SprachenEntity
				{
					ID = languageId,
					Sprache = "deutsch"
				};

				var languageEntities = Infrastructure.Data.Access.Tables.STG.SprachenAccess.GetWithTransaction(botransaction.connection, botransaction.transaction);
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
				rechnungReportEntity = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.GetByLanguageAndType(languageId, this._data.TypeId, botransaction.connection, botransaction.transaction);
				if(rechnungReportEntity == null)
				{
					rechnungReportEntity = new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity
					{
						Id = -1,
						CompanyLogoImageId = -1,
						ImportLogoImageId = -1,
						Header = string.Empty,
						ItemsHeader = string.Empty,
						ItemsFooter1 = string.Empty,
						ItemsFooter2 = string.Empty,
						Footer11 = string.Empty,
						Footer12 = string.Empty,
						Footer13 = string.Empty,
						Footer14 = string.Empty,
						Footer15 = string.Empty,
						Footer16 = string.Empty,
						Footer17 = string.Empty,
						Footer21 = string.Empty,
						Footer22 = string.Empty,
						Footer23 = string.Empty,
						Footer24 = string.Empty,
						Footer25 = string.Empty,
						Footer26 = string.Empty,
						Footer27 = string.Empty,

						Footer31 = string.Empty,
						Footer32 = string.Empty,
						Footer33 = string.Empty,
						Footer34 = string.Empty,
						Footer35 = string.Empty,
						Footer36 = string.Empty,
						Footer37 = string.Empty,

						Footer41 = string.Empty,
						Footer42 = string.Empty,
						Footer43 = string.Empty,
						Footer44 = string.Empty,
						Footer45 = string.Empty,
						Footer46 = string.Empty,
						Footer47 = string.Empty,

						Footer51 = string.Empty,
						Footer52 = string.Empty,
						Footer53 = string.Empty,
						Footer54 = string.Empty,
						Footer55 = string.Empty,
						Footer56 = string.Empty,
						Footer57 = string.Empty,

						Footer61 = string.Empty,
						Footer62 = string.Empty,
						Footer63 = string.Empty,
						Footer64 = string.Empty,
						Footer65 = string.Empty,
						Footer66 = string.Empty,
						Footer67 = string.Empty,

						Footer71 = string.Empty,
						Footer72 = string.Empty,
						Footer73 = string.Empty,
						Footer74 = string.Empty,
						Footer75 = string.Empty,
						Footer76 = string.Empty,
						Footer77 = string.Empty,


						// PSZ Address
						Address1 = string.Empty,
						Address2 = string.Empty,
						Address3 = string.Empty,
						Address4 = string.Empty,

						// Document
						OrderNumberPO = string.Empty,
						DocumentType = string.Empty,
						OrderNumber = string.Empty,
						OrderDate = string.Empty,

						// Client
						ClientNumber = string.Empty,
						InternalNumber = string.Empty,
						ShippingMethod = string.Empty,
						PaymentMethod = string.Empty,
						PaymentTarget = string.Empty,

						UST_ID = string.Empty,

						// Items
						Position = string.Empty,
						Article = string.Empty,
						Description = string.Empty,
						Amount = string.Empty,
						PE = string.Empty,
						BasisPrice150 = string.Empty,
						Cu_G = string.Empty,
						Cu_Surcharge = string.Empty,
						UnitPrice = string.Empty,
						Designation = string.Empty,
						Unit = string.Empty,
						TotalPrice150 = string.Empty,
						DEL = string.Empty,
						Cu_Total = string.Empty,
						UnitTotal = string.Empty,

						//
						Bestellt = string.Empty,
						Geliefert = string.Empty,
						Liefertermin = string.Empty,
						Offen = string.Empty,

						Abladestelle = string.Empty,

						LastPageText1 = string.Empty,
						LastPageText2 = string.Empty,
						LastPageText3 = string.Empty,
						LastPageText4 = string.Empty,
						LastPageText5 = string.Empty,
						LastPageText6 = string.Empty,
						LastPageText7 = string.Empty,
						LastPageText8 = string.Empty,
						LastPageText9 = string.Empty,

						SummarySum = string.Empty,
						SummaryTotal = string.Empty,
						SummaryUST = string.Empty,

						LanguageId = this._data.LanguageId,
						OrderTypeId = this._data.TypeId,

						LastUpdateTime = DateTime.Now,
						LastUpdateUserId = this._user.Id,

						Lieferadresse = string.Empty,
						Index_Kunde = string.Empty
					};
					rechnungReportEntity.Id = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.InsertWithTransaction(rechnungReportEntity, botransaction.connection, botransaction.transaction);
				}
				var rechnungReportModel = new CreateModel(rechnungReportEntity);
				var orders = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByAngebotNr(order.Angebot_Nr.ToString(), botransaction.connection, botransaction.transaction);

				var buyerEntity = Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.GetByOrderType(order.Nr, (int)Enums.ReportingEnums.OrderTypes.Order, botransaction.connection, botransaction.transaction);
				var customer = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(order.Kunden_Nr ?? -1, botransaction.connection, botransaction.transaction);
				var customerConditions = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customer.Konditionszuordnungs_Nr ?? -1);

				var orderItems = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
				var kundenRechnungType = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.GetByKundennummer(order.Kunden_Nr ?? -1, botransaction.connection, botransaction.transaction);
				orderItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyAngeboteNrs(orders?.Select(x => x.Nr).ToList(), botransaction.connection, botransaction.transaction)?
							.Where(y => y.erledigt_pos != true).ToList();



				// - 
				var liefersheinItems = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(orders.Select(x => x.Nr_lie ?? -1).ToList(), botransaction.connection, botransaction.transaction);
				var addresses = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orders.Select(x => x.Kunden_Nr ?? -1).ToList(), botransaction.connection, botransaction.transaction);
				var articleItems = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(orderItems.Select(x => x.ArtikelNr ?? -1).ToList(), botransaction.connection, botransaction.transaction);
				responseBody = getHTMLData(rechnungReportModel, customer, order, orders, orderItems, liefersheinItems, addresses, articleItems, rechnungReportEntity, kundenRechnungType, buyerEntity,
					language, customerConditions);
				#endregion // -- transaction-based logic -- //

				return ResponseModel<byte[]>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
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

		internal string getFooterText(decimal sum, DateTime? date, Infrastructure.Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity condition,
			Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity orderReportEntity)
		{
			NumberFormatInfo nfi = (NumberFormatInfo)
			CultureInfo.InvariantCulture.NumberFormat.Clone();
			nfi.NumberGroupSeparator = " ";
			sum = Math.Round(sum, 2);
			var percentage = Convert.ToDecimal(condition.Skonto * 100, CultureInfo.InvariantCulture);
			var percentValue = Math.Round(sum * Convert.ToDecimal(condition.Skonto), 2);
			var _date = date.Value.AddDays(condition.Skontotage ?? 0);
			var rest = Math.Round((sum - percentValue), 2);
			return ""; 
			//  - 2025-09-02 - ticket # 47024 Lang $"{orderReportEntity.DiscountText1} {Convert.ToInt32(percentage)}% {orderReportEntity.DiscountText2} {percentValue.FormatDecimal(2)} € {orderReportEntity.DiscountText3} {_date.ToString("dd/MM/yyyy")}{orderReportEntity.DiscountText4} {rest.FormatDecimal(2)} €";
		}
		internal List<RechnungReportingModel> setBigFooter(List<RechnungReportingModel> data, bool factoring)
		{
			var footerFactoring = Module.CTS.FooterFactoring;
			var footerNotFactoring = Module.CTS.FooterNotFactoring;
			if(factoring)
			{

				data.ForEach(x =>
				{
					x.Footer67 = "";
					x.Footer65 = "";
					x.Footer64 = "";
					x.Footer61 = "";
					x.Footer77 = "";
					x.Footer75 = "";
					x.Footer74 = "";
					x.Footer71 = "";
					x.Footer63 = "";
					x.Footer73 = "";
					x.Footer78 = "";
					x.Footer79 = "";
					x.Footer80 = "";
					x.Footer81 = "";
					x.Footer82 = "";
					//
					if(footerFactoring is not null)
					{
						x.Footer57 = footerFactoring?.SWOFT_BIC ?? "";
						x.Footer55 = footerFactoring?.IBAN ?? "";
						x.Footer54 = footerFactoring?.BLZ ?? "";
						x.Footer51 = footerFactoring?.Bank ?? "";
						x.Footer53 = footerFactoring?.Konto ?? "";
					}

				});
			}
			else
			{
				data.ForEach(x =>
				{
					x.Footer78 = footerNotFactoring?.Bank ?? "";
					x.Footer79 = footerNotFactoring?.Konto ?? "";
					x.Footer80 = footerNotFactoring?.BLZ ?? "";
					x.Footer81 = footerNotFactoring?.IBAN ?? "";
					x.Footer82 = footerNotFactoring?.SWOFT_BIC ?? "";
				});
			}
			return data;
		}
		internal byte[] getHTMLData(CreateModel rechnungReportModel, Infrastructure.Data.Entities.Tables.PRS.KundenEntity customer,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity order,
			List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> orders,
			List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> orderItems,
			List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> liefersheinItems,
			List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity> addresses,
			List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> articleItems,
			Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity rechnungReportEntity,
			Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity kundenRechnungType,
			Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity buyerEntity,
			Infrastructure.Data.Entities.Tables.STG.SprachenEntity language,
			Infrastructure.Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity customerConditions)
		{
			byte[] responseBody = null;
			List<RechnungReportingModel> invoiceFields = null;
			var invoiceData = new List<RechnungReportingModel>();
			var invoiceItemData = new List<RechnungReportingItemModel>();
			invoiceFields = new List<RechnungReportingModel> { rechnungReportModel.ToInvoiceFields() };

			var reportType = order.Typ == Enums.OrderEnums.Types.Invoice.GetDescription() ? Enums.ReportingEnums.ReportType.CTS_RECHNUNG :
				Enums.ReportingEnums.ReportType.ORDER_FORECAST;

			setBigFooter(invoiceFields, customer.Factoring ?? false);
			NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
			nfi.NumberGroupSeparator = " ";
			nfi.NumberDecimalSeparator = ",";
			var hasRGAbweichendeAdr = false;

			//for(int i = 0; i < orders.Count; i++)
			{
				var liefersheinItem = liefersheinItems.FirstOrDefault(x => x.Nr == order.Nr_lie);
				var address = addresses.FirstOrDefault(x => x.Nr == order.Kunden_Nr);
				var sumSum = 0m;
				var sumUST = 0m;
				var ust = 0m;
				hasRGAbweichendeAdr = !string.IsNullOrWhiteSpace(customer.RG_Abteilung) || !string.IsNullOrWhiteSpace(customer.RG_Strasse_Postfach) || !string.IsNullOrWhiteSpace(customer.RG_Land_PLZ_ORT);
				for(int j = 0; j < orderItems.Count; j++)
				{
					var rg = orders.FirstOrDefault(x => x.Nr == orderItems[j].AngebotNr);
					var ls = liefersheinItems.FirstOrDefault(x => x.Nr == rg?.Nr_lie);
					if(j == 0)
					{
						ust = orderItems[j].USt ?? 0;
					}
					else
					{
						if(orderItems[j].USt != ust)
							throw new Exception("problem Ust value not compatible");
					}
					sumSum += Convert.ToDecimal(orderItems[j].Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture);
					sumUST += Convert.ToDecimal(orderItems[j].Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture) * Convert.ToDecimal(orderItems[j].USt, System.Globalization.CultureInfo.InvariantCulture);
					var _articleItem = articleItems.FirstOrDefault(x => x.ArtikelNr == orderItems[j].ArtikelNr);
					invoiceItemData.Add(new RechnungReportingItemModel()
					{

						InvoiceId = 0,
						PositionNumber = orderItems[j].Position?.ToString(),
						ItemNumber = _articleItem?.ArtikelNummer.cleanArticleSuffix(),
						Description = orderItems[j].Bezeichnung1?.ToString(),
						Designation = orderItems[j].Bezeichnung2?.ToString(),
						Amount = orderItems[j].OriginalAnzahl?.ToString(),
						PE = orderItems[j].Preiseinheit?.ToString(),
						Unit = orderItems[j].Einheit?.ToString(),
						BasePrice = orderItems[j].VKEinzelpreis ?? 0m, // basispreis
						TotalPrice = orderItems[j].VKGesamtpreis ?? 0m, // Gesamppreis
						TotalCopper = $"{(orderItems[j].EinzelCuGewicht ?? 0m).FormatDecimal(3)}kg",
						DEL = orderItems[j].DEL?.ToString(),
						SurchargeCopper = orderItems[j].Einzelkupferzuschlag ?? 0m, // Cu-Zuschlag
						TotalSurchargeCopper = orderItems[j].Gesamtkupferzuschlag ?? 0m, // Cu-Zuschlag Gesamt
						UnitPrice = orderItems[j].Einzelpreis ?? 0m, // Einzelpreis
						TotalUnitPrice = orderItems[j].Gesamtpreis ?? 0m, // Einzelpreis Gesamt

						AB_Pos_zu_RA_Pos = $"{orderItems[j]?.ABPoszuRAPos}",
						Liefertermin = $"{orderItems[j]?.Liefertermin?.ToString("dd.MM.yyyy")}",
						Geliefert = $"{orderItems[j]?.Geliefert}",
						Anzahl = orderItems[j]?.Anzahl ?? 0m,
						Bestellt = $"{orderItems[j]?.Bestellnummer}",
						Offen = $"{orderItems[j]?.RA_Offen}",
						Abladestelle = !string.IsNullOrEmpty(orderItems[j]?.Abladestelle) && !string.IsNullOrWhiteSpace(orderItems[j]?.Abladestelle) ?
						$"{rechnungReportEntity?.Abladestelle} {orderItems[j]?.Abladestelle}" : "",
						Postext = kundenRechnungType != null && !kundenRechnungType.Typ.StringIsNullOrEmptyOrWhiteSpaces() && kundenRechnungType.Typ == Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung.GetDescription()
						? $"{orderItems[j].POSTEXT?.ToString()?.Trim()}<br/>{rechnungReportEntity?.ForPosDeliveryNote} {ls.Angebot_Nr}"
						: orderItems[j].POSTEXT?.ToString()?.Trim(),
						DELFixiert = orderItems[j].DELFixiert == true ? "1" : "0",
						Index_Kunde = orderItems[j].Index_Kunde?.Trim() ?? "",
						DelFixedText = orderItems[j].DELFixiert.HasValue && orderItems[j].DELFixiert.Value ? "DEL fixiert lauf Angebot" : null,
						ExternComment = orderItems[j].GSExternComment,
						TotalUnitSurcharge = $"{(orderItems[j].Zuschlag_VK ?? 0 * orderItems[j].Einzelpreis ?? 0).ToString("n", nfi)} €",
						TotalSurcharge = $"{(orderItems[j].Zuschlag_VK ?? 0 * orderItems[j].Gesamtpreis ?? 0).ToString("n", nfi)} €",
						LSBezug = kundenRechnungType != null && !kundenRechnungType.Typ.StringIsNullOrEmptyOrWhiteSpaces() && kundenRechnungType.Typ == Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung.GetDescription()
						? $"{rechnungReportEntity.OrderNumberPO} {rg?.Bezug}"
						: "",
						Factoring = customer.Factoring ?? false,
						Ursprungsland = _articleItem.Ursprungsland,
					});
				}
				invoiceData.Add(new RechnungReportingModel
				{
					Id = 0,

					Header = order.Typ,
					ItemsHeader = string.Empty,
					ItemsFooter1 = order.Anrede,
					ItemsFooter2 = order.Anrede,

					Footer11 = string.Empty,
					Footer12 = string.Empty,
					Footer13 = string.Empty,
					Footer14 = string.Empty,
					Footer15 = string.Empty,
					Footer16 = string.Empty,
					Footer17 = string.Empty,

					Footer21 = string.Empty,
					Footer22 = string.Empty,
					Footer23 = string.Empty,
					Footer24 = string.Empty,
					Footer25 = string.Empty,
					Footer26 = string.Empty,
					Footer27 = string.Empty,

					Footer31 = string.Empty,
					Footer32 = string.Empty,
					Footer33 = string.Empty,
					Footer34 = string.Empty,
					Footer35 = string.Empty,
					Footer36 = string.Empty,
					Footer37 = string.Empty,

					Footer41 = string.Empty,
					Footer42 = string.Empty,
					Footer43 = string.Empty,
					Footer44 = string.Empty,
					Footer45 = string.Empty,
					Footer46 = string.Empty,
					Footer47 = string.Empty,

					Footer51 = string.Empty,
					Footer52 = string.Empty,
					Footer53 = string.Empty,
					Footer54 = string.Empty,
					Footer55 = string.Empty,
					Footer56 = string.Empty,
					Footer57 = string.Empty,

					Footer61 = string.Empty,
					Footer62 = string.Empty,
					Footer63 = string.Empty,
					Footer64 = string.Empty,
					Footer65 = string.Empty,
					Footer66 = string.Empty,
					Footer67 = string.Empty,

					Footer71 = string.Empty,
					Footer72 = string.Empty,
					Footer73 = string.Empty,
					Footer74 = string.Empty,
					Footer75 = string.Empty,
					Footer76 = string.Empty,
					Footer77 = string.Empty,

					//
					DocumentType = getOrderTypeI18N(reportType, language?.Sprache),
					OrderNumberPO = order.Bezug,
					OrderNumber = order.Angebot_Nr?.ToString(),
					OrderDate = order.Datum?.ToString("dd.MM.yyyy"),

					//
					ClientNumber = order.Ihr_Zeichen,
					InternalNumber = order.Unser_Zeichen.ToString(),
					ShippingMethod = order.Versandart,
					PaymentMethod = order.Zahlungsweise,
					PaymentTarget = order.Konditionen,
					RechnungNummer = !(kundenRechnungType?.Typ ?? "").StringIsNullOrEmptyOrWhiteSpaces() && (kundenRechnungType?.Typ ?? "") == Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung.GetDescription()
					? ""
					: $"{liefersheinItem?.Angebot_Nr.ToString()}",

					Address1 = order.Anrede?.Trim(),
					Address2 = order.Vorname_NameFirma?.Trim(),
					Address2_2 = hasRGAbweichendeAdr ? "" : order.Name2?.Trim(),
					Address2_3 = hasRGAbweichendeAdr ? "" : order.Name3?.Trim(),
					Address2_4 = order.Ansprechpartner?.Trim(),
					Address2_5 = !string.IsNullOrWhiteSpace(customer.RG_Abteilung) ? "" : order.Abteilung?.Trim(),
					Address2_6 = !string.IsNullOrWhiteSpace(customer.RG_Abteilung) ? customer?.RG_Abteilung?.Trim() : "Buchhaltung",
					Address3 = !string.IsNullOrWhiteSpace(customer.RG_Strasse_Postfach) ? customer?.RG_Strasse_Postfach?.Trim() :
								(order.Land_PLZ_Ort.Contains(order.Straße_Postfach?.Trim()) == true
								  ? buyerEntity?.Name2.Trim()
									: ((string.IsNullOrWhiteSpace(buyerEntity?.Name2) || order.Straße_Postfach?.Contains(buyerEntity?.Name2?.Trim()) == true)
										? order.Straße_Postfach?.Trim()
										: order.Straße_Postfach?.Trim() + " " + buyerEntity?.Name2)?.Trim()),
					Address4 = !string.IsNullOrWhiteSpace(customer.RG_Land_PLZ_ORT) ? customer?.RG_Land_PLZ_ORT?.Trim() : order.Land_PLZ_Ort?.Trim(),
					Address5 = "", // - 2023-05-22 - Heidenreich no need for Fax // hasRGAbweichendeAdr ? "" : address?.Fax?.Trim(),
					Address6 = hasRGAbweichendeAdr ? "" : (address.Land?.Trim()?.ToLower() == "d" ? "" : address.Land?.Trim()),


					UST_ID = order?.Freitext?.Trim(),

					SummarySumValue = sumSum,
					SummaryUSTValue = sumUST,
					SummaryTotalValue = sumSum + sumUST,
					Ust = ust == 0 ? "0%" : $"{Convert.ToInt32((ust * 100))}%",

					LAnrede = order?.LAnrede ?? "",
					LVorname = order?.LVorname_NameFirma ?? "",
					LName2 = order?.LName2 ?? "",
					LName3 = order?.LName3 ?? "",
					Labteilung = order?.LAbteilung ?? "",
					Lansprechpartner = order?.LAnsprechpartner ?? "",
					LStrabe = order?.LStraße_Postfach ?? "",
					LLandPLZOrt = order?.LLand_PLZ_Ort ?? "",
					FooterText1 = !customerConditions.Skonto.HasValue || customerConditions.Skonto.Value == 0 ? "" :
					getFooterText((sumSum + sumUST), order?.Datum, customerConditions, rechnungReportEntity),
					FooterText2 = customer.Factoring.HasValue && customer.Factoring.Value
					? $"{rechnungReportEntity.ItemsFooter1}\n{rechnungReportEntity.ItemsFooter2}" : "",
					FooterText3 = customer.Factoring.HasValue && customer.Factoring.Value
					? $"{rechnungReportEntity?.FactoringText1} F-6444  {rechnungReportEntity?.FactoringText2} {order.Unser_Zeichen}  {rechnungReportEntity?.FactoringText3} {order.Angebot_Nr}  {rechnungReportEntity?.FactoringText4} {order.Datum.Value.ToString("dd/MM/yyyy")}"
					: "",
					FooterText4 = !customer.Umsatzsteuer_berechnen.HasValue || !customer.Umsatzsteuer_berechnen.Value
					? rechnungReportEntity.Footer24
					: "",

				});

				#region convertHtml
				string header = HtmlPdfHelper.Template("Header");
				RechnungReportingModel oneInvoiceData = invoiceData.FirstOrDefault();
				string body = string.Empty;
				string Logo = string.Empty;
				string invoiceHeader = string.Empty;
				string invoiceNumber = string.Empty;

				if(oneInvoiceData is not null)
				{
					if(invoiceFields is not null && invoiceFields.Count > 0)
					{
						RechnungReportingModel invoiceField = invoiceFields.FirstOrDefault();
						if(invoiceField is not null)
						{
							var companyLogo = Module.FilesManager.GetFile(invoiceField.CompanyLogoImageId);
							if(companyLogo != null && companyLogo.FileBytes is not null)
							{
								Logo = Convert.ToBase64String(companyLogo?.FileBytes);
							}
							if(string.IsNullOrEmpty(Logo))
							{
								// UNDONE : Fix logo
								Logo = "data:image/png;base64,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";
							}
							invoiceHeader = oneInvoiceData.Header;
							invoiceNumber = oneInvoiceData.OrderNumber;
							string invoiceDate = oneInvoiceData.OrderDate;
							string positionLines = string.Empty;
							foreach(var itemData in invoiceItemData.OrderBy(x => int.TryParse(x.PositionNumber, out var _x) ? _x : 0))
							{
								positionLines +=
												(!string.IsNullOrEmpty(itemData.Postext) ? "<tr><td colspan='10'><b>" + itemData.Postext + "</b></td></tr>" : string.Empty) +
												(!string.IsNullOrEmpty(itemData.LSBezug) ? "<tr><td colspan='10'><b>" + itemData.LSBezug + "</b></td></tr>" : string.Empty) +

												"<tr class='RowInvoice'>" +
													"<td>" + itemData.PositionNumber + "</td>" +
													"<td>" + itemData.ItemNumber + "</td>" +
													"<td>" + itemData.Description + "</td>" +
													"<td colspan='2'>" + itemData.Liefertermin + "</td>" +
													"<td>" + itemData.PE + "</td>" +
													$"<td align='right'>{itemData.BasePrice.FormatDecimal(2)} €</td>" +
													"<td align='right' class='nobr'>" + itemData.TotalCopper + "</td>" +
													$"<td align='right'>{itemData.SurchargeCopper.FormatDecimal(2)} €</td>" +
													$"<td align='right'>{itemData.UnitPrice.FormatDecimal(2)} €</td>" +
												"</tr>" +
												"<tr class='RowInvoice'>" +
													"<td colspan='3' class='firstItemPadding'>" + itemData.Designation + "</td>" +
													$"<td colspan=2>{itemData.Anzahl.FormatDecimal(0)} {itemData.Unit}</td>" +
													"<td>" + itemData.Index_Kunde + "</td>" +
													$"<td align='right'>{itemData.TotalPrice.FormatDecimal(2)} €</td>" +
													"<td align='right'>" + itemData.DEL + "</td>" +
													$"<td align='right'>{itemData.TotalSurchargeCopper.FormatDecimal(2)} €</td>" +
													$"<td align='right'>{itemData.TotalUnitPrice.FormatDecimal(2)} €</td>" +
												"</tr>" +
												(!string.IsNullOrEmpty(itemData.DelFixedText) ? "<tr class='RowInvoice'><td colspan='10' align='right'>" + itemData.DelFixedText + "</td></tr>" : string.Empty) +

												"<tr class='RowInvoice'><td colspan='10'><hr class='hr'></td></tr>";
							}

							body = HtmlPdfHelper.Template("Invoice", new List<PdfTag> {
						new PdfTag("<%invoice.header%>", rechnungReportEntity.DocumentType/*invoiceHeader*/),
						new PdfTag("<%invoice.number%>", invoiceNumber),
						new PdfTag("<%invoice.date%>", invoiceDate),
						new PdfTag("<%invoice.logo%>", Logo),
						new PdfTag("<%invoice.logoTop100%>", "data:image/png;base64,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"),


						new PdfTag("<%invoice.pos%>", rechnungReportEntity.Position),
						new PdfTag("<%invoice.DesignationLabel%>", rechnungReportEntity.Designation),
						new PdfTag("<%invoice.Lieferadresse%>", rechnungReportEntity.Lieferadresse),

						new PdfTag("<%invoice.DeliveryNoteNumberLabel%>", string.IsNullOrWhiteSpace( oneInvoiceData.RechnungNummer) ? "": $"{rechnungReportEntity.ForDeliveryNote} "),
						new PdfTag("<%invoice.DeliveryNoteNumber%>", oneInvoiceData.RechnungNummer),
						new PdfTag("<%invoice.rechnungNummer%>", rechnungReportEntity.OrderNumber),
						new PdfTag("<%invoice.artikel%>", rechnungReportEntity.Article),
						new PdfTag("<%invoice.basispreis150%>", rechnungReportEntity.BasisPrice150),
						new PdfTag("<%invoice.designation%>", rechnungReportEntity.Designation),
						new PdfTag("<%invoice.liefertermin%>", rechnungReportEntity.Liefertermin),
						new PdfTag("<%invoice.Cu_G%>", rechnungReportEntity.Cu_G),
						new PdfTag("<%invoice.pe%>", rechnungReportEntity.PE),
						new PdfTag("<%invoice.Cu_Surcharge%>", rechnungReportEntity.Cu_Surcharge),
						new PdfTag("<%invoice.UnitPrice%>", rechnungReportEntity.UnitPrice),
						new PdfTag("<%invoice.Amount%>", rechnungReportEntity.Amount),
						new PdfTag("<%invoice.Unit%>", rechnungReportEntity.Unit),
						new PdfTag("<%invoice.TotalPrice150%>", rechnungReportEntity.TotalPrice150),
						new PdfTag("<%invoice.DEL%>", rechnungReportEntity.DEL),
						new PdfTag("<%invoice.Cu_Total%>", rechnungReportEntity.Cu_Total),
						new PdfTag("<%invoice.UnitTotal%>", rechnungReportEntity.UnitTotal),
						new PdfTag("<%invoice.Description%>", rechnungReportEntity.Description),
						new PdfTag("<%invoice.ClientNumberLabel%>", rechnungReportEntity.ClientNumber),
						new PdfTag("<%invoice.InternalNumberLabel%>", rechnungReportEntity.InternalNumber),
						new PdfTag("<%invoice.ShippingMethodLabel%>", rechnungReportEntity.ShippingMethod),
						new PdfTag("<%invoice.PaymentMethodLabel%>", rechnungReportEntity.PaymentMethod),
						new PdfTag("<%invoice.PaymentTargetLabel%>", rechnungReportEntity.PaymentTarget),
						new PdfTag("<%invoice.OrderDate%>", rechnungReportEntity.OrderDate),
						new PdfTag("<%invoice.ItemsHeader%>", rechnungReportEntity.ItemsHeader),
						new PdfTag("<%invoice.SummarySumLabel%>", rechnungReportEntity.SummarySum),
						new PdfTag("<%invoice.SummaryTotalLabel%>", rechnungReportEntity.SummaryTotal),
						new PdfTag("<%invoice.SummaryUSTLabel%>", rechnungReportEntity.SummaryUST),
						new PdfTag("<%invoice.TopHeader%>", rechnungReportEntity.Header),
						new PdfTag("<%invoice.Title%>", rechnungReportEntity.OrderNumberPO+" "+oneInvoiceData.OrderNumberPO),
						new PdfTag("<%invoice.HeaderAddress%>",
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address1)?"":$"<br>{oneInvoiceData.Address1}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2)?"":$"<br>{oneInvoiceData.Address2}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2_2)?"":$"<br>{oneInvoiceData.Address2_2}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2_3)?"":$"<br>{oneInvoiceData.Address2_3}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2_4)?"":$"<br>{oneInvoiceData.Address2_4}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2_5)?"":$"<br>{oneInvoiceData.Address2_5}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address2_6)?"":$"<br>{oneInvoiceData.Address2_6}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address3)?"":$"<br>{oneInvoiceData.Address3}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address4)?"":$"<br>{oneInvoiceData.Address4}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address5)?"":$"<br>{oneInvoiceData.Address5}")}"+
						$"{(string.IsNullOrWhiteSpace(oneInvoiceData.Address6)?"":$"<br>{oneInvoiceData.Address6}")}"
						),
						new PdfTag("<%invoice.LastPageText1%>", rechnungReportEntity.LastPageText1),
						new PdfTag("<%invoice.LastPageText2%>", rechnungReportEntity.LastPageText2),
						new PdfTag("<%invoice.LastPageText3%>", rechnungReportEntity.LastPageText3),
						new PdfTag("<%invoice.LastPageText4%>", rechnungReportEntity.LastPageText4),
						new PdfTag("<%invoice.LastPageText5%>",!string.IsNullOrEmpty(rechnungReportEntity.LastPageText5)?rechnungReportEntity.LastPageText5: "https://www.psz-electronic.com/assets/public/images/dsgvo/19-02-04 PSZ_Informationspflicht Datenschutz Art.13_Gesch%C3%A4ftspartnerBewerber.pdf"),
						new PdfTag("<%invoice.LastPageText6%>", rechnungReportEntity.LastPageText6),
						new PdfTag("<%invoice.LastPageText7%>", rechnungReportEntity.LastPageText7),
						new PdfTag("<%invoice.LastPageText8%>", rechnungReportEntity.LastPageText8),
						new PdfTag("<%invoice.LastPageText10%>", rechnungReportEntity.LastPageText10),
						new PdfTag("<%invoice.LastPageText11%>", rechnungReportEntity.LastPageText11),

						new PdfTag("<%invoice.positionLines%>", positionLines),

						new PdfTag("<%invoice.FooterText1%>", oneInvoiceData.FooterText1),
						new PdfTag("<%invoice.FooterText2%>", oneInvoiceData.FooterText2),
						new PdfTag("<%invoice.FooterText3%>", oneInvoiceData.FooterText3),
						new PdfTag("<%invoice.FooterText4%>", oneInvoiceData.FooterText4),
						new PdfTag("<%invoice.Footer11%>", oneInvoiceData.Footer11),
						new PdfTag("<%invoice.LAnrede%>", oneInvoiceData.LAnrede),
						new PdfTag("<%invoice.LVorname%>", oneInvoiceData.LVorname),
						new PdfTag("<%invoice.LName2%>", oneInvoiceData.LName2),
						new PdfTag("<%invoice.LStrabe%>", oneInvoiceData.LStrabe),
						new PdfTag("<%invoice.LLandPLZOrt%>", oneInvoiceData.LLandPLZOrt),
						new PdfTag("<%invoice.ClientNumber%>", oneInvoiceData.ClientNumber),
						new PdfTag("<%invoice.InternalNumber%>", oneInvoiceData.InternalNumber),
						new PdfTag("<%invoice.ShippingMethod%>", oneInvoiceData.ShippingMethod),
						new PdfTag("<%invoice.PaymentMethod%>", oneInvoiceData.PaymentMethod),
						new PdfTag("<%invoice.PaymentTarget%>", oneInvoiceData.PaymentTarget),

						// HT
						new PdfTag("<%invoice.SummarySum%>", $"{oneInvoiceData.SummarySumValue.FormatDecimal(2)} €"),
						// Montant TVA
						new PdfTag("<%invoice.SummaryUST%>", $"{oneInvoiceData.SummaryUSTValue.FormatDecimal(2)} €"),

						// Taux TVA
						new PdfTag("<%invoice.Ust%>", oneInvoiceData.Ust),
						// TTC
						new PdfTag("<%invoice.SummaryTotal%>", $"{oneInvoiceData.SummaryTotalValue.FormatDecimal(2)} €"),
						new PdfTag("<%invoice.UST_ID%>", oneInvoiceData.UST_ID),



					});

						}
						// Footer

						string footer = HtmlPdfHelper.Template("Footer", new List<PdfTag> {
						new PdfTag("<%invoice.Footer11%>", rechnungReportEntity.Footer11),
						new PdfTag("<%invoice.Footer12%>", rechnungReportEntity.Footer12),
						new PdfTag("<%invoice.Footer13%>", rechnungReportEntity.Footer13),
						new PdfTag("<%invoice.Footer14%>", rechnungReportEntity.Footer14),
						new PdfTag("<%invoice.Footer15%>", rechnungReportEntity.Footer15),
						new PdfTag("<%invoice.Footer16%>", rechnungReportEntity.Footer16),
						new PdfTag("<%invoice.Footer17%>", rechnungReportEntity.Footer17),

						new PdfTag("<%invoice.Footer21%>", rechnungReportEntity.Footer21),
						new PdfTag("<%invoice.Footer22%>", rechnungReportEntity.Footer22),
						new PdfTag("<%invoice.Footer23%>", rechnungReportEntity.Footer23),
						new PdfTag("<%invoice.Footer26%>", rechnungReportEntity.Footer26),
						new PdfTag("<%invoice.Footer27%>", rechnungReportEntity.Footer27),

						new PdfTag("<%invoice.Footer31%>", rechnungReportEntity.Footer31),
						new PdfTag("<%invoice.Footer32%>", rechnungReportEntity.Footer32),
						new PdfTag("<%invoice.Footer33%>", rechnungReportEntity.Footer33),
						new PdfTag("<%invoice.Footer34%>", rechnungReportEntity.Footer34),
						new PdfTag("<%invoice.Footer35%>", rechnungReportEntity.Footer35),
						new PdfTag("<%invoice.Footer36%>", rechnungReportEntity.Footer36),
						new PdfTag("<%invoice.Footer37%>", rechnungReportEntity.Footer37),

						new PdfTag("<%invoice.Footer41%>", rechnungReportEntity.Footer41),
						new PdfTag("<%invoice.Footer43%>", rechnungReportEntity.Footer43),
						new PdfTag("<%invoice.Footer44%>", rechnungReportEntity.Footer44),
						new PdfTag("<%invoice.Footer45%>", rechnungReportEntity.Footer45),
						new PdfTag("<%invoice.Footer47%>", rechnungReportEntity.Footer47),

						new PdfTag("<%invoice.Footer51%>", $"{invoiceField.Footer51} {(string.IsNullOrWhiteSpace(invoiceField.Footer61)?"":$"<br/>{invoiceField.Footer61}")} {(string.IsNullOrWhiteSpace(invoiceField.Footer71)?"":$"<br/>{invoiceField.Footer71}")} {(string.IsNullOrWhiteSpace(invoiceField?.Footer78)?"":$"<br/>{invoiceField?.Footer78}")}"),
						new PdfTag("<%invoice.Footer53%>", $"{invoiceField.Footer53} {(string.IsNullOrWhiteSpace(invoiceField.Footer63)?"":$"<br/>{invoiceField.Footer63}")} {(string.IsNullOrWhiteSpace(invoiceField.Footer73)?"":$"<br/>{invoiceField.Footer73}")} {(string.IsNullOrWhiteSpace(invoiceField?.Footer79)?"":$"<br/>{invoiceField?.Footer79}")}"),
						new PdfTag("<%invoice.Footer54%>", $"{invoiceField.Footer54} {(string.IsNullOrWhiteSpace(invoiceField.Footer64)?"":$"<br/>{invoiceField.Footer64}")} {(string.IsNullOrWhiteSpace(invoiceField.Footer74)?"":$"<br/>{invoiceField.Footer74}")} {(string.IsNullOrWhiteSpace(invoiceField?.Footer80)?"":$"<br/>{invoiceField ?.Footer80}")}"),
						new PdfTag("<%invoice.Footer55%>", $"{invoiceField.Footer55} {(string.IsNullOrWhiteSpace(invoiceField.Footer65)?"":$"<br/>{invoiceField.Footer65}")} {(string.IsNullOrWhiteSpace(invoiceField.Footer75)?"":$"<br/>{invoiceField.Footer75}")} {(string.IsNullOrWhiteSpace(invoiceField?.Footer81)?"":$"<br/>{invoiceField?.Footer81}")}"),
						new PdfTag("<%invoice.Footer57%>", $"{invoiceField.Footer57} {(string.IsNullOrWhiteSpace(invoiceField.Footer67)?"":$"<br/>{invoiceField.Footer67}")} {(string.IsNullOrWhiteSpace(invoiceField.Footer77)?"":$"<br/>{invoiceField.Footer77}")} {(string.IsNullOrWhiteSpace(invoiceField?.Footer82)?"":$"<br/>{invoiceField?.Footer82}")}")
							});
						responseBody = HtmlPdfHelper.Convert(body, header, footer, " von " + invoiceHeader + " " + invoiceNumber + " ");
					}
				}
				#endregion
				// -
			}

			// - 
			return responseBody;
		}
	}
}
