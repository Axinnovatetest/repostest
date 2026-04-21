using Infrastructure.Data.Entities.Functions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Functions
{
	public static class ArticleStatisticsAccess
	{
		public static List<ArticleROHNeedStockEntity> GetArticleROHNeedStock(string articleNumber, int year, int maxRecord = 100)
		{
			articleNumber = articleNumber ?? "";
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand($"SELECT * FROM [stats].[ufnGetArticleMaterialRequirement]('{articleNumber.SqlEscape()}', {year}, {maxRecord});", sqlConnection))
			{
				sqlCommand.CommandTimeout = 300;
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ArticleROHNeedStockEntity(x))?.ToList();
			}
			else
			{
				return new List<ArticleROHNeedStockEntity>();
			}
		}
		public static List<ArticleROHNeedStockEntity> GetArticleROHNeedStockBySupplierClass(string supplierClass, int? supplierNr)
		{
			supplierClass = supplierClass ?? "";
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand($"SELECT * FROM [stats].[MTD_MaterialRequirements] WHERE [LieferantStufe]={supplierClass.SqlEscape()}{(supplierNr.HasValue ? $" AND [LieferantNummer]={supplierNr}" : "")});", sqlConnection))
			{
				sqlCommand.CommandTimeout = 300;
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ArticleROHNeedStockEntity(x))?.ToList();
			}
			else
			{
				return new List<ArticleROHNeedStockEntity>();
			}
		}
	}
}
