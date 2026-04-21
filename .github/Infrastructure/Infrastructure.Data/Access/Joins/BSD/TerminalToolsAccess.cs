using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Infrastructure.Data.Entities.Joins;

namespace Infrastructure.Data.Access.Joins.BSD
{
	public class TerminalToolsAccess
	{
		public static List<TerminalToolsEntity> GetTerminlaTools(string terminal)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT distinct w.[Inventarnummer JPM],w.[Lagerplatz] FROM [kontakte] k iNNER JOIN [Werkzeug] w
                                 ON k.[Inventarnummer JPM] =w.[Inventarnummer JPM]
                                 WHERE k.[Artikelnummer JPM]=@terminal
                                 ORDER BY w.[Lagerplatz]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("terminal", terminal);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new TerminalToolsEntity(x))?.ToList();
			}
			else
			{
				return null;
			}
		}

	}
}
