using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.CTS
{
	public class StatisticFAAccess
	{

		public static int GetFA1(bool angeboteNr, bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select	count ([Fertigungsnummer]) AS NB from [Fertigung] ";


				if(curentYear == true)
				{
					query += $"  where YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}

				}
				else if(year != 0)
				{
					query += $"  where YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}

				if(angeboteNr == true)
					query += $" AND [Angebot_nr] > 0";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0]["NB"]);
			}
			else
			{
				return 0;
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatLagerEntity> GetFA2(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select F.[Lagerort_id],L.[Lagerort],count(F.[Lagerort_id]) as NB from [Fertigung] F inner join [Lagerorte] L
                                    on F.[Lagerort_id]=L.[Lagerort_id]
                                  
                                    ";
				if(curentYear == true)
				{
					query += $"  where YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  where YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}
				query += "  group by F.[Lagerort_id],L.[Lagerort]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatLagerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatLagerEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatLagerZuEntity> GetFA3(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select F.[Lagerort_id zubuchen],L.[Lagerort],count([Lagerort_id zubuchen]) as NB                      
                                  from [Fertigung] F inner join [Lagerorte] L
                                   on F.[Lagerort_id zubuchen]=L.[Lagerort_id]
                             
                                  ";
				if(curentYear == true)
				{
					query += $"  where YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  where YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}
				query += "  group by F.[Lagerort_id zubuchen],L.[Lagerort]";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatLagerZuEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatLagerZuEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKenzEntity> GetFA4(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select distinct [Kennzeichen],count([Kennzeichen] ) as NB  from [Fertigung]                        
                                where [Kennzeichen] is not null and [Kennzeichen]<>''
                               AND [Kennzeichen] in ('Offen','Storno','Erledigt')
                          
							";
				if(curentYear == true)
				{
					query += $"  AND YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  AND YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}
				query += "  group by [Kennzeichen]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKenzEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKenzEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatGestEntity> GetFA5(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select distinct [FA_Gestartet] ,count([FA_Gestartet]) as NB from [Fertigung] 
                                 where [FA_Gestartet] is not null
                                
								";
				if(curentYear == true)
				{
					query += $"  AND YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  AND YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}
				query += " group by [FA_Gestartet]";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatGestEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatGestEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKomCoEntity> GetFA6(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select distinct [Kommisioniert_komplett],  count([Kommisioniert_komplett]) as NB from [Fertigung] 
                                 where [Kommisioniert_komplett] is not null
                               
								";
				if(curentYear == true)
				{
					query += $"  AND YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  AND YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}
				query += " group by [Kommisioniert_komplett]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKomCoEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKomCoEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKomTrEntity> GetFA7(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @" Select distinct [Kommisioniert_teilweise],  count([Kommisioniert_teilweise]) as NB from [Fertigung] 
                                 where [Kommisioniert_teilweise] is not null
								 AND [Kommisioniert_teilweise] = 0
                              
								";
				if(curentYear == true)
				{
					query += $"  AND YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  AND YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}
				query += " group by [Kommisioniert_teilweise]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKomTrEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKomTrEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTechEntity> GetFA8(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @" Select distinct [Technik] , count([Technik]) as NB from [Fertigung] 
                                 where [Technik] is not null
                            
								";

				if(curentYear == true)
				{
					query += $"  AND YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  AND YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}
				query += " group by [Technik]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTechEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTechEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatARTEntity> GetFA9(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"  Select  top 3  A.[Artikelnummer],A.[Artikel-Nr],L.[Lagerort],COUNT(F.[ID]) AS NB
                                   FROM [Artikel] A inner join [Fertigung] F
                                  inner join [Lagerorte] L on F.[Lagerort_id]=L.[Lagerort_id] 
                                   on A.[Artikel-Nr]=F.[Artikel_Nr]
                                ";

				if(curentYear == true)
				{
					query += $"  where YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  where YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}

				query += $"   group by A.[Artikelnummer],A.[Artikel-Nr],L.[Lagerort] Order by NB desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatARTEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatARTEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerFAEntity> GetFA10(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"  Select  top 3 A.[Artikelnummer],A.[Artikel-Nr],N.[Kunde],COUNT(F.[ID]) AS NB
                                      FROM [Artikel] A inner join [Fertigung] F
                                     on A.[Artikel-Nr]=F.[Artikel_Nr]
                                     inner join [PSZ_Nummerschlüssel Kunde] N
                                     on N.[Nummerschlüssel]=SUBSTRING(A.[Artikelnummer], 1, 3)                                 
                                     where A.[Artikel-Nr] not in (3604,8295,16584,36967)
                                ";

				if(curentYear == true)
				{
					query += $"  AND YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  AND YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}

				query += $"  group by A.[Artikelnummer],A.[Artikel-Nr],N.[Kunde] Order by NB desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerFAEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerFAEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeFAEntity> GetFA11(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"  Select  top 5 [Fertigungsnummer] ,SUM([Zeit]*[Anzahl_erledigt])/60 as _time
                                  from [Fertigung]                    
                                ";

				if(curentYear == true)
				{
					query += $"  where YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  where YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month(D[Datum]atum) = @Month ";
					}

				}
				query += $"    group by [Fertigungsnummer] having SUM([Zeit]* [Anzahl_erledigt])> 0   order by _time desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeFAEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeFAEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeAREntity> GetFA12(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @" Select top 5 A.[Artikelnummer],SUM(F.[Zeit]*F.[Anzahl_erledigt]) as _time
                                  from [Fertigung] F inner join [Artikel] A on F.[Artikel_Nr]=A.[Artikel-Nr]                    
                                ";
				if(curentYear == true)
				{
					query += $"  where YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  where YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}
				query += $"    group by A.[Artikelnummer] having SUM(F.[Zeit]* F.[Anzahl_erledigt])> 0   order by _time desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeAREntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeAREntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeByLagerEntity> GetFA13(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @" Select L.[Lagerort],SUM(F.[Zeit]*F.[Anzahl_erledigt])/60 as _time
                                  from [Fertigung] F inner join [Lagerorte] L on F.[Lagerort_id]=L.[Lagerort_id]                  
                                ";

				if(curentYear == true)
				{
					query += $"  where YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND  MONTH([Datum]) = MONTH(GETDATE())";
					}
				}
				else if(year != 0)
				{
					query += $"  where YEAR([Datum]) = @Year";
					if(month != 0)
					{
						query += $" AND Month([Datum]) = @Month ";
					}

				}
				query += $"   group by L.[Lagerort]  order by _time desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeByLagerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeByLagerEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerFAEntity> GetFA14()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"  Select  top 3 A.[Artikelnummer],A.[Artikel-Nr],N.[Kunde],COUNT(F.[ID]) AS NB
                                      FROM [Artikel] A inner join [Fertigung] F
                                     on A.[Artikel-Nr]=F.[Artikel_Nr]
                                     inner join [PSZ_Nummerschlüssel Kunde] N
                                     on N.[Nummerschlüssel]=SUBSTRING(A.[Artikelnummer], 1, 3)                                 
                                     where A.[Artikel-Nr] not in (3604,8295,16584,36967)
                                     group by A.[Artikelnummer],A.[Artikel-Nr],N.[Kunde] Order by NB desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerFAEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerFAEntity>();
			}
		}
	}
}
