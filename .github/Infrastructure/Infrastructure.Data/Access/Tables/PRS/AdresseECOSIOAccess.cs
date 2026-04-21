using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class AdresseECOSIOAccess
	{
		public static bool IsECOSIO { get; set; } = true;

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_AdresseECOSIO] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_AdresseECOSIO]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [EDI_AdresseECOSIO] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [EDI_AdresseECOSIO] ([AnlieferLagerort],[Bezeichnung],[DUNSNummer],[Firma],[LOrt],[LPLZ],[LStrasse],[ROrt],[RPLZ],[RStrasse],[Werksnummer])  VALUES (@AnlieferLagerort,@Bezeichnung,@DUNSNummer,@Firma,@LOrt,@LPLZ,@LStrasse,@ROrt,@RPLZ,@RStrasse,@Werksnummer);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AnlieferLagerort", item.AnlieferLagerort == null ? (object)DBNull.Value : item.AnlieferLagerort);
					sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("DUNSNummer", item.DUNSNummer == null ? (object)DBNull.Value : item.DUNSNummer);
					sqlCommand.Parameters.AddWithValue("Firma", item.Firma == null ? (object)DBNull.Value : item.Firma);
					sqlCommand.Parameters.AddWithValue("LOrt", item.LOrt == null ? (object)DBNull.Value : item.LOrt);
					sqlCommand.Parameters.AddWithValue("LPLZ", item.LPLZ == null ? (object)DBNull.Value : item.LPLZ);
					sqlCommand.Parameters.AddWithValue("LStrasse", item.LStrasse == null ? (object)DBNull.Value : item.LStrasse);
					sqlCommand.Parameters.AddWithValue("ROrt", item.ROrt == null ? (object)DBNull.Value : item.ROrt);
					sqlCommand.Parameters.AddWithValue("RPLZ", item.RPLZ == null ? (object)DBNull.Value : item.RPLZ);
					sqlCommand.Parameters.AddWithValue("RStrasse", item.RStrasse == null ? (object)DBNull.Value : item.RStrasse);
					sqlCommand.Parameters.AddWithValue("Werksnummer", item.Werksnummer == null ? (object)DBNull.Value : item.Werksnummer);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity> items)
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
						query += " INSERT INTO [EDI_AdresseECOSIO] ([AnlieferLagerort],[Bezeichnung],[DUNSNummer],[Firma],[LOrt],[LPLZ],[LStrasse],[ROrt],[RPLZ],[RStrasse],[Werksnummer]) VALUES ( "

							+ "@AnlieferLagerort" + i + ","
							+ "@Bezeichnung" + i + ","
							+ "@DUNSNummer" + i + ","
							+ "@Firma" + i + ","
							+ "@LOrt" + i + ","
							+ "@LPLZ" + i + ","
							+ "@LStrasse" + i + ","
							+ "@ROrt" + i + ","
							+ "@RPLZ" + i + ","
							+ "@RStrasse" + i + ","
							+ "@Werksnummer" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AnlieferLagerort" + i, item.AnlieferLagerort == null ? (object)DBNull.Value : item.AnlieferLagerort);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("DUNSNummer" + i, item.DUNSNummer == null ? (object)DBNull.Value : item.DUNSNummer);
						sqlCommand.Parameters.AddWithValue("Firma" + i, item.Firma == null ? (object)DBNull.Value : item.Firma);
						sqlCommand.Parameters.AddWithValue("LOrt" + i, item.LOrt == null ? (object)DBNull.Value : item.LOrt);
						sqlCommand.Parameters.AddWithValue("LPLZ" + i, item.LPLZ == null ? (object)DBNull.Value : item.LPLZ);
						sqlCommand.Parameters.AddWithValue("LStrasse" + i, item.LStrasse == null ? (object)DBNull.Value : item.LStrasse);
						sqlCommand.Parameters.AddWithValue("ROrt" + i, item.ROrt == null ? (object)DBNull.Value : item.ROrt);
						sqlCommand.Parameters.AddWithValue("RPLZ" + i, item.RPLZ == null ? (object)DBNull.Value : item.RPLZ);
						sqlCommand.Parameters.AddWithValue("RStrasse" + i, item.RStrasse == null ? (object)DBNull.Value : item.RStrasse);
						sqlCommand.Parameters.AddWithValue("Werksnummer" + i, item.Werksnummer == null ? (object)DBNull.Value : item.Werksnummer);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [EDI_AdresseECOSIO] SET [AnlieferLagerort]=@AnlieferLagerort, [Bezeichnung]=@Bezeichnung, [DUNSNummer]=@DUNSNummer, [Firma]=@Firma, [LOrt]=@LOrt, [LPLZ]=@LPLZ, [LStrasse]=@LStrasse, [ROrt]=@ROrt, [RPLZ]=@RPLZ, [RStrasse]=@RStrasse, [Werksnummer]=@Werksnummer WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AnlieferLagerort", item.AnlieferLagerort == null ? (object)DBNull.Value : item.AnlieferLagerort);
				sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
				sqlCommand.Parameters.AddWithValue("DUNSNummer", item.DUNSNummer == null ? (object)DBNull.Value : item.DUNSNummer);
				sqlCommand.Parameters.AddWithValue("Firma", item.Firma == null ? (object)DBNull.Value : item.Firma);
				sqlCommand.Parameters.AddWithValue("LOrt", item.LOrt == null ? (object)DBNull.Value : item.LOrt);
				sqlCommand.Parameters.AddWithValue("LPLZ", item.LPLZ == null ? (object)DBNull.Value : item.LPLZ);
				sqlCommand.Parameters.AddWithValue("LStrasse", item.LStrasse == null ? (object)DBNull.Value : item.LStrasse);
				sqlCommand.Parameters.AddWithValue("ROrt", item.ROrt == null ? (object)DBNull.Value : item.ROrt);
				sqlCommand.Parameters.AddWithValue("RPLZ", item.RPLZ == null ? (object)DBNull.Value : item.RPLZ);
				sqlCommand.Parameters.AddWithValue("RStrasse", item.RStrasse == null ? (object)DBNull.Value : item.RStrasse);
				sqlCommand.Parameters.AddWithValue("Werksnummer", item.Werksnummer == null ? (object)DBNull.Value : item.Werksnummer);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity> items)
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
						query += " UPDATE [EDI_AdresseECOSIO] SET "

							+ "[AnlieferLagerort]=@AnlieferLagerort" + i + ","
							+ "[Bezeichnung]=@Bezeichnung" + i + ","
							+ "[DUNSNummer]=@DUNSNummer" + i + ","
							+ "[Firma]=@Firma" + i + ","
							+ "[LOrt]=@LOrt" + i + ","
							+ "[LPLZ]=@LPLZ" + i + ","
							+ "[LStrasse]=@LStrasse" + i + ","
							+ "[ROrt]=@ROrt" + i + ","
							+ "[RPLZ]=@RPLZ" + i + ","
							+ "[RStrasse]=@RStrasse" + i + ","
							+ "[Werksnummer]=@Werksnummer" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AnlieferLagerort" + i, item.AnlieferLagerort == null ? (object)DBNull.Value : item.AnlieferLagerort);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("DUNSNummer" + i, item.DUNSNummer == null ? (object)DBNull.Value : item.DUNSNummer);
						sqlCommand.Parameters.AddWithValue("Firma" + i, item.Firma == null ? (object)DBNull.Value : item.Firma);
						sqlCommand.Parameters.AddWithValue("LOrt" + i, item.LOrt == null ? (object)DBNull.Value : item.LOrt);
						sqlCommand.Parameters.AddWithValue("LPLZ" + i, item.LPLZ == null ? (object)DBNull.Value : item.LPLZ);
						sqlCommand.Parameters.AddWithValue("LStrasse" + i, item.LStrasse == null ? (object)DBNull.Value : item.LStrasse);
						sqlCommand.Parameters.AddWithValue("ROrt" + i, item.ROrt == null ? (object)DBNull.Value : item.ROrt);
						sqlCommand.Parameters.AddWithValue("RPLZ" + i, item.RPLZ == null ? (object)DBNull.Value : item.RPLZ);
						sqlCommand.Parameters.AddWithValue("RStrasse" + i, item.RStrasse == null ? (object)DBNull.Value : item.RStrasse);
						sqlCommand.Parameters.AddWithValue("Werksnummer" + i, item.Werksnummer == null ? (object)DBNull.Value : item.Werksnummer);
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
				string query = "DELETE FROM [EDI_AdresseECOSIO] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

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

					string query = "DELETE FROM [EDI_AdresseECOSIO] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity GetByDuns(long duns)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_AdresseECOSIO] WHERE [DUNSNummer]=@duns";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("duns", duns);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity GetByDuns(long duns, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [EDI_AdresseECOSIO] WHERE [DUNSNummer]=@duns";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("duns", duns);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity GetByUnloadingPoint(string unloadingPoint)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_AdresseECOSIO] WHERE [Werksnummer]=@unloadingPoint";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("unloadingPoint", unloadingPoint == null ? (object)DBNull.Value : unloadingPoint);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity GetByUnloadingPointAndStorageLocation(string unloadingPoint, string storageLocation)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_AdresseECOSIO] WHERE [Werksnummer]=TRY_CAST(@unloadingPoint AS BIGINT) AND [AnlieferLagerort]=TRY_CAST(@storageLocation AS BIGINT)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("unloadingPoint", unloadingPoint == null ? (object)DBNull.Value : unloadingPoint);
				sqlCommand.Parameters.AddWithValue("storageLocation", storageLocation == null ? (object)DBNull.Value : storageLocation);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity GetByUnloadingPointAndStorageLocation(string unloadingPoint, string storageLocation, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [EDI_AdresseECOSIO] WHERE [Werksnummer]=TRY_CAST(@unloadingPoint AS BIGINT) AND [AnlieferLagerort]=TRY_CAST(@storageLocation AS BIGINT)";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("unloadingPoint", unloadingPoint == null ? (object)DBNull.Value : unloadingPoint);
			sqlCommand.Parameters.AddWithValue("storageLocation", storageLocation == null ? (object)DBNull.Value : storageLocation);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static bool IsECOSIOByDuns(string duns)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_AdresseECOSIO] WHERE [DUNSNummer]=try_cast(@duns as bigint)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("duns", duns?.Trim());

				DbExecution.Fill(sqlCommand, dataTable);

			}

			return dataTable.Rows.Count > 0;
		}
		public static bool IsECOSIOByDuns(string duns, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [EDI_AdresseECOSIO] WHERE [DUNSNummer]=try_cast(@duns as bigint)";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("duns", duns?.Trim());

			DbExecution.Fill(sqlCommand, dataTable);

			return dataTable.Rows.Count > 0;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity> GetAllByDuns(long duns)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_AdresseECOSIO] WHERE [DUNSNummer]=@duns";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("duns", duns);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity> GetAllByDuns(long duns, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [EDI_AdresseECOSIO] WHERE [DUNSNummer]=@duns";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("duns", duns);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AdresseECOSIOEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}
