using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.Statistics.MGO
{
	public class PeriodicSalesAccess
	{
		#region Default Methods
		public static List<Infrastructure.Data.Entities.Tables.Statistics.MGO.PeriodicSalesEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [stats].[PeriodicSales] WHERE [Year]>=YEAR(GETDATE())-3 order by [Year] desc,[KW] desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				//sqlCommand.Parameters.AddWithValue("year", year);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Statistics.MGO.PeriodicSalesEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Statistics.MGO.PeriodicSalesEntity>();
			}
		}

		public static List<Entities.Tables.Statistics.MGO.InvoiceAmountEntity> GetInvoiceAmount()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT Belegdatum,
								sum(betrag) as Amount
							  FROM [dbo].[PSZ_BH_Transfertabelle_Archiv]
							  where CAST(Belegdatum as date)  > GETDATE()-14
							  group by Belegdatum
							  order by Belegdatum";

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
	}
}
