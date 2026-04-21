using Psz.Core.Common;
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
				var reportModel = new Infrastructure.Services.Reporting.Models.Itext.CTS_GSReportModel(reportEntity, orderEntity, buyerEntity, addressEntity, itemsEntity?[0].USt ?? 0m);
				;
				var itemsReportModel = itemsEntity?.Select(x => new Infrastructure.Services.Reporting.Models.Itext.CTS_GSReportItemsModel(x)).ToList();
				reportModel.Items = itemsReportModel;
				reportModel.DocumentType = getOrderTypeI18N(reportType, language?.Sprache);

				reportModel.Logo = Module.CTS.Logo;
				reportModel.Top100Logo = Module.CTS.Top100Logo;
				reportModel.Top100Description = Module.CTS.Top100Description;
				reportModel.Top100_2026Logo = Module.CTS.Top100_2026Logo;
				//sums
				var summarySum = itemsEntity?.Sum(x => Convert.ToDecimal(x.Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture));
				var summaryUST = itemsEntity?.Sum(x => Convert.ToDecimal(x.Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture) * Convert.ToDecimal(x.USt, System.Globalization.CultureInfo.InvariantCulture));
				reportModel.SummarySumValue = $"{summarySum.Value.ToString("0.00")} €";
				reportModel.SummaryUSTValue = $"{summaryUST.Value.ToString("0.00")} €";
				reportModel.SummaryTotalValue = $"{(summarySum + summaryUST).Value.ToString("0.00")} €";
				var footerData = new Infrastructure.Services.Reporting.Models.Itext.DocFooterModel(reportEntity);
				//{
				//	footerAddress1 = "Im Gstaudach 6",
				//	footerAddress2 = "92648 Vohenstrauß",
				//	footerAddress3 = "Tel.: +49 9651 924 117-0",

				//	footerBankLabel = "Bankverbindung:",
				//	footerBankValue1 = "Commerzbank AG Filiale Weiden",
				//	footerBankValue2 = "Raiffeisenbank im Naabtal eG",
				//	footerBankValue3 = "HypoVereinsbank Weiden",
				//	footerBankValue4 = "",

				//	footerLabelUst = "Ust.-Id-Nr.:",
				//	footerValueUst = "DE 813706578",
				//	footerLabelSite = "Sitz:",
				//	footerValueSite = "Vohenstrauß",
				//	footerLabelFax = "Fax:",
				//	footerValueFax = "+49 9651 924 117-212",


				//	footerLabelManager = "Geschäftsführer:",
				//	footerValueManager = "Werner Steinbacher",
				//	footerLabelManager2 = "",
				//	footerValueManager2 = "",
				//	footerLabelTaxId = "Steuernummer:",
				//	footerValueTaxId = "255/135/40526",

				//	footerLabelHRB = "HRB:",
				//	footerValueHRB = "2907 AG Weiden",
				//	footerLabelEmail = "E-mail:",
				//	footerValueEmail = "info@psz-electronic.com",
				//	footerLabelCustomsId = "Zollnummer:",
				//	footerValueCustomsId = "488 26 28",
				//	//Konto
				//	footerAccountLabel = "Konto:",
				//	footerAccountValue1 = "775 321 300",
				//	footerAccountValue2 = "3 22 66 03",
				//	footerAccountValue3 = "234 354 89",
				//	footerAccountValue4 = "",
				//	//BLZ
				//	footerBLZLabel = "BLZ:",
				//	footerBLZValue1 = "753 400 90",
				//	footerBLZValue2 = "750691 71",
				//	footerBLZValue3 = "753 200 75",
				//	footerBLZValue4 = "",
				//	//IBAN
				//	footerIBANLabel = "IBAN:",
				//	footerIBANValue1 = "DE41 7534 0090 0775 3213 00",
				//	footerIBANValue2 = "DE04 7506 9171 0003 2266 03",
				//	footerIBANValue3 = "DE56 7532 0075 0023 4354 89",
				//	footerIBANValue4 = "",
				//	//SWIFT
				//	footerSWIFTLabel = "SWIFT-BIC:",
				//	footerSWIFTValue1 = "COBADEFF753",
				//	footerSWIFTValue2 = "GENODEF1SWD",
				//	footerSWIFTValue3 = "HYVEDEMM454",
				//	footerSWIFTValue4 = "",
				//};
				responseBody = await Infrastructure.Services.Reporting.IText.CTS.GetGS(reportModel, footerData);
				return ResponseModel<byte[]>.SuccessResponse(responseBody);
				// get order template data
				//var gutshriftReportEntity = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.GetByLanguageAndType(language.ID, this._data.TypeId);
				//if(gutshriftReportEntity == null)
				//{
				//	gutshriftReportEntity = new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity
				//	{
				//		Id = -1,
				//		CompanyLogoImageId = -1,
				//		ImportLogoImageId = -1,
				//		Header = string.Empty,
				//		ItemsHeader = string.Empty,
				//		ItemsFooter1 = string.Empty,
				//		ItemsFooter2 = string.Empty,
				//		Footer11 = string.Empty,
				//		Footer12 = string.Empty,
				//		Footer13 = string.Empty,
				//		Footer14 = string.Empty,
				//		Footer15 = string.Empty,
				//		Footer16 = string.Empty,
				//		Footer17 = string.Empty,
				//		Footer21 = string.Empty,
				//		Footer22 = string.Empty,
				//		Footer23 = string.Empty,
				//		Footer24 = string.Empty,
				//		Footer25 = string.Empty,
				//		Footer26 = string.Empty,
				//		Footer27 = string.Empty,

				//		Footer31 = string.Empty,
				//		Footer32 = string.Empty,
				//		Footer33 = string.Empty,
				//		Footer34 = string.Empty,
				//		Footer35 = string.Empty,
				//		Footer36 = string.Empty,
				//		Footer37 = string.Empty,

				//		Footer41 = string.Empty,
				//		Footer42 = string.Empty,
				//		Footer43 = string.Empty,
				//		Footer44 = string.Empty,
				//		Footer45 = string.Empty,
				//		Footer46 = string.Empty,
				//		Footer47 = string.Empty,

				//		Footer51 = string.Empty,
				//		Footer52 = string.Empty,
				//		Footer53 = string.Empty,
				//		Footer54 = string.Empty,
				//		Footer55 = string.Empty,
				//		Footer56 = string.Empty,
				//		Footer57 = string.Empty,

				//		Footer61 = string.Empty,
				//		Footer62 = string.Empty,
				//		Footer63 = string.Empty,
				//		Footer64 = string.Empty,
				//		Footer65 = string.Empty,
				//		Footer66 = string.Empty,
				//		Footer67 = string.Empty,

				//		Footer71 = string.Empty,
				//		Footer72 = string.Empty,
				//		Footer73 = string.Empty,
				//		Footer74 = string.Empty,
				//		Footer75 = string.Empty,
				//		Footer76 = string.Empty,
				//		Footer77 = string.Empty,


				//		// PSZ Address
				//		Address1 = string.Empty,
				//		Address2 = string.Empty,
				//		Address3 = string.Empty,
				//		Address4 = string.Empty,

				//		// Document
				//		OrderNumberPO = string.Empty,
				//		DocumentType = string.Empty,
				//		OrderNumber = string.Empty,
				//		OrderDate = string.Empty,

				//		// Client
				//		ClientNumber = string.Empty,
				//		InternalNumber = string.Empty,
				//		ShippingMethod = string.Empty,
				//		PaymentMethod = string.Empty,
				//		PaymentTarget = string.Empty,

				//		UST_ID = string.Empty,

				//		// Items
				//		Position = string.Empty,
				//		Article = string.Empty,
				//		Description = string.Empty,
				//		Amount = string.Empty,
				//		PE = string.Empty,
				//		BasisPrice150 = string.Empty,
				//		Cu_G = string.Empty,
				//		Cu_Surcharge = string.Empty,
				//		UnitPrice = string.Empty,
				//		Designation = string.Empty,
				//		Unit = string.Empty,
				//		TotalPrice150 = string.Empty,
				//		DEL = string.Empty,
				//		Cu_Total = string.Empty,
				//		UnitTotal = string.Empty,

				//		//
				//		Bestellt = string.Empty,
				//		Geliefert = string.Empty,
				//		Liefertermin = string.Empty,
				//		Offen = string.Empty,

				//		Abladestelle = string.Empty,

				//		LastPageText1 = string.Empty,
				//		LastPageText2 = string.Empty,
				//		LastPageText3 = string.Empty,
				//		LastPageText4 = string.Empty,
				//		LastPageText5 = string.Empty,
				//		LastPageText6 = string.Empty,
				//		LastPageText7 = string.Empty,
				//		LastPageText8 = string.Empty,
				//		LastPageText9 = string.Empty,

				//		SummarySum = string.Empty,
				//		SummaryTotal = string.Empty,
				//		SummaryUST = string.Empty,

				//		LanguageId = this._data.LanguageId,
				//		OrderTypeId = this._data.TypeId,

				//		LastUpdateTime = DateTime.Now,
				//		LastUpdateUserId = this._user.Id,

				//		Lieferadresse = string.Empty,
				//		Index_Kunde = string.Empty
				//	};
				//	gutshriftReportEntity.Id = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.Insert(gutshriftReportEntity);
				//}
				//var gutshriftReportModel = new CreateModel(gutshriftReportEntity);
				//var gutshrifts = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>
				//{
				//	gutshrift
				//};

				//var kundenNummers = from x in gutshrifts select x.Kunden_Nr ?? -1;
				//var addresses = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummers(kundenNummers.ToList());
				//var buyerEntity = Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.GetByOrderType(gutshrift.Nr, (int)Enums.ReportingEnums.OrderTypes.Order);

				//var orderItems = (Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.GutshriftId)
				//		?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>())
				//		.Where(x => x.erledigt_pos != true).ToList(); // - do not show erledigt pos - Schremmer 2022-03-10

				//var reportType = (Enums.ReportingEnums.ReportType)this._data.TypeId;

				//// build report data
				//var invoiceFields = new List<Psz.Core.CustomerService.Reporting.Models.GutshriftReportingModel> { gutshriftReportModel.ToInvoiceFields() };
				//var invoiceData = new List<Psz.Core.CustomerService.Reporting.Models.GutshriftReportingModel>();
				//var invoiceItemData = new List<Psz.Core.CustomerService.Reporting.Models.GutshriftReportingItemModel>();

				//for(int i = 0; i < gutshrifts.Count; i++)
				//{
				//	var address = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(gutshrifts[i].Kunden_Nr ?? -1);
				//	var sumSum = 0m;
				//	var sumUST = 0m;
				//	var ust = 0m;
				//	for(int j = 0; j < orderItems.Count; j++)
				//	{
				//		ust = orderItems[j].USt ?? 0m;
				//		// - 2022-08-04 - Schremmer all Pos should have VAT
				//		if(j > 0 && ust != (orderItems[j].USt ?? 0m))
				//		{
				//			return ResponseModel<byte[]>.FailureResponse("Invalid VAT");
				//		}
				//		sumSum += Convert.ToDecimal(orderItems[j].Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture);
				//		sumUST += Convert.ToDecimal(orderItems[j].Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture) * Convert.ToDecimal(orderItems[j].USt, System.Globalization.CultureInfo.InvariantCulture);
				//		var _articleItem = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItems[j].ArtikelNr ?? -1);
				//		invoiceItemData.Add(new Psz.Core.CustomerService.Reporting.Models.GutshriftReportingItemModel()
				//		{
				//			InvoiceId = i,
				//			PositionNumber = orderItems[j].Position?.ToString(), // Pos
				//			ItemNumber = cleanArticleSuffix(_articleItem?.ArtikelNummer),
				//			//_articleItem?.ArtikelNummer, // Artikel
				//			Description = orderItems[j].Bezeichnung1?.ToString(), // Beschreibung
				//			Designation = orderItems[j].Bezeichnung2?.ToString(), // Bezeichnung
				//			Amount = orderItems[j].OriginalAnzahl?.ToString(), // Menge
				//			PE = orderItems[j].Preiseinheit?.ToString(), // PE
				//			Unit = orderItems[j].Einheit?.ToString(), // Einheit
				//			BasePrice = $"{Convert.ToDecimal(orderItems[j].VKEinzelpreis, System.Globalization.CultureInfo.InvariantCulture).ToString("0.00")} €", // basispreis
				//			TotalPrice = $"{Convert.ToDecimal(orderItems[j].VKGesamtpreis, System.Globalization.CultureInfo.InvariantCulture).ToString("0.00")} €", // Gesamppreis
				//			TotalCopper = orderItems[j].EinzelCuGewicht?.ToString(), // Cu-G
				//			DEL = orderItems[j].DEL?.ToString(), // DEL
				//			SurchargeCopper = $"{Convert.ToDecimal(orderItems[j].Einzelkupferzuschlag, System.Globalization.CultureInfo.InvariantCulture).ToString("0.00")} €", // Cu-Zuschlag
				//			TotalSurchargeCopper = $"{Convert.ToDecimal(orderItems[j].Gesamtkupferzuschlag, System.Globalization.CultureInfo.InvariantCulture).ToString("0.00")} €", // Cu-Zuschlag Gesamt
				//			UnitPrice = $"{Convert.ToDecimal(orderItems[j].Einzelpreis, System.Globalization.CultureInfo.InvariantCulture).ToString("0.00")} €", // Einzelpreis
				//			TotalUnitPrice = $"{Convert.ToDecimal(orderItems[j].Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture).ToString("0.00")} €", // Einzelpreis Gesamt

				//			AB_Pos_zu_RA_Pos = $"{orderItems[j]?.ABPoszuRAPos}",
				//			Liefertermin = $"{orderItems[j]?.Liefertermin?.ToString("dd.MM.yyyy")}",
				//			Geliefert = $"{orderItems[j]?.Geliefert}",
				//			Anzahl = $"{orderItems[j]?.Anzahl}",
				//			Bestellt = $"{orderItems[j]?.Bestellnummer}",
				//			Offen = $"{orderItems[j]?.RA_Offen}",
				//			Abladestelle = !string.IsNullOrEmpty(orderItems[j]?.Abladestelle) && !string.IsNullOrWhiteSpace(orderItems[j]?.Abladestelle) ?
				//			$"Abladestelle:{orderItems[j]?.Abladestelle}" : "",
				//			Postext = orderItems[j].POSTEXT?.ToString()?.Trim(),
				//			DELFixiert = orderItems[j].DELFixiert == true ? "1" : "0",
				//			Index_Kunde = orderItems[j].Index_Kunde?.Trim() ?? "",
				//			DelFixedText = orderItems[j].DELFixiert.HasValue && orderItems[j].DELFixiert.Value ? "DEL fixiert lauf Angebot" : null,
				//			ExternComment = orderItems[j].GSExternComment,
				//		});
				//	}
				//	var rechungItem = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(gutshrifts[i].Nr_rec ?? -1);
				//	invoiceData.Add(new Psz.Core.CustomerService.Reporting.Models.GutshriftReportingModel
				//	{
				//		Id = i,

				//		Header = gutshrifts[i].Typ,
				//		ItemsHeader = string.Empty,
				//		ItemsFooter1 = gutshrifts[i].Anrede,
				//		ItemsFooter2 = gutshrifts[i].Anrede,

				//		Footer11 = string.Empty, // orders[i].Anrede,
				//		Footer12 = string.Empty, // orders[i].Anrede,
				//		Footer13 = string.Empty, // orders[i].Anrede,
				//		Footer14 = string.Empty, // orders[i].Anrede,
				//		Footer15 = string.Empty,
				//		Footer16 = string.Empty,
				//		Footer17 = string.Empty,

				//		Footer21 = string.Empty,
				//		Footer22 = string.Empty,
				//		Footer23 = string.Empty,
				//		Footer24 = string.Empty,
				//		Footer25 = string.Empty,
				//		Footer26 = string.Empty,
				//		Footer27 = string.Empty,

				//		Footer31 = string.Empty,
				//		Footer32 = string.Empty,
				//		Footer33 = string.Empty,
				//		Footer34 = string.Empty,
				//		Footer35 = string.Empty,
				//		Footer36 = string.Empty,
				//		Footer37 = string.Empty,

				//		Footer41 = string.Empty,
				//		Footer42 = string.Empty,
				//		Footer43 = string.Empty,
				//		Footer44 = string.Empty,
				//		Footer45 = string.Empty,
				//		Footer46 = string.Empty,
				//		Footer47 = string.Empty,

				//		Footer51 = string.Empty,
				//		Footer52 = string.Empty,
				//		Footer53 = string.Empty,
				//		Footer54 = string.Empty,
				//		Footer55 = string.Empty,
				//		Footer56 = string.Empty,
				//		Footer57 = string.Empty,

				//		Footer61 = string.Empty,
				//		Footer62 = string.Empty,
				//		Footer63 = string.Empty,
				//		Footer64 = string.Empty,
				//		Footer65 = string.Empty,
				//		Footer66 = string.Empty,
				//		Footer67 = string.Empty,

				//		Footer71 = string.Empty,
				//		Footer72 = string.Empty,
				//		Footer73 = string.Empty,
				//		Footer74 = string.Empty,
				//		Footer75 = string.Empty,
				//		Footer76 = string.Empty,
				//		Footer77 = string.Empty,

				//		//
				//		DocumentType = getOrderTypeI18N(reportType, language?.Sprache),
				//		OrderNumberPO = gutshrifts[i].Bezug?.ToString(),
				//		OrderNumber = gutshrifts[i].Angebot_Nr?.ToString(),
				//		OrderDate = gutshrifts[i].Datum?.ToString("dd.MM.yyyy"),

				//		//
				//		ClientNumber = gutshrifts[i].Ihr_Zeichen,
				//		InternalNumber = gutshrifts[i].Unser_Zeichen?.ToString(),
				//		ShippingMethod = gutshrifts[i].Versandart,
				//		PaymentMethod = gutshrifts[i].Zahlungsweise,
				//		PaymentTarget = gutshrifts[i].Konditionen,
				//		RechnungNummer = rechungItem?.Angebot_Nr?.ToString(),

				//		// PSZ Address
				//		Address1 = gutshrifts[i].Anrede?.Trim(), // "PSB GmbH",
				//		Address2 = gutshrifts[i].Vorname_NameFirma?.Trim(), // "Max Planck Straße 20",
				//		Address3 = gutshrifts[i].Land_PLZ_Ort?.Contains(gutshrifts[i].Straße_Postfach?.Trim()) == true
				//		  ? buyerEntity?.Name2?.Trim()
				//			: ((string.IsNullOrWhiteSpace(buyerEntity?.Name2) || gutshrifts[i].Straße_Postfach?.Contains(buyerEntity?.Name2?.Trim()) == true)
				//				? gutshrifts[i].Straße_Postfach?.Trim()
				//				: gutshrifts[i].Straße_Postfach?.Trim() + " " + buyerEntity?.Name2)?.Trim(), // "63303 Dreieich",
				//		Address4 = gutshrifts[i].Land_PLZ_Ort?.Trim(), // "Fax: +49(0) 6103/8097-27",
				//		Address5 = address?.Fax?.Trim(), // "Fax: +49(0) 6103/8097-27",
				//		Address6 = gutshrifts[i].Name2,

				//		UST_ID = gutshrifts[i]?.Freitext?.Trim(), // "", // gutshrifts[i].USt_Berechnen.ToString(), // >>>>>>

				//		SummarySum = $"{sumSum.ToString("0.00")} €",
				//		SummaryUST = $"{sumUST.ToString("0.00")} €",
				//		SummaryTotal = $"{(sumSum + sumUST).ToString("0.00")} €",
				//		Ust = $"{(ust * 100).ToString("0.##")}%",

				//		LAnrede = gutshrifts[i]?.LAnrede ?? "",
				//		LVorname = gutshrifts[i]?.LVorname_NameFirma ?? "",
				//		LName2 = gutshrifts[i]?.LName2 ?? "",
				//		LName3 = gutshrifts[i]?.LName3 ?? "",
				//		Labteilung = gutshrifts[i]?.LAbteilung ?? "",
				//		Lansprechpartner = gutshrifts[i]?.LAnsprechpartner ?? "",
				//		LStrabe = gutshrifts[i]?.LStraße_Postfach ?? "",
				//		LLandPLZOrt = gutshrifts[i]?.LLand_PLZ_Ort ?? "",
				//		LastPageText9 = $"Verwendungszweck: F-6444 Deb-Nr.: {gutshrifts[i].Unser_Zeichen} Re.-Nr.: {rechungItem?.Angebot_Nr} Re.-Datum: {(rechungItem?.Datum.HasValue == true ? rechungItem?.Datum.Value.ToString("dd.MM.yyyy") : "")}"
				//	});

				//	responseBody = Module.CS_ReportingService.GenerateGutshriftReport(reportType, invoiceFields, invoiceData, invoiceItemData.OrderBy(x => int.TryParse(x.PositionNumber, out var p) ? p : 0).ToList());
				//return ResponseModel<byte[]>.SuccessResponse(responseBody);
				//}
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
