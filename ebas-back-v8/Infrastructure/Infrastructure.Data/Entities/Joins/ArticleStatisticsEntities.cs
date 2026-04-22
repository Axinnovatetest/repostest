namespace Infrastructure.Data.Entities.Joins
{
	public class ArticleStatisticsEntities
	{
		public class LogisticsIn
		{
			public string ProjectNr { get; set; }
			public string Type { get; set; }
			public string ArticleNumber { get; set; }
			public int? ArtikleNr { get; set; }
			public string OpenCurrent { get; set; }
			public string Unit { get; set; }
			public DateTime? Date { get; set; }
			public string Name { get; set; }
			public DateTime? DeliveryDate { get; set; }
			public string SupplierABNr { get; set; }
			public DateTime? ConfirmDate { get; set; }
			public string Comments { get; set; }
			public string OrderNr { get; set; }
			public string CompletePos { get; set; }
			public string Booked { get; set; }
			public string Production { get; set; }
			public string FrameworkOrder { get; set; }

			public LogisticsIn(DataRow dataRow)
			{
				ArtikleNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				ProjectNr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
				Type = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
				ArticleNumber = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				OpenCurrent = (dataRow["Anzahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anzahl"]);
				Unit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
				Date = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
				Name = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				DeliveryDate = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
				SupplierABNr = (dataRow["AB-Nr_Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AB-Nr_Lieferant"]);
				ConfirmDate = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
				Comments = (dataRow["Bemerkung_Pos"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Pos"]);
				OrderNr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellung-Nr"]);
				CompletePos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["erledigt_pos"]);
				Booked = (dataRow["gebucht"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["gebucht"]);
				Production = (dataRow["Fertigung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fertigung"]);
				FrameworkOrder = (dataRow["Rahmenbestellung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmenbestellung"]);
			}
		}
		public class LogisticsOut
		{
			public string PSZ_Number { get; set; }
			public int? ArtikelNr { get; set; }
			public string CustomerNr { get; set; }
			public string Type { get; set; }
			public string Number { get; set; }
			public string Ordered { get; set; }
			public string OpenCurrent { get; set; }
			public DateTime? Date { get; set; }
			public string Reference { get; set; }
			public string Booked { get; set; }
			public string DistributionWarehouse { get; set; }
			public string Completed { get; set; }
			public int? OrderId { get; set; }
			public LogisticsOut(DataRow dataRow)
			{
				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				PSZ_Number = (dataRow["PSZ_Nummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ_Nummer"]);
				CustomerNr = (dataRow["Kundennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer"]);
				Type = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
				Number = (dataRow["Nummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nummer"]);
				Ordered = (dataRow["Bestellt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellt"]);
				OpenCurrent = (dataRow["OffenAktuell"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OffenAktuell"]);
				Date = (dataRow["Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin"]);
				Reference = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
				Booked = (dataRow["gebucht"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["gebucht"]);
				DistributionWarehouse = (dataRow["Auslieferlager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Auslieferlager"]);
				Completed = (dataRow["Erledigt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Erledigt"]);
				OrderId = (dataRow["OrderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderId"]);
			}
		}
		public class LogisticsInOutDetails
		{
			public string ArticleNumber { get; set; }
			public int? ArtikelNr { get; set; }
			public string Type { get; set; }
			public string OrderNumber { get; set; }
			public string Number { get; set; }
			public DateTime? Date { get; set; }
			public string Name { get; set; }
			public string StorageLocationBefore { get; set; }
			public string StorageLocationAfter { get; set; }
			public long? Rollennummer { get; set; }
			public string Gebucht_von { get; set; }
			public int? OrderId { get; set; }
			public string Bemerkung { get; set; }
			public LogisticsInOutDetails(DataRow dataRow)
			{
				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				OrderId = (dataRow["OrderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderId"]);
				ArticleNumber = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Type = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
				OrderNumber = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellung-Nr"]);
				Number = (dataRow["Anzahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anzahl"]);
				Date = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
				Name = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				StorageLocationBefore = (dataRow["Lagerplatz_von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerplatz_von"]);
				StorageLocationAfter = (dataRow["Lagerplatz_nach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerplatz_nach"]);
				Rollennummer = (dataRow["Rollennummer"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["Rollennummer"]);
				Gebucht_von = (dataRow["Gebucht von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gebucht von"]);
				Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			}
		}
		public class LogisticsFaStatus
		{
			public string Designation { get; set; }
			public string FaNumber { get; set; }
			public string ArticleNumber { get; set; }
			public int? ArtikelNr { get; set; }
			public string FaQuantity { get; set; }
			public string Completed { get; set; }
			public string Open { get; set; }
			public string DesiredDate { get; set; }
			public string ScheduledDate { get; set; }
			public string Label { get; set; }
			public string Comments { get; set; }
			public string CustomerIndex { get; set; }
			public DateTime? CustomerIndexDatum { get; set; }
			public string WorkDate { get; set; }
			public string BomVersion { get; set; }
			public string CpVersion { get; set; }
			public string Kommisioniert_komplett { get; set; }
			public string Kommisioniert_teilweise { get; set; }
			public string FA_Gestartet { get; set; }
			public int? FaID { get; set; }

			public LogisticsFaStatus(DataRow dataRow)
			{
				ArtikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArtikelNr"]);
				Designation = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
				FaID = (dataRow["FaID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaID"]);
				FaNumber = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fertigungsnummer"]);
				ArticleNumber = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				FaQuantity = (dataRow["FA Menge"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FA Menge"]);
				Completed = (dataRow["Erledigt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Erledigt"]);
				Open = (dataRow["Offen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Offen"]);
				DesiredDate = (dataRow["Wunschtermin"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Wunschtermin"]);
				ScheduledDate = (dataRow["Termin_Planung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Termin_Planung"]);
				Label = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
				Comments = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
				CustomerIndex = (dataRow["KundenIndex"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["KundenIndex"]);
				CustomerIndexDatum = (dataRow["KundenIndexDatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["KundenIndexDatum"]);
				BomVersion = (dataRow["BomVersion"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BomVersion"]);
				CpVersion = (dataRow["CpVersion"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CpVersion"]);
				WorkDate = (dataRow["Termin_werk"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Termin_werk"]);
				Kommisioniert_komplett = (dataRow["Kommisioniert_komplett"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kommisioniert_komplett"]);
				Kommisioniert_teilweise = (dataRow["Kommisioniert_teilweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kommisioniert_teilweise"]);
				FA_Gestartet = (dataRow["FA_Gestartet"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FA_Gestartet"]);
			}
		}
		public class LogisticsDeliveryList
		{
			public string CustomsTariff { get; set; }
			public string EAN { get; set; }
			public string ArticleNumber { get; set; }
			public int? ArticleNr { get; set; }
			public string OriginCountry { get; set; }
			public string Designation1 { get; set; }
			public string Designation2 { get; set; }
			public string SupplierNumber { get; set; }
			public string Name1 { get; set; }
			public string ULCertificated { get; set; }
			public string OrderNumber { get; set; }
			public string ReplacementPeriod { get; set; }
			public string StandardSupplier { get; set; }
			public string PurchasingPrice { get; set; }
			public string SalesPrice { get; set; }
			public string MinimumOrderQuantity { get; set; }
			public string DrawingNumber { get; set; }

			public LogisticsDeliveryList(DataRow dataRow)
			{
				ArticleNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				CustomsTariff = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
				EAN = (dataRow["EAN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EAN"]);
				ArticleNumber = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				OriginCountry = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
				Designation1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Designation2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				SupplierNumber = (dataRow["Lieferantennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferantennummer"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				ULCertificated = (dataRow["UL zertifiziert"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UL zertifiziert"]);
				OrderNumber = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
				ReplacementPeriod = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Wiederbeschaffungszeitraum"]);
				StandardSupplier = (dataRow["Standardlieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standardlieferant"]);
				PurchasingPrice = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einkaufspreis"]);
				SalesPrice = (dataRow["Verpackungseinheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungseinheit"]);
				MinimumOrderQuantity = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestellmenge"]);
				DrawingNumber = (dataRow["Zeichnungsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zeichnungsnummer"]);
			}
		}
		public class Logistics_TN_AL_Logistics
		{
			public string Fertigprodukt { get; set; }
			public string Designation2 { get; set; }
			public string Designation1 { get; set; }
			public string Zolltarif_nr { get; set; }
			public string Exportgewicht { get; set; }
			public string Materialkosten { get; set; }
			public string Arbeitskosten { get; set; }
			public string Verpackungsart { get; set; }
			public string Verpackungsmenge { get; set; }

			public Logistics_TN_AL_Logistics(DataRow dataRow)
			{
				Fertigprodukt = (dataRow["Fertigprodukt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fertigprodukt"]);
				Designation1 = (dataRow["Designation1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Designation1"]);
				Designation2 = (dataRow["Designation2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Designation2"]);
				Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
				Exportgewicht = (dataRow["Exportgewicht Fertigprodukt in gr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Exportgewicht Fertigprodukt in gr"]);
				Materialkosten = (dataRow["Materialkosten"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Materialkosten"]);
				Arbeitskosten = (dataRow["Arbeitskosten"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Arbeitskosten"]);
				Verpackungsart = (dataRow["Verpackungsart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungsart"]);
				Verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungsmenge"]);
			}
		}
		public class LogisticsDeliveryOverview
		{
			public string Name { get; set; }
			public string SupplierNumber { get; set; }
			public string StandardSupplier { get; set; }
			public string ArticleNumber { get; set; }
			public string Designation1 { get; set; }
			public string Designation2 { get; set; }
			public string OrderNumber { get; set; }
			public string PurchasePrice { get; set; }
			public string CustomsTariff { get; set; }
			public string OriginCountry { get; set; }
			public string NetWeightinGr { get; set; }

			public LogisticsDeliveryOverview(DataRow dataRow)
			{
				Name = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				SupplierNumber = (dataRow["Lieferantennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferantennummer"]);
				StandardSupplier = (dataRow["Standardlieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standardlieferant"]);
				ArticleNumber = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Designation1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Designation2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				OrderNumber = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
				PurchasePrice = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einkaufspreis"]);
				CustomsTariff = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
				OriginCountry = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
				NetWeightinGr = (dataRow["Nettogewicht in gr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nettogewicht in gr"]);
			}
		}
		public class LogisticsPreferences
		{
			public string ArticleNumber { get; set; }
			public string Designation1 { get; set; }
			public string PurchasePricingGroup { get; set; }
			public string Position { get; set; }
			public string ArticleNumberBOM { get; set; }
			public string DescriptionBauteils { get; set; }
			public string Name { get; set; }
			public string Number { get; set; }
			public string PurchasePrice { get; set; }
			public string StandardSupplier { get; set; }
			public string Preference { get; set; }
			public string OriginCountry { get; set; }
			public string TotalEK { get; set; }
			public string WennK { get; set; }
			public string SummevonBetrag { get; set; }

			public LogisticsPreferences(DataRow dataRow)
			{
				ArticleNumber = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Designation1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				PurchasePricingGroup = (dataRow["Verkaufspreis_Preisgruppe_1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verkaufspreis_Preisgruppe_1"]);
				Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
				ArticleNumberBOM = (dataRow["Artikelnummer_stückliste"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer_stückliste"]);
				DescriptionBauteils = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung des Bauteils"]);
				Name = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Number = (dataRow["Anzahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anzahl"]);
				PurchasePrice = (dataRow["Verkaufspreis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verkaufspreis"]);
				StandardSupplier = (dataRow["Standardlieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standardlieferant"]);
				Preference = (dataRow["Praeferenz"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Praeferenz"]);
				OriginCountry = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
				TotalEK = (dataRow["SummeEK1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SummeEK1"]);
				WennK = (dataRow["WennK"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WennK"]);
				SummevonBetrag = (dataRow["SummevonBetrag"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SummevonBetrag"]);
			}
		}
		public class Logistics_TransPot
		{
			public int? AnzahlvonFertigungsnummer { get; set; }
			public string Artikelnummer { get; set; }
			public string Bezeichnung_1 { get; set; }
			public decimal? Kennzahl { get; set; }
			public Single? Produktionszeit { get; set; }
			public decimal? Produktivität { get; set; }
			public decimal? Stundensatz { get; set; }

			public Logistics_TransPot() { }

			public Logistics_TransPot(DataRow dataRow)
			{
				AnzahlvonFertigungsnummer = (dataRow["AnzahlvonFertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AnzahlvonFertigungsnummer"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Kennzahl = (dataRow["Kennzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kennzahl"]);
				Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Produktionszeit"]);
				Produktivität = (dataRow["Produktivität"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Produktivität"]);
				Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
			}
		}
		public class ControllingDB
		{
			public Single? Anzahl { get; set; }
			public int? Artikel_Nr { get; set; }
			public string Artikelnummer { get; set; }
			public string ArtikelnummerOriginal { get; set; }
			public string Bezeichnung_1 { get; set; }
			public string Bezeichnung_2 { get; set; }
			public string Bezeichnung_des_Bauteils { get; set; }
			public int? DEL { get; set; }
			public decimal? Einkaufspreis { get; set; }
			public decimal? Gewicht { get; set; }
			public int? Kupferbasis { get; set; }
			public decimal? Kupferzahl { get; set; }
			public decimal? Kupferzuschlag { get; set; }
			public string Name1 { get; set; }
			public string Position { get; set; }
			public int? Preisgruppe { get; set; }
			public bool? Standardlieferant { get; set; }
			public decimal? Summe { get; set; }
			public decimal? SummeEK { get; set; }
			public decimal? SummeEKohneCU { get; set; }
			public decimal? SummevonBetrag { get; set; }
			public decimal? Verkaufspreis { get; set; }
			public decimal? Verkaufspreis_1 { get; set; }
			public decimal? VK_PSZ_ink_Kupfer { get; set; }

			public ControllingDB() { }

			public ControllingDB(DataRow dataRow)
			{
				Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Anzahl"]);
				Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				ArtikelnummerOriginal = (dataRow["ArtikelnummerOriginal"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelnummerOriginal"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				Bezeichnung_des_Bauteils = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung des Bauteils"]);
				DEL = (dataRow["DEL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DEL"]);
				Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
				Gewicht = (dataRow["Gewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gewicht"]);
				Kupferbasis = (dataRow["Kupferbasis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kupferbasis"]);
				Kupferzahl = (dataRow["Kupferzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kupferzahl"]);
				Kupferzuschlag = (dataRow["Kupferzuschlag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kupferzuschlag"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
				Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
				Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Standardlieferant"]);
				Summe = (dataRow["Summe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Summe"]);
				SummeEK = (dataRow["SummeEK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SummeEK"]);
				SummeEKohneCU = (dataRow["SummeEKohneCU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SummeEKohneCU"]);
				SummevonBetrag = (dataRow["SummevonBetrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SummevonBetrag"]);
				Verkaufspreis = (dataRow["Verkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verkaufspreis"]);
				Verkaufspreis_1 = (dataRow["Verkaufspreis_1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verkaufspreis_1"]);
				VK_PSZ_ink_Kupfer = (dataRow["VK_PSZ_ink_Kupfer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK_PSZ_ink_Kupfer"]);
			}
		}
		public class ControllingHistory
		{
			public string Id { get; set; }
			public string ArticleId { get; set; }
			public string EditTime { get; set; }
			public string EditAction { get; set; }
			public string EditDescription { get; set; }
			public string EditUser { get; set; }

			public ControllingHistory(DataRow dataRow)
			{
				Id = (dataRow["ID"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ID"]);
				ArticleId = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikel-Nr"]);
				EditTime = (dataRow["Datum Änderung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Datum Änderung"]);
				EditAction = (dataRow["Änderungsbereich"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Änderungsbereich"]);
				EditDescription = (dataRow["Änderungsbeschreibung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Änderungsbeschreibung"]);
				EditUser = (dataRow["Änderung von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Änderung von"]);

			}
		}
		public class ControllingArtikelEntity
		{
			public string Abladestelle { get; set; }
			public bool? aktiv { get; set; }
			public DateTime? aktualisiert { get; set; }
			public decimal? Anfangsbestand { get; set; }
			public bool? Artikel_aus_eigener_Produktion { get; set; }
			public bool? Artikel_für_weitere_Bestellungen_sperren { get; set; }
			public string Artikelfamilie_Kunde { get; set; }
			public string Artikelfamilie_Kunde_Detail1 { get; set; }
			public string Artikelfamilie_Kunde_Detail2 { get; set; }
			public string artikelklassifizierung { get; set; }
			public string Artikelkurztext { get; set; }
			public int Artikel_Nr { get; set; }
			public string Artikelnummer { get; set; }
			public bool? Barverkauf { get; set; }
			public string Bezeichnung_1 { get; set; }
			public string Bezeichnung_2 { get; set; }
			public string Bezeichnung_3 { get; set; }
			public string BezeichnungAL { get; set; }
			public bool? Blokiert_Status { get; set; }
			public bool? COF_Pflichtig { get; set; }
			public bool? CP_required { get; set; }
			public string Crossreferenz { get; set; }
			public Single? Cu_Gewicht { get; set; }
			public DateTime? Datum_Anfangsbestand { get; set; }
			public int? DEL { get; set; }
			public bool? DEL_fixiert { get; set; }
			public int? Dienstelistung { get; set; }
			public string Dokumente { get; set; }
			public string EAN { get; set; }
			public string Einheit { get; set; }
			public bool? EMPB { get; set; }
			public bool? EMPB_Freigegeben { get; set; }
			public int? Ersatzartikel { get; set; }
			public bool? ESD_Schutz { get; set; }
			public string ESD_Schutz_Text { get; set; }
			public Single? Exportgewicht { get; set; }
			public bool? fakturieren_Stückliste { get; set; }
			public string Farbe { get; set; }
			public int? fibu_rahmen { get; set; }
			public string Freigabestatus { get; set; }
			public string Freigabestatus_TN_intern { get; set; }
			public string Gebinde { get; set; }
			public decimal? Gewicht { get; set; }
			public Single? Größe { get; set; }
			public string Grund_für_Sperre { get; set; }
			public DateTime? gültig_bis { get; set; }
			public string Halle { get; set; }
			public bool? Hubmastleitungen { get; set; }
			public int? ID_Klassifizierung { get; set; }
			public string Index_Kunde { get; set; }
			public DateTime? Index_Kunde_Datum { get; set; }
			public string Info_WE { get; set; }
			public bool? Kanban { get; set; }
			public string Kategorie { get; set; }
			public string Klassifizierung { get; set; }
			public string Kriterium1 { get; set; }
			public string Kriterium2 { get; set; }
			public string Kriterium3 { get; set; }
			public string Kriterium4 { get; set; }
			public int? Kupferbasis { get; set; }
			public decimal? Kupferzahl { get; set; }
			public bool? Lagerartikel { get; set; }
			public decimal? Lagerhaltungskosten { get; set; }
			public string Langtext { get; set; }
			public bool? Langtext_drucken_AB { get; set; }
			public bool? Langtext_drucken_BW { get; set; }
			public string Lieferzeit { get; set; }
			public int? Losgroesse { get; set; }
			public Single? Materialkosten_Alt { get; set; }
			public bool? MHD { get; set; }
			public bool? Minerals_Confirmity { get; set; }
			public string Praeferenz_Aktuelles_jahr { get; set; }
			public string Praeferenz_Folgejahr { get; set; }
			public decimal? Preiseinheit { get; set; }
			public string pro_Zeiteinheit { get; set; }
			public Single? Produktionszeit { get; set; }
			public bool? Provisionsartikel { get; set; }
			public string Prüfstatus_TN_Ware { get; set; }
			public bool? Rabattierfähig { get; set; }
			public bool? Rahmen { get; set; }
			public bool? Rahmen2 { get; set; }
			public DateTime? Rahmenauslauf { get; set; }
			public DateTime? Rahmenauslauf2 { get; set; }
			public Single? Rahmenmenge { get; set; }
			public Single? Rahmenmenge2 { get; set; }
			public string Rahmen_Nr { get; set; }
			public string Rahmen_Nr2 { get; set; }
			public bool? REACH_SVHC_Confirmity { get; set; }
			public bool? ROHS_EEE_Confirmity { get; set; }
			public string Seriennummer { get; set; }
			public bool? Seriennummernverwaltung { get; set; }
			public Single? Sonderrabatt { get; set; }
			public int? Standard_Lagerort_id { get; set; }
			public bool? Stückliste { get; set; }
			public decimal? Stundensatz { get; set; }
			public string Sysmonummer { get; set; }
			public bool? UL_Etikett { get; set; }
			public bool? UL_zertifiziert { get; set; }
			public decimal? Umsatzsteuer { get; set; }
			public string Ursprungsland { get; set; }
			public bool? VDA_1 { get; set; }
			public bool? VDA_2 { get; set; }
			public string Verpackung { get; set; }
			public string Verpackungsart { get; set; }
			public int? Verpackungsmenge { get; set; }
			public bool? VK_Festpreis { get; set; }
			public string Volumen { get; set; }
			public string Warengruppe { get; set; }
			public int? Warentyp { get; set; }
			public bool? Webshop { get; set; }
			public string Werkzeug { get; set; }
			public decimal? Wert_Anfangsbestand { get; set; }
			public string Zeichnungsnummer { get; set; }
			public string Zeitraum_MHD { get; set; }
			public string Zolltarif_nr { get; set; }
			public decimal? Zuschlag_VK { get; set; }

			public ControllingArtikelEntity() { }

			public ControllingArtikelEntity(DataRow dataRow)
			{
				Abladestelle = (dataRow["Abladestelle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abladestelle"]);
				aktiv = (dataRow["aktiv"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["aktiv"]);
				aktualisiert = (dataRow["aktualisiert"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["aktualisiert"]);
				Anfangsbestand = (dataRow["Anfangsbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anfangsbestand"]);
				Artikel_aus_eigener_Produktion = (dataRow["Artikel aus eigener Produktion"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Artikel aus eigener Produktion"]);
				Artikel_für_weitere_Bestellungen_sperren = (dataRow["Artikel für weitere Bestellungen sperren"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Artikel für weitere Bestellungen sperren"]);
				Artikelfamilie_Kunde = (dataRow["Artikelfamilie_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde"]);
				Artikelfamilie_Kunde_Detail1 = (dataRow["Artikelfamilie_Kunde_Detail1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail1"]);
				Artikelfamilie_Kunde_Detail2 = (dataRow["Artikelfamilie_Kunde_Detail2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail2"]);
				artikelklassifizierung = (dataRow["artikelklassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["artikelklassifizierung"]);
				Artikelkurztext = (dataRow["Artikelkurztext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelkurztext"]);
				Artikel_Nr = Convert.ToInt32(dataRow["Artikel-Nr"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Barverkauf = (dataRow["Barverkauf"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Barverkauf"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				Bezeichnung_3 = (dataRow["Bezeichnung 3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 3"]);
				BezeichnungAL = (dataRow["BezeichnungAL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BezeichnungAL"]);
				Blokiert_Status = (dataRow["Blokiert_Status"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blokiert_Status"]);
				COF_Pflichtig = (dataRow["COF_Pflichtig"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["COF_Pflichtig"]);
				CP_required = (dataRow["CP_required"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CP_required"]);
				Crossreferenz = (dataRow["Crossreferenz"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Crossreferenz"]);
				Cu_Gewicht = (dataRow["Cu-Gewicht"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Cu-Gewicht"]);
				Datum_Anfangsbestand = (dataRow["Datum Anfangsbestand"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum Anfangsbestand"]);
				DEL = (dataRow["DEL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DEL"]);
				DEL_fixiert = (dataRow["DEL fixiert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DEL fixiert"]);
				Dienstelistung = (dataRow["Dienstelistung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Dienstelistung"]);
				Dokumente = (dataRow["Dokumente"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dokumente"]);
				EAN = (dataRow["EAN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EAN"]);
				Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
				EMPB = (dataRow["EMPB"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB"]);
				EMPB_Freigegeben = (dataRow["EMPB_Freigegeben"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB_Freigegeben"]);
				Ersatzartikel = (dataRow["Ersatzartikel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Ersatzartikel"]);
				ESD_Schutz = (dataRow["ESD_Schutz"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ESD_Schutz"]);
				ESD_Schutz_Text = (dataRow["ESD_Schutz_Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ESD_Schutz_Text"]);
				Exportgewicht = (dataRow["Exportgewicht"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Exportgewicht"]);
				fakturieren_Stückliste = (dataRow["fakturieren Stückliste"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["fakturieren Stückliste"]);
				Farbe = (dataRow["Farbe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Farbe"]);
				fibu_rahmen = (dataRow["fibu_rahmen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["fibu_rahmen"]);
				Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
				Freigabestatus_TN_intern = (dataRow["Freigabestatus TN intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus TN intern"]);
				Gebinde = (dataRow["Gebinde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gebinde"]);
				Gewicht = (dataRow["Gewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gewicht"]);
				Größe = (dataRow["Größe"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Größe"]);
				Grund_für_Sperre = (dataRow["Grund für Sperre"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund für Sperre"]);
				gültig_bis = (dataRow["gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["gültig bis"]);
				Halle = (dataRow["Halle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Halle"]);
				Hubmastleitungen = (dataRow["Hubmastleitungen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Hubmastleitungen"]);
				ID_Klassifizierung = (dataRow["ID_Klassifizierung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Klassifizierung"]);
				Index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
				Index_Kunde_Datum = (dataRow["Index_Kunde_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Kunde_Datum"]);
				Info_WE = (dataRow["Info_WE"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Info_WE"]);
				Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
				Kategorie = (dataRow["Kategorie"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kategorie"]);
				Klassifizierung = (dataRow["Klassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Klassifizierung"]);
				Kriterium1 = (dataRow["Kriterium1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium1"]);
				Kriterium2 = (dataRow["Kriterium2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium2"]);
				Kriterium3 = (dataRow["Kriterium3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium3"]);
				Kriterium4 = (dataRow["Kriterium4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium4"]);
				Kupferbasis = (dataRow["Kupferbasis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kupferbasis"]);
				Kupferzahl = (dataRow["Kupferzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kupferzahl"]);
				Lagerartikel = (dataRow["Lagerartikel"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Lagerartikel"]);
				Lagerhaltungskosten = (dataRow["Lagerhaltungskosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Lagerhaltungskosten"]);
				Langtext = (dataRow["Langtext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Langtext"]);
				Langtext_drucken_AB = (dataRow["Langtext_drucken_AB"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Langtext_drucken_AB"]);
				Langtext_drucken_BW = (dataRow["Langtext_drucken_BW"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Langtext_drucken_BW"]);
				Lieferzeit = (dataRow["Lieferzeit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferzeit"]);
				Losgroesse = (dataRow["Losgroesse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Losgroesse"]);
				Materialkosten_Alt = (dataRow["Materialkosten_Alt"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Materialkosten_Alt"]);
				MHD = (dataRow["MHD"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MHD"]);
				Minerals_Confirmity = (dataRow["Minerals Confirmity"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Minerals Confirmity"]);
				Praeferenz_Aktuelles_jahr = (dataRow["Praeferenz_Aktuelles_jahr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Praeferenz_Aktuelles_jahr"]);
				Praeferenz_Folgejahr = (dataRow["Praeferenz_Folgejahr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Praeferenz_Folgejahr"]);
				Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
				pro_Zeiteinheit = (dataRow["pro Zeiteinheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["pro Zeiteinheit"]);
				Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Produktionszeit"]);
				Provisionsartikel = (dataRow["Provisionsartikel"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Provisionsartikel"]);
				Prüfstatus_TN_Ware = (dataRow["Prüfstatus TN Ware"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prüfstatus TN Ware"]);
				Rabattierfähig = (dataRow["Rabattierfähig"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rabattierfähig"]);
				Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen"]);
				Rahmen2 = (dataRow["Rahmen2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen2"]);
				Rahmenauslauf = (dataRow["Rahmenauslauf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf"]);
				Rahmenauslauf2 = (dataRow["Rahmenauslauf2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf2"]);
				Rahmenmenge = (dataRow["Rahmenmenge"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rahmenmenge"]);
				Rahmenmenge2 = (dataRow["Rahmenmenge2"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rahmenmenge2"]);
				Rahmen_Nr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr"]);
				Rahmen_Nr2 = (dataRow["Rahmen-Nr2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr2"]);
				REACH_SVHC_Confirmity = (dataRow["REACH SVHC Confirmity"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["REACH SVHC Confirmity"]);
				ROHS_EEE_Confirmity = (dataRow["ROHS EEE Confirmity"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ROHS EEE Confirmity"]);
				Seriennummer = (dataRow["Seriennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Seriennummer"]);
				Seriennummernverwaltung = (dataRow["Seriennummernverwaltung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Seriennummernverwaltung"]);
				Sonderrabatt = (dataRow["Sonderrabatt"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Sonderrabatt"]);
				Standard_Lagerort_id = (dataRow["Standard_Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Standard_Lagerort_id"]);
				Stückliste = (dataRow["Stückliste"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Stückliste"]);
				Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
				Sysmonummer = (dataRow["Sysmonummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sysmonummer"]);
				UL_Etikett = (dataRow["UL Etikett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UL Etikett"]);
				UL_zertifiziert = (dataRow["UL zertifiziert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UL zertifiziert"]);
				Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Umsatzsteuer"]);
				Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
				VDA_1 = (dataRow["VDA_1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VDA_1"]);
				VDA_2 = (dataRow["VDA_2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VDA_2"]);
				Verpackung = (dataRow["Verpackung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackung"]);
				Verpackungsart = (dataRow["Verpackungsart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungsart"]);
				Verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verpackungsmenge"]);
				VK_Festpreis = (dataRow["VK-Festpreis"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VK-Festpreis"]);
				Volumen = (dataRow["Volumen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Volumen"]);
				Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
				Warentyp = (dataRow["Warentyp"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Warentyp"]);
				Webshop = (dataRow["Webshop"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Webshop"]);
				Werkzeug = (dataRow["Werkzeug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Werkzeug"]);
				Wert_Anfangsbestand = (dataRow["Wert_Anfangsbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Wert_Anfangsbestand"]);
				Zeichnungsnummer = (dataRow["Zeichnungsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zeichnungsnummer"]);
				Zeitraum_MHD = (dataRow["Zeitraum_MHD"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zeitraum_MHD"]);
				Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
				Zuschlag_VK = (dataRow["Zuschlag_VK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zuschlag_VK"]);
			}
		}
		public class ControllingBomReport
		{
			public string Position { get; set; }
			public string ArticleNumber { get; set; }
			public int? ArtikleNr { get; set; }
			public string Quantity { get; set; }
			public string DesignationBom { get; set; }
			public string Supplier { get; set; }
			public string OrderNumber { get; set; }

			public ControllingBomReport(DataRow dataRow)
			{
				ArtikleNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				Position = (dataRow["Pos"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Pos"]);
				ArticleNumber = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Quantity = (dataRow["Anzahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anzahl"]);
				DesignationBom = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung des Bauteils"]);
				Supplier = (dataRow["Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferant"]);
				OrderNumber = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);

			}
		}

		// -- Controlling Analysis - 
		public class ControllingAnalysis_ArticleMargeEntity
		{
			public int Artikel_Nr { get; set; }
			public decimal? DB { get; set; }
			public decimal? DBMitCu { get; set; }
			public decimal? Marge { get; set; }
			public decimal? MargeMitCu { get; set; }
			public decimal Materialkosten { get; set; }
			public decimal? MaterialkostenMitCu { get; set; }
			public string PriceType { get; set; }

			public ControllingAnalysis_ArticleMargeEntity() { }

			public ControllingAnalysis_ArticleMargeEntity(DataRow dataRow)
			{
				Artikel_Nr = Convert.ToInt32(dataRow["Artikel-Nr"]);
				DB = (dataRow["DB"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB"]);
				DBMitCu = (dataRow["DBMitCu"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DBMitCu"]);
				Marge = (dataRow["Marge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Marge"]);
				MargeMitCu = (dataRow["MargeMitCu"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["MargeMitCu"]);
				Materialkosten = Convert.ToDecimal(dataRow["Materialkosten"]);
				MaterialkostenMitCu = (dataRow["MaterialkostenMitCu"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["MaterialkostenMitCu"]);
				PriceType = (dataRow["PriceType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PriceType"]);
			}

			public ControllingAnalysis_ArticleMargeEntity ShallowClone()
			{
				return new ControllingAnalysis_ArticleMargeEntity
				{
					Artikel_Nr = Artikel_Nr,
					DB = DB,
					DBMitCu = DBMitCu,
					Marge = Marge,
					MargeMitCu = MargeMitCu,
					Materialkosten = Materialkosten,
					MaterialkostenMitCu = MaterialkostenMitCu,
					PriceType = PriceType
				};
			}
		}
		public class ControllingAnalysis_VKIncludingCopper
		{
			public string Artikelfamilie_Kunde { get; set; }
			public string Artikelfamilie_Kunde_Detail1 { get; set; }
			public string Artikelfamilie_Kunde_Detail2 { get; set; }
			public int Artikel_Nr { get; set; }
			public string Artikelnummer { get; set; }
			public string Bezeichnung_1 { get; set; }
			public string Bezeichnung_2 { get; set; }
			public string Bezeichnung_3 { get; set; }
			public decimal? BisMenge { get; set; }
			public decimal? Cu_Gewicht { get; set; }
			public decimal? DB { get; set; }
			public decimal? DB_I_mit { get; set; }
			public decimal? DBMitCu { get; set; }
			public int? DEL { get; set; }
			public bool? DEL_fixiert { get; set; }
			public bool? EdiDefault { get; set; }
			public string Freigabestatus { get; set; }
			public decimal? Gewicht_in_gr { get; set; }
			public decimal? HourlyRate { get; set; }
			public bool? Hubmastleitungen { get; set; }
			public string Index_Kunde { get; set; }
			public DateTime? Index_Kunde_Datum { get; set; }
			public decimal Jahresmenge { get; set; }
			public decimal Jahresumsatz { get; set; }
			public decimal? Kalkulatorische_kosten { get; set; }
			public decimal? Kupferzuschlag { get; set; }
			public int? LotSize { get; set; }
			public decimal? Marge { get; set; }
			public decimal? Marge_mit_CU { get; set; }
			public decimal? MargeMitCu { get; set; }
			public decimal Materialkosten { get; set; }
			public decimal? MaterialkostenMitCu { get; set; }
			public decimal? Preiseinheit { get; set; }
			public decimal? Price { get; set; }
			public decimal? PriceInclCu { get; set; }
			public string PriceType { get; set; }
			public decimal? ProductionCosts { get; set; }
			public decimal? ProductionLotSize { get; set; }
			public decimal? ProductionTime { get; set; }
			public decimal? Produktionszeit { get; set; }
			public decimal? Stundensatz { get; set; }
			public string Sysmonummer { get; set; }
			public bool? UBG { get; set; }
			public string Ursprungsland { get; set; }
			public decimal? Verkaufspreis { get; set; }
			public int? Verpackungsmenge { get; set; }
			public decimal? VK_inkl_Kupfer { get; set; }
			public bool? VK_Festpreis { get; set; }
			public string Warengruppe { get; set; }
			public string Zolltarif_nr { get; set; }

			public ControllingAnalysis_VKIncludingCopper() { }

			public ControllingAnalysis_VKIncludingCopper(DataRow dataRow)
			{
				Artikelfamilie_Kunde = (dataRow["Artikelfamilie_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde"]);
				Artikelfamilie_Kunde_Detail1 = (dataRow["Artikelfamilie_Kunde_Detail1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail1"]);
				Artikelfamilie_Kunde_Detail2 = (dataRow["Artikelfamilie_Kunde_Detail2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail2"]);
				Artikel_Nr = Convert.ToInt32(dataRow["Artikel-Nr"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				Bezeichnung_3 = (dataRow["Bezeichnung 3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 3"]);
				BisMenge = (dataRow["BisMenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["BisMenge"]);
				Cu_Gewicht = (dataRow["Cu-Gewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Cu-Gewicht"]);
				DB = (dataRow["DB"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB"]);
				DB_I_mit = (dataRow["DB I mit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB I mit"]);
				DBMitCu = (dataRow["DBMitCu"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DBMitCu"]);
				DEL = (dataRow["DEL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DEL"]);
				DEL_fixiert = (dataRow["DEL fixiert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DEL fixiert"]);
				EdiDefault = (dataRow["EdiDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EdiDefault"]);
				Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
				Gewicht_in_gr = (dataRow["Gewicht in gr"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gewicht in gr"]);
				HourlyRate = (dataRow["HourlyRate"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["HourlyRate"]);
				Hubmastleitungen = (dataRow["Hubmastleitungen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Hubmastleitungen"]);
				Index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
				Index_Kunde_Datum = (dataRow["Index_Kunde_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Kunde_Datum"]);
				Jahresmenge = Convert.ToDecimal(dataRow["Jahresmenge"]);
				Jahresumsatz = Convert.ToDecimal(dataRow["Jahresumsatz"]);
				Kalkulatorische_kosten = (dataRow["Kalkulatorische kosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kalkulatorische kosten"]);
				Kupferzuschlag = (dataRow["Kupferzuschlag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kupferzuschlag"]);
				LotSize = (dataRow["LotSize"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LotSize"]);
				Marge = (dataRow["Marge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Marge"]);
				Marge_mit_CU = (dataRow["Marge mit CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Marge mit CU"]);
				MargeMitCu = (dataRow["MargeMitCu"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["MargeMitCu"]);
				Materialkosten = Convert.ToDecimal(dataRow["Materialkosten"]);
				MaterialkostenMitCu = (dataRow["MaterialkostenMitCu"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["MaterialkostenMitCu"]);
				Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
				Price = (dataRow["Price"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Price"]);
				PriceInclCu = (dataRow["PriceInclCu"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PriceInclCu"]);
				PriceType = (dataRow["PriceType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PriceType"]);
				ProductionCosts = (dataRow["ProductionCosts"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionCosts"]);
				ProductionLotSize = (dataRow["ProductionLotSize"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionLotSize"]);
				ProductionTime = (dataRow["ProductionTime"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionTime"]);
				Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Produktionszeit"]);
				Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
				Sysmonummer = (dataRow["Sysmonummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sysmonummer"]);
				UBG = (dataRow["UBG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UBG"]);
				Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
				Verkaufspreis = (dataRow["Verkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verkaufspreis"]);
				Verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verpackungsmenge"]);
				VK_inkl_Kupfer = (dataRow["VK inkl Kupfer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK inkl Kupfer"]);
				VK_Festpreis = (dataRow["VK-Festpreis"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VK-Festpreis"]);
				Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
				Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
			}
		}
		public class ControllingAnalysis_VKIncludingCopper_OLD
		{
			public string Artikelnummer { get; set; }
			public string Bezeichnung_1 { get; set; }
			public string Bezeichnung_2 { get; set; }
			public double? Cu_Gewicht { get; set; }
			public int? DEL { get; set; }
			public bool? DEL_fixiert { get; set; }
			public string Freigabestatus { get; set; }
			public double? Gewicht_in_gr { get; set; }
			public bool? Hubmastleitungen { get; set; }
			public string Index_Kunde { get; set; }
			public DateTime? Index_Kunde_Datum { get; set; }
			public int Jahresmenge { get; set; }
			public int Jahresumsatz { get; set; }
			public decimal? Kupferzuschlag { get; set; }
			public decimal Materialkosten { get; set; }
			public decimal? ME1 { get; set; }
			public decimal? ME2 { get; set; }
			public decimal? ME3 { get; set; }
			public decimal? ME4 { get; set; }
			public decimal? Preiseinheit { get; set; }
			public double? Produktionszeit { get; set; }
			public decimal? Staffelpreis1 { get; set; }
			public decimal? Staffelpreis2 { get; set; }
			public decimal? Staffelpreis3 { get; set; }
			public decimal? Staffelpreis4 { get; set; }
			public decimal? Stundensatz { get; set; }
			public string Sysmonummer { get; set; }
			public string Ursprungsland { get; set; }
			public decimal? Verkaufspreis { get; set; }
			public decimal? VK_inkl_Kupfer { get; set; }
			public bool? VK_Festpreis { get; set; }
			public string Zolltarif_nr { get; set; }

			// -
			public int? Losgroesse { get; set; }
			public int? Verpackungsmenge { get; set; }
			public string Warengruppe { get; set; }
			public decimal? Kalkulatorische_kosten { get; set; }
			public decimal? Marge_mit_CU { get; set; }
			public decimal? DB_I_mit { get; set; }

			// - 2023-01-18 - P. Luff
			public decimal? ErstmusterVKpreis { get; set; }
			public decimal? ErstmusterVKpreisInklKupfer { get; set; }
			// - 2023-01-31
			public bool? EdiDefault { get; set; }
			public bool? UBG { get; set; }

			public string Bezeichnung_3 { get; set; }
			public decimal? ProductionLotSize { get; set; }
			public string Artikelfamilie_Kunde_Detail1 { get; set; }
			public string Artikelfamilie_Kunde_Detail2 { get; set; }
			public string Artikelfamilie_Kunde { get; set; }

			public ControllingAnalysis_VKIncludingCopper_OLD(DataRow dataRow)
			{
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				Cu_Gewicht = (dataRow["Cu-Gewicht"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Cu-Gewicht"]);
				DEL = (dataRow["DEL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DEL"]);
				DEL_fixiert = (dataRow["DEL fixiert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DEL fixiert"]);
				Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
				Gewicht_in_gr = (dataRow["Gewicht in gr"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Gewicht in gr"]);
				Hubmastleitungen = (dataRow["Hubmastleitungen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Hubmastleitungen"]);
				Index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
				Index_Kunde_Datum = (dataRow["Index_Kunde_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Kunde_Datum"]);
				Jahresmenge = Convert.ToInt32(dataRow["Jahresmenge"]);
				Jahresumsatz = Convert.ToInt32(dataRow["Jahresumsatz"]);
				Kupferzuschlag = (dataRow["Kupferzuschlag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kupferzuschlag"]);
				Materialkosten = Convert.ToDecimal(dataRow["Materialkosten"]);
				ME1 = (dataRow["ME1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ME1"]);
				ME2 = (dataRow["ME2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ME2"]);
				ME3 = (dataRow["ME3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ME3"]);
				ME4 = (dataRow["ME4"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ME4"]);
				Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
				Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Produktionszeit"]);
				Staffelpreis1 = (dataRow["Staffelpreis1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis1"]);
				Staffelpreis2 = (dataRow["Staffelpreis2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis2"]);
				Staffelpreis3 = (dataRow["Staffelpreis3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis3"]);
				Staffelpreis4 = (dataRow["Staffelpreis4"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis4"]);
				Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
				Sysmonummer = (dataRow["Sysmonummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sysmonummer"]);
				Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
				Verkaufspreis = (dataRow["Verkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verkaufspreis"]);
				VK_inkl_Kupfer = (dataRow["VK inkl Kupfer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK inkl Kupfer"]);
				VK_Festpreis = (dataRow["VK-Festpreis"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VK-Festpreis"]);
				Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);

				// -
				Losgroesse = (dataRow["Losgroesse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Losgroesse"]);
				Verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verpackungsmenge"]);
				Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
				Kalkulatorische_kosten = (dataRow["Kalkulatorische kosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kalkulatorische kosten"]);
				Marge_mit_CU = (dataRow["Marge mit CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Marge mit CU"]);
				DB_I_mit = (dataRow["DB I mit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB I mit"]);

				ErstmusterVKpreis = (dataRow["ErstmusterVKpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ErstmusterVKpreis"]);
				ErstmusterVKpreisInklKupfer = (dataRow["ErstmusterVKpreis Inkl Kupfer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ErstmusterVKpreis Inkl Kupfer"]);
				ProductionLotSize = (dataRow["ProductionLotSize"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionLotSize"]);

				EdiDefault = (dataRow["EdiDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EdiDefault"]);
				UBG = (dataRow["UBG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UBG"]);
				Bezeichnung_3 = (dataRow["Bezeichnung 3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 3"]);
				Artikelfamilie_Kunde_Detail1 = (dataRow["Artikelfamilie_Kunde_Detail1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail1"]);
				Artikelfamilie_Kunde_Detail2 = (dataRow["Artikelfamilie_Kunde_Detail2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail2"]);
				Artikelfamilie_Kunde = (dataRow["Artikelfamilie_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde"]);
			}
		}
		public class ControllingAnalysis_AverageMaterialContent
		{
			public string Artikelnummer { get; set; }
			public string Liefermenge { get; set; }
			public string Gesamtnettoumsatz { get; set; }
			public string Einzelmaterialkosten { get; set; }
			public string Einzellohnkosten { get; set; }
			public string Einzelnettoumsatz { get; set; }
			public string Materialanteil_ungewichtet { get; set; }
			public string Lohnanteil { get; set; }
			public string DB1 { get; set; }
			public string Materialanteil_gewichtet { get; set; }
			public string Lohnanteil_gewichtet { get; set; }
			public string DB1_gewichtet { get; set; }

			public ControllingAnalysis_AverageMaterialContent(DataRow dataRow)
			{
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Liefermenge = (dataRow["Liefermenge"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Liefermenge"]);
				Gesamtnettoumsatz = (dataRow["Gesamtnettoumsatz"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gesamtnettoumsatz"]);
				Einzelmaterialkosten = (dataRow["Einzelmaterialkosten"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einzelmaterialkosten"]);
				Einzellohnkosten = (dataRow["Einzellohnkosten"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einzellohnkosten"]);
				Einzelnettoumsatz = (dataRow["Einzelnettoumsatz"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einzelnettoumsatz"]);
				Materialanteil_ungewichtet = (dataRow["Materialanteil_ungewichtet"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Materialanteil_ungewichtet"]);
				Lohnanteil = (dataRow["Lohnanteil"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lohnanteil"]);
				DB1 = (dataRow["DB1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DB1"]);
				Materialanteil_gewichtet = (dataRow["Materialanteil_gewichtet"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Materialanteil_gewichtet"]);
				Lohnanteil_gewichtet = (dataRow["Lohnanteil_gewichtet"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lohnanteil_gewichtet"]);
				DB1_gewichtet = (dataRow["DB1_gewichtet"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DB1_gewichtet"]);

			}
		}
		public class ControllingAnalysis_ProjectMessage
		{
			public DateTime? AB_Datum { get; set; }
			public int? Arbeitszeit_Serien_Pro_Kabesatz { get; set; }
			public string Artikelnummer { get; set; }
			public string Bemerkungen { get; set; }
			public int? EAU { get; set; }
			public string EMPB { get; set; }
			public DateTime? Erstanlage { get; set; }
			public DateTime? FA_Datum { get; set; }
			public int ID { get; set; }
			public string Kontakt_AV_PSZ { get; set; }
			public string Kontakt_CS_PSZ { get; set; }
			public string Kontakt_Technik_Kunde { get; set; }
			public string Kontakt_Technik_PSZ { get; set; }
			public int? Kosten { get; set; }
			public string Krimp_WKZ { get; set; }
			public string Material_Eskalation_AV { get; set; }
			public string Material_Eskalation_Termin { get; set; }
			public string Material_Komplett { get; set; }
			public int? Menge { get; set; }
			public int? MOQ { get; set; }
			public string Projekt_betreung { get; set; }
			public string Projekt_Start { get; set; }
			public bool? Projektmeldung { get; set; }
			public string Projekt_Nr { get; set; }
			public string Rapid_Prototyp { get; set; }
			public string Serie_PSZ { get; set; }
			public string SG_WKZ { get; set; }
			public string Standort_Muster { get; set; }
			public string Standort_Serie { get; set; }
			public string Summe_Arbeitszeit { get; set; }
			public string Termin_mit_Technik_abgesprochen { get; set; }
			public string TSP_Kunden { get; set; }
			public string Typ { get; set; }
			public string UL_Verpackung { get; set; }
			public DateTime? Wunschtermin_Kunde { get; set; }
			public string Zuschlag { get; set; }

			public ControllingAnalysis_ProjectMessage() { }

			public ControllingAnalysis_ProjectMessage(DataRow dataRow)
			{
				AB_Datum = (dataRow["AB_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AB_Datum"]);
				Arbeitszeit_Serien_Pro_Kabesatz = (dataRow["Arbeitszeit Serien Pro Kabesatz"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Arbeitszeit Serien Pro Kabesatz"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
				EAU = (dataRow["EAU"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EAU"]);
				EMPB = (dataRow["EMPB"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EMPB"]);
				Erstanlage = (dataRow["Erstanlage"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Erstanlage"]);
				FA_Datum = (dataRow["FA_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_Datum"]);
				ID = Convert.ToInt32(dataRow["ID"]);
				Kontakt_AV_PSZ = (dataRow["Kontakt_AV_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_AV_PSZ"]);
				Kontakt_CS_PSZ = (dataRow["Kontakt_CS_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_CS_PSZ"]);
				Kontakt_Technik_Kunde = (dataRow["Kontakt_Technik_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_Technik_Kunde"]);
				Kontakt_Technik_PSZ = (dataRow["Kontakt_Technik_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_Technik_PSZ"]);
				Kosten = (dataRow["Kosten"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kosten"]);
				Krimp_WKZ = (dataRow["Krimp_WKZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Krimp_WKZ"]);
				Material_Eskalation_AV = (dataRow["Material_Eskalation_AV"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Eskalation_AV"]);
				Material_Eskalation_Termin = (dataRow["Material_Eskalation_Termin"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Eskalation_Termin"]);
				Material_Komplett = (dataRow["Material_Komplett"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Komplett"]);
				Menge = (dataRow["Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge"]);
				MOQ = (dataRow["MOQ"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MOQ"]);
				Projekt_betreung = (dataRow["Projekt betreung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt betreung"]);
				Projekt_Start = (dataRow["Projekt_Start"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt_Start"]);
				Projektmeldung = (dataRow["Projektmeldung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Projektmeldung"]);
				Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
				Rapid_Prototyp = (dataRow["Rapid Prototyp"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rapid Prototyp"]);
				Serie_PSZ = (dataRow["Serie_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Serie_PSZ"]);
				SG_WKZ = (dataRow["SG_WKZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SG_WKZ"]);
				Standort_Muster = (dataRow["Standort_Muster"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standort_Muster"]);
				Standort_Serie = (dataRow["Standort_Serie"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standort_Serie"]);
				Summe_Arbeitszeit = (dataRow["Summe Arbeitszeit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Summe Arbeitszeit"]);
				Termin_mit_Technik_abgesprochen = (dataRow["Termin mit Technik abgesprochen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Termin mit Technik abgesprochen"]);
				TSP_Kunden = (dataRow["TSP Kunden"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TSP Kunden"]);
				Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
				UL_Verpackung = (dataRow["UL_Verpackung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UL_Verpackung"]);
				Wunschtermin_Kunde = (dataRow["Wunschtermin_Kunde"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Wunschtermin_Kunde"]);
				Zuschlag = (dataRow["Zuschlag"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zuschlag"]);
			}
		}
		public class ControllingAnalysis_ProjectMessagePDF
		{
			public DateTime? AB_Datum { get; set; }
			public int? Arbeitszeit_Serien_Pro_Kabesatz { get; set; }
			public string Artikelnummer { get; set; }
			public string Bemerkungen { get; set; }
			public int? EAU { get; set; }
			public string EMPB { get; set; }
			public DateTime? Erstanlage { get; set; }
			public DateTime? FA_Datum { get; set; }
			public int ID { get; set; }
			public string Kontakt_AV_PSZ { get; set; }
			public string Kontakt_CS_PSZ { get; set; }
			public string Kontakt_Technik_Kunde { get; set; }
			public string Kontakt_Technik_PSZ { get; set; }
			public int? Kosten { get; set; }
			public string Krimp_WKZ { get; set; }
			public string Material_Eskalation_AV { get; set; }
			public string Material_Eskalation_Termin { get; set; }
			public string Material_Komplett { get; set; }
			public int? Menge { get; set; }
			public int? MOQ { get; set; }
			public string Projekt_betreung { get; set; }
			public string Projekt_Start { get; set; }
			public bool? Projektmeldung { get; set; }
			public string Projekt_Nr { get; set; }
			public string Rapid_Prototyp { get; set; }
			public string Serie_PSZ { get; set; }
			public string SG_WKZ { get; set; }
			public string Standort_Muster { get; set; }
			public string Standort_Serie { get; set; }
			public string Summe_Arbeitszeit { get; set; }
			public string Termin_mit_Technik_abgesprochen { get; set; }
			public string TSP_Kunden { get; set; }
			public string Typ { get; set; }
			public string UL_Verpackung { get; set; }
			public DateTime? Wunschtermin_Kunde { get; set; }
			public string Zuschlag { get; set; }

			// 
			public string Kunde { get; set; }
			public string Kundenschlussel { get; set; }

			public ControllingAnalysis_ProjectMessagePDF() { }

			public ControllingAnalysis_ProjectMessagePDF(DataRow dataRow)
			{
				AB_Datum = (dataRow["AB_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AB_Datum"]);
				Arbeitszeit_Serien_Pro_Kabesatz = (dataRow["Arbeitszeit Serien Pro Kabesatz"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Arbeitszeit Serien Pro Kabesatz"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
				EAU = (dataRow["EAU"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EAU"]);
				EMPB = (dataRow["EMPB"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EMPB"]);
				Erstanlage = (dataRow["Erstanlage"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Erstanlage"]);
				FA_Datum = (dataRow["FA_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_Datum"]);
				ID = Convert.ToInt32(dataRow["ID"]);
				Kontakt_AV_PSZ = (dataRow["Kontakt_AV_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_AV_PSZ"]);
				Kontakt_CS_PSZ = (dataRow["Kontakt_CS_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_CS_PSZ"]);
				Kontakt_Technik_Kunde = (dataRow["Kontakt_Technik_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_Technik_Kunde"]);
				Kontakt_Technik_PSZ = (dataRow["Kontakt_Technik_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_Technik_PSZ"]);
				Kosten = (dataRow["Kosten"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kosten"]);
				Krimp_WKZ = (dataRow["Krimp_WKZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Krimp_WKZ"]);
				Material_Eskalation_AV = (dataRow["Material_Eskalation_AV"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Eskalation_AV"]);
				Material_Eskalation_Termin = (dataRow["Material_Eskalation_Termin"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Eskalation_Termin"]);
				Material_Komplett = (dataRow["Material_Komplett"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Komplett"]);
				Menge = (dataRow["Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge"]);
				MOQ = (dataRow["MOQ"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MOQ"]);
				Projekt_betreung = (dataRow["Projekt betreung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt betreung"]);
				Projekt_Start = (dataRow["Projekt_Start"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt_Start"]);
				Projektmeldung = (dataRow["Projektmeldung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Projektmeldung"]);
				Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
				Rapid_Prototyp = (dataRow["Rapid Prototyp"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rapid Prototyp"]);
				Serie_PSZ = (dataRow["Serie_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Serie_PSZ"]);
				SG_WKZ = (dataRow["SG_WKZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SG_WKZ"]);
				Standort_Muster = (dataRow["Standort_Muster"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standort_Muster"]);
				Standort_Serie = (dataRow["Standort_Serie"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standort_Serie"]);
				Summe_Arbeitszeit = (dataRow["Summe Arbeitszeit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Summe Arbeitszeit"]);
				Termin_mit_Technik_abgesprochen = (dataRow["Termin mit Technik abgesprochen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Termin mit Technik abgesprochen"]);
				TSP_Kunden = (dataRow["TSP Kunden"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TSP Kunden"]);
				Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
				UL_Verpackung = (dataRow["UL_Verpackung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UL_Verpackung"]);
				Wunschtermin_Kunde = (dataRow["Wunschtermin_Kunde"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Wunschtermin_Kunde"]);
				Zuschlag = (dataRow["Zuschlag"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zuschlag"]);
				//
				Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
				Kundenschlussel = (dataRow["Kundenschlüssel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundenschlüssel"]);
			}
		}

		public class ControllingAnalysis_HighRunner
		{
			public string ArtikelNummer { get; set; }
			public string Bezeichnung1 { get; set; }
			public string Bezeichnung2 { get; set; }
			public string Bestell_Nr { get; set; }
			public string Name1 { get; set; }
			public string GebuchterWareneingang { get; set; }
			public string MengeGebuchtFA { get; set; }
			public string EinkaufsVolume { get; set; }
			public string Einkaufspreis { get; set; }
			public string Zolltarif_Nr { get; set; }
			public string Getwichte { get; set; }
			public ControllingAnalysis_HighRunner() { }
			public ControllingAnalysis_HighRunner(DataRow dataRow)
			{
				ArtikelNummer = (dataRow["ArtikelNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelNummer"]);
				Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Bezeichnung2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				GebuchterWareneingang = (dataRow["Gebuchter Wareneingang"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gebuchter Wareneingang"]);
				MengeGebuchtFA = (dataRow["Menge Gebucht FA"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Menge Gebucht FA"]);
				EinkaufsVolume = (dataRow["Einkaufsvolumen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einkaufsvolumen"]);
				Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einkaufspreis"]);
				Zolltarif_Nr = (dataRow["Zolltarif_Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_Nr"]);
				Getwichte = (dataRow["Gewichte"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewichte"]);
			}
		}
		public class CA_HighRunner
		{
			public string Artikelnummer { get; set; }
			public string Bestell_Nr { get; set; }
			public int? ArtikelNr { get; set; }
			public string Bezeichnung_2 { get; set; }
			public string Bezeichnung1 { get; set; }
			public decimal? Einkaufsmenge { get; set; }
			public decimal? Einkaufspreis { get; set; }
			public decimal? Einkaufsvolumen { get; set; }
			public decimal? Gewichte { get; set; }
			public string Name1 { get; set; }
			public string Zolltarif_nr { get; set; }

			public CA_HighRunner() { }

			public CA_HighRunner(DataRow dataRow)
			{
				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Artikel-Nr"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
				Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				Bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
				Einkaufsmenge = (dataRow["Einkaufsmenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufsmenge"]);
				Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
				Einkaufsvolumen = (dataRow["Einkaufsvolumen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufsvolumen"]);
				Gewichte = (dataRow["Gewichte"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gewichte"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
			}
		}
		public class CA_SupplierHitCount
		{
			public decimal? Einkaufsvolumen { get; set; }
			public string Name1 { get; set; }

			public CA_SupplierHitCount() { }

			public CA_SupplierHitCount(DataRow dataRow)
			{
				Einkaufsvolumen = (dataRow["Einkaufsvolumen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufsvolumen"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			}
		}
		public class ControllingAnalysis_MaterialbestandSpezifischLautNummernkreis
		{
			public string Nummernkreis { get; set; }
			public string Artikelnummer { get; set; }
			public string Bezeichnung_1 { get; set; }
			public int Lagerort_id { get; set; }
			public string Lagerort { get; set; }
			public long Bestand { get; set; }
			public decimal Mindestbestand { get; set; }
			public string Bestell_Nr { get; set; }
			public string Name1 { get; set; }
			public decimal Einkaufspreis { get; set; }
			public decimal Kupferzahl { get; set; }
			public decimal Bestandskosten { get; set; }

			public ControllingAnalysis_MaterialbestandSpezifischLautNummernkreis(DataRow dataRow)
			{
				Nummernkreis = (dataRow["Nummernkreis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nummernkreis"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lagerort_id"]);
				Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
				Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Bestand"]);
				Mindestbestand = (dataRow["Mindestbestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Mindestbestand"]);
				Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Einkaufspreis"]);
				Kupferzahl = (dataRow["Kupferzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Kupferzahl"]);
				Bestandskosten = (dataRow["Bestandskosten"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Bestandskosten"]);
			}
		}

		public class ControllingAnalysis_MaterialbestandSpezifischLautNummernkreisPurchase
		{
			public string Artikelnummer { get; set; }
			public string Bezeichnung_1 { get; set; }
			public long Bestand { get; set; }
			public decimal Mindestbestand { get; set; }
			public string Bestell_Nr { get; set; }
			public string Name1 { get; set; }
			public decimal Einkaufspreis { get; set; }
			public decimal Kupferzahl { get; set; }
			public decimal Bestandskosten { get; set; }
			public decimal Lieferzeit { get; set; }

			public ControllingAnalysis_MaterialbestandSpezifischLautNummernkreisPurchase(DataRow dataRow)
			{
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Bestand"]);
				Mindestbestand = (dataRow["Mindestbestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Mindestbestand"]);
				Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Einkaufspreis"]);
				Kupferzahl = (dataRow["Kupferzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Kupferzahl"]);
				Bestandskosten = (dataRow["Bestandskosten"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Bestandskosten"]);
				Lieferzeit = (dataRow["Lieferzeit"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Lieferzeit"]);
			}
		}
		public class ControllingAnalysis_PszPrioeinkauf_report2
		{
			public string Bestellung_Nr { get; set; }
			public DateTime? Datum { get; set; }
			public string Anzahl { get; set; }
			public string Artikelnummer { get; set; }
			public string Bezeichnung { get; set; }
			public DateTime? Liefertermin { get; set; }
			public DateTime? Bestatigter_Termin { get; set; }
			public string Name1 { get; set; }
			public string Telefon { get; set; }
			public string Fax { get; set; }
			public string Lagerort_id { get; set; }
			public string erledigt_pos { get; set; }
			public string Typ { get; set; }
			public string gebucht { get; set; }
			public string erledigt { get; set; }
			public string Position_erledigt { get; set; }

			public ControllingAnalysis_PszPrioeinkauf_report2(DataRow dataRow)
			{
				Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellung-Nr"]);
				Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
				Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anzahl"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
				Bestatigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
				Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
				Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort_id"]);
				erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["erledigt_pos"]);
				Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
				gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["gebucht"]);
				erledigt = (dataRow["erledigt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["erledigt"]);
				Position_erledigt = (dataRow["Position erledigt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position erledigt"]);
			}
		}
		public class ControllingAnalysis_PszPrioeinkauf_report1
		{
			public string Bestellung_Nr { get; set; }
			public DateTime? Datum { get; set; }
			public string Anzahl { get; set; }
			public string Artikelnummer { get; set; }
			public string Bezeichnung_1 { get; set; }
			public DateTime? Liefertermin { get; set; }
			public string Bestatigter_Termin { get; set; }
			public string Name1 { get; set; }
			public string Telefon { get; set; }
			public string Fax { get; set; }
			public string Lagerort_id { get; set; }
			public string erledigt_pos { get; set; }
			public string Typ { get; set; }
			public string gebucht { get; set; }
			public string erledigt { get; set; }
			public string Position_erledigt { get; set; }
			public string Differenz { get; set; }

			public ControllingAnalysis_PszPrioeinkauf_report1(DataRow dataRow)
			{
				Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellung-Nr"]);
				Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
				Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anzahl"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
				Bestatigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestätigter_Termin"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
				Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
				Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort_id"]);
				erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["erledigt_pos"]);
				Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
				gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["gebucht"]);
				erledigt = (dataRow["erledigt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["erledigt"]);
				Position_erledigt = (dataRow["Position erledigt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position erledigt"]);
				Differenz = (dataRow["Differenz"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Differenz"]);
			}
		}

		// - 15
		public class ControllingAnalysis_SuperbillROHInput
		{
			public string Artikelnummer { get; set; }
			public decimal Menge { get; set; }
			public int Fertigungsnummer { get; set; }
		}
		public class ControllingAnalysis_SuperbillROH
		{
			public List<ControllingAnalysis_SuperbillROHDetail> SuperbillROHDetails { get; set; }
			public List<ControllingAnalysis_SuperbillROHSum> SuperbillROHSums { get; set; }
			public byte[] SuperbillROHDetailsData { get; set; }
			public byte[] SuperbillROHSumsData { get; set; }
		}

		// - 16
		public class ControllingAnalysis_SuperbillROHDetail
		{
			public string Artikelnummer_FG { get; set; }
			public string Menge_FG { get; set; }
			public string Bez1_FG { get; set; }
			public string Bez2_FG { get; set; }
			public string Artikelnummer_ROH { get; set; }
			public string Menge_ROH { get; set; }
			public string Bez1_ROH { get; set; }
			public string Bez2_ROH { get; set; }
			public string Standardlieferant { get; set; }
			public string Bestell_Nr_ROH { get; set; }
			public string Einkaufspreis_ROH { get; set; }
			public string Kupferzahl_ROH { get; set; }
			public string Mindestbestellmenge_ROH { get; set; }
			public string Wiederbeschaffungszeitraum_ROH { get; set; }
			public string UL_zertifiziert_ROH { get; set; }
			public string Bestand_ROH_CZ { get; set; }
			public string Bestand_ROH_TN { get; set; }
			public string Bestand_ROH_AL { get; set; }
			public string Bestand_ROH_WS { get; set; }
			public string Bestand_ROH_BETN { get; set; }
			public string Bestand_ROH_GZTN { get; set; }
			public string Bestand_ROH_Obsolete { get; set; }
			public string Rahmen { get; set; }
			public string Rahmen_Nr { get; set; }
			public string Rahmenmenge { get; set; }
			public string Rahmenauslauf { get; set; }
			public string Mindestbestand_ROH_CZ { get; set; }
			public string Mindestbestand_ROH_TN { get; set; }
			public string Mindestbestand_ROH_AL { get; set; }
			public string Mindestbestand_ROH_KHTN { get; set; }
			public string Mindestbestand_ROH_BETN { get; set; }
			public string Mindestbestand_ROH_GZTN { get; set; }
			public string Mindestbestand_ROH_Obsolete { get; set; }
			public string ROH_Angebotsdatum { get; set; }
			public ControllingAnalysis_SuperbillROHDetail() { }
			public ControllingAnalysis_SuperbillROHDetail(DataRow dataRow)
			{
				Artikelnummer_FG = (dataRow["FG"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FG"]);
				Menge_FG = (dataRow["Menge_FG"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Menge_FG"]);
				Bez1_FG = (dataRow["Bez1_FG"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bez1_FG"]);
				Bez2_FG = (dataRow["Bez2_FG"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bez2_FG"]);
				Artikelnummer_ROH = (dataRow["Rohmaterial"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rohmaterial"]);
				Menge_ROH = (dataRow["Menge_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Menge_ROH"]);
				Bez1_ROH = (dataRow["Bez1_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bez1_ROH"]);
				Bez2_ROH = (dataRow["Bez2_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bez2_ROH"]);
				Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standardlieferant"]);
				Bestell_Nr_ROH = (dataRow["Bestell-Nr_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr_ROH"]);
				Einkaufspreis_ROH = (dataRow["Einkaufspreis_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einkaufspreis_ROH"]);
				Kupferzahl_ROH = (dataRow["Kupferzahl_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kupferzahl_ROH"]);
				Mindestbestellmenge_ROH = (dataRow["Mindestbestellmenge_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestellmenge_ROH"]);
				Wiederbeschaffungszeitraum_ROH = (dataRow["Wiederbeschaffungszeitraum_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Wiederbeschaffungszeitraum_ROH"]);
				UL_zertifiziert_ROH = (dataRow["UL zertifiziert_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UL zertifiziert_ROH"]);
				Bestand_ROH_CZ = (dataRow["Bestand_ROH_CZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_CZ"]);
				Bestand_ROH_TN = (dataRow["Bestand_ROH_TN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_TN"]);
				Bestand_ROH_AL = (dataRow["Bestand_ROH_AL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_AL"]);
				Bestand_ROH_WS = (dataRow["Bestand_ROH_WS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_WS"]);
				Bestand_ROH_BETN = (dataRow["Bestand_ROH_BETN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_BETN"]);
				Bestand_ROH_GZTN = (dataRow["Bestand_ROH_GZTN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_GZTN"]);
				Bestand_ROH_Obsolete = (dataRow["Bestand_ROH_Obsolete"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_Obsolete"]);
				Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen"]);
				Rahmen_Nr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr"]);
				Rahmenmenge = (dataRow["Rahmenmenge"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmenmenge"]);
				Rahmenauslauf = (dataRow["Rahmenauslauf"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmenauslauf"]);
				Mindestbestand_ROH_CZ = (dataRow["Mindestbestand_ROH_CZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_CZ"]);
				Mindestbestand_ROH_TN = (dataRow["Mindestbestand_ROH_TN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_TN"]);
				Mindestbestand_ROH_AL = (dataRow["Mindestbestand_ROH_AL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_AL"]);
				Mindestbestand_ROH_KHTN = (dataRow["Mindestbestand_ROH_KHTN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_KHTN"]);
				Mindestbestand_ROH_BETN = (dataRow["Mindestbestand_ROH_BETN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_BETN"]);
				Mindestbestand_ROH_GZTN = (dataRow["Mindestbestand_ROH_GZTN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_GZTN"]);
				Mindestbestand_ROH_Obsolete = (dataRow["Mindestbestand_ROH_Obsolete"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_Obsolete"]);
				ROH_Angebotsdatum = (dataRow["ROH_Angebotsdatum"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ROH_Angebotsdatum"]);
			}
		}
		// - 17
		public class ControllingAnalysis_SuperbillROHSum
		{
			public string Artikelnummer_ROH { get; set; }
			public string Menge_ROH { get; set; }
			public string Bez1_ROH { get; set; }
			public string Bez2_ROH { get; set; }
			public string Standardlieferant { get; set; }
			public string Bestell_Nr_ROH { get; set; }
			public string Einkaufspreis_ROH { get; set; }
			public string Kupferzahl_ROH { get; set; }
			public string Mindestbestellmenge_ROH { get; set; }
			public string Wiederbeschaffungszeitraum_ROH { get; set; }
			public string UL_zertifiziert_ROH { get; set; }
			public string Bestand_ROH_CZ { get; set; }
			public string Bestand_ROH_TN { get; set; }
			public string Bestand_ROH_AL { get; set; }
			public string Rahmen { get; set; }
			public string Rahmen_Nr { get; set; }
			public string Rahmenmenge { get; set; }
			public string Rahmenauslauf { get; set; }
			public string Mindestbestand_ROH_TN { get; set; }
			public string Mindestbestand_ROH_AL { get; set; }
			public string Mindestbestand_ROH_CZ { get; set; }
			public string Bestand_ROH_WS { get; set; }
			public string Mindestbestand_ROH_WS { get; set; }
			public string Bestand_ROH_BETN { get; set; }
			public string Mindestbestand_ROH_BETN { get; set; }
			public string Bestand_ROH_Obsolete { get; set; }
			public string Mindestbestand_ROH_Obsolete { get; set; }
			public string ROH_Angebotsdatum { get; set; }
			public string Bestand_ROH_GZTN { get; set; }
			public string Mindestbestand_ROH_GZTN { get; set; }
			public ControllingAnalysis_SuperbillROHSum() { }
			public ControllingAnalysis_SuperbillROHSum(DataRow dataRow)
			{
				Artikelnummer_ROH = (dataRow["Rohmaterial#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rohmaterial#"]);
				Menge_ROH = (dataRow["Menge_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Menge_ROH"]);
				Bez1_ROH = (dataRow["Bez1_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bez1_ROH"]);
				Bez2_ROH = (dataRow["Bez2_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bez2_ROH"]);
				Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standardlieferant"]);
				Bestell_Nr_ROH = (dataRow["Bestell-Nr_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr_ROH"]);
				Einkaufspreis_ROH = (dataRow["Einkaufspreis_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einkaufspreis_ROH"]);
				Kupferzahl_ROH = (dataRow["Kupferzahl_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kupferzahl_ROH"]);
				Mindestbestellmenge_ROH = (dataRow["Mindestbestellmenge_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestellmenge_ROH"]);
				Wiederbeschaffungszeitraum_ROH = (dataRow["Wiederbeschaffungszeitraum_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Wiederbeschaffungszeitraum_ROH"]);
				UL_zertifiziert_ROH = (dataRow["UL zertifiziert_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UL zertifiziert_ROH"]);
				Bestand_ROH_CZ = (dataRow["Bestand_ROH_CZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_CZ"]);
				Bestand_ROH_TN = (dataRow["Bestand_ROH_TN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_TN"]);
				Bestand_ROH_AL = (dataRow["Bestand_ROH_AL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_AL"]);
				Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen"]);
				Rahmen_Nr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr"]);
				Rahmenmenge = (dataRow["Rahmenmenge"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmenmenge"]);
				Rahmenauslauf = (dataRow["Rahmenauslauf"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmenauslauf"]);
				Mindestbestand_ROH_TN = (dataRow["Mindestbestand_ROH_TN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_TN"]);
				Mindestbestand_ROH_AL = (dataRow["Mindestbestand_ROH_AL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_AL"]);
				Mindestbestand_ROH_CZ = (dataRow["Mindestbestand_ROH_CZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_CZ"]);
				Bestand_ROH_WS = (dataRow["Bestand_ROH_WS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_WS"]);
				Mindestbestand_ROH_WS = (dataRow["Mindestbestand_ROH_WS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_WS"]);
				Bestand_ROH_BETN = (dataRow["Bestand_ROH_BETN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_BETN"]);
				Mindestbestand_ROH_BETN = (dataRow["Mindestbestand_ROH_BETN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_BETN"]);
				Bestand_ROH_Obsolete = (dataRow["Bestand_ROH_Obsolete"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_Obsolete"]);
				Mindestbestand_ROH_Obsolete = (dataRow["Mindestbestand_ROH_Obsolete"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_Obsolete"]);
				ROH_Angebotsdatum = (dataRow["ROH_Angebotsdatum"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ROH_Angebotsdatum"]);
				Bestand_ROH_GZTN = (dataRow["Bestand_ROH_GZTN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestand_ROH_GZTN"]);
				Mindestbestand_ROH_GZTN = (dataRow["Mindestbestand_ROH_GZTN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mindestbestand_ROH_GZTN"]);
			}
		}
		// - 18
		public class ControllingAnalysis_DBzuArtikel
		{
			public string Artikelnummer { get; set; }
			public string VerkauftArtikel { get; set; }
			public string DBI { get; set; }
			public ControllingAnalysis_DBzuArtikel() { }
			public ControllingAnalysis_DBzuArtikel(DataRow dataRow)
			{
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				VerkauftArtikel = (dataRow["verkaufte Artikel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["verkaufte Artikel"]);
				DBI = (dataRow["DB I"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DB I"]);
			}
		}
		// - 20, 21
		public class ControllingAnalysis_BestellungProDisponent
		{
			public string Nummer { get; set; }
			public string Disponent { get; set; }
			public string Lieferant { get; set; }
			public string AnzahlBestellungen { get; set; }
			public string SummePositionen { get; set; }
			public ControllingAnalysis_BestellungProDisponent() { }
			public ControllingAnalysis_BestellungProDisponent(DataRow dataRow)
			{
				Nummer = (dataRow["Nummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nummer"]);
				Disponent = (dataRow["Disponent"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Disponent"]);
				Lieferant = (dataRow["Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferant"]);
				AnzahlBestellungen = (dataRow["AnzahlBestellungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AnzahlBestellungen"]);
				SummePositionen = (dataRow["SummePositionen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SummePositionen"]);
			}
		}
		public class ControllingAnalysis_Rahmen
		{
			public int ArtikelNr { get; set; }
			public string ArtikelNummer { get; set; }
			public string Bezeichnung1 { get; set; }
			public string Bezeichnung2 { get; set; }
			public string Rahmen { get; set; }
			public string Rahmen_Nr { get; set; }
			public decimal Rahmenmenge { get; set; }
			public decimal Bestellt { get; set; }
			public decimal Rahmenrest { get; set; }
			public DateTime? Rahmenauslauf { get; set; }
			public ControllingAnalysis_Rahmen() { }
			public ControllingAnalysis_Rahmen(DataRow dataRow)
			{
				ArtikelNummer = (dataRow["ArtikelNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelNummer"]);
				Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Bezeichnung2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen"]);
				Rahmen_Nr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr"]);
				Rahmenmenge = Convert.ToDecimal(dataRow["Rahmenmenge"]);
				Bestellt = Convert.ToDecimal(dataRow["Bestellt"]);
				ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
				Rahmenrest = Convert.ToDecimal(dataRow["Rahmenrest"]);
				Rahmenauslauf = (dataRow["Rahmenauslauf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf"]);
			}
		}

		public class ControllingAnalysis_ProjektdatenDetails
		{
			public string Artikelnummer { get; set; }
			public DateTime? Erstanlage { get; set; }
			public string Kontakt_AV_PSZ { get; set; }
			public string Projekt_Nr { get; set; }

			public ControllingAnalysis_ProjektdatenDetails() { }

			public ControllingAnalysis_ProjektdatenDetails(DataRow dataRow)
			{
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Erstanlage = (dataRow["Erstanlage"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Erstanlage"]);
				Kontakt_AV_PSZ = (dataRow["Kontakt_AV_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_AV_PSZ"]);
				Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
			}
		}
		public class ControllingAnalysis_RohArticlesSuppliers
		{
			public string Angebot { get; set; }
			public DateTime? Angebot_Datum { get; set; }
			public string Artikelbezeichnung { get; set; }
			public string Artikelbezeichnung2 { get; set; }
			public string artikelklassifizierung { get; set; }
			public string Artikelnummer { get; set; }
			public decimal BedarfPO { get; set; }
			public string Bestell_Nr { get; set; }
			public decimal? Einkaufspreis { get; set; }
			public string ist_Priolieferant { get; set; }
			public string ist_Systemaktiv { get; set; }
			public decimal Last2YearsOrderQuantity { get; set; }
			public decimal LastYearsBookingQuantity { get; set; }
			public string Lieferant { get; set; }
			public int? lieferantennummer { get; set; }
			public Single? Mindestbestellmenge { get; set; }
			public string Status { get; set; }
			public string Stufe { get; set; }
			public Single? Verpackungseinheit { get; set; }
			public int? Wiederbeschaffungszeitraum { get; set; }
			public string Manufacturer { get; set; }
			public string ManufacturerNumber { get; set; }

			public ControllingAnalysis_RohArticlesSuppliers() { }

			public ControllingAnalysis_RohArticlesSuppliers(DataRow dataRow)
			{
				Angebot = (dataRow["Angebot"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot"]);
				Angebot_Datum = (dataRow["Angebot_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Angebot_Datum"]);
				Artikelbezeichnung = (dataRow["Artikelbezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung"]);
				Artikelbezeichnung2 = (dataRow["Artikelbezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung2"]);
				artikelklassifizierung = (dataRow["artikelklassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["artikelklassifizierung"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				BedarfPO = Convert.ToDecimal(dataRow["BedarfPO"]);
				Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
				Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
				ist_Priolieferant = Convert.ToString(dataRow["ist_Priolieferant"]);
				ist_Systemaktiv = Convert.ToString(dataRow["ist_Systemaktiv"]);
				Last2YearsOrderQuantity = Convert.ToDecimal(dataRow["Last2YearsOrderQuantity"]);
				LastYearsBookingQuantity = Convert.ToDecimal(dataRow["LastYearsBookingQuantity"]);
				Lieferant = (dataRow["Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferant"]);
				lieferantennummer = (dataRow["lieferantennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["lieferantennummer"]);
				Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Mindestbestellmenge"]);
				Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
				Stufe = (dataRow["Stufe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Stufe"]);
				Verpackungseinheit = (dataRow["Verpackungseinheit"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Verpackungseinheit"]);
				Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"]);
				Manufacturer = (dataRow["Manufacturer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Manufacturer"]);
				ManufacturerNumber = (dataRow["ManufacturerNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ManufacturerNumber"]);
			}
		}

		// - Technic
		public class Technic_PlanningOrder
		{
			public DateTime? AB_Termin { get; set; }
			public string Bemerkung_Technik { get; set; }
			public bool? Erstmuster { get; set; }
			public int? FA { get; set; }
			public bool? FA_Gestartet { get; set; }
			public string Info_CS { get; set; }
			public bool? Kabel_geschnitten { get; set; }
			public DateTime? Kabel_geschnitten_Datum { get; set; }
			public bool? Kommisioniert_komplett { get; set; }
			public bool? Kommisioniert_teilweise { get; set; }
			public int? Lagerort_id { get; set; }
			public int? Menge { get; set; }
			public int? Offen_Anzahl { get; set; }
			public DateTime? Plan { get; set; }
			public string Prufstatus_TN_Ware { get; set; }
			public string PSZ_ { get; set; }
			public bool? Quick_Area { get; set; }
			public bool? Sonderfertigung { get; set; }
			public string Status { get; set; }
			public string Status_intern { get; set; }
			public string Techniker { get; set; }
			public string Termin_besprochen { get; set; }
			public string Urs_Artikelnummer { get; set; }
			public Single? Zeit_in_min_pro_Stuck { get; set; }

			public Technic_PlanningOrder() { }

			public Technic_PlanningOrder(DataRow dataRow)
			{
				AB_Termin = (dataRow["AB_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AB_Termin"]);
				Bemerkung_Technik = (dataRow["Bemerkung_Technik"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Technik"]);
				Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
				FA = (dataRow["FA"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA"]);
				FA_Gestartet = (dataRow["FA_Gestartet"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FA_Gestartet"]);
				Info_CS = (dataRow["Info CS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Info CS"]);
				Kabel_geschnitten = (dataRow["Kabel_geschnitten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kabel_geschnitten"]);
				Kabel_geschnitten_Datum = (dataRow["Kabel_geschnitten_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Kabel_geschnitten_Datum"]);
				Kommisioniert_komplett = (dataRow["Kommisioniert_komplett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_komplett"]);
				Kommisioniert_teilweise = (dataRow["Kommisioniert_teilweise"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_teilweise"]);
				Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
				Menge = (dataRow["Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge"]);
				Offen_Anzahl = (dataRow["Offen_Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Offen_Anzahl"]);
				Plan = (dataRow["Plan"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Plan"]);
				Prufstatus_TN_Ware = (dataRow["Prüfstatus TN Ware"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prüfstatus TN Ware"]);
				PSZ_ = (dataRow["PSZ#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ#"]);
				Quick_Area = (dataRow["Quick_Area"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Quick_Area"]);
				Sonderfertigung = (dataRow["Sonderfertigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Sonderfertigung"]);
				Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
				Status_intern = (dataRow["Status intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status intern"]);
				Techniker = (dataRow["Techniker"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Techniker"]);
				Termin_besprochen = Convert.ToString(dataRow["Termin besprochen"]);
				Urs_Artikelnummer = (dataRow["Urs-Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Urs-Artikelnummer"]);
				Zeit_in_min_pro_Stuck = (dataRow["Zeit in min pro Stück"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Zeit in min pro Stück"]);
			}
		}
		public class TechnicTechnikerEntity
		{
			public int ID { get; set; }
			public string Name { get; set; }

			public TechnicTechnikerEntity() { }

			public TechnicTechnikerEntity(DataRow dataRow)
			{
				ID = Convert.ToInt32(dataRow["ID"]);
				Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			}
		}
		public class Technic_PSZ_Projektdaten_DetailsEntity
		{
			public DateTime? AB_Datum { get; set; }
			public int? Arbeitszeit_Serien_Pro_Kabesatz { get; set; }
			public string Artikelnummer { get; set; }
			public string Bemerkungen { get; set; }
			public int? EAU { get; set; }
			public string EMPB { get; set; }
			public DateTime? Erstanlage { get; set; }
			public DateTime? FA_Datum { get; set; }
			public int ID { get; set; }
			public string Kontakt_AV_PSZ { get; set; }
			public string Kontakt_CS_PSZ { get; set; }
			public string Kontakt_Technik_Kunde { get; set; }
			public string Kontakt_Technik_PSZ { get; set; }
			public int? Kosten { get; set; }
			public string Krimp_WKZ { get; set; }
			public string Material_Eskalation_AV { get; set; }
			public string Material_Eskalation_Termin { get; set; }
			public string Material_Komplett { get; set; }
			public int? Menge { get; set; }
			public int? MOQ { get; set; }
			public string Projekt_betreung { get; set; }
			public string Projekt_Start { get; set; }
			public bool? Projektmeldung { get; set; }
			public string Projekt_Nr { get; set; }
			public string Rapid_Prototyp { get; set; }
			public string Serie_PSZ { get; set; }
			public string SG_WKZ { get; set; }
			public string Standort_Muster { get; set; }
			public string Standort_Serie { get; set; }
			public string Summe_Arbeitszeit { get; set; }
			public string Termin_mit_Technik_abgesprochen { get; set; }
			public string TSP_Kunden { get; set; }
			public string Typ { get; set; }
			public string UL_Verpackung { get; set; }
			public DateTime? Wunschtermin_Kunde { get; set; }
			public string Zuschlag { get; set; }

			public Technic_PSZ_Projektdaten_DetailsEntity() { }

			public Technic_PSZ_Projektdaten_DetailsEntity(DataRow dataRow)
			{
				AB_Datum = (dataRow["AB_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AB_Datum"]);
				Arbeitszeit_Serien_Pro_Kabesatz = (dataRow["Arbeitszeit Serien Pro Kabesatz"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Arbeitszeit Serien Pro Kabesatz"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
				EAU = (dataRow["EAU"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EAU"]);
				EMPB = (dataRow["EMPB"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EMPB"]);
				Erstanlage = (dataRow["Erstanlage"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Erstanlage"]);
				FA_Datum = (dataRow["FA_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_Datum"]);
				ID = Convert.ToInt32(dataRow["ID"]);
				Kontakt_AV_PSZ = (dataRow["Kontakt_AV_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_AV_PSZ"]);
				Kontakt_CS_PSZ = (dataRow["Kontakt_CS_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_CS_PSZ"]);
				Kontakt_Technik_Kunde = (dataRow["Kontakt_Technik_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_Technik_Kunde"]);
				Kontakt_Technik_PSZ = (dataRow["Kontakt_Technik_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_Technik_PSZ"]);
				Kosten = (dataRow["Kosten"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kosten"]);
				Krimp_WKZ = (dataRow["Krimp_WKZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Krimp_WKZ"]);
				Material_Eskalation_AV = (dataRow["Material_Eskalation_AV"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Eskalation_AV"]);
				Material_Eskalation_Termin = (dataRow["Material_Eskalation_Termin"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Eskalation_Termin"]);
				Material_Komplett = (dataRow["Material_Komplett"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Komplett"]);
				Menge = (dataRow["Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge"]);
				MOQ = (dataRow["MOQ"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MOQ"]);
				Projekt_betreung = (dataRow["Projekt betreung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt betreung"]);
				Projekt_Start = (dataRow["Projekt_Start"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt_Start"]);
				Projektmeldung = (dataRow["Projektmeldung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Projektmeldung"]);
				Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
				Rapid_Prototyp = (dataRow["Rapid Prototyp"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rapid Prototyp"]);
				Serie_PSZ = (dataRow["Serie_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Serie_PSZ"]);
				SG_WKZ = (dataRow["SG_WKZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SG_WKZ"]);
				Standort_Muster = (dataRow["Standort_Muster"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standort_Muster"]);
				Standort_Serie = (dataRow["Standort_Serie"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standort_Serie"]);
				Summe_Arbeitszeit = (dataRow["Summe Arbeitszeit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Summe Arbeitszeit"]);
				Termin_mit_Technik_abgesprochen = (dataRow["Termin mit Technik abgesprochen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Termin mit Technik abgesprochen"]);
				TSP_Kunden = (dataRow["TSP Kunden"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TSP Kunden"]);
				Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
				UL_Verpackung = (dataRow["UL_Verpackung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UL_Verpackung"]);
				Wunschtermin_Kunde = (dataRow["Wunschtermin_Kunde"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Wunschtermin_Kunde"]);
				Zuschlag = (dataRow["Zuschlag"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zuschlag"]);
			}
		}
		public class Technic_QuickAreaBestand
		{
			public string Artikel_Artikelnummer { get; set; }
			public int? Artikel_Nr_des_Bauteils { get; set; }
			public string Artikelnummer { get; set; }
			public string Artikelnummer_Stucklisten { get; set; }
			public string Bezeichnung_1 { get; set; }
			public int? Fertigungsnummer { get; set; }
			public decimal? SummevonBestand { get; set; }
			public decimal? SummevonBruttobedarf { get; set; }
			public decimal? Verfugbar { get; set; }

			public Technic_QuickAreaBestand() { }

			public Technic_QuickAreaBestand(DataRow dataRow)
			{
				Artikel_Artikelnummer = (dataRow["Artikel_Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikel_Artikelnummer"]);
				Artikel_Nr_des_Bauteils = (dataRow["Artikel-Nr des Bauteils"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr des Bauteils"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Artikelnummer_Stucklisten = (dataRow["Artikelnummer_Stücklisten"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer_Stücklisten"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
				SummevonBestand = (dataRow["SummevonBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SummevonBestand"]);
				SummevonBruttobedarf = (dataRow["SummevonBruttobedarf"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SummevonBruttobedarf"]);
				Verfugbar = (dataRow["Verfügbar"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verfügbar"]);
			}
		}
		public class Technic_ProdTN
		{
			public int? AnzahlvonFertigungsnummer { get; set; }
			public string Artikelnummer { get; set; }
			public int? ArtikelNr { get; set; }
			public string Bezeichnung_1 { get; set; }
			public string Bezeichnung_3 { get; set; }
			public decimal? Kennzahl { get; set; }
			public Single? Produktionszeit { get; set; }
			public decimal? Produktivität { get; set; }
			public decimal? Stundensatz { get; set; }

			public Technic_ProdTN() { }

			public Technic_ProdTN(DataRow dataRow)
			{
				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				AnzahlvonFertigungsnummer = (dataRow["AnzahlvonFertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AnzahlvonFertigungsnummer"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Bezeichnung_3 = (dataRow["Bezeichnung 3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 3"]);
				Kennzahl = (dataRow["Kennzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kennzahl"]);
				Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Produktionszeit"]);
				Produktivität = (dataRow["Produktivität"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Produktivität"]);
				Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
			}
		}
		public class ControllingAnalysis_InventurROH
		{
			public string Artikelnummer { get; set; }
			public decimal? Bestand { get; set; }
			public string Bestell_Nr { get; set; }
			public string Bezeichnung_1 { get; set; }
			public double? EK { get; set; }
			public double? EK_Summe { get; set; }
			public double? Gesamtgewicht { get; set; }
			public Single? Gewicht { get; set; }
			public string Lagerort { get; set; }
			public int? Lagerort_id { get; set; }
			public int? Lieferanten_Nr { get; set; }
			public string Name1 { get; set; }
			public string Ursprungsland { get; set; }
			public string Zolltarif_nr { get; set; }

			public ControllingAnalysis_InventurROH() { }

			public ControllingAnalysis_InventurROH(DataRow dataRow)
			{
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
				Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				EK = (dataRow["EK"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["EK"]);
				EK_Summe = (dataRow["EK_Summe"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["EK_Summe"]);
				Gesamtgewicht = (dataRow["Gesamtgewicht"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Gesamtgewicht"]);
				Gewicht = (dataRow["Gewicht"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Gewicht"]);
				Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
				Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
				Lieferanten_Nr = (dataRow["Lieferanten-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferanten-Nr"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
				Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
			}
		}
		public class ControllingAnalysis_VKSimulationInData
		{
			public decimal? Anteilberechen { get; set; }
			public int Artikel_Nr { get; set; }
			public string Artikelnummer { get; set; }
			public string Bezeichnung_1 { get; set; }
			public string Bezeichnung_2 { get; set; }
			public int? DEL { get; set; }
			public string Prozent { get; set; }
			public int Update { get; set; }
			public decimal? VK { get; set; }
			public decimal? VK_Simulation { get; set; }
			public decimal? VKCU { get; set; }
			public decimal? VKCU_Simulation { get; set; }

			public ControllingAnalysis_VKSimulationInData() { }

			public ControllingAnalysis_VKSimulationInData(DataRow dataRow)
			{
				Anteilberechen = (dataRow["Anteilberechen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anteilberechen"]);
				Artikel_Nr = Convert.ToInt32(dataRow["Artikel-Nr"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				DEL = (dataRow["DEL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DEL"]);
				Prozent = (dataRow["Prozent"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prozent"]);
				Update = Convert.ToInt32(dataRow["Update"]);
				VK = (dataRow["VK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK"]);
				VK_Simulation = (dataRow["VK_Simulation"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK_Simulation"]);
				VKCU = (dataRow["VKCU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VKCU"]);
				VKCU_Simulation = (dataRow["VKCU_Simulation"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VKCU_Simulation"]);
			}
		}
		public class ControllingAnalysis_VKSimulationStffelPreis
		{
			public string Artikelnummer { get; set; }
			public decimal? Staffelpreis1 { get; set; }
			public decimal? Staffelpreis2 { get; set; }
			public decimal? Staffelpreis3 { get; set; }
			public decimal? Staffelpreis4 { get; set; }
			public decimal? VK { get; set; }
			public decimal? VK_Simulation { get; set; }

			public ControllingAnalysis_VKSimulationStffelPreis() { }

			public ControllingAnalysis_VKSimulationStffelPreis(DataRow dataRow)
			{
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Staffelpreis1 = (dataRow["Staffelpreis1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis1"]);
				Staffelpreis2 = (dataRow["Staffelpreis2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis2"]);
				Staffelpreis3 = (dataRow["Staffelpreis3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis3"]);
				Staffelpreis4 = (dataRow["Staffelpreis4"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis4"]);
				VK = (dataRow["VK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK"]);
				VK_Simulation = (dataRow["VK_Simulation"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK_Simulation"]);
			}
		}

		// - CS
		public class CS_PlanningTechnicOrder
		{
			public DateTime? AB_Termin { get; set; }
			public string Bemerkung_Technik { get; set; }
			public string CS_Kontakt { get; set; }
			public bool? EM { get; set; }
			public int? FA { get; set; }
			public int? Fertigung { get; set; }
			public string Info_CS { get; set; }
			public string Kundencode { get; set; }
			public int? Menge { get; set; }
			public DateTime? Plan { get; set; }
			public string Planungsstatus { get; set; }
			public string PSZ_ { get; set; }
			public string Status { get; set; }
			public string Techniker { get; set; }
			public Single? Zeit_in_min_pro_Stuck { get; set; }

			public CS_PlanningTechnicOrder() { }

			public CS_PlanningTechnicOrder(DataRow dataRow)
			{
				AB_Termin = (dataRow["AB_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AB_Termin"]);
				Bemerkung_Technik = (dataRow["Bemerkung_Technik"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Technik"]);
				CS_Kontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
				EM = (dataRow["EM"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EM"]);
				FA = (dataRow["FA"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA"]);
				Fertigung = (dataRow["Fertigung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigung"]);
				Info_CS = (dataRow["Info CS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Info CS"]);
				Kundencode = (dataRow["Kundencode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundencode"]);
				Menge = (dataRow["Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge"]);
				Plan = (dataRow["Plan"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Plan"]);
				Planungsstatus = (dataRow["Planungsstatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Planungsstatus"]);
				PSZ_ = (dataRow["PSZ#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ#"]);
				Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
				Techniker = (dataRow["Techniker"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Techniker"]);
				Zeit_in_min_pro_Stuck = (dataRow["Zeit in min pro Stück"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Zeit in min pro Stück"]);
			}
		}
		public class CS_StatusPArticle
		{
			public string CS_Kontakt { get; set; }
			public string Freigabestatus { get; set; }
			public string Index { get; set; }
			public DateTime? Index_Datum { get; set; }
			public string Kunde { get; set; }
			public string Kunde_ { get; set; }
			public string PSZ { get; set; }

			public CS_StatusPArticle() { }

			public CS_StatusPArticle(DataRow dataRow)
			{
				CS_Kontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
				Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
				Index = (dataRow["Index"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index"]);
				Index_Datum = (dataRow["Index_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Datum"]);
				Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
				Kunde_ = (dataRow["Kunde#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde#"]);
				PSZ = (dataRow["PSZ#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ#"]);
			}
		}
		public class CS_RepairEvaluation
		{
			public string Bezeichnung_1 { get; set; }
			public string Bezeichnung_2 { get; set; }
			public int? Fertigungsnummer { get; set; }
			public string Kunde { get; set; }
			public int? Menge_Offen { get; set; }
			public string Nummerschlussel { get; set; }
			public int? Originalanzahl { get; set; }
			public int? Produktionsort_ID { get; set; }
			public string Urs_Artikelnummer { get; set; }

			public CS_RepairEvaluation() { }

			public CS_RepairEvaluation(DataRow dataRow)
			{
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
				Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
				Menge_Offen = (dataRow["Menge Offen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge Offen"]);
				Nummerschlussel = (dataRow["Nummerschlüssel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nummerschlüssel"]);
				Originalanzahl = (dataRow["Originalanzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Originalanzahl"]);
				Produktionsort_ID = (dataRow["Produktionsort ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Produktionsort ID"]);
				Urs_Artikelnummer = (dataRow["Urs-Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Urs-Artikelnummer"]);
			}
		}
		public class CS_AVEvaluation
		{
			public string Anderung_von { get; set; }
			public string Anderungsbeschreibung { get; set; }
			public string Artikelnummer { get; set; }
			public int? ArtikelNr { get; set; }
			public string Bezeichnung_1 { get; set; }
			public DateTime? Datum_Anderung { get; set; }
			public decimal? DB_I_ohne { get; set; }
			public string Einheit { get; set; }
			public decimal? Einkaufspreis { get; set; }
			public Single? Exportgewicht { get; set; }
			public decimal? Gewicht { get; set; }
			public Single? Groesse { get; set; }
			public int? Losgroesse { get; set; }
			public string Name1 { get; set; }
			public decimal? Preiseinheit { get; set; }
			public decimal? Preisgruppen_Einkaufspreis { get; set; }
			public Single? Produktionszeit { get; set; }
			public bool? Standardlieferant { get; set; }
			public decimal? Stundensatz { get; set; }
			public decimal? Umsatzsteuer { get; set; }
			public decimal? Verkaufspreis { get; set; }
			public string Verpackungsart { get; set; }
			public int? Verpackungsmenge { get; set; }
			public string Warengruppe { get; set; }
			public int? Warentyp { get; set; }
			public string Zolltarif_nr { get; set; }

			public CS_AVEvaluation() { }

			public CS_AVEvaluation(DataRow dataRow)
			{
				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				Anderung_von = (dataRow["Änderung von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Änderung von"]);
				Anderungsbeschreibung = (dataRow["Änderungsbeschreibung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Änderungsbeschreibung"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Datum_Anderung = (dataRow["Datum Änderung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum Änderung"]);
				DB_I_ohne = (dataRow["DB I ohne"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB I ohne"]);
				Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
				Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
				Exportgewicht = (dataRow["Exportgewicht"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Exportgewicht"]);
				Gewicht = (dataRow["Gewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gewicht"]);
				Groesse = (dataRow["Größe"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Größe"]);
				Losgroesse = (dataRow["Losgroesse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Losgroesse"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
				Preisgruppen_Einkaufspreis = (dataRow["Preisgruppen_Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preisgruppen_Einkaufspreis"]);
				Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Produktionszeit"]);
				Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Standardlieferant"]);
				Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
				Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Umsatzsteuer"]);
				Verkaufspreis = (dataRow["Verkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verkaufspreis"]);
				Verpackungsart = (dataRow["Verpackungsart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungsart"]);
				Verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verpackungsmenge"]);
				Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
				Warentyp = (dataRow["Warentyp"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Warentyp"]);
				Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
			}
		}
		public class CS_StockStatus
		{
			#region // - Article - //
			public bool? aktiv { get; set; }
			public int ArtikelNr { get; set; }
			public string ArtikelNummer { get; set; }
			public string Bezeichnung1 { get; set; }
			public int? CustomerNumber { get; set; }
			public string CustomerItemNumber { get; set; }
			public string Index_Kunde { get; set; }
			public DateTime? Index_Kunde_Datum { get; set; }
			public bool? EdiDefault { get; set; }
			public string ProductionCountryCode { get; set; }
			public string ProductionCountryName { get; set; }
			public string ProductionSiteCode { get; set; }
			public string ProductionSiteName { get; set; }
			// - 2022-09-05
			public bool UBG { get; set; }
			#endregion


			#region // - Lager - //
			public decimal? Bestand { get; set; }
			public decimal? Bestand_reserviert { get; set; }
			public int ID { get; set; }
			public int? Lagerort_id { get; set; }
			public DateTime? letzte_Bewegung { get; set; }
			public decimal? Mindestbestand { get; set; }
			#endregion

			public decimal FaNegativeQuantity { get; set; }
			public decimal FaPositiveQuantity { get; set; }
			public decimal AbQuantity { get; set; }

			public CS_StockStatus() { }

			public CS_StockStatus(DataRow dataRow)
			{
				#region // - Article - //

				aktiv = (dataRow["aktiv"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["aktiv"]);
				ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
				ArtikelNummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
				Index_Kunde_Datum = (dataRow["Index_Kunde_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Kunde_Datum"]);
				EdiDefault = (dataRow["EdiDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EdiDefault"]);

				// 2022-07-06
				CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
				CustomerItemNumber = (dataRow["CustomerItemNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerItemNumber"]);
				ProductionCountryCode = (dataRow["ProductionCountryCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionCountryCode"]);
				ProductionCountryName = (dataRow["ProductionCountryName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionCountryName"]);
				ProductionSiteCode = (dataRow["ProductionSiteCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionSiteCode"]);
				ProductionSiteName = (dataRow["ProductionSiteName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionSiteName"]);
				// - 2022-09-05
				UBG = Convert.ToBoolean(dataRow["UBG"]);
				#endregion

				#region // - Lager - //
				Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
				Bestand_reserviert = (dataRow["Bestand_reserviert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand_reserviert"]);
				ID = Convert.ToInt32(dataRow["ID"]);
				Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
				letzte_Bewegung = (dataRow["letzte Bewegung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["letzte Bewegung"]);
				Mindestbestand = (dataRow["Mindestbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestand"]);
				#endregion

				AbQuantity = Convert.ToDecimal(dataRow["nbAb"]);
				FaNegativeQuantity = Convert.ToDecimal(dataRow["nbFa"]);
				FaPositiveQuantity = Convert.ToDecimal(dataRow["nbFaPositive"]);
			}
		}

		// - Basics
		public class BS_Mindesbestand
		{
			public decimal? Aktueller_Bestand { get; set; }
			public decimal? Bestandskosten { get; set; }
			public string Bezeichnung { get; set; }
			public decimal? EK { get; set; }
			public string Lagerort { get; set; }
			public decimal? Mindestbestand { get; set; }
			public decimal? Mindestbestandskosten { get; set; }
			public string PSZ_Nummer { get; set; }
			public int? ArtikleNr { get; set; }

			public BS_Mindesbestand() { }

			public BS_Mindesbestand(DataRow dataRow)
			{
				ArtikleNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				Aktueller_Bestand = (dataRow["Aktueller Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Aktueller Bestand"]);
				Bestandskosten = (dataRow["Bestandskosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestandskosten"]);
				Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
				EK = (dataRow["EK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK"]);
				Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
				Mindestbestand = (dataRow["Mindestbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestand"]);
				Mindestbestandskosten = (dataRow["Mindestbestandskosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestandskosten"]);
				PSZ_Nummer = (dataRow["PSZ Nummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ Nummer"]);
			}
		}
		public class BS_Umlauf
		{
			public decimal? Anzahl { get; set; }
			public string Artikelnummer_Umlauf { get; set; }
			public decimal? Bestand { get; set; }
			public DateTime? Bestatigter_Termin { get; set; }
			public int? Bestellung_Nr { get; set; }
			public DateTime? Liefertermin { get; set; }
			public int? SummevonBedarf { get; set; }
			public decimal? Transfer_Bestand { get; set; }

			public BS_Umlauf() { }

			public BS_Umlauf(DataRow dataRow)
			{
				Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
				Artikelnummer_Umlauf = (dataRow["Artikelnummer_Umlauf"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer_Umlauf"]);
				Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
				Bestatigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
				Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
				Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
				SummevonBedarf = (dataRow["SummevonBedarf"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SummevonBedarf"]);
				Transfer_Bestand = (dataRow["Transfer Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Transfer Bestand"]);
			}
		}
		public class BS_MaterialBestandProd
		{
			public string Artikelnummer { get; set; }
			public int? ArtikleNr { get; set; }
			public decimal? Bestand { get; set; }
			public decimal? Im_Lager { get; set; }
			public decimal? In_Produktion { get; set; }

			public BS_MaterialBestandProd() { }

			public BS_MaterialBestandProd(DataRow dataRow)
			{
				ArtikleNr = (dataRow["ArtikleNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArtikleNr"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
				Im_Lager = (dataRow["Im_Lager"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Im_Lager"]);
				In_Produktion = (dataRow["In_Produktion"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["In_Produktion"]);
			}
		}
		public class BS_MaterialBestandAnalyse
		{
			public decimal? Abweichung { get; set; }
			public string Artikelnummer { get; set; }
			public int? ArtikelNr { get; set; }
			public decimal? Bedarf_nachste_8_Wochen { get; set; }
			public decimal? Bestand { get; set; }
			public decimal? Empfohlener_Mindestbestand { get; set; }
			public decimal? Mindestbestand { get; set; }
			public int? Wiederbeschaffungszeitraum { get; set; }

			public BS_MaterialBestandAnalyse() { }

			public BS_MaterialBestandAnalyse(DataRow dataRow)
			{
				//[Artikel-Nr]
				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				Abweichung = (dataRow["Abweichung"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Abweichung"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bedarf_nachste_8_Wochen = (dataRow["Bedarf_nächste_8_Wochen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bedarf_nächste_8_Wochen"]);
				Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
				Empfohlener_Mindestbestand = (dataRow["Empfohlener_Mindestbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Empfohlener_Mindestbestand"]);
				Mindestbestand = (dataRow["Mindestbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestand"]);
				Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"]);
			}
		}

		public class BS_OFFeneFaEsd
		{
			public string Artikelnummer { get; set; }
			public string ESD_ARTIKEL { get; set; }
			public int? ArtikelNr { get; set; }
			public int? Fertigungsnummer { get; set; }
			public string Kennzeichen { get; set; }
			public int? Lagerort_id { get; set; }
			public DateTime? Termin_Bestatigt1 { get; set; }

			public BS_OFFeneFaEsd() { }

			public BS_OFFeneFaEsd(DataRow dataRow)
			{
				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				ESD_ARTIKEL = (dataRow["ESD_ARTIKEL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ESD_ARTIKEL"]);
				Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
				Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
				Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
				Termin_Bestatigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			}
		}
		public class BS_ProjektartArtikel
		{
			public string artikelklassifizierung { get; set; }
			public string Artikelnummer { get; set; }
			public int? ArtikelNr { get; set; }
			public string Freigabestatus { get; set; }
			public string Warengruppe { get; set; }
			public string Zeichnungsnummer { get; set; }

			public BS_ProjektartArtikel() { }

			public BS_ProjektartArtikel(DataRow dataRow)
			{
				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				artikelklassifizierung = (dataRow["artikelklassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["artikelklassifizierung"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
				Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
				Zeichnungsnummer = (dataRow["Zeichnungsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zeichnungsnummer"]);
			}
		}
		public class BS_ProofOfUsage
		{
			public Single? Anzahl { get; set; }
			public string Artikelnummer { get; set; }
			public int? ArtikelNr { get; set; }
			public string Bezeichnung_1 { get; set; }
			public string Bezeichnung_2 { get; set; }
			public string Freigabestatus { get; set; }
			public string Position { get; set; }
			public bool? Rahmen { get; set; }
			public DateTime? Rahmenauslauf { get; set; }
			public Single? Rahmenmenge { get; set; }
			public string Rahmen_Nr { get; set; }
			public string Sysmonummer { get; set; }
			public string Variante { get; set; }

			public BS_ProofOfUsage() { }

			public BS_ProofOfUsage(DataRow dataRow)
			{
				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Anzahl"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
				Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
				Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
				Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen"]);
				Rahmenauslauf = (dataRow["Rahmenauslauf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf"]);
				Rahmenmenge = (dataRow["Rahmenmenge"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rahmenmenge"]);
				Rahmen_Nr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr"]);
				Sysmonummer = (dataRow["Sysmonummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sysmonummer"]);
				Variante = (dataRow["Variante"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Variante"]);
			}
		}
		public class BS_Bedarf
		{

			public BS_Bedarf() { }

			public BS_Bedarf(DataRow dataRow)
			{
			}

			#region temp tables
			public class RST
			{
				public double? Anzahl_F { get; set; }
				public string Artikel_Bau { get; set; }
				public string Artikel_H { get; set; }
				public int? Artikel_Nr { get; set; }
				public int? Artikel_Nr_H { get; set; }
				public string Bezeichnung_1 { get; set; }
				public string Bezeichnung_D { get; set; }
				public string Bezeichnung_H { get; set; }
				public double? Bruttobedarf { get; set; }
				public int? Fertigungsnummer { get; set; }
				public string Freigabestatus { get; set; }
				public string FreigabestatusTN_Int { get; set; }
				public bool? Gestart { get; set; }
				public bool? Kabel_geschnitten { get; set; }
				public bool? Kommisioniert_komplett { get; set; }
				public bool? Kommisioniert_teilweise { get; set; }
				public int? Lagerort_id { get; set; }
				public double Reserviert_Menge { get; set; }
				public double? St_Anzahl { get; set; }
				public DateTime? Termin_Bestätigt1 { get; set; }
				public DateTime? Termin_Fertigstellung { get; set; }
				public DateTime? Termin_Materialbedarf { get; set; }
				public double? Verfug_Ini { get; set; }

				public RST() { }

				public RST(DataRow dataRow)
				{
					Anzahl_F = (dataRow["Anzahl_F"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Anzahl_F"]);
					Artikel_Bau = (dataRow["Artikel_Bau"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikel_Bau"]);
					Artikel_H = (dataRow["Artikel_H"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikel_H"]);
					Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
					//Artikel_Nr_des_Bauteils = (dataRow["Artikel-Nr des Bauteils"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr des Bauteils"]);
					//Bezeichnung_des_Bauteils = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung des Bauteils"]);
					Bezeichnung_D = (dataRow["Bezeichnung_D"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung_D"]);
					Bezeichnung_H = (dataRow["Bezeichnung_H"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung_H"]);
					Bruttobedarf = (dataRow["Bruttobedarf"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Bruttobedarf"]);
					Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
					Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
					FreigabestatusTN_Int = (dataRow["FreigabestatusTN_Int"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FreigabestatusTN_Int"]);
					Gestart = (dataRow["Gestart"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Gestart"]);
					Kabel_geschnitten = (dataRow["Kabel_geschnitten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kabel_geschnitten"]);
					Kommisioniert_komplett = (dataRow["Kommisioniert_komplett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_komplett"]);
					Kommisioniert_teilweise = (dataRow["Kommisioniert_teilweise"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_teilweise"]);
					Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
					Reserviert_Menge = Convert.ToInt32(dataRow["Reserviert_Menge"]);
					St_Anzahl = (dataRow["St_Anzahl"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["St_Anzahl"]);
					Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
					Termin_Fertigstellung = (dataRow["Termin_Fertigstellung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Fertigstellung"]);
					Termin_Materialbedarf = (dataRow["Termin_Materialbedarf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Materialbedarf"]);
					Verfug_Ini = (dataRow["Verfug_Ini"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Verfug_Ini"]);
				}
			}
			public class RST1
			{
				public int? Artikel_Nr { get; set; }
				public string B1 { get; set; }
				public int BW { get; set; }
				public string F1 { get; set; }
				public int Lief_Nr { get; set; }
				public Single M1 { get; set; }
				public string N1 { get; set; }
				public double P1 { get; set; }
				public bool? St1 { get; set; }
				public string T1 { get; set; }

				public RST1() { }

				public RST1(DataRow dataRow)
				{
					Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
					B1 = Convert.ToString(dataRow["B1"]);
					BW = Convert.ToInt32(dataRow["BW"]);
					F1 = Convert.ToString(dataRow["F1"]);
					Lief_Nr = Convert.ToInt32(dataRow["Lief_Nr"]);
					M1 = Convert.ToSingle(dataRow["M1"]);
					N1 = Convert.ToString(dataRow["N1"]);
					P1 = Convert.ToDouble(dataRow["P1"]);
					St1 = (dataRow["St1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["St1"]);
					T1 = Convert.ToString(dataRow["T1"]);
				}
			}
			public class RST2
			{
				public string AB_L { get; set; }
				public double? Anzahl1 { get; set; }
				public string Bemerk { get; set; }
				public DateTime? Bestätigter_Termin { get; set; }
				public int? Lief_Nr { get; set; }
				public DateTime? Liefertermin { get; set; }
				public string Projekt_Nr { get; set; }
				public string VornameFirma { get; set; }

				public RST2() { }

				public RST2(DataRow dataRow)
				{
					AB_L = Convert.ToString(dataRow["AB_L"]);
					Anzahl1 = (dataRow["Anzahl1"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Anzahl1"]);
					Bemerk = Convert.ToString(dataRow["Bemerk"]);
					Bestätigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
					Lief_Nr = (dataRow["Lief_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lief_Nr"]);
					Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
					Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
					VornameFirma = (dataRow["VornameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["VornameFirma"]);
				}
			}
			public class RST3
			{
				public double? Bestand { get; set; }

				public RST3() { }

				public RST3(DataRow dataRow)
				{
					Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Bestand"]);
				}
			}
			public class Disposition_Bedarf
			{
				public decimal? Anzahl { get; set; }
				public string ArtikelNummer { get; set; }
				public decimal? Bedarf_FA { get; set; }
				public decimal? Bedarf_Summiert { get; set; }
				public string Bezeichnung { get; set; }
				public string Bezeichnung_des_Bauteils { get; set; }
				public decimal? FA_Offen { get; set; }
				public string Fertigung { get; set; }
				public bool? Gestart { get; set; }
				public bool? Kabel_geschnitten { get; set; }
				public bool? Kommisioniert_komplett { get; set; }
				public bool? Kommisioniert_teilweise { get; set; }
				public decimal? Reserviert_Menge { get; set; }
				public string S_Intern { get; set; }
				public string S_Extetrn { get; set; }
				public string Stücklisten_Artikelnummer { get; set; }
				public DateTime? Termin_Bestatigen { get; set; }
				public DateTime? Termin_MA { get; set; }
				public decimal? Verfug_Ini { get; set; }
				public decimal? Verfügbar { get; set; }
				public DateTime? CreateDateFA { get; set; } // - 2025-02-10 - AK - show Fa Date

				public Disposition_Bedarf() { }

				public Disposition_Bedarf(DataRow dataRow)
				{
					Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
					ArtikelNummer = (dataRow["ArtikelNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelNummer"]);
					Bedarf_FA = (dataRow["Bedarf_FA"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bedarf_FA"]);
					Bedarf_Summiert = (dataRow["Bedarf_Summiert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bedarf_Summiert"]);
					Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
					Bezeichnung_des_Bauteils = (dataRow["Bezeichnung_des_Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung_des_Bauteils"]);
					FA_Offen = (dataRow["FA_Offen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["FA_Offen"]);
					Fertigung = (dataRow["Fertigung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fertigung"]);
					Gestart = (dataRow["Gestart"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Gestart"]);
					Kabel_geschnitten = (dataRow["Kabel_geschnitten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kabel_geschnitten"]);
					Kommisioniert_komplett = (dataRow["Kommisioniert_komplett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_komplett"]);
					Kommisioniert_teilweise = (dataRow["Kommisioniert_teilweise"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_teilweise"]);
					Reserviert_Menge = (dataRow["Reserviert_Menge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Reserviert_Menge"]);
					S_Intern = (dataRow["S_Intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["S_Intern"]);
					S_Extetrn = (dataRow["S-Extetrn"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["S-Extetrn"]);
					Stücklisten_Artikelnummer = (dataRow["Stücklisten_Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Stücklisten_Artikelnummer"]);
					Termin_Bestatigen = (dataRow["Termin_Bestatigen"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestatigen"]);
					Termin_MA = (dataRow["Termin_MA"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_MA"]);
					Verfug_Ini = (dataRow["Verfug_Ini"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verfug_Ini"]);
					Verfügbar = (dataRow["Verfügbar"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verfügbar"]);
				}
			}
			public class Disposition_Liferant
			{
				public int? Artikel_Nr { get; set; }
				public string Bestell_Nr { get; set; }
				public string Fax { get; set; }
				public int? Lief_Nr { get; set; }
				public string Lieferant { get; set; }
				public int? LT { get; set; }
				public decimal? MQO { get; set; }
				public decimal? Peis { get; set; }
				public bool? Standar_Liferent { get; set; }
				public string Telefon { get; set; }

				public Disposition_Liferant() { }

				public Disposition_Liferant(DataRow dataRow)
				{
					Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
					Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
					Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
					Lief_Nr = (dataRow["Lief_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lief_Nr"]);
					Lieferant = (dataRow["Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferant"]);
					LT = (dataRow["LT"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LT"]);
					MQO = (dataRow["MQO"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["MQO"]);
					Peis = (dataRow["Peis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Peis"]);
					Standar_Liferent = (dataRow["Standar_Liferent"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Standar_Liferent"]);
					Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
				}
			}
			public class Disposition_Bestellung
			{
				public string AB { get; set; }
				public DateTime? ABtermin { get; set; }
				public decimal? Anzhal { get; set; }
				public string Bemerkung { get; set; }
				public int? Lief_Nr { get; set; }
				public DateTime? Liefertermin { get; set; }
				public int? PO { get; set; }
				public string VornameFirma { get; set; }
				public bool? ProjectPurchase { get; set; }
				public DateTime? CreateDatePO { get; set; } // - 2025-02-10 AK - show date PO

				public Disposition_Bestellung() { }

				public Disposition_Bestellung(DataRow dataRow)
				{
					AB = (dataRow["AB"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AB"]);
					ABtermin = (dataRow["ABtermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ABtermin"]);
					Anzhal = (dataRow["Anzhal"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzhal"]);
					Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
					Lief_Nr = (dataRow["Lief_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lief_Nr"]);
					Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
					PO = (dataRow["PO"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PO"]);
					VornameFirma = (dataRow["VornameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["VornameFirma"]);
					ProjectPurchase = (dataRow["ProjectPurchase"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProjectPurchase"]);
				}
			}
			#endregion
		}
		public class BS_BomTz
		{
			public decimal? Anzahl { get; set; }
			public string ArtikelNrFG { get; set; }
			public string Artikelnummer { get; set; }
			public decimal? Bestand { get; set; }
			public string Bestell_Nr { get; set; }
			public string Bezeichnung_des_Bauteils { get; set; }
			public decimal? Einkaufspreis { get; set; }
			public decimal? Gesamtbestand { get; set; }
			public decimal? Kupferzahl { get; set; }
			public decimal? Mindestbestellmenge { get; set; }
			public string Name1 { get; set; }
			public int? Wiederbeschaffungszeitraum { get; set; }

			public BS_BomTz() { }

			public BS_BomTz(DataRow dataRow)
			{
				Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
				//ArtikelNrFG = (dataRow["ArtikelNrFG"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelNrFG"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
				Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
				Bezeichnung_des_Bauteils = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung des Bauteils"]);
				Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
				Gesamtbestand = (dataRow["Gesamtbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtbestand"]);
				Kupferzahl = (dataRow["Kupferzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kupferzahl"]);
				Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestellmenge"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"]);
			}
		}
		public class BS_Productivity
		{
			public string Artikel_Kunde { get; set; }
			public string Artikelnummer { get; set; }
			public decimal? Artikelzeit { get; set; }
			public int? Fertigungen { get; set; }
			public decimal? Prod_Artikelzeit { get; set; }
			public decimal? Prod_FA_Zeit { get; set; }
			public decimal? Std_Satz_aktuell { get; set; }

			public BS_Productivity() { }

			public BS_Productivity(DataRow dataRow)
			{
				Artikel_Kunde = (dataRow["Artikel Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikel Kunde"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Artikelzeit = (dataRow["Artikelzeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Artikelzeit"]);
				Fertigungen = (dataRow["Fertigungen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungen"]);
				Prod_Artikelzeit = (dataRow["Prod Artikelzeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Prod Artikelzeit"]);
				Prod_FA_Zeit = (dataRow["Prod FA Zeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Prod FA Zeit"]);
				Std_Satz_aktuell = (dataRow["Std Satz aktuell"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Std Satz aktuell"]);
			}
		}
		public class BS_ProductivityDetails
		{
			public int? Anzahl { get; set; }
			public decimal? Artikelzeit { get; set; }
			public int? FA { get; set; }
			public decimal? FA_Zeit { get; set; }
			public decimal? Prod_Artikelzeit { get; set; }
			public decimal? Prod_FA_Zeit { get; set; }
			public DateTime? Termin { get; set; }

			public BS_ProductivityDetails() { }

			public BS_ProductivityDetails(DataRow dataRow)
			{
				Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
				Artikelzeit = (dataRow["Artikelzeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Artikelzeit"]);
				FA = (dataRow["FA"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA"]);
				FA_Zeit = (dataRow["FA Zeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["FA Zeit"]);
				Prod_Artikelzeit = (dataRow["Prod Artikelzeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Prod Artikelzeit"]);
				Prod_FA_Zeit = (dataRow["Prod FA Zeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Prod FA Zeit"]);
				Termin = (dataRow["Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin"]);
			}
		}

		public class BS_Binsh
		{
			public string Artikelnummer { get; set; }
			public int? ArtikelNr { get; set; }
			public string Bezeichnung_1 { get; set; }
			public string Prufstatus_TN_AL { get; set; }
			public string Pruftiefe { get; set; }

			public BS_Binsh() { }

			public BS_Binsh(DataRow dataRow)
			{
				ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
				Prufstatus_TN_AL = (dataRow["Prüfstatus TN_AL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prüfstatus TN_AL"]);
				Pruftiefe = (dataRow["Prüftiefe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prüftiefe"]);
			}
		}
		public class CA_AnalyseFibuHeader
		{
			public string Ausdruck { get; set; }
			public string Debitorenname { get; set; }
			public string Debitorennummer { get; set; }
			public decimal? Gesamtkupferzuschlag { get; set; }
			public int? RechnungsCount { get; set; }

			public CA_AnalyseFibuHeader() { }

			public CA_AnalyseFibuHeader(DataRow dataRow)
			{
				Ausdruck = (dataRow["Ausdruck"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ausdruck"]);
				Debitorenname = (dataRow["Debitorenname"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Debitorenname"]);
				Debitorennummer = (dataRow["Debitorennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Debitorennummer"]);
				Gesamtkupferzuschlag = (dataRow["Gesamtkupferzuschlag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtkupferzuschlag"]);
				RechnungsCount = (dataRow["RechnungsCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RechnungsCount"]);
			}

		}
		public class CA_AnalyseFibu
		{
			public string Ausdruck { get; set; }
			public string Debitorenname { get; set; }
			public string Debitorennummer { get; set; }
			public decimal? Gesamtkupferzuschlag { get; set; }
			public DateTime? Rechnungsdatum { get; set; }
			public int? Rechnungsnummer { get; set; }
			public string Typ { get; set; }

			public CA_AnalyseFibu() { }

			public CA_AnalyseFibu(DataRow dataRow)
			{
				Ausdruck = (dataRow["Ausdruck"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ausdruck"]);
				Debitorenname = (dataRow["Debitorenname"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Debitorenname"]);
				Debitorennummer = (dataRow["Debitorennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Debitorennummer"]);
				Gesamtkupferzuschlag = (dataRow["Gesamtkupferzuschlag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtkupferzuschlag"]);
				Rechnungsdatum = (dataRow["Rechnungsdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rechnungsdatum"]);
				Rechnungsnummer = (dataRow["Rechnungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Rechnungsnummer"]);
				Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			}

		}
		public class BS_SiteArticle
		{
			public string ABProjectNr { get; set; }
			public string ArticleNumber { get; set; }
			public string Designation { get; set; }
			public string KundenIndex { get; set; }
			public string Type { get; set; }

			public BS_SiteArticle() { }

			public BS_SiteArticle(DataRow dataRow)
			{
				ABProjectNr = (dataRow["ABProjectNr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ABProjectNr"]);
				ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
				Designation = (dataRow["Designation"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Designation"]);
				KundenIndex = (dataRow["KundenIndex"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["KundenIndex"]);
				Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
			}

			public BS_SiteArticle ShallowClone()
			{
				return new BS_SiteArticle
				{
					ABProjectNr = ABProjectNr,
					ArticleNumber = ArticleNumber,
					Designation = Designation,
					KundenIndex = KundenIndex,
					Type = Type
				};
			}
		}
		public class BS_SiteBom
		{
			public string Artikelnummer { get; set; }
			public string Bezeichnung { get; set; }
			public string Material { get; set; }
			public Single? Quantity { get; set; }

			public BS_SiteBom() { }

			public BS_SiteBom(DataRow dataRow)
			{
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
				Material = (dataRow["Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material"]);
				Quantity = (dataRow["Quantity"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Quantity"]);
			}

			public BS_SiteBom ShallowClone()
			{
				return new BS_SiteBom
				{
					Artikelnummer = Artikelnummer,
					Bezeichnung = Bezeichnung,
					Material = Material,
					Quantity = Quantity
				};
			}
		}
		public class BS_HbgUbg
		{
			public string HBG_Freigabestatus { get; set; }
			public string HBG_FG { get; set; }
			public int? Losgroesse_UBG { get; set; }
			public int? Losgroesse_HBG { get; set; }
			public Single? Menge_Stuckliste { get; set; }
			public bool? UBG { get; set; }
			public string UBG_Artikelnummer { get; set; }
			public int? UBG_ArtikelNr { get; set; }
			public string UBG_Warengruppe { get; set; }

			public BS_HbgUbg() { }

			public BS_HbgUbg(DataRow dataRow)
			{
				UBG_ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
				HBG_Freigabestatus = (dataRow["HBG Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HBG Freigabestatus"]);
				HBG_FG = (dataRow["HBG_FG"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HBG_FG"]);
				Losgroesse_UBG = (dataRow["Losgroesse UBG"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Losgroesse UBG"]);
				Losgroesse_HBG = (dataRow["Losgroesse_HBG"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Losgroesse_HBG"]);
				Menge_Stuckliste = (dataRow["Menge_Stückliste"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Menge_Stückliste"]);
				UBG = (dataRow["UBG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UBG"]);
				UBG_Artikelnummer = (dataRow["UBG Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UBG Artikelnummer"]);
				UBG_Warengruppe = (dataRow["UBG Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UBG Warengruppe"]);
			}
			public BS_HbgUbg ShallowClone()
			{
				return new BS_HbgUbg
				{
					HBG_Freigabestatus = HBG_Freigabestatus,
					HBG_FG = HBG_FG,
					Losgroesse_UBG = Losgroesse_UBG,
					Losgroesse_HBG = Losgroesse_HBG,
					Menge_Stuckliste = Menge_Stuckliste,
					UBG = UBG,
					UBG_Artikelnummer = UBG_Artikelnummer,
					UBG_Warengruppe = UBG_Warengruppe
				};
			}
		}
		public class BS_SalesRa
		{
			public string Artikelnummer { get; set; }
			public double? Einkaufspreis { get; set; }
			public string Name1 { get; set; }
			public bool? Rahmen { get; set; }
			public bool? Rahmen2 { get; set; }
			public DateTime? Rahmenauslauf { get; set; }
			public DateTime? Rahmenauslauf2 { get; set; }
			public Single? Rahmenmenge { get; set; }
			public Single? Rahmenmenge2 { get; set; }
			public string Rahmen_Nr { get; set; }
			public string Rahmen_Nr2 { get; set; }

			public BS_SalesRa() { }

			public BS_SalesRa(DataRow dataRow)
			{
				Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Einkaufspreis"]);
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen"]);
				Rahmen2 = (dataRow["Rahmen2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen2"]);
				Rahmenauslauf = (dataRow["Rahmenauslauf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf"]);
				Rahmenauslauf2 = (dataRow["Rahmenauslauf2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf2"]);
				Rahmenmenge = (dataRow["Rahmenmenge"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rahmenmenge"]);
				Rahmenmenge2 = (dataRow["Rahmenmenge2"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rahmenmenge2"]);
				Rahmen_Nr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr"]);
				Rahmen_Nr2 = (dataRow["Rahmen-Nr2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr2"]);
			}
			public BS_SalesRa ShallowClone()
			{
				return new BS_SalesRa
				{
					Artikelnummer = Artikelnummer,
					Einkaufspreis = Einkaufspreis,
					Name1 = Name1,
					Rahmen = Rahmen,
					Rahmen2 = Rahmen2,
					Rahmenauslauf = Rahmenauslauf,
					Rahmenauslauf2 = Rahmenauslauf2,
					Rahmenmenge = Rahmenmenge,
					Rahmenmenge2 = Rahmenmenge2,
					Rahmen_Nr = Rahmen_Nr,
					Rahmen_Nr2 = Rahmen_Nr2
				};
			}
		}
		public class Sl_SupplierClass
		{
			public int Nr { get; set; }
			public string Name1 { get; set; }
			public string Stufe { get; set; }

			public Sl_SupplierClass() { }

			public Sl_SupplierClass(DataRow dataRow)
			{
				Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
				Stufe = (dataRow["Stufe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Stufe"]);
				Nr = Convert.ToInt32(dataRow["Nr"]);
			}
		}
	}
}
