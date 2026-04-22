using Infrastructure.Data.Entities.Tables.PRS;
using System.Diagnostics;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class FertigungAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.FertigungEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Fertigung] WHERE [ID]=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigung]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [Fertigung] WHERE [ID] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
		}
		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Fertigung] ([Angebot_Artikel_Nr],[Angebot_nr],[PlanningDateViolation],[Anzahl],[Anzahl_aktuell],[Anzahl_erledigt],[AnzahlnachgedrucktPPS],[Artikel_Nr],[Ausgangskontrolle],[Bemerkung],[Bemerkung II Planung],[Bemerkung ohne stätte],[Bemerkung_Kommissionierung_AL],[Bemerkung_Planung],[Bemerkung_Technik],[Bemerkung_zu_Prio],[BomVersion],[CAO],[Check_FAbegonnen],[Check_Gewerk1],[Check_Gewerk1_Teilweise],[Check_Gewerk2],[Check_Gewerk2_Teilweise],[Check_Gewerk3],[Check_Gewerk3_Teilweise],[Check_Kabelgeschnitten],[CPVersion],[Datum],[Endkontrolle],[Erledigte_FA_Datum],[Erstmuster],[FA_begonnen],[FA_Druckdatum],[FA_Gestartet],[Fa-NachdruckPPS],[Fertigungsnummer],[gebucht],[gedruckt],[Gewerk 1],[Gewerk 2],[Gewerk 3],[Gewerk_Teilweise_Bemerkung],[GrundNachdruckPPS],[HBGFAPositionId],[ID_Hauptartikel],[ID_Rahmenfertigung],[Kabel_geschnitten],[Kabel_geschnitten_Datum],[Kabel_Schneidebeginn],[Kabel_Schneidebeginn_Datum],[Kennzeichen],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kunden_Index_Datum],[KundenIndex],[Lagerort_id],[Lagerort_id zubuchen],[Letzte_Gebuchte_Menge],[Löschen],[Mandant],[Menge1],[Menge2],[Originalanzahl],[Planungsstatus],[Preis],[Prio],[Quick_Area],[ROH_umgebucht],[Spritzgießerei_abgeschlossen],[Tage Abweichung],[Technik],[Techniker],[Termin_Bestätigt1],[Termin_Bestätigt2],[Termin_Fertigstellung],[Termin_Material],[Termin_Ursprünglich],[Termin_voränderung],[UBG],[UBGTransfer],[Urs-Artikelnummer],[Urs-Fa],[Zeit]) OUTPUT INSERTED.[ID] VALUES (@Angebot_Artikel_Nr,@Angebot_nr,@Anzahl,@Anzahl_aktuell,@Anzahl_erledigt,@AnzahlnachgedrucktPPS,@Artikel_Nr,@Ausgangskontrolle,@Bemerkung,@Bemerkung_II_Planung,@Bemerkung_ohne_statte,@Bemerkung_Kommissionierung_AL,@Bemerkung_Planung,@Bemerkung_Technik,@Bemerkung_zu_Prio,@BomVersion,@CAO,@Check_FAbegonnen,@Check_Gewerk1,@Check_Gewerk1_Teilweise,@Check_Gewerk2,@Check_Gewerk2_Teilweise,@Check_Gewerk3,@Check_Gewerk3_Teilweise,@Check_Kabelgeschnitten,@CPVersion,@Datum,@Endkontrolle,@Erledigte_FA_Datum,@Erstmuster,@FA_begonnen,@FA_Druckdatum,@FA_Gestartet,@Fa_NachdruckPPS,@Fertigungsnummer,@gebucht,@gedruckt,@Gewerk_1,@Gewerk_2,@Gewerk_3,@Gewerk_Teilweise_Bemerkung,@GrundNachdruckPPS,@HBGFAPositionId,@ID_Hauptartikel,@ID_Rahmenfertigung,@Kabel_geschnitten,@Kabel_geschnitten_Datum,@Kabel_Schneidebeginn,@Kabel_Schneidebeginn_Datum,@Kennzeichen,@Kommisioniert_komplett,@Kommisioniert_teilweise,@Kunden_Index_Datum,@KundenIndex,@Lagerort_id,@Lagerort_id_zubuchen,@Letzte_Gebuchte_Menge,@Loschen,@Mandant,@Menge1,@Menge2,@Originalanzahl,@Planungsstatus,@Preis,@Prio,@Quick_Area,@ROH_umgebucht,@Spritzgiesserei_abgeschlossen,@Tage_Abweichung,@Technik,@Techniker,@Termin_Bestatigt1,@Termin_Bestatigt2,@Termin_Fertigstellung,@Termin_Material,@Termin_Ursprunglich,@Termin_voranderung,@UBG,@UBGTransfer,@Urs_Artikelnummer,@Urs_Fa,@Zeit,@PlanningDateViolation); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr", item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Angebot_nr", item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Anzahl_aktuell", item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
					sqlCommand.Parameters.AddWithValue("Anzahl_erledigt", item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
					sqlCommand.Parameters.AddWithValue("AnzahlnachgedrucktPPS", item.AnzahlnachgedrucktPPS);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Ausgangskontrolle", item.Ausgangskontrolle == null ? (object)DBNull.Value : item.Ausgangskontrolle);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_II_Planung", item.Bemerkung_II_Planung == null ? (object)DBNull.Value : item.Bemerkung_II_Planung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_ohne_statte", item.Bemerkung_ohne_statte == null ? (object)DBNull.Value : item.Bemerkung_ohne_statte);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL", item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Planung", item.Bemerkung_Planung == null ? (object)DBNull.Value : item.Bemerkung_Planung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Technik", item.Bemerkung_Technik == null ? (object)DBNull.Value : item.Bemerkung_Technik);
					sqlCommand.Parameters.AddWithValue("Bemerkung_zu_Prio", item.Bemerkung_zu_Prio == null ? (object)DBNull.Value : item.Bemerkung_zu_Prio);
					sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("CAO", item.CAO == null ? (object)DBNull.Value : item.CAO);
					sqlCommand.Parameters.AddWithValue("Check_FAbegonnen", item.Check_FAbegonnen == null ? (object)DBNull.Value : item.Check_FAbegonnen);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk1", item.Check_Gewerk1 == null ? (object)DBNull.Value : item.Check_Gewerk1);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk1_Teilweise", item.Check_Gewerk1_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk1_Teilweise);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk2", item.Check_Gewerk2 == null ? (object)DBNull.Value : item.Check_Gewerk2);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk2_Teilweise", item.Check_Gewerk2_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk2_Teilweise);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk3", item.Check_Gewerk3 == null ? (object)DBNull.Value : item.Check_Gewerk3);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk3_Teilweise", item.Check_Gewerk3_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk3_Teilweise);
					sqlCommand.Parameters.AddWithValue("Check_Kabelgeschnitten", item.Check_Kabelgeschnitten == null ? (object)DBNull.Value : item.Check_Kabelgeschnitten);
					sqlCommand.Parameters.AddWithValue("CPVersion", item.CPVersion == null ? (object)DBNull.Value : item.CPVersion);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Endkontrolle", item.Endkontrolle == null ? (object)DBNull.Value : item.Endkontrolle);
					sqlCommand.Parameters.AddWithValue("Erledigte_FA_Datum", item.Erledigte_FA_Datum == null ? (object)DBNull.Value : item.Erledigte_FA_Datum);
					sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
					sqlCommand.Parameters.AddWithValue("FA_begonnen", item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
					sqlCommand.Parameters.AddWithValue("FA_Druckdatum", item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
					sqlCommand.Parameters.AddWithValue("FA_Gestartet", item.FA_Gestartet == null ? (object)DBNull.Value : item.FA_Gestartet);
					sqlCommand.Parameters.AddWithValue("Fa_NachdruckPPS", item.Fa_NachdruckPPS == null ? (object)DBNull.Value : item.Fa_NachdruckPPS);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("gebucht", item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
					sqlCommand.Parameters.AddWithValue("Gewerk_1", item.Gewerk_1 == null ? "" : item.Gewerk_1);
					sqlCommand.Parameters.AddWithValue("Gewerk_2", item.Gewerk_2 == null ? "" : item.Gewerk_2);
					sqlCommand.Parameters.AddWithValue("Gewerk_3", item.Gewerk_3 == null ? "" : item.Gewerk_3);
					sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung", item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
					sqlCommand.Parameters.AddWithValue("GrundNachdruckPPS", item.GrundNachdruckPPS == null ? (object)DBNull.Value : item.GrundNachdruckPPS);
					sqlCommand.Parameters.AddWithValue("HBGFAPositionId", item.HBGFAPositionId == null ? (object)DBNull.Value : item.HBGFAPositionId);
					sqlCommand.Parameters.AddWithValue("ID_Hauptartikel", item.ID_Hauptartikel == null ? (object)DBNull.Value : item.ID_Hauptartikel);
					sqlCommand.Parameters.AddWithValue("ID_Rahmenfertigung", item.ID_Rahmenfertigung == null ? (object)DBNull.Value : item.ID_Rahmenfertigung);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum", item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
					sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn", item.Kabel_Schneidebeginn == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn);
					sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn_Datum", item.Kabel_Schneidebeginn_Datum == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn_Datum);
					sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett", item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise", item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
					sqlCommand.Parameters.AddWithValue("Kunden_Index_Datum", item.Kunden_Index_Datum == null ? (object)DBNull.Value : item.Kunden_Index_Datum);
					sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Lagerort_id_zubuchen", item.Lagerort_id_zubuchen == null ? (object)DBNull.Value : item.Lagerort_id_zubuchen);
					sqlCommand.Parameters.AddWithValue("Letzte_Gebuchte_Menge", item.Letzte_Gebuchte_Menge == null ? (object)DBNull.Value : item.Letzte_Gebuchte_Menge);
					sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Menge1", item.Menge1 == null ? (object)DBNull.Value : item.Menge1);
					sqlCommand.Parameters.AddWithValue("Menge2", item.Menge2 == null ? (object)DBNull.Value : item.Menge2);
					sqlCommand.Parameters.AddWithValue("Originalanzahl", item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
					sqlCommand.Parameters.AddWithValue("Planungsstatus", item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
					sqlCommand.Parameters.AddWithValue("Preis", item.Preis == null ? (object)DBNull.Value : item.Preis);
					sqlCommand.Parameters.AddWithValue("Prio", item.Prio == null ? (object)DBNull.Value : item.Prio);
					sqlCommand.Parameters.AddWithValue("Quick_Area", item.Quick_Area == null ? (object)DBNull.Value : item.Quick_Area);
					sqlCommand.Parameters.AddWithValue("ROH_umgebucht", item.ROH_umgebucht == null ? (object)DBNull.Value : item.ROH_umgebucht);
					sqlCommand.Parameters.AddWithValue("Spritzgiesserei_abgeschlossen", item.SpritzgieBerei_abgeschlossen == null ? (object)DBNull.Value : item.SpritzgieBerei_abgeschlossen);
					sqlCommand.Parameters.AddWithValue("Tage_Abweichung", item.Tage_Abweichung == null ? (object)DBNull.Value : item.Tage_Abweichung);
					sqlCommand.Parameters.AddWithValue("Technik", item.Technik == null ? (object)DBNull.Value : item.Technik);
					sqlCommand.Parameters.AddWithValue("Techniker", item.Techniker == null ? (object)DBNull.Value : item.Techniker);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2", item.Termin_Bestatigt2 == null ? (object)DBNull.Value : item.Termin_Bestatigt2);
					sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung", item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
					sqlCommand.Parameters.AddWithValue("Termin_Material", item.Termin_Material == null ? (object)DBNull.Value : item.Termin_Material);
					sqlCommand.Parameters.AddWithValue("Termin_Ursprunglich", item.Termin_Ursprunglich == null ? (object)DBNull.Value : item.Termin_Ursprunglich);
					sqlCommand.Parameters.AddWithValue("Termin_voranderung", item.Termin_voranderung == null ? (object)DBNull.Value : item.Termin_voranderung);
					sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
					sqlCommand.Parameters.AddWithValue("UBGTransfer", item.UBGTransfer == null ? (object)DBNull.Value : item.UBGTransfer);
					sqlCommand.Parameters.AddWithValue("Urs_Artikelnummer", item.Urs_Artikelnummer == null ? (object)DBNull.Value : item.Urs_Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Urs_Fa", item.Urs_Fa == null ? (object)DBNull.Value : item.Urs_Fa);
					sqlCommand.Parameters.AddWithValue("Zeit", item.Zeit == null ? (object)DBNull.Value : item.Zeit);
					sqlCommand.Parameters.AddWithValue("PlanningDateViolation", item.PlanningDateViolation == null ? (object)DBNull.Value : item.PlanningDateViolation);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 84; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> items)
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
						query += " INSERT INTO [Fertigung] ([Angebot_Artikel_Nr],[Angebot_nr],[Anzahl],[Anzahl_aktuell],[Anzahl_erledigt],[AnzahlnachgedrucktPPS],[Artikel_Nr],[Ausgangskontrolle],[Bemerkung],[Bemerkung II Planung],[Bemerkung ohne stätte],[Bemerkung_Kommissionierung_AL],[Bemerkung_Planung],[Bemerkung_Technik],[Bemerkung_zu_Prio],[BomVersion],[CAO],[Check_FAbegonnen],[Check_Gewerk1],[Check_Gewerk1_Teilweise],[Check_Gewerk2],[Check_Gewerk2_Teilweise],[Check_Gewerk3],[Check_Gewerk3_Teilweise],[Check_Kabelgeschnitten],[CPVersion],[Datum],[Endkontrolle],[Erledigte_FA_Datum],[Erstmuster],[FA_begonnen],[FA_Druckdatum],[FA_Gestartet],[Fa-NachdruckPPS],[Fertigungsnummer],[gebucht],[gedruckt],[Gewerk 1],[Gewerk 2],[Gewerk 3],[Gewerk_Teilweise_Bemerkung],[GrundNachdruckPPS],[HBGFAPositionId],[ID_Hauptartikel],[ID_Rahmenfertigung],[Kabel_geschnitten],[Kabel_geschnitten_Datum],[Kabel_Schneidebeginn],[Kabel_Schneidebeginn_Datum],[Kennzeichen],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kunden_Index_Datum],[KundenIndex],[Lagerort_id],[Lagerort_id zubuchen],[Letzte_Gebuchte_Menge],[Löschen],[Mandant],[Menge1],[Menge2],[Originalanzahl],[Planungsstatus],[Preis],[Prio],[Quick_Area],[ROH_umgebucht],[Spritzgießerei_abgeschlossen],[Tage Abweichung],[Technik],[Techniker],[Termin_Bestätigt1],[Termin_Bestätigt2],[Termin_Fertigstellung],[Termin_Material],[Termin_Ursprünglich],[Termin_voränderung],[UBG],[UBGTransfer],[Urs-Artikelnummer],[Urs-Fa],[Zeit],[PlanningDateViolation]) VALUES ( "

							+ "@Angebot_Artikel_Nr" + i + ","
							+ "@Angebot_nr" + i + ","
							+ "@Anzahl" + i + ","
							+ "@Anzahl_aktuell" + i + ","
							+ "@Anzahl_erledigt" + i + ","
							+ "@AnzahlnachgedrucktPPS" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Ausgangskontrolle" + i + ","
							+ "@Bemerkung" + i + ","
							+ "@Bemerkung_II_Planung" + i + ","
							+ "@Bemerkung_ohne_statte" + i + ","
							+ "@Bemerkung_Kommissionierung_AL" + i + ","
							+ "@Bemerkung_Planung" + i + ","
							+ "@Bemerkung_Technik" + i + ","
							+ "@Bemerkung_zu_Prio" + i + ","
							+ "@BomVersion" + i + ","
							+ "@CAO" + i + ","
							+ "@Check_FAbegonnen" + i + ","
							+ "@Check_Gewerk1" + i + ","
							+ "@Check_Gewerk1_Teilweise" + i + ","
							+ "@Check_Gewerk2" + i + ","
							+ "@Check_Gewerk2_Teilweise" + i + ","
							+ "@Check_Gewerk3" + i + ","
							+ "@Check_Gewerk3_Teilweise" + i + ","
							+ "@Check_Kabelgeschnitten" + i + ","
							+ "@CPVersion" + i + ","
							+ "@Datum" + i + ","
							+ "@Endkontrolle" + i + ","
							+ "@Erledigte_FA_Datum" + i + ","
							+ "@Erstmuster" + i + ","
							+ "@FA_begonnen" + i + ","
							+ "@FA_Druckdatum" + i + ","
							+ "@FA_Gestartet" + i + ","
							+ "@Fa_NachdruckPPS" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@gebucht" + i + ","
							+ "@gedruckt" + i + ","
							+ "@Gewerk_1" + i + ","
							+ "@Gewerk_2" + i + ","
							+ "@Gewerk_3" + i + ","
							+ "@Gewerk_Teilweise_Bemerkung" + i + ","
							+ "@GrundNachdruckPPS" + i + ","
							+ "@HBGFAPositionId" + i + ","
							+ "@ID_Hauptartikel" + i + ","
							+ "@ID_Rahmenfertigung" + i + ","
							+ "@Kabel_geschnitten" + i + ","
							+ "@Kabel_geschnitten_Datum" + i + ","
							+ "@Kabel_Schneidebeginn" + i + ","
							+ "@Kabel_Schneidebeginn_Datum" + i + ","
							+ "@Kennzeichen" + i + ","
							+ "@Kommisioniert_komplett" + i + ","
							+ "@Kommisioniert_teilweise" + i + ","
							+ "@Kunden_Index_Datum" + i + ","
							+ "@KundenIndex" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@Lagerort_id_zubuchen" + i + ","
							+ "@Letzte_Gebuchte_Menge" + i + ","
							+ "@Loschen" + i + ","
							+ "@Mandant" + i + ","
							+ "@Menge1" + i + ","
							+ "@Menge2" + i + ","
							+ "@Originalanzahl" + i + ","
							+ "@Planungsstatus" + i + ","
							+ "@Preis" + i + ","
							+ "@Prio" + i + ","
							+ "@Quick_Area" + i + ","
							+ "@ROH_umgebucht" + i + ","
							+ "@Spritzgiesserei_abgeschlossen" + i + ","
							+ "@Tage_Abweichung" + i + ","
							+ "@Technik" + i + ","
							+ "@Techniker" + i + ","
							+ "@Termin_Bestatigt1" + i + ","
							+ "@Termin_Bestatigt2" + i + ","
							+ "@Termin_Fertigstellung" + i + ","
							+ "@Termin_Material" + i + ","
							+ "@Termin_Ursprunglich" + i + ","
							+ "@Termin_voranderung" + i + ","
							+ "@UBG" + i + ","
							+ "@UBGTransfer" + i + ","
							+ "@Urs_Artikelnummer" + i + ","
							+ "@Urs_Fa" + i + ","
							+ "@PlanningDateViolation" + i + ","
							+ "@Zeit" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr" + i, item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Angebot_nr" + i, item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Anzahl_aktuell" + i, item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
						sqlCommand.Parameters.AddWithValue("Anzahl_erledigt" + i, item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
						sqlCommand.Parameters.AddWithValue("AnzahlnachgedrucktPPS" + i, item.AnzahlnachgedrucktPPS);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Ausgangskontrolle" + i, item.Ausgangskontrolle == null ? (object)DBNull.Value : item.Ausgangskontrolle);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bemerkung_II_Planung" + i, item.Bemerkung_II_Planung == null ? (object)DBNull.Value : item.Bemerkung_II_Planung);
						sqlCommand.Parameters.AddWithValue("Bemerkung_ohne_statte" + i, item.Bemerkung_ohne_statte == null ? (object)DBNull.Value : item.Bemerkung_ohne_statte);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL" + i, item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Planung" + i, item.Bemerkung_Planung == null ? (object)DBNull.Value : item.Bemerkung_Planung);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Technik" + i, item.Bemerkung_Technik == null ? (object)DBNull.Value : item.Bemerkung_Technik);
						sqlCommand.Parameters.AddWithValue("Bemerkung_zu_Prio" + i, item.Bemerkung_zu_Prio == null ? (object)DBNull.Value : item.Bemerkung_zu_Prio);
						sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
						sqlCommand.Parameters.AddWithValue("CAO" + i, item.CAO == null ? (object)DBNull.Value : item.CAO);
						sqlCommand.Parameters.AddWithValue("Check_FAbegonnen" + i, item.Check_FAbegonnen == null ? (object)DBNull.Value : item.Check_FAbegonnen);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk1" + i, item.Check_Gewerk1 == null ? (object)DBNull.Value : item.Check_Gewerk1);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk1_Teilweise" + i, item.Check_Gewerk1_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk1_Teilweise);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk2" + i, item.Check_Gewerk2 == null ? (object)DBNull.Value : item.Check_Gewerk2);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk2_Teilweise" + i, item.Check_Gewerk2_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk2_Teilweise);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk3" + i, item.Check_Gewerk3 == null ? (object)DBNull.Value : item.Check_Gewerk3);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk3_Teilweise" + i, item.Check_Gewerk3_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk3_Teilweise);
						sqlCommand.Parameters.AddWithValue("Check_Kabelgeschnitten" + i, item.Check_Kabelgeschnitten == null ? (object)DBNull.Value : item.Check_Kabelgeschnitten);
						sqlCommand.Parameters.AddWithValue("CPVersion" + i, item.CPVersion == null ? (object)DBNull.Value : item.CPVersion);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Endkontrolle" + i, item.Endkontrolle == null ? (object)DBNull.Value : item.Endkontrolle);
						sqlCommand.Parameters.AddWithValue("Erledigte_FA_Datum" + i, item.Erledigte_FA_Datum == null ? (object)DBNull.Value : item.Erledigte_FA_Datum);
						sqlCommand.Parameters.AddWithValue("Erstmuster" + i, item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
						sqlCommand.Parameters.AddWithValue("FA_begonnen" + i, item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
						sqlCommand.Parameters.AddWithValue("FA_Druckdatum" + i, item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
						sqlCommand.Parameters.AddWithValue("FA_Gestartet" + i, item.FA_Gestartet == null ? (object)DBNull.Value : item.FA_Gestartet);
						sqlCommand.Parameters.AddWithValue("Fa_NachdruckPPS" + i, item.Fa_NachdruckPPS == null ? (object)DBNull.Value : item.Fa_NachdruckPPS);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
						sqlCommand.Parameters.AddWithValue("Gewerk_1" + i, item.Gewerk_1);
						sqlCommand.Parameters.AddWithValue("Gewerk_2" + i, item.Gewerk_2);
						sqlCommand.Parameters.AddWithValue("Gewerk_3" + i, item.Gewerk_3);
						sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung" + i, item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
						sqlCommand.Parameters.AddWithValue("GrundNachdruckPPS" + i, item.GrundNachdruckPPS == null ? (object)DBNull.Value : item.GrundNachdruckPPS);
						sqlCommand.Parameters.AddWithValue("HBGFAPositionId" + i, item.HBGFAPositionId == null ? (object)DBNull.Value : item.HBGFAPositionId);
						sqlCommand.Parameters.AddWithValue("ID_Hauptartikel" + i, item.ID_Hauptartikel == null ? (object)DBNull.Value : item.ID_Hauptartikel);
						sqlCommand.Parameters.AddWithValue("ID_Rahmenfertigung" + i, item.ID_Rahmenfertigung == null ? (object)DBNull.Value : item.ID_Rahmenfertigung);
						sqlCommand.Parameters.AddWithValue("Kabel_geschnitten" + i, item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
						sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum" + i, item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
						sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn" + i, item.Kabel_Schneidebeginn == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn);
						sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn_Datum" + i, item.Kabel_Schneidebeginn_Datum == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn_Datum);
						sqlCommand.Parameters.AddWithValue("Kennzeichen" + i, item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
						sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett" + i, item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
						sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise" + i, item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
						sqlCommand.Parameters.AddWithValue("Kunden_Index_Datum" + i, item.Kunden_Index_Datum == null ? (object)DBNull.Value : item.Kunden_Index_Datum);
						sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Lagerort_id_zubuchen" + i, item.Lagerort_id_zubuchen == null ? (object)DBNull.Value : item.Lagerort_id_zubuchen);
						sqlCommand.Parameters.AddWithValue("Letzte_Gebuchte_Menge" + i, item.Letzte_Gebuchte_Menge == null ? (object)DBNull.Value : item.Letzte_Gebuchte_Menge);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("Menge1" + i, item.Menge1 == null ? (object)DBNull.Value : item.Menge1);
						sqlCommand.Parameters.AddWithValue("Menge2" + i, item.Menge2 == null ? (object)DBNull.Value : item.Menge2);
						sqlCommand.Parameters.AddWithValue("Originalanzahl" + i, item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
						sqlCommand.Parameters.AddWithValue("Planungsstatus" + i, item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
						sqlCommand.Parameters.AddWithValue("Preis" + i, item.Preis == null ? (object)DBNull.Value : item.Preis);
						sqlCommand.Parameters.AddWithValue("Prio" + i, item.Prio == null ? (object)DBNull.Value : item.Prio);
						sqlCommand.Parameters.AddWithValue("Quick_Area" + i, item.Quick_Area == null ? (object)DBNull.Value : item.Quick_Area);
						sqlCommand.Parameters.AddWithValue("ROH_umgebucht" + i, item.ROH_umgebucht == null ? (object)DBNull.Value : item.ROH_umgebucht);
						sqlCommand.Parameters.AddWithValue("Spritzgiesserei_abgeschlossen" + i, item.SpritzgieBerei_abgeschlossen == null ? (object)DBNull.Value : item.SpritzgieBerei_abgeschlossen);
						sqlCommand.Parameters.AddWithValue("Tage_Abweichung" + i, item.Tage_Abweichung == null ? (object)DBNull.Value : item.Tage_Abweichung);
						sqlCommand.Parameters.AddWithValue("Technik" + i, item.Technik == null ? (object)DBNull.Value : item.Technik);
						sqlCommand.Parameters.AddWithValue("Techniker" + i, item.Techniker == null ? (object)DBNull.Value : item.Techniker);
						sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
						sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2" + i, item.Termin_Bestatigt2 == null ? (object)DBNull.Value : item.Termin_Bestatigt2);
						sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung" + i, item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
						sqlCommand.Parameters.AddWithValue("Termin_Material" + i, item.Termin_Material == null ? (object)DBNull.Value : item.Termin_Material);
						sqlCommand.Parameters.AddWithValue("Termin_Ursprunglich" + i, item.Termin_Ursprunglich == null ? (object)DBNull.Value : item.Termin_Ursprunglich);
						sqlCommand.Parameters.AddWithValue("Termin_voranderung" + i, item.Termin_voranderung == null ? (object)DBNull.Value : item.Termin_voranderung);
						sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
						sqlCommand.Parameters.AddWithValue("UBGTransfer" + i, item.UBGTransfer == null ? (object)DBNull.Value : item.UBGTransfer);
						sqlCommand.Parameters.AddWithValue("Urs_Artikelnummer" + i, item.Urs_Artikelnummer == null ? (object)DBNull.Value : item.Urs_Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Urs_Fa" + i, item.Urs_Fa == null ? (object)DBNull.Value : item.Urs_Fa);
						sqlCommand.Parameters.AddWithValue("Zeit" + i, item.Zeit == null ? (object)DBNull.Value : item.Zeit);
						sqlCommand.Parameters.AddWithValue("PlanningDateViolation" + i, item.PlanningDateViolation == null ? (object)DBNull.Value : item.PlanningDateViolation);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Fertigung] SET [Angebot_Artikel_Nr]=@Angebot_Artikel_Nr,[Angebot_nr]=@Angebot_nr, [Anzahl]=@Anzahl, [Anzahl_aktuell]=@Anzahl_aktuell, [Anzahl_erledigt]=@Anzahl_erledigt, [AnzahlnachgedrucktPPS]=@AnzahlnachgedrucktPPS, [Artikel_Nr]=@Artikel_Nr, [Ausgangskontrolle]=@Ausgangskontrolle, [Bemerkung]=@Bemerkung, [Bemerkung II Planung]=@Bemerkung_II_Planung, [Bemerkung ohne stätte]=@Bemerkung_ohne_statte, [Bemerkung_Kommissionierung_AL]=@Bemerkung_Kommissionierung_AL, [Bemerkung_Planung]=@Bemerkung_Planung, [Bemerkung_Technik]=@Bemerkung_Technik, [Bemerkung_zu_Prio]=@Bemerkung_zu_Prio, [BomVersion]=@BomVersion, [CAO]=@CAO, [Check_FAbegonnen]=@Check_FAbegonnen, [Check_Gewerk1]=@Check_Gewerk1, [Check_Gewerk1_Teilweise]=@Check_Gewerk1_Teilweise, [Check_Gewerk2]=@Check_Gewerk2, [Check_Gewerk2_Teilweise]=@Check_Gewerk2_Teilweise, [Check_Gewerk3]=@Check_Gewerk3, [Check_Gewerk3_Teilweise]=@Check_Gewerk3_Teilweise, [Check_Kabelgeschnitten]=@Check_Kabelgeschnitten, [CPVersion]=@CPVersion, [Datum]=@Datum, [Endkontrolle]=@Endkontrolle, [Erledigte_FA_Datum]=@Erledigte_FA_Datum, [Erstmuster]=@Erstmuster, [FA_begonnen]=@FA_begonnen, [FA_Druckdatum]=@FA_Druckdatum, [FA_Gestartet]=@FA_Gestartet, [Fa-NachdruckPPS]=@Fa_NachdruckPPS, [Fertigungsnummer]=@Fertigungsnummer, [gebucht]=@gebucht, [gedruckt]=@gedruckt, [Gewerk 1]=@Gewerk_1, [Gewerk 2]=@Gewerk_2, [Gewerk 3]=@Gewerk_3, [Gewerk_Teilweise_Bemerkung]=@Gewerk_Teilweise_Bemerkung, [GrundNachdruckPPS]=@GrundNachdruckPPS, [HBGFAPositionId]=@HBGFAPositionId, [ID_Hauptartikel]=@ID_Hauptartikel, [ID_Rahmenfertigung]=@ID_Rahmenfertigung, [Kabel_geschnitten]=@Kabel_geschnitten, [Kabel_geschnitten_Datum]=@Kabel_geschnitten_Datum, [Kabel_Schneidebeginn]=@Kabel_Schneidebeginn, [Kabel_Schneidebeginn_Datum]=@Kabel_Schneidebeginn_Datum, [Kennzeichen]=@Kennzeichen, [Kommisioniert_komplett]=@Kommisioniert_komplett, [Kommisioniert_teilweise]=@Kommisioniert_teilweise, [Kunden_Index_Datum]=@Kunden_Index_Datum, [KundenIndex]=@KundenIndex, [Lagerort_id]=@Lagerort_id, [Lagerort_id zubuchen]=@Lagerort_id_zubuchen, [Letzte_Gebuchte_Menge]=@Letzte_Gebuchte_Menge, [Löschen]=@Loschen, [Mandant]=@Mandant, [Menge1]=@Menge1, [Menge2]=@Menge2, [Originalanzahl]=@Originalanzahl, [Planungsstatus]=@Planungsstatus, [Preis]=@Preis, [Prio]=@Prio, [Quick_Area]=@Quick_Area, [ROH_umgebucht]=@ROH_umgebucht, [Spritzgießerei_abgeschlossen]=@Spritzgiesserei_abgeschlossen, [Tage Abweichung]=@Tage_Abweichung, [Technik]=@Technik, [Techniker]=@Techniker, [Termin_Bestätigt1]=@Termin_Bestatigt1, [Termin_Bestätigt2]=@Termin_Bestatigt2, [Termin_Fertigstellung]=@Termin_Fertigstellung, [Termin_Material]=@Termin_Material, [Termin_Ursprünglich]=@Termin_Ursprunglich, [Termin_voränderung]=@Termin_voranderung, [UBG]=@UBG, [UBGTransfer]=@UBGTransfer, [Urs-Artikelnummer]=@Urs_Artikelnummer, [Urs-Fa]=@Urs_Fa, [Zeit]=@Zeit, [Termin_Bestatigt2_Updated]=@Termin_Bestatigt2_Updated,[PlanningDateViolation]=@PlanningDateViolation WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr", item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Angebot_nr", item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2_Updated", item.Termin_Bestatigt2_Updated == null ? (object)DBNull.Value : item.Termin_Bestatigt2_Updated);
				sqlCommand.Parameters.AddWithValue("Anzahl_aktuell", item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
				sqlCommand.Parameters.AddWithValue("Anzahl_erledigt", item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
				sqlCommand.Parameters.AddWithValue("AnzahlnachgedrucktPPS", item.AnzahlnachgedrucktPPS);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Ausgangskontrolle", item.Ausgangskontrolle == null ? (object)DBNull.Value : item.Ausgangskontrolle);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Bemerkung_II_Planung", item.Bemerkung_II_Planung == null ? (object)DBNull.Value : item.Bemerkung_II_Planung);
				sqlCommand.Parameters.AddWithValue("Bemerkung_ohne_statte", item.Bemerkung_ohne_statte == null ? (object)DBNull.Value : item.Bemerkung_ohne_statte);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL", item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Planung", item.Bemerkung_Planung == null ? (object)DBNull.Value : item.Bemerkung_Planung);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Technik", item.Bemerkung_Technik == null ? (object)DBNull.Value : item.Bemerkung_Technik);
				sqlCommand.Parameters.AddWithValue("Bemerkung_zu_Prio", item.Bemerkung_zu_Prio == null ? (object)DBNull.Value : item.Bemerkung_zu_Prio);
				sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
				sqlCommand.Parameters.AddWithValue("CAO", item.CAO == null ? (object)DBNull.Value : item.CAO);
				sqlCommand.Parameters.AddWithValue("Check_FAbegonnen", item.Check_FAbegonnen == null ? (object)DBNull.Value : item.Check_FAbegonnen);
				sqlCommand.Parameters.AddWithValue("Check_Gewerk1", item.Check_Gewerk1 == null ? (object)DBNull.Value : item.Check_Gewerk1);
				sqlCommand.Parameters.AddWithValue("Check_Gewerk1_Teilweise", item.Check_Gewerk1_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk1_Teilweise);
				sqlCommand.Parameters.AddWithValue("Check_Gewerk2", item.Check_Gewerk2 == null ? (object)DBNull.Value : item.Check_Gewerk2);
				sqlCommand.Parameters.AddWithValue("Check_Gewerk2_Teilweise", item.Check_Gewerk2_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk2_Teilweise);
				sqlCommand.Parameters.AddWithValue("Check_Gewerk3", item.Check_Gewerk3 == null ? (object)DBNull.Value : item.Check_Gewerk3);
				sqlCommand.Parameters.AddWithValue("Check_Gewerk3_Teilweise", item.Check_Gewerk3_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk3_Teilweise);
				sqlCommand.Parameters.AddWithValue("Check_Kabelgeschnitten", item.Check_Kabelgeschnitten == null ? (object)DBNull.Value : item.Check_Kabelgeschnitten);
				sqlCommand.Parameters.AddWithValue("CPVersion", item.CPVersion == null ? (object)DBNull.Value : item.CPVersion);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Endkontrolle", item.Endkontrolle == null ? (object)DBNull.Value : item.Endkontrolle);
				sqlCommand.Parameters.AddWithValue("Erledigte_FA_Datum", item.Erledigte_FA_Datum == null ? (object)DBNull.Value : item.Erledigte_FA_Datum);
				sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
				sqlCommand.Parameters.AddWithValue("FA_begonnen", item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
				sqlCommand.Parameters.AddWithValue("FA_Druckdatum", item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
				sqlCommand.Parameters.AddWithValue("FA_Gestartet", item.FA_Gestartet == null ? (object)DBNull.Value : item.FA_Gestartet);
				sqlCommand.Parameters.AddWithValue("Fa_NachdruckPPS", item.Fa_NachdruckPPS == null ? (object)DBNull.Value : item.Fa_NachdruckPPS);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("gebucht", item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
				sqlCommand.Parameters.AddWithValue("gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
				sqlCommand.Parameters.AddWithValue("Gewerk_1", item.Gewerk_1);
				sqlCommand.Parameters.AddWithValue("Gewerk_2", item.Gewerk_2);
				sqlCommand.Parameters.AddWithValue("Gewerk_3", item.Gewerk_3);
				sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung", item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
				sqlCommand.Parameters.AddWithValue("GrundNachdruckPPS", item.GrundNachdruckPPS == null ? (object)DBNull.Value : item.GrundNachdruckPPS);
				sqlCommand.Parameters.AddWithValue("HBGFAPositionId", item.HBGFAPositionId == null ? (object)DBNull.Value : item.HBGFAPositionId);
				sqlCommand.Parameters.AddWithValue("ID_Hauptartikel", item.ID_Hauptartikel == null ? (object)DBNull.Value : item.ID_Hauptartikel);
				sqlCommand.Parameters.AddWithValue("ID_Rahmenfertigung", item.ID_Rahmenfertigung == null ? (object)DBNull.Value : item.ID_Rahmenfertigung);
				sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
				sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum", item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
				sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn", item.Kabel_Schneidebeginn == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn);
				sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn_Datum", item.Kabel_Schneidebeginn_Datum == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn_Datum);
				sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
				sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett", item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
				sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise", item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
				sqlCommand.Parameters.AddWithValue("Kunden_Index_Datum", item.Kunden_Index_Datum == null ? (object)DBNull.Value : item.Kunden_Index_Datum);
				sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Lagerort_id_zubuchen", item.Lagerort_id_zubuchen == null ? (object)DBNull.Value : item.Lagerort_id_zubuchen);
				sqlCommand.Parameters.AddWithValue("Letzte_Gebuchte_Menge", item.Letzte_Gebuchte_Menge == null ? (object)DBNull.Value : item.Letzte_Gebuchte_Menge);
				sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
				sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
				sqlCommand.Parameters.AddWithValue("Menge1", item.Menge1 == null ? (object)DBNull.Value : item.Menge1);
				sqlCommand.Parameters.AddWithValue("Menge2", item.Menge2 == null ? (object)DBNull.Value : item.Menge2);
				sqlCommand.Parameters.AddWithValue("Originalanzahl", item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
				sqlCommand.Parameters.AddWithValue("Planungsstatus", item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
				sqlCommand.Parameters.AddWithValue("Preis", item.Preis == null ? (object)DBNull.Value : item.Preis);
				sqlCommand.Parameters.AddWithValue("Prio", item.Prio == null ? (object)DBNull.Value : item.Prio);
				sqlCommand.Parameters.AddWithValue("Quick_Area", item.Quick_Area == null ? (object)DBNull.Value : item.Quick_Area);
				sqlCommand.Parameters.AddWithValue("ROH_umgebucht", item.ROH_umgebucht == null ? (object)DBNull.Value : item.ROH_umgebucht);
				sqlCommand.Parameters.AddWithValue("Spritzgiesserei_abgeschlossen", item.SpritzgieBerei_abgeschlossen == null ? (object)DBNull.Value : item.SpritzgieBerei_abgeschlossen);
				sqlCommand.Parameters.AddWithValue("Tage_Abweichung", item.Tage_Abweichung == null ? (object)DBNull.Value : item.Tage_Abweichung);
				sqlCommand.Parameters.AddWithValue("Technik", item.Technik == null ? (object)DBNull.Value : item.Technik);
				sqlCommand.Parameters.AddWithValue("Techniker", item.Techniker == null ? (object)DBNull.Value : item.Techniker);
				sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
				sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2", item.Termin_Bestatigt2 == null ? (object)DBNull.Value : item.Termin_Bestatigt2);
				sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung", item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
				sqlCommand.Parameters.AddWithValue("Termin_Material", item.Termin_Material == null ? (object)DBNull.Value : item.Termin_Material);
				sqlCommand.Parameters.AddWithValue("Termin_Ursprunglich", item.Termin_Ursprunglich == null ? (object)DBNull.Value : item.Termin_Ursprunglich);
				sqlCommand.Parameters.AddWithValue("Termin_voranderung", item.Termin_voranderung == null ? (object)DBNull.Value : item.Termin_voranderung);
				sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
				sqlCommand.Parameters.AddWithValue("UBGTransfer", item.UBGTransfer == null ? (object)DBNull.Value : item.UBGTransfer);
				sqlCommand.Parameters.AddWithValue("Urs_Artikelnummer", item.Urs_Artikelnummer == null ? (object)DBNull.Value : item.Urs_Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Urs_Fa", item.Urs_Fa == null ? (object)DBNull.Value : item.Urs_Fa);
				sqlCommand.Parameters.AddWithValue("Zeit", item.Zeit == null ? (object)DBNull.Value : item.Zeit);
				sqlCommand.Parameters.AddWithValue("PlanningDateViolation", item.PlanningDateViolation == null ? (object)DBNull.Value : item.PlanningDateViolation);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 84; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> items)
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
						query += " UPDATE [Fertigung] SET "

							+ "[Angebot_Artikel_Nr]=@Angebot_Artikel_Nr" + i + ","
							+ "[Angebot_nr]=@Angebot_nr" + i + ","
							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[Anzahl_aktuell]=@Anzahl_aktuell" + i + ","
							+ "[Anzahl_erledigt]=@Anzahl_erledigt" + i + ","
							+ "[AnzahlnachgedrucktPPS]=@AnzahlnachgedrucktPPS" + i + ","
							+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
							+ "[Ausgangskontrolle]=@Ausgangskontrolle" + i + ","
							+ "[Bemerkung]=@Bemerkung" + i + ","
							+ "[Bemerkung II Planung]=@Bemerkung_II_Planung" + i + ","
							+ "[Bemerkung ohne stätte]=@Bemerkung_ohne_statte" + i + ","
							+ "[Bemerkung_Kommissionierung_AL]=@Bemerkung_Kommissionierung_AL" + i + ","
							+ "[Bemerkung_Planung]=@Bemerkung_Planung" + i + ","
							+ "[Bemerkung_Technik]=@Bemerkung_Technik" + i + ","
							+ "[Bemerkung_zu_Prio]=@Bemerkung_zu_Prio" + i + ","
							+ "[BomVersion]=@BomVersion" + i + ","
							+ "[CAO]=@CAO" + i + ","
							+ "[Check_FAbegonnen]=@Check_FAbegonnen" + i + ","
							+ "[Check_Gewerk1]=@Check_Gewerk1" + i + ","
							+ "[Check_Gewerk1_Teilweise]=@Check_Gewerk1_Teilweise" + i + ","
							+ "[Check_Gewerk2]=@Check_Gewerk2" + i + ","
							+ "[Check_Gewerk2_Teilweise]=@Check_Gewerk2_Teilweise" + i + ","
							+ "[Check_Gewerk3]=@Check_Gewerk3" + i + ","
							+ "[Check_Gewerk3_Teilweise]=@Check_Gewerk3_Teilweise" + i + ","
							+ "[Check_Kabelgeschnitten]=@Check_Kabelgeschnitten" + i + ","
							+ "[CPVersion]=@CPVersion" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[Endkontrolle]=@Endkontrolle" + i + ","
							+ "[Erledigte_FA_Datum]=@Erledigte_FA_Datum" + i + ","
							+ "[Erstmuster]=@Erstmuster" + i + ","
							+ "[FA_begonnen]=@FA_begonnen" + i + ","
							+ "[FA_Druckdatum]=@FA_Druckdatum" + i + ","
							+ "[FA_Gestartet]=@FA_Gestartet" + i + ","
							+ "[Fa-NachdruckPPS]=@Fa_NachdruckPPS" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[gebucht]=@gebucht" + i + ","
							+ "[gedruckt]=@gedruckt" + i + ","
							+ "[Gewerk 1]=@Gewerk_1" + i + ","
							+ "[Gewerk 2]=@Gewerk_2" + i + ","
							+ "[Gewerk 3]=@Gewerk_3" + i + ","
							+ "[Gewerk_Teilweise_Bemerkung]=@Gewerk_Teilweise_Bemerkung" + i + ","
							+ "[GrundNachdruckPPS]=@GrundNachdruckPPS" + i + ","
							+ "[HBGFAPositionId]=@HBGFAPositionId" + i + ","
							+ "[ID_Hauptartikel]=@ID_Hauptartikel" + i + ","
							+ "[ID_Rahmenfertigung]=@ID_Rahmenfertigung" + i + ","
							+ "[Kabel_geschnitten]=@Kabel_geschnitten" + i + ","
							+ "[Kabel_geschnitten_Datum]=@Kabel_geschnitten_Datum" + i + ","
							+ "[Kabel_Schneidebeginn]=@Kabel_Schneidebeginn" + i + ","
							+ "[Kabel_Schneidebeginn_Datum]=@Kabel_Schneidebeginn_Datum" + i + ","
							+ "[Kennzeichen]=@Kennzeichen" + i + ","
							+ "[Kommisioniert_komplett]=@Kommisioniert_komplett" + i + ","
							+ "[Kommisioniert_teilweise]=@Kommisioniert_teilweise" + i + ","
							+ "[Kunden_Index_Datum]=@Kunden_Index_Datum" + i + ","
							+ "[KundenIndex]=@KundenIndex" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[Lagerort_id zubuchen]=@Lagerort_id_zubuchen" + i + ","
							+ "[Letzte_Gebuchte_Menge]=@Letzte_Gebuchte_Menge" + i + ","
							+ "[Löschen]=@Loschen" + i + ","
							+ "[Mandant]=@Mandant" + i + ","
							+ "[Menge1]=@Menge1" + i + ","
							+ "[Menge2]=@Menge2" + i + ","
							+ "[Originalanzahl]=@Originalanzahl" + i + ","
							+ "[Planungsstatus]=@Planungsstatus" + i + ","
							+ "[Preis]=@Preis" + i + ","
							+ "[Prio]=@Prio" + i + ","
							+ "[Quick_Area]=@Quick_Area" + i + ","
							+ "[ROH_umgebucht]=@ROH_umgebucht" + i + ","
							+ "[Spritzgießerei_abgeschlossen]=@Spritzgiesserei_abgeschlossen" + i + ","
							+ "[Tage Abweichung]=@Tage_Abweichung" + i + ","
							+ "[Technik]=@Technik" + i + ","
							+ "[Techniker]=@Techniker" + i + ","
							+ "[Termin_Bestätigt1]=@Termin_Bestatigt1" + i + ","
							+ "[Termin_Bestätigt2]=@Termin_Bestatigt2" + i + ","
							+ "[Termin_Fertigstellung]=@Termin_Fertigstellung" + i + ","
							+ "[Termin_Material]=@Termin_Material" + i + ","
							+ "[Termin_Ursprünglich]=@Termin_Ursprunglich" + i + ","
							+ "[Termin_voränderung]=@Termin_voranderung" + i + ","
							+ "[UBG]=@UBG" + i + ","
							+ "[UBGTransfer]=@UBGTransfer" + i + ","
							+ "[Urs-Artikelnummer]=@Urs_Artikelnummer" + i + ","
							+ "[Urs-Fa]=@Urs_Fa" + i + ","
							+ "[Termin_Bestatigt2_Updated]=@Termin_Bestatigt2_Updated" + i + ","
							+ "[PlanningDateViolation]=@PlanningDateViolation" + i + ","
							+ "[Zeit]=@Zeit" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr" + i, item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Angebot_nr" + i, item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Anzahl_aktuell" + i, item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
						sqlCommand.Parameters.AddWithValue("Anzahl_erledigt" + i, item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
						sqlCommand.Parameters.AddWithValue("AnzahlnachgedrucktPPS" + i, item.AnzahlnachgedrucktPPS);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Ausgangskontrolle" + i, item.Ausgangskontrolle == null ? (object)DBNull.Value : item.Ausgangskontrolle);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bemerkung_II_Planung" + i, item.Bemerkung_II_Planung == null ? (object)DBNull.Value : item.Bemerkung_II_Planung);
						sqlCommand.Parameters.AddWithValue("Bemerkung_ohne_statte" + i, item.Bemerkung_ohne_statte == null ? (object)DBNull.Value : item.Bemerkung_ohne_statte);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL" + i, item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Planung" + i, item.Bemerkung_Planung == null ? (object)DBNull.Value : item.Bemerkung_Planung);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Technik" + i, item.Bemerkung_Technik == null ? (object)DBNull.Value : item.Bemerkung_Technik);
						sqlCommand.Parameters.AddWithValue("Bemerkung_zu_Prio" + i, item.Bemerkung_zu_Prio == null ? (object)DBNull.Value : item.Bemerkung_zu_Prio);
						sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
						sqlCommand.Parameters.AddWithValue("CAO" + i, item.CAO == null ? (object)DBNull.Value : item.CAO);
						sqlCommand.Parameters.AddWithValue("Check_FAbegonnen" + i, item.Check_FAbegonnen == null ? (object)DBNull.Value : item.Check_FAbegonnen);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk1" + i, item.Check_Gewerk1 == null ? (object)DBNull.Value : item.Check_Gewerk1);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk1_Teilweise" + i, item.Check_Gewerk1_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk1_Teilweise);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk2" + i, item.Check_Gewerk2 == null ? (object)DBNull.Value : item.Check_Gewerk2);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk2_Teilweise" + i, item.Check_Gewerk2_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk2_Teilweise);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk3" + i, item.Check_Gewerk3 == null ? (object)DBNull.Value : item.Check_Gewerk3);
						sqlCommand.Parameters.AddWithValue("Check_Gewerk3_Teilweise" + i, item.Check_Gewerk3_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk3_Teilweise);
						sqlCommand.Parameters.AddWithValue("Check_Kabelgeschnitten" + i, item.Check_Kabelgeschnitten == null ? (object)DBNull.Value : item.Check_Kabelgeschnitten);
						sqlCommand.Parameters.AddWithValue("CPVersion" + i, item.CPVersion == null ? (object)DBNull.Value : item.CPVersion);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Endkontrolle" + i, item.Endkontrolle == null ? (object)DBNull.Value : item.Endkontrolle);
						sqlCommand.Parameters.AddWithValue("Erledigte_FA_Datum" + i, item.Erledigte_FA_Datum == null ? (object)DBNull.Value : item.Erledigte_FA_Datum);
						sqlCommand.Parameters.AddWithValue("Erstmuster" + i, item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
						sqlCommand.Parameters.AddWithValue("FA_begonnen" + i, item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
						sqlCommand.Parameters.AddWithValue("FA_Druckdatum" + i, item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
						sqlCommand.Parameters.AddWithValue("FA_Gestartet" + i, item.FA_Gestartet == null ? (object)DBNull.Value : item.FA_Gestartet);
						sqlCommand.Parameters.AddWithValue("Fa_NachdruckPPS" + i, item.Fa_NachdruckPPS == null ? (object)DBNull.Value : item.Fa_NachdruckPPS);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
						sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
						sqlCommand.Parameters.AddWithValue("Gewerk_1" + i, item.Gewerk_1);
						sqlCommand.Parameters.AddWithValue("Gewerk_2" + i, item.Gewerk_2);
						sqlCommand.Parameters.AddWithValue("Gewerk_3" + i, item.Gewerk_3);
						sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung" + i, item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
						sqlCommand.Parameters.AddWithValue("GrundNachdruckPPS" + i, item.GrundNachdruckPPS == null ? (object)DBNull.Value : item.GrundNachdruckPPS);
						sqlCommand.Parameters.AddWithValue("HBGFAPositionId" + i, item.HBGFAPositionId == null ? (object)DBNull.Value : item.HBGFAPositionId);
						sqlCommand.Parameters.AddWithValue("ID_Hauptartikel" + i, item.ID_Hauptartikel == null ? (object)DBNull.Value : item.ID_Hauptartikel);
						sqlCommand.Parameters.AddWithValue("ID_Rahmenfertigung" + i, item.ID_Rahmenfertigung == null ? (object)DBNull.Value : item.ID_Rahmenfertigung);
						sqlCommand.Parameters.AddWithValue("Kabel_geschnitten" + i, item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
						sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum" + i, item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
						sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn" + i, item.Kabel_Schneidebeginn == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn);
						sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn_Datum" + i, item.Kabel_Schneidebeginn_Datum == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn_Datum);
						sqlCommand.Parameters.AddWithValue("Kennzeichen" + i, item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
						sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett" + i, item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
						sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise" + i, item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
						sqlCommand.Parameters.AddWithValue("Kunden_Index_Datum" + i, item.Kunden_Index_Datum == null ? (object)DBNull.Value : item.Kunden_Index_Datum);
						sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Lagerort_id_zubuchen" + i, item.Lagerort_id_zubuchen == null ? (object)DBNull.Value : item.Lagerort_id_zubuchen);
						sqlCommand.Parameters.AddWithValue("Letzte_Gebuchte_Menge" + i, item.Letzte_Gebuchte_Menge == null ? (object)DBNull.Value : item.Letzte_Gebuchte_Menge);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("Menge1" + i, item.Menge1 == null ? (object)DBNull.Value : item.Menge1);
						sqlCommand.Parameters.AddWithValue("Menge2" + i, item.Menge2 == null ? (object)DBNull.Value : item.Menge2);
						sqlCommand.Parameters.AddWithValue("Originalanzahl" + i, item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
						sqlCommand.Parameters.AddWithValue("Planungsstatus" + i, item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
						sqlCommand.Parameters.AddWithValue("Preis" + i, item.Preis == null ? (object)DBNull.Value : item.Preis);
						sqlCommand.Parameters.AddWithValue("Prio" + i, item.Prio == null ? (object)DBNull.Value : item.Prio);
						sqlCommand.Parameters.AddWithValue("Quick_Area" + i, item.Quick_Area == null ? (object)DBNull.Value : item.Quick_Area);
						sqlCommand.Parameters.AddWithValue("ROH_umgebucht" + i, item.ROH_umgebucht == null ? (object)DBNull.Value : item.ROH_umgebucht);
						sqlCommand.Parameters.AddWithValue("Spritzgiesserei_abgeschlossen" + i, item.SpritzgieBerei_abgeschlossen == null ? (object)DBNull.Value : item.SpritzgieBerei_abgeschlossen);
						sqlCommand.Parameters.AddWithValue("Tage_Abweichung" + i, item.Tage_Abweichung == null ? (object)DBNull.Value : item.Tage_Abweichung);
						sqlCommand.Parameters.AddWithValue("Technik" + i, item.Technik == null ? (object)DBNull.Value : item.Technik);
						sqlCommand.Parameters.AddWithValue("Techniker" + i, item.Techniker == null ? (object)DBNull.Value : item.Techniker);
						sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
						sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2" + i, item.Termin_Bestatigt2 == null ? (object)DBNull.Value : item.Termin_Bestatigt2);
						sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung" + i, item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
						sqlCommand.Parameters.AddWithValue("Termin_Material" + i, item.Termin_Material == null ? (object)DBNull.Value : item.Termin_Material);
						sqlCommand.Parameters.AddWithValue("Termin_Ursprunglich" + i, item.Termin_Ursprunglich == null ? (object)DBNull.Value : item.Termin_Ursprunglich);
						sqlCommand.Parameters.AddWithValue("Termin_voranderung" + i, item.Termin_voranderung == null ? (object)DBNull.Value : item.Termin_voranderung);
						sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
						sqlCommand.Parameters.AddWithValue("UBGTransfer" + i, item.UBGTransfer == null ? (object)DBNull.Value : item.UBGTransfer);
						sqlCommand.Parameters.AddWithValue("Urs_Artikelnummer" + i, item.Urs_Artikelnummer == null ? (object)DBNull.Value : item.Urs_Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Urs_Fa" + i, item.Urs_Fa == null ? (object)DBNull.Value : item.Urs_Fa);
						sqlCommand.Parameters.AddWithValue("Zeit" + i, item.Zeit == null ? (object)DBNull.Value : item.Zeit);
						sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2_Updated" + i, item.Termin_Bestatigt2_Updated == null ? (object)DBNull.Value : item.Termin_Bestatigt2_Updated);
						sqlCommand.Parameters.AddWithValue("PlanningDateViolation" + i, item.PlanningDateViolation == null ? (object)DBNull.Value : item.PlanningDateViolation);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int Delete(int angebot_artikel_nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Fertigung] WHERE [Angebot_Artikel_Nr]=@Angebot_Artikel_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr", angebot_artikel_nr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#region Methods with transaction
		//public static Infrastructure.Data.Entities.Tables.PRS.FertigungEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		//{
		//    var dataTable = new DataTable();

		//    string query = "SELECT * FROM [Fertigung] WHERE [ID]=@Id";
		//    var sqlCommand = new SqlCommand(query, connection, transaction);
		//    sqlCommand.Parameters.AddWithValue("Id", id);
		//    DbExecution.Fill(sqlCommand, dataTable);

		//    if (dataTable.Rows.Count > 0)
		//    {
		//        return new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(dataTable.Rows[0]);
		//    }
		//    else
		//    {
		//        return null;
		//    }
		//}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Fertigung]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Fertigung] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
		}

		//public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity item, SqlConnection connection, SqlTransaction transaction)
		//{
		//    int response = int.MinValue;


		//    string query = "INSERT INTO [Fertigung] ([Angebot_Artikel_Nr],[Angebot_nr],[Anzahl],[Anzahl_aktuell],[Anzahl_erledigt],[AnzahlnachgedrucktPPS],[Artikel_Nr],[Ausgangskontrolle],[Bemerkung],[Bemerkung II Planung],[Bemerkung ohne stätte],[Bemerkung_Kommissionierung_AL],[Bemerkung_Planung],[Bemerkung_Technik],[Bemerkung_zu_Prio],[BomVersion],[CAO],[Check_FAbegonnen],[Check_Gewerk1],[Check_Gewerk1_Teilweise],[Check_Gewerk2],[Check_Gewerk2_Teilweise],[Check_Gewerk3],[Check_Gewerk3_Teilweise],[Check_Kabelgeschnitten],[CPVersion],[Datum],[Endkontrolle],[Erledigte_FA_Datum],[Erstmuster],[FA_begonnen],[FA_Druckdatum],[FA_Gestartet],[Fa-NachdruckPPS],[Fertigungsnummer],[gebucht],[gedruckt],[Gewerk 1],[Gewerk 2],[Gewerk 3],[Gewerk_Teilweise_Bemerkung],[GrundNachdruckPPS],[ID_Hauptartikel],[ID_Rahmenfertigung],[Kabel_geschnitten],[Kabel_geschnitten_Datum],[Kabel_Schneidebeginn],[Kabel_Schneidebeginn_Datum],[Kennzeichen],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kunden_Index_Datum],[KundenIndex],[Lagerort_id],[Lagerort_id zubuchen],[Letzte_Gebuchte_Menge],[Löschen],[Mandant],[Menge1],[Menge2],[Originalanzahl],[Planungsstatus],[Preis],[Prio],[Quick_Area],[ROH_umgebucht],[Spritzgießerei_abgeschlossen],[Tage Abweichung],[Technik],[Techniker],[Termin_Bestätigt1],[Termin_Bestätigt2],[Termin_Fertigstellung],[Termin_Material],[Termin_Ursprünglich],[Termin_voränderung],[UBG],[UBGTransfer],[Urs-Artikelnummer],[Urs-Fa],[Zeit]) OUTPUT INSERTED.[ID] VALUES (@Angebot_Artikel_Nr,@Angebot_nr,@Anzahl,@Anzahl_aktuell,@Anzahl_erledigt,@AnzahlnachgedrucktPPS,@Artikel_Nr,@Ausgangskontrolle,@Bemerkung,@Bemerkung_II_Planung,@Bemerkung_ohne_statte,@Bemerkung_Kommissionierung_AL,@Bemerkung_Planung,@Bemerkung_Technik,@Bemerkung_zu_Prio,@BomVersion,@CAO,@Check_FAbegonnen,@Check_Gewerk1,@Check_Gewerk1_Teilweise,@Check_Gewerk2,@Check_Gewerk2_Teilweise,@Check_Gewerk3,@Check_Gewerk3_Teilweise,@Check_Kabelgeschnitten,@CPVersion,@Datum,@Endkontrolle,@Erledigte_FA_Datum,@Erstmuster,@FA_begonnen,@FA_Druckdatum,@FA_Gestartet,@Fa_NachdruckPPS,@Fertigungsnummer,@gebucht,@gedruckt,@Gewerk_1,@Gewerk_2,@Gewerk_3,@Gewerk_Teilweise_Bemerkung,@GrundNachdruckPPS,@ID_Hauptartikel,@ID_Rahmenfertigung,@Kabel_geschnitten,@Kabel_geschnitten_Datum,@Kabel_Schneidebeginn,@Kabel_Schneidebeginn_Datum,@Kennzeichen,@Kommisioniert_komplett,@Kommisioniert_teilweise,@Kunden_Index_Datum,@KundenIndex,@Lagerort_id,@Lagerort_id_zubuchen,@Letzte_Gebuchte_Menge,@Loschen,@Mandant,@Menge1,@Menge2,@Originalanzahl,@Planungsstatus,@Preis,@Prio,@Quick_Area,@ROH_umgebucht,@Spritzgiesserei_abgeschlossen,@Tage_Abweichung,@Technik,@Techniker,@Termin_Bestatigt1,@Termin_Bestatigt2,@Termin_Fertigstellung,@Termin_Material,@Termin_Ursprunglich,@Termin_voranderung,@UBG,@UBGTransfer,@Urs_Artikelnummer,@Urs_Fa,@Zeit); ";


		//    var sqlCommand = new SqlCommand(query, connection, transaction);
		//    sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr", item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
		//    sqlCommand.Parameters.AddWithValue("Angebot_nr", item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
		//    sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
		//    sqlCommand.Parameters.AddWithValue("Anzahl_aktuell", item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
		//    sqlCommand.Parameters.AddWithValue("Anzahl_erledigt", item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
		//    sqlCommand.Parameters.AddWithValue("AnzahlnachgedrucktPPS", item.AnzahlnachgedrucktPPS);
		//    sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
		//    sqlCommand.Parameters.AddWithValue("Ausgangskontrolle", item.Ausgangskontrolle == null ? (object)DBNull.Value : item.Ausgangskontrolle);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_II_Planung", item.Bemerkung_II_Planung == null ? (object)DBNull.Value : item.Bemerkung_II_Planung);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_ohne_statte", item.Bemerkung_ohne_statte == null ? (object)DBNull.Value : item.Bemerkung_ohne_statte);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL", item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_Planung", item.Bemerkung_Planung == null ? (object)DBNull.Value : item.Bemerkung_Planung);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_Technik", item.Bemerkung_Technik == null ? (object)DBNull.Value : item.Bemerkung_Technik);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_zu_Prio", item.Bemerkung_zu_Prio == null ? (object)DBNull.Value : item.Bemerkung_zu_Prio);
		//    sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
		//    sqlCommand.Parameters.AddWithValue("CAO", item.CAO == null ? (object)DBNull.Value : item.CAO);
		//    sqlCommand.Parameters.AddWithValue("Check_FAbegonnen", item.Check_FAbegonnen == null ? (object)DBNull.Value : item.Check_FAbegonnen);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk1", item.Check_Gewerk1 == null ? (object)DBNull.Value : item.Check_Gewerk1);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk1_Teilweise", item.Check_Gewerk1_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk1_Teilweise);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk2", item.Check_Gewerk2 == null ? (object)DBNull.Value : item.Check_Gewerk2);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk2_Teilweise", item.Check_Gewerk2_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk2_Teilweise);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk3", item.Check_Gewerk3 == null ? (object)DBNull.Value : item.Check_Gewerk3);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk3_Teilweise", item.Check_Gewerk3_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk3_Teilweise);
		//    sqlCommand.Parameters.AddWithValue("Check_Kabelgeschnitten", item.Check_Kabelgeschnitten == null ? (object)DBNull.Value : item.Check_Kabelgeschnitten);
		//    sqlCommand.Parameters.AddWithValue("CPVersion", item.CPVersion == null ? (object)DBNull.Value : item.CPVersion);
		//    sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
		//    sqlCommand.Parameters.AddWithValue("Endkontrolle", item.Endkontrolle == null ? (object)DBNull.Value : item.Endkontrolle);
		//    sqlCommand.Parameters.AddWithValue("Erledigte_FA_Datum", item.Erledigte_FA_Datum == null ? (object)DBNull.Value : item.Erledigte_FA_Datum);
		//    sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
		//    sqlCommand.Parameters.AddWithValue("FA_begonnen", item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
		//    sqlCommand.Parameters.AddWithValue("FA_Druckdatum", item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
		//    sqlCommand.Parameters.AddWithValue("FA_Gestartet", item.FA_Gestartet == null ? (object)DBNull.Value : item.FA_Gestartet);
		//    sqlCommand.Parameters.AddWithValue("Fa_NachdruckPPS", item.Fa_NachdruckPPS == null ? (object)DBNull.Value : item.Fa_NachdruckPPS);
		//    sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
		//    sqlCommand.Parameters.AddWithValue("gebucht", item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
		//    sqlCommand.Parameters.AddWithValue("gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
		//    sqlCommand.Parameters.AddWithValue("Gewerk_1", item.Gewerk_1 == null ? (object)DBNull.Value : item.Gewerk_1);
		//    sqlCommand.Parameters.AddWithValue("Gewerk_2", item.Gewerk_2);
		//    sqlCommand.Parameters.AddWithValue("Gewerk_3", item.Gewerk_3);
		//    sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung", item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
		//    sqlCommand.Parameters.AddWithValue("GrundNachdruckPPS", item.GrundNachdruckPPS == null ? (object)DBNull.Value : item.GrundNachdruckPPS);
		//    sqlCommand.Parameters.AddWithValue("ID_Hauptartikel", item.ID_Hauptartikel == null ? (object)DBNull.Value : item.ID_Hauptartikel);
		//    sqlCommand.Parameters.AddWithValue("ID_Rahmenfertigung", item.ID_Rahmenfertigung == null ? (object)DBNull.Value : item.ID_Rahmenfertigung);
		//    sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
		//    sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum", item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
		//    sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn", item.Kabel_Schneidebeginn == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn);
		//    sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn_Datum", item.Kabel_Schneidebeginn_Datum == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn_Datum);
		//    sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
		//    sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett", item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
		//    sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise", item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
		//    sqlCommand.Parameters.AddWithValue("Kunden_Index_Datum", item.Kunden_Index_Datum == null ? (object)DBNull.Value : item.Kunden_Index_Datum);
		//    sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
		//    sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
		//    sqlCommand.Parameters.AddWithValue("Lagerort_id_zubuchen", item.Lagerort_id_zubuchen == null ? (object)DBNull.Value : item.Lagerort_id_zubuchen);
		//    sqlCommand.Parameters.AddWithValue("Letzte_Gebuchte_Menge", item.Letzte_Gebuchte_Menge == null ? (object)DBNull.Value : item.Letzte_Gebuchte_Menge);
		//    sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
		//    sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
		//    sqlCommand.Parameters.AddWithValue("Menge1", item.Menge1 == null ? (object)DBNull.Value : item.Menge1);
		//    sqlCommand.Parameters.AddWithValue("Menge2", item.Menge2 == null ? (object)DBNull.Value : item.Menge2);
		//    sqlCommand.Parameters.AddWithValue("Originalanzahl", item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
		//    sqlCommand.Parameters.AddWithValue("Planungsstatus", item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
		//    sqlCommand.Parameters.AddWithValue("Preis", item.Preis == null ? (object)DBNull.Value : item.Preis);
		//    sqlCommand.Parameters.AddWithValue("Prio", item.Prio == null ? (object)DBNull.Value : item.Prio);
		//    sqlCommand.Parameters.AddWithValue("Quick_Area", item.Quick_Area == null ? (object)DBNull.Value : item.Quick_Area);
		//    sqlCommand.Parameters.AddWithValue("ROH_umgebucht", item.ROH_umgebucht == null ? (object)DBNull.Value : item.ROH_umgebucht);
		//    sqlCommand.Parameters.AddWithValue("Spritzgiesserei_abgeschlossen", item.SpritzgieBerei_abgeschlossen == null ? (object)DBNull.Value : item.SpritzgieBerei_abgeschlossen);
		//    sqlCommand.Parameters.AddWithValue("Tage_Abweichung", item.Tage_Abweichung == null ? (object)DBNull.Value : item.Tage_Abweichung);
		//    sqlCommand.Parameters.AddWithValue("Technik", item.Technik == null ? (object)DBNull.Value : item.Technik);
		//    sqlCommand.Parameters.AddWithValue("Techniker", item.Techniker == null ? (object)DBNull.Value : item.Techniker);
		//    sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
		//    sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2", item.Termin_Bestatigt2 == null ? (object)DBNull.Value : item.Termin_Bestatigt2);
		//    sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung", item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
		//    sqlCommand.Parameters.AddWithValue("Termin_Material", item.Termin_Material == null ? (object)DBNull.Value : item.Termin_Material);
		//    sqlCommand.Parameters.AddWithValue("Termin_Ursprunglich", item.Termin_Ursprunglich == null ? (object)DBNull.Value : item.Termin_Ursprunglich);
		//    sqlCommand.Parameters.AddWithValue("Termin_voranderung", item.Termin_voranderung == null ? (object)DBNull.Value : item.Termin_voranderung);
		//    sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
		//    sqlCommand.Parameters.AddWithValue("UBGTransfer", item.UBGTransfer == null ? (object)DBNull.Value : item.UBGTransfer);
		//    sqlCommand.Parameters.AddWithValue("Urs_Artikelnummer", item.Urs_Artikelnummer == null ? (object)DBNull.Value : item.Urs_Artikelnummer);
		//    sqlCommand.Parameters.AddWithValue("Urs_Fa", item.Urs_Fa == null ? (object)DBNull.Value : item.Urs_Fa);
		//    sqlCommand.Parameters.AddWithValue("Zeit", item.Zeit == null ? (object)DBNull.Value : item.Zeit);

		//    var result = DbExecution.ExecuteScalar(sqlCommand);
		//    return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		//}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 83; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Fertigung] ([Angebot_Artikel_Nr],[Angebot_nr],[Anzahl],[Anzahl_aktuell],[Anzahl_erledigt],[AnzahlnachgedrucktPPS],[Artikel_Nr],[Ausgangskontrolle],[Bemerkung],[Bemerkung II Planung],[Bemerkung ohne stätte],[Bemerkung_Kommissionierung_AL],[Bemerkung_Planung],[Bemerkung_Technik],[Bemerkung_zu_Prio],[BomVersion],[CAO],[Check_FAbegonnen],[Check_Gewerk1],[Check_Gewerk1_Teilweise],[Check_Gewerk2],[Check_Gewerk2_Teilweise],[Check_Gewerk3],[Check_Gewerk3_Teilweise],[Check_Kabelgeschnitten],[CPVersion],[Datum],[Endkontrolle],[Erledigte_FA_Datum],[Erstmuster],[FA_begonnen],[FA_Druckdatum],[FA_Gestartet],[Fa-NachdruckPPS],[Fertigungsnummer],[gebucht],[gedruckt],[Gewerk 1],[Gewerk 2],[Gewerk 3],[Gewerk_Teilweise_Bemerkung],[GrundNachdruckPPS],[ID_Hauptartikel],[ID_Rahmenfertigung],[Kabel_geschnitten],[Kabel_geschnitten_Datum],[Kabel_Schneidebeginn],[Kabel_Schneidebeginn_Datum],[Kennzeichen],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kunden_Index_Datum],[KundenIndex],[Lagerort_id],[Lagerort_id zubuchen],[Letzte_Gebuchte_Menge],[Löschen],[Mandant],[Menge1],[Menge2],[Originalanzahl],[Planungsstatus],[Preis],[Prio],[Quick_Area],[ROH_umgebucht],[Spritzgießerei_abgeschlossen],[Tage Abweichung],[Technik],[Techniker],[Termin_Bestätigt1],[Termin_Bestätigt2],[Termin_Fertigstellung],[Termin_Material],[Termin_Ursprünglich],[Termin_voränderung],[UBG],[UBGTransfer],[Urs-Artikelnummer],[Urs-Fa],[Zeit],[PlanningDateViolation]) VALUES ( "

						+ "@Angebot_Artikel_Nr" + i + ","
						+ "@Angebot_nr" + i + ","
						+ "@Anzahl" + i + ","
						+ "@Anzahl_aktuell" + i + ","
						+ "@Anzahl_erledigt" + i + ","
						+ "@AnzahlnachgedrucktPPS" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Ausgangskontrolle" + i + ","
						+ "@Bemerkung" + i + ","
						+ "@Bemerkung_II_Planung" + i + ","
						+ "@Bemerkung_ohne_statte" + i + ","
						+ "@Bemerkung_Kommissionierung_AL" + i + ","
						+ "@Bemerkung_Planung" + i + ","
						+ "@Bemerkung_Technik" + i + ","
						+ "@Bemerkung_zu_Prio" + i + ","
						+ "@BomVersion" + i + ","
						+ "@CAO" + i + ","
						+ "@Check_FAbegonnen" + i + ","
						+ "@Check_Gewerk1" + i + ","
						+ "@Check_Gewerk1_Teilweise" + i + ","
						+ "@Check_Gewerk2" + i + ","
						+ "@Check_Gewerk2_Teilweise" + i + ","
						+ "@Check_Gewerk3" + i + ","
						+ "@Check_Gewerk3_Teilweise" + i + ","
						+ "@Check_Kabelgeschnitten" + i + ","
						+ "@CPVersion" + i + ","
						+ "@Datum" + i + ","
						+ "@Endkontrolle" + i + ","
						+ "@Erledigte_FA_Datum" + i + ","
						+ "@Erstmuster" + i + ","
						+ "@FA_begonnen" + i + ","
						+ "@FA_Druckdatum" + i + ","
						+ "@FA_Gestartet" + i + ","
						+ "@Fa_NachdruckPPS" + i + ","
						+ "@Fertigungsnummer" + i + ","
						+ "@gebucht" + i + ","
						+ "@gedruckt" + i + ","
						+ "@Gewerk_1" + i + ","
						+ "@Gewerk_2" + i + ","
						+ "@Gewerk_3" + i + ","
						+ "@Gewerk_Teilweise_Bemerkung" + i + ","
						+ "@GrundNachdruckPPS" + i + ","
						+ "@ID_Hauptartikel" + i + ","
						+ "@ID_Rahmenfertigung" + i + ","
						+ "@Kabel_geschnitten" + i + ","
						+ "@Kabel_geschnitten_Datum" + i + ","
						+ "@Kabel_Schneidebeginn" + i + ","
						+ "@Kabel_Schneidebeginn_Datum" + i + ","
						+ "@Kennzeichen" + i + ","
						+ "@Kommisioniert_komplett" + i + ","
						+ "@Kommisioniert_teilweise" + i + ","
						+ "@Kunden_Index_Datum" + i + ","
						+ "@KundenIndex" + i + ","
						+ "@Lagerort_id" + i + ","
						+ "@Lagerort_id_zubuchen" + i + ","
						+ "@Letzte_Gebuchte_Menge" + i + ","
						+ "@Loschen" + i + ","
						+ "@Mandant" + i + ","
						+ "@Menge1" + i + ","
						+ "@Menge2" + i + ","
						+ "@Originalanzahl" + i + ","
						+ "@Planungsstatus" + i + ","
						+ "@Preis" + i + ","
						+ "@Prio" + i + ","
						+ "@Quick_Area" + i + ","
						+ "@ROH_umgebucht" + i + ","
						+ "@Spritzgiesserei_abgeschlossen" + i + ","
						+ "@Tage_Abweichung" + i + ","
						+ "@Technik" + i + ","
						+ "@Techniker" + i + ","
						+ "@Termin_Bestatigt1" + i + ","
						+ "@Termin_Bestatigt2" + i + ","
						+ "@Termin_Fertigstellung" + i + ","
						+ "@Termin_Material" + i + ","
						+ "@Termin_Ursprunglich" + i + ","
						+ "@Termin_voranderung" + i + ","
						+ "@UBG" + i + ","
						+ "@UBGTransfer" + i + ","
						+ "@Urs_Artikelnummer" + i + ","
						+ "@Urs_Fa" + i + ","
						+ "@Zeit" + i + ","
						+ "@PlanningDateViolation"
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr" + i, item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Angebot_nr" + i, item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Anzahl_aktuell" + i, item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
					sqlCommand.Parameters.AddWithValue("Anzahl_erledigt" + i, item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
					sqlCommand.Parameters.AddWithValue("AnzahlnachgedrucktPPS" + i, item.AnzahlnachgedrucktPPS);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Ausgangskontrolle" + i, item.Ausgangskontrolle == null ? (object)DBNull.Value : item.Ausgangskontrolle);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_II_Planung" + i, item.Bemerkung_II_Planung == null ? (object)DBNull.Value : item.Bemerkung_II_Planung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_ohne_statte" + i, item.Bemerkung_ohne_statte == null ? (object)DBNull.Value : item.Bemerkung_ohne_statte);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL" + i, item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Planung" + i, item.Bemerkung_Planung == null ? (object)DBNull.Value : item.Bemerkung_Planung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Technik" + i, item.Bemerkung_Technik == null ? (object)DBNull.Value : item.Bemerkung_Technik);
					sqlCommand.Parameters.AddWithValue("Bemerkung_zu_Prio" + i, item.Bemerkung_zu_Prio == null ? (object)DBNull.Value : item.Bemerkung_zu_Prio);
					sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("CAO" + i, item.CAO == null ? (object)DBNull.Value : item.CAO);
					sqlCommand.Parameters.AddWithValue("Check_FAbegonnen" + i, item.Check_FAbegonnen == null ? (object)DBNull.Value : item.Check_FAbegonnen);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk1" + i, item.Check_Gewerk1 == null ? (object)DBNull.Value : item.Check_Gewerk1);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk1_Teilweise" + i, item.Check_Gewerk1_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk1_Teilweise);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk2" + i, item.Check_Gewerk2 == null ? (object)DBNull.Value : item.Check_Gewerk2);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk2_Teilweise" + i, item.Check_Gewerk2_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk2_Teilweise);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk3" + i, item.Check_Gewerk3 == null ? (object)DBNull.Value : item.Check_Gewerk3);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk3_Teilweise" + i, item.Check_Gewerk3_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk3_Teilweise);
					sqlCommand.Parameters.AddWithValue("Check_Kabelgeschnitten" + i, item.Check_Kabelgeschnitten == null ? (object)DBNull.Value : item.Check_Kabelgeschnitten);
					sqlCommand.Parameters.AddWithValue("CPVersion" + i, item.CPVersion == null ? (object)DBNull.Value : item.CPVersion);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Endkontrolle" + i, item.Endkontrolle == null ? (object)DBNull.Value : item.Endkontrolle);
					sqlCommand.Parameters.AddWithValue("Erledigte_FA_Datum" + i, item.Erledigte_FA_Datum == null ? (object)DBNull.Value : item.Erledigte_FA_Datum);
					sqlCommand.Parameters.AddWithValue("Erstmuster" + i, item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
					sqlCommand.Parameters.AddWithValue("FA_begonnen" + i, item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
					sqlCommand.Parameters.AddWithValue("FA_Druckdatum" + i, item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
					sqlCommand.Parameters.AddWithValue("FA_Gestartet" + i, item.FA_Gestartet == null ? (object)DBNull.Value : item.FA_Gestartet);
					sqlCommand.Parameters.AddWithValue("Fa_NachdruckPPS" + i, item.Fa_NachdruckPPS == null ? (object)DBNull.Value : item.Fa_NachdruckPPS);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
					sqlCommand.Parameters.AddWithValue("Gewerk_1" + i, item.Gewerk_1 == null ? (object)DBNull.Value : item.Gewerk_1);
					sqlCommand.Parameters.AddWithValue("Gewerk_2" + i, item.Gewerk_2);
					sqlCommand.Parameters.AddWithValue("Gewerk_3" + i, item.Gewerk_3);
					sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung" + i, item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
					sqlCommand.Parameters.AddWithValue("GrundNachdruckPPS" + i, item.GrundNachdruckPPS == null ? (object)DBNull.Value : item.GrundNachdruckPPS);
					sqlCommand.Parameters.AddWithValue("ID_Hauptartikel" + i, item.ID_Hauptartikel == null ? (object)DBNull.Value : item.ID_Hauptartikel);
					sqlCommand.Parameters.AddWithValue("ID_Rahmenfertigung" + i, item.ID_Rahmenfertigung == null ? (object)DBNull.Value : item.ID_Rahmenfertigung);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten" + i, item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum" + i, item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
					sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn" + i, item.Kabel_Schneidebeginn == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn);
					sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn_Datum" + i, item.Kabel_Schneidebeginn_Datum == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn_Datum);
					sqlCommand.Parameters.AddWithValue("Kennzeichen" + i, item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett" + i, item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise" + i, item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
					sqlCommand.Parameters.AddWithValue("Kunden_Index_Datum" + i, item.Kunden_Index_Datum == null ? (object)DBNull.Value : item.Kunden_Index_Datum);
					sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Lagerort_id_zubuchen" + i, item.Lagerort_id_zubuchen == null ? (object)DBNull.Value : item.Lagerort_id_zubuchen);
					sqlCommand.Parameters.AddWithValue("Letzte_Gebuchte_Menge" + i, item.Letzte_Gebuchte_Menge == null ? (object)DBNull.Value : item.Letzte_Gebuchte_Menge);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Menge1" + i, item.Menge1 == null ? (object)DBNull.Value : item.Menge1);
					sqlCommand.Parameters.AddWithValue("Menge2" + i, item.Menge2 == null ? (object)DBNull.Value : item.Menge2);
					sqlCommand.Parameters.AddWithValue("Originalanzahl" + i, item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
					sqlCommand.Parameters.AddWithValue("Planungsstatus" + i, item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
					sqlCommand.Parameters.AddWithValue("Preis" + i, item.Preis == null ? (object)DBNull.Value : item.Preis);
					sqlCommand.Parameters.AddWithValue("Prio" + i, item.Prio == null ? (object)DBNull.Value : item.Prio);
					sqlCommand.Parameters.AddWithValue("Quick_Area" + i, item.Quick_Area == null ? (object)DBNull.Value : item.Quick_Area);
					sqlCommand.Parameters.AddWithValue("ROH_umgebucht" + i, item.ROH_umgebucht == null ? (object)DBNull.Value : item.ROH_umgebucht);
					sqlCommand.Parameters.AddWithValue("Spritzgiesserei_abgeschlossen" + i, item.SpritzgieBerei_abgeschlossen == null ? (object)DBNull.Value : item.SpritzgieBerei_abgeschlossen);
					sqlCommand.Parameters.AddWithValue("Tage_Abweichung" + i, item.Tage_Abweichung == null ? (object)DBNull.Value : item.Tage_Abweichung);
					sqlCommand.Parameters.AddWithValue("Technik" + i, item.Technik == null ? (object)DBNull.Value : item.Technik);
					sqlCommand.Parameters.AddWithValue("Techniker" + i, item.Techniker == null ? (object)DBNull.Value : item.Techniker);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2" + i, item.Termin_Bestatigt2 == null ? (object)DBNull.Value : item.Termin_Bestatigt2);
					sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung" + i, item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
					sqlCommand.Parameters.AddWithValue("Termin_Material" + i, item.Termin_Material == null ? (object)DBNull.Value : item.Termin_Material);
					sqlCommand.Parameters.AddWithValue("Termin_Ursprunglich" + i, item.Termin_Ursprunglich == null ? (object)DBNull.Value : item.Termin_Ursprunglich);
					sqlCommand.Parameters.AddWithValue("Termin_voranderung" + i, item.Termin_voranderung == null ? (object)DBNull.Value : item.Termin_voranderung);
					sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
					sqlCommand.Parameters.AddWithValue("UBGTransfer" + i, item.UBGTransfer == null ? (object)DBNull.Value : item.UBGTransfer);
					sqlCommand.Parameters.AddWithValue("Urs_Artikelnummer" + i, item.Urs_Artikelnummer == null ? (object)DBNull.Value : item.Urs_Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Urs_Fa" + i, item.Urs_Fa == null ? (object)DBNull.Value : item.Urs_Fa);
					sqlCommand.Parameters.AddWithValue("Zeit" + i, item.Zeit == null ? (object)DBNull.Value : item.Zeit);
					sqlCommand.Parameters.AddWithValue("PlanningDateViolation" + i, item.PlanningDateViolation == null ? (object)DBNull.Value : item.PlanningDateViolation);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		//public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity item, SqlConnection connection, SqlTransaction transaction)
		//{
		//    int results = -1;

		//    string query = "UPDATE [Fertigung] SET [Angebot_Artikel_Nr]=@Angebot_Artikel_Nr, [Angebot_nr]=@Angebot_nr, [Anzahl]=@Anzahl, [Anzahl_aktuell]=@Anzahl_aktuell, [Anzahl_erledigt]=@Anzahl_erledigt, [AnzahlnachgedrucktPPS]=@AnzahlnachgedrucktPPS, [Artikel_Nr]=@Artikel_Nr, [Ausgangskontrolle]=@Ausgangskontrolle, [Bemerkung]=@Bemerkung, [Bemerkung II Planung]=@Bemerkung_II_Planung, [Bemerkung ohne stätte]=@Bemerkung_ohne_statte, [Bemerkung_Kommissionierung_AL]=@Bemerkung_Kommissionierung_AL, [Bemerkung_Planung]=@Bemerkung_Planung, [Bemerkung_Technik]=@Bemerkung_Technik, [Bemerkung_zu_Prio]=@Bemerkung_zu_Prio, [BomVersion]=@BomVersion, [CAO]=@CAO, [Check_FAbegonnen]=@Check_FAbegonnen, [Check_Gewerk1]=@Check_Gewerk1, [Check_Gewerk1_Teilweise]=@Check_Gewerk1_Teilweise, [Check_Gewerk2]=@Check_Gewerk2, [Check_Gewerk2_Teilweise]=@Check_Gewerk2_Teilweise, [Check_Gewerk3]=@Check_Gewerk3, [Check_Gewerk3_Teilweise]=@Check_Gewerk3_Teilweise, [Check_Kabelgeschnitten]=@Check_Kabelgeschnitten, [CPVersion]=@CPVersion, [Datum]=@Datum, [Endkontrolle]=@Endkontrolle, [Erledigte_FA_Datum]=@Erledigte_FA_Datum, [Erstmuster]=@Erstmuster, [FA_begonnen]=@FA_begonnen, [FA_Druckdatum]=@FA_Druckdatum, [FA_Gestartet]=@FA_Gestartet, [Fa-NachdruckPPS]=@Fa_NachdruckPPS, [Fertigungsnummer]=@Fertigungsnummer, [gebucht]=@gebucht, [gedruckt]=@gedruckt, [Gewerk 1]=@Gewerk_1, [Gewerk 2]=@Gewerk_2, [Gewerk 3]=@Gewerk_3, [Gewerk_Teilweise_Bemerkung]=@Gewerk_Teilweise_Bemerkung, [GrundNachdruckPPS]=@GrundNachdruckPPS, [ID_Hauptartikel]=@ID_Hauptartikel, [ID_Rahmenfertigung]=@ID_Rahmenfertigung, [Kabel_geschnitten]=@Kabel_geschnitten, [Kabel_geschnitten_Datum]=@Kabel_geschnitten_Datum, [Kabel_Schneidebeginn]=@Kabel_Schneidebeginn, [Kabel_Schneidebeginn_Datum]=@Kabel_Schneidebeginn_Datum, [Kennzeichen]=@Kennzeichen, [Kommisioniert_komplett]=@Kommisioniert_komplett, [Kommisioniert_teilweise]=@Kommisioniert_teilweise, [Kunden_Index_Datum]=@Kunden_Index_Datum, [KundenIndex]=@KundenIndex, [Lagerort_id]=@Lagerort_id, [Lagerort_id zubuchen]=@Lagerort_id_zubuchen, [Letzte_Gebuchte_Menge]=@Letzte_Gebuchte_Menge, [Löschen]=@Loschen, [Mandant]=@Mandant, [Menge1]=@Menge1, [Menge2]=@Menge2, [Originalanzahl]=@Originalanzahl, [Planungsstatus]=@Planungsstatus, [Preis]=@Preis, [Prio]=@Prio, [Quick_Area]=@Quick_Area, [ROH_umgebucht]=@ROH_umgebucht, [Spritzgießerei_abgeschlossen]=@Spritzgiesserei_abgeschlossen, [Tage Abweichung]=@Tage_Abweichung, [Technik]=@Technik, [Techniker]=@Techniker, [Termin_Bestätigt1]=@Termin_Bestatigt1, [Termin_Bestätigt2]=@Termin_Bestatigt2, [Termin_Fertigstellung]=@Termin_Fertigstellung, [Termin_Material]=@Termin_Material, [Termin_Ursprünglich]=@Termin_Ursprunglich, [Termin_voränderung]=@Termin_voranderung, [UBG]=@UBG, [UBGTransfer]=@UBGTransfer, [Urs-Artikelnummer]=@Urs_Artikelnummer, [Urs-Fa]=@Urs_Fa, [Zeit]=@Zeit WHERE [ID]=@ID";
		//    var sqlCommand = new SqlCommand(query, connection, transaction);

		//    sqlCommand.Parameters.AddWithValue("ID", item.ID);
		//    sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr", item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
		//    sqlCommand.Parameters.AddWithValue("Angebot_nr", item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
		//    sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
		//    sqlCommand.Parameters.AddWithValue("Anzahl_aktuell", item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
		//    sqlCommand.Parameters.AddWithValue("Anzahl_erledigt", item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
		//    sqlCommand.Parameters.AddWithValue("AnzahlnachgedrucktPPS", item.AnzahlnachgedrucktPPS);
		//    sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
		//    sqlCommand.Parameters.AddWithValue("Ausgangskontrolle", item.Ausgangskontrolle == null ? (object)DBNull.Value : item.Ausgangskontrolle);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_II_Planung", item.Bemerkung_II_Planung == null ? (object)DBNull.Value : item.Bemerkung_II_Planung);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_ohne_statte", item.Bemerkung_ohne_statte == null ? (object)DBNull.Value : item.Bemerkung_ohne_statte);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL", item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_Planung", item.Bemerkung_Planung == null ? (object)DBNull.Value : item.Bemerkung_Planung);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_Technik", item.Bemerkung_Technik == null ? (object)DBNull.Value : item.Bemerkung_Technik);
		//    sqlCommand.Parameters.AddWithValue("Bemerkung_zu_Prio", item.Bemerkung_zu_Prio == null ? (object)DBNull.Value : item.Bemerkung_zu_Prio);
		//    sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
		//    sqlCommand.Parameters.AddWithValue("CAO", item.CAO == null ? (object)DBNull.Value : item.CAO);
		//    sqlCommand.Parameters.AddWithValue("Check_FAbegonnen", item.Check_FAbegonnen == null ? (object)DBNull.Value : item.Check_FAbegonnen);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk1", item.Check_Gewerk1 == null ? (object)DBNull.Value : item.Check_Gewerk1);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk1_Teilweise", item.Check_Gewerk1_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk1_Teilweise);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk2", item.Check_Gewerk2 == null ? (object)DBNull.Value : item.Check_Gewerk2);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk2_Teilweise", item.Check_Gewerk2_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk2_Teilweise);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk3", item.Check_Gewerk3 == null ? (object)DBNull.Value : item.Check_Gewerk3);
		//    sqlCommand.Parameters.AddWithValue("Check_Gewerk3_Teilweise", item.Check_Gewerk3_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk3_Teilweise);
		//    sqlCommand.Parameters.AddWithValue("Check_Kabelgeschnitten", item.Check_Kabelgeschnitten == null ? (object)DBNull.Value : item.Check_Kabelgeschnitten);
		//    sqlCommand.Parameters.AddWithValue("CPVersion", item.CPVersion == null ? (object)DBNull.Value : item.CPVersion);
		//    sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
		//    sqlCommand.Parameters.AddWithValue("Endkontrolle", item.Endkontrolle == null ? (object)DBNull.Value : item.Endkontrolle);
		//    sqlCommand.Parameters.AddWithValue("Erledigte_FA_Datum", item.Erledigte_FA_Datum == null ? (object)DBNull.Value : item.Erledigte_FA_Datum);
		//    sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
		//    sqlCommand.Parameters.AddWithValue("FA_begonnen", item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
		//    sqlCommand.Parameters.AddWithValue("FA_Druckdatum", item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
		//    sqlCommand.Parameters.AddWithValue("FA_Gestartet", item.FA_Gestartet == null ? (object)DBNull.Value : item.FA_Gestartet);
		//    sqlCommand.Parameters.AddWithValue("Fa_NachdruckPPS", item.Fa_NachdruckPPS == null ? (object)DBNull.Value : item.Fa_NachdruckPPS);
		//    sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
		//    sqlCommand.Parameters.AddWithValue("gebucht", item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
		//    sqlCommand.Parameters.AddWithValue("gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
		//    sqlCommand.Parameters.AddWithValue("Gewerk_1", item.Gewerk_1 == null ? (object)DBNull.Value : item.Gewerk_1);
		//    sqlCommand.Parameters.AddWithValue("Gewerk_2", item.Gewerk_2);
		//    sqlCommand.Parameters.AddWithValue("Gewerk_3", item.Gewerk_3);
		//    sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung", item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
		//    sqlCommand.Parameters.AddWithValue("GrundNachdruckPPS", item.GrundNachdruckPPS == null ? (object)DBNull.Value : item.GrundNachdruckPPS);
		//    sqlCommand.Parameters.AddWithValue("ID_Hauptartikel", item.ID_Hauptartikel == null ? (object)DBNull.Value : item.ID_Hauptartikel);
		//    sqlCommand.Parameters.AddWithValue("ID_Rahmenfertigung", item.ID_Rahmenfertigung == null ? (object)DBNull.Value : item.ID_Rahmenfertigung);
		//    sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
		//    sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum", item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
		//    sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn", item.Kabel_Schneidebeginn == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn);
		//    sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn_Datum", item.Kabel_Schneidebeginn_Datum == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn_Datum);
		//    sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
		//    sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett", item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
		//    sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise", item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
		//    sqlCommand.Parameters.AddWithValue("Kunden_Index_Datum", item.Kunden_Index_Datum == null ? (object)DBNull.Value : item.Kunden_Index_Datum);
		//    sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
		//    sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
		//    sqlCommand.Parameters.AddWithValue("Lagerort_id_zubuchen", item.Lagerort_id_zubuchen == null ? (object)DBNull.Value : item.Lagerort_id_zubuchen);
		//    sqlCommand.Parameters.AddWithValue("Letzte_Gebuchte_Menge", item.Letzte_Gebuchte_Menge == null ? (object)DBNull.Value : item.Letzte_Gebuchte_Menge);
		//    sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
		//    sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
		//    sqlCommand.Parameters.AddWithValue("Menge1", item.Menge1 == null ? (object)DBNull.Value : item.Menge1);
		//    sqlCommand.Parameters.AddWithValue("Menge2", item.Menge2 == null ? (object)DBNull.Value : item.Menge2);
		//    sqlCommand.Parameters.AddWithValue("Originalanzahl", item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
		//    sqlCommand.Parameters.AddWithValue("Planungsstatus", item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
		//    sqlCommand.Parameters.AddWithValue("Preis", item.Preis == null ? (object)DBNull.Value : item.Preis);
		//    sqlCommand.Parameters.AddWithValue("Prio", item.Prio == null ? (object)DBNull.Value : item.Prio);
		//    sqlCommand.Parameters.AddWithValue("Quick_Area", item.Quick_Area == null ? (object)DBNull.Value : item.Quick_Area);
		//    sqlCommand.Parameters.AddWithValue("ROH_umgebucht", item.ROH_umgebucht == null ? (object)DBNull.Value : item.ROH_umgebucht);
		//    sqlCommand.Parameters.AddWithValue("Spritzgiesserei_abgeschlossen", item.SpritzgieBerei_abgeschlossen == null ? (object)DBNull.Value : item.SpritzgieBerei_abgeschlossen);
		//    sqlCommand.Parameters.AddWithValue("Tage_Abweichung", item.Tage_Abweichung == null ? (object)DBNull.Value : item.Tage_Abweichung);
		//    sqlCommand.Parameters.AddWithValue("Technik", item.Technik == null ? (object)DBNull.Value : item.Technik);
		//    sqlCommand.Parameters.AddWithValue("Techniker", item.Techniker == null ? (object)DBNull.Value : item.Techniker);
		//    sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
		//    sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2", item.Termin_Bestatigt2 == null ? (object)DBNull.Value : item.Termin_Bestatigt2);
		//    sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung", item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
		//    sqlCommand.Parameters.AddWithValue("Termin_Material", item.Termin_Material == null ? (object)DBNull.Value : item.Termin_Material);
		//    sqlCommand.Parameters.AddWithValue("Termin_Ursprunglich", item.Termin_Ursprunglich == null ? (object)DBNull.Value : item.Termin_Ursprunglich);
		//    sqlCommand.Parameters.AddWithValue("Termin_voranderung", item.Termin_voranderung == null ? (object)DBNull.Value : item.Termin_voranderung);
		//    sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
		//    sqlCommand.Parameters.AddWithValue("UBGTransfer", item.UBGTransfer == null ? (object)DBNull.Value : item.UBGTransfer);
		//    sqlCommand.Parameters.AddWithValue("Urs_Artikelnummer", item.Urs_Artikelnummer == null ? (object)DBNull.Value : item.Urs_Artikelnummer);
		//    sqlCommand.Parameters.AddWithValue("Urs_Fa", item.Urs_Fa == null ? (object)DBNull.Value : item.Urs_Fa);
		//    sqlCommand.Parameters.AddWithValue("Zeit", item.Zeit == null ? (object)DBNull.Value : item.Zeit);

		//    results = DbExecution.ExecuteNonQuery(sqlCommand);
		//    return results;
		//}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 83; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Fertigung] SET "

					+ "[Angebot_Artikel_Nr]=@Angebot_Artikel_Nr" + i + ","
					+ "[Angebot_nr]=@Angebot_nr" + i + ","
					+ "[Anzahl]=@Anzahl" + i + ","
					+ "[Anzahl_aktuell]=@Anzahl_aktuell" + i + ","
					+ "[Anzahl_erledigt]=@Anzahl_erledigt" + i + ","
					+ "[AnzahlnachgedrucktPPS]=@AnzahlnachgedrucktPPS" + i + ","
					+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
					+ "[Ausgangskontrolle]=@Ausgangskontrolle" + i + ","
					+ "[Bemerkung]=@Bemerkung" + i + ","
					+ "[Bemerkung II Planung]=@Bemerkung_II_Planung" + i + ","
					+ "[Bemerkung ohne stätte]=@Bemerkung_ohne_statte" + i + ","
					+ "[Bemerkung_Kommissionierung_AL]=@Bemerkung_Kommissionierung_AL" + i + ","
					+ "[Bemerkung_Planung]=@Bemerkung_Planung" + i + ","
					+ "[Bemerkung_Technik]=@Bemerkung_Technik" + i + ","
					+ "[Bemerkung_zu_Prio]=@Bemerkung_zu_Prio" + i + ","
					+ "[BomVersion]=@BomVersion" + i + ","
					+ "[CAO]=@CAO" + i + ","
					+ "[Check_FAbegonnen]=@Check_FAbegonnen" + i + ","
					+ "[Check_Gewerk1]=@Check_Gewerk1" + i + ","
					+ "[Check_Gewerk1_Teilweise]=@Check_Gewerk1_Teilweise" + i + ","
					+ "[Check_Gewerk2]=@Check_Gewerk2" + i + ","
					+ "[Check_Gewerk2_Teilweise]=@Check_Gewerk2_Teilweise" + i + ","
					+ "[Check_Gewerk3]=@Check_Gewerk3" + i + ","
					+ "[Check_Gewerk3_Teilweise]=@Check_Gewerk3_Teilweise" + i + ","
					+ "[Check_Kabelgeschnitten]=@Check_Kabelgeschnitten" + i + ","
					+ "[CPVersion]=@CPVersion" + i + ","
					+ "[Datum]=@Datum" + i + ","
					+ "[Endkontrolle]=@Endkontrolle" + i + ","
					+ "[Erledigte_FA_Datum]=@Erledigte_FA_Datum" + i + ","
					+ "[Erstmuster]=@Erstmuster" + i + ","
					+ "[FA_begonnen]=@FA_begonnen" + i + ","
					+ "[FA_Druckdatum]=@FA_Druckdatum" + i + ","
					+ "[FA_Gestartet]=@FA_Gestartet" + i + ","
					+ "[Fa-NachdruckPPS]=@Fa_NachdruckPPS" + i + ","
					+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
					+ "[gebucht]=@gebucht" + i + ","
					+ "[gedruckt]=@gedruckt" + i + ","
					+ "[Gewerk 1]=@Gewerk_1" + i + ","
					+ "[Gewerk 2]=@Gewerk_2" + i + ","
					+ "[Gewerk 3]=@Gewerk_3" + i + ","
					+ "[Gewerk_Teilweise_Bemerkung]=@Gewerk_Teilweise_Bemerkung" + i + ","
					+ "[GrundNachdruckPPS]=@GrundNachdruckPPS" + i + ","
					+ "[ID_Hauptartikel]=@ID_Hauptartikel" + i + ","
					+ "[ID_Rahmenfertigung]=@ID_Rahmenfertigung" + i + ","
					+ "[Kabel_geschnitten]=@Kabel_geschnitten" + i + ","
					+ "[Kabel_geschnitten_Datum]=@Kabel_geschnitten_Datum" + i + ","
					+ "[Kabel_Schneidebeginn]=@Kabel_Schneidebeginn" + i + ","
					+ "[Kabel_Schneidebeginn_Datum]=@Kabel_Schneidebeginn_Datum" + i + ","
					+ "[Kennzeichen]=@Kennzeichen" + i + ","
					+ "[Kommisioniert_komplett]=@Kommisioniert_komplett" + i + ","
					+ "[Kommisioniert_teilweise]=@Kommisioniert_teilweise" + i + ","
					+ "[Kunden_Index_Datum]=@Kunden_Index_Datum" + i + ","
					+ "[KundenIndex]=@KundenIndex" + i + ","
					+ "[Lagerort_id]=@Lagerort_id" + i + ","
					+ "[Lagerort_id zubuchen]=@Lagerort_id_zubuchen" + i + ","
					+ "[Letzte_Gebuchte_Menge]=@Letzte_Gebuchte_Menge" + i + ","
					+ "[Löschen]=@Loschen" + i + ","
					+ "[Mandant]=@Mandant" + i + ","
					+ "[Menge1]=@Menge1" + i + ","
					+ "[Menge2]=@Menge2" + i + ","
					+ "[Originalanzahl]=@Originalanzahl" + i + ","
					+ "[Planungsstatus]=@Planungsstatus" + i + ","
					+ "[Preis]=@Preis" + i + ","
					+ "[Prio]=@Prio" + i + ","
					+ "[Quick_Area]=@Quick_Area" + i + ","
					+ "[ROH_umgebucht]=@ROH_umgebucht" + i + ","
					+ "[Spritzgießerei_abgeschlossen]=@Spritzgiesserei_abgeschlossen" + i + ","
					+ "[Tage Abweichung]=@Tage_Abweichung" + i + ","
					+ "[Technik]=@Technik" + i + ","
					+ "[Techniker]=@Techniker" + i + ","
					+ "[Termin_Bestätigt1]=@Termin_Bestatigt1" + i + ","
					+ "[Termin_Bestätigt2]=@Termin_Bestatigt2" + i + ","
					+ "[Termin_Fertigstellung]=@Termin_Fertigstellung" + i + ","
					+ "[Termin_Material]=@Termin_Material" + i + ","
					+ "[Termin_Ursprünglich]=@Termin_Ursprunglich" + i + ","
					+ "[Termin_voränderung]=@Termin_voranderung" + i + ","
					+ "[UBG]=@UBG" + i + ","
					+ "[UBGTransfer]=@UBGTransfer" + i + ","
					+ "[Urs-Artikelnummer]=@Urs_Artikelnummer" + i + ","
					+ "[Urs-Fa]=@Urs_Fa" + i + ","
					+ "[Termin_Bestatigt2_Updated]=@Termin_Bestatigt2_Updated" + i + ","
					+ "[PlanningDateViolation]=@PlanningDateViolation" + i + ","
					+ "[Zeit]=@Zeit" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr" + i, item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Angebot_nr" + i, item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Anzahl_aktuell" + i, item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
					sqlCommand.Parameters.AddWithValue("Anzahl_erledigt" + i, item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
					sqlCommand.Parameters.AddWithValue("AnzahlnachgedrucktPPS" + i, item.AnzahlnachgedrucktPPS);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Ausgangskontrolle" + i, item.Ausgangskontrolle == null ? (object)DBNull.Value : item.Ausgangskontrolle);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_II_Planung" + i, item.Bemerkung_II_Planung == null ? (object)DBNull.Value : item.Bemerkung_II_Planung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_ohne_statte" + i, item.Bemerkung_ohne_statte == null ? (object)DBNull.Value : item.Bemerkung_ohne_statte);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Kommissionierung_AL" + i, item.Bemerkung_Kommissionierung_AL == null ? (object)DBNull.Value : item.Bemerkung_Kommissionierung_AL);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Planung" + i, item.Bemerkung_Planung == null ? (object)DBNull.Value : item.Bemerkung_Planung);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Technik" + i, item.Bemerkung_Technik == null ? (object)DBNull.Value : item.Bemerkung_Technik);
					sqlCommand.Parameters.AddWithValue("Bemerkung_zu_Prio" + i, item.Bemerkung_zu_Prio == null ? (object)DBNull.Value : item.Bemerkung_zu_Prio);
					sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("CAO" + i, item.CAO == null ? (object)DBNull.Value : item.CAO);
					sqlCommand.Parameters.AddWithValue("Check_FAbegonnen" + i, item.Check_FAbegonnen == null ? (object)DBNull.Value : item.Check_FAbegonnen);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk1" + i, item.Check_Gewerk1 == null ? (object)DBNull.Value : item.Check_Gewerk1);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk1_Teilweise" + i, item.Check_Gewerk1_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk1_Teilweise);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk2" + i, item.Check_Gewerk2 == null ? (object)DBNull.Value : item.Check_Gewerk2);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk2_Teilweise" + i, item.Check_Gewerk2_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk2_Teilweise);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk3" + i, item.Check_Gewerk3 == null ? (object)DBNull.Value : item.Check_Gewerk3);
					sqlCommand.Parameters.AddWithValue("Check_Gewerk3_Teilweise" + i, item.Check_Gewerk3_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk3_Teilweise);
					sqlCommand.Parameters.AddWithValue("Check_Kabelgeschnitten" + i, item.Check_Kabelgeschnitten == null ? (object)DBNull.Value : item.Check_Kabelgeschnitten);
					sqlCommand.Parameters.AddWithValue("CPVersion" + i, item.CPVersion == null ? (object)DBNull.Value : item.CPVersion);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Endkontrolle" + i, item.Endkontrolle == null ? (object)DBNull.Value : item.Endkontrolle);
					sqlCommand.Parameters.AddWithValue("Erledigte_FA_Datum" + i, item.Erledigte_FA_Datum == null ? (object)DBNull.Value : item.Erledigte_FA_Datum);
					sqlCommand.Parameters.AddWithValue("Erstmuster" + i, item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
					sqlCommand.Parameters.AddWithValue("FA_begonnen" + i, item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
					sqlCommand.Parameters.AddWithValue("FA_Druckdatum" + i, item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
					sqlCommand.Parameters.AddWithValue("FA_Gestartet" + i, item.FA_Gestartet == null ? (object)DBNull.Value : item.FA_Gestartet);
					sqlCommand.Parameters.AddWithValue("Fa_NachdruckPPS" + i, item.Fa_NachdruckPPS == null ? (object)DBNull.Value : item.Fa_NachdruckPPS);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
					sqlCommand.Parameters.AddWithValue("gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
					sqlCommand.Parameters.AddWithValue("Gewerk_1" + i, item.Gewerk_1 == null ? (object)DBNull.Value : item.Gewerk_1);
					sqlCommand.Parameters.AddWithValue("Gewerk_2" + i, item.Gewerk_2);
					sqlCommand.Parameters.AddWithValue("Gewerk_3" + i, item.Gewerk_3);
					sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung" + i, item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
					sqlCommand.Parameters.AddWithValue("GrundNachdruckPPS" + i, item.GrundNachdruckPPS == null ? (object)DBNull.Value : item.GrundNachdruckPPS);
					sqlCommand.Parameters.AddWithValue("ID_Hauptartikel" + i, item.ID_Hauptartikel == null ? (object)DBNull.Value : item.ID_Hauptartikel);
					sqlCommand.Parameters.AddWithValue("ID_Rahmenfertigung" + i, item.ID_Rahmenfertigung == null ? (object)DBNull.Value : item.ID_Rahmenfertigung);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten" + i, item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum" + i, item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
					sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn" + i, item.Kabel_Schneidebeginn == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn);
					sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn_Datum" + i, item.Kabel_Schneidebeginn_Datum == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn_Datum);
					sqlCommand.Parameters.AddWithValue("Kennzeichen" + i, item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett" + i, item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
					sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise" + i, item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
					sqlCommand.Parameters.AddWithValue("Kunden_Index_Datum" + i, item.Kunden_Index_Datum == null ? (object)DBNull.Value : item.Kunden_Index_Datum);
					sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Lagerort_id_zubuchen" + i, item.Lagerort_id_zubuchen == null ? (object)DBNull.Value : item.Lagerort_id_zubuchen);
					sqlCommand.Parameters.AddWithValue("Letzte_Gebuchte_Menge" + i, item.Letzte_Gebuchte_Menge == null ? (object)DBNull.Value : item.Letzte_Gebuchte_Menge);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Menge1" + i, item.Menge1 == null ? (object)DBNull.Value : item.Menge1);
					sqlCommand.Parameters.AddWithValue("Menge2" + i, item.Menge2 == null ? (object)DBNull.Value : item.Menge2);
					sqlCommand.Parameters.AddWithValue("Originalanzahl" + i, item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
					sqlCommand.Parameters.AddWithValue("Planungsstatus" + i, item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
					sqlCommand.Parameters.AddWithValue("Preis" + i, item.Preis == null ? (object)DBNull.Value : item.Preis);
					sqlCommand.Parameters.AddWithValue("Prio" + i, item.Prio == null ? (object)DBNull.Value : item.Prio);
					sqlCommand.Parameters.AddWithValue("Quick_Area" + i, item.Quick_Area == null ? (object)DBNull.Value : item.Quick_Area);
					sqlCommand.Parameters.AddWithValue("ROH_umgebucht" + i, item.ROH_umgebucht == null ? (object)DBNull.Value : item.ROH_umgebucht);
					sqlCommand.Parameters.AddWithValue("Spritzgiesserei_abgeschlossen" + i, item.SpritzgieBerei_abgeschlossen == null ? (object)DBNull.Value : item.SpritzgieBerei_abgeschlossen);
					sqlCommand.Parameters.AddWithValue("Tage_Abweichung" + i, item.Tage_Abweichung == null ? (object)DBNull.Value : item.Tage_Abweichung);
					sqlCommand.Parameters.AddWithValue("Technik" + i, item.Technik == null ? (object)DBNull.Value : item.Technik);
					sqlCommand.Parameters.AddWithValue("Techniker" + i, item.Techniker == null ? (object)DBNull.Value : item.Techniker);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2" + i, item.Termin_Bestatigt2 == null ? (object)DBNull.Value : item.Termin_Bestatigt2);
					sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung" + i, item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
					sqlCommand.Parameters.AddWithValue("Termin_Material" + i, item.Termin_Material == null ? (object)DBNull.Value : item.Termin_Material);
					sqlCommand.Parameters.AddWithValue("Termin_Ursprunglich" + i, item.Termin_Ursprunglich == null ? (object)DBNull.Value : item.Termin_Ursprunglich);
					sqlCommand.Parameters.AddWithValue("Termin_voranderung" + i, item.Termin_voranderung == null ? (object)DBNull.Value : item.Termin_voranderung);
					sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
					sqlCommand.Parameters.AddWithValue("UBGTransfer" + i, item.UBGTransfer == null ? (object)DBNull.Value : item.UBGTransfer);
					sqlCommand.Parameters.AddWithValue("Urs_Artikelnummer" + i, item.Urs_Artikelnummer == null ? (object)DBNull.Value : item.Urs_Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Urs_Fa" + i, item.Urs_Fa == null ? (object)DBNull.Value : item.Urs_Fa);
					sqlCommand.Parameters.AddWithValue("Zeit" + i, item.Zeit == null ? (object)DBNull.Value : item.Zeit);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2_Updated" + i, item.Termin_Bestatigt2_Updated == null ? (object)DBNull.Value : item.Termin_Bestatigt2_Updated);
					sqlCommand.Parameters.AddWithValue("PlanningDateViolation" + i, item.PlanningDateViolation == null ? (object)DBNull.Value : item.PlanningDateViolation);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}
		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Fertigung] WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ID", id);

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

				string query = "DELETE FROM [Fertigung] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetOpenByArticleInLager(int id, int? lagerId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Fertigung] WHERE [Artikel_Nr]=@id AND Kennzeichen='offen' /*AND IsNULL(FA_Gestartet,0)=0 -- 202212-20 - Schremmer */ AND [Anzahl]>0 {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetByFertigungsnummerWithTransaction(List<int> fertigungsnummers, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(fertigungsnummers == null || fertigungsnummers.Count <= 0)
				return null;

			var dataTable = new DataTable();

			// using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				// sqlConnection.Open();

				string query = $"SELECT * FROM [Fertigung] WHERE [Fertigungsnummer] IN ({string.Join(",", fertigungsnummers)})";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static int GetMaxFertigungsnummer(string mandant)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT MAX(Fertigungsnummer) FROM [Fertigung] WHERE Mandant=@mandant AND Endkontrolle IS NULL";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("mandant", mandant);

				if(int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var response) == true)
				{
					return response;
				}

				return 0;
			}
		}
		public static int GetMaxFertigungsnummer(string mandant, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "SELECT MAX(Fertigungsnummer) FROM [Fertigung] WHERE Mandant=@mandant AND Endkontrolle IS NULL";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("mandant", mandant);

			if(int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var response) == true)
			{
				return response;
			}

			return 0;
		}
		public static List<Tuple<string, int?>> Get_Gewerk_Fertigungsnummer_Query11(int fertigungsnummer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT T.Gewerk AS TGewerk, T.Fertigungsnummer AS TFertigungsnummer  
									FROM (SELECT Artikel_1.Artikelnummer AS Artikelnummer1, Artikel_1.[Bezeichnung 1] AS B1, 
											Artikel_1.[Bezeichnung 2] AS B2, 
											Fertigung.Anzahl AS Anzahl1, 
											Lagerorte.Lagerort AS L1, 
											Fertigung.Fertigungsnummer, 
											Fertigung.Datum, 
											Fertigung.Termin_Fertigstellung, 
											Fertigung.Kennzeichen, 
											Fertigung.Bemerkung, 
											Artikel.Artikelnummer, 
											Artikel.[Bezeichnung 1], 
											Artikel.[Bezeichnung 2], 
											Fertigung_Positionen.Anzahl, 
											Fertigung_Positionen.Arbeitsanweisung, 
											Fertigung_Positionen.Fertiger, 
											Fertigung_Positionen.Termin_Soll, 
											Fertigung_Positionen.Bemerkungen, 
											Lagerorte_1.Lagerort, 
											Artikel_1.EAN, 
											artikel_kalkulatorische_kosten.Betrag, 
											Artikel_1.Freigabestatus, 
											Fertigung.Zeit AS Produktionszeit, 
											Fertigung.Termin_Bestätigt1, 
											Fertigung.Erstmuster, 
											Artikel_1.[Freigabestatus TN intern] , 
											Artikel_1.Index_Kunde, 
											Fertigung.[Lagerort_ID zubuchen], 
											Fertigung.Mandant, Artikel_1.Sysmonummer AS S1, 
											Artikel.Sysmonummer, 
											Artikel_1.[UL Etikett], 
											Fertigung.Technik, 
											Fertigung.Techniker,
											Artikel_1.Kanban, 
											Artikel_1.Verpackungsart, 
											Artikel_1.Verpackungsmenge, 
											Artikel_1.Losgroesse, 
											Fertigung.Quick_Area, 
											Artikel_1.Artikelfamilie_Kunde, 
											Artikel_1.Artikelfamilie_Kunde_Detail1, 
											Artikel_1.Artikelfamilie_Kunde_Detail2, 
											Artikel.Klassifizierung, 
											Artikelstamm_Klassifizierung.Bezeichnung, 
											Artikelstamm_Klassifizierung.Nummernkreis, 
											Artikelstamm_Klassifizierung.Kupferzahl, 
											Artikelstamm_Klassifizierung.ID, 
											Artikelstamm_Klassifizierung.Gewerk 
											FROM 
											(( (Lagerorte AS Lagerorte_1 INNER JOIN 
												((Artikel AS Artikel_1 INNER JOIN 
												(Artikel INNER JOIN 
												(Fertigung INNER JOIN Fertigung_Positionen 
													ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung) 
													ON Artikel.[Artikel-Nr] = Fertigung_Positionen.Artikel_Nr) 
													ON Artikel_1.[Artikel-Nr] = Fertigung.Artikel_Nr) INNER JOIN Lagerorte 
													ON Fertigung.Lagerort_ID = Lagerorte.Lagerort_ID) 
													ON Lagerorte_1.Lagerort_ID = Fertigung_Positionen.Lagerort_ID) 
												LEFT JOIN artikel_kalkulatorische_kosten 
													ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[Artikel-Nr]) 
												INNER JOIN Artikelstamm_Klassifizierung 
													ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID) 
											WHERE Artikelstamm_Klassifizierung.Gewerk IS NOT NULL AND Fertigung.Fertigungsnummer=@fertigungsnummer) AS T
								GROUP BY T.Klassifizierung, T.Gewerk, T.Artikelnummer1, T.Anzahl1, T.Termin_Bestätigt1, T.Fertigungsnummer, T.Bezeichnung, T.B1, T.Artikelfamilie_Kunde 
								ORDER BY T.Gewerk;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fertigungsnummer", fertigungsnummer);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			var response = new List<Tuple<string, int?>>();
			foreach(DataRow dataRow in dataTable.Rows)
			{
				var tGewerk = (dataRow["TGewerk"] == System.DBNull.Value) ? null : dataRow["TGewerk"].ToString();
				var tFertigungsnummer = (dataRow["TFertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TFertigungsnummer"]);

				response.Add(new Tuple<string, int?>(tGewerk, tFertigungsnummer));
			}
			return response;
		}
		public static List<Tuple<string, int?>> Get_Gewerk_Fertigungsnummer_Query11(int fertigungsnummer, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = @"SELECT T.Gewerk AS TGewerk, T.Fertigungsnummer AS TFertigungsnummer  
									FROM (SELECT Artikel_1.Artikelnummer AS Artikelnummer1, Artikel_1.[Bezeichnung 1] AS B1, 
											Artikel_1.[Bezeichnung 2] AS B2, 
											Fertigung.Anzahl AS Anzahl1, 
											Lagerorte.Lagerort AS L1, 
											Fertigung.Fertigungsnummer, 
											Fertigung.Datum, 
											Fertigung.Termin_Fertigstellung, 
											Fertigung.Kennzeichen, 
											Fertigung.Bemerkung, 
											Artikel.Artikelnummer, 
											Artikel.[Bezeichnung 1], 
											Artikel.[Bezeichnung 2], 
											Fertigung_Positionen.Anzahl, 
											Fertigung_Positionen.Arbeitsanweisung, 
											Fertigung_Positionen.Fertiger, 
											Fertigung_Positionen.Termin_Soll, 
											Fertigung_Positionen.Bemerkungen, 
											Lagerorte_1.Lagerort, 
											Artikel_1.EAN, 
											artikel_kalkulatorische_kosten.Betrag, 
											Artikel_1.Freigabestatus, 
											Fertigung.Zeit AS Produktionszeit, 
											Fertigung.Termin_Bestätigt1, 
											Fertigung.Erstmuster, 
											Artikel_1.[Freigabestatus TN intern] , 
											Artikel_1.Index_Kunde, 
											Fertigung.[Lagerort_ID zubuchen], 
											Fertigung.Mandant, Artikel_1.Sysmonummer AS S1, 
											Artikel.Sysmonummer, 
											Artikel_1.[UL Etikett], 
											Fertigung.Technik, 
											Fertigung.Techniker,
											Artikel_1.Kanban, 
											Artikel_1.Verpackungsart, 
											Artikel_1.Verpackungsmenge, 
											Artikel_1.Losgroesse, 
											Fertigung.Quick_Area, 
											Artikel_1.Artikelfamilie_Kunde, 
											Artikel_1.Artikelfamilie_Kunde_Detail1, 
											Artikel_1.Artikelfamilie_Kunde_Detail2, 
											Artikel.Klassifizierung, 
											Artikelstamm_Klassifizierung.Bezeichnung, 
											Artikelstamm_Klassifizierung.Nummernkreis, 
											Artikelstamm_Klassifizierung.Kupferzahl, 
											Artikelstamm_Klassifizierung.ID, 
											Artikelstamm_Klassifizierung.Gewerk 
											FROM 
											(( (Lagerorte AS Lagerorte_1 INNER JOIN 
												((Artikel AS Artikel_1 INNER JOIN 
												(Artikel INNER JOIN 
												(Fertigung INNER JOIN Fertigung_Positionen 
													ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung) 
													ON Artikel.[Artikel-Nr] = Fertigung_Positionen.Artikel_Nr) 
													ON Artikel_1.[Artikel-Nr] = Fertigung.Artikel_Nr) INNER JOIN Lagerorte 
													ON Fertigung.Lagerort_ID = Lagerorte.Lagerort_ID) 
													ON Lagerorte_1.Lagerort_ID = Fertigung_Positionen.Lagerort_ID) 
												LEFT JOIN artikel_kalkulatorische_kosten 
													ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[Artikel-Nr]) 
												INNER JOIN Artikelstamm_Klassifizierung 
													ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID) 
											WHERE Artikelstamm_Klassifizierung.Gewerk IS NOT NULL AND Fertigung.Fertigungsnummer=@fertigungsnummer) AS T
								GROUP BY T.Klassifizierung, T.Gewerk, T.Artikelnummer1, T.Anzahl1, T.Termin_Bestätigt1, T.Fertigungsnummer, T.Bezeichnung, T.B1, T.Artikelfamilie_Kunde 
								ORDER BY T.Gewerk;";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("fertigungsnummer", fertigungsnummer);

			DbExecution.Fill(sqlCommand, dataTable);

			var response = new List<Tuple<string, int?>>();
			foreach(DataRow dataRow in dataTable.Rows)
			{
				var tGewerk = (dataRow["TGewerk"] == System.DBNull.Value) ? null : dataRow["TGewerk"].ToString();
				var tFertigungsnummer = (dataRow["TFertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TFertigungsnummer"]);

				response.Add(new Tuple<string, int?>(tGewerk, tFertigungsnummer));
			}
			return response;
		}
		public static int UpdateGewerk(int fertigungsnummer,
			string gewerk1,
			string gewerk2,
			string gewerk3)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " UPDATE Fertigung SET "
					+ " [Gewerk 1]=@gewerk1, [Gewerk 2]=@gewerk2, [Gewerk 3]=@gewerk3 "
					+ " WHERE [Fertigungsnummer]=@fertigungsnummer ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fertigungsnummer", fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("gewerk1", gewerk1);
				sqlCommand.Parameters.AddWithValue("gewerk2", gewerk2);
				sqlCommand.Parameters.AddWithValue("gewerk3", gewerk3);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int UpdateGewerk(int fertigungsnummer,
			string gewerk1,
			string gewerk2,
			string gewerk3, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = " UPDATE Fertigung SET "
				+ " [Gewerk 1]=@gewerk1, [Gewerk 2]=@gewerk2, [Gewerk 3]=@gewerk3 "
				+ " WHERE [Fertigungsnummer]=@fertigungsnummer ";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("fertigungsnummer", fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("gewerk1", gewerk1);
			sqlCommand.Parameters.AddWithValue("gewerk2", gewerk2);
			sqlCommand.Parameters.AddWithValue("gewerk3", gewerk3);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static Infrastructure.Data.Entities.Tables.PRS.FertigungEntity GetByFertigungsnummer(int fertigungsnummer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Fertigung] WHERE [Fertigungsnummer]=@fertigungsnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fertigungsnummer", fertigungsnummer);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetByFertigungsnummer(List<int> fertigungsnummers)
		{
			if(fertigungsnummers == null || fertigungsnummers.Count <= 0)
				return null;

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Fertigung] WHERE [Fertigungsnummer] IN ({string.Join(",", fertigungsnummers)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<FertigungEntity> GetByFertigungsnummers(List<int> fertigungsnummers)
		{
			if(fertigungsnummers == null || fertigungsnummers.Count == 0)
				return new List<FertigungEntity>();

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				// Build query with parameters
				var parameters = new List<string>();
				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;

				for(int i = 0; i < fertigungsnummers.Count; i++)
				{
					var paramName = $"@id{i}";
					parameters.Add(paramName);
					sqlCommand.Parameters.AddWithValue(paramName, fertigungsnummers[i]);
				}

				sqlCommand.CommandText = $"SELECT * FROM [Fertigung] WHERE [Fertigungsnummer] IN ({string.Join(",", parameters)})";

				DbExecution.Fill(sqlCommand, dataTable);
			}

			return dataTable.Rows.Count > 0
				? toList(dataTable)
				: new List<FertigungEntity>();
		}
		public static Infrastructure.Data.Entities.Tables.PRS.FertigungEntity GetByFertigungsnummer(int fertigungsnummer, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Fertigung] WHERE [Fertigungsnummer]=@fertigungsnummer";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("fertigungsnummer", fertigungsnummer);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetByFertigungsnummer(List<int> fertigungsnummers, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(fertigungsnummers == null || fertigungsnummers.Count <= 0)
				return null;

			var dataTable = new DataTable();

			string query = $"SELECT * FROM [Fertigung] WHERE [Fertigungsnummer] IN ({string.Join(",", fertigungsnummers)})";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static int UpdateAfterCancel(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "UPDATE [Fertigung] SET [Bemerkung]=@Bemerkung, [Kennzeichen]=@Kennzeichen, [Angebot_Artikel_Nr]=@Angebot_Artikel_Nr, [Angebot_nr]=@Angebot_nr, [Anzahl]=@Anzahl "
				+ " WHERE [ID]=@ID";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
			sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr", item.Angebot_Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Angebot_nr", item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}

		// -- Werk and wunsh update from excel
		public static int UpdateTerminWerk(int fertigungsnummer, DateTime terminWerk, string bemerkumg2, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int results = -1;
			//using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				//sqlConnection.Open();
				string query = @"UPDATE [Fertigung] SET [Termin_Bestätigt2] = @terminWerk, 
                               [Bemerkung II Planung] =CONCAT([Bemerkung II Planung],'/',GETDATE(),':',@bemerkumg2)
                               WHERE [Fertigungsnummer]=@fertigungsnummer /*and [FA_Druckdatum] Is Not Null -- 2022-05-19 - K*/";


				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

				sqlCommand.Parameters.AddWithValue("terminWerk", terminWerk);
				sqlCommand.Parameters.AddWithValue("bemerkumg2", bemerkumg2);
				sqlCommand.Parameters.AddWithValue("fertigungsnummer", fertigungsnummer);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateTerminWerk(List<Tuple<int, DateTime, string>> items, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(items == null || items.Count <= 0)
			{
				return 0;
			}
			string query = "";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			for(int i = 0; i < items.Count; i++)
			{
				query += $@"UPDATE [Fertigung] SET [Termin_Bestätigt2] = @terminWerk{i}, 
                               [Bemerkung II Planung] =CONCAT([Bemerkung II Planung],'/',GETDATE(),':',@bemerkumg2{i}), Termin_Bestatigt2_Updated=1
                               WHERE [Fertigungsnummer]=@fertigungsnummer{i} /*and [FA_Druckdatum] Is Not Null -- 2022-05-19 - K*/; ";

				sqlCommand.Parameters.AddWithValue($"terminWerk{i}", items[i].Item2);
				sqlCommand.Parameters.AddWithValue($"bemerkumg2{i}", items[i].Item3);
				sqlCommand.Parameters.AddWithValue($"fertigungsnummer{i}", items[i].Item1);
			}

			sqlCommand.CommandText = query;
			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int UpdateBemerkung(List<Tuple<int, DateTime, string>> items, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(items == null || items.Count <= 0)
			{
				return 0;
			}
			string query = "";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			for(int i = 0; i < items.Count; i++)
			{
				query += $@"UPDATE [Fertigung] SET [Bemerkung II Planung] =CONCAT([Bemerkung II Planung],'/',GETDATE(),':',@bemerkumg2{i})
                            WHERE [Fertigungsnummer]=@fertigungsnummer{i};";

				sqlCommand.Parameters.AddWithValue($"bemerkumg2{i}", items[i].Item3);
				sqlCommand.Parameters.AddWithValue($"fertigungsnummer{i}", items[i].Item1);
			}

			sqlCommand.CommandText = query;
			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int UpdateTerminWunshAdmin(string joint, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int results = -1;
			//using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				//sqlConnection.Open();
				string query = $@"UPDATE Fertigung
                               SET Fertigung.Termin_Bestätigt1 = tblExcelImport.Termin
                               FROM Fertigung INNER JOIN
                               ({joint}) tblExcelImport
                               ON Fertigung.Fertigungsnummer = tblExcelImport.Fertigungsnummer 
                               WHERE (((Fertigung.Termin_Bestätigt1) Is Not Null))";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateTerminWunshUser(string joint)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"UPDATE Fertigung
                               SET Fertigung.Termin_Bestätigt1 = tblExcelImport.Termin
from Fertigung
INNER JOIN
({joint}) 
tblExcelImport
ON Fertigung.Fertigungsnummer = tblExcelImport.Fertigungsnummer
left join Artikel on Fertigung.Artikel_Nr=Artikel.[Artikel-Nr]
WHERE (((Fertigung.Lagerort_id)<>42 And (Fertigung.Lagerort_id)<>60 And (Fertigung.Lagerort_id)<>102 AND 
(Fertigung.Lagerort_id)<>7 And (Fertigung.Lagerort_id)<>6 AND
(Fertigung.Lagerort_id)<>21 and (Fertigung.Lagerort_id)<>26) AND ((Fertigung.Kennzeichen)='Offen') AND
((IIf(ISNULL([Kabel_geschnitten],0)=0,0,1))=0) AND ((IIf(ISNULL([Check_FAbegonnen],0)=0,0,1))=0) AND
((IIf(ISNULL([Check_Gewerk2_Teilweise],0)=0,0,1))=0) AND ((IIf(ISNULL([Check_Gewerk3_Teilweise],0)=0,0,1))=0) AND
((IIf(ISNULL([Check_Gewerk1_Teilweise],0)=0,0,1))=0) AND ((IIf(ISNULL([Check_Gewerk1],0)=0,0,1))=0) AND
((IIf(ISNULL([Check_Gewerk2],0)=0,0,1))=0) AND ((IIf(ISNULL([Check_Gewerk3],0)=0,0,1))=0) AND
((Fertigung.Gedruckt)=0) AND ((Fertigung.FA_Druckdatum) Is Null) AND
((tblExcelImport.Termin)>=GETDATE()+21) and Artikel.Artikelnummer is not null) OR 

(((Fertigung.Lagerort_id)=42 Or
(Fertigung.Lagerort_id)=60 Or (Fertigung.Lagerort_id)=102 Or (Fertigung.Lagerort_id)=7 Or (Fertigung.Lagerort_id)=6 Or
(Fertigung.Lagerort_id)=21 Or (Fertigung.Lagerort_id)=26) AND ((Fertigung.Kennzeichen)='Offen') AND
((IIf(ISNULL([Kabel_geschnitten],0)=0,0,1))=0) AND ((IIf(ISNULL([Check_FAbegonnen],0)=0,0,1))=0) AND
((IIf(ISNULL([Check_Gewerk2_Teilweise],0)=0,0,1))=0) AND ((IIf(ISNULL([Check_Gewerk3_Teilweise],0)=0,0,1))=0) AND
((IIf(ISNULL([Check_Gewerk1_Teilweise],0)=0,0,1))=0) AND ((IIf(ISNULL([Check_Gewerk1],0)=0,0,1))=0) AND
((IIf(ISNULL([Check_Gewerk2],0)=0,0,1))=0) AND ((IIf(ISNULL([Check_Gewerk3],0)=0,0,1))=0) AND
((Fertigung.Gedruckt)=0) AND ((Fertigung.FA_Druckdatum) Is Null) AND
((tblExcelImport.Termin)>=GETDATE()+21) AND ((Fertigung.FA_Gestartet)=0 Or
(Fertigung.FA_Gestartet) Is Null) and Artikel.Artikelnummer is not null)";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		//
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetByAngeboteNr(int angebotNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigung] WHERE [Angebot_nr]=@angebotNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}

		// -- Get all FA of the Article with BOM Version smaller than current
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetBOMValidationUpgradable(int articleId, int? bomVersion, bool cpRequired, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			bomVersion = bomVersion ?? 0;
			var dataTable = new DataTable();
			{
				string query = $"SELECT * FROM [Fertigung] WHERE Artikel_Nr=@articleId AND ([BomVersion] IS NULL OR [BomVersion]<@bomVersion) {(cpRequired ? " AND CPVersion IS NOT NULL" : "")} AND Kennzeichen='Offen' AND (FA_Gestartet IS NULL OR FA_Gestartet=0)";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("bomVersion", bomVersion);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetBOMValidationUpgradable(List<int> articleIds, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(articleIds == null || articleIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			{
				string query = $"SELECT * FROM [Fertigung] WHERE Artikel_Nr IN ({string.Join(",", articleIds)}) AND Kennzeichen='Offen' AND (FA_Gestartet IS NULL OR FA_Gestartet=0)";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetNonStartedByArticle(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigung] WHERE Artikel_Nr=@articleId AND Kennzeichen='Offen' AND (FA_Gestartet IS NULL OR FA_Gestartet=0)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetNonStartedByArticle(int articleId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			{
				string query = "SELECT * FROM [Fertigung] WHERE Artikel_Nr=@articleId AND Kennzeichen='Offen' AND (FA_Gestartet IS NULL OR FA_Gestartet=0)";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static bool ConfirmBOMValidationUpgradable(int bomVersion, List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
				return true;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(*) FROM [Fertigung] WHERE ID IN ({string.Join(",", ids)}) AND ([BomVersion] IS NULL OR [BomVersion]<={bomVersion}) AND Kennzeichen='Offen' AND (FA_Gestartet IS NULL OR FA_Gestartet=0)";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				return (int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var v) ? v : 0) == ids.Count;
			}
		}
		public static int UpgradeBOM(int bomVersion, string kundenIndex, DateTime? kundenIndexDate, int articleId, List<int> faIds, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(faIds == null || faIds.Count <= 0)
				return 0;

			string query = $"UPDATE [Fertigung] SET [Artikel_Nr]=@articleId, [CPVersion]=@cpVersion, [BomVersion]=@bomVersion, [KundenIndex]=@kundenIndex, [Kunden_Index_Datum]=@kundenIndexDate WHERE [ID] IN ({string.Join(",", faIds)})";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("articleId", articleId);
			sqlCommand.Parameters.AddWithValue("bomVersion", bomVersion);
			sqlCommand.Parameters.AddWithValue("cpVersion", (object)DBNull.Value);
			sqlCommand.Parameters.AddWithValue("kundenIndex", kundenIndex == null ? (object)DBNull.Value : kundenIndex);
			sqlCommand.Parameters.AddWithValue("kundenIndexDate", kundenIndexDate == null ? (object)DBNull.Value : kundenIndexDate);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int UpgradePreviousFABOM(int bomVersion)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"UPDATE [Fertigung] SET [BomVersion]=@bomVersion WHERE [BomVersion]<@bomVersion AND [CPVersion] IS NULL;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("bomVersion", bomVersion);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetListFertigung(string client, string article, string order, string status, int? lager, bool? gestart, bool? prio, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT F.* FROM [Fertigung] F INNER JOIN [Artikel] A on F.[Artikel_Nr]=A.[Artikel-Nr] LEFT JOIN" + " " +
							   "(" + " " +
							   "SELECT DISTINCT [Nummerschlüssel] FROM [PSZ_Nummerschlüssel Kunde]" + " " +
							   ") K" + " " +
							   "ON SUBSTRING(A.Artikelnummer, 1, 3) = K.[Nummerschlüssel]  WHERE F.[Fertigungsnummer] IS NOT NULL ";

				if(!string.IsNullOrEmpty(client) && !string.IsNullOrWhiteSpace(client))
				{
					query += " AND K.[Nummerschlüssel]='" + client + "'";
				}
				if(!string.IsNullOrEmpty(article) && !string.IsNullOrWhiteSpace(article))
				{
					query += " AND A.[Artikelnummer] LIKE '%" + article + "%'";
				}
				if(!string.IsNullOrEmpty(order) && !string.IsNullOrWhiteSpace(order))
				{
					query += " AND F.[Fertigungsnummer] =" + order + "";
				}
				if(!string.IsNullOrWhiteSpace(status) && !string.IsNullOrEmpty(status))
				{
					query += " AND F.[Kennzeichen] = '" + status + "'";
				}
				if(lager.HasValue)
				{
					query += $" AND F.[Lagerort_id] = {lager}";
				}
				if(gestart.HasValue)
				{
					var value = gestart.Value ? "1" : "0 OR F.[FA_Gestartet] IS NULL";
					query += $" AND (F.[FA_Gestartet] = {value})";
				}
				if(prio.HasValue && prio.Value)
				{
					query += $" AND F.[Prio] = 1";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY F.[Fertigungsnummer] DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetLikeNumber(string number)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT TOP 10 * FROM [Fertigung] WHERE [Fertigungsnummer] LIKE @number";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("number", number + "%");
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static int GetListFertigungCount(string client, string article, string order, string status, int? lager, bool? gestart, bool? prio)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT COUNT(*) FROM [Fertigung] F inner join [Artikel] A on F.[Artikel_Nr]=A.[Artikel-Nr] inner join [PSZ_Nummerschlüssel Kunde] K on SUBSTRING(A.Artikelnummer, 1, 3)=K.[Nummerschlüssel] where F.Fertigungsnummer is not null ";

				using(var sqlCommand = new SqlCommand())
				{
					if(!string.IsNullOrEmpty(client) && !string.IsNullOrWhiteSpace(client))
					{
						query += " and K.[Nummerschlüssel]='" + client + "'";
					}
					if(!string.IsNullOrEmpty(article) && !string.IsNullOrWhiteSpace(article))
					{
						query += " and A.Artikelnummer like '%" + article + "%'";
					}
					if(!string.IsNullOrEmpty(order) && !string.IsNullOrWhiteSpace(order))
					{
						query += " and F.Fertigungsnummer =" + order + "";
					}
					if(!string.IsNullOrWhiteSpace(status) && !string.IsNullOrEmpty(status))
					{
						query += " and F.Kennzeichen = '" + status + "'";
					}
					if(lager.HasValue)
					{
						query += " AND F.[Lagerort_id] = @lager";
						sqlCommand.Parameters.AddWithValue("lager", lager);
					}
					if(gestart.HasValue)
					{
						query += " AND F.[FA_Gestartet] = @gestart";
						sqlCommand.Parameters.AddWithValue("gestart", gestart);
					}
					if(prio.HasValue && prio.Value)
					{
						query += " AND F.[Prio] = 1";
					}
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
				}
			}
		}
		//
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> getByFAIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [Fertigung] WHERE [Fertigungsnummer] IN (" + queryIds + ")";
					sqlCommand.CommandTimeout = 240; //in seconds
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetByFAIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getByFAIds(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByFAIds(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getByFAIds(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetOpenByArticle(int id, int? lagerId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Fertigung] WHERE [Artikel_Nr]=@id AND Kennzeichen = 'offen' {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetDoneByArticle(List<int> ids, int? lagerId = null)
		{
			if(ids == null || ids.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Fertigung] WHERE [Artikel_Nr] IN ({(string.Join(",", ids))}) AND Kennzeichen = 'erledigt' {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetOpenByArticle(IEnumerable<int> ids, int? lagerId = null)
		{
			if(ids == null || ids.Count() <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Fertigung] WHERE [Artikel_Nr] IN ({(string.Join(",", ids))}) AND Kennzeichen = 'offen' {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetDoneByArticleAndIndex(List<int> ids, string index, int? lagerId = null)
		{
			if(ids == null || ids.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Fertigung] WHERE [Artikel_Nr] IN ({(string.Join(",", ids))}) AND [KundenIndex]=@index AND Kennzeichen = 'erledigt' {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("index", index);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetOpenByArticleAndIndex(int id, string index, int? lagerId)
		{
			index = index ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Fertigung] WHERE [Artikel_Nr]=@id AND Kennzeichen = 'offen' AND [KundenIndex]=@index {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				sqlCommand.Parameters.AddWithValue("index", index);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetOpenByArticleAndIndex(List<int> ids, string index, int? lagerId)
		{
			if(ids == null || ids.Count <= 0)
			{
				return null;
			}
			index = index ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Fertigung] WHERE [Artikel_Nr] IN ({(string.Join(",", ids))}) AND Kennzeichen = 'offen' AND [KundenIndex]=@index {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("index", index);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetAvailableUBGByArticle(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigung] WHERE [Anzahl]=[OriginalAnzahl] AND IsNULL([HBGFAPositionId],0)=0 AND [Artikel_Nr]=@articleIdarticleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> getByArticles(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [Fertigung] WHERE [Artikel_Nr] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetByArticles(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getByArticles(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByArticles(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getByArticles(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
		}
		public static List<int> GetByArtikelNr(int articleNR, string faNummer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT distinct Fertigungsnummer FROM [Fertigung] f WHERE f.Kennzeichen='offen' AND f.Artikel_Nr=@ArticleNr AND Fertigungsnummer LIKE '{faNummer}%' ORDER BY Fertigungsnummer ASC";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ArticleNr", articleNR);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => int.TryParse(x["Fertigungsnummer"].ToString(), out var _x) ? _x : 0).ToList();
				;
			}
			else
			{
				return new List<int>();
			}
		}
		#endregion
		#region Querys with transactions
		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			Debug.WriteLine($"transaction FA insert {transaction.ToString()}");
			int response = -1;

			string query = @"INSERT INTO [Fertigung] ([Angebot_Artikel_Nr],[Angebot_nr],[Anzahl],[Anzahl_aktuell],[Anzahl_erledigt],[AnzahlnachgedrucktPPS],[Artikel_Nr],
									[Ausgangskontrolle],[Bemerkung],[Bemerkung II Planung],[Bemerkung ohne stätte],[Bemerkung_Planung],[Bemerkung_Technik],[Bemerkung_zu_Prio],
									[Check_FAbegonnen],[Check_Gewerk1],[Check_Gewerk1_Teilweise],[Check_Gewerk2],[Check_Gewerk2_Teilweise],[Check_Gewerk3],[Check_Gewerk3_Teilweise],
									[Check_Kabelgeschnitten],[Datum],[Endkontrolle],[Erledigte_FA_Datum],[Erstmuster],[FA_begonnen],[FA_Druckdatum],[FA_Gestartet],[Fa-NachdruckPPS],
									[Fertigungsnummer],[gebucht],[gedruckt],[Gewerk 1],[Gewerk 2],[Gewerk 3],[Gewerk_Teilweise_Bemerkung],[GrundNachdruckPPS],[ID_Hauptartikel],
									[ID_Rahmenfertigung],[Kabel_geschnitten],[Kabel_geschnitten_Datum],[Kabel_Schneidebeginn],[Kabel_Schneidebeginn_Datum],[Kennzeichen],
									[Kommisioniert_komplett],[Kommisioniert_teilweise],[KundenIndex],[Lagerort_id],[Lagerort_id zubuchen],[Letzte_Gebuchte_Menge],[Löschen],[Mandant],
									[Menge1],[Menge2],[Originalanzahl],[Planungsstatus],[Preis],[Prio],[Quick_Area],[ROH_umgebucht],[Spritzgießerei_abgeschlossen],[Tage Abweichung],
									[Technik],[Techniker],[Termin_Bestätigt1],[Termin_Bestätigt2],[Termin_Fertigstellung],[Termin_Material],[Termin_Ursprünglich],[Termin_voränderung],
									[UBG],[UBGTransfer],[Urs-Artikelnummer],[Urs-Fa],[Zeit],[BomVersion],[CPVersion],[HBGFAPositionId],[FertigungType],[PlanningDateViolation])  
							VALUES (
									@Angebot_Artikel_Nr,@Angebot_nr,@Anzahl,@Anzahl_aktuell,@Anzahl_erledigt,@AnzahlnachgedrucktPPS,@Artikel_Nr,
									@Ausgangskontrolle,@Bemerkung,@Bemerkung_II_Planung,@Bemerkung_ohne_stätte,@Bemerkung_Planung,@Bemerkung_Technik,@Bemerkung_zu_Prio,
									@Check_FAbegonnen,@Check_Gewerk1,@Check_Gewerk1_Teilweise,@Check_Gewerk2,@Check_Gewerk2_Teilweise,@Check_Gewerk3,@Check_Gewerk3_Teilweise,
									@Check_Kabelgeschnitten,@Datum,@Endkontrolle,@Erledigte_FA_Datum,@Erstmuster,@FA_begonnen,@FA_Druckdatum,@FA_Gestartet,@Fa_NachdruckPPS,
									@Fertigungsnummer,@gebucht,@gedruckt,@Gewerk_1,@Gewerk_2,@Gewerk_3,@Gewerk_Teilweise_Bemerkung,@GrundNachdruckPPS,@ID_Hauptartikel,
									@ID_Rahmenfertigung,@Kabel_geschnitten,@Kabel_geschnitten_Datum,@Kabel_Schneidebeginn,@Kabel_Schneidebeginn_Datum,@Kennzeichen,
									@Kommisioniert_komplett,@Kommisioniert_teilweise,@KundenIndex,@Lagerort_id,@Lagerort_id_zubuchen,@Letzte_Gebuchte_Menge,@Löschen,@Mandant,
									@Menge1,@Menge2,@Originalanzahl,@Planungsstatus,@Preis,@Prio,@Quick_Area,@ROH_umgebucht,@Spritzgießerei_abgeschlossen,@Tage_Abweichung,
									@Technik,@Techniker,@Termin_Bestätigt1,@Termin_Bestätigt2,@Termin_Fertigstellung,@Termin_Material,@Termin_Ursprünglich,@Termin_voränderung,
									@UBG,@UBGTransfer,@Urs_Artikelnummer,@Urs_Fa,@Zeit,@BomVersion,@CPVersion,@HBGFAPositionId,@FertigungType,@PlanningDateViolation);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr", item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Angebot_nr", item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Anzahl_aktuell", item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
			sqlCommand.Parameters.AddWithValue("Anzahl_erledigt", item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
			sqlCommand.Parameters.AddWithValue("AnzahlnachgedrucktPPS", item.AnzahlnachgedrucktPPS);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Ausgangskontrolle", item.Ausgangskontrolle == null ? (object)DBNull.Value : item.Ausgangskontrolle);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkung_II_Planung", item.Bemerkung_II_Planung == null ? (object)DBNull.Value : item.Bemerkung_II_Planung);
			sqlCommand.Parameters.AddWithValue("Bemerkung_ohne_stätte", item.Bemerkung_ohne_statte == null ? (object)DBNull.Value : item.Bemerkung_ohne_statte);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Planung", item.Bemerkung_Planung == null ? (object)DBNull.Value : item.Bemerkung_Planung);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Technik", item.Bemerkung_Technik == null ? (object)DBNull.Value : item.Bemerkung_Technik);
			sqlCommand.Parameters.AddWithValue("Bemerkung_zu_Prio", item.Bemerkung_zu_Prio == null ? (object)DBNull.Value : item.Bemerkung_zu_Prio);
			sqlCommand.Parameters.AddWithValue("Check_FAbegonnen", item.Check_FAbegonnen == null ? (object)DBNull.Value : item.Check_FAbegonnen);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk1", item.Check_Gewerk1 == null ? (object)DBNull.Value : item.Check_Gewerk1);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk1_Teilweise", item.Check_Gewerk1_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk1_Teilweise);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk2", item.Check_Gewerk2 == null ? (object)DBNull.Value : item.Check_Gewerk2);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk2_Teilweise", item.Check_Gewerk2_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk2_Teilweise);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk3", item.Check_Gewerk3 == null ? (object)DBNull.Value : item.Check_Gewerk3);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk3_Teilweise", item.Check_Gewerk3_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk3_Teilweise);
			sqlCommand.Parameters.AddWithValue("Check_Kabelgeschnitten", item.Check_Kabelgeschnitten == null ? (object)DBNull.Value : item.Check_Kabelgeschnitten);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Endkontrolle", item.Endkontrolle == null ? (object)DBNull.Value : item.Endkontrolle);
			sqlCommand.Parameters.AddWithValue("Erledigte_FA_Datum", item.Erledigte_FA_Datum == null ? (object)DBNull.Value : item.Erledigte_FA_Datum);
			sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
			sqlCommand.Parameters.AddWithValue("FA_begonnen", item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
			sqlCommand.Parameters.AddWithValue("FA_Druckdatum", item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
			sqlCommand.Parameters.AddWithValue("FA_Gestartet", item.FA_Gestartet == null ? (object)DBNull.Value : item.FA_Gestartet);
			sqlCommand.Parameters.AddWithValue("Fa_NachdruckPPS", item.Fa_NachdruckPPS == null ? (object)DBNull.Value : item.Fa_NachdruckPPS);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("gebucht", item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
			sqlCommand.Parameters.AddWithValue("gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
			sqlCommand.Parameters.AddWithValue("Gewerk_1", item.Gewerk_1 == null ? "" : item.Gewerk_1);
			sqlCommand.Parameters.AddWithValue("Gewerk_2", item.Gewerk_2 == null ? "" : item.Gewerk_2);
			sqlCommand.Parameters.AddWithValue("Gewerk_3", item.Gewerk_3 == null ? "" : item.Gewerk_3);
			sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung", item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
			sqlCommand.Parameters.AddWithValue("GrundNachdruckPPS", item.GrundNachdruckPPS == null ? (object)DBNull.Value : item.GrundNachdruckPPS);
			//sqlCommand.Parameters.AddWithValue("ID",item.ID);
			sqlCommand.Parameters.AddWithValue("ID_Hauptartikel", item.ID_Hauptartikel == null ? (object)DBNull.Value : item.ID_Hauptartikel);
			sqlCommand.Parameters.AddWithValue("ID_Rahmenfertigung", item.ID_Rahmenfertigung == null ? (object)DBNull.Value : item.ID_Rahmenfertigung);
			sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
			sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum", item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
			sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn", item.Kabel_Schneidebeginn == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn);
			sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn_Datum", item.Kabel_Schneidebeginn_Datum == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn_Datum);
			sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
			sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett", item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
			sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise", item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
			sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Lagerort_id_zubuchen", item.Lagerort_id_zubuchen == null ? (object)DBNull.Value : item.Lagerort_id_zubuchen);
			sqlCommand.Parameters.AddWithValue("Letzte_Gebuchte_Menge", item.Letzte_Gebuchte_Menge == null ? (object)DBNull.Value : item.Letzte_Gebuchte_Menge);
			sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("Menge1", item.Menge1 == null ? (object)DBNull.Value : item.Menge1);
			sqlCommand.Parameters.AddWithValue("Menge2", item.Menge2 == null ? (object)DBNull.Value : item.Menge2);
			sqlCommand.Parameters.AddWithValue("Originalanzahl", item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
			sqlCommand.Parameters.AddWithValue("Planungsstatus", item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
			sqlCommand.Parameters.AddWithValue("Preis", item.Preis == null ? (object)DBNull.Value : item.Preis);
			sqlCommand.Parameters.AddWithValue("Prio", item.Prio == null ? (object)DBNull.Value : item.Prio);
			sqlCommand.Parameters.AddWithValue("Quick_Area", item.Quick_Area == null ? (object)DBNull.Value : item.Quick_Area);
			sqlCommand.Parameters.AddWithValue("ROH_umgebucht", item.ROH_umgebucht == null ? (object)DBNull.Value : item.ROH_umgebucht);
			sqlCommand.Parameters.AddWithValue("Spritzgießerei_abgeschlossen", item.SpritzgieBerei_abgeschlossen == null ? (object)DBNull.Value : item.SpritzgieBerei_abgeschlossen);
			sqlCommand.Parameters.AddWithValue("Tage_Abweichung", item.Tage_Abweichung == null ? (object)DBNull.Value : item.Tage_Abweichung);
			sqlCommand.Parameters.AddWithValue("Technik", item.Technik == null ? (object)DBNull.Value : item.Technik);
			sqlCommand.Parameters.AddWithValue("Techniker", item.Techniker == null ? (object)DBNull.Value : item.Techniker);
			sqlCommand.Parameters.AddWithValue("Termin_Bestätigt1", item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
			sqlCommand.Parameters.AddWithValue("Termin_Bestätigt2", item.Termin_Bestatigt2 == null ? (object)DBNull.Value : item.Termin_Bestatigt2);
			sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung", item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
			sqlCommand.Parameters.AddWithValue("Termin_Material", item.Termin_Material == null ? (object)DBNull.Value : item.Termin_Material);
			sqlCommand.Parameters.AddWithValue("Termin_Ursprünglich", item.Termin_Ursprunglich == null ? (object)DBNull.Value : item.Termin_Ursprunglich);
			sqlCommand.Parameters.AddWithValue("Termin_voränderung", item.Termin_voranderung == null ? (object)DBNull.Value : item.Termin_voranderung);
			sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
			sqlCommand.Parameters.AddWithValue("UBGTransfer", item.UBGTransfer == null ? (object)DBNull.Value : item.UBGTransfer);
			sqlCommand.Parameters.AddWithValue("Urs_Artikelnummer", item.Urs_Artikelnummer == null ? (object)DBNull.Value : item.Urs_Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Urs_Fa", item.Urs_Fa == null ? (object)DBNull.Value : item.Urs_Fa);
			sqlCommand.Parameters.AddWithValue("Zeit", item.Zeit == null ? (object)DBNull.Value : item.Zeit);

			sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
			sqlCommand.Parameters.AddWithValue("CPVersion", item.CPVersion == null ? (object)DBNull.Value : item.CPVersion);
			sqlCommand.Parameters.AddWithValue("HBGFAPositionId", item.HBGFAPositionId == null ? (object)DBNull.Value : item.HBGFAPositionId);
			sqlCommand.Parameters.AddWithValue("FertigungType", item.FertigungType == null ? (object)DBNull.Value : item.FertigungType);
			sqlCommand.Parameters.AddWithValue("PlanningDateViolation", item.PlanningDateViolation == null ? (object)DBNull.Value : item.PlanningDateViolation);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

			return response;
		}
		public static Infrastructure.Data.Entities.Tables.PRS.FertigungEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			Debug.WriteLine($"transaction FA Get {transaction.ToString()}");
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Fertigung] WHERE [ID]=@id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("id", id);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			Debug.WriteLine($"transaction FA update {transaction.ToString()}");
			int results = -1;
			//using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			//{
			//sqlConnection.Open();
			string query = "UPDATE [Fertigung] SET [Angebot_nr]=@Angebot_nr, [Anzahl]=@Anzahl, [Anzahl_aktuell]=@Anzahl_aktuell, [Anzahl_erledigt]=@Anzahl_erledigt, "
				+ " [AnzahlnachgedrucktPPS]=@AnzahlnachgedrucktPPS, [Artikel_Nr]=@Artikel_Nr, [Ausgangskontrolle]=@Ausgangskontrolle, [Bemerkung]=@Bemerkung, "
				+ " [Bemerkung II Planung]=@Bemerkung_II_Planung, [Bemerkung ohne stätte]=@Bemerkung_ohne_stätte, [Bemerkung_Planung]=@Bemerkung_Planung, "
				+ " [Bemerkung_Technik]=@Bemerkung_Technik, [Bemerkung_zu_Prio]=@Bemerkung_zu_Prio, [Check_FAbegonnen]=@Check_FAbegonnen, [Check_Gewerk1]=@Check_Gewerk1, "
				+ " [Check_Gewerk1_Teilweise]=@Check_Gewerk1_Teilweise, [Check_Gewerk2]=@Check_Gewerk2, [Check_Gewerk2_Teilweise]=@Check_Gewerk2_Teilweise, "
				+ " [Check_Gewerk3]=@Check_Gewerk3, [Check_Gewerk3_Teilweise]=@Check_Gewerk3_Teilweise, [Check_Kabelgeschnitten]=@Check_Kabelgeschnitten, "
				+ " [Datum]=@Datum, [Endkontrolle]=@Endkontrolle, [Erledigte_FA_Datum]=@Erledigte_FA_Datum, [Erstmuster]=@Erstmuster, [FA_begonnen]=@FA_begonnen, "
				+ " [FA_Druckdatum]=@FA_Druckdatum, [FA_Gestartet]=@FA_Gestartet, [Fa-NachdruckPPS]=@Fa_NachdruckPPS, [Fertigungsnummer]=@Fertigungsnummer, [gebucht]=@gebucht, "
				+ " [gedruckt]=@gedruckt, [Gewerk 1]=@Gewerk_1, [Gewerk 2]=@Gewerk_2, [Gewerk 3]=@Gewerk_3, [Gewerk_Teilweise_Bemerkung]=@Gewerk_Teilweise_Bemerkung, "
				+ " [GrundNachdruckPPS]=@GrundNachdruckPPS, [ID_Hauptartikel]=@ID_Hauptartikel, [ID_Rahmenfertigung]=@ID_Rahmenfertigung, [Kabel_geschnitten]=@Kabel_geschnitten, "
				+ " [Kabel_geschnitten_Datum]=@Kabel_geschnitten_Datum, [Kabel_Schneidebeginn]=@Kabel_Schneidebeginn, [Kabel_Schneidebeginn_Datum]=@Kabel_Schneidebeginn_Datum, "
				+ " [Kennzeichen]=@Kennzeichen, [Kommisioniert_komplett]=@Kommisioniert_komplett, [Kommisioniert_teilweise]=@Kommisioniert_teilweise, [KundenIndex]=@KundenIndex, "
				+ " [Lagerort_id]=@Lagerort_id, [Lagerort_id zubuchen]=@Lagerort_id_zubuchen, [Letzte_Gebuchte_Menge]=@Letzte_Gebuchte_Menge, [Löschen]=@Löschen, [Mandant]=@Mandant, "
				+ " [Menge1]=@Menge1, [Menge2]=@Menge2, [Originalanzahl]=@Originalanzahl, [Planungsstatus]=@Planungsstatus, [Preis]=@Preis, [Prio]=@Prio, [Quick_Area]=@Quick_Area, "
				+ " [ROH_umgebucht]=@ROH_umgebucht, [Spritzgießerei_abgeschlossen]=@Spritzgießerei_abgeschlossen, [Tage Abweichung]=@Tage_Abweichung, [Technik]=@Technik, "
				+ " [Techniker]=@Techniker, [Termin_Bestätigt1]=@Termin_Bestätigt1, [Termin_Bestätigt2]=@Termin_Bestätigt2, [Termin_Fertigstellung]=@Termin_Fertigstellung, "
				+ " [Termin_Material]=@Termin_Material, [Termin_Ursprünglich]=@Termin_Ursprünglich, [Termin_voränderung]=@Termin_voränderung, [UBG]=@UBG, [UBGTransfer]=@UBGTransfer, "
				+ " [Urs-Artikelnummer]=@Urs_Artikelnummer, [Urs-Fa]=@Urs_Fa, [Zeit]=@Zeit, [Angebot_Artikel_Nr]=@Angebot_Artikel_Nr,[CPVersion]=@CPVersion,[BomVersion]=@BomVersion,[HBGFAPositionId]=@HBGFAPositionId,[Termin_Bestatigt2_Updated]=@Termin_Bestatigt2_Updated,[PlanningDateViolation]=@PlanningDateViolation "
				+ " WHERE [ID]=@ID";

			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Termin_Bestatigt2_Updated", item.Termin_Bestatigt2_Updated == null ? (object)DBNull.Value : item.Termin_Bestatigt2_Updated);
			sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr", item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Angebot_nr", item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Anzahl_aktuell", item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
			sqlCommand.Parameters.AddWithValue("Anzahl_erledigt", item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
			sqlCommand.Parameters.AddWithValue("AnzahlnachgedrucktPPS", item.AnzahlnachgedrucktPPS);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Ausgangskontrolle", item.Ausgangskontrolle == null ? (object)DBNull.Value : item.Ausgangskontrolle);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkung_II_Planung", item.Bemerkung_II_Planung == null ? (object)DBNull.Value : item.Bemerkung_II_Planung);
			sqlCommand.Parameters.AddWithValue("Bemerkung_ohne_stätte", item.Bemerkung_ohne_statte == null ? (object)DBNull.Value : item.Bemerkung_ohne_statte);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Planung", item.Bemerkung_Planung == null ? (object)DBNull.Value : item.Bemerkung_Planung);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Technik", item.Bemerkung_Technik == null ? (object)DBNull.Value : item.Bemerkung_Technik);
			sqlCommand.Parameters.AddWithValue("Bemerkung_zu_Prio", item.Bemerkung_zu_Prio == null ? (object)DBNull.Value : item.Bemerkung_zu_Prio);
			sqlCommand.Parameters.AddWithValue("Check_FAbegonnen", item.Check_FAbegonnen == null ? (object)DBNull.Value : item.Check_FAbegonnen);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk1", item.Check_Gewerk1 == null ? (object)DBNull.Value : item.Check_Gewerk1);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk1_Teilweise", item.Check_Gewerk1_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk1_Teilweise);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk2", item.Check_Gewerk2 == null ? (object)DBNull.Value : item.Check_Gewerk2);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk2_Teilweise", item.Check_Gewerk2_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk2_Teilweise);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk3", item.Check_Gewerk3 == null ? (object)DBNull.Value : item.Check_Gewerk3);
			sqlCommand.Parameters.AddWithValue("Check_Gewerk3_Teilweise", item.Check_Gewerk3_Teilweise == null ? (object)DBNull.Value : item.Check_Gewerk3_Teilweise);
			sqlCommand.Parameters.AddWithValue("Check_Kabelgeschnitten", item.Check_Kabelgeschnitten == null ? (object)DBNull.Value : item.Check_Kabelgeschnitten);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Endkontrolle", item.Endkontrolle == null ? (object)DBNull.Value : item.Endkontrolle);
			sqlCommand.Parameters.AddWithValue("Erledigte_FA_Datum", item.Erledigte_FA_Datum == null ? (object)DBNull.Value : item.Erledigte_FA_Datum);
			sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
			sqlCommand.Parameters.AddWithValue("FA_begonnen", item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
			sqlCommand.Parameters.AddWithValue("FA_Druckdatum", item.FA_Druckdatum == null ? (object)DBNull.Value : item.FA_Druckdatum);
			sqlCommand.Parameters.AddWithValue("FA_Gestartet", item.FA_Gestartet == null ? (object)DBNull.Value : item.FA_Gestartet);
			sqlCommand.Parameters.AddWithValue("Fa_NachdruckPPS", item.Fa_NachdruckPPS == null ? (object)DBNull.Value : item.Fa_NachdruckPPS);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("gebucht", item.Gebucht == null ? (object)DBNull.Value : item.Gebucht);
			sqlCommand.Parameters.AddWithValue("gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
			sqlCommand.Parameters.AddWithValue("Gewerk_1", item.Gewerk_1 == null ? "" : item.Gewerk_1);
			sqlCommand.Parameters.AddWithValue("Gewerk_2", item.Gewerk_2 == null ? "" : item.Gewerk_2);
			sqlCommand.Parameters.AddWithValue("Gewerk_3", item.Gewerk_3 == null ? "" : item.Gewerk_3);
			sqlCommand.Parameters.AddWithValue("Gewerk_Teilweise_Bemerkung", item.Gewerk_Teilweise_Bemerkung == null ? (object)DBNull.Value : item.Gewerk_Teilweise_Bemerkung);
			sqlCommand.Parameters.AddWithValue("GrundNachdruckPPS", item.GrundNachdruckPPS == null ? (object)DBNull.Value : item.GrundNachdruckPPS);
			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("ID_Hauptartikel", item.ID_Hauptartikel == null ? (object)DBNull.Value : item.ID_Hauptartikel);
			sqlCommand.Parameters.AddWithValue("ID_Rahmenfertigung", item.ID_Rahmenfertigung == null ? (object)DBNull.Value : item.ID_Rahmenfertigung);
			sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
			sqlCommand.Parameters.AddWithValue("Kabel_geschnitten_Datum", item.Kabel_geschnitten_Datum == null ? (object)DBNull.Value : item.Kabel_geschnitten_Datum);
			sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn", item.Kabel_Schneidebeginn == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn);
			sqlCommand.Parameters.AddWithValue("Kabel_Schneidebeginn_Datum", item.Kabel_Schneidebeginn_Datum == null ? (object)DBNull.Value : item.Kabel_Schneidebeginn_Datum);
			sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
			sqlCommand.Parameters.AddWithValue("Kommisioniert_komplett", item.Kommisioniert_komplett == null ? (object)DBNull.Value : item.Kommisioniert_komplett);
			sqlCommand.Parameters.AddWithValue("Kommisioniert_teilweise", item.Kommisioniert_teilweise == null ? (object)DBNull.Value : item.Kommisioniert_teilweise);
			sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Lagerort_id_zubuchen", item.Lagerort_id_zubuchen == null ? (object)DBNull.Value : item.Lagerort_id_zubuchen);
			sqlCommand.Parameters.AddWithValue("Letzte_Gebuchte_Menge", item.Letzte_Gebuchte_Menge == null ? (object)DBNull.Value : item.Letzte_Gebuchte_Menge);
			sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("Menge1", item.Menge1 == null ? (object)DBNull.Value : item.Menge1);
			sqlCommand.Parameters.AddWithValue("Menge2", item.Menge2 == null ? (object)DBNull.Value : item.Menge2);
			sqlCommand.Parameters.AddWithValue("Originalanzahl", item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
			sqlCommand.Parameters.AddWithValue("Planungsstatus", item.Planungsstatus == null ? (object)DBNull.Value : item.Planungsstatus);
			sqlCommand.Parameters.AddWithValue("Preis", item.Preis == null ? (object)DBNull.Value : item.Preis);
			sqlCommand.Parameters.AddWithValue("Prio", item.Prio == null ? (object)DBNull.Value : item.Prio);
			sqlCommand.Parameters.AddWithValue("Quick_Area", item.Quick_Area == null ? (object)DBNull.Value : item.Quick_Area);
			sqlCommand.Parameters.AddWithValue("ROH_umgebucht", item.ROH_umgebucht == null ? (object)DBNull.Value : item.ROH_umgebucht);
			sqlCommand.Parameters.AddWithValue("Spritzgießerei_abgeschlossen", item.SpritzgieBerei_abgeschlossen == null ? (object)DBNull.Value : item.SpritzgieBerei_abgeschlossen);
			sqlCommand.Parameters.AddWithValue("Tage_Abweichung", item.Tage_Abweichung == null ? (object)DBNull.Value : item.Tage_Abweichung);
			sqlCommand.Parameters.AddWithValue("Technik", item.Technik == null ? (object)DBNull.Value : item.Technik);
			sqlCommand.Parameters.AddWithValue("Techniker", item.Techniker == null ? (object)DBNull.Value : item.Techniker);
			sqlCommand.Parameters.AddWithValue("Termin_Bestätigt1", item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
			sqlCommand.Parameters.AddWithValue("Termin_Bestätigt2", item.Termin_Bestatigt2 == null ? (object)DBNull.Value : item.Termin_Bestatigt2);
			sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung", item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
			sqlCommand.Parameters.AddWithValue("Termin_Material", item.Termin_Material == null ? (object)DBNull.Value : item.Termin_Material);
			sqlCommand.Parameters.AddWithValue("Termin_Ursprünglich", item.Termin_Ursprunglich == null ? (object)DBNull.Value : item.Termin_Ursprunglich);
			sqlCommand.Parameters.AddWithValue("Termin_voränderung", item.Termin_voranderung == null ? (object)DBNull.Value : item.Termin_voranderung);
			sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
			sqlCommand.Parameters.AddWithValue("UBGTransfer", item.UBGTransfer == null ? (object)DBNull.Value : item.UBGTransfer);
			sqlCommand.Parameters.AddWithValue("Urs_Artikelnummer", item.Urs_Artikelnummer == null ? (object)DBNull.Value : item.Urs_Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Urs_Fa", item.Urs_Fa == null ? (object)DBNull.Value : item.Urs_Fa);
			sqlCommand.Parameters.AddWithValue("Zeit", item.Zeit == null ? (object)DBNull.Value : item.Zeit);
			sqlCommand.Parameters.AddWithValue("CPVersion", item.CPVersion == null ? (object)DBNull.Value : item.CPVersion);
			sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
			sqlCommand.Parameters.AddWithValue("HBGFAPositionId", item.HBGFAPositionId == null ? (object)DBNull.Value : item.HBGFAPositionId);
			sqlCommand.Parameters.AddWithValue("PlanningDateViolation", item.PlanningDateViolation == null ? (object)DBNull.Value : item.PlanningDateViolation);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			//}

			return results;
		}
		public static int UpdateArticleIndexWithTransaction(int articleId, string index, DateTime? indexDate, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Fertigung] SET [KundenIndex]=@KundenIndex, Kunden_Index_Datum=@indexDate"
				+ " WHERE [Artikel_Nr]=@Artikel_Nr AND Kennzeichen='offen' AND IsNULL(FA_Gestartet,0)=0";

			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Artikel_Nr", articleId);
			sqlCommand.Parameters.AddWithValue("KundenIndex", index == null ? (object)DBNull.Value : index);
			sqlCommand.Parameters.AddWithValue("indexDate", indexDate == null ? (object)DBNull.Value : indexDate);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int UpdateCommentsWithTransaction(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [Fertigung] SET [Bemerkung]=@Bemerkung, [Bemerkung ohne stätte]=@Bemerkung_ohne_stätte, [Bemerkung_Planung]=@Bemerkung_Planung WHERE [ID]=@ID";

			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkung_ohne_stätte", item.Bemerkung_ohne_statte == null ? (object)DBNull.Value : item.Bemerkung_ohne_statte);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Planung", item.Bemerkung_Planung == null ? (object)DBNull.Value : item.Bemerkung_Planung);
			sqlCommand.Parameters.AddWithValue("ID", item.ID);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			//}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetNotStartedOpenByArticle(int id, int? lagerId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = $"SELECT * FROM [Fertigung] WHERE [Artikel_Nr]=@id AND IsNULL(FA_Gestartet,0)=0 AND Kennzeichen = 'offen' {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")}";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("id", id);

			DbExecution.Fill(sqlCommand, dataTable);


			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetNonStartedByHBGFaPositionId(List<int> hbgPositionIds, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(hbgPositionIds == null || hbgPositionIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			{
				string query = $"SELECT * FROM [Fertigung] WHERE HBGFAPositionId IN ({string.Join(",", hbgPositionIds)}) AND Kennzeichen='Offen' AND (FA_Gestartet IS NULL OR FA_Gestartet=0)";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetNonStartedByHBGFaPositionId(List<int> hbgPositionIds)
		{
			if(hbgPositionIds == null || hbgPositionIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Fertigung] WHERE HBGFAPositionId IN ({string.Join(",", hbgPositionIds)}) AND Kennzeichen='Offen' AND (FA_Gestartet IS NULL OR FA_Gestartet=0)";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static int ChangeLagerNdArticleWithTransaction(IEnumerable<KeyValuePair<int, int>> faNewLagerArticles, int newLager, SqlConnection connection, SqlTransaction transaction)
		{
			if(faNewLagerArticles == null || faNewLagerArticles.Count() <= 0)
			{
				return -1;
			}
			string query = $"";
			foreach(var item in faNewLagerArticles)
			{
				query += $"UPDATE [Fertigung] SET [Lagerort_id]={newLager},[Artikel_Nr]={item.Value} WHERE [ID]={item.Key};";
			}

			var sqlCommand = new SqlCommand(query, connection, transaction);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetFaultyFA(string searchTerms, bool ubg, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			searchTerms = searchTerms ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM [Fertigung] WHERE [Kennzeichen]=N'offen' AND [Termin_Bestätigt1] < GETDATE() {(ubg == true ? " AND [Artikel_Nr] IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)" : " AND [Artikel_Nr] NOT IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)")} AND ([Fertigungsnummer] LIKE '{searchTerms.SqlEscape()}%' OR CAST([Bemerkung] AS nvarchar(2000)) LIKE '{searchTerms.SqlEscape()}%' OR CAST([Bemerkung_Planung] AS nvarchar(2000)) LIKE '{searchTerms.SqlEscape()}%')";

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [Fertigungsnummer] DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static int GetCountFaultyFA(string searchTerms, bool ubg)
		{
			searchTerms = searchTerms ?? "";
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT COUNT(*) FROM [Fertigung] WHERE [Kennzeichen]=N'offen' AND [Termin_Bestätigt1] < GETDATE(){(ubg == true ? " AND [Artikel_Nr] IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)" : " AND [Artikel_Nr] NOT IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)")} AND ([Fertigungsnummer] LIKE '{searchTerms.SqlEscape()}%' OR CAST([Bemerkung] AS nvarchar(2000)) LIKE '{searchTerms.SqlEscape()}%' OR CAST([Bemerkung_Planung] AS nvarchar(2000)) LIKE '{searchTerms.SqlEscape()}%')";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetFirstSampleTechnicByArticles(IEnumerable<int> ids)
		{
			if(ids != null && ids.Count() > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> results = null;
				if(ids.Count() <= maxQueryNumber)
				{
					results = getFirstSampleTechnicByArticles(ids);
				}
				else
				{
					int batchNumber = ids.Count() / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						var batch = ids.Skip(i * maxQueryNumber).Take(maxQueryNumber);
						results.AddRange(getFirstSampleTechnicByArticles(batch));
					}

					// Handle the remaining items (if any)
					var remaining = ids.Skip(batchNumber * maxQueryNumber);
					if(remaining.Any())
					{
						results.AddRange(getFirstSampleTechnicByArticles(remaining));
					}
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> getFirstSampleTechnicByArticles(IEnumerable<int> ids)
		{
			if(ids != null && ids.Count() > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					using(var sqlCommand = sqlConnection.CreateCommand())
					{
						var idsList = ids.ToList(); // Materialize the IEnumerable so we can enumerate it more than once
						var parameterNames = new List<string>();

						for(int i = 0; i < idsList.Count; i++)
						{
							string paramName = $"@Id{i}";
							parameterNames.Add(paramName);
							sqlCommand.Parameters.AddWithValue(paramName, idsList[i]);
						}

						sqlCommand.CommandText = $"SELECT * FROM [Fertigung] WHERE [Artikel_Nr] IN ({string.Join(",", parameterNames)}) AND (ISNULL(Erstmuster,0)=1 OR ISNULL(Technik,0)=1)";
						using(var adapter = new SqlDataAdapter(sqlCommand))
						{
							adapter.Fill(dataTable);
						}
					}
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
		}
		public static List<FertigungEntity> GetByPlanningViolation(string faStatus, DateTime? from, DateTime? to, int? Lagerort_id = null, Settings.SortingModel sorting = null, Data.Access.Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = " select * from [Fertigung] as fa";
				List<string> clauses = new List<string>();


				clauses.Add("fa.PlanningDateViolation=1 ");
				if(Lagerort_id is not null)
				{
					clauses.Add($"fa.Lagerort_id = {Lagerort_id}");
				}

				if(!string.IsNullOrWhiteSpace(faStatus))
				{
					clauses.Add($"LOWER(fa.Kennzeichen) = '{faStatus.ToLower()}'");
				}

				if(from is not null)
				{
					clauses.Add($"CONVERT(date, fa.Termin_Bestätigt1) >= CONVERT(date, '{from:yyyy-MM-dd}')");
				}

				if(to is not null)
				{
					clauses.Add($"CONVERT(date, fa.Termin_Bestätigt1) <= CONVERT(date, '{to:yyyy-MM-dd}')");
				}

				if(clauses.Count > 0)
				{
					query += " WHERE " + string.Join(" AND ", clauses);
				}

				#region >>>>> pagination <<<<<<<
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY fa.Fertigungsnummer DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				#endregion pagination sorting


				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<FertigungEntity>();
			}
		}
		public static int GetByPlanningViolationCount(string faStatus, DateTime? from, DateTime? to, int? Lagerort_id = null)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " select count(*) from [Fertigung] as fa";
				List<string> clauses = new List<string>();


				clauses.Add("fa.PlanningDateViolation=1 ");
				if(Lagerort_id is not null)
				{
					clauses.Add($"fa.Lagerort_id = {Lagerort_id}");
				}

				if(!string.IsNullOrWhiteSpace(faStatus))
				{
					clauses.Add($"LOWER(fa.Kennzeichen) = '{faStatus.ToLower()}'");
				}

				if(from is not null)
				{
					clauses.Add($"CONVERT(date, fa.Termin_Bestätigt1) >= CONVERT(date, '{from:yyyy-MM-dd}')");
				}

				if(to is not null)
				{
					clauses.Add($"CONVERT(date, fa.Termin_Bestätigt1) <= CONVERT(date, '{to:yyyy-MM-dd}')");
				}

				if(clauses.Count > 0)
				{
					query += " WHERE " + string.Join(" AND ", clauses);
				}

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
				}
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetOpenSerieWithNullOrZeroTimeCosts(int? lagerId=null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Fertigung] WHERE FertigungType='Serie' AND Kennzeichen='offen' AND (ISNULL([Zeit],0)=0 OR ISNULL(Preis,0)=0) {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
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
		public static int UpdateProdTimeCosts(List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [Fertigung] SET "

					+ "[Preis]=@Preis" + i + ","
					+ "[Zeit]=@Zeit" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Preis" + i, item.Preis == null ? (object)DBNull.Value : item.Preis);
					sqlCommand.Parameters.AddWithValue("Zeit" + i, item.Zeit == null ? (object)DBNull.Value : item.Zeit);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}
		//
		public static bool ExistFertigungsnummer(int faNummer)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) FROM [Fertigung] WHERE [Fertigungsnummer]=@faNummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("faNummer", faNummer);

				return (int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var _x) ? _x : 0) > 0;
			}
		}
		#endregion
		public static int SetPrintDate(int faId, DateTime printDate)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				using(var sqlCommand = new SqlCommand("UPDATE [Fertigung] Set [Gedruckt]=1, [FA_Druckdatum]=@printDate WHERE [ID]=@faId", sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("faId", faId);
					sqlCommand.Parameters.AddWithValue("printDate", printDate);
					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out int count) ? count : 0;
				}
			}
		}
		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(dataRow)); }
			return list;
		}
		public static int UpdatePrioById(int faId, bool prio)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"UPDATE [Fertigung] SET [prio]=@prio WHERE [ID]=@faId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("faId", faId);
				sqlCommand.Parameters.AddWithValue("prio", prio);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		#endregion
	}
}