using System.Globalization;

namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class InfoRahmennummerAccess
	{
		#region MTM - CTS
		public static List<Entities.Joins.MTM.Order.InfoRahmennummerEntity> GetRahmennummer(int articleId, int supplierId, decimal quantity, DateTime extensionDate)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				var quantityD = quantity.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." });
				sqlConnection.Open();
				string query = $@"select a.[Angebot-Nr],apo.Nr as PositionNr,apo.Position,apo.Anzahl,po.ExtensionDate ,
                                po.GultigAb,po.GultigBis, a.Nr, a.[Bezug]
                                from __CTS_AngeboteBlanketExtension ra 
	                                Join __CTS_AngeboteArticleBlanketExtension po on po.RahmenNr=ra.AngeboteNr
									left Join Lieferanten l on l.Nr=ra.SupplierId	                                
									left Join Angebote a on a.Nr=ra.AngeboteNr
	                                left Join [angebotene Artikel] apo on apo.Nr=po.AngeboteArtikelNr
                                WHERE 
		                                po.MaterialNr = {articleId}
		                                AND ra.BlanketTypeId=1
		                                AND ra.StatusId=2 
		                                AND apo.Anzahl >= {quantityD} AND ISNULL(apo.erledigt_pos,0)=0 
                                        AND ra.SupplierId={supplierId}
		                                --AND '{extensionDate.ToString("dd/MM/yyyy")}' between po.GultigAb and po.ExtensionDate";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.InfoRahmennummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.InfoRahmennummerEntity>();
			}
		}
		#endregion
	}
}
