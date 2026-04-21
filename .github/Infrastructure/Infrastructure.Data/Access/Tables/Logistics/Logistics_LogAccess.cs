using System;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.Logistics
{
	public class Logistics_LogAccess
	{
		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.Logistics_LogEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__LGT_Logistics_Log] ([AngebotNr],[DateTime],[LogObject],[LogText],[LogType],[Origin],[ProjektNr],[UserId],[Username]) OUTPUT INSERTED.[Id] VALUES (@AngebotNr,@DateTime,@LogObject,@LogText,@LogType,@Origin,@ProjektNr,@UserId,@Username); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AngebotNr", item.AngebotNr == null ? (object)DBNull.Value : item.AngebotNr);
					sqlCommand.Parameters.AddWithValue("DateTime", item.DateTime == null ? (object)DBNull.Value : item.DateTime);
					sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogText", item.LogText == null ? (object)DBNull.Value : item.LogText);
					sqlCommand.Parameters.AddWithValue("LogType", item.LogType == null ? (object)DBNull.Value : item.LogType);
					sqlCommand.Parameters.AddWithValue("Origin", item.Origin == null ? (object)DBNull.Value : item.Origin);
					sqlCommand.Parameters.AddWithValue("ProjektNr", item.ProjektNr == null ? (object)DBNull.Value : item.ProjektNr);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
	}
}
