using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class AngeboteAccess
	{
		public const string TYP_CONFIRMATION = "Auftragsbest‰tigung";
		public const string TYP_FORECAST = "Bedarfsvorschau";
		public const string TYP_CONTRACT = "Rahmenauftrag";
		public const string TYP_KANBAN = "Kanban";
		public const string TYP_DELIVERY = "Lieferschein";
		public const string TYP_INVOICE = "Rechnung";
		public const string TYP_CREDIT = "Gutschrift";
		public const string TYP_CRPFORECAST = "Forecast";
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Angebote] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Angebote]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					sqlCommand.CommandText = $"SELECT * FROM [Angebote] WHERE [Nr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Angebote] ([ab_id],[ABSENDER],[Abteilung],[Angebot-Nr],[Anrede],[Ansprechpartner],[Auswahl],[Belegkreis],[Bemerkung],[Benutzer],[Bereich],[Bezug],[Briefanrede],[datueber],[Datum],[Debitorennummer],[Dplatz_Sirona],[EDI_Dateiname_CSV],[EDI_Kundenbestellnummer],[EDI_Order_Change],[EDI_Order_Change_Updated],[EDI_Order_Neu],[erledigt],[F‰lligkeit],[Freie_Text],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Interessent],[Konditionen],[Kunden-Nr],[LAbteilung],[Land/PLZ/Ort],[LAnrede],[LAnsprechpartner],[LBriefanrede],[Lieferadresse],[Liefertermin],[LLand/PLZ/Ort],[LName2],[LName3],[Lˆschen],[LsAddressNr],[LsDeliveryDate],[LStraﬂe/Postfach],[LVorname/NameFirma],[Mahnung],[Mandant],[Name2],[Name3],[Neu],[Neu_Order],[nr_ang],[nr_auf],[nr_BV],[nr_dlf],[nr_gut],[nr_Kanban],[nr_lie],[nr_pro],[nr_RA],[nr_rec],[nr_sto],[÷ffnen],[Personal-Nr],[Projekt-Nr],[rec_sent],[reparatur_nr],[Status],[StorageLocation],[Straﬂe/Postfach],[termin_eingehalten],[Typ],[UnloadingPoint],[Unser Zeichen],[USt_Berechnen],[Versandart],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Vorname/NameFirma],[Wunschtermin],[Zahlungsweise],[Zahlungsziel]) OUTPUT INSERTED.[Nr] VALUES (@ab_id,@ABSENDER,@Abteilung,@Angebot_Nr,@Anrede,@Ansprechpartner,@Auswahl,@Belegkreis,@Bemerkung,@Benutzer,@Bereich,@Bezug,@Briefanrede,@datueber,@Datum,@Debitorennummer,@Dplatz_Sirona,@EDI_Dateiname_CSV,@EDI_Kundenbestellnummer,@EDI_Order_Change,@EDI_Order_Change_Updated,@EDI_Order_Neu,@erledigt,@Falligkeit,@Freie_Text,@Freitext,@gebucht,@gedruckt,@Ihr_Zeichen,@In_Bearbeitung,@Interessent,@Konditionen,@Kunden_Nr,@LAbteilung,@Land_PLZ_Ort,@LAnrede,@LAnsprechpartner,@LBriefanrede,@Lieferadresse,@Liefertermin,@LLand_PLZ_Ort,@LName2,@LName3,@Loschen,@LsAddressNr,@LsDeliveryDate,@LStrasse_Postfach,@LVorname_NameFirma,@Mahnung,@Mandant,@Name2,@Name3,@Neu,@Neu_Order,@nr_ang,@nr_auf,@nr_BV,@nr_dlf,@nr_gut,@nr_Kanban,@nr_lie,@nr_pro,@nr_RA,@nr_rec,@nr_sto,@Offnen,@Personal_Nr,@Projekt_Nr,@rec_sent,@reparatur_nr,@Status,@StorageLocation,@Strasse_Postfach,@termin_eingehalten,@Typ,@UnloadingPoint,@Unser_Zeichen,@USt_Berechnen,@Versandart,@Versandarten_Auswahl,@Versanddatum_Auswahl,@Vorname_NameFirma,@Wunschtermin,@Zahlungsweise,@Zahlungsziel); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ab_id", item.Ab_id == null ? (object)DBNull.Value : item.Ab_id);
					sqlCommand.Parameters.AddWithValue("ABSENDER", item.ABSENDER == null ? (object)DBNull.Value : item.ABSENDER);
					sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
					sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
					sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
					sqlCommand.Parameters.AddWithValue("Bereich", item.Bereich == null ? (object)DBNull.Value : item.Bereich);
					sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
					sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("datueber", item.Datueber == null ? (object)DBNull.Value : item.Datueber);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Debitorennummer", item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
					sqlCommand.Parameters.AddWithValue("Dplatz_Sirona", item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
					sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV", item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
					sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer", item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change", item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated", item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Neu", item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
					sqlCommand.Parameters.AddWithValue("erledigt", item.Erledigt == null ? (object)DBNull.Value : item.Erledigt);
					sqlCommand.Parameters.AddWithValue("Falligkeit", item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
					sqlCommand.Parameters.AddWithValue("Freie_Text", item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
					sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht", item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
					sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("Interessent", item.Interessent == null ? (object)DBNull.Value : item.Interessent);
					sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
					sqlCommand.Parameters.AddWithValue("Kunden_Nr", item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
					sqlCommand.Parameters.AddWithValue("LAbteilung", item.LAbteilung == null ? (object)DBNull.Value : item.LAbteilung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("LAnrede", item.LAnrede == null ? (object)DBNull.Value : item.LAnrede);
					sqlCommand.Parameters.AddWithValue("LAnsprechpartner", item.LAnsprechpartner == null ? (object)DBNull.Value : item.LAnsprechpartner);
					sqlCommand.Parameters.AddWithValue("LBriefanrede", item.LBriefanrede == null ? (object)DBNull.Value : item.LBriefanrede);
					sqlCommand.Parameters.AddWithValue("Lieferadresse", item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("LLand_PLZ_Ort", item.LLand_PLZ_Ort == null ? (object)DBNull.Value : item.LLand_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("LName2", item.LName2 == null ? (object)DBNull.Value : item.LName2);
					sqlCommand.Parameters.AddWithValue("LName3", item.LName3 == null ? (object)DBNull.Value : item.LName3);
					sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("LsAddressNr", item.LsAddressNr == null ? (object)DBNull.Value : item.LsAddressNr);
					sqlCommand.Parameters.AddWithValue("LsDeliveryDate", item.LsDeliveryDate == null ? (object)DBNull.Value : item.LsDeliveryDate);
					sqlCommand.Parameters.AddWithValue("LStrasse_Postfach", item.LStraﬂe_Postfach == null ? (object)DBNull.Value : item.LStraﬂe_Postfach);
					sqlCommand.Parameters.AddWithValue("LVorname_NameFirma", item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("Neu_Order", item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
					sqlCommand.Parameters.AddWithValue("nr_ang", item.Nr_ang == null ? (object)DBNull.Value : item.Nr_ang);
					sqlCommand.Parameters.AddWithValue("nr_auf", item.Nr_auf == null ? (object)DBNull.Value : item.Nr_auf);
					sqlCommand.Parameters.AddWithValue("nr_BV", item.Nr_BV == null ? (object)DBNull.Value : item.Nr_BV);
					sqlCommand.Parameters.AddWithValue("nr_dlf", item.nr_dlf == null ? (object)DBNull.Value : item.nr_dlf);
					sqlCommand.Parameters.AddWithValue("nr_gut", item.Nr_gut == null ? (object)DBNull.Value : item.Nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_Kanban", item.Nr_Kanban == null ? (object)DBNull.Value : item.Nr_Kanban);
					sqlCommand.Parameters.AddWithValue("nr_lie", item.Nr_lie == null ? (object)DBNull.Value : item.Nr_lie);
					sqlCommand.Parameters.AddWithValue("nr_pro", item.Nr_pro == null ? (object)DBNull.Value : item.Nr_pro);
					sqlCommand.Parameters.AddWithValue("nr_RA", item.Nr_RA == null ? (object)DBNull.Value : item.Nr_RA);
					sqlCommand.Parameters.AddWithValue("nr_rec", item.Nr_rec == null ? (object)DBNull.Value : item.Nr_rec);
					sqlCommand.Parameters.AddWithValue("nr_sto", item.Nr_sto == null ? (object)DBNull.Value : item.Nr_sto);
					sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("rec_sent", item.rec_sent == null ? (object)DBNull.Value : item.rec_sent);
					sqlCommand.Parameters.AddWithValue("reparatur_nr", item.Reparatur_nr == null ? (object)DBNull.Value : item.Reparatur_nr);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Straﬂe_Postfach == null ? (object)DBNull.Value : item.Straﬂe_Postfach);
					sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.Termin_eingehalten == null ? (object)DBNull.Value : item.Termin_eingehalten);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt_Berechnen", item.USt_Berechnen == null ? (object)DBNull.Value : item.USt_Berechnen);
					sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl", item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
					sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl", item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wunschtermin", item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 87; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insert(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}
				return results;
			}

			return -1;
		}
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Angebote] ([ab_id],[ABSENDER],[Abteilung],[Angebot-Nr],[Anrede],[Ansprechpartner],[Auswahl],[Belegkreis],[Bemerkung],[Benutzer],[Bereich],[Bezug],[Briefanrede],[datueber],[Datum],[Debitorennummer],[Dplatz_Sirona],[EDI_Dateiname_CSV],[EDI_Kundenbestellnummer],[EDI_Order_Change],[EDI_Order_Change_Updated],[EDI_Order_Neu],[erledigt],[F‰lligkeit],[Freie_Text],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Interessent],[Konditionen],[Kunden-Nr],[LAbteilung],[Land/PLZ/Ort],[LAnrede],[LAnsprechpartner],[LBriefanrede],[Lieferadresse],[Liefertermin],[LLand/PLZ/Ort],[LName2],[LName3],[Lˆschen],[LsAddressNr],[LsDeliveryDate],[LStraﬂe/Postfach],[LVorname/NameFirma],[Mahnung],[Mandant],[Name2],[Name3],[Neu],[Neu_Order],[nr_ang],[nr_auf],[nr_BV],[nr_dlf],[nr_gut],[nr_Kanban],[nr_lie],[nr_pro],[nr_RA],[nr_rec],[nr_sto],[÷ffnen],[Personal-Nr],[Projekt-Nr],[rec_sent],[reparatur_nr],[Status],[StorageLocation],[Straﬂe/Postfach],[termin_eingehalten],[Typ],[UnloadingPoint],[Unser Zeichen],[USt_Berechnen],[Versandart],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Vorname/NameFirma],[Wunschtermin],[Zahlungsweise],[Zahlungsziel]) VALUES ( "

							+ "@ab_id" + i + ","
							+ "@ABSENDER" + i + ","
							+ "@Abteilung" + i + ","
							+ "@Angebot_Nr" + i + ","
							+ "@Anrede" + i + ","
							+ "@Ansprechpartner" + i + ","
							+ "@Auswahl" + i + ","
							+ "@Belegkreis" + i + ","
							+ "@Bemerkung" + i + ","
							+ "@Benutzer" + i + ","
							+ "@Bereich" + i + ","
							+ "@Bezug" + i + ","
							+ "@Briefanrede" + i + ","
							+ "@datueber" + i + ","
							+ "@Datum" + i + ","
							+ "@Debitorennummer" + i + ","
							+ "@Dplatz_Sirona" + i + ","
							+ "@EDI_Dateiname_CSV" + i + ","
							+ "@EDI_Kundenbestellnummer" + i + ","
							+ "@EDI_Order_Change" + i + ","
							+ "@EDI_Order_Change_Updated" + i + ","
							+ "@EDI_Order_Neu" + i + ","
							+ "@erledigt" + i + ","
							+ "@Falligkeit" + i + ","
							+ "@Freie_Text" + i + ","
							+ "@Freitext" + i + ","
							+ "@gebucht" + i + ","
							+ "@gedruckt" + i + ","
							+ "@Ihr_Zeichen" + i + ","
							+ "@In_Bearbeitung" + i + ","
							+ "@Interessent" + i + ","
							+ "@Konditionen" + i + ","
							+ "@Kunden_Nr" + i + ","
							+ "@LAbteilung" + i + ","
							+ "@Land_PLZ_Ort" + i + ","
							+ "@LAnrede" + i + ","
							+ "@LAnsprechpartner" + i + ","
							+ "@LBriefanrede" + i + ","
							+ "@Lieferadresse" + i + ","
							+ "@Liefertermin" + i + ","
							+ "@LLand_PLZ_Ort" + i + ","
							+ "@LName2" + i + ","
							+ "@LName3" + i + ","
							+ "@Loschen" + i + ","
							+ "@LsAddressNr" + i + ","
							+ "@LsDeliveryDate" + i + ","
							+ "@LStrasse_Postfach" + i + ","
							+ "@LVorname_NameFirma" + i + ","
							+ "@Mahnung" + i + ","
							+ "@Mandant" + i + ","
							+ "@Name2" + i + ","
							+ "@Name3" + i + ","
							+ "@Neu" + i + ","
							+ "@Neu_Order" + i + ","
							+ "@nr_ang" + i + ","
							+ "@nr_auf" + i + ","
							+ "@nr_BV" + i + ","
							+ "@nr_dlf" + i + ","
							+ "@nr_gut" + i + ","
							+ "@nr_Kanban" + i + ","
							+ "@nr_lie" + i + ","
							+ "@nr_pro" + i + ","
							+ "@nr_RA" + i + ","
							+ "@nr_rec" + i + ","
							+ "@nr_sto" + i + ","
							+ "@Offnen" + i + ","
							+ "@Personal_Nr" + i + ","
							+ "@Projekt_Nr" + i + ","
							+ "@rec_sent" + i + ","
							+ "@reparatur_nr" + i + ","
							+ "@Status" + i + ","
							+ "@StorageLocation" + i + ","
							+ "@Strasse_Postfach" + i + ","
							+ "@termin_eingehalten" + i + ","
							+ "@Typ" + i + ","
							+ "@UnloadingPoint" + i + ","
							+ "@Unser_Zeichen" + i + ","
							+ "@USt_Berechnen" + i + ","
							+ "@Versandart" + i + ","
							+ "@Versandarten_Auswahl" + i + ","
							+ "@Versanddatum_Auswahl" + i + ","
							+ "@Vorname_NameFirma" + i + ","
							+ "@Wunschtermin" + i + ","
							+ "@Zahlungsweise" + i + ","
							+ "@Zahlungsziel" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ab_id" + i, item.Ab_id == null ? (object)DBNull.Value : item.Ab_id);
						sqlCommand.Parameters.AddWithValue("ABSENDER" + i, item.ABSENDER == null ? (object)DBNull.Value : item.ABSENDER);
						sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
						sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
						sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
						sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
						sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
						sqlCommand.Parameters.AddWithValue("Bereich" + i, item.Bereich == null ? (object)DBNull.Value : item.Bereich);
						sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
						sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
						sqlCommand.Parameters.AddWithValue("datueber" + i, item.Datueber == null ? (object)DBNull.Value : item.Datueber);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Debitorennummer" + i, item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
						sqlCommand.Parameters.AddWithValue("Dplatz_Sirona" + i, item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
						sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV" + i, item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
						sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer" + i, item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Change" + i, item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated" + i, item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Neu" + i, item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.Erledigt == null ? (object)DBNull.Value : item.Erledigt);
						sqlCommand.Parameters.AddWithValue("Falligkeit" + i, item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
						sqlCommand.Parameters.AddWithValue("Freie_Text" + i, item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
						sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
						sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("Interessent" + i, item.Interessent == null ? (object)DBNull.Value : item.Interessent);
						sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
						sqlCommand.Parameters.AddWithValue("Kunden_Nr" + i, item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
						sqlCommand.Parameters.AddWithValue("LAbteilung" + i, item.LAbteilung == null ? (object)DBNull.Value : item.LAbteilung);
						sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("LAnrede" + i, item.LAnrede == null ? (object)DBNull.Value : item.LAnrede);
						sqlCommand.Parameters.AddWithValue("LAnsprechpartner" + i, item.LAnsprechpartner == null ? (object)DBNull.Value : item.LAnsprechpartner);
						sqlCommand.Parameters.AddWithValue("LBriefanrede" + i, item.LBriefanrede == null ? (object)DBNull.Value : item.LBriefanrede);
						sqlCommand.Parameters.AddWithValue("Lieferadresse" + i, item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("LLand_PLZ_Ort" + i, item.LLand_PLZ_Ort == null ? (object)DBNull.Value : item.LLand_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("LName2" + i, item.LName2 == null ? (object)DBNull.Value : item.LName2);
						sqlCommand.Parameters.AddWithValue("LName3" + i, item.LName3 == null ? (object)DBNull.Value : item.LName3);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("LsAddressNr" + i, item.LsAddressNr == null ? (object)DBNull.Value : item.LsAddressNr);
						sqlCommand.Parameters.AddWithValue("LsDeliveryDate" + i, item.LsDeliveryDate == null ? (object)DBNull.Value : item.LsDeliveryDate);
						sqlCommand.Parameters.AddWithValue("LStrasse_Postfach" + i, item.LStraﬂe_Postfach == null ? (object)DBNull.Value : item.LStraﬂe_Postfach);
						sqlCommand.Parameters.AddWithValue("LVorname_NameFirma" + i, item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
						sqlCommand.Parameters.AddWithValue("Neu_Order" + i, item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
						sqlCommand.Parameters.AddWithValue("nr_ang" + i, item.Nr_ang == null ? (object)DBNull.Value : item.Nr_ang);
						sqlCommand.Parameters.AddWithValue("nr_auf" + i, item.Nr_auf == null ? (object)DBNull.Value : item.Nr_auf);
						sqlCommand.Parameters.AddWithValue("nr_BV" + i, item.Nr_BV == null ? (object)DBNull.Value : item.Nr_BV);
						sqlCommand.Parameters.AddWithValue("nr_dlf" + i, item.nr_dlf == null ? (object)DBNull.Value : item.nr_dlf);
						sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.Nr_gut == null ? (object)DBNull.Value : item.Nr_gut);
						sqlCommand.Parameters.AddWithValue("nr_Kanban" + i, item.Nr_Kanban == null ? (object)DBNull.Value : item.Nr_Kanban);
						sqlCommand.Parameters.AddWithValue("nr_lie" + i, item.Nr_lie == null ? (object)DBNull.Value : item.Nr_lie);
						sqlCommand.Parameters.AddWithValue("nr_pro" + i, item.Nr_pro == null ? (object)DBNull.Value : item.Nr_pro);
						sqlCommand.Parameters.AddWithValue("nr_RA" + i, item.Nr_RA == null ? (object)DBNull.Value : item.Nr_RA);
						sqlCommand.Parameters.AddWithValue("nr_rec" + i, item.Nr_rec == null ? (object)DBNull.Value : item.Nr_rec);
						sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.Nr_sto == null ? (object)DBNull.Value : item.Nr_sto);
						sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("rec_sent" + i, item.rec_sent == null ? (object)DBNull.Value : item.rec_sent);
						sqlCommand.Parameters.AddWithValue("reparatur_nr" + i, item.Reparatur_nr == null ? (object)DBNull.Value : item.Reparatur_nr);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
						sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Straﬂe_Postfach == null ? (object)DBNull.Value : item.Straﬂe_Postfach);
						sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.Termin_eingehalten == null ? (object)DBNull.Value : item.Termin_eingehalten);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("UnloadingPoint" + i, item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
						sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
						sqlCommand.Parameters.AddWithValue("USt_Berechnen" + i, item.USt_Berechnen == null ? (object)DBNull.Value : item.USt_Berechnen);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl" + i, item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
						sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl" + i, item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Wunschtermin" + i, item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
						sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
						sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Angebote] SET [ab_id]=@ab_id, [ABSENDER]=@ABSENDER, [Abteilung]=@Abteilung, [Angebot-Nr]=@Angebot_Nr, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [Auswahl]=@Auswahl, [Belegkreis]=@Belegkreis, [Bemerkung]=@Bemerkung, [Benutzer]=@Benutzer, [Bereich]=@Bereich, [Bezug]=@Bezug, [Briefanrede]=@Briefanrede, [datueber]=@datueber, [Datum]=@Datum, [Debitorennummer]=@Debitorennummer, [Dplatz_Sirona]=@Dplatz_Sirona, [EDI_Dateiname_CSV]=@EDI_Dateiname_CSV, [EDI_Kundenbestellnummer]=@EDI_Kundenbestellnummer, [EDI_Order_Change]=@EDI_Order_Change, [EDI_Order_Change_Updated]=@EDI_Order_Change_Updated, [EDI_Order_Neu]=@EDI_Order_Neu, [erledigt]=@erledigt, [F‰lligkeit]=@Falligkeit, [Freie_Text]=@Freie_Text, [Freitext]=@Freitext, [gebucht]=@gebucht, [gedruckt]=@gedruckt, [Ihr Zeichen]=@Ihr_Zeichen, [In Bearbeitung]=@In_Bearbeitung, [Interessent]=@Interessent, [Konditionen]=@Konditionen, [Kunden-Nr]=@Kunden_Nr, [LAbteilung]=@LAbteilung, [Land/PLZ/Ort]=@Land_PLZ_Ort, [LAnrede]=@LAnrede, [LAnsprechpartner]=@LAnsprechpartner, [LBriefanrede]=@LBriefanrede, [Lieferadresse]=@Lieferadresse, [Liefertermin]=@Liefertermin, [LLand/PLZ/Ort]=@LLand_PLZ_Ort, [LName2]=@LName2, [LName3]=@LName3, [Lˆschen]=@Loschen, [LsAddressNr]=@LsAddressNr, [LsDeliveryDate]=@LsDeliveryDate, [LStraﬂe/Postfach]=@LStrasse_Postfach, [LVorname/NameFirma]=@LVorname_NameFirma, [Mahnung]=@Mahnung, [Mandant]=@Mandant, [Name2]=@Name2, [Name3]=@Name3, [Neu]=@Neu, [Neu_Order]=@Neu_Order, [nr_ang]=@nr_ang, [nr_auf]=@nr_auf, [nr_BV]=@nr_BV, [nr_dlf]=@nr_dlf, [nr_gut]=@nr_gut, [nr_Kanban]=@nr_Kanban, [nr_lie]=@nr_lie, [nr_pro]=@nr_pro, [nr_RA]=@nr_RA, [nr_rec]=@nr_rec, [nr_sto]=@nr_sto, [÷ffnen]=@Offnen, [Personal-Nr]=@Personal_Nr, [Projekt-Nr]=@Projekt_Nr, [rec_sent]=@rec_sent, [reparatur_nr]=@reparatur_nr, [Status]=@Status, [StorageLocation]=@StorageLocation, [Straﬂe/Postfach]=@Strasse_Postfach, [termin_eingehalten]=@termin_eingehalten, [Typ]=@Typ, [UnloadingPoint]=@UnloadingPoint, [Unser Zeichen]=@Unser_Zeichen, [USt_Berechnen]=@USt_Berechnen, [Versandart]=@Versandart, [Versandarten_Auswahl]=@Versandarten_Auswahl, [Versanddatum_Auswahl]=@Versanddatum_Auswahl, [Vorname/NameFirma]=@Vorname_NameFirma, [Wunschtermin]=@Wunschtermin, [Zahlungsweise]=@Zahlungsweise, [Zahlungsziel]=@Zahlungsziel WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("ab_id", item.Ab_id == null ? (object)DBNull.Value : item.Ab_id);
				sqlCommand.Parameters.AddWithValue("ABSENDER", item.ABSENDER == null ? (object)DBNull.Value : item.ABSENDER);
				sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
				sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
				sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
				sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
				sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
				sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
				sqlCommand.Parameters.AddWithValue("Bereich", item.Bereich == null ? (object)DBNull.Value : item.Bereich);
				sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
				sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
				sqlCommand.Parameters.AddWithValue("datueber", item.Datueber == null ? (object)DBNull.Value : item.Datueber);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Debitorennummer", item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
				sqlCommand.Parameters.AddWithValue("Dplatz_Sirona", item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
				sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV", item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
				sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer", item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
				sqlCommand.Parameters.AddWithValue("EDI_Order_Change", item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
				sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated", item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
				sqlCommand.Parameters.AddWithValue("EDI_Order_Neu", item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
				sqlCommand.Parameters.AddWithValue("erledigt", item.Erledigt == null ? (object)DBNull.Value : item.Erledigt);
				sqlCommand.Parameters.AddWithValue("Falligkeit", item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
				sqlCommand.Parameters.AddWithValue("Freie_Text", item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
				sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
				sqlCommand.Parameters.AddWithValue("gebucht", item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
				sqlCommand.Parameters.AddWithValue("gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
				sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
				sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
				sqlCommand.Parameters.AddWithValue("Interessent", item.Interessent == null ? (object)DBNull.Value : item.Interessent);
				sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
				sqlCommand.Parameters.AddWithValue("Kunden_Nr", item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
				sqlCommand.Parameters.AddWithValue("LAbteilung", item.LAbteilung == null ? (object)DBNull.Value : item.LAbteilung);
				sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
				sqlCommand.Parameters.AddWithValue("LAnrede", item.LAnrede == null ? (object)DBNull.Value : item.LAnrede);
				sqlCommand.Parameters.AddWithValue("LAnsprechpartner", item.LAnsprechpartner == null ? (object)DBNull.Value : item.LAnsprechpartner);
				sqlCommand.Parameters.AddWithValue("LBriefanrede", item.LBriefanrede == null ? (object)DBNull.Value : item.LBriefanrede);
				sqlCommand.Parameters.AddWithValue("Lieferadresse", item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("LLand_PLZ_Ort", item.LLand_PLZ_Ort == null ? (object)DBNull.Value : item.LLand_PLZ_Ort);
				sqlCommand.Parameters.AddWithValue("LName2", item.LName2 == null ? (object)DBNull.Value : item.LName2);
				sqlCommand.Parameters.AddWithValue("LName3", item.LName3 == null ? (object)DBNull.Value : item.LName3);
				sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
				sqlCommand.Parameters.AddWithValue("LsAddressNr", item.LsAddressNr == null ? (object)DBNull.Value : item.LsAddressNr);
				sqlCommand.Parameters.AddWithValue("LsDeliveryDate", item.LsDeliveryDate == null ? (object)DBNull.Value : item.LsDeliveryDate);
				sqlCommand.Parameters.AddWithValue("LStrasse_Postfach", item.LStraﬂe_Postfach == null ? (object)DBNull.Value : item.LStraﬂe_Postfach);
				sqlCommand.Parameters.AddWithValue("LVorname_NameFirma", item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
				sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
				sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
				sqlCommand.Parameters.AddWithValue("Neu_Order", item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
				sqlCommand.Parameters.AddWithValue("nr_ang", item.Nr_ang == null ? (object)DBNull.Value : item.Nr_ang);
				sqlCommand.Parameters.AddWithValue("nr_auf", item.Nr_auf == null ? (object)DBNull.Value : item.Nr_auf);
				sqlCommand.Parameters.AddWithValue("nr_BV", item.Nr_BV == null ? (object)DBNull.Value : item.Nr_BV);
				sqlCommand.Parameters.AddWithValue("nr_dlf", item.nr_dlf == null ? (object)DBNull.Value : item.nr_dlf);
				sqlCommand.Parameters.AddWithValue("nr_gut", item.Nr_gut == null ? (object)DBNull.Value : item.Nr_gut);
				sqlCommand.Parameters.AddWithValue("nr_Kanban", item.Nr_Kanban == null ? (object)DBNull.Value : item.Nr_Kanban);
				sqlCommand.Parameters.AddWithValue("nr_lie", item.Nr_lie == null ? (object)DBNull.Value : item.Nr_lie);
				sqlCommand.Parameters.AddWithValue("nr_pro", item.Nr_pro == null ? (object)DBNull.Value : item.Nr_pro);
				sqlCommand.Parameters.AddWithValue("nr_RA", item.Nr_RA == null ? (object)DBNull.Value : item.Nr_RA);
				sqlCommand.Parameters.AddWithValue("nr_rec", item.Nr_rec == null ? (object)DBNull.Value : item.Nr_rec);
				sqlCommand.Parameters.AddWithValue("nr_sto", item.Nr_sto == null ? (object)DBNull.Value : item.Nr_sto);
				sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
				sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
				sqlCommand.Parameters.AddWithValue("rec_sent", item.rec_sent == null ? (object)DBNull.Value : item.rec_sent);
				sqlCommand.Parameters.AddWithValue("reparatur_nr", item.Reparatur_nr == null ? (object)DBNull.Value : item.Reparatur_nr);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
				sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Straﬂe_Postfach == null ? (object)DBNull.Value : item.Straﬂe_Postfach);
				sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.Termin_eingehalten == null ? (object)DBNull.Value : item.Termin_eingehalten);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
				sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
				sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
				sqlCommand.Parameters.AddWithValue("USt_Berechnen", item.USt_Berechnen == null ? (object)DBNull.Value : item.USt_Berechnen);
				sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
				sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl", item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
				sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl", item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
				sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("Wunschtermin", item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
				sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
				sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 87; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = update(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}

				return results;
			}

			return -1;
		}
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [Angebote] SET "

							+ "[ab_id]=@ab_id" + i + ","
							+ "[ABSENDER]=@ABSENDER" + i + ","
							+ "[Abteilung]=@Abteilung" + i + ","
							+ "[Angebot-Nr]=@Angebot_Nr" + i + ","
							+ "[Anrede]=@Anrede" + i + ","
							+ "[Ansprechpartner]=@Ansprechpartner" + i + ","
							+ "[Auswahl]=@Auswahl" + i + ","
							+ "[Belegkreis]=@Belegkreis" + i + ","
							+ "[Bemerkung]=@Bemerkung" + i + ","
							+ "[Benutzer]=@Benutzer" + i + ","
							+ "[Bereich]=@Bereich" + i + ","
							+ "[Bezug]=@Bezug" + i + ","
							+ "[Briefanrede]=@Briefanrede" + i + ","
							+ "[datueber]=@datueber" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[Debitorennummer]=@Debitorennummer" + i + ","
							+ "[Dplatz_Sirona]=@Dplatz_Sirona" + i + ","
							+ "[EDI_Dateiname_CSV]=@EDI_Dateiname_CSV" + i + ","
							+ "[EDI_Kundenbestellnummer]=@EDI_Kundenbestellnummer" + i + ","
							+ "[EDI_Order_Change]=@EDI_Order_Change" + i + ","
							+ "[EDI_Order_Change_Updated]=@EDI_Order_Change_Updated" + i + ","
							+ "[EDI_Order_Neu]=@EDI_Order_Neu" + i + ","
							+ "[erledigt]=@erledigt" + i + ","
							+ "[F‰lligkeit]=@Falligkeit" + i + ","
							+ "[Freie_Text]=@Freie_Text" + i + ","
							+ "[Freitext]=@Freitext" + i + ","
							+ "[gebucht]=@gebucht" + i + ","
							+ "[gedruckt]=@gedruckt" + i + ","
							+ "[Ihr Zeichen]=@Ihr_Zeichen" + i + ","
							+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
							+ "[Interessent]=@Interessent" + i + ","
							+ "[Konditionen]=@Konditionen" + i + ","
							+ "[Kunden-Nr]=@Kunden_Nr" + i + ","
							+ "[LAbteilung]=@LAbteilung" + i + ","
							+ "[Land/PLZ/Ort]=@Land_PLZ_Ort" + i + ","
							+ "[LAnrede]=@LAnrede" + i + ","
							+ "[LAnsprechpartner]=@LAnsprechpartner" + i + ","
							+ "[LBriefanrede]=@LBriefanrede" + i + ","
							+ "[Lieferadresse]=@Lieferadresse" + i + ","
							+ "[Liefertermin]=@Liefertermin" + i + ","
							+ "[LLand/PLZ/Ort]=@LLand_PLZ_Ort" + i + ","
							+ "[LName2]=@LName2" + i + ","
							+ "[LName3]=@LName3" + i + ","
							+ "[Lˆschen]=@Loschen" + i + ","
							+ "[LsAddressNr]=@LsAddressNr" + i + ","
							+ "[LsDeliveryDate]=@LsDeliveryDate" + i + ","
							+ "[LStraﬂe/Postfach]=@LStrasse_Postfach" + i + ","
							+ "[LVorname/NameFirma]=@LVorname_NameFirma" + i + ","
							+ "[Mahnung]=@Mahnung" + i + ","
							+ "[Mandant]=@Mandant" + i + ","
							+ "[Name2]=@Name2" + i + ","
							+ "[Name3]=@Name3" + i + ","
							+ "[Neu]=@Neu" + i + ","
							+ "[Neu_Order]=@Neu_Order" + i + ","
							+ "[nr_ang]=@nr_ang" + i + ","
							+ "[nr_auf]=@nr_auf" + i + ","
							+ "[nr_BV]=@nr_BV" + i + ","
							+ "[nr_dlf]=@nr_dlf" + i + ","
							+ "[nr_gut]=@nr_gut" + i + ","
							+ "[nr_Kanban]=@nr_Kanban" + i + ","
							+ "[nr_lie]=@nr_lie" + i + ","
							+ "[nr_pro]=@nr_pro" + i + ","
							+ "[nr_RA]=@nr_RA" + i + ","
							+ "[nr_rec]=@nr_rec" + i + ","
							+ "[nr_sto]=@nr_sto" + i + ","
							+ "[÷ffnen]=@Offnen" + i + ","
							+ "[Personal-Nr]=@Personal_Nr" + i + ","
							+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
							+ "[rec_sent]=@rec_sent" + i + ","
							+ "[reparatur_nr]=@reparatur_nr" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[StorageLocation]=@StorageLocation" + i + ","
							+ "[Straﬂe/Postfach]=@Strasse_Postfach" + i + ","
							+ "[termin_eingehalten]=@termin_eingehalten" + i + ","
							+ "[Typ]=@Typ" + i + ","
							+ "[UnloadingPoint]=@UnloadingPoint" + i + ","
							+ "[Unser Zeichen]=@Unser_Zeichen" + i + ","
							+ "[USt_Berechnen]=@USt_Berechnen" + i + ","
							+ "[Versandart]=@Versandart" + i + ","
							+ "[Versandarten_Auswahl]=@Versandarten_Auswahl" + i + ","
							+ "[Versanddatum_Auswahl]=@Versanddatum_Auswahl" + i + ","
							+ "[Vorname/NameFirma]=@Vorname_NameFirma" + i + ","
							+ "[Wunschtermin]=@Wunschtermin" + i + ","
							+ "[Zahlungsweise]=@Zahlungsweise" + i + ","
							+ "[Zahlungsziel]=@Zahlungsziel" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("ab_id" + i, item.Ab_id == null ? (object)DBNull.Value : item.Ab_id);
						sqlCommand.Parameters.AddWithValue("ABSENDER" + i, item.ABSENDER == null ? (object)DBNull.Value : item.ABSENDER);
						sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
						sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
						sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
						sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
						sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
						sqlCommand.Parameters.AddWithValue("Bereich" + i, item.Bereich == null ? (object)DBNull.Value : item.Bereich);
						sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
						sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
						sqlCommand.Parameters.AddWithValue("datueber" + i, item.Datueber == null ? (object)DBNull.Value : item.Datueber);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Debitorennummer" + i, item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
						sqlCommand.Parameters.AddWithValue("Dplatz_Sirona" + i, item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
						sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV" + i, item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
						sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer" + i, item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Change" + i, item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated" + i, item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Neu" + i, item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.Erledigt == null ? (object)DBNull.Value : item.Erledigt);
						sqlCommand.Parameters.AddWithValue("Falligkeit" + i, item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
						sqlCommand.Parameters.AddWithValue("Freie_Text" + i, item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
						sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
						sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("Interessent" + i, item.Interessent == null ? (object)DBNull.Value : item.Interessent);
						sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
						sqlCommand.Parameters.AddWithValue("Kunden_Nr" + i, item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
						sqlCommand.Parameters.AddWithValue("LAbteilung" + i, item.LAbteilung == null ? (object)DBNull.Value : item.LAbteilung);
						sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("LAnrede" + i, item.LAnrede == null ? (object)DBNull.Value : item.LAnrede);
						sqlCommand.Parameters.AddWithValue("LAnsprechpartner" + i, item.LAnsprechpartner == null ? (object)DBNull.Value : item.LAnsprechpartner);
						sqlCommand.Parameters.AddWithValue("LBriefanrede" + i, item.LBriefanrede == null ? (object)DBNull.Value : item.LBriefanrede);
						sqlCommand.Parameters.AddWithValue("Lieferadresse" + i, item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("LLand_PLZ_Ort" + i, item.LLand_PLZ_Ort == null ? (object)DBNull.Value : item.LLand_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("LName2" + i, item.LName2 == null ? (object)DBNull.Value : item.LName2);
						sqlCommand.Parameters.AddWithValue("LName3" + i, item.LName3 == null ? (object)DBNull.Value : item.LName3);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("LsAddressNr" + i, item.LsAddressNr == null ? (object)DBNull.Value : item.LsAddressNr);
						sqlCommand.Parameters.AddWithValue("LsDeliveryDate" + i, item.LsDeliveryDate == null ? (object)DBNull.Value : item.LsDeliveryDate);
						sqlCommand.Parameters.AddWithValue("LStrasse_Postfach" + i, item.LStraﬂe_Postfach == null ? (object)DBNull.Value : item.LStraﬂe_Postfach);
						sqlCommand.Parameters.AddWithValue("LVorname_NameFirma" + i, item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
						sqlCommand.Parameters.AddWithValue("Neu_Order" + i, item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
						sqlCommand.Parameters.AddWithValue("nr_ang" + i, item.Nr_ang == null ? (object)DBNull.Value : item.Nr_ang);
						sqlCommand.Parameters.AddWithValue("nr_auf" + i, item.Nr_auf == null ? (object)DBNull.Value : item.Nr_auf);
						sqlCommand.Parameters.AddWithValue("nr_BV" + i, item.Nr_BV == null ? (object)DBNull.Value : item.Nr_BV);
						sqlCommand.Parameters.AddWithValue("nr_dlf" + i, item.nr_dlf == null ? (object)DBNull.Value : item.nr_dlf);
						sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.Nr_gut == null ? (object)DBNull.Value : item.Nr_gut);
						sqlCommand.Parameters.AddWithValue("nr_Kanban" + i, item.Nr_Kanban == null ? (object)DBNull.Value : item.Nr_Kanban);
						sqlCommand.Parameters.AddWithValue("nr_lie" + i, item.Nr_lie == null ? (object)DBNull.Value : item.Nr_lie);
						sqlCommand.Parameters.AddWithValue("nr_pro" + i, item.Nr_pro == null ? (object)DBNull.Value : item.Nr_pro);
						sqlCommand.Parameters.AddWithValue("nr_RA" + i, item.Nr_RA == null ? (object)DBNull.Value : item.Nr_RA);
						sqlCommand.Parameters.AddWithValue("nr_rec" + i, item.Nr_rec == null ? (object)DBNull.Value : item.Nr_rec);
						sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.Nr_sto == null ? (object)DBNull.Value : item.Nr_sto);
						sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("rec_sent" + i, item.rec_sent == null ? (object)DBNull.Value : item.rec_sent);
						sqlCommand.Parameters.AddWithValue("reparatur_nr" + i, item.Reparatur_nr == null ? (object)DBNull.Value : item.Reparatur_nr);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
						sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Straﬂe_Postfach == null ? (object)DBNull.Value : item.Straﬂe_Postfach);
						sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.Termin_eingehalten == null ? (object)DBNull.Value : item.Termin_eingehalten);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("UnloadingPoint" + i, item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
						sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
						sqlCommand.Parameters.AddWithValue("USt_Berechnen" + i, item.USt_Berechnen == null ? (object)DBNull.Value : item.USt_Berechnen);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl" + i, item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
						sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl" + i, item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Wunschtermin" + i, item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
						sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
						sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Angebote] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					string query = "DELETE FROM [Angebote] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Angebote] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Angebote]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Angebote] WHERE [Nr] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Angebote] ([ab_id],[ABSENDER],[Abteilung],[Angebot-Nr],[Anrede],[Ansprechpartner],[Auswahl],[Belegkreis],[Bemerkung],[Benutzer],[Bereich],[Bezug],[Briefanrede],[datueber],[Datum],[Debitorennummer],[Dplatz_Sirona],[EDI_Dateiname_CSV],[EDI_Kundenbestellnummer],[EDI_Order_Change],[EDI_Order_Change_Updated],[EDI_Order_Neu],[erledigt],[F‰lligkeit],[Freie_Text],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Interessent],[Konditionen],[Kunden-Nr],[LAbteilung],[Land/PLZ/Ort],[LAnrede],[LAnsprechpartner],[LBriefanrede],[Lieferadresse],[Liefertermin],[LLand/PLZ/Ort],[LName2],[LName3],[Lˆschen],[LsAddressNr],[LsDeliveryDate],[LStraﬂe/Postfach],[LVorname/NameFirma],[Mahnung],[Mandant],[Name2],[Name3],[Neu],[Neu_Order],[nr_ang],[nr_auf],[nr_BV],[nr_dlf],[nr_gut],[nr_Kanban],[nr_lie],[nr_pro],[nr_RA],[nr_rec],[nr_sto],[÷ffnen],[Personal-Nr],[Projekt-Nr],[rec_sent],[reparatur_nr],[Status],[StorageLocation],[Straﬂe/Postfach],[termin_eingehalten],[Typ],[UnloadingPoint],[Unser Zeichen],[USt_Berechnen],[Versandart],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Vorname/NameFirma],[Wunschtermin],[Zahlungsweise],[Zahlungsziel]) OUTPUT INSERTED.[Nr] VALUES (@ab_id,@ABSENDER,@Abteilung,@Angebot_Nr,@Anrede,@Ansprechpartner,@Auswahl,@Belegkreis,@Bemerkung,@Benutzer,@Bereich,@Bezug,@Briefanrede,@datueber,@Datum,@Debitorennummer,@Dplatz_Sirona,@EDI_Dateiname_CSV,@EDI_Kundenbestellnummer,@EDI_Order_Change,@EDI_Order_Change_Updated,@EDI_Order_Neu,@erledigt,@Falligkeit,@Freie_Text,@Freitext,@gebucht,@gedruckt,@Ihr_Zeichen,@In_Bearbeitung,@Interessent,@Konditionen,@Kunden_Nr,@LAbteilung,@Land_PLZ_Ort,@LAnrede,@LAnsprechpartner,@LBriefanrede,@Lieferadresse,@Liefertermin,@LLand_PLZ_Ort,@LName2,@LName3,@Loschen,@LsAddressNr,@LsDeliveryDate,@LStrasse_Postfach,@LVorname_NameFirma,@Mahnung,@Mandant,@Name2,@Name3,@Neu,@Neu_Order,@nr_ang,@nr_auf,@nr_BV,@nr_dlf,@nr_gut,@nr_Kanban,@nr_lie,@nr_pro,@nr_RA,@nr_rec,@nr_sto,@Offnen,@Personal_Nr,@Projekt_Nr,@rec_sent,@reparatur_nr,@Status,@StorageLocation,@Strasse_Postfach,@termin_eingehalten,@Typ,@UnloadingPoint,@Unser_Zeichen,@USt_Berechnen,@Versandart,@Versandarten_Auswahl,@Versanddatum_Auswahl,@Vorname_NameFirma,@Wunschtermin,@Zahlungsweise,@Zahlungsziel); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ab_id", item.Ab_id == null ? (object)DBNull.Value : item.Ab_id);
			sqlCommand.Parameters.AddWithValue("ABSENDER", item.ABSENDER == null ? (object)DBNull.Value : item.ABSENDER);
			sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
			sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
			sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
			sqlCommand.Parameters.AddWithValue("Bereich", item.Bereich == null ? (object)DBNull.Value : item.Bereich);
			sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
			sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
			sqlCommand.Parameters.AddWithValue("datueber", item.Datueber == null ? (object)DBNull.Value : item.Datueber);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Debitorennummer", item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
			sqlCommand.Parameters.AddWithValue("Dplatz_Sirona", item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
			sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV", item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
			sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer", item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Change", item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated", item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Neu", item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
			sqlCommand.Parameters.AddWithValue("erledigt", item.Erledigt == null ? (object)DBNull.Value : item.Erledigt);
			sqlCommand.Parameters.AddWithValue("Falligkeit", item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
			sqlCommand.Parameters.AddWithValue("Freie_Text", item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
			sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
			sqlCommand.Parameters.AddWithValue("gebucht", item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
			sqlCommand.Parameters.AddWithValue("gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
			sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
			sqlCommand.Parameters.AddWithValue("Interessent", item.Interessent == null ? (object)DBNull.Value : item.Interessent);
			sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
			sqlCommand.Parameters.AddWithValue("Kunden_Nr", item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
			sqlCommand.Parameters.AddWithValue("LAbteilung", item.LAbteilung == null ? (object)DBNull.Value : item.LAbteilung);
			sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
			sqlCommand.Parameters.AddWithValue("LAnrede", item.LAnrede == null ? (object)DBNull.Value : item.LAnrede);
			sqlCommand.Parameters.AddWithValue("LAnsprechpartner", item.LAnsprechpartner == null ? (object)DBNull.Value : item.LAnsprechpartner);
			sqlCommand.Parameters.AddWithValue("LBriefanrede", item.LBriefanrede == null ? (object)DBNull.Value : item.LBriefanrede);
			sqlCommand.Parameters.AddWithValue("Lieferadresse", item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("LLand_PLZ_Ort", item.LLand_PLZ_Ort == null ? (object)DBNull.Value : item.LLand_PLZ_Ort);
			sqlCommand.Parameters.AddWithValue("LName2", item.LName2 == null ? (object)DBNull.Value : item.LName2);
			sqlCommand.Parameters.AddWithValue("LName3", item.LName3 == null ? (object)DBNull.Value : item.LName3);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("LsAddressNr", item.LsAddressNr == null ? (object)DBNull.Value : item.LsAddressNr);
			sqlCommand.Parameters.AddWithValue("LsDeliveryDate", item.LsDeliveryDate == null ? (object)DBNull.Value : item.LsDeliveryDate);
			sqlCommand.Parameters.AddWithValue("LStrasse_Postfach", item.LStraﬂe_Postfach == null ? (object)DBNull.Value : item.LStraﬂe_Postfach);
			sqlCommand.Parameters.AddWithValue("LVorname_NameFirma", item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
			sqlCommand.Parameters.AddWithValue("Neu_Order", item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
			sqlCommand.Parameters.AddWithValue("nr_ang", item.Nr_ang == null ? (object)DBNull.Value : item.Nr_ang);
			sqlCommand.Parameters.AddWithValue("nr_auf", item.Nr_auf == null ? (object)DBNull.Value : item.Nr_auf);
			sqlCommand.Parameters.AddWithValue("nr_BV", item.Nr_BV == null ? (object)DBNull.Value : item.Nr_BV);
			sqlCommand.Parameters.AddWithValue("nr_dlf", item.nr_dlf == null ? (object)DBNull.Value : item.nr_dlf);
			sqlCommand.Parameters.AddWithValue("nr_gut", item.Nr_gut == null ? (object)DBNull.Value : item.Nr_gut);
			sqlCommand.Parameters.AddWithValue("nr_Kanban", item.Nr_Kanban == null ? (object)DBNull.Value : item.Nr_Kanban);
			sqlCommand.Parameters.AddWithValue("nr_lie", item.Nr_lie == null ? (object)DBNull.Value : item.Nr_lie);
			sqlCommand.Parameters.AddWithValue("nr_pro", item.Nr_pro == null ? (object)DBNull.Value : item.Nr_pro);
			sqlCommand.Parameters.AddWithValue("nr_RA", item.Nr_RA == null ? (object)DBNull.Value : item.Nr_RA);
			sqlCommand.Parameters.AddWithValue("nr_rec", item.Nr_rec == null ? (object)DBNull.Value : item.Nr_rec);
			sqlCommand.Parameters.AddWithValue("nr_sto", item.Nr_sto == null ? (object)DBNull.Value : item.Nr_sto);
			sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("rec_sent", item.rec_sent == null ? (object)DBNull.Value : item.rec_sent);
			sqlCommand.Parameters.AddWithValue("reparatur_nr", item.Reparatur_nr == null ? (object)DBNull.Value : item.Reparatur_nr);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
			sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Straﬂe_Postfach == null ? (object)DBNull.Value : item.Straﬂe_Postfach);
			sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.Termin_eingehalten == null ? (object)DBNull.Value : item.Termin_eingehalten);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
			sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
			sqlCommand.Parameters.AddWithValue("USt_Berechnen", item.USt_Berechnen == null ? (object)DBNull.Value : item.USt_Berechnen);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl", item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
			sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl", item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
			sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Wunschtermin", item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 87; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Angebote] ([ab_id],[ABSENDER],[Abteilung],[Angebot-Nr],[Anrede],[Ansprechpartner],[Auswahl],[Belegkreis],[Bemerkung],[Benutzer],[Bereich],[Bezug],[Briefanrede],[datueber],[Datum],[Debitorennummer],[Dplatz_Sirona],[EDI_Dateiname_CSV],[EDI_Kundenbestellnummer],[EDI_Order_Change],[EDI_Order_Change_Updated],[EDI_Order_Neu],[erledigt],[F‰lligkeit],[Freie_Text],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Interessent],[Konditionen],[Kunden-Nr],[LAbteilung],[Land/PLZ/Ort],[LAnrede],[LAnsprechpartner],[LBriefanrede],[Lieferadresse],[Liefertermin],[LLand/PLZ/Ort],[LName2],[LName3],[Lˆschen],[LsAddressNr],[LsDeliveryDate],[LStraﬂe/Postfach],[LVorname/NameFirma],[Mahnung],[Mandant],[Name2],[Name3],[Neu],[Neu_Order],[nr_ang],[nr_auf],[nr_BV],[nr_dlf],[nr_gut],[nr_Kanban],[nr_lie],[nr_pro],[nr_RA],[nr_rec],[nr_sto],[÷ffnen],[Personal-Nr],[Projekt-Nr],[rec_sent],[reparatur_nr],[Status],[StorageLocation],[Straﬂe/Postfach],[termin_eingehalten],[Typ],[UnloadingPoint],[Unser Zeichen],[USt_Berechnen],[Versandart],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Vorname/NameFirma],[Wunschtermin],[Zahlungsweise],[Zahlungsziel]) VALUES ( "

						+ "@ab_id" + i + ","
						+ "@ABSENDER" + i + ","
						+ "@Abteilung" + i + ","
						+ "@Angebot_Nr" + i + ","
						+ "@Anrede" + i + ","
						+ "@Ansprechpartner" + i + ","
						+ "@Auswahl" + i + ","
						+ "@Belegkreis" + i + ","
						+ "@Bemerkung" + i + ","
						+ "@Benutzer" + i + ","
						+ "@Bereich" + i + ","
						+ "@Bezug" + i + ","
						+ "@Briefanrede" + i + ","
						+ "@datueber" + i + ","
						+ "@Datum" + i + ","
						+ "@Debitorennummer" + i + ","
						+ "@Dplatz_Sirona" + i + ","
						+ "@EDI_Dateiname_CSV" + i + ","
						+ "@EDI_Kundenbestellnummer" + i + ","
						+ "@EDI_Order_Change" + i + ","
						+ "@EDI_Order_Change_Updated" + i + ","
						+ "@EDI_Order_Neu" + i + ","
						+ "@erledigt" + i + ","
						+ "@Falligkeit" + i + ","
						+ "@Freie_Text" + i + ","
						+ "@Freitext" + i + ","
						+ "@gebucht" + i + ","
						+ "@gedruckt" + i + ","
						+ "@Ihr_Zeichen" + i + ","
						+ "@In_Bearbeitung" + i + ","
						+ "@Interessent" + i + ","
						+ "@Konditionen" + i + ","
						+ "@Kunden_Nr" + i + ","
						+ "@LAbteilung" + i + ","
						+ "@Land_PLZ_Ort" + i + ","
						+ "@LAnrede" + i + ","
						+ "@LAnsprechpartner" + i + ","
						+ "@LBriefanrede" + i + ","
						+ "@Lieferadresse" + i + ","
						+ "@Liefertermin" + i + ","
						+ "@LLand_PLZ_Ort" + i + ","
						+ "@LName2" + i + ","
						+ "@LName3" + i + ","
						+ "@Loschen" + i + ","
						+ "@LsAddressNr" + i + ","
						+ "@LsDeliveryDate" + i + ","
						+ "@LStrasse_Postfach" + i + ","
						+ "@LVorname_NameFirma" + i + ","
						+ "@Mahnung" + i + ","
						+ "@Mandant" + i + ","
						+ "@Name2" + i + ","
						+ "@Name3" + i + ","
						+ "@Neu" + i + ","
						+ "@Neu_Order" + i + ","
						+ "@nr_ang" + i + ","
						+ "@nr_auf" + i + ","
						+ "@nr_BV" + i + ","
						+ "@nr_dlf" + i + ","
						+ "@nr_gut" + i + ","
						+ "@nr_Kanban" + i + ","
						+ "@nr_lie" + i + ","
						+ "@nr_pro" + i + ","
						+ "@nr_RA" + i + ","
						+ "@nr_rec" + i + ","
						+ "@nr_sto" + i + ","
						+ "@Offnen" + i + ","
						+ "@Personal_Nr" + i + ","
						+ "@Projekt_Nr" + i + ","
						+ "@rec_sent" + i + ","
						+ "@reparatur_nr" + i + ","
						+ "@Status" + i + ","
						+ "@StorageLocation" + i + ","
						+ "@Strasse_Postfach" + i + ","
						+ "@termin_eingehalten" + i + ","
						+ "@Typ" + i + ","
						+ "@UnloadingPoint" + i + ","
						+ "@Unser_Zeichen" + i + ","
						+ "@USt_Berechnen" + i + ","
						+ "@Versandart" + i + ","
						+ "@Versandarten_Auswahl" + i + ","
						+ "@Versanddatum_Auswahl" + i + ","
						+ "@Vorname_NameFirma" + i + ","
						+ "@Wunschtermin" + i + ","
						+ "@Zahlungsweise" + i + ","
						+ "@Zahlungsziel" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ab_id" + i, item.Ab_id == null ? (object)DBNull.Value : item.Ab_id);
					sqlCommand.Parameters.AddWithValue("ABSENDER" + i, item.ABSENDER == null ? (object)DBNull.Value : item.ABSENDER);
					sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
					sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
					sqlCommand.Parameters.AddWithValue("Bereich" + i, item.Bereich == null ? (object)DBNull.Value : item.Bereich);
					sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
					sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("datueber" + i, item.Datueber == null ? (object)DBNull.Value : item.Datueber);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Debitorennummer" + i, item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
					sqlCommand.Parameters.AddWithValue("Dplatz_Sirona" + i, item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
					sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV" + i, item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
					sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer" + i, item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change" + i, item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated" + i, item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Neu" + i, item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
					sqlCommand.Parameters.AddWithValue("erledigt" + i, item.Erledigt == null ? (object)DBNull.Value : item.Erledigt);
					sqlCommand.Parameters.AddWithValue("Falligkeit" + i, item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
					sqlCommand.Parameters.AddWithValue("Freie_Text" + i, item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
					sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
					sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("Interessent" + i, item.Interessent == null ? (object)DBNull.Value : item.Interessent);
					sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
					sqlCommand.Parameters.AddWithValue("Kunden_Nr" + i, item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
					sqlCommand.Parameters.AddWithValue("LAbteilung" + i, item.LAbteilung == null ? (object)DBNull.Value : item.LAbteilung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("LAnrede" + i, item.LAnrede == null ? (object)DBNull.Value : item.LAnrede);
					sqlCommand.Parameters.AddWithValue("LAnsprechpartner" + i, item.LAnsprechpartner == null ? (object)DBNull.Value : item.LAnsprechpartner);
					sqlCommand.Parameters.AddWithValue("LBriefanrede" + i, item.LBriefanrede == null ? (object)DBNull.Value : item.LBriefanrede);
					sqlCommand.Parameters.AddWithValue("Lieferadresse" + i, item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("LLand_PLZ_Ort" + i, item.LLand_PLZ_Ort == null ? (object)DBNull.Value : item.LLand_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("LName2" + i, item.LName2 == null ? (object)DBNull.Value : item.LName2);
					sqlCommand.Parameters.AddWithValue("LName3" + i, item.LName3 == null ? (object)DBNull.Value : item.LName3);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("LsAddressNr" + i, item.LsAddressNr == null ? (object)DBNull.Value : item.LsAddressNr);
					sqlCommand.Parameters.AddWithValue("LsDeliveryDate" + i, item.LsDeliveryDate == null ? (object)DBNull.Value : item.LsDeliveryDate);
					sqlCommand.Parameters.AddWithValue("LStrasse_Postfach" + i, item.LStraﬂe_Postfach == null ? (object)DBNull.Value : item.LStraﬂe_Postfach);
					sqlCommand.Parameters.AddWithValue("LVorname_NameFirma" + i, item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("Neu_Order" + i, item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
					sqlCommand.Parameters.AddWithValue("nr_ang" + i, item.Nr_ang == null ? (object)DBNull.Value : item.Nr_ang);
					sqlCommand.Parameters.AddWithValue("nr_auf" + i, item.Nr_auf == null ? (object)DBNull.Value : item.Nr_auf);
					sqlCommand.Parameters.AddWithValue("nr_BV" + i, item.Nr_BV == null ? (object)DBNull.Value : item.Nr_BV);
					sqlCommand.Parameters.AddWithValue("nr_dlf" + i, item.nr_dlf == null ? (object)DBNull.Value : item.nr_dlf);
					sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.Nr_gut == null ? (object)DBNull.Value : item.Nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_Kanban" + i, item.Nr_Kanban == null ? (object)DBNull.Value : item.Nr_Kanban);
					sqlCommand.Parameters.AddWithValue("nr_lie" + i, item.Nr_lie == null ? (object)DBNull.Value : item.Nr_lie);
					sqlCommand.Parameters.AddWithValue("nr_pro" + i, item.Nr_pro == null ? (object)DBNull.Value : item.Nr_pro);
					sqlCommand.Parameters.AddWithValue("nr_RA" + i, item.Nr_RA == null ? (object)DBNull.Value : item.Nr_RA);
					sqlCommand.Parameters.AddWithValue("nr_rec" + i, item.Nr_rec == null ? (object)DBNull.Value : item.Nr_rec);
					sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.Nr_sto == null ? (object)DBNull.Value : item.Nr_sto);
					sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("rec_sent" + i, item.rec_sent == null ? (object)DBNull.Value : item.rec_sent);
					sqlCommand.Parameters.AddWithValue("reparatur_nr" + i, item.Reparatur_nr == null ? (object)DBNull.Value : item.Reparatur_nr);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Straﬂe_Postfach == null ? (object)DBNull.Value : item.Straﬂe_Postfach);
					sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.Termin_eingehalten == null ? (object)DBNull.Value : item.Termin_eingehalten);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("UnloadingPoint" + i, item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt_Berechnen" + i, item.USt_Berechnen == null ? (object)DBNull.Value : item.USt_Berechnen);
					sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl" + i, item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
					sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl" + i, item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wunschtermin" + i, item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Angebote] SET [ab_id]=@ab_id, [ABSENDER]=@ABSENDER, [Abteilung]=@Abteilung, [Angebot-Nr]=@Angebot_Nr, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [Auswahl]=@Auswahl, [Belegkreis]=@Belegkreis, [Bemerkung]=@Bemerkung, [Benutzer]=@Benutzer, [Bereich]=@Bereich, [Bezug]=@Bezug, [Briefanrede]=@Briefanrede, [datueber]=@datueber, [Datum]=@Datum, [Debitorennummer]=@Debitorennummer, [Dplatz_Sirona]=@Dplatz_Sirona, [EDI_Dateiname_CSV]=@EDI_Dateiname_CSV, [EDI_Kundenbestellnummer]=@EDI_Kundenbestellnummer, [EDI_Order_Change]=@EDI_Order_Change, [EDI_Order_Change_Updated]=@EDI_Order_Change_Updated, [EDI_Order_Neu]=@EDI_Order_Neu, [erledigt]=@erledigt, [F‰lligkeit]=@Falligkeit, [Freie_Text]=@Freie_Text, [Freitext]=@Freitext, [gebucht]=@gebucht, [gedruckt]=@gedruckt, [Ihr Zeichen]=@Ihr_Zeichen, [In Bearbeitung]=@In_Bearbeitung, [Interessent]=@Interessent, [Konditionen]=@Konditionen, [Kunden-Nr]=@Kunden_Nr, [LAbteilung]=@LAbteilung, [Land/PLZ/Ort]=@Land_PLZ_Ort, [LAnrede]=@LAnrede, [LAnsprechpartner]=@LAnsprechpartner, [LBriefanrede]=@LBriefanrede, [Lieferadresse]=@Lieferadresse, [Liefertermin]=@Liefertermin, [LLand/PLZ/Ort]=@LLand_PLZ_Ort, [LName2]=@LName2, [LName3]=@LName3, [Lˆschen]=@Loschen, [LsAddressNr]=@LsAddressNr, [LsDeliveryDate]=@LsDeliveryDate, [LStraﬂe/Postfach]=@LStrasse_Postfach, [LVorname/NameFirma]=@LVorname_NameFirma, [Mahnung]=@Mahnung, [Mandant]=@Mandant, [Name2]=@Name2, [Name3]=@Name3, [Neu]=@Neu, [Neu_Order]=@Neu_Order, [nr_ang]=@nr_ang, [nr_auf]=@nr_auf, [nr_BV]=@nr_BV, [nr_dlf]=@nr_dlf, [nr_gut]=@nr_gut, [nr_Kanban]=@nr_Kanban, [nr_lie]=@nr_lie, [nr_pro]=@nr_pro, [nr_RA]=@nr_RA, [nr_rec]=@nr_rec, [nr_sto]=@nr_sto, [÷ffnen]=@Offnen, [Personal-Nr]=@Personal_Nr, [Projekt-Nr]=@Projekt_Nr, [rec_sent]=@rec_sent, [reparatur_nr]=@reparatur_nr, [Status]=@Status, [StorageLocation]=@StorageLocation, [Straﬂe/Postfach]=@Strasse_Postfach, [termin_eingehalten]=@termin_eingehalten, [Typ]=@Typ, [UnloadingPoint]=@UnloadingPoint, [Unser Zeichen]=@Unser_Zeichen, [USt_Berechnen]=@USt_Berechnen, [Versandart]=@Versandart, [Versandarten_Auswahl]=@Versandarten_Auswahl, [Versanddatum_Auswahl]=@Versanddatum_Auswahl, [Vorname/NameFirma]=@Vorname_NameFirma, [Wunschtermin]=@Wunschtermin, [Zahlungsweise]=@Zahlungsweise, [Zahlungsziel]=@Zahlungsziel WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("ab_id", item.Ab_id == null ? (object)DBNull.Value : item.Ab_id);
			sqlCommand.Parameters.AddWithValue("ABSENDER", item.ABSENDER == null ? (object)DBNull.Value : item.ABSENDER);
			sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
			sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
			sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
			sqlCommand.Parameters.AddWithValue("Bereich", item.Bereich == null ? (object)DBNull.Value : item.Bereich);
			sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
			sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
			sqlCommand.Parameters.AddWithValue("datueber", item.Datueber == null ? (object)DBNull.Value : item.Datueber);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Debitorennummer", item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
			sqlCommand.Parameters.AddWithValue("Dplatz_Sirona", item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
			sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV", item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
			sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer", item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Change", item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated", item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Neu", item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
			sqlCommand.Parameters.AddWithValue("erledigt", item.Erledigt == null ? (object)DBNull.Value : item.Erledigt);
			sqlCommand.Parameters.AddWithValue("Falligkeit", item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
			sqlCommand.Parameters.AddWithValue("Freie_Text", item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
			sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
			sqlCommand.Parameters.AddWithValue("gebucht", item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
			sqlCommand.Parameters.AddWithValue("gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
			sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
			sqlCommand.Parameters.AddWithValue("Interessent", item.Interessent == null ? (object)DBNull.Value : item.Interessent);
			sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
			sqlCommand.Parameters.AddWithValue("Kunden_Nr", item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
			sqlCommand.Parameters.AddWithValue("LAbteilung", item.LAbteilung == null ? (object)DBNull.Value : item.LAbteilung);
			sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
			sqlCommand.Parameters.AddWithValue("LAnrede", item.LAnrede == null ? (object)DBNull.Value : item.LAnrede);
			sqlCommand.Parameters.AddWithValue("LAnsprechpartner", item.LAnsprechpartner == null ? (object)DBNull.Value : item.LAnsprechpartner);
			sqlCommand.Parameters.AddWithValue("LBriefanrede", item.LBriefanrede == null ? (object)DBNull.Value : item.LBriefanrede);
			sqlCommand.Parameters.AddWithValue("Lieferadresse", item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("LLand_PLZ_Ort", item.LLand_PLZ_Ort == null ? (object)DBNull.Value : item.LLand_PLZ_Ort);
			sqlCommand.Parameters.AddWithValue("LName2", item.LName2 == null ? (object)DBNull.Value : item.LName2);
			sqlCommand.Parameters.AddWithValue("LName3", item.LName3 == null ? (object)DBNull.Value : item.LName3);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("LsAddressNr", item.LsAddressNr == null ? (object)DBNull.Value : item.LsAddressNr);
			sqlCommand.Parameters.AddWithValue("LsDeliveryDate", item.LsDeliveryDate == null ? (object)DBNull.Value : item.LsDeliveryDate);
			sqlCommand.Parameters.AddWithValue("LStrasse_Postfach", item.LStraﬂe_Postfach == null ? (object)DBNull.Value : item.LStraﬂe_Postfach);
			sqlCommand.Parameters.AddWithValue("LVorname_NameFirma", item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
			sqlCommand.Parameters.AddWithValue("Neu_Order", item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
			sqlCommand.Parameters.AddWithValue("nr_ang", item.Nr_ang == null ? (object)DBNull.Value : item.Nr_ang);
			sqlCommand.Parameters.AddWithValue("nr_auf", item.Nr_auf == null ? (object)DBNull.Value : item.Nr_auf);
			sqlCommand.Parameters.AddWithValue("nr_BV", item.Nr_BV == null ? (object)DBNull.Value : item.Nr_BV);
			sqlCommand.Parameters.AddWithValue("nr_dlf", item.nr_dlf == null ? (object)DBNull.Value : item.nr_dlf);
			sqlCommand.Parameters.AddWithValue("nr_gut", item.Nr_gut == null ? (object)DBNull.Value : item.Nr_gut);
			sqlCommand.Parameters.AddWithValue("nr_Kanban", item.Nr_Kanban == null ? (object)DBNull.Value : item.Nr_Kanban);
			sqlCommand.Parameters.AddWithValue("nr_lie", item.Nr_lie == null ? (object)DBNull.Value : item.Nr_lie);
			sqlCommand.Parameters.AddWithValue("nr_pro", item.Nr_pro == null ? (object)DBNull.Value : item.Nr_pro);
			sqlCommand.Parameters.AddWithValue("nr_RA", item.Nr_RA == null ? (object)DBNull.Value : item.Nr_RA);
			sqlCommand.Parameters.AddWithValue("nr_rec", item.Nr_rec == null ? (object)DBNull.Value : item.Nr_rec);
			sqlCommand.Parameters.AddWithValue("nr_sto", item.Nr_sto == null ? (object)DBNull.Value : item.Nr_sto);
			sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("rec_sent", item.rec_sent == null ? (object)DBNull.Value : item.rec_sent);
			sqlCommand.Parameters.AddWithValue("reparatur_nr", item.Reparatur_nr == null ? (object)DBNull.Value : item.Reparatur_nr);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
			sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Straﬂe_Postfach == null ? (object)DBNull.Value : item.Straﬂe_Postfach);
			sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.Termin_eingehalten == null ? (object)DBNull.Value : item.Termin_eingehalten);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
			sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
			sqlCommand.Parameters.AddWithValue("USt_Berechnen", item.USt_Berechnen == null ? (object)DBNull.Value : item.USt_Berechnen);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl", item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
			sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl", item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
			sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Wunschtermin", item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 87; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Angebote] SET "

					+ "[ab_id]=@ab_id" + i + ","
					+ "[ABSENDER]=@ABSENDER" + i + ","
					+ "[Abteilung]=@Abteilung" + i + ","
					+ "[Angebot-Nr]=@Angebot_Nr" + i + ","
					+ "[Anrede]=@Anrede" + i + ","
					+ "[Ansprechpartner]=@Ansprechpartner" + i + ","
					+ "[Auswahl]=@Auswahl" + i + ","
					+ "[Belegkreis]=@Belegkreis" + i + ","
					+ "[Bemerkung]=@Bemerkung" + i + ","
					+ "[Benutzer]=@Benutzer" + i + ","
					+ "[Bereich]=@Bereich" + i + ","
					+ "[Bezug]=@Bezug" + i + ","
					+ "[Briefanrede]=@Briefanrede" + i + ","
					+ "[datueber]=@datueber" + i + ","
					+ "[Datum]=@Datum" + i + ","
					+ "[Debitorennummer]=@Debitorennummer" + i + ","
					+ "[Dplatz_Sirona]=@Dplatz_Sirona" + i + ","
					+ "[EDI_Dateiname_CSV]=@EDI_Dateiname_CSV" + i + ","
					+ "[EDI_Kundenbestellnummer]=@EDI_Kundenbestellnummer" + i + ","
					+ "[EDI_Order_Change]=@EDI_Order_Change" + i + ","
					+ "[EDI_Order_Change_Updated]=@EDI_Order_Change_Updated" + i + ","
					+ "[EDI_Order_Neu]=@EDI_Order_Neu" + i + ","
					+ "[erledigt]=@erledigt" + i + ","
					+ "[F‰lligkeit]=@Falligkeit" + i + ","
					+ "[Freie_Text]=@Freie_Text" + i + ","
					+ "[Freitext]=@Freitext" + i + ","
					+ "[gebucht]=@gebucht" + i + ","
					+ "[gedruckt]=@gedruckt" + i + ","
					+ "[Ihr Zeichen]=@Ihr_Zeichen" + i + ","
					+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
					+ "[Interessent]=@Interessent" + i + ","
					+ "[Konditionen]=@Konditionen" + i + ","
					+ "[Kunden-Nr]=@Kunden_Nr" + i + ","
					+ "[LAbteilung]=@LAbteilung" + i + ","
					+ "[Land/PLZ/Ort]=@Land_PLZ_Ort" + i + ","
					+ "[LAnrede]=@LAnrede" + i + ","
					+ "[LAnsprechpartner]=@LAnsprechpartner" + i + ","
					+ "[LBriefanrede]=@LBriefanrede" + i + ","
					+ "[Lieferadresse]=@Lieferadresse" + i + ","
					+ "[Liefertermin]=@Liefertermin" + i + ","
					+ "[LLand/PLZ/Ort]=@LLand_PLZ_Ort" + i + ","
					+ "[LName2]=@LName2" + i + ","
					+ "[LName3]=@LName3" + i + ","
					+ "[Lˆschen]=@Loschen" + i + ","
					+ "[LsAddressNr]=@LsAddressNr" + i + ","
					+ "[LsDeliveryDate]=@LsDeliveryDate" + i + ","
					+ "[LStraﬂe/Postfach]=@LStrasse_Postfach" + i + ","
					+ "[LVorname/NameFirma]=@LVorname_NameFirma" + i + ","
					+ "[Mahnung]=@Mahnung" + i + ","
					+ "[Mandant]=@Mandant" + i + ","
					+ "[Name2]=@Name2" + i + ","
					+ "[Name3]=@Name3" + i + ","
					+ "[Neu]=@Neu" + i + ","
					+ "[Neu_Order]=@Neu_Order" + i + ","
					+ "[nr_ang]=@nr_ang" + i + ","
					+ "[nr_auf]=@nr_auf" + i + ","
					+ "[nr_BV]=@nr_BV" + i + ","
					+ "[nr_dlf]=@nr_dlf" + i + ","
					+ "[nr_gut]=@nr_gut" + i + ","
					+ "[nr_Kanban]=@nr_Kanban" + i + ","
					+ "[nr_lie]=@nr_lie" + i + ","
					+ "[nr_pro]=@nr_pro" + i + ","
					+ "[nr_RA]=@nr_RA" + i + ","
					+ "[nr_rec]=@nr_rec" + i + ","
					+ "[nr_sto]=@nr_sto" + i + ","
					+ "[÷ffnen]=@Offnen" + i + ","
					+ "[Personal-Nr]=@Personal_Nr" + i + ","
					+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
					+ "[rec_sent]=@rec_sent" + i + ","
					+ "[reparatur_nr]=@reparatur_nr" + i + ","
					+ "[Status]=@Status" + i + ","
					+ "[StorageLocation]=@StorageLocation" + i + ","
					+ "[Straﬂe/Postfach]=@Strasse_Postfach" + i + ","
					+ "[termin_eingehalten]=@termin_eingehalten" + i + ","
					+ "[Typ]=@Typ" + i + ","
					+ "[UnloadingPoint]=@UnloadingPoint" + i + ","
					+ "[Unser Zeichen]=@Unser_Zeichen" + i + ","
					+ "[USt_Berechnen]=@USt_Berechnen" + i + ","
					+ "[Versandart]=@Versandart" + i + ","
					+ "[Versandarten_Auswahl]=@Versandarten_Auswahl" + i + ","
					+ "[Versanddatum_Auswahl]=@Versanddatum_Auswahl" + i + ","
					+ "[Vorname/NameFirma]=@Vorname_NameFirma" + i + ","
					+ "[Wunschtermin]=@Wunschtermin" + i + ","
					+ "[Zahlungsweise]=@Zahlungsweise" + i + ","
					+ "[Zahlungsziel]=@Zahlungsziel" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("ab_id" + i, item.Ab_id == null ? (object)DBNull.Value : item.Ab_id);
					sqlCommand.Parameters.AddWithValue("ABSENDER" + i, item.ABSENDER == null ? (object)DBNull.Value : item.ABSENDER);
					sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
					sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
					sqlCommand.Parameters.AddWithValue("Bereich" + i, item.Bereich == null ? (object)DBNull.Value : item.Bereich);
					sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
					sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("datueber" + i, item.Datueber == null ? (object)DBNull.Value : item.Datueber);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Debitorennummer" + i, item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
					sqlCommand.Parameters.AddWithValue("Dplatz_Sirona" + i, item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
					sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV" + i, item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
					sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer" + i, item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change" + i, item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated" + i, item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Neu" + i, item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
					sqlCommand.Parameters.AddWithValue("erledigt" + i, item.Erledigt == null ? (object)DBNull.Value : item.Erledigt);
					sqlCommand.Parameters.AddWithValue("Falligkeit" + i, item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
					sqlCommand.Parameters.AddWithValue("Freie_Text" + i, item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
					sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
					sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("Interessent" + i, item.Interessent == null ? (object)DBNull.Value : item.Interessent);
					sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
					sqlCommand.Parameters.AddWithValue("Kunden_Nr" + i, item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
					sqlCommand.Parameters.AddWithValue("LAbteilung" + i, item.LAbteilung == null ? (object)DBNull.Value : item.LAbteilung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("LAnrede" + i, item.LAnrede == null ? (object)DBNull.Value : item.LAnrede);
					sqlCommand.Parameters.AddWithValue("LAnsprechpartner" + i, item.LAnsprechpartner == null ? (object)DBNull.Value : item.LAnsprechpartner);
					sqlCommand.Parameters.AddWithValue("LBriefanrede" + i, item.LBriefanrede == null ? (object)DBNull.Value : item.LBriefanrede);
					sqlCommand.Parameters.AddWithValue("Lieferadresse" + i, item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("LLand_PLZ_Ort" + i, item.LLand_PLZ_Ort == null ? (object)DBNull.Value : item.LLand_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("LName2" + i, item.LName2 == null ? (object)DBNull.Value : item.LName2);
					sqlCommand.Parameters.AddWithValue("LName3" + i, item.LName3 == null ? (object)DBNull.Value : item.LName3);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("LsAddressNr" + i, item.LsAddressNr == null ? (object)DBNull.Value : item.LsAddressNr);
					sqlCommand.Parameters.AddWithValue("LsDeliveryDate" + i, item.LsDeliveryDate == null ? (object)DBNull.Value : item.LsDeliveryDate);
					sqlCommand.Parameters.AddWithValue("LStrasse_Postfach" + i, item.LStraﬂe_Postfach == null ? (object)DBNull.Value : item.LStraﬂe_Postfach);
					sqlCommand.Parameters.AddWithValue("LVorname_NameFirma" + i, item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("Neu_Order" + i, item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
					sqlCommand.Parameters.AddWithValue("nr_ang" + i, item.Nr_ang == null ? (object)DBNull.Value : item.Nr_ang);
					sqlCommand.Parameters.AddWithValue("nr_auf" + i, item.Nr_auf == null ? (object)DBNull.Value : item.Nr_auf);
					sqlCommand.Parameters.AddWithValue("nr_BV" + i, item.Nr_BV == null ? (object)DBNull.Value : item.Nr_BV);
					sqlCommand.Parameters.AddWithValue("nr_dlf" + i, item.nr_dlf == null ? (object)DBNull.Value : item.nr_dlf);
					sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.Nr_gut == null ? (object)DBNull.Value : item.Nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_Kanban" + i, item.Nr_Kanban == null ? (object)DBNull.Value : item.Nr_Kanban);
					sqlCommand.Parameters.AddWithValue("nr_lie" + i, item.Nr_lie == null ? (object)DBNull.Value : item.Nr_lie);
					sqlCommand.Parameters.AddWithValue("nr_pro" + i, item.Nr_pro == null ? (object)DBNull.Value : item.Nr_pro);
					sqlCommand.Parameters.AddWithValue("nr_RA" + i, item.Nr_RA == null ? (object)DBNull.Value : item.Nr_RA);
					sqlCommand.Parameters.AddWithValue("nr_rec" + i, item.Nr_rec == null ? (object)DBNull.Value : item.Nr_rec);
					sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.Nr_sto == null ? (object)DBNull.Value : item.Nr_sto);
					sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("rec_sent" + i, item.rec_sent == null ? (object)DBNull.Value : item.rec_sent);
					sqlCommand.Parameters.AddWithValue("reparatur_nr" + i, item.Reparatur_nr == null ? (object)DBNull.Value : item.Reparatur_nr);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Straﬂe_Postfach == null ? (object)DBNull.Value : item.Straﬂe_Postfach);
					sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.Termin_eingehalten == null ? (object)DBNull.Value : item.Termin_eingehalten);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("UnloadingPoint" + i, item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt_Berechnen" + i, item.USt_Berechnen == null ? (object)DBNull.Value : item.USt_Berechnen);
					sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl" + i, item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
					sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl" + i, item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wunschtermin" + i, item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Angebote] WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

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

				string query = "DELETE FROM [Angebote] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods
		public static int InsertWithTransaction__(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity element,
			int maxCurrentValue, int minNewValue, string type,
			SqlConnection connection, SqlTransaction transaction)
		{
			int response = -1;

			string query = @"DECLARE @NewAngebotNr AS INT
                SET @NewAngebotNr=(select IIF((select MAX([Angebot-Nr])+1 from Angebote where typ=@type)<@maxCurrentValue,(select MAX([Angebot-Nr])+1 from Angebote where typ=@type),@minNewValue))
                INSERT INTO [Angebote]
                (Neu_Order,ab_id,ABSENDER,Abteilung,Anrede,Ansprechpartner,Auswahl,
                Belegkreis,Bemerkung,Benutzer,Bereich,Bezug,Briefanrede,datueber,Datum,Debitorennummer,
                Dplatz_Sirona,EDI_Dateiname_CSV,EDI_Kundenbestellnummer,EDI_Order_Change,EDI_Order_Change_Updated,
                EDI_Order_Neu,erledigt,F‰lligkeit,Freie_Text,Freitext,gebucht,gedruckt,[Ihr Zeichen],
                [In Bearbeitung],Interessent,Konditionen,[Kunden-Nr],LAbteilung,[Land/PLZ/Ort],LAnrede,
                LAnsprechpartner,LBriefanrede,Lieferadresse,Liefertermin,[LLand/PLZ/Ort],LName2,LName3,Lˆschen,
                [LStraﬂe/Postfach],[LVorname/NameFirma],Mahnung,Mandant,Name2,Name3,Neu,nr_ang,nr_auf,nr_BV,nr_gut,
                nr_Kanban,nr_lie,nr_pro,nr_RA,nr_rec,nr_sto,÷ffnen,[Personal-Nr],rec_sent,reparatur_nr,Status,
                [Straﬂe/Postfach],termin_eingehalten,Typ,[Unser Zeichen],USt_Berechnen,Versandart,
                Versandarten_Auswahl,Versanddatum_Auswahl,[Vorname/NameFirma],Wunschtermin,Zahlungsweise,Zahlungsziel,[Angebot-Nr],[Projekt-Nr],[LsDeliveryDate],[LsAddressNr],[StorageLocation],[UnloadingPoint])
                OUTPUT INSERTED.[Nr] VALUES
                (@Neu_Order,@ab_id,@ABSENDER,@Abteilung,@Anrede,@Ansprechpartner,@Auswahl,
                @Belegkreis,@Bemerkung,@Benutzer,@Bereich,@Bezug,@Briefanrede,@datueber,@Datum,@Debitorennummer,
                @Dplatz_Sirona,@EDI_Dateiname_CSV,@EDI_Kundenbestellnummer,@EDI_Order_Change,@EDI_Order_Change_Updated,
                @EDI_Order_Neu,@erledigt,@F‰lligkeit,@Freie_Text,@Freitext,@gebucht,@gedruckt,@Ihr_Zeichen,
                @In_Bearbeitung,@Interessent,@Konditionen,@Kunden_Nr,@LAbteilung,@Land_PLZ_Ort,@LAnrede,
                @LAnsprechpartner,@LBriefanrede,@Lieferadresse,@Liefertermin,@LLand_PLZ_Ort,@LName2,@LName3,@Lˆschen,
                @LStraﬂe_Postfach,@LVorname_NameFirma,@Mahnung,@Mandant,@Name2,@Name3,@Neu,@nr_ang,@nr_auf,@nr_BV,@nr_gut,
                @nr_Kanban,@nr_lie,@nr_pro,@nr_RA,@nr_rec,@nr_sto,@÷ffnen,@Personal_Nr,@rec_sent,@reparatur_nr,@Status,
                @Straﬂe_Postfach,@termin_eingehalten,@Typ,@Unser_Zeichen,@USt_Berechnen,@Versandart,
                @Versandarten_Auswahl,@Versanddatum_Auswahl,@Vorname_NameFirma,@Wunschtermin,@Zahlungsweise,@Zahlungsziel,@NewAngebotNr,@NewAngebotNr,@LsDeliveryDate,@LsAddressNr,@StorageLocation,@UnloadingPoint)
                ";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ab_id", element.Ab_id == null ? (object)DBNull.Value : element.Ab_id);
				sqlCommand.Parameters.AddWithValue("Neu_Order", element.Neu_Order == null ? (object)DBNull.Value : element.Neu_Order);
				sqlCommand.Parameters.AddWithValue("ABSENDER", element.ABSENDER == null ? (object)DBNull.Value : element.ABSENDER);
				sqlCommand.Parameters.AddWithValue("Abteilung", element.Abteilung == null ? (object)DBNull.Value : element.Abteilung);
				sqlCommand.Parameters.AddWithValue("Anrede", element.Anrede == null ? (object)DBNull.Value : element.Anrede);
				sqlCommand.Parameters.AddWithValue("Ansprechpartner", element.Ansprechpartner == null ? (object)DBNull.Value : element.Ansprechpartner);
				sqlCommand.Parameters.AddWithValue("Auswahl", element.Auswahl == null ? (object)DBNull.Value : element.Auswahl);
				sqlCommand.Parameters.AddWithValue("Belegkreis", element.Belegkreis == null ? (object)DBNull.Value : element.Belegkreis);
				sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung == null ? (object)DBNull.Value : element.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Benutzer", element.Benutzer == null ? (object)DBNull.Value : element.Benutzer);
				sqlCommand.Parameters.AddWithValue("Bereich", element.Bereich == null ? (object)DBNull.Value : element.Bereich);
				sqlCommand.Parameters.AddWithValue("Bezug", element.Bezug == null ? (object)DBNull.Value : element.Bezug);
				sqlCommand.Parameters.AddWithValue("Briefanrede", element.Briefanrede == null ? (object)DBNull.Value : element.Briefanrede);
				sqlCommand.Parameters.AddWithValue("datueber", element.Datueber == null ? (object)DBNull.Value : element.Datueber);
				sqlCommand.Parameters.AddWithValue("Datum", !element.Datum.HasValue ? (object)DBNull.Value : element.Datum.Value.Date); // << minor changes
				sqlCommand.Parameters.AddWithValue("Debitorennummer", element.Debitorennummer == null ? (object)DBNull.Value : element.Debitorennummer);
				sqlCommand.Parameters.AddWithValue("Dplatz_Sirona", element.Dplatz_Sirona == null ? (object)DBNull.Value : element.Dplatz_Sirona);
				sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV", element.EDI_Dateiname_CSV == null ? (object)DBNull.Value : element.EDI_Dateiname_CSV);
				sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer", element.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : element.EDI_Kundenbestellnummer);
				sqlCommand.Parameters.AddWithValue("EDI_Order_Change", element.EDI_Order_Change == null ? (object)DBNull.Value : element.EDI_Order_Change);
				sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated", element.EDI_Order_Change_Updated == null ? (object)DBNull.Value : element.EDI_Order_Change_Updated);
				sqlCommand.Parameters.AddWithValue("EDI_Order_Neu", element.EDI_Order_Neu == null ? (object)DBNull.Value : element.EDI_Order_Neu);
				sqlCommand.Parameters.AddWithValue("erledigt", element.Erledigt == null ? (object)DBNull.Value : element.Erledigt);
				sqlCommand.Parameters.AddWithValue("F‰lligkeit", !element.Falligkeit.HasValue ? (object)DBNull.Value : element.Falligkeit.Value.Date); // << minor changes
				sqlCommand.Parameters.AddWithValue("Freie_Text", element.Freie_Text == null ? (object)DBNull.Value : element.Freie_Text);
				sqlCommand.Parameters.AddWithValue("Freitext", element.Freitext == null ? (object)DBNull.Value : element.Freitext);
				sqlCommand.Parameters.AddWithValue("gebucht", element.Gebucht == null ? (object)DBNull.Value : element.Gebucht);
				sqlCommand.Parameters.AddWithValue("gedruckt", element.Gedruckt == null ? (object)DBNull.Value : element.Gedruckt);
				sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", element.Ihr_Zeichen == null ? (object)DBNull.Value : element.Ihr_Zeichen);
				sqlCommand.Parameters.AddWithValue("In_Bearbeitung", element.In_Bearbeitung == null ? (object)DBNull.Value : element.In_Bearbeitung);
				sqlCommand.Parameters.AddWithValue("Interessent", element.Interessent == null ? (object)DBNull.Value : element.Interessent);
				sqlCommand.Parameters.AddWithValue("Konditionen", element.Konditionen == null ? (object)DBNull.Value : element.Konditionen);
				sqlCommand.Parameters.AddWithValue("Kunden_Nr", element.Kunden_Nr == null ? (object)DBNull.Value : element.Kunden_Nr);
				sqlCommand.Parameters.AddWithValue("LAbteilung", element.LAbteilung == null ? (object)DBNull.Value : element.LAbteilung);
				sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", element.Land_PLZ_Ort == null ? (object)DBNull.Value : element.Land_PLZ_Ort);
				sqlCommand.Parameters.AddWithValue("LAnrede", element.LAnrede == null ? (object)DBNull.Value : element.LAnrede);
				sqlCommand.Parameters.AddWithValue("LAnsprechpartner", element.LAnsprechpartner == null ? (object)DBNull.Value : element.LAnsprechpartner);
				sqlCommand.Parameters.AddWithValue("LBriefanrede", element.LBriefanrede == null ? (object)DBNull.Value : element.LBriefanrede);
				sqlCommand.Parameters.AddWithValue("Lieferadresse", element.Lieferadresse == null ? (object)DBNull.Value : element.Lieferadresse);
				sqlCommand.Parameters.AddWithValue("Liefertermin", element.Liefertermin == null ? (object)DBNull.Value : element.Liefertermin);
				sqlCommand.Parameters.AddWithValue("LLand_PLZ_Ort", element.LLand_PLZ_Ort == null ? (object)DBNull.Value : element.LLand_PLZ_Ort);
				sqlCommand.Parameters.AddWithValue("LName2", element.LName2 == null ? (object)DBNull.Value : element.LName2);
				sqlCommand.Parameters.AddWithValue("LName3", element.LName3 == null ? (object)DBNull.Value : element.LName3);
				sqlCommand.Parameters.AddWithValue("Lˆschen", element.Loschen == null ? (object)DBNull.Value : element.Loschen);
				sqlCommand.Parameters.AddWithValue("LStraﬂe_Postfach", element.LStraﬂe_Postfach == null ? (object)DBNull.Value : element.LStraﬂe_Postfach);
				sqlCommand.Parameters.AddWithValue("LVorname_NameFirma", element.LVorname_NameFirma == null ? (object)DBNull.Value : element.LVorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("Mahnung", element.Mahnung == null ? (object)DBNull.Value : element.Mahnung);
				sqlCommand.Parameters.AddWithValue("Mandant", element.Mandant == null ? (object)DBNull.Value : element.Mandant);
				sqlCommand.Parameters.AddWithValue("Name2", element.Name2 == null ? (object)DBNull.Value : element.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", element.Name3 == null ? (object)DBNull.Value : element.Name3);
				sqlCommand.Parameters.AddWithValue("Neu", element.Neu == null ? (object)DBNull.Value : element.Neu);
				sqlCommand.Parameters.AddWithValue("nr_ang", element.Nr_ang == null ? (object)DBNull.Value : element.Nr_ang);
				sqlCommand.Parameters.AddWithValue("nr_auf", element.Nr_auf == null ? (object)DBNull.Value : element.Nr_auf);
				sqlCommand.Parameters.AddWithValue("nr_BV", element.Nr_BV == null ? (object)DBNull.Value : element.Nr_BV);
				sqlCommand.Parameters.AddWithValue("nr_gut", element.Nr_gut == null ? (object)DBNull.Value : element.Nr_gut);
				sqlCommand.Parameters.AddWithValue("nr_Kanban", element.Nr_Kanban == null ? (object)DBNull.Value : element.Nr_Kanban);
				sqlCommand.Parameters.AddWithValue("nr_lie", element.Nr_lie == null ? (object)DBNull.Value : element.Nr_lie);
				sqlCommand.Parameters.AddWithValue("nr_pro", element.Nr_pro == null ? (object)DBNull.Value : element.Nr_pro);
				sqlCommand.Parameters.AddWithValue("nr_RA", element.Nr_RA == null ? (object)DBNull.Value : element.Nr_RA);
				sqlCommand.Parameters.AddWithValue("nr_rec", element.Nr_rec == null ? (object)DBNull.Value : element.Nr_rec);
				sqlCommand.Parameters.AddWithValue("nr_sto", element.Nr_sto == null ? (object)DBNull.Value : element.Nr_sto);
				sqlCommand.Parameters.AddWithValue("÷ffnen", element.Offnen == null ? (object)DBNull.Value : element.Offnen);
				sqlCommand.Parameters.AddWithValue("Personal_Nr", element.Personal_Nr == null ? (object)DBNull.Value : element.Personal_Nr);
				sqlCommand.Parameters.AddWithValue("rec_sent", element.rec_sent == null ? (object)DBNull.Value : element.rec_sent);
				sqlCommand.Parameters.AddWithValue("reparatur_nr", element.Reparatur_nr == null ? (object)DBNull.Value : element.Reparatur_nr);
				sqlCommand.Parameters.AddWithValue("Status", element.Status == null ? (object)DBNull.Value : element.Status);
				sqlCommand.Parameters.AddWithValue("Straﬂe_Postfach", element.Straﬂe_Postfach == null ? (object)DBNull.Value : element.Straﬂe_Postfach);
				sqlCommand.Parameters.AddWithValue("termin_eingehalten", element.Termin_eingehalten == null ? (object)DBNull.Value : element.Termin_eingehalten);
				sqlCommand.Parameters.AddWithValue("Typ", element.Typ == null ? (object)DBNull.Value : element.Typ);
				sqlCommand.Parameters.AddWithValue("Unser_Zeichen", element.Unser_Zeichen == null ? (object)DBNull.Value : element.Unser_Zeichen);
				sqlCommand.Parameters.AddWithValue("USt_Berechnen", element.USt_Berechnen == null ? (object)DBNull.Value : element.USt_Berechnen);
				sqlCommand.Parameters.AddWithValue("Versandart", element.Versandart == null ? (object)DBNull.Value : element.Versandart);
				sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl", element.Versandarten_Auswahl == null ? (object)DBNull.Value : element.Versandarten_Auswahl);
				sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl", element.Versanddatum_Auswahl == null ? (object)DBNull.Value : element.Versanddatum_Auswahl);
				sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", element.Vorname_NameFirma == null ? (object)DBNull.Value : element.Vorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("Wunschtermin", element.Wunschtermin == null ? (object)DBNull.Value : element.Wunschtermin);
				sqlCommand.Parameters.AddWithValue("Zahlungsweise", element.Zahlungsweise == null ? (object)DBNull.Value : element.Zahlungsweise);
				sqlCommand.Parameters.AddWithValue("Zahlungsziel", element.Zahlungsziel == null ? (object)DBNull.Value : element.Zahlungsziel);
				sqlCommand.Parameters.AddWithValue("LsDeliveryDate", element.LsDeliveryDate == null ? (object)DBNull.Value : element.LsDeliveryDate);
				sqlCommand.Parameters.AddWithValue("LsAddressNr", element.LsAddressNr == null ? (object)DBNull.Value : element.LsAddressNr);
				sqlCommand.Parameters.AddWithValue("StorageLocation", element.StorageLocation == null ? (object)DBNull.Value : element.StorageLocation);
				sqlCommand.Parameters.AddWithValue("UnloadingPoint", element.UnloadingPoint == null ? (object)DBNull.Value : element.UnloadingPoint);

				sqlCommand.Parameters.AddWithValue("maxCurrentValue", maxCurrentValue);
				sqlCommand.Parameters.AddWithValue("minNewValue", minNewValue);
				sqlCommand.Parameters.AddWithValue("type", type);

				var result = sqlCommand.ExecuteScalar();
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
			return response;
		}
		public static void UpdateWithTransaction(int nr, string angebotNr, string projektNr, string benutzer, bool gebucht, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE Angebote SET [Angebot-Nr]=@angebotNr, [Projekt-Nr]=@projektNr, [Benutzer]=@benutzer, [gebucht]=@gebucht WHERE Nr=@nr";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);
			sqlCommand.Parameters.AddWithValue("projektNr", projektNr);
			sqlCommand.Parameters.AddWithValue("benutzer", benutzer);
			sqlCommand.Parameters.AddWithValue("gebucht", gebucht);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

			sqlCommand.ExecuteNonQuery();
		}
		public static void UpdateAbIdWithTransaction(int nr, int abId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE Angebote SET ab_id=@ab_id WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ab_id", abId);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

			sqlCommand.ExecuteNonQuery();
		}
		public static int UpdateCustomerUstIdWithTransaction(int nr, string ustId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE Angebote SET [Freie_Text]=CAST((CAST([Freie_Text] AS NVARCHAR(MAX)) + ' '+ CAST([Freitext] AS NVARCHAR(MAX))) AS NTEXT), [Freitext]=@ustId WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ustId", ustId);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

			return sqlCommand.ExecuteNonQuery();
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetConfirmationByKundenNr(int kundenkNr)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM [Angebote] WHERE [Kunden-Nr]=@kundenkNr AND typ=@typ";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("kundenkNr", kundenkNr);
			sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static IEnumerable<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetConfirmationByKundenNr(IEnumerable<int> kundenkNrs,
			string? searchText, DateTime? startDate, DateTime? endDate,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging)
		{
			if(kundenkNrs == null || kundenkNrs.Count() <= 0)
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $"SELECT * FROM [Angebote] WHERE [Kunden-Nr] IN ({string.Join(",", kundenkNrs)}) AND typ=@typ";
			if(!string.IsNullOrWhiteSpace(searchText))
			{
				query += $" AND ([Angebot-Nr] LIKE '%{searchText.SqlEscape()}%' OR [Bezug] LIKE '%{searchText.SqlEscape()}%')";
			}
			if(startDate.HasValue)
			{
				query += $" AND ([Datum]>='{startDate.Value.ToString("yyyyMMdd")}')";
			}
			if(endDate.HasValue)
			{
				query += $" AND ([Datum]<='{endDate.Value.ToString("yyyyMMdd")}')";
			}
			if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
			{
				query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
			}
			else
			{
				query += " ORDER BY Datum DESC ";
			}
			if(paging != null)
			{
				query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
			}

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x));
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetUnconfirmedConfirmationByKundenNrEDI(List<int> kundenkNrs)
		{
			if(kundenkNrs == null || kundenkNrs.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $"SELECT * FROM [Angebote] WHERE [Kunden-Nr] IN ({string.Join(",", kundenkNrs)}) AND typ=@typ AND Neu_Order=1 AND IsNULL(EDI_Dateiname_CSV, '')<>'' AND IsNULL([Angebot-nr],0)<=0 ";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByTyp(string typ)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Angebote WHERE [Typ]=@typ";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("typ", typ);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static int CountCustomerOrdersByIsNew(bool isNew)
		{
			try
			{
				var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS OrdersCount FROM Angebote WHERE [Typ]=@typ AND Neu_Order=@isNew";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);
				sqlCommand.Parameters.AddWithValue("isNew", isNew);

				sqlConnection.Close();

				var dataTable = new DataTable();

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count == 0)
				{
					return 0;
				}
				else
				{
					var value = (dataTable.Rows[0]["OrdersCount"] == System.DBNull.Value)
						? (int?)null
						: Convert.ToInt32(dataTable.Rows[0]["OrdersCount"]);

					return value ?? 0;
				}
			} catch(Exception e)
			{
				return 0;
			}
		}
		public static int CountCustomerOrdersByIsNew(bool isNew, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			try
			{
				string query = "SELECT COUNT(*) AS OrdersCount FROM Angebote WHERE [Typ]=@typ AND ISNULL(Neu_Order,0)=@isNew AND ISNULL(EDI_Dateiname_CSV,'')<>''";

				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

				sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);
				sqlCommand.Parameters.AddWithValue("isNew", isNew);

				var dataTable = new DataTable();

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count == 0)
				{
					return 0;
				}
				else
				{
					var value = (dataTable.Rows[0]["OrdersCount"] == System.DBNull.Value)
						? (int?)null
						: Convert.ToInt32(dataTable.Rows[0]["OrdersCount"]);

					return value ?? 0;
				}
			} catch(Exception e)
			{
				return 0;
			}
		}

		public static bool UpdateNeuOrder(int orderId, bool neuOrder)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE Angebote SET Neu_Order=@neuOrder WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Nr", orderId);
			sqlCommand.Parameters.AddWithValue("neuOrder", neuOrder);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response > 0;
		}
		public static bool UpdateNeuOrder(int orderId, bool neuOrder, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "UPDATE Angebote SET Neu_Order=@neuOrder WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Nr", orderId);
			sqlCommand.Parameters.AddWithValue("neuOrder", neuOrder);

			int response = sqlCommand.ExecuteNonQuery();


			return response > 0;
		}

		public static void UpdateAbId(int nr, int abId)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE Angebote SET ab_id=@ab_id WHERE Nr=@Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ab_id", abId);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				sqlCommand.ExecuteNonQuery();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetCustomerOrdersByNeuOrder(bool neuOrder)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Angebote WHERE [Typ]=@typ AND Neu_Order=@neuOrder";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);
			sqlCommand.Parameters.AddWithValue("neuOrder", neuOrder);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetCustomerOrdersByNeuOrder(bool neuOrder, int customerNr)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Angebote WHERE [Typ]=@typ AND ISNULL(Neu_Order,0)=@neuOrder AND ISNULL(EDI_Dateiname_CSV,'')<>'' AND [Kunden-Nr]=@customerNr";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);
			sqlCommand.Parameters.AddWithValue("neuOrder", neuOrder);
			sqlCommand.Parameters.AddWithValue("customerNr", customerNr);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetCustomerOrdersByNeuOrder(bool neuOrder, IEnumerable<int> customerNrs)
		{
			if(customerNrs == null || customerNrs.Count() <= 0)
			{
				return null;
			}
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $"SELECT * FROM Angebote WHERE [Typ]=@typ AND ISNULL(Neu_Order,0)=@neuOrder AND ISNULL(EDI_Dateiname_CSV,'')<>'' AND [Kunden-Nr] IN ({string.Join(",", customerNrs)})";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);
			sqlCommand.Parameters.AddWithValue("neuOrder", neuOrder);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}

		public static IEnumerable<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetCustomerOrdersByNeuOrder(bool neuOrder, IEnumerable<int> customerNrs,
			string? searchText, DateTime? startDate, DateTime? endDate,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging)
		{
			if(customerNrs == null || customerNrs.Count() <= 0)
			{
				return null;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM Angebote WHERE [Typ]=@typ AND ISNULL(Neu_Order,0)=@neuOrder AND ISNULL(EDI_Dateiname_CSV,'')<>'' AND [Kunden-Nr] IN ({string.Join(",", customerNrs)})";
				if(!string.IsNullOrWhiteSpace(searchText))
				{
					query += $" AND ([Angebot-Nr] LIKE '%{searchText.SqlEscape()}%' OR [Bezug] LIKE '%{searchText.SqlEscape()}%')";
				}
				if(startDate.HasValue)
				{
					query += $" AND ([Datum]>='{startDate.Value.ToString("yyyyMMdd")}')";
				}
				if(endDate.HasValue)
				{
					query += $" AND ([Datum]<='{endDate.Value.ToString("yyyyMMdd")}')";
				}

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Datum DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);
				sqlCommand.Parameters.AddWithValue("neuOrder", neuOrder);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x));
			}
			else
			{
				return null;
			}
		}
		public static IEnumerable<KeyValuePair<int, int>> GetCustomerOrdersByNeuOrderCounts(bool? neuOrder, IEnumerable<int> customerNrs)
		{
			if(customerNrs == null || customerNrs.Count() <= 0)
			{
				return null;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT [Kunden-Nr], count(*) counts FROM Angebote WHERE [Typ]=@typ AND ISNULL(EDI_Dateiname_CSV,'')<>'' AND [Kunden-Nr] IN ({string.Join(",", customerNrs)}){(!neuOrder.HasValue ? "" : ($"AND ISNULL(Neu_Order,0)={(neuOrder.Value ? "1" : 0)}"))} GROUP BY [Kunden-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, int>(int.TryParse(x[0].ToString(), out int _x) ? _x : 0, int.TryParse(x[1].ToString(), out int _y) ? _y : 0));
			}
			else
			{
				return null;
			}
		}
		public static int GetCustomerOrdersByNeuOrderCount(bool? neuOrder, IEnumerable<int> customerNrs)
		{
			if(customerNrs == null || customerNrs.Count() <= 0)
			{
				return 0;
			}

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT count(*) counts FROM Angebote WHERE [Typ]=@typ AND ISNULL(EDI_Dateiname_CSV,'')<>'' AND [Kunden-Nr] IN ({string.Join(",", customerNrs)}){(!neuOrder.HasValue ? "" : ($"AND ISNULL(Neu_Order,0)={(neuOrder.Value ? "1" : 0)}"))}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int _x) ? _x : 0;
			}
		}
		public static int GetCustomerOrdersByNeuOrderCount(bool? neuOrder)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT count(*) counts FROM Angebote WHERE [Typ]=@typ AND ISNULL(EDI_Dateiname_CSV,'')<>'' {(!neuOrder.HasValue ? "" : ($"AND ISNULL(Neu_Order,0)={(neuOrder.Value ? "1" : 0)}"))}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int _x) ? _x : 0;
			}
		}

		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity GetByBezugAndKundenNr(string bezug,
			int kundenkNr, string type)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = !string.IsNullOrEmpty(type) && !string.IsNullOrWhiteSpace(type)
				? $"SELECT * FROM Angebote WHERE [Bezug]=@bezug AND [Kunden-Nr]=@kundenkNr AND Typ='{type}'"
				: "SELECT * FROM Angebote WHERE [Bezug]=@bezug AND [Kunden-Nr]=@kundenkNr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("bezug", bezug);
			sqlCommand.Parameters.AddWithValue("kundenkNr", kundenkNr);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity GetByBezugAndKundenNr(string bezug,
			int kundenkNr, string type, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = !string.IsNullOrEmpty(type) && !string.IsNullOrWhiteSpace(type)
				? $"SELECT * FROM Angebote WHERE [Bezug]=@bezug AND [Kunden-Nr]=@kundenkNr AND Typ='{type}'"
				: "SELECT * FROM Angebote WHERE [Bezug]=@bezug AND [Kunden-Nr]=@kundenkNr";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("bezug", bezug);
			sqlCommand.Parameters.AddWithValue("kundenkNr", kundenkNr);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static int GetCountByCustomer(int Kunden_nr, string type = "Auftragsbest‰tigung", bool? onlyValidated = null, bool? closed = null)
		{
			try
			{
				var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
				sqlConnection.Open();

				string query = $"SELECT COUNT(*) AS KundenCount FROM Angebote WHERE [Kunden-Nr]=@Kunden_nr AND Typ='{type}'{(onlyValidated.HasValue ? $" AND IsNULL(Gebucht,0)={(onlyValidated == true ? "1" : "0")}" : "")}{(closed.HasValue ? $" AND IsNULL(Erledigt,0)={(closed == true ? "1" : "0")}" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Kunden_nr", Kunden_nr);

				sqlConnection.Close();

				var dataTable = new DataTable();

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count == 0)
				{
					return 0;
				}
				else
				{
					var value = (dataTable.Rows[0]["KundenCount"] == System.DBNull.Value)
						? (int?)null
						: Convert.ToInt32(dataTable.Rows[0]["KundenCount"]);

					return value ?? 0;
				}
			} catch(Exception e)
			{
				return 0;
			}
		}
		public static List<int> GetIds()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT [Nr] FROM Angebote";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				var result = new List<int>();
				foreach(DataRow dataRow in dataTable.Rows)
				{
					result.Add(Convert.ToInt32(dataRow["Nr"]));
				}
				return result;
			}
			else
			{
				return new List<int>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity GetUniqueByKundenkNr(int kundenkNr, string typ, bool isContract = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Angebote] WHERE "
					+ $" [Bezug] = (SELECT max ([Bezug]) FROM [Angebote] WHERE [Kunden-Nr]=@kundenkNr and [Bezug] like '{(isContract ? "RA" : "mc")}-%' AND Typ=@typ)";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("kundenkNr", kundenkNr);
				sqlCommand.Parameters.AddWithValue("typ", typ?.Trim());
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity GetTopRABezug()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Angebote] WHERE [Nr] = (SELECT max([Nr]) FROM [Angebote] WHERE [Bezug] like 'RA-%' AND Typ='Rahmenauftrag')";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity GetUniqueByKundenkNr(int kundenkNr, string typ, string prefix)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT * FROM [Angebote] WHERE 
                 [Bezug] = (SELECT max ([Bezug]) FROM [Angebote] WHERE [Kunden-Nr]=@kundenkNr and [Bezug] like '{prefix}%' AND Typ=@typ)";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("kundenkNr", kundenkNr);
			sqlCommand.Parameters.AddWithValue("typ", typ?.Trim());

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity GetUniqueByKundenkNr(int kundenkNr, string typ, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "SELECT * FROM [Angebote] WHERE "
				+ " [Bezug] = (SELECT max ([Bezug]) FROM [Angebote] WHERE [Kunden-Nr]=@kundenkNr and [Bezug] like 'mc-%' AND Typ=@typ)";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("kundenkNr", kundenkNr);
			sqlCommand.Parameters.AddWithValue("typ", typ?.Trim());

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity GetUniqueAngeboteByNr(int? angeboteNr)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Angebote WHERE [Nr]=@angebotNr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("angebotNr", angeboteNr);
			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByBezug(string bezug,
			Data.Access.Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM Angebote WHERE [Bezug]=@bezug "
					+ " ORDER BY Datum DESC "
					+ (paging != null
						? $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY "
						: string.Empty);

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("bezug", bezug);

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static int CountByBezug(string bezug,
			Data.Access.Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT([Nr]) AS CountNr FROM Angebote WHERE [Bezug]=@bezug "
					+ (paging != null
						? $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY "
						: string.Empty);

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("bezug", bezug);

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0]["CountNr"]);
			}
			else
			{
				return 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByProjectNr(string projektNr,
			Data.Access.Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM Angebote WHERE [Projekt-Nr]=@projektNr "
					+ " ORDER BY Datum DESC "
					+ (paging != null
						? $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY "
						: string.Empty);

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("projektNr", projektNr);

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByProjectNr2(int projektNr, string typ)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Angebote] WHERE [Projekt-Nr]=@projektNr AND [Typ]=@typ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("projektNr", projektNr);
					sqlCommand.Parameters.AddWithValue("typ", typ);
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static int CountByProjectNr(string projektNr,
			Data.Access.Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT([Nr]) AS CountNr FROM Angebote WHERE [Projekt-Nr]=@projektNr "
					+ (paging != null
						? $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY "
						: string.Empty);

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("projektNr", projektNr);

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0]["CountNr"]);
			}
			else
			{
				return 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByAngebotNr(string angebotNr,
			Data.Access.Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM Angebote WHERE [Angebot-Nr]=@angebotNr "
					+ " ORDER BY Datum DESC "
					+ (paging != null
						? $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY "
						: string.Empty);

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByAngebotNr(string angebotNr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM Angebote WHERE [Angebot-Nr]=@angebotNr  ORDER BY Datum DESC ";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static int CountByAngebotNr(string angebotNr,
			Data.Access.Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT([Nr]) AS CountNr FROM Angebote WHERE [Angebot-Nr]=@angebotNr "
					+ (paging != null
						? $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY "
						: string.Empty);

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0]["CountNr"]);
			}
			else
			{
				return 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByTypsAndKundenNrs(List<string> typs,
			List<int> kundenNrs,
			Data.Access.Settings.PaginModel paging = null, string searchText = null)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM Angebote ";

				using(var sqlCommand = new SqlCommand())
				{
					bool isFirstCondition = true;

					if(typs != null && typs.Count > 0)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} [Typ] IN ({string.Join(", ", typs.Select(t => @$"'{t}'"))}) ";
						isFirstCondition = false;
					}

					if(kundenNrs != null && kundenNrs.Count > 0)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} [Kunden-Nr] IN ({string.Join(", ", kundenNrs)}) ";
					}
					if(!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} (([Projekt-Nr] LIKE '{searchText}%') OR ([Angebot-Nr] LIKE '{searchText}%') OR ([Bezug] LIKE '{searchText}%'))";
					}

					query += " ORDER BY Datum DESC ";

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByTypsAndKundenNrsDate(List<string> typs,
			List<int> kundenNrs, DateTime minDate,
			Data.Access.Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM Angebote WHERE Datum > '{minDate.ToString("yyyyMMdd")}' ";

				using(var sqlCommand = new SqlCommand())
				{

					if(typs != null && typs.Count > 0)
					{
						query += $" AND [Typ] IN ({string.Join(", ", typs)}) ";
					}

					if(kundenNrs != null && kundenNrs.Count > 0)
					{
						query += $" AND [Kunden-Nr] IN ({string.Join(", ", kundenNrs)}) ";
					}

					query += " ORDER BY Datum DESC ";

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static int CountByTypsAndKundenNrs(List<string> typs,
			List<int> kundenNrs)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT([Nr]) AS CountNr FROM [Angebote] ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					bool isFirstCondition = true;

					if(typs != null && typs.Count > 0)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} [Typ] IN ({string.Join(", ", typs)}) ";
						isFirstCondition = false;
					}

					if(kundenNrs != null && kundenNrs.Count > 0)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} [Kunden-Nr] IN ({string.Join(", ", kundenNrs)}) ";
					}

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0]["CountNr"]);
			}
			else
			{
				return 0;
			}
		}
		public static List<Tuple<int, string>> Get_TupleKundenNrBezug_Like_Bezug(string searchText)
		{
			searchText = (searchText ?? "").Trim();
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $" SELECT DISTINCT TOP 10 [Kunden-Nr],[Bezug] FROM [Angebote] WHERE [Bezug] LIKE '{searchText}%' ORDER BY [Bezug]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Tuple<int, string>>();
			}

			var response = new List<Tuple<int, string>>();
			foreach(DataRow dataRow in dataTable.Rows)
			{
				if(dataRow["Kunden-Nr"] != System.DBNull.Value && dataRow["Bezug"] != System.DBNull.Value)
				{
					response.Add(new Tuple<int, string>(Convert.ToInt32(dataRow["Kunden-Nr"]), Convert.ToString(dataRow["Bezug"])));
				}
			}
			return response;
		}
		public static List<Tuple<int, string>> Get_TupleKundenNrAngebotNr_Like_AngebotNr(string searchText)
		{
			searchText = (searchText ?? "").Trim();
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $" SELECT DISTINCT TOP 10 [Kunden-Nr],[Angebot-Nr] FROM [Angebote] WHERE [Angebot-Nr] LIKE '{searchText}%'  ORDER BY [Angebot-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Tuple<int, string>>();
			}

			var response = new List<Tuple<int, string>>();
			foreach(DataRow dataRow in dataTable.Rows)
			{
				if(dataRow["Kunden-Nr"] != System.DBNull.Value && dataRow["Angebot-Nr"] != System.DBNull.Value)
				{
					response.Add(new Tuple<int, string>(Convert.ToInt32(dataRow["Kunden-Nr"]), Convert.ToString(dataRow["Angebot-Nr"])));
				}
			}
			return response;
		}
		public static List<Tuple<int, string>> Get_TupleKundenNrProjektNr_Like_ProjektNr(string searchText)
		{
			searchText = (searchText ?? "").Trim();
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $" SELECT DISTINCT TOP 10 [Kunden-Nr],[Projekt-Nr] FROM [Angebote] "
					+ $" WHERE [Projekt-Nr] LIKE '{searchText}%' ORDER BY [Projekt-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Tuple<int, string>>();
			}

			var response = new List<Tuple<int, string>>();
			foreach(DataRow dataRow in dataTable.Rows)
			{
				if(dataRow["Kunden-Nr"] != System.DBNull.Value && dataRow["Projekt-Nr"] != System.DBNull.Value)
				{
					response.Add(new Tuple<int, string>(Convert.ToInt32(dataRow["Kunden-Nr"]), Convert.ToString(dataRow["Projekt-Nr"])));
				}
			}
			return response;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetBy_Bezug_ProjectNr_AngebotNr_Typs_KundenNrs(string bezug,
			bool onlyUnbooked,
			bool onlyEmptyProjektNr,
			string projektNr,
			string angebotNr,
			List<string> typs,
			List<int> kundenNrs,
			string rechnungTyp,
			int? rahmenType,
			DateTime? ramenFrom,
			DateTime? rahmenTo,
			DateTime? createdFrom,
			DateTime? createdTo,
			List<string> exceptTypes,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging, bool onlyExpiredRahmens = false)
		{
			var dataTable = new DataTable();

			bool isFirstCondition = true;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Angebote] a";
				if(onlyExpiredRahmens)
				{
					query += @" INNER JOIN [__CTS_AngeboteBlanketExtension] be ON a.[Nr]=be.[AngeboteNr]
                              INNER JOIN [__CTS_AngeboteArticleBlanketExtension] bae on bae.[RahmenNr]=be.[AngeboteNr]
                              WHERE be.[StatusId]=2 AND GETDATE()>bae.[ExtensionDate]";
					isFirstCondition = false;
				}

				using(var sqlCommand = new SqlCommand())
				{

					if(!string.IsNullOrWhiteSpace(bezug))
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Bezug] LIKE '{bezug.Trim()}%' ";
						isFirstCondition = false;
					}

					if(onlyUnbooked)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} (IsNULL([a.Gebucht],0) = 0) ";
						isFirstCondition = false;
					}
					if(onlyEmptyProjektNr)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} (a.[Projekt-Nr] IS NULL OR a.[Projekt-Nr]='') ";
						isFirstCondition = false;
					}
					else if(!string.IsNullOrWhiteSpace(projektNr))
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Projekt-Nr] = '{projektNr.Trim()}' ";
						isFirstCondition = false;
					}

					if(!string.IsNullOrWhiteSpace(angebotNr))
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Angebot-Nr] = '{(long.TryParse(angebotNr.Trim(), out var a) ? a : 0)}' ";
						isFirstCondition = false;
					}

					if(typs != null && typs.Count > 0)
					{
						var formatedTyps = new List<string>();
						typs.ForEach(e => formatedTyps.Add($"'{e}'"));

						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Typ] IN ({string.Join(", ", formatedTyps)}) ";
						isFirstCondition = false;
					}

					if(exceptTypes != null && exceptTypes.Count > 0)
					{
						var formatedTyps = new List<string>();
						exceptTypes.ForEach(e => formatedTyps.Add($"'{e}'"));

						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Typ] NOT IN ({string.Join(", ", formatedTyps)}) ";
						isFirstCondition = false;
					}

					if(kundenNrs != null && kundenNrs.Count > 0)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Kunden-Nr] IN ({string.Join(", ", kundenNrs)}) ";
						isFirstCondition = false;
					}

					if(!string.IsNullOrEmpty(rechnungTyp) && !string.IsNullOrWhiteSpace(rechnungTyp))
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Kunden-Nr] IN (select Kundennummer FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [Typ]='{rechnungTyp}') ";
						isFirstCondition = false;
					}

					if(rahmenType.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Nr] IN (SELECT [AngeboteNr] FROM [__CTS_AngeboteBlanketExtension] WHERE [BlanketTypeId]={rahmenType})";
						isFirstCondition = false;
					}

					if(ramenFrom.HasValue || rahmenTo.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Nr] in (SELECT DISTINCT [RahmenNr] FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [GultigAb]>='{(ramenFrom.HasValue ? ramenFrom.Value.ToString("dd/MM/yyyy") : "")}' and GultigBis<='{(rahmenTo.HasValue ? rahmenTo.Value.ToString("dd/MM/yyyy") : "")}')";
					}
					if(createdFrom.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Datum]>='{createdFrom.Value.ToString("dd/MM/yyyy")}'";
					}
					if(createdTo.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Datum]<='{createdTo.Value.ToString("dd/MM/yyyy")}'";
					}
					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					else
					{
						query += " ORDER BY a.Datum DESC ";
					}

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}

		public static int CountBy_Bezug_ProjectNr_AngebotNr_Typs_KundenNrs(string bezug,
			bool onlyUnbooked,
			bool onlyEmptyProjektNr,
			string projektNr,
			string angebotNr,
			string rechnungTyp,
			int? rahmenType,
			DateTime? ramenFrom,
			DateTime? rahmenTo,
			DateTime? createdFrom,
			DateTime? createdTo,
			List<string> typs,
			List<int> kundenNrs,
			List<string> exceptTypes, bool onlyExpiredRahmens = false)
		{
			var dataTable = new DataTable();

			bool isFirstCondition = true;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT([Nr]) AS CountNr FROM [Angebote] a";
				if(onlyExpiredRahmens)
				{
					query += @" INNER JOIN [__CTS_AngeboteBlanketExtension] be ON a.[Nr]=be.[AngeboteNr]
                              INNER JOIN [__CTS_AngeboteArticleBlanketExtension] bae on bae.[RahmenNr]=be.[AngeboteNr]
                              WHERE be.[StatusId]=2 AND GETDATE()>bae.[ExtensionDate]";
					isFirstCondition = false;
				}

				using(var sqlCommand = new SqlCommand())
				{

					if(!string.IsNullOrWhiteSpace(bezug))
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Bezug] LIKE '{bezug.Trim()}%' ";
						isFirstCondition = false;
					}

					if(onlyUnbooked)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} (IsNULL(a.[Gebucht],0) = 0) ";
						isFirstCondition = false;
					}
					if(onlyEmptyProjektNr)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} (a.[Projekt-Nr] IS NULL OR a.[Projekt-Nr]='') ";
						isFirstCondition = false;
					}
					else if(!string.IsNullOrWhiteSpace(projektNr))
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Projekt-Nr] = '{projektNr.Trim()}' ";
						isFirstCondition = false;
					}

					if(!string.IsNullOrWhiteSpace(angebotNr))
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Angebot-Nr] = '{angebotNr.Trim()}' ";
						isFirstCondition = false;
					}

					if(typs != null && typs.Count > 0)
					{
						var formatedTyps = new List<string>();
						typs.ForEach(e => formatedTyps.Add($"'{e}'"));

						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Typ] IN ({string.Join(", ", formatedTyps)}) ";
						isFirstCondition = false;
					}

					if(exceptTypes != null && exceptTypes.Count > 0)
					{
						var formatedTyps = new List<string>();
						exceptTypes.ForEach(e => formatedTyps.Add($"'{e}'"));

						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Typ] NOT IN ({string.Join(", ", formatedTyps)}) ";
						isFirstCondition = false;
					}

					if(kundenNrs != null && kundenNrs.Count > 0)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Kunden-Nr] IN ({string.Join(", ", kundenNrs)}) ";
						isFirstCondition = false;
					}
					if(!string.IsNullOrEmpty(rechnungTyp) && !string.IsNullOrWhiteSpace(rechnungTyp))
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Kunden-Nr] IN (select Kundennummer FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [Typ]='{rechnungTyp}') ";
						isFirstCondition = false;
					}
					if(rahmenType.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Nr] IN (SELECT [AngeboteNr] FROM [__CTS_AngeboteBlanketExtension] WHERE [BlanketTypeId]={rahmenType})";
						isFirstCondition = false;
					}

					if(ramenFrom.HasValue || rahmenTo.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Nr] in (SELECT DISTINCT [RahmenNr] FROM [__CTS_AngeboteArticleBlanketExtension] WHERE [GultigAb]>='{(ramenFrom.HasValue ? ramenFrom.Value.ToString("dd/MM/yyyy") : "")}' and GultigBis<='{(rahmenTo.HasValue ? rahmenTo.Value.ToString("dd/MM/yyyy") : "")}')";
					}
					if(createdFrom.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Datum]>='{createdFrom.Value.ToString("dd/MM/yyyy")}'";
					}
					if(createdTo.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} a.[Datum]<='{createdTo.Value.ToString("dd/MM/yyyy")}'";
					}
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return 0;
			}

			return Convert.ToInt32(dataTable.Rows[0]["CountNr"]);
		}

		public static void Update(int nr, string angebotNr, string projektNr, string benutzer)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE Angebote SET [Angebot-Nr]=@angebotNr, [Projekt-Nr]=@projektNr, [Benutzer]=@benutzer WHERE Nr=@nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);
				sqlCommand.Parameters.AddWithValue("projektNr", projektNr);
				sqlCommand.Parameters.AddWithValue("benutzer", benutzer);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				sqlCommand.ExecuteNonQuery();
			}
		}
		public static void Update(int nr, string angebotNr, string projektNr, string benutzer, bool gebucht)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE Angebote SET [Angebot-Nr]=@angebotNr, [Projekt-Nr]=@projektNr, [Benutzer]=@benutzer, [gebucht]=@gebucht WHERE Nr=@nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);
				sqlCommand.Parameters.AddWithValue("projektNr", projektNr);
				sqlCommand.Parameters.AddWithValue("benutzer", benutzer);
				sqlCommand.Parameters.AddWithValue("gebucht", gebucht);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				sqlCommand.ExecuteNonQuery();
			}
		}
		public static void UpdateGebucht(int nr, bool gebucht)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE Angebote SET [gebucht]=@gebucht WHERE Nr=@nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("gebucht", gebucht);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				sqlCommand.ExecuteNonQuery();
			}
		}
		public static void UpdateGebucht(int nr, bool gebucht, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "UPDATE Angebote SET [gebucht]=@gebucht WHERE Nr=@nr";
			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("gebucht", gebucht);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				sqlCommand.ExecuteNonQuery();
			}
		}
		public static string MaxProjektNrByTyp(string typ)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT MAX ([Projekt-Nr]) AS MaxProjektNr FROM [Angebote] WHERE [Typ]=@typ ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("typ", typ);

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToString(dataTable.Rows[0]["MaxProjektNr"]);
			}
			else
			{
				return "0";
			}
		}
		public static string MaxAngebotNrByTyp(string typ)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT MAX ([Angebot-Nr]) AS MaxAngebotNr FROM [Angebote] WHERE [Typ]=@typ ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("typ", typ);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToString(dataTable.Rows[0]["MaxAngebotNr"]);
			}
			else
			{
				return "0";
			}
		}
		public static string MaxAngebotNrByTyp(string typ, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = " SELECT MAX ([Angebot-Nr]) AS MaxAngebotNr FROM [Angebote] WHERE [Typ]=@typ ";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("typ", typ);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToString(dataTable.Rows[0]["MaxAngebotNr"]);
			}
			else
			{
				return "0";
			}
		}

		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity GetConfirmation(int nr)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Angebote WHERE [Typ]=@typ AND nr=@nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);
			sqlCommand.Parameters.AddWithValue("nr", nr);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count == 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}

			return null;
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity GetConfirmationForDeliveryNote(int nr, bool gebucht = true, bool erledigt = false)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Angebote WHERE nr=@nr AND [Typ]=@typ AND gebucht=@gebucht AND erledigt=@erledigt";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);
			sqlCommand.Parameters.AddWithValue("nr", nr);
			sqlCommand.Parameters.AddWithValue("gebucht", gebucht);
			sqlCommand.Parameters.AddWithValue("erledigt", erledigt);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count == 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}

			return null;
		}
		public static int GetConfirmationForKonditionUpdate(int kundenNr, string newConditions = null)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand($"SELECT COUNT(*) FROM Angebote WHERE [Kunden-Nr]=@kundenNr AND [Typ]=@typ AND isnull(erledigt,0)=0 AND isnull(gebucht,0)=1{(newConditions is null ? "" : $" AND trim(isnull(Konditionen,''))<>TRIM('{newConditions}')")}", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("typ", TYP_CONFIRMATION);
				sqlCommand.Parameters.AddWithValue("kundenNr", kundenNr);

				return int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var x) ? x : 0;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetDeliveryNotes()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM Angebote WHERE [Typ]=@type ORDER BY Nr DESC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("type", TYP_DELIVERY);

				var selectAdapter = new SqlDataAdapter(sqlCommand);
				selectAdapter.Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity GetDeliveryNotesByNr(int nr)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM Angebote WHERE [Typ]=@type AND [Nr]=@nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("type", TYP_DELIVERY);
				sqlCommand.Parameters.AddWithValue("nr", nr);

				var selectAdapter = new SqlDataAdapter(sqlCommand);
				selectAdapter.Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetDeliveryNotesByAB(int abNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM Angebote WHERE [Typ]=@type AND nr_auf=@abNr ORDER BY Nr DESC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("type", TYP_DELIVERY);
				sqlCommand.Parameters.AddWithValue("abNr", abNr);

				var selectAdapter = new SqlDataAdapter(sqlCommand);
				selectAdapter.Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetDeliveryNotesLastByCustomer(int customerNr, DateTime minDate)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM Angebote WHERE [Typ]=@type AND [Kunden-Nr]=@customerNr AND [Datum]>=@date ORDER BY Nr DESC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("type", TYP_DELIVERY);
				sqlCommand.Parameters.AddWithValue("customerNr", customerNr);
				sqlCommand.Parameters.AddWithValue("date", minDate);

				var selectAdapter = new SqlDataAdapter(sqlCommand);
				selectAdapter.Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static int UpdateInvoicingAddress(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity element)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE Angebote SET "
					 + "Abteilung=@Abteilung,Ansprechpartner=@Ansprechpartner,Briefanrede=@Briefanrede,"
					+ " [Land/PLZ/Ort]=@Land_PLZ_Ort,Name2=@Name2,Name3=@Name3,[Straﬂe/Postfach]=@Straﬂe_Postfach,[Vorname/NameFirma]=@Vorname_NameFirma"
					+ " WHERE Nr=@Nr";
				;

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
				sqlCommand.Parameters.AddWithValue("Abteilung", element.Abteilung == null ? (object)DBNull.Value : element.Abteilung);
				sqlCommand.Parameters.AddWithValue("Ansprechpartner", element.Ansprechpartner == null ? (object)DBNull.Value : element.Ansprechpartner);
				sqlCommand.Parameters.AddWithValue("Briefanrede", element.Briefanrede == null ? (object)DBNull.Value : element.Briefanrede);
				sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", element.Land_PLZ_Ort == null ? (object)DBNull.Value : element.Land_PLZ_Ort);
				sqlCommand.Parameters.AddWithValue("Name2", element.Name2 == null ? (object)DBNull.Value : element.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", element.Name3 == null ? (object)DBNull.Value : element.Name3);
				sqlCommand.Parameters.AddWithValue("Straﬂe_Postfach", element.Straﬂe_Postfach == null ? (object)DBNull.Value : element.Straﬂe_Postfach);
				sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", element.Vorname_NameFirma == null ? (object)DBNull.Value : element.Vorname_NameFirma);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int UpdateDeliveryAddress(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity element)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE Angebote SET "
					+ " LAbteilung=@LAbteilung,LAnsprechpartner=@LAnsprechpartner,LBriefanrede=@LBriefanrede,"
					+ " [LLand/PLZ/Ort]=@LLand_PLZ_Ort,LName2=@LName2,LName3=@LName3,[LStraﬂe/Postfach]=@LStraﬂe_Postfach,[LVorname/NameFirma]=@LVorname_NameFirma, [LsAddressNr]=@LsAddressNr, [UnloadingPoint]=@UnloadingPoint, [StorageLocation]=@StorageLocation "
					+ " WHERE Nr=@Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
				sqlCommand.Parameters.AddWithValue("LAbteilung", element.LAbteilung == null ? (object)DBNull.Value : element.LAbteilung);
				sqlCommand.Parameters.AddWithValue("LAnsprechpartner", element.LAnsprechpartner == null ? (object)DBNull.Value : element.LAnsprechpartner);
				sqlCommand.Parameters.AddWithValue("LBriefanrede", element.LBriefanrede == null ? (object)DBNull.Value : element.LBriefanrede);
				sqlCommand.Parameters.AddWithValue("LLand_PLZ_Ort", element.LLand_PLZ_Ort == null ? (object)DBNull.Value : element.LLand_PLZ_Ort);
				sqlCommand.Parameters.AddWithValue("LName2", element.LName2 == null ? (object)DBNull.Value : element.LName2);
				sqlCommand.Parameters.AddWithValue("LName3", element.LName3 == null ? (object)DBNull.Value : element.LName3);
				sqlCommand.Parameters.AddWithValue("LStraﬂe_Postfach", element.LStraﬂe_Postfach == null ? (object)DBNull.Value : element.LStraﬂe_Postfach);
				sqlCommand.Parameters.AddWithValue("LVorname_NameFirma", element.LVorname_NameFirma == null ? (object)DBNull.Value : element.LVorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("LsAddressNr", element.LsAddressNr == null ? (object)DBNull.Value : element.LsAddressNr);
				sqlCommand.Parameters.AddWithValue("UnloadingPoint", element.UnloadingPoint == null ? (object)DBNull.Value : element.UnloadingPoint);
				sqlCommand.Parameters.AddWithValue("StorageLocation", element.StorageLocation == null ? (object)DBNull.Value : element.StorageLocation);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static bool CanDelete(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT Count([Nr]) FROM [Angebote] WHERE [Projekt-Nr]  IS NOT NULL AND [Projekt-Nr] IN (SELECT [Projekt-Nr] FROM dbo.[Angebote] WHERE [Nr]=@Id)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return int.TryParse(dataTable.Rows[0][0].ToString(), out var result) ? result <= 1 : false;
			}
			else
			{
				return false;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetDeliveryNoteByProject(string projectNr)
		{
			projectNr = projectNr ?? "";
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM [Angebote] WHERE [Projekt-Nr]=@projectNr AND typ=@typ";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("projectNr", projectNr);
			sqlCommand.Parameters.AddWithValue("typ", TYP_DELIVERY);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetInvoiceByProject(string projectNr)
		{
			projectNr = projectNr ?? "";
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM [Angebote] WHERE [Projekt-Nr]=@projectNr AND typ=@typ";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("projectNr", projectNr);
			sqlCommand.Parameters.AddWithValue("typ", TYP_INVOICE);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetCreditByProject(string projectNr)
		{
			projectNr = projectNr ?? "";
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM [Angebote] WHERE [Projekt-Nr]=@projectNr AND typ=@typ";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("projectNr", projectNr);
			sqlCommand.Parameters.AddWithValue("typ", TYP_CREDIT);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetInvoiceByAB(int abId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM [Angebote] WHERE [nr_auf]=@projectNr AND typ=@typ";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("projectNr", abId);
			sqlCommand.Parameters.AddWithValue("typ", TYP_INVOICE);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetInvoices(DateTime dateFrom, DateTime dateTo, bool includeFrom = false, bool includeTo = false)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $"SELECT * FROM [Angebote] WHERE [Typ]=@typ AND @dateFrom<{(includeFrom ? "=" : "")}[Datum] AND [Datum]<{(includeTo ? "=" : "")}@dateTo";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
			sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
			sqlCommand.Parameters.AddWithValue("typ", TYP_INVOICE);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetCreditByAB(int abId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM [Angebote] WHERE [nr_auf]=@projectNr AND typ=@typ";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("projectNr", abId);
			sqlCommand.Parameters.AddWithValue("typ", TYP_CREDIT);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> QuickSearch(string projectNr, string vorfailNr)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Angebote WHERE [Projekt-Nr] IS NOT NULL AND [Angebot-Nr] IS NOT NULL";
			if(!string.IsNullOrEmpty(projectNr) && !string.IsNullOrWhiteSpace(projectNr))
			{
				query += $" AND [Projekt-Nr]={projectNr}";
			}
			if(!string.IsNullOrEmpty(vorfailNr) && !string.IsNullOrWhiteSpace(vorfailNr))
			{
				query += $" AND [Angebot-Nr]={vorfailNr}";
			}
			query += $" ORDER BY [Nr]";
			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static int GetByTypAndDocumentAndCustomer(string typ, string document, int customerNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM [Angebote] WHERE LTRIM(RTRIM([Bezug])) =LTRIM(RTRIM(@document)) AND [Kunden-Nr]=@customerNr AND LTRIM(RTRIM([Typ]))=LTRIM(RTRIM(@typ))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("document", document);
				sqlCommand.Parameters.AddWithValue("customerNr", customerNr);
				sqlCommand.Parameters.AddWithValue("typ", typ);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}
		public static int GetByTypAndDocumentAndCustomer(string typ, string document, int customerNr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT COUNT(*) FROM [Angebote] WHERE LTRIM(RTRIM([Bezug])) =LTRIM(RTRIM(@document)) AND [Kunden-Nr]=@customerNr AND LTRIM(RTRIM([Typ]))=LTRIM(RTRIM(@typ))";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("document", document);
				sqlCommand.Parameters.AddWithValue("customerNr", customerNr);
				sqlCommand.Parameters.AddWithValue("typ", typ);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByTypAndDocumentAndCustomerEdi(string typ, string document, int customerNr, bool? edi = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Angebote] WHERE LTRIM(RTRIM([Bezug])) =LTRIM(RTRIM(@document)) AND [Kunden-Nr]=@customerNr AND LTRIM(RTRIM([Typ]))=LTRIM(RTRIM(@typ))" +
					$"{(edi.HasValue == false ? "" : $" AND COALESCE(EDI_Dateiname_CSV,''){(edi.Value == true ? "<>" : "=")}''")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("document", document);
				sqlCommand.Parameters.AddWithValue("customerNr", customerNr);
				sqlCommand.Parameters.AddWithValue("typ", typ);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static int GetByTypAndDocumentAndCustomer__(string typ, string document, int customerNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM [Angebote] WHERE [Bezug] =@document AND [Kunden-Nr]=@customerNr AND [Typ]=@typ AND [Personal-Nr]=-1";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("document", document);
				sqlCommand.Parameters.AddWithValue("customerNr", customerNr);
				sqlCommand.Parameters.AddWithValue("typ", typ);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}
		public static int DeleteByNr(int id)
		{
			var sqlConection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConection.Open();

			string query = "DELETE FROM Angebote WHERE Nr=@Id";

			var sqlCommand = new SqlCommand(query, sqlConection);
			sqlCommand.Parameters.AddWithValue("Id", id);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConection.Close();

			return response;
		}
		public static int GetCount(bool? isDone = null)
		{
			try
			{
				var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
				sqlConnection.Open();

				string query = $"SELECT COUNT(*) AS orderCount FROM Angebote ";
				if(isDone.HasValue)
				{
					if(isDone.Value == true)
					{
						query += $"WHERE erledigt = 1";
					}
					else
					{
						query += $"WHERE erledigt <> 1";
					}
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlConnection.Close();

				var dataTable = new DataTable();

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count == 0)
				{
					return 0;
				}
				else
				{
					var value = (dataTable.Rows[0]["orderCount"] == System.DBNull.Value)
						? (int?)null
						: Convert.ToInt32(dataTable.Rows[0]["orderCount"]);

					return value ?? 0;
				}
			} catch(Exception e)
			{
				return 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> Get(bool? isDone = null, Data.Access.Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM Angebote";
				if(isDone.HasValue)
				{
					if(isDone.Value == true)
					{
						query += $" WHERE erledigt = 1";
					}
					else
					{
						query += $" WHERE erledigt <> 1";
					}
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				if(paging != null)
				{
					query += $" ORDER BY Nr DESC OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}

				var selectAdapter = new SqlDataAdapter(sqlCommand);
				selectAdapter.Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}

		public static void UpdateDLF(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity element, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE Angebote SET nr_dlf=@nr_dlf,Bezug=@Bezug, [LVorname/NameFirma]=@lvorname, [LStraﬂe/Postfach]=@lstrasse, [LLand/PLZ/Ort]=@lland," +
				"[LAnrede]=@lanrede, [LName2]=@lname2, [LName3]=@lname3, [LAnsprechpartner]=@lansprechpartner, [LAbteilung]=@labteilung, [LBriefanrede]=@lbriefanrede, [StorageLocation]=@StorageLocation, [UnloadingPoint]=@UnloadingPoint"
				+ " WHERE Nr=@Nr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Nr", element.Nr);

				sqlCommand.Parameters.AddWithValue("nr_dlf", element.nr_dlf == null ? (object)DBNull.Value : element.nr_dlf);
				sqlCommand.Parameters.AddWithValue("Bezug", element.Bezug == null ? (object)DBNull.Value : element.Bezug);

				sqlCommand.Parameters.AddWithValue("lvorname", element.LVorname_NameFirma == null ? (object)DBNull.Value : element.LVorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("lstrasse", element.LStraﬂe_Postfach == null ? (object)DBNull.Value : element.LStraﬂe_Postfach);
				sqlCommand.Parameters.AddWithValue("lland", element.LLand_PLZ_Ort == null ? (object)DBNull.Value : element.LLand_PLZ_Ort);
				sqlCommand.Parameters.AddWithValue("lanrede", element.LAnrede == null ? (object)DBNull.Value : element.LAnrede);
				sqlCommand.Parameters.AddWithValue("lname2", element.LName2 == null ? (object)DBNull.Value : element.LName2);
				sqlCommand.Parameters.AddWithValue("lname3", element.LName3 == null ? (object)DBNull.Value : element.LName3);
				sqlCommand.Parameters.AddWithValue("lansprechpartner", element.LAnsprechpartner == null ? (object)DBNull.Value : element.LAnsprechpartner);
				sqlCommand.Parameters.AddWithValue("labteilung", element.LAbteilung == null ? (object)DBNull.Value : element.LAbteilung);
				sqlCommand.Parameters.AddWithValue("lbriefanrede", element.LBriefanrede == null ? (object)DBNull.Value : element.LBriefanrede);

				sqlCommand.Parameters.AddWithValue("StorageLocation", element.StorageLocation == null ? (object)DBNull.Value : element.StorageLocation);
				sqlCommand.Parameters.AddWithValue("UnloadingPoint", element.UnloadingPoint == null ? (object)DBNull.Value : element.UnloadingPoint);

				sqlCommand.ExecuteNonQuery();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByLineItemPlanIds(List<long> ids, string type = TYP_CONFIRMATION)
		{
			if(ids == null || ids.Count <= 0)
			{
				return null;
			}
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $"SELECT * FROM Angebote WHERE [Typ]='{type}' AND [nr_dlf] IN ({string.Join(",", ids)})";

			var sqlCommand = new SqlCommand(query, sqlConnection);


			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static int UpdateErledigtWithTransaction(int Nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Angebote] SET [erledigt] = 1, [Gebucht]=1 WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", Nr);
			results = sqlCommand.ExecuteNonQuery();
			return results;
		}


		#region Blanket
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetBelegfluss(List<int> Ids)
		{
			if(Ids == null || Ids.Count == 0)
				return null;
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT * FROM [Angebote] WHERE [Nr] IN ({string.Join(",", Ids)}) AND [Typ]='Auftragsbest‰tigung'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.RahmenLinkToAbPosEntity> GetRahmenLinkAB(int ArtikelNr, int KundenNr, int Anzahl)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select C.[GultigAb],C.[GultigBis],A.[Angebot-Nr],A.Nr as NrRA,A.[Kunden-Nr],Ar.[Artikelnummer],An.[Anzahl],An.[Nr],An.Position as Position
                             from [Angebote] A inner join [Angebotene Artikel] An on A.[Nr]=An.[Angebot-Nr]
                             inner join [__CTS_AngeboteArticleBlanketExtension] C
                             on C.[AngeboteArtikelNr]=An.[Nr] inner join [Artikel] Ar on An.[Artikel-Nr]=Ar.[Artikel-Nr]
                             inner join __CTS_AngeboteBlanketExtension B on C.RahmenNr=B.AngeboteNr
                             Where An.[OriginalAnzahl]>=@Anzahl
                             And An.[Artikel-Nr]=@ArtikelNr
                             And A.[Kunden-Nr]=@KundenNr
                             And c.GultigBis>=getdate() 
                             And B.StatusId=2";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", ArtikelNr);
				sqlCommand.Parameters.AddWithValue("KundenNr", KundenNr);
				sqlCommand.Parameters.AddWithValue("Anzahl", Anzahl);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RahmenLinkToAbPosEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.RahmenLinkToAbPosEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetAlreadyConverted(List<string> documents)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT * FROM [Angebote] WHERE [Bezug] IN ({string.Join(",", documents.Select(x => $"'{x}'").ToList())})
                               AND [Typ]='Rahmenauftrag' and [Personal-Nr]=-1";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<int> GetSearchNrs(string text, string column, bool top10 = true)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT DISTINCT {(top10 ? "TOP 10" : "")} A.[{column}] FROM [Angebote] A INNER JOIN __CTS_AngeboteBlanketExtension E
                           ON A.Nr=E.AngeboteNr
                           WHERE 
                           A.Typ='Rahmenauftrag' AND
                           A.[{column}] LIKE '%{text}%'
                           AND E.BlanketTypeId=1";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				var result = new List<int>();
				foreach(DataRow dataRow in dataTable.Rows)
				{
					result.Add(Convert.ToInt32(dataRow[column]));
				}
				return result;
			}
			else
			{
				return new List<int>();
			}
		}
		public static List<string> GetProjectNrsNrs(string text)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT DISTINCT TOP 10 A.[Bezug] FROM [Angebote] A INNER JOIN __CTS_AngeboteBlanketExtension E
                           ON A.Nr=E.AngeboteNr
                           WHERE 
                           A.Typ='Rahmenauftrag' AND
                           A.[Bezug] LIKE '%{text}%'
                           AND E.BlanketTypeId=1";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				var result = new List<string>();
				foreach(DataRow dataRow in dataTable.Rows)
				{
					result.Add(Convert.ToString(dataRow["Bezug"]));
				}
				return result;
			}
			else
			{
				return new List<string>();
			}
		}

		#endregion Blanket
		#region Rechnung / Invoice
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByNrRechnung(int nr_rechnung)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();
			var dataTable = new DataTable();
			string query = $@"SELECT * FROM [Angebote] WHERE [nr_rec]=@nr_rechnung";
			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("nr_rechnung", nr_rechnung);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);
			sqlConnection.Close();

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetInvoiceByLieferschein(int id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Angebote WHERE [Typ]=@typ AND [nr_lie]=@id";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("id", id);
			sqlCommand.Parameters.AddWithValue("typ", TYP_INVOICE);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetOpenDeliveriesForRechnung()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT * FROM [Angebote] WHERE [Typ] = 'Lieferschein' AND [gebucht] = 1 AND ISNULL([erledigt],0) = 0 AND [Datum] <= getdate()";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetDeliveriesWoRechnung(IEnumerable<int> lsIds)
		{
			if(lsIds == null || lsIds.Count() <= 0)
			{
				return null;
			}
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"
SELECT l.* FROM (SELECT * FROM Angebote WHERE Typ='Lieferschein' AND nr IN (
{string.Join(",", lsIds)}
)) l
Join (SELECT nr_lie FROM Angebote WHERE Typ='Rechnung' AND nr_lie IN (
{string.Join(",", lsIds)}
)) r on r.Nr_lie=l.Nr
	WHERE r.Nr_lie is null";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetOpenDeliveriesForRechnung(int kundenNr)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $@"SELECT * FROM [Angebote] WHERE [Kunden-Nr]=@kundenNr
            AND [Typ] = 'Lieferschein' AND [gebucht] = 1 AND ISNULL([erledigt],0) = 0 AND [Datum] <= getdate()";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("kundenNr", kundenNr);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByTypAndABid(string typ, int abid)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Angebote] WHERE [Typ]=@typ and [ab_id]=@abid";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("typ", typ);
				sqlCommand.Parameters.AddWithValue("abid", abid);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetByTypAndAngebotNr(string typ, int angebotNr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Angebote] WHERE [Typ]=@typ and [Angebot-Nr]=@angebotNr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("typ", typ);
			sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetForEinzelrechnung_Archives(string q1, string q2, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = $@"SELECT Angebote.*,[Y].Betrag_MWSt FROM 
                         (
                         ({q1}) X
                         INNER JOIN Angebote ON [X].[Angebot-Nr] = Angebote.[Angebot-Nr])
                         INNER JOIN 
                         ({q2}) Y
                         ON Angebote.[Angebot-Nr] = [Y].[Angebot-Nr];";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x, true)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		#endregion Rechnung


		#region  // - TO-DELETE - Static methods
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetHorschOrdrsp()
		{
			/* all sent ordrsp to Horsch (Masch & Leeb) from 2023-04-01 that have starting Pos(or Artikel of Pos).Bz1 w/ 0*/
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
							/* all sent ordrsp to Horsch (Masch & Leeb) from 2023-04-01 that have starting Pos(or Artikel of Pos).Bz1 w/ 0*/
							select * from Angebote where Nr In (select distinct b.[Nr]
								from [angebotene Artikel] as p join Artikel as a on a.[Artikel-Nr]=p.[Artikel-Nr]
								join Angebote as b on b.Nr=p.[Angebot-Nr] Join EDI_OrderExtension as e on e.OrderId=b.Nr
								where IsNull(b.EDI_Dateiname_CSV,'') like '\ord%' and  b.[Kunden-Nr] in (1045,1064) and  b.Neu_order =0
								and (a.[Bezeichnung 1] like '0%' or p.[Bezeichnung1] like '0%') and e.ValidationTime>='20230401'  and e.ValidationTime<'20230419')";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		#endregion

		public static int GetMaxAngebotNrByTypeAndSettingsValues(string typ, int maxCurrentValue, int minNewValue)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "select IIF((select MAX([Angebot-Nr])+1 from Angebote where typ=@type)<@maxCurrentValue,(select MAX([Angebot-Nr])+1 from Angebote where typ=@type),@minNewValue)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("maxCurrentValue", maxCurrentValue);
				sqlCommand.Parameters.AddWithValue("minNewValue", minNewValue);
				sqlCommand.Parameters.AddWithValue("type", typ);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}
		public static int GetMaxAngebotNrByTypeAndPrefix(string typ, int prefix)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"EXEC GetMaxAngebotNr @typ, @prefix";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("typ", typ);
				sqlCommand.Parameters.AddWithValue("prefix", prefix);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}
	}
}
