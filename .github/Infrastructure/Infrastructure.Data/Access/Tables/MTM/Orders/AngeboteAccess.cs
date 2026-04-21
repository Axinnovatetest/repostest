using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class AngeboteAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity Get(int nr)
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
				return new Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> Get()
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
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> get(List<int> ids)
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
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Angebote] ([ab_id],[ABSENDER],[Abteilung],[Angebot-Nr],[Anrede],[Ansprechpartner],[Auswahl],[Belegkreis],[Bemerkung],[Benutzer],[Bereich],[Bezug],[Briefanrede],[datueber],[Datum],[Debitorennummer],[Dplatz_Sirona],[EDI_Dateiname_CSV],[EDI_Kundenbestellnummer],[EDI_Order_Change],[EDI_Order_Change_Updated],[EDI_Order_Neu],[erledigt],[Fälligkeit],[Freie_Text],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Interessent],[Konditionen],[Kunden-Nr],[LAbteilung],[Land/PLZ/Ort],[LAnrede],[LAnsprechpartner],[LBriefanrede],[Lieferadresse],[Liefertermin],[LLand/PLZ/Ort],[LName2],[LName3],[Löschen],[LStraße/Postfach],[LVorname/NameFirma],[Mahnung],[Mandant],[Name2],[Name3],[Neu],[Neu_Order],[nr_ang],[nr_auf],[nr_BV],[nr_gut],[nr_Kanban],[nr_lie],[nr_pro],[nr_RA],[nr_rec],[nr_sto],[Öffnen],[Personal-Nr],[Projekt-Nr],[reparatur_nr],[Status],[Straße/Postfach],[termin_eingehalten],[Typ],[Unser Zeichen],[USt_Berechnen],[Versandart],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Vorname/NameFirma],[Wunschtermin],[Zahlungsweise],[Zahlungsziel]) OUTPUT INSERTED.[Nr] VALUES (@ab_id,@ABSENDER,@Abteilung,@Angebot_Nr,@Anrede,@Ansprechpartner,@Auswahl,@Belegkreis,@Bemerkung,@Benutzer,@Bereich,@Bezug,@Briefanrede,@datueber,@Datum,@Debitorennummer,@Dplatz_Sirona,@EDI_Dateiname_CSV,@EDI_Kundenbestellnummer,@EDI_Order_Change,@EDI_Order_Change_Updated,@EDI_Order_Neu,@erledigt,@Falligkeit,@Freie_Text,@Freitext,@gebucht,@gedruckt,@Ihr_Zeichen,@In_Bearbeitung,@Interessent,@Konditionen,@Kunden_Nr,@LAbteilung,@Land_PLZ_Ort,@LAnrede,@LAnsprechpartner,@LBriefanrede,@Lieferadresse,@Liefertermin,@LLand_PLZ_Ort,@LName2,@LName3,@Loschen,@LStrasse_Postfach,@LVorname_NameFirma,@Mahnung,@Mandant,@Name2,@Name3,@Neu,@Neu_Order,@nr_ang,@nr_auf,@nr_BV,@nr_gut,@nr_Kanban,@nr_lie,@nr_pro,@nr_RA,@nr_rec,@nr_sto,@Offnen,@Personal_Nr,@Projekt_Nr,@reparatur_nr,@Status,@Strasse_Postfach,@termin_eingehalten,@Typ,@Unser_Zeichen,@USt_Berechnen,@Versandart,@Versandarten_Auswahl,@Versanddatum_Auswahl,@Vorname_NameFirma,@Wunschtermin,@Zahlungsweise,@Zahlungsziel); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ab_id", item.ab_id == null ? (object)DBNull.Value : item.ab_id);
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
					sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Debitorennummer", item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
					sqlCommand.Parameters.AddWithValue("Dplatz_Sirona", item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
					sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV", item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
					sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer", item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change", item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated", item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Neu", item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
					sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("Falligkeit", item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
					sqlCommand.Parameters.AddWithValue("Freie_Text", item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
					sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
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
					sqlCommand.Parameters.AddWithValue("LStrasse_Postfach", item.LStrasse_Postfach == null ? (object)DBNull.Value : item.LStrasse_Postfach);
					sqlCommand.Parameters.AddWithValue("LVorname_NameFirma", item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("Neu_Order", item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
					sqlCommand.Parameters.AddWithValue("nr_ang", item.nr_ang == null ? (object)DBNull.Value : item.nr_ang);
					sqlCommand.Parameters.AddWithValue("nr_auf", item.nr_auf == null ? (object)DBNull.Value : item.nr_auf);
					sqlCommand.Parameters.AddWithValue("nr_BV", item.nr_BV == null ? (object)DBNull.Value : item.nr_BV);
					sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_Kanban", item.nr_Kanban == null ? (object)DBNull.Value : item.nr_Kanban);
					sqlCommand.Parameters.AddWithValue("nr_lie", item.nr_lie == null ? (object)DBNull.Value : item.nr_lie);
					sqlCommand.Parameters.AddWithValue("nr_pro", item.nr_pro == null ? (object)DBNull.Value : item.nr_pro);
					sqlCommand.Parameters.AddWithValue("nr_RA", item.nr_RA == null ? (object)DBNull.Value : item.nr_RA);
					sqlCommand.Parameters.AddWithValue("nr_rec", item.nr_rec == null ? (object)DBNull.Value : item.nr_rec);
					sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
					sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("reparatur_nr", item.reparatur_nr == null ? (object)DBNull.Value : item.reparatur_nr);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
					sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
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
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 81; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> items)
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
						query += " INSERT INTO [Angebote] ([ab_id],[ABSENDER],[Abteilung],[Angebot-Nr],[Anrede],[Ansprechpartner],[Auswahl],[Belegkreis],[Bemerkung],[Benutzer],[Bereich],[Bezug],[Briefanrede],[datueber],[Datum],[Debitorennummer],[Dplatz_Sirona],[EDI_Dateiname_CSV],[EDI_Kundenbestellnummer],[EDI_Order_Change],[EDI_Order_Change_Updated],[EDI_Order_Neu],[erledigt],[Fälligkeit],[Freie_Text],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Interessent],[Konditionen],[Kunden-Nr],[LAbteilung],[Land/PLZ/Ort],[LAnrede],[LAnsprechpartner],[LBriefanrede],[Lieferadresse],[Liefertermin],[LLand/PLZ/Ort],[LName2],[LName3],[Löschen],[LStraße/Postfach],[LVorname/NameFirma],[Mahnung],[Mandant],[Name2],[Name3],[Neu],[Neu_Order],[nr_ang],[nr_auf],[nr_BV],[nr_gut],[nr_Kanban],[nr_lie],[nr_pro],[nr_RA],[nr_rec],[nr_sto],[Öffnen],[Personal-Nr],[Projekt-Nr],[reparatur_nr],[Status],[Straße/Postfach],[termin_eingehalten],[Typ],[Unser Zeichen],[USt_Berechnen],[Versandart],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Vorname/NameFirma],[Wunschtermin],[Zahlungsweise],[Zahlungsziel]) VALUES ( "

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
							+ "@reparatur_nr" + i + ","
							+ "@Status" + i + ","
							+ "@Strasse_Postfach" + i + ","
							+ "@termin_eingehalten" + i + ","
							+ "@Typ" + i + ","
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


						sqlCommand.Parameters.AddWithValue("ab_id" + i, item.ab_id == null ? (object)DBNull.Value : item.ab_id);
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
						sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Debitorennummer" + i, item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
						sqlCommand.Parameters.AddWithValue("Dplatz_Sirona" + i, item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
						sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV" + i, item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
						sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer" + i, item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Change" + i, item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated" + i, item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Neu" + i, item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
						sqlCommand.Parameters.AddWithValue("Falligkeit" + i, item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
						sqlCommand.Parameters.AddWithValue("Freie_Text" + i, item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
						sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
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
						sqlCommand.Parameters.AddWithValue("LStrasse_Postfach" + i, item.LStrasse_Postfach == null ? (object)DBNull.Value : item.LStrasse_Postfach);
						sqlCommand.Parameters.AddWithValue("LVorname_NameFirma" + i, item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
						sqlCommand.Parameters.AddWithValue("Neu_Order" + i, item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
						sqlCommand.Parameters.AddWithValue("nr_ang" + i, item.nr_ang == null ? (object)DBNull.Value : item.nr_ang);
						sqlCommand.Parameters.AddWithValue("nr_auf" + i, item.nr_auf == null ? (object)DBNull.Value : item.nr_auf);
						sqlCommand.Parameters.AddWithValue("nr_BV" + i, item.nr_BV == null ? (object)DBNull.Value : item.nr_BV);
						sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
						sqlCommand.Parameters.AddWithValue("nr_Kanban" + i, item.nr_Kanban == null ? (object)DBNull.Value : item.nr_Kanban);
						sqlCommand.Parameters.AddWithValue("nr_lie" + i, item.nr_lie == null ? (object)DBNull.Value : item.nr_lie);
						sqlCommand.Parameters.AddWithValue("nr_pro" + i, item.nr_pro == null ? (object)DBNull.Value : item.nr_pro);
						sqlCommand.Parameters.AddWithValue("nr_RA" + i, item.nr_RA == null ? (object)DBNull.Value : item.nr_RA);
						sqlCommand.Parameters.AddWithValue("nr_rec" + i, item.nr_rec == null ? (object)DBNull.Value : item.nr_rec);
						sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
						sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("reparatur_nr" + i, item.reparatur_nr == null ? (object)DBNull.Value : item.reparatur_nr);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
						sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
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

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Angebote] SET [ab_id]=@ab_id, [ABSENDER]=@ABSENDER, [Abteilung]=@Abteilung, [Angebot-Nr]=@Angebot_Nr, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [Auswahl]=@Auswahl, [Belegkreis]=@Belegkreis, [Bemerkung]=@Bemerkung, [Benutzer]=@Benutzer, [Bereich]=@Bereich, [Bezug]=@Bezug, [Briefanrede]=@Briefanrede, [datueber]=@datueber, [Datum]=@Datum, [Debitorennummer]=@Debitorennummer, [Dplatz_Sirona]=@Dplatz_Sirona, [EDI_Dateiname_CSV]=@EDI_Dateiname_CSV, [EDI_Kundenbestellnummer]=@EDI_Kundenbestellnummer, [EDI_Order_Change]=@EDI_Order_Change, [EDI_Order_Change_Updated]=@EDI_Order_Change_Updated, [EDI_Order_Neu]=@EDI_Order_Neu, [erledigt]=@erledigt, [Fälligkeit]=@Falligkeit, [Freie_Text]=@Freie_Text, [Freitext]=@Freitext, [gebucht]=@gebucht, [gedruckt]=@gedruckt, [Ihr Zeichen]=@Ihr_Zeichen, [In Bearbeitung]=@In_Bearbeitung, [Interessent]=@Interessent, [Konditionen]=@Konditionen, [Kunden-Nr]=@Kunden_Nr, [LAbteilung]=@LAbteilung, [Land/PLZ/Ort]=@Land_PLZ_Ort, [LAnrede]=@LAnrede, [LAnsprechpartner]=@LAnsprechpartner, [LBriefanrede]=@LBriefanrede, [Lieferadresse]=@Lieferadresse, [Liefertermin]=@Liefertermin, [LLand/PLZ/Ort]=@LLand_PLZ_Ort, [LName2]=@LName2, [LName3]=@LName3, [Löschen]=@Loschen, [LStraße/Postfach]=@LStrasse_Postfach, [LVorname/NameFirma]=@LVorname_NameFirma, [Mahnung]=@Mahnung, [Mandant]=@Mandant, [Name2]=@Name2, [Name3]=@Name3, [Neu]=@Neu, [Neu_Order]=@Neu_Order, [nr_ang]=@nr_ang, [nr_auf]=@nr_auf, [nr_BV]=@nr_BV, [nr_gut]=@nr_gut, [nr_Kanban]=@nr_Kanban, [nr_lie]=@nr_lie, [nr_pro]=@nr_pro, [nr_RA]=@nr_RA, [nr_rec]=@nr_rec, [nr_sto]=@nr_sto, [Öffnen]=@Offnen, [Personal-Nr]=@Personal_Nr, [Projekt-Nr]=@Projekt_Nr, [reparatur_nr]=@reparatur_nr, [Status]=@Status, [Straße/Postfach]=@Strasse_Postfach, [termin_eingehalten]=@termin_eingehalten, [Typ]=@Typ, [Unser Zeichen]=@Unser_Zeichen, [USt_Berechnen]=@USt_Berechnen, [Versandart]=@Versandart, [Versandarten_Auswahl]=@Versandarten_Auswahl, [Versanddatum_Auswahl]=@Versanddatum_Auswahl, [Vorname/NameFirma]=@Vorname_NameFirma, [Wunschtermin]=@Wunschtermin, [Zahlungsweise]=@Zahlungsweise, [Zahlungsziel]=@Zahlungsziel WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("ab_id", item.ab_id == null ? (object)DBNull.Value : item.ab_id);
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
				sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Debitorennummer", item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
				sqlCommand.Parameters.AddWithValue("Dplatz_Sirona", item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
				sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV", item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
				sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer", item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
				sqlCommand.Parameters.AddWithValue("EDI_Order_Change", item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
				sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated", item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
				sqlCommand.Parameters.AddWithValue("EDI_Order_Neu", item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
				sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
				sqlCommand.Parameters.AddWithValue("Falligkeit", item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
				sqlCommand.Parameters.AddWithValue("Freie_Text", item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
				sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
				sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
				sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
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
				sqlCommand.Parameters.AddWithValue("LStrasse_Postfach", item.LStrasse_Postfach == null ? (object)DBNull.Value : item.LStrasse_Postfach);
				sqlCommand.Parameters.AddWithValue("LVorname_NameFirma", item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
				sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
				sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
				sqlCommand.Parameters.AddWithValue("Neu_Order", item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
				sqlCommand.Parameters.AddWithValue("nr_ang", item.nr_ang == null ? (object)DBNull.Value : item.nr_ang);
				sqlCommand.Parameters.AddWithValue("nr_auf", item.nr_auf == null ? (object)DBNull.Value : item.nr_auf);
				sqlCommand.Parameters.AddWithValue("nr_BV", item.nr_BV == null ? (object)DBNull.Value : item.nr_BV);
				sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
				sqlCommand.Parameters.AddWithValue("nr_Kanban", item.nr_Kanban == null ? (object)DBNull.Value : item.nr_Kanban);
				sqlCommand.Parameters.AddWithValue("nr_lie", item.nr_lie == null ? (object)DBNull.Value : item.nr_lie);
				sqlCommand.Parameters.AddWithValue("nr_pro", item.nr_pro == null ? (object)DBNull.Value : item.nr_pro);
				sqlCommand.Parameters.AddWithValue("nr_RA", item.nr_RA == null ? (object)DBNull.Value : item.nr_RA);
				sqlCommand.Parameters.AddWithValue("nr_rec", item.nr_rec == null ? (object)DBNull.Value : item.nr_rec);
				sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
				sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
				sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
				sqlCommand.Parameters.AddWithValue("reparatur_nr", item.reparatur_nr == null ? (object)DBNull.Value : item.reparatur_nr);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
				sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
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
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 81; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> items)
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
							+ "[Fälligkeit]=@Falligkeit" + i + ","
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
							+ "[Löschen]=@Loschen" + i + ","
							+ "[LStraße/Postfach]=@LStrasse_Postfach" + i + ","
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
							+ "[nr_gut]=@nr_gut" + i + ","
							+ "[nr_Kanban]=@nr_Kanban" + i + ","
							+ "[nr_lie]=@nr_lie" + i + ","
							+ "[nr_pro]=@nr_pro" + i + ","
							+ "[nr_RA]=@nr_RA" + i + ","
							+ "[nr_rec]=@nr_rec" + i + ","
							+ "[nr_sto]=@nr_sto" + i + ","
							+ "[Öffnen]=@Offnen" + i + ","
							+ "[Personal-Nr]=@Personal_Nr" + i + ","
							+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
							+ "[reparatur_nr]=@reparatur_nr" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[Straße/Postfach]=@Strasse_Postfach" + i + ","
							+ "[termin_eingehalten]=@termin_eingehalten" + i + ","
							+ "[Typ]=@Typ" + i + ","
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
						sqlCommand.Parameters.AddWithValue("ab_id" + i, item.ab_id == null ? (object)DBNull.Value : item.ab_id);
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
						sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Debitorennummer" + i, item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
						sqlCommand.Parameters.AddWithValue("Dplatz_Sirona" + i, item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
						sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV" + i, item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
						sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer" + i, item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Change" + i, item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated" + i, item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
						sqlCommand.Parameters.AddWithValue("EDI_Order_Neu" + i, item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
						sqlCommand.Parameters.AddWithValue("Falligkeit" + i, item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
						sqlCommand.Parameters.AddWithValue("Freie_Text" + i, item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
						sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
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
						sqlCommand.Parameters.AddWithValue("LStrasse_Postfach" + i, item.LStrasse_Postfach == null ? (object)DBNull.Value : item.LStrasse_Postfach);
						sqlCommand.Parameters.AddWithValue("LVorname_NameFirma" + i, item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
						sqlCommand.Parameters.AddWithValue("Neu_Order" + i, item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
						sqlCommand.Parameters.AddWithValue("nr_ang" + i, item.nr_ang == null ? (object)DBNull.Value : item.nr_ang);
						sqlCommand.Parameters.AddWithValue("nr_auf" + i, item.nr_auf == null ? (object)DBNull.Value : item.nr_auf);
						sqlCommand.Parameters.AddWithValue("nr_BV" + i, item.nr_BV == null ? (object)DBNull.Value : item.nr_BV);
						sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
						sqlCommand.Parameters.AddWithValue("nr_Kanban" + i, item.nr_Kanban == null ? (object)DBNull.Value : item.nr_Kanban);
						sqlCommand.Parameters.AddWithValue("nr_lie" + i, item.nr_lie == null ? (object)DBNull.Value : item.nr_lie);
						sqlCommand.Parameters.AddWithValue("nr_pro" + i, item.nr_pro == null ? (object)DBNull.Value : item.nr_pro);
						sqlCommand.Parameters.AddWithValue("nr_RA" + i, item.nr_RA == null ? (object)DBNull.Value : item.nr_RA);
						sqlCommand.Parameters.AddWithValue("nr_rec" + i, item.nr_rec == null ? (object)DBNull.Value : item.nr_rec);
						sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
						sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("reparatur_nr" + i, item.reparatur_nr == null ? (object)DBNull.Value : item.reparatur_nr);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
						sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
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
		public static Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Angebote] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Angebote]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Angebote] ([ab_id],[ABSENDER],[Abteilung],[Angebot-Nr],[Anrede],[Ansprechpartner],[Auswahl],[Belegkreis],[Bemerkung],[Benutzer],[Bereich],[Bezug],[Briefanrede],[datueber],[Datum],[Debitorennummer],[Dplatz_Sirona],[EDI_Dateiname_CSV],[EDI_Kundenbestellnummer],[EDI_Order_Change],[EDI_Order_Change_Updated],[EDI_Order_Neu],[erledigt],[Fälligkeit],[Freie_Text],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Interessent],[Konditionen],[Kunden-Nr],[LAbteilung],[Land/PLZ/Ort],[LAnrede],[LAnsprechpartner],[LBriefanrede],[Lieferadresse],[Liefertermin],[LLand/PLZ/Ort],[LName2],[LName3],[Löschen],[LStraße/Postfach],[LVorname/NameFirma],[Mahnung],[Mandant],[Name2],[Name3],[Neu],[Neu_Order],[nr_ang],[nr_auf],[nr_BV],[nr_gut],[nr_Kanban],[nr_lie],[nr_pro],[nr_RA],[nr_rec],[nr_sto],[Öffnen],[Personal-Nr],[Projekt-Nr],[reparatur_nr],[Status],[Straße/Postfach],[termin_eingehalten],[Typ],[Unser Zeichen],[USt_Berechnen],[Versandart],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Vorname/NameFirma],[Wunschtermin],[Zahlungsweise],[Zahlungsziel]) OUTPUT INSERTED.[Nr] VALUES (@ab_id,@ABSENDER,@Abteilung,@Angebot_Nr,@Anrede,@Ansprechpartner,@Auswahl,@Belegkreis,@Bemerkung,@Benutzer,@Bereich,@Bezug,@Briefanrede,@datueber,@Datum,@Debitorennummer,@Dplatz_Sirona,@EDI_Dateiname_CSV,@EDI_Kundenbestellnummer,@EDI_Order_Change,@EDI_Order_Change_Updated,@EDI_Order_Neu,@erledigt,@Falligkeit,@Freie_Text,@Freitext,@gebucht,@gedruckt,@Ihr_Zeichen,@In_Bearbeitung,@Interessent,@Konditionen,@Kunden_Nr,@LAbteilung,@Land_PLZ_Ort,@LAnrede,@LAnsprechpartner,@LBriefanrede,@Lieferadresse,@Liefertermin,@LLand_PLZ_Ort,@LName2,@LName3,@Loschen,@LStrasse_Postfach,@LVorname_NameFirma,@Mahnung,@Mandant,@Name2,@Name3,@Neu,@Neu_Order,@nr_ang,@nr_auf,@nr_BV,@nr_gut,@nr_Kanban,@nr_lie,@nr_pro,@nr_RA,@nr_rec,@nr_sto,@Offnen,@Personal_Nr,@Projekt_Nr,@reparatur_nr,@Status,@Strasse_Postfach,@termin_eingehalten,@Typ,@Unser_Zeichen,@USt_Berechnen,@Versandart,@Versandarten_Auswahl,@Versanddatum_Auswahl,@Vorname_NameFirma,@Wunschtermin,@Zahlungsweise,@Zahlungsziel); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ab_id", item.ab_id == null ? (object)DBNull.Value : item.ab_id);
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
			sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Debitorennummer", item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
			sqlCommand.Parameters.AddWithValue("Dplatz_Sirona", item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
			sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV", item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
			sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer", item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Change", item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated", item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Neu", item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
			sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
			sqlCommand.Parameters.AddWithValue("Falligkeit", item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
			sqlCommand.Parameters.AddWithValue("Freie_Text", item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
			sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
			sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
			sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
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
			sqlCommand.Parameters.AddWithValue("LStrasse_Postfach", item.LStrasse_Postfach == null ? (object)DBNull.Value : item.LStrasse_Postfach);
			sqlCommand.Parameters.AddWithValue("LVorname_NameFirma", item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
			sqlCommand.Parameters.AddWithValue("Neu_Order", item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
			sqlCommand.Parameters.AddWithValue("nr_ang", item.nr_ang == null ? (object)DBNull.Value : item.nr_ang);
			sqlCommand.Parameters.AddWithValue("nr_auf", item.nr_auf == null ? (object)DBNull.Value : item.nr_auf);
			sqlCommand.Parameters.AddWithValue("nr_BV", item.nr_BV == null ? (object)DBNull.Value : item.nr_BV);
			sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
			sqlCommand.Parameters.AddWithValue("nr_Kanban", item.nr_Kanban == null ? (object)DBNull.Value : item.nr_Kanban);
			sqlCommand.Parameters.AddWithValue("nr_lie", item.nr_lie == null ? (object)DBNull.Value : item.nr_lie);
			sqlCommand.Parameters.AddWithValue("nr_pro", item.nr_pro == null ? (object)DBNull.Value : item.nr_pro);
			sqlCommand.Parameters.AddWithValue("nr_RA", item.nr_RA == null ? (object)DBNull.Value : item.nr_RA);
			sqlCommand.Parameters.AddWithValue("nr_rec", item.nr_rec == null ? (object)DBNull.Value : item.nr_rec);
			sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
			sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("reparatur_nr", item.reparatur_nr == null ? (object)DBNull.Value : item.reparatur_nr);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
			sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
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
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 81; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Angebote] ([ab_id],[ABSENDER],[Abteilung],[Angebot-Nr],[Anrede],[Ansprechpartner],[Auswahl],[Belegkreis],[Bemerkung],[Benutzer],[Bereich],[Bezug],[Briefanrede],[datueber],[Datum],[Debitorennummer],[Dplatz_Sirona],[EDI_Dateiname_CSV],[EDI_Kundenbestellnummer],[EDI_Order_Change],[EDI_Order_Change_Updated],[EDI_Order_Neu],[erledigt],[Fälligkeit],[Freie_Text],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Interessent],[Konditionen],[Kunden-Nr],[LAbteilung],[Land/PLZ/Ort],[LAnrede],[LAnsprechpartner],[LBriefanrede],[Lieferadresse],[Liefertermin],[LLand/PLZ/Ort],[LName2],[LName3],[Löschen],[LStraße/Postfach],[LVorname/NameFirma],[Mahnung],[Mandant],[Name2],[Name3],[Neu],[Neu_Order],[nr_ang],[nr_auf],[nr_BV],[nr_gut],[nr_Kanban],[nr_lie],[nr_pro],[nr_RA],[nr_rec],[nr_sto],[Öffnen],[Personal-Nr],[Projekt-Nr],[reparatur_nr],[Status],[Straße/Postfach],[termin_eingehalten],[Typ],[Unser Zeichen],[USt_Berechnen],[Versandart],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Vorname/NameFirma],[Wunschtermin],[Zahlungsweise],[Zahlungsziel]) VALUES ( "

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
						+ "@reparatur_nr" + i + ","
						+ "@Status" + i + ","
						+ "@Strasse_Postfach" + i + ","
						+ "@termin_eingehalten" + i + ","
						+ "@Typ" + i + ","
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


					sqlCommand.Parameters.AddWithValue("ab_id" + i, item.ab_id == null ? (object)DBNull.Value : item.ab_id);
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
					sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Debitorennummer" + i, item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
					sqlCommand.Parameters.AddWithValue("Dplatz_Sirona" + i, item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
					sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV" + i, item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
					sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer" + i, item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change" + i, item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated" + i, item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Neu" + i, item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
					sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("Falligkeit" + i, item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
					sqlCommand.Parameters.AddWithValue("Freie_Text" + i, item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
					sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
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
					sqlCommand.Parameters.AddWithValue("LStrasse_Postfach" + i, item.LStrasse_Postfach == null ? (object)DBNull.Value : item.LStrasse_Postfach);
					sqlCommand.Parameters.AddWithValue("LVorname_NameFirma" + i, item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("Neu_Order" + i, item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
					sqlCommand.Parameters.AddWithValue("nr_ang" + i, item.nr_ang == null ? (object)DBNull.Value : item.nr_ang);
					sqlCommand.Parameters.AddWithValue("nr_auf" + i, item.nr_auf == null ? (object)DBNull.Value : item.nr_auf);
					sqlCommand.Parameters.AddWithValue("nr_BV" + i, item.nr_BV == null ? (object)DBNull.Value : item.nr_BV);
					sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_Kanban" + i, item.nr_Kanban == null ? (object)DBNull.Value : item.nr_Kanban);
					sqlCommand.Parameters.AddWithValue("nr_lie" + i, item.nr_lie == null ? (object)DBNull.Value : item.nr_lie);
					sqlCommand.Parameters.AddWithValue("nr_pro" + i, item.nr_pro == null ? (object)DBNull.Value : item.nr_pro);
					sqlCommand.Parameters.AddWithValue("nr_RA" + i, item.nr_RA == null ? (object)DBNull.Value : item.nr_RA);
					sqlCommand.Parameters.AddWithValue("nr_rec" + i, item.nr_rec == null ? (object)DBNull.Value : item.nr_rec);
					sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
					sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("reparatur_nr" + i, item.reparatur_nr == null ? (object)DBNull.Value : item.reparatur_nr);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
					sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
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

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Angebote] SET [ab_id]=@ab_id, [ABSENDER]=@ABSENDER, [Abteilung]=@Abteilung, [Angebot-Nr]=@Angebot_Nr, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [Auswahl]=@Auswahl, [Belegkreis]=@Belegkreis, [Bemerkung]=@Bemerkung, [Benutzer]=@Benutzer, [Bereich]=@Bereich, [Bezug]=@Bezug, [Briefanrede]=@Briefanrede, [datueber]=@datueber, [Datum]=@Datum, [Debitorennummer]=@Debitorennummer, [Dplatz_Sirona]=@Dplatz_Sirona, [EDI_Dateiname_CSV]=@EDI_Dateiname_CSV, [EDI_Kundenbestellnummer]=@EDI_Kundenbestellnummer, [EDI_Order_Change]=@EDI_Order_Change, [EDI_Order_Change_Updated]=@EDI_Order_Change_Updated, [EDI_Order_Neu]=@EDI_Order_Neu, [erledigt]=@erledigt, [Fälligkeit]=@Falligkeit, [Freie_Text]=@Freie_Text, [Freitext]=@Freitext, [gebucht]=@gebucht, [gedruckt]=@gedruckt, [Ihr Zeichen]=@Ihr_Zeichen, [In Bearbeitung]=@In_Bearbeitung, [Interessent]=@Interessent, [Konditionen]=@Konditionen, [Kunden-Nr]=@Kunden_Nr, [LAbteilung]=@LAbteilung, [Land/PLZ/Ort]=@Land_PLZ_Ort, [LAnrede]=@LAnrede, [LAnsprechpartner]=@LAnsprechpartner, [LBriefanrede]=@LBriefanrede, [Lieferadresse]=@Lieferadresse, [Liefertermin]=@Liefertermin, [LLand/PLZ/Ort]=@LLand_PLZ_Ort, [LName2]=@LName2, [LName3]=@LName3, [Löschen]=@Loschen, [LStraße/Postfach]=@LStrasse_Postfach, [LVorname/NameFirma]=@LVorname_NameFirma, [Mahnung]=@Mahnung, [Mandant]=@Mandant, [Name2]=@Name2, [Name3]=@Name3, [Neu]=@Neu, [Neu_Order]=@Neu_Order, [nr_ang]=@nr_ang, [nr_auf]=@nr_auf, [nr_BV]=@nr_BV, [nr_gut]=@nr_gut, [nr_Kanban]=@nr_Kanban, [nr_lie]=@nr_lie, [nr_pro]=@nr_pro, [nr_RA]=@nr_RA, [nr_rec]=@nr_rec, [nr_sto]=@nr_sto, [Öffnen]=@Offnen, [Personal-Nr]=@Personal_Nr, [Projekt-Nr]=@Projekt_Nr, [reparatur_nr]=@reparatur_nr, [Status]=@Status, [Straße/Postfach]=@Strasse_Postfach, [termin_eingehalten]=@termin_eingehalten, [Typ]=@Typ, [Unser Zeichen]=@Unser_Zeichen, [USt_Berechnen]=@USt_Berechnen, [Versandart]=@Versandart, [Versandarten_Auswahl]=@Versandarten_Auswahl, [Versanddatum_Auswahl]=@Versanddatum_Auswahl, [Vorname/NameFirma]=@Vorname_NameFirma, [Wunschtermin]=@Wunschtermin, [Zahlungsweise]=@Zahlungsweise, [Zahlungsziel]=@Zahlungsziel WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("ab_id", item.ab_id == null ? (object)DBNull.Value : item.ab_id);
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
			sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Debitorennummer", item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
			sqlCommand.Parameters.AddWithValue("Dplatz_Sirona", item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
			sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV", item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
			sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer", item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Change", item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated", item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
			sqlCommand.Parameters.AddWithValue("EDI_Order_Neu", item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
			sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
			sqlCommand.Parameters.AddWithValue("Falligkeit", item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
			sqlCommand.Parameters.AddWithValue("Freie_Text", item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
			sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
			sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
			sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
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
			sqlCommand.Parameters.AddWithValue("LStrasse_Postfach", item.LStrasse_Postfach == null ? (object)DBNull.Value : item.LStrasse_Postfach);
			sqlCommand.Parameters.AddWithValue("LVorname_NameFirma", item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
			sqlCommand.Parameters.AddWithValue("Neu_Order", item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
			sqlCommand.Parameters.AddWithValue("nr_ang", item.nr_ang == null ? (object)DBNull.Value : item.nr_ang);
			sqlCommand.Parameters.AddWithValue("nr_auf", item.nr_auf == null ? (object)DBNull.Value : item.nr_auf);
			sqlCommand.Parameters.AddWithValue("nr_BV", item.nr_BV == null ? (object)DBNull.Value : item.nr_BV);
			sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
			sqlCommand.Parameters.AddWithValue("nr_Kanban", item.nr_Kanban == null ? (object)DBNull.Value : item.nr_Kanban);
			sqlCommand.Parameters.AddWithValue("nr_lie", item.nr_lie == null ? (object)DBNull.Value : item.nr_lie);
			sqlCommand.Parameters.AddWithValue("nr_pro", item.nr_pro == null ? (object)DBNull.Value : item.nr_pro);
			sqlCommand.Parameters.AddWithValue("nr_RA", item.nr_RA == null ? (object)DBNull.Value : item.nr_RA);
			sqlCommand.Parameters.AddWithValue("nr_rec", item.nr_rec == null ? (object)DBNull.Value : item.nr_rec);
			sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
			sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("reparatur_nr", item.reparatur_nr == null ? (object)DBNull.Value : item.reparatur_nr);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
			sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
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
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 81; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				//int results = -1;
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
					+ "[Fälligkeit]=@Falligkeit" + i + ","
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
					+ "[Löschen]=@Loschen" + i + ","
					+ "[LStraße/Postfach]=@LStrasse_Postfach" + i + ","
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
					+ "[nr_gut]=@nr_gut" + i + ","
					+ "[nr_Kanban]=@nr_Kanban" + i + ","
					+ "[nr_lie]=@nr_lie" + i + ","
					+ "[nr_pro]=@nr_pro" + i + ","
					+ "[nr_RA]=@nr_RA" + i + ","
					+ "[nr_rec]=@nr_rec" + i + ","
					+ "[nr_sto]=@nr_sto" + i + ","
					+ "[Öffnen]=@Offnen" + i + ","
					+ "[Personal-Nr]=@Personal_Nr" + i + ","
					+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
					+ "[reparatur_nr]=@reparatur_nr" + i + ","
					+ "[Status]=@Status" + i + ","
					+ "[Straße/Postfach]=@Strasse_Postfach" + i + ","
					+ "[termin_eingehalten]=@termin_eingehalten" + i + ","
					+ "[Typ]=@Typ" + i + ","
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
					sqlCommand.Parameters.AddWithValue("ab_id" + i, item.ab_id == null ? (object)DBNull.Value : item.ab_id);
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
					sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Debitorennummer" + i, item.Debitorennummer == null ? (object)DBNull.Value : item.Debitorennummer);
					sqlCommand.Parameters.AddWithValue("Dplatz_Sirona" + i, item.Dplatz_Sirona == null ? (object)DBNull.Value : item.Dplatz_Sirona);
					sqlCommand.Parameters.AddWithValue("EDI_Dateiname_CSV" + i, item.EDI_Dateiname_CSV == null ? (object)DBNull.Value : item.EDI_Dateiname_CSV);
					sqlCommand.Parameters.AddWithValue("EDI_Kundenbestellnummer" + i, item.EDI_Kundenbestellnummer == null ? (object)DBNull.Value : item.EDI_Kundenbestellnummer);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change" + i, item.EDI_Order_Change == null ? (object)DBNull.Value : item.EDI_Order_Change);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Change_Updated" + i, item.EDI_Order_Change_Updated == null ? (object)DBNull.Value : item.EDI_Order_Change_Updated);
					sqlCommand.Parameters.AddWithValue("EDI_Order_Neu" + i, item.EDI_Order_Neu == null ? (object)DBNull.Value : item.EDI_Order_Neu);
					sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("Falligkeit" + i, item.Falligkeit == null ? (object)DBNull.Value : item.Falligkeit);
					sqlCommand.Parameters.AddWithValue("Freie_Text" + i, item.Freie_Text == null ? (object)DBNull.Value : item.Freie_Text);
					sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
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
					sqlCommand.Parameters.AddWithValue("LStrasse_Postfach" + i, item.LStrasse_Postfach == null ? (object)DBNull.Value : item.LStrasse_Postfach);
					sqlCommand.Parameters.AddWithValue("LVorname_NameFirma" + i, item.LVorname_NameFirma == null ? (object)DBNull.Value : item.LVorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("Neu_Order" + i, item.Neu_Order == null ? (object)DBNull.Value : item.Neu_Order);
					sqlCommand.Parameters.AddWithValue("nr_ang" + i, item.nr_ang == null ? (object)DBNull.Value : item.nr_ang);
					sqlCommand.Parameters.AddWithValue("nr_auf" + i, item.nr_auf == null ? (object)DBNull.Value : item.nr_auf);
					sqlCommand.Parameters.AddWithValue("nr_BV" + i, item.nr_BV == null ? (object)DBNull.Value : item.nr_BV);
					sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_Kanban" + i, item.nr_Kanban == null ? (object)DBNull.Value : item.nr_Kanban);
					sqlCommand.Parameters.AddWithValue("nr_lie" + i, item.nr_lie == null ? (object)DBNull.Value : item.nr_lie);
					sqlCommand.Parameters.AddWithValue("nr_pro" + i, item.nr_pro == null ? (object)DBNull.Value : item.nr_pro);
					sqlCommand.Parameters.AddWithValue("nr_RA" + i, item.nr_RA == null ? (object)DBNull.Value : item.nr_RA);
					sqlCommand.Parameters.AddWithValue("nr_rec" + i, item.nr_rec == null ? (object)DBNull.Value : item.nr_rec);
					sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
					sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("reparatur_nr" + i, item.reparatur_nr == null ? (object)DBNull.Value : item.reparatur_nr);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
					sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
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

		#region Custom Methods

		#endregion Custom Methods

	}
}
