using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class Stucklisten_costenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.Stucklisten_costenEntity Get(int articleId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $"EXEC Stucklisten_costen @articleNr = {articleId}";

			var sqlCommand = new SqlCommand(query, sqlConnection);


			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_costenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.Stucklisten_costenEntity Get(int articleId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = $"EXEC Stucklisten_costen @articleNr = {articleId}";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				var selectAdapter = new SqlDataAdapter(sqlCommand);
				selectAdapter.Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_costenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion
		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_costenEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_costenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_costenEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
