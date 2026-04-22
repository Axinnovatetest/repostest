using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class ArtikelKalkulatorischeKostenAccess
	{
		public static string ARBEITS_KOSTEN = "Arbeitskosten";
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Artikel_kalkulatorische_kosten] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Artikel_kalkulatorische_kosten]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Artikel_kalkulatorische_kosten] WHERE [ID] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Artikel_kalkulatorische_kosten] ([artikel-nr],[Betrag],[Kostenart])  VALUES (@artikel_nr,@Betrag,@Kostenart);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("artikel_nr", item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
					sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
					sqlCommand.Parameters.AddWithValue("Kostenart", item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Artikel_kalkulatorische_kosten] ([artikel-nr],[Betrag],[Kostenart]) VALUES ( "

							+ "@artikel_nr" + i + ","
							+ "@Betrag" + i + ","
							+ "@Kostenart" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("artikel_nr" + i, item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
						sqlCommand.Parameters.AddWithValue("Betrag" + i, item.Betrag == null ? (object)DBNull.Value : item.Betrag);
						sqlCommand.Parameters.AddWithValue("Kostenart" + i, item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Artikel_kalkulatorische_kosten] SET [artikel-nr]=@artikel_nr, [Betrag]=@Betrag, [Kostenart]=@Kostenart WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("artikel_nr", item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
				sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
				sqlCommand.Parameters.AddWithValue("Kostenart", item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [Artikel_kalkulatorische_kosten] SET "

							+ "[artikel-nr]=@artikel-nr" + i + ","
							+ "[Betrag]=@Betrag" + i + ","
							+ "[Kostenart]=@Kostenart" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("artikel_nr" + i, item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
						sqlCommand.Parameters.AddWithValue("Betrag" + i, item.Betrag == null ? (object)DBNull.Value : item.Betrag);
						sqlCommand.Parameters.AddWithValue("Kostenart" + i, item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);
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
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Artikel_kalkulatorische_kosten] WHERE [ID]=@ID";
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
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
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

					string query = "DELETE FROM [Artikel_kalkulatorische_kosten] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}


		public static int Update(Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int results = -1;
			string query = "UPDATE [Artikel_kalkulatorische_kosten] SET [artikel-nr]=@artikel_nr, [Betrag]=@Betrag, [Kostenart]=@Kostenart WHERE [ID]=@ID";
			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("artikel_nr", item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
				sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
				sqlCommand.Parameters.AddWithValue("Kostenart", item.Kostenart == null ? (object)DBNull.Value : item.Kostenart);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#endregion

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity GetByArtikelNr(int artikelNr)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Artikel_kalkulatorische_kosten] WHERE [artikel-nr]=@artikelNr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			return dataTable.Rows.Count > 0
				? new Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity(dataTable.Rows[0])
				: null;
		}
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity GetByArtikelNr(int artikelNr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Artikel_kalkulatorische_kosten] WHERE [artikel-nr]=@artikelNr";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);

			DbExecution.Fill(sqlCommand, dataTable);


			return dataTable.Rows.Count > 0
				? new Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity(dataTable.Rows[0])
				: null;
		}
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity GetArbeitskostenByArtikelNr(int artikelNr)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Artikel_kalkulatorische_kosten] WHERE [artikel-nr]=@artikelNr AND [Kostenart]='{ARBEITS_KOSTEN}'";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			return dataTable.Rows.Count > 0
				? new Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity(dataTable.Rows[0])
				: null;
		}
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity GetArbeitskostenByArtikelNr(int artikelNr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = $"SELECT * FROM [Artikel_kalkulatorische_kosten] WHERE [artikel-nr]=@artikelNr AND [Kostenart]='{ARBEITS_KOSTEN}'";
			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			return dataTable.Rows.Count > 0
				? new Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity(dataTable.Rows[0])
				: null;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity> GetArbeitskostenByArtikelNr(List<int> articleIds, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(articleIds == null || articleIds.Count==0)
			{
				return null;
			}
			var dataTable = new DataTable();

			string query = $"SELECT * FROM [Artikel_kalkulatorische_kosten] WHERE [artikel-nr] IN ({string.Join(",", articleIds)}) AND [Kostenart]='{ARBEITS_KOSTEN}'";
			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				DbExecution.Fill(sqlCommand, dataTable);
			}

			return dataTable.Rows.Count > 0
				? toList(dataTable)
				: null;
		}
		public static int DeleteArbeitskostenByArtikelNr(int artikelNr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"DELETE FROM [Artikel_kalkulatorische_kosten] WHERE [artikel-nr]=@artikelNr AND [Kostenart]='{ARBEITS_KOSTEN}'";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int InsertArbeitskostenForArtikel(Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = -1;
			string query = "INSERT INTO [Artikel_kalkulatorische_kosten] ([artikel-nr],[Betrag],[Kostenart])  VALUES (@artikel_nr,@Betrag,@Kostenart);";
			query += "SELECT SCOPE_IDENTITY();";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("artikel_nr", item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
				sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
				sqlCommand.Parameters.AddWithValue("Kostenart", ARBEITS_KOSTEN);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static int UpdateProductionCost(IEnumerable<KeyValuePair<int, decimal>> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items == null || items.Count() <= 0)
			{
				return 0;
			}

			string query = "";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				for(int i = 0; i < items.Count(); i++)
				{
					query += $"UPDATE [Artikel_kalkulatorische_kosten] SET [Betrag]=@betrag{i} WHERE [artikel-nr]=@articleNr{i};";
					sqlCommand.Parameters.AddWithValue($"articleNr{i}", items.ElementAt(i).Key);
					sqlCommand.Parameters.AddWithValue($"betrag{i}", items.ElementAt(i).Value);
				}
				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
