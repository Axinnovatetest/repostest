using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class StaffelpreisExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StaffelpreisExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StaffelpreisExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__BSD_StaffelpreisExtension] WHERE [Id] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_StaffelpreisExtension] ([DeliveryTime],[LotSize],[PackagingQuantity],[PackagingType],[PackagingTypeId],[StaffelNr],[Type],[TypeId])  VALUES (@DeliveryTime,@LotSize,@PackagingQuantity,@PackagingType,@PackagingTypeId,@StaffelNr,@Type,@TypeId);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("DeliveryTime", item.DeliveryTime == null ? (object)DBNull.Value : item.DeliveryTime);
					sqlCommand.Parameters.AddWithValue("LotSize", item.LotSize == null ? (object)DBNull.Value : item.LotSize);
					sqlCommand.Parameters.AddWithValue("PackagingQuantity", item.PackagingQuantity == null ? (object)DBNull.Value : item.PackagingQuantity);
					sqlCommand.Parameters.AddWithValue("PackagingType", item.PackagingType == null ? (object)DBNull.Value : item.PackagingType);
					sqlCommand.Parameters.AddWithValue("PackagingTypeId", item.PackagingTypeId == null ? (object)DBNull.Value : item.PackagingTypeId);
					sqlCommand.Parameters.AddWithValue("StaffelNr", item.StaffelNr == null ? (object)DBNull.Value : item.StaffelNr);
					sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);
					sqlCommand.Parameters.AddWithValue("TypeId", item.TypeId == null ? (object)DBNull.Value : item.TypeId);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity> items)
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
						query += " INSERT INTO [__BSD_StaffelpreisExtension] ([DeliveryTime],[LotSize],[PackagingQuantity],[PackagingType],[PackagingTypeId],[StaffelNr],[Type],[TypeId]) VALUES ( "

							+ "@DeliveryTime" + i + ","
							+ "@LotSize" + i + ","
							+ "@PackagingQuantity" + i + ","
							+ "@PackagingType" + i + ","
							+ "@PackagingTypeId" + i + ","
							+ "@StaffelNr" + i + ","
							+ "@Type" + i + ","
							+ "@TypeId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("DeliveryTime" + i, item.DeliveryTime == null ? (object)DBNull.Value : item.DeliveryTime);
						sqlCommand.Parameters.AddWithValue("LotSize" + i, item.LotSize == null ? (object)DBNull.Value : item.LotSize);
						sqlCommand.Parameters.AddWithValue("PackagingQuantity" + i, item.PackagingQuantity == null ? (object)DBNull.Value : item.PackagingQuantity);
						sqlCommand.Parameters.AddWithValue("PackagingType" + i, item.PackagingType == null ? (object)DBNull.Value : item.PackagingType);
						sqlCommand.Parameters.AddWithValue("PackagingTypeId" + i, item.PackagingTypeId == null ? (object)DBNull.Value : item.PackagingTypeId);
						sqlCommand.Parameters.AddWithValue("StaffelNr" + i, item.StaffelNr == null ? (object)DBNull.Value : item.StaffelNr);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
						sqlCommand.Parameters.AddWithValue("TypeId" + i, item.TypeId == null ? (object)DBNull.Value : item.TypeId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_StaffelpreisExtension] SET [DeliveryTime]=@DeliveryTime, [LotSize]=@LotSize, [PackagingQuantity]=@PackagingQuantity, [PackagingType]=@PackagingType, [PackagingTypeId]=@PackagingTypeId, [StaffelNr]=@StaffelNr, [Type]=@Type, [TypeId]=@TypeId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("DeliveryTime", item.DeliveryTime == null ? (object)DBNull.Value : item.DeliveryTime);
				sqlCommand.Parameters.AddWithValue("LotSize", item.LotSize == null ? (object)DBNull.Value : item.LotSize);
				sqlCommand.Parameters.AddWithValue("PackagingQuantity", item.PackagingQuantity == null ? (object)DBNull.Value : item.PackagingQuantity);
				sqlCommand.Parameters.AddWithValue("PackagingType", item.PackagingType == null ? (object)DBNull.Value : item.PackagingType);
				sqlCommand.Parameters.AddWithValue("PackagingTypeId", item.PackagingTypeId == null ? (object)DBNull.Value : item.PackagingTypeId);
				sqlCommand.Parameters.AddWithValue("StaffelNr", item.StaffelNr == null ? (object)DBNull.Value : item.StaffelNr);
				sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);
				sqlCommand.Parameters.AddWithValue("TypeId", item.TypeId == null ? (object)DBNull.Value : item.TypeId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity> items)
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
						query += " UPDATE [__BSD_StaffelpreisExtension] SET "

							+ "[DeliveryTime]=@DeliveryTime" + i + ","
							+ "[LotSize]=@LotSize" + i + ","
							+ "[PackagingQuantity]=@PackagingQuantity" + i + ","
							+ "[PackagingType]=@PackagingType" + i + ","
							+ "[PackagingTypeId]=@PackagingTypeId" + i + ","
							+ "[StaffelNr]=@StaffelNr" + i + ","
							+ "[Type]=@Type" + i + ","
							+ "[TypeId]=@TypeId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("DeliveryTime" + i, item.DeliveryTime == null ? (object)DBNull.Value : item.DeliveryTime);
						sqlCommand.Parameters.AddWithValue("LotSize" + i, item.LotSize == null ? (object)DBNull.Value : item.LotSize);
						sqlCommand.Parameters.AddWithValue("PackagingQuantity" + i, item.PackagingQuantity == null ? (object)DBNull.Value : item.PackagingQuantity);
						sqlCommand.Parameters.AddWithValue("PackagingType" + i, item.PackagingType == null ? (object)DBNull.Value : item.PackagingType);
						sqlCommand.Parameters.AddWithValue("PackagingTypeId" + i, item.PackagingTypeId == null ? (object)DBNull.Value : item.PackagingTypeId);
						sqlCommand.Parameters.AddWithValue("StaffelNr" + i, item.StaffelNr == null ? (object)DBNull.Value : item.StaffelNr);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
						sqlCommand.Parameters.AddWithValue("TypeId" + i, item.TypeId == null ? (object)DBNull.Value : item.TypeId);
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
				string query = "DELETE FROM [__BSD_StaffelpreisExtension] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_StaffelpreisExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods


		public static List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity> GetStaffelNrs(List<int> nrs)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__BSD_StaffelpreisExtension] WHERE [StaffelNr] IN ({string.Join(",", nrs)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity>();
			}
		}

		public static Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity GetByStaffelPreis(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StaffelpreisExtension] WHERE [StaffelNr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}
