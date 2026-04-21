using Infrastructure.Data.Entities.Tables.PRS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins.CTS
{
	public class BackgroundWorkerAccess
	{
		public static async Task<List<AngeboteneArtikelEntity>> GetPosWrongVatAsync(List<decimal> goodVat, DateTime minDate)
		{
			if(goodVat == null || goodVat.Count <= 0)
				return null;

			var response = new List<AngeboteneArtikelEntity>();
			var dataTable = new DataTable();

			return await Task<List<AngeboteneArtikelEntity>>.Factory.StartNew(() =>
			{
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [Angebotene Artikel] 
                                        WHERE cast([USt] as decimal(20,6)) NOT IN ({string.Join(",", goodVat)})
                                            AND[Angebot-Nr] IN (Select Distinct Nr From Angebote WHERE Datum > '{minDate.ToString("yyyyMMdd")}')";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 660;

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			});
		}
	}
}
