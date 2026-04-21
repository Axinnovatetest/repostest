using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class PSZ_Historique_Import_Excel_FAAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Historique_Import_Excel_FA] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Historique_Import_Excel_FA]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_Historique_Import_Excel_FA] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_Historique_Import_Excel_FA] ([Änderungsdatum],[bermerkung],[Fertigungsnummer],[Mitarbeiter],[PC],[Termin_Wunsch])  VALUES (@Änderungsdatum,@bermerkung,@Fertigungsnummer,@Mitarbeiter,@PC,@Termin_Wunsch); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Änderungsdatum", item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
					sqlCommand.Parameters.AddWithValue("bermerkung", item.bermerkung == null ? (object)DBNull.Value : item.bermerkung);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("PC", item.PC == null ? (object)DBNull.Value : item.PC);
					sqlCommand.Parameters.AddWithValue("Termin_Wunsch", item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity> items)
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
						query += " INSERT INTO [PSZ_Historique_Import_Excel_FA] ([Änderungsdatum],[bermerkung],[Fertigungsnummer],[Mitarbeiter],[PC],[Termin_Wunsch]) VALUES ( "

							+ "@Änderungsdatum" + i + ","
							+ "@bermerkung" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@Mitarbeiter" + i + ","
							+ "@PC" + i + ","
							+ "@Termin_Wunsch" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Änderungsdatum" + i, item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
						sqlCommand.Parameters.AddWithValue("bermerkung" + i, item.bermerkung == null ? (object)DBNull.Value : item.bermerkung);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("PC" + i, item.PC == null ? (object)DBNull.Value : item.PC);
						sqlCommand.Parameters.AddWithValue("Termin_Wunsch" + i, item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_Historique_Import_Excel_FA] SET [Änderungsdatum]=@Änderungsdatum, [bermerkung]=@bermerkung, [Fertigungsnummer]=@Fertigungsnummer, [Mitarbeiter]=@Mitarbeiter, [PC]=@PC, [Termin_Wunsch]=@Termin_Wunsch WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Änderungsdatum", item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
				sqlCommand.Parameters.AddWithValue("bermerkung", item.bermerkung == null ? (object)DBNull.Value : item.bermerkung);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
				sqlCommand.Parameters.AddWithValue("PC", item.PC == null ? (object)DBNull.Value : item.PC);
				sqlCommand.Parameters.AddWithValue("Termin_Wunsch", item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Historique_Import_Excel_FAEntity> items)
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
						query += " UPDATE [PSZ_Historique_Import_Excel_FA] SET "

							+ "[Änderungsdatum]=@Änderungsdatum" + i + ","
							+ "[bermerkung]=@bermerkung" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[Mitarbeiter]=@Mitarbeiter" + i + ","
							+ "[PC]=@PC" + i + ","
							+ "[Termin_Wunsch]=@Termin_Wunsch" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Änderungsdatum" + i, item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
						sqlCommand.Parameters.AddWithValue("bermerkung" + i, item.bermerkung == null ? (object)DBNull.Value : item.bermerkung);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("PC" + i, item.PC == null ? (object)DBNull.Value : item.PC);
						sqlCommand.Parameters.AddWithValue("Termin_Wunsch" + i, item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [PSZ_Historique_Import_Excel_FA] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

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

					string query = "DELETE FROM [PSZ_Historique_Import_Excel_FA] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static int InsertWusnUpdateAdmin(string joint, string username, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = int.MinValue;
			//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				//sqlConnection.Open();
				//var sqlTransaction = sqlConnection.BeginTransaction();

				string query = $@"INSERT INTO PSZ_Historique_Import_Excel_FA ( Termin_Wunsch, Fertigungsnummer, Mitarbeiter, PC, bermerkung, Änderungsdatum )
                               SELECT Fertigung.Termin_Bestätigt1, Fertigung.Fertigungsnummer, '{username}' AS Benutzer, '{username}' AS PC, 
                               concat('Update durch  Exce-Import am   ',getdate()) AS Bemerkung, getdate() AS Ausdr1
                               FROM Fertigung INNER JOIN 
                               ({joint})
                               tblExcelImport 
                               ON Fertigung.Fertigungsnummer = tblExcelImport.Fertigungsnummer
                               WHERE tblExcelImport.Termin Is Not Null";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				//sqlTransaction.Commit();

				return response;
			}
		}
		public static int InsertWusnUpdateUser(string joint, string username, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = int.MinValue;
			//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				//sqlConnection.Open();
				//var sqlTransaction = sqlConnection.BeginTransaction();

				string query = $@"INSERT INTO PSZ_Historique_Import_Excel_FA
                               ( Termin_Wunsch, Fertigungsnummer, Mitarbeiter, PC, bermerkung, Änderungsdatum )
                               SELECT Fertigung.Termin_Bestätigt1, Fertigung.Fertigungsnummer, '{username}' AS Benutzer, '{username}' AS PC,
                               CONCAT('Update durch  Exce-Import am',GETDATE()) AS Bemerkung, GETDATE() AS Ausdr1
                               FROM Fertigung INNER JOIN 
                               ({joint})
                               tblExcelImport 
 ON Fertigung.Fertigungsnummer = tblExcelImport.Fertigungsnummer
left JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
WHERE


(Fertigung.Kennzeichen = N'offen' AND 
IIf(ISNULL([Kabel_geschnitten],0) = 0, 0, 1) = 0 AND
IIf (ISNULL([Check_FAbegonnen],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_Gewerk2_Teilweise],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk3_Teilweise],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_Gewerk1_Teilweise],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk1],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk2],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk3],0) = 0, 0, 1) = 0 AND
Fertigung.gedruckt = 0 AND
Fertigung.FA_Druckdatum Is Null AND 
tblExcelImport.Termin >= GETDATE() + 21 AND
Fertigung.Lagerort_id <> 42 and Fertigung.Lagerort_id <> 7 and Fertigung.Lagerort_id <> 60 and Fertigung.Lagerort_id <> 6 and Fertigung.Lagerort_id <> 26)
and Artikel.Artikelnummer is not null						 
OR

(Fertigung.Kennzeichen = N'offen' AND
IIf(ISNULL([Kabel_geschnitten],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_FAbegonnen],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk2_Teilweise],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_Gewerk3_Teilweise],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk1_Teilweise],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_Gewerk1],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk2],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_Gewerk3],0) = 0, 0, 1) = 0 AND
Fertigung.gedruckt = 0 AND 
Fertigung.FA_Druckdatum Is Null AND
tblExcelImport.Termin >= GETDATE() + 21 
AND (Fertigung.Lagerort_id = 42 or Fertigung.Lagerort_id = 6 or Fertigung.Lagerort_id = 60 or Fertigung.Lagerort_id = 7 or Fertigung.Lagerort_id = 26) AND
(Fertigung.FA_Gestartet = 0 Or Fertigung.FA_Gestartet Is Null))
and Artikel.Artikelnummer is not null";
				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				//sqlTransaction.Commit();

				return response;
			}
		}

		#endregion
	}
}
