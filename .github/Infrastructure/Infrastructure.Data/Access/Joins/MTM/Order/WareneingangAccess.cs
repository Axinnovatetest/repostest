using Infrastructure.Data.Entities.Joins.MTM.Order;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class WareneingangAccess
	{


		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.WareneingangEntity> GetWareneingangEntitiesByBestellungNr(int orderId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query =
					@$"SELECT 
					Bestellungen.[Bestellung-Nr],
					Bestellungen.Nr As BestellungenNr,
					Bestellungen.Typ, 
					Bestellungen.Mandant, 
					Bestellungen.Eingangslieferscheinnr,
					Bestellungen.[Vorname/NameFirma],
					[bestellte Artikel].Position, 
					[bestellte Artikel].Liefertermin, 
					[bestellte Artikel].Bestätigter_Termin, 
					[bestellte Artikel].Nr, 
					[bestellte Artikel].[WE Pos zu Bestellposition], 
					[bestellte Artikel].erledigt_pos, 
					[bestellte Artikel].[Artikel-Nr], 
					[bestellte Artikel].[Bezeichnung 1], 
					[bestellte Artikel].Einheit, 
					[bestellte Artikel].AnfangLagerBestand, 
					[bestellte Artikel].Anzahl, 
					[bestellte Artikel].[Start Anzahl],
					[bestellte Artikel].Erhalten, 
					[bestellte Artikel].[Aktuelle Anzahl], 
					[bestellte Artikel].EndeLagerBestand, 
					[bestellte Artikel].Bestellnummer, 
					[bestellte Artikel].Lagerort_id, 
					[bestellte Artikel].MhdDatumArtikel, 
					[bestellte Artikel].COC_bestätigung, 
					[bestellte Artikel].EMPB_Bestätigung,
					[bestellte Artikel].Lagerort_id AS Original_IDOrt,
					[bestellte Artikel].CocVersion,
					Artikel.ESD_Schutz,
					CAST(Artikel.Größe AS DECIMAL(38,15)) AS Größe, 
					Artikel.EMPB, 
					Artikel.EMPB_Freigegeben, 
					Artikel.MHD, 
					Artikel.COF_Pflichtig, 
					Artikel.Zeitraum_MHD,
					Artikel.Artikelnummer,
					Lagerorte.Lagerort, 
					0 AS Erstellt,
					0 AS Id_Alt_Ligne,
					Bestellungen.Benutzer as Username

					FROM Bestellungen 
					JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
					Inner JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
					INNER JOIN Lagerorte ON [bestellte Artikel].Lagerort_id = Lagerorte.Lagerort_id
					WHERE
					Bestellungen.[Nr]={orderId}
					AND (Bestellungen.Typ='Bestellung' Or Bestellungen.Typ='Kanbanabruf')
					AND ISNULL([bestellte Artikel].erledigt_pos,0) = 0 
					ORDER BY [bestellte Artikel].Position, [bestellte Artikel].Liefertermin, [bestellte Artikel].Bestätigter_Termin;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.WareneingangEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.WareneingangEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.WareneingangEntity> GetWareneingangEntities(int orderId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query =
					@$"SELECT 
					Bestellungen.[Bestellung-Nr],
					Bestellungen.Nr As BestellungenNr,
					Bestellungen.Typ, 
					Bestellungen.Mandant, 
					Bestellungen.Eingangslieferscheinnr,
					Bestellungen.[Vorname/NameFirma],
					[bestellte Artikel].Position, 
					[bestellte Artikel].Liefertermin, 
					[bestellte Artikel].Bestätigter_Termin, 
					[bestellte Artikel].Nr, 
					[bestellte Artikel].[WE Pos zu Bestellposition], 
					[bestellte Artikel].erledigt_pos, 
					[bestellte Artikel].[Artikel-Nr], 
					[bestellte Artikel].[Bezeichnung 1], 
					[bestellte Artikel].Einheit, 
					[bestellte Artikel].AnfangLagerBestand, 
					[bestellte Artikel].Anzahl, 
					[bestellte Artikel].[Start Anzahl],
					[bestellte Artikel].Erhalten, 
					[bestellte Artikel].[Aktuelle Anzahl], 
					[bestellte Artikel].EndeLagerBestand, 
					[bestellte Artikel].Bestellnummer, 
					[bestellte Artikel].Lagerort_id, 
					[bestellte Artikel].MhdDatumArtikel, 
					[bestellte Artikel].COC_bestätigung, 
					[bestellte Artikel].EMPB_Bestätigung,
					[bestellte Artikel].Lagerort_id AS Original_IDOrt,
					[bestellte Artikel].CocVersion,
					Artikel.ESD_Schutz,
					cast(Artikel.Größe as decimal(38,15)) AS Größe, 
					Artikel.EMPB, 
					Artikel.EMPB_Freigegeben, 
					Artikel.MHD, 
					Artikel.COF_Pflichtig, 
					Artikel.Zeitraum_MHD,
					Artikel.Artikelnummer,
					Lagerorte.Lagerort, 
					0 AS Erstellt,
					0 AS Id_Alt_Ligne,
					Bestellungen.Benutzer as Username

					FROM Bestellungen 
					JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
					Inner JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
					INNER JOIN Lagerorte ON [bestellte Artikel].Lagerort_id = Lagerorte.Lagerort_id
					WHERE
					Bestellungen.[Nr]={orderId}
					AND (Bestellungen.Typ='Bestellung' Or Bestellungen.Typ='Kanbanabruf')
					AND ISNULL([bestellte Artikel].erledigt_pos,0) = 0 
					ORDER BY [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Position, [bestellte Artikel].Liefertermin;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.WareneingangEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.WareneingangEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.ArtikelWareneingangGewichtEntity> GetArtikelWareingangGewicht(int artikelNr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query =
					@$"SELECT 
						  Artikel.Artikelnummer, 
						  Sum(Stücklisten.Anzahl * CAST(Artikel_1.Größe AS DECIMAL(38,15))) AS Materialgewicht
						FROM 
							Artikel 
							INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]
							INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]
							INNER JOIN (
								SELECT 
								  Artikel.[Artikel-Nr], 
								  Artikel.Artikelnummer 
								FROM 
								  Stücklisten 
								  INNER JOIN Artikel ON Stücklisten.[Artikel-Nr] = Artikel.[Artikel-Nr] 
								WHERE 
								  Stücklisten.[Artikel-Nr des Bauteils] = {artikelNr}
							) s ON Artikel.[Artikel-Nr] = s.[Artikel-Nr] 
						GROUP BY 
						  Artikel.Artikelnummer, 
						  Artikel.Stückliste, 
						  Artikel.aktiv 
						HAVING 
							  Artikel.Stückliste = 1
							And 
							  Artikel.aktiv = 1
						ORDER BY 
						  Artikel.Artikelnummer";

				var sqlCommand = new SqlCommand(query, connection, transaction);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.ArtikelWareneingangGewichtEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.ArtikelWareneingangGewichtEntity>();
			}
		}
		public static int UpdateArtikelGrossWithTransaction(List<Infrastructure.Data.Entities.Joins.MTM.Order.ArtikelWareneingangGewichtEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 129;
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateArtikelGrossWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateArtikelGrossWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateArtikelGrossWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateArtikelGrossWithTransaction(List<Infrastructure.Data.Entities.Joins.MTM.Order.ArtikelWareneingangGewichtEntity> items, SqlConnection connection, SqlTransaction transaction)
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

					+ "[Größe]=@Materialgewicht" + i
					+ " WHERE [Artikelnummer]=@Artikelnummer" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Materialgewicht" + i, item.Materialgewicht == null ? (object)DBNull.Value : item.Materialgewicht);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int GetMaxWareingangNrByMandant(int Nr, SqlConnection connection, SqlTransaction transaction)
		{

			var query = $"select max([Bestellung-Nr]) from Bestellungen WHERE Mandant = (select mandant from Bestellungen where [Nr] = @Nr) and typ='Wareneingang'";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", Nr);


			return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int maxNr) ? maxNr : 0;
		}

		public static int InsertWareneingangWithTransaction(int bestellungNr, string LegacyUserName, int maxbestellungNr, string lsNummer, SqlConnection connection, SqlTransaction transaction)
		{

			var query = @$"INSERT INTO Bestellungen
					([Projekt-Nr],
					[Bestellung-Nr],
					Typ,
					Datum,
					Liefertermin,
					[Lieferanten-Nr],
					Kreditorennummer,
					Anrede,
					[Vorname/NameFirma],
					Name2,
					Name3,
					Ansprechpartner,
					Abteilung,
					[Straße/Postfach],
					[Land/PLZ/Ort],
					Briefanrede,
					[Personal-Nr],
					Versandart,
					Zahlungsweise,
					Konditionen,
					Zahlungsziel,
					USt,
					Rabatt,
					Bezug,
					[Ihr Zeichen],
					[Unser Zeichen],
					Freitext,
					gebucht,
					gedruckt,
					erledigt,
					Währung,
					Kundenbestellung,
					Anfrage_Lieferfrist,
					Mahnung,
					Bemerkungen,
					best_id,
					datueber,
					nr_bes,
					Belegkreis,
					Rahmenbestellung,
					Bearbeiter,
					Benutzer,
					Mandant,
					Eingangslieferscheinnr)

					SELECT Bestellungen.[Projekt-Nr], 
					{maxbestellungNr}, 
					'Wareneingang', 
					cast(getdate() as date), 
					cast(getdate() as date), 
					Bestellungen.[Lieferanten-Nr], 
					Bestellungen.Kreditorennummer, 
					Bestellungen.Anrede, 
					Bestellungen.[Vorname/NameFirma], 
					Bestellungen.Name2, 
					Bestellungen.Name3, 
					Bestellungen.Ansprechpartner, 
					Bestellungen.Abteilung, 
					Bestellungen.[Straße/Postfach], 
					Bestellungen.[Land/PLZ/Ort], 
					Bestellungen.Briefanrede, 
					Bestellungen.[Personal-Nr], 
					Bestellungen.Versandart, 
					Bestellungen.Zahlungsweise, 
					Bestellungen.Konditionen, 
					Bestellungen.Zahlungsziel, 
					Bestellungen.USt, 
					Bestellungen.Rabatt, 
					Bestellungen.Bezug, 
					Bestellungen.[Ihr Zeichen], 
					Bestellungen.[Unser Zeichen], 
					Bestellungen.Freitext, 
					1,
					0, 
					1, 
					Bestellungen.Währung, 
					Bestellungen.Kundenbestellung,
					Bestellungen.Anfrage_Lieferfrist, 
					Bestellungen.Mahnung, 
					Bestellungen.Bemerkungen, 
					Bestellungen.Nr, 
					0, 
					Bestellungen.[Bestellung-Nr], 
					Bestellungen.Belegkreis, 
					Bestellungen.Rahmenbestellung, 
					Bestellungen.Bearbeiter, 
					'{LegacyUserName} {DateTime.Today}',
					Bestellungen.Mandant,
					'{lsNummer}'

					FROM Bestellungen
					WHERE Bestellungen.[Nr]={bestellungNr};";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}

		public static int InsertBestellteArtikelWareneingangWithTransaction(List<Infrastructure.Data.Entities.Joins.MTM.Order.BestellteArtikelWareneingangEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 129;
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertBestellteArtikelWareneingangWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertBestellteArtikelWareneingangWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += insertBestellteArtikelWareneingangWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int insertBestellteArtikelWareneingangWithTransaction(List<Infrastructure.Data.Entities.Joins.MTM.Order.BestellteArtikelWareneingangEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += $@"
							INSERT INTO [bestellte Artikel] 
							([Position],[Artikel-Nr],[Bezeichnung 1],[Bezeichnung 2],Einheit,AnfangLagerBestand,Anzahl,[Start Anzahl],Erhalten,[Aktuelle Anzahl],EndeLagerBestand,Umsatzsteuer,
							Einzelpreis,Gesamtpreis,Preisgruppe,Bestellnummer,Rabatt,Rabatt1,sortierung,schriftart,Preiseinheit,Liefertermin,erledigt_pos,Bestätigter_Termin,[Position erledigt],
							Bemerkung_Pos,Bemerkung_Pos_ID,Produktionsort,[WE Pos zu Bestellposition],[Bestellung-Nr],Lagerort_id,Kanban,MhdDatumArtikel,InfoRahmennummer,CUPreis,CoCVersion)
							SELECT 
							@Position{i},[bestellte Artikel].[Artikel-Nr],[bestellte Artikel].[Bezeichnung 1],[bestellte Artikel].[Bezeichnung 2],[bestellte Artikel].Einheit,
							[bestellte Artikel].AnfangLagerBestand,@AktuelleAnzahl{i},[bestellte Artikel].[Start Anzahl],[bestellte Artikel].Erhalten,[bestellte Artikel].[Aktuelle Anzahl],
							[bestellte Artikel].EndeLagerBestand,[bestellte Artikel].Umsatzsteuer,[bestellte Artikel].Einzelpreis,@AktuelleAnzahl{i}*[bestellte Artikel].Einzelpreis / [bestellte Artikel].Preiseinheit,
							[bestellte Artikel].Preisgruppe,[bestellte Artikel].Bestellnummer,[bestellte Artikel].Rabatt,[bestellte Artikel].Rabatt1,[bestellte Artikel].sortierung,[bestellte Artikel].schriftart,
							[bestellte Artikel].Preiseinheit,CAST(getdate() AS date),1, [bestellte Artikel].Bestätigter_Termin,[bestellte Artikel].[Position erledigt],[bestellte Artikel].Bemerkung_Pos,
							[bestellte Artikel].Bemerkung_Pos_ID,[bestellte Artikel].Produktionsort,[bestellte Artikel].Nr,@newWareneingangNr{i},@Lagerort_id{i},[bestellte Artikel].Kanban,
							@MhdDatumArtikel{i},[bestellte Artikel].InfoRahmennummer,[CUPreis]/[bestellte Artikel].[Start Anzahl] * @AktuelleAnzahl{i},[bestellte Artikel].CoCVersion
							FROM [bestellte Artikel] 
							WHERE [bestellte Artikel].Nr = @CurrentBestellteArtikelNr{i} ";
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position);
					sqlCommand.Parameters.AddWithValue("AktuelleAnzahl" + i, item.AktuelleAnzahl);
					sqlCommand.Parameters.AddWithValue("newWareneingangNr" + i, item.NewWareneingangNr);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
					sqlCommand.Parameters.AddWithValue("CurrentBestellteArtikelNr" + i, item.CurrentBestellteArtikelNr);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}


		public static int UpdateCurrentBestellungWithTransaction(List<Infrastructure.Data.Entities.Joins.MTM.Order.BestellteArtikelWareneingangEntity> items, string LegacyUserName, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 129;
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateCurrentBestellungWithTransaction(items, LegacyUserName, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateCurrentBestellungWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), LegacyUserName, connection, transaction);
					}
					results += updateCurrentBestellungWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), LegacyUserName, connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateCurrentBestellungWithTransaction(List<Infrastructure.Data.Entities.Joins.MTM.Order.BestellteArtikelWareneingangEntity> items, string LegacyUserName, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += $@"
							UPDATE [bestellte Artikel] 
								SET
								[bestellte Artikel].Anzahl = [bestellte Artikel].Anzahl - @AktuelleAnzahl{i}, 
								[bestellte Artikel].Erhalten = [bestellte Artikel].Erhalten + @AktuelleAnzahl{i},
								[bestellte Artikel].Gesamtpreis = ([bestellte Artikel].Anzahl-@AktuelleAnzahl{i}) / [bestellte Artikel].Preiseinheit * [bestellte Artikel].Einzelpreis, 
								[bestellte Artikel].erledigt_pos = @erledigt_pos{i}, 
								[bestellte Artikel].Bemerkung_Pos = CONCAT([bestellte Artikel].Bemerkung_Pos,', Materialeingang:',@AktuelleAnzahl{i},[bestellte Artikel].Einheit,' gebucht von: ',@userLegacyUserName{i},' am: ', getDate())
								WHERE [bestellte Artikel].Nr = @currentBestellteArtikelNr{i}";

					sqlCommand.Parameters.AddWithValue("AktuelleAnzahl" + i, item.AktuelleAnzahl);
					sqlCommand.Parameters.AddWithValue("currentBestellteArtikelNr" + i, item.CurrentBestellteArtikelNr);
					sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("userLegacyUserName" + i, LegacyUserName);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}



		public static int UpdateLagerortQuantitiesWithTransaction(List<Infrastructure.Data.Entities.Joins.MTM.Order.BestellteArtikelWareneingangEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 129;
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateLagerortQuantitiesWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateLagerortQuantitiesWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateLagerortQuantitiesWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateLagerortQuantitiesWithTransaction(List<Infrastructure.Data.Entities.Joins.MTM.Order.BestellteArtikelWareneingangEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += $@"
							UPDATE  
							Lager
							SET Lager.Bestand = ISNULL(Lager.Bestand,0) + @AktuelleAnzahl{i}, Lager.[letzte Bewegung] = cast(getdate() as date)
							WHERE Lager.Lagerort_Id = @lagerortId{i} AND Lager.[Artikel-Nr] = @ArtikelNr{i};";

					sqlCommand.Parameters.AddWithValue("AktuelleAnzahl" + i, item.AktuelleAnzahl);
					sqlCommand.Parameters.AddWithValue("lagerortId" + i, item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static List<WareneingangReportEntity> GetWareneingangData(int bestellungNr, int Lagerort_id, int bestellteArtikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
					$@"SELECT 
					[PSZ_Lagerorte Zuordnung Fertigung].Lagerort_id AS NrTransfer, 
					Bestellungen.[Bestellung-Nr],
					[bestellte Artikel].Lagerort_id, 
						CASE [bestellte Artikel].Lagerort_id 
						WHEN '8' THEN CONCAT('DE-',Bestellungen.[Projekt-Nr])
						WHEN '4' THEN   CONCAT('TN-' ,Bestellungen.[Projekt-Nr])
						else CONCAT('CZ-' ,Bestellungen.[Projekt-Nr])
						END AS 'Lagerort_Projekt_Nr',
					Bestellungen.[Projekt-Nr], 
					Bestellungen.Eingangslieferscheinnr, 
					Artikel.Artikelnummer,
					[bestellte Artikel].[Bezeichnung 1], 
					[bestellte Artikel].[Bezeichnung 2],
					[bestellte Artikel].Bestellnummer, 
					[bestellte Artikel].Anzahl AS Menge, 
					[bestellte Artikel].Einheit,
					IIF([bestellte Artikel].Liefertermin >0,Concat('Liefertermin: ',FORMAT([bestellte Artikel].Liefertermin,'d', 'de-DE')),'') as LT,
					IIF([bestellte Artikel].Lagerort_id = 13 AND ISNULL([bestellte Artikel].EMPB_Bestätigung,0) = 1,[PSZ_Lagerorte Zuordnung Fertigung_3].Fertigung,[PSZ_Lagerorte Zuordnung Fertigung].Fertigung) as Fertigung,
					IIF([bestellte Artikel].Lagerort_id = 13 AND ISNULL([bestellte Artikel].EMPB_Bestätigung,0) = 1,'WEK','') AS WEK,
					[bestellte Artikel].Umsatzsteuer, 
					FORMAT(Bestellungen.Liefertermin,'d', 'de-DE') as Liefertermin,
					[bestellte Artikel].Einzelpreis, 
					[bestellte Artikel].Preiseinheit,
					[bestellte Artikel].Rabatt AS Rabatt1,
					[bestellte Artikel].Rabatt1 AS Rabatt2, 
					[bestellte Artikel].Gesamtpreis,
					[bestellte Artikel].EMPB_Bestätigung,
					[bestellte Artikel].Nr AS best_art_nr, 
					Bestellungen.[Vorname/NameFirma],  
					adressen.Lieferantennummer,
					[bestellte Artikel].Kanban,
					CONCAT('*%',Artikel.Artikelnummer,'%',[bestellte Artikel].Anzahl,'%*') AS Code,
					bestellungen.[Land/PLZ/Ort]
					FROM 
					Bestellungen
					INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
					INNER JOIN [PSZ_Lagerorte Zuordnung Fertigung] AS [PSZ_Lagerorte Zuordnung Fertigung_3] on [PSZ_Lagerorte Zuordnung Fertigung_3].Lagerort_id = [bestellte Artikel].Lagerort_id
					INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
					INNER JOIN adressen ON Bestellungen.[Lieferanten-Nr] = adressen.Nr 
					INNER JOIN Bestellnummern ON adressen.Nr = Bestellnummern.[Lieferanten-Nr] AND Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
					INNER JOIN [PSZ_Lagerorte Zuordnung Fertigung] ON [bestellte Artikel].Lagerort_id = [PSZ_Lagerorte Zuordnung Fertigung].Lagerort_id
					WHERE
					[PSZ_Lagerorte Zuordnung Fertigung_3].Lagerort_id ={Lagerort_id}
					AND Bestellungen.[Bestellung-Nr]={bestellungNr}
					AND [bestellte Artikel].Nr={bestellteArtikelNr};";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new WareneingangReportEntity(x)).ToList();
				}
				else
				{
					return new List<WareneingangReportEntity>();
				}
			}
		}


		public static List<WareneingangReportEntity> GetWareneingangData(int bestellungNr, List<int?> Lagerort_id, List<int> bestellteArtikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
					$@"SELECT 
					[PSZ_Lagerorte Zuordnung Fertigung].Lagerort_id AS NrTransfer, 
					Bestellungen.[Bestellung-Nr],
					[bestellte Artikel].Lagerort_id, 
						CASE [bestellte Artikel].Lagerort_id 
						WHEN '8' THEN CONCAT('DE-',Bestellungen.[Projekt-Nr])
						WHEN '4' THEN   CONCAT('TN-' ,Bestellungen.[Projekt-Nr])
						else CONCAT('CZ-' ,Bestellungen.[Projekt-Nr])
						END AS 'Lagerort_Projekt_Nr',
					Bestellungen.[Projekt-Nr], 
					Bestellungen.Eingangslieferscheinnr, 
					Artikel.Artikelnummer,
					[bestellte Artikel].[Bezeichnung 1], 
					[bestellte Artikel].[Bezeichnung 2],
					[bestellte Artikel].Bestellnummer, 
					[bestellte Artikel].Anzahl AS Menge, 
					[bestellte Artikel].Einheit,
					IIF([bestellte Artikel].Liefertermin >0,Concat('Liefertermin: ',FORMAT([bestellte Artikel].Liefertermin,'d', 'de-DE')),'') as LT,
					IIF([bestellte Artikel].Lagerort_id = 13 AND ISNULL([bestellte Artikel].EMPB_Bestätigung,0) = 1,[PSZ_Lagerorte Zuordnung Fertigung_3].Fertigung,[PSZ_Lagerorte Zuordnung Fertigung].Fertigung) as Fertigung,
					IIF([bestellte Artikel].Lagerort_id = 13 AND ISNULL([bestellte Artikel].EMPB_Bestätigung,0) = 1,'WEK','') AS WEK,
					[bestellte Artikel].Umsatzsteuer, 
					FORMAT(Bestellungen.Liefertermin,'d', 'de-DE') as Liefertermin,
					[bestellte Artikel].Einzelpreis, 
					[bestellte Artikel].Preiseinheit,
					[bestellte Artikel].Rabatt AS Rabatt1,
					[bestellte Artikel].Rabatt1 AS Rabatt2, 
					[bestellte Artikel].Gesamtpreis,
					[bestellte Artikel].EMPB_Bestätigung,
					[bestellte Artikel].Nr AS best_art_nr, 
					Bestellungen.[Vorname/NameFirma],  
					adressen.Lieferantennummer,
					[bestellte Artikel].Kanban,
					CONCAT('*%',Artikel.Artikelnummer,'%',[bestellte Artikel].Anzahl,'%*') AS Code,
					bestellungen.[Land/PLZ/Ort]
					FROM 
					Bestellungen
					INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
					INNER JOIN [PSZ_Lagerorte Zuordnung Fertigung] AS [PSZ_Lagerorte Zuordnung Fertigung_3] on [PSZ_Lagerorte Zuordnung Fertigung_3].Lagerort_id = [bestellte Artikel].Lagerort_id
					INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
					INNER JOIN adressen ON Bestellungen.[Lieferanten-Nr] = adressen.Nr 
					INNER JOIN Bestellnummern ON adressen.Nr = Bestellnummern.[Lieferanten-Nr] AND Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
					INNER JOIN [PSZ_Lagerorte Zuordnung Fertigung] ON [bestellte Artikel].Lagerort_id = [PSZ_Lagerorte Zuordnung Fertigung].Lagerort_id
					WHERE
					--[PSZ_Lagerorte Zuordnung Fertigung_3].Lagerort_id in ({String.Join(", ", Lagerort_id.Select(i => i.ToString()).ToList())})
					Bestellungen.[Bestellung-Nr]={bestellungNr}
					--AND [bestellte Artikel].Nr in ({String.Join(", ", bestellteArtikelNr.Select(i => i.ToString()).ToList())});";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new WareneingangReportEntity(x)).ToList();
				}
				else
				{
					return new List<WareneingangReportEntity>();
				}
			}
		}
	}
}
