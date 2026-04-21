using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CRP
{
	public class ForecastsPositionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [ForecastsPosition] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [ForecastsPosition]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [ForecastsPosition] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [ForecastsPosition] ([ArtikelNr],[Artikelnummer],[Datum],[GesamtPreis],[IdForcast],[IsOrdered],[Jahr],[KW],[Material],[Menge],[VKE]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@Artikelnummer,@Datum,@GesamtPreis,@IdForcast,@IsOrdered,@Jahr,@KW,@Material,@Menge,@VKE); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("GesamtPreis", item.GesamtPreis == null ? (object)DBNull.Value : item.GesamtPreis);
					sqlCommand.Parameters.AddWithValue("IdForcast", item.IdForcast == null ? (object)DBNull.Value : item.IdForcast);
					sqlCommand.Parameters.AddWithValue("IsOrdered", item.IsOrdered == null ? (object)DBNull.Value : item.IsOrdered);
					sqlCommand.Parameters.AddWithValue("Jahr", item.Jahr == null ? (object)DBNull.Value : item.Jahr);
					sqlCommand.Parameters.AddWithValue("KW", item.KW == null ? (object)DBNull.Value : item.KW);
					sqlCommand.Parameters.AddWithValue("Material", item.Material == null ? (object)DBNull.Value : item.Material);
					sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("VKE", item.VKE == null ? (object)DBNull.Value : item.VKE);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> items)
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
						query += " INSERT INTO [ForecastsPosition] ([ArtikelNr],[Artikelnummer],[Datum],[GesamtPreis],[IdForcast],[IsOrdered],[Jahr],[KW],[Material],[Menge],[VKE]) VALUES ( "

							+ "@ArtikelNr" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Datum" + i + ","
							+ "@GesamtPreis" + i + ","
							+ "@IdForcast" + i + ","
							+ "@IsOrdered" + i + ","
							+ "@Jahr" + i + ","
							+ "@KW" + i + ","
							+ "@Material" + i + ","
							+ "@Menge" + i + ","
							+ "@VKE" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("GesamtPreis" + i, item.GesamtPreis == null ? (object)DBNull.Value : item.GesamtPreis);
						sqlCommand.Parameters.AddWithValue("IdForcast" + i, item.IdForcast == null ? (object)DBNull.Value : item.IdForcast);
						sqlCommand.Parameters.AddWithValue("IsOrdered" + i, item.IsOrdered == null ? (object)DBNull.Value : item.IsOrdered);
						sqlCommand.Parameters.AddWithValue("Jahr" + i, item.Jahr == null ? (object)DBNull.Value : item.Jahr);
						sqlCommand.Parameters.AddWithValue("KW" + i, item.KW == null ? (object)DBNull.Value : item.KW);
						sqlCommand.Parameters.AddWithValue("Material" + i, item.Material == null ? (object)DBNull.Value : item.Material);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("VKE" + i, item.VKE == null ? (object)DBNull.Value : item.VKE);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [ForecastsPosition] SET [ArtikelNr]=@ArtikelNr, [Artikelnummer]=@Artikelnummer, [Datum]=@Datum, [GesamtPreis]=@GesamtPreis, [IdForcast]=@IdForcast, [IsOrdered]=@IsOrdered, [Jahr]=@Jahr, [KW]=@KW, [Material]=@Material, [Menge]=@Menge, [VKE]=@VKE WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("GesamtPreis", item.GesamtPreis == null ? (object)DBNull.Value : item.GesamtPreis);
				sqlCommand.Parameters.AddWithValue("IdForcast", item.IdForcast == null ? (object)DBNull.Value : item.IdForcast);
				sqlCommand.Parameters.AddWithValue("IsOrdered", item.IsOrdered == null ? (object)DBNull.Value : item.IsOrdered);
				sqlCommand.Parameters.AddWithValue("Jahr", item.Jahr == null ? (object)DBNull.Value : item.Jahr);
				sqlCommand.Parameters.AddWithValue("KW", item.KW == null ? (object)DBNull.Value : item.KW);
				sqlCommand.Parameters.AddWithValue("Material", item.Material == null ? (object)DBNull.Value : item.Material);
				sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
				sqlCommand.Parameters.AddWithValue("VKE", item.VKE == null ? (object)DBNull.Value : item.VKE);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> items)
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
						query += " UPDATE [ForecastsPosition] SET "

							+ "[ArtikelNr]=@ArtikelNr" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[GesamtPreis]=@GesamtPreis" + i + ","
							+ "[IdForcast]=@IdForcast" + i + ","
							+ "[IsOrdered]=@IsOrdered" + i + ","
							+ "[Jahr]=@Jahr" + i + ","
							+ "[KW]=@KW" + i + ","
							+ "[Material]=@Material" + i + ","
							+ "[Menge]=@Menge" + i + ","
							+ "[VKE]=@VKE" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("GesamtPreis" + i, item.GesamtPreis == null ? (object)DBNull.Value : item.GesamtPreis);
						sqlCommand.Parameters.AddWithValue("IdForcast" + i, item.IdForcast == null ? (object)DBNull.Value : item.IdForcast);
						sqlCommand.Parameters.AddWithValue("IsOrdered" + i, item.IsOrdered == null ? (object)DBNull.Value : item.IsOrdered);
						sqlCommand.Parameters.AddWithValue("Jahr" + i, item.Jahr == null ? (object)DBNull.Value : item.Jahr);
						sqlCommand.Parameters.AddWithValue("KW" + i, item.KW == null ? (object)DBNull.Value : item.KW);
						sqlCommand.Parameters.AddWithValue("Material" + i, item.Material == null ? (object)DBNull.Value : item.Material);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("VKE" + i, item.VKE == null ? (object)DBNull.Value : item.VKE);
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
				string query = "DELETE FROM [ForecastsPosition] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

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

					string query = "DELETE FROM [ForecastsPosition] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [ForecastsPosition] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [ForecastsPosition]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [ForecastsPosition] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [ForecastsPosition] ([ArtikelNr],[Artikelnummer],[Datum],[GesamtPreis],[IdForcast],[IsOrdered],[Jahr],[KW],[Material],[Menge],[VKE]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@Artikelnummer,@Datum,@GesamtPreis,@IdForcast,@IsOrdered,@Jahr,@KW,@Material,@Menge,@VKE); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("GesamtPreis", item.GesamtPreis == null ? (object)DBNull.Value : item.GesamtPreis);
			sqlCommand.Parameters.AddWithValue("IdForcast", item.IdForcast == null ? (object)DBNull.Value : item.IdForcast);
			sqlCommand.Parameters.AddWithValue("IsOrdered", item.IsOrdered == null ? (object)DBNull.Value : item.IsOrdered);
			sqlCommand.Parameters.AddWithValue("Jahr", item.Jahr == null ? (object)DBNull.Value : item.Jahr);
			sqlCommand.Parameters.AddWithValue("KW", item.KW == null ? (object)DBNull.Value : item.KW);
			sqlCommand.Parameters.AddWithValue("Material", item.Material == null ? (object)DBNull.Value : item.Material);
			sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
			sqlCommand.Parameters.AddWithValue("VKE", item.VKE == null ? (object)DBNull.Value : item.VKE);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [ForecastsPosition] ([ArtikelNr],[Artikelnummer],[Datum],[GesamtPreis],[IdForcast],[IsOrdered],[Jahr],[KW],[Material],[Menge],[VKE]) VALUES ( "

						+ "@ArtikelNr" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Datum" + i + ","
						+ "@GesamtPreis" + i + ","
						+ "@IdForcast" + i + ","
						+ "@IsOrdered" + i + ","
						+ "@Jahr" + i + ","
						+ "@KW" + i + ","
						+ "@Material" + i + ","
						+ "@Menge" + i + ","
						+ "@VKE" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("GesamtPreis" + i, item.GesamtPreis == null ? (object)DBNull.Value : item.GesamtPreis);
					sqlCommand.Parameters.AddWithValue("IdForcast" + i, item.IdForcast == null ? (object)DBNull.Value : item.IdForcast);
					sqlCommand.Parameters.AddWithValue("IsOrdered" + i, item.IsOrdered == null ? (object)DBNull.Value : item.IsOrdered);
					sqlCommand.Parameters.AddWithValue("Jahr" + i, item.Jahr == null ? (object)DBNull.Value : item.Jahr);
					sqlCommand.Parameters.AddWithValue("KW" + i, item.KW == null ? (object)DBNull.Value : item.KW);
					sqlCommand.Parameters.AddWithValue("Material" + i, item.Material == null ? (object)DBNull.Value : item.Material);
					sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("VKE" + i, item.VKE == null ? (object)DBNull.Value : item.VKE);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [ForecastsPosition] SET [ArtikelNr]=@ArtikelNr, [Artikelnummer]=@Artikelnummer, [Datum]=@Datum, [GesamtPreis]=@GesamtPreis, [IdForcast]=@IdForcast, [IsOrdered]=@IsOrdered, [Jahr]=@Jahr, [KW]=@KW, [Material]=@Material, [Menge]=@Menge, [VKE]=@VKE WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("GesamtPreis", item.GesamtPreis == null ? (object)DBNull.Value : item.GesamtPreis);
			sqlCommand.Parameters.AddWithValue("IdForcast", item.IdForcast == null ? (object)DBNull.Value : item.IdForcast);
			sqlCommand.Parameters.AddWithValue("IsOrdered", item.IsOrdered == null ? (object)DBNull.Value : item.IsOrdered);
			sqlCommand.Parameters.AddWithValue("Jahr", item.Jahr == null ? (object)DBNull.Value : item.Jahr);
			sqlCommand.Parameters.AddWithValue("KW", item.KW == null ? (object)DBNull.Value : item.KW);
			sqlCommand.Parameters.AddWithValue("Material", item.Material == null ? (object)DBNull.Value : item.Material);
			sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
			sqlCommand.Parameters.AddWithValue("VKE", item.VKE == null ? (object)DBNull.Value : item.VKE);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [ForecastsPosition] SET "

					+ "[ArtikelNr]=@ArtikelNr" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[Datum]=@Datum" + i + ","
					+ "[GesamtPreis]=@GesamtPreis" + i + ","
					+ "[IdForcast]=@IdForcast" + i + ","
					+ "[IsOrdered]=@IsOrdered" + i + ","
					+ "[Jahr]=@Jahr" + i + ","
					+ "[KW]=@KW" + i + ","
					+ "[Material]=@Material" + i + ","
					+ "[Menge]=@Menge" + i + ","
					+ "[VKE]=@VKE" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("GesamtPreis" + i, item.GesamtPreis == null ? (object)DBNull.Value : item.GesamtPreis);
					sqlCommand.Parameters.AddWithValue("IdForcast" + i, item.IdForcast == null ? (object)DBNull.Value : item.IdForcast);
					sqlCommand.Parameters.AddWithValue("IsOrdered" + i, item.IsOrdered == null ? (object)DBNull.Value : item.IsOrdered);
					sqlCommand.Parameters.AddWithValue("Jahr" + i, item.Jahr == null ? (object)DBNull.Value : item.Jahr);
					sqlCommand.Parameters.AddWithValue("KW" + i, item.KW == null ? (object)DBNull.Value : item.KW);
					sqlCommand.Parameters.AddWithValue("Material" + i, item.Material == null ? (object)DBNull.Value : item.Material);
					sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("VKE" + i, item.VKE == null ? (object)DBNull.Value : item.VKE);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [ForecastsPosition] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

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

				string query = "DELETE FROM [ForecastsPosition] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> GetByForecastId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [ForecastsPosition] WHERE [IdForcast]=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int ToggleOrdered(int itemId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [ForecastsPosition] SET [IsOrdered]=1-ISNULL(IsOrdered,0) /*, [Menge]=[Menge] * CAST(ISNULL(IsOrdered,0) AS INT) */WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", itemId);

			return sqlCommand.ExecuteNonQuery();
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity> GetByForecastIdPaginated(int id, string searchText, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [ForecastsPosition] WHERE [IdForcast]=@id";
				if(!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
				{
					query += @$" AND ([Artikelnummer] LIKE '%{searchText}%' OR [Material] LIKE '%{searchText}%' OR CONVERT(varchar,Menge) LIKE '%{searchText}%' OR CONVERT(varchar,Datum)='%{searchText}%'
                             OR CONVERT(varchar,Jahr) LIKE '%{searchText}%' OR CONVERT(varchar,KW) LIKE '%{searchText}%' OR CONVERT(varchar,VKE) LIKE '%{searchText}%' 
                             OR CONVERT(varchar,GesamtPreis) LIKE '%{searchText}%')";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [Artikelnummer] DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int GetByForecastIdPaginatedCount(int id, string searchText)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM [ForecastsPosition] WHERE [IdForcast]=@id";
				if(!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
				{
					query += @$" AND ([Artikelnummer] LIKE '%{searchText}%' OR [Material] LIKE '%{searchText}%' OR CONVERT(varchar,Menge) LIKE '%{searchText}%' OR CONVERT(varchar,Datum)='%{searchText}%'
                             OR CONVERT(varchar,Jahr) LIKE '%{searchText}%' OR CONVERT(varchar,KW) LIKE '%{searchText}%' OR CONVERT(varchar,VKE) LIKE '%{searchText}%' 
                             OR CONVERT(varchar,GesamtPreis) LIKE '%{searchText}%')";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
			}
		}

		#endregion Custom Methods

	}
}
