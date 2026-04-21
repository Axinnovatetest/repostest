using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class OrderedArticlesAccess
	{

		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.OrderedQuantityEntity> GetOrderedQuantitiy(List<int?> LagersList, int Months, int Artikel_Nr)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.OrderedQuantityEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				string Lagerort_sub_filter = "";
				string artikelNr_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND Lagerort_id  IN({pattern})";
				}
				if(Artikel_Nr > 0)
				{
					artikelNr_sub_filter = $"AND [Artikel-Nr] = {Artikel_Nr}";
				}
				sqlConnection.Open();
				string query =
					@$"
						SELECT 
						case 
							WHEN 
								DATEPART(MONTH , Bestätigter_Termin) = 1 
									AND DATEPART(DAY , Bestätigter_Termin) < 8  
									AND DATEPART(iso_week , Bestätigter_Termin) <> 1 
								THEN CONCAT(DATEPART(iso_week, Bestätigter_Termin) , '/' , DATEPART(YEAR , Bestätigter_Termin) - 1)  
								ELSE 
									CONCAT(DATEPART(iso_week, Bestätigter_Termin) , '/' , DATEPART(YEAR , Bestätigter_Termin))  
								END AS 
								[WeekPO],
								[Artikel-Nr],
								SUM(Anzahl) OrderedQuantity 
						FROM
							( 
								select [Artikel-Nr] , 
									Bestätigter_Termin, 
									Anzahl
								from [bestellte Artikel]  ba
								inner join Bestellungen b ON ba.[Bestellung-Nr] = b.Nr
								WHERE 
									Bestätigter_Termin  BETWEEN  GETDATE() AND DATEADD(MONTH , 6 , GETDATE()) 
									AND [Position erledigt] <> 1 
									AND Anzahl <> 0 
									{Lagerort_sub_filter}
									{artikelNr_sub_filter}
									AND b.Typ = 'Bestellung' 
									) 
								s 
		
						Group by Bestätigter_Termin,[Artikel-Nr]
					";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.OrderedQuantityEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.OrderedQuantityEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersCountEntity GetFaultyOrdersCount(List<int?> LagersList, int artikelNr)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersCountEntity();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				string Lagerort_sub_filter = "";
				string artikelNr_Sub_Filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND BA.Lagerort_id IN({pattern})";
				}
				if(artikelNr > 0)
				{
					artikelNr_Sub_Filter = $"AND BA.[Artikel-Nr] = {artikelNr}";
				}

				sqlConnection.Open();
				string query =
					$@"select COUNT(DISTINCT  B.[Bestellung-Nr]) faulty_Orders from Bestellungen B
					INNER JOIN [bestellte Artikel] BA
					on B.Nr = BA.[Bestellung-Nr]
					AND(BA.Bestätigter_Termin <= GETDATE() OR ISDATE(BA.Bestätigter_Termin) = 0)
					AND ISNULL(BA.[Start Anzahl],0) > 0
					AND ISNULL(BA.Anzahl,0) > 0
					AND ISNULL(BA.erledigt_pos,0) <> 1
					{Lagerort_sub_filter}  
					{artikelNr_Sub_Filter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersCountEntity(x)).FirstOrDefault();
			}
			else
			{
				return new Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersCountEntity();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersEntity> GetFaultyOrders(List<int?> LagersList, int ArtikelNr, int RequestedPage, int PageSize)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				string Lagerort_sub_filter = "";
				string artikelNr_Sub_Filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND BA.Lagerort_id IN({pattern})";
				}
				//AND BA.[Artikel-Nr] =1475 
				if(ArtikelNr > 0)
				{
					artikelNr_Sub_Filter = $"AND BA.[Artikel-Nr] = {ArtikelNr}";
				}

				sqlConnection.Open();
				string query =
					$@"SELECT COUNT(*) OVER() TotalCount
					,B.[Bestellung-Nr],BA.Bestätigter_Termin,B.[Vorname/NameFirma] Supplier
					FROM [bestellte Artikel] BA 
					JOIN Bestellungen B 
					ON BA.[Bestellung-Nr] = B.Nr 
					AND ISNULL(BA.Anzahl,0) > 0 
					AND (BA.Bestätigter_Termin <=  GETDATE() OR ISDATE(BA.Bestätigter_Termin) = 0) 
					AND ISNULL(BA.[Start Anzahl],0) > 0
					AND ISNULL(BA.erledigt_pos,0) <> 1 
					{Lagerort_sub_filter}
					{artikelNr_Sub_Filter}
					GROUP BY B.[Bestellung-Nr],BA.Bestätigter_Termin,B.[Vorname/NameFirma]
					ORDER BY BA.Bestätigter_Termin ASC ";
				if(RequestedPage >= 0 && PageSize > 0)
				{
					query = query + $" OFFSET {RequestedPage * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.GetOrdersInTimeSpanEntity> GetOrdersInTimeSpan(List<int?> LagersList, int ArtikelNr, DateTime startSpan, DateTime EndSpan)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.GetOrdersInTimeSpanEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				string Lagerort_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND Lagerort_id   IN({pattern})";
				}

				sqlConnection.Open();
				string query =
				 @$"SELECT SUM(b.Anzahl) orderedQuantity,
					Bestätigter_Termin,
					[Artikel-Nr],
					be.[Bestellung-Nr],
					be.Nr,
					b.Liefertermin,
					A.Name1 [SupplierName]
					FROM 
						[bestellte Artikel] b 
						INNER JOIN Bestellungen be ON b.[Bestellung-Nr] = be.Nr 
						Inner Join adressen A On A.Nr = be.[Lieferanten-Nr]
						WHERE 
						b.Bestätigter_Termin BETWEEN '{startSpan.ToString("dd/MM/yyyy")}'  AND '{EndSpan.ToString("dd/MM/yyyy")}'
						AND b.[Position erledigt] <> 1 
						AND ISNULL(Anzahl,0 ) > 0 
						AND be.Typ = 'Bestellung' 
						AND [Artikel-Nr] =  {ArtikelNr} 
						{Lagerort_sub_filter}
					GROUP BY   be.Nr,be.[Bestellung-Nr],b.Liefertermin,Bestätigter_Termin,[Artikel-Nr],A.Name1
					";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.GetOrdersInTimeSpanEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.GetOrdersInTimeSpanEntity>();
			}
		}
		public static async Task<List<Infrastructure.Data.Entities.Joins.MTM.Order.OrdersForArticlesByWeekEntity>> GetOrdersByArtikelNrAndWeekAsync(List<int?> LagersList)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.OrdersForArticlesByWeekEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				string Lagerort_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND Lagerort_id  IN({pattern})";
				}

				await sqlConnection.OpenAsync().ConfigureAwait(false);
				string query =
				$@"SELECT 
					[WeekO]  
					,[Artikel-Nr] artikelNr
					,SUM(Anzahl) orderedQuantitiy
					FROM (
						select 
						case WHEN DATEPART(MONTH , Bestätigter_Termin) = 1 AND DATEPART(DAY , Bestätigter_Termin) < 8 AND DATEPART(iso_week , Bestätigter_Termin) <> 1 
						THEN CONCAT(DATEPART(iso_week , Bestätigter_Termin) , '/' , DATEPART(YEAR , Bestätigter_Termin)-1) 
						ELSE CONCAT(DATEPART(iso_week , Bestätigter_Termin) , '/' , DATEPART(YEAR , Bestätigter_Termin)) 
						END AS [WeekO],
						[Artikel-Nr]
						,Bestätigter_Termin 
						,Anzahl
						from [bestellte Artikel]
						WHERE 
						Anzahl > 0 
						AND Bestätigter_Termin  BETWEEN  GETDATE() AND DATEADD(MONTH , 3 , GETDATE())
						AND [Position erledigt] <> 1 
						 AND Anzahl > 0 
						{Lagerort_sub_filter}
					) s
					Group by  
					[WeekO]
					,[Artikel-Nr]";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				await Task.Run(() => DbExecution.Fill(sqlCommand, dataTable)).ConfigureAwait(false);
				//DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.OrdersForArticlesByWeekEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.OrdersForArticlesByWeekEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.OrdersForArticlesByWeekEntity> GetOrdersByArtikelNrAndWeekFoSearchNummer(List<int?> LagersList, List<int> articles)
		{
			if(LagersList is null || LagersList.Count <= 0 || articles.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.OrdersForArticlesByWeekEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				string Lagerort_sub_filter = "";
				string articles_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND Lagerort_id  IN({pattern})";
				}
				if(articles is not null && (articles.Count > 0))
				{
					string pattern = null;
					foreach(var item in articles)
					{
						if(articles.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(articles.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					articles_sub_filter = $"AND [Artikel-Nr] IN ({pattern})";
				}

				sqlConnection.Open();
				string query =
				$@"SELECT 
					[WeekO]  
					,[Artikel-Nr] artikelNr
					,SUM(Anzahl) orderedQuantitiy
					FROM (
						select 
						case WHEN DATEPART(MONTH , Bestätigter_Termin) = 1 AND DATEPART(DAY , Bestätigter_Termin) < 8 AND DATEPART(iso_week , Bestätigter_Termin) <> 1 
						THEN CONCAT(DATEPART(iso_week , Bestätigter_Termin) , '/' , DATEPART(YEAR , Bestätigter_Termin)-1) 
						ELSE CONCAT(DATEPART(iso_week , Bestätigter_Termin) , '/' , DATEPART(YEAR , Bestätigter_Termin)) 
						END AS [WeekO],
						[Artikel-Nr]
						,Bestätigter_Termin 
						,Anzahl
						from [bestellte Artikel]
						WHERE 
						Anzahl > 0 
						AND Bestätigter_Termin  BETWEEN  GETDATE() AND DATEADD(MONTH , 3 , GETDATE())
						AND [Position erledigt] <> 1 
						 AND Anzahl > 0 
						{Lagerort_sub_filter}
						{articles_sub_filter}
						) s
					Group by  
					[WeekO]
					,[Artikel-Nr]";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.OrdersForArticlesByWeekEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.OrdersForArticlesByWeekEntity>();
			}
		}

	}
}
