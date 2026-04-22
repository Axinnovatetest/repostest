using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class KonditionsZuordnungstabelleEntity
	{
		#region Default Methods
		public static Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Konditionszuordnungstabelle] WHERE [Nr]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Konditionszuordnungstabelle]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity>();
			}
		}
		public static List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity> Get(List<string> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					result = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					result = new List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity>();
		}
		private static List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity> get(List<string> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Konditionszuordnungstabelle] WHERE [Nr] IN (" + queryIds + ")";

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity>();
				}
			}
			return new List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity>();
		}

		public static int Insert(Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "INSERT INTO [Konditionszuordnungstabelle] "
					+ " ([Nettotage],[Skonto],[Skontotage],[Text],[Bemerkung]) "
					+ " VALUES "
					+ " (@Nettotage,@Skonto,@Skontotage,@Text,@Bemerkung)";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nettotage", element.Nettotage == null ? (object)DBNull.Value : element.Nettotage);
				sqlCommand.Parameters.AddWithValue("Skonto", element.Skonto == null ? (object)DBNull.Value : element.Skonto);
				sqlCommand.Parameters.AddWithValue("Skontotage", element.Skontotage == null ? (object)DBNull.Value : element.Skontotage);
				sqlCommand.Parameters.AddWithValue("Text", element.Text == null ? (object)DBNull.Value : element.Text);
				sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		public static int Insert(List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Data.Access.Settings.MAX_BATCH_SIZE / 7; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = insert(elements);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += insert(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += insert(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber));
				}
			}

			return -1;
		}
		private static int insert(List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int response = -1;

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					;

					string query = " INSERT INTO [Konditionszuordnungstabelle] ([Nettotage],[Nr],[Skonto],[Skontotage],[Text], [Bemerkung]) VALUES ";

					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var element in elements)
					{
						i++;
						query += " ( "

							+ "@Nettotage" + i + ","
							+ "@Nr" + i + ","
							+ "@Skonto" + i + ","
							+ "@Skontotage" + i + ","
							+ "@Text" + i + ","
							+ "@Bemerkung" + i
							+ "), ";

						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, element.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Nettotage" + i, element.Nettotage == null ? (object)DBNull.Value : element.Nettotage);
						sqlCommand.Parameters.AddWithValue("Nr" + i, element.Nr);
						sqlCommand.Parameters.AddWithValue("Skonto" + i, element.Skonto == null ? (object)DBNull.Value : element.Skonto);
						sqlCommand.Parameters.AddWithValue("Skontotage" + i, element.Skontotage == null ? (object)DBNull.Value : element.Skontotage);
						sqlCommand.Parameters.AddWithValue("Text" + i, element.Text == null ? (object)DBNull.Value : element.Text);
					}

					query = query.TrimEnd(',');
					query += ';';
					sqlCommand.CommandText = query;

					response = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return response;
			}

			return -1;
		}

		public static int Update(Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Konditionszuordnungstabelle] "
					+ " SET [Nettotage]=@Nettotage,[Bemerkung]=@Bemerkung,[Skonto]=@Skonto,[Skontotage]=@Skontotage,[Text]=@Text "
					+ " WHERE [Nr]=@Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Bemerkung", element.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Nettotage", element.Nettotage == null ? (object)DBNull.Value : element.Nettotage);
				sqlCommand.Parameters.AddWithValue("Nr", element.Nr);
				sqlCommand.Parameters.AddWithValue("Skonto", element.Skonto == null ? (object)DBNull.Value : element.Skonto);
				sqlCommand.Parameters.AddWithValue("Skontotage", element.Skontotage == null ? (object)DBNull.Value : element.Skontotage);
				sqlCommand.Parameters.AddWithValue("Text", element.Text == null ? (object)DBNull.Value : element.Text);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		public static int Update(List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Data.Access.Settings.MAX_BATCH_SIZE / 7; // Nb params per query
				int result = 0;

				if(elements.Count <= maxParamsNumber)
				{
					result = update(elements);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += update(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += update(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber));
				}
			}

			return -1;
		}
		private static int update(List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int response = -1;

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();

					string query = "";

					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var element in elements)
					{
						i++;
						query += " UPDATE [Konditionszuordnungstabelle] SET "

							+ "[Nettotage]=@Nettotage" + i + ","
							+ "[Nr]=@Nr" + i + ","
							+ "[Skonto]=@Skonto" + i + ","
							+ "[Skontotage]=@Skontotage" + i + ","
							+ "[Text]=@Text" + i + ","
							+ "[Bemerkung]=@Bemerkung" + i
							+ " WHERE [Nr]=@Id" + i
							 + "; ";

						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, element.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Nettotage" + i, element.Nettotage == null ? (object)DBNull.Value : element.Nettotage);
						sqlCommand.Parameters.AddWithValue("Nr" + i, element.Nr);
						sqlCommand.Parameters.AddWithValue("Skonto" + i, element.Skonto == null ? (object)DBNull.Value : element.Skonto);
						sqlCommand.Parameters.AddWithValue("Skontotage" + i, element.Skontotage == null ? (object)DBNull.Value : element.Skontotage);
						sqlCommand.Parameters.AddWithValue("Text" + i, element.Text == null ? (object)DBNull.Value : element.Text);
					}

					sqlCommand.CommandText = query;

					response = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return response;
			}

			return -1;
		}

		public static int Delete(string id)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [Konditionszuordnungstabelle] WHERE [Nr]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		public static int Delete(List<string> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Data.Access.Settings.MAX_BATCH_SIZE;
				int result = 0;
				if(ids.Count <= maxParamsNumber)
				{
					result = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<string> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int response = -1;

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					;
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					string query = "DELETE FROM [Konditionszuordnungstabelle] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					response = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity Get(int id, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Konditionszuordnungstabelle] WHERE [Nr]=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Helpers
		private static List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity> toList(DataTable dataTable)
		{
			var result = new List<Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
