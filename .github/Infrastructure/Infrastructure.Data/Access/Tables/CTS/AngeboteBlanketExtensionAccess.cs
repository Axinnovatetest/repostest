using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class AngeboteBlanketExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AngeboteBlanketExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AngeboteBlanketExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__CTS_AngeboteBlanketExtension] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__CTS_AngeboteBlanketExtension] ([AngeboteNr],[Anhage],[Archived],[ArchiveTime],[ArchiveUserId],[Auftraggeber],[BlanketTypeId],[BlanketTypeName],[CreateTime],[CreateUserId],[CustomerId],[CustomerName],[Gesamtpreis],[GesamtpreisDefault],[LastEditTime],[LastEditUserId],[StatusId],[StatusName],[SupplierId],[SupplierName],[Warenemfanger]) OUTPUT INSERTED.[Id] VALUES (@AngeboteNr,@Anhage,@Archived,@ArchiveTime,@ArchiveUserId,@Auftraggeber,@BlanketTypeId,@BlanketTypeName,@CreateTime,@CreateUserId,@CustomerId,@CustomerName,@Gesamtpreis,@GesamtpreisDefault,@LastEditTime,@LastEditUserId,@StatusId,@StatusName,@SupplierId,@SupplierName,@Warenemfanger); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AngeboteNr", item.AngeboteNr);
					sqlCommand.Parameters.AddWithValue("Anhage", item.Anhage == null ? (object)DBNull.Value : item.Anhage);
					sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
					sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Auftraggeber", item.Auftraggeber == null ? (object)DBNull.Value : item.Auftraggeber);
					sqlCommand.Parameters.AddWithValue("BlanketTypeId", item.BlanketTypeId == null ? (object)DBNull.Value : item.BlanketTypeId);
					sqlCommand.Parameters.AddWithValue("BlanketTypeName", item.BlanketTypeName == null ? (object)DBNull.Value : item.BlanketTypeName);
					sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("GesamtpreisDefault", item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
					sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
					sqlCommand.Parameters.AddWithValue("StatusName", item.StatusName == null ? (object)DBNull.Value : item.StatusName);
					sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
					sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
					sqlCommand.Parameters.AddWithValue("Warenemfanger", item.Warenemfanger == null ? (object)DBNull.Value : item.Warenemfanger);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> items)
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
						query += " INSERT INTO [__CTS_AngeboteBlanketExtension] ([AngeboteNr],[Anhage],[Archived],[ArchiveTime],[ArchiveUserId],[Auftraggeber],[BlanketTypeId],[BlanketTypeName],[CreateTime],[CreateUserId],[CustomerId],[CustomerName],[Gesamtpreis],[GesamtpreisDefault],[LastEditTime],[LastEditUserId],[StatusId],[StatusName],[SupplierId],[SupplierName],[Warenemfanger]) VALUES ( "

							+ "@AngeboteNr" + i + ","
							+ "@Anhage" + i + ","
							+ "@Archived" + i + ","
							+ "@ArchiveTime" + i + ","
							+ "@ArchiveUserId" + i + ","
							+ "@Auftraggeber" + i + ","
							+ "@BlanketTypeId" + i + ","
							+ "@BlanketTypeName" + i + ","
							+ "@CreateTime" + i + ","
							+ "@CreateUserId" + i + ","
							+ "@CustomerId" + i + ","
							+ "@CustomerName" + i + ","
							+ "@Gesamtpreis" + i + ","
							+ "@GesamtpreisDefault" + i + ","
							+ "@LastEditTime" + i + ","
							+ "@LastEditUserId" + i + ","
							+ "@StatusId" + i + ","
							+ "@StatusName" + i + ","
							+ "@SupplierId" + i + ","
							+ "@SupplierName" + i + ","
							+ "@Warenemfanger" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AngeboteNr" + i, item.AngeboteNr);
						sqlCommand.Parameters.AddWithValue("Anhage" + i, item.Anhage == null ? (object)DBNull.Value : item.Anhage);
						sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("Auftraggeber" + i, item.Auftraggeber == null ? (object)DBNull.Value : item.Auftraggeber);
						sqlCommand.Parameters.AddWithValue("BlanketTypeId" + i, item.BlanketTypeId == null ? (object)DBNull.Value : item.BlanketTypeId);
						sqlCommand.Parameters.AddWithValue("BlanketTypeName" + i, item.BlanketTypeName == null ? (object)DBNull.Value : item.BlanketTypeName);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("GesamtpreisDefault" + i, item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
						sqlCommand.Parameters.AddWithValue("StatusName" + i, item.StatusName == null ? (object)DBNull.Value : item.StatusName);
						sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
						sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
						sqlCommand.Parameters.AddWithValue("Warenemfanger" + i, item.Warenemfanger == null ? (object)DBNull.Value : item.Warenemfanger);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_AngeboteBlanketExtension] SET [AngeboteNr]=@AngeboteNr, [Anhage]=@Anhage, [Archived]=@Archived, [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [Auftraggeber]=@Auftraggeber, [BlanketTypeId]=@BlanketTypeId, [BlanketTypeName]=@BlanketTypeName, [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [CustomerId]=@CustomerId, [CustomerName]=@CustomerName, [Gesamtpreis]=@Gesamtpreis, [GesamtpreisDefault]=@GesamtpreisDefault, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [StatusId]=@StatusId, [StatusName]=@StatusName, [SupplierId]=@SupplierId, [SupplierName]=@SupplierName, [Warenemfanger]=@Warenemfanger WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AngeboteNr", item.AngeboteNr);
				sqlCommand.Parameters.AddWithValue("Anhage", item.Anhage == null ? (object)DBNull.Value : item.Anhage);
				sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("Auftraggeber", item.Auftraggeber == null ? (object)DBNull.Value : item.Auftraggeber);
				sqlCommand.Parameters.AddWithValue("BlanketTypeId", item.BlanketTypeId == null ? (object)DBNull.Value : item.BlanketTypeId);
				sqlCommand.Parameters.AddWithValue("BlanketTypeName", item.BlanketTypeName == null ? (object)DBNull.Value : item.BlanketTypeName);
				sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
				sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId);
				sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
				sqlCommand.Parameters.AddWithValue("GesamtpreisDefault", item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
				sqlCommand.Parameters.AddWithValue("StatusName", item.StatusName == null ? (object)DBNull.Value : item.StatusName);
				sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
				sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
				sqlCommand.Parameters.AddWithValue("Warenemfanger", item.Warenemfanger == null ? (object)DBNull.Value : item.Warenemfanger);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> items)
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
						query += " UPDATE [__CTS_AngeboteBlanketExtension] SET "

							+ "[AngeboteNr]=@AngeboteNr" + i + ","
							+ "[Anhage]=@Anhage" + i + ","
							+ "[Archived]=@Archived" + i + ","
							+ "[ArchiveTime]=@ArchiveTime" + i + ","
							+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
							+ "[Auftraggeber]=@Auftraggeber" + i + ","
							+ "[BlanketTypeId]=@BlanketTypeId" + i + ","
							+ "[BlanketTypeName]=@BlanketTypeName" + i + ","
							+ "[CreateTime]=@CreateTime" + i + ","
							+ "[CreateUserId]=@CreateUserId" + i + ","
							+ "[CustomerId]=@CustomerId" + i + ","
							+ "[CustomerName]=@CustomerName" + i + ","
							+ "[Gesamtpreis]=@Gesamtpreis" + i + ","
							+ "[GesamtpreisDefault]=@GesamtpreisDefault" + i + ","
							+ "[LastEditTime]=@LastEditTime" + i + ","
							+ "[LastEditUserId]=@LastEditUserId" + i + ","
							+ "[StatusId]=@StatusId" + i + ","
							+ "[StatusName]=@StatusName" + i + ","
							+ "[SupplierId]=@SupplierId" + i + ","
							+ "[SupplierName]=@SupplierName" + i + ","
							+ "[Warenemfanger]=@Warenemfanger" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AngeboteNr" + i, item.AngeboteNr);
						sqlCommand.Parameters.AddWithValue("Anhage" + i, item.Anhage == null ? (object)DBNull.Value : item.Anhage);
						sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("Auftraggeber" + i, item.Auftraggeber == null ? (object)DBNull.Value : item.Auftraggeber);
						sqlCommand.Parameters.AddWithValue("BlanketTypeId" + i, item.BlanketTypeId == null ? (object)DBNull.Value : item.BlanketTypeId);
						sqlCommand.Parameters.AddWithValue("BlanketTypeName" + i, item.BlanketTypeName == null ? (object)DBNull.Value : item.BlanketTypeName);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("GesamtpreisDefault" + i, item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
						sqlCommand.Parameters.AddWithValue("StatusName" + i, item.StatusName == null ? (object)DBNull.Value : item.StatusName);
						sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
						sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
						sqlCommand.Parameters.AddWithValue("Warenemfanger" + i, item.Warenemfanger == null ? (object)DBNull.Value : item.Warenemfanger);
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
				string query = "DELETE FROM [__CTS_AngeboteBlanketExtension] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__CTS_AngeboteBlanketExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_AngeboteBlanketExtension] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_AngeboteBlanketExtension]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__CTS_AngeboteBlanketExtension] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "INSERT INTO [__CTS_AngeboteBlanketExtension] ([AngeboteNr],[Anhage],[Archived],[ArchiveTime],[ArchiveUserId],[Auftraggeber],[BlanketTypeId],[BlanketTypeName],[CreateTime],[CreateUserId],[CustomerId],[CustomerName],[Gesamtpreis],[GesamtpreisDefault],[LastEditTime],[LastEditUserId],[StatusId],[StatusName],[SupplierId],[SupplierName],[Warenemfanger]) OUTPUT INSERTED.[Id] VALUES (@AngeboteNr,@Anhage,@Archived,@ArchiveTime,@ArchiveUserId,@Auftraggeber,@BlanketTypeId,@BlanketTypeName,@CreateTime,@CreateUserId,@CustomerId,@CustomerName,@Gesamtpreis,@GesamtpreisDefault,@LastEditTime,@LastEditUserId,@StatusId,@StatusName,@SupplierId,@SupplierName,@Warenemfanger); ";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("AngeboteNr", item.AngeboteNr);
				sqlCommand.Parameters.AddWithValue("Anhage", item.Anhage == null ? (object)DBNull.Value : item.Anhage);
				sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("Auftraggeber", item.Auftraggeber == null ? (object)DBNull.Value : item.Auftraggeber);
				sqlCommand.Parameters.AddWithValue("BlanketTypeId", item.BlanketTypeId == null ? (object)DBNull.Value : item.BlanketTypeId);
				sqlCommand.Parameters.AddWithValue("BlanketTypeName", item.BlanketTypeName == null ? (object)DBNull.Value : item.BlanketTypeName);
				sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
				sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId);
				sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
				sqlCommand.Parameters.AddWithValue("GesamtpreisDefault", item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
				sqlCommand.Parameters.AddWithValue("StatusName", item.StatusName == null ? (object)DBNull.Value : item.StatusName);
				sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
				sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
				sqlCommand.Parameters.AddWithValue("Warenemfanger", item.Warenemfanger == null ? (object)DBNull.Value : item.Warenemfanger);

				var result = sqlCommand.ExecuteScalar();
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__CTS_AngeboteBlanketExtension] ([AngeboteNr],[Anhage],[Archived],[ArchiveTime],[ArchiveUserId],[Auftraggeber],[BlanketTypeId],[BlanketTypeName],[CreateTime],[CreateUserId],[CustomerId],[CustomerName],[Gesamtpreis],[GesamtpreisDefault],[LastEditTime],[LastEditUserId],[StatusId],[StatusName],[SupplierId],[SupplierName],[Warenemfanger]) VALUES ( "

						+ "@AngeboteNr" + i + ","
						+ "@Anhage" + i + ","
						+ "@Archived" + i + ","
						+ "@ArchiveTime" + i + ","
						+ "@ArchiveUserId" + i + ","
						+ "@Auftraggeber" + i + ","
						+ "@BlanketTypeId" + i + ","
						+ "@BlanketTypeName" + i + ","
						+ "@CreateTime" + i + ","
						+ "@CreateUserId" + i + ","
						+ "@CustomerId" + i + ","
						+ "@CustomerName" + i + ","
						+ "@Gesamtpreis" + i + ","
						+ "@GesamtpreisDefault" + i + ","
						+ "@LastEditTime" + i + ","
						+ "@LastEditUserId" + i + ","
						+ "@StatusId" + i + ","
						+ "@StatusName" + i + ","
						+ "@SupplierId" + i + ","
						+ "@SupplierName" + i + ","
						+ "@Warenemfanger" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AngeboteNr" + i, item.AngeboteNr);
					sqlCommand.Parameters.AddWithValue("Anhage" + i, item.Anhage == null ? (object)DBNull.Value : item.Anhage);
					sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
					sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Auftraggeber" + i, item.Auftraggeber == null ? (object)DBNull.Value : item.Auftraggeber);
					sqlCommand.Parameters.AddWithValue("BlanketTypeId" + i, item.BlanketTypeId == null ? (object)DBNull.Value : item.BlanketTypeId);
					sqlCommand.Parameters.AddWithValue("BlanketTypeName" + i, item.BlanketTypeName == null ? (object)DBNull.Value : item.BlanketTypeName);
					sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("GesamtpreisDefault" + i, item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
					sqlCommand.Parameters.AddWithValue("StatusName" + i, item.StatusName == null ? (object)DBNull.Value : item.StatusName);
					sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
					sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
					sqlCommand.Parameters.AddWithValue("Warenemfanger" + i, item.Warenemfanger == null ? (object)DBNull.Value : item.Warenemfanger);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__CTS_AngeboteBlanketExtension] SET [AngeboteNr]=@AngeboteNr, [Anhage]=@Anhage, [Archived]=@Archived, [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [Auftraggeber]=@Auftraggeber, [BlanketTypeId]=@BlanketTypeId, [BlanketTypeName]=@BlanketTypeName, [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [CustomerId]=@CustomerId, [CustomerName]=@CustomerName, [Gesamtpreis]=@Gesamtpreis, [GesamtpreisDefault]=@GesamtpreisDefault, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [StatusId]=@StatusId, [StatusName]=@StatusName, [SupplierId]=@SupplierId, [SupplierName]=@SupplierName, [Warenemfanger]=@Warenemfanger WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AngeboteNr", item.AngeboteNr);
			sqlCommand.Parameters.AddWithValue("Anhage", item.Anhage == null ? (object)DBNull.Value : item.Anhage);
			sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
			sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
			sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
			sqlCommand.Parameters.AddWithValue("Auftraggeber", item.Auftraggeber == null ? (object)DBNull.Value : item.Auftraggeber);
			sqlCommand.Parameters.AddWithValue("BlanketTypeId", item.BlanketTypeId == null ? (object)DBNull.Value : item.BlanketTypeId);
			sqlCommand.Parameters.AddWithValue("BlanketTypeName", item.BlanketTypeName == null ? (object)DBNull.Value : item.BlanketTypeName);
			sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
			sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId);
			sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("GesamtpreisDefault", item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
			sqlCommand.Parameters.AddWithValue("StatusName", item.StatusName == null ? (object)DBNull.Value : item.StatusName);
			sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
			sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
			sqlCommand.Parameters.AddWithValue("Warenemfanger", item.Warenemfanger == null ? (object)DBNull.Value : item.Warenemfanger);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__CTS_AngeboteBlanketExtension] SET "

					+ "[AngeboteNr]=@AngeboteNr" + i + ","
					+ "[Anhage]=@Anhage" + i + ","
					+ "[Archived]=@Archived" + i + ","
					+ "[ArchiveTime]=@ArchiveTime" + i + ","
					+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
					+ "[Auftraggeber]=@Auftraggeber" + i + ","
					+ "[BlanketTypeId]=@BlanketTypeId" + i + ","
					+ "[BlanketTypeName]=@BlanketTypeName" + i + ","
					+ "[CreateTime]=@CreateTime" + i + ","
					+ "[CreateUserId]=@CreateUserId" + i + ","
					+ "[CustomerId]=@CustomerId" + i + ","
					+ "[CustomerName]=@CustomerName" + i + ","
					+ "[Gesamtpreis]=@Gesamtpreis" + i + ","
					+ "[GesamtpreisDefault]=@GesamtpreisDefault" + i + ","
					+ "[LastEditTime]=@LastEditTime" + i + ","
					+ "[LastEditUserId]=@LastEditUserId" + i + ","
					+ "[StatusId]=@StatusId" + i + ","
					+ "[StatusName]=@StatusName" + i + ","
					+ "[SupplierId]=@SupplierId" + i + ","
					+ "[SupplierName]=@SupplierName" + i + ","
					+ "[Warenemfanger]=@Warenemfanger" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AngeboteNr" + i, item.AngeboteNr);
					sqlCommand.Parameters.AddWithValue("Anhage" + i, item.Anhage == null ? (object)DBNull.Value : item.Anhage);
					sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
					sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Auftraggeber" + i, item.Auftraggeber == null ? (object)DBNull.Value : item.Auftraggeber);
					sqlCommand.Parameters.AddWithValue("BlanketTypeId" + i, item.BlanketTypeId == null ? (object)DBNull.Value : item.BlanketTypeId);
					sqlCommand.Parameters.AddWithValue("BlanketTypeName" + i, item.BlanketTypeName == null ? (object)DBNull.Value : item.BlanketTypeName);
					sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("GesamtpreisDefault" + i, item.GesamtpreisDefault == null ? (object)DBNull.Value : item.GesamtpreisDefault);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
					sqlCommand.Parameters.AddWithValue("StatusName" + i, item.StatusName == null ? (object)DBNull.Value : item.StatusName);
					sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
					sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
					sqlCommand.Parameters.AddWithValue("Warenemfanger" + i, item.Warenemfanger == null ? (object)DBNull.Value : item.Warenemfanger);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__CTS_AngeboteBlanketExtension] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__CTS_AngeboteBlanketExtension] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods


		//string queryIds = string.Empty;
		//for (int i = 0 ; i < ids.Count ; i++)
		//{
		//	queryIds += "@Id" + i + ",";
		//	sqlCommand.Parameters.AddWithValue("Id" + i , ids [i]);
		//}
		//queryIds = queryIds.TrimEnd(',');

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity GetByAngeboteNr(int id)

		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AngeboteBlanketExtension] WHERE [AngeboteNr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity GetByAngeboteNr(int id, SqlConnection connection, SqlTransaction transaction)

		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [__CTS_AngeboteBlanketExtension] WHERE [AngeboteNr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);


			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> GetByAngeboteNrs(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getByAngeboteNrs(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByAngeboteNrs(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getByAngeboteNrs(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
		}
		public static int DeleteByAngeboteNr(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__CTS_AngeboteBlanketExtension] WHERE [AngeboteNr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int UpdateBlaqnketImage(int AngeboteNr, int newImageId)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_AngeboteBlanketExtension] SET [Anhage]=@Anhage WHERE [AngeboteNr]=@AngeboteNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("AngeboteNr", AngeboteNr);
				sqlCommand.Parameters.AddWithValue("Anhage", newImageId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> getByAngeboteNrs(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__CTS_AngeboteBlanketExtension] WHERE [AngeboteNr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> GetByTypeAndSupplier(string document, int typ, int supplierNr)
		{
			var dataTable = new DataTable();
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();
			string query = "SELECT * FROM [__CTS_AngeboteBlanketExtension] C inner join [Angebote] A on A.[Nr]=C.[AngeboteNr] WHERE LOWER(LTRIM(RTRIM(A.[Bezug])))=@document AND C.[SupplierId]=@supplierNr AND C.[BlanketTypeId]=@typ";
			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("document", document);
			sqlCommand.Parameters.AddWithValue("supplierNr", supplierNr);
			sqlCommand.Parameters.AddWithValue("typ", typ);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity> GetByStatus(int statusId, int typeId, bool? archived = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__CTS_AngeboteBlanketExtension] Where {(!archived.HasValue ? "" : $"IsNULL([Archived],0)={(archived.Value ? 1 : 0)} AND ")} StatusId=@statusId AND BlanketTypeId={typeId}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("statusId", statusId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();
			}
		}
		#endregion
	}
}
