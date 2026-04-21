using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class ArtikelAccess
	{
		#region Default Methods
		public static Entities.Tables.PRS.ArtikelEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel] WHERE [Artikel-Nr]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.PRS.ArtikelEntity> Get()
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					result = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		private static List<Entities.Tables.PRS.ArtikelEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();

				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [Artikel] WHERE [Artikel-Nr] IN (" + queryIds + ")";

					using(var reader = sqlCommand.ExecuteReader())
					{
						result = toList(reader);
					}
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}

		public static int Add(Entities.Tables.PRS.ArtikelEntity element)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Artikel] ([Abladestelle],[aktiv],[aktualisiert],[Anfangsbestand],[ArticleNumber],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelbezeichnung],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[artikelklassifizierung],[Artikelkurztext],[Artikelnummer],[Barverkauf],[BemerkungCRP],[BemerkungCRPPlanung],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[BezeichnungAL],[Blokiert_Status],[CocVersion],[COF_Pflichtig],[CP_required],[Crossreferenz],[Cu-Gewicht],[CustomerEnd],[CustomerIndex],[CustomerIndexSequence],[CustomerItemNumber],[CustomerItemNumberSequence],[CustomerNumber],[CustomerPrefix],[CustomerTechnic],[CustomerTechnicId],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dienstelistung],[Dokumente],[EAN],[EdiDefault],[Einheit],[EMPB],[EMPB_Freigegeben],[Ersatzartikel],[ESD_Schutz],[ESD_Schutz_Text],[Exportgewicht],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Hubmastleitungen],[ID_Klassifizierung],[Index_Kunde],[Index_Kunde_Datum],[Info_WE],[IsArticleNumberSpecial],[IsEDrawing],[Kanban],[Kategorie],[Klassifizierung],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Lieferzeit],[Losgroesse],[Manufacturer],[ManufacturerNextArticle],[ManufacturerNextArticleId],[ManufacturerNumber],[ManufacturerPreviousArticle],[ManufacturerPreviousArticleId],[Materialkosten_Alt],[MHD],[Minerals Confirmity],[Praeferenz_Aktuelles_jahr],[Praeferenz_Folgejahr],[Preiseinheit],[pro Zeiteinheit],[ProductionCountryCode],[ProductionCountryName],[ProductionCountrySequence],[ProductionLotSize],[ProductionSiteCode],[ProductionSiteName],[ProductionSiteSequence],[Produktionszeit],[Projektname],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmen2],[Rahmenauslauf],[Rahmenauslauf2],[Rahmenmenge],[Rahmenmenge2],[Rahmen-Nr],[Rahmen-Nr2],[REACH SVHC Confirmity],[ROHS EEE Confirmity],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UBG],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[VDA_1],[VDA_2],[Verpackung],[Verpackungsart],[Verpackungsmenge],[VK-Festpreis],[Volumen],[Warengruppe],[Warentyp],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zeitraum_MHD],[Zolltarif_nr],[Zuschlag_VK]) OUTPUT INSERTED.[Artikel-Nr] VALUES (@Abladestelle,@aktiv,@aktualisiert,@Anfangsbestand,@ArticleNumber,@Artikel_aus_eigener_Produktion,@Artikel_fur_weitere_Bestellungen_sperren,@Artikelbezeichnung,@Artikelfamilie_Kunde,@Artikelfamilie_Kunde_Detail1,@Artikelfamilie_Kunde_Detail2,@artikelklassifizierung,@Artikelkurztext,@Artikelnummer,@Barverkauf,@BemerkungCRP,@Bezeichnung_1,@Bezeichnung_2,@Bezeichnung_3,@BezeichnungAL,@Blokiert_Status,@CocVersion,@COF_Pflichtig,@CP_required,@Crossreferenz,@Cu_Gewicht,@CustomerEnd,@CustomerIndex,@CustomerIndexSequence,@CustomerItemNumber,@CustomerItemNumberSequence,@CustomerNumber,@CustomerPrefix,@CustomerTechnic,@CustomerTechnicId,@Datum_Anfangsbestand,@DEL,@DEL_fixiert,@Dienstelistung,@Dokumente,@EAN,@EdiDefault,@Einheit,@EMPB,@EMPB_Freigegeben,@Ersatzartikel,@ESD_Schutz,@ESD_Schutz_Text,@Exportgewicht,@fakturieren_Stuckliste,@Farbe,@fibu_rahmen,@Freigabestatus,@Freigabestatus_TN_intern,@Gebinde,@Gewicht,@Grosse,@Grund_fur_Sperre,@gultig_bis,@Halle,@Hubmastleitungen,@ID_Klassifizierung,@Index_Kunde,@Index_Kunde_Datum,@Info_WE,@IsArticleNumberSpecial,@IsEDrawing,@Kanban,@Kategorie,@Klassifizierung,@Kriterium1,@Kriterium2,@Kriterium3,@Kriterium4,@Kupferbasis,@Kupferzahl,@Lagerartikel,@Lagerhaltungskosten,@Langtext,@Langtext_drucken_AB,@Langtext_drucken_BW,@Lieferzeit,@Losgroesse,@Manufacturer,@ManufacturerNextArticle,@ManufacturerNextArticleId,@ManufacturerNumber,@ManufacturerPreviousArticle,@ManufacturerPreviousArticleId,@Materialkosten_Alt,@MHD,@Minerals_Confirmity,@Praeferenz_Aktuelles_jahr,@Praeferenz_Folgejahr,@Preiseinheit,@pro_Zeiteinheit,@ProductionCountryCode,@ProductionCountryName,@ProductionCountrySequence,@ProductionLotSize,@ProductionSiteCode,@ProductionSiteName,@ProductionSiteSequence,@Produktionszeit,@Projektname,@Provisionsartikel,@Prufstatus_TN_Ware,@Rabattierfahig,@Rahmen,@Rahmen2,@Rahmenauslauf,@Rahmenauslauf2,@Rahmenmenge,@Rahmenmenge2,@Rahmen_Nr,@Rahmen_Nr2,@REACH_SVHC_Confirmity,@ROHS_EEE_Confirmity,@Seriennummer,@Seriennummernverwaltung,@Sonderrabatt,@Standard_Lagerort_id,@Stuckliste,@Stundensatz,@Sysmonummer,@UBG,@UL_Etikett,@UL_zertifiziert,@Umsatzsteuer,@Ursprungsland,@VDA_1,@VDA_2,@Verpackung,@Verpackungsart,@Verpackungsmenge,@VK_Festpreis,@Volumen,@Warengruppe,@Warentyp,@Webshop,@Werkzeug,@Wert_Anfangsbestand,@Zeichnungsnummer,@Zeitraum_MHD,@Zolltarif_nr,@Zuschlag_VK); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Abladestelle", element.Abladestelle == null ? (object)DBNull.Value : element.Abladestelle);
					sqlCommand.Parameters.AddWithValue("aktiv", element.aktiv == null ? (object)DBNull.Value : element.aktiv);
					sqlCommand.Parameters.AddWithValue("aktualisiert", element.aktualisiert == null ? (object)DBNull.Value : element.aktualisiert);
					sqlCommand.Parameters.AddWithValue("Anfangsbestand", element.Anfangsbestand == null ? (object)DBNull.Value : element.Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", element.ArticleNumber == null ? (object)DBNull.Value : element.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", element.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : element.ArtikelAusEigenerProduktion);
					sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren", element.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : element.ArtikelFürWeitereBestellungenSperren);
					sqlCommand.Parameters.AddWithValue("Artikelbezeichnung", element.Artikelbezeichnung == null ? (object)DBNull.Value : element.Artikelbezeichnung);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", element.Artikelfamilie_Kunde == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", element.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail1);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", element.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail2);
					sqlCommand.Parameters.AddWithValue("artikelklassifizierung", element.artikelklassifizierung == null ? (object)DBNull.Value : element.artikelklassifizierung);
					sqlCommand.Parameters.AddWithValue("Artikelkurztext", element.Artikelkurztext == null ? (object)DBNull.Value : element.Artikelkurztext);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", element.ArtikelNummer == null ? (object)DBNull.Value : element.ArtikelNummer);
					sqlCommand.Parameters.AddWithValue("Barverkauf", element.Barverkauf == null ? (object)DBNull.Value : element.Barverkauf);
					sqlCommand.Parameters.AddWithValue("BemerkungCRP", element.BemerkungCRP == null ? (object)DBNull.Value : element.BemerkungCRP);
					sqlCommand.Parameters.AddWithValue("BemerkungCRPPlanung", element.BemerkungCRPPlanung == null ? (object)DBNull.Value : element.BemerkungCRPPlanung);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", element.Bezeichnung1 == null ? (object)DBNull.Value : element.Bezeichnung1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2", element.Bezeichnung2 == null ? (object)DBNull.Value : element.Bezeichnung2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_3", element.Bezeichnung3 == null ? (object)DBNull.Value : element.Bezeichnung3);
					sqlCommand.Parameters.AddWithValue("BezeichnungAL", element.BezeichnungAL == null ? (object)DBNull.Value : element.BezeichnungAL);
					sqlCommand.Parameters.AddWithValue("Blokiert_Status", element.Blokiert_Status == null ? (object)DBNull.Value : element.Blokiert_Status);
					sqlCommand.Parameters.AddWithValue("CocVersion", element.CocVersion == null ? (object)DBNull.Value : element.CocVersion);
					sqlCommand.Parameters.AddWithValue("COF_Pflichtig", element.COF_Pflichtig == null ? (object)DBNull.Value : element.COF_Pflichtig);
					sqlCommand.Parameters.AddWithValue("CP_required", element.CP_required == null ? (object)DBNull.Value : element.CP_required);
					sqlCommand.Parameters.AddWithValue("Crossreferenz", element.Crossreferenz == null ? (object)DBNull.Value : element.Crossreferenz);
					sqlCommand.Parameters.AddWithValue("Cu_Gewicht", element.CuGewicht == null ? (object)DBNull.Value : element.CuGewicht);
					sqlCommand.Parameters.AddWithValue("CustomerEnd", element.CustomerEnd == null ? (object)DBNull.Value : element.CustomerEnd);
					sqlCommand.Parameters.AddWithValue("CustomerIndex", element.CustomerIndex == null ? (object)DBNull.Value : element.CustomerIndex);
					sqlCommand.Parameters.AddWithValue("CustomerIndexSequence", element.CustomerIndexSequence == null ? (object)DBNull.Value : element.CustomerIndexSequence);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumber", element.CustomerItemNumber == null ? (object)DBNull.Value : element.CustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence", element.CustomerItemNumberSequence == null ? (object)DBNull.Value : element.CustomerItemNumberSequence);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", element.CustomerNumber == null ? (object)DBNull.Value : element.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerPrefix", element.CustomerPrefix == null ? (object)DBNull.Value : element.CustomerPrefix);
					sqlCommand.Parameters.AddWithValue("CustomerTechnic", element.CustomerTechnic == null ? (object)DBNull.Value : element.CustomerTechnic);
					sqlCommand.Parameters.AddWithValue("CustomerTechnicId", element.CustomerTechnicId == null ? (object)DBNull.Value : element.CustomerTechnicId);
					sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", element.DatumAnfangsbestand == null ? (object)DBNull.Value : element.DatumAnfangsbestand);
					sqlCommand.Parameters.AddWithValue("DEL", element.DEL == null ? (object)DBNull.Value : element.DEL);
					sqlCommand.Parameters.AddWithValue("DEL_fixiert", element.DELFixiert == null ? (object)DBNull.Value : element.DELFixiert);
					sqlCommand.Parameters.AddWithValue("Dienstelistung", element.Dienstelistung == null ? (object)DBNull.Value : element.Dienstelistung);
					sqlCommand.Parameters.AddWithValue("Dokumente", element.Dokumente == null ? (object)DBNull.Value : element.Dokumente);
					sqlCommand.Parameters.AddWithValue("EAN", element.EAN == null ? (object)DBNull.Value : element.EAN);
					sqlCommand.Parameters.AddWithValue("EdiDefault", element.EdiDefault == null ? (object)DBNull.Value : element.EdiDefault);
					sqlCommand.Parameters.AddWithValue("Einheit", element.Einheit == null ? (object)DBNull.Value : element.Einheit);
					sqlCommand.Parameters.AddWithValue("EMPB", element.EMPB == null ? (object)DBNull.Value : element.EMPB);
					sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben", element.EMPB_Freigegeben == null ? (object)DBNull.Value : element.EMPB_Freigegeben);
					sqlCommand.Parameters.AddWithValue("Ersatzartikel", element.Ersatzartikel == null ? (object)DBNull.Value : element.Ersatzartikel);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz", element.ESD_Schutz == null ? (object)DBNull.Value : element.ESD_Schutz);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text", element.ESD_Schutz_Text == null ? (object)DBNull.Value : element.ESD_Schutz_Text);
					sqlCommand.Parameters.AddWithValue("Exportgewicht", element.Exportgewicht == null ? (object)DBNull.Value : element.Exportgewicht);
					sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste", element.fakturierenStückliste == null ? (object)DBNull.Value : element.fakturierenStückliste);
					sqlCommand.Parameters.AddWithValue("Farbe", element.Farbe == null ? (object)DBNull.Value : element.Farbe);
					sqlCommand.Parameters.AddWithValue("fibu_rahmen", element.fibu_rahmen == null ? (object)DBNull.Value : element.fibu_rahmen);
					sqlCommand.Parameters.AddWithValue("Freigabestatus", element.Freigabestatus == null ? (object)DBNull.Value : element.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", element.FreigabestatusTNIntern == null ? (object)DBNull.Value : element.FreigabestatusTNIntern);
					sqlCommand.Parameters.AddWithValue("Gebinde", element.Gebinde == null ? (object)DBNull.Value : element.Gebinde);
					sqlCommand.Parameters.AddWithValue("Gewicht", element.Gewicht == null ? (object)DBNull.Value : element.Gewicht);
					sqlCommand.Parameters.AddWithValue("Grosse", element.Größe == null ? (object)DBNull.Value : element.Größe);
					sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", element.GrundFürSperre == null ? (object)DBNull.Value : element.GrundFürSperre);
					sqlCommand.Parameters.AddWithValue("gultig_bis", element.gültigBis == null ? (object)DBNull.Value : element.gültigBis);
					sqlCommand.Parameters.AddWithValue("Halle", element.Halle == null ? (object)DBNull.Value : element.Halle);
					sqlCommand.Parameters.AddWithValue("Hubmastleitungen", element.Hubmastleitungen == null ? (object)DBNull.Value : element.Hubmastleitungen);
					sqlCommand.Parameters.AddWithValue("ID_Klassifizierung", element.ID_Klassifizierung == null ? (object)DBNull.Value : element.ID_Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Index_Kunde", element.Index_Kunde == null ? (object)DBNull.Value : element.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", element.Index_Kunde_Datum == null ? (object)DBNull.Value : element.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("Info_WE", element.Info_WE == null ? (object)DBNull.Value : element.Info_WE);
					sqlCommand.Parameters.AddWithValue("IsArticleNumberSpecial", element.IsArticleNumberSpecial == null ? (object)DBNull.Value : element.IsArticleNumberSpecial);
					sqlCommand.Parameters.AddWithValue("IsEDrawing", element.IsEDrawing == null ? (object)DBNull.Value : element.IsEDrawing);
					sqlCommand.Parameters.AddWithValue("Kanban", element.Kanban == null ? (object)DBNull.Value : element.Kanban);
					sqlCommand.Parameters.AddWithValue("Kategorie", element.Kategorie == null ? (object)DBNull.Value : element.Kategorie);
					sqlCommand.Parameters.AddWithValue("Klassifizierung", element.Klassifizierung == null ? (object)DBNull.Value : element.Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Kriterium1", element.Kriterium1 == null ? (object)DBNull.Value : element.Kriterium1);
					sqlCommand.Parameters.AddWithValue("Kriterium2", element.Kriterium2 == null ? (object)DBNull.Value : element.Kriterium2);
					sqlCommand.Parameters.AddWithValue("Kriterium3", element.Kriterium3 == null ? (object)DBNull.Value : element.Kriterium3);
					sqlCommand.Parameters.AddWithValue("Kriterium4", element.Kriterium4 == null ? (object)DBNull.Value : element.Kriterium4);
					sqlCommand.Parameters.AddWithValue("Kupferbasis", element.Kupferbasis == null ? (object)DBNull.Value : element.Kupferbasis);
					sqlCommand.Parameters.AddWithValue("Kupferzahl", element.Kupferzahl == null ? (object)DBNull.Value : element.Kupferzahl);
					sqlCommand.Parameters.AddWithValue("Lagerartikel", element.Lagerartikel == null ? (object)DBNull.Value : element.Lagerartikel);
					sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", element.Lagerhaltungskosten == null ? (object)DBNull.Value : element.Lagerhaltungskosten);
					sqlCommand.Parameters.AddWithValue("Langtext", element.Langtext == null ? (object)DBNull.Value : element.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", element.Langtext_drucken_AB == null ? (object)DBNull.Value : element.Langtext_drucken_AB);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", element.Langtext_drucken_BW == null ? (object)DBNull.Value : element.Langtext_drucken_BW);
					sqlCommand.Parameters.AddWithValue("Lieferzeit", element.Lieferzeit == null ? (object)DBNull.Value : element.Lieferzeit);
					sqlCommand.Parameters.AddWithValue("Losgroesse", element.Losgroesse == null ? (object)DBNull.Value : element.Losgroesse);
					sqlCommand.Parameters.AddWithValue("Manufacturer", element.Manufacturer == null ? (object)DBNull.Value : element.Manufacturer);
					sqlCommand.Parameters.AddWithValue("ManufacturerNextArticle", element.ManufacturerNextArticle == null ? (object)DBNull.Value : element.ManufacturerNextArticle);
					sqlCommand.Parameters.AddWithValue("ManufacturerNextArticleId", element.ManufacturerNextArticleId == null ? (object)DBNull.Value : element.ManufacturerNextArticleId);
					sqlCommand.Parameters.AddWithValue("ManufacturerNumber", element.ManufacturerNumber == null ? (object)DBNull.Value : element.ManufacturerNumber);
					sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticle", element.ManufacturerPreviousArticle == null ? (object)DBNull.Value : element.ManufacturerPreviousArticle);
					sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticleId", element.ManufacturerPreviousArticleId == null ? (object)DBNull.Value : element.ManufacturerPreviousArticleId);
					sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", element.Materialkosten_Alt == null ? (object)DBNull.Value : element.Materialkosten_Alt);
					sqlCommand.Parameters.AddWithValue("MHD", element.MHD == null ? (object)DBNull.Value : element.MHD);
					sqlCommand.Parameters.AddWithValue("Minerals_Confirmity", element.MineralsConfirmity == null ? (object)DBNull.Value : element.MineralsConfirmity);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr", element.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : element.Praeferenz_Aktuelles_jahr);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr", element.Praeferenz_Folgejahr == null ? (object)DBNull.Value : element.Praeferenz_Folgejahr);
					sqlCommand.Parameters.AddWithValue("Preiseinheit", element.Preiseinheit == null ? (object)DBNull.Value : element.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", element.proZeiteinheit == null ? (object)DBNull.Value : element.proZeiteinheit);
					sqlCommand.Parameters.AddWithValue("ProductionCountryCode", element.ProductionCountryCode == null ? (object)DBNull.Value : element.ProductionCountryCode);
					sqlCommand.Parameters.AddWithValue("ProductionCountryName", element.ProductionCountryName == null ? (object)DBNull.Value : element.ProductionCountryName);
					sqlCommand.Parameters.AddWithValue("ProductionCountrySequence", element.ProductionCountrySequence == null ? (object)DBNull.Value : element.ProductionCountrySequence);
					sqlCommand.Parameters.AddWithValue("ProductionLotSize", element.ProductionLotSize == null ? (object)DBNull.Value : element.ProductionLotSize);
					sqlCommand.Parameters.AddWithValue("ProductionSiteCode", element.ProductionSiteCode == null ? (object)DBNull.Value : element.ProductionSiteCode);
					sqlCommand.Parameters.AddWithValue("ProductionSiteName", element.ProductionSiteName == null ? (object)DBNull.Value : element.ProductionSiteName);
					sqlCommand.Parameters.AddWithValue("ProductionSiteSequence", element.ProductionSiteSequence == null ? (object)DBNull.Value : element.ProductionSiteSequence);
					sqlCommand.Parameters.AddWithValue("Produktionszeit", element.Produktionszeit == null ? (object)DBNull.Value : element.Produktionszeit);
					sqlCommand.Parameters.AddWithValue("Projektname", element.Projektname == null ? (object)DBNull.Value : element.Projektname);
					sqlCommand.Parameters.AddWithValue("Provisionsartikel", element.Provisionsartikel == null ? (object)DBNull.Value : element.Provisionsartikel);
					sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware", element.PrufstatusTNWare == null ? (object)DBNull.Value : element.PrufstatusTNWare);
					sqlCommand.Parameters.AddWithValue("Rabattierfahig", element.Rabattierfähig == null ? (object)DBNull.Value : element.Rabattierfähig);
					sqlCommand.Parameters.AddWithValue("Rahmen", element.Rahmen == null ? (object)DBNull.Value : element.Rahmen);
					sqlCommand.Parameters.AddWithValue("Rahmen2", element.Rahmen2 == null ? (object)DBNull.Value : element.Rahmen2);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf", element.Rahmenauslauf == null ? (object)DBNull.Value : element.Rahmenauslauf);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf2", element.Rahmenauslauf2 == null ? (object)DBNull.Value : element.Rahmenauslauf2);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge", element.Rahmenmenge == null ? (object)DBNull.Value : element.Rahmenmenge);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge2", element.Rahmenmenge2 == null ? (object)DBNull.Value : element.Rahmenmenge2);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr", element.RahmenNr == null ? (object)DBNull.Value : element.RahmenNr);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr2", element.RahmenNr2 == null ? (object)DBNull.Value : element.RahmenNr2);
					sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity", element.REACHSVHCConfirmity == null ? (object)DBNull.Value : element.REACHSVHCConfirmity);
					sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity", element.ROHSEEEConfirmity == null ? (object)DBNull.Value : element.ROHSEEEConfirmity);
					sqlCommand.Parameters.AddWithValue("Seriennummer", element.Seriennummer == null ? (object)DBNull.Value : element.Seriennummer);
					sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", element.Seriennummernverwaltung == null ? (object)DBNull.Value : element.Seriennummernverwaltung);
					sqlCommand.Parameters.AddWithValue("Sonderrabatt", element.Sonderrabatt == null ? (object)DBNull.Value : element.Sonderrabatt);
					sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", element.Standard_Lagerort_id == null ? (object)DBNull.Value : element.Standard_Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Stuckliste", element.Stuckliste == null ? (object)DBNull.Value : element.Stuckliste);
					sqlCommand.Parameters.AddWithValue("Stundensatz", element.Stundensatz == null ? (object)DBNull.Value : element.Stundensatz);
					sqlCommand.Parameters.AddWithValue("Sysmonummer", element.Sysmonummer == null ? (object)DBNull.Value : element.Sysmonummer);
					sqlCommand.Parameters.AddWithValue("UBG", element.UBG == null ? (object)DBNull.Value : element.UBG);
					sqlCommand.Parameters.AddWithValue("UL_Etikett", element.ULEtikett == null ? (object)DBNull.Value : element.ULEtikett);
					sqlCommand.Parameters.AddWithValue("UL_zertifiziert", element.ULzertifiziert == null ? (object)DBNull.Value : element.ULzertifiziert);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer", element.Umsatzsteuer == null ? (object)DBNull.Value : element.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Ursprungsland", element.Ursprungsland == null ? (object)DBNull.Value : element.Ursprungsland);
					sqlCommand.Parameters.AddWithValue("VDA_1", element.VDA_1 == null ? (object)DBNull.Value : element.VDA_1);
					sqlCommand.Parameters.AddWithValue("VDA_2", element.VDA_2 == null ? (object)DBNull.Value : element.VDA_2);
					sqlCommand.Parameters.AddWithValue("Verpackung", element.Verpackung == null ? (object)DBNull.Value : element.Verpackung);
					sqlCommand.Parameters.AddWithValue("Verpackungsart", element.Verpackungsart == null ? (object)DBNull.Value : element.Verpackungsart);
					sqlCommand.Parameters.AddWithValue("Verpackungsmenge", element.Verpackungsmenge == null ? (object)DBNull.Value : element.Verpackungsmenge);
					sqlCommand.Parameters.AddWithValue("VK_Festpreis", element.VKFestpreis == null ? (object)DBNull.Value : element.VKFestpreis);
					sqlCommand.Parameters.AddWithValue("Volumen", element.Volumen == null ? (object)DBNull.Value : element.Volumen);
					sqlCommand.Parameters.AddWithValue("Warengruppe", element.Warengruppe == null ? (object)DBNull.Value : element.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Warentyp", element.Warentyp == null ? (object)DBNull.Value : element.Warentyp);
					sqlCommand.Parameters.AddWithValue("Webshop", element.Webshop == null ? (object)DBNull.Value : element.Webshop);
					sqlCommand.Parameters.AddWithValue("Werkzeug", element.Werkzeug == null ? (object)DBNull.Value : element.Werkzeug);
					sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", element.Wert_Anfangsbestand == null ? (object)DBNull.Value : element.Wert_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", element.Zeichnungsnummer == null ? (object)DBNull.Value : element.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", element.Zeitraum_MHD == null ? (object)DBNull.Value : element.Zeitraum_MHD);
					sqlCommand.Parameters.AddWithValue("Zolltarif_nr", element.Zolltarif_nr == null ? (object)DBNull.Value : element.Zolltarif_nr);
					sqlCommand.Parameters.AddWithValue("Zuschlag_VK", element.Zuschlag_VK == null ? (object)DBNull.Value : element.Zuschlag_VK);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Add(List<Entities.Tables.PRS.ArtikelEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 109; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = add(elements);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += add(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += add(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber));
				}
			}

			return -1;
		}
		private static int add(List<Entities.Tables.PRS.ArtikelEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in elements)
					{
						i++;
						query += " INSERT INTO [Artikel] ([Abladestelle],[aktiv],[aktualisiert],[Anfangsbestand],[ArticleNumber],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelbezeichnung],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[artikelklassifizierung],[Artikelkurztext],[Artikelnummer],[Barverkauf],[BemerkungCRP],[BemerkungCRPPlanung],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[BezeichnungAL],[Blokiert_Status],[CocVersion],[COF_Pflichtig],[CP_required],[Crossreferenz],[Cu-Gewicht],[CustomerEnd],[CustomerIndex],[CustomerIndexSequence],[CustomerItemNumber],[CustomerItemNumberSequence],[CustomerNumber],[CustomerPrefix],[CustomerTechnic],[CustomerTechnicId],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dienstelistung],[Dokumente],[EAN],[EdiDefault],[Einheit],[EMPB],[EMPB_Freigegeben],[Ersatzartikel],[ESD_Schutz],[ESD_Schutz_Text],[Exportgewicht],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Hubmastleitungen],[ID_Klassifizierung],[Index_Kunde],[Index_Kunde_Datum],[Info_WE],[IsArticleNumberSpecial],[IsEDrawing],[Kanban],[Kategorie],[Klassifizierung],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Lieferzeit],[Losgroesse],[Manufacturer],[ManufacturerNextArticle],[ManufacturerNextArticleId],[ManufacturerNumber],[ManufacturerPreviousArticle],[ManufacturerPreviousArticleId],[Materialkosten_Alt],[MHD],[Minerals Confirmity],[Praeferenz_Aktuelles_jahr],[Praeferenz_Folgejahr],[Preiseinheit],[pro Zeiteinheit],[ProductionCountryCode],[ProductionCountryName],[ProductionCountrySequence],[ProductionLotSize],[ProductionSiteCode],[ProductionSiteName],[ProductionSiteSequence],[Produktionszeit],[Projektname],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmen2],[Rahmenauslauf],[Rahmenauslauf2],[Rahmenmenge],[Rahmenmenge2],[Rahmen-Nr],[Rahmen-Nr2],[REACH SVHC Confirmity],[ROHS EEE Confirmity],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UBG],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[VDA_1],[VDA_2],[Verpackung],[Verpackungsart],[Verpackungsmenge],[VK-Festpreis],[Volumen],[Warengruppe],[Warentyp],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zeitraum_MHD],[Zolltarif_nr],[Zuschlag_VK]) VALUES ( "

							+ "@Abladestelle" + i + ","
							+ "@aktiv" + i + ","
							+ "@aktualisiert" + i + ","
							+ "@Anfangsbestand" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@Artikel_aus_eigener_Produktion" + i + ","
							+ "@Artikel_fur_weitere_Bestellungen_sperren" + i + ","
							+ "@Artikelbezeichnung" + i + ","
							+ "@Artikelfamilie_Kunde" + i + ","
							+ "@Artikelfamilie_Kunde_Detail1" + i + ","
							+ "@Artikelfamilie_Kunde_Detail2" + i + ","
							+ "@artikelklassifizierung" + i + ","
							+ "@Artikelkurztext" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Barverkauf" + i + ","
							+ "@BemerkungCRP" + i + ","
							+ "@BemerkungCRPPlanung" + i + ","
							+ "@Bezeichnung_1" + i + ","
							+ "@Bezeichnung_2" + i + ","
							+ "@Bezeichnung_3" + i + ","
							+ "@BezeichnungAL" + i + ","
							+ "@Blokiert_Status" + i + ","
							+ "@CocVersion" + i + ","
							+ "@COF_Pflichtig" + i + ","
							+ "@CP_required" + i + ","
							+ "@Crossreferenz" + i + ","
							+ "@Cu_Gewicht" + i + ","
							+ "@CustomerEnd" + i + ","
							+ "@CustomerIndex" + i + ","
							+ "@CustomerIndexSequence" + i + ","
							+ "@CustomerItemNumber" + i + ","
							+ "@CustomerItemNumberSequence" + i + ","
							+ "@CustomerNumber" + i + ","
							+ "@CustomerPrefix" + i + ","
							+ "@CustomerTechnic" + i + ","
							+ "@CustomerTechnicId" + i + ","
							+ "@Datum_Anfangsbestand" + i + ","
							+ "@DEL" + i + ","
							+ "@DEL_fixiert" + i + ","
							+ "@Dienstelistung" + i + ","
							+ "@Dokumente" + i + ","
							+ "@EAN" + i + ","
							+ "@EdiDefault" + i + ","
							+ "@Einheit" + i + ","
							+ "@EMPB" + i + ","
							+ "@EMPB_Freigegeben" + i + ","
							+ "@Ersatzartikel" + i + ","
							+ "@ESD_Schutz" + i + ","
							+ "@ESD_Schutz_Text" + i + ","
							+ "@Exportgewicht" + i + ","
							+ "@fakturieren_Stuckliste" + i + ","
							+ "@Farbe" + i + ","
							+ "@fibu_rahmen" + i + ","
							+ "@Freigabestatus" + i + ","
							+ "@Freigabestatus_TN_intern" + i + ","
							+ "@Gebinde" + i + ","
							+ "@Gewicht" + i + ","
							+ "@Grosse" + i + ","
							+ "@Grund_fur_Sperre" + i + ","
							+ "@gultig_bis" + i + ","
							+ "@Halle" + i + ","
							+ "@Hubmastleitungen" + i + ","
							+ "@ID_Klassifizierung" + i + ","
							+ "@Index_Kunde" + i + ","
							+ "@Index_Kunde_Datum" + i + ","
							+ "@Info_WE" + i + ","
							+ "@IsArticleNumberSpecial" + i + ","
							+ "@IsEDrawing" + i + ","
							+ "@Kanban" + i + ","
							+ "@Kategorie" + i + ","
							+ "@Klassifizierung" + i + ","
							+ "@Kriterium1" + i + ","
							+ "@Kriterium2" + i + ","
							+ "@Kriterium3" + i + ","
							+ "@Kriterium4" + i + ","
							+ "@Kupferbasis" + i + ","
							+ "@Kupferzahl" + i + ","
							+ "@Lagerartikel" + i + ","
							+ "@Lagerhaltungskosten" + i + ","
							+ "@Langtext" + i + ","
							+ "@Langtext_drucken_AB" + i + ","
							+ "@Langtext_drucken_BW" + i + ","
							+ "@Lieferzeit" + i + ","
							+ "@Losgroesse" + i + ","
							+ "@Manufacturer" + i + ","
							+ "@ManufacturerNextArticle" + i + ","
							+ "@ManufacturerNextArticleId" + i + ","
							+ "@ManufacturerNumber" + i + ","
							+ "@ManufacturerPreviousArticle" + i + ","
							+ "@ManufacturerPreviousArticleId" + i + ","
							+ "@Materialkosten_Alt" + i + ","
							+ "@MHD" + i + ","
							+ "@Minerals_Confirmity" + i + ","
							+ "@Praeferenz_Aktuelles_jahr" + i + ","
							+ "@Praeferenz_Folgejahr" + i + ","
							+ "@Preiseinheit" + i + ","
							+ "@pro_Zeiteinheit" + i + ","
							+ "@ProductionCountryCode" + i + ","
							+ "@ProductionCountryName" + i + ","
							+ "@ProductionCountrySequence" + i + ","
							+ "@ProductionLotSize" + i + ","
							+ "@ProductionSiteCode" + i + ","
							+ "@ProductionSiteName" + i + ","
							+ "@ProductionSiteSequence" + i + ","
							+ "@Produktionszeit" + i + ","
							+ "@Projektname" + i + ","
							+ "@Provisionsartikel" + i + ","
							+ "@Prufstatus_TN_Ware" + i + ","
							+ "@Rabattierfahig" + i + ","
							+ "@Rahmen" + i + ","
							+ "@Rahmen2" + i + ","
							+ "@Rahmenauslauf" + i + ","
							+ "@Rahmenauslauf2" + i + ","
							+ "@Rahmenmenge" + i + ","
							+ "@Rahmenmenge2" + i + ","
							+ "@Rahmen_Nr" + i + ","
							+ "@Rahmen_Nr2" + i + ","
							+ "@REACH_SVHC_Confirmity" + i + ","
							+ "@ROHS_EEE_Confirmity" + i + ","
							+ "@Seriennummer" + i + ","
							+ "@Seriennummernverwaltung" + i + ","
							+ "@Sonderrabatt" + i + ","
							+ "@Standard_Lagerort_id" + i + ","
							+ "@Stuckliste" + i + ","
							+ "@Stundensatz" + i + ","
							+ "@Sysmonummer" + i + ","
							+ "@UBG" + i + ","
							+ "@UL_Etikett" + i + ","
							+ "@UL_zertifiziert" + i + ","
							+ "@Umsatzsteuer" + i + ","
							+ "@Ursprungsland" + i + ","
							+ "@VDA_1" + i + ","
							+ "@VDA_2" + i + ","
							+ "@Verpackung" + i + ","
							+ "@Verpackungsart" + i + ","
							+ "@Verpackungsmenge" + i + ","
							+ "@VK_Festpreis" + i + ","
							+ "@Volumen" + i + ","
							+ "@Warengruppe" + i + ","
							+ "@Warentyp" + i + ","
							+ "@Webshop" + i + ","
							+ "@Werkzeug" + i + ","
							+ "@Wert_Anfangsbestand" + i + ","
							+ "@Zeichnungsnummer" + i + ","
							+ "@Zeitraum_MHD" + i + ","
							+ "@Zolltarif_nr" + i + ","
							+ "@Zuschlag_VK" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
						sqlCommand.Parameters.AddWithValue("aktiv" + i, item.aktiv == null ? (object)DBNull.Value : item.aktiv);
						sqlCommand.Parameters.AddWithValue("aktualisiert" + i, item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
						sqlCommand.Parameters.AddWithValue("Anfangsbestand" + i, item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion" + i, item.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : item.ArtikelAusEigenerProduktion);
						sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren" + i, item.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : item.ArtikelFürWeitereBestellungenSperren);
						sqlCommand.Parameters.AddWithValue("Artikelbezeichnung" + i, item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
						sqlCommand.Parameters.AddWithValue("artikelklassifizierung" + i, item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
						sqlCommand.Parameters.AddWithValue("Artikelkurztext" + i, item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
						sqlCommand.Parameters.AddWithValue("Barverkauf" + i, item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
						sqlCommand.Parameters.AddWithValue("BemerkungCRP" + i, item.BemerkungCRP == null ? (object)DBNull.Value : item.BemerkungCRP);
						sqlCommand.Parameters.AddWithValue("BemerkungCRPPlanung" + i, item.BemerkungCRPPlanung == null ? (object)DBNull.Value : item.BemerkungCRPPlanung);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_3" + i, item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
						sqlCommand.Parameters.AddWithValue("BezeichnungAL" + i, item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
						sqlCommand.Parameters.AddWithValue("Blokiert_Status" + i, item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
						sqlCommand.Parameters.AddWithValue("CocVersion" + i, item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
						sqlCommand.Parameters.AddWithValue("COF_Pflichtig" + i, item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
						sqlCommand.Parameters.AddWithValue("CP_required" + i, item.CP_required == null ? (object)DBNull.Value : item.CP_required);
						sqlCommand.Parameters.AddWithValue("Crossreferenz" + i, item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
						sqlCommand.Parameters.AddWithValue("Cu_Gewicht" + i, item.CuGewicht == null ? (object)DBNull.Value : item.CuGewicht);
						sqlCommand.Parameters.AddWithValue("CustomerEnd" + i, item.CustomerEnd == null ? (object)DBNull.Value : item.CustomerEnd);
						sqlCommand.Parameters.AddWithValue("CustomerIndex" + i, item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
						sqlCommand.Parameters.AddWithValue("CustomerIndexSequence" + i, item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence" + i, item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerPrefix" + i, item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
						sqlCommand.Parameters.AddWithValue("CustomerTechnic" + i, item.CustomerTechnic == null ? (object)DBNull.Value : item.CustomerTechnic);
						sqlCommand.Parameters.AddWithValue("CustomerTechnicId" + i, item.CustomerTechnicId == null ? (object)DBNull.Value : item.CustomerTechnicId);
						sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand" + i, item.DatumAnfangsbestand == null ? (object)DBNull.Value : item.DatumAnfangsbestand);
						sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
						sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
						sqlCommand.Parameters.AddWithValue("Dienstelistung" + i, item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
						sqlCommand.Parameters.AddWithValue("Dokumente" + i, item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
						sqlCommand.Parameters.AddWithValue("EAN" + i, item.EAN == null ? (object)DBNull.Value : item.EAN);
						sqlCommand.Parameters.AddWithValue("EdiDefault" + i, item.EdiDefault == null ? (object)DBNull.Value : item.EdiDefault);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("EMPB" + i, item.EMPB == null ? (object)DBNull.Value : item.EMPB);
						sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben" + i, item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
						sqlCommand.Parameters.AddWithValue("Ersatzartikel" + i, item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
						sqlCommand.Parameters.AddWithValue("ESD_Schutz" + i, item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
						sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text" + i, item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
						sqlCommand.Parameters.AddWithValue("Exportgewicht" + i, item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
						sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste" + i, item.fakturierenStückliste == null ? (object)DBNull.Value : item.fakturierenStückliste);
						sqlCommand.Parameters.AddWithValue("Farbe" + i, item.Farbe == null ? (object)DBNull.Value : item.Farbe);
						sqlCommand.Parameters.AddWithValue("fibu_rahmen" + i, item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
						sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
						sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern" + i, item.FreigabestatusTNIntern == null ? (object)DBNull.Value : item.FreigabestatusTNIntern);
						sqlCommand.Parameters.AddWithValue("Gebinde" + i, item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
						sqlCommand.Parameters.AddWithValue("Gewicht" + i, item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
						sqlCommand.Parameters.AddWithValue("Grosse" + i, item.Größe == null ? (object)DBNull.Value : item.Größe);
						sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.GrundFürSperre == null ? (object)DBNull.Value : item.GrundFürSperre);
						sqlCommand.Parameters.AddWithValue("gultig_bis" + i, item.gültigBis == null ? (object)DBNull.Value : item.gültigBis);
						sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
						sqlCommand.Parameters.AddWithValue("Hubmastleitungen" + i, item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
						sqlCommand.Parameters.AddWithValue("ID_Klassifizierung" + i, item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
						sqlCommand.Parameters.AddWithValue("Info_WE" + i, item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
						sqlCommand.Parameters.AddWithValue("IsArticleNumberSpecial" + i, item.IsArticleNumberSpecial == null ? (object)DBNull.Value : item.IsArticleNumberSpecial);
						sqlCommand.Parameters.AddWithValue("IsEDrawing" + i, item.IsEDrawing == null ? (object)DBNull.Value : item.IsEDrawing);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Kategorie" + i, item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
						sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
						sqlCommand.Parameters.AddWithValue("Kriterium1" + i, item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
						sqlCommand.Parameters.AddWithValue("Kriterium2" + i, item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
						sqlCommand.Parameters.AddWithValue("Kriterium3" + i, item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
						sqlCommand.Parameters.AddWithValue("Kriterium4" + i, item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
						sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
						sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
						sqlCommand.Parameters.AddWithValue("Lagerartikel" + i, item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
						sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten" + i, item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
						sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB" + i, item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW" + i, item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
						sqlCommand.Parameters.AddWithValue("Lieferzeit" + i, item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
						sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
						sqlCommand.Parameters.AddWithValue("Manufacturer" + i, item.Manufacturer == null ? (object)DBNull.Value : item.Manufacturer);
						sqlCommand.Parameters.AddWithValue("ManufacturerNextArticle" + i, item.ManufacturerNextArticle == null ? (object)DBNull.Value : item.ManufacturerNextArticle);
						sqlCommand.Parameters.AddWithValue("ManufacturerNextArticleId" + i, item.ManufacturerNextArticleId == null ? (object)DBNull.Value : item.ManufacturerNextArticleId);
						sqlCommand.Parameters.AddWithValue("ManufacturerNumber" + i, item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
						sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticle" + i, item.ManufacturerPreviousArticle == null ? (object)DBNull.Value : item.ManufacturerPreviousArticle);
						sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticleId" + i, item.ManufacturerPreviousArticleId == null ? (object)DBNull.Value : item.ManufacturerPreviousArticleId);
						sqlCommand.Parameters.AddWithValue("Materialkosten_Alt" + i, item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
						sqlCommand.Parameters.AddWithValue("MHD" + i, item.MHD == null ? (object)DBNull.Value : item.MHD);
						sqlCommand.Parameters.AddWithValue("Minerals_Confirmity" + i, item.MineralsConfirmity == null ? (object)DBNull.Value : item.MineralsConfirmity);
						sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr" + i, item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
						sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr" + i, item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit" + i, item.proZeiteinheit == null ? (object)DBNull.Value : item.proZeiteinheit);
						sqlCommand.Parameters.AddWithValue("ProductionCountryCode" + i, item.ProductionCountryCode == null ? (object)DBNull.Value : item.ProductionCountryCode);
						sqlCommand.Parameters.AddWithValue("ProductionCountryName" + i, item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
						sqlCommand.Parameters.AddWithValue("ProductionCountrySequence" + i, item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
						sqlCommand.Parameters.AddWithValue("ProductionLotSize" + i, item.ProductionLotSize == null ? (object)DBNull.Value : item.ProductionLotSize);
						sqlCommand.Parameters.AddWithValue("ProductionSiteCode" + i, item.ProductionSiteCode == null ? (object)DBNull.Value : item.ProductionSiteCode);
						sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
						sqlCommand.Parameters.AddWithValue("ProductionSiteSequence" + i, item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
						sqlCommand.Parameters.AddWithValue("Produktionszeit" + i, item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
						sqlCommand.Parameters.AddWithValue("Projektname" + i, item.Projektname == null ? (object)DBNull.Value : item.Projektname);
						sqlCommand.Parameters.AddWithValue("Provisionsartikel" + i, item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
						sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware" + i, item.PrufstatusTNWare == null ? (object)DBNull.Value : item.PrufstatusTNWare);
						sqlCommand.Parameters.AddWithValue("Rabattierfahig" + i, item.Rabattierfähig == null ? (object)DBNull.Value : item.Rabattierfähig);
						sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
						sqlCommand.Parameters.AddWithValue("Rahmen2" + i, item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
						sqlCommand.Parameters.AddWithValue("Rahmenauslauf" + i, item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
						sqlCommand.Parameters.AddWithValue("Rahmenauslauf2" + i, item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
						sqlCommand.Parameters.AddWithValue("Rahmenmenge" + i, item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
						sqlCommand.Parameters.AddWithValue("Rahmenmenge2" + i, item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
						sqlCommand.Parameters.AddWithValue("Rahmen_Nr" + i, item.RahmenNr == null ? (object)DBNull.Value : item.RahmenNr);
						sqlCommand.Parameters.AddWithValue("Rahmen_Nr2" + i, item.RahmenNr2 == null ? (object)DBNull.Value : item.RahmenNr2);
						sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity" + i, item.REACHSVHCConfirmity == null ? (object)DBNull.Value : item.REACHSVHCConfirmity);
						sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity" + i, item.ROHSEEEConfirmity == null ? (object)DBNull.Value : item.ROHSEEEConfirmity);
						sqlCommand.Parameters.AddWithValue("Seriennummer" + i, item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
						sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung" + i, item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
						sqlCommand.Parameters.AddWithValue("Sonderrabatt" + i, item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
						sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id" + i, item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
						sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
						sqlCommand.Parameters.AddWithValue("Sysmonummer" + i, item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
						sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
						sqlCommand.Parameters.AddWithValue("UL_Etikett" + i, item.ULEtikett == null ? (object)DBNull.Value : item.ULEtikett);
						sqlCommand.Parameters.AddWithValue("UL_zertifiziert" + i, item.ULzertifiziert == null ? (object)DBNull.Value : item.ULzertifiziert);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Ursprungsland" + i, item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
						sqlCommand.Parameters.AddWithValue("VDA_1" + i, item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
						sqlCommand.Parameters.AddWithValue("VDA_2" + i, item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
						sqlCommand.Parameters.AddWithValue("Verpackung" + i, item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
						sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
						sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
						sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
						sqlCommand.Parameters.AddWithValue("Volumen" + i, item.Volumen == null ? (object)DBNull.Value : item.Volumen);
						sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
						sqlCommand.Parameters.AddWithValue("Warentyp" + i, item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
						sqlCommand.Parameters.AddWithValue("Webshop" + i, item.Webshop == null ? (object)DBNull.Value : item.Webshop);
						sqlCommand.Parameters.AddWithValue("Werkzeug" + i, item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
						sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand" + i, item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
						sqlCommand.Parameters.AddWithValue("Zeitraum_MHD" + i, item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
						sqlCommand.Parameters.AddWithValue("Zolltarif_nr" + i, item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
						sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Edit(Entities.Tables.PRS.ArtikelEntity element)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Artikel] SET [Abladestelle]=@Abladestelle, [aktiv]=@aktiv, [aktualisiert]=@aktualisiert, [Anfangsbestand]=@Anfangsbestand, [ArticleNumber]=@ArticleNumber, [Artikel aus eigener Produktion]=@Artikel_aus_eigener_Produktion, [Artikel für weitere Bestellungen sperren]=@Artikel_fur_weitere_Bestellungen_sperren, [Artikelbezeichnung]=@Artikelbezeichnung, [Artikelfamilie_Kunde]=@Artikelfamilie_Kunde, [Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1, [Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2, [artikelklassifizierung]=@artikelklassifizierung, [Artikelkurztext]=@Artikelkurztext, [Artikelnummer]=@Artikelnummer, [Barverkauf]=@Barverkauf, [BemerkungCRP]=@BemerkungCRP,[BemerkungCRPPlanung]=@BemerkungCRPPlanung, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 2]=@Bezeichnung_2, [Bezeichnung 3]=@Bezeichnung_3, [BezeichnungAL]=@BezeichnungAL, [Blokiert_Status]=@Blokiert_Status, [CocVersion]=@CocVersion, [COF_Pflichtig]=@COF_Pflichtig, [CP_required]=@CP_required, [Crossreferenz]=@Crossreferenz, [Cu-Gewicht]=@Cu_Gewicht, [CustomerEnd]=@CustomerEnd, [CustomerIndex]=@CustomerIndex, [CustomerIndexSequence]=@CustomerIndexSequence, [CustomerItemNumber]=@CustomerItemNumber, [CustomerItemNumberSequence]=@CustomerItemNumberSequence, [CustomerNumber]=@CustomerNumber, [CustomerPrefix]=@CustomerPrefix, [CustomerTechnic]=@CustomerTechnic, [CustomerTechnicId]=@CustomerTechnicId, [Datum Anfangsbestand]=@Datum_Anfangsbestand, [DEL]=@DEL, [DEL fixiert]=@DEL_fixiert, [Dienstelistung]=@Dienstelistung, [Dokumente]=@Dokumente, [EAN]=@EAN, [EdiDefault]=@EdiDefault, [Einheit]=@Einheit, [EMPB]=@EMPB, [EMPB_Freigegeben]=@EMPB_Freigegeben, [Ersatzartikel]=@Ersatzartikel, [ESD_Schutz]=@ESD_Schutz, [ESD_Schutz_Text]=@ESD_Schutz_Text, [Exportgewicht]=@Exportgewicht, [fakturieren Stückliste]=@fakturieren_Stuckliste, [Farbe]=@Farbe, [fibu_rahmen]=@fibu_rahmen, [Freigabestatus]=@Freigabestatus, [Freigabestatus TN intern]=@Freigabestatus_TN_intern, [Gebinde]=@Gebinde, [Gewicht]=@Gewicht, [Größe]=@Grosse, [Grund für Sperre]=@Grund_fur_Sperre, [gültig bis]=@gultig_bis, [Halle]=@Halle, [Hubmastleitungen]=@Hubmastleitungen, [ID_Klassifizierung]=@ID_Klassifizierung, [Index_Kunde]=@Index_Kunde, [Index_Kunde_Datum]=@Index_Kunde_Datum, [Info_WE]=@Info_WE, [IsArticleNumberSpecial]=@IsArticleNumberSpecial, [IsEDrawing]=@IsEDrawing, [Kanban]=@Kanban, [Kategorie]=@Kategorie, [Klassifizierung]=@Klassifizierung, [Kriterium1]=@Kriterium1, [Kriterium2]=@Kriterium2, [Kriterium3]=@Kriterium3, [Kriterium4]=@Kriterium4, [Kupferbasis]=@Kupferbasis, [Kupferzahl]=@Kupferzahl, [Lagerartikel]=@Lagerartikel, [Lagerhaltungskosten]=@Lagerhaltungskosten, [Langtext]=@Langtext, [Langtext_drucken_AB]=@Langtext_drucken_AB, [Langtext_drucken_BW]=@Langtext_drucken_BW, [Lieferzeit]=@Lieferzeit, [Losgroesse]=@Losgroesse, [Manufacturer]=@Manufacturer, [ManufacturerNextArticle]=@ManufacturerNextArticle, [ManufacturerNextArticleId]=@ManufacturerNextArticleId, [ManufacturerNumber]=@ManufacturerNumber, [ManufacturerPreviousArticle]=@ManufacturerPreviousArticle, [ManufacturerPreviousArticleId]=@ManufacturerPreviousArticleId, [Materialkosten_Alt]=@Materialkosten_Alt, [MHD]=@MHD, [Minerals Confirmity]=@Minerals_Confirmity, [Praeferenz_Aktuelles_jahr]=@Praeferenz_Aktuelles_jahr, [Praeferenz_Folgejahr]=@Praeferenz_Folgejahr, [Preiseinheit]=@Preiseinheit, [pro Zeiteinheit]=@pro_Zeiteinheit, [ProductionCountryCode]=@ProductionCountryCode, [ProductionCountryName]=@ProductionCountryName, [ProductionCountrySequence]=@ProductionCountrySequence, [ProductionLotSize]=@ProductionLotSize, [ProductionSiteCode]=@ProductionSiteCode, [ProductionSiteName]=@ProductionSiteName, [ProductionSiteSequence]=@ProductionSiteSequence, [Produktionszeit]=@Produktionszeit, [Projektname]=@Projektname, [Provisionsartikel]=@Provisionsartikel, [Prüfstatus TN Ware]=@Prufstatus_TN_Ware, [Rabattierfähig]=@Rabattierfahig, [Rahmen]=@Rahmen, [Rahmen2]=@Rahmen2, [Rahmenauslauf]=@Rahmenauslauf, [Rahmenauslauf2]=@Rahmenauslauf2, [Rahmenmenge]=@Rahmenmenge, [Rahmenmenge2]=@Rahmenmenge2, [Rahmen-Nr]=@Rahmen_Nr, [Rahmen-Nr2]=@Rahmen_Nr2, [REACH SVHC Confirmity]=@REACH_SVHC_Confirmity, [ROHS EEE Confirmity]=@ROHS_EEE_Confirmity, [Seriennummer]=@Seriennummer, [Seriennummernverwaltung]=@Seriennummernverwaltung, [Sonderrabatt]=@Sonderrabatt, [Standard_Lagerort_id]=@Standard_Lagerort_id, [Stückliste]=@Stuckliste, [Stundensatz]=@Stundensatz, [Sysmonummer]=@Sysmonummer, [UBG]=@UBG, [UL Etikett]=@UL_Etikett, [UL zertifiziert]=@UL_zertifiziert, [Umsatzsteuer]=@Umsatzsteuer, [Ursprungsland]=@Ursprungsland, [VDA_1]=@VDA_1, [VDA_2]=@VDA_2, [Verpackung]=@Verpackung, [Verpackungsart]=@Verpackungsart, [Verpackungsmenge]=@Verpackungsmenge, [VK-Festpreis]=@VK_Festpreis, [Volumen]=@Volumen, [Warengruppe]=@Warengruppe, [Warentyp]=@Warentyp, [Webshop]=@Webshop, [Werkzeug]=@Werkzeug, [Wert_Anfangsbestand]=@Wert_Anfangsbestand, [Zeichnungsnummer]=@Zeichnungsnummer, [Zeitraum_MHD]=@Zeitraum_MHD, [Zolltarif_nr]=@Zolltarif_nr, [Zuschlag_VK]=@Zuschlag_VK WHERE [Artikel-Nr]=@Artikel_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Artikel_Nr", element.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Abladestelle", element.Abladestelle == null ? (object)DBNull.Value : element.Abladestelle);
				sqlCommand.Parameters.AddWithValue("aktiv", element.aktiv == null ? (object)DBNull.Value : element.aktiv);
				sqlCommand.Parameters.AddWithValue("aktualisiert", element.aktualisiert == null ? (object)DBNull.Value : element.aktualisiert);
				sqlCommand.Parameters.AddWithValue("Anfangsbestand", element.Anfangsbestand == null ? (object)DBNull.Value : element.Anfangsbestand);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", element.ArticleNumber == null ? (object)DBNull.Value : element.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", element.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : element.ArtikelAusEigenerProduktion);
				sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren", element.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : element.ArtikelFürWeitereBestellungenSperren);
				sqlCommand.Parameters.AddWithValue("Artikelbezeichnung", element.Artikelbezeichnung == null ? (object)DBNull.Value : element.Artikelbezeichnung);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", element.Artikelfamilie_Kunde == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", element.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail1);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", element.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail2);
				sqlCommand.Parameters.AddWithValue("artikelklassifizierung", element.artikelklassifizierung == null ? (object)DBNull.Value : element.artikelklassifizierung);
				sqlCommand.Parameters.AddWithValue("Artikelkurztext", element.Artikelkurztext == null ? (object)DBNull.Value : element.Artikelkurztext);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", element.ArtikelNummer == null ? (object)DBNull.Value : element.ArtikelNummer);
				sqlCommand.Parameters.AddWithValue("Barverkauf", element.Barverkauf == null ? (object)DBNull.Value : element.Barverkauf);
				sqlCommand.Parameters.AddWithValue("BemerkungCRP", element.BemerkungCRP == null ? (object)DBNull.Value : element.BemerkungCRP);
				sqlCommand.Parameters.AddWithValue("BemerkungCRPPlanung", element.BemerkungCRPPlanung == null ? (object)DBNull.Value : element.BemerkungCRPPlanung);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", element.Bezeichnung1 == null ? (object)DBNull.Value : element.Bezeichnung1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_2", element.Bezeichnung2 == null ? (object)DBNull.Value : element.Bezeichnung2);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_3", element.Bezeichnung3 == null ? (object)DBNull.Value : element.Bezeichnung3);
				sqlCommand.Parameters.AddWithValue("BezeichnungAL", element.BezeichnungAL == null ? (object)DBNull.Value : element.BezeichnungAL);
				sqlCommand.Parameters.AddWithValue("Blokiert_Status", element.Blokiert_Status == null ? (object)DBNull.Value : element.Blokiert_Status);
				sqlCommand.Parameters.AddWithValue("CocVersion", element.CocVersion == null ? (object)DBNull.Value : element.CocVersion);
				sqlCommand.Parameters.AddWithValue("COF_Pflichtig", element.COF_Pflichtig == null ? (object)DBNull.Value : element.COF_Pflichtig);
				sqlCommand.Parameters.AddWithValue("CP_required", element.CP_required == null ? (object)DBNull.Value : element.CP_required);
				sqlCommand.Parameters.AddWithValue("Crossreferenz", element.Crossreferenz == null ? (object)DBNull.Value : element.Crossreferenz);
				sqlCommand.Parameters.AddWithValue("Cu_Gewicht", element.CuGewicht == null ? (object)DBNull.Value : element.CuGewicht);
				sqlCommand.Parameters.AddWithValue("CustomerEnd", element.CustomerEnd == null ? (object)DBNull.Value : element.CustomerEnd);
				sqlCommand.Parameters.AddWithValue("CustomerIndex", element.CustomerIndex == null ? (object)DBNull.Value : element.CustomerIndex);
				sqlCommand.Parameters.AddWithValue("CustomerIndexSequence", element.CustomerIndexSequence == null ? (object)DBNull.Value : element.CustomerIndexSequence);
				sqlCommand.Parameters.AddWithValue("CustomerItemNumber", element.CustomerItemNumber == null ? (object)DBNull.Value : element.CustomerItemNumber);
				sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence", element.CustomerItemNumberSequence == null ? (object)DBNull.Value : element.CustomerItemNumberSequence);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", element.CustomerNumber == null ? (object)DBNull.Value : element.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("CustomerPrefix", element.CustomerPrefix == null ? (object)DBNull.Value : element.CustomerPrefix);
				sqlCommand.Parameters.AddWithValue("CustomerTechnic", element.CustomerTechnic == null ? (object)DBNull.Value : element.CustomerTechnic);
				sqlCommand.Parameters.AddWithValue("CustomerTechnicId", element.CustomerTechnicId == null ? (object)DBNull.Value : element.CustomerTechnicId);
				sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", element.DatumAnfangsbestand == null ? (object)DBNull.Value : element.DatumAnfangsbestand);
				sqlCommand.Parameters.AddWithValue("DEL", element.DEL == null ? (object)DBNull.Value : element.DEL);
				sqlCommand.Parameters.AddWithValue("DEL_fixiert", element.DELFixiert == null ? (object)DBNull.Value : element.DELFixiert);
				sqlCommand.Parameters.AddWithValue("Dienstelistung", element.Dienstelistung == null ? (object)DBNull.Value : element.Dienstelistung);
				sqlCommand.Parameters.AddWithValue("Dokumente", element.Dokumente == null ? (object)DBNull.Value : element.Dokumente);
				sqlCommand.Parameters.AddWithValue("EAN", element.EAN == null ? (object)DBNull.Value : element.EAN);
				sqlCommand.Parameters.AddWithValue("EdiDefault", element.EdiDefault == null ? (object)DBNull.Value : element.EdiDefault);
				sqlCommand.Parameters.AddWithValue("Einheit", element.Einheit == null ? (object)DBNull.Value : element.Einheit);
				sqlCommand.Parameters.AddWithValue("EMPB", element.EMPB == null ? (object)DBNull.Value : element.EMPB);
				sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben", element.EMPB_Freigegeben == null ? (object)DBNull.Value : element.EMPB_Freigegeben);
				sqlCommand.Parameters.AddWithValue("Ersatzartikel", element.Ersatzartikel == null ? (object)DBNull.Value : element.Ersatzartikel);
				sqlCommand.Parameters.AddWithValue("ESD_Schutz", element.ESD_Schutz == null ? (object)DBNull.Value : element.ESD_Schutz);
				sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text", element.ESD_Schutz_Text == null ? (object)DBNull.Value : element.ESD_Schutz_Text);
				sqlCommand.Parameters.AddWithValue("Exportgewicht", element.Exportgewicht == null ? (object)DBNull.Value : element.Exportgewicht);
				sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste", element.fakturierenStückliste == null ? (object)DBNull.Value : element.fakturierenStückliste);
				sqlCommand.Parameters.AddWithValue("Farbe", element.Farbe == null ? (object)DBNull.Value : element.Farbe);
				sqlCommand.Parameters.AddWithValue("fibu_rahmen", element.fibu_rahmen == null ? (object)DBNull.Value : element.fibu_rahmen);
				sqlCommand.Parameters.AddWithValue("Freigabestatus", element.Freigabestatus == null ? (object)DBNull.Value : element.Freigabestatus);
				sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", element.FreigabestatusTNIntern == null ? (object)DBNull.Value : element.FreigabestatusTNIntern);
				sqlCommand.Parameters.AddWithValue("Gebinde", element.Gebinde == null ? (object)DBNull.Value : element.Gebinde);
				sqlCommand.Parameters.AddWithValue("Gewicht", element.Gewicht == null ? (object)DBNull.Value : element.Gewicht);
				sqlCommand.Parameters.AddWithValue("Grosse", element.Größe == null ? (object)DBNull.Value : element.Größe);
				sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", element.GrundFürSperre == null ? (object)DBNull.Value : element.GrundFürSperre);
				sqlCommand.Parameters.AddWithValue("gultig_bis", element.gültigBis == null ? (object)DBNull.Value : element.gültigBis);
				sqlCommand.Parameters.AddWithValue("Halle", element.Halle == null ? (object)DBNull.Value : element.Halle);
				sqlCommand.Parameters.AddWithValue("Hubmastleitungen", element.Hubmastleitungen == null ? (object)DBNull.Value : element.Hubmastleitungen);
				sqlCommand.Parameters.AddWithValue("ID_Klassifizierung", element.ID_Klassifizierung == null ? (object)DBNull.Value : element.ID_Klassifizierung);
				sqlCommand.Parameters.AddWithValue("Index_Kunde", element.Index_Kunde == null ? (object)DBNull.Value : element.Index_Kunde);
				sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", element.Index_Kunde_Datum == null ? (object)DBNull.Value : element.Index_Kunde_Datum);
				sqlCommand.Parameters.AddWithValue("Info_WE", element.Info_WE == null ? (object)DBNull.Value : element.Info_WE);
				sqlCommand.Parameters.AddWithValue("IsArticleNumberSpecial", element.IsArticleNumberSpecial == null ? (object)DBNull.Value : element.IsArticleNumberSpecial);
				sqlCommand.Parameters.AddWithValue("IsEDrawing", element.IsEDrawing == null ? (object)DBNull.Value : element.IsEDrawing);
				sqlCommand.Parameters.AddWithValue("Kanban", element.Kanban == null ? (object)DBNull.Value : element.Kanban);
				sqlCommand.Parameters.AddWithValue("Kategorie", element.Kategorie == null ? (object)DBNull.Value : element.Kategorie);
				sqlCommand.Parameters.AddWithValue("Klassifizierung", element.Klassifizierung == null ? (object)DBNull.Value : element.Klassifizierung);
				sqlCommand.Parameters.AddWithValue("Kriterium1", element.Kriterium1 == null ? (object)DBNull.Value : element.Kriterium1);
				sqlCommand.Parameters.AddWithValue("Kriterium2", element.Kriterium2 == null ? (object)DBNull.Value : element.Kriterium2);
				sqlCommand.Parameters.AddWithValue("Kriterium3", element.Kriterium3 == null ? (object)DBNull.Value : element.Kriterium3);
				sqlCommand.Parameters.AddWithValue("Kriterium4", element.Kriterium4 == null ? (object)DBNull.Value : element.Kriterium4);
				sqlCommand.Parameters.AddWithValue("Kupferbasis", element.Kupferbasis == null ? (object)DBNull.Value : element.Kupferbasis);
				sqlCommand.Parameters.AddWithValue("Kupferzahl", element.Kupferzahl == null ? (object)DBNull.Value : element.Kupferzahl);
				sqlCommand.Parameters.AddWithValue("Lagerartikel", element.Lagerartikel == null ? (object)DBNull.Value : element.Lagerartikel);
				sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", element.Lagerhaltungskosten == null ? (object)DBNull.Value : element.Lagerhaltungskosten);
				sqlCommand.Parameters.AddWithValue("Langtext", element.Langtext == null ? (object)DBNull.Value : element.Langtext);
				sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", element.Langtext_drucken_AB == null ? (object)DBNull.Value : element.Langtext_drucken_AB);
				sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", element.Langtext_drucken_BW == null ? (object)DBNull.Value : element.Langtext_drucken_BW);
				sqlCommand.Parameters.AddWithValue("Lieferzeit", element.Lieferzeit == null ? (object)DBNull.Value : element.Lieferzeit);
				sqlCommand.Parameters.AddWithValue("Losgroesse", element.Losgroesse == null ? (object)DBNull.Value : element.Losgroesse);
				sqlCommand.Parameters.AddWithValue("Manufacturer", element.Manufacturer == null ? (object)DBNull.Value : element.Manufacturer);
				sqlCommand.Parameters.AddWithValue("ManufacturerNextArticle", element.ManufacturerNextArticle == null ? (object)DBNull.Value : element.ManufacturerNextArticle);
				sqlCommand.Parameters.AddWithValue("ManufacturerNextArticleId", element.ManufacturerNextArticleId == null ? (object)DBNull.Value : element.ManufacturerNextArticleId);
				sqlCommand.Parameters.AddWithValue("ManufacturerNumber", element.ManufacturerNumber == null ? (object)DBNull.Value : element.ManufacturerNumber);
				sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticle", element.ManufacturerPreviousArticle == null ? (object)DBNull.Value : element.ManufacturerPreviousArticle);
				sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticleId", element.ManufacturerPreviousArticleId == null ? (object)DBNull.Value : element.ManufacturerPreviousArticleId);
				sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", element.Materialkosten_Alt == null ? (object)DBNull.Value : element.Materialkosten_Alt);
				sqlCommand.Parameters.AddWithValue("MHD", element.MHD == null ? (object)DBNull.Value : element.MHD);
				sqlCommand.Parameters.AddWithValue("Minerals_Confirmity", element.MineralsConfirmity == null ? (object)DBNull.Value : element.MineralsConfirmity);
				sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr", element.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : element.Praeferenz_Aktuelles_jahr);
				sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr", element.Praeferenz_Folgejahr == null ? (object)DBNull.Value : element.Praeferenz_Folgejahr);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", element.Preiseinheit == null ? (object)DBNull.Value : element.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", element.proZeiteinheit == null ? (object)DBNull.Value : element.proZeiteinheit);
				sqlCommand.Parameters.AddWithValue("ProductionCountryCode", element.ProductionCountryCode == null ? (object)DBNull.Value : element.ProductionCountryCode);
				sqlCommand.Parameters.AddWithValue("ProductionCountryName", element.ProductionCountryName == null ? (object)DBNull.Value : element.ProductionCountryName);
				sqlCommand.Parameters.AddWithValue("ProductionCountrySequence", element.ProductionCountrySequence == null ? (object)DBNull.Value : element.ProductionCountrySequence);
				sqlCommand.Parameters.AddWithValue("ProductionLotSize", element.ProductionLotSize == null ? (object)DBNull.Value : element.ProductionLotSize);
				sqlCommand.Parameters.AddWithValue("ProductionSiteCode", element.ProductionSiteCode == null ? (object)DBNull.Value : element.ProductionSiteCode);
				sqlCommand.Parameters.AddWithValue("ProductionSiteName", element.ProductionSiteName == null ? (object)DBNull.Value : element.ProductionSiteName);
				sqlCommand.Parameters.AddWithValue("ProductionSiteSequence", element.ProductionSiteSequence == null ? (object)DBNull.Value : element.ProductionSiteSequence);
				sqlCommand.Parameters.AddWithValue("Produktionszeit", element.Produktionszeit == null ? (object)DBNull.Value : element.Produktionszeit);
				sqlCommand.Parameters.AddWithValue("Projektname", element.Projektname == null ? (object)DBNull.Value : element.Projektname);
				sqlCommand.Parameters.AddWithValue("Provisionsartikel", element.Provisionsartikel == null ? (object)DBNull.Value : element.Provisionsartikel);
				sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware", element.PrufstatusTNWare == null ? (object)DBNull.Value : element.PrufstatusTNWare);
				sqlCommand.Parameters.AddWithValue("Rabattierfahig", element.Rabattierfähig == null ? (object)DBNull.Value : element.Rabattierfähig);
				sqlCommand.Parameters.AddWithValue("Rahmen", element.Rahmen == null ? (object)DBNull.Value : element.Rahmen);
				sqlCommand.Parameters.AddWithValue("Rahmen2", element.Rahmen2 == null ? (object)DBNull.Value : element.Rahmen2);
				sqlCommand.Parameters.AddWithValue("Rahmenauslauf", element.Rahmenauslauf == null ? (object)DBNull.Value : element.Rahmenauslauf);
				sqlCommand.Parameters.AddWithValue("Rahmenauslauf2", element.Rahmenauslauf2 == null ? (object)DBNull.Value : element.Rahmenauslauf2);
				sqlCommand.Parameters.AddWithValue("Rahmenmenge", element.Rahmenmenge == null ? (object)DBNull.Value : element.Rahmenmenge);
				sqlCommand.Parameters.AddWithValue("Rahmenmenge2", element.Rahmenmenge2 == null ? (object)DBNull.Value : element.Rahmenmenge2);
				sqlCommand.Parameters.AddWithValue("Rahmen_Nr", element.RahmenNr == null ? (object)DBNull.Value : element.RahmenNr);
				sqlCommand.Parameters.AddWithValue("Rahmen_Nr2", element.RahmenNr2 == null ? (object)DBNull.Value : element.RahmenNr2);
				sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity", element.REACHSVHCConfirmity == null ? (object)DBNull.Value : element.REACHSVHCConfirmity);
				sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity", element.ROHSEEEConfirmity == null ? (object)DBNull.Value : element.ROHSEEEConfirmity);
				sqlCommand.Parameters.AddWithValue("Seriennummer", element.Seriennummer == null ? (object)DBNull.Value : element.Seriennummer);
				sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", element.Seriennummernverwaltung == null ? (object)DBNull.Value : element.Seriennummernverwaltung);
				sqlCommand.Parameters.AddWithValue("Sonderrabatt", element.Sonderrabatt == null ? (object)DBNull.Value : element.Sonderrabatt);
				sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", element.Standard_Lagerort_id == null ? (object)DBNull.Value : element.Standard_Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Stuckliste", element.Stuckliste == null ? (object)DBNull.Value : element.Stuckliste);
				sqlCommand.Parameters.AddWithValue("Stundensatz", element.Stundensatz == null ? (object)DBNull.Value : element.Stundensatz);
				sqlCommand.Parameters.AddWithValue("Sysmonummer", element.Sysmonummer == null ? (object)DBNull.Value : element.Sysmonummer);
				sqlCommand.Parameters.AddWithValue("UBG", element.UBG == null ? (object)DBNull.Value : element.UBG);
				sqlCommand.Parameters.AddWithValue("UL_Etikett", element.ULEtikett == null ? (object)DBNull.Value : element.ULEtikett);
				sqlCommand.Parameters.AddWithValue("UL_zertifiziert", element.ULzertifiziert == null ? (object)DBNull.Value : element.ULzertifiziert);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer", element.Umsatzsteuer == null ? (object)DBNull.Value : element.Umsatzsteuer);
				sqlCommand.Parameters.AddWithValue("Ursprungsland", element.Ursprungsland == null ? (object)DBNull.Value : element.Ursprungsland);
				sqlCommand.Parameters.AddWithValue("VDA_1", element.VDA_1 == null ? (object)DBNull.Value : element.VDA_1);
				sqlCommand.Parameters.AddWithValue("VDA_2", element.VDA_2 == null ? (object)DBNull.Value : element.VDA_2);
				sqlCommand.Parameters.AddWithValue("Verpackung", element.Verpackung == null ? (object)DBNull.Value : element.Verpackung);
				sqlCommand.Parameters.AddWithValue("Verpackungsart", element.Verpackungsart == null ? (object)DBNull.Value : element.Verpackungsart);
				sqlCommand.Parameters.AddWithValue("Verpackungsmenge", element.Verpackungsmenge == null ? (object)DBNull.Value : element.Verpackungsmenge);
				sqlCommand.Parameters.AddWithValue("VK_Festpreis", element.VKFestpreis == null ? (object)DBNull.Value : element.VKFestpreis);
				sqlCommand.Parameters.AddWithValue("Volumen", element.Volumen == null ? (object)DBNull.Value : element.Volumen);
				sqlCommand.Parameters.AddWithValue("Warengruppe", element.Warengruppe == null ? (object)DBNull.Value : element.Warengruppe);
				sqlCommand.Parameters.AddWithValue("Warentyp", element.Warentyp == null ? (object)DBNull.Value : element.Warentyp);
				sqlCommand.Parameters.AddWithValue("Webshop", element.Webshop == null ? (object)DBNull.Value : element.Webshop);
				sqlCommand.Parameters.AddWithValue("Werkzeug", element.Werkzeug == null ? (object)DBNull.Value : element.Werkzeug);
				sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", element.Wert_Anfangsbestand == null ? (object)DBNull.Value : element.Wert_Anfangsbestand);
				sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", element.Zeichnungsnummer == null ? (object)DBNull.Value : element.Zeichnungsnummer);
				sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", element.Zeitraum_MHD == null ? (object)DBNull.Value : element.Zeitraum_MHD);
				sqlCommand.Parameters.AddWithValue("Zolltarif_nr", element.Zolltarif_nr == null ? (object)DBNull.Value : element.Zolltarif_nr);
				sqlCommand.Parameters.AddWithValue("Zuschlag_VK", element.Zuschlag_VK == null ? (object)DBNull.Value : element.Zuschlag_VK);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Edit(List<Entities.Tables.PRS.ArtikelEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 109; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = edit(elements);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += edit(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += edit(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber));
				}
			}

			return -1;
		}
		private static int edit(List<Entities.Tables.PRS.ArtikelEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in elements)
					{
						i++;
						query += " UPDATE [Artikel] SET "

							+ "[Abladestelle]=@Abladestelle" + i + ","
							+ "[aktiv]=@aktiv" + i + ","
							+ "[aktualisiert]=@aktualisiert" + i + ","
							+ "[Anfangsbestand]=@Anfangsbestand" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[Artikel aus eigener Produktion]=@Artikel_aus_eigener_Produktion" + i + ","
							+ "[Artikel für weitere Bestellungen sperren]=@Artikel_fur_weitere_Bestellungen_sperren" + i + ","
							+ "[Artikelbezeichnung]=@Artikelbezeichnung" + i + ","
							+ "[Artikelfamilie_Kunde]=@Artikelfamilie_Kunde" + i + ","
							+ "[Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1" + i + ","
							+ "[Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2" + i + ","
							+ "[artikelklassifizierung]=@artikelklassifizierung" + i + ","
							+ "[Artikelkurztext]=@Artikelkurztext" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Barverkauf]=@Barverkauf" + i + ","
							+ "[BemerkungCRP]=@BemerkungCRP" + i + ","
							+ "[BemerkungCRPPlanung]=@BemerkungCRPPlanung" + i + ","
							+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
							+ "[Bezeichnung 2]=@Bezeichnung_2" + i + ","
							+ "[Bezeichnung 3]=@Bezeichnung_3" + i + ","
							+ "[BezeichnungAL]=@BezeichnungAL" + i + ","
							+ "[Blokiert_Status]=@Blokiert_Status" + i + ","
							+ "[CocVersion]=@CocVersion" + i + ","
							+ "[COF_Pflichtig]=@COF_Pflichtig" + i + ","
							+ "[CP_required]=@CP_required" + i + ","
							+ "[Crossreferenz]=@Crossreferenz" + i + ","
							+ "[Cu-Gewicht]=@Cu_Gewicht" + i + ","
							+ "[CustomerEnd]=@CustomerEnd" + i + ","
							+ "[CustomerIndex]=@CustomerIndex" + i + ","
							+ "[CustomerIndexSequence]=@CustomerIndexSequence" + i + ","
							+ "[CustomerItemNumber]=@CustomerItemNumber" + i + ","
							+ "[CustomerItemNumberSequence]=@CustomerItemNumberSequence" + i + ","
							+ "[CustomerNumber]=@CustomerNumber" + i + ","
							+ "[CustomerPrefix]=@CustomerPrefix" + i + ","
							+ "[CustomerTechnic]=@CustomerTechnic" + i + ","
							+ "[CustomerTechnicId]=@CustomerTechnicId" + i + ","
							+ "[Datum Anfangsbestand]=@Datum_Anfangsbestand" + i + ","
							+ "[DEL]=@DEL" + i + ","
							+ "[DEL fixiert]=@DEL_fixiert" + i + ","
							+ "[Dienstelistung]=@Dienstelistung" + i + ","
							+ "[Dokumente]=@Dokumente" + i + ","
							+ "[EAN]=@EAN" + i + ","
							+ "[EdiDefault]=@EdiDefault" + i + ","
							+ "[Einheit]=@Einheit" + i + ","
							+ "[EMPB]=@EMPB" + i + ","
							+ "[EMPB_Freigegeben]=@EMPB_Freigegeben" + i + ","
							+ "[Ersatzartikel]=@Ersatzartikel" + i + ","
							+ "[ESD_Schutz]=@ESD_Schutz" + i + ","
							+ "[ESD_Schutz_Text]=@ESD_Schutz_Text" + i + ","
							+ "[Exportgewicht]=@Exportgewicht" + i + ","
							+ "[fakturieren Stückliste]=@fakturieren_Stuckliste" + i + ","
							+ "[Farbe]=@Farbe" + i + ","
							+ "[fibu_rahmen]=@fibu_rahmen" + i + ","
							+ "[Freigabestatus]=@Freigabestatus" + i + ","
							+ "[Freigabestatus TN intern]=@Freigabestatus_TN_intern" + i + ","
							+ "[Gebinde]=@Gebinde" + i + ","
							+ "[Gewicht]=@Gewicht" + i + ","
							+ "[Größe]=@Grosse" + i + ","
							+ "[Grund für Sperre]=@Grund_fur_Sperre" + i + ","
							+ "[gültig bis]=@gultig_bis" + i + ","
							+ "[Halle]=@Halle" + i + ","
							+ "[Hubmastleitungen]=@Hubmastleitungen" + i + ","
							+ "[ID_Klassifizierung]=@ID_Klassifizierung" + i + ","
							+ "[Index_Kunde]=@Index_Kunde" + i + ","
							+ "[Index_Kunde_Datum]=@Index_Kunde_Datum" + i + ","
							+ "[Info_WE]=@Info_WE" + i + ","
							+ "[IsArticleNumberSpecial]=@IsArticleNumberSpecial" + i + ","
							+ "[IsEDrawing]=@IsEDrawing" + i + ","
							+ "[Kanban]=@Kanban" + i + ","
							+ "[Kategorie]=@Kategorie" + i + ","
							+ "[Klassifizierung]=@Klassifizierung" + i + ","
							+ "[Kriterium1]=@Kriterium1" + i + ","
							+ "[Kriterium2]=@Kriterium2" + i + ","
							+ "[Kriterium3]=@Kriterium3" + i + ","
							+ "[Kriterium4]=@Kriterium4" + i + ","
							+ "[Kupferbasis]=@Kupferbasis" + i + ","
							+ "[Kupferzahl]=@Kupferzahl" + i + ","
							+ "[Lagerartikel]=@Lagerartikel" + i + ","
							+ "[Lagerhaltungskosten]=@Lagerhaltungskosten" + i + ","
							+ "[Langtext]=@Langtext" + i + ","
							+ "[Langtext_drucken_AB]=@Langtext_drucken_AB" + i + ","
							+ "[Langtext_drucken_BW]=@Langtext_drucken_BW" + i + ","
							+ "[Lieferzeit]=@Lieferzeit" + i + ","
							+ "[Losgroesse]=@Losgroesse" + i + ","
							+ "[Manufacturer]=@Manufacturer" + i + ","
							+ "[ManufacturerNextArticle]=@ManufacturerNextArticle" + i + ","
							+ "[ManufacturerNextArticleId]=@ManufacturerNextArticleId" + i + ","
							+ "[ManufacturerNumber]=@ManufacturerNumber" + i + ","
							+ "[ManufacturerPreviousArticle]=@ManufacturerPreviousArticle" + i + ","
							+ "[ManufacturerPreviousArticleId]=@ManufacturerPreviousArticleId" + i + ","
							+ "[Materialkosten_Alt]=@Materialkosten_Alt" + i + ","
							+ "[MHD]=@MHD" + i + ","
							+ "[Minerals Confirmity]=@Minerals_Confirmity" + i + ","
							+ "[Praeferenz_Aktuelles_jahr]=@Praeferenz_Aktuelles_jahr" + i + ","
							+ "[Praeferenz_Folgejahr]=@Praeferenz_Folgejahr" + i + ","
							+ "[Preiseinheit]=@Preiseinheit" + i + ","
							+ "[pro Zeiteinheit]=@pro_Zeiteinheit" + i + ","
							+ "[ProductionCountryCode]=@ProductionCountryCode" + i + ","
							+ "[ProductionCountryName]=@ProductionCountryName" + i + ","
							+ "[ProductionCountrySequence]=@ProductionCountrySequence" + i + ","
							+ "[ProductionLotSize]=@ProductionLotSize" + i + ","
							+ "[ProductionSiteCode]=@ProductionSiteCode" + i + ","
							+ "[ProductionSiteName]=@ProductionSiteName" + i + ","
							+ "[ProductionSiteSequence]=@ProductionSiteSequence" + i + ","
							+ "[Produktionszeit]=@Produktionszeit" + i + ","
							+ "[Projektname]=@Projektname" + i + ","
							+ "[Provisionsartikel]=@Provisionsartikel" + i + ","
							+ "[Prüfstatus TN Ware]=@Prufstatus_TN_Ware" + i + ","
							+ "[Rabattierfähig]=@Rabattierfahig" + i + ","
							+ "[Rahmen]=@Rahmen" + i + ","
							+ "[Rahmen2]=@Rahmen2" + i + ","
							+ "[Rahmenauslauf]=@Rahmenauslauf" + i + ","
							+ "[Rahmenauslauf2]=@Rahmenauslauf2" + i + ","
							+ "[Rahmenmenge]=@Rahmenmenge" + i + ","
							+ "[Rahmenmenge2]=@Rahmenmenge2" + i + ","
							+ "[Rahmen-Nr]=@Rahmen_Nr" + i + ","
							+ "[Rahmen-Nr2]=@Rahmen_Nr2" + i + ","
							+ "[REACH SVHC Confirmity]=@REACH_SVHC_Confirmity" + i + ","
							+ "[ROHS EEE Confirmity]=@ROHS_EEE_Confirmity" + i + ","
							+ "[Seriennummer]=@Seriennummer" + i + ","
							+ "[Seriennummernverwaltung]=@Seriennummernverwaltung" + i + ","
							+ "[Sonderrabatt]=@Sonderrabatt" + i + ","
							+ "[Standard_Lagerort_id]=@Standard_Lagerort_id" + i + ","
							+ "[Stückliste]=@Stuckliste" + i + ","
							+ "[Stundensatz]=@Stundensatz" + i + ","
							+ "[Sysmonummer]=@Sysmonummer" + i + ","
							+ "[UBG]=@UBG" + i + ","
							+ "[UL Etikett]=@UL_Etikett" + i + ","
							+ "[UL zertifiziert]=@UL_zertifiziert" + i + ","
							+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
							+ "[Ursprungsland]=@Ursprungsland" + i + ","
							+ "[VDA_1]=@VDA_1" + i + ","
							+ "[VDA_2]=@VDA_2" + i + ","
							+ "[Verpackung]=@Verpackung" + i + ","
							+ "[Verpackungsart]=@Verpackungsart" + i + ","
							+ "[Verpackungsmenge]=@Verpackungsmenge" + i + ","
							+ "[VK-Festpreis]=@VK_Festpreis" + i + ","
							+ "[Volumen]=@Volumen" + i + ","
							+ "[Warengruppe]=@Warengruppe" + i + ","
							+ "[Warentyp]=@Warentyp" + i + ","
							+ "[Webshop]=@Webshop" + i + ","
							+ "[Werkzeug]=@Werkzeug" + i + ","
							+ "[Wert_Anfangsbestand]=@Wert_Anfangsbestand" + i + ","
							+ "[Zeichnungsnummer]=@Zeichnungsnummer" + i + ","
							+ "[Zeitraum_MHD]=@Zeitraum_MHD" + i + ","
							+ "[Zolltarif_nr]=@Zolltarif_nr" + i + ","
							+ "[Zuschlag_VK]=@Zuschlag_VK" + i + " WHERE [Artikel-Nr]=@Artikel_Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
						sqlCommand.Parameters.AddWithValue("aktiv" + i, item.aktiv == null ? (object)DBNull.Value : item.aktiv);
						sqlCommand.Parameters.AddWithValue("aktualisiert" + i, item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
						sqlCommand.Parameters.AddWithValue("Anfangsbestand" + i, item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion" + i, item.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : item.ArtikelAusEigenerProduktion);
						sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren" + i, item.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : item.ArtikelFürWeitereBestellungenSperren);
						sqlCommand.Parameters.AddWithValue("Artikelbezeichnung" + i, item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
						sqlCommand.Parameters.AddWithValue("artikelklassifizierung" + i, item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
						sqlCommand.Parameters.AddWithValue("Artikelkurztext" + i, item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
						sqlCommand.Parameters.AddWithValue("Barverkauf" + i, item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
						sqlCommand.Parameters.AddWithValue("BemerkungCRP" + i, item.BemerkungCRP == null ? (object)DBNull.Value : item.BemerkungCRP);
						sqlCommand.Parameters.AddWithValue("BemerkungCRPPlanung" + i, item.BemerkungCRPPlanung == null ? (object)DBNull.Value : item.BemerkungCRPPlanung);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_3" + i, item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
						sqlCommand.Parameters.AddWithValue("BezeichnungAL" + i, item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
						sqlCommand.Parameters.AddWithValue("Blokiert_Status" + i, item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
						sqlCommand.Parameters.AddWithValue("CocVersion" + i, item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
						sqlCommand.Parameters.AddWithValue("COF_Pflichtig" + i, item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
						sqlCommand.Parameters.AddWithValue("CP_required" + i, item.CP_required == null ? (object)DBNull.Value : item.CP_required);
						sqlCommand.Parameters.AddWithValue("Crossreferenz" + i, item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
						sqlCommand.Parameters.AddWithValue("Cu_Gewicht" + i, item.CuGewicht == null ? (object)DBNull.Value : item.CuGewicht);
						sqlCommand.Parameters.AddWithValue("CustomerEnd" + i, item.CustomerEnd == null ? (object)DBNull.Value : item.CustomerEnd);
						sqlCommand.Parameters.AddWithValue("CustomerIndex" + i, item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
						sqlCommand.Parameters.AddWithValue("CustomerIndexSequence" + i, item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence" + i, item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerPrefix" + i, item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
						sqlCommand.Parameters.AddWithValue("CustomerTechnic" + i, item.CustomerTechnic == null ? (object)DBNull.Value : item.CustomerTechnic);
						sqlCommand.Parameters.AddWithValue("CustomerTechnicId" + i, item.CustomerTechnicId == null ? (object)DBNull.Value : item.CustomerTechnicId);
						sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand" + i, item.DatumAnfangsbestand == null ? (object)DBNull.Value : item.DatumAnfangsbestand);
						sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
						sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
						sqlCommand.Parameters.AddWithValue("Dienstelistung" + i, item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
						sqlCommand.Parameters.AddWithValue("Dokumente" + i, item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
						sqlCommand.Parameters.AddWithValue("EAN" + i, item.EAN == null ? (object)DBNull.Value : item.EAN);
						sqlCommand.Parameters.AddWithValue("EdiDefault" + i, item.EdiDefault == null ? (object)DBNull.Value : item.EdiDefault);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("EMPB" + i, item.EMPB == null ? (object)DBNull.Value : item.EMPB);
						sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben" + i, item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
						sqlCommand.Parameters.AddWithValue("Ersatzartikel" + i, item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
						sqlCommand.Parameters.AddWithValue("ESD_Schutz" + i, item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
						sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text" + i, item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
						sqlCommand.Parameters.AddWithValue("Exportgewicht" + i, item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
						sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste" + i, item.fakturierenStückliste == null ? (object)DBNull.Value : item.fakturierenStückliste);
						sqlCommand.Parameters.AddWithValue("Farbe" + i, item.Farbe == null ? (object)DBNull.Value : item.Farbe);
						sqlCommand.Parameters.AddWithValue("fibu_rahmen" + i, item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
						sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
						sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern" + i, item.FreigabestatusTNIntern == null ? (object)DBNull.Value : item.FreigabestatusTNIntern);
						sqlCommand.Parameters.AddWithValue("Gebinde" + i, item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
						sqlCommand.Parameters.AddWithValue("Gewicht" + i, item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
						sqlCommand.Parameters.AddWithValue("Grosse" + i, item.Größe == null ? (object)DBNull.Value : item.Größe);
						sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.GrundFürSperre == null ? (object)DBNull.Value : item.GrundFürSperre);
						sqlCommand.Parameters.AddWithValue("gultig_bis" + i, item.gültigBis == null ? (object)DBNull.Value : item.gültigBis);
						sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
						sqlCommand.Parameters.AddWithValue("Hubmastleitungen" + i, item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
						sqlCommand.Parameters.AddWithValue("ID_Klassifizierung" + i, item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
						sqlCommand.Parameters.AddWithValue("Info_WE" + i, item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
						sqlCommand.Parameters.AddWithValue("IsArticleNumberSpecial" + i, item.IsArticleNumberSpecial == null ? (object)DBNull.Value : item.IsArticleNumberSpecial);
						sqlCommand.Parameters.AddWithValue("IsEDrawing" + i, item.IsEDrawing == null ? (object)DBNull.Value : item.IsEDrawing);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Kategorie" + i, item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
						sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
						sqlCommand.Parameters.AddWithValue("Kriterium1" + i, item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
						sqlCommand.Parameters.AddWithValue("Kriterium2" + i, item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
						sqlCommand.Parameters.AddWithValue("Kriterium3" + i, item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
						sqlCommand.Parameters.AddWithValue("Kriterium4" + i, item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
						sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
						sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
						sqlCommand.Parameters.AddWithValue("Lagerartikel" + i, item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
						sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten" + i, item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
						sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB" + i, item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW" + i, item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
						sqlCommand.Parameters.AddWithValue("Lieferzeit" + i, item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
						sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
						sqlCommand.Parameters.AddWithValue("Manufacturer" + i, item.Manufacturer == null ? (object)DBNull.Value : item.Manufacturer);
						sqlCommand.Parameters.AddWithValue("ManufacturerNextArticle" + i, item.ManufacturerNextArticle == null ? (object)DBNull.Value : item.ManufacturerNextArticle);
						sqlCommand.Parameters.AddWithValue("ManufacturerNextArticleId" + i, item.ManufacturerNextArticleId == null ? (object)DBNull.Value : item.ManufacturerNextArticleId);
						sqlCommand.Parameters.AddWithValue("ManufacturerNumber" + i, item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
						sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticle" + i, item.ManufacturerPreviousArticle == null ? (object)DBNull.Value : item.ManufacturerPreviousArticle);
						sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticleId" + i, item.ManufacturerPreviousArticleId == null ? (object)DBNull.Value : item.ManufacturerPreviousArticleId);
						sqlCommand.Parameters.AddWithValue("Materialkosten_Alt" + i, item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
						sqlCommand.Parameters.AddWithValue("MHD" + i, item.MHD == null ? (object)DBNull.Value : item.MHD);
						sqlCommand.Parameters.AddWithValue("Minerals_Confirmity" + i, item.MineralsConfirmity == null ? (object)DBNull.Value : item.MineralsConfirmity);
						sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr" + i, item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
						sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr" + i, item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit" + i, item.proZeiteinheit == null ? (object)DBNull.Value : item.proZeiteinheit);
						sqlCommand.Parameters.AddWithValue("ProductionCountryCode" + i, item.ProductionCountryCode == null ? (object)DBNull.Value : item.ProductionCountryCode);
						sqlCommand.Parameters.AddWithValue("ProductionCountryName" + i, item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
						sqlCommand.Parameters.AddWithValue("ProductionCountrySequence" + i, item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
						sqlCommand.Parameters.AddWithValue("ProductionLotSize" + i, item.ProductionLotSize == null ? (object)DBNull.Value : item.ProductionLotSize);
						sqlCommand.Parameters.AddWithValue("ProductionSiteCode" + i, item.ProductionSiteCode == null ? (object)DBNull.Value : item.ProductionSiteCode);
						sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
						sqlCommand.Parameters.AddWithValue("ProductionSiteSequence" + i, item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
						sqlCommand.Parameters.AddWithValue("Produktionszeit" + i, item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
						sqlCommand.Parameters.AddWithValue("Projektname" + i, item.Projektname == null ? (object)DBNull.Value : item.Projektname);
						sqlCommand.Parameters.AddWithValue("Provisionsartikel" + i, item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
						sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware" + i, item.PrufstatusTNWare == null ? (object)DBNull.Value : item.PrufstatusTNWare);
						sqlCommand.Parameters.AddWithValue("Rabattierfahig" + i, item.Rabattierfähig == null ? (object)DBNull.Value : item.Rabattierfähig);
						sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
						sqlCommand.Parameters.AddWithValue("Rahmen2" + i, item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
						sqlCommand.Parameters.AddWithValue("Rahmenauslauf" + i, item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
						sqlCommand.Parameters.AddWithValue("Rahmenauslauf2" + i, item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
						sqlCommand.Parameters.AddWithValue("Rahmenmenge" + i, item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
						sqlCommand.Parameters.AddWithValue("Rahmenmenge2" + i, item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
						sqlCommand.Parameters.AddWithValue("Rahmen_Nr" + i, item.RahmenNr == null ? (object)DBNull.Value : item.RahmenNr);
						sqlCommand.Parameters.AddWithValue("Rahmen_Nr2" + i, item.RahmenNr2 == null ? (object)DBNull.Value : item.RahmenNr2);
						sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity" + i, item.REACHSVHCConfirmity == null ? (object)DBNull.Value : item.REACHSVHCConfirmity);
						sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity" + i, item.ROHSEEEConfirmity == null ? (object)DBNull.Value : item.ROHSEEEConfirmity);
						sqlCommand.Parameters.AddWithValue("Seriennummer" + i, item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
						sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung" + i, item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
						sqlCommand.Parameters.AddWithValue("Sonderrabatt" + i, item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
						sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id" + i, item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
						sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
						sqlCommand.Parameters.AddWithValue("Sysmonummer" + i, item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
						sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
						sqlCommand.Parameters.AddWithValue("UL_Etikett" + i, item.ULEtikett == null ? (object)DBNull.Value : item.ULEtikett);
						sqlCommand.Parameters.AddWithValue("UL_zertifiziert" + i, item.ULzertifiziert == null ? (object)DBNull.Value : item.ULzertifiziert);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Ursprungsland" + i, item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
						sqlCommand.Parameters.AddWithValue("VDA_1" + i, item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
						sqlCommand.Parameters.AddWithValue("VDA_2" + i, item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
						sqlCommand.Parameters.AddWithValue("Verpackung" + i, item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
						sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
						sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
						sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
						sqlCommand.Parameters.AddWithValue("Volumen" + i, item.Volumen == null ? (object)DBNull.Value : item.Volumen);
						sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
						sqlCommand.Parameters.AddWithValue("Warentyp" + i, item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
						sqlCommand.Parameters.AddWithValue("Webshop" + i, item.Webshop == null ? (object)DBNull.Value : item.Webshop);
						sqlCommand.Parameters.AddWithValue("Werkzeug" + i, item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
						sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand" + i, item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
						sqlCommand.Parameters.AddWithValue("Zeitraum_MHD" + i, item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
						sqlCommand.Parameters.AddWithValue("Zolltarif_nr" + i, item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
						sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity GetWithTransaction(int artikel_nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Artikel] WHERE [Artikel-Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", artikel_nr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Artikel]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [Artikel] WHERE [Artikel-Nr] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Artikel] ([Abladestelle],[aktiv],[aktualisiert],[Anfangsbestand],[ArticleNumber],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelbezeichnung],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[artikelklassifizierung],[Artikelkurztext],[Artikelnummer],[Barverkauf],[BemerkungCRP],[BemerkungCRPPlanung],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[BezeichnungAL],[Blokiert_Status],[CocVersion],[COF_Pflichtig],[CP_required],[Crossreferenz],[Cu-Gewicht],[CustomerEnd],[CustomerIndex],[CustomerIndexSequence],[CustomerItemNumber],[CustomerItemNumberSequence],[CustomerNumber],[CustomerPrefix],[CustomerTechnic],[CustomerTechnicId],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dienstelistung],[Dokumente],[EAN],[EdiDefault],[Einheit],[EMPB],[EMPB_Freigegeben],[Ersatzartikel],[ESD_Schutz],[ESD_Schutz_Text],[Exportgewicht],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Hubmastleitungen],[ID_Klassifizierung],[Index_Kunde],[Index_Kunde_Datum],[Info_WE],[IsArticleNumberSpecial],[IsEDrawing],[Kanban],[Kategorie],[Klassifizierung],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Lieferzeit],[Losgroesse],[Manufacturer],[ManufacturerNextArticle],[ManufacturerNextArticleId],[ManufacturerNumber],[ManufacturerPreviousArticle],[ManufacturerPreviousArticleId],[Materialkosten_Alt],[MHD],[Minerals Confirmity],[Praeferenz_Aktuelles_jahr],[Praeferenz_Folgejahr],[Preiseinheit],[pro Zeiteinheit],[ProductionCountryCode],[ProductionCountryName],[ProductionCountrySequence],[ProductionLotSize],[ProductionSiteCode],[ProductionSiteName],[ProductionSiteSequence],[Produktionszeit],[Projektname],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmen2],[Rahmenauslauf],[Rahmenauslauf2],[Rahmenmenge],[Rahmenmenge2],[Rahmen-Nr],[Rahmen-Nr2],[REACH SVHC Confirmity],[ROHS EEE Confirmity],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UBG],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[VDA_1],[VDA_2],[Verpackung],[Verpackungsart],[Verpackungsmenge],[VK-Festpreis],[Volumen],[Warengruppe],[Warentyp],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zeitraum_MHD],[Zolltarif_nr],[Zuschlag_VK]) OUTPUT INSERTED.[Artikel-Nr] VALUES (@Abladestelle,@aktiv,@aktualisiert,@Anfangsbestand,@ArticleNumber,@Artikel_aus_eigener_Produktion,@Artikel_fur_weitere_Bestellungen_sperren,@Artikelbezeichnung,@Artikelfamilie_Kunde,@Artikelfamilie_Kunde_Detail1,@Artikelfamilie_Kunde_Detail2,@artikelklassifizierung,@Artikelkurztext,@Artikelnummer,@Barverkauf,@BemerkungCRP,@BemerkungCRPPlanung,@Bezeichnung_1,@Bezeichnung_2,@Bezeichnung_3,@BezeichnungAL,@Blokiert_Status,@CocVersion,@COF_Pflichtig,@CP_required,@Crossreferenz,@Cu_Gewicht,@CustomerEnd,@CustomerIndex,@CustomerIndexSequence,@CustomerItemNumber,@CustomerItemNumberSequence,@CustomerNumber,@CustomerPrefix,@CustomerTechnic,@CustomerTechnicId,@Datum_Anfangsbestand,@DEL,@DEL_fixiert,@Dienstelistung,@Dokumente,@EAN,@EdiDefault,@Einheit,@EMPB,@EMPB_Freigegeben,@Ersatzartikel,@ESD_Schutz,@ESD_Schutz_Text,@Exportgewicht,@fakturieren_Stuckliste,@Farbe,@fibu_rahmen,@Freigabestatus,@Freigabestatus_TN_intern,@Gebinde,@Gewicht,@Grosse,@Grund_fur_Sperre,@gultig_bis,@Halle,@Hubmastleitungen,@ID_Klassifizierung,@Index_Kunde,@Index_Kunde_Datum,@Info_WE,@IsArticleNumberSpecial,@IsEDrawing,@Kanban,@Kategorie,@Klassifizierung,@Kriterium1,@Kriterium2,@Kriterium3,@Kriterium4,@Kupferbasis,@Kupferzahl,@Lagerartikel,@Lagerhaltungskosten,@Langtext,@Langtext_drucken_AB,@Langtext_drucken_BW,@Lieferzeit,@Losgroesse,@Manufacturer,@ManufacturerNextArticle,@ManufacturerNextArticleId,@ManufacturerNumber,@ManufacturerPreviousArticle,@ManufacturerPreviousArticleId,@Materialkosten_Alt,@MHD,@Minerals_Confirmity,@Praeferenz_Aktuelles_jahr,@Praeferenz_Folgejahr,@Preiseinheit,@pro_Zeiteinheit,@ProductionCountryCode,@ProductionCountryName,@ProductionCountrySequence,@ProductionLotSize,@ProductionSiteCode,@ProductionSiteName,@ProductionSiteSequence,@Produktionszeit,@Projektname,@Provisionsartikel,@Prufstatus_TN_Ware,@Rabattierfahig,@Rahmen,@Rahmen2,@Rahmenauslauf,@Rahmenauslauf2,@Rahmenmenge,@Rahmenmenge2,@Rahmen_Nr,@Rahmen_Nr2,@REACH_SVHC_Confirmity,@ROHS_EEE_Confirmity,@Seriennummer,@Seriennummernverwaltung,@Sonderrabatt,@Standard_Lagerort_id,@Stuckliste,@Stundensatz,@Sysmonummer,@UBG,@UL_Etikett,@UL_zertifiziert,@Umsatzsteuer,@Ursprungsland,@VDA_1,@VDA_2,@Verpackung,@Verpackungsart,@Verpackungsmenge,@VK_Festpreis,@Volumen,@Warengruppe,@Warentyp,@Webshop,@Werkzeug,@Wert_Anfangsbestand,@Zeichnungsnummer,@Zeitraum_MHD,@Zolltarif_nr,@Zuschlag_VK); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
			sqlCommand.Parameters.AddWithValue("aktiv", item.aktiv == null ? (object)DBNull.Value : item.aktiv);
			sqlCommand.Parameters.AddWithValue("aktualisiert", item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
			sqlCommand.Parameters.AddWithValue("Anfangsbestand", item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", item.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : item.ArtikelAusEigenerProduktion);
			sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren", item.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : item.ArtikelFürWeitereBestellungenSperren);
			sqlCommand.Parameters.AddWithValue("Artikelbezeichnung", item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
			sqlCommand.Parameters.AddWithValue("artikelklassifizierung", item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
			sqlCommand.Parameters.AddWithValue("Artikelkurztext", item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
			sqlCommand.Parameters.AddWithValue("Barverkauf", item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
			sqlCommand.Parameters.AddWithValue("BemerkungCRP", item.BemerkungCRP == null ? (object)DBNull.Value : item.BemerkungCRP);
			sqlCommand.Parameters.AddWithValue("BemerkungCRPPlanung", item.BemerkungCRPPlanung == null ? (object)DBNull.Value : item.BemerkungCRPPlanung);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_3", item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
			sqlCommand.Parameters.AddWithValue("BezeichnungAL", item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
			sqlCommand.Parameters.AddWithValue("Blokiert_Status", item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
			sqlCommand.Parameters.AddWithValue("CocVersion", item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
			sqlCommand.Parameters.AddWithValue("COF_Pflichtig", item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
			sqlCommand.Parameters.AddWithValue("CP_required", item.CP_required == null ? (object)DBNull.Value : item.CP_required);
			sqlCommand.Parameters.AddWithValue("Crossreferenz", item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
			sqlCommand.Parameters.AddWithValue("Cu_Gewicht", item.CuGewicht == null ? (object)DBNull.Value : item.CuGewicht);
			sqlCommand.Parameters.AddWithValue("CustomerEnd", item.CustomerEnd == null ? (object)DBNull.Value : item.CustomerEnd);
			sqlCommand.Parameters.AddWithValue("CustomerIndex", item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
			sqlCommand.Parameters.AddWithValue("CustomerIndexSequence", item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
			sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
			sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence", item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("CustomerPrefix", item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
			sqlCommand.Parameters.AddWithValue("CustomerTechnic", item.CustomerTechnic == null ? (object)DBNull.Value : item.CustomerTechnic);
			sqlCommand.Parameters.AddWithValue("CustomerTechnicId", item.CustomerTechnicId == null ? (object)DBNull.Value : item.CustomerTechnicId);
			sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", item.DatumAnfangsbestand == null ? (object)DBNull.Value : item.DatumAnfangsbestand);
			sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
			sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
			sqlCommand.Parameters.AddWithValue("Dienstelistung", item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
			sqlCommand.Parameters.AddWithValue("Dokumente", item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
			sqlCommand.Parameters.AddWithValue("EAN", item.EAN == null ? (object)DBNull.Value : item.EAN);
			sqlCommand.Parameters.AddWithValue("EdiDefault", item.EdiDefault == null ? (object)DBNull.Value : item.EdiDefault);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("EMPB", item.EMPB == null ? (object)DBNull.Value : item.EMPB);
			sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben", item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
			sqlCommand.Parameters.AddWithValue("Ersatzartikel", item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
			sqlCommand.Parameters.AddWithValue("ESD_Schutz", item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
			sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text", item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
			sqlCommand.Parameters.AddWithValue("Exportgewicht", item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
			sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste", item.fakturierenStückliste == null ? (object)DBNull.Value : item.fakturierenStückliste);
			sqlCommand.Parameters.AddWithValue("Farbe", item.Farbe == null ? (object)DBNull.Value : item.Farbe);
			sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
			sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
			sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", item.FreigabestatusTNIntern == null ? (object)DBNull.Value : item.FreigabestatusTNIntern);
			sqlCommand.Parameters.AddWithValue("Gebinde", item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
			sqlCommand.Parameters.AddWithValue("Gewicht", item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
			sqlCommand.Parameters.AddWithValue("Grosse", item.Größe == null ? (object)DBNull.Value : item.Größe);
			sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", item.GrundFürSperre == null ? (object)DBNull.Value : item.GrundFürSperre);
			sqlCommand.Parameters.AddWithValue("gultig_bis", item.gültigBis == null ? (object)DBNull.Value : item.gültigBis);
			sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
			sqlCommand.Parameters.AddWithValue("Hubmastleitungen", item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
			sqlCommand.Parameters.AddWithValue("ID_Klassifizierung", item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
			sqlCommand.Parameters.AddWithValue("Info_WE", item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
			sqlCommand.Parameters.AddWithValue("IsArticleNumberSpecial", item.IsArticleNumberSpecial == null ? (object)DBNull.Value : item.IsArticleNumberSpecial);
			sqlCommand.Parameters.AddWithValue("IsEDrawing", item.IsEDrawing == null ? (object)DBNull.Value : item.IsEDrawing);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Kategorie", item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
			sqlCommand.Parameters.AddWithValue("Klassifizierung", item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Kriterium1", item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
			sqlCommand.Parameters.AddWithValue("Kriterium2", item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
			sqlCommand.Parameters.AddWithValue("Kriterium3", item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
			sqlCommand.Parameters.AddWithValue("Kriterium4", item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
			sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
			sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
			sqlCommand.Parameters.AddWithValue("Lagerartikel", item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
			sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
			sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
			sqlCommand.Parameters.AddWithValue("Lieferzeit", item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
			sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
			sqlCommand.Parameters.AddWithValue("Manufacturer", item.Manufacturer == null ? (object)DBNull.Value : item.Manufacturer);
			sqlCommand.Parameters.AddWithValue("ManufacturerNextArticle", item.ManufacturerNextArticle == null ? (object)DBNull.Value : item.ManufacturerNextArticle);
			sqlCommand.Parameters.AddWithValue("ManufacturerNextArticleId", item.ManufacturerNextArticleId == null ? (object)DBNull.Value : item.ManufacturerNextArticleId);
			sqlCommand.Parameters.AddWithValue("ManufacturerNumber", item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
			sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticle", item.ManufacturerPreviousArticle == null ? (object)DBNull.Value : item.ManufacturerPreviousArticle);
			sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticleId", item.ManufacturerPreviousArticleId == null ? (object)DBNull.Value : item.ManufacturerPreviousArticleId);
			sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
			sqlCommand.Parameters.AddWithValue("MHD", item.MHD == null ? (object)DBNull.Value : item.MHD);
			sqlCommand.Parameters.AddWithValue("Minerals_Confirmity", item.MineralsConfirmity == null ? (object)DBNull.Value : item.MineralsConfirmity);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr", item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr", item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", item.proZeiteinheit == null ? (object)DBNull.Value : item.proZeiteinheit);
			sqlCommand.Parameters.AddWithValue("ProductionCountryCode", item.ProductionCountryCode == null ? (object)DBNull.Value : item.ProductionCountryCode);
			sqlCommand.Parameters.AddWithValue("ProductionCountryName", item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
			sqlCommand.Parameters.AddWithValue("ProductionCountrySequence", item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
			sqlCommand.Parameters.AddWithValue("ProductionLotSize", item.ProductionLotSize == null ? (object)DBNull.Value : item.ProductionLotSize);
			sqlCommand.Parameters.AddWithValue("ProductionSiteCode", item.ProductionSiteCode == null ? (object)DBNull.Value : item.ProductionSiteCode);
			sqlCommand.Parameters.AddWithValue("ProductionSiteName", item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
			sqlCommand.Parameters.AddWithValue("ProductionSiteSequence", item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
			sqlCommand.Parameters.AddWithValue("Produktionszeit", item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
			sqlCommand.Parameters.AddWithValue("Projektname", item.Projektname == null ? (object)DBNull.Value : item.Projektname);
			sqlCommand.Parameters.AddWithValue("Provisionsartikel", item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
			sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware", item.PrufstatusTNWare == null ? (object)DBNull.Value : item.PrufstatusTNWare);
			sqlCommand.Parameters.AddWithValue("Rabattierfahig", item.Rabattierfähig == null ? (object)DBNull.Value : item.Rabattierfähig);
			sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
			sqlCommand.Parameters.AddWithValue("Rahmen2", item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf", item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf2", item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge", item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge2", item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
			sqlCommand.Parameters.AddWithValue("Rahmen_Nr", item.RahmenNr == null ? (object)DBNull.Value : item.RahmenNr);
			sqlCommand.Parameters.AddWithValue("Rahmen_Nr2", item.RahmenNr2 == null ? (object)DBNull.Value : item.RahmenNr2);
			sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity", item.REACHSVHCConfirmity == null ? (object)DBNull.Value : item.REACHSVHCConfirmity);
			sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity", item.ROHSEEEConfirmity == null ? (object)DBNull.Value : item.ROHSEEEConfirmity);
			sqlCommand.Parameters.AddWithValue("Seriennummer", item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
			sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
			sqlCommand.Parameters.AddWithValue("Sonderrabatt", item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
			sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Stuckliste", item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
			sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
			sqlCommand.Parameters.AddWithValue("Sysmonummer", item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
			sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
			sqlCommand.Parameters.AddWithValue("UL_Etikett", item.ULEtikett == null ? (object)DBNull.Value : item.ULEtikett);
			sqlCommand.Parameters.AddWithValue("UL_zertifiziert", item.ULzertifiziert == null ? (object)DBNull.Value : item.ULzertifiziert);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("Ursprungsland", item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
			sqlCommand.Parameters.AddWithValue("VDA_1", item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
			sqlCommand.Parameters.AddWithValue("VDA_2", item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
			sqlCommand.Parameters.AddWithValue("Verpackung", item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
			sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
			sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
			sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
			sqlCommand.Parameters.AddWithValue("Volumen", item.Volumen == null ? (object)DBNull.Value : item.Volumen);
			sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
			sqlCommand.Parameters.AddWithValue("Warentyp", item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
			sqlCommand.Parameters.AddWithValue("Webshop", item.Webshop == null ? (object)DBNull.Value : item.Webshop);
			sqlCommand.Parameters.AddWithValue("Werkzeug", item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
			sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
			sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
			sqlCommand.Parameters.AddWithValue("Zolltarif_nr", item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
			sqlCommand.Parameters.AddWithValue("Zuschlag_VK", item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 149; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}

			return -1;
		}
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Artikel] ([Abladestelle],[aktiv],[aktualisiert],[Anfangsbestand],[ArticleNumber],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelbezeichnung],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[artikelklassifizierung],[Artikelkurztext],[Artikelnummer],[Barverkauf],[BemerkungCRP],[BemerkungCRPPlanung],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[BezeichnungAL],[Blokiert_Status],[CocVersion],[COF_Pflichtig],[CP_required],[Crossreferenz],[Cu-Gewicht],[CustomerEnd],[CustomerIndex],[CustomerIndexSequence],[CustomerItemNumber],[CustomerItemNumberSequence],[CustomerNumber],[CustomerPrefix],[CustomerTechnic],[CustomerTechnicId],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dienstelistung],[Dokumente],[EAN],[EdiDefault],[Einheit],[EMPB],[EMPB_Freigegeben],[Ersatzartikel],[ESD_Schutz],[ESD_Schutz_Text],[Exportgewicht],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Hubmastleitungen],[ID_Klassifizierung],[Index_Kunde],[Index_Kunde_Datum],[Info_WE],[IsArticleNumberSpecial],[IsEDrawing],[Kanban],[Kategorie],[Klassifizierung],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Lieferzeit],[Losgroesse],[Manufacturer],[ManufacturerNextArticle],[ManufacturerNextArticleId],[ManufacturerNumber],[ManufacturerPreviousArticle],[ManufacturerPreviousArticleId],[Materialkosten_Alt],[MHD],[Minerals Confirmity],[Praeferenz_Aktuelles_jahr],[Praeferenz_Folgejahr],[Preiseinheit],[pro Zeiteinheit],[ProductionCountryCode],[ProductionCountryName],[ProductionCountrySequence],[ProductionLotSize],[ProductionSiteCode],[ProductionSiteName],[ProductionSiteSequence],[Produktionszeit],[Projektname],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmen2],[Rahmenauslauf],[Rahmenauslauf2],[Rahmenmenge],[Rahmenmenge2],[Rahmen-Nr],[Rahmen-Nr2],[REACH SVHC Confirmity],[ROHS EEE Confirmity],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UBG],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[VDA_1],[VDA_2],[Verpackung],[Verpackungsart],[Verpackungsmenge],[VK-Festpreis],[Volumen],[Warengruppe],[Warentyp],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zeitraum_MHD],[Zolltarif_nr],[Zuschlag_VK]) VALUES ( "

						+ "@Abladestelle" + i + ","
						+ "@aktiv" + i + ","
						+ "@aktualisiert" + i + ","
						+ "@Anfangsbestand" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@Artikel_aus_eigener_Produktion" + i + ","
						+ "@Artikel_fur_weitere_Bestellungen_sperren" + i + ","
						+ "@Artikelbezeichnung" + i + ","
						+ "@Artikelfamilie_Kunde" + i + ","
						+ "@Artikelfamilie_Kunde_Detail1" + i + ","
						+ "@Artikelfamilie_Kunde_Detail2" + i + ","
						+ "@artikelklassifizierung" + i + ","
						+ "@Artikelkurztext" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Barverkauf" + i + ","
						+ "@BemerkungCRP" + i + ","
						+ "@BemerkungCRPPlanung" + i + ","
						+ "@Bezeichnung_1" + i + ","
						+ "@Bezeichnung_2" + i + ","
						+ "@Bezeichnung_3" + i + ","
						+ "@BezeichnungAL" + i + ","
						+ "@Blokiert_Status" + i + ","
						+ "@CocVersion" + i + ","
						+ "@COF_Pflichtig" + i + ","
						+ "@CP_required" + i + ","
						+ "@Crossreferenz" + i + ","
						+ "@Cu_Gewicht" + i + ","
						+ "@CustomerEnd" + i + ","
						+ "@CustomerIndex" + i + ","
						+ "@CustomerIndexSequence" + i + ","
						+ "@CustomerItemNumber" + i + ","
						+ "@CustomerItemNumberSequence" + i + ","
						+ "@CustomerNumber" + i + ","
						+ "@CustomerPrefix" + i + ","
						+ "@CustomerTechnic" + i + ","
						+ "@CustomerTechnicId" + i + ","
						+ "@Datum_Anfangsbestand" + i + ","
						+ "@DEL" + i + ","
						+ "@DEL_fixiert" + i + ","
						+ "@Dienstelistung" + i + ","
						+ "@Dokumente" + i + ","
						+ "@EAN" + i + ","
						+ "@EdiDefault" + i + ","
						+ "@Einheit" + i + ","
						+ "@EMPB" + i + ","
						+ "@EMPB_Freigegeben" + i + ","
						+ "@Ersatzartikel" + i + ","
						+ "@ESD_Schutz" + i + ","
						+ "@ESD_Schutz_Text" + i + ","
						+ "@Exportgewicht" + i + ","
						+ "@fakturieren_Stuckliste" + i + ","
						+ "@Farbe" + i + ","
						+ "@fibu_rahmen" + i + ","
						+ "@Freigabestatus" + i + ","
						+ "@Freigabestatus_TN_intern" + i + ","
						+ "@Gebinde" + i + ","
						+ "@Gewicht" + i + ","
						+ "@Grosse" + i + ","
						+ "@Grund_fur_Sperre" + i + ","
						+ "@gultig_bis" + i + ","
						+ "@Halle" + i + ","
						+ "@Hubmastleitungen" + i + ","
						+ "@ID_Klassifizierung" + i + ","
						+ "@Index_Kunde" + i + ","
						+ "@Index_Kunde_Datum" + i + ","
						+ "@Info_WE" + i + ","
						+ "@IsArticleNumberSpecial" + i + ","
						+ "@IsEDrawing" + i + ","
						+ "@Kanban" + i + ","
						+ "@Kategorie" + i + ","
						+ "@Klassifizierung" + i + ","
						+ "@Kriterium1" + i + ","
						+ "@Kriterium2" + i + ","
						+ "@Kriterium3" + i + ","
						+ "@Kriterium4" + i + ","
						+ "@Kupferbasis" + i + ","
						+ "@Kupferzahl" + i + ","
						+ "@Lagerartikel" + i + ","
						+ "@Lagerhaltungskosten" + i + ","
						+ "@Langtext" + i + ","
						+ "@Langtext_drucken_AB" + i + ","
						+ "@Langtext_drucken_BW" + i + ","
						+ "@Lieferzeit" + i + ","
						+ "@Losgroesse" + i + ","
						+ "@Manufacturer" + i + ","
						+ "@ManufacturerNextArticle" + i + ","
						+ "@ManufacturerNextArticleId" + i + ","
						+ "@ManufacturerNumber" + i + ","
						+ "@ManufacturerPreviousArticle" + i + ","
						+ "@ManufacturerPreviousArticleId" + i + ","
						+ "@Materialkosten_Alt" + i + ","
						+ "@MHD" + i + ","
						+ "@Minerals_Confirmity" + i + ","
						+ "@Praeferenz_Aktuelles_jahr" + i + ","
						+ "@Praeferenz_Folgejahr" + i + ","
						+ "@Preiseinheit" + i + ","
						+ "@pro_Zeiteinheit" + i + ","
						+ "@ProductionCountryCode" + i + ","
						+ "@ProductionCountryName" + i + ","
						+ "@ProductionCountrySequence" + i + ","
						+ "@ProductionLotSize" + i + ","
						+ "@ProductionSiteCode" + i + ","
						+ "@ProductionSiteName" + i + ","
						+ "@ProductionSiteSequence" + i + ","
						+ "@Produktionszeit" + i + ","
						+ "@Projektname" + i + ","
						+ "@Provisionsartikel" + i + ","
						+ "@Prufstatus_TN_Ware" + i + ","
						+ "@Rabattierfahig" + i + ","
						+ "@Rahmen" + i + ","
						+ "@Rahmen2" + i + ","
						+ "@Rahmenauslauf" + i + ","
						+ "@Rahmenauslauf2" + i + ","
						+ "@Rahmenmenge" + i + ","
						+ "@Rahmenmenge2" + i + ","
						+ "@Rahmen_Nr" + i + ","
						+ "@Rahmen_Nr2" + i + ","
						+ "@REACH_SVHC_Confirmity" + i + ","
						+ "@ROHS_EEE_Confirmity" + i + ","
						+ "@Seriennummer" + i + ","
						+ "@Seriennummernverwaltung" + i + ","
						+ "@Sonderrabatt" + i + ","
						+ "@Standard_Lagerort_id" + i + ","
						+ "@Stuckliste" + i + ","
						+ "@Stundensatz" + i + ","
						+ "@Sysmonummer" + i + ","
						+ "@UBG" + i + ","
						+ "@UL_Etikett" + i + ","
						+ "@UL_zertifiziert" + i + ","
						+ "@Umsatzsteuer" + i + ","
						+ "@Ursprungsland" + i + ","
						+ "@VDA_1" + i + ","
						+ "@VDA_2" + i + ","
						+ "@Verpackung" + i + ","
						+ "@Verpackungsart" + i + ","
						+ "@Verpackungsmenge" + i + ","
						+ "@VK_Festpreis" + i + ","
						+ "@Volumen" + i + ","
						+ "@Warengruppe" + i + ","
						+ "@Warentyp" + i + ","
						+ "@Webshop" + i + ","
						+ "@Werkzeug" + i + ","
						+ "@Wert_Anfangsbestand" + i + ","
						+ "@Zeichnungsnummer" + i + ","
						+ "@Zeitraum_MHD" + i + ","
						+ "@Zolltarif_nr" + i + ","
						+ "@Zuschlag_VK" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("aktiv" + i, item.aktiv == null ? (object)DBNull.Value : item.aktiv);
					sqlCommand.Parameters.AddWithValue("aktualisiert" + i, item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
					sqlCommand.Parameters.AddWithValue("Anfangsbestand" + i, item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion" + i, item.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : item.ArtikelAusEigenerProduktion);
					sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren" + i, item.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : item.ArtikelFürWeitereBestellungenSperren);
					sqlCommand.Parameters.AddWithValue("Artikelbezeichnung" + i, item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
					sqlCommand.Parameters.AddWithValue("artikelklassifizierung" + i, item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
					sqlCommand.Parameters.AddWithValue("Artikelkurztext" + i, item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
					sqlCommand.Parameters.AddWithValue("Barverkauf" + i, item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
					sqlCommand.Parameters.AddWithValue("BemerkungCRP" + i, item.BemerkungCRP == null ? (object)DBNull.Value : item.BemerkungCRP);
					sqlCommand.Parameters.AddWithValue("BemerkungCRPPlanung" + i, item.BemerkungCRPPlanung == null ? (object)DBNull.Value : item.BemerkungCRPPlanung);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_3" + i, item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
					sqlCommand.Parameters.AddWithValue("BezeichnungAL" + i, item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
					sqlCommand.Parameters.AddWithValue("Blokiert_Status" + i, item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
					sqlCommand.Parameters.AddWithValue("CocVersion" + i, item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
					sqlCommand.Parameters.AddWithValue("COF_Pflichtig" + i, item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
					sqlCommand.Parameters.AddWithValue("CP_required" + i, item.CP_required == null ? (object)DBNull.Value : item.CP_required);
					sqlCommand.Parameters.AddWithValue("Crossreferenz" + i, item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
					sqlCommand.Parameters.AddWithValue("Cu_Gewicht" + i, item.CuGewicht == null ? (object)DBNull.Value : item.CuGewicht);
					sqlCommand.Parameters.AddWithValue("CustomerEnd" + i, item.CustomerEnd == null ? (object)DBNull.Value : item.CustomerEnd);
					sqlCommand.Parameters.AddWithValue("CustomerIndex" + i, item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
					sqlCommand.Parameters.AddWithValue("CustomerIndexSequence" + i, item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence" + i, item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerPrefix" + i, item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
					sqlCommand.Parameters.AddWithValue("CustomerTechnic" + i, item.CustomerTechnic == null ? (object)DBNull.Value : item.CustomerTechnic);
					sqlCommand.Parameters.AddWithValue("CustomerTechnicId" + i, item.CustomerTechnicId == null ? (object)DBNull.Value : item.CustomerTechnicId);
					sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand" + i, item.DatumAnfangsbestand == null ? (object)DBNull.Value : item.DatumAnfangsbestand);
					sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
					sqlCommand.Parameters.AddWithValue("Dienstelistung" + i, item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
					sqlCommand.Parameters.AddWithValue("Dokumente" + i, item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
					sqlCommand.Parameters.AddWithValue("EAN" + i, item.EAN == null ? (object)DBNull.Value : item.EAN);
					sqlCommand.Parameters.AddWithValue("EdiDefault" + i, item.EdiDefault == null ? (object)DBNull.Value : item.EdiDefault);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("EMPB" + i, item.EMPB == null ? (object)DBNull.Value : item.EMPB);
					sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben" + i, item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
					sqlCommand.Parameters.AddWithValue("Ersatzartikel" + i, item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz" + i, item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text" + i, item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
					sqlCommand.Parameters.AddWithValue("Exportgewicht" + i, item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
					sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste" + i, item.fakturierenStückliste == null ? (object)DBNull.Value : item.fakturierenStückliste);
					sqlCommand.Parameters.AddWithValue("Farbe" + i, item.Farbe == null ? (object)DBNull.Value : item.Farbe);
					sqlCommand.Parameters.AddWithValue("fibu_rahmen" + i, item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
					sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern" + i, item.FreigabestatusTNIntern == null ? (object)DBNull.Value : item.FreigabestatusTNIntern);
					sqlCommand.Parameters.AddWithValue("Gebinde" + i, item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
					sqlCommand.Parameters.AddWithValue("Gewicht" + i, item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
					sqlCommand.Parameters.AddWithValue("Grosse" + i, item.Größe == null ? (object)DBNull.Value : item.Größe);
					sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.GrundFürSperre == null ? (object)DBNull.Value : item.GrundFürSperre);
					sqlCommand.Parameters.AddWithValue("gultig_bis" + i, item.gültigBis == null ? (object)DBNull.Value : item.gültigBis);
					sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
					sqlCommand.Parameters.AddWithValue("Hubmastleitungen" + i, item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
					sqlCommand.Parameters.AddWithValue("ID_Klassifizierung" + i, item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("Info_WE" + i, item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
					sqlCommand.Parameters.AddWithValue("IsArticleNumberSpecial" + i, item.IsArticleNumberSpecial == null ? (object)DBNull.Value : item.IsArticleNumberSpecial);
					sqlCommand.Parameters.AddWithValue("IsEDrawing" + i, item.IsEDrawing == null ? (object)DBNull.Value : item.IsEDrawing);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Kategorie" + i, item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
					sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Kriterium1" + i, item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
					sqlCommand.Parameters.AddWithValue("Kriterium2" + i, item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
					sqlCommand.Parameters.AddWithValue("Kriterium3" + i, item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
					sqlCommand.Parameters.AddWithValue("Kriterium4" + i, item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
					sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
					sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
					sqlCommand.Parameters.AddWithValue("Lagerartikel" + i, item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
					sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten" + i, item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
					sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB" + i, item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW" + i, item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
					sqlCommand.Parameters.AddWithValue("Lieferzeit" + i, item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
					sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
					sqlCommand.Parameters.AddWithValue("Manufacturer" + i, item.Manufacturer == null ? (object)DBNull.Value : item.Manufacturer);
					sqlCommand.Parameters.AddWithValue("ManufacturerNextArticle" + i, item.ManufacturerNextArticle == null ? (object)DBNull.Value : item.ManufacturerNextArticle);
					sqlCommand.Parameters.AddWithValue("ManufacturerNextArticleId" + i, item.ManufacturerNextArticleId == null ? (object)DBNull.Value : item.ManufacturerNextArticleId);
					sqlCommand.Parameters.AddWithValue("ManufacturerNumber" + i, item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
					sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticle" + i, item.ManufacturerPreviousArticle == null ? (object)DBNull.Value : item.ManufacturerPreviousArticle);
					sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticleId" + i, item.ManufacturerPreviousArticleId == null ? (object)DBNull.Value : item.ManufacturerPreviousArticleId);
					sqlCommand.Parameters.AddWithValue("Materialkosten_Alt" + i, item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
					sqlCommand.Parameters.AddWithValue("MHD" + i, item.MHD == null ? (object)DBNull.Value : item.MHD);
					sqlCommand.Parameters.AddWithValue("Minerals_Confirmity" + i, item.MineralsConfirmity == null ? (object)DBNull.Value : item.MineralsConfirmity);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr" + i, item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr" + i, item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit" + i, item.proZeiteinheit == null ? (object)DBNull.Value : item.proZeiteinheit);
					sqlCommand.Parameters.AddWithValue("ProductionCountryCode" + i, item.ProductionCountryCode == null ? (object)DBNull.Value : item.ProductionCountryCode);
					sqlCommand.Parameters.AddWithValue("ProductionCountryName" + i, item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
					sqlCommand.Parameters.AddWithValue("ProductionCountrySequence" + i, item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
					sqlCommand.Parameters.AddWithValue("ProductionLotSize" + i, item.ProductionLotSize == null ? (object)DBNull.Value : item.ProductionLotSize);
					sqlCommand.Parameters.AddWithValue("ProductionSiteCode" + i, item.ProductionSiteCode == null ? (object)DBNull.Value : item.ProductionSiteCode);
					sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
					sqlCommand.Parameters.AddWithValue("ProductionSiteSequence" + i, item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
					sqlCommand.Parameters.AddWithValue("Produktionszeit" + i, item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
					sqlCommand.Parameters.AddWithValue("Projektname" + i, item.Projektname == null ? (object)DBNull.Value : item.Projektname);
					sqlCommand.Parameters.AddWithValue("Provisionsartikel" + i, item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
					sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware" + i, item.PrufstatusTNWare == null ? (object)DBNull.Value : item.PrufstatusTNWare);
					sqlCommand.Parameters.AddWithValue("Rabattierfahig" + i, item.Rabattierfähig == null ? (object)DBNull.Value : item.Rabattierfähig);
					sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
					sqlCommand.Parameters.AddWithValue("Rahmen2" + i, item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf" + i, item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf2" + i, item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge" + i, item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge2" + i, item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr" + i, item.RahmenNr == null ? (object)DBNull.Value : item.RahmenNr);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr2" + i, item.RahmenNr2 == null ? (object)DBNull.Value : item.RahmenNr2);
					sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity" + i, item.REACHSVHCConfirmity == null ? (object)DBNull.Value : item.REACHSVHCConfirmity);
					sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity" + i, item.ROHSEEEConfirmity == null ? (object)DBNull.Value : item.ROHSEEEConfirmity);
					sqlCommand.Parameters.AddWithValue("Seriennummer" + i, item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
					sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung" + i, item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
					sqlCommand.Parameters.AddWithValue("Sonderrabatt" + i, item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
					sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id" + i, item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
					sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
					sqlCommand.Parameters.AddWithValue("Sysmonummer" + i, item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
					sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
					sqlCommand.Parameters.AddWithValue("UL_Etikett" + i, item.ULEtikett == null ? (object)DBNull.Value : item.ULEtikett);
					sqlCommand.Parameters.AddWithValue("UL_zertifiziert" + i, item.ULzertifiziert == null ? (object)DBNull.Value : item.ULzertifiziert);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Ursprungsland" + i, item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
					sqlCommand.Parameters.AddWithValue("VDA_1" + i, item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
					sqlCommand.Parameters.AddWithValue("VDA_2" + i, item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
					sqlCommand.Parameters.AddWithValue("Verpackung" + i, item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
					sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
					sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
					sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
					sqlCommand.Parameters.AddWithValue("Volumen" + i, item.Volumen == null ? (object)DBNull.Value : item.Volumen);
					sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Warentyp" + i, item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
					sqlCommand.Parameters.AddWithValue("Webshop" + i, item.Webshop == null ? (object)DBNull.Value : item.Webshop);
					sqlCommand.Parameters.AddWithValue("Werkzeug" + i, item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
					sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand" + i, item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zeitraum_MHD" + i, item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
					sqlCommand.Parameters.AddWithValue("Zolltarif_nr" + i, item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
					sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Artikel] SET [Abladestelle]=@Abladestelle, [aktiv]=@aktiv, [aktualisiert]=@aktualisiert, [Anfangsbestand]=@Anfangsbestand, [ArticleNumber]=@ArticleNumber, [Artikel aus eigener Produktion]=@Artikel_aus_eigener_Produktion, [Artikel für weitere Bestellungen sperren]=@Artikel_fur_weitere_Bestellungen_sperren, [Artikelbezeichnung]=@Artikelbezeichnung, [Artikelfamilie_Kunde]=@Artikelfamilie_Kunde, [Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1, [Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2, [artikelklassifizierung]=@artikelklassifizierung, [Artikelkurztext]=@Artikelkurztext, [Artikelnummer]=@Artikelnummer, [Barverkauf]=@Barverkauf, [BemerkungCRP]=@BemerkungCRP,[BemerkungCRPPlanung]=@BemerkungCRPPlanung, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 2]=@Bezeichnung_2, [Bezeichnung 3]=@Bezeichnung_3, [BezeichnungAL]=@BezeichnungAL, [Blokiert_Status]=@Blokiert_Status, [CocVersion]=@CocVersion, [COF_Pflichtig]=@COF_Pflichtig, [CP_required]=@CP_required, [Crossreferenz]=@Crossreferenz, [Cu-Gewicht]=@Cu_Gewicht, [CustomerEnd]=@CustomerEnd, [CustomerIndex]=@CustomerIndex, [CustomerIndexSequence]=@CustomerIndexSequence, [CustomerItemNumber]=@CustomerItemNumber, [CustomerItemNumberSequence]=@CustomerItemNumberSequence, [CustomerNumber]=@CustomerNumber, [CustomerPrefix]=@CustomerPrefix, [CustomerTechnic]=@CustomerTechnic, [CustomerTechnicId]=@CustomerTechnicId, [Datum Anfangsbestand]=@Datum_Anfangsbestand, [DEL]=@DEL, [DEL fixiert]=@DEL_fixiert, [Dienstelistung]=@Dienstelistung, [Dokumente]=@Dokumente, [EAN]=@EAN, [EdiDefault]=@EdiDefault, [Einheit]=@Einheit, [EMPB]=@EMPB, [EMPB_Freigegeben]=@EMPB_Freigegeben, [Ersatzartikel]=@Ersatzartikel, [ESD_Schutz]=@ESD_Schutz, [ESD_Schutz_Text]=@ESD_Schutz_Text, [Exportgewicht]=@Exportgewicht, [fakturieren Stückliste]=@fakturieren_Stuckliste, [Farbe]=@Farbe, [fibu_rahmen]=@fibu_rahmen, [Freigabestatus]=@Freigabestatus, [Freigabestatus TN intern]=@Freigabestatus_TN_intern, [Gebinde]=@Gebinde, [Gewicht]=@Gewicht, [Größe]=@Grosse, [Grund für Sperre]=@Grund_fur_Sperre, [gültig bis]=@gultig_bis, [Halle]=@Halle, [Hubmastleitungen]=@Hubmastleitungen, [ID_Klassifizierung]=@ID_Klassifizierung, [Index_Kunde]=@Index_Kunde, [Index_Kunde_Datum]=@Index_Kunde_Datum, [Info_WE]=@Info_WE, [IsArticleNumberSpecial]=@IsArticleNumberSpecial, [IsEDrawing]=@IsEDrawing, [Kanban]=@Kanban, [Kategorie]=@Kategorie, [Klassifizierung]=@Klassifizierung, [Kriterium1]=@Kriterium1, [Kriterium2]=@Kriterium2, [Kriterium3]=@Kriterium3, [Kriterium4]=@Kriterium4, [Kupferbasis]=@Kupferbasis, [Kupferzahl]=@Kupferzahl, [Lagerartikel]=@Lagerartikel, [Lagerhaltungskosten]=@Lagerhaltungskosten, [Langtext]=@Langtext, [Langtext_drucken_AB]=@Langtext_drucken_AB, [Langtext_drucken_BW]=@Langtext_drucken_BW, [Lieferzeit]=@Lieferzeit, [Losgroesse]=@Losgroesse, [Manufacturer]=@Manufacturer, [ManufacturerNextArticle]=@ManufacturerNextArticle, [ManufacturerNextArticleId]=@ManufacturerNextArticleId, [ManufacturerNumber]=@ManufacturerNumber, [ManufacturerPreviousArticle]=@ManufacturerPreviousArticle, [ManufacturerPreviousArticleId]=@ManufacturerPreviousArticleId, [Materialkosten_Alt]=@Materialkosten_Alt, [MHD]=@MHD, [Minerals Confirmity]=@Minerals_Confirmity, [Praeferenz_Aktuelles_jahr]=@Praeferenz_Aktuelles_jahr, [Praeferenz_Folgejahr]=@Praeferenz_Folgejahr, [Preiseinheit]=@Preiseinheit, [pro Zeiteinheit]=@pro_Zeiteinheit, [ProductionCountryCode]=@ProductionCountryCode, [ProductionCountryName]=@ProductionCountryName, [ProductionCountrySequence]=@ProductionCountrySequence, [ProductionLotSize]=@ProductionLotSize, [ProductionSiteCode]=@ProductionSiteCode, [ProductionSiteName]=@ProductionSiteName, [ProductionSiteSequence]=@ProductionSiteSequence, [Produktionszeit]=@Produktionszeit, [Projektname]=@Projektname, [Provisionsartikel]=@Provisionsartikel, [Prüfstatus TN Ware]=@Prufstatus_TN_Ware, [Rabattierfähig]=@Rabattierfahig, [Rahmen]=@Rahmen, [Rahmen2]=@Rahmen2, [Rahmenauslauf]=@Rahmenauslauf, [Rahmenauslauf2]=@Rahmenauslauf2, [Rahmenmenge]=@Rahmenmenge, [Rahmenmenge2]=@Rahmenmenge2, [Rahmen-Nr]=@Rahmen_Nr, [Rahmen-Nr2]=@Rahmen_Nr2, [REACH SVHC Confirmity]=@REACH_SVHC_Confirmity, [ROHS EEE Confirmity]=@ROHS_EEE_Confirmity, [Seriennummer]=@Seriennummer, [Seriennummernverwaltung]=@Seriennummernverwaltung, [Sonderrabatt]=@Sonderrabatt, [Standard_Lagerort_id]=@Standard_Lagerort_id, [Stückliste]=@Stuckliste, [Stundensatz]=@Stundensatz, [Sysmonummer]=@Sysmonummer, [UBG]=@UBG, [UL Etikett]=@UL_Etikett, [UL zertifiziert]=@UL_zertifiziert, [Umsatzsteuer]=@Umsatzsteuer, [Ursprungsland]=@Ursprungsland, [VDA_1]=@VDA_1, [VDA_2]=@VDA_2, [Verpackung]=@Verpackung, [Verpackungsart]=@Verpackungsart, [Verpackungsmenge]=@Verpackungsmenge, [VK-Festpreis]=@VK_Festpreis, [Volumen]=@Volumen, [Warengruppe]=@Warengruppe, [Warentyp]=@Warentyp, [Webshop]=@Webshop, [Werkzeug]=@Werkzeug, [Wert_Anfangsbestand]=@Wert_Anfangsbestand, [Zeichnungsnummer]=@Zeichnungsnummer, [Zeitraum_MHD]=@Zeitraum_MHD, [Zolltarif_nr]=@Zolltarif_nr, [Zuschlag_VK]=@Zuschlag_VK WHERE [Artikel-Nr]=@Artikel_Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
			sqlCommand.Parameters.AddWithValue("aktiv", item.aktiv == null ? (object)DBNull.Value : item.aktiv);
			sqlCommand.Parameters.AddWithValue("aktualisiert", item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
			sqlCommand.Parameters.AddWithValue("Anfangsbestand", item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", item.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : item.ArtikelAusEigenerProduktion);
			sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren", item.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : item.ArtikelFürWeitereBestellungenSperren);
			sqlCommand.Parameters.AddWithValue("Artikelbezeichnung", item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
			sqlCommand.Parameters.AddWithValue("artikelklassifizierung", item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
			sqlCommand.Parameters.AddWithValue("Artikelkurztext", item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
			sqlCommand.Parameters.AddWithValue("Barverkauf", item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
			sqlCommand.Parameters.AddWithValue("BemerkungCRP", item.BemerkungCRP == null ? (object)DBNull.Value : item.BemerkungCRP);
			sqlCommand.Parameters.AddWithValue("BemerkungCRPPlanung", item.BemerkungCRPPlanung == null ? (object)DBNull.Value : item.BemerkungCRPPlanung);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_3", item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
			sqlCommand.Parameters.AddWithValue("BezeichnungAL", item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
			sqlCommand.Parameters.AddWithValue("Blokiert_Status", item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
			sqlCommand.Parameters.AddWithValue("CocVersion", item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
			sqlCommand.Parameters.AddWithValue("COF_Pflichtig", item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
			sqlCommand.Parameters.AddWithValue("CP_required", item.CP_required == null ? (object)DBNull.Value : item.CP_required);
			sqlCommand.Parameters.AddWithValue("Crossreferenz", item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
			sqlCommand.Parameters.AddWithValue("Cu_Gewicht", item.CuGewicht == null ? (object)DBNull.Value : item.CuGewicht);
			sqlCommand.Parameters.AddWithValue("CustomerEnd", item.CustomerEnd == null ? (object)DBNull.Value : item.CustomerEnd);
			sqlCommand.Parameters.AddWithValue("CustomerIndex", item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
			sqlCommand.Parameters.AddWithValue("CustomerIndexSequence", item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
			sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
			sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence", item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("CustomerPrefix", item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
			sqlCommand.Parameters.AddWithValue("CustomerTechnic", item.CustomerTechnic == null ? (object)DBNull.Value : item.CustomerTechnic);
			sqlCommand.Parameters.AddWithValue("CustomerTechnicId", item.CustomerTechnicId == null ? (object)DBNull.Value : item.CustomerTechnicId);
			sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", item.DatumAnfangsbestand == null ? (object)DBNull.Value : item.DatumAnfangsbestand);
			sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
			sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
			sqlCommand.Parameters.AddWithValue("Dienstelistung", item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
			sqlCommand.Parameters.AddWithValue("Dokumente", item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
			sqlCommand.Parameters.AddWithValue("EAN", item.EAN == null ? (object)DBNull.Value : item.EAN);
			sqlCommand.Parameters.AddWithValue("EdiDefault", item.EdiDefault == null ? (object)DBNull.Value : item.EdiDefault);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("EMPB", item.EMPB == null ? (object)DBNull.Value : item.EMPB);
			sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben", item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
			sqlCommand.Parameters.AddWithValue("Ersatzartikel", item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
			sqlCommand.Parameters.AddWithValue("ESD_Schutz", item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
			sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text", item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
			sqlCommand.Parameters.AddWithValue("Exportgewicht", item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
			sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste", item.fakturierenStückliste == null ? (object)DBNull.Value : item.fakturierenStückliste);
			sqlCommand.Parameters.AddWithValue("Farbe", item.Farbe == null ? (object)DBNull.Value : item.Farbe);
			sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
			sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
			sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", item.FreigabestatusTNIntern == null ? (object)DBNull.Value : item.FreigabestatusTNIntern);
			sqlCommand.Parameters.AddWithValue("Gebinde", item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
			sqlCommand.Parameters.AddWithValue("Gewicht", item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
			sqlCommand.Parameters.AddWithValue("Grosse", item.Größe == null ? (object)DBNull.Value : item.Größe);
			sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", item.GrundFürSperre == null ? (object)DBNull.Value : item.GrundFürSperre);
			sqlCommand.Parameters.AddWithValue("gultig_bis", item.gültigBis == null ? (object)DBNull.Value : item.gültigBis);
			sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
			sqlCommand.Parameters.AddWithValue("Hubmastleitungen", item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
			sqlCommand.Parameters.AddWithValue("ID_Klassifizierung", item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
			sqlCommand.Parameters.AddWithValue("Info_WE", item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
			sqlCommand.Parameters.AddWithValue("IsArticleNumberSpecial", item.IsArticleNumberSpecial == null ? (object)DBNull.Value : item.IsArticleNumberSpecial);
			sqlCommand.Parameters.AddWithValue("IsEDrawing", item.IsEDrawing == null ? (object)DBNull.Value : item.IsEDrawing);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Kategorie", item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
			sqlCommand.Parameters.AddWithValue("Klassifizierung", item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Kriterium1", item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
			sqlCommand.Parameters.AddWithValue("Kriterium2", item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
			sqlCommand.Parameters.AddWithValue("Kriterium3", item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
			sqlCommand.Parameters.AddWithValue("Kriterium4", item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
			sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
			sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
			sqlCommand.Parameters.AddWithValue("Lagerartikel", item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
			sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
			sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
			sqlCommand.Parameters.AddWithValue("Lieferzeit", item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
			sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
			sqlCommand.Parameters.AddWithValue("Manufacturer", item.Manufacturer == null ? (object)DBNull.Value : item.Manufacturer);
			sqlCommand.Parameters.AddWithValue("ManufacturerNextArticle", item.ManufacturerNextArticle == null ? (object)DBNull.Value : item.ManufacturerNextArticle);
			sqlCommand.Parameters.AddWithValue("ManufacturerNextArticleId", item.ManufacturerNextArticleId == null ? (object)DBNull.Value : item.ManufacturerNextArticleId);
			sqlCommand.Parameters.AddWithValue("ManufacturerNumber", item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
			sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticle", item.ManufacturerPreviousArticle == null ? (object)DBNull.Value : item.ManufacturerPreviousArticle);
			sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticleId", item.ManufacturerPreviousArticleId == null ? (object)DBNull.Value : item.ManufacturerPreviousArticleId);
			sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
			sqlCommand.Parameters.AddWithValue("MHD", item.MHD == null ? (object)DBNull.Value : item.MHD);
			sqlCommand.Parameters.AddWithValue("Minerals_Confirmity", item.MineralsConfirmity == null ? (object)DBNull.Value : item.MineralsConfirmity);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr", item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr", item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", item.proZeiteinheit == null ? (object)DBNull.Value : item.proZeiteinheit);
			sqlCommand.Parameters.AddWithValue("ProductionCountryCode", item.ProductionCountryCode == null ? (object)DBNull.Value : item.ProductionCountryCode);
			sqlCommand.Parameters.AddWithValue("ProductionCountryName", item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
			sqlCommand.Parameters.AddWithValue("ProductionCountrySequence", item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
			sqlCommand.Parameters.AddWithValue("ProductionLotSize", item.ProductionLotSize == null ? (object)DBNull.Value : item.ProductionLotSize);
			sqlCommand.Parameters.AddWithValue("ProductionSiteCode", item.ProductionSiteCode == null ? (object)DBNull.Value : item.ProductionSiteCode);
			sqlCommand.Parameters.AddWithValue("ProductionSiteName", item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
			sqlCommand.Parameters.AddWithValue("ProductionSiteSequence", item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
			sqlCommand.Parameters.AddWithValue("Produktionszeit", item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
			sqlCommand.Parameters.AddWithValue("Projektname", item.Projektname == null ? (object)DBNull.Value : item.Projektname);
			sqlCommand.Parameters.AddWithValue("Provisionsartikel", item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
			sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware", item.PrufstatusTNWare == null ? (object)DBNull.Value : item.PrufstatusTNWare);
			sqlCommand.Parameters.AddWithValue("Rabattierfahig", item.Rabattierfähig == null ? (object)DBNull.Value : item.Rabattierfähig);
			sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
			sqlCommand.Parameters.AddWithValue("Rahmen2", item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf", item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf2", item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge", item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge2", item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
			sqlCommand.Parameters.AddWithValue("Rahmen_Nr", item.RahmenNr == null ? (object)DBNull.Value : item.RahmenNr);
			sqlCommand.Parameters.AddWithValue("Rahmen_Nr2", item.RahmenNr2 == null ? (object)DBNull.Value : item.RahmenNr2);
			sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity", item.REACHSVHCConfirmity == null ? (object)DBNull.Value : item.REACHSVHCConfirmity);
			sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity", item.ROHSEEEConfirmity == null ? (object)DBNull.Value : item.ROHSEEEConfirmity);
			sqlCommand.Parameters.AddWithValue("Seriennummer", item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
			sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
			sqlCommand.Parameters.AddWithValue("Sonderrabatt", item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
			sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Stuckliste", item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
			sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
			sqlCommand.Parameters.AddWithValue("Sysmonummer", item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
			sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
			sqlCommand.Parameters.AddWithValue("UL_Etikett", item.ULEtikett == null ? (object)DBNull.Value : item.ULEtikett);
			sqlCommand.Parameters.AddWithValue("UL_zertifiziert", item.ULzertifiziert == null ? (object)DBNull.Value : item.ULzertifiziert);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("Ursprungsland", item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
			sqlCommand.Parameters.AddWithValue("VDA_1", item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
			sqlCommand.Parameters.AddWithValue("VDA_2", item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
			sqlCommand.Parameters.AddWithValue("Verpackung", item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
			sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
			sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
			sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
			sqlCommand.Parameters.AddWithValue("Volumen", item.Volumen == null ? (object)DBNull.Value : item.Volumen);
			sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
			sqlCommand.Parameters.AddWithValue("Warentyp", item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
			sqlCommand.Parameters.AddWithValue("Webshop", item.Webshop == null ? (object)DBNull.Value : item.Webshop);
			sqlCommand.Parameters.AddWithValue("Werkzeug", item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
			sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
			sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
			sqlCommand.Parameters.AddWithValue("Zolltarif_nr", item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
			sqlCommand.Parameters.AddWithValue("Zuschlag_VK", item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 149; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [Artikel] SET "

					+ "[Abladestelle]=@Abladestelle" + i + ","
					+ "[aktiv]=@aktiv" + i + ","
					+ "[aktualisiert]=@aktualisiert" + i + ","
					+ "[Anfangsbestand]=@Anfangsbestand" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[Artikel aus eigener Produktion]=@Artikel_aus_eigener_Produktion" + i + ","
					+ "[Artikel für weitere Bestellungen sperren]=@Artikel_fur_weitere_Bestellungen_sperren" + i + ","
					+ "[Artikelbezeichnung]=@Artikelbezeichnung" + i + ","
					+ "[Artikelfamilie_Kunde]=@Artikelfamilie_Kunde" + i + ","
					+ "[Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1" + i + ","
					+ "[Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2" + i + ","
					+ "[artikelklassifizierung]=@artikelklassifizierung" + i + ","
					+ "[Artikelkurztext]=@Artikelkurztext" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[Barverkauf]=@Barverkauf" + i + ","
					+ "[BemerkungCRP]=@BemerkungCRP" + i + ","
					+ "[BemerkungCRPPlanung]=@BemerkungCRPPlanung" + i + ","
					+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
					+ "[Bezeichnung 2]=@Bezeichnung_2" + i + ","
					+ "[Bezeichnung 3]=@Bezeichnung_3" + i + ","
					+ "[BezeichnungAL]=@BezeichnungAL" + i + ","
					+ "[Blokiert_Status]=@Blokiert_Status" + i + ","
					+ "[CocVersion]=@CocVersion" + i + ","
					+ "[COF_Pflichtig]=@COF_Pflichtig" + i + ","
					+ "[CP_required]=@CP_required" + i + ","
					+ "[Crossreferenz]=@Crossreferenz" + i + ","
					+ "[Cu-Gewicht]=@Cu_Gewicht" + i + ","
					+ "[CustomerEnd]=@CustomerEnd" + i + ","
					+ "[CustomerIndex]=@CustomerIndex" + i + ","
					+ "[CustomerIndexSequence]=@CustomerIndexSequence" + i + ","
					+ "[CustomerItemNumber]=@CustomerItemNumber" + i + ","
					+ "[CustomerItemNumberSequence]=@CustomerItemNumberSequence" + i + ","
					+ "[CustomerNumber]=@CustomerNumber" + i + ","
					+ "[CustomerPrefix]=@CustomerPrefix" + i + ","
					+ "[CustomerTechnic]=@CustomerTechnic" + i + ","
					+ "[CustomerTechnicId]=@CustomerTechnicId" + i + ","
					+ "[Datum Anfangsbestand]=@Datum_Anfangsbestand" + i + ","
					+ "[DEL]=@DEL" + i + ","
					+ "[DEL fixiert]=@DEL_fixiert" + i + ","
					+ "[Dienstelistung]=@Dienstelistung" + i + ","
					+ "[Dokumente]=@Dokumente" + i + ","
					+ "[EAN]=@EAN" + i + ","
					+ "[EdiDefault]=@EdiDefault" + i + ","
					+ "[Einheit]=@Einheit" + i + ","
					+ "[EMPB]=@EMPB" + i + ","
					+ "[EMPB_Freigegeben]=@EMPB_Freigegeben" + i + ","
					+ "[Ersatzartikel]=@Ersatzartikel" + i + ","
					+ "[ESD_Schutz]=@ESD_Schutz" + i + ","
					+ "[ESD_Schutz_Text]=@ESD_Schutz_Text" + i + ","
					+ "[Exportgewicht]=@Exportgewicht" + i + ","
					+ "[fakturieren Stückliste]=@fakturieren_Stuckliste" + i + ","
					+ "[Farbe]=@Farbe" + i + ","
					+ "[fibu_rahmen]=@fibu_rahmen" + i + ","
					+ "[Freigabestatus]=@Freigabestatus" + i + ","
					+ "[Freigabestatus TN intern]=@Freigabestatus_TN_intern" + i + ","
					+ "[Gebinde]=@Gebinde" + i + ","
					+ "[Gewicht]=@Gewicht" + i + ","
					+ "[Größe]=@Grosse" + i + ","
					+ "[Grund für Sperre]=@Grund_fur_Sperre" + i + ","
					+ "[gültig bis]=@gultig_bis" + i + ","
					+ "[Halle]=@Halle" + i + ","
					+ "[Hubmastleitungen]=@Hubmastleitungen" + i + ","
					+ "[ID_Klassifizierung]=@ID_Klassifizierung" + i + ","
					+ "[Index_Kunde]=@Index_Kunde" + i + ","
					+ "[Index_Kunde_Datum]=@Index_Kunde_Datum" + i + ","
					+ "[Info_WE]=@Info_WE" + i + ","
					+ "[IsArticleNumberSpecial]=@IsArticleNumberSpecial" + i + ","
					+ "[IsEDrawing]=@IsEDrawing" + i + ","
					+ "[Kanban]=@Kanban" + i + ","
					+ "[Kategorie]=@Kategorie" + i + ","
					+ "[Klassifizierung]=@Klassifizierung" + i + ","
					+ "[Kriterium1]=@Kriterium1" + i + ","
					+ "[Kriterium2]=@Kriterium2" + i + ","
					+ "[Kriterium3]=@Kriterium3" + i + ","
					+ "[Kriterium4]=@Kriterium4" + i + ","
					+ "[Kupferbasis]=@Kupferbasis" + i + ","
					+ "[Kupferzahl]=@Kupferzahl" + i + ","
					+ "[Lagerartikel]=@Lagerartikel" + i + ","
					+ "[Lagerhaltungskosten]=@Lagerhaltungskosten" + i + ","
					+ "[Langtext]=@Langtext" + i + ","
					+ "[Langtext_drucken_AB]=@Langtext_drucken_AB" + i + ","
					+ "[Langtext_drucken_BW]=@Langtext_drucken_BW" + i + ","
					+ "[Lieferzeit]=@Lieferzeit" + i + ","
					+ "[Losgroesse]=@Losgroesse" + i + ","
					+ "[Manufacturer]=@Manufacturer" + i + ","
					+ "[ManufacturerNextArticle]=@ManufacturerNextArticle" + i + ","
					+ "[ManufacturerNextArticleId]=@ManufacturerNextArticleId" + i + ","
					+ "[ManufacturerNumber]=@ManufacturerNumber" + i + ","
					+ "[ManufacturerPreviousArticle]=@ManufacturerPreviousArticle" + i + ","
					+ "[ManufacturerPreviousArticleId]=@ManufacturerPreviousArticleId" + i + ","
					+ "[Materialkosten_Alt]=@Materialkosten_Alt" + i + ","
					+ "[MHD]=@MHD" + i + ","
					+ "[Minerals Confirmity]=@Minerals_Confirmity" + i + ","
					+ "[Praeferenz_Aktuelles_jahr]=@Praeferenz_Aktuelles_jahr" + i + ","
					+ "[Praeferenz_Folgejahr]=@Praeferenz_Folgejahr" + i + ","
					+ "[Preiseinheit]=@Preiseinheit" + i + ","
					+ "[pro Zeiteinheit]=@pro_Zeiteinheit" + i + ","
					+ "[ProductionCountryCode]=@ProductionCountryCode" + i + ","
					+ "[ProductionCountryName]=@ProductionCountryName" + i + ","
					+ "[ProductionCountrySequence]=@ProductionCountrySequence" + i + ","
					+ "[ProductionLotSize]=@ProductionLotSize" + i + ","
					+ "[ProductionSiteCode]=@ProductionSiteCode" + i + ","
					+ "[ProductionSiteName]=@ProductionSiteName" + i + ","
					+ "[ProductionSiteSequence]=@ProductionSiteSequence" + i + ","
					+ "[Produktionszeit]=@Produktionszeit" + i + ","
					+ "[Projektname]=@Projektname" + i + ","
					+ "[Provisionsartikel]=@Provisionsartikel" + i + ","
					+ "[Prüfstatus TN Ware]=@Prufstatus_TN_Ware" + i + ","
					+ "[Rabattierfähig]=@Rabattierfahig" + i + ","
					+ "[Rahmen]=@Rahmen" + i + ","
					+ "[Rahmen2]=@Rahmen2" + i + ","
					+ "[Rahmenauslauf]=@Rahmenauslauf" + i + ","
					+ "[Rahmenauslauf2]=@Rahmenauslauf2" + i + ","
					+ "[Rahmenmenge]=@Rahmenmenge" + i + ","
					+ "[Rahmenmenge2]=@Rahmenmenge2" + i + ","
					+ "[Rahmen-Nr]=@Rahmen_Nr" + i + ","
					+ "[Rahmen-Nr2]=@Rahmen_Nr2" + i + ","
					+ "[REACH SVHC Confirmity]=@REACH_SVHC_Confirmity" + i + ","
					+ "[ROHS EEE Confirmity]=@ROHS_EEE_Confirmity" + i + ","
					+ "[Seriennummer]=@Seriennummer" + i + ","
					+ "[Seriennummernverwaltung]=@Seriennummernverwaltung" + i + ","
					+ "[Sonderrabatt]=@Sonderrabatt" + i + ","
					+ "[Standard_Lagerort_id]=@Standard_Lagerort_id" + i + ","
					+ "[Stückliste]=@Stuckliste" + i + ","
					+ "[Stundensatz]=@Stundensatz" + i + ","
					+ "[Sysmonummer]=@Sysmonummer" + i + ","
					+ "[UBG]=@UBG" + i + ","
					+ "[UL Etikett]=@UL_Etikett" + i + ","
					+ "[UL zertifiziert]=@UL_zertifiziert" + i + ","
					+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
					+ "[Ursprungsland]=@Ursprungsland" + i + ","
					+ "[VDA_1]=@VDA_1" + i + ","
					+ "[VDA_2]=@VDA_2" + i + ","
					+ "[Verpackung]=@Verpackung" + i + ","
					+ "[Verpackungsart]=@Verpackungsart" + i + ","
					+ "[Verpackungsmenge]=@Verpackungsmenge" + i + ","
					+ "[VK-Festpreis]=@VK_Festpreis" + i + ","
					+ "[Volumen]=@Volumen" + i + ","
					+ "[Warengruppe]=@Warengruppe" + i + ","
					+ "[Warentyp]=@Warentyp" + i + ","
					+ "[Webshop]=@Webshop" + i + ","
					+ "[Werkzeug]=@Werkzeug" + i + ","
					+ "[Wert_Anfangsbestand]=@Wert_Anfangsbestand" + i + ","
					+ "[Zeichnungsnummer]=@Zeichnungsnummer" + i + ","
					+ "[Zeitraum_MHD]=@Zeitraum_MHD" + i + ","
					+ "[Zolltarif_nr]=@Zolltarif_nr" + i + ","
					+ "[Zuschlag_VK]=@Zuschlag_VK" + i + " WHERE [Artikel-Nr]=@Artikel_Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("aktiv" + i, item.aktiv == null ? (object)DBNull.Value : item.aktiv);
					sqlCommand.Parameters.AddWithValue("aktualisiert" + i, item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
					sqlCommand.Parameters.AddWithValue("Anfangsbestand" + i, item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion" + i, item.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : item.ArtikelAusEigenerProduktion);
					sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren" + i, item.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : item.ArtikelFürWeitereBestellungenSperren);
					sqlCommand.Parameters.AddWithValue("Artikelbezeichnung" + i, item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
					sqlCommand.Parameters.AddWithValue("artikelklassifizierung" + i, item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
					sqlCommand.Parameters.AddWithValue("Artikelkurztext" + i, item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
					sqlCommand.Parameters.AddWithValue("Barverkauf" + i, item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
					sqlCommand.Parameters.AddWithValue("BemerkungCRP" + i, item.BemerkungCRP == null ? (object)DBNull.Value : item.BemerkungCRP);
					sqlCommand.Parameters.AddWithValue("BemerkungCRPPlanung" + i, item.BemerkungCRPPlanung == null ? (object)DBNull.Value : item.BemerkungCRPPlanung);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_3" + i, item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
					sqlCommand.Parameters.AddWithValue("BezeichnungAL" + i, item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
					sqlCommand.Parameters.AddWithValue("Blokiert_Status" + i, item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
					sqlCommand.Parameters.AddWithValue("CocVersion" + i, item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
					sqlCommand.Parameters.AddWithValue("COF_Pflichtig" + i, item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
					sqlCommand.Parameters.AddWithValue("CP_required" + i, item.CP_required == null ? (object)DBNull.Value : item.CP_required);
					sqlCommand.Parameters.AddWithValue("Crossreferenz" + i, item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
					sqlCommand.Parameters.AddWithValue("Cu_Gewicht" + i, item.CuGewicht == null ? (object)DBNull.Value : item.CuGewicht);
					sqlCommand.Parameters.AddWithValue("CustomerEnd" + i, item.CustomerEnd == null ? (object)DBNull.Value : item.CustomerEnd);
					sqlCommand.Parameters.AddWithValue("CustomerIndex" + i, item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
					sqlCommand.Parameters.AddWithValue("CustomerIndexSequence" + i, item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence" + i, item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerPrefix" + i, item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
					sqlCommand.Parameters.AddWithValue("CustomerTechnic" + i, item.CustomerTechnic == null ? (object)DBNull.Value : item.CustomerTechnic);
					sqlCommand.Parameters.AddWithValue("CustomerTechnicId" + i, item.CustomerTechnicId == null ? (object)DBNull.Value : item.CustomerTechnicId);
					sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand" + i, item.DatumAnfangsbestand == null ? (object)DBNull.Value : item.DatumAnfangsbestand);
					sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
					sqlCommand.Parameters.AddWithValue("Dienstelistung" + i, item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
					sqlCommand.Parameters.AddWithValue("Dokumente" + i, item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
					sqlCommand.Parameters.AddWithValue("EAN" + i, item.EAN == null ? (object)DBNull.Value : item.EAN);
					sqlCommand.Parameters.AddWithValue("EdiDefault" + i, item.EdiDefault == null ? (object)DBNull.Value : item.EdiDefault);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("EMPB" + i, item.EMPB == null ? (object)DBNull.Value : item.EMPB);
					sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben" + i, item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
					sqlCommand.Parameters.AddWithValue("Ersatzartikel" + i, item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz" + i, item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text" + i, item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
					sqlCommand.Parameters.AddWithValue("Exportgewicht" + i, item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
					sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste" + i, item.fakturierenStückliste == null ? (object)DBNull.Value : item.fakturierenStückliste);
					sqlCommand.Parameters.AddWithValue("Farbe" + i, item.Farbe == null ? (object)DBNull.Value : item.Farbe);
					sqlCommand.Parameters.AddWithValue("fibu_rahmen" + i, item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
					sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern" + i, item.FreigabestatusTNIntern == null ? (object)DBNull.Value : item.FreigabestatusTNIntern);
					sqlCommand.Parameters.AddWithValue("Gebinde" + i, item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
					sqlCommand.Parameters.AddWithValue("Gewicht" + i, item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
					sqlCommand.Parameters.AddWithValue("Grosse" + i, item.Größe == null ? (object)DBNull.Value : item.Größe);
					sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.GrundFürSperre == null ? (object)DBNull.Value : item.GrundFürSperre);
					sqlCommand.Parameters.AddWithValue("gultig_bis" + i, item.gültigBis == null ? (object)DBNull.Value : item.gültigBis);
					sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
					sqlCommand.Parameters.AddWithValue("Hubmastleitungen" + i, item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
					sqlCommand.Parameters.AddWithValue("ID_Klassifizierung" + i, item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("Info_WE" + i, item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
					sqlCommand.Parameters.AddWithValue("IsArticleNumberSpecial" + i, item.IsArticleNumberSpecial == null ? (object)DBNull.Value : item.IsArticleNumberSpecial);
					sqlCommand.Parameters.AddWithValue("IsEDrawing" + i, item.IsEDrawing == null ? (object)DBNull.Value : item.IsEDrawing);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Kategorie" + i, item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
					sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Kriterium1" + i, item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
					sqlCommand.Parameters.AddWithValue("Kriterium2" + i, item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
					sqlCommand.Parameters.AddWithValue("Kriterium3" + i, item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
					sqlCommand.Parameters.AddWithValue("Kriterium4" + i, item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
					sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
					sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
					sqlCommand.Parameters.AddWithValue("Lagerartikel" + i, item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
					sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten" + i, item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
					sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB" + i, item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW" + i, item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
					sqlCommand.Parameters.AddWithValue("Lieferzeit" + i, item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
					sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
					sqlCommand.Parameters.AddWithValue("Manufacturer" + i, item.Manufacturer == null ? (object)DBNull.Value : item.Manufacturer);
					sqlCommand.Parameters.AddWithValue("ManufacturerNextArticle" + i, item.ManufacturerNextArticle == null ? (object)DBNull.Value : item.ManufacturerNextArticle);
					sqlCommand.Parameters.AddWithValue("ManufacturerNextArticleId" + i, item.ManufacturerNextArticleId == null ? (object)DBNull.Value : item.ManufacturerNextArticleId);
					sqlCommand.Parameters.AddWithValue("ManufacturerNumber" + i, item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
					sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticle" + i, item.ManufacturerPreviousArticle == null ? (object)DBNull.Value : item.ManufacturerPreviousArticle);
					sqlCommand.Parameters.AddWithValue("ManufacturerPreviousArticleId" + i, item.ManufacturerPreviousArticleId == null ? (object)DBNull.Value : item.ManufacturerPreviousArticleId);
					sqlCommand.Parameters.AddWithValue("Materialkosten_Alt" + i, item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
					sqlCommand.Parameters.AddWithValue("MHD" + i, item.MHD == null ? (object)DBNull.Value : item.MHD);
					sqlCommand.Parameters.AddWithValue("Minerals_Confirmity" + i, item.MineralsConfirmity == null ? (object)DBNull.Value : item.MineralsConfirmity);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr" + i, item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr" + i, item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit" + i, item.proZeiteinheit == null ? (object)DBNull.Value : item.proZeiteinheit);
					sqlCommand.Parameters.AddWithValue("ProductionCountryCode" + i, item.ProductionCountryCode == null ? (object)DBNull.Value : item.ProductionCountryCode);
					sqlCommand.Parameters.AddWithValue("ProductionCountryName" + i, item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
					sqlCommand.Parameters.AddWithValue("ProductionCountrySequence" + i, item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
					sqlCommand.Parameters.AddWithValue("ProductionLotSize" + i, item.ProductionLotSize == null ? (object)DBNull.Value : item.ProductionLotSize);
					sqlCommand.Parameters.AddWithValue("ProductionSiteCode" + i, item.ProductionSiteCode == null ? (object)DBNull.Value : item.ProductionSiteCode);
					sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
					sqlCommand.Parameters.AddWithValue("ProductionSiteSequence" + i, item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
					sqlCommand.Parameters.AddWithValue("Produktionszeit" + i, item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
					sqlCommand.Parameters.AddWithValue("Projektname" + i, item.Projektname == null ? (object)DBNull.Value : item.Projektname);
					sqlCommand.Parameters.AddWithValue("Provisionsartikel" + i, item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
					sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware" + i, item.PrufstatusTNWare == null ? (object)DBNull.Value : item.PrufstatusTNWare);
					sqlCommand.Parameters.AddWithValue("Rabattierfahig" + i, item.Rabattierfähig == null ? (object)DBNull.Value : item.Rabattierfähig);
					sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
					sqlCommand.Parameters.AddWithValue("Rahmen2" + i, item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf" + i, item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf2" + i, item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge" + i, item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge2" + i, item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr" + i, item.RahmenNr == null ? (object)DBNull.Value : item.RahmenNr);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr2" + i, item.RahmenNr2 == null ? (object)DBNull.Value : item.RahmenNr2);
					sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity" + i, item.REACHSVHCConfirmity == null ? (object)DBNull.Value : item.REACHSVHCConfirmity);
					sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity" + i, item.ROHSEEEConfirmity == null ? (object)DBNull.Value : item.ROHSEEEConfirmity);
					sqlCommand.Parameters.AddWithValue("Seriennummer" + i, item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
					sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung" + i, item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
					sqlCommand.Parameters.AddWithValue("Sonderrabatt" + i, item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
					sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id" + i, item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
					sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
					sqlCommand.Parameters.AddWithValue("Sysmonummer" + i, item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
					sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
					sqlCommand.Parameters.AddWithValue("UL_Etikett" + i, item.ULEtikett == null ? (object)DBNull.Value : item.ULEtikett);
					sqlCommand.Parameters.AddWithValue("UL_zertifiziert" + i, item.ULzertifiziert == null ? (object)DBNull.Value : item.ULzertifiziert);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Ursprungsland" + i, item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
					sqlCommand.Parameters.AddWithValue("VDA_1" + i, item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
					sqlCommand.Parameters.AddWithValue("VDA_2" + i, item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
					sqlCommand.Parameters.AddWithValue("Verpackung" + i, item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
					sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
					sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
					sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
					sqlCommand.Parameters.AddWithValue("Volumen" + i, item.Volumen == null ? (object)DBNull.Value : item.Volumen);
					sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Warentyp" + i, item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
					sqlCommand.Parameters.AddWithValue("Webshop" + i, item.Webshop == null ? (object)DBNull.Value : item.Webshop);
					sqlCommand.Parameters.AddWithValue("Werkzeug" + i, item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
					sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand" + i, item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zeitraum_MHD" + i, item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
					sqlCommand.Parameters.AddWithValue("Zolltarif_nr" + i, item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
					sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int artikel_nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Artikel] WHERE [Artikel-Nr]=@Artikel_Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", artikel_nr);

			results = sqlCommand.ExecuteNonQuery();


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}
			return -1;
		}
		private static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [Artikel] WHERE [Artikel-Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion

		#region Custom Methods
		public static Entities.Tables.PRS.ArtikelEntity GetByArtikelNummer(string? articleNummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel] WHERE [Artikelnummer]=@articleNummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNummer", articleNummer ?? "");
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static Entities.Tables.PRS.ArtikelEntity GetWithIndex(int id, string index)
		{
			index = index ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel] WHERE [Artikel-Nr]=@Id AND [Index_Kunde]=@index";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("index", index ?? "");

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int EditPackaging(Entities.Tables.PRS.ArtikelEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Artikel] SET  [Verpackungsart]=@Verpackungsart,[Verpackungsmenge]=@Verpackungsmenge WHERE [Artikel-Nr]=@ArtikelNr";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ArtikelNr", element.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Verpackungsart", element.Verpackungsart == null ? (object)DBNull.Value : element.Verpackungsart);
				sqlCommand.Parameters.AddWithValue("Verpackungsmenge", element.Verpackungsmenge == null ? (object)DBNull.Value : element.Verpackungsmenge);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int EditDesignation(List<Entities.Tables.PRS.ArtikelEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 109; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = editDesignation(elements);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += editDesignation(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += editDesignation(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber));
				}
			}

			return -1;
		}
		private static int editDesignation(List<Entities.Tables.PRS.ArtikelEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int r = -1;
				using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					cnn.Open();
					string query = "";
					SqlCommand cmd = new SqlCommand(query, cnn);

					int i = 0;
					foreach(Entities.Tables.PRS.ArtikelEntity t in elements)
					{
						i++;
						query += " UPDATE [Artikel] SET "

							+ "[Bezeichnung 1]=@Bezeichnung1" + i + ","
							+ "[Bezeichnung 2]=@Bezeichnung2" + i + ","
							+ "[Bezeichnung 3]=@Bezeichnung3" + i + ","
							+ "[BezeichnungAL]=@BezeichnungAL" + i + " WHERE [Artikel-Nr]=@ArtikelNr" + i
							 + "; ";

						cmd.Parameters.AddWithValue("ArtikelNr" + i, t.ArtikelNr);
						cmd.Parameters.AddWithValue("Bezeichnung1" + i, t.Bezeichnung1 == null ? (object)DBNull.Value : t.Bezeichnung1);
						cmd.Parameters.AddWithValue("Bezeichnung2" + i, t.Bezeichnung2 == null ? (object)DBNull.Value : t.Bezeichnung2);
						cmd.Parameters.AddWithValue("Bezeichnung3" + i, t.Bezeichnung3 == null ? (object)DBNull.Value : t.Bezeichnung3);
						cmd.Parameters.AddWithValue("BezeichnungAL" + i, t.BezeichnungAL == null ? (object)DBNull.Value : t.BezeichnungAL);
					}

					cmd.CommandText = query;

					r = cmd.ExecuteNonQuery();
				}

				return r;
			}

			return -1;
		}
		public static List<Entities.Tables.PRS.ArtikelNrsOnlyEntity> GetROH()
		{
			var result = new List<Entities.Tables.PRS.ArtikelNrsOnlyEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "select [Artikel-Nr],Artikelnummer from Artikel where Warengruppe='ROH'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toNrsList(reader);
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelNrsOnlyEntity> GetNrsAndNummer()
		{
			var result = new List<Entities.Tables.PRS.ArtikelNrsOnlyEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "select [Artikel-Nr],Artikelnummer from Artikel";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toNrsList(reader);
				}
			}

			return result;
		}
		public static Entities.Tables.PRS.ArtikelEntity GetByNumber(string number, bool? includeInactive = null)
		{
			if(string.IsNullOrWhiteSpace(number))
				return null;

			var response = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT top 1 * FROM [Artikel] WHERE LTRIM(RTRIM([Artikelnummer]))=LTRIM(RTRIM(@ArticleNummer))";
				if(includeInactive.HasValue)
				{
					query += $" AND aktiv = {(includeInactive.Value ? "0" : "1")}";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ArticleNummer", number ?? "");

				using(var reader = sqlCommand.ExecuteReader())
				{
					response = toList(reader);
				}
			}

			return response.Count > 0 ? response[0] : null;
		}
		public static Entities.Tables.PRS.ArtikelEntity GetByNumberWithTransaction(string number, bool? includeInactive = null, SqlConnection connection = null, SqlTransaction transaction = null)
		{
			if(string.IsNullOrWhiteSpace(number))
				return null;

			var response = new List<Entities.Tables.PRS.ArtikelEntity>();
			string query = "SELECT top 1 * FROM [Artikel] WHERE LTRIM(RTRIM([Artikelnummer]))=@ArticleNummer";
			if(includeInactive.HasValue)
			{
				query += $" AND aktiv = {(includeInactive.Value ? "0" : "1")}";
			}

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ArticleNummer", number ?? "");

				using(var reader = sqlCommand.ExecuteReader())
				{
					response = toList(reader);
				}
			}

			return response.Count > 0 ? response[0] : null;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetLikeNumber(string number, bool? includeInactive = null)
		{
			if(string.IsNullOrWhiteSpace(number))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT top 1 * FROM [Artikel] WHERE [Artikelnummer] LIKE @ArticleNummer";
				if(includeInactive.HasValue)
				{
					query += $" AND aktiv = {(includeInactive.Value ? "0" : "1")}";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ArticleNummer", $"{number}%");
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.PRS.ArtikelEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByNumbers(List<string> numbers)
		{
			if(numbers != null && numbers.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();
				if(numbers.Count <= maxQueryNumber)
				{
					result = getByNumbers(numbers);
				}
				else
				{
					int batchNumber = numbers.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByNumbers(numbers.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(getByNumbers(numbers.GetRange(batchNumber * maxQueryNumber, numbers.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		private static List<Entities.Tables.PRS.ArtikelEntity> getByNumbers(List<string> numbers)
		{
			if(numbers != null && numbers.Count > 0)
			{
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();

				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < numbers.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, numbers[i]?.Trim());
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [Artikel] WHERE [Artikelnummer] IN (" + queryIds + ")";
					using(var reader = sqlCommand.ExecuteReader())
					{
						result = toList(reader);
					}
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByNumbers(List<string> numbers, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(numbers != null && numbers.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();
				if(numbers.Count <= maxQueryNumber)
				{
					result = getByNumbers(numbers, sqlConnection, sqlTransaction);
				}
				else
				{
					int batchNumber = numbers.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByNumbers(numbers.GetRange(i * maxQueryNumber, maxQueryNumber), sqlConnection, sqlTransaction));
					}
					result.AddRange(getByNumbers(numbers.GetRange(batchNumber * maxQueryNumber, numbers.Count - batchNumber * maxQueryNumber), sqlConnection, sqlTransaction));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		private static List<Entities.Tables.PRS.ArtikelEntity> getByNumbers(List<string> numbers, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(numbers != null && numbers.Count > 0)
			{
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();

				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.Transaction = sqlTransaction;

				string queryIds = string.Empty;
				for(int i = 0; i < numbers.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, numbers[i]?.Trim());
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = "SELECT * FROM [Artikel] WHERE [Artikelnummer] IN (" + queryIds + ")";
				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}

				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByNumbers(List<string> numbers, List<string> customerNumbers)
		{
			if(numbers != null && numbers.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();
				if(numbers.Count <= maxQueryNumber)
				{
					result = getByNumbers(numbers, customerNumbers);
				}
				else
				{
					int batchNumber = numbers.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByNumbers(numbers.GetRange(i * maxQueryNumber, maxQueryNumber), customerNumbers));
					}
					result.AddRange(getByNumbers(numbers.GetRange(batchNumber * maxQueryNumber, numbers.Count - batchNumber * maxQueryNumber), customerNumbers));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		private static List<Entities.Tables.PRS.ArtikelEntity> getByNumbers(List<string> numbers, List<string> customerNumbers)
		{
			if(numbers != null && numbers.Count > 0)
			{
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();

				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < numbers.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, numbers[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					if(customerNumbers != null && customerNumbers.Count > 0)
					{
						sqlCommand.CommandText = "SELECT * FROM [Artikel] WHERE [Artikelnummer] IN (" + queryIds + ") OR [Bezeichnung 1] LIKE '%" + string.Join("%' OR [Bezeichnung 1] LIKE '%", customerNumbers.Select(x => x.SqlEscape())) + "%'";
					}
					else
					{
						sqlCommand.CommandText = "SELECT * FROM [Artikel] WHERE [Artikelnummer] IN (" + queryIds + ")";
					}

					using(var reader = sqlCommand.ExecuteReader())
					{
						result = toList(reader);
					}
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}

		public static List<Entities.Tables.PRS.ArtikelEntity> GetByBezeichnung1(List<string> bezeichnung1s)
		{
			if(bezeichnung1s != null && bezeichnung1s.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();
				if(bezeichnung1s.Count <= maxQueryNumber)
				{
					result = getByBezeichnung1(bezeichnung1s);
				}
				else
				{
					int batchNumber = bezeichnung1s.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByBezeichnung1(bezeichnung1s.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(getByBezeichnung1(bezeichnung1s.GetRange(batchNumber * maxQueryNumber, bezeichnung1s.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		private static List<Entities.Tables.PRS.ArtikelEntity> getByBezeichnung1(List<string> bezeichnung1s)
		{
			if(bezeichnung1s != null && bezeichnung1s.Count > 0)
			{
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();

				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					//string queryIds = string.Empty;
					//for (int i = 0; i < bezeichnung1s.Count; i++)
					//{
					//    queryIds += "@Id" + i + ",";
					//    sqlCommand.Parameters.AddWithValue("Id" + i, bezeichnung1s[i]);
					//}
					//queryIds = queryIds.TrimEnd(',');

					var bez = bezeichnung1s.Select(x => $"[Bezeichnung 1]  LIKE '%{x.SqlEscape()}%'");

					//sqlCommand.CommandText = "SELECT * FROM [Artikel] WHERE [Bezeichnung 1] IN (" + queryIds + ")";
					sqlCommand.CommandText = $"SELECT * FROM [Artikel] WHERE {string.Join(" OR ", bez.Select(x => x))}";

					using(var reader = sqlCommand.ExecuteReader())
					{
						result = toList(reader);
					}
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByBezeichnung1(List<string> bezeichnung1s, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(bezeichnung1s != null && bezeichnung1s.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();
				if(bezeichnung1s.Count <= maxQueryNumber)
				{
					result = getByBezeichnung1(bezeichnung1s, sqlConnection, sqlTransaction);
				}
				else
				{
					int batchNumber = bezeichnung1s.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByBezeichnung1(bezeichnung1s.GetRange(i * maxQueryNumber, maxQueryNumber), sqlConnection, sqlTransaction));
					}
					result.AddRange(getByBezeichnung1(bezeichnung1s.GetRange(batchNumber * maxQueryNumber, bezeichnung1s.Count - batchNumber * maxQueryNumber), sqlConnection, sqlTransaction));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		private static List<Entities.Tables.PRS.ArtikelEntity> getByBezeichnung1(List<string> bezeichnung1s, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(bezeichnung1s != null && bezeichnung1s.Count > 0)
			{
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();

				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.Transaction = sqlTransaction;

				var bez = bezeichnung1s.Select(x => $"[Bezeichnung 1]  LIKE '%{x.SqlEscape()}%'");

				sqlCommand.CommandText = $"SELECT * FROM [Artikel] WHERE {string.Join(" OR ", bez.Select(x => x))}";

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}

		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCustomerItemNumbersOrBz(List<string> bezeichnung1s, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(bezeichnung1s != null && bezeichnung1s.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();
				if(bezeichnung1s.Count <= maxQueryNumber)
				{
					result = getByCustomerItemNumbersOrBz(bezeichnung1s, sqlConnection, sqlTransaction);
				}
				else
				{
					int batchNumber = bezeichnung1s.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByCustomerItemNumbersOrBz(bezeichnung1s.GetRange(i * maxQueryNumber, maxQueryNumber), sqlConnection, sqlTransaction));
					}
					result.AddRange(getByCustomerItemNumbersOrBz(bezeichnung1s.GetRange(batchNumber * maxQueryNumber, bezeichnung1s.Count - batchNumber * maxQueryNumber), sqlConnection, sqlTransaction));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		private static List<Entities.Tables.PRS.ArtikelEntity> getByCustomerItemNumbersOrBz(List<string> bezeichnung1s, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(bezeichnung1s != null && bezeichnung1s.Count > 0)
			{
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();

				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.Transaction = sqlTransaction;

				var bez = bezeichnung1s.Select(x => $"([Bezeichnung 1]='{x.SqlEscape()}' OR [CustomerItemNumber]='{x.SqlEscape()}')");

				sqlCommand.CommandText = $"SELECT * FROM [Artikel] WHERE {string.Join(" OR ", bez.Select(x => x))}";

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}

		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCustomerItemNumbersOrBz(List<string> bezeichnung1s)
		{
			if(bezeichnung1s != null && bezeichnung1s.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();
				if(bezeichnung1s.Count <= maxQueryNumber)
				{
					result = getByCustomerItemNumbersOrBz(bezeichnung1s);
				}
				else
				{
					int batchNumber = bezeichnung1s.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByCustomerItemNumbersOrBz(bezeichnung1s.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(getByCustomerItemNumbersOrBz(bezeichnung1s.GetRange(batchNumber * maxQueryNumber, bezeichnung1s.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		private static List<Entities.Tables.PRS.ArtikelEntity> getByCustomerItemNumbersOrBz(List<string> bezeichnung1s)
		{
			if(bezeichnung1s != null && bezeichnung1s.Count > 0)
			{
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					var bez = bezeichnung1s.Select(x => $"([Bezeichnung 1]='{x.SqlEscape()}' OR [CustomerItemNumber]='{x.SqlEscape()}')");

					var query = $"SELECT * FROM [Artikel] WHERE {string.Join(" OR ", bez.Select(x => x))}";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					using(var reader = sqlCommand.ExecuteReader())
					{
						result = toList(reader);
					}
					return result;
				}
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}

		public static List<Entities.Tables.PRS.ArtikelEntity> GetLikeNumber(string searchText)
		{
			searchText = searchText ?? "";
			var response = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @" SELECT * FROM [Artikel] WHERE [Artikelnummer] LIKE @searchText and Warengruppe='EF' order by [Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("searchText", "%" + searchText + "%");

				using(var reader = sqlCommand.ExecuteReader())
				{
					return toList(reader);
				}
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetLikeNumberPreffix(string searchText)
		{
			searchText = searchText ?? "";
			var response = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @" SELECT * FROM [Artikel] WHERE [Artikelnummer] LIKE @searchText";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("searchText", searchText + "%");

				using(var reader = sqlCommand.ExecuteReader())
				{
					return toList(reader);
				}
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetLikeNumberPreffixWDelFixed(string searchText)
		{
			searchText = searchText ?? "";
			var response = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @" SELECT * FROM [Artikel] WHERE [Artikelnummer] LIKE @searchText AND [DEL fixiert]=1";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("searchText", searchText + "%");

				using(var reader = sqlCommand.ExecuteReader())
				{
					return toList(reader);
				}
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetLikeNumberSuffix(string searchText)
		{
			searchText = searchText ?? "";
			var response = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @" SELECT * FROM [Artikel] WHERE [Artikelnummer] LIKE @searchText";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("searchText", "%" + searchText);

				using(var reader = sqlCommand.ExecuteReader())
				{
					return toList(reader);
				}
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetLikeNumberV2(string searchText)
		{
			var response = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT * FROM [Artikel] WHERE ([Artikelnummer] LIKE @searchText AND Warengruppe='EF')
                               OR [Artikelnummer] LIKE @searchText
                               ORDER BY [Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("searchText", "%" + searchText + "%");

				using(var reader = sqlCommand.ExecuteReader())
				{
					return toList(reader);
				}
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetLikeNumberV3(string searchText)
		{
			var response = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT TOP 10 * FROM [Artikel] WHERE ([Artikelnummer] LIKE @searchText AND Warengruppe='EF')
                               ORDER BY [Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("searchText", searchText + "%");

				using(var reader = sqlCommand.ExecuteReader())
				{
					return toList(reader);
				}
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetProductionOriginalArticles()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel]"
						+ " WHERE  Artikelnummer Not Like 'Endkontroll%'"
						+ " And Artikelnummer Not Like 'Rep%'"
						+ " And Artikelnummer Not Like 'Umb%'"
						+ " And Artikelnummer Not Like 'Analy%'"
						+ " And Artikelnummer Not Like 'Technik%'"
						+ " AND Freigabestatus<> 'O'"
						+ " AND Stückliste = 1 AND Artikel.aktiv = 1"
						+ " ORDER BY Artikelnummer; ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetProductionOriginalArticles(string name)
		{
			name = name.SqlEscape();
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel]"
						+ " WHERE  Artikelnummer Not Like 'Endkontroll%'"
						+ " And Artikelnummer Not Like 'Rep%'"
						+ " And Artikelnummer Not Like 'Umb%'"
						+ " And Artikelnummer Not Like 'Analy%'"
						+ " And Artikelnummer Not Like 'Technik%'"
						+ " AND Freigabestatus <> 'O'"
						+ " AND Stückliste = 1 AND Artikel.aktiv = 1"
						+ $" AND (Artikelnummer LIKE '%{name.SqlEscape()}%' OR [Bezeichnung 1] LIKE '%{name.SqlEscape()}%')"
						+ " ORDER BY Artikelnummer; ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}

		#region ♥♥♥♥♥♥-PSZ-♥♥♥♥♥♥ searchAdvanced
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> SearchAdvancedByNrNumberDesignation(
		List<string> articleNummers, string nummer, string designation, string goodsGroup, bool? active, string articleFamily, string customerItemNumber, string details, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging
		 , string AddedQuery)

		{

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT Count(*) over() as CountRows, * FROM [Artikel] ";
				string clause = " WHERE [Artikel-Nr] IS NOT NULL";


				if(AddedQuery != null && AddedQuery != "")
				{
					clause += AddedQuery;
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					clause += $" ORDER BY {(sorting.SortFieldName == "[ArtikelNummer]" ? "CAST(Replace(LEFT(SUBSTRING([ArtikelNummer], PATINDEX('%[0-9.-]%', [ArtikelNummer]), 8000), PATINDEX('%[^0-9.-]%', SUBSTRING([ArtikelNummer], PATINDEX('%[0-9.-]%', [ArtikelNummer]), 8000) + 'X') -1), '-', '') AS BIGINT)" : sorting.SortFieldName)} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					clause += " ORDER BY [Artikel-Nr] DESC ";
				}

				if(paging != null)
				{
					clause += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = query + clause;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			}

			return toList(dataTable);
		}



		#endregion
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> SearchByNrNumberDesignation(
			List<string> articleNummers, string nummer, string designation, string goodsGroup, bool? active, string articleFamily, string customerItemNumber, string details, bool? ediDefault, bool? eDrawing, string ArticleReference, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			nummer = nummer?.Trim();
			designation = designation?.Trim();
			goodsGroup = goodsGroup?.Trim();
			articleFamily = articleFamily?.Trim();
			customerItemNumber = customerItemNumber?.Trim();
			details = details?.Trim();
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT * FROM [Artikel] ";
				string clause = " WHERE [Artikel-Nr] IS NOT NULL";

				if(ArticleReference is not null && !string.IsNullOrEmpty(ArticleReference))
				{
					query += @" a right join ArtikelCustomerReferences ar on a.[Artikel-Nr] = ar.ArticleId ";
					clause += @$" AND  ar.CustomerReference like '%{ArticleReference.Trim()}%' ";
				}
				if(articleNummers != null)
				{
					clause += $" AND [Artikelnummer] IN ('{string.Join("','", articleNummers.Select(x => x.SqlEscape()))}') ";
				}
				if(!string.IsNullOrEmpty(nummer) && !string.IsNullOrWhiteSpace(nummer))
				{
					clause += $" AND [Artikelnummer] LIKE '{nummer.SqlEscape()}%' ";
				}
				if(!string.IsNullOrWhiteSpace(designation))
				{
					clause += $" AND ([Bezeichnung 1] Like '%{designation.SqlEscape(true)}%' OR  [Bezeichnung 2] Like '%{designation.SqlEscape(true)}%' OR  [Bezeichnung 3] Like '%{designation.SqlEscape(true)}%') ";
				}
				if(!string.IsNullOrWhiteSpace(articleFamily))
				{
					clause += $" AND ([Artikelfamilie_Kunde] Like '%{articleFamily.SqlEscape(true)}%') ";
				}
				if(active.HasValue)
				{
					clause += $" AND Aktiv = {(active.Value ? 1 : 0)} ";
				}
				if(!string.IsNullOrEmpty(goodsGroup) && !string.IsNullOrWhiteSpace(goodsGroup))
				{
					clause += $" AND [Warengruppe] = '{goodsGroup}' ";
				}
				if(!string.IsNullOrWhiteSpace(customerItemNumber))
				{
					clause += $" AND ([CustomerItemNumber] = '{customerItemNumber.SqlEscape(true)}' OR [ManufacturerNumber]='{customerItemNumber.SqlEscape(true)}') ";
				}
				if(!string.IsNullOrWhiteSpace(details))
				{
					clause += $" AND ([Artikelfamilie_Kunde_Detail1] Like '%{details.SqlEscape(true)}%' OR [Artikelfamilie_Kunde_Detail2] Like '%{details.SqlEscape(true)}%') ";
				}
				if(ediDefault.HasValue)
				{
					clause += $" AND [EdiDefault] = {(ediDefault.Value ? 1 : 0)} ";
				}
				if(eDrawing.HasValue)
				{
					clause += $" AND [IsEDrawing] = {(eDrawing.Value ? 1 : 0)} ";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					clause += $" ORDER BY {(sorting.SortFieldName == "[ArtikelNummer]" ? "CAST(Replace(LEFT(SUBSTRING([ArtikelNummer], PATINDEX('%[0-9.-]%', [ArtikelNummer]), 8000), PATINDEX('%[^0-9.-]%', SUBSTRING([ArtikelNummer], PATINDEX('%[0-9.-]%', [ArtikelNummer]), 8000) + 'X') -1), '-', '') AS BIGINT)" : sorting.SortFieldName)} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					clause += " ORDER BY [Artikel-Nr] DESC ";
				}

				if(paging != null)
				{
					clause += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = query + clause;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}


			if(dataTable.Rows.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			}

			return toList(dataTable);
		}


		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> SearchByNrNumberDesignationAdvanced(string condition, bool includeExtension, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $" SELECT * FROM [Artikel] {(includeExtension ? " a LEFT JOIN [__PRS_ArtikelExtension] e on e.[ArtikelNr]=a.[Artikel-Nr]" : "")}";
				string clause = " WHERE [Artikel-Nr] IS NOT NULL ";
				clause += condition;
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					clause += $" ORDER BY {(sorting.SortFieldName == "[ArtikelNummer]" ? "CAST(Replace(LEFT(SUBSTRING([ArtikelNummer], PATINDEX('%[0-9.-]%', [ArtikelNummer]), 8000), PATINDEX('%[^0-9.-]%', SUBSTRING([ArtikelNummer], PATINDEX('%[0-9.-]%', [ArtikelNummer]), 8000) + 'X') -1), '-', '') AS BIGINT)" : sorting.SortFieldName)} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					clause += " ORDER BY [Artikel-Nr] DESC ";
				}
				if(paging != null)
				{
					clause += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = query + clause;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			}

			return toList(dataTable);
		}

		public static int SearchByNrNumberDesignation_CountAll(List<string> articleNummers, string nummer, string designation, string goodsGroup, bool? active, string articleFamily, string customerItemNumber, string details, bool? ediDefault, bool? eDrawing, string ArticleReference = null)
		{
			nummer = nummer?.Trim();
			designation = designation?.Trim();
			goodsGroup = goodsGroup?.Trim();
			articleFamily = articleFamily?.Trim();
			details = details?.Trim();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT COUNT(*) FROM [Artikel] ";
				string clause = " WHERE [Artikel-Nr] IS NOT NULL";

				if(ArticleReference is not null && !string.IsNullOrEmpty(ArticleReference))
				{
					query += @" a right join ArtikelCustomerReferences ar on a.[Artikel-Nr] = ar.ArticleId ";
					clause += @$" AND  ar.CustomerReference like '%{ArticleReference.Trim()}%' ";
				}
				if(articleNummers != null)
				{
					clause += $" AND [Artikelnummer] IN ('{string.Join("','", articleNummers.Select(x => x.SqlEscape()))}') ";
				}
				if(!string.IsNullOrEmpty(nummer) && !string.IsNullOrWhiteSpace(nummer))
				{
					clause += $" AND [Artikelnummer] LIKE '{nummer.SqlEscape()}%' ";
				}
				if(!string.IsNullOrWhiteSpace(designation))
				{
					clause += $" AND ([Bezeichnung 1] Like '%{designation.SqlEscape(true)}%' OR  [Bezeichnung 2] Like '%{designation.SqlEscape(true)}%' OR  [Bezeichnung 3] Like '%{designation.SqlEscape(true)}%') ";
				}
				if(!string.IsNullOrWhiteSpace(articleFamily))
				{
					clause += $" AND ([Artikelfamilie_Kunde] Like '%{articleFamily.SqlEscape(true)}%') ";
				}
				if(active.HasValue)
				{
					clause += $" AND Aktiv = {(active.Value ? 1 : 0)} ";
				}
				if(!string.IsNullOrEmpty(goodsGroup) && !string.IsNullOrWhiteSpace(goodsGroup))
				{
					clause += $" AND [Warengruppe] = '{goodsGroup}' ";
				}
				if(!string.IsNullOrWhiteSpace(customerItemNumber))
				{
					clause += $" AND ([CustomerItemNumber] = '{customerItemNumber.SqlEscape(true)}' OR [ManufacturerNumber]='{customerItemNumber.SqlEscape(true)}') ";
				}
				if(!string.IsNullOrWhiteSpace(details))
				{
					clause += $" AND ([Artikelfamilie_Kunde_Detail1] Like '%{details.SqlEscape(true)}%' OR [Artikelfamilie_Kunde_Detail2] Like '%{details.SqlEscape(true)}%') ";
				}
				if(ediDefault.HasValue)
				{
					clause += $" AND [EdiDefault] = {(ediDefault.Value ? 1 : 0)} ";
				}
				if(eDrawing.HasValue)
				{
					clause += $" AND [IsEDrawing] = {(eDrawing.Value ? 1 : 0)} ";
				}

				using(var sqlCommand = new SqlCommand())
				{

					sqlCommand.CommandText = query + clause;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}
		public static int SearchByNrNumberDesignationAdvanced_CountAll(string Condition, bool includeExtension)
		{

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $" SELECT COUNT(*) FROM [Artikel] {(includeExtension ? " a LEFT JOIN [__PRS_ArtikelExtension] e on e.[ArtikelNr]=a.[Artikel-Nr]" : "")}";
				string clause = " WHERE [Artikel-Nr] IS NOT NULL";
				clause += Condition;


				using(var sqlCommand = new SqlCommand())
				{

					sqlCommand.CommandText = query + clause;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetLikeNr(string nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT * FROM [Artikel] WHERE [Artikel-Nr] LIKE '{nr}%' ORDER by [Artikel-Nr] ASC";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetLikeNummer(string nummer, int? maxItemsCount = null)
		{
			nummer = nummer?.Trim() ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query;
				if(String.IsNullOrEmpty(nummer))
				{
					query = "SELECT TOP 10 * FROM [Artikel] ORDER by [Artikelnummer] ASC";
				}
				else
				{
					query = $"SELECT TOP 10 * FROM [Artikel] WHERE [Artikelnummer] LIKE '{nummer.SqlEscape()}%' ORDER by [Artikelnummer] ASC";
				}

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetNumberStartingWith(string nummer, string hm)
		{
			nummer = nummer?.Trim() ?? "";
			hm = hm?.Trim() ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT TOP 10 * FROM [Artikel] WHERE [Artikelnummer] LIKE '{nummer.SqlEscape()}%' ORDER by [Artikelnummer] ASC";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetLikeDesignation(string text, int? maxItemsCount = null)
		{
			text = text?.Trim();
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT {(maxItemsCount.HasValue ? $"TOP {maxItemsCount.Value}" : "")} * FROM [Artikel] WHERE LTRIM([Bezeichnung 1]) LIKE '%{text.SqlEscape()}%' OR LTRIM([Bezeichnung 2]) LIKE '%{text.SqlEscape()}%' OR LTRIM([Bezeichnung 3]) LIKE '%{text.SqlEscape()}%' ORDER by [Bezeichnung 1] ASC";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static int ToggleActiveStatus(int artikelNr)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Artikel] SET [aktiv]= CASE WHEN [aktiv] IS NULL THEN 0 ELSE 1-[aktiv] END  WHERE [Artikel-Nr]=@ArtikelNr";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ArtikelNr", artikelNr);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}


		public static List<Entities.Tables.PRS.ArtikelEntity> GetByClassId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand("SELECT * FROM [Artikel] WHERE [ID_Klassifizierung]=@id", sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("id", id);
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByProjectType(string projectType)
		{
			if(string.IsNullOrWhiteSpace(projectType))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand("SELECT * FROM [Artikel] WHERE RTRIM(LTRIM([artikelklassifizierung]))=@projectType", sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("projectType", projectType.Trim());
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByProductGroupName(string name)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand($"SELECT * FROM [Artikel] WHERE [Warengruppe]='{name.SqlEscape()}' ORDER by [Artikelnummer] ASC", sqlConnection))
				{
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}


		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCheckStatus(string status)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand($"SELECT * FROM [Artikel] WHERE [Prüfstatus TN Ware]='{status.SqlEscape()}' ORDER by [Artikelnummer] ASC", sqlConnection))
				{
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByExternalStatus(string status)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand($"SELECT * FROM [Artikel] WHERE [Freigabestatus]='{status.SqlEscape()}' ORDER by [Artikelnummer] ASC", sqlConnection))
				{
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByInternalStatus(string status)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand($"SELECT * FROM [Artikel] WHERE [Freigabestatus TN intern]='{status.SqlEscape()}' ORDER by [Artikelnummer] ASC", sqlConnection))
				{
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByMHDTag(string status)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand($"SELECT * FROM [Artikel] WHERE [Zeitraum_MHD]='{status.SqlEscape()}' ORDER by [Artikelnummer] ASC", sqlConnection))
				{
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.PRS.ArtikelEntity> GetByNotNumbers(List<int> numbers, string searchNummer, int? max)
		{
			if(numbers != null && numbers.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();
				if(numbers.Count <= maxQueryNumber)
				{
					result = getByNotNumbers(numbers, searchNummer, max);
				}
				else
				{
					int batchNumber = numbers.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByNotNumbers(numbers.GetRange(i * maxQueryNumber, maxQueryNumber), searchNummer, max));
					}
					result.AddRange(getByNotNumbers(numbers.GetRange(batchNumber * maxQueryNumber, numbers.Count - batchNumber * maxQueryNumber), searchNummer, max));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		private static List<Entities.Tables.PRS.ArtikelEntity> getByNotNumbers(List<int> numbers, string searchNummer, int? max)
		{
			if(!max.HasValue)
				max = 1000;

			if(numbers != null && numbers.Count > 0)
			{
				var result = new List<Entities.Tables.PRS.ArtikelEntity>();

				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					sqlCommand.CommandText = $"SELECT TOP ({max}) * FROM [Artikel] WHERE [Artikelnummer] LIKE '{searchNummer.SqlEscape()}%' AND [Artikel-Nr] NOT IN ({string.Join(", ", numbers)}) ORDER by [Artikelnummer] ASC ";

					using(var reader = sqlCommand.ExecuteReader())
					{
						result = toList(reader);
					}
				}
				return result;
			}
			return new List<Entities.Tables.PRS.ArtikelEntity>();
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> GetArticleCandidates(int articleId, string searchNummer, int max)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Artikel] WHERE [Artikel-Nr]<>@articleId AND [Artikelnummer] LIKE '{searchNummer.SqlEscape()}%' ORDER by [Artikelnummer] ASC";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			}
		}
		public static IEnumerable<int> GetUmbauIds()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT [Artikel-Nr] FROM [Artikel] WHERE TRIM([Warengruppe]) = 'UM'";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => int.Parse(x[0].ToString()));
			}
			else
			{
				return null;
			}
		}

		#region Tabs Cruds
		public static Entities.Tables.PRS.ArtikelEntity GetArticleOverview(int number)
		{
			var response = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "select [Artikel-Nr],Artikelnummer,[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3] from Artikel where [Artikel-Nr]=@Artikel_nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikel_nr", number);

				using(var reader = sqlCommand.ExecuteReader())
				{
					response = toList(reader);
				}
			}

			return response.Count > 0 ? response[0] : null;
		}


		#endregion

		#region >>>> Overview
		public static int EditOverview(Entities.Tables.PRS.ArtikelEntity element, SqlConnection connection, SqlTransaction transaction)
		{
			int response = -1;
			string query = "UPDATE [Artikel] SET [Bezeichnung 1]=@Bezeichnung1," +
				"[Bezeichnung 2]=@Bezeichnung2,[Bezeichnung 3]=@Bezeichnung3, [Verpackung]=@Verpackung,[Artikelbezeichnung]=@Artikelbezeichnung," +
				"[Losgroesse]=@Losgroesse,[ProductionlotSize]=@Produktionlosgrosse,[Langtext]=@Langtext,[Lieferzeit]=@Lieferzeit,[Manufacturer]=@Manufacturer,[ManufacturerNumber]=@ManufacturerNumber WHERE [Artikel-Nr]=@ArtikelNr";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ArtikelNr", element.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Bezeichnung1", element.Bezeichnung1 == null ? (object)DBNull.Value : element.Bezeichnung1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung2", element.Bezeichnung2 == null ? (object)DBNull.Value : element.Bezeichnung2);
				sqlCommand.Parameters.AddWithValue("Bezeichnung3", element.Bezeichnung3 == null ? (object)DBNull.Value : element.Bezeichnung3);
				sqlCommand.Parameters.AddWithValue("Verpackung", element.Verpackung == null ? (object)DBNull.Value : element.Verpackung);
				sqlCommand.Parameters.AddWithValue("Artikelbezeichnung", element.Artikelbezeichnung == null ? (object)DBNull.Value : element.Artikelbezeichnung);
				sqlCommand.Parameters.AddWithValue("Losgroesse", element.Losgroesse == null ? (object)DBNull.Value : element.Losgroesse);
				sqlCommand.Parameters.AddWithValue("Produktionlosgrosse", element.Produktionlosgrosse == null ? (object)DBNull.Value : element.Produktionlosgrosse);
				sqlCommand.Parameters.AddWithValue("Langtext", element.Langtext == null ? (object)DBNull.Value : element.Langtext);
				sqlCommand.Parameters.AddWithValue("Lieferzeit", element.Lieferzeit == null ? (object)DBNull.Value : element.Lieferzeit);
				sqlCommand.Parameters.AddWithValue("Manufacturer", element.Manufacturer == null ? (object)DBNull.Value : element.Manufacturer);
				sqlCommand.Parameters.AddWithValue("ManufacturerNumber", element.ManufacturerNumber == null ? (object)DBNull.Value : element.ManufacturerNumber);
				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int updatePreviousManufacturerAticle(int articleNr, int prevArticleId, string prevArticle, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Artikel] SET [ManufacturerPreviousArticleId]=@prevArticleId, [ManufacturerPreviousArticle]=@prevArticle where [Artikel-Nr]=@Artikel_Nr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", articleNr);
				sqlCommand.Parameters.AddWithValue("prevArticleId", prevArticleId);
				sqlCommand.Parameters.AddWithValue("prevArticle", prevArticle);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int ResetPreviousManufacturerAticle(int articleNr, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Artikel] SET [ManufacturerPreviousArticleId]=NULL, [ManufacturerPreviousArticle]=NULL where [Artikel-Nr]=@Artikel_Nr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", articleNr);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int updateNextManufacturerAticle(int articleNr, int nextArticleId, string nextArticle, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Artikel] SET [ManufacturerNextArticleId]=@nextArticleId, [ManufacturerNextArticle]=@nextArticle where [Artikel-Nr]=@Artikel_Nr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", articleNr);
				sqlCommand.Parameters.AddWithValue("nextArticleId", nextArticleId);
				sqlCommand.Parameters.AddWithValue("nextArticle", nextArticle);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int ResetNextManufacturerAticle(int articleNr, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Artikel] SET [ManufacturerNextArticleId]=NULL, [ManufacturerNextArticle]=NULL where [Artikel-Nr]=@Artikel_Nr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", articleNr);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int updatePmDatArticle(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity article, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Artikel] SET [Projektname]=@Projektname, [CustomerEnd]=@CustomerEnd, [CustomerTechnic]=@CustomerTechnic, [CustomerTechnicId]=@CustomerTechnicId where [Artikel-Nr]=@ArtikelNr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ArtikelNr", article.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Projektname", article.Projektname == null ? (object)DBNull.Value : article.Projektname);
				sqlCommand.Parameters.AddWithValue("CustomerEnd", article.CustomerEnd == null ? (object)DBNull.Value : article.CustomerEnd);
				sqlCommand.Parameters.AddWithValue("CustomerTechnic", article.CustomerTechnic == null ? (object)DBNull.Value : article.CustomerTechnic);
				sqlCommand.Parameters.AddWithValue("CustomerTechnicId", article.CustomerTechnicId == null ? (object)DBNull.Value : article.CustomerTechnicId);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		#endregion

		#region >>> data
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByKundeBezeichnung(string number, string bezeichnung)
		{
			SqlDataAdapter SelectAdapter = new SqlDataAdapter();
			DataTable dt = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Artikel] WHERE [Artikelnummer] LIKE '{number.Substring(0, Math.Max(number.IndexOf('-') + 1, 3)).SqlEscape()}%' AND [Bezeichnung 1]=@bezeichnung";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("bezeichnung", bezeichnung ?? "");

				SelectAdapter = new SqlDataAdapter(sqlCommand);
				SelectAdapter.Fill(dt);
			}

			if(dt.Rows.Count > 0)
			{
				return dt.Rows.Cast<DataRow>().Select(x => new Entities.Tables.PRS.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.PRS.ArtikelEntity>();

			}
		}
		public static int EditData(Entities.Tables.PRS.ArtikelEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Artikel] SET " +

					"[ArtikelNummer]=@ArtikelNummer," +
					"[Bezeichnung 1]=@Bezeichnung1," +
					"[Bezeichnung 2]=@Bezeichnung2,[Bezeichnung 3]=@Bezeichnung3," +
					"[Cu-Gewicht]=@CuGewicht,[DEL]=@DEL,[DEL fixiert]=@DELFixiert," +

					"[Index_Kunde]=@Index_Kunde,[Index_Kunde_Datum]=@Index_Kunde_Datum," +
					"[Klassifizierung]=@Klassifizierung,[ID_Klassifizierung]=@ID_Klassifizierung," +
					//"[Kupferbasis]=@Kupferbasis," +
					"[UL Etikett]=@ULEtikett," +
					"[UL zertifiziert]=@ULzertifiziert," +
					"[VK-Festpreis]=@VKFestpreis," +
					"[Warengruppe]=@Warengruppe," +

					"[Größe]=@Größe," +
					"[Lagerartikel]=@Lagerartikel," +
					"[Stückliste]=@Stückliste," +
					"[artikelklassifizierung]=@artikelklassifizierung," +
					"[Einheit]=@Einheit," +
					"[Zeichnungsnummer]=@Zeichnungsnummer," +
					"[Verpackungsmenge]=@Verpackungsmenge," +
					"[Stundensatz]=@Stundensatz," +
					"[Warentyp]=@Warentyp," +
					"[Gewicht]=@Gewicht," +
					"[Kupferzahl]=@Kupferzahl," +
					"[Zuschlag_VK]=@Zuschlag_VK," +
					"[Umsatzsteuer]=@Umsatzsteuer," +
					"[Preiseinheit]=@Preiseinheit," +
					"[Werkzeug]=@Werkzeug," +
					"[Langtext]=@Langtext," +
					"[Verpackung]=@Verpackung," +
					"[Lieferzeit]=@Lieferzeit," +
					"[Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1," +
					"[Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2," +
					"[Artikelfamilie_Kunde]=@Artikelfamilie_Kunde" +
					" WHERE [Artikel-Nr]=@ArtikelNr";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ArtikelNr", element.ArtikelNr);

				sqlCommand.Parameters.AddWithValue("ArtikelNummer", element.ArtikelNummer == null ? (object)DBNull.Value : element.ArtikelNummer);
				sqlCommand.Parameters.AddWithValue("Bezeichnung1", element.Bezeichnung1 == null ? (object)DBNull.Value : element.Bezeichnung1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung2", element.Bezeichnung2 == null ? (object)DBNull.Value : element.Bezeichnung2);
				sqlCommand.Parameters.AddWithValue("Bezeichnung3", element.Bezeichnung3 == null ? (object)DBNull.Value : element.Bezeichnung3);
				sqlCommand.Parameters.AddWithValue("CuGewicht", element.CuGewicht == null ? (object)DBNull.Value : element.CuGewicht);
				sqlCommand.Parameters.AddWithValue("DEL", element.DEL == null ? (object)DBNull.Value : element.DEL);
				sqlCommand.Parameters.AddWithValue("DELFixiert", element.DELFixiert == null ? (object)DBNull.Value : element.DELFixiert);
				sqlCommand.Parameters.AddWithValue("Index_Kunde", element.Index_Kunde == null ? (object)DBNull.Value : element.Index_Kunde);

				sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", element.Index_Kunde_Datum == null ? (object)DBNull.Value : element.Index_Kunde_Datum);
				sqlCommand.Parameters.AddWithValue("Klassifizierung", element.Klassifizierung == null ? (object)DBNull.Value : element.Klassifizierung);
				sqlCommand.Parameters.AddWithValue("ID_Klassifizierung", element.ID_Klassifizierung == null ? (object)DBNull.Value : element.ID_Klassifizierung);

				sqlCommand.Parameters.AddWithValue("Kupferbasis", element.Kupferbasis == null ? (object)DBNull.Value : element.Kupferbasis);
				sqlCommand.Parameters.AddWithValue("ULEtikett", element.ULEtikett == null ? (object)DBNull.Value : element.ULEtikett);
				sqlCommand.Parameters.AddWithValue("ULzertifiziert", element.ULzertifiziert == null ? (object)DBNull.Value : element.ULzertifiziert);
				sqlCommand.Parameters.AddWithValue("VKFestpreis", element.VKFestpreis == null ? (object)DBNull.Value : element.VKFestpreis);
				sqlCommand.Parameters.AddWithValue("Warengruppe", element.Warengruppe == null ? (object)DBNull.Value : element.Warengruppe);

				sqlCommand.Parameters.AddWithValue("Größe", element.Größe == null ? (object)DBNull.Value : element.Größe);
				sqlCommand.Parameters.AddWithValue("Lagerartikel", element.Lagerartikel == null ? (object)DBNull.Value : element.Lagerartikel);
				sqlCommand.Parameters.AddWithValue("Stückliste", element.Stuckliste == null ? (object)DBNull.Value : element.Stuckliste);
				sqlCommand.Parameters.AddWithValue("artikelklassifizierung", element.artikelklassifizierung == null ? (object)DBNull.Value : element.artikelklassifizierung);
				sqlCommand.Parameters.AddWithValue("Einheit", element.Einheit == null ? (object)DBNull.Value : element.Einheit);
				sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", element.Zeichnungsnummer == null ? (object)DBNull.Value : element.Zeichnungsnummer);
				sqlCommand.Parameters.AddWithValue("Verpackungsmenge", element.Verpackungsmenge == null ? (object)DBNull.Value : element.Verpackungsmenge);
				sqlCommand.Parameters.AddWithValue("Stundensatz", element.Stundensatz == null ? (object)DBNull.Value : element.Stundensatz);
				sqlCommand.Parameters.AddWithValue("Warentyp", element.Warentyp == null ? (object)DBNull.Value : element.Warentyp);
				sqlCommand.Parameters.AddWithValue("Gewicht", element.Gewicht == null ? (object)DBNull.Value : element.Gewicht);
				sqlCommand.Parameters.AddWithValue("Kupferzahl", element.Kupferzahl == null ? (object)DBNull.Value : element.Kupferzahl);
				sqlCommand.Parameters.AddWithValue("Zuschlag_VK", element.Zuschlag_VK == null ? (object)DBNull.Value : element.Zuschlag_VK);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer", element.Umsatzsteuer == null ? (object)DBNull.Value : element.Umsatzsteuer);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", element.Preiseinheit == null ? (object)DBNull.Value : element.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("Werkzeug", element.Werkzeug == null ? (object)DBNull.Value : element.Werkzeug);
				sqlCommand.Parameters.AddWithValue("Langtext", element.Langtext == null ? (object)DBNull.Value : element.Langtext);
				sqlCommand.Parameters.AddWithValue("Verpackung", element.Verpackung == null ? (object)DBNull.Value : element.Verpackung);
				sqlCommand.Parameters.AddWithValue("Lieferzeit", element.Lieferzeit == null ? (object)DBNull.Value : element.Lieferzeit);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", element.Artikelfamilie_Kunde == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", element.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail1);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", element.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail2);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int EditQuality(Entities.Tables.PRS.ArtikelEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Artikel] SET " +
					"[Kanban]=@Kanban," +
					"[UL zertifiziert]=@UL_zertifiziert," +
					"[UL Etikett]=@UL_Etikett," +
					"Webshop=@Webshop," +
					"ESD_Schutz=@ESD_Schutz," +
					"ESD_Schutz_Text=@ESD_Schutz_Text," +
					"Hubmastleitungen=@Hubmastleitungen," +
					"[ROHS EEE Confirmity]=@ROHS_EEE_Confirmity," +
					"[Minerals Confirmity]=@Minerals_Confirmity," +
					"[REACH SVHC Confirmity]=@REACH_SVHC_Confirmity," +
					"Dienstelistung=@Dienstelistung," +
					"MHD=@MHD," +
					"Zeitraum_MHD=@Zeitraum_MHD," +
					"COF_Pflichtig=@COF_Pflichtig," +
					"EMPB=@EMPB," +
					"EMPB_Freigegeben=@EMPB_Freigegeben," +
					"Freigabestatus=@Freigabestatus," +
					"[Prüfstatus TN Ware]=@Prüfstatus_TN_Ware," +
					"[UBG]=@UBG," +
					"[CocVersion]=@CocVersion," +
					"[DeliveryNoteCustomerComments]=@DeliveryNoteCustomerComments," +
					"[Freigabestatus TN intern]=@Freigabestatus_TN_intern " +
					"where [Artikel-Nr]=@Artikel_Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", element.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("UL_zertifiziert", element.ULzertifiziert == null ? (object)DBNull.Value : element.ULzertifiziert);
				sqlCommand.Parameters.AddWithValue("UL_Etikett", element.ULEtikett == null ? (object)DBNull.Value : element.ULEtikett);
				sqlCommand.Parameters.AddWithValue("Webshop", element.Webshop == null ? (object)DBNull.Value : element.Webshop);
				sqlCommand.Parameters.AddWithValue("ESD_Schutz", element.ESD_Schutz == null ? (object)DBNull.Value : element.ESD_Schutz);
				sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text", element.ESD_Schutz_Text == null ? (object)DBNull.Value : element.ESD_Schutz_Text);
				sqlCommand.Parameters.AddWithValue("Hubmastleitungen", element.Hubmastleitungen == null ? (object)DBNull.Value : element.Hubmastleitungen);
				sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity", element.ROHSEEEConfirmity == null ? (object)DBNull.Value : element.ROHSEEEConfirmity);
				sqlCommand.Parameters.AddWithValue("Minerals_Confirmity", element.MineralsConfirmity == null ? (object)DBNull.Value : element.MineralsConfirmity);
				sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity", element.REACHSVHCConfirmity == null ? (object)DBNull.Value : element.REACHSVHCConfirmity);
				sqlCommand.Parameters.AddWithValue("Dienstelistung", element.Dienstelistung == null ? (object)DBNull.Value : element.Dienstelistung);
				sqlCommand.Parameters.AddWithValue("MHD", element.MHD == null ? (object)DBNull.Value : element.MHD);
				sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", element.Zeitraum_MHD == null ? (object)DBNull.Value : element.Zeitraum_MHD);
				sqlCommand.Parameters.AddWithValue("COF_Pflichtig", element.COF_Pflichtig == null ? (object)DBNull.Value : element.COF_Pflichtig);
				sqlCommand.Parameters.AddWithValue("EMPB", element.EMPB == null ? (object)DBNull.Value : element.EMPB);
				sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben", element.EMPB_Freigegeben == null ? (object)DBNull.Value : element.EMPB_Freigegeben);
				sqlCommand.Parameters.AddWithValue("Freigabestatus", element.Freigabestatus == null ? (object)DBNull.Value : element.Freigabestatus);
				sqlCommand.Parameters.AddWithValue("Prüfstatus_TN_Ware", element.PrufstatusTNWare == null ? (object)DBNull.Value : element.PrufstatusTNWare);
				sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", element.FreigabestatusTNIntern == null ? (object)DBNull.Value : element.FreigabestatusTNIntern);
				sqlCommand.Parameters.AddWithValue("Kanban", element.Kanban == null ? (object)DBNull.Value : element.Kanban);
				sqlCommand.Parameters.AddWithValue("UBG", element.UBG);
				sqlCommand.Parameters.AddWithValue("CocVersion", element.CocVersion == null ? (object)DBNull.Value : element.CocVersion);
				sqlCommand.Parameters.AddWithValue("DeliveryNoteCustomerComments", element.DeliveryNoteCustomerComments == null ? (object)DBNull.Value : element.DeliveryNoteCustomerComments);
				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int EditProduction(Entities.Tables.PRS.ArtikelEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Artikel] SET " +
					"[Artikelkurztext]=@Artikelkurztext," +
					"[Halle]=@Halle, " +
					"[ProductionLotSize]=@ProductionLotSize " +
					"where [Artikel-Nr]=@Artikel_Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", element.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Artikelkurztext", element.Artikelkurztext == null ? (object)DBNull.Value : element.Artikelkurztext);
				sqlCommand.Parameters.AddWithValue("Halle", element.Halle == null ? (object)DBNull.Value : element.Halle);
				sqlCommand.Parameters.AddWithValue("ProductionLotSize", element.ProductionLotSize == null ? (object)DBNull.Value : element.ProductionLotSize);
				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int EditBlanket(Entities.Tables.PRS.ArtikelEntity element, bool isFirst = true)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"UPDATE [Artikel] SET " +
					(isFirst
						? $"[Rahmen-Nr]=@RahmenNr, [Rahmenmenge]=@Rahmenmenge, [Rahmenauslauf]=@Rahmenauslauf "
						: $"[Rahmen-Nr2]=@RahmenNr, [Rahmenmenge2]=@Rahmenmenge, [Rahmenauslauf2]=@Rahmenauslauf "
					) +
					$"where [Artikel-Nr]=@Artikel_Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", element.ArtikelNr);
				if(isFirst)
				{
					sqlCommand.Parameters.AddWithValue("RahmenNr", element.RahmenNr == null ? (object)DBNull.Value : element.RahmenNr);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge", element.Rahmenmenge == null ? (object)DBNull.Value : element.Rahmenmenge);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf", element.Rahmenauslauf == null ? (object)DBNull.Value : element.Rahmenauslauf);
				}
				else
				{
					sqlCommand.Parameters.AddWithValue("RahmenNr", element.RahmenNr2 == null ? (object)DBNull.Value : element.RahmenNr2);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge", element.Rahmenmenge2 == null ? (object)DBNull.Value : element.Rahmenmenge2);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf", element.Rahmenauslauf2 == null ? (object)DBNull.Value : element.Rahmenauslauf2);
				}
				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int EditBlanketChecks(Entities.Tables.PRS.ArtikelEntity element, bool isFirst = true)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"UPDATE [Artikel] SET " +
					(isFirst
						? $"[Rahmen]=@Rahmen "
						: $"[Rahmen2]=@Rahmen "
					) +
					$"where [Artikel-Nr]=@Artikel_Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", element.ArtikelNr);
				if(isFirst)
				{
					sqlCommand.Parameters.AddWithValue("Rahmen", element.Rahmen == null ? (object)DBNull.Value : element.Rahmen);
				}
				else
				{
					sqlCommand.Parameters.AddWithValue("Rahmen", element.Rahmen2 == null ? (object)DBNull.Value : element.Rahmen2);
				}
				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static decimal GetCuGewicht(int articleNr)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"WITH CTE_ArticleData AS (
						-- Combine Q1 and Q2 in a single CTE
						SELECT 
							A.[Artikel-Nr], 
							S.Anzahl, 
							A1.Einheit, 
							A1.Kupferzahl
						FROM Artikel A
						INNER JOIN Stücklisten S ON A.[Artikel-Nr] = S.[Artikel-Nr]
						INNER JOIN Artikel A1 ON S.[Artikel-Nr des Bauteils] = A1.[Artikel-Nr]
						WHERE A.[Artikel-Nr] = @articleNr 
						AND A1.Kupferzahl > 0

						UNION ALL

						SELECT 
							A.[Artikel-Nr], 
							S.Anzahl, 
							A2.Einheit, 
							A2.Kupferzahl
						FROM Artikel A
						INNER JOIN Bestellnummern B ON A.[Artikel-Nr] = B.[Artikel-Nr]
						INNER JOIN Artikel A1 ON B.[Bestell-Nr] = A1.Artikelnummer
						INNER JOIN Stücklisten S ON A1.[Artikel-Nr] = S.[Artikel-Nr]
						INNER JOIN Artikel A2 ON S.[Artikel-Nr des Bauteils] = A2.[Artikel-Nr]
						WHERE A.[Artikel-Nr] = @articleNr
						AND (A.Artikelnummer LIKE '226%' OR A.Artikelnummer LIKE '227%' OR A.Artikelnummer LIKE '228%')
						AND A2.Kupferzahl > 0
					)

					-- Final Query
					SELECT 
						SUM(IIF(Einheit = 'm', Anzahl / 1000 * Kupferzahl, Anzahl * Kupferzahl)) AS Gewicht
					FROM CTE_ArticleData
					GROUP BY [Artikel-Nr]
					HAVING [Artikel-Nr] = @articleNr;";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);

				return decimal.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var _val) == true ? _val : 0m;
			}
		}
		public static int UpdateCustomerIndexWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [Artikel] SET "

					+ "[Index_Kunde]=@Index_Kunde" + i + ","
					+ "[Index_Kunde_Datum]=@Index_Kunde_Datum" + i + ","
					+ "[CustomerIndex]=@CustomerIndex" + i + " WHERE [Artikel-Nr]=@Artikel_Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("CustomerIndex" + i, item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);

				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}
		#endregion >>>Data

		#region Sales
		public static decimal GetMaterialCost(string articleNumber)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT TOP 1 [Summe Material mit] FROM [View_ERP_MaterialCost_02] WHERE Artikelnummer=@ArticleNummer;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ArticleNummer", articleNumber ?? "");

				return decimal.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var _val) == true ? _val : 0m;
			}
		}
		public static decimal GetMaterialCostDBwoCU(string articleNumber)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT TOP 1 [DB I mit] FROM [View_PSZ_steinbacher Marge berechnung alle Artikel ergebniss] WHERE Artikelnummer=@ArticleNummer;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ArticleNummer", articleNumber ?? "");

				return decimal.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var _val) == true ? _val : 0m;
			}
		}
		public static decimal GetMaterialCostDBwoCUPercent(string articleNumber)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT TOP 1 [Marge mit CU] FROM [View_PSZ_steinbacher Marge berechnung alle Artikel ergebniss] WHERE Artikelnummer=@ArticleNummer;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ArticleNummer", articleNumber ?? "");

				return decimal.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var _val) == true ? _val : 0m;
			}
		}
		#endregion

		#region BOM
		public static int EditCPRequirement(int articleId, bool cpRequired)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Artikel] SET [CP_required]=@cpRequired WHERE [Artikel-Nr]=@ArtikelNr";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ArtikelNr", articleId);
				sqlCommand.Parameters.AddWithValue("cpRequired", cpRequired);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int EditCPRequirement(int articleId, bool cpRequired, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Artikel] SET [CP_required]=@cpRequired WHERE [Artikel-Nr]=@ArtikelNr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{

				sqlCommand.Parameters.AddWithValue("ArtikelNr", articleId);
				sqlCommand.Parameters.AddWithValue("cpRequired", cpRequired);

				return int.TryParse(sqlCommand.ExecuteNonQuery().ToString(), out var i) ? i : 0;
			}
		}
		public static int updateGrosse(int articleNr, decimal grosse)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Artikel] SET [Größe]=@grosse where [Artikel-Nr]=@Artikel_Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", articleNr);
				sqlCommand.Parameters.AddWithValue("grosse", grosse);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int updateGrosse(int articleNr, decimal grosse, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Artikel] SET [Größe]=@grosse where [Artikel-Nr]=@Artikel_Nr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", articleNr);
				sqlCommand.Parameters.AddWithValue("grosse", grosse);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		#endregion BOM

		public static List<Entities.Tables.PRS.ArtikelEntity> GetByProjectClass(int classId)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel] WHERE [ID_Klassifizierung]=@classId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("classId", classId);

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetActiveStucklisted(string searchTerms, string currentNumber = "", int maxResults = 20)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $" {(string.IsNullOrWhiteSpace(currentNumber) ? "" : $"SELECT * FROM [Artikel] WHERE Artikel.Stückliste=1 AND Artikel.aktiv=1 AND Artikelnummer = '{currentNumber.SqlEscape().Trim()}' UNION ")}"
					 + $"SELECT TOP {maxResults} * FROM [Artikel] WHERE Artikel.Stückliste=1 AND Artikel.aktiv=1 AND Artikelnummer LIKE '{searchTerms.SqlEscape().Trim()}%' ORDER BY Artikel.ArtikelNummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetLikeNummerForAutocomplete(string nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT TOP 10 * FROM  [Artikel] WHERE [Artikelnummer] LIKE '{nummer.SqlEscape()}%' AND [Stückliste]=1 AND [aktiv]=1 AND [Freigabestatus]<>'O' ORDER by [Artikelnummer] ASC";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}

		#region Custumer service
		public static List<Entities.Tables.PRS.ArtikelEntity> GetForFAStcklistSelect(string text)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT TOP 10 * FROM [Artikel] 
                                 WHERE [Artikelnummer] Not Like 'Endkontroll%'
                                 AND [Freigabestatus]<>'O'
                                 AND [Stückliste]=1
                                 AND [aktiv]=1";
				if(!string.IsNullOrEmpty(text) || !string.IsNullOrWhiteSpace(text))
				{
					query += $" AND [Artikelnummer] LIKE '{text.SqlEscape()}%'";
				}
				query += " ORDER BY [Artikelnummer]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
			}

			return result;
		}
		public static int EditCTS(Entities.Tables.PRS.ArtikelEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Artikel] SET " +

					"[Langtext]=@Langtext," +
					"[Verpackung]=@Verpackung ," +
					"[BemerkungCRP]=@BemerkungCRP, " +
					"[BemerkungCRPPlanung]=@BemerkungCRPPlanung" +
					" WHERE [Artikel-Nr]=@ArtikelNr";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ArtikelNr", element.ArtikelNr);

				sqlCommand.Parameters.AddWithValue("Langtext", element.Langtext == null ? (object)DBNull.Value : element.Langtext);
				sqlCommand.Parameters.AddWithValue("Verpackung", element.Verpackung == null ? (object)DBNull.Value : element.Verpackung);
				sqlCommand.Parameters.AddWithValue("BemerkungCRP", element.BemerkungCRP == null ? (object)DBNull.Value : element.BemerkungCRP);
				sqlCommand.Parameters.AddWithValue("BemerkungCRPPlanung", element.BemerkungCRPPlanung == null ? (object)DBNull.Value : element.BemerkungCRPPlanung);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		#endregion

		#region QueryWithTransaction
		public static int EditWithTransaction(Entities.Tables.PRS.ArtikelEntity element, SqlConnection connection, SqlTransaction transaction)
		{
			int response = -1;

			//using (var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			//{
			//    sqlConnection.Open();

			string query = "UPDATE [Artikel] SET [Abladestelle]=@Abladestelle,[aktiv]=@aktiv,[aktualisiert]=@aktualisiert," +
				"[Anfangsbestand]=@Anfangsbestand,[Artikel aus eigener Produktion]=@ArtikelAusEigenerProduktion," +
				"[Artikel für weitere Bestellungen sperren]=@ArtikelFürWeitereBestellungenSperren,[Artikelfamilie_Kunde]=@Artikelfamilie_Kunde," +
				"[Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1,[Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2," +
				"[Artikelkurztext]=@Artikelkurztext,[Artikelnummer]=@Artikelnummer,[Barverkauf]=@Barverkauf,[Bezeichnung 1]=@Bezeichnung1," +
				"[Bezeichnung 2]=@Bezeichnung2,[Bezeichnung 3]=@Bezeichnung3,[BezeichnungAL]=@BezeichnungAL,[COF_Pflichtig]=@COF_Pflichtig," +
				"[Crossreferenz]=@Crossreferenz,[Cu-Gewicht]=@CuGewicht,[Datum Anfangsbestand]=@DatumAnfangsbestand,[DEL]=@DEL," +
				"[DEL fixiert]=@DELFixiert,[Dokumente]=@Dokumente,[EAN]=@EAN,[Einheit]=@Einheit,[EMPB]=@EMPB,[EMPB_Freigegeben]=@EMPB_Freigegeben," +
				"[Ersatzartikel]=@Ersatzartikel,[ESD_Schutz]=@ESD_Schutz,[Exportgewicht]=@Exportgewicht," +
				"[fakturieren Stückliste]=@fakturierenStückliste,[Farbe]=@Farbe,[fibu_rahmen]=@fibu_rahmen,[Freigabestatus]=@Freigabestatus," +
				"[Freigabestatus TN intern]=@FreigabestatusTNIntern,[Gebinde]=@Gebinde,[Gewicht]=@Gewicht,[Größe]=@Größe," +
				"[Grund für Sperre]=@GrundFürSperre,[gültig bis]=@gültigBis,[Halle]=@Halle,[Hubmastleitungen]=@Hubmastleitungen," +
				"[ID_Klassifizierung]=@ID_Klassifizierung,[Index_Kunde]=@Index_Kunde,[Index_Kunde_Datum]=@Index_Kunde_Datum,[Info_WE]=@Info_WE," +
				"[Kanban]=@Kanban,[Kategorie]=@Kategorie,[Klassifizierung]=@Klassifizierung,[Kriterium1]=@Kriterium1,[Kriterium2]=@Kriterium2," +
				"[Kriterium3]=@Kriterium3,[Kriterium4]=@Kriterium4,[Kupferbasis]=@Kupferbasis,[Kupferzahl]=@Kupferzahl,[Lagerartikel]=@Lagerartikel," +
				"[Lagerhaltungskosten]=@Lagerhaltungskosten,[Langtext]=@Langtext,[Langtext_drucken_AB]=@Langtext_drucken_AB," +
				"[Langtext_drucken_BW]=@Langtext_drucken_BW,[Lieferzeit]=@Lieferzeit,[Losgroesse]=@Losgroesse,[Materialkosten_Alt]=@Materialkosten_Alt," +
				"[MHD]=@MHD,[Minerals Confirmity]=@MineralsConfirmity,[Praeferenz_Aktuelles_jahr]=@Praeferenz_Aktuelles_jahr," +
				"[Praeferenz_Folgejahr]=@Praeferenz_Folgejahr,[Preiseinheit]=@Preiseinheit,[pro Zeiteinheit]=@proZeiteinheit," +
				"[Produktionszeit]=@Produktionszeit,[Provisionsartikel]=@Provisionsartikel,[Prüfstatus TN Ware]=@PrüfstatusTNWare," +
				"[Rabattierfähig]=@Rabattierfähig,[Rahmen]=@Rahmen,[Rahmen2]=@Rahmen2,[Rahmenauslauf]=@Rahmenauslauf,[Rahmenauslauf2]=@Rahmenauslauf2," +
				"[Rahmenmenge]=@Rahmenmenge,[Rahmenmenge2]=@Rahmenmenge2,[Rahmen-Nr]=@RahmenNr,[Rahmen-Nr2]=@RahmenNr2," +
				"[REACH SVHC Confirmity]=@REACHSVHCConfirmity,[ROHS EEE Confirmity]=@ROHSEEEConfirmity,[Seriennummer]=@Seriennummer," +
				"[Seriennummernverwaltung]=@Seriennummernverwaltung,[Sonderrabatt]=@Sonderrabatt,[Standard_Lagerort_id]=@Standard_Lagerort_id," +
				"[Stückliste]=@Stückliste,[Stundensatz]=@Stundensatz,[Sysmonummer]=@Sysmonummer,[UL Etikett]=@ULEtikett," +
				"[UL zertifiziert]=@ULzertifiziert,[Umsatzsteuer]=@Umsatzsteuer,[Ursprungsland]=@Ursprungsland," +
				"[Verpackung]=@Verpackung,[Verpackungsart]=@Verpackungsart,[Verpackungsmenge]=@Verpackungsmenge,[VK-Festpreis]=@VKFestpreis" +
				",[Volumen]=@Volumen,[Warengruppe]=@Warengruppe,[Warentyp]=@Warentyp,[Webshop]=@Webshop,[Werkzeug]=@Werkzeug," +
				"[Wert_Anfangsbestand]=@Wert_Anfangsbestand,[Zeichnungsnummer]=@Zeichnungsnummer,[Zeitraum_MHD]=@Zeitraum_MHD," +
				"[Zolltarif_nr]=@Zolltarif_nr,[VDA_1]=@VDA_1,[VDA_2]=@VDA_2,[Zuschlag_VK]=@Zuschlag_VK,[artikelklassifizierung]=@artikelklassifizierung,[UBG]=@UBG WHERE [Artikel-Nr]=@ArtikelNr";

			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ArtikelNr", element.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Abladestelle", element.Abladestelle == null ? (object)DBNull.Value : element.Abladestelle);
			sqlCommand.Parameters.AddWithValue("aktiv", element.aktiv == null ? (object)DBNull.Value : element.aktiv);
			sqlCommand.Parameters.AddWithValue("aktualisiert", element.aktualisiert == null ? (object)DBNull.Value : element.aktualisiert);
			sqlCommand.Parameters.AddWithValue("Anfangsbestand", element.Anfangsbestand == null ? (object)DBNull.Value : element.Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("ArtikelAusEigenerProduktion", element.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : element.ArtikelAusEigenerProduktion);
			sqlCommand.Parameters.AddWithValue("ArtikelFürWeitereBestellungenSperren", element.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : element.ArtikelFürWeitereBestellungenSperren);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", element.Artikelfamilie_Kunde == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", element.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail1);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", element.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail2);
			sqlCommand.Parameters.AddWithValue("Artikelkurztext", element.Artikelkurztext == null ? (object)DBNull.Value : element.Artikelkurztext);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", element.ArtikelNummer == null ? (object)DBNull.Value : element.ArtikelNummer);
			sqlCommand.Parameters.AddWithValue("Barverkauf", element.Barverkauf == null ? (object)DBNull.Value : element.Barverkauf);
			sqlCommand.Parameters.AddWithValue("Bezeichnung1", element.Bezeichnung1 == null ? (object)DBNull.Value : element.Bezeichnung1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung2", element.Bezeichnung2 == null ? (object)DBNull.Value : element.Bezeichnung2);
			sqlCommand.Parameters.AddWithValue("Bezeichnung3", element.Bezeichnung3 == null ? (object)DBNull.Value : element.Bezeichnung3);
			sqlCommand.Parameters.AddWithValue("BezeichnungAL", element.BezeichnungAL == null ? (object)DBNull.Value : element.BezeichnungAL);
			sqlCommand.Parameters.AddWithValue("COF_Pflichtig", element.COF_Pflichtig == null ? (object)DBNull.Value : element.COF_Pflichtig);
			sqlCommand.Parameters.AddWithValue("Crossreferenz", element.Crossreferenz == null ? (object)DBNull.Value : element.Crossreferenz);
			sqlCommand.Parameters.AddWithValue("CuGewicht", element.CuGewicht == null ? (object)DBNull.Value : element.CuGewicht);
			sqlCommand.Parameters.AddWithValue("DatumAnfangsbestand", element.DatumAnfangsbestand == null ? (object)DBNull.Value : element.DatumAnfangsbestand);
			sqlCommand.Parameters.AddWithValue("DEL", element.DEL == null ? (object)DBNull.Value : element.DEL);
			sqlCommand.Parameters.AddWithValue("DELFixiert", element.DELFixiert == null ? (object)DBNull.Value : element.DELFixiert);
			sqlCommand.Parameters.AddWithValue("Dokumente", element.Dokumente == null ? (object)DBNull.Value : element.Dokumente);
			sqlCommand.Parameters.AddWithValue("EAN", element.EAN == null ? (object)DBNull.Value : element.EAN);
			sqlCommand.Parameters.AddWithValue("Einheit", element.Einheit == null ? (object)DBNull.Value : element.Einheit);
			sqlCommand.Parameters.AddWithValue("EMPB", element.EMPB == null ? (object)DBNull.Value : element.EMPB);
			sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben", element.EMPB_Freigegeben == null ? (object)DBNull.Value : element.EMPB_Freigegeben);
			sqlCommand.Parameters.AddWithValue("Ersatzartikel", element.Ersatzartikel == null ? (object)DBNull.Value : element.Ersatzartikel);
			sqlCommand.Parameters.AddWithValue("ESD_Schutz", element.ESD_Schutz == null ? (object)DBNull.Value : element.ESD_Schutz);
			sqlCommand.Parameters.AddWithValue("Exportgewicht", element.Exportgewicht == null ? (object)DBNull.Value : element.Exportgewicht);
			sqlCommand.Parameters.AddWithValue("fakturierenStückliste", element.fakturierenStückliste == null ? (object)DBNull.Value : element.fakturierenStückliste);
			sqlCommand.Parameters.AddWithValue("Farbe", element.Farbe == null ? (object)DBNull.Value : element.Farbe);
			sqlCommand.Parameters.AddWithValue("fibu_rahmen", element.fibu_rahmen == null ? (object)DBNull.Value : element.fibu_rahmen);
			sqlCommand.Parameters.AddWithValue("Freigabestatus", element.Freigabestatus == null ? (object)DBNull.Value : element.Freigabestatus);
			sqlCommand.Parameters.AddWithValue("FreigabestatusTNIntern", element.FreigabestatusTNIntern == null ? (object)DBNull.Value : element.FreigabestatusTNIntern);
			sqlCommand.Parameters.AddWithValue("Gebinde", element.Gebinde == null ? (object)DBNull.Value : element.Gebinde);
			sqlCommand.Parameters.AddWithValue("Gewicht", element.Gewicht == null ? (object)DBNull.Value : element.Gewicht);
			sqlCommand.Parameters.AddWithValue("Größe", element.Größe == null ? (object)DBNull.Value : element.Größe);
			sqlCommand.Parameters.AddWithValue("GrundFürSperre", element.GrundFürSperre == null ? (object)DBNull.Value : element.GrundFürSperre);
			sqlCommand.Parameters.AddWithValue("gültigBis", element.gültigBis == null ? (object)DBNull.Value : element.gültigBis);
			sqlCommand.Parameters.AddWithValue("Halle", element.Halle == null ? (object)DBNull.Value : element.Halle);
			sqlCommand.Parameters.AddWithValue("Hubmastleitungen", element.Hubmastleitungen == null ? (object)DBNull.Value : element.Hubmastleitungen);
			sqlCommand.Parameters.AddWithValue("ID_Klassifizierung", element.ID_Klassifizierung == null ? (object)DBNull.Value : element.ID_Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", element.Index_Kunde == null ? (object)DBNull.Value : element.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", element.Index_Kunde_Datum == null ? (object)DBNull.Value : element.Index_Kunde_Datum);
			sqlCommand.Parameters.AddWithValue("Info_WE", element.Info_WE == null ? (object)DBNull.Value : element.Info_WE);
			sqlCommand.Parameters.AddWithValue("Kanban", element.Kanban == null ? (object)DBNull.Value : element.Kanban);
			sqlCommand.Parameters.AddWithValue("Kategorie", element.Kategorie == null ? (object)DBNull.Value : element.Kategorie);
			sqlCommand.Parameters.AddWithValue("Klassifizierung", element.Klassifizierung == null ? (object)DBNull.Value : element.Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Kriterium1", element.Kriterium1 == null ? (object)DBNull.Value : element.Kriterium1);
			sqlCommand.Parameters.AddWithValue("Kriterium2", element.Kriterium2 == null ? (object)DBNull.Value : element.Kriterium2);
			sqlCommand.Parameters.AddWithValue("Kriterium3", element.Kriterium3 == null ? (object)DBNull.Value : element.Kriterium3);
			sqlCommand.Parameters.AddWithValue("Kriterium4", element.Kriterium4 == null ? (object)DBNull.Value : element.Kriterium4);
			sqlCommand.Parameters.AddWithValue("Kupferbasis", element.Kupferbasis == null ? (object)DBNull.Value : element.Kupferbasis);
			sqlCommand.Parameters.AddWithValue("Kupferzahl", element.Kupferzahl == null ? (object)DBNull.Value : element.Kupferzahl);
			sqlCommand.Parameters.AddWithValue("Lagerartikel", element.Lagerartikel == null ? (object)DBNull.Value : element.Lagerartikel);
			sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", element.Lagerhaltungskosten == null ? (object)DBNull.Value : element.Lagerhaltungskosten);
			sqlCommand.Parameters.AddWithValue("Langtext", element.Langtext == null ? (object)DBNull.Value : element.Langtext);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", element.Langtext_drucken_AB == null ? (object)DBNull.Value : element.Langtext_drucken_AB);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", element.Langtext_drucken_BW == null ? (object)DBNull.Value : element.Langtext_drucken_BW);
			sqlCommand.Parameters.AddWithValue("Lieferzeit", element.Lieferzeit == null ? (object)DBNull.Value : element.Lieferzeit);
			sqlCommand.Parameters.AddWithValue("Losgroesse", element.Losgroesse == null ? (object)DBNull.Value : element.Losgroesse);
			sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", element.Materialkosten_Alt == null ? (object)DBNull.Value : element.Materialkosten_Alt);
			sqlCommand.Parameters.AddWithValue("MHD", element.MHD == null ? (object)DBNull.Value : element.MHD);
			sqlCommand.Parameters.AddWithValue("MineralsConfirmity", element.MineralsConfirmity == null ? (object)DBNull.Value : element.MineralsConfirmity);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr", element.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : element.Praeferenz_Aktuelles_jahr);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr", element.Praeferenz_Folgejahr == null ? (object)DBNull.Value : element.Praeferenz_Folgejahr);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", element.Preiseinheit == null ? (object)DBNull.Value : element.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("proZeiteinheit", element.proZeiteinheit == null ? (object)DBNull.Value : element.proZeiteinheit);
			sqlCommand.Parameters.AddWithValue("Produktionszeit", element.Produktionszeit == null ? (object)DBNull.Value : element.Produktionszeit);
			sqlCommand.Parameters.AddWithValue("Provisionsartikel", element.Provisionsartikel == null ? (object)DBNull.Value : element.Provisionsartikel);
			sqlCommand.Parameters.AddWithValue("PrüfstatusTNWare", element.PrufstatusTNWare == null ? (object)DBNull.Value : element.PrufstatusTNWare);
			sqlCommand.Parameters.AddWithValue("Rabattierfähig", element.Rabattierfähig == null ? (object)DBNull.Value : element.Rabattierfähig);
			sqlCommand.Parameters.AddWithValue("Rahmen", element.Rahmen == null ? (object)DBNull.Value : element.Rahmen);
			sqlCommand.Parameters.AddWithValue("Rahmen2", element.Rahmen2 == null ? (object)DBNull.Value : element.Rahmen2);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf", element.Rahmenauslauf == null ? (object)DBNull.Value : element.Rahmenauslauf);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf2", element.Rahmenauslauf2 == null ? (object)DBNull.Value : element.Rahmenauslauf2);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge", element.Rahmenmenge == null ? (object)DBNull.Value : element.Rahmenmenge);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge2", element.Rahmenmenge2 == null ? (object)DBNull.Value : element.Rahmenmenge2);
			sqlCommand.Parameters.AddWithValue("RahmenNr", element.RahmenNr == null ? (object)DBNull.Value : element.RahmenNr);
			sqlCommand.Parameters.AddWithValue("RahmenNr2", element.RahmenNr2 == null ? (object)DBNull.Value : element.RahmenNr2);
			sqlCommand.Parameters.AddWithValue("REACHSVHCConfirmity", element.REACHSVHCConfirmity == null ? (object)DBNull.Value : element.REACHSVHCConfirmity);
			sqlCommand.Parameters.AddWithValue("ROHSEEEConfirmity", element.ROHSEEEConfirmity == null ? (object)DBNull.Value : element.ROHSEEEConfirmity);
			sqlCommand.Parameters.AddWithValue("Seriennummer", element.Seriennummer == null ? (object)DBNull.Value : element.Seriennummer);
			sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", element.Seriennummernverwaltung == null ? (object)DBNull.Value : element.Seriennummernverwaltung);
			sqlCommand.Parameters.AddWithValue("Sonderrabatt", element.Sonderrabatt == null ? (object)DBNull.Value : element.Sonderrabatt);
			sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", element.Standard_Lagerort_id == null ? (object)DBNull.Value : element.Standard_Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Stückliste", element.Stuckliste == null ? (object)DBNull.Value : element.Stuckliste);
			sqlCommand.Parameters.AddWithValue("Stundensatz", element.Stundensatz == null ? (object)DBNull.Value : element.Stundensatz);
			sqlCommand.Parameters.AddWithValue("Sysmonummer", element.Sysmonummer == null ? (object)DBNull.Value : element.Sysmonummer);
			sqlCommand.Parameters.AddWithValue("ULEtikett", element.ULEtikett == null ? (object)DBNull.Value : element.ULEtikett);
			sqlCommand.Parameters.AddWithValue("ULzertifiziert", element.ULzertifiziert == null ? (object)DBNull.Value : element.ULzertifiziert);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", element.Umsatzsteuer == null ? (object)DBNull.Value : element.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("Ursprungsland", element.Ursprungsland == null ? (object)DBNull.Value : element.Ursprungsland);
			sqlCommand.Parameters.AddWithValue("Verpackung", element.Verpackung == null ? (object)DBNull.Value : element.Verpackung);
			sqlCommand.Parameters.AddWithValue("Verpackungsart", element.Verpackungsart == null ? (object)DBNull.Value : element.Verpackungsart);
			sqlCommand.Parameters.AddWithValue("Verpackungsmenge", element.Verpackungsmenge == null ? (object)DBNull.Value : element.Verpackungsmenge);
			sqlCommand.Parameters.AddWithValue("VKFestpreis", element.VKFestpreis == null ? (object)DBNull.Value : element.VKFestpreis);
			sqlCommand.Parameters.AddWithValue("Volumen", element.Volumen == null ? (object)DBNull.Value : element.Volumen);
			sqlCommand.Parameters.AddWithValue("Warengruppe", element.Warengruppe == null ? (object)DBNull.Value : element.Warengruppe);
			sqlCommand.Parameters.AddWithValue("Warentyp", element.Warentyp == null ? (object)DBNull.Value : element.Warentyp);
			sqlCommand.Parameters.AddWithValue("Webshop", element.Webshop == null ? (object)DBNull.Value : element.Webshop);
			sqlCommand.Parameters.AddWithValue("Werkzeug", element.Werkzeug == null ? (object)DBNull.Value : element.Werkzeug);
			sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", element.Wert_Anfangsbestand == null ? (object)DBNull.Value : element.Wert_Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", element.Zeichnungsnummer == null ? (object)DBNull.Value : element.Zeichnungsnummer);
			sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", element.Zeitraum_MHD == null ? (object)DBNull.Value : element.Zeitraum_MHD);
			sqlCommand.Parameters.AddWithValue("Zolltarif_nr", element.Zolltarif_nr == null ? (object)DBNull.Value : element.Zolltarif_nr);
			sqlCommand.Parameters.AddWithValue("VDA_1", element.VDA_1 == null ? (object)DBNull.Value : element.VDA_1);
			sqlCommand.Parameters.AddWithValue("VDA_2", element.VDA_2 == null ? (object)DBNull.Value : element.VDA_2);
			sqlCommand.Parameters.AddWithValue("Zuschlag_VK", element.Zuschlag_VK == null ? (object)DBNull.Value : element.Zuschlag_VK);
			sqlCommand.Parameters.AddWithValue("artikelklassifizierung", element.artikelklassifizierung == null ? (object)DBNull.Value : element.artikelklassifizierung);
			sqlCommand.Parameters.AddWithValue("UBG", element.UBG);

			response = sqlCommand.ExecuteNonQuery();
			//}

			return response;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCustomerItemIndex(int customerNumber, string kreis, string cutomerItemNumber, string customerItemIndex, bool topOne = false)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			customerItemIndex = customerItemIndex ?? "";
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT {(topOne ? "TOP 1 " : "")}* FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber AND [CustomerIndex]=@customerItemIndex";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("cutomerItemNumber", cutomerItemNumber ?? "");
				sqlCommand.Parameters.AddWithValue("customerItemIndex", customerItemIndex ?? "");

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCustomerItemNumber(int customerNumber, string kreis, string cutomerItemNumber, bool? isArticleNumberSpecial = null, bool topOne = false)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT {(topOne ? "TOP 1 " : "")}* FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber {(isArticleNumberSpecial.HasValue == true ? $" AND ISNULL([IsArticleNumberSpecial],0)={(isArticleNumberSpecial.Value == true ? "1" : "0")}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("cutomerItemNumber", cutomerItemNumber ?? "");

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCustomerItemNumber(int customerNumber, string kreis, string cutomerItemNumber, SqlConnection connection, SqlTransaction transaction)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			string query = $"SELECT * FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
			sqlCommand.Parameters.AddWithValue("cutomerItemNumber", cutomerItemNumber ?? "");

			using(var reader = sqlCommand.ExecuteReader())
			{
				result = toList(reader);
			}
			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCustomerItemNumber(List<int> customerNumbers, string cutomerItemNumber, SqlConnection connection, SqlTransaction transaction, bool includeDeisgnation1 = false, bool trimLeadingZeros = false)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			string query = $"SELECT * FROM [Artikel] WHERE [CustomerNumber] IN ({string.Join(',', customerNumbers)}) AND ISNULL([EdiDefault],0)=0 AND ";

			if(includeDeisgnation1)
			{
				if(trimLeadingZeros)
				{
					//true - true
					query += @$"(STUFF(CONVERT(nvarchar,[CustomerItemNumber]), 1, PATINDEX('%[^0]%', CONVERT(nvarchar,[CustomerItemNumber])) - 1, '') ='{cutomerItemNumber}' 
                           OR STUFF([Bezeichnung 1], 1, PATINDEX('%[^0]%', [Bezeichnung 1]) - 1, '') ='{cutomerItemNumber}')";
				}
				else
				{
					//true - false
					query += @$"(CONVERT(nvarchar,[CustomerItemNumber]) ='{cutomerItemNumber}'
                           OR [Bezeichnung 1] ='{cutomerItemNumber}')";
				}

			}
			else
			{
				if(trimLeadingZeros)
				{
					//false - true
					query += @$"(STUFF(CONVERT(nvarchar,[CustomerItemNumber]), 1, PATINDEX('%[^0]%', CONVERT(nvarchar,[CustomerItemNumber])) - 1, '') ='{cutomerItemNumber}')";
				}
				else
				{
					//false - false
					query += @$"(CustomerItemNumber ='{cutomerItemNumber}')";
				}
			}
			var sqlCommand = new SqlCommand(query, connection, transaction);

			using(var reader = sqlCommand.ExecuteReader())
			{
				result = toList(reader);
			}
			return result;
		}
		internal static string sameKreisCondition(string kreis, string concatOperator)
		{
			return string.IsNullOrWhiteSpace(kreis) || string.IsNullOrWhiteSpace(concatOperator) ? "" : $@" {concatOperator} (TRIM(SUBSTRING(Artikelnummer, 1, CHARINDEX('-', Artikelnummer) - 1))=TRIM('{kreis}')
					/* -- Ensure there are exactly two hyphens */
					AND LEN(Artikelnummer) - LEN(REPLACE(Artikelnummer, '-', '')) = 2
    
					/* -- Ensure the first part exists (before the first hyphen) */
					AND CHARINDEX('-', Artikelnummer) > 1
    
					/* -- Ensure the second part exists (between the two hyphens) */
					AND CHARINDEX('-', Artikelnummer, CHARINDEX('-', Artikelnummer) + 1) - CHARINDEX('-', Artikelnummer) > 1
    
					/* -- Ensure the third part exists (after the second hyphen) */
					AND LEN(Artikelnummer) - CHARINDEX('-', Artikelnummer, CHARINDEX('-', Artikelnummer) + 1) > 1)";
		}
		public static int GetCustomerMaxNumberSequence(int customerNumber, string kreis)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT ISNULL(MAX([CustomerItemNumberSequence]), -1) FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);

				return (int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0);
			}
		}
		public static int GetCustomerNumberSequence(int customerNumber, string kreis, string cutomerItemNumber)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT ISNULL(MAX([CustomerItemNumberSequence]), -1)  FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("cutomerItemNumber", cutomerItemNumber ?? "");

				return (int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0);
			}
		}
		public static int GetCustomerIndexSequence(int customerNumber, string kreis, string cutomerItemNumber, string customerIndex, bool? isSpecialNumber = null)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT ISNULL(MAX([CustomerIndexSequence]), -1) FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber AND [CustomerIndex]=@customerIndex{(isSpecialNumber.HasValue ? $" AND ISNULL([IsArticleNumberSpecial],0)='{isSpecialNumber.Value}'" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("cutomerItemNumber", cutomerItemNumber ?? "");
				sqlCommand.Parameters.AddWithValue("customerIndex", customerIndex ?? "");

				return (int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0);
			}
		}
		public static int GetCustomerIndexSequence(int customerNumber, string kreis, string cutomerItemNumber, string customerIndex, string productionCountryName, string productionSiteCode)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT ISNULL(MAX([CustomerIndexSequence]), -1) FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber AND [CustomerIndex]=@customerIndex AND [ProductionCountryName]=@prodCountry AND [ProductionSiteCode]=@prodSite";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("cutomerItemNumber", cutomerItemNumber ?? "");
				sqlCommand.Parameters.AddWithValue("customerIndex", customerIndex ?? "");
				sqlCommand.Parameters.AddWithValue("prodCountry", productionCountryName ?? "");
				sqlCommand.Parameters.AddWithValue("prodSite", productionSiteCode ?? "");

				return (int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0);
			}
		}
		public static int GetCustomerNextIndexSequence(int customerNumber, string kreis, string cutomerItemNumber, bool isArticleNumberSpecial = false)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT ISNULL(MAX([CustomerIndexSequence]), -1) + 1 FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber AND IsNULL(IsArticleNumberSpecial,0)<>1";
				if(isArticleNumberSpecial)
				{
					query = $"SELECT ISNULL(MIN([CustomerIndexSequence]), 100) - 1 FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber AND IsArticleNumberSpecial=1";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("cutomerItemNumber", cutomerItemNumber ?? "");

				return (int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0);
			}
		}
		public static int GetProductionCountrySequence(int customerNumber, string kreis, string cutomerItemNumber, string customerIndex, string prodCountry)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT ISNULL(MAX([ProductionCountrySequence]), -1) FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber AND [CustomerIndex]=@customerIndex AND [ProductionCountryName]=@prodCountry";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("cutomerItemNumber", cutomerItemNumber ?? "");
				sqlCommand.Parameters.AddWithValue("customerIndex", customerIndex ?? "");
				sqlCommand.Parameters.AddWithValue("prodCountry", prodCountry ?? "");

				return (int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0);
			}
		}
		public static int GetProductionNextCountrySequence(int customerNumber, string kreis, string cutomerItemNumber, string customerIndex)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT ISNULL(MAX([ProductionCountrySequence]), -1) + 1 FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber AND [CustomerIndex]=@customerIndex";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("cutomerItemNumber", cutomerItemNumber ?? "");
				sqlCommand.Parameters.AddWithValue("customerIndex", customerIndex ?? "");

				return (int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0);
			}
		}
		public static int GetProductionNextSiteSequence(int customerNumber, string kreis, string cutomerItemNumber, string customerIndex, string prodCountry)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT ISNULL(MAX([CustomerIndexSequence]), -1) + 1 FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber AND [CustomerIndex]=@customerIndex AND [ProductionCountryName]=@prodCountry";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("cutomerItemNumber", cutomerItemNumber ?? "");
				sqlCommand.Parameters.AddWithValue("customerIndex", customerIndex ?? "");
				sqlCommand.Parameters.AddWithValue("prodCountry", prodCountry ?? "");

				return (int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0);
			}
		}
		public static int GetProductionSiteSequence(int customerNumber, string kreis, string cutomerItemNumber, string customerIndex, string prodCountry, string prodSite)
		{
			cutomerItemNumber = cutomerItemNumber ?? "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT ISNULL(MAX([CustomerIndexSequence]), -1) FROM [Artikel] Where [CustomerNumber]=@customerNumber{sameKreisCondition(kreis, "AND")} AND [CustomerItemNumber]=@cutomerItemNumber AND [CustomerIndex]=@customerIndex AND [ProductionCountryName]=@prodCountry AND [ProductionSiteCode]=@prodSite";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("cutomerItemNumber", cutomerItemNumber ?? "");
				sqlCommand.Parameters.AddWithValue("customerIndex", customerIndex ?? "");
				sqlCommand.Parameters.AddWithValue("prodCountry", prodCountry ?? "");
				sqlCommand.Parameters.AddWithValue("prodSite", prodSite ?? "");

				return (int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0);
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCustomerNumber(int customerNumber, bool topOne = false)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT {(topOne ? "TOP 1 " : "")}* FROM [Artikel] Where [CustomerNumber]=@customerNumber";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);

				using(var dataReader = sqlCommand.ExecuteReader())
				{
					while(dataReader.Read())
					{
						result.Add(new Entities.Tables.PRS.ArtikelEntity(dataReader));
					}
				}
			}

			return result;
		}
		public static List<Tuple<int, string, string>> GetByCustomerNumberAndPrefix(int customerNumber, string prefix, bool topOne = false)
		{
			var result = new List<Tuple<int, string, string>>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT Artikelnummer, CustomerItemNumber, CustomerItemNumberSequence FROM [Artikel] Where [CustomerNumber]=@customerNumber AND [CustomerPrefix]=@prefix";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("prefix", prefix ?? "");

				using(var dataReader = sqlCommand.ExecuteReader())
				{
					while(dataReader.Read())
					{
						result.Add(new Tuple<int, string, string>((dataReader["CustomerItemNumberSequence"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataReader["CustomerItemNumberSequence"]),
							(dataReader["CustomerItemNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataReader["CustomerItemNumber"]),
							(dataReader["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataReader["Artikelnummer"])));
					}
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetEFByKreis(string kreis)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel] Where (LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END)))=@kreis AND Warengruppe='EF'";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kreis", kreis ?? "");

				using(var dataReader = sqlCommand.ExecuteReader())
				{
					while(dataReader.Read())
					{
						result.Add(new Entities.Tables.PRS.ArtikelEntity(dataReader));
					}
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetEFByKreis(string kreis, SqlConnection connection, SqlTransaction transaction)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			string query = "SELECT * FROM [Artikel] Where (LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END)))=@kreis AND Warengruppe='EF'";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("kreis", kreis ?? "");

			using(var dataReader = sqlCommand.ExecuteReader())
			{
				while(dataReader.Read())
				{
					result.Add(new Entities.Tables.PRS.ArtikelEntity(dataReader));
				}
			}

			return result;
		}
		public static List<string> GetUniqueNumbers()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT DISTINCT Artikelnummer FROM [Artikel] ORDER BY Artikelnummer Desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x[0].ToString()).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		public static int EditCustomerWithTransaction(int? kundenummmer, string kreis, SqlConnection connection, SqlTransaction transaction)
		{
			string query = " UPDATE [Artikel] SET "
				+ "[CustomerNumber]=@CustomerNumber,"
				+ "[CustomerPrefix]=@CustomerPrefix WHERE (LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END)))=@CustomerPrefix; ";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", kundenummmer == null ? (object)DBNull.Value : kundenummmer);
			sqlCommand.Parameters.AddWithValue("CustomerPrefix", kreis ?? "");

			sqlCommand.CommandText = query;
			return sqlCommand.ExecuteNonQuery();
		}
		public static int EditCustomerWithTransaction(int? kundenummmer, List<int> articleIds, SqlConnection connection, SqlTransaction transaction)
		{
			if(articleIds == null || articleIds.Count <= 0)
			{
				return 0;
			}
			string query = " UPDATE [Artikel] SET "
				+ $"[CustomerNumber]=@CustomerNumber  WHERE [Artikel-Nr] IN ({string.Join(",", articleIds)});";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", kundenummmer == null ? (object)DBNull.Value : kundenummmer);

			sqlCommand.CommandText = query;
			return sqlCommand.ExecuteNonQuery();
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetKreisSiblings(int id)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT b.* FROM [Artikel] a Join [Artikel] b on (LEFT(b.[Artikelnummer], (CASE WHEN CHARINDEX('-',b.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',b.[Artikelnummer],0)-1 END)))=(LEFT(a.[Artikelnummer], (CASE WHEN CHARINDEX('-',a.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',a.[Artikelnummer],0)-1 END))) Where a.[Artikel-Nr]=@id /*AND b.Warengruppe='EF'*/ AND IsNULL(b.Aktiv, 0)<>0 ORDER BY b.ArtikelNummer, b.[CustomerItemNumber]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				using(var dataReader = sqlCommand.ExecuteReader())
				{
					while(dataReader.Read())
					{
						result.Add(new Entities.Tables.PRS.ArtikelEntity(dataReader));
					}
				}
			}

			return result;
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.Orders.ArtikelStatisticsEntity> GetArtikelNrOrArtikelNummer(int ArtikleNr = 0, string ArtikleNummer = "", bool choice = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				//paginationFilter
				string query = $@"";
				if(choice && !string.IsNullOrEmpty(ArtikleNummer))
				{
					query = $@"select top 1 a.[Artikel-Nr] , '{ArtikleNummer}' as Artikelnummer  from Artikel a where a.ArtikelNummer = '{ArtikleNummer}' ";
				}
				else
				{
					query = $@"select top 1 a.ArtikelNummer , {ArtikleNr} as [Artikel-Nr]  from Artikel a where a.[Artikel-Nr] = {ArtikleNr} ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.Orders.ArtikelStatisticsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.Orders.ArtikelStatisticsEntity>();
			}
		}


		#endregion
		#endregion

		public static List<Entities.Tables.PRS.ArtikelEntity> GetCNumberSiblings(int id)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT b.* FROM [Artikel] a Join [Artikel] b on (LEFT(b.[Artikelnummer], (CASE WHEN CHARINDEX('-',b.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',b.[Artikelnummer],0)-1 END)))=(LEFT(a.[Artikelnummer], (CASE WHEN CHARINDEX('-',a.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',a.[Artikelnummer],0)-1 END))) AND IsNULL(b.[CustomerItemNumber],'')=IsNULL(a.[CustomerItemNumber], '') Where a.[Artikel-Nr]=@id /*AND b.Warengruppe='EF'*/ AND IsNULL(b.Aktiv, 0)<>0 ORDER BY b.ArtikelNummer, b.[CustomerItemNumber]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				using(var dataReader = sqlCommand.ExecuteReader())
				{
					while(dataReader.Read())
					{
						result.Add(new Entities.Tables.PRS.ArtikelEntity(dataReader));
					}
				}
			}

			return result;
		}

		public static List<Entities.Tables.PRS.ArtikelEntity> GetCNumberSiblings(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			{
				string query = "SELECT b.* FROM [Artikel] a Join [Artikel] b on (LEFT(b.[Artikelnummer], (CASE WHEN CHARINDEX('-',b.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',b.[Artikelnummer],0)-1 END)))=(LEFT(a.[Artikelnummer], (CASE WHEN CHARINDEX('-',a.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',a.[Artikelnummer],0)-1 END))) AND IsNULL(b.[CustomerItemNumber],'')=IsNULL(a.[CustomerItemNumber], '') Where a.[Artikel-Nr]=@id /*AND b.Warengruppe='EF'*/ AND IsNULL(b.Aktiv, 0)<>0 ORDER BY b.ArtikelNummer, b.[CustomerItemNumber]";
				var sqlCommand = new SqlCommand(query, connection, transaction);
				sqlCommand.Parameters.AddWithValue("id", id);

				using(var dataReader = sqlCommand.ExecuteReader())
				{
					while(dataReader.Read())
					{
						result.Add(new Entities.Tables.PRS.ArtikelEntity(dataReader));
					}
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetIndexSiblings(int id)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT b.* FROM [Artikel] a Join [Artikel] b on (LEFT(b.[Artikelnummer], (CASE WHEN CHARINDEX('-',b.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',b.[Artikelnummer],0)-1 END)))=(LEFT(a.[Artikelnummer], (CASE WHEN CHARINDEX('-',a.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',a.[Artikelnummer],0)-1 END))) AND IsNULL(b.[CustomerItemNumber],'')=IsNULL(a.[CustomerItemNumber], '') AND IsNULL(b.Index_Kunde,'')=IsNULL(a.Index_Kunde, '') Where a.[Artikel-Nr]=@id /*AND b.Warengruppe='EF'*/ AND IsNULL(b.Aktiv, 0)<>0 ORDER BY b.ArtikelNummer, b.[CustomerItemNumber]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				using(var dataReader = sqlCommand.ExecuteReader())
				{
					while(dataReader.Read())
					{
						result.Add(new Entities.Tables.PRS.ArtikelEntity(dataReader));
					}
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetIndexSiblings(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			{

				string query = "SELECT b.* FROM [Artikel] a Join [Artikel] b on (LEFT(b.[Artikelnummer], (CASE WHEN CHARINDEX('-',b.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',b.[Artikelnummer],0)-1 END)))=(LEFT(a.[Artikelnummer], (CASE WHEN CHARINDEX('-',a.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',a.[Artikelnummer],0)-1 END))) AND IsNULL(b.[CustomerItemNumber],'')=IsNULL(a.[CustomerItemNumber], '') AND IsNULL(b.Index_Kunde,'')=IsNULL(a.Index_Kunde, '') Where a.[Artikel-Nr]=@id /*AND b.Warengruppe='EF'*/ AND IsNULL(b.Aktiv, 0)<>0 ORDER BY b.ArtikelNummer, b.[CustomerItemNumber]";
				var sqlCommand = new SqlCommand(query, connection, transaction);
				sqlCommand.Parameters.AddWithValue("id", id);

				using(var dataReader = sqlCommand.ExecuteReader())
				{
					while(dataReader.Read())
					{
						result.Add(new Entities.Tables.PRS.ArtikelEntity(dataReader));
					}
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetIndexSiblings(int customerNumber, string customerItemNumber, string customerItemIndex, bool? isSpecialNumber = null)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Artikel] Where CustomerNumber=@customerNumber AND CustomerItemNumber=@customerItemNumber AND CustomerIndex=@customerItemIndex /*AND b.Warengruppe='EF'*/ AND IsNULL(Aktiv, 0)<>0 {(isSpecialNumber.HasValue == true ? $" AND ISNULL([IsArticleNumberSpecial],0)={(isSpecialNumber.Value == true ? "1" : "0")}" : "")} ORDER BY Artikelnummer, [CustomerNumber]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("customerItemNumber", customerItemNumber ?? "");
				sqlCommand.Parameters.AddWithValue("customerItemIndex", customerItemIndex ?? "");

				using(var dataReader = sqlCommand.ExecuteReader())
				{
					while(dataReader.Read())
					{
						result.Add(new Entities.Tables.PRS.ArtikelEntity(dataReader));
					}
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByArchiveStatus(List<int> ids = null, bool? active = null)
		{
			int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();
			if(ids.Count <= maxQueryNumber)
			{
				result = getByArchiveStatus(ids, active);
			}
			else
			{
				int batchNumber = ids.Count / maxQueryNumber;
				result = new List<Entities.Tables.PRS.ArtikelEntity>();
				for(int i = 0; i < batchNumber; i++)
				{
					result.AddRange(getByArchiveStatus(ids.GetRange(i * maxQueryNumber, maxQueryNumber), active));
				}
				result.AddRange(getByArchiveStatus(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), active));
			}
			return result;
		}
		private static List<Entities.Tables.PRS.ArtikelEntity> getByArchiveStatus(List<int> ids = null, bool? active = null)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				if(ids == null || ids.Count <= 0)
					return new List<Entities.Tables.PRS.ArtikelEntity>();

				sqlConnection.Open();

				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;

				string queryIds = string.Empty;
				string whereClause = "";
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				whereClause = $" WHERE [Artikel-Nr] IN ({queryIds})";
				if(active.HasValue)
				{
					whereClause += $" AND aktiv={(active.Value ? "1" : "0")} ";
				}

				sqlCommand.CommandText = $"SELECT * FROM [Artikel] {whereClause}";

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
			}
			return result;
		}
		public static int ResetCustomerEdiDefaultWithTransaction(int customerNumber, string customerItemNumber, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Artikel] SET [EdiDefault]=0 WHERE [CustomerNumber]=@customerNumber AND [CustomerItemNumber]=@customerItemNumber;";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
			sqlCommand.Parameters.AddWithValue("customerItemNumber", customerItemNumber ?? "");

			sqlCommand.CommandText = query;
			return sqlCommand.ExecuteNonQuery();
		}
		public static int SetCustomerEdiDefaultWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Artikel] SET [EdiDefault]=1 WHERE [Artikel-Nr]=@id;";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("id", id);

			sqlCommand.CommandText = query;
			return sqlCommand.ExecuteNonQuery();
		}
		public static int SetIsEDrawingWithTransaction(int id, bool isEDrawing, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Artikel] SET [IsEDrawing]=@IsEDrawing WHERE [Artikel-Nr]=@id;";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("id", id);
			sqlCommand.Parameters.AddWithValue("IsEDrawing", isEDrawing);

			sqlCommand.CommandText = query;
			return sqlCommand.ExecuteNonQuery();
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetAllCustomersEdi(bool onlyEdiActive = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Artikel] WHERE LEFT(Artikelnummer,3) IN ('985', '986', '989') {(onlyEdiActive ? "AND [EdiDefault]=1" : "")};";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			}
		}
		public static List<Entities.Tables.PRS.MinimalArtikelEntity> GetDefaultEdi(int customerNumber, bool includeDeisgnation1 = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				//var descriptionName = "[Bezeichnung 1]";
				//if(trimLeadingZeros == true)
				//{
				//	descriptionName = $"CASE WHEN {descriptionName} LIKE '0%' THEN STUFF({descriptionName}, 1, PATINDEX('%[^0]%', {descriptionName}) - 1, '') ELSE {descriptionName} END";
				//}

				string query = $"SELECT * FROM [Artikel] WHERE  [EdiDefault]=1 AND ([CustomerNumber]=@customerNumber {(includeDeisgnation1 ? " OR [Bezeichnung 1]=@customerNumber" : "")})";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity>();
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetIsEDrawing(int customerNumber)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Artikel] WHERE [IsEDrawing]=1 AND [CustomerNumber]=@customerNumber;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetDefaultEdi(List<int> customerNumbers)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Artikel] WHERE [EdiDefault]=1 AND [CustomerNumber] IN ('{string.Join("','", customerNumbers)}');";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			}
		}
		public static List<Entities.Tables.PRS.MinimalArtikelEntity> GetDefaultEdi(List<int> customerNumbers, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();


			string query = $"SELECT * FROM [Artikel] WHERE [EdiDefault]=1 AND [CustomerNumber] IN ({string.Join(',', customerNumbers)})";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity>();
			}
		}
		public static Entities.Tables.PRS.ArtikelEntity GetDefaultEdi(int customerNumber, string customerItemNumber)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT TOP 1 * FROM [Artikel] WHERE [CustomerNumber]=@customerNumber AND [CustomerItemNumber]=@customerItemNumber AND [EdiDefault]=1;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("customerItemNumber", customerItemNumber ?? "");
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.ArtikelEntity GetDefaultEdi(int customerNumber, string customerItemNumber, SqlConnection connection, SqlTransaction transaction)
		{
			customerItemNumber = customerItemNumber ?? "";
			var dataTable = new DataTable();
			string query = "SELECT TOP 1 * FROM [Artikel] WHERE [CustomerNumber]=@customerNumber AND [CustomerItemNumber]=@customerItemNumber AND [EdiDefault]=1;";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
			sqlCommand.Parameters.AddWithValue("customerItemNumber", customerItemNumber ?? "");
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.MinimalArtikelEntity> GetDefaultEdi(int customerNumber, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = $"SELECT * FROM [Artikel] WHERE [EdiDefault]=1 AND CustomerNumber=@customerNumber";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity>();
			}
		}


		#region Transactions 
		public static int AddWithTransaction(Entities.Tables.PRS.ArtikelEntity element, SqlConnection connection, SqlTransaction transaction)
		{
			int response = -1;
			string query = "INSERT INTO [Artikel] "
				+ " ([Abladestelle],[aktiv],[aktualisiert],[Anfangsbestand],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[Artikelkurztext],[Artikelnummer],[Barverkauf],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[BezeichnungAL],[COF_Pflichtig],[Crossreferenz],[Cu-Gewicht],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dokumente],[EAN],[Einheit],[EMPB],[EMPB_Freigegeben],[Ersatzartikel],[ESD_Schutz],[Exportgewicht],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Hubmastleitungen],[ID_Klassifizierung],[Index_Kunde],[Index_Kunde_Datum],[Info_WE],[Kanban],[Kategorie],[Klassifizierung],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Lieferzeit],[Losgroesse],[Materialkosten_Alt],[MHD],[Minerals Confirmity],[Praeferenz_Aktuelles_jahr],[Praeferenz_Folgejahr],[Preiseinheit],[pro Zeiteinheit],[Produktionszeit],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmen2],[Rahmenauslauf],[Rahmenauslauf2],[Rahmenmenge],[Rahmenmenge2],[Rahmen-Nr],[Rahmen-Nr2],[REACH SVHC Confirmity],[ROHS EEE Confirmity],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[Verpackung],[Verpackungsart],[Verpackungsmenge],[VK-Festpreis],[Volumen],[Warengruppe],[Warentyp],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zeitraum_MHD],[Zolltarif_nr],[VDA_1],[VDA_2],[Zuschlag_VK],[artikelklassifizierung],[UBG],[EdiDefault],[IsEDrawing]) "
				+ " OUTPUT INSERTED.[Artikel-Nr] VALUES "
				+ " (@Abladestelle,@aktiv,@aktualisiert,@Anfangsbestand,@ArtikelAusEigenerProduktion,@ArtikelFürWeitereBestellungenSperren,@Artikelfamilie_Kunde,@Artikelfamilie_Kunde_Detail1,@Artikelfamilie_Kunde_Detail2,@Artikelkurztext,@Artikelnummer,@Barverkauf,@Bezeichnung1,@Bezeichnung2,@Bezeichnung3,@BezeichnungAL,@COF_Pflichtig,@Crossreferenz,@CuGewicht,@DatumAnfangsbestand,@DEL,@DELFixiert,@Dokumente,@EAN,@Einheit,@EMPB,@EMPB_Freigegeben,@Ersatzartikel,@ESD_Schutz,@Exportgewicht,@fakturierenStückliste,@Farbe,@fibu_rahmen,@Freigabestatus,@FreigabestatusTNIntern,@Gebinde,@Gewicht,@Größe,@GrundFürSperre,@gültigBis,@Halle,@Hubmastleitungen,@ID_Klassifizierung,@Index_Kunde,@Index_Kunde_Datum,@Info_WE,@Kanban,@Kategorie,@Klassifizierung,@Kriterium1,@Kriterium2,@Kriterium3,@Kriterium4,@Kupferbasis,@Kupferzahl,@Lagerartikel,@Lagerhaltungskosten,@Langtext,@Langtext_drucken_AB,@Langtext_drucken_BW,@Lieferzeit,@Losgroesse,@Materialkosten_Alt,@MHD,@MineralsConfirmity,@Praeferenz_Aktuelles_jahr,@Praeferenz_Folgejahr,@Preiseinheit,@proZeiteinheit,@Produktionszeit,@Provisionsartikel,@PrüfstatusTNWare,@Rabattierfähig,@Rahmen,@Rahmen2,@Rahmenauslauf,@Rahmenauslauf2,@Rahmenmenge,@Rahmenmenge2,@RahmenNr,@RahmenNr2,@REACHSVHCConfirmity,@ROHSEEEConfirmity,@Seriennummer,@Seriennummernverwaltung,@Sonderrabatt,@Standard_Lagerort_id,@Stückliste,@Stundensatz,@Sysmonummer,@ULEtikett,@ULzertifiziert,@Umsatzsteuer,@Ursprungsland,@Verpackung,@Verpackungsart,@Verpackungsmenge,@VKFestpreis,@Volumen,@Warengruppe,@Warentyp,@Webshop,@Werkzeug,@Wert_Anfangsbestand,@Zeichnungsnummer,@Zeitraum_MHD,@Zolltarif_nr,@VDA_1,@VDA_2,@Zuschlag_VK,@artikelklassifizierung,@UBG,@EdiDefault,@IsEDrawing);";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Abladestelle", element.Abladestelle == null ? (object)DBNull.Value : element.Abladestelle);
			sqlCommand.Parameters.AddWithValue("aktiv", element.aktiv == null ? (object)DBNull.Value : element.aktiv);
			sqlCommand.Parameters.AddWithValue("aktualisiert", element.aktualisiert == null ? (object)DBNull.Value : element.aktualisiert);
			sqlCommand.Parameters.AddWithValue("Anfangsbestand", element.Anfangsbestand == null ? (object)DBNull.Value : element.Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("ArtikelAusEigenerProduktion", element.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : element.ArtikelAusEigenerProduktion);
			sqlCommand.Parameters.AddWithValue("ArtikelFürWeitereBestellungenSperren", element.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : element.ArtikelFürWeitereBestellungenSperren);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", element.Artikelfamilie_Kunde == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", element.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail1);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", element.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail2);
			sqlCommand.Parameters.AddWithValue("Artikelkurztext", element.Artikelkurztext == null ? (object)DBNull.Value : element.Artikelkurztext);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", element.ArtikelNummer == null ? (object)DBNull.Value : element.ArtikelNummer);
			sqlCommand.Parameters.AddWithValue("Barverkauf", element.Barverkauf == null ? (object)DBNull.Value : element.Barverkauf);
			sqlCommand.Parameters.AddWithValue("Bezeichnung1", element.Bezeichnung1 == null ? (object)DBNull.Value : element.Bezeichnung1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung2", element.Bezeichnung2 == null ? (object)DBNull.Value : element.Bezeichnung2);
			sqlCommand.Parameters.AddWithValue("Bezeichnung3", element.Bezeichnung3 == null ? (object)DBNull.Value : element.Bezeichnung3);
			sqlCommand.Parameters.AddWithValue("BezeichnungAL", element.BezeichnungAL == null ? (object)DBNull.Value : element.BezeichnungAL);
			sqlCommand.Parameters.AddWithValue("COF_Pflichtig", element.COF_Pflichtig == null ? (object)DBNull.Value : element.COF_Pflichtig);
			sqlCommand.Parameters.AddWithValue("Crossreferenz", element.Crossreferenz == null ? (object)DBNull.Value : element.Crossreferenz);
			sqlCommand.Parameters.AddWithValue("CuGewicht", element.CuGewicht == null ? (object)DBNull.Value : element.CuGewicht);
			sqlCommand.Parameters.AddWithValue("DatumAnfangsbestand", element.DatumAnfangsbestand == null ? (object)DBNull.Value : element.DatumAnfangsbestand);
			sqlCommand.Parameters.AddWithValue("DEL", element.DEL == null ? (object)DBNull.Value : element.DEL);
			sqlCommand.Parameters.AddWithValue("DELFixiert", element.DELFixiert == null ? (object)DBNull.Value : element.DELFixiert);
			sqlCommand.Parameters.AddWithValue("Dokumente", element.Dokumente == null ? (object)DBNull.Value : element.Dokumente);
			sqlCommand.Parameters.AddWithValue("EAN", element.EAN == null ? (object)DBNull.Value : element.EAN);
			sqlCommand.Parameters.AddWithValue("Einheit", element.Einheit == null ? (object)DBNull.Value : element.Einheit);
			sqlCommand.Parameters.AddWithValue("EMPB", element.EMPB == null ? (object)DBNull.Value : element.EMPB);
			sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben", element.EMPB_Freigegeben == null ? (object)DBNull.Value : element.EMPB_Freigegeben);
			sqlCommand.Parameters.AddWithValue("Ersatzartikel", element.Ersatzartikel == null ? (object)DBNull.Value : element.Ersatzartikel);
			sqlCommand.Parameters.AddWithValue("ESD_Schutz", element.ESD_Schutz == null ? (object)DBNull.Value : element.ESD_Schutz);
			sqlCommand.Parameters.AddWithValue("Exportgewicht", element.Exportgewicht == null ? (object)DBNull.Value : element.Exportgewicht);
			sqlCommand.Parameters.AddWithValue("fakturierenStückliste", element.fakturierenStückliste == null ? (object)DBNull.Value : element.fakturierenStückliste);
			sqlCommand.Parameters.AddWithValue("Farbe", element.Farbe == null ? (object)DBNull.Value : element.Farbe);
			sqlCommand.Parameters.AddWithValue("fibu_rahmen", element.fibu_rahmen == null ? (object)DBNull.Value : element.fibu_rahmen);
			sqlCommand.Parameters.AddWithValue("Freigabestatus", element.Freigabestatus == null ? (object)DBNull.Value : element.Freigabestatus);
			sqlCommand.Parameters.AddWithValue("FreigabestatusTNIntern", element.FreigabestatusTNIntern == null ? (object)DBNull.Value : element.FreigabestatusTNIntern);
			sqlCommand.Parameters.AddWithValue("Gebinde", element.Gebinde == null ? (object)DBNull.Value : element.Gebinde);
			sqlCommand.Parameters.AddWithValue("Gewicht", element.Gewicht == null ? (object)DBNull.Value : element.Gewicht);
			sqlCommand.Parameters.AddWithValue("Größe", element.Größe == null ? (object)DBNull.Value : element.Größe);
			sqlCommand.Parameters.AddWithValue("GrundFürSperre", element.GrundFürSperre == null ? (object)DBNull.Value : element.GrundFürSperre);
			sqlCommand.Parameters.AddWithValue("gültigBis", element.gültigBis == null ? (object)DBNull.Value : element.gültigBis);
			sqlCommand.Parameters.AddWithValue("Halle", element.Halle == null ? (object)DBNull.Value : element.Halle);
			sqlCommand.Parameters.AddWithValue("Hubmastleitungen", element.Hubmastleitungen == null ? (object)DBNull.Value : element.Hubmastleitungen);
			sqlCommand.Parameters.AddWithValue("ID_Klassifizierung", element.ID_Klassifizierung == null ? (object)DBNull.Value : element.ID_Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", element.Index_Kunde == null ? (object)DBNull.Value : element.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", element.Index_Kunde_Datum == null ? (object)DBNull.Value : element.Index_Kunde_Datum);
			sqlCommand.Parameters.AddWithValue("Info_WE", element.Info_WE == null ? (object)DBNull.Value : element.Info_WE);
			sqlCommand.Parameters.AddWithValue("Kanban", element.Kanban == null ? (object)DBNull.Value : element.Kanban);
			sqlCommand.Parameters.AddWithValue("Kategorie", element.Kategorie == null ? (object)DBNull.Value : element.Kategorie);
			sqlCommand.Parameters.AddWithValue("Klassifizierung", element.Klassifizierung == null ? (object)DBNull.Value : element.Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Kriterium1", element.Kriterium1 == null ? (object)DBNull.Value : element.Kriterium1);
			sqlCommand.Parameters.AddWithValue("Kriterium2", element.Kriterium2 == null ? (object)DBNull.Value : element.Kriterium2);
			sqlCommand.Parameters.AddWithValue("Kriterium3", element.Kriterium3 == null ? (object)DBNull.Value : element.Kriterium3);
			sqlCommand.Parameters.AddWithValue("Kriterium4", element.Kriterium4 == null ? (object)DBNull.Value : element.Kriterium4);
			sqlCommand.Parameters.AddWithValue("Kupferbasis", element.Kupferbasis == null ? (object)DBNull.Value : element.Kupferbasis);
			sqlCommand.Parameters.AddWithValue("Kupferzahl", element.Kupferzahl == null ? (object)DBNull.Value : element.Kupferzahl);
			sqlCommand.Parameters.AddWithValue("Lagerartikel", element.Lagerartikel == null ? (object)DBNull.Value : element.Lagerartikel);
			sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", element.Lagerhaltungskosten == null ? (object)DBNull.Value : element.Lagerhaltungskosten);
			sqlCommand.Parameters.AddWithValue("Langtext", element.Langtext == null ? (object)DBNull.Value : element.Langtext);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", element.Langtext_drucken_AB == null ? (object)DBNull.Value : element.Langtext_drucken_AB);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", element.Langtext_drucken_BW == null ? (object)DBNull.Value : element.Langtext_drucken_BW);
			sqlCommand.Parameters.AddWithValue("Lieferzeit", element.Lieferzeit == null ? (object)DBNull.Value : element.Lieferzeit);
			sqlCommand.Parameters.AddWithValue("Losgroesse", element.Losgroesse == null ? (object)DBNull.Value : element.Losgroesse);
			sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", element.Materialkosten_Alt == null ? (object)DBNull.Value : element.Materialkosten_Alt);
			sqlCommand.Parameters.AddWithValue("MHD", element.MHD == null ? (object)DBNull.Value : element.MHD);
			sqlCommand.Parameters.AddWithValue("MineralsConfirmity", element.MineralsConfirmity == null ? (object)DBNull.Value : element.MineralsConfirmity);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr", element.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : element.Praeferenz_Aktuelles_jahr);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr", element.Praeferenz_Folgejahr == null ? (object)DBNull.Value : element.Praeferenz_Folgejahr);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", element.Preiseinheit == null ? (object)DBNull.Value : element.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("proZeiteinheit", element.proZeiteinheit == null ? (object)DBNull.Value : element.proZeiteinheit);
			sqlCommand.Parameters.AddWithValue("Produktionszeit", element.Produktionszeit == null ? (object)DBNull.Value : element.Produktionszeit);
			sqlCommand.Parameters.AddWithValue("Provisionsartikel", element.Provisionsartikel == null ? (object)DBNull.Value : element.Provisionsartikel);
			sqlCommand.Parameters.AddWithValue("PrüfstatusTNWare", element.PrufstatusTNWare == null ? (object)DBNull.Value : element.PrufstatusTNWare);
			sqlCommand.Parameters.AddWithValue("Rabattierfähig", element.Rabattierfähig == null ? (object)DBNull.Value : element.Rabattierfähig);
			sqlCommand.Parameters.AddWithValue("Rahmen", element.Rahmen == null ? (object)DBNull.Value : element.Rahmen);
			sqlCommand.Parameters.AddWithValue("Rahmen2", element.Rahmen2 == null ? (object)DBNull.Value : element.Rahmen2);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf", element.Rahmenauslauf == null ? (object)DBNull.Value : element.Rahmenauslauf);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf2", element.Rahmenauslauf2 == null ? (object)DBNull.Value : element.Rahmenauslauf2);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge", element.Rahmenmenge == null ? (object)DBNull.Value : element.Rahmenmenge);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge2", element.Rahmenmenge2 == null ? (object)DBNull.Value : element.Rahmenmenge2);
			sqlCommand.Parameters.AddWithValue("RahmenNr", element.RahmenNr == null ? (object)DBNull.Value : element.RahmenNr);
			sqlCommand.Parameters.AddWithValue("RahmenNr2", element.RahmenNr2 == null ? (object)DBNull.Value : element.RahmenNr2);
			sqlCommand.Parameters.AddWithValue("REACHSVHCConfirmity", element.REACHSVHCConfirmity == null ? (object)DBNull.Value : element.REACHSVHCConfirmity);
			sqlCommand.Parameters.AddWithValue("ROHSEEEConfirmity", element.ROHSEEEConfirmity == null ? (object)DBNull.Value : element.ROHSEEEConfirmity);
			sqlCommand.Parameters.AddWithValue("Seriennummer", element.Seriennummer == null ? (object)DBNull.Value : element.Seriennummer);
			sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", element.Seriennummernverwaltung == null ? (object)DBNull.Value : element.Seriennummernverwaltung);
			sqlCommand.Parameters.AddWithValue("Sonderrabatt", element.Sonderrabatt == null ? (object)DBNull.Value : element.Sonderrabatt);
			sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", element.Standard_Lagerort_id == null ? (object)DBNull.Value : element.Standard_Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Stückliste", element.Stuckliste == null ? (object)DBNull.Value : element.Stuckliste);
			sqlCommand.Parameters.AddWithValue("Stundensatz", element.Stundensatz == null ? (object)DBNull.Value : element.Stundensatz);
			sqlCommand.Parameters.AddWithValue("Sysmonummer", element.Sysmonummer == null ? (object)DBNull.Value : element.Sysmonummer);
			sqlCommand.Parameters.AddWithValue("ULEtikett", element.ULEtikett == null ? (object)DBNull.Value : element.ULEtikett);
			sqlCommand.Parameters.AddWithValue("ULzertifiziert", element.ULzertifiziert == null ? (object)DBNull.Value : element.ULzertifiziert);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", element.Umsatzsteuer == null ? (object)DBNull.Value : element.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("Ursprungsland", element.Ursprungsland == null ? (object)DBNull.Value : element.Ursprungsland);
			sqlCommand.Parameters.AddWithValue("Verpackung", element.Verpackung == null ? (object)DBNull.Value : element.Verpackung);
			sqlCommand.Parameters.AddWithValue("Verpackungsart", element.Verpackungsart == null ? (object)DBNull.Value : element.Verpackungsart);
			sqlCommand.Parameters.AddWithValue("Verpackungsmenge", element.Verpackungsmenge == null ? (object)DBNull.Value : element.Verpackungsmenge);
			sqlCommand.Parameters.AddWithValue("VKFestpreis", element.VKFestpreis == null ? (object)DBNull.Value : element.VKFestpreis);
			sqlCommand.Parameters.AddWithValue("Volumen", element.Volumen == null ? (object)DBNull.Value : element.Volumen);
			sqlCommand.Parameters.AddWithValue("Warengruppe", element.Warengruppe == null ? (object)DBNull.Value : element.Warengruppe);
			sqlCommand.Parameters.AddWithValue("Warentyp", element.Warentyp == null ? (object)DBNull.Value : element.Warentyp);
			sqlCommand.Parameters.AddWithValue("Webshop", element.Webshop == null ? (object)DBNull.Value : element.Webshop);
			sqlCommand.Parameters.AddWithValue("Werkzeug", element.Werkzeug == null ? (object)DBNull.Value : element.Werkzeug);
			sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", element.Wert_Anfangsbestand == null ? (object)DBNull.Value : element.Wert_Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", element.Zeichnungsnummer == null ? (object)DBNull.Value : element.Zeichnungsnummer);
			sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", element.Zeitraum_MHD == null ? (object)DBNull.Value : element.Zeitraum_MHD);
			sqlCommand.Parameters.AddWithValue("Zolltarif_nr", element.Zolltarif_nr == null ? (object)DBNull.Value : element.Zolltarif_nr);
			sqlCommand.Parameters.AddWithValue("VDA_1", element.VDA_1 == null ? (object)DBNull.Value : element.VDA_1);
			sqlCommand.Parameters.AddWithValue("VDA_2", element.VDA_2 == null ? (object)DBNull.Value : element.VDA_2);
			sqlCommand.Parameters.AddWithValue("Zuschlag_VK", element.Zuschlag_VK == null ? (object)DBNull.Value : element.Zuschlag_VK);
			sqlCommand.Parameters.AddWithValue("artikelklassifizierung", element.artikelklassifizierung == null ? (object)DBNull.Value : element.artikelklassifizierung);
			sqlCommand.Parameters.AddWithValue("UBG", element.UBG);
			sqlCommand.Parameters.AddWithValue("EdiDefault", element.EdiDefault == null ? (object)DBNull.Value : element.EdiDefault);
			sqlCommand.Parameters.AddWithValue("IsEDrawing", element.IsEDrawing == null ? (object)DBNull.Value : element.IsEDrawing);

			var result = sqlCommand.ExecuteScalar();
			response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;


			return response;
		}
		public static int AddWithTransaction(List<Entities.Tables.PRS.ArtikelEntity> elements, SqlConnection connection, SqlTransaction transaction)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 109; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = addWithTransaction(elements, connection, transaction);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += addWithTransaction(elements.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					result += addWithTransaction(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}

			return -1;
		}
		private static int addWithTransaction(List<Entities.Tables.PRS.ArtikelEntity> elements, SqlConnection connection, SqlTransaction transaction)
		{
			if(elements != null && elements.Count > 0)
			{
				int response = -1;

				//using (var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				//{
				//    sqlConnection.Open();

				string query = " INSERT INTO [Artikel] ([Abladestelle],[aktiv],[aktualisiert],[Anfangsbestand],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[Artikelkurztext],[Artikelnummer],[Barverkauf],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[BezeichnungAL],[COF_Pflichtig],[Crossreferenz],[Cu-Gewicht],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dokumente],[EAN],[Einheit],[EMPB],[EMPB_Freigegeben],[Ersatzartikel],[ESD_Schutz],[Exportgewicht],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Hubmastleitungen],[ID_Klassifizierung],[Index_Kunde],[Index_Kunde_Datum],[Info_WE],[Kanban],[Kategorie],[Klassifizierung],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Lieferzeit],[Losgroesse],[Materialkosten_Alt],[MHD],[Minerals Confirmity],[Praeferenz_Aktuelles_jahr],[Praeferenz_Folgejahr],[Preiseinheit],[pro Zeiteinheit],[Produktionszeit],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmen2],[Rahmenauslauf],[Rahmenauslauf2],[Rahmenmenge],[Rahmenmenge2],[Rahmen-Nr],[Rahmen-Nr2],[REACH SVHC Confirmity],[ROHS EEE Confirmity],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[Verpackung],[Verpackungsart],[Verpackungsmenge],[VKFestpreis],[Volumen],[Warengruppe],[Warentyp],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zeitraum_MHD],[Zolltarif_nr],[VDA_1],[VDA_2],[Zuschlag_VK],[artikelklassifizierung],[UBG],[EdiDefault],[IsEDrawing]) VALUES ";

				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var element in elements)
				{
					i++;
					query += " ( "

						+ "@Abladestelle" + i + ","
						+ "@aktiv" + i + ","
						+ "@aktualisiert" + i + ","
						+ "@Anfangsbestand" + i + ","
						+ "@ArtikelAusEigenerProduktion" + i + ","
						+ "@ArtikelFürWeitereBestellungenSperren" + i + ","
						+ "@Artikelfamilie_Kunde" + i + ","
						+ "@Artikelfamilie_Kunde_Detail1" + i + ","
						+ "@Artikelfamilie_Kunde_Detail2" + i + ","
						+ "@Artikelkurztext" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Barverkauf" + i + ","
						+ "@Bezeichnung1" + i + ","
						+ "@Bezeichnung2" + i + ","
						+ "@Bezeichnung3" + i + ","
						+ "@BezeichnungAL" + i + ","
						+ "@COF_Pflichtig" + i + ","
						+ "@Crossreferenz" + i + ","
						+ "@CuGewicht" + i + ","
						+ "@DatumAnfangsbestand" + i + ","
						+ "@DEL" + i + ","
						+ "@DELFixiert" + i + ","
						+ "@Dokumente" + i + ","
						+ "@EAN" + i + ","
						+ "@Einheit" + i + ","
						+ "@EMPB" + i + ","
						+ "@EMPB_Freigegeben" + i + ","
						+ "@Ersatzartikel" + i + ","
						+ "@ESD_Schutz" + i + ","
						+ "@Exportgewicht" + i + ","
						+ "@fakturierenStückliste" + i + ","
						+ "@Farbe" + i + ","
						+ "@fibu_rahmen" + i + ","
						+ "@Freigabestatus" + i + ","
						+ "@FreigabestatusTNIntern" + i + ","
						+ "@Gebinde" + i + ","
						+ "@Gewicht" + i + ","
						+ "@Größe" + i + ","
						+ "@GrundFürSperre" + i + ","
						+ "@gültigBis" + i + ","
						+ "@Halle" + i + ","
						+ "@Hubmastleitungen" + i + ","
						+ "@ID_Klassifizierung" + i + ","
						+ "@Index_Kunde" + i + ","
						+ "@Index_Kunde_Datum" + i + ","
						+ "@Info_WE" + i + ","
						+ "@Kanban" + i + ","
						+ "@Kategorie" + i + ","
						+ "@Klassifizierung" + i + ","
						+ "@Kriterium1" + i + ","
						+ "@Kriterium2" + i + ","
						+ "@Kriterium3" + i + ","
						+ "@Kriterium4" + i + ","
						+ "@Kupferbasis" + i + ","
						+ "@Kupferzahl" + i + ","
						+ "@Lagerartikel" + i + ","
						+ "@Lagerhaltungskosten" + i + ","
						+ "@Langtext" + i + ","
						+ "@Langtext_drucken_AB" + i + ","
						+ "@Langtext_drucken_BW" + i + ","
						+ "@Lieferzeit" + i + ","
						+ "@Losgroesse" + i + ","
						+ "@Materialkosten_Alt" + i + ","
						+ "@MHD" + i + ","
						+ "@MineralsConfirmity" + i + ","
						+ "@Praeferenz_Aktuelles_jahr" + i + ","
						+ "@Praeferenz_Folgejahr" + i + ","
						+ "@Preiseinheit" + i + ","
						+ "@proZeiteinheit" + i + ","
						+ "@Produktionszeit" + i + ","
						+ "@Provisionsartikel" + i + ","
						+ "@PrüfstatusTNWare" + i + ","
						+ "@Rabattierfähig" + i + ","
						+ "@Rahmen" + i + ","
						+ "@Rahmen2" + i + ","
						+ "@Rahmenauslauf" + i + ","
						+ "@Rahmenauslauf2" + i + ","
						+ "@Rahmenmenge" + i + ","
						+ "@Rahmenmenge2" + i + ","
						+ "@RahmenNr" + i + ","
						+ "@RahmenNr2" + i + ","
						+ "@REACHSVHCConfirmity" + i + ","
						+ "@ROHSEEEConfirmity" + i + ","
						+ "@Seriennummer" + i + ","
						+ "@Seriennummernverwaltung" + i + ","
						+ "@Sonderrabatt" + i + ","
						+ "@Standard_Lagerort_id" + i + ","
						+ "@Stückliste" + i + ","
						+ "@Stundensatz" + i + ","
						+ "@Sysmonummer" + i + ","
						+ "@ULEtikett" + i + ","
						+ "@ULzertifiziert" + i + ","
						+ "@Umsatzsteuer" + i + ","
						+ "@Ursprungsland" + i + ","
						+ "@Verpackung" + i + ","
						+ "@Verpackungsart" + i + ","
						+ "@Verpackungsmenge" + i + ","
						+ "@VKFestpreis" + i + ","
						+ "@Volumen" + i + ","
						+ "@Warengruppe" + i + ","
						+ "@Warentyp" + i + ","
						+ "@Webshop" + i + ","
						+ "@Werkzeug" + i + ","
						+ "@Wert_Anfangsbestand" + i + ","
						+ "@Zeichnungsnummer" + i + ","
						+ "@Zeitraum_MHD" + i + ","
						+ "@Zolltarif_nr" + i + ","
						+ "@VDA_1" + i + ","
						+ "@VDA_2" + i + ","
						+ "@Zuschlag_VK" + i + ","
						+ "@artikelklassifizierung" + i + ","
						+ "@UBG" + i + ","
						+ "@EdiDefault" + i + ","
						+ "@IsEDrawing" + i
						 + "), ";

					sqlCommand.Parameters.AddWithValue("Abladestelle" + i, element.Abladestelle == null ? (object)DBNull.Value : element.Abladestelle);
					sqlCommand.Parameters.AddWithValue("aktiv" + i, element.aktiv == null ? (object)DBNull.Value : element.aktiv);
					sqlCommand.Parameters.AddWithValue("aktualisiert" + i, element.aktualisiert == null ? (object)DBNull.Value : element.aktualisiert);
					sqlCommand.Parameters.AddWithValue("Anfangsbestand" + i, element.Anfangsbestand == null ? (object)DBNull.Value : element.Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("ArtikelAusEigenerProduktion" + i, element.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : element.ArtikelAusEigenerProduktion);
					sqlCommand.Parameters.AddWithValue("ArtikelFürWeitereBestellungenSperren" + i, element.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : element.ArtikelFürWeitereBestellungenSperren);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, element.Artikelfamilie_Kunde == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, element.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail1);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, element.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : element.Artikelfamilie_Kunde_Detail2);
					sqlCommand.Parameters.AddWithValue("Artikelkurztext" + i, element.Artikelkurztext == null ? (object)DBNull.Value : element.Artikelkurztext);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, element.ArtikelNummer == null ? (object)DBNull.Value : element.ArtikelNummer);
					sqlCommand.Parameters.AddWithValue("Barverkauf" + i, element.Barverkauf == null ? (object)DBNull.Value : element.Barverkauf);
					sqlCommand.Parameters.AddWithValue("Bezeichnung1" + i, element.Bezeichnung1 == null ? (object)DBNull.Value : element.Bezeichnung1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung2" + i, element.Bezeichnung2 == null ? (object)DBNull.Value : element.Bezeichnung2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung3" + i, element.Bezeichnung3 == null ? (object)DBNull.Value : element.Bezeichnung3);
					sqlCommand.Parameters.AddWithValue("BezeichnungAL" + i, element.BezeichnungAL == null ? (object)DBNull.Value : element.BezeichnungAL);
					sqlCommand.Parameters.AddWithValue("COF_Pflichtig" + i, element.COF_Pflichtig == null ? (object)DBNull.Value : element.COF_Pflichtig);
					sqlCommand.Parameters.AddWithValue("Crossreferenz" + i, element.Crossreferenz == null ? (object)DBNull.Value : element.Crossreferenz);
					sqlCommand.Parameters.AddWithValue("CuGewicht" + i, element.CuGewicht == null ? (object)DBNull.Value : element.CuGewicht);
					sqlCommand.Parameters.AddWithValue("DatumAnfangsbestand" + i, element.DatumAnfangsbestand == null ? (object)DBNull.Value : element.DatumAnfangsbestand);
					sqlCommand.Parameters.AddWithValue("DEL" + i, element.DEL == null ? (object)DBNull.Value : element.DEL);
					sqlCommand.Parameters.AddWithValue("DELFixiert" + i, element.DELFixiert == null ? (object)DBNull.Value : element.DELFixiert);
					sqlCommand.Parameters.AddWithValue("Dokumente" + i, element.Dokumente == null ? (object)DBNull.Value : element.Dokumente);
					sqlCommand.Parameters.AddWithValue("EAN" + i, element.EAN == null ? (object)DBNull.Value : element.EAN);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, element.Einheit == null ? (object)DBNull.Value : element.Einheit);
					sqlCommand.Parameters.AddWithValue("EMPB" + i, element.EMPB == null ? (object)DBNull.Value : element.EMPB);
					sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben" + i, element.EMPB_Freigegeben == null ? (object)DBNull.Value : element.EMPB_Freigegeben);
					sqlCommand.Parameters.AddWithValue("Ersatzartikel" + i, element.Ersatzartikel == null ? (object)DBNull.Value : element.Ersatzartikel);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz" + i, element.ESD_Schutz == null ? (object)DBNull.Value : element.ESD_Schutz);
					sqlCommand.Parameters.AddWithValue("Exportgewicht" + i, element.Exportgewicht == null ? (object)DBNull.Value : element.Exportgewicht);
					sqlCommand.Parameters.AddWithValue("fakturierenStückliste" + i, element.fakturierenStückliste == null ? (object)DBNull.Value : element.fakturierenStückliste);
					sqlCommand.Parameters.AddWithValue("Farbe" + i, element.Farbe == null ? (object)DBNull.Value : element.Farbe);
					sqlCommand.Parameters.AddWithValue("fibu_rahmen" + i, element.fibu_rahmen == null ? (object)DBNull.Value : element.fibu_rahmen);
					sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, element.Freigabestatus == null ? (object)DBNull.Value : element.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("FreigabestatusTNIntern" + i, element.FreigabestatusTNIntern == null ? (object)DBNull.Value : element.FreigabestatusTNIntern);
					sqlCommand.Parameters.AddWithValue("Gebinde" + i, element.Gebinde == null ? (object)DBNull.Value : element.Gebinde);
					sqlCommand.Parameters.AddWithValue("Gewicht" + i, element.Gewicht == null ? (object)DBNull.Value : element.Gewicht);
					sqlCommand.Parameters.AddWithValue("Größe" + i, element.Größe == null ? (object)DBNull.Value : element.Größe);
					sqlCommand.Parameters.AddWithValue("GrundFürSperre" + i, element.GrundFürSperre == null ? (object)DBNull.Value : element.GrundFürSperre);
					sqlCommand.Parameters.AddWithValue("gültigBis" + i, element.gültigBis == null ? (object)DBNull.Value : element.gültigBis);
					sqlCommand.Parameters.AddWithValue("Halle" + i, element.Halle == null ? (object)DBNull.Value : element.Halle);
					sqlCommand.Parameters.AddWithValue("Hubmastleitungen" + i, element.Hubmastleitungen == null ? (object)DBNull.Value : element.Hubmastleitungen);
					sqlCommand.Parameters.AddWithValue("ID_Klassifizierung" + i, element.ID_Klassifizierung == null ? (object)DBNull.Value : element.ID_Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, element.Index_Kunde == null ? (object)DBNull.Value : element.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, element.Index_Kunde_Datum == null ? (object)DBNull.Value : element.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("Info_WE" + i, element.Info_WE == null ? (object)DBNull.Value : element.Info_WE);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, element.Kanban == null ? (object)DBNull.Value : element.Kanban);
					sqlCommand.Parameters.AddWithValue("Kategorie" + i, element.Kategorie == null ? (object)DBNull.Value : element.Kategorie);
					sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, element.Klassifizierung == null ? (object)DBNull.Value : element.Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Kriterium1" + i, element.Kriterium1 == null ? (object)DBNull.Value : element.Kriterium1);
					sqlCommand.Parameters.AddWithValue("Kriterium2" + i, element.Kriterium2 == null ? (object)DBNull.Value : element.Kriterium2);
					sqlCommand.Parameters.AddWithValue("Kriterium3" + i, element.Kriterium3 == null ? (object)DBNull.Value : element.Kriterium3);
					sqlCommand.Parameters.AddWithValue("Kriterium4" + i, element.Kriterium4 == null ? (object)DBNull.Value : element.Kriterium4);
					sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, element.Kupferbasis == null ? (object)DBNull.Value : element.Kupferbasis);
					sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, element.Kupferzahl == null ? (object)DBNull.Value : element.Kupferzahl);
					sqlCommand.Parameters.AddWithValue("Lagerartikel" + i, element.Lagerartikel == null ? (object)DBNull.Value : element.Lagerartikel);
					sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten" + i, element.Lagerhaltungskosten == null ? (object)DBNull.Value : element.Lagerhaltungskosten);
					sqlCommand.Parameters.AddWithValue("Langtext" + i, element.Langtext == null ? (object)DBNull.Value : element.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB" + i, element.Langtext_drucken_AB == null ? (object)DBNull.Value : element.Langtext_drucken_AB);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW" + i, element.Langtext_drucken_BW == null ? (object)DBNull.Value : element.Langtext_drucken_BW);
					sqlCommand.Parameters.AddWithValue("Lieferzeit" + i, element.Lieferzeit == null ? (object)DBNull.Value : element.Lieferzeit);
					sqlCommand.Parameters.AddWithValue("Losgroesse" + i, element.Losgroesse == null ? (object)DBNull.Value : element.Losgroesse);
					sqlCommand.Parameters.AddWithValue("Materialkosten_Alt" + i, element.Materialkosten_Alt == null ? (object)DBNull.Value : element.Materialkosten_Alt);
					sqlCommand.Parameters.AddWithValue("MHD" + i, element.MHD == null ? (object)DBNull.Value : element.MHD);
					sqlCommand.Parameters.AddWithValue("MineralsConfirmity" + i, element.MineralsConfirmity == null ? (object)DBNull.Value : element.MineralsConfirmity);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr" + i, element.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : element.Praeferenz_Aktuelles_jahr);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr" + i, element.Praeferenz_Folgejahr == null ? (object)DBNull.Value : element.Praeferenz_Folgejahr);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, element.Preiseinheit == null ? (object)DBNull.Value : element.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("proZeiteinheit" + i, element.proZeiteinheit == null ? (object)DBNull.Value : element.proZeiteinheit);
					sqlCommand.Parameters.AddWithValue("Produktionszeit" + i, element.Produktionszeit == null ? (object)DBNull.Value : element.Produktionszeit);
					sqlCommand.Parameters.AddWithValue("Provisionsartikel" + i, element.Provisionsartikel == null ? (object)DBNull.Value : element.Provisionsartikel);
					sqlCommand.Parameters.AddWithValue("PrüfstatusTNWare" + i, element.PrufstatusTNWare == null ? (object)DBNull.Value : element.PrufstatusTNWare);
					sqlCommand.Parameters.AddWithValue("Rabattierfähig" + i, element.Rabattierfähig == null ? (object)DBNull.Value : element.Rabattierfähig);
					sqlCommand.Parameters.AddWithValue("Rahmen" + i, element.Rahmen == null ? (object)DBNull.Value : element.Rahmen);
					sqlCommand.Parameters.AddWithValue("Rahmen2" + i, element.Rahmen2 == null ? (object)DBNull.Value : element.Rahmen2);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf" + i, element.Rahmenauslauf == null ? (object)DBNull.Value : element.Rahmenauslauf);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf2" + i, element.Rahmenauslauf2 == null ? (object)DBNull.Value : element.Rahmenauslauf2);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge" + i, element.Rahmenmenge == null ? (object)DBNull.Value : element.Rahmenmenge);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge2" + i, element.Rahmenmenge2 == null ? (object)DBNull.Value : element.Rahmenmenge2);
					sqlCommand.Parameters.AddWithValue("RahmenNr" + i, element.RahmenNr == null ? (object)DBNull.Value : element.RahmenNr);
					sqlCommand.Parameters.AddWithValue("RahmenNr2" + i, element.RahmenNr2 == null ? (object)DBNull.Value : element.RahmenNr2);
					sqlCommand.Parameters.AddWithValue("REACHSVHCConfirmity" + i, element.REACHSVHCConfirmity == null ? (object)DBNull.Value : element.REACHSVHCConfirmity);
					sqlCommand.Parameters.AddWithValue("ROHSEEEConfirmity" + i, element.ROHSEEEConfirmity == null ? (object)DBNull.Value : element.ROHSEEEConfirmity);
					sqlCommand.Parameters.AddWithValue("Seriennummer" + i, element.Seriennummer == null ? (object)DBNull.Value : element.Seriennummer);
					sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung" + i, element.Seriennummernverwaltung == null ? (object)DBNull.Value : element.Seriennummernverwaltung);
					sqlCommand.Parameters.AddWithValue("Sonderrabatt" + i, element.Sonderrabatt == null ? (object)DBNull.Value : element.Sonderrabatt);
					sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id" + i, element.Standard_Lagerort_id == null ? (object)DBNull.Value : element.Standard_Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Stückliste" + i, element.Stuckliste == null ? (object)DBNull.Value : element.Stuckliste);
					sqlCommand.Parameters.AddWithValue("Stundensatz" + i, element.Stundensatz == null ? (object)DBNull.Value : element.Stundensatz);
					sqlCommand.Parameters.AddWithValue("Sysmonummer" + i, element.Sysmonummer == null ? (object)DBNull.Value : element.Sysmonummer);
					sqlCommand.Parameters.AddWithValue("ULEtikett" + i, element.ULEtikett == null ? (object)DBNull.Value : element.ULEtikett);
					sqlCommand.Parameters.AddWithValue("ULzertifiziert" + i, element.ULzertifiziert == null ? (object)DBNull.Value : element.ULzertifiziert);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, element.Umsatzsteuer == null ? (object)DBNull.Value : element.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Ursprungsland" + i, element.Ursprungsland == null ? (object)DBNull.Value : element.Ursprungsland);
					sqlCommand.Parameters.AddWithValue("Verpackung" + i, element.Verpackung == null ? (object)DBNull.Value : element.Verpackung);
					sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, element.Verpackungsart == null ? (object)DBNull.Value : element.Verpackungsart);
					sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, element.Verpackungsmenge == null ? (object)DBNull.Value : element.Verpackungsmenge);
					sqlCommand.Parameters.AddWithValue("VKFestpreis" + i, element.VKFestpreis == null ? (object)DBNull.Value : element.VKFestpreis);
					sqlCommand.Parameters.AddWithValue("Volumen" + i, element.Volumen == null ? (object)DBNull.Value : element.Volumen);
					sqlCommand.Parameters.AddWithValue("Warengruppe" + i, element.Warengruppe == null ? (object)DBNull.Value : element.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Warentyp" + i, element.Warentyp == null ? (object)DBNull.Value : element.Warentyp);
					sqlCommand.Parameters.AddWithValue("Webshop" + i, element.Webshop == null ? (object)DBNull.Value : element.Webshop);
					sqlCommand.Parameters.AddWithValue("Werkzeug" + i, element.Werkzeug == null ? (object)DBNull.Value : element.Werkzeug);
					sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand" + i, element.Wert_Anfangsbestand == null ? (object)DBNull.Value : element.Wert_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, element.Zeichnungsnummer == null ? (object)DBNull.Value : element.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zeitraum_MHD" + i, element.Zeitraum_MHD == null ? (object)DBNull.Value : element.Zeitraum_MHD);
					sqlCommand.Parameters.AddWithValue("Zolltarif_nr" + i, element.Zolltarif_nr == null ? (object)DBNull.Value : element.Zolltarif_nr);
					sqlCommand.Parameters.AddWithValue("VDA_1" + i, element.VDA_1 == null ? (object)DBNull.Value : element.VDA_1);
					sqlCommand.Parameters.AddWithValue("VDA_2" + i, element.VDA_2 == null ? (object)DBNull.Value : element.VDA_2);
					sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, element.Zuschlag_VK == null ? (object)DBNull.Value : element.Zuschlag_VK);
					sqlCommand.Parameters.AddWithValue("artikelklassifizierung" + i, element.artikelklassifizierung == null ? (object)DBNull.Value : element.artikelklassifizierung);
					sqlCommand.Parameters.AddWithValue("UBG" + i, element.UBG);
					sqlCommand.Parameters.AddWithValue("EdiDefault" + i, element.EdiDefault == null ? (object)DBNull.Value : element.EdiDefault);
					sqlCommand.Parameters.AddWithValue("IsEDrawing" + i, element.IsEDrawing == null ? (object)DBNull.Value : element.IsEDrawing);
				}

				query = query.TrimEnd(',');
				query += ';';
				sqlCommand.CommandText = query;

				response = sqlCommand.ExecuteNonQuery();
				//}

				return response;
			}

			return -1;
		}

		public static int EditWithTransaction(List<Entities.Tables.PRS.ArtikelEntity> elements, SqlConnection connection, SqlTransaction transaction)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 109; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = editWithTransaction(elements, connection, transaction);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += editWithTransaction(elements.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					result += editWithTransaction(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}

			return -1;
		}
		private static int editWithTransaction(List<Entities.Tables.PRS.ArtikelEntity> elements, SqlConnection connection, SqlTransaction transaction)
		{
			if(elements != null && elements.Count > 0)
			{
				int r = -1;
				//using (var connection = new SqlConnection(Settings.ConnectionString))
				//{
				//    cnn.Open();
				string query = "";
				SqlCommand cmd = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(Entities.Tables.PRS.ArtikelEntity t in elements)
				{
					i++;
					query += " UPDATE [Artikel] SET "

						+ "[Abladestelle]=@Abladestelle" + i + ","
						+ "[aktiv]=@aktiv" + i + ","
						+ "[aktualisiert]=@aktualisiert" + i + ","
						+ "[Anfangsbestand]=@Anfangsbestand" + i + ","
						+ "[Artikel aus eigener Produktion]=@ArtikelAusEigenerProduktion" + i + ","
						+ "[Artikel für weitere Bestellungen sperren]=@ArtikelFürWeitereBestellungenSperren" + i + ","
						+ "[Artikelfamilie_Kunde]=@Artikelfamilie_Kunde" + i + ","
						+ "[Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1" + i + ","
						+ "[Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2" + i + ","
						+ "[Artikelkurztext]=@Artikelkurztext" + i + ","
						+ "[Artikelnummer]=@Artikelnummer" + i + ","
						+ "[Barverkauf]=@Barverkauf" + i + ","
						+ "[Bezeichnung 1]=@Bezeichnung1" + i + ","
						+ "[Bezeichnung 2]=@Bezeichnung2" + i + ","
						+ "[Bezeichnung 3]=@Bezeichnung3" + i + ","
						+ "[BezeichnungAL]=@BezeichnungAL" + i + ","
						+ "[COF_Pflichtig]=@COF_Pflichtig" + i + ","
						+ "[Crossreferenz]=@Crossreferenz" + i + ","
						+ "[Cu-Gewicht]=@CuGewicht" + i + ","
						+ "[Datum Anfangsbestand]=@DatumAnfangsbestand" + i + ","
						+ "[DEL]=@DEL" + i + ","
						+ "[DEL fixiert]=@DELFixiert" + i + ","
						+ "[Dokumente]=@Dokumente" + i + ","
						+ "[EAN]=@EAN" + i + ","
						+ "[Einheit]=@Einheit" + i + ","
						+ "[EMPB]=@EMPB" + i + ","
						+ "[EMPB_Freigegeben]=@EMPB_Freigegeben" + i + ","
						+ "[Ersatzartikel]=@Ersatzartikel" + i + ","
						+ "[ESD_Schutz]=@ESD_Schutz" + i + ","
						+ "[Exportgewicht]=@Exportgewicht" + i + ","
						+ "[fakturieren Stückliste]=@fakturierenStückliste" + i + ","
						+ "[Farbe]=@Farbe" + i + ","
						+ "[fibu_rahmen]=@fibu_rahmen" + i + ","
						+ "[Freigabestatus]=@Freigabestatus" + i + ","
						+ "[Freigabestatus TN intern]=@FreigabestatusTNintern" + i + ","
						+ "[Gebinde]=@Gebinde" + i + ","
						+ "[Gewicht]=@Gewicht" + i + ","
						+ "[Größe]=@Größe" + i + ","
						+ "[Grund für Sperre]=@GrundFürSperre" + i + ","
						+ "[gültig bis]=@gültigBis" + i + ","
						+ "[Halle]=@Halle" + i + ","
						+ "[Hubmastleitungen]=@Hubmastleitungen" + i + ","
						+ "[ID_Klassifizierung]=@ID_Klassifizierung" + i + ","
						+ "[Index_Kunde]=@Index_Kunde" + i + ","
						+ "[Index_Kunde_Datum]=@Index_Kunde_Datum" + i + ","
						+ "[Info_WE]=@Info_WE" + i + ","
						+ "[Kanban]=@Kanban" + i + ","
						+ "[Kategorie]=@Kategorie" + i + ","
						+ "[Klassifizierung]=@Klassifizierung" + i + ","
						+ "[Kriterium1]=@Kriterium1" + i + ","
						+ "[Kriterium2]=@Kriterium2" + i + ","
						+ "[Kriterium3]=@Kriterium3" + i + ","
						+ "[Kriterium4]=@Kriterium4" + i + ","
						+ "[Kupferbasis]=@Kupferbasis" + i + ","
						+ "[Kupferzahl]=@Kupferzahl" + i + ","
						+ "[Lagerartikel]=@Lagerartikel" + i + ","
						+ "[Lagerhaltungskosten]=@Lagerhaltungskosten" + i + ","
						+ "[Langtext]=@Langtext" + i + ","
						+ "[Langtext_drucken_AB]=@Langtext_drucken_AB" + i + ","
						+ "[Langtext_drucken_BW]=@Langtext_drucken_BW" + i + ","
						+ "[Lieferzeit]=@Lieferzeit" + i + ","
						+ "[Losgroesse]=@Losgroesse" + i + ","
						+ "[Materialkosten_Alt]=@Materialkosten_Alt" + i + ","
						+ "[MHD]=@MHD" + i + ","
						+ "[Minerals Confirmity]=@MineralsConfirmity" + i + ","
						+ "[Praeferenz_Aktuelles_jahr]=@Praeferenz_Aktuelles_jahr" + i + ","
						+ "[Praeferenz_Folgejahr]=@Praeferenz_Folgejahr" + i + ","
						+ "[Preiseinheit]=@Preiseinheit" + i + ","
						+ "[pro Zeiteinheit]=@proZeiteinheit" + i + ","
						+ "[Produktionszeit]=@Produktionszeit" + i + ","
						+ "[Provisionsartikel]=@Provisionsartikel" + i + ","
						+ "[Prüfstatus TN Ware]=@PrüfstatusTNWare" + i + ","
						+ "[Rabattierfähig]=@Rabattierfähig" + i + ","
						+ "[Rahmen]=@Rahmen" + i + ","
						+ "[Rahmen2]=@Rahmen2" + i + ","
						+ "[Rahmenauslauf]=@Rahmenauslauf" + i + ","
						+ "[Rahmenauslauf2]=@Rahmenauslauf2" + i + ","
						+ "[Rahmenmenge]=@Rahmenmenge" + i + ","
						+ "[Rahmenmenge2]=@Rahmenmenge2" + i + ","
						+ "[Rahmen-Nr]=@RahmenNr" + i + ","
						+ "[Rahmen-Nr2]=@RahmenNr2" + i + ","
						+ "[REACH SVHC Confirmity]=@REACHSVHCConfirmity" + i + ","
						+ "[ROHS EEE Confirmity]=@ROHSEEEConfirmity" + i + ","
						+ "[Seriennummer]=@Seriennummer" + i + ","
						+ "[Seriennummernverwaltung]=@Seriennummernverwaltung" + i + ","
						+ "[Sonderrabatt]=@Sonderrabatt" + i + ","
						+ "[Standard_Lagerort_id]=@Standard_Lagerort_id" + i + ","
						+ "[Stückliste]=@Stückliste" + i + ","
						+ "[Stundensatz]=@Stundensatz" + i + ","
						+ "[Sysmonummer]=@Sysmonummer" + i + ","
						+ "[UL Etikett]=@ULEtikett" + i + ","
						+ "[UL zertifiziert]=@ULzertifiziert" + i + ","
						+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
						+ "[Ursprungsland]=@Ursprungsland" + i + ","
						+ "[Verpackung]=@Verpackung" + i + ","
						+ "[Verpackungsart]=@Verpackungsart" + i + ","
						+ "[Verpackungsmenge]=@Verpackungsmenge" + i + ","
						+ "[VK-Festpreis]=@VKFestpreis" + i + ","
						+ "[Volumen]=@Volumen" + i + ","
						+ "[Warengruppe]=@Warengruppe" + i + ","
						+ "[Warentyp]=@Warentyp" + i + ","
						+ "[Webshop]=@Webshop" + i + ","
						+ "[Werkzeug]=@Werkzeug" + i + ","
						+ "[Wert_Anfangsbestand]=@Wert_Anfangsbestand" + i + ","
						+ "[Zeichnungsnummer]=@Zeichnungsnummer" + i + ","
						+ "[Zeitraum_MHD]=@Zeitraum_MHD" + i + ","
						+ "[VDA_1]=@VDA_1" + i + ","
						+ "[VDA_2]=@VDA_2" + i + ","
						+ "[Zuschlag_VK]=@Zuschlag_VK" + i + ","
						+ "[artikelklassifizierung]=@artikelklassifizierung" + i + ","
						+ "[UBG]=@UBG" + i + ","
						+ "[EdiDefault]=@EdiDefault" + i + ","
						+ "[IsEDrawing]=@IsEDrawing" + i + ","
						+ "[Zolltarif_nr]=@Zolltarif_nr" + i + " WHERE [Artikel-Nr]=@ArtikelNr" + i
						 + "; ";

					cmd.Parameters.AddWithValue("ArtikelNr" + i, t.ArtikelNr);
					cmd.Parameters.AddWithValue("Abladestelle" + i, t.Abladestelle == null ? (object)DBNull.Value : t.Abladestelle);
					cmd.Parameters.AddWithValue("aktiv" + i, t.aktiv == null ? (object)DBNull.Value : t.aktiv);
					cmd.Parameters.AddWithValue("aktualisiert" + i, t.aktualisiert == null ? (object)DBNull.Value : t.aktualisiert);
					cmd.Parameters.AddWithValue("Anfangsbestand" + i, t.Anfangsbestand == null ? (object)DBNull.Value : t.Anfangsbestand);
					cmd.Parameters.AddWithValue("ArtikelAusEigenerProduktion" + i, t.ArtikelAusEigenerProduktion == null ? (object)DBNull.Value : t.ArtikelAusEigenerProduktion);
					cmd.Parameters.AddWithValue("ArtikelFürWeitereBestellungenSperren" + i, t.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : t.ArtikelFürWeitereBestellungenSperren);
					cmd.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, t.Artikelfamilie_Kunde == null ? (object)DBNull.Value : t.Artikelfamilie_Kunde);
					cmd.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, t.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : t.Artikelfamilie_Kunde_Detail1);
					cmd.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, t.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : t.Artikelfamilie_Kunde_Detail2);
					cmd.Parameters.AddWithValue("Artikelkurztext" + i, t.Artikelkurztext == null ? (object)DBNull.Value : t.Artikelkurztext);
					cmd.Parameters.AddWithValue("Artikelnummer" + i, t.ArtikelNummer == null ? (object)DBNull.Value : t.ArtikelNummer);
					cmd.Parameters.AddWithValue("Barverkauf" + i, t.Barverkauf == null ? (object)DBNull.Value : t.Barverkauf);
					cmd.Parameters.AddWithValue("Bezeichnung1" + i, t.Bezeichnung1 == null ? (object)DBNull.Value : t.Bezeichnung1);
					cmd.Parameters.AddWithValue("Bezeichnung2" + i, t.Bezeichnung2 == null ? (object)DBNull.Value : t.Bezeichnung2);
					cmd.Parameters.AddWithValue("Bezeichnung3" + i, t.Bezeichnung3 == null ? (object)DBNull.Value : t.Bezeichnung3);
					cmd.Parameters.AddWithValue("BezeichnungAL" + i, t.BezeichnungAL == null ? (object)DBNull.Value : t.BezeichnungAL);
					cmd.Parameters.AddWithValue("COF_Pflichtig" + i, t.COF_Pflichtig == null ? (object)DBNull.Value : t.COF_Pflichtig);
					cmd.Parameters.AddWithValue("Crossreferenz" + i, t.Crossreferenz == null ? (object)DBNull.Value : t.Crossreferenz);
					cmd.Parameters.AddWithValue("CuGewicht" + i, t.CuGewicht == null ? (object)DBNull.Value : t.CuGewicht);
					cmd.Parameters.AddWithValue("DatumAnfangsbestand" + i, t.DatumAnfangsbestand == null ? (object)DBNull.Value : t.DatumAnfangsbestand);
					cmd.Parameters.AddWithValue("DEL" + i, t.DEL == null ? (object)DBNull.Value : t.DEL);
					cmd.Parameters.AddWithValue("DELFixiert" + i, t.DELFixiert == null ? (object)DBNull.Value : t.DELFixiert);
					cmd.Parameters.AddWithValue("Dokumente" + i, t.Dokumente == null ? (object)DBNull.Value : t.Dokumente);
					cmd.Parameters.AddWithValue("EAN" + i, t.EAN == null ? (object)DBNull.Value : t.EAN);
					cmd.Parameters.AddWithValue("Einheit" + i, t.Einheit == null ? (object)DBNull.Value : t.Einheit);
					cmd.Parameters.AddWithValue("EMPB" + i, t.EMPB == null ? (object)DBNull.Value : t.EMPB);
					cmd.Parameters.AddWithValue("EMPB_Freigegeben" + i, t.EMPB_Freigegeben == null ? (object)DBNull.Value : t.EMPB_Freigegeben);
					cmd.Parameters.AddWithValue("Ersatzartikel" + i, t.Ersatzartikel == null ? (object)DBNull.Value : t.Ersatzartikel);
					cmd.Parameters.AddWithValue("ESD_Schutz" + i, t.ESD_Schutz == null ? (object)DBNull.Value : t.ESD_Schutz);
					cmd.Parameters.AddWithValue("Exportgewicht" + i, t.Exportgewicht == null ? (object)DBNull.Value : t.Exportgewicht);
					cmd.Parameters.AddWithValue("ArtikelFürWeitereBestellungenSperren" + i, t.ArtikelFürWeitereBestellungenSperren == null ? (object)DBNull.Value : t.ArtikelFürWeitereBestellungenSperren);
					cmd.Parameters.AddWithValue("fakturierenStückliste" + i, t.fakturierenStückliste == null ? (object)DBNull.Value : t.fakturierenStückliste);
					cmd.Parameters.AddWithValue("Farbe" + i, t.Farbe == null ? (object)DBNull.Value : t.Farbe);
					cmd.Parameters.AddWithValue("fibu_rahmen" + i, t.fibu_rahmen == null ? (object)DBNull.Value : t.fibu_rahmen);
					cmd.Parameters.AddWithValue("Freigabestatus" + i, t.Freigabestatus == null ? (object)DBNull.Value : t.Freigabestatus);
					cmd.Parameters.AddWithValue("FreigabestatusTNIntern" + i, t.FreigabestatusTNIntern == null ? (object)DBNull.Value : t.FreigabestatusTNIntern);
					cmd.Parameters.AddWithValue("Gebinde" + i, t.Gebinde == null ? (object)DBNull.Value : t.Gebinde);
					cmd.Parameters.AddWithValue("Gewicht" + i, t.Gewicht == null ? (object)DBNull.Value : t.Gewicht);
					cmd.Parameters.AddWithValue("Größe" + i, t.Größe == null ? (object)DBNull.Value : t.Größe);
					cmd.Parameters.AddWithValue("GrundFürSperre" + i, t.GrundFürSperre == null ? (object)DBNull.Value : t.GrundFürSperre);
					cmd.Parameters.AddWithValue("gültigBis" + i, t.gültigBis == null ? (object)DBNull.Value : t.gültigBis);
					cmd.Parameters.AddWithValue("Halle" + i, t.Halle == null ? (object)DBNull.Value : t.Halle);
					cmd.Parameters.AddWithValue("Hubmastleitungen" + i, t.Hubmastleitungen == null ? (object)DBNull.Value : t.Hubmastleitungen);
					cmd.Parameters.AddWithValue("ID_Klassifizierung" + i, t.ID_Klassifizierung == null ? (object)DBNull.Value : t.ID_Klassifizierung);
					cmd.Parameters.AddWithValue("Index_Kunde" + i, t.Index_Kunde == null ? (object)DBNull.Value : t.Index_Kunde);
					cmd.Parameters.AddWithValue("Index_Kunde_Datum" + i, t.Index_Kunde_Datum == null ? (object)DBNull.Value : t.Index_Kunde_Datum);
					cmd.Parameters.AddWithValue("Info_WE" + i, t.Info_WE == null ? (object)DBNull.Value : t.Info_WE);
					cmd.Parameters.AddWithValue("Kanban" + i, t.Kanban == null ? (object)DBNull.Value : t.Kanban);
					cmd.Parameters.AddWithValue("Kategorie" + i, t.Kategorie == null ? (object)DBNull.Value : t.Kategorie);
					cmd.Parameters.AddWithValue("Klassifizierung" + i, t.Klassifizierung == null ? (object)DBNull.Value : t.Klassifizierung);
					cmd.Parameters.AddWithValue("Kriterium1" + i, t.Kriterium1 == null ? (object)DBNull.Value : t.Kriterium1);
					cmd.Parameters.AddWithValue("Kriterium2" + i, t.Kriterium2 == null ? (object)DBNull.Value : t.Kriterium2);
					cmd.Parameters.AddWithValue("Kriterium3" + i, t.Kriterium3 == null ? (object)DBNull.Value : t.Kriterium3);
					cmd.Parameters.AddWithValue("Kriterium4" + i, t.Kriterium4 == null ? (object)DBNull.Value : t.Kriterium4);
					cmd.Parameters.AddWithValue("Kupferbasis" + i, t.Kupferbasis == null ? (object)DBNull.Value : t.Kupferbasis);
					cmd.Parameters.AddWithValue("Kupferzahl" + i, t.Kupferzahl == null ? (object)DBNull.Value : t.Kupferzahl);
					cmd.Parameters.AddWithValue("Lagerartikel" + i, t.Lagerartikel == null ? (object)DBNull.Value : t.Lagerartikel);
					cmd.Parameters.AddWithValue("Lagerhaltungskosten" + i, t.Lagerhaltungskosten == null ? (object)DBNull.Value : t.Lagerhaltungskosten);
					cmd.Parameters.AddWithValue("Langtext" + i, t.Langtext == null ? (object)DBNull.Value : t.Langtext);
					cmd.Parameters.AddWithValue("Langtext_drucken_AB" + i, t.Langtext_drucken_AB == null ? (object)DBNull.Value : t.Langtext_drucken_AB);
					cmd.Parameters.AddWithValue("Langtext_drucken_BW" + i, t.Langtext_drucken_BW == null ? (object)DBNull.Value : t.Langtext_drucken_BW);
					cmd.Parameters.AddWithValue("Lieferzeit" + i, t.Lieferzeit == null ? (object)DBNull.Value : t.Lieferzeit);
					cmd.Parameters.AddWithValue("Losgroesse" + i, t.Losgroesse == null ? (object)DBNull.Value : t.Losgroesse);
					cmd.Parameters.AddWithValue("Materialkosten_Alt" + i, t.Materialkosten_Alt == null ? (object)DBNull.Value : t.Materialkosten_Alt);
					cmd.Parameters.AddWithValue("MHD" + i, t.MHD == null ? (object)DBNull.Value : t.MHD);
					cmd.Parameters.AddWithValue("MineralsConfirmity" + i, t.MineralsConfirmity == null ? (object)DBNull.Value : t.MineralsConfirmity);
					cmd.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr" + i, t.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : t.Praeferenz_Aktuelles_jahr);
					cmd.Parameters.AddWithValue("Praeferenz_Folgejahr" + i, t.Praeferenz_Folgejahr == null ? (object)DBNull.Value : t.Praeferenz_Folgejahr);
					cmd.Parameters.AddWithValue("Preiseinheit" + i, t.Preiseinheit == null ? (object)DBNull.Value : t.Preiseinheit);
					cmd.Parameters.AddWithValue("proZeiteinheit" + i, t.proZeiteinheit == null ? (object)DBNull.Value : t.proZeiteinheit);
					cmd.Parameters.AddWithValue("Produktionszeit" + i, t.Produktionszeit == null ? (object)DBNull.Value : t.Produktionszeit);
					cmd.Parameters.AddWithValue("Provisionsartikel" + i, t.Provisionsartikel == null ? (object)DBNull.Value : t.Provisionsartikel);
					cmd.Parameters.AddWithValue("PrüfstatusTNWare" + i, t.PrufstatusTNWare == null ? (object)DBNull.Value : t.PrufstatusTNWare);
					cmd.Parameters.AddWithValue("Rabattierfähig" + i, t.Rabattierfähig == null ? (object)DBNull.Value : t.Rabattierfähig);
					cmd.Parameters.AddWithValue("Rahmen" + i, t.Rahmen == null ? (object)DBNull.Value : t.Rahmen);
					cmd.Parameters.AddWithValue("Rahmen2" + i, t.Rahmen2 == null ? (object)DBNull.Value : t.Rahmen2);
					cmd.Parameters.AddWithValue("Rahmenauslauf" + i, t.Rahmenauslauf == null ? (object)DBNull.Value : t.Rahmenauslauf);
					cmd.Parameters.AddWithValue("Rahmenauslauf2" + i, t.Rahmenauslauf2 == null ? (object)DBNull.Value : t.Rahmenauslauf2);
					cmd.Parameters.AddWithValue("Rahmenmenge" + i, t.Rahmenmenge == null ? (object)DBNull.Value : t.Rahmenmenge);
					cmd.Parameters.AddWithValue("Rahmenmenge2" + i, t.Rahmenmenge2 == null ? (object)DBNull.Value : t.Rahmenmenge2);
					cmd.Parameters.AddWithValue("RahmenNr" + i, t.RahmenNr == null ? (object)DBNull.Value : t.RahmenNr);
					cmd.Parameters.AddWithValue("RahmenNr2" + i, t.RahmenNr2 == null ? (object)DBNull.Value : t.RahmenNr2);
					cmd.Parameters.AddWithValue("REACHSVHCConfirmity" + i, t.REACHSVHCConfirmity == null ? (object)DBNull.Value : t.REACHSVHCConfirmity);
					cmd.Parameters.AddWithValue("ROHSEEEConfirmity" + i, t.ROHSEEEConfirmity == null ? (object)DBNull.Value : t.ROHSEEEConfirmity);
					cmd.Parameters.AddWithValue("Seriennummer" + i, t.Seriennummer == null ? (object)DBNull.Value : t.Seriennummer);
					cmd.Parameters.AddWithValue("Seriennummernverwaltung" + i, t.Seriennummernverwaltung == null ? (object)DBNull.Value : t.Seriennummernverwaltung);
					cmd.Parameters.AddWithValue("Sonderrabatt" + i, t.Sonderrabatt == null ? (object)DBNull.Value : t.Sonderrabatt);
					cmd.Parameters.AddWithValue("Standard_Lagerort_id" + i, t.Standard_Lagerort_id == null ? (object)DBNull.Value : t.Standard_Lagerort_id);
					cmd.Parameters.AddWithValue("Stückliste" + i, t.Stuckliste == null ? (object)DBNull.Value : t.Stuckliste);
					cmd.Parameters.AddWithValue("Stundensatz" + i, t.Stundensatz == null ? (object)DBNull.Value : t.Stundensatz);
					cmd.Parameters.AddWithValue("Sysmonummer" + i, t.Sysmonummer == null ? (object)DBNull.Value : t.Sysmonummer);
					cmd.Parameters.AddWithValue("ULEtikett" + i, t.ULEtikett == null ? (object)DBNull.Value : t.ULEtikett);
					cmd.Parameters.AddWithValue("ULzertifiziert" + i, t.ULzertifiziert == null ? (object)DBNull.Value : t.ULzertifiziert);
					cmd.Parameters.AddWithValue("Umsatzsteuer" + i, t.Umsatzsteuer == null ? (object)DBNull.Value : t.Umsatzsteuer);
					cmd.Parameters.AddWithValue("Ursprungsland" + i, t.Ursprungsland == null ? (object)DBNull.Value : t.Ursprungsland);
					cmd.Parameters.AddWithValue("Verpackung" + i, t.Verpackung == null ? (object)DBNull.Value : t.Verpackung);
					cmd.Parameters.AddWithValue("Verpackungsart" + i, t.Verpackungsart == null ? (object)DBNull.Value : t.Verpackungsart);
					cmd.Parameters.AddWithValue("Verpackungsmenge" + i, t.Verpackungsmenge == null ? (object)DBNull.Value : t.Verpackungsmenge);
					cmd.Parameters.AddWithValue("VKFestpreis" + i, t.VKFestpreis == null ? (object)DBNull.Value : t.VKFestpreis);
					cmd.Parameters.AddWithValue("Volumen" + i, t.Volumen == null ? (object)DBNull.Value : t.Volumen);
					cmd.Parameters.AddWithValue("Warengruppe" + i, t.Warengruppe == null ? (object)DBNull.Value : t.Warengruppe);
					cmd.Parameters.AddWithValue("Warentyp" + i, t.Warentyp == null ? (object)DBNull.Value : t.Warentyp);
					cmd.Parameters.AddWithValue("Webshop" + i, t.Webshop == null ? (object)DBNull.Value : t.Webshop);
					cmd.Parameters.AddWithValue("Werkzeug" + i, t.Werkzeug == null ? (object)DBNull.Value : t.Werkzeug);
					cmd.Parameters.AddWithValue("Wert_Anfangsbestand" + i, t.Wert_Anfangsbestand == null ? (object)DBNull.Value : t.Wert_Anfangsbestand);
					cmd.Parameters.AddWithValue("Zeichnungsnummer" + i, t.Zeichnungsnummer == null ? (object)DBNull.Value : t.Zeichnungsnummer);
					cmd.Parameters.AddWithValue("Zeitraum_MHD" + i, t.Zeitraum_MHD == null ? (object)DBNull.Value : t.Zeitraum_MHD);
					cmd.Parameters.AddWithValue("Zolltarif_nr" + i, t.Zolltarif_nr == null ? (object)DBNull.Value : t.Zolltarif_nr);
					cmd.Parameters.AddWithValue("VDA_1" + i, t.VDA_1 == null ? (object)DBNull.Value : t.VDA_1);
					cmd.Parameters.AddWithValue("VDA_2" + i, t.VDA_2 == null ? (object)DBNull.Value : t.VDA_2);
					cmd.Parameters.AddWithValue("Zuschlag_VK" + i, t.Zuschlag_VK == null ? (object)DBNull.Value : t.Zuschlag_VK);
					cmd.Parameters.AddWithValue("artikelklassifizierung" + i, t.artikelklassifizierung == null ? (object)DBNull.Value : t.artikelklassifizierung);
					cmd.Parameters.AddWithValue("UBG" + i, t.UBG);
					cmd.Parameters.AddWithValue("EdiDefault" + i, t.EdiDefault == null ? (object)DBNull.Value : t.EdiDefault);
					cmd.Parameters.AddWithValue("IsEDrawing" + i, t.IsEDrawing == null ? (object)DBNull.Value : t.IsEDrawing);
				}

				cmd.CommandText = query;

				r = cmd.ExecuteNonQuery();
				//}

				return r;
			}

			return -1;
		}
		#endregion Transactions

		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCountry(string country)
		{
			country = country ?? "";
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel] WHERE TRIM([Ursprungsland])=@country";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("country", country.SqlEscape().Trim());

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByTeam(string team)
		{
			team = team ?? "";
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [Artikel] WHERE TRIM([Artikelkurztext])=@team", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("team", team.SqlEscape().Trim());

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
			}

			return result;
		}

		#region CoC
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCoCVersion(string cocVersion)
		{
			cocVersion = cocVersion ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel] WHERE [CocVersion]=@cocVersion";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("cocVersion", cocVersion);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.PRS.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.PRS.ArtikelEntity>();
			}
		}
		#endregion CoC
		public static int GetArticlesCount(SqlConnection connection, SqlTransaction transaction)
		{
			using(var sqlCommand = new SqlCommand("SELECT COUNT(*) FROM [Artikel];", connection, transaction))
			{
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		public static int GetArticlesWoZollNrCount(SqlConnection connection, SqlTransaction transaction)
		{
			using(var sqlCommand = new SqlCommand("SELECT COUNT(*) FROM [Artikel] WHERE TRIM(ISNULL(Zolltarif_nr,''))='';", connection, transaction))
			{
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		public static int GetArticlesWithZollNrNotInXLSCount(IEnumerable<string> xlsNumbers, string tempId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "";
			if(xlsNumbers == null || xlsNumbers.Count() <= 0)
			{
				query = "SELECT COUNT(*) FROM [Artikel] WHERE TRIM(ISNULL(Zolltarif_nr,''))<>'';";
			}
			else
			{
				query = $@"
							DROP TABLE IF EXISTS [##_LGT_STS_ArticleCustomsChecks];
							CREATE TABLE [##_LGT_STS_ArticleCustomsChecks] (Zollnummer NVARCHAR(50));
							INSERT INTO [##_LGT_STS_ArticleCustomsChecks] (Zollnummer) SELECT '' ";
				// -
				foreach(var item in xlsNumbers)
				{
					query += $"\nUNION ALL SELECT '{item}' ";
				}
				query += $";";

				// -
				query += $"DROP TABLE IF EXISTS tempdb.dbo.LGT_STS_ArticleCustomsChecks_{tempId.Replace('-', '_')};SELECT [Artikel-Nr] INTO tempdb.dbo.LGT_STS_ArticleCustomsChecks_{tempId.Replace('-', '_')} FROM [Artikel] WHERE TRIM(ISNULL(Zolltarif_nr,''))<>'' AND TRIM(LOWER(Zolltarif_nr)) NOT IN (SELECT DISTINCT TRIM(LOWER(Zollnummer)) FROM [##_LGT_STS_ArticleCustomsChecks]);";
				// - 
				query += $"SELECT COUNT(*) FROM Artikel WHERE TRIM(ISNULL(Zolltarif_nr,''))<>'' AND TRIM(LOWER(Zolltarif_nr)) NOT IN (SELECT DISTINCT TRIM(LOWER(Zollnummer)) FROM [##_LGT_STS_ArticleCustomsChecks]);";
			}
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.CommandTimeout = 180;
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetArticlesWithZollNrNotInXLS(IEnumerable<string> xlsNumbers, string tempId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "";
			if(xlsNumbers == null || xlsNumbers.Count() <= 0)
			{
				query = "SELECT COUNT(*) FROM [Artikel] WHERE TRIM(ISNULL(Zolltarif_nr,''))<>'';";
			}
			else
			{
				var tempTable = $"##LGT_STS_ArticleCustomsChecks_{tempId.Replace('-', '_')}";
				query = $@"
							DROP TABLE IF EXISTS [{tempTable}];
							CREATE TABLE [{tempTable}] (Zollnummer NVARCHAR(50));
							INSERT INTO [{tempTable}] (Zollnummer) SELECT '' ";
				// -
				foreach(var item in xlsNumbers)
				{
					query += $"\nUNION ALL SELECT '{item}' ";
				}
				query += $";";

				// - 
				query += $"SELECT * FROM Artikel WHERE TRIM(ISNULL(Zolltarif_nr,''))<>'' AND TRIM(LOWER(Zolltarif_nr)) NOT IN (SELECT DISTINCT TRIM(LOWER(Zollnummer)) FROM [{tempTable}]);";
			}
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.CommandTimeout = 180;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetArticlesWoZollNr()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [Artikel] WHERE TRIM(ISNULL(Zolltarif_nr,''))='';", sqlConnection))
			{
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetArticlesWithZollNrNotInXLS(string tempId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand($"SELECT * FROM [Artikel] WHERE [Artikel-Nr] IN (SELECT [Artikel-Nr] FROM tempdb.dbo.LGT_STS_ArticleCustomsChecks_{tempId.Replace('-', '_')});DROP TABLE IF EXISTS tempdb.dbo.LGT_STS_ArticleCustomsChecks_{tempId.Replace('-', '_')};", sqlConnection))
			{
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			}
		}

		public static bool IsArticleReparatur(int articleNr, List<string> reparaturArticles)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var query = $@"SELECT COUNT([Artikel-Nr]) FROM [artikel] WHERE [Artikel-Nr]={articleNr} AND TRIM(Artikelnummer) IN ('{string.Join("','", reparaturArticles.Select(x => $"{x.Trim()}"))}')";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				return (int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : -1) > 0;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByUoMSymbol(string symbol)
		{
			var response = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @" SELECT * FROM [Artikel] WHERE TRIM([Einheit]) = TRIM(@symbol) order by [Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("symbol", symbol ?? "");

				using(var reader = sqlCommand.ExecuteReader())
				{
					return toList(reader);
				}
			}
		}
		public static int GetDELKunden(int Kundennummer, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $"select k.Del from Kunden k JOIN adressen a on a.nr=k.nummer Where a.Kundennummer={Kundennummer}";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		public static List<Entities.Tables.PRS.PreviousAndNextArtikelEntity> GetPrevArtikel(string Nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT top 1  Artikelnummer,[Artikel-Nr] FROM [Artikel] WHERE Artikelnummer < '{Nr}' order by Artikelnummer desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				//sqlCommand.Parameters.AddWithValue("artiklenummer", $"'{Nr}'");
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.PreviousAndNextArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.PreviousAndNextArtikelEntity>();
				}
			}

		}
		public static List<Entities.Tables.PRS.PreviousAndNextArtikelEntity> GetNextArtikel(string Nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT top 1  Artikelnummer,[Artikel-Nr] FROM [Artikel] WHERE Artikelnummer > '{Nr}' order by Artikelnummer asc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				//sqlCommand.Parameters.AddWithValue("artiklenummer", $"'{Nr}'");
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.PreviousAndNextArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.PreviousAndNextArtikelEntity>();
			}
		}
		public static int UpdateCustomerTechnician(int id, string name, SqlConnection connection, SqlTransaction transaction)
		{
			name = name ?? "";
			string query = $"UPDATE [Artikel] SET [CustomerTechnic]=@name WHERE [Aktiv]=1 AND [CustomerTechnicId]=@id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("name", name);
				sqlCommand.Parameters.AddWithValue("id", id);
				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCustomerTechnicianId(int id)
		{
			var response = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @" SELECT * FROM [Artikel] WHERE [CustomerTechnicId] = @id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				using(var reader = sqlCommand.ExecuteReader())
				{
					return toList(reader);
				}
			}
		}
		public static List<Entities.Tables.PRS.ArtikelEntity> GetByCustomerSaleContact(int id)
		{
			var response = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @" SELECT * FROM [Artikel] WHERE [CustomerTechnicId] = @id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				using(var reader = sqlCommand.ExecuteReader())
				{
					return toList(reader);
				}
			}
		}
		#region Helpers
		private static List<Entities.Tables.PRS.ArtikelEntity> toList(SqlDataReader dataReader)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();
			while(dataReader.Read())
			{
				try
				{
					result.Add(new Entities.Tables.PRS.ArtikelEntity(dataReader));
				} catch(Exception)
				{
					Debug.WriteLine($"error in loading article {dataReader["Artikel-Nr"]}");
				}
			}
			return result;
		}
		private static List<Entities.Tables.PRS.ArtikelNrsOnlyEntity> toNrsList(SqlDataReader dataReader)
		{
			var result = new List<Entities.Tables.PRS.ArtikelNrsOnlyEntity>();
			while(dataReader.Read())
			{
				result.Add(new Entities.Tables.PRS.ArtikelNrsOnlyEntity(dataReader));
			}
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(dataRow)); }
			return list;
		}
		public static int GetCountArticlesByNummerkreis(string expansion)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT COUNT(*) FROM [Artikel] WHERE [Warengruppe]='EF'";
				query += expansion;

				var sqlCommand = new SqlCommand(query, sqlConnection);


				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		public static int ToggleEDrawing(int articleId, SqlConnection connection, SqlTransaction transaction)
		{
			using(var sqlCommand = new SqlCommand($"UPDATE [Artikel] SET [IsEDrawing]=1-ISNULL([IsEDrawing],0) WHERE [Artikel-Nr]={articleId};", connection, transaction))
			{
				return sqlCommand.ExecuteNonQuery();
			}
		}

		#endregion
		public static List<Infrastructure.Data.Access.Tables.BSD.ArtikelCurrentQuantityEntity> GetArticlesActualQuantity(List<int> articlesnr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$" select   [Artikel-Nr] Artikel_Nr,SUM(Bestand) quantity from Lager  where [Artikel-Nr] IN ({string.Join(", ", articlesnr)}) group by [Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Access.Tables.BSD.ArtikelCurrentQuantityEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Access.Tables.BSD.ArtikelCurrentQuantityEntity>();
			}

		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.ROHArticlesUnitsEntity> GetROHArticlesUnits()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "select distinct  Einheit  from Artikel where ISNULL(Einheit,'') != ''  and Warengruppe = 'ROH'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ROHArticlesUnitsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ROHArticlesUnitsEntity>();
			}
		}

		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity GetArtikelByManNr(string artikel_nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT top 1 * FROM [Artikel] WHERE Warengruppe = 'ROH' and  ManufacturerNumber  = '{artikel_nr}'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> SearchArticleByStatus(string artikelnummer, int? maxItemsCount = null)
		{
			artikelnummer = artikelnummer?.Trim();
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{

					sqlCommand.CommandText = $"SELECT top 10 * from [Artikel] WHERE [Artikel-Nr] IS NOT NULL AND [Artikelnummer] like '{artikelnummer.SqlEscape()}%'" +
						$" AND [Warengruppe] = 'EF' AND [Freigabestatus]='P'  ORDER by [Artikelnummer] ASC";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}


			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.PRS.ArtikelEntity> GetByWarengruppe(string artikelnummer)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT Top 10 * FROM [Artikel] WHERE Warengruppe = 'ROH' AND ISNULL(aktiv,0)=1  AND [Artikelnummer] like '{artikelnummer.SqlEscape()}%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
			}

			return result;
		}

		public static List<Entities.Tables.PRS.ArtikelEntity> GetByNrs(List<int> nrs)
		{
			var result = new List<Entities.Tables.PRS.ArtikelEntity>();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT Top 10 * FROM [Artikel] WHERE [Artikel-Nr] IN ({string.Join(",", nrs)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				using(var reader = sqlCommand.ExecuteReader())
				{
					result = toList(reader);
				}
			}

			return result;
		}
		public static List<Entities.Tables.PRS.MinimalArtikelEntity> GetMinimal(List<int> ids, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.MinimalArtikelEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					result = getMinimal(ids, sqlConnection, sqlTransaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.MinimalArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getMinimal(ids.GetRange(i * maxQueryNumber, maxQueryNumber), sqlConnection, sqlTransaction));
					}
					result.AddRange(getMinimal(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), sqlConnection, sqlTransaction));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.MinimalArtikelEntity>();
		}
		private static List<Entities.Tables.PRS.MinimalArtikelEntity> getMinimal(List<int> ids, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlCommand = new SqlCommand("", sqlConnection, sqlTransaction))
				{
					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [Artikel] WHERE [Artikel-Nr] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity>();
				}
			}
			return new List<Entities.Tables.PRS.MinimalArtikelEntity>();
		}
	}
}