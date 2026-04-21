using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins
{
	public class CapitalRequestsJointsAccess
	{
		public static int GetWerkeId(int companyId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"select Id from __LGT_Werke where IdCompany=@companyId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("companyId", companyId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}
	}
}
