using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class StandardOperationDescriptionAccess
	{
		public static Boolean GetArchived { get; private set; } = false;
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity Get(int id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM StandardOperationDescription WHERE Id=@Id AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> Get()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM StandardOperationDescription WHERE Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			var sqlCommand = new SqlCommand
			{
				Connection = sqlConnection
			};

			string queryIds = string.Empty;
			for(int i = 0; i < ids.Count; i++)
			{
				queryIds += "@Id" + i + ",";
				sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
			}
			queryIds = queryIds.TrimEnd(',');

			sqlCommand.CommandText = "SELECT * FROM StandardOperationDescription WHERE Id IN (" + queryIds + ") AND Is_Archived=@IsArchived";

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			}
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [StandardOperationDescription] ([Code],[Creation_Date],[Creation_User_Id],[Delete_Date],[Delete_User_Id],[Description],[Is_Archived],[Last_Edit_Date],[Last_Edit_User_Id],[LotPiece],[MachineToolInsert],[ManuelMachinel],[Operation_Value_Adding],[ReationSetup],[RelationOperationSetup],[Remark],[Remark2],[SecondsPerSubOperation],[Setup],[StdOperationId],[TechnologieArea],[ValueAdding]) OUTPUT INSERTED.[Id] VALUES (@Code,@Creation_Date,@Creation_User_Id,@Delete_Date,@Delete_User_Id,@Description,@Is_Archived,@Last_Edit_Date,@Last_Edit_User_Id,@LotPiece,@MachineToolInsert,@ManuelMachinel,@Operation_Value_Adding,@ReationSetup,@RelationOperationSetup,@Remark,@Remark2,@SecondsPerSubOperation,@Setup,@StdOperationId,@TechnologieArea,@ValueAdding); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Code", item.Code == null ? (object)DBNull.Value : item.Code);
					sqlCommand.Parameters.AddWithValue("Creation_Date", item.Creation_Date);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.Creation_User_Id);
					sqlCommand.Parameters.AddWithValue("Delete_Date", item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
					sqlCommand.Parameters.AddWithValue("Description", item.Description);
					sqlCommand.Parameters.AddWithValue("Is_Archived", item.Is_Archived);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
					sqlCommand.Parameters.AddWithValue("LotPiece", item.LotPiece == null ? (object)DBNull.Value : item.LotPiece);
					sqlCommand.Parameters.AddWithValue("MachineToolInsert", item.MachineToolInsert == null ? (object)DBNull.Value : item.MachineToolInsert);
					sqlCommand.Parameters.AddWithValue("ManuelMachinel", item.ManuelMachinel == null ? (object)DBNull.Value : item.ManuelMachinel);
					sqlCommand.Parameters.AddWithValue("Operation_Value_Adding", item.Operation_Value_Adding);
					sqlCommand.Parameters.AddWithValue("ReationSetup", item.ReationSetup == null ? (object)DBNull.Value : item.ReationSetup);
					sqlCommand.Parameters.AddWithValue("RelationOperationSetup", item.RelationOperationSetup == null ? (object)DBNull.Value : item.RelationOperationSetup);
					sqlCommand.Parameters.AddWithValue("Remark", item.Remark == null ? (object)DBNull.Value : item.Remark);
					sqlCommand.Parameters.AddWithValue("Remark2", item.Remark2 == null ? (object)DBNull.Value : item.Remark2);
					sqlCommand.Parameters.AddWithValue("SecondsPerSubOperation", item.SecondsPerSubOperation == null ? (object)DBNull.Value : item.SecondsPerSubOperation);
					sqlCommand.Parameters.AddWithValue("Setup", item.Setup == null ? (object)DBNull.Value : item.Setup);
					sqlCommand.Parameters.AddWithValue("StdOperationId", item.StdOperationId);
					sqlCommand.Parameters.AddWithValue("TechnologieArea", item.TechnologieArea == null ? (object)DBNull.Value : item.TechnologieArea);
					sqlCommand.Parameters.AddWithValue("ValueAdding", item.ValueAdding == null ? (object)DBNull.Value : item.ValueAdding);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 23; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [StandardOperationDescription] ([Code],[Creation_Date],[Creation_User_Id],[Delete_Date],[Delete_User_Id],[Description],[Is_Archived],[Last_Edit_Date],[Last_Edit_User_Id],[LotPiece],[MachineToolInsert],[ManuelMachinel],[Operation_Value_Adding],[ReationSetup],[RelationOperationSetup],[Remark],[Remark2],[SecondsPerSubOperation],[Setup],[StdOperationId],[TechnologieArea],[ValueAdding]) VALUES ( "

							+ "@Code" + i + ","
							+ "@Creation_Date" + i + ","
							+ "@Creation_User_Id" + i + ","
							+ "@Delete_Date" + i + ","
							+ "@Delete_User_Id" + i + ","
							+ "@Description" + i + ","
							+ "@Is_Archived" + i + ","
							+ "@Last_Edit_Date" + i + ","
							+ "@Last_Edit_User_Id" + i + ","
							+ "@LotPiece" + i + ","
							+ "@MachineToolInsert" + i + ","
							+ "@ManuelMachinel" + i + ","
							+ "@Operation_Value_Adding" + i + ","
							+ "@ReationSetup" + i + ","
							+ "@RelationOperationSetup" + i + ","
							+ "@Remark" + i + ","
							+ "@Remark2" + i + ","
							+ "@SecondsPerSubOperation" + i + ","
							+ "@Setup" + i + ","
							+ "@StdOperationId" + i + ","
							+ "@TechnologieArea" + i + ","
							+ "@ValueAdding" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Code" + i, item.Code == null ? (object)DBNull.Value : item.Code);
						sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.Creation_Date);
						sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.Creation_User_Id);
						sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
						sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description);
						sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.Is_Archived);
						sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
						sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
						sqlCommand.Parameters.AddWithValue("LotPiece" + i, item.LotPiece == null ? (object)DBNull.Value : item.LotPiece);
						sqlCommand.Parameters.AddWithValue("MachineToolInsert" + i, item.MachineToolInsert == null ? (object)DBNull.Value : item.MachineToolInsert);
						sqlCommand.Parameters.AddWithValue("ManuelMachinel" + i, item.ManuelMachinel == null ? (object)DBNull.Value : item.ManuelMachinel);
						sqlCommand.Parameters.AddWithValue("Operation_Value_Adding" + i, item.Operation_Value_Adding);
						sqlCommand.Parameters.AddWithValue("ReationSetup" + i, item.ReationSetup == null ? (object)DBNull.Value : item.ReationSetup);
						sqlCommand.Parameters.AddWithValue("RelationOperationSetup" + i, item.RelationOperationSetup == null ? (object)DBNull.Value : item.RelationOperationSetup);
						sqlCommand.Parameters.AddWithValue("Remark" + i, item.Remark == null ? (object)DBNull.Value : item.Remark);
						sqlCommand.Parameters.AddWithValue("Remark2" + i, item.Remark2 == null ? (object)DBNull.Value : item.Remark2);
						sqlCommand.Parameters.AddWithValue("SecondsPerSubOperation" + i, item.SecondsPerSubOperation == null ? (object)DBNull.Value : item.SecondsPerSubOperation);
						sqlCommand.Parameters.AddWithValue("Setup" + i, item.Setup == null ? (object)DBNull.Value : item.Setup);
						sqlCommand.Parameters.AddWithValue("StdOperationId" + i, item.StdOperationId);
						sqlCommand.Parameters.AddWithValue("TechnologieArea" + i, item.TechnologieArea == null ? (object)DBNull.Value : item.TechnologieArea);
						sqlCommand.Parameters.AddWithValue("ValueAdding" + i, item.ValueAdding == null ? (object)DBNull.Value : item.ValueAdding);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [StandardOperationDescription] SET [Code]=@Code, [Creation_Date]=@Creation_Date, [Creation_User_Id]=@Creation_User_Id, [Delete_Date]=@Delete_Date, [Delete_User_Id]=@Delete_User_Id, [Description]=@Description, [Is_Archived]=@Is_Archived, [Last_Edit_Date]=@Last_Edit_Date, [Last_Edit_User_Id]=@Last_Edit_User_Id, [LotPiece]=@LotPiece, [MachineToolInsert]=@MachineToolInsert, [ManuelMachinel]=@ManuelMachinel, [Operation_Value_Adding]=@Operation_Value_Adding, [ReationSetup]=@ReationSetup, [RelationOperationSetup]=@RelationOperationSetup, [Remark]=@Remark, [Remark2]=@Remark2, [SecondsPerSubOperation]=@SecondsPerSubOperation, [Setup]=@Setup, [StdOperationId]=@StdOperationId, [TechnologieArea]=@TechnologieArea, [ValueAdding]=@ValueAdding WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Code", item.Code == null ? (object)DBNull.Value : item.Code);
				sqlCommand.Parameters.AddWithValue("Creation_Date", item.Creation_Date);
				sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.Creation_User_Id);
				sqlCommand.Parameters.AddWithValue("Delete_Date", item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
				sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
				sqlCommand.Parameters.AddWithValue("Description", item.Description);
				sqlCommand.Parameters.AddWithValue("Is_Archived", item.Is_Archived);
				sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
				sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
				sqlCommand.Parameters.AddWithValue("LotPiece", item.LotPiece == null ? (object)DBNull.Value : item.LotPiece);
				sqlCommand.Parameters.AddWithValue("MachineToolInsert", item.MachineToolInsert == null ? (object)DBNull.Value : item.MachineToolInsert);
				sqlCommand.Parameters.AddWithValue("ManuelMachinel", item.ManuelMachinel == null ? (object)DBNull.Value : item.ManuelMachinel);
				sqlCommand.Parameters.AddWithValue("Operation_Value_Adding", item.Operation_Value_Adding);
				sqlCommand.Parameters.AddWithValue("ReationSetup", item.ReationSetup == null ? (object)DBNull.Value : item.ReationSetup);
				sqlCommand.Parameters.AddWithValue("RelationOperationSetup", item.RelationOperationSetup == null ? (object)DBNull.Value : item.RelationOperationSetup);
				sqlCommand.Parameters.AddWithValue("Remark", item.Remark == null ? (object)DBNull.Value : item.Remark);
				sqlCommand.Parameters.AddWithValue("Remark2", item.Remark2 == null ? (object)DBNull.Value : item.Remark2);
				sqlCommand.Parameters.AddWithValue("SecondsPerSubOperation", item.SecondsPerSubOperation == null ? (object)DBNull.Value : item.SecondsPerSubOperation);
				sqlCommand.Parameters.AddWithValue("Setup", item.Setup == null ? (object)DBNull.Value : item.Setup);
				sqlCommand.Parameters.AddWithValue("StdOperationId", item.StdOperationId);
				sqlCommand.Parameters.AddWithValue("TechnologieArea", item.TechnologieArea == null ? (object)DBNull.Value : item.TechnologieArea);
				sqlCommand.Parameters.AddWithValue("ValueAdding", item.ValueAdding == null ? (object)DBNull.Value : item.ValueAdding);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 23; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [StandardOperationDescription] SET "

							+ "[Code]=@Code" + i + ","
							+ "[Creation_Date]=@Creation_Date" + i + ","
							+ "[Creation_User_Id]=@Creation_User_Id" + i + ","
							+ "[Delete_Date]=@Delete_Date" + i + ","
							+ "[Delete_User_Id]=@Delete_User_Id" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Is_Archived]=@Is_Archived" + i + ","
							+ "[Last_Edit_Date]=@Last_Edit_Date" + i + ","
							+ "[Last_Edit_User_Id]=@Last_Edit_User_Id" + i + ","
							+ "[LotPiece]=@LotPiece" + i + ","
							+ "[MachineToolInsert]=@MachineToolInsert" + i + ","
							+ "[ManuelMachinel]=@ManuelMachinel" + i + ","
							+ "[Operation_Value_Adding]=@Operation_Value_Adding" + i + ","
							+ "[ReationSetup]=@ReationSetup" + i + ","
							+ "[RelationOperationSetup]=@RelationOperationSetup" + i + ","
							+ "[Remark]=@Remark" + i + ","
							+ "[Remark2]=@Remark2" + i + ","
							+ "[SecondsPerSubOperation]=@SecondsPerSubOperation" + i + ","
							+ "[Setup]=@Setup" + i + ","
							+ "[StdOperationId]=@StdOperationId" + i + ","
							+ "[TechnologieArea]=@TechnologieArea" + i + ","
							+ "[ValueAdding]=@ValueAdding" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Code" + i, item.Code == null ? (object)DBNull.Value : item.Code);
						sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.Creation_Date);
						sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.Creation_User_Id);
						sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
						sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description);
						sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.Is_Archived);
						sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
						sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
						sqlCommand.Parameters.AddWithValue("LotPiece" + i, item.LotPiece == null ? (object)DBNull.Value : item.LotPiece);
						sqlCommand.Parameters.AddWithValue("MachineToolInsert" + i, item.MachineToolInsert == null ? (object)DBNull.Value : item.MachineToolInsert);
						sqlCommand.Parameters.AddWithValue("ManuelMachinel" + i, item.ManuelMachinel == null ? (object)DBNull.Value : item.ManuelMachinel);
						sqlCommand.Parameters.AddWithValue("Operation_Value_Adding" + i, item.Operation_Value_Adding);
						sqlCommand.Parameters.AddWithValue("ReationSetup" + i, item.ReationSetup == null ? (object)DBNull.Value : item.ReationSetup);
						sqlCommand.Parameters.AddWithValue("RelationOperationSetup" + i, item.RelationOperationSetup == null ? (object)DBNull.Value : item.RelationOperationSetup);
						sqlCommand.Parameters.AddWithValue("Remark" + i, item.Remark == null ? (object)DBNull.Value : item.Remark);
						sqlCommand.Parameters.AddWithValue("Remark2" + i, item.Remark2 == null ? (object)DBNull.Value : item.Remark2);
						sqlCommand.Parameters.AddWithValue("SecondsPerSubOperation" + i, item.SecondsPerSubOperation == null ? (object)DBNull.Value : item.SecondsPerSubOperation);
						sqlCommand.Parameters.AddWithValue("Setup" + i, item.Setup == null ? (object)DBNull.Value : item.Setup);
						sqlCommand.Parameters.AddWithValue("StdOperationId" + i, item.StdOperationId);
						sqlCommand.Parameters.AddWithValue("TechnologieArea" + i, item.TechnologieArea == null ? (object)DBNull.Value : item.TechnologieArea);
						sqlCommand.Parameters.AddWithValue("ValueAdding" + i, item.ValueAdding == null ? (object)DBNull.Value : item.ValueAdding);
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
			var sqlConection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConection.Open();

			string query = "DELETE FROM StandardOperationDescription WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConection);
			sqlCommand.Parameters.AddWithValue("Id", id);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConection.Close();

			return response;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

				if(ids.Count <= maxParamsNumber)
				{
					return delete(ids);
				}
				int result = 0;
				int batchNumber = ids.Count / maxParamsNumber;
				for(int i = 0; i < batchNumber; i++)
				{
					result += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
				}
				result += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				return result;
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
				sqlConnection.Open();

				var sqlCommand = new SqlCommand
				{
					Connection = sqlConnection
				};

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM StandardOperationDescription WHERE Id IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				int response = DbExecution.ExecuteNonQuery(sqlCommand);

				sqlConnection.Close();

				return response;
			}
			return -1;
		}
		

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [StandardOperationDescription] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [StandardOperationDescription]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [StandardOperationDescription] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [StandardOperationDescription] ([Code],[Creation_Date],[Creation_User_Id],[Delete_Date],[Delete_User_Id],[Description],[Is_Archived],[Last_Edit_Date],[Last_Edit_User_Id],[LotPiece],[MachineToolInsert],[ManuelMachinel],[Operation_Value_Adding],[ReationSetup],[RelationOperationSetup],[Remark],[Remark2],[SecondsPerSubOperation],[Setup],[StdOperationId],[TechnologieArea],[ValueAdding]) OUTPUT INSERTED.[Id] VALUES (@Code,@Creation_Date,@Creation_User_Id,@Delete_Date,@Delete_User_Id,@Description,@Is_Archived,@Last_Edit_Date,@Last_Edit_User_Id,@LotPiece,@MachineToolInsert,@ManuelMachinel,@Operation_Value_Adding,@ReationSetup,@RelationOperationSetup,@Remark,@Remark2,@SecondsPerSubOperation,@Setup,@StdOperationId,@TechnologieArea,@ValueAdding); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Code", item.Code == null ? (object)DBNull.Value : item.Code);
			sqlCommand.Parameters.AddWithValue("Creation_Date", item.Creation_Date);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.Creation_User_Id);
			sqlCommand.Parameters.AddWithValue("Delete_Date", item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
			sqlCommand.Parameters.AddWithValue("Description", item.Description);
			sqlCommand.Parameters.AddWithValue("Is_Archived", item.Is_Archived);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
			sqlCommand.Parameters.AddWithValue("LotPiece", item.LotPiece == null ? (object)DBNull.Value : item.LotPiece);
			sqlCommand.Parameters.AddWithValue("MachineToolInsert", item.MachineToolInsert == null ? (object)DBNull.Value : item.MachineToolInsert);
			sqlCommand.Parameters.AddWithValue("ManuelMachinel", item.ManuelMachinel == null ? (object)DBNull.Value : item.ManuelMachinel);
			sqlCommand.Parameters.AddWithValue("Operation_Value_Adding", item.Operation_Value_Adding);
			sqlCommand.Parameters.AddWithValue("ReationSetup", item.ReationSetup == null ? (object)DBNull.Value : item.ReationSetup);
			sqlCommand.Parameters.AddWithValue("RelationOperationSetup", item.RelationOperationSetup == null ? (object)DBNull.Value : item.RelationOperationSetup);
			sqlCommand.Parameters.AddWithValue("Remark", item.Remark == null ? (object)DBNull.Value : item.Remark);
			sqlCommand.Parameters.AddWithValue("Remark2", item.Remark2 == null ? (object)DBNull.Value : item.Remark2);
			sqlCommand.Parameters.AddWithValue("SecondsPerSubOperation", item.SecondsPerSubOperation == null ? (object)DBNull.Value : item.SecondsPerSubOperation);
			sqlCommand.Parameters.AddWithValue("Setup", item.Setup == null ? (object)DBNull.Value : item.Setup);
			sqlCommand.Parameters.AddWithValue("StdOperationId", item.StdOperationId);
			sqlCommand.Parameters.AddWithValue("TechnologieArea", item.TechnologieArea == null ? (object)DBNull.Value : item.TechnologieArea);
			sqlCommand.Parameters.AddWithValue("ValueAdding", item.ValueAdding == null ? (object)DBNull.Value : item.ValueAdding);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 23; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [StandardOperationDescription] ([Code],[Creation_Date],[Creation_User_Id],[Delete_Date],[Delete_User_Id],[Description],[Is_Archived],[Last_Edit_Date],[Last_Edit_User_Id],[LotPiece],[MachineToolInsert],[ManuelMachinel],[Operation_Value_Adding],[ReationSetup],[RelationOperationSetup],[Remark],[Remark2],[SecondsPerSubOperation],[Setup],[StdOperationId],[TechnologieArea],[ValueAdding]) VALUES ( "

						+ "@Code" + i + ","
						+ "@Creation_Date" + i + ","
						+ "@Creation_User_Id" + i + ","
						+ "@Delete_Date" + i + ","
						+ "@Delete_User_Id" + i + ","
						+ "@Description" + i + ","
						+ "@Is_Archived" + i + ","
						+ "@Last_Edit_Date" + i + ","
						+ "@Last_Edit_User_Id" + i + ","
						+ "@LotPiece" + i + ","
						+ "@MachineToolInsert" + i + ","
						+ "@ManuelMachinel" + i + ","
						+ "@Operation_Value_Adding" + i + ","
						+ "@ReationSetup" + i + ","
						+ "@RelationOperationSetup" + i + ","
						+ "@Remark" + i + ","
						+ "@Remark2" + i + ","
						+ "@SecondsPerSubOperation" + i + ","
						+ "@Setup" + i + ","
						+ "@StdOperationId" + i + ","
						+ "@TechnologieArea" + i + ","
						+ "@ValueAdding" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Code" + i, item.Code == null ? (object)DBNull.Value : item.Code);
					sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.Creation_Date);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.Creation_User_Id);
					sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
					sqlCommand.Parameters.AddWithValue("Description" + i, item.Description);
					sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.Is_Archived);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
					sqlCommand.Parameters.AddWithValue("LotPiece" + i, item.LotPiece == null ? (object)DBNull.Value : item.LotPiece);
					sqlCommand.Parameters.AddWithValue("MachineToolInsert" + i, item.MachineToolInsert == null ? (object)DBNull.Value : item.MachineToolInsert);
					sqlCommand.Parameters.AddWithValue("ManuelMachinel" + i, item.ManuelMachinel == null ? (object)DBNull.Value : item.ManuelMachinel);
					sqlCommand.Parameters.AddWithValue("Operation_Value_Adding" + i, item.Operation_Value_Adding);
					sqlCommand.Parameters.AddWithValue("ReationSetup" + i, item.ReationSetup == null ? (object)DBNull.Value : item.ReationSetup);
					sqlCommand.Parameters.AddWithValue("RelationOperationSetup" + i, item.RelationOperationSetup == null ? (object)DBNull.Value : item.RelationOperationSetup);
					sqlCommand.Parameters.AddWithValue("Remark" + i, item.Remark == null ? (object)DBNull.Value : item.Remark);
					sqlCommand.Parameters.AddWithValue("Remark2" + i, item.Remark2 == null ? (object)DBNull.Value : item.Remark2);
					sqlCommand.Parameters.AddWithValue("SecondsPerSubOperation" + i, item.SecondsPerSubOperation == null ? (object)DBNull.Value : item.SecondsPerSubOperation);
					sqlCommand.Parameters.AddWithValue("Setup" + i, item.Setup == null ? (object)DBNull.Value : item.Setup);
					sqlCommand.Parameters.AddWithValue("StdOperationId" + i, item.StdOperationId);
					sqlCommand.Parameters.AddWithValue("TechnologieArea" + i, item.TechnologieArea == null ? (object)DBNull.Value : item.TechnologieArea);
					sqlCommand.Parameters.AddWithValue("ValueAdding" + i, item.ValueAdding == null ? (object)DBNull.Value : item.ValueAdding);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [StandardOperationDescription] SET [Code]=@Code, [Creation_Date]=@Creation_Date, [Creation_User_Id]=@Creation_User_Id, [Delete_Date]=@Delete_Date, [Delete_User_Id]=@Delete_User_Id, [Description]=@Description, [Is_Archived]=@Is_Archived, [Last_Edit_Date]=@Last_Edit_Date, [Last_Edit_User_Id]=@Last_Edit_User_Id, [LotPiece]=@LotPiece, [MachineToolInsert]=@MachineToolInsert, [ManuelMachinel]=@ManuelMachinel, [Operation_Value_Adding]=@Operation_Value_Adding, [ReationSetup]=@ReationSetup, [RelationOperationSetup]=@RelationOperationSetup, [Remark]=@Remark, [Remark2]=@Remark2, [SecondsPerSubOperation]=@SecondsPerSubOperation, [Setup]=@Setup, [StdOperationId]=@StdOperationId, [TechnologieArea]=@TechnologieArea, [ValueAdding]=@ValueAdding WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Code", item.Code == null ? (object)DBNull.Value : item.Code);
			sqlCommand.Parameters.AddWithValue("Creation_Date", item.Creation_Date);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.Creation_User_Id);
			sqlCommand.Parameters.AddWithValue("Delete_Date", item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
			sqlCommand.Parameters.AddWithValue("Description", item.Description);
			sqlCommand.Parameters.AddWithValue("Is_Archived", item.Is_Archived);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
			sqlCommand.Parameters.AddWithValue("LotPiece", item.LotPiece == null ? (object)DBNull.Value : item.LotPiece);
			sqlCommand.Parameters.AddWithValue("MachineToolInsert", item.MachineToolInsert == null ? (object)DBNull.Value : item.MachineToolInsert);
			sqlCommand.Parameters.AddWithValue("ManuelMachinel", item.ManuelMachinel == null ? (object)DBNull.Value : item.ManuelMachinel);
			sqlCommand.Parameters.AddWithValue("Operation_Value_Adding", item.Operation_Value_Adding);
			sqlCommand.Parameters.AddWithValue("ReationSetup", item.ReationSetup == null ? (object)DBNull.Value : item.ReationSetup);
			sqlCommand.Parameters.AddWithValue("RelationOperationSetup", item.RelationOperationSetup == null ? (object)DBNull.Value : item.RelationOperationSetup);
			sqlCommand.Parameters.AddWithValue("Remark", item.Remark == null ? (object)DBNull.Value : item.Remark);
			sqlCommand.Parameters.AddWithValue("Remark2", item.Remark2 == null ? (object)DBNull.Value : item.Remark2);
			sqlCommand.Parameters.AddWithValue("SecondsPerSubOperation", item.SecondsPerSubOperation == null ? (object)DBNull.Value : item.SecondsPerSubOperation);
			sqlCommand.Parameters.AddWithValue("Setup", item.Setup == null ? (object)DBNull.Value : item.Setup);
			sqlCommand.Parameters.AddWithValue("StdOperationId", item.StdOperationId);
			sqlCommand.Parameters.AddWithValue("TechnologieArea", item.TechnologieArea == null ? (object)DBNull.Value : item.TechnologieArea);
			sqlCommand.Parameters.AddWithValue("ValueAdding", item.ValueAdding == null ? (object)DBNull.Value : item.ValueAdding);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 23; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [StandardOperationDescription] SET "

					+ "[Code]=@Code" + i + ","
					+ "[Creation_Date]=@Creation_Date" + i + ","
					+ "[Creation_User_Id]=@Creation_User_Id" + i + ","
					+ "[Delete_Date]=@Delete_Date" + i + ","
					+ "[Delete_User_Id]=@Delete_User_Id" + i + ","
					+ "[Description]=@Description" + i + ","
					+ "[Is_Archived]=@Is_Archived" + i + ","
					+ "[Last_Edit_Date]=@Last_Edit_Date" + i + ","
					+ "[Last_Edit_User_Id]=@Last_Edit_User_Id" + i + ","
					+ "[LotPiece]=@LotPiece" + i + ","
					+ "[MachineToolInsert]=@MachineToolInsert" + i + ","
					+ "[ManuelMachinel]=@ManuelMachinel" + i + ","
					+ "[Operation_Value_Adding]=@Operation_Value_Adding" + i + ","
					+ "[ReationSetup]=@ReationSetup" + i + ","
					+ "[RelationOperationSetup]=@RelationOperationSetup" + i + ","
					+ "[Remark]=@Remark" + i + ","
					+ "[Remark2]=@Remark2" + i + ","
					+ "[SecondsPerSubOperation]=@SecondsPerSubOperation" + i + ","
					+ "[Setup]=@Setup" + i + ","
					+ "[StdOperationId]=@StdOperationId" + i + ","
					+ "[TechnologieArea]=@TechnologieArea" + i + ","
					+ "[ValueAdding]=@ValueAdding" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("Code" + i, item.Code == null ? (object)DBNull.Value : item.Code);
					sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.Creation_Date);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.Creation_User_Id);
					sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
					sqlCommand.Parameters.AddWithValue("Description" + i, item.Description);
					sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.Is_Archived);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
					sqlCommand.Parameters.AddWithValue("LotPiece" + i, item.LotPiece == null ? (object)DBNull.Value : item.LotPiece);
					sqlCommand.Parameters.AddWithValue("MachineToolInsert" + i, item.MachineToolInsert == null ? (object)DBNull.Value : item.MachineToolInsert);
					sqlCommand.Parameters.AddWithValue("ManuelMachinel" + i, item.ManuelMachinel == null ? (object)DBNull.Value : item.ManuelMachinel);
					sqlCommand.Parameters.AddWithValue("Operation_Value_Adding" + i, item.Operation_Value_Adding);
					sqlCommand.Parameters.AddWithValue("ReationSetup" + i, item.ReationSetup == null ? (object)DBNull.Value : item.ReationSetup);
					sqlCommand.Parameters.AddWithValue("RelationOperationSetup" + i, item.RelationOperationSetup == null ? (object)DBNull.Value : item.RelationOperationSetup);
					sqlCommand.Parameters.AddWithValue("Remark" + i, item.Remark == null ? (object)DBNull.Value : item.Remark);
					sqlCommand.Parameters.AddWithValue("Remark2" + i, item.Remark2 == null ? (object)DBNull.Value : item.Remark2);
					sqlCommand.Parameters.AddWithValue("SecondsPerSubOperation" + i, item.SecondsPerSubOperation == null ? (object)DBNull.Value : item.SecondsPerSubOperation);
					sqlCommand.Parameters.AddWithValue("Setup" + i, item.Setup == null ? (object)DBNull.Value : item.Setup);
					sqlCommand.Parameters.AddWithValue("StdOperationId" + i, item.StdOperationId);
					sqlCommand.Parameters.AddWithValue("TechnologieArea" + i, item.TechnologieArea == null ? (object)DBNull.Value : item.TechnologieArea);
					sqlCommand.Parameters.AddWithValue("ValueAdding" + i, item.ValueAdding == null ? (object)DBNull.Value : item.ValueAdding);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [StandardOperationDescription] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
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

				string query = "DELETE FROM [StandardOperationDescription] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction

		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> GetByOperationId(int StandardOperationId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM StandardOperationDescription WHERE StdOperationId=@StdOperationId AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);
			sqlCommand.Parameters.AddWithValue("StdOperationId", StandardOperationId);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> GetByDescription(int StandardOperationId, string description)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM StandardOperationDescription WHERE StdOperationId=@StdOperationId AND Is_Archived=@IsArchived AND [Description]=@description";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);
			sqlCommand.Parameters.AddWithValue("StdOperationId", StandardOperationId);
			sqlCommand.Parameters.AddWithValue("description", description);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> GetByDescription(int StandardOperationId, string description, int suboperationId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM StandardOperationDescription WHERE StdOperationId=@StdOperationId AND Is_Archived=@IsArchived AND [Description]=@description AND [Id]<>@suboperationId";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);
			sqlCommand.Parameters.AddWithValue("StdOperationId", StandardOperationId);
			sqlCommand.Parameters.AddWithValue("description", description);
			sqlCommand.Parameters.AddWithValue("suboperationId", suboperationId);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> GetByCode(int StandardOperationId, string code)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM StandardOperationDescription WHERE StdOperationId=@StdOperationId AND Is_Archived=@IsArchived AND [Code]=@code";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);
			sqlCommand.Parameters.AddWithValue("StdOperationId", StandardOperationId);
			sqlCommand.Parameters.AddWithValue("code", code);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> GetByCode(int StandardOperationId, string code, int suboperationId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM StandardOperationDescription WHERE StdOperationId=@StdOperationId AND Is_Archived=@IsArchived AND [Code]=@code AND [Id]<>@suboperationId";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);
			sqlCommand.Parameters.AddWithValue("StdOperationId", StandardOperationId);
			sqlCommand.Parameters.AddWithValue("code", code);
			sqlCommand.Parameters.AddWithValue("suboperationId", suboperationId);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
