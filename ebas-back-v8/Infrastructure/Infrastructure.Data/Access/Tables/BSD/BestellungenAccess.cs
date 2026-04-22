using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class BestellungenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellungen] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellungen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> get(List<int> ids)
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
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity item)
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
					sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
					sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
					sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis", item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
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
					sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
					sqlCommand.Parameters.AddWithValue("Offnen", item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("ProjectPurchase", item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wahrung", item.Währung == null ? (object)DBNull.Value : item.Währung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> items)
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
						sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
						sqlCommand.Parameters.AddWithValue("best_id" + i, item.best_id == null ? (object)DBNull.Value : item.best_id);
						sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis" + i, item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
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
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
						sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("ProjectPurchase" + i, item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
						sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Währung == null ? (object)DBNull.Value : item.Währung);
						sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
						sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity item)
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
				sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
				sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
				sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis", item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
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
				sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
				sqlCommand.Parameters.AddWithValue("Offnen", item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
				sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
				sqlCommand.Parameters.AddWithValue("ProjectPurchase", item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
				sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
				sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
				sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
				sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
				sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
				sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
				sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("Wahrung", item.Währung == null ? (object)DBNull.Value : item.Währung);
				sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
				sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> items)
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
						sqlCommand.Parameters.AddWithValue("Benutzer" + i, item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
						sqlCommand.Parameters.AddWithValue("best_id" + i, item.best_id == null ? (object)DBNull.Value : item.best_id);
						sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis" + i, item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
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
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
						sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("ProjectPurchase" + i, item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
						sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Währung == null ? (object)DBNull.Value : item.Währung);
						sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
						sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
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

				results = DbExecution.ExecuteNonQuery(sqlCommand);
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

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Bestellungen] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Bestellungen]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


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
			sqlCommand.Parameters.AddWithValue("Benutzer", item.Benutzer == null ? (object)DBNull.Value : item.Benutzer);
			sqlCommand.Parameters.AddWithValue("best_id", item.best_id == null ? (object)DBNull.Value : item.best_id);
			sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis", item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
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
			sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
			sqlCommand.Parameters.AddWithValue("Offnen", item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("ProjectPurchase", item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
			sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
			sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Wahrung", item.Währung == null ? (object)DBNull.Value : item.Währung);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis" + i, item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
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
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
					sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("ProjectPurchase" + i, item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Währung == null ? (object)DBNull.Value : item.Währung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity item, SqlConnection connection, SqlTransaction transaction)
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
			sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis", item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
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
			sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
			sqlCommand.Parameters.AddWithValue("Offnen", item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("ProjectPurchase", item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("Rahmenbestellung", item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
			sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("Unser_Zeichen", item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
			sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Wahrung", item.Währung == null ? (object)DBNull.Value : item.Währung);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zahlungsziel", item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					sqlCommand.Parameters.AddWithValue("Bestellbestatigung_erbeten_bis" + i, item.Bestellbestätigung_erbeten_bis == null ? (object)DBNull.Value : item.Bestellbestätigung_erbeten_bis);
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
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
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
					sqlCommand.Parameters.AddWithValue("Offnen" + i, item.Öffnen == null ? (object)DBNull.Value : item.Öffnen);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("ProjectPurchase" + i, item.ProjectPurchase == null ? (object)DBNull.Value : item.ProjectPurchase);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rahmenbestellung" + i, item.Rahmenbestellung == null ? (object)DBNull.Value : item.Rahmenbestellung);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Straße_Postfach == null ? (object)DBNull.Value : item.Straße_Postfach);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Unser_Zeichen" + i, item.Unser_Zeichen == null ? (object)DBNull.Value : item.Unser_Zeichen);
					sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Währung == null ? (object)DBNull.Value : item.Währung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zahlungsziel" + i, item.Zahlungsziel == null ? (object)DBNull.Value : item.Zahlungsziel);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Bestellungen] WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


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

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods


		public static int GetCountBySupplier(int Lieferanten_nr, bool? onlyValidated = null)
		{
			try
			{
				var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
				sqlConnection.Open();

				string query = $"SELECT COUNT(*) AS LieferantenCount FROM Bestellungen WHERE [Lieferanten-Nr]=@Lieferanten_nr{(onlyValidated.HasValue ? $" AND IsNULL(Gebucht,0)={(onlyValidated == true ? "1" : "0")}" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Lieferanten_nr", Lieferanten_nr);

				sqlConnection.Close();

				var dataTable = new DataTable();

				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count == 0)
				{
					return 0;
				}
				else
				{
					var value = (dataTable.Rows[0]["LieferantenCount"] == System.DBNull.Value)
						? (int?)null
						: Convert.ToInt32(dataTable.Rows[0]["LieferantenCount"]);

					return value ?? 0;
				}
			} catch(Exception)
			{
				return 0;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> GetByBlanket(List<int> Ids)
		{
			if(Ids == null || Ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Bestellungen] WHERE Rahmenbestellung=0 AND Typ ='Bestellung' AND Nr IN ({string.Join(",", Ids)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> GetOpenOrdersByLieferant(int lieferantNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Bestellungen] WHERE ISNULL([erledigt],0)=0 AND Rahmenbestellung=0 AND Typ ='Bestellung' AND [Lieferanten-Nr] = {lieferantNr}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity> GetByProjectNr(List<int> Ids)
		{
			if(Ids == null || Ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Bestellungen] WHERE Rahmenbestellung=0 AND Typ ='Bestellung' AND [Projekt-Nr] IN ({string.Join(",", Ids.Select(x => $"'{x}'"))})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity>();
			}
		}
		#endregion
	}
}
