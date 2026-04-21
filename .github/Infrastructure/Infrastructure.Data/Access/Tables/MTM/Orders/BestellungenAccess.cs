using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class BestellungenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellungen] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellungen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
			}
		}



		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Bestellungen] WHERE [Nr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Bestellungen] ([AB-Nr_Lieferant],[Abteilung],[Anfrage_Lieferfrist],[Anrede],[Ansprechpartner],[Bearbeiter],[Belegkreis],[Bemerkungen],[Benutzer],[best_id],[Bestellbestätigung erbeten bis],[Bestellung-Nr],[Bezug],[Briefanrede],[datueber],[Datum],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[erledigt],[Frachtfreigrenze],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Kanban],[Konditionen],[Kreditorennummer],[Kundenbestellung],[Land/PLZ/Ort],[Lieferanten-Nr],[Liefertermin],[Löschen],[Mahnung],[Mandant],[Mindestbestellwert],[Name2],[Name3],[Neu],[nr_anf],[nr_bes],[nr_gut],[nr_RB],[nr_sto],[nr_war],[Öffnen],[Personal-Nr],[ProjectPurchase],[Projekt-Nr],[Rabatt],[Rahmenbestellung],[Straße/Postfach],[Typ],[Unser Zeichen],[USt],[Versandart],[Vorname/NameFirma],[Währung],[Zahlungsweise],[Zahlungsziel]) OUTPUT INSERTED.[Nr] VALUES (@AB_Nr_Lieferant,@Abteilung,@Anfrage_Lieferfrist,@Anrede,@Ansprechpartner,@Bearbeiter,@Belegkreis,@Bemerkungen,@Benutzer,@best_id,@Bestellbestatigung_erbeten_bis,@Bestellung_Nr,@Bezug,@Briefanrede,@datueber,@Datum,@Eingangslieferscheinnr,@Eingangsrechnungsnr,@erledigt,@Frachtfreigrenze,@Freitext,@gebucht,@gedruckt,@Ihr_Zeichen,@In_Bearbeitung,@Kanban,@Konditionen,@Kreditorennummer,@Kundenbestellung,@Land_PLZ_Ort,@Lieferanten_Nr,@Liefertermin,@Loschen,@Mahnung,@Mandant,@Mindestbestellwert,@Name2,@Name3,@Neu,@nr_anf,@nr_bes,@nr_gut,@nr_RB,@nr_sto,@nr_war,@Offnen,@Personal_Nr,@ProjectPurchase,@Projekt_Nr,@Rabatt,@Rahmenbestellung,@Strasse_Postfach,@Typ,@Unser_Zeichen,@USt,@Versandart,@Vorname_NameFirma,@Wahrung,@Zahlungsweise,@Zahlungsziel); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist", item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
					sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
					sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer.TruncateLeft(50));
					sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
					sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis", item.Bestellbestatigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestatigung_erbeten_bis);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
					sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr", item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
					sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
					sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
					sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
					sqlCommand.Parameters.AddWithValue("Kreditorennummer", item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
					sqlCommand.Parameters.AddWithValue("Kundenbestellung", item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
					sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("nr_anf", item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
					sqlCommand.Parameters.AddWithValue("nr_bes", item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
					sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_RB", item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
					sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
					sqlCommand.Parameters.AddWithValue("nr_war", item.nr_war == null ? (object)DBNull.Value : item.nr_war);
					sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("ProjectPurchase", item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 62; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> items)
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
						query += " INSERT INTO [Bestellungen] ([AB-Nr_Lieferant],[Abteilung],[Anfrage_Lieferfrist],[Anrede],[Ansprechpartner],[Bearbeiter],[Belegkreis],[Bemerkungen],[Benutzer],[best_id],[Bestellbestätigung erbeten bis],[Bestellung-Nr],[Bezug],[Briefanrede],[datueber],[Datum],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[erledigt],[Frachtfreigrenze],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Kanban],[Konditionen],[Kreditorennummer],[Kundenbestellung],[Land/PLZ/Ort],[Lieferanten-Nr],[Liefertermin],[Löschen],[Mahnung],[Mandant],[Mindestbestellwert],[Name2],[Name3],[Neu],[nr_anf],[nr_bes],[nr_gut],[nr_RB],[nr_sto],[nr_war],[Öffnen],[Personal-Nr],[ProjectPurchase],[Projekt-Nr],[Rabatt],[Rahmenbestellung],[Straße/Postfach],[Typ],[Unser Zeichen],[USt],[Versandart],[Vorname/NameFirma],[Währung],[Zahlungsweise],[Zahlungsziel]) VALUES ( "

							+ "@AB_Nr_Lieferant" + i + ","
							+ "@Abteilung" + i + ","
							+ "@Anfrage_Lieferfrist" + i + ","
							+ "@Anrede" + i + ","
							+ "@Ansprechpartner" + i + ","
							+ "@Bearbeiter" + i + ","
							+ "@Belegkreis" + i + ","
							+ "@Bemerkungen" + i + ","
							+ "@Benutzer" + i + ","
							+ "@best_id" + i + ","
							+ "@Bestellbestatigung_erbeten_bis" + i + ","
							+ "@Bestellung_Nr" + i + ","
							+ "@Bezug" + i + ","
							+ "@Briefanrede" + i + ","
							+ "@datueber" + i + ","
							+ "@Datum" + i + ","
							+ "@Eingangslieferscheinnr" + i + ","
							+ "@Eingangsrechnungsnr" + i + ","
							+ "@erledigt" + i + ","
							+ "@Frachtfreigrenze" + i + ","
							+ "@Freitext" + i + ","
							+ "@gebucht" + i + ","
							+ "@gedruckt" + i + ","
							+ "@Ihr_Zeichen" + i + ","
							+ "@In_Bearbeitung" + i + ","
							+ "@Kanban" + i + ","
							+ "@Konditionen" + i + ","
							+ "@Kreditorennummer" + i + ","
							+ "@Kundenbestellung" + i + ","
							+ "@Land_PLZ_Ort" + i + ","
							+ "@Lieferanten_Nr" + i + ","
							+ "@Liefertermin" + i + ","
							+ "@Loschen" + i + ","
							+ "@Mahnung" + i + ","
							+ "@Mandant" + i + ","
							+ "@Mindestbestellwert" + i + ","
							+ "@Name2" + i + ","
							+ "@Name3" + i + ","
							+ "@Neu" + i + ","
							+ "@nr_anf" + i + ","
							+ "@nr_bes" + i + ","
							+ "@nr_gut" + i + ","
							+ "@nr_RB" + i + ","
							+ "@nr_sto" + i + ","
							+ "@nr_war" + i + ","
							+ "@Offnen" + i + ","
							+ "@Personal_Nr" + i + ","
							+ "@ProjectPurchase" + i + ","
							+ "@Projekt_Nr" + i + ","
							+ "@Rabatt" + i + ","
							+ "@Rahmenbestellung" + i + ","
							+ "@Strasse_Postfach" + i + ","
							+ "@Typ" + i + ","
							+ "@Unser_Zeichen" + i + ","
							+ "@USt" + i + ","
							+ "@Versandart" + i + ","
							+ "@Vorname_NameFirma" + i + ","
							+ "@Wahrung" + i + ","
							+ "@Zahlungsweise" + i + ","
							+ "@Zahlungsziel" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
						sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist" + i, item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
						sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
						sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
						sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
						sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer.TruncateLeft(50));
						sqlCommand.Parameters.AddWithValue("best_id" + i, item.best_id == null ? (object)DBNull.Value : item.best_id);
						sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis" + i, item.Bestellbestatigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestatigung_erbeten_bis);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
						sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
						sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr" + i, item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
						sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
						sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
						sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
						sqlCommand.Parameters.AddWithValue("Kreditorennummer" + i, item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
						sqlCommand.Parameters.AddWithValue("Kundenbestellung" + i, item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
						sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
						sqlCommand.Parameters.AddWithValue("nr_anf" + i, item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
						sqlCommand.Parameters.AddWithValue("nr_bes" + i, item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
						sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
						sqlCommand.Parameters.AddWithValue("nr_RB" + i, item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
						sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
						sqlCommand.Parameters.AddWithValue("nr_war" + i, item.nr_war == null ? (object)DBNull.Value : item.nr_war);
						sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("ProjectPurchase" + i, item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
						sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
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

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Bestellungen] SET [AB-Nr_Lieferant]=@AB_Nr_Lieferant, [Abteilung]=@Abteilung, [Anfrage_Lieferfrist]=@Anfrage_Lieferfrist, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [Bearbeiter]=@Bearbeiter, [Belegkreis]=@Belegkreis, [Bemerkungen]=@Bemerkungen, [Benutzer]=@Benutzer, [best_id]=@best_id, [Bestellbestätigung erbeten bis]=@Bestellbestatigung_erbeten_bis, [Bestellung-Nr]=@Bestellung_Nr, [Bezug]=@Bezug, [Briefanrede]=@Briefanrede, [datueber]=@datueber, [Datum]=@Datum, [Eingangslieferscheinnr]=@Eingangslieferscheinnr, [Eingangsrechnungsnr]=@Eingangsrechnungsnr, [erledigt]=@erledigt, [Frachtfreigrenze]=@Frachtfreigrenze, [Freitext]=@Freitext, [gebucht]=@gebucht, [gedruckt]=@gedruckt, [Ihr Zeichen]=@Ihr_Zeichen, [In Bearbeitung]=@In_Bearbeitung, [Kanban]=@Kanban, [Konditionen]=@Konditionen, [Kreditorennummer]=@Kreditorennummer, [Kundenbestellung]=@Kundenbestellung, [Land/PLZ/Ort]=@Land_PLZ_Ort, [Lieferanten-Nr]=@Lieferanten_Nr, [Liefertermin]=@Liefertermin, [Löschen]=@Loschen, [Mahnung]=@Mahnung, [Mandant]=@Mandant, [Mindestbestellwert]=@Mindestbestellwert, [Name2]=@Name2, [Name3]=@Name3, [Neu]=@Neu, [nr_anf]=@nr_anf, [nr_bes]=@nr_bes, [nr_gut]=@nr_gut, [nr_RB]=@nr_RB, [nr_sto]=@nr_sto, [nr_war]=@nr_war, [Öffnen]=@Offnen, [Personal-Nr]=@Personal_Nr, [ProjectPurchase]=@ProjectPurchase, [Projekt-Nr]=@Projekt_Nr, [Rabatt]=@Rabatt, [Rahmenbestellung]=@Rahmenbestellung, [Straße/Postfach]=@Strasse_Postfach, [Typ]=@Typ, [Unser Zeichen]=@Unser_Zeichen, [USt]=@USt, [Versandart]=@Versandart, [Vorname/NameFirma]=@Vorname_NameFirma, [Währung]=@Wahrung, [Zahlungsweise]=@Zahlungsweise, [Zahlungsziel]=@Zahlungsziel WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
				sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
				sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist", item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
				sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
				sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
				sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
				sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer.TruncateLeft(50));
				sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
				sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis", item.Bestellbestatigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestatigung_erbeten_bis);
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
				sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
				sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
				sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr", item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
				sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
				sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
				sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
				sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
				sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
				sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
				sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
				sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
				sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
				sqlCommand.Parameters.AddWithValue("Kreditorennummer", item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
				sqlCommand.Parameters.AddWithValue("Kundenbestellung", item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
				sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
				sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
				sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
				sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
				sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
				sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
				sqlCommand.Parameters.AddWithValue("nr_anf", item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
				sqlCommand.Parameters.AddWithValue("nr_bes", item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
				sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
				sqlCommand.Parameters.AddWithValue("nr_RB", item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
				sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
				sqlCommand.Parameters.AddWithValue("nr_war", item.nr_war == null ? (object)DBNull.Value : item.nr_war);
				sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
				sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
				sqlCommand.Parameters.AddWithValue("ProjectPurchase", item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
				sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
				sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
				sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
				sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
				sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
				sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
				sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
				sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
				sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 62; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> items)
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
						query += " UPDATE [Bestellungen] SET "

							+ "[AB-Nr_Lieferant]=@AB_Nr_Lieferant" + i + ","
							+ "[Abteilung]=@Abteilung" + i + ","
							+ "[Anfrage_Lieferfrist]=@Anfrage_Lieferfrist" + i + ","
							+ "[Anrede]=@Anrede" + i + ","
							+ "[Ansprechpartner]=@Ansprechpartner" + i + ","
							+ "[Bearbeiter]=@Bearbeiter" + i + ","
							+ "[Belegkreis]=@Belegkreis" + i + ","
							+ "[Bemerkungen]=@Bemerkungen" + i + ","
							+ "[Benutzer]=@Benutzer" + i + ","
							+ "[best_id]=@best_id" + i + ","
							+ "[Bestellbestätigung erbeten bis]=@Bestellbestatigung_erbeten_bis" + i + ","
							+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
							+ "[Bezug]=@Bezug" + i + ","
							+ "[Briefanrede]=@Briefanrede" + i + ","
							+ "[datueber]=@datueber" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[Eingangslieferscheinnr]=@Eingangslieferscheinnr" + i + ","
							+ "[Eingangsrechnungsnr]=@Eingangsrechnungsnr" + i + ","
							+ "[erledigt]=@erledigt" + i + ","
							+ "[Frachtfreigrenze]=@Frachtfreigrenze" + i + ","
							+ "[Freitext]=@Freitext" + i + ","
							+ "[gebucht]=@gebucht" + i + ","
							+ "[gedruckt]=@gedruckt" + i + ","
							+ "[Ihr Zeichen]=@Ihr_Zeichen" + i + ","
							+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
							+ "[Kanban]=@Kanban" + i + ","
							+ "[Konditionen]=@Konditionen" + i + ","
							+ "[Kreditorennummer]=@Kreditorennummer" + i + ","
							+ "[Kundenbestellung]=@Kundenbestellung" + i + ","
							+ "[Land/PLZ/Ort]=@Land_PLZ_Ort" + i + ","
							+ "[Lieferanten-Nr]=@Lieferanten_Nr" + i + ","
							+ "[Liefertermin]=@Liefertermin" + i + ","
							+ "[Löschen]=@Loschen" + i + ","
							+ "[Mahnung]=@Mahnung" + i + ","
							+ "[Mandant]=@Mandant" + i + ","
							+ "[Mindestbestellwert]=@Mindestbestellwert" + i + ","
							+ "[Name2]=@Name2" + i + ","
							+ "[Name3]=@Name3" + i + ","
							+ "[Neu]=@Neu" + i + ","
							+ "[nr_anf]=@nr_anf" + i + ","
							+ "[nr_bes]=@nr_bes" + i + ","
							+ "[nr_gut]=@nr_gut" + i + ","
							+ "[nr_RB]=@nr_RB" + i + ","
							+ "[nr_sto]=@nr_sto" + i + ","
							+ "[nr_war]=@nr_war" + i + ","
							+ "[Öffnen]=@Offnen" + i + ","
							+ "[Personal-Nr]=@Personal_Nr" + i + ","
							+ "[ProjectPurchase]=@ProjectPurchase" + i + ","
							+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
							+ "[Rabatt]=@Rabatt" + i + ","
							+ "[Rahmenbestellung]=@Rahmenbestellung" + i + ","
							+ "[Straße/Postfach]=@Strasse_Postfach" + i + ","
							+ "[Typ]=@Typ" + i + ","
							+ "[Unser Zeichen]=@Unser_Zeichen" + i + ","
							+ "[USt]=@USt" + i + ","
							+ "[Versandart]=@Versandart" + i + ","
							+ "[Vorname/NameFirma]=@Vorname_NameFirma" + i + ","
							+ "[Währung]=@Wahrung" + i + ","
							+ "[Zahlungsweise]=@Zahlungsweise" + i + ","
							+ "[Zahlungsziel]=@Zahlungsziel" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
						sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist" + i, item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
						sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
						sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
						sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
						sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer.TruncateLeft(50));
						sqlCommand.Parameters.AddWithValue("best_id" + i, item.best_id == null ? (object)DBNull.Value : item.best_id);
						sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis" + i, item.Bestellbestatigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestatigung_erbeten_bis);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
						sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
						sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr" + i, item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
						sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
						sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
						sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
						sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
						sqlCommand.Parameters.AddWithValue("Kreditorennummer" + i, item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
						sqlCommand.Parameters.AddWithValue("Kundenbestellung" + i, item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
						sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
						sqlCommand.Parameters.AddWithValue("nr_anf" + i, item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
						sqlCommand.Parameters.AddWithValue("nr_bes" + i, item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
						sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
						sqlCommand.Parameters.AddWithValue("nr_RB" + i, item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
						sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
						sqlCommand.Parameters.AddWithValue("nr_war" + i, item.nr_war == null ? (object)DBNull.Value : item.nr_war);
						sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("ProjectPurchase" + i, item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
						sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
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
				string query = "DELETE FROM [Bestellungen] WHERE [Nr]=@Nr";
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

					string query = "DELETE FROM [Bestellungen] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Bestellungen] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Bestellungen]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Bestellungen] WHERE [Nr] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Bestellungen] ([AB-Nr_Lieferant],[Abteilung],[Anfrage_Lieferfrist],[Anrede],[Ansprechpartner],[Bearbeiter],[Belegkreis],[Bemerkungen],[Benutzer],[best_id],[Bestellbestätigung erbeten bis],[Bestellung-Nr],[Bezug],[Briefanrede],[datueber],[Datum],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[erledigt],[Frachtfreigrenze],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Kanban],[Konditionen],[Kreditorennummer],[Kundenbestellung],[Land/PLZ/Ort],[Lieferanten-Nr],[Liefertermin],[Löschen],[Mahnung],[Mandant],[Mindestbestellwert],[Name2],[Name3],[Neu],[nr_anf],[nr_bes],[nr_gut],[nr_RB],[nr_sto],[nr_war],[Öffnen],[Personal-Nr],[ProjectPurchase],[Projekt-Nr],[Rabatt],[Rahmenbestellung],[Straße/Postfach],[Typ],[Unser Zeichen],[USt],[Versandart],[Vorname/NameFirma],[Währung],[Zahlungsweise],[Zahlungsziel]) OUTPUT INSERTED.[Nr] VALUES (@AB_Nr_Lieferant,@Abteilung,@Anfrage_Lieferfrist,@Anrede,@Ansprechpartner,@Bearbeiter,@Belegkreis,@Bemerkungen,@Benutzer,@best_id,@Bestellbestatigung_erbeten_bis,@Bestellung_Nr,@Bezug,@Briefanrede,@datueber,@Datum,@Eingangslieferscheinnr,@Eingangsrechnungsnr,@erledigt,@Frachtfreigrenze,@Freitext,@gebucht,@gedruckt,@Ihr_Zeichen,@In_Bearbeitung,@Kanban,@Konditionen,@Kreditorennummer,@Kundenbestellung,@Land_PLZ_Ort,@Lieferanten_Nr,@Liefertermin,@Loschen,@Mahnung,@Mandant,@Mindestbestellwert,@Name2,@Name3,@Neu,@nr_anf,@nr_bes,@nr_gut,@nr_RB,@nr_sto,@nr_war,@Offnen,@Personal_Nr,@ProjectPurchase,@Projekt_Nr,@Rabatt,@Rahmenbestellung,@Strasse_Postfach,@Typ,@Unser_Zeichen,@USt,@Versandart,@Vorname_NameFirma,@Wahrung,@Zahlungsweise,@Zahlungsziel); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
			sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
			sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist", item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
			sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer.TruncateLeft(50));
			sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
			sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis", item.Bestellbestatigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestatigung_erbeten_bis);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
			sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
			sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
			sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr", item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
			sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
			sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
			sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
			sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
			sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
			sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
			sqlCommand.Parameters.AddWithValue("Kreditorennummer", item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
			sqlCommand.Parameters.AddWithValue("Kundenbestellung", item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
			sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
			sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
			sqlCommand.Parameters.AddWithValue("nr_anf", item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
			sqlCommand.Parameters.AddWithValue("nr_bes", item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
			sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
			sqlCommand.Parameters.AddWithValue("nr_RB", item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
			sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
			sqlCommand.Parameters.AddWithValue("nr_war", item.nr_war == null ? (object)DBNull.Value : item.nr_war);
			sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("ProjectPurchase", item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
			sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
			sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 62; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Bestellungen] ([AB-Nr_Lieferant],[Abteilung],[Anfrage_Lieferfrist],[Anrede],[Ansprechpartner],[Bearbeiter],[Belegkreis],[Bemerkungen],[Benutzer],[best_id],[Bestellbestätigung erbeten bis],[Bestellung-Nr],[Bezug],[Briefanrede],[datueber],[Datum],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[erledigt],[Frachtfreigrenze],[Freitext],[gebucht],[gedruckt],[Ihr Zeichen],[In Bearbeitung],[Kanban],[Konditionen],[Kreditorennummer],[Kundenbestellung],[Land/PLZ/Ort],[Lieferanten-Nr],[Liefertermin],[Löschen],[Mahnung],[Mandant],[Mindestbestellwert],[Name2],[Name3],[Neu],[nr_anf],[nr_bes],[nr_gut],[nr_RB],[nr_sto],[nr_war],[Öffnen],[Personal-Nr],[ProjectPurchase],[Projekt-Nr],[Rabatt],[Rahmenbestellung],[Straße/Postfach],[Typ],[Unser Zeichen],[USt],[Versandart],[Vorname/NameFirma],[Währung],[Zahlungsweise],[Zahlungsziel]) VALUES ( "

						+ "@AB_Nr_Lieferant" + i + ","
						+ "@Abteilung" + i + ","
						+ "@Anfrage_Lieferfrist" + i + ","
						+ "@Anrede" + i + ","
						+ "@Ansprechpartner" + i + ","
						+ "@Bearbeiter" + i + ","
						+ "@Belegkreis" + i + ","
						+ "@Bemerkungen" + i + ","
						+ "@Benutzer" + i + ","
						+ "@best_id" + i + ","
						+ "@Bestellbestatigung_erbeten_bis" + i + ","
						+ "@Bestellung_Nr" + i + ","
						+ "@Bezug" + i + ","
						+ "@Briefanrede" + i + ","
						+ "@datueber" + i + ","
						+ "@Datum" + i + ","
						+ "@Eingangslieferscheinnr" + i + ","
						+ "@Eingangsrechnungsnr" + i + ","
						+ "@erledigt" + i + ","
						+ "@Frachtfreigrenze" + i + ","
						+ "@Freitext" + i + ","
						+ "@gebucht" + i + ","
						+ "@gedruckt" + i + ","
						+ "@Ihr_Zeichen" + i + ","
						+ "@In_Bearbeitung" + i + ","
						+ "@Kanban" + i + ","
						+ "@Konditionen" + i + ","
						+ "@Kreditorennummer" + i + ","
						+ "@Kundenbestellung" + i + ","
						+ "@Land_PLZ_Ort" + i + ","
						+ "@Lieferanten_Nr" + i + ","
						+ "@Liefertermin" + i + ","
						+ "@Loschen" + i + ","
						+ "@Mahnung" + i + ","
						+ "@Mandant" + i + ","
						+ "@Mindestbestellwert" + i + ","
						+ "@Name2" + i + ","
						+ "@Name3" + i + ","
						+ "@Neu" + i + ","
						+ "@nr_anf" + i + ","
						+ "@nr_bes" + i + ","
						+ "@nr_gut" + i + ","
						+ "@nr_RB" + i + ","
						+ "@nr_sto" + i + ","
						+ "@nr_war" + i + ","
						+ "@Offnen" + i + ","
						+ "@Personal_Nr" + i + ","
						+ "@ProjectPurchase" + i + ","
						+ "@Projekt_Nr" + i + ","
						+ "@Rabatt" + i + ","
						+ "@Rahmenbestellung" + i + ","
						+ "@Strasse_Postfach" + i + ","
						+ "@Typ" + i + ","
						+ "@Unser_Zeichen" + i + ","
						+ "@USt" + i + ","
						+ "@Versandart" + i + ","
						+ "@Vorname_NameFirma" + i + ","
						+ "@Wahrung" + i + ","
						+ "@Zahlungsweise" + i + ","
						+ "@Zahlungsziel" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist" + i, item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
					sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
					sqlCommand.Parameters.AddWithValue("best_id" + i, item.best_id == null ? (object)DBNull.Value : item.best_id);
					sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis" + i, item.Bestellbestatigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestatigung_erbeten_bis);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
					sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr" + i, item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
					sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
					sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
					sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
					sqlCommand.Parameters.AddWithValue("Kreditorennummer" + i, item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
					sqlCommand.Parameters.AddWithValue("Kundenbestellung" + i, item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("nr_anf" + i, item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
					sqlCommand.Parameters.AddWithValue("nr_bes" + i, item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
					sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_RB" + i, item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
					sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
					sqlCommand.Parameters.AddWithValue("nr_war" + i, item.nr_war == null ? (object)DBNull.Value : item.nr_war);
					sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("ProjectPurchase" + i, item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Bestellungen] SET [AB-Nr_Lieferant]=@AB_Nr_Lieferant, [Abteilung]=@Abteilung, [Anfrage_Lieferfrist]=@Anfrage_Lieferfrist, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [Bearbeiter]=@Bearbeiter, [Belegkreis]=@Belegkreis, [Bemerkungen]=@Bemerkungen, [Benutzer]=@Benutzer, [best_id]=@best_id, [Bestellbestätigung erbeten bis]=@Bestellbestatigung_erbeten_bis, [Bestellung-Nr]=@Bestellung_Nr, [Bezug]=@Bezug, [Briefanrede]=@Briefanrede, [datueber]=@datueber, [Datum]=@Datum, [Eingangslieferscheinnr]=@Eingangslieferscheinnr, [Eingangsrechnungsnr]=@Eingangsrechnungsnr, [erledigt]=@erledigt, [Frachtfreigrenze]=@Frachtfreigrenze, [Freitext]=@Freitext, [gebucht]=@gebucht, [gedruckt]=@gedruckt, [Ihr Zeichen]=@Ihr_Zeichen, [In Bearbeitung]=@In_Bearbeitung, [Kanban]=@Kanban, [Konditionen]=@Konditionen, [Kreditorennummer]=@Kreditorennummer, [Kundenbestellung]=@Kundenbestellung, [Land/PLZ/Ort]=@Land_PLZ_Ort, [Lieferanten-Nr]=@Lieferanten_Nr, [Liefertermin]=@Liefertermin, [Löschen]=@Loschen, [Mahnung]=@Mahnung, [Mandant]=@Mandant, [Mindestbestellwert]=@Mindestbestellwert, [Name2]=@Name2, [Name3]=@Name3, [Neu]=@Neu, [nr_anf]=@nr_anf, [nr_bes]=@nr_bes, [nr_gut]=@nr_gut, [nr_RB]=@nr_RB, [nr_sto]=@nr_sto, [nr_war]=@nr_war, [Öffnen]=@Offnen, [Personal-Nr]=@Personal_Nr, [ProjectPurchase]=@ProjectPurchase, [Projekt-Nr]=@Projekt_Nr, [Rabatt]=@Rabatt, [Rahmenbestellung]=@Rahmenbestellung, [Straße/Postfach]=@Strasse_Postfach, [Typ]=@Typ, [Unser Zeichen]=@Unser_Zeichen, [USt]=@USt, [Versandart]=@Versandart, [Vorname/NameFirma]=@Vorname_NameFirma, [Währung]=@Wahrung, [Zahlungsweise]=@Zahlungsweise, [Zahlungsziel]=@Zahlungsziel WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
			sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
			sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist", item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
			sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
			sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
			sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis", item.Bestellbestatigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestatigung_erbeten_bis);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
			sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
			sqlCommand.Parameters.AddWithValue("datueber", item.datueber == null ? (object)DBNull.Value : item.datueber);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
			sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr", item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
			sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
			sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
			sqlCommand.Parameters.AddWithValue("Freitext", item.Freitext == null ? (object)DBNull.Value : item.Freitext);
			sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
			sqlCommand.Parameters.AddWithValue("gedruckt", item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
			sqlCommand.Parameters.AddWithValue("Ihr_Zeichen", item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Konditionen", item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
			sqlCommand.Parameters.AddWithValue("Kreditorennummer", item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
			sqlCommand.Parameters.AddWithValue("Kundenbestellung", item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
			sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
			sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("Mahnung", item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Neu", item.Neu == null ? (object)DBNull.Value : item.Neu);
			sqlCommand.Parameters.AddWithValue("nr_anf", item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
			sqlCommand.Parameters.AddWithValue("nr_bes", item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
			sqlCommand.Parameters.AddWithValue("nr_gut", item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
			sqlCommand.Parameters.AddWithValue("nr_RB", item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
			sqlCommand.Parameters.AddWithValue("nr_sto", item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
			sqlCommand.Parameters.AddWithValue("nr_war", item.nr_war == null ? (object)DBNull.Value : item.nr_war);
			sqlCommand.Parameters.AddWithValue("Offnen", item.Offnen == null ? (object)DBNull.Value : item.Offnen);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("ProjectPurchase", item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
			sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
			sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 62; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Bestellungen] SET "

					+ "[AB-Nr_Lieferant]=@AB_Nr_Lieferant" + i + ","
					+ "[Abteilung]=@Abteilung" + i + ","
					+ "[Anfrage_Lieferfrist]=@Anfrage_Lieferfrist" + i + ","
					+ "[Anrede]=@Anrede" + i + ","
					+ "[Ansprechpartner]=@Ansprechpartner" + i + ","
					+ "[Bearbeiter]=@Bearbeiter" + i + ","
					+ "[Belegkreis]=@Belegkreis" + i + ","
					+ "[Bemerkungen]=@Bemerkungen" + i + ","
					+ "[Benutzer]=@Benutzer" + i + ","
					+ "[best_id]=@best_id" + i + ","
					+ "[Bestellbestätigung erbeten bis]=@Bestellbestatigung_erbeten_bis" + i + ","
					+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
					+ "[Bezug]=@Bezug" + i + ","
					+ "[Briefanrede]=@Briefanrede" + i + ","
					+ "[datueber]=@datueber" + i + ","
					+ "[Datum]=@Datum" + i + ","
					+ "[Eingangslieferscheinnr]=@Eingangslieferscheinnr" + i + ","
					+ "[Eingangsrechnungsnr]=@Eingangsrechnungsnr" + i + ","
					+ "[erledigt]=@erledigt" + i + ","
					+ "[Frachtfreigrenze]=@Frachtfreigrenze" + i + ","
					+ "[Freitext]=@Freitext" + i + ","
					+ "[gebucht]=@gebucht" + i + ","
					+ "[gedruckt]=@gedruckt" + i + ","
					+ "[Ihr Zeichen]=@Ihr_Zeichen" + i + ","
					+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
					+ "[Kanban]=@Kanban" + i + ","
					+ "[Konditionen]=@Konditionen" + i + ","
					+ "[Kreditorennummer]=@Kreditorennummer" + i + ","
					+ "[Kundenbestellung]=@Kundenbestellung" + i + ","
					+ "[Land/PLZ/Ort]=@Land_PLZ_Ort" + i + ","
					+ "[Lieferanten-Nr]=@Lieferanten_Nr" + i + ","
					+ "[Liefertermin]=@Liefertermin" + i + ","
					+ "[Löschen]=@Loschen" + i + ","
					+ "[Mahnung]=@Mahnung" + i + ","
					+ "[Mandant]=@Mandant" + i + ","
					+ "[Mindestbestellwert]=@Mindestbestellwert" + i + ","
					+ "[Name2]=@Name2" + i + ","
					+ "[Name3]=@Name3" + i + ","
					+ "[Neu]=@Neu" + i + ","
					+ "[nr_anf]=@nr_anf" + i + ","
					+ "[nr_bes]=@nr_bes" + i + ","
					+ "[nr_gut]=@nr_gut" + i + ","
					+ "[nr_RB]=@nr_RB" + i + ","
					+ "[nr_sto]=@nr_sto" + i + ","
					+ "[nr_war]=@nr_war" + i + ","
					+ "[Öffnen]=@Offnen" + i + ","
					+ "[Personal-Nr]=@Personal_Nr" + i + ","
					+ "[ProjectPurchase]=@ProjectPurchase" + i + ","
					+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
					+ "[Rabatt]=@Rabatt" + i + ","
					+ "[Rahmenbestellung]=@Rahmenbestellung" + i + ","
					+ "[Straße/Postfach]=@Strasse_Postfach" + i + ","
					+ "[Typ]=@Typ" + i + ","
					+ "[Unser Zeichen]=@Unser_Zeichen" + i + ","
					+ "[USt]=@USt" + i + ","
					+ "[Versandart]=@Versandart" + i + ","
					+ "[Vorname/NameFirma]=@Vorname_NameFirma" + i + ","
					+ "[Währung]=@Wahrung" + i + ","
					+ "[Zahlungsweise]=@Zahlungsweise" + i + ","
					+ "[Zahlungsziel]=@Zahlungsziel" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Anfrage_Lieferfrist" + i, item.Anfrage_Lieferfrist == null ? (object)DBNull.Value : item.Anfrage_Lieferfrist);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
					sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
					sqlCommand.Parameters.AddWithValue("best_id" + i, item.best_id == null ? (object)DBNull.Value : item.best_id);
					sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis" + i, item.Bestellbestatigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestatigung_erbeten_bis);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
					sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("datueber" + i, item.datueber == null ? (object)DBNull.Value : item.datueber);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Eingangsrechnungsnr" + i, item.Eingangsrechnungsnr == null ? (object)DBNull.Value : item.Eingangsrechnungsnr);
					sqlCommand.Parameters.AddWithValue("erledigt" + i, item.erledigt == null ? (object)DBNull.Value : item.erledigt);
					sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
					sqlCommand.Parameters.AddWithValue("Freitext" + i, item.Freitext == null ? (object)DBNull.Value : item.Freitext);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.gedruckt == null ? (object)DBNull.Value : item.gedruckt);
					sqlCommand.Parameters.AddWithValue("Ihr_Zeichen" + i, item.Ihr_Zeichen == null ? (object)DBNull.Value : item.Ihr_Zeichen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Konditionen" + i, item.Konditionen == null ? (object)DBNull.Value : item.Konditionen);
					sqlCommand.Parameters.AddWithValue("Kreditorennummer" + i, item.Kreditorennummer == null ? (object)DBNull.Value : item.Kreditorennummer);
					sqlCommand.Parameters.AddWithValue("Kundenbestellung" + i, item.Kundenbestellung == null ? (object)DBNull.Value : item.Kundenbestellung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Mahnung" + i, item.Mahnung == null ? (object)DBNull.Value : item.Mahnung);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Neu" + i, item.Neu == null ? (object)DBNull.Value : item.Neu);
					sqlCommand.Parameters.AddWithValue("nr_anf" + i, item.nr_anf == null ? (object)DBNull.Value : item.nr_anf);
					sqlCommand.Parameters.AddWithValue("nr_bes" + i, item.nr_bes == null ? (object)DBNull.Value : item.nr_bes);
					sqlCommand.Parameters.AddWithValue("nr_gut" + i, item.nr_gut == null ? (object)DBNull.Value : item.nr_gut);
					sqlCommand.Parameters.AddWithValue("nr_RB" + i, item.nr_RB == null ? (object)DBNull.Value : item.nr_RB);
					sqlCommand.Parameters.AddWithValue("nr_sto" + i, item.nr_sto == null ? (object)DBNull.Value : item.nr_sto);
					sqlCommand.Parameters.AddWithValue("nr_war" + i, item.nr_war == null ? (object)DBNull.Value : item.nr_war);
					sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Offnen == null ? (object)DBNull.Value : item.Offnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("ProjectPurchase" + i, item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
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

			string query = "DELETE FROM [Bestellungen] WHERE [Nr]=@Nr";
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

				string query = "DELETE FROM [Bestellungen] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<string> GetFilteredWithUserName(string filter)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT CASE WHEN CHARINDEX(' ', [Benutzer] )= 0 then[Benutzer] else (SUBSTRING([Benutzer], 1, CHARINDEX(' ', [Benutzer]) - 1)) end as [Benutzer] FROM [Bestellungen] WHERE [Benutzer] IS NOT NULL AND[Benutzer] <> '' AND[Benutzer] Like @Benutzer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Benutzer", "%" + filter.SqlEscape() + "%");


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x.Field<string>("Benutzer")).ToList().Take(10).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> Search(
			Settings.SortingModel sorting,
			Settings.PaginModel paging,
			List<Settings.FilterModel> filters)
		{
			if(0 >= paging.RequestRows || paging.RequestRows > 100)
				paging.RequestRows = 100;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string clause = "";
				string query = @"SELECT a.*,COUNT(*)  over() TotalCount,
								CASE WHEN (ba.erledigt_pos = 0 AND a.erledigt = 0 AND a.gebucht = 1 AND a.Rahmenbestellung = 0) 
									THEN 1
									ELSE 0 END 
								as CanCreateWereingang  
								FROM [Bestellungen] a 
								Join 
									(SELECT distinct [Nr],[Projekt-Nr],[Bestellung-Nr],[Typ],[Datum],[Liefertermin],[Lieferanten-Nr],
													[Kreditorennummer],[Anrede],[Vorname/NameFirma],[Name2],[Name3],[Ansprechpartner],[Abteilung],
													[Straße/Postfach],[Land/PLZ/Ort],[Briefanrede],[Personal-Nr],[Versandart],[Zahlungsweise],[Konditionen],
													[Zahlungsziel],[USt],[Rabatt],[Bezug],[Ihr Zeichen],[Unser Zeichen],[Bestellbestätigung erbeten bis],
													[Freitext],[gebucht],[gedruckt],[erledigt],[Währung],[Kundenbestellung],[Anfrage_Lieferfrist],[Mahnung],
													[best_id],[datueber],[nr_anf],[nr_RB],[nr_bes],[nr_war],[nr_gut],[nr_sto],[Belegkreis],[Rahmenbestellung],
													[Bearbeiter],[Benutzer],[Mandant],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[Frachtfreigrenze],
													[Mindestbestellwert],[AB-Nr_Lieferant],[Neu],[Löschen],[In Bearbeitung],[Öffnen],[Kanban] 
									FROM [Bestellungen]) b on b.Nr=a.Nr
								JOIN [bestellte Artikel] ba on a.Nr = ba.[Bestellung-Nr] ";
				bool first = false;
				if(filters.Count > 0)
				{

					query += " WHERE ";
					foreach(var filter in filters)
					{
						switch(filter.FilterType)
						{
							case Settings.FilterTypes.Number:

								if(!string.IsNullOrWhiteSpace(filter.FirstFilterValue))
								{
									if(!first)
									{
										query += "";
										first = true;
									}
									else
									{ query += filter.ConnectorType; }

									query += $" {filter.FilterFieldName} = {filter.FirstFilterValue} ";
								}
								if(!string.IsNullOrWhiteSpace(filter.SecondFilterValue))
								{
									if(!first)
									{
										query += "";
										first = true;
									}
									else
									{ query += filter.ConnectorType; }

									query += $" ({filter.FilterFieldName} <> {filter.SecondFilterValue} OR {filter.FilterFieldName} IS NULL)";
								}
								break;
							case Settings.FilterTypes.String:
								if(!first)
								{
									query += "";
									first = true;
								}
								else
								{ query += filter.ConnectorType; }
								query += $" {filter.FilterFieldName} Like '{filter.FirstFilterValue.SqlEscape()}%'";
								break;
							case Settings.FilterTypes.Date:
								if(!first)
								{
									query += "";
									first = true;
								}
								else
								{ query += filter.ConnectorType; }
								if(!String.IsNullOrEmpty(filter.SecondFilterValue) && !String.IsNullOrEmpty(filter.FirstFilterValue))
									query += $" {filter.FilterFieldName} Between '{filter.FirstFilterValue.SqlEscape()}' AND '{filter.SecondFilterValue.SqlEscape()}'  ";
								else if(!String.IsNullOrEmpty(filter.FirstFilterValue) && String.IsNullOrEmpty(filter.SecondFilterValue))
									query += $" {filter.FilterFieldName} >= '{filter.FirstFilterValue.SqlEscape()}' ";
								else if(String.IsNullOrEmpty(filter.FirstFilterValue) && !String.IsNullOrEmpty(filter.SecondFilterValue))
									query += $" {filter.FilterFieldName} <= '{filter.SecondFilterValue.SqlEscape()}' ";
								break;
							case Settings.FilterTypes.Boolean:
								if(!first)
								{
									query += "";
									first = true;
								}
								else
								{ query += filter.ConnectorType; }
								if(filter.FirstFilterValue.ToLower() == "true")
									query += $" ISNULL({filter.FilterFieldName},0) = 0";
								else
									query += $" {filter.FilterFieldName} = 1";
								break;
							default:
								break;
						}
					}
				}


				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					clause += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					clause += " ORDER BY [Nr] ASC ";
				}

				if(paging != null)
				{
					clause += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}

				query += clause;
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> Search(string filter)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string clause = "";
				string query = $"SELECT TOP 20 a.* FROM [Bestellungen] a {(string.IsNullOrWhiteSpace(filter) ? "" : $" WHERE {filter}")} ";

				query += clause;
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
			}
		}
		public static int GetMax(string type = "Bestellung")
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT MAX([bestellung-Nr]) as OrderNR from Bestellungen WHERE Typ='{type.SqlEscape()}'";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
			}
		}

		public static int GetMax(string Client, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = $"SELECT MAX([bestellung-Nr]) as OrderNR from Bestellungen WHERE Typ='Bestellung' AND Mandant='{Client}'";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> GetFiltered(string filter, bool isBestellung = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				//var WhereAdded = false;
				string query = "SELECT TOP 10 * FROM [Bestellungen]";
				if(!string.IsNullOrEmpty(filter))
				{
					if(isBestellung)
					{
						query += $" WHERE [Bestellung-Nr] LIKE '{filter.SqlEscape()}%'";

					}
					else
					{
						query += $" WHERE Nr in (select distinct Nr from [Bestellungen] WHERE[Bestellungen].[Projekt-Nr] LIKE '{filter.SqlEscape()}%') AND Typ = 'Bestellung' ";
					}
					// query += $" WHERE {(isBestellung == true ? "[Bestellung-Nr]" : "[Projekt-Nr]")} LIKE '{filter.SqlEscape()}%'";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> GetFilteredProjectNr(string filter)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT TOP 10 * FROM [Bestellungen]";
				if(!string.IsNullOrEmpty(filter))
					query += $" WHERE [Projekt-Nr] LIKE '{filter.SqlEscape()}%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> GetByBearbeiter(int Bearbeiter)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellungen]";
				query += $" WHERE [Bearbeiter] = @Bearbeiter";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Bearbeiter", Bearbeiter);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity GetByBestellung_Nr(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellungen] WHERE [Bestellung-Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity GetByWareneingangByNr(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT * FROM [Bestellungen] 
							WHERE best_id=@Id
							and typ='Wareneingang'
							";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity GetWithTransactionByBestellung_Nr(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Bestellungen] WHERE [Bestellung-Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity GetWithTransactionByBestellung_NrAndType(int nr, string Typ, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = $"SELECT * FROM [Bestellungen] WHERE [Bestellung-Nr]=@Id  AND Bestellungen.Typ = '{Typ.SqlEscape()}'";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<int> GetOpen(DateTime? dateFrom)
		{
			dateFrom = dateFrom ?? new DateTime(DateTime.Today.Year - 1, 1, 1);
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT Nr FROM [Bestellungen] WHERE Typ = 'Bestellung' AND [Datum]>='{dateFrom.Value.ToString("yyyyMMdd")}' AND ISNULL(erledigt,0)=0 AND [Projekt-Nr] NOT IN (SELECT [Projekt-Nr] FROM [Bestellungen] WHERE Typ = 'Wareneingang' AND [Datum]>='{dateFrom.Value.ToString("yyyyMMdd")}');";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => int.TryParse(x[0]?.ToString(), out var _x) ? _x : 0).ToList();
			}
			else
			{
				return new List<int>();
			}
		}
		public static int UpdateErledigt(Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Bestellungen] SET [erledigt]=@erledigt WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("erledigt", item.erledigt == null ? (object)DBNull.Value : item.erledigt);
			return sqlCommand.ExecuteNonQuery();
		}
		public static int UpdatePurchaseProject(Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Bestellungen] SET [ProjectPurchase]=@erledigt WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("erledigt", item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
			return sqlCommand.ExecuteNonQuery();
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> GetBelegfluss(List<int> Ids)
		{
			if(Ids == null || Ids.Count == 0)
				return null;
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT a.* FROM [Bestellungen] a
							Join (SELECT distinct [Nr],[Projekt-Nr],[Bestellung-Nr],[Typ],[Datum],[Liefertermin],[Lieferanten-Nr],[Kreditorennummer],
                            [Anrede],[Vorname/NameFirma],[Name2],[Name3],[Ansprechpartner],[Abteilung],[Straße/Postfach],[Land/PLZ/Ort],[Briefanrede],
                            [Personal-Nr],[Versandart],[Zahlungsweise],[Konditionen],[Zahlungsziel],[USt],[Rabatt],[Bezug],[Ihr Zeichen],[Unser Zeichen],
                            [Bestellbestätigung erbeten bis],[Freitext],[gebucht],[gedruckt],[erledigt],[Währung],[Kundenbestellung],[Anfrage_Lieferfrist],
                            [Mahnung],[best_id],[datueber],[nr_anf],[nr_RB],[nr_bes],[nr_war],[nr_gut],[nr_sto],[Belegkreis],[Rahmenbestellung],[Bearbeiter],
                            [Benutzer],[Mandant],[Eingangslieferscheinnr],[Eingangsrechnungsnr],[Frachtfreigrenze],[Mindestbestellwert],[AB-Nr_Lieferant],[Neu],
                            [Löschen],[In Bearbeitung],[Öffnen],[Kanban] FROM [Bestellungen]) b	on b.Nr=a.Nr WHERE a.[NR] IN ({string.Join(",", Ids)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity> GetByBlanket(List<int> Ids)
		{
			if(Ids == null || Ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Bestellungen] WHERE Rahmenbestellung=0 AND Typ ='Bestellung' AND Nr IN ({string.Join(",", Ids)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity>();
			}
		}
		public static int UpdateBestId(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Bestellungen] SET [best_id]=@Nr WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", nr);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}

		#endregion Custom Methods
	}
}
