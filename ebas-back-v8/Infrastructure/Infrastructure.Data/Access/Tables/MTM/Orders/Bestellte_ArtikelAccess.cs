using Infrastructure.Data.Entities.Tables.MTM;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public static class Bestellte_ArtikelAccess
	{
		#region Default Methods
		public static Bestellte_ArtikelEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [bestellte Artikel] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Nr", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Bestellte_ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Bestellte_ArtikelEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [bestellte Artikel]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Bestellte_ArtikelEntity>();
			}
		}
		public static List<Bestellte_ArtikelEntity> get(List<int> ids)
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
						queryIds += "@Nr" + i + ",";
						sqlCommand.Parameters.AddWithValue("Nr" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = $"SELECT * FROM [bestellte Artikel] WHERE [Nr] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Bestellte_ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Bestellte_ArtikelEntity>();
				}
			}
			return new List<Bestellte_ArtikelEntity>();
		}
		public static List<Bestellte_ArtikelEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Bestellte_ArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Bestellte_ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Bestellte_ArtikelEntity>();
		}
		public static int Insert(Bestellte_ArtikelEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [bestellte Artikel] ([Position],[Bestellung-Nr],[Artikel-Nr],[Bezeichnung 1],[Bezeichnung 2],[Einheit],[AnfangLagerBestand],[Anzahl],[Start Anzahl],[Erhalten],[Aktuelle Anzahl],[EndeLagerBestand],[Umsatzsteuer],[Einzelpreis],[Gesamtpreis],[Preisgruppe],[Bestellnummer],[Rabatt],[Rabatt1],[sortierung],[schriftart],[Preiseinheit],[Liefertermin],[erledigt_pos],[Lagerort_id],[Bestätigter_Termin],[Position erledigt],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Produktionsort],[BP zu RBposition],[WE Pos zu Bestellposition],[AB-Nr_Lieferant],[RB_OriginalAnzahl],[RB_Abgerufen],[RB_Offen],[In Bearbeitung],[Löschen],[Kanban],[MhdDatumArtikel],[COC_bestätigung],[InfoRahmennummer],[EMPB_Bestätigung],[CUPreis],[RA Pos zu Bestellposition],[CocVersion],[LagerbewegungPositionId],[StandardSupplierViolation]) OUTPUT INSERTED.[Nr] VALUES (@Position,@Bestellung_Nr,@Artikel_Nr,@Bezeichnung_1,@Bezeichnung_2,@Einheit,@AnfangLagerBestand,@Anzahl,@Start_Anzahl,@Erhalten,@Aktuelle_Anzahl,@EndeLagerBestand,@Umsatzsteuer,@Einzelpreis,@Gesamtpreis,@Preisgruppe,@Bestellnummer,@Rabatt,@Rabatt1,@sortierung,@schriftart,@Preiseinheit,@Liefertermin,@erledigt_pos,@Lagerort_id,@Bestatigter_Termin,@Position_erledigt,@Bemerkung_Pos,@Bemerkung_Pos_ID,@Produktionsort,@BP_zu_RBposition,@WE_Pos_zu_Bestellposition,@AB_Nr_Lieferant,@RB_OriginalAnzahl,@RB_Abgerufen,@RB_Offen,@In_Bearbeitung,@Loschen,@Kanban,@MhdDatumArtikel,@COC_bestatigung,@InfoRahmennummer,@EMPB_Bestatigung,@CUPreis,@RA_Pos_zu_Bestellposition,@CocVersion,@LagerbewegungPositionId,@StandardSupplierViolation); SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Start_Anzahl", item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
					sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rabatt1", item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
					sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
					sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
					sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
					sqlCommand.Parameters.AddWithValue("Produktionsort", item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
					sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
					sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl", item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("RB_Abgerufen", item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
					sqlCommand.Parameters.AddWithValue("RB_Offen", item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
					sqlCommand.Parameters.AddWithValue("COC_bestatigung", item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
					sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
					sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung", item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
					sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
					sqlCommand.Parameters.AddWithValue("RA_Pos_zu_Bestellposition", item.RA_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.RA_Pos_zu_Bestellposition);
					sqlCommand.Parameters.AddWithValue("CocVersion", item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
					sqlCommand.Parameters.AddWithValue("LagerbewegungPositionId", item.LagerbewegungPositionId == null ? (object)DBNull.Value : item.LagerbewegungPositionId);
					sqlCommand.Parameters.AddWithValue("StandardSupplierViolation", item.StandardSupplierViolation == null ? (object)DBNull.Value : item.StandardSupplierViolation);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int insert(List<Bestellte_ArtikelEntity> items)
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
						query += " INSERT INTO [bestellte Artikel] ([Position],[Bestellung-Nr],[Artikel-Nr],[Bezeichnung 1],[Bezeichnung 2],[Einheit],[AnfangLagerBestand],[Anzahl],[Start Anzahl],[Erhalten],[Aktuelle Anzahl],[EndeLagerBestand],[Umsatzsteuer],[Einzelpreis],[Gesamtpreis],[Preisgruppe],[Bestellnummer],[Rabatt],[Rabatt1],[sortierung],[schriftart],[Preiseinheit],[Liefertermin],[erledigt_pos],[Lagerort_id],[Bestätigter_Termin],[Position erledigt],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Produktionsort],[BP zu RBposition],[WE Pos zu Bestellposition],[AB-Nr_Lieferant],[RB_OriginalAnzahl],[RB_Abgerufen],[RB_Offen],[In Bearbeitung],[Löschen],[Kanban],[MhdDatumArtikel],[COC_bestätigung],[InfoRahmennummer],[EMPB_Bestätigung],[CUPreis],[RA Pos zu Bestellposition],[CocVersion],[LagerbewegungPositionId],[StandardSupplierViolation]) VALUES ("
							+ "@Position" + i +
							 ","
							+ "@Bestellung_Nr" + i +
							 ","
							+ "@Artikel_Nr" + i +
							 ","
							+ "@Bezeichnung_1" + i +
							 ","
							+ "@Bezeichnung_2" + i +
							 ","
							+ "@Einheit" + i +
							 ","
							+ "@AnfangLagerBestand" + i +
							 ","
							+ "@Anzahl" + i +
							 ","
							+ "@Start_Anzahl" + i +
							 ","
							+ "@Erhalten" + i +
							 ","
							+ "@Aktuelle_Anzahl" + i +
							 ","
							+ "@EndeLagerBestand" + i +
							 ","
							+ "@Umsatzsteuer" + i +
							 ","
							+ "@Einzelpreis" + i +
							 ","
							+ "@Gesamtpreis" + i +
							 ","
							+ "@Preisgruppe" + i +
							 ","
							+ "@Bestellnummer" + i +
							 ","
							+ "@Rabatt" + i +
							 ","
							+ "@Rabatt1" + i +
							 ","
							+ "@sortierung" + i +
							 ","
							+ "@schriftart" + i +
							 ","
							+ "@Preiseinheit" + i +
							 ","
							+ "@Liefertermin" + i +
							 ","
							+ "@erledigt_pos" + i +
							 ","
							+ "@Lagerort_id" + i +
							 ","
							+ "@Bestatigter_Termin" + i +
							 ","
							+ "@Position_erledigt" + i +
							 ","
							+ "@Bemerkung_Pos" + i +
							 ","
							+ "@Bemerkung_Pos_ID" + i +
							 ","
							+ "@Produktionsort" + i +
							 ","
							+ "@BP_zu_RBposition" + i +
							 ","
							+ "@WE_Pos_zu_Bestellposition" + i +
							 ","
							+ "@AB_Nr_Lieferant" + i +
							 ","
							+ "@RB_OriginalAnzahl" + i +
							 ","
							+ "@RB_Abgerufen" + i +
							 ","
							+ "@RB_Offen" + i +
							 ","
							+ "@In_Bearbeitung" + i +
							 ","
							+ "@Loschen" + i +
							 ","
							+ "@Kanban" + i +
							 ","
							+ "@MhdDatumArtikel" + i +
							 ","
							+ "@COC_bestatigung" + i +
							 ","
							+ "@InfoRahmennummer" + i +
							 ","
							+ "@EMPB_Bestatigung" + i +
							 ","
							+ "@CUPreis" + i +
							 ","
							+ "@RA_Pos_zu_Bestellposition" + i +
							 ","
							+ "@CocVersion" + i +
							 ","
							+ "@LagerbewegungPositionId" + i +
							 ","
							+ "@StandardSupplierViolation" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Start_Anzahl" + i, item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
						sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
						sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rabatt1" + i, item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
						sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
						sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
						sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
						sqlCommand.Parameters.AddWithValue("Produktionsort" + i, item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
						sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
						sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl" + i, item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
						sqlCommand.Parameters.AddWithValue("RB_Abgerufen" + i, item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
						sqlCommand.Parameters.AddWithValue("RB_Offen" + i, item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
						sqlCommand.Parameters.AddWithValue("COC_bestatigung" + i, item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
						sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
						sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung" + i, item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
						sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
						sqlCommand.Parameters.AddWithValue("RA_Pos_zu_Bestellposition" + i, item.RA_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.RA_Pos_zu_Bestellposition);
						sqlCommand.Parameters.AddWithValue("CocVersion" + i, item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
						sqlCommand.Parameters.AddWithValue("LagerbewegungPositionId" + i, item.LagerbewegungPositionId == null ? (object)DBNull.Value : item.LagerbewegungPositionId);
						sqlCommand.Parameters.AddWithValue("StandardSupplierViolation" + i, item.StandardSupplierViolation == null ? (object)DBNull.Value : item.StandardSupplierViolation);
					}
					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}
				return results;
			}
			return -1;
		}
		public static int Insert(List<Bestellte_ArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 50;
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
		public static int Update(Bestellte_ArtikelEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "UPDATE [bestellte Artikel] SET [Position] = @Position, [Bestellung-Nr] = @Bestellung_Nr, [Artikel-Nr] = @Artikel_Nr, [Bezeichnung 1] = @Bezeichnung_1, [Bezeichnung 2] = @Bezeichnung_2, [Einheit] = @Einheit, [AnfangLagerBestand] = @AnfangLagerBestand, [Anzahl] = @Anzahl, [Start Anzahl] = @Start_Anzahl, [Erhalten] = @Erhalten, [Aktuelle Anzahl] = @Aktuelle_Anzahl, [EndeLagerBestand] = @EndeLagerBestand, [Umsatzsteuer] = @Umsatzsteuer, [Einzelpreis] = @Einzelpreis, [Gesamtpreis] = @Gesamtpreis, [Preisgruppe] = @Preisgruppe, [Bestellnummer] = @Bestellnummer, [Rabatt] = @Rabatt, [Rabatt1] = @Rabatt1, [sortierung] = @sortierung, [schriftart] = @schriftart, [Preiseinheit] = @Preiseinheit, [Liefertermin] = @Liefertermin, [erledigt_pos] = @erledigt_pos, [Lagerort_id] = @Lagerort_id, [Bestätigter_Termin] = @Bestatigter_Termin, [Position erledigt] = @Position_erledigt, [Bemerkung_Pos] = @Bemerkung_Pos, [Bemerkung_Pos_ID] = @Bemerkung_Pos_ID, [Produktionsort] = @Produktionsort, [BP zu RBposition] = @BP_zu_RBposition, [WE Pos zu Bestellposition] = @WE_Pos_zu_Bestellposition, [AB-Nr_Lieferant] = @AB_Nr_Lieferant, [RB_OriginalAnzahl] = @RB_OriginalAnzahl, [RB_Abgerufen] = @RB_Abgerufen, [RB_Offen] = @RB_Offen, [In Bearbeitung] = @In_Bearbeitung, [Löschen] = @Loschen, [Kanban] = @Kanban, [MhdDatumArtikel] = @MhdDatumArtikel, [COC_bestätigung] = @COC_bestatigung, [InfoRahmennummer] = @InfoRahmennummer, [EMPB_Bestätigung] = @EMPB_Bestatigung, [CUPreis] = @CUPreis, [RA Pos zu Bestellposition] = @RA_Pos_zu_Bestellposition, [CocVersion] = @CocVersion, [LagerbewegungPositionId] = @LagerbewegungPositionId, [StandardSupplierViolation] = @StandardSupplierViolation WHERE [Nr] = @Nr";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Start_Anzahl", item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
					sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rabatt1", item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
					sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
					sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
					sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
					sqlCommand.Parameters.AddWithValue("Produktionsort", item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
					sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
					sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl", item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("RB_Abgerufen", item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
					sqlCommand.Parameters.AddWithValue("RB_Offen", item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
					sqlCommand.Parameters.AddWithValue("COC_bestatigung", item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
					sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
					sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung", item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
					sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
					sqlCommand.Parameters.AddWithValue("RA_Pos_zu_Bestellposition", item.RA_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.RA_Pos_zu_Bestellposition);
					sqlCommand.Parameters.AddWithValue("CocVersion", item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
					sqlCommand.Parameters.AddWithValue("LagerbewegungPositionId", item.LagerbewegungPositionId == null ? (object)DBNull.Value : item.LagerbewegungPositionId);
					sqlCommand.Parameters.AddWithValue("StandardSupplierViolation", item.StandardSupplierViolation == null ? (object)DBNull.Value : item.StandardSupplierViolation);

					int rowsAffected = DbExecution.ExecuteNonQuery(sqlCommand);
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int update(List<Bestellte_ArtikelEntity> items)
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
						query += " UPDATE [bestellte Artikel] SET "
						  + "[Position]=@Position" + i +
						   ","
						  + "[Bestellung-Nr]=@Bestellung_Nr" + i +
						   ","
						  + "[Artikel-Nr]=@Artikel_Nr" + i +
						   ","
						  + "[Bezeichnung 1]=@Bezeichnung_1" + i +
						   ","
						  + "[Bezeichnung 2]=@Bezeichnung_2" + i +
						   ","
						  + "[Einheit]=@Einheit" + i +
						   ","
						  + "[AnfangLagerBestand]=@AnfangLagerBestand" + i +
						   ","
						  + "[Anzahl]=@Anzahl" + i +
						   ","
						  + "[Start Anzahl]=@Start_Anzahl" + i +
						   ","
						  + "[Erhalten]=@Erhalten" + i +
						   ","
						  + "[Aktuelle Anzahl]=@Aktuelle_Anzahl" + i +
						   ","
						  + "[EndeLagerBestand]=@EndeLagerBestand" + i +
						   ","
						  + "[Umsatzsteuer]=@Umsatzsteuer" + i +
						   ","
						  + "[Einzelpreis]=@Einzelpreis" + i +
						   ","
						  + "[Gesamtpreis]=@Gesamtpreis" + i +
						   ","
						  + "[Preisgruppe]=@Preisgruppe" + i +
						   ","
						  + "[Bestellnummer]=@Bestellnummer" + i +
						   ","
						  + "[Rabatt]=@Rabatt" + i +
						   ","
						  + "[Rabatt1]=@Rabatt1" + i +
						   ","
						  + "[sortierung]=@sortierung" + i +
						   ","
						  + "[schriftart]=@schriftart" + i +
						   ","
						  + "[Preiseinheit]=@Preiseinheit" + i +
						   ","
						  + "[Liefertermin]=@Liefertermin" + i +
						   ","
						  + "[erledigt_pos]=@erledigt_pos" + i +
						   ","
						  + "[Lagerort_id]=@Lagerort_id" + i +
						   ","
						  + "[Bestätigter_Termin]=@Bestatigter_Termin" + i +
						   ","
						  + "[Position erledigt]=@Position_erledigt" + i +
						   ","
						  + "[Bemerkung_Pos]=@Bemerkung_Pos" + i +
						   ","
						  + "[Bemerkung_Pos_ID]=@Bemerkung_Pos_ID" + i +
						   ","
						  + "[Produktionsort]=@Produktionsort" + i +
						   ","
						  + "[BP zu RBposition]=@BP_zu_RBposition" + i +
						   ","
						  + "[WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition" + i +
						   ","
						  + "[AB-Nr_Lieferant]=@AB_Nr_Lieferant" + i +
						   ","
						  + "[RB_OriginalAnzahl]=@RB_OriginalAnzahl" + i +
						   ","
						  + "[RB_Abgerufen]=@RB_Abgerufen" + i +
						   ","
						  + "[RB_Offen]=@RB_Offen" + i +
						   ","
						  + "[In Bearbeitung]=@In_Bearbeitung" + i +
						   ","
						  + "[Löschen]=@Loschen" + i +
						   ","
						  + "[Kanban]=@Kanban" + i +
						   ","
						  + "[MhdDatumArtikel]=@MhdDatumArtikel" + i +
						   ","
						  + "[COC_bestätigung]=@COC_bestatigung" + i +
						   ","
						  + "[InfoRahmennummer]=@InfoRahmennummer" + i +
						   ","
						  + "[EMPB_Bestätigung]=@EMPB_Bestatigung" + i +
						   ","
						  + "[CUPreis]=@CUPreis" + i +
						   ","
						  + "[RA Pos zu Bestellposition]=@RA_Pos_zu_Bestellposition" + i +
						   ","
						  + "[CocVersion]=@CocVersion" + i +
						   ","
						  + "[LagerbewegungPositionId]=@LagerbewegungPositionId" + i +
						   ","
						  + "[StandardSupplierViolation]=@StandardSupplierViolation" + i +
						 " WHERE [Nr]=@Nr" + i
							+ "; ";
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Start_Anzahl" + i, item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
						sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
						sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rabatt1" + i, item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
						sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
						sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
						sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
						sqlCommand.Parameters.AddWithValue("Produktionsort" + i, item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
						sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
						sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl" + i, item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
						sqlCommand.Parameters.AddWithValue("RB_Abgerufen" + i, item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
						sqlCommand.Parameters.AddWithValue("RB_Offen" + i, item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
						sqlCommand.Parameters.AddWithValue("COC_bestatigung" + i, item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
						sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
						sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung" + i, item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
						sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
						sqlCommand.Parameters.AddWithValue("RA_Pos_zu_Bestellposition" + i, item.RA_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.RA_Pos_zu_Bestellposition);
						sqlCommand.Parameters.AddWithValue("CocVersion" + i, item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
						sqlCommand.Parameters.AddWithValue("LagerbewegungPositionId" + i, item.LagerbewegungPositionId == null ? (object)DBNull.Value : item.LagerbewegungPositionId);
						sqlCommand.Parameters.AddWithValue("StandardSupplierViolation" + i, item.StandardSupplierViolation == null ? (object)DBNull.Value : item.StandardSupplierViolation);
						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int Update(List<Bestellte_ArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 50;
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
		public static int Delete(int id)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "DELETE FROM [bestellte Artikel] WHERE [Nr] = @Nr";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Nr", id);

					int rowsAffected = DbExecution.ExecuteNonQuery(sqlCommand);
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int delete(List<int> ids)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string queryIds = string.Join(",", Enumerable.Range(0, ids.Count).Select(i => "@Nr" + i));
				string query = "DELETE FROM [bestellte Artikel] WHERE [Nr] IN (" + queryIds + ")";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					for(int i = 0; i < ids.Count; i++)
					{
						sqlCommand.Parameters.AddWithValue("Nr" + i, ids[i]);
					}

					int rowsAffected = DbExecution.ExecuteNonQuery(sqlCommand);
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
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

				return results;
			}
			else
			{
				return -1;
			}
		}
		#region Transaction Methods
		public static Bestellte_ArtikelEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [bestellte Artikel] WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", id);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
				return new Bestellte_ArtikelEntity(dataTable.Rows[0]);

			else
				return null;
		}
		public static List<Bestellte_ArtikelEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [bestellte Artikel]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
				return dataTable.Rows.Cast<DataRow>().Select(x => new Bestellte_ArtikelEntity(x)).ToList();
			else
				return new List<Bestellte_ArtikelEntity>();
		}
		public static List<Bestellte_ArtikelEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;

				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Nr" + i + ",";
					sqlCommand.Parameters.AddWithValue("Nr" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = "SELECT * FROM [bestellte Artikel] WHERE [Nr] IN (" + queryIds + ")";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
					return dataTable.Rows.Cast<DataRow>().Select(x => new Bestellte_ArtikelEntity(x)).ToList();
				else
					return new List<Bestellte_ArtikelEntity>();
			}
			return new List<Bestellte_ArtikelEntity>();
		}
		public static List<Bestellte_ArtikelEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Bestellte_ArtikelEntity> results = null;

				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Bestellte_ArtikelEntity>();

					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}

					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}

				return results;
			}
			return new List<Bestellte_ArtikelEntity>();
		}
		public static int InsertWithTransaction(Bestellte_ArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;
			string query = "INSERT INTO [bestellte Artikel] ([Position],[Bestellung-Nr],[Artikel-Nr],[Bezeichnung 1],[Bezeichnung 2],[Einheit],[AnfangLagerBestand],[Anzahl],[Start Anzahl],[Erhalten],[Aktuelle Anzahl],[EndeLagerBestand],[Umsatzsteuer],[Einzelpreis],[Gesamtpreis],[Preisgruppe],[Bestellnummer],[Rabatt],[Rabatt1],[sortierung],[schriftart],[Preiseinheit],[Liefertermin],[erledigt_pos],[Lagerort_id],[Bestätigter_Termin],[Position erledigt],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Produktionsort],[BP zu RBposition],[WE Pos zu Bestellposition],[AB-Nr_Lieferant],[RB_OriginalAnzahl],[RB_Abgerufen],[RB_Offen],[In Bearbeitung],[Löschen],[Kanban],[MhdDatumArtikel],[COC_bestätigung],[InfoRahmennummer],[EMPB_Bestätigung],[CUPreis],[RA Pos zu Bestellposition],[CocVersion],[LagerbewegungPositionId],[StandardSupplierViolation]) OUTPUT INSERTED.[Nr] VALUES (@Position,@Bestellung_Nr,@Artikel_Nr,@Bezeichnung_1,@Bezeichnung_2,@Einheit,@AnfangLagerBestand,@Anzahl,@Start_Anzahl,@Erhalten,@Aktuelle_Anzahl,@EndeLagerBestand,@Umsatzsteuer,@Einzelpreis,@Gesamtpreis,@Preisgruppe,@Bestellnummer,@Rabatt,@Rabatt1,@sortierung,@schriftart,@Preiseinheit,@Liefertermin,@erledigt_pos,@Lagerort_id,@Bestatigter_Termin,@Position_erledigt,@Bemerkung_Pos,@Bemerkung_Pos_ID,@Produktionsort,@BP_zu_RBposition,@WE_Pos_zu_Bestellposition,@AB_Nr_Lieferant,@RB_OriginalAnzahl,@RB_Abgerufen,@RB_Offen,@In_Bearbeitung,@Loschen,@Kanban,@MhdDatumArtikel,@COC_bestatigung,@InfoRahmennummer,@EMPB_Bestatigung,@CUPreis,@RA_Pos_zu_Bestellposition,@CocVersion,@LagerbewegungPositionId,@StandardSupplierViolation); SELECT SCOPE_IDENTITY();";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
				sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
				sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Start_Anzahl", item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
				sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
				sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
				sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
				sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
				sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
				sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
				sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
				sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
				sqlCommand.Parameters.AddWithValue("Rabatt1", item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
				sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
				sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
				sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
				sqlCommand.Parameters.AddWithValue("Produktionsort", item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
				sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
				sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
				sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
				sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl", item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
				sqlCommand.Parameters.AddWithValue("RB_Abgerufen", item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
				sqlCommand.Parameters.AddWithValue("RB_Offen", item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
				sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
				sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
				sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
				sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
				sqlCommand.Parameters.AddWithValue("COC_bestatigung", item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
				sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
				sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung", item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
				sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
				sqlCommand.Parameters.AddWithValue("RA_Pos_zu_Bestellposition", item.RA_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.RA_Pos_zu_Bestellposition);
				sqlCommand.Parameters.AddWithValue("CocVersion", item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
				sqlCommand.Parameters.AddWithValue("LagerbewegungPositionId", item.LagerbewegungPositionId == null ? (object)DBNull.Value : item.LagerbewegungPositionId);
				sqlCommand.Parameters.AddWithValue("StandardSupplierViolation", item.StandardSupplierViolation == null ? (object)DBNull.Value : item.StandardSupplierViolation);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
			}

			return response;
		}
		public static int insertWithTransaction(List<Bestellte_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [bestellte Artikel] ([Position],[Bestellung-Nr],[Artikel-Nr],[Bezeichnung 1],[Bezeichnung 2],[Einheit],[AnfangLagerBestand],[Anzahl],[Start Anzahl],[Erhalten],[Aktuelle Anzahl],[EndeLagerBestand],[Umsatzsteuer],[Einzelpreis],[Gesamtpreis],[Preisgruppe],[Bestellnummer],[Rabatt],[Rabatt1],[sortierung],[schriftart],[Preiseinheit],[Liefertermin],[erledigt_pos],[Lagerort_id],[Bestätigter_Termin],[Position erledigt],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Produktionsort],[BP zu RBposition],[WE Pos zu Bestellposition],[AB-Nr_Lieferant],[RB_OriginalAnzahl],[RB_Abgerufen],[RB_Offen],[In Bearbeitung],[Löschen],[Kanban],[MhdDatumArtikel],[COC_bestätigung],[InfoRahmennummer],[EMPB_Bestätigung],[CUPreis],[RA Pos zu Bestellposition],[CocVersion],[LagerbewegungPositionId],[StandardSupplierViolation]) VALUES ("
							+ "@Position" + i +
							 ","
							+ "@Bestellung_Nr" + i +
							 ","
							+ "@Artikel_Nr" + i +
							 ","
							+ "@Bezeichnung_1" + i +
							 ","
							+ "@Bezeichnung_2" + i +
							 ","
							+ "@Einheit" + i +
							 ","
							+ "@AnfangLagerBestand" + i +
							 ","
							+ "@Anzahl" + i +
							 ","
							+ "@Start_Anzahl" + i +
							 ","
							+ "@Erhalten" + i +
							 ","
							+ "@Aktuelle_Anzahl" + i +
							 ","
							+ "@EndeLagerBestand" + i +
							 ","
							+ "@Umsatzsteuer" + i +
							 ","
							+ "@Einzelpreis" + i +
							 ","
							+ "@Gesamtpreis" + i +
							 ","
							+ "@Preisgruppe" + i +
							 ","
							+ "@Bestellnummer" + i +
							 ","
							+ "@Rabatt" + i +
							 ","
							+ "@Rabatt1" + i +
							 ","
							+ "@sortierung" + i +
							 ","
							+ "@schriftart" + i +
							 ","
							+ "@Preiseinheit" + i +
							 ","
							+ "@Liefertermin" + i +
							 ","
							+ "@erledigt_pos" + i +
							 ","
							+ "@Lagerort_id" + i +
							 ","
							+ "@Bestatigter_Termin" + i +
							 ","
							+ "@Position_erledigt" + i +
							 ","
							+ "@Bemerkung_Pos" + i +
							 ","
							+ "@Bemerkung_Pos_ID" + i +
							 ","
							+ "@Produktionsort" + i +
							 ","
							+ "@BP_zu_RBposition" + i +
							 ","
							+ "@WE_Pos_zu_Bestellposition" + i +
							 ","
							+ "@AB_Nr_Lieferant" + i +
							 ","
							+ "@RB_OriginalAnzahl" + i +
							 ","
							+ "@RB_Abgerufen" + i +
							 ","
							+ "@RB_Offen" + i +
							 ","
							+ "@In_Bearbeitung" + i +
							 ","
							+ "@Loschen" + i +
							 ","
							+ "@Kanban" + i +
							 ","
							+ "@MhdDatumArtikel" + i +
							 ","
							+ "@COC_bestatigung" + i +
							 ","
							+ "@InfoRahmennummer" + i +
							 ","
							+ "@EMPB_Bestatigung" + i +
							 ","
							+ "@CUPreis" + i +
							 ","
							+ "@RA_Pos_zu_Bestellposition" + i +
							 ","
							+ "@CocVersion" + i +
							 ","
							+ "@LagerbewegungPositionId" + i +
							 ","
							+ "@StandardSupplierViolation" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Start_Anzahl" + i, item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
						sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
						sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rabatt1" + i, item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
						sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
						sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
						sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
						sqlCommand.Parameters.AddWithValue("Produktionsort" + i, item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
						sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
						sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl" + i, item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
						sqlCommand.Parameters.AddWithValue("RB_Abgerufen" + i, item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
						sqlCommand.Parameters.AddWithValue("RB_Offen" + i, item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
						sqlCommand.Parameters.AddWithValue("COC_bestatigung" + i, item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
						sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
						sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung" + i, item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
						sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
						sqlCommand.Parameters.AddWithValue("RA_Pos_zu_Bestellposition" + i, item.RA_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.RA_Pos_zu_Bestellposition);
						sqlCommand.Parameters.AddWithValue("CocVersion" + i, item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
						sqlCommand.Parameters.AddWithValue("LagerbewegungPositionId" + i, item.LagerbewegungPositionId == null ? (object)DBNull.Value : item.LagerbewegungPositionId);
						sqlCommand.Parameters.AddWithValue("StandardSupplierViolation" + i, item.StandardSupplierViolation == null ? (object)DBNull.Value : item.StandardSupplierViolation);
					}
					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}
				return results;
			}
			return -1;
		}
		public static int InsertWithTransaction(List<Bestellte_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					results = 0;
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
		public static int UpdateWithTransaction(Bestellte_ArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [bestellte Artikel] SET [Position] = @Position, [Bestellung-Nr] = @Bestellung_Nr, [Artikel-Nr] = @Artikel_Nr, [Bezeichnung 1] = @Bezeichnung_1, [Bezeichnung 2] = @Bezeichnung_2, [Einheit] = @Einheit, [AnfangLagerBestand] = @AnfangLagerBestand, [Anzahl] = @Anzahl, [Start Anzahl] = @Start_Anzahl, [Erhalten] = @Erhalten, [Aktuelle Anzahl] = @Aktuelle_Anzahl, [EndeLagerBestand] = @EndeLagerBestand, [Umsatzsteuer] = @Umsatzsteuer, [Einzelpreis] = @Einzelpreis, [Gesamtpreis] = @Gesamtpreis, [Preisgruppe] = @Preisgruppe, [Bestellnummer] = @Bestellnummer, [Rabatt] = @Rabatt, [Rabatt1] = @Rabatt1, [sortierung] = @sortierung, [schriftart] = @schriftart, [Preiseinheit] = @Preiseinheit, [Liefertermin] = @Liefertermin, [erledigt_pos] = @erledigt_pos, [Lagerort_id] = @Lagerort_id, [Bestätigter_Termin] = @Bestatigter_Termin, [Position erledigt] = @Position_erledigt, [Bemerkung_Pos] = @Bemerkung_Pos, [Bemerkung_Pos_ID] = @Bemerkung_Pos_ID, [Produktionsort] = @Produktionsort, [BP zu RBposition] = @BP_zu_RBposition, [WE Pos zu Bestellposition] = @WE_Pos_zu_Bestellposition, [AB-Nr_Lieferant] = @AB_Nr_Lieferant, [RB_OriginalAnzahl] = @RB_OriginalAnzahl, [RB_Abgerufen] = @RB_Abgerufen, [RB_Offen] = @RB_Offen, [In Bearbeitung] = @In_Bearbeitung, [Löschen] = @Loschen, [Kanban] = @Kanban, [MhdDatumArtikel] = @MhdDatumArtikel, [COC_bestätigung] = @COC_bestatigung, [InfoRahmennummer] = @InfoRahmennummer, [EMPB_Bestätigung] = @EMPB_Bestatigung, [CUPreis] = @CUPreis, [RA Pos zu Bestellposition] = @RA_Pos_zu_Bestellposition, [CocVersion] = @CocVersion, [LagerbewegungPositionId] = @LagerbewegungPositionId, [StandardSupplierViolation] = @StandardSupplierViolation WHERE [Nr] = @Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Start_Anzahl", item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
			sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
			sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
			sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
			sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
			sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("Rabatt1", item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
			sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
			sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
			sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
			sqlCommand.Parameters.AddWithValue("Produktionsort", item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
			sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
			sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
			sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
			sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl", item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
			sqlCommand.Parameters.AddWithValue("RB_Abgerufen", item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
			sqlCommand.Parameters.AddWithValue("RB_Offen", item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
			sqlCommand.Parameters.AddWithValue("COC_bestatigung", item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
			sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
			sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung", item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
			sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
			sqlCommand.Parameters.AddWithValue("RA_Pos_zu_Bestellposition", item.RA_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.RA_Pos_zu_Bestellposition);
			sqlCommand.Parameters.AddWithValue("CocVersion", item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
			sqlCommand.Parameters.AddWithValue("LagerbewegungPositionId", item.LagerbewegungPositionId == null ? (object)DBNull.Value : item.LagerbewegungPositionId);
			sqlCommand.Parameters.AddWithValue("StandardSupplierViolation", item.StandardSupplierViolation == null ? (object)DBNull.Value : item.StandardSupplierViolation);
			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int updateWithTransaction(List<Bestellte_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [bestellte Artikel] SET "
					  + "[Position]=@Position" + i +
					   ","
					  + "[Bestellung-Nr]=@Bestellung_Nr" + i +
					   ","
					  + "[Artikel-Nr]=@Artikel_Nr" + i +
					   ","
					  + "[Bezeichnung 1]=@Bezeichnung_1" + i +
					   ","
					  + "[Bezeichnung 2]=@Bezeichnung_2" + i +
					   ","
					  + "[Einheit]=@Einheit" + i +
					   ","
					  + "[AnfangLagerBestand]=@AnfangLagerBestand" + i +
					   ","
					  + "[Anzahl]=@Anzahl" + i +
					   ","
					  + "[Start Anzahl]=@Start_Anzahl" + i +
					   ","
					  + "[Erhalten]=@Erhalten" + i +
					   ","
					  + "[Aktuelle Anzahl]=@Aktuelle_Anzahl" + i +
					   ","
					  + "[EndeLagerBestand]=@EndeLagerBestand" + i +
					   ","
					  + "[Umsatzsteuer]=@Umsatzsteuer" + i +
					   ","
					  + "[Einzelpreis]=@Einzelpreis" + i +
					   ","
					  + "[Gesamtpreis]=@Gesamtpreis" + i +
					   ","
					  + "[Preisgruppe]=@Preisgruppe" + i +
					   ","
					  + "[Bestellnummer]=@Bestellnummer" + i +
					   ","
					  + "[Rabatt]=@Rabatt" + i +
					   ","
					  + "[Rabatt1]=@Rabatt1" + i +
					   ","
					  + "[sortierung]=@sortierung" + i +
					   ","
					  + "[schriftart]=@schriftart" + i +
					   ","
					  + "[Preiseinheit]=@Preiseinheit" + i +
					   ","
					  + "[Liefertermin]=@Liefertermin" + i +
					   ","
					  + "[erledigt_pos]=@erledigt_pos" + i +
					   ","
					  + "[Lagerort_id]=@Lagerort_id" + i +
					   ","
					  + "[Bestätigter_Termin]=@Bestatigter_Termin" + i +
					   ","
					  + "[Position erledigt]=@Position_erledigt" + i +
					   ","
					  + "[Bemerkung_Pos]=@Bemerkung_Pos" + i +
					   ","
					  + "[Bemerkung_Pos_ID]=@Bemerkung_Pos_ID" + i +
					   ","
					  + "[Produktionsort]=@Produktionsort" + i +
					   ","
					  + "[BP zu RBposition]=@BP_zu_RBposition" + i +
					   ","
					  + "[WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition" + i +
					   ","
					  + "[AB-Nr_Lieferant]=@AB_Nr_Lieferant" + i +
					   ","
					  + "[RB_OriginalAnzahl]=@RB_OriginalAnzahl" + i +
					   ","
					  + "[RB_Abgerufen]=@RB_Abgerufen" + i +
					   ","
					  + "[RB_Offen]=@RB_Offen" + i +
					   ","
					  + "[In Bearbeitung]=@In_Bearbeitung" + i +
					   ","
					  + "[Löschen]=@Loschen" + i +
					   ","
					  + "[Kanban]=@Kanban" + i +
					   ","
					  + "[MhdDatumArtikel]=@MhdDatumArtikel" + i +
					   ","
					  + "[COC_bestätigung]=@COC_bestatigung" + i +
					   ","
					  + "[InfoRahmennummer]=@InfoRahmennummer" + i +
					   ","
					  + "[EMPB_Bestätigung]=@EMPB_Bestatigung" + i +
					   ","
					  + "[CUPreis]=@CUPreis" + i +
					   ","
					  + "[RA Pos zu Bestellposition]=@RA_Pos_zu_Bestellposition" + i +
					   ","
					  + "[CocVersion]=@CocVersion" + i +
					   ","
					  + "[LagerbewegungPositionId]=@LagerbewegungPositionId" + i +
					   ","
					  + "[StandardSupplierViolation]=@StandardSupplierViolation" + i +
					 " WHERE [Nr]=@Nr" + i
						+ "; ";
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Start_Anzahl" + i, item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
					sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rabatt1" + i, item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
					sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
					sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
					sqlCommand.Parameters.AddWithValue("Produktionsort" + i, item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
					sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
					sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl" + i, item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("RB_Abgerufen" + i, item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
					sqlCommand.Parameters.AddWithValue("RB_Offen" + i, item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
					sqlCommand.Parameters.AddWithValue("COC_bestatigung" + i, item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
					sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
					sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung" + i, item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
					sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
					sqlCommand.Parameters.AddWithValue("RA_Pos_zu_Bestellposition" + i, item.RA_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.RA_Pos_zu_Bestellposition);
					sqlCommand.Parameters.AddWithValue("CocVersion" + i, item.CocVersion == null ? (object)DBNull.Value : item.CocVersion);
					sqlCommand.Parameters.AddWithValue("LagerbewegungPositionId" + i, item.LagerbewegungPositionId == null ? (object)DBNull.Value : item.LagerbewegungPositionId);
					sqlCommand.Parameters.AddWithValue("StandardSupplierViolation" + i, item.StandardSupplierViolation == null ? (object)DBNull.Value : item.StandardSupplierViolation);
					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
				}
				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return -1;
		}
		public static int UpdateWithTransaction(List<Bestellte_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					results = 0;
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
		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "DELETE FROM [bestellte Artikel] WHERE [Nr] = @Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", id);
			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Join(",", ids.Select((id, i) => "@Nr" + i));
				sqlCommand.CommandText = $"DELETE FROM [bestellte Artikel] WHERE [Nr] IN (" + queryIds + ")";
				for(int i = 0; i < ids.Count; i++)
				{
					sqlCommand.Parameters.AddWithValue("Nr" + i, ids[i]);
				}
				results = DbExecution.ExecuteNonQuery(sqlCommand);
				return results;
			}
			return -1;
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
					results = 0;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}
			return -1;
		}
		#endregion Transaction Methods
		#endregion Default Methods
		public static List<Entities.Tables.MTM.Bestellte_ArtikelEntity> GetByOrderId(int BestellungId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellte Artikel]" +
					"WHERE [Bestellung-Nr]=@BestellungId " +
					"ORDER BY [bestellte Artikel].sortierung, [bestellte Artikel].Nr;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("BestellungId", BestellungId);


				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.MTM.Bestellte_ArtikelEntity>();
			}
		}
		public static List<Entities.Tables.MTM.Bestellte_ArtikelEntity> GetByBestellungNr(int BestellungNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT ba.* FROM [Bestellte Artikel] ba
			Join Bestellungen b on b.Nr = ba.[Bestellung-Nr]
			WHERE b.[Bestellung-Nr]= @BestellungId 
		ORDER BY ba.sortierung,ba.Nr;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("BestellungId", BestellungNr);


				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.MTM.Bestellte_ArtikelEntity>();
			}
		}
		public static List<Entities.Tables.MTM.Bestellte_ArtikelEntity> GetByOrderId(int BestellungId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Bestellte Artikel]" +
					"WHERE [Bestellung-Nr]=@BestellungId " +
					"ORDER BY [bestellte Artikel].sortierung, [bestellte Artikel].Nr;";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("BestellungId", BestellungId);


			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.MTM.Bestellte_ArtikelEntity>();
			}
		}
		public static int GetByOrderIdCount(int BestellungId)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT count([Bestellung-Nr]) FROM [Bestellte Artikel]" +
					"WHERE [Bestellung-Nr]=@BestellungId ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("BestellungId", BestellungId);


				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}
		public static Dictionary<int, int> GetCountWE(List<int> bestellteArtikelNrs)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"select count(*) as CountWE, b.Nr from  [bestellte Artikel] b
						join (select [WE Pos zu Bestellposition] from  [bestellte Artikel] WHERE [WE Pos zu Bestellposition] in ({string.Join(',', bestellteArtikelNrs)}) ) a on a.[WE Pos zu Bestellposition] = b.Nr
						Group by Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			Dictionary<int, int> result = new Dictionary<int, int>();

			if(dataTable.Rows.Count > 0)
			{

				dataTable.Rows.Cast<DataRow>().ToList().ForEach(x => result.Add(Convert.ToInt32(x[1].ToString()), Convert.ToInt32(x[0].ToString())));
				return result;
			}
			else
			{
				return result;
			}
		}

		public static List<Entities.Tables.MTM.Bestellte_ArtikelEntity> GetByOrderIdPaged(int BestellungId, Settings.SortingModel sorting,
			Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string clause = "";
				string query = "SELECT * FROM [Bestellte Artikel]" +
					"WHERE [Bestellung-Nr]=@BestellungId ";

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					clause += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					clause += " ORDER BY [bestellte Artikel].sortierung, [bestellte Artikel].Nr ";
				}

				if(paging != null)
				{
					clause += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}

				query += clause;


				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("BestellungId", BestellungId);


				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.MTM.Bestellte_ArtikelEntity>();
			}
		}
		public static List<Entities.Tables.MTM.ArtikelEntity> GetPaginatedFiltered(Settings.SortingModel sorting,
			Settings.PaginModel paging,
			List<Settings.FilterModel> filters, List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.MTM.ArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getPaginatedFiltered(sorting, paging, filters, ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.MTM.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getPaginatedFiltered(sorting, paging, filters, ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getPaginatedFiltered(sorting, paging, filters, ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Entities.Tables.MTM.ArtikelEntity>();
		}
		private static List<Entities.Tables.MTM.ArtikelEntity> getPaginatedFiltered(Settings.SortingModel sorting,
			Settings.PaginModel paging,
			List<Settings.FilterModel> filters,
			List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Artikel] WHERE [Artikel-Nr] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.MTM.ArtikelEntity>();
				}
			}
			return new List<Entities.Tables.MTM.ArtikelEntity>();
		}

		public static Entities.Tables.MTM.Bestellte_ArtikelEntity GetByOrderIdArtikelId(int BestellungId, int ArtikelId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellte Artikel]" +
					"WHERE [Bestellung-Nr]=@BestellungId AND [Artikel-Nr]=@ArtikelId ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("BestellungId", BestellungId);
				sqlCommand.Parameters.AddWithValue("ArtikelId", ArtikelId);


				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.MTM.Bestellte_ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, int>> GetOrderPositions(int BestellungId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT Nr, Position FROM [Bestellte Artikel] WHERE [Bestellung-Nr]=@BestellungId;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("BestellungId", BestellungId);


				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, int>(int.TryParse(x[0]?.ToString(), out var _x) ? _x : 0, int.TryParse(x[1]?.ToString(), out var _y) ? _y : 0)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int GetOrderNextPosition(int BestellungId)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT 10 * (1+(CAST(IsNULL(MAX(Position),0) AS int) / 10)) Position FROM [Bestellte Artikel] WHERE [Bestellung-Nr]=@BestellungId;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("BestellungId", BestellungId);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var _x) ? _x : 10;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity> GetByBestellungen(List<int> bestellungNrs)
		{
			if(bestellungNrs?.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Bestellte Artikel] WHERE [Bestellung-Nr] IN ({string.Join(",", bestellungNrs)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity>();
			}
		}
		public static int UpdatePos(List<Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity> items)
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
						query += " UPDATE [Bestellte Artikel] SET [Position]=@Position" + i + " WHERE [Nr]=@Nr" + i + "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static List<Entities.Tables.MTM.Bestellte_ArtikelEntity> GetbyRahmenPositions(List<int> RAposIds)
		{
			try
			{
				if(RAposIds == null || RAposIds.Count == 0)
					return null;
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dt = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $"SELECT * FROM [bestellte Artikel] WHERE [RA Pos zu Bestellposition] IN ({string.Join(",", RAposIds)})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.Bestellte_ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.MTM.Bestellte_ArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<Entities.Tables.MTM.Bestellte_ArtikelEntity> GetByBlanket(string blanketNr)
		{
			if(string.IsNullOrWhiteSpace(blanketNr))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellte Artikel] WHERE [InfoRahmennummer]=@blanketNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("blanketNr", blanketNr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.MTM.Bestellte_ArtikelEntity>();
			}
		}
		public static int UpdateConfirmationDate(int id, DateTime confirmationDate, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Bestellte Artikel] SET [Bestätigter_Termin]=@Bestatigter_Termin WHERE [Nr]=@Nr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Nr", id);
				sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", confirmationDate);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int UpdateTransferedQuantityWithTransaction(int Nr, decimal? deletedVar, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			return results;
		}
		public static int UpdateStandardSupplierViolation(int Nr, bool violation, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = @"UPDATE [Bestellte Artikel] SET [StandardSupplierViolation]=@violation WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", Nr);
			sqlCommand.Parameters.AddWithValue("violation", violation);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateStandardSupplierViolation(List<KeyValuePair< int,bool>> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items == null || items.Count <= 0)
				return 0;

			string query = string.Join("; ", items.Select(x => $"UPDATE [Bestellte Artikel] SET [StandardSupplierViolation]={(x.Value ? "1": "0")} WHERE [Nr]={x.Key}"));
			var sqlCommand = new SqlCommand(query, connection, transaction);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}

		public static decimal GetTransferQuantity(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT 0 FROM [Bestellte Artikel] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return (dataTable.Rows[0][0] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}
		#region Custom Methods
		#endregion Custom Methods
	}
}
