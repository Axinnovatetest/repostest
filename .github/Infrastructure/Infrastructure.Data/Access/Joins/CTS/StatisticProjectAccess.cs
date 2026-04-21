using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.CTS
{
	public class StatisticProjectAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerEntity> GetStats1(string typ)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select TOP 3 [Vorname/NameFirma],COUNT([Typ]) AS NB from [Angebote]
                               ";
				if(!string.IsNullOrEmpty(typ) && !string.IsNullOrWhiteSpace(typ))
				{
					query += $"where [Typ]='{typ.Trim()}'";
				}
				query += " group by [Vorname/NameFirma] order by NB desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerEntity>();
			}
		}
		public static int GetStats2(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"Select COUNT([Projekt-Nr]) AS NB  from [Angebote]
                                 ";
				if(curentYear == true)
				{
					query += $"  where YEAR([Datum]) = YEAR(GETDATE())";

					if(curentMonth == true)
					{
						query += $" AND MONTH([Datum]) = MONTH(GETDATE())";
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

		public static int GetStats3()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select top 1 [Projekt-Nr] FROM [Angebote] where [Angebot-Nr] != 0
                                    and [Projekt-Nr] = [Angebot-Nr]
                                     order by [Datum] desc 
                                              ";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return Convert.ToInt32(dataTable.Rows[0]["Projekt-Nr"]);
			}
			else
			{
				return 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTypEntity> GetStats4(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select distinct Typ,count([Typ]) as NB from [Angebote] where [Typ] is not null and [Typ]<>''
                                 AND [Typ] in ('Auftragsbestätigung','Lieferschein','Rahmenauftrag','Gutschrift')";
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
				query += " group by Typ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTypEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTypEntity>();
			}

		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatPayEntity> GetStats5(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select distinct [Zahlungsweise],count([Zahlungsweise]) as NB  from [Angebote]                         
                                where [Zahlungsweise] is not null and [Zahlungsweise]<>''
                               AND [Zahlungsweise] in ('Rechnung','Vorkasse','Vorauskasse','Gutschriftverfahren','Lastschrift','Überweisung')  ";

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
				query += " group by Zahlungsweise";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatPayEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatPayEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatEdiEntity> GetStats6(bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select distinct [EDI_Order_Neu],[Typ],count([EDI_Order_Neu]) as NB from [Angebote] 
                                 where [EDI_Order_Neu] is not null  
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
				query += " group by [EDI_Order_Neu],[Typ]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatEdiEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatEdiEntity>();
			}

		}

		public static int GetStats7(string typ, bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"Select count([Typ]) as NB from [Angebote] where [Typ] is not null and Typ<>''
                                                ";
				if(!string.IsNullOrEmpty(typ) && !string.IsNullOrWhiteSpace(typ))
				{
					query += $"AND [Typ]='{typ.Trim()}'";
				}
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
		public static List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerEntity> GetStats8(string typ, bool curentYear, bool curentMonth, int year, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"Select TOP 3 [Vorname/NameFirma],COUNT([Typ]) AS NB from [Angebote] where [Typ] <>''
                               ";
				if(!string.IsNullOrEmpty(typ) && !string.IsNullOrWhiteSpace(typ))
				{
					query += $"AND [Typ]='{typ}'";
				}
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
				query += " group by [Vorname/NameFirma] order by NB desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				sqlCommand.Parameters.AddWithValue("@Month", month);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerEntity>();
			}
		}
		public static int GetStatsZahlungsweise(string typ, bool curentYear, bool curentMonth, int year, int month, string statType)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"Select count([Zahlungsweise]) as NB  from [Angebote]                         
                                where [Zahlungsweise] is not null and [Zahlungsweise]<>''
                               AND [Zahlungsweise] = '{statType.Trim()}' ";
				if(!string.IsNullOrEmpty(typ) && !string.IsNullOrWhiteSpace(typ))
				{
					query += $"AND [Typ]='{typ.Trim()}'";
				}

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

		public static int GetStatsEdi(string typ, bool curentYear, bool curentMonth, int year, int month, int statEdi)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@" Select count([EDI_Order_Neu]) as NB from [Angebote] 
                                 where [EDI_Order_Neu] is not null  AND [EDI_Order_Neu] ='{statEdi}'
                                ";
				if(!string.IsNullOrEmpty(typ) && !string.IsNullOrWhiteSpace(typ))
				{
					query += $"AND [Typ]='{typ.Trim()}'";
				}
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
	}
}
