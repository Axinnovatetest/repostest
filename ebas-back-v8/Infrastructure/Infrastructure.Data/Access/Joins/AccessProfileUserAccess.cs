using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins
{
    public class AccessProfileUserAccess
    {
		public static int ReplaceForUser(int userId, int destinationUserId, SqlConnection connection, SqlTransaction transaction)
		{
			if(userId <= 0 || destinationUserId <= 0)
				return -1;

			int results = -1;
			using(var sqlCommand = new SqlCommand("", connection, transaction))
			{
				string query = $@"/* CTS */
							DELETE FROM [__CTS_AccessProfileUsers] WHERE [UserId]={destinationUserId};
								INSERT INTO [__CTS_AccessProfileUsers]([AccessProfileId],[AccessProfileName],[CreationTime],[CreationUserId],[UserEmail],[UserId],[UserName])
								SELECT a.[AccessProfileId],[AccessProfileName],GETDATE()[CreationTime],[CreationUserId],dest.[Email],dest.Id [UserId],dest.[UserName] 
								FROM [__CTS_AccessProfileUsers] a, [user] dest WHERE [UserId]={userId} AND dest.Id={destinationUserId};

							/* CRP */
							DELETE FROM [__CRP_AccessProfileUsers] WHERE [UserId]={destinationUserId};
								INSERT INTO [__CRP_AccessProfileUsers]([AccessProfileId],[AccessProfileName],[CreationTime],[CreationUserId],[UserEmail],[UserId],[UserName])
								SELECT a.[AccessProfileId],[AccessProfileName],GETDATE()[CreationTime],[CreationUserId],dest.[Email],dest.Id [UserId],dest.[UserName] 
								FROM [__CRP_AccessProfileUsers] a, [user] dest WHERE [UserId]={userId} AND dest.Id={destinationUserId};

							/* LGT */
							DELETE FROM [__LGT_AccessProfileUsers] WHERE [UserId]={destinationUserId};
								INSERT INTO [__LGT_AccessProfileUsers]([AccessProfileId],[AccessProfileName],[CreationTime],[CreationUserId],[UserEmail],[UserId],[UserName])
								SELECT a.[AccessProfileId],[AccessProfileName],GETDATE()[CreationTime],[CreationUserId],dest.[Email],dest.Id [UserId],dest.[UserName] 
								FROM [__LGT_AccessProfileUsers] a, [user] dest WHERE [UserId]={userId} AND dest.Id={destinationUserId};

							/* MGO */
							DELETE FROM [__MGO_AccessProfileUsers] WHERE [UserId]={destinationUserId};
								INSERT INTO [__MGO_AccessProfileUsers]([AccessProfileId],[AccessProfileName],[CreationTime],[CreationUserId],[UserEmail],[UserId],[UserName])
								SELECT a.[AccessProfileId],[AccessProfileName],GETDATE()[CreationTime],[CreationUserId],dest.[Email],dest.Id [UserId],dest.[UserName] 
								FROM [__MGO_AccessProfileUsers] a, [user] dest WHERE [UserId]={userId} AND dest.Id={destinationUserId};

							/* BSD */
							DELETE FROM [__BSD_AccessProfileUsers] WHERE [UserId]={destinationUserId};
								INSERT INTO [__BSD_AccessProfileUsers]([AccessProfileId],[AccessProfileName],[CreationTime],[CreationUserId],[UserEmail],[UserId],[UserName])
								SELECT a.[AccessProfileId],[AccessProfileName],GETDATE()[CreationTime],[CreationUserId],dest.[Email],dest.Id [UserId],dest.[UserName] 
								FROM [__BSD_AccessProfileUsers] a, [user] dest WHERE [UserId]={userId} AND dest.Id={destinationUserId};

							/* CPL */
							DELETE FROM [__CPL_AccessProfileUsers] WHERE [UserId]={destinationUserId};
								INSERT INTO [__CPL_AccessProfileUsers]([AccessProfileId],[AccessProfileName],[CreationTime],[CreationUserId],[UserEmail],[UserId],[UserName])
								SELECT a.[AccessProfileId],[AccessProfileName],GETDATE()[CreationTime],[CreationUserId],dest.[Email],dest.Id [UserId],dest.[UserName] 
								FROM [__CPL_AccessProfileUsers] a, [user] dest WHERE [UserId]={userId} AND dest.Id={destinationUserId};";

				sqlCommand.CommandText= query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int ReplaceForUserFNC(int userId, int destUserId, string destUserName, string destUserEmail, SqlConnection connection, SqlTransaction transaction)
		{
			if(userId <= 0 || destUserId <= 0)
				return -1;

			int results = -1;
			using(var sqlCommand = new SqlCommand("", connection, transaction))
			{
				string query = $@"/* FNC */
							DELETE FROM [__FNC_UserAccessProfiles] WHERE [UserId]={destUserId};
								INSERT INTO [__FNC_UserAccessProfiles]([AccessProfileId],[AccessProfileName],[CreationTime],[CreationUserId],[UserEmail],[UserId],[UserName])
								SELECT a.[AccessProfileId],[AccessProfileName],GETDATE()[CreationTime],[CreationUserId],'{destUserEmail}' [Email],{destUserId} [UserId],'{destUserName}' [UserName] 
								FROM [__FNC_UserAccessProfiles] a WHERE [UserId]={userId};";

				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int ReplaceForUserMTM(int userId, int destUserId, string destUserName, string destUserEmail, SqlConnection connection, SqlTransaction transaction)
		{
			if(userId <= 0 || destUserId <= 0)
				return -1;

			int results = -1;
			using(var sqlCommand = new SqlCommand("", connection, transaction))
			{
				string query = $@"/* MTM */
							DELETE FROM [__MTM_AccessProfileUsers] WHERE [UserId]={destUserId};
								INSERT INTO [__MTM_AccessProfileUsers]([AccessProfileId],[AccessProfileName],[CreationTime],[CreationUserId],[UserEmail],[UserId],[UserName])
								SELECT a.[AccessProfileId],[AccessProfileName],GETDATE()[CreationTime],[CreationUserId],'{destUserEmail}' [Email],{destUserId} [UserId],'{destUserName}' [UserName] 
								FROM [__MTM_AccessProfileUsers] a WHERE [UserId]={userId};";

				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
	}
}
