using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class AccessProfileAccess
	{
		#region Default Methods
		public static Entities.Tables.PRS.AccessProfileEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [EDI_AccessProfile] WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.PRS.AccessProfileEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_AccessProfile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.PRS.AccessProfileEntity>();
			}
		}
		public static List<Entities.Tables.PRS.AccessProfileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.PRS.AccessProfileEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					response = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					response = new List<Entities.Tables.PRS.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					response.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return response;
			}
			return new List<Entities.Tables.PRS.AccessProfileEntity>();
		}
		private static List<Entities.Tables.PRS.AccessProfileEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [EDI_AccessProfile] WHERE [Id] IN (" + queryIds + ")";

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.PRS.AccessProfileEntity>();
				}
			}
			return new List<Entities.Tables.PRS.AccessProfileEntity>();
		}

		public static int Insert(Entities.Tables.PRS.AccessProfileEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "INSERT INTO [EDI_AccessProfile] "
					+ " ([Access],[AccessUpdate],[Customer],[CustomerUpdate],[MainAccessProfileId],[ModuleActivated],[Order],[OrderError],[OrderErrorHistory],[OrderErrorValidate],[OrderHistory],[OrderUpdate],[OrderValidate],[AllCustomers],[EDI]) "
					+ " VALUES "
					+ " (@Access,@AccessUpdate,@Customer,@CustomerUpdate,@MainAccessProfileId,@ModuleActivated,@Order,@OrderError,@OrderErrorHistory,@OrderErrorValidate,@OrderHistory,@OrderUpdate,@OrderValidate,@AllCustomers,@EDI);";
				query += "SELECT SCOPE_IDENTITY();";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Access", element.Access);
				sqlCommand.Parameters.AddWithValue("AccessUpdate", element.AccessUpdate);
				sqlCommand.Parameters.AddWithValue("Customer", element.Customer);
				sqlCommand.Parameters.AddWithValue("CustomerUpdate", element.CustomerUpdate);
				sqlCommand.Parameters.AddWithValue("MainAccessProfileId", element.MainAccessProfileId);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", element.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("Order", element.Order);
				sqlCommand.Parameters.AddWithValue("OrderError", element.OrderError);
				sqlCommand.Parameters.AddWithValue("OrderErrorHistory", element.OrderErrorHistory);
				sqlCommand.Parameters.AddWithValue("OrderErrorValidate", element.OrderErrorValidate);
				sqlCommand.Parameters.AddWithValue("OrderHistory", element.OrderHistory);
				sqlCommand.Parameters.AddWithValue("OrderUpdate", element.OrderUpdate);
				sqlCommand.Parameters.AddWithValue("OrderValidate", element.OrderValidate);

				sqlCommand.Parameters.AddWithValue("AllCustomers", element.AllCustomers);
				sqlCommand.Parameters.AddWithValue("EDI", element.EDI);


				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}

		public static int Update(Entities.Tables.PRS.AccessProfileEntity element)
		{
			int r = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [EDI_AccessProfile] SET "
					+ " [Access]=@Access,[AccessUpdate]=@AccessUpdate,[Customer]=@Customer,[CustomerUpdate]=@CustomerUpdate, "
					+ " [MainAccessProfileId]=@MainAccessProfileId,[ModuleActivated]=@ModuleActivated,[Order]=@Order,[OrderError]=@OrderError, "
					+ " [OrderErrorHistory]=@OrderErrorHistory,[OrderErrorValidate]=@OrderErrorValidate,[OrderHistory]=@OrderHistory, "
					+ " [OrderUpdate]=@OrderUpdate,[OrderValidate]=@OrderValidate, [AllCustomers]=@AllCustomers, [EDI]=@EDI  "
					+ " WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", element.Id);
				sqlCommand.Parameters.AddWithValue("Access", element.Access);
				sqlCommand.Parameters.AddWithValue("AccessUpdate", element.AccessUpdate);
				sqlCommand.Parameters.AddWithValue("Customer", element.Customer);
				sqlCommand.Parameters.AddWithValue("CustomerUpdate", element.CustomerUpdate);
				sqlCommand.Parameters.AddWithValue("MainAccessProfileId", element.MainAccessProfileId);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", element.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("Order", element.Order);
				sqlCommand.Parameters.AddWithValue("OrderError", element.OrderError);
				sqlCommand.Parameters.AddWithValue("OrderErrorHistory", element.OrderErrorHistory);
				sqlCommand.Parameters.AddWithValue("OrderErrorValidate", element.OrderErrorValidate);
				sqlCommand.Parameters.AddWithValue("OrderHistory", element.OrderHistory);
				sqlCommand.Parameters.AddWithValue("OrderUpdate", element.OrderUpdate);
				sqlCommand.Parameters.AddWithValue("OrderValidate", element.OrderValidate);

				sqlCommand.Parameters.AddWithValue("AllCustomers", element.AllCustomers);
				sqlCommand.Parameters.AddWithValue("EDI", element.EDI);

				r = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return r;
		}

		public static int Delete(int id)
		{
			try
			{
				int r = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "DELETE FROM [EDI_AccessProfile] WHERE [Id]=@Id";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("Id", id);

					r = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return r;
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				try
				{
					int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
					int response = 0;
					if(ids.Count <= maxParamsNumber)
					{
						response = delete(ids);
					}
					else
					{
						int batchNumber = ids.Count / maxParamsNumber;
						for(int i = 0; i < batchNumber; i++)
						{
							response += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
						}
						response += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
					}
				} catch(Exception Ex)
				{
					throw;
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				try
				{
					int r = -1;
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

						string query = "DELETE FROM [EDI_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
						sqlCommand.CommandText = query;

						r = DbExecution.ExecuteNonQuery(sqlCommand);
					}

					return r;
				} catch(Exception Ex)
				{
					throw;
				}
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Entities.Tables.PRS.AccessProfileEntity> GetByMainAccessProfilesIds(List<int> mainAccessProfilesIds)
		{
			if(mainAccessProfilesIds != null && mainAccessProfilesIds.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.PRS.AccessProfileEntity>();
				if(mainAccessProfilesIds.Count <= maxQueryNumber)
				{
					response = getByMainAccessProfilesIds(mainAccessProfilesIds);
				}
				else
				{
					int batchNumber = mainAccessProfilesIds.Count / maxQueryNumber;
					response = new List<Entities.Tables.PRS.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(getByMainAccessProfilesIds(mainAccessProfilesIds.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					response.AddRange(getByMainAccessProfilesIds(mainAccessProfilesIds.GetRange(batchNumber * maxQueryNumber, mainAccessProfilesIds.Count - batchNumber * maxQueryNumber)));
				}
				return response;
			}
			return new List<Entities.Tables.PRS.AccessProfileEntity>();
		}
		private static List<Entities.Tables.PRS.AccessProfileEntity> getByMainAccessProfilesIds(List<int> mainAccessProfilesIds)
		{
			if(mainAccessProfilesIds != null && mainAccessProfilesIds.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < mainAccessProfilesIds.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, mainAccessProfilesIds[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [EDI_AccessProfile] WHERE [MainAccessProfileId] IN (" + queryIds + ")";

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.PRS.AccessProfileEntity>();
				}
			}
			return new List<Entities.Tables.PRS.AccessProfileEntity>();
		}
		public static int DeleteByMainAccessProfilesId(int id)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [EDI_AccessProfile] WHERE [MainAccessProfileId]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		#endregion

		#region Helpers
		private static List<Entities.Tables.PRS.AccessProfileEntity> toList(DataTable dt)
		{
			List<Entities.Tables.PRS.AccessProfileEntity> L = new List<Entities.Tables.PRS.AccessProfileEntity>(dt.Rows.Count);
			foreach(DataRow dr in dt.Rows)
			{ L.Add(new Entities.Tables.PRS.AccessProfileEntity(dr)); }
			return L;
		}
		#endregion
	}
}
