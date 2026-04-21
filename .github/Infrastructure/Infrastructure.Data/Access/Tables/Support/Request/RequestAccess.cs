using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.Support.Request
{
	public class RequestAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [support].[Request] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [support].[Request]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [support].[Request] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [support].[Request] ([ValidationDate],[ValidationUserId],[Validated],[Application],[Benefits],[BuisnessProcess],[Consequences],[CreationTime],[CreationUserId],[Date],[Department],[Dependencies],[Goals],[ItConcept],[ItConditions],[ItEffort],[ItFeasibility],[ItNr],[LastEditTime],[LastEditUserId],[OtherApplication],[OtherReason],[Priority],[Problem],[Reason],[Requester],[Requirement],[Status],[Theme]) " +
					"OUTPUT INSERTED.[Id] VALUES (@ValidationDate,@ValidationUserId,@Validated,@Application,@Benefits,@BuisnessProcess,@Consequences,@CreationTime,@CreationUserId,@Date,@Department,@Dependencies,@Goals,@ItConcept,@ItConditions,@ItEffort,@ItFeasibility,@ItNr,@LastEditTime,@LastEditUserId,@OtherApplication,@OtherReason,@Priority,@Problem,@Reason,@Requester,@Requirement,@Status,@Theme); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Application", item.Application);
					sqlCommand.Parameters.AddWithValue("Benefits", item.Benefits == null ? (object)DBNull.Value : item.Benefits);
					sqlCommand.Parameters.AddWithValue("BuisnessProcess", item.BuisnessProcess == null ? (object)DBNull.Value : item.BuisnessProcess);
					sqlCommand.Parameters.AddWithValue("Consequences", item.Consequences == null ? (object)DBNull.Value : item.Consequences);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Date", item.Date);
					sqlCommand.Parameters.AddWithValue("Department", item.Department);
					sqlCommand.Parameters.AddWithValue("Dependencies", item.Dependencies == null ? (object)DBNull.Value : item.Dependencies);
					sqlCommand.Parameters.AddWithValue("Goals", item.Goals == null ? (object)DBNull.Value : item.Goals);
					sqlCommand.Parameters.AddWithValue("ItConcept", item.ItConcept == null ? (object)DBNull.Value : item.ItConcept);
					sqlCommand.Parameters.AddWithValue("ItConditions", item.ItConditions == null ? (object)DBNull.Value : item.ItConditions);
					sqlCommand.Parameters.AddWithValue("ItEffort", item.ItEffort == null ? (object)DBNull.Value : item.ItEffort);
					sqlCommand.Parameters.AddWithValue("ItFeasibility", item.ItFeasibility == null ? (object)DBNull.Value : item.ItFeasibility);
					sqlCommand.Parameters.AddWithValue("ItNr", item.ItNr == null ? (object)DBNull.Value : item.ItNr);
					sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("OtherApplication", item.OtherApplication == null ? (object)DBNull.Value : item.OtherApplication);
					sqlCommand.Parameters.AddWithValue("OtherReason", item.OtherReason == null ? (object)DBNull.Value : item.OtherReason);
					sqlCommand.Parameters.AddWithValue("Priority", item.Priority == null ? (object)DBNull.Value : item.Priority);
					sqlCommand.Parameters.AddWithValue("Problem", item.Problem == null ? (object)DBNull.Value : item.Problem);
					sqlCommand.Parameters.AddWithValue("Reason", item.Reason);
					sqlCommand.Parameters.AddWithValue("Requester", item.Requester);
					sqlCommand.Parameters.AddWithValue("Requirement", item.Requirement == null ? (object)DBNull.Value : item.Requirement);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Theme", item.Theme == null ? (object)DBNull.Value : item.Theme);
					sqlCommand.Parameters.AddWithValue("Validated", item.Validated == null ? (object)DBNull.Value : item.Validated);
					sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
					sqlCommand.Parameters.AddWithValue("ValidationDate", item.ValidationDate == null ? (object)DBNull.Value : item.ValidationDate);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> items)
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
						query += " INSERT INTO [support].[Request] ([ValidationDate],[ValidationUserId],[Validated],[Application],[Benefits],[BuisnessProcess],[Consequences],[CreationTime],[CreationUserId],[Date],[Department],[Dependencies],[Goals],[ItConcept],[ItConditions],[ItEffort],[ItFeasibility],[ItNr],[LastEditTime],[LastEditUserId],[OtherApplication],[OtherReason],[Priority],[Problem],[Reason],[Requester],[Requirement],[Status],[Theme]) VALUES ( "

							+ "@ValidationDate" + i + ","
							+ "@ValidationUserId" + i + ","
							+ "@Validated" + i + ","
							+ "@Application" + i + ","
							+ "@Benefits" + i + ","
							+ "@BuisnessProcess" + i + ","
							+ "@Consequences" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@Date" + i + ","
							+ "@Department" + i + ","
							+ "@Dependencies" + i + ","
							+ "@Goals" + i + ","
							+ "@ItConcept" + i + ","
							+ "@ItConditions" + i + ","
							+ "@ItEffort" + i + ","
							+ "@ItFeasibility" + i + ","
							+ "@ItNr" + i + ","
							+ "@LastEditTime" + i + ","
							+ "@LastEditUserId" + i + ","
							+ "@OtherApplication" + i + ","
							+ "@OtherReason" + i + ","
							+ "@Priority" + i + ","
							+ "@Problem" + i + ","
							+ "@Reason" + i + ","
							+ "@Requester" + i + ","
							+ "@Requirement" + i + ","
							+ "@Status" + i + ","
							+ "@Theme" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Application" + i, item.Application);
						sqlCommand.Parameters.AddWithValue("Benefits" + i, item.Benefits == null ? (object)DBNull.Value : item.Benefits);
						sqlCommand.Parameters.AddWithValue("BuisnessProcess" + i, item.BuisnessProcess == null ? (object)DBNull.Value : item.BuisnessProcess);
						sqlCommand.Parameters.AddWithValue("Consequences" + i, item.Consequences == null ? (object)DBNull.Value : item.Consequences);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Date" + i, item.Date);
						sqlCommand.Parameters.AddWithValue("Department" + i, item.Department);
						sqlCommand.Parameters.AddWithValue("Dependencies" + i, item.Dependencies == null ? (object)DBNull.Value : item.Dependencies);
						sqlCommand.Parameters.AddWithValue("Goals" + i, item.Goals == null ? (object)DBNull.Value : item.Goals);
						sqlCommand.Parameters.AddWithValue("ItConcept" + i, item.ItConcept == null ? (object)DBNull.Value : item.ItConcept);
						sqlCommand.Parameters.AddWithValue("ItConditions" + i, item.ItConditions == null ? (object)DBNull.Value : item.ItConditions);
						sqlCommand.Parameters.AddWithValue("ItEffort" + i, item.ItEffort == null ? (object)DBNull.Value : item.ItEffort);
						sqlCommand.Parameters.AddWithValue("ItFeasibility" + i, item.ItFeasibility == null ? (object)DBNull.Value : item.ItFeasibility);
						sqlCommand.Parameters.AddWithValue("ItNr" + i, item.ItNr == null ? (object)DBNull.Value : item.ItNr);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("OtherApplication" + i, item.OtherApplication == null ? (object)DBNull.Value : item.OtherApplication);
						sqlCommand.Parameters.AddWithValue("OtherReason" + i, item.OtherReason == null ? (object)DBNull.Value : item.OtherReason);
						sqlCommand.Parameters.AddWithValue("Priority" + i, item.Priority == null ? (object)DBNull.Value : item.Priority);
						sqlCommand.Parameters.AddWithValue("Problem" + i, item.Problem == null ? (object)DBNull.Value : item.Problem);
						sqlCommand.Parameters.AddWithValue("Reason" + i, item.Reason);
						sqlCommand.Parameters.AddWithValue("Requester" + i, item.Requester);
						sqlCommand.Parameters.AddWithValue("Requirement" + i, item.Requirement == null ? (object)DBNull.Value : item.Requirement);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("Theme" + i, item.Theme == null ? (object)DBNull.Value : item.Theme);
						sqlCommand.Parameters.AddWithValue("Validated", item.Validated == null ? (object)DBNull.Value : item.Validated);
						sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
						sqlCommand.Parameters.AddWithValue("ValidationDate", item.ValidationDate == null ? (object)DBNull.Value : item.ValidationDate);

					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [support].[Request] SET  [ValidationDate]=@ValidationDate,[ValidationUserId]=@ValidationUserId,[Validated]=@Validated,[Application]=@Application, [Benefits]=@Benefits, [BuisnessProcess]=@BuisnessProcess, [Consequences]=@Consequences, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [Date]=@Date, [Department]=@Department, [Dependencies]=@Dependencies, [Goals]=@Goals, [ItConcept]=@ItConcept, [ItConditions]=@ItConditions, [ItEffort]=@ItEffort, [ItFeasibility]=@ItFeasibility, [ItNr]=@ItNr, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [OtherApplication]=@OtherApplication, [OtherReason]=@OtherReason, [Priority]=@Priority, [Problem]=@Problem, [Reason]=@Reason, [Requester]=@Requester, [Requirement]=@Requirement, [Status]=@Status, [Theme]=@Theme WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Application", item.Application);
				sqlCommand.Parameters.AddWithValue("Benefits", item.Benefits == null ? (object)DBNull.Value : item.Benefits);
				sqlCommand.Parameters.AddWithValue("BuisnessProcess", item.BuisnessProcess == null ? (object)DBNull.Value : item.BuisnessProcess);
				sqlCommand.Parameters.AddWithValue("Consequences", item.Consequences == null ? (object)DBNull.Value : item.Consequences);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("Date", item.Date);
				sqlCommand.Parameters.AddWithValue("Department", item.Department);
				sqlCommand.Parameters.AddWithValue("Dependencies", item.Dependencies == null ? (object)DBNull.Value : item.Dependencies);
				sqlCommand.Parameters.AddWithValue("Goals", item.Goals == null ? (object)DBNull.Value : item.Goals);
				sqlCommand.Parameters.AddWithValue("ItConcept", item.ItConcept == null ? (object)DBNull.Value : item.ItConcept);
				sqlCommand.Parameters.AddWithValue("ItConditions", item.ItConditions == null ? (object)DBNull.Value : item.ItConditions);
				sqlCommand.Parameters.AddWithValue("ItEffort", item.ItEffort == null ? (object)DBNull.Value : item.ItEffort);
				sqlCommand.Parameters.AddWithValue("ItFeasibility", item.ItFeasibility == null ? (object)DBNull.Value : item.ItFeasibility);
				sqlCommand.Parameters.AddWithValue("ItNr", item.ItNr == null ? (object)DBNull.Value : item.ItNr);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("OtherApplication", item.OtherApplication == null ? (object)DBNull.Value : item.OtherApplication);
				sqlCommand.Parameters.AddWithValue("OtherReason", item.OtherReason == null ? (object)DBNull.Value : item.OtherReason);
				sqlCommand.Parameters.AddWithValue("Priority", item.Priority == null ? (object)DBNull.Value : item.Priority);
				sqlCommand.Parameters.AddWithValue("Problem", item.Problem == null ? (object)DBNull.Value : item.Problem);
				sqlCommand.Parameters.AddWithValue("Reason", item.Reason);
				sqlCommand.Parameters.AddWithValue("Requester", item.Requester);
				sqlCommand.Parameters.AddWithValue("Requirement", item.Requirement == null ? (object)DBNull.Value : item.Requirement);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("Theme", item.Theme == null ? (object)DBNull.Value : item.Theme);
				sqlCommand.Parameters.AddWithValue("Validated", item.Validated == null ? (object)DBNull.Value : item.Validated);
				sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
				sqlCommand.Parameters.AddWithValue("ValidationDate", item.ValidationDate == null ? (object)DBNull.Value : item.ValidationDate);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> items)
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
						query += " UPDATE [support].[Request] SET "

							+ "[Validated]=@Validated" + i + ","
							+ "[ValidationUserId]=@ValidationUserId" + i + ","
							+ "[ValidationDate]=@ValidationDate" + i + ","
							+ "[Application]=@Application" + i + ","
							+ "[Benefits]=@Benefits" + i + ","
							+ "[BuisnessProcess]=@BuisnessProcess" + i + ","
							+ "[Consequences]=@Consequences" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[Date]=@Date" + i + ","
							+ "[Department]=@Department" + i + ","
							+ "[Dependencies]=@Dependencies" + i + ","
							+ "[Goals]=@Goals" + i + ","
							+ "[ItConcept]=@ItConcept" + i + ","
							+ "[ItConditions]=@ItConditions" + i + ","
							+ "[ItEffort]=@ItEffort" + i + ","
							+ "[ItFeasibility]=@ItFeasibility" + i + ","
							+ "[ItNr]=@ItNr" + i + ","
							+ "[LastEditTime]=@LastEditTime" + i + ","
							+ "[LastEditUserId]=@LastEditUserId" + i + ","
							+ "[OtherApplication]=@OtherApplication" + i + ","
							+ "[OtherReason]=@OtherReason" + i + ","
							+ "[Priority]=@Priority" + i + ","
							+ "[Problem]=@Problem" + i + ","
							+ "[Reason]=@Reason" + i + ","
							+ "[Requester]=@Requester" + i + ","
							+ "[Requirement]=@Requirement" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[Theme]=@Theme" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Application" + i, item.Application);
						sqlCommand.Parameters.AddWithValue("Benefits" + i, item.Benefits == null ? (object)DBNull.Value : item.Benefits);
						sqlCommand.Parameters.AddWithValue("BuisnessProcess" + i, item.BuisnessProcess == null ? (object)DBNull.Value : item.BuisnessProcess);
						sqlCommand.Parameters.AddWithValue("Consequences" + i, item.Consequences == null ? (object)DBNull.Value : item.Consequences);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Date" + i, item.Date);
						sqlCommand.Parameters.AddWithValue("Department" + i, item.Department);
						sqlCommand.Parameters.AddWithValue("Dependencies" + i, item.Dependencies == null ? (object)DBNull.Value : item.Dependencies);
						sqlCommand.Parameters.AddWithValue("Goals" + i, item.Goals == null ? (object)DBNull.Value : item.Goals);
						sqlCommand.Parameters.AddWithValue("ItConcept" + i, item.ItConcept == null ? (object)DBNull.Value : item.ItConcept);
						sqlCommand.Parameters.AddWithValue("ItConditions" + i, item.ItConditions == null ? (object)DBNull.Value : item.ItConditions);
						sqlCommand.Parameters.AddWithValue("ItEffort" + i, item.ItEffort == null ? (object)DBNull.Value : item.ItEffort);
						sqlCommand.Parameters.AddWithValue("ItFeasibility" + i, item.ItFeasibility == null ? (object)DBNull.Value : item.ItFeasibility);
						sqlCommand.Parameters.AddWithValue("ItNr" + i, item.ItNr == null ? (object)DBNull.Value : item.ItNr);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("OtherApplication" + i, item.OtherApplication == null ? (object)DBNull.Value : item.OtherApplication);
						sqlCommand.Parameters.AddWithValue("OtherReason" + i, item.OtherReason == null ? (object)DBNull.Value : item.OtherReason);
						sqlCommand.Parameters.AddWithValue("Priority" + i, item.Priority == null ? (object)DBNull.Value : item.Priority);
						sqlCommand.Parameters.AddWithValue("Problem" + i, item.Problem == null ? (object)DBNull.Value : item.Problem);
						sqlCommand.Parameters.AddWithValue("Reason" + i, item.Reason);
						sqlCommand.Parameters.AddWithValue("Requester" + i, item.Requester);
						sqlCommand.Parameters.AddWithValue("Requirement" + i, item.Requirement == null ? (object)DBNull.Value : item.Requirement);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("Theme" + i, item.Theme == null ? (object)DBNull.Value : item.Theme);
						sqlCommand.Parameters.AddWithValue("Validated", item.Validated == null ? (object)DBNull.Value : item.Validated);
						sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
						sqlCommand.Parameters.AddWithValue("ValidationDate", item.ValidationDate == null ? (object)DBNull.Value : item.ValidationDate);

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
				string query = "DELETE FROM [support].[Request] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [support].[Request] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [support].[Request] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [support].[Request]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [support].[Request] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [support].[Request] ([ValidationDate],[ValidationUserId],[Validated],[Application],[Benefits],[BuisnessProcess],[Consequences],[CreationTime],[CreationUserId],[Date],[Department],[Dependencies],[Goals],[ItConcept],[ItConditions],[ItEffort],[ItFeasibility],[ItNr],[LastEditTime],[LastEditUserId],[OtherApplication],[OtherReason],[Priority],[Problem],[Reason],[Requester],[Requirement],[Status],[Theme]) " +
				"OUTPUT INSERTED.[Id] VALUES (@ValidationDate,@ValidationUserId,@Validated,@Application,@Benefits,@BuisnessProcess,@Consequences,@CreationTime,@CreationUserId,@Date,@Department,@Dependencies,@Goals,@ItConcept,@ItConditions,@ItEffort,@ItFeasibility,@ItNr,@LastEditTime,@LastEditUserId,@OtherApplication,@OtherReason,@Priority,@Problem,@Reason,@Requester,@Requirement,@Status,@Theme); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Application", item.Application);
			sqlCommand.Parameters.AddWithValue("Benefits", item.Benefits == null ? (object)DBNull.Value : item.Benefits);
			sqlCommand.Parameters.AddWithValue("BuisnessProcess", item.BuisnessProcess == null ? (object)DBNull.Value : item.BuisnessProcess);
			sqlCommand.Parameters.AddWithValue("Consequences", item.Consequences == null ? (object)DBNull.Value : item.Consequences);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Date", item.Date);
			sqlCommand.Parameters.AddWithValue("Department", item.Department);
			sqlCommand.Parameters.AddWithValue("Dependencies", item.Dependencies == null ? (object)DBNull.Value : item.Dependencies);
			sqlCommand.Parameters.AddWithValue("Goals", item.Goals == null ? (object)DBNull.Value : item.Goals);
			sqlCommand.Parameters.AddWithValue("ItConcept", item.ItConcept == null ? (object)DBNull.Value : item.ItConcept);
			sqlCommand.Parameters.AddWithValue("ItConditions", item.ItConditions == null ? (object)DBNull.Value : item.ItConditions);
			sqlCommand.Parameters.AddWithValue("ItEffort", item.ItEffort == null ? (object)DBNull.Value : item.ItEffort);
			sqlCommand.Parameters.AddWithValue("ItFeasibility", item.ItFeasibility == null ? (object)DBNull.Value : item.ItFeasibility);
			sqlCommand.Parameters.AddWithValue("ItNr", item.ItNr == null ? (object)DBNull.Value : item.ItNr);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("OtherApplication", item.OtherApplication == null ? (object)DBNull.Value : item.OtherApplication);
			sqlCommand.Parameters.AddWithValue("OtherReason", item.OtherReason == null ? (object)DBNull.Value : item.OtherReason);
			sqlCommand.Parameters.AddWithValue("Priority", item.Priority == null ? (object)DBNull.Value : item.Priority);
			sqlCommand.Parameters.AddWithValue("Problem", item.Problem == null ? (object)DBNull.Value : item.Problem);
			sqlCommand.Parameters.AddWithValue("Reason", item.Reason);
			sqlCommand.Parameters.AddWithValue("Requester", item.Requester);
			sqlCommand.Parameters.AddWithValue("Requirement", item.Requirement == null ? (object)DBNull.Value : item.Requirement);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("Theme", item.Theme == null ? (object)DBNull.Value : item.Theme);
			sqlCommand.Parameters.AddWithValue("Validated", item.Validated == null ? (object)DBNull.Value : item.Validated);
			sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
			sqlCommand.Parameters.AddWithValue("ValidationDate", item.ValidationDate == null ? (object)DBNull.Value : item.ValidationDate);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [support].[Request] ([Validated],[ValidationUserId],[ValidationDate],[Application],[Benefits],[BuisnessProcess],[Consequences],[CreationTime],[CreationUserId],[Date],[Department],[Dependencies],[Goals],[ItConcept],[ItConditions],[ItEffort],[ItFeasibility],[ItNr],[LastEditTime],[LastEditUserId],[OtherApplication],[OtherReason],[Priority],[Problem],[Reason],[Requester],[Requirement],[Status],[Theme]) VALUES ( "

						+ "@Validated" + i + ","
						+ "@ValidationUserId" + i + ","
						+ "@ValidationDate" + i + ","
						+ "@Application" + i + ","
						+ "@Benefits" + i + ","
						+ "@BuisnessProcess" + i + ","
						+ "@Consequences" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@Date" + i + ","
						+ "@Department" + i + ","
						+ "@Dependencies" + i + ","
						+ "@Goals" + i + ","
						+ "@ItConcept" + i + ","
						+ "@ItConditions" + i + ","
						+ "@ItEffort" + i + ","
						+ "@ItFeasibility" + i + ","
						+ "@ItNr" + i + ","
						+ "@LastEditTime" + i + ","
						+ "@LastEditUserId" + i + ","
						+ "@OtherApplication" + i + ","
						+ "@OtherReason" + i + ","
						+ "@Priority" + i + ","
						+ "@Problem" + i + ","
						+ "@Reason" + i + ","
						+ "@Requester" + i + ","
						+ "@Requirement" + i + ","
						+ "@Status" + i + ","
						+ "@Theme" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Application" + i, item.Application);
					sqlCommand.Parameters.AddWithValue("Benefits" + i, item.Benefits == null ? (object)DBNull.Value : item.Benefits);
					sqlCommand.Parameters.AddWithValue("BuisnessProcess" + i, item.BuisnessProcess == null ? (object)DBNull.Value : item.BuisnessProcess);
					sqlCommand.Parameters.AddWithValue("Consequences" + i, item.Consequences == null ? (object)DBNull.Value : item.Consequences);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Date" + i, item.Date);
					sqlCommand.Parameters.AddWithValue("Department" + i, item.Department);
					sqlCommand.Parameters.AddWithValue("Dependencies" + i, item.Dependencies == null ? (object)DBNull.Value : item.Dependencies);
					sqlCommand.Parameters.AddWithValue("Goals" + i, item.Goals == null ? (object)DBNull.Value : item.Goals);
					sqlCommand.Parameters.AddWithValue("ItConcept" + i, item.ItConcept == null ? (object)DBNull.Value : item.ItConcept);
					sqlCommand.Parameters.AddWithValue("ItConditions" + i, item.ItConditions == null ? (object)DBNull.Value : item.ItConditions);
					sqlCommand.Parameters.AddWithValue("ItEffort" + i, item.ItEffort == null ? (object)DBNull.Value : item.ItEffort);
					sqlCommand.Parameters.AddWithValue("ItFeasibility" + i, item.ItFeasibility == null ? (object)DBNull.Value : item.ItFeasibility);
					sqlCommand.Parameters.AddWithValue("ItNr" + i, item.ItNr == null ? (object)DBNull.Value : item.ItNr);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("OtherApplication" + i, item.OtherApplication == null ? (object)DBNull.Value : item.OtherApplication);
					sqlCommand.Parameters.AddWithValue("OtherReason" + i, item.OtherReason == null ? (object)DBNull.Value : item.OtherReason);
					sqlCommand.Parameters.AddWithValue("Priority" + i, item.Priority == null ? (object)DBNull.Value : item.Priority);
					sqlCommand.Parameters.AddWithValue("Problem" + i, item.Problem == null ? (object)DBNull.Value : item.Problem);
					sqlCommand.Parameters.AddWithValue("Reason" + i, item.Reason);
					sqlCommand.Parameters.AddWithValue("Requester" + i, item.Requester);
					sqlCommand.Parameters.AddWithValue("Requirement" + i, item.Requirement == null ? (object)DBNull.Value : item.Requirement);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Theme" + i, item.Theme == null ? (object)DBNull.Value : item.Theme);
					sqlCommand.Parameters.AddWithValue("Validated", item.Validated == null ? (object)DBNull.Value : item.Validated);
					sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
					sqlCommand.Parameters.AddWithValue("ValidationDate", item.ValidationDate == null ? (object)DBNull.Value : item.ValidationDate);

				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [support].[Request] SET [ValidationUserId]=@ValidationUserId,[ValidationDate]=@ValidationDate,[Validated]=@Validated,[Application]=@Application, [Benefits]=@Benefits, [BuisnessProcess]=@BuisnessProcess, [Consequences]=@Consequences, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [Date]=@Date, [Department]=@Department, [Dependencies]=@Dependencies, [Goals]=@Goals, [ItConcept]=@ItConcept, [ItConditions]=@ItConditions, [ItEffort]=@ItEffort, [ItFeasibility]=@ItFeasibility, [ItNr]=@ItNr, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [OtherApplication]=@OtherApplication, [OtherReason]=@OtherReason, [Priority]=@Priority, [Problem]=@Problem, [Reason]=@Reason, [Requester]=@Requester, [Requirement]=@Requirement, [Status]=@Status, [Theme]=@Theme WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Application", item.Application);
			sqlCommand.Parameters.AddWithValue("Benefits", item.Benefits == null ? (object)DBNull.Value : item.Benefits);
			sqlCommand.Parameters.AddWithValue("BuisnessProcess", item.BuisnessProcess == null ? (object)DBNull.Value : item.BuisnessProcess);
			sqlCommand.Parameters.AddWithValue("Consequences", item.Consequences == null ? (object)DBNull.Value : item.Consequences);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Date", item.Date);
			sqlCommand.Parameters.AddWithValue("Department", item.Department);
			sqlCommand.Parameters.AddWithValue("Dependencies", item.Dependencies == null ? (object)DBNull.Value : item.Dependencies);
			sqlCommand.Parameters.AddWithValue("Goals", item.Goals == null ? (object)DBNull.Value : item.Goals);
			sqlCommand.Parameters.AddWithValue("ItConcept", item.ItConcept == null ? (object)DBNull.Value : item.ItConcept);
			sqlCommand.Parameters.AddWithValue("ItConditions", item.ItConditions == null ? (object)DBNull.Value : item.ItConditions);
			sqlCommand.Parameters.AddWithValue("ItEffort", item.ItEffort == null ? (object)DBNull.Value : item.ItEffort);
			sqlCommand.Parameters.AddWithValue("ItFeasibility", item.ItFeasibility == null ? (object)DBNull.Value : item.ItFeasibility);
			sqlCommand.Parameters.AddWithValue("ItNr", item.ItNr == null ? (object)DBNull.Value : item.ItNr);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("OtherApplication", item.OtherApplication == null ? (object)DBNull.Value : item.OtherApplication);
			sqlCommand.Parameters.AddWithValue("OtherReason", item.OtherReason == null ? (object)DBNull.Value : item.OtherReason);
			sqlCommand.Parameters.AddWithValue("Priority", item.Priority == null ? (object)DBNull.Value : item.Priority);
			sqlCommand.Parameters.AddWithValue("Problem", item.Problem == null ? (object)DBNull.Value : item.Problem);
			sqlCommand.Parameters.AddWithValue("Reason", item.Reason);
			sqlCommand.Parameters.AddWithValue("Requester", item.Requester);
			sqlCommand.Parameters.AddWithValue("Requirement", item.Requirement == null ? (object)DBNull.Value : item.Requirement);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("Theme", item.Theme == null ? (object)DBNull.Value : item.Theme);
			sqlCommand.Parameters.AddWithValue("Validated", item.Validated == null ? (object)DBNull.Value : item.Validated);
			sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
			sqlCommand.Parameters.AddWithValue("ValidationDate", item.ValidationDate == null ? (object)DBNull.Value : item.ValidationDate);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [support].[Request] SET "

					+ "[Validated]=@Validated" + i + ","
					+ "[ValidationUserId]=@ValidationUserId" + i + ","
					+ "[ValidationDate]=@ValidationDate" + i + ","
					+ "[Application]=@Application" + i + ","
					+ "[Benefits]=@Benefits" + i + ","
					+ "[BuisnessProcess]=@BuisnessProcess" + i + ","
					+ "[Consequences]=@Consequences" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[Date]=@Date" + i + ","
					+ "[Department]=@Department" + i + ","
					+ "[Dependencies]=@Dependencies" + i + ","
					+ "[Goals]=@Goals" + i + ","
					+ "[ItConcept]=@ItConcept" + i + ","
					+ "[ItConditions]=@ItConditions" + i + ","
					+ "[ItEffort]=@ItEffort" + i + ","
					+ "[ItFeasibility]=@ItFeasibility" + i + ","
					+ "[ItNr]=@ItNr" + i + ","
					+ "[LastEditTime]=@LastEditTime" + i + ","
					+ "[LastEditUserId]=@LastEditUserId" + i + ","
					+ "[OtherApplication]=@OtherApplication" + i + ","
					+ "[OtherReason]=@OtherReason" + i + ","
					+ "[Priority]=@Priority" + i + ","
					+ "[Problem]=@Problem" + i + ","
					+ "[Reason]=@Reason" + i + ","
					+ "[Requester]=@Requester" + i + ","
					+ "[Requirement]=@Requirement" + i + ","
					+ "[Status]=@Status" + i + ","
					+ "[Theme]=@Theme" + i + " WHERE [Id]=@Id" + i
						+ "; ";
					sqlCommand.Parameters.AddWithValue("Validated", item.Validated == null ? (object)DBNull.Value : item.Validated);
					sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
					sqlCommand.Parameters.AddWithValue("ValidationDate", item.ValidationDate == null ? (object)DBNull.Value : item.ValidationDate);
					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("Application" + i, item.Application);
					sqlCommand.Parameters.AddWithValue("Benefits" + i, item.Benefits == null ? (object)DBNull.Value : item.Benefits);
					sqlCommand.Parameters.AddWithValue("BuisnessProcess" + i, item.BuisnessProcess == null ? (object)DBNull.Value : item.BuisnessProcess);
					sqlCommand.Parameters.AddWithValue("Consequences" + i, item.Consequences == null ? (object)DBNull.Value : item.Consequences);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Date" + i, item.Date);
					sqlCommand.Parameters.AddWithValue("Department" + i, item.Department);
					sqlCommand.Parameters.AddWithValue("Dependencies" + i, item.Dependencies == null ? (object)DBNull.Value : item.Dependencies);
					sqlCommand.Parameters.AddWithValue("Goals" + i, item.Goals == null ? (object)DBNull.Value : item.Goals);
					sqlCommand.Parameters.AddWithValue("ItConcept" + i, item.ItConcept == null ? (object)DBNull.Value : item.ItConcept);
					sqlCommand.Parameters.AddWithValue("ItConditions" + i, item.ItConditions == null ? (object)DBNull.Value : item.ItConditions);
					sqlCommand.Parameters.AddWithValue("ItEffort" + i, item.ItEffort == null ? (object)DBNull.Value : item.ItEffort);
					sqlCommand.Parameters.AddWithValue("ItFeasibility" + i, item.ItFeasibility == null ? (object)DBNull.Value : item.ItFeasibility);
					sqlCommand.Parameters.AddWithValue("ItNr" + i, item.ItNr == null ? (object)DBNull.Value : item.ItNr);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("OtherApplication" + i, item.OtherApplication == null ? (object)DBNull.Value : item.OtherApplication);
					sqlCommand.Parameters.AddWithValue("OtherReason" + i, item.OtherReason == null ? (object)DBNull.Value : item.OtherReason);
					sqlCommand.Parameters.AddWithValue("Priority" + i, item.Priority == null ? (object)DBNull.Value : item.Priority);
					sqlCommand.Parameters.AddWithValue("Problem" + i, item.Problem == null ? (object)DBNull.Value : item.Problem);
					sqlCommand.Parameters.AddWithValue("Reason" + i, item.Reason);
					sqlCommand.Parameters.AddWithValue("Requester" + i, item.Requester);
					sqlCommand.Parameters.AddWithValue("Requirement" + i, item.Requirement == null ? (object)DBNull.Value : item.Requirement);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Theme" + i, item.Theme == null ? (object)DBNull.Value : item.Theme);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [support].[Request] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


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

				string query = "DELETE FROM [support].[Request] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}


		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity> GetByUsers(List<int> userIds)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [support].[Request] WHERE CreationUserId in ({string.Join<int>(",", userIds)})";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
			}
		}


		#endregion Custom Methods

	}
}
