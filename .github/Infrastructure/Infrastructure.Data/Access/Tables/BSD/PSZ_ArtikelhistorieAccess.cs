using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class PSZ_ArtikelhistorieAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Artikelhistorie] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Artikelhistorie]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_Artikelhistorie] WHERE [ID] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_Artikelhistorie] ([Änderung von],[Änderungsbereich],[Änderungsbeschreibung],[Artikel-Nr],[Datum Änderung]) OUTPUT INSERTED.[ID] VALUES (@Anderung_von,@Anderungsbereich,@Anderungsbeschreibung,@Artikel_Nr,@Datum_Anderung); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Anderung_von", item.Anderung_von == null ? (object)DBNull.Value : item.Anderung_von);
					sqlCommand.Parameters.AddWithValue("Anderungsbereich", item.Anderungsbereich == null ? (object)DBNull.Value : item.Anderungsbereich);
					sqlCommand.Parameters.AddWithValue("Anderungsbeschreibung", item.Anderungsbeschreibung == null ? (object)DBNull.Value : item.Anderungsbeschreibung);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Datum_Anderung", item.Datum_Anderung == null ? (object)DBNull.Value : item.Datum_Anderung);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> items)
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
						query += " INSERT INTO [PSZ_Artikelhistorie] ([Änderung von],[Änderungsbereich],[Änderungsbeschreibung],[Artikel-Nr],[Datum Änderung]) VALUES ( "

							+ "@Anderung_von" + i + ","
							+ "@Anderungsbereich" + i + ","
							+ "@Anderungsbeschreibung" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Datum_Anderung" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Anderung_von" + i, item.Anderung_von == null ? (object)DBNull.Value : item.Anderung_von);
						sqlCommand.Parameters.AddWithValue("Anderungsbereich" + i, item.Anderungsbereich == null ? (object)DBNull.Value : item.Anderungsbereich);
						sqlCommand.Parameters.AddWithValue("Anderungsbeschreibung" + i, item.Anderungsbeschreibung == null ? (object)DBNull.Value : item.Anderungsbeschreibung);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Datum_Anderung" + i, item.Datum_Anderung == null ? (object)DBNull.Value : item.Datum_Anderung);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_Artikelhistorie] SET [Änderung von]=@Anderung_von, [Änderungsbereich]=@Anderungsbereich, [Änderungsbeschreibung]=@Anderungsbeschreibung, [Artikel-Nr]=@Artikel_Nr, [Datum Änderung]=@Datum_Anderung WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Anderung_von", item.Anderung_von == null ? (object)DBNull.Value : item.Anderung_von);
				sqlCommand.Parameters.AddWithValue("Anderungsbereich", item.Anderungsbereich == null ? (object)DBNull.Value : item.Anderungsbereich);
				sqlCommand.Parameters.AddWithValue("Anderungsbeschreibung", item.Anderungsbeschreibung == null ? (object)DBNull.Value : item.Anderungsbeschreibung);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Datum_Anderung", item.Datum_Anderung == null ? (object)DBNull.Value : item.Datum_Anderung);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> items)
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
						query += " UPDATE [PSZ_Artikelhistorie] SET "

							+ "[Änderung von]=@Anderung_von" + i + ","
							+ "[Änderungsbereich]=@Anderungsbereich" + i + ","
							+ "[Änderungsbeschreibung]=@Anderungsbeschreibung" + i + ","
							+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
							+ "[Datum Änderung]=@Datum_Anderung" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Anderung_von" + i, item.Anderung_von == null ? (object)DBNull.Value : item.Anderung_von);
						sqlCommand.Parameters.AddWithValue("Anderungsbereich" + i, item.Anderungsbereich == null ? (object)DBNull.Value : item.Anderungsbereich);
						sqlCommand.Parameters.AddWithValue("Anderungsbeschreibung" + i, item.Anderungsbeschreibung == null ? (object)DBNull.Value : item.Anderungsbeschreibung);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Datum_Anderung" + i, item.Datum_Anderung == null ? (object)DBNull.Value : item.Datum_Anderung);
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
				string query = "DELETE FROM [PSZ_Artikelhistorie] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [PSZ_Artikelhistorie] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_Artikelhistorie] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_Artikelhistorie]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [PSZ_Artikelhistorie] WHERE [ID] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [PSZ_Artikelhistorie] ([Änderung von],[Änderungsbereich],[Änderungsbeschreibung],[Artikel-Nr],[Datum Änderung]) OUTPUT INSERTED.[ID] VALUES (@Anderung_von,@Anderungsbereich,@Anderungsbeschreibung,@Artikel_Nr,@Datum_Anderung); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Anderung_von", item.Anderung_von == null ? (object)DBNull.Value : item.Anderung_von);
			sqlCommand.Parameters.AddWithValue("Anderungsbereich", item.Anderungsbereich == null ? (object)DBNull.Value : item.Anderungsbereich);
			sqlCommand.Parameters.AddWithValue("Anderungsbeschreibung", item.Anderungsbeschreibung == null ? (object)DBNull.Value : item.Anderungsbeschreibung);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Datum_Anderung", item.Datum_Anderung == null ? (object)DBNull.Value : item.Datum_Anderung);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [PSZ_Artikelhistorie] ([Änderung von],[Änderungsbereich],[Änderungsbeschreibung],[Artikel-Nr],[Datum Änderung]) VALUES ( "

						+ "@Anderung_von" + i + ","
						+ "@Anderungsbereich" + i + ","
						+ "@Anderungsbeschreibung" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Datum_Anderung" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Anderung_von" + i, item.Anderung_von == null ? (object)DBNull.Value : item.Anderung_von);
					sqlCommand.Parameters.AddWithValue("Anderungsbereich" + i, item.Anderungsbereich == null ? (object)DBNull.Value : item.Anderungsbereich);
					sqlCommand.Parameters.AddWithValue("Anderungsbeschreibung" + i, item.Anderungsbeschreibung == null ? (object)DBNull.Value : item.Anderungsbeschreibung);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Datum_Anderung" + i, item.Datum_Anderung == null ? (object)DBNull.Value : item.Datum_Anderung);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [PSZ_Artikelhistorie] SET [Änderung von]=@Anderung_von, [Änderungsbereich]=@Anderungsbereich, [Änderungsbeschreibung]=@Anderungsbeschreibung, [Artikel-Nr]=@Artikel_Nr, [Datum Änderung]=@Datum_Anderung WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Anderung_von", item.Anderung_von == null ? (object)DBNull.Value : item.Anderung_von);
			sqlCommand.Parameters.AddWithValue("Anderungsbereich", item.Anderungsbereich == null ? (object)DBNull.Value : item.Anderungsbereich);
			sqlCommand.Parameters.AddWithValue("Anderungsbeschreibung", item.Anderungsbeschreibung == null ? (object)DBNull.Value : item.Anderungsbeschreibung);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Datum_Anderung", item.Datum_Anderung == null ? (object)DBNull.Value : item.Datum_Anderung);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [PSZ_Artikelhistorie] SET "

					+ "[Änderung von]=@Anderung_von" + i + ","
					+ "[Änderungsbereich]=@Anderungsbereich" + i + ","
					+ "[Änderungsbeschreibung]=@Anderungsbeschreibung" + i + ","
					+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
					+ "[Datum Änderung]=@Datum_Anderung" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Anderung_von" + i, item.Anderung_von == null ? (object)DBNull.Value : item.Anderung_von);
					sqlCommand.Parameters.AddWithValue("Anderungsbereich" + i, item.Anderungsbereich == null ? (object)DBNull.Value : item.Anderungsbereich);
					sqlCommand.Parameters.AddWithValue("Anderungsbeschreibung" + i, item.Anderungsbeschreibung == null ? (object)DBNull.Value : item.Anderungsbeschreibung);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Datum_Anderung" + i, item.Datum_Anderung == null ? (object)DBNull.Value : item.Datum_Anderung);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [PSZ_Artikelhistorie] WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ID", id);

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

				string query = "DELETE FROM [PSZ_Artikelhistorie] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity> GetByArticle(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Artikelhistorie] WHERE [Artikel-Nr]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();
			}
		}

		#endregion
	}
}
