using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class ZahlungskonditionenAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Zahlungskonditionen] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Zahlungskonditionen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Zahlungskonditionen] WHERE [ID] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Zahlungskonditionen] ([Bemerkungen],[Betrag_berechnen],[Nachlaß1],[Nachlaß2],[Nachlaß3],[Tage1],[Tage2],[Tage3],[Text11],[Text12],[Text21],[Text22],[Text31],[Text32])  VALUES (@Bemerkungen,@Betrag_berechnen,@Nachlaß1,@Nachlaß2,@Nachlaß3,@Tage1,@Tage2,@Tage3,@Text11,@Text12,@Text21,@Text22,@Text31,@Text32);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					//sqlCommand.Parameters.AddWithValue("ID",item.ID);
					sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Betrag_berechnen", item.Betrag_berechnen == null ? (object)DBNull.Value : item.Betrag_berechnen);
					sqlCommand.Parameters.AddWithValue("Nachlaß1", item.Nachlaß1 == null ? (object)DBNull.Value : item.Nachlaß1);
					sqlCommand.Parameters.AddWithValue("Nachlaß2", item.Nachlaß2 == null ? (object)DBNull.Value : item.Nachlaß2);
					sqlCommand.Parameters.AddWithValue("Nachlaß3", item.Nachlaß3 == null ? (object)DBNull.Value : item.Nachlaß3);
					sqlCommand.Parameters.AddWithValue("Tage1", item.Tage1 == null ? (object)DBNull.Value : item.Tage1);
					sqlCommand.Parameters.AddWithValue("Tage2", item.Tage2 == null ? (object)DBNull.Value : item.Tage2);
					sqlCommand.Parameters.AddWithValue("Tage3", item.Tage3 == null ? (object)DBNull.Value : item.Tage3);
					sqlCommand.Parameters.AddWithValue("Text11", item.Text11 == null ? (object)DBNull.Value : item.Text11);
					sqlCommand.Parameters.AddWithValue("Text12", item.Text12 == null ? (object)DBNull.Value : item.Text12);
					sqlCommand.Parameters.AddWithValue("Text21", item.Text21 == null ? (object)DBNull.Value : item.Text21);
					sqlCommand.Parameters.AddWithValue("Text22", item.Text22 == null ? (object)DBNull.Value : item.Text22);
					sqlCommand.Parameters.AddWithValue("Text31", item.Text31 == null ? (object)DBNull.Value : item.Text31);
					sqlCommand.Parameters.AddWithValue("Text32", item.Text32 == null ? (object)DBNull.Value : item.Text32);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity> items)
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
						query += " INSERT INTO [Zahlungskonditionen] ([ID],[Bemerkungen],[Betrag_berechnen],[Nachlaß1],[Nachlaß2],[Nachlaß3],[Tage1],[Tage2],[Tage3],[Text11],[Text12],[Text21],[Text22],[Text31],[Text32]) VALUES ( "

							+ "@ID" + i + ","
							+ "@Bemerkungen" + i + ","
							+ "@Betrag_berechnen" + i + ","
							+ "@Nachlaß1" + i + ","
							+ "@Nachlaß2" + i + ","
							+ "@Nachlaß3" + i + ","
							+ "@Tage1" + i + ","
							+ "@Tage2" + i + ","
							+ "@Tage3" + i + ","
							+ "@Text11" + i + ","
							+ "@Text12" + i + ","
							+ "@Text21" + i + ","
							+ "@Text22" + i + ","
							+ "@Text31" + i + ","
							+ "@Text32" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Betrag_berechnen" + i, item.Betrag_berechnen == null ? (object)DBNull.Value : item.Betrag_berechnen);
						sqlCommand.Parameters.AddWithValue("Nachlaß1" + i, item.Nachlaß1 == null ? (object)DBNull.Value : item.Nachlaß1);
						sqlCommand.Parameters.AddWithValue("Nachlaß2" + i, item.Nachlaß2 == null ? (object)DBNull.Value : item.Nachlaß2);
						sqlCommand.Parameters.AddWithValue("Nachlaß3" + i, item.Nachlaß3 == null ? (object)DBNull.Value : item.Nachlaß3);
						sqlCommand.Parameters.AddWithValue("Tage1" + i, item.Tage1 == null ? (object)DBNull.Value : item.Tage1);
						sqlCommand.Parameters.AddWithValue("Tage2" + i, item.Tage2 == null ? (object)DBNull.Value : item.Tage2);
						sqlCommand.Parameters.AddWithValue("Tage3" + i, item.Tage3 == null ? (object)DBNull.Value : item.Tage3);
						sqlCommand.Parameters.AddWithValue("Text11" + i, item.Text11 == null ? (object)DBNull.Value : item.Text11);
						sqlCommand.Parameters.AddWithValue("Text12" + i, item.Text12 == null ? (object)DBNull.Value : item.Text12);
						sqlCommand.Parameters.AddWithValue("Text21" + i, item.Text21 == null ? (object)DBNull.Value : item.Text21);
						sqlCommand.Parameters.AddWithValue("Text22" + i, item.Text22 == null ? (object)DBNull.Value : item.Text22);
						sqlCommand.Parameters.AddWithValue("Text31" + i, item.Text31 == null ? (object)DBNull.Value : item.Text31);
						sqlCommand.Parameters.AddWithValue("Text32" + i, item.Text32 == null ? (object)DBNull.Value : item.Text32);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Zahlungskonditionen] SET [Bemerkungen]=@Bemerkungen, [Betrag_berechnen]=@Betrag_berechnen, [Nachlaß1]=@Nachlaß1, [Nachlaß2]=@Nachlaß2, [Nachlaß3]=@Nachlaß3, [Tage1]=@Tage1, [Tage2]=@Tage2, [Tage3]=@Tage3, [Text11]=@Text11, [Text12]=@Text12, [Text21]=@Text21, [Text22]=@Text22, [Text31]=@Text31, [Text32]=@Text32 WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("Betrag_berechnen", item.Betrag_berechnen == null ? (object)DBNull.Value : item.Betrag_berechnen);
				sqlCommand.Parameters.AddWithValue("Nachlaß1", item.Nachlaß1 == null ? (object)DBNull.Value : item.Nachlaß1);
				sqlCommand.Parameters.AddWithValue("Nachlaß2", item.Nachlaß2 == null ? (object)DBNull.Value : item.Nachlaß2);
				sqlCommand.Parameters.AddWithValue("Nachlaß3", item.Nachlaß3 == null ? (object)DBNull.Value : item.Nachlaß3);
				sqlCommand.Parameters.AddWithValue("Tage1", item.Tage1 == null ? (object)DBNull.Value : item.Tage1);
				sqlCommand.Parameters.AddWithValue("Tage2", item.Tage2 == null ? (object)DBNull.Value : item.Tage2);
				sqlCommand.Parameters.AddWithValue("Tage3", item.Tage3 == null ? (object)DBNull.Value : item.Tage3);
				sqlCommand.Parameters.AddWithValue("Text11", item.Text11 == null ? (object)DBNull.Value : item.Text11);
				sqlCommand.Parameters.AddWithValue("Text12", item.Text12 == null ? (object)DBNull.Value : item.Text12);
				sqlCommand.Parameters.AddWithValue("Text21", item.Text21 == null ? (object)DBNull.Value : item.Text21);
				sqlCommand.Parameters.AddWithValue("Text22", item.Text22 == null ? (object)DBNull.Value : item.Text22);
				sqlCommand.Parameters.AddWithValue("Text31", item.Text31 == null ? (object)DBNull.Value : item.Text31);
				sqlCommand.Parameters.AddWithValue("Text32", item.Text32 == null ? (object)DBNull.Value : item.Text32);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity> items)
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
						query += " UPDATE [Zahlungskonditionen] SET "

							+ "[Bemerkungen]=@Bemerkungen" + i + ","
							+ "[Betrag_berechnen]=@Betrag_berechnen" + i + ","
							+ "[Nachlaß1]=@Nachlaß1" + i + ","
							+ "[Nachlaß2]=@Nachlaß2" + i + ","
							+ "[Nachlaß3]=@Nachlaß3" + i + ","
							+ "[Tage1]=@Tage1" + i + ","
							+ "[Tage2]=@Tage2" + i + ","
							+ "[Tage3]=@Tage3" + i + ","
							+ "[Text11]=@Text11" + i + ","
							+ "[Text12]=@Text12" + i + ","
							+ "[Text21]=@Text21" + i + ","
							+ "[Text22]=@Text22" + i + ","
							+ "[Text31]=@Text31" + i + ","
							+ "[Text32]=@Text32" + i
							+ " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Betrag_berechnen" + i, item.Betrag_berechnen == null ? (object)DBNull.Value : item.Betrag_berechnen);
						sqlCommand.Parameters.AddWithValue("Nachlaß1" + i, item.Nachlaß1 == null ? (object)DBNull.Value : item.Nachlaß1);
						sqlCommand.Parameters.AddWithValue("Nachlaß2" + i, item.Nachlaß2 == null ? (object)DBNull.Value : item.Nachlaß2);
						sqlCommand.Parameters.AddWithValue("Nachlaß3" + i, item.Nachlaß3 == null ? (object)DBNull.Value : item.Nachlaß3);
						sqlCommand.Parameters.AddWithValue("Tage1" + i, item.Tage1 == null ? (object)DBNull.Value : item.Tage1);
						sqlCommand.Parameters.AddWithValue("Tage2" + i, item.Tage2 == null ? (object)DBNull.Value : item.Tage2);
						sqlCommand.Parameters.AddWithValue("Tage3" + i, item.Tage3 == null ? (object)DBNull.Value : item.Tage3);
						sqlCommand.Parameters.AddWithValue("Text11" + i, item.Text11 == null ? (object)DBNull.Value : item.Text11);
						sqlCommand.Parameters.AddWithValue("Text12" + i, item.Text12 == null ? (object)DBNull.Value : item.Text12);
						sqlCommand.Parameters.AddWithValue("Text21" + i, item.Text21 == null ? (object)DBNull.Value : item.Text21);
						sqlCommand.Parameters.AddWithValue("Text22" + i, item.Text22 == null ? (object)DBNull.Value : item.Text22);
						sqlCommand.Parameters.AddWithValue("Text31" + i, item.Text31 == null ? (object)DBNull.Value : item.Text31);
						sqlCommand.Parameters.AddWithValue("Text32" + i, item.Text32 == null ? (object)DBNull.Value : item.Text32);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
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
				string query = "DELETE FROM [Zahlungskonditionen] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

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

					string query = "DELETE FROM [Zahlungskonditionen] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods


		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
