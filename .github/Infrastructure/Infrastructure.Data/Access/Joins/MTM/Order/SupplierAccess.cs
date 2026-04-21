using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class SupplierAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.SupplierEntity> GetSuppliersInfoByArticle(int ArticleNr)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query =
					@$"select  b.[Lieferanten-Nr], b.Mindestbestellmenge,B.Einkaufspreis,B.Wiederbeschaffungszeitraum,A.Name1 [Name],B.Standardlieferant  from Bestellnummern B
						Inner join Adressen A on A.Nr = B.[Lieferanten-Nr] 

						where [Artikel-Nr] = {ArticleNr}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.SupplierEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.SupplierEntity>();
			}
		}
	}
}
