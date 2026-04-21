namespace Infrastructure.Data.Access.Joins.CRP
{
	public class BestellenFGHistoryAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.CRP.BestellenFgHistoryEntity> GetBestellenFG(DateTime? from, DateTime? to,int? AdressCustomerNumber)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
								SELECT 
									Cd.ArticleNumber,
									Cd.CustomerNumber,
									Cd.CustomerName,
									Cd.CsContact,
                          		    Cd.WarehouseName,
									Cd.ArticleReleaseStatus,
									Cd.ArticleDesignation1,
									Cd.ArticleDesignation2,
									Cd.StockQuantity,
								    Cd.UnitSalesPrice,
									Cd.TotalCostsWithCu,
									Cd.TotalCostsWithoutCu,
									Cd.UBG,
									Cd.VKE,
									Cd.TotalSalesPrice,
									Cd.EdiStandard,
									Ch.ImportDate
									FROM [stats].[__CRP_HistoryFG_Details] Cd
									INNER JOIN [stats].[__CRP_HistoryFG_Header] Ch ON Ch.Id = Cd.HeaderId
                                 ";
								var isFirstClause = true;
								if(from is not null)
								{
									query += $"{(isFirstClause ? " WHERE" : " AND")}  CONVERT(date,Ch.ImportDate) >= '{from}'";
									isFirstClause = false;
								}
								if(to is not null)
								{
									query += $"{(isFirstClause ? " WHERE" : " AND")} CONVERT(date,Ch.ImportDate) <= '{to}'";
									isFirstClause = false;
								}
								if(AdressCustomerNumber is not null)
								{
									query += $"{(isFirstClause ? " WHERE" : " AND")} Cd.CustomerNumber={AdressCustomerNumber}";
									isFirstClause = false;
								}


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CRP.BestellenFgHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CRP.BestellenFgHistoryEntity>();
			}
		}
	}
}
