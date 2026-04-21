using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins.MTM.WPL
{
	public class WorkSchedule
	{
		public static List<Entities.Joins.MTM.Order.ArticlesInFaFiltered> GetArticles(IEnumerable<int> idStandardOperation)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
					$@" Select * from Artikel a  ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 120;
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.ArticlesInFaFiltered(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.ArticlesInFaFiltered>();
			}
		}

	}
}
