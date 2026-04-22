using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.Statistics
{
	public class PeriodicSalesAccess
	{
		#region Default Methods
		public static List<Entities.Tables.Statistics.PeriodicSalesEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [stats].[PeriodicSales]  order by [Year] desc,[KW] desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				//sqlCommand.Parameters.AddWithValue("year", year);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.Statistics.MGO.InvoiceAmountEntity> GetInvoiceAmount()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT top(15)  Belegdatum,YEAR(Belegdatum) as Year, DATEPART(ISO_WEEK,Belegdatum) as KW,
								sum(betrag) as Amount
							  FROM [dbo].[PSZ_BH_Transfertabelle_Archiv]
							  --where CAST(Belegdatum as date)  > GETDATE()-14
							  group by Belegdatum
							  order by Belegdatum desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.Statistics.MGO.InvoiceAmountEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.Statistics.MGO.InvoiceAmountEntity>();
			}
		}
		#endregion

		#region Custom Methods
		#endregion

		#region Helpers
		private static List<Data.Entities.Tables.Statistics.PeriodicSalesEntity> toList(DataTable dataTable)
		{
			var result = new List<Entities.Tables.Statistics.PeriodicSalesEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Entities.Tables.Statistics.PeriodicSalesEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
