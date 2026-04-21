using System;
using System.Data.SqlClient;
using System.Data;
using static Infrastructure.Data.Access.Enums.FNCEnums;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.Entities.Joins.FNC.Statistics;
using System.ComponentModel.Design;

namespace Infrastructure.Data.Access.Joins.FNC.Statistics
{
	public class StatsticsAccess
	{
		public static decimal GetAmountTotal(Amountname name, AmountTable table, int id, int year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var column = "";
				if(table == AmountTable.__FNC_BudgetAllocationCompany)
					column = "CompanyId";
				if(table == AmountTable.__FNC_BudgetAllocationDepartment)
					column = "DepartmentId";
				if(table == AmountTable.__FNC_BudgetAllocationUser)
					column = "UserId";
				string query = $"SELECT SUM([{name.ToString()}]) FROM [{table.ToString()}] WHERE [Year]={year} {(id != -1 ? $"AND [{column}]=@Id" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				if(id != -1)
					sqlCommand.Parameters.AddWithValue("Id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows[0][0] == System.DBNull.Value
					? 0
					: Convert.ToDecimal(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}
		public static decimal GetProjectsCount()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = "select COUNT(Id) from __FNC_BudgetProject";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows[0][0] == System.DBNull.Value
					? 0
					: Convert.ToDecimal(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}
		public static List<KeyValuePair<int, int>> GetProjectsCountByApprovalStatus(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT COUNT(Id) AS _count,[Id_State] FROM [__FNC_BudgetProject]
                                 WHERE [ProjectStatusName] IS NOT NULL";
				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND CreationUserId={employeeId}";
				query += " GROUP BY [Id_State]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x => new KeyValuePair<int, int>(Convert.ToInt32(x["_count"]), Convert.ToInt32(x["Id_State"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, string>> GetProjectsCountByStatus(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT COUNT(Id) AS _count,[ProjectStatusName] FROM [__FNC_BudgetProject]
                                 WHERE [ProjectStatusName] IS NOT NULL";
				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND CreationUserId={employeeId}";
				query += " GROUP BY [ProjectStatusName]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x => new KeyValuePair<int, string>(Convert.ToInt32(x["_count"]), Convert.ToString(x["ProjectStatusName"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, string>> GetProjectsCountByType(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT COUNT(Id) AS _count,[Type] FROM [__FNC_BudgetProject]
                                 WHERE [Type] IS NOT NULL";
				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND CreationUserId={employeeId}";
				query += " GROUP BY [Type]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x => new KeyValuePair<int, string>(Convert.ToInt32(x["_count"]), Convert.ToString(x["Type"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<AllocationsVsOrdersAmountEntity> GetProjectsVsOrdersAmount(int? month = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = $@"SELECT TOP 10 B.[ProjectName],COUNT(O.Id) AS OrdersCount,B.[ProjectBudget],SUM(A.[TotalCostDefaultCurrency]) AS OrdersAmount
                               FROM [__FNC_BudgetProject] B INNER JOIN [__FNC_BestellungenExtension] O
                               ON B.[Id]=O.[ProjectId]
                               INNER JOIN [__FNC_BestellteArtikelExtension] A
                               ON O.[OrderId]=A.[OrderId]
                               {(month.HasValue ? $"WHERE MONTH(B.CreationDate)={month}" : "")}
                               GROUP BY B.[ProjectName],B.[ProjectBudget]
                               ORDER BY OrdersCount DESC";
				sqlConnection.Open();
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.AllocationsVsOrdersAmountEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<AllocationsVsOrdersAmountEntity> GetTop10ProjectsByAmountOrBudget(int? year, int? companyId, int? departmentId, int? employeeId, bool byAmount = false, int? month = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = $@"SELECT Id,ProjectName,OrdersCount,ProjectBudget,OrdersAmount-ISNULL(Discount,0) as OrdersAmount FROM
								(
								SELECT TOP 10 B.Id,B.[ProjectName],COUNT(distinct O.Id) AS OrdersCount,B.[ProjectBudget],
								CASE 
									WHEN O.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])
									WHEN O.PoPaymentTypeName = 'Leasing' THEN O.LeasingNbMonths*O.LeasingMonthAmount
									ELSE 0
								END
							   AS OrdersAmount,O.Discount
                               FROM [__FNC_BudgetProject] B INNER JOIN [__FNC_BestellungenExtension] O
                               ON B.[Id]=O.[ProjectId]
                               INNER JOIN [__FNC_BestellteArtikelExtension] A
                               ON O.[OrderId]=A.[OrderId] ";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(B.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} B.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} B.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} MONTH(B.CreationUserId)={employeeId}";
					isFirstCondition = false;
				}
				if(month.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} MONTH(B.CreationDate)={month}";
					isFirstCondition = false;
				}
				query += $@"{(isFirstCondition ? " WHERE" : " AND")} B.Id_Type<>3 GROUP BY B.[Id],B.[ProjectName],B.[ProjectBudget],O.Discount,O.PoPaymentTypeName,O.LeasingNbMonths,O.LeasingMonthAmount
                           ORDER BY {(byAmount ? "OrdersAmount" : "ProjectBudget")} DESC ) X";

				sqlConnection.Open();
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.AllocationsVsOrdersAmountEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		//
		public static List<KeyValuePair<int, int>> GetOrdersCountByStatus(int? year, int? companyId, int? departmentId, int? emplyoeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT COUNT(distinct B.[OrderId]) AS _count,[Status]
                                 FROM [__FNC_BestellungenExtension] B INNER JOIN [__FNC_BestellteArtikelExtension] A
                                 ON B.[OrderId]=A.[OrderId]
                                 WHERE B.[Status]<=3";

				if(year.HasValue)
					query += $" AND YEAR(B.CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND B.CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND B.DepartmentId={departmentId}";
				if(emplyoeeId.HasValue)
					query += $" AND B.IssuerId={emplyoeeId}";

				query += "GROUP BY B.[Status]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x => new KeyValuePair<int, int>(Convert.ToInt32(x["_count"]), Convert.ToInt32(x["Status"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, string>> GetOrdersCountByPoPayementType(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT COUNT(distinct B.[OrderId]) AS _count,[PoPaymentTypeName]
                                 FROM [__FNC_BestellungenExtension] B INNER JOIN [__FNC_BestellteArtikelExtension] A
                                 ON B.[OrderId]=A.[OrderId]";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(B.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} B.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} B.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} B.IssuerId={employeeId}";
					isFirstCondition = false;
				}

				query += $"{(isFirstCondition ? " WHERE" : " AND")} B.Status=4 GROUP BY B.[PoPaymentTypeName]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x => new KeyValuePair<int, string>(Convert.ToInt32(x["_count"]), Convert.ToString(x["PoPaymentTypeName"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, string>> GetOrdersCountByType(int? year, int? companyId, int? departmentId, int? emplyoeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT COUNT(distinct B.[OrderId]) AS _count,[OrderType]
                                FROM [__FNC_BestellungenExtension] B INNER JOIN [__FNC_BestellteArtikelExtension] A
                                ON B.[OrderId]=A.[OrderId]";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(B.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} B.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} B.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(emplyoeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} B.IssuerId={emplyoeeId}";
					isFirstCondition = false;
				}
				query += $"{(isFirstCondition ? " WHERE" : " AND")} B.Status=4 GROUP BY B.[OrderType]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x => new KeyValuePair<int, string>(Convert.ToInt32(x["_count"]), Convert.ToString(x["OrderType"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int GetOrdersCountPlacedOrBooked(int? year, int? companyId, int? departmentId, int? emplyoeeId, bool placed = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = placed
					? @"SELECT COUNT(distinct B.[OrderId]) AS _count FROM [__FNC_BestellungenExtension] B INNER JOIN [__FNC_BestellteArtikelExtension] A
                       ON B.[OrderId]=A.[OrderId]
                       WHERE B.[OrderPlacedTime] IS NOT NULL"
					: @"SELECT COUNT(distinct B.[Bestellung-Nr]) AS _count 
                      FROM [Bestellungen] B INNER JOIN [__FNC_BestellungenExtension] BA
                      ON B.[Bestellung-Nr]=BA.[OrderId] INNER JOIN [__FNC_BestellteArtikelExtension] A
                      ON BA.[OrderId]=A.[OrderId]
                      WHERE [Typ]='Wareneingang'";

				if(year.HasValue)
					query += $" AND YEAR({(placed ? "B." : "BA.")}CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND {(placed ? "B." : "BA.")}CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND {(placed ? "B." : "BA.")}DepartmentId={departmentId}";
				if(emplyoeeId.HasValue)
					query += $" AND {(placed ? "B." : "BA.")}IssuerId={emplyoeeId}";

				query += $"AND {(placed ? "B." : "BA.")}Status=4";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows[0][0] == System.DBNull.Value
					? 0
					: Convert.ToInt32(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}
		//
		public static List<Tuple<string, int, string, int>> GetOrdersMonthlyViewByType(string type, int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT DATENAME(MONTH, CreationDate) AS _month,
                               MONTH(CreationDate) as _monthNumber,
                               OrderType,COUNT(distinct B.OrderId) AS _count
                               FROM [__FNC_BestellungenExtension] B INNER JOIN [__FNC_BestellteArtikelExtension] A
                               ON B.[OrderId]=A.[OrderId] 
                               WHERE OrderType=@type
                               AND [Status]=4";

				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId= {departmentId}";
				if(employeeId.HasValue)
					query += $" AND IssuerId= {employeeId}";
				query += @" GROUP BY MONTH(CreationDate),OrderType,DATENAME(MONTH, CreationDate)
                           ORDER BY MONTH(CreationDate)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("type", type);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new Tuple<string, int, string, int>(
					Convert.ToString(x["_month"]),
					Convert.ToInt32(x["_monthNumber"]),
					Convert.ToString(x["OrderType"]),
					Convert.ToInt32(x["_count"])
					)
				).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Tuple<string, int, string, int>> GetOrdersMonthlyViewByPoPaymentType(string poPayementtype, int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT DATENAME(MONTH, CreationDate) AS _month,
                               MONTH(CreationDate) as _monthNumber,
                               PoPaymentTypeName,COUNT(distinct B.OrderId) AS _count
                               FROM [__FNC_BestellungenExtension] B INNER JOIN [__FNC_BestellteArtikelExtension] A
                               ON B.[OrderId]=A.[OrderId] 
                               WHERE PoPaymentTypeName=@poPayementtype AND Status=4";

				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND IssuerId={employeeId}";
				query += @" GROUP BY MONTH(CreationDate),PoPaymentTypeName,DATENAME(MONTH, CreationDate)
                           ORDER BY MONTH(CreationDate)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("poPayementtype", poPayementtype);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new Tuple<string, int, string, int>(
					Convert.ToString(x["_month"]),
					Convert.ToInt32(x["_monthNumber"]),
					Convert.ToString(x["PoPaymentTypeName"]),
					Convert.ToInt32(x["_count"])
					)
				).ToList();
			}
			else
			{
				return null;
			}
		}
		//
		public static List<KeyValuePair<string, int>> GetProjectsmonthlyViewAll(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT DATENAME(MONTH, CreationDate) AS _month,
                               COUNT(Id) AS _count
                               FROM __FNC_BudgetProject";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} CreationUserId={employeeId}";
					isFirstCondition = false;
				}
				query += @" GROUP BY MONTH(CreationDate),DATENAME(MONTH, CreationDate)
                           ORDER BY MONTH(CreationDate)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x => new KeyValuePair<string, int>(Convert.ToString(x["_month"]), Convert.ToInt32(x["_count"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Tuple<string, int, int>> GetProjectsMonthlyViewByStatus(int status, int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT DATENAME(MONTH, CreationDate) AS _month,Id_State,COUNT(Id) AS _count
                               FROM __FNC_BudgetProject
                               WHERE Id_State=@status";

				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND CreationUserId={employeeId}";
				query += @" GROUP BY MONTH(CreationDate),Id_State,DATENAME(MONTH, CreationDate)
                           ORDER BY MONTH(CreationDate)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("status", status);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new Tuple<string, int, int>(
					Convert.ToString(x["_month"]),
					Convert.ToInt32(x["Id_State"]),
					Convert.ToInt32(x["_count"])
					)
				).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Tuple<string, string, int>> GetProjectsMonthlyViewByApprovalStatus(int approvalStatus, int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT DATENAME(MONTH, CreationDate) AS _month,ProjectStatusName,COUNT(Id) AS _count
                               FROM __FNC_BudgetProject
                               WHERE ProjectStatus=@approvalStatus";

				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND CreationUserId={employeeId}";
				query += @" GROUP BY MONTH(CreationDate),ProjectStatusName,DATENAME(MONTH, CreationDate)
                           ORDER BY MONTH(CreationDate)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("approvalStatus", approvalStatus);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new Tuple<string, string, int>(
					Convert.ToString(x["_month"]),
					Convert.ToString(x["ProjectStatusName"]),
					Convert.ToInt32(x["_count"])
					)
				).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Tuple<string, string, int>> GetProjectsMonthlyViewByType(string type, int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT DATENAME(MONTH, CreationDate) AS _month,Type,COUNT(Id) AS _count
                               FROM __FNC_BudgetProject
                               WHERE Type=@type";

				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND CreationUserId={employeeId}";
				query += @" GROUP BY MONTH(CreationDate),Type,DATENAME(MONTH, CreationDate)
                           ORDER BY MONTH(CreationDate)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("type", type);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new Tuple<string, string, int>(
					Convert.ToString(x["_month"]),
					Convert.ToString(x["Type"]),
					Convert.ToInt32(x["_count"])
					)
				).ToList();
			}
			else
			{
				return null;
			}
		}
		//
		public static List<Tuple<int, string, decimal>> GetOrdersHighestOrLowestAmount(int? year, int? companyId, int? departmentId, int? employeeId, bool desc = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = $@"SELECT Id,OrderNumber,ISNULL(Amount,0)-ISNULL(Discount,0) as Amount FROM
								(
								SELECT TOP 5 B.[OrderId] AS Id,B.[OrderNumber],CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN  SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								END AS Amount,B.Discount,B.PoPaymentTypeName FROM
                               [__FNC_BestellungenExtension] B INNER JOIN [__FNC_BestellteArtikelExtension] A
                               ON B.[OrderId]=A.[OrderId]";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} YEAR(B.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} B.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} B.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} B.IssuerId={employeeId}";
					isFirstCondition = false;
				}
				query += @$" GROUP BY B.[OrderId],B.[OrderNumber],B.Discount,B.PoPaymentTypeName,B.LeasingNbMonths,B.LeasingMonthAmount
                            ORDER BY Amount {(desc ? "DESC" : "")} ) X";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new Tuple<int, string, decimal>(
					   Convert.ToInt32(x["Id"]),
					   Convert.ToString(x["OrderNumber"]),
					   Convert.ToDecimal(x["Amount"]))
				).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<FastestOrSlowestOrdersEntity> GetSlowestOrFastestOrders(int? year, int? companyId, int? departmentId, int? employeeId, bool desc = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = $@"select top 5 Id as Id,OrderNumber,ValidationRequestTime,Termin,DATEDIFF(DAY,ValidationRequestTime,termin) as Diff from
								(
								select b.OrderId as Id,b.OrderNumber,MIN(validationTime) as ValidationRequestTime,b.ApprovalTime as Termin
								from __FNC_OrderValidation v inner join __FNC_BestellungenExtension b
								on v.OrderId=b.OrderId
								Where ValidationLevel=0
								and b.ApprovalTime is not null";

				if(year.HasValue)
					query += $" AND YEAR(b.CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND b.CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND b.DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND b.IssuerId={employeeId}";
				query += @$" group by b.OrderId,b.OrderNumber,b.ApprovalTime
							) X
                            ORDER BY Diff {(desc ? "DESC" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.FastestOrSlowestOrdersEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<OrdersHighestDelayEntity> GetOrdersHighestDelay(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"select top 5 A.*,DATEDIFF(DAY,A.DeliveryWishDate,A.DeliveryActualDate) as Diff from 
								(
								select X.*,Datum as DeliveryActualDate from Bestellungen b inner join 
								(
								select bae.OrderId as Id,be.OrderNumber,B.[Vorname/NameFirma] as Supplier,MIN(COALESCE(ba.Bestätigter_Termin,ba.Liefertermin)) as DeliveryWishDate
								from __FNC_BestellteArtikelExtension bae inner join Bestellungen b
								on bae.OrderId=b.[Bestellung-Nr]
								inner join [bestellte Artikel] ba on b.Nr=ba.[Bestellung-Nr]
								inner join __FNC_BestellungenExtension be on be.OrderId=b.[Bestellung-Nr]";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} IssuerId={employeeId}";
					isFirstCondition = false;
				}
				query += @" group by bae.OrderId,be.OrderNumber,B.[Vorname/NameFirma]
								) X
								on b.[Bestellung-Nr]=x.Id
								where b.[Typ]='Wareneingang'
								) A
								order by diff desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.OrdersHighestDelayEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<OrdersSlowestBookingEntity> GetOrdersSlowestBookings(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT TOP 5 * FROM (
								SELECT BE.OrderId AS Id, BE.OrderNumber,MAX(B.Datum) AS MaxDate,MIN(B.Datum) AS MinDate,
								DATEDIFF(DAY,MIN(B.Datum), MAX(B.Datum)) AS Diff
								FROM (
									SELECT TRY_CAST([Projekt-Nr] AS INT) [Projekt-Nr], Datum FROM [Bestellungen]
									WHERE [Bestellungen].[Typ]='Wareneingang' and TRY_CAST([Projekt-Nr] AS INT) IS NOT NULL 
                                     ) B
								INNER JOIN __FNC_BestellungenExtension BE
								ON B.[Projekt-Nr]=BE.OrderId";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(BE.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} BE.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} BE.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} BE.IssuerId={employeeId}";
					isFirstCondition = false;
				}
				query += @"GROUP BY BE.OrderId,BE.OrderNumber
						) X
						WHERE Diff>0
						ORDER BY Diff DESC";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.OrdersSlowestBookingEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		//
		public static List<Tuple<int, string, decimal>> GetProjectsWithBiggestBudget(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT TOP 5 [Id],[ProjectName],[ProjectBudget] AS Amount 
                               FROM [__FNC_BudgetProject]";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} CreationUserId={employeeId}";
					isFirstCondition = false;
				}
				query += $" {(isFirstCondition ? " WHERE" : " AND")} Id_Type<>3";
				query += " ORDER BY [ProjectBudget] DESC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new Tuple<int, string, decimal>(
					Convert.ToInt32(x["Id"]),
					Convert.ToString(x["ProjectName"]),
					Convert.ToDecimal(x["Amount"]))
				).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Tuple<int, string, decimal>> GetProjectsWithBiggestOrdersAmounts(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT [Id],[ProjectName],Amount-ISNULL(Discount,0) as Amount from 
								(
								SELECT TOP 5 P.[Id],P.[ProjectName],CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN  SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								END AS Amount,B.Discount
                               FROM [__FNC_BudgetProject] P 
                               INNER JOIN 
                               [__FNC_BestellungenExtension] B
                               INNER JOIN [__FNC_BestellteArtikelExtension] A
                               ON B.[OrderId]=A.[OrderId]
                               ON P.[Id]=B.[ProjectId]";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(P.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} P.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} P.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} P.CreationUserId={employeeId}";
					isFirstCondition = false;
				}
				query += $" {(isFirstCondition ? "WHERE" : "AND")} B.[Archived]=0 AND B.[Deleted]=0 AND B.[Level]>0 AND P.ProjectStatusName='Closed'";
				query += $" {(isFirstCondition ? "WHERE" : "AND")} P.Id_Type<>3 AND B.Status=4";
				query += @" GROUP BY P.[Id],P.[ProjectName],B.Discount,B.PoPaymentTypeName,B.LeasingNbMonths,B.LeasingMonthAmount
                            ORDER BY Amount DESC ) X";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new Tuple<int, string, decimal>(
					Convert.ToInt32(x["Id"]),
					Convert.ToString(x["ProjectName"]),
					Convert.ToDecimal(x["Amount"]))
				).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Tuple<int, string, DateTime>> GetOldestProjects(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT TOP 5 [Id],[ProjectName],MIN([ApprovalTime]) OldestApproval
                               FROM [__FNC_BudgetProject]
                               WHERE [ApprovalTime] IS NOT NULL AND [ProjectStatus]=1 AND [Id_Type]<>3";

				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND CreationUserId={employeeId}";
				query += " GROUP BY [Id],[ProjectName] ORDER BY MIN([ApprovalTime])";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new Tuple<int, string, DateTime>(
					Convert.ToInt32(x["Id"]),
					Convert.ToString(x["ProjectName"]),
					Convert.ToDateTime(x["OldestApproval"]))
				).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Tuple<int, string, decimal>> GetMostProfitableProjects(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT TOP 5 [Id],[ProjectName],[PSZOffer]-[ProjectBudget] AS Amount FROM [__FNC_BudgetProject]
                                 WHERE [Id_Type]=1 AND [PSZOffer] IS NOT NULL AND [ProjectBudget] IS NOT NULL";

				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND CreationUserId={employeeId}";
				query += " ORDER BY [PSZOffer]-[ProjectBudget] DESC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new Tuple<int, string, decimal>(
					Convert.ToInt32(x["Id"]),
					Convert.ToString(x["ProjectName"]),
					Convert.ToDecimal(x["Amount"]))
				).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<OverbudgetedProjectsEntity> GetOverbudgetedProjects(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT [Id],[ProjectName],[ProjectBudget], SUM((1-ISNULL(Discount,0)/100) * OrdersAmount) AS OrdersAmount,0 AS Diffrence FROM 
                                 (
                                 SELECT TOP 5 P.[Id],P.[ProjectName],P.[ProjectBudget],B.Discount,
                                 
								 CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								 END
							      AS OrdersAmount,
                                 CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								 END
							     -P.[ProjectBudget] AS Diffrence
                                 FROM [__FNC_BudgetProject] P 
                                 INNER JOIN 
                                 [__FNC_BestellungenExtension] B
                                 INNER JOIN [__FNC_BestellteArtikelExtension] A
                                 ON B.[OrderId]=A.[OrderId]
                                 ON P.[Id]=B.[ProjectId]";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(P.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} P.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} P.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} P.CreationUserId={employeeId}";
					isFirstCondition = false;
				}
				query += $@"{(isFirstCondition ? " WHERE" : " AND")} B.[Archived]=0 AND B.[Deleted]=0 AND B.[Level]>0 AND P.ProjectStatusName='Closed' AND P.Id_Type<>3";
				isFirstCondition = false;
				query += @" GROUP BY P.[Id],P.[ProjectName],P.[ProjectBudget],B.Discount,B.PoPaymentTypeName,B.LeasingNbMonths,B.LeasingMonthAmount
                           HAVING CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								 END>P.[ProjectBudget]
                           ORDER BY CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								 END-P.[ProjectBudget] DESC ) X
						  GROUP BY [Id],[ProjectName],[ProjectBudget]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.OverbudgetedProjectsEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<OverbudgetedProjectsEntity> GetBudgetLeakProjects(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT [Id],[ProjectName],[ProjectBudget], SUM((1-ISNULL(Discount,0)/100) * OrdersAmount) AS OrdersAmount,0 AS Diffrence FROM 
                                 (
                                 SELECT TOP 5 P.[Id],P.[ProjectName],P.[ProjectBudget],B.Discount,
                                 
								 CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								 END
							      AS OrdersAmount,
                                P.[ProjectBudget]-
                                CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								 END
							     AS Diffrence
                                 FROM [__FNC_BudgetProject] P 
                                 INNER JOIN 
                                 [__FNC_BestellungenExtension] B
                                 INNER JOIN [__FNC_BestellteArtikelExtension] A
                                 ON B.[OrderId]=A.[OrderId]
                                 ON P.[Id]=B.[ProjectId]";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(P.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} P.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} P.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} P.CreationUserId={employeeId}";
					isFirstCondition = false;
				}
				query += $@"{(isFirstCondition ? " WHERE" : " AND")} B.[Archived]=0 AND B.[Deleted]=0 AND B.[Level]>0 AND P.ProjectStatusName='Closed' AND P.Id_Type<>3";
				isFirstCondition = false;
				query += @" GROUP BY P.[Id],P.[ProjectName],P.[ProjectBudget],B.Discount,B.PoPaymentTypeName,B.LeasingNbMonths,B.LeasingMonthAmount
                           HAVING CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								 END<P.[ProjectBudget]
                           ORDER BY CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								 END-P.[ProjectBudget] DESC ) X
						  GROUP BY [Id],[ProjectName],[ProjectBudget]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.OverbudgetedProjectsEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		//
		public static List<OverdueOrdersEntity> GetWorstSuppliersBiggestOverdueOrders(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"select top 5 A.*,DATEDIFF(DAY,A.DeliveryWishDate,A.DeliveryActualDate) as Diff from 
								(
								select X.*,Datum as DeliveryActualDate from Bestellungen b inner join 
								(
								select bae.OrderId as Id,be.OrderNumber,B.[Vorname/NameFirma] as Supplier,MIN(COALESCE(ba.Bestätigter_Termin,ba.Liefertermin)) as DeliveryWishDate
								from __FNC_BestellteArtikelExtension bae inner join Bestellungen b
								on bae.OrderId=b.[Bestellung-Nr]
								inner join [bestellte Artikel] ba on b.Nr=ba.[Bestellung-Nr]
								inner join __FNC_BestellungenExtension be on be.OrderId=b.[Bestellung-Nr]";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} IssuerId={employeeId}";
					isFirstCondition = false;
				}
				query += @" group by bae.OrderId,be.OrderNumber,B.[Vorname/NameFirma]
								) X
								on b.[Bestellung-Nr]=x.Id
								where b.[Typ]='Wareneingang'
								) A
								order by diff desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.OverdueOrdersEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<string, int>> GetWorstSuppliersBiggestCountOfOverdueOrders(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"select top 5 Supplier,COUNT(distinct Id) as OverdueOrders from 
(
								select A.*,DATEDIFF(DAY,A.DeliveryWishDate,A.DeliveryActualDate) as Diff from 
								(
								select X.*,Datum as DeliveryActualDate from Bestellungen b inner join 
								(
								select bae.OrderId as Id,be.OrderNumber,be.SupplierName as Supplier,MIN(COALESCE(ba.Bestätigter_Termin,ba.Liefertermin)) as DeliveryWishDate
								from __FNC_BestellteArtikelExtension bae 
								inner join __FNC_BestellungenExtension be on be.OrderId=bae.OrderId
								inner join [bestellte Artikel] ba on be.OrderId=ba.[Bestellung-Nr]";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} IssuerId={employeeId}";
					isFirstCondition = false;
				}
				query += @" group by bae.OrderId,be.OrderNumber,be.SupplierName
								) X
								on b.[Bestellung-Nr]=x.Id
								where b.[Typ]='Wareneingang'
								) A where DATEDIFF(DAY,A.DeliveryWishDate,A.DeliveryActualDate)>0
								) Y
								group by Supplier
								order by OverdueOrders desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x => new KeyValuePair<string, int>
								(Convert.ToString(x["Supplier"]),
								Convert.ToInt32(x["OverdueOrders"]))
								).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<BookingCountForOneOrderEntity> GetWorstSuppliersBiggestBookingCountForOneOrder(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"select top 5 be.OrderId as Id,be.OrderNumber,b.[Vorname/NameFirma] AS Supplier, count(distinct b.Nr) as BookingCount  
								from bestellungen b inner join __FNC_BestellungenExtension be on b.[Bestellung-Nr]=be.OrderId
								Where b.Typ='Wareneingang'";

				if(year.HasValue)
					query += $" AND YEAR(be.CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND be.CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND be.DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND be.IssuerId={employeeId}";
				query += @" GROUP BY [Projekt-Nr],b.[Vorname/NameFirma],be.OrderId,be.OrderNumber
							HAVING count(distinct b.Nr)>1 and [Projekt-Nr] is not null and [Projekt-Nr]<>''
							order by BookingCount desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.BookingCountForOneOrderEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		//
		public static List<KeyValuePair<string, int>> GetBestSuppliersOrdersCount(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT TOP 5 [Vorname/NameFirma] AS Supplier,
                                 COUNT(distinct b.[Bestellung-Nr]) AS OrdersCount
                                 FROM [Bestellungen] b inner join __FNC_BestellungenExtension be on b.[Bestellung-Nr]=be.OrderId
                                 INNER JOIN [__FNC_BestellteArtikelExtension] a on be.[OrderId]=a.[OrderId]
                                 where A.[TotalCostDefaultCurrency] IS NOT NULL";

				if(year.HasValue)
					query += $" AND YEAR(be.CreationDate)={year}";

				if(companyId.HasValue)
					query += $" AND be.CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND be.DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND be.IssuerId={employeeId}";
				query += @" GROUP BY [Vorname/NameFirma] ORDER BY OrdersCount DESC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x => new KeyValuePair<string, int>
				(Convert.ToString(x["Supplier"]),
				Convert.ToInt32(x["OrdersCount"]))
				).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<string, decimal>> GetBestSuppliersOrdersAmount(int? year, int? companyId, int? departmentId, int? employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT Supplier,OrdersAmount-ISNULL(Discount,0) AS OrdersAmount from
								(
								SELECT TOP 5 be.SupplierName AS Supplier,
									CASE 
										WHEN be.PoPaymentTypeName = 'Purchase' THEN SUM(BAE.[TotalCostDefaultCurrency])
										WHEN be.PoPaymentTypeName = 'Leasing' THEN be.LeasingNbMonths*be.LeasingMonthAmount
										ELSE 0
									END
								 AS OrdersAmount,be.Discount
                                 FROM __FNC_BestellungenExtension be inner join [__FNC_BestellteArtikelExtension] BAE
								 on be.OrderId=BAE.OrderId";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} YEAR(be.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} be.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} be.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} be.IssuerId={employeeId}";
					isFirstCondition = false;
				}
				query += @" GROUP BY be.SupplierName,be.Discount,be.PoPaymentTypeName,be.LeasingNbMonths,be.LeasingMonthAmount
								 ORDER BY OrdersAmount DESC
								 ) X";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new KeyValuePair<string, decimal>(
					Convert.ToString(x["Supplier"]),
					Convert.ToDecimal(x["OrdersAmount"]))
				).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<OrdersOverviewEntity> GetBestSuppliersOverviwOrdersCount(int? year, int? companyId, int? departmentId, int? employeeId, string supplier)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = $@"SELECT Id,OrderNumber,SupplierName,Amount-ISNULL(Discount,0) as Amount,PoPaymentTypeName,OrderType,Status FROM 
								(
								SELECT B.[OrderId] AS Id,B.[OrderNumber],B.SupplierName as SupplierName,
								CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								END
							    AS Amount,
                                B.PoPaymentTypeName,B.OrderType,B.Status,B.Discount FROM
								[__FNC_BestellungenExtension] B INNER JOIN [__FNC_BestellteArtikelExtension] A
								ON B.[OrderId]=A.[OrderId]
								WHERE B.SupplierName='{supplier}'";

				if(year.HasValue)
					query += $" AND YEAR(B.CreationDate)={year}";

				if(companyId.HasValue)
					query += $" AND B.CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND B.DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND B.IssuerId={employeeId}";
				query += @" GROUP BY B.[OrderId],B.[OrderNumber],B.SupplierName,B.PoPaymentTypeName,B.OrderType,B.Status,B.Discount,B.LeasingNbMonths,B.LeasingMonthAmount) X";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.OrdersOverviewEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<OrdersOverviewEntity> GetWorstSuppliersOverviewBiggetCountOfOverdueOrders(List<int> ids)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = $@"select B.OrderId AS Id,B.OrderNumber,B.PoPaymentTypeName,B.SupplierName,B.OrderType,
                                    CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN  SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								END as Amount,B.Status
								from __FNC_BestellungenExtension B inner join __FNC_BestellteArtikelExtension A
								on A.OrderId=B.OrderId
								where B.OrderId in 
								(
                                 {string.Join(",", ids)}
								)
								GROUP BY 
									B.OrderId, 
									B.OrderNumber, 
									B.PoPaymentTypeName, 
									B.SupplierName, 
									B.LeasingNbMonths, 
									B.LeasingMonthAmount, 
									B.OrderType,
									B.Status;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.OrdersOverviewEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		//
		public static List<KeyValuePair<string, decimal>> GetBestCustomersBestProfit(int? year, int? companyId, int? departmentId, int? employeeId, bool desc = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = $@"SELECT TOP 5 [CustomerName],SUM([PSZOffer]-[ProjectBudget]) AS Profit
                                 FROM [__FNC_BudgetProject]
                                 WHERE [CustomerName] IS NOT NULL AND [CustomerName]<>''";

				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND CreationUserId={employeeId}";
				query += @$" GROUP BY [CustomerName]
                            HAVING SUM([PSZOffer]-[ProjectBudget]) IS NOT NULL
                            ORDER BY Profit {(desc ? "DESC" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new KeyValuePair<string, decimal>(
					Convert.ToString(x["CustomerName"]),
					Convert.ToDecimal(x["Profit"]))
				).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<string, decimal>> GetBestCustomersBestPszOffer(int? year, int? companyId, int? departmentId, int? employeeId, bool desc = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = $@"SELECT TOP 5 [CustomerName],[PSZOffer]
                                 FROM [__FNC_BudgetProject]
                                 WHERE [CustomerName] IS NOT NULL AND [CustomerName]<>''";

				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND CreationUserId={employeeId}";
				query += $"ORDER BY [PSZOffer] {(desc ? "DESC" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				new KeyValuePair<string, decimal>(
					Convert.ToString(x["CustomerName"]),
					Convert.ToDecimal(x["PSZOffer"]))
				).ToList();
			}
			else
			{
				return null;
			}
		}
		//
		public static List<ProjectsOverviewEntity> GetProjectsOverviewByStatus(string status, int? year, int? companyId, int? departmentId, int? employeeId, string month = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT  P.[Id],P.[ProjectName],[Id_State],P.ProjectStatusName,P.[Type],P.[ProjectBudget]
                                 FROM [__FNC_BudgetProject] P 
								 WHERE P.ProjectStatusName=@status";

				if(year.HasValue)
					query += $" AND YEAR(P.CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND P.CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND P.DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND P.CreationUserId={employeeId}";
				if(!string.IsNullOrEmpty(month) && !string.IsNullOrWhiteSpace(month))
					query += $" AND DATENAME(MONTH, CreationDate)='{month}'";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("status", status);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.ProjectsOverviewEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<ProjectsOverviewEntity> GetProjectsOverviewByCustomer(int? year, int? companyId, int? departmentId, int? employeeId, string customer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"select [Id],[ProjectName],[Id_State],ProjectStatusName,[Type],[ProjectBudget]
								from [__FNC_BudgetProject]
								where CustomerName=@customer";

				if(year.HasValue)
					query += $" AND YEAR(CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND CreationUserId={employeeId}";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customer", customer);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.ProjectsOverviewEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static List<ProjectsOverviewEntity> GetProjectsOverviewByApprovalStatus(int status, int? year, int? companyId, int? departmentId, int? employeeId, string month = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT  P.[Id],P.[ProjectName],[Id_State],P.ProjectStatusName,P.[Type],P.[ProjectBudget]
                                 FROM [__FNC_BudgetProject] P 
								 WHERE P.[Id_State]=@status";

				if(year.HasValue)
					query += $" AND YEAR(P.CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND P.CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND P.DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND P.CreationUserId={employeeId}";
				if(!string.IsNullOrEmpty(month) && !string.IsNullOrWhiteSpace(month))
					query += $" AND DATENAME(MONTH, CreationDate)='{month}'";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("status", status);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.ProjectsOverviewEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<ProjectsOverviewEntity> GetProjectsOverviewByType(string type, int? year, int? companyId, int? departmentId, int? employeeId, string month = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT  P.[Id],P.[ProjectName],[Id_State],P.ProjectStatusName,P.[Type],P.[ProjectBudget]
                                 FROM [__FNC_BudgetProject] P 
								 WHERE P.[Type]=@type";

				if(year.HasValue)
					query += $" AND YEAR(P.CreationDate)={year}";
				if(companyId.HasValue)
					query += $" AND P.CompanyId={companyId}";
				if(departmentId.HasValue)
					query += $" AND P.DepartmentId={departmentId}";
				if(employeeId.HasValue)
					query += $" AND P.CreationUserId={employeeId}";
				if(!string.IsNullOrEmpty(value: month) && !string.IsNullOrWhiteSpace(month))
					query += $" AND DATENAME(MONTH, CreationDate)='{month}'";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("type", type);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.ProjectsOverviewEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<ProjectsOverviewEntity> GetProjectsOverviewAll(int? year, int? companyId, int? departmentId, int? employeeId, string month = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"SELECT  P.[Id],P.[ProjectName],[Id_State],P.ProjectStatusName,P.[Type],P.[ProjectBudget]
                                 FROM [__FNC_BudgetProject] P";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} YEAR(P.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} P.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} P.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} P.CreationUserId={employeeId}";
					isFirstCondition = false;
				}
				if(!string.IsNullOrEmpty(value: month) && !string.IsNullOrWhiteSpace(month))
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} DATENAME(MONTH, CreationDate)='{month}'";
					isFirstCondition = false;
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.ProjectsOverviewEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<OrdersOverviewEntity> GetOrdersOverview(string text, int filter, int? status, int? year, int? companyId, int? departmentId, int? employeeId, bool placed = false, string month = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				var isFirstCondition = true;
				var filterQuery = "";
				switch(filter)
				{
					case 1:
						filterQuery = $"{(isFirstCondition ? "WHERE" : "AND")} B.[OrderType]='{text}'";
						isFirstCondition = false;
						break;
					case 2:
						filterQuery = $"{(isFirstCondition ? "WHERE" : "AND")} B.[PoPaymentTypeName]='{text}'";
						isFirstCondition = false;
						break;
					case 3:
						filterQuery = $"{(isFirstCondition ? "WHERE" : "AND")} B.[Status]={status}";
						isFirstCondition = false;
						break;
					case 4:
						filterQuery = placed ? $"{(isFirstCondition ? "WHERE" : "AND")} [OrderPlacedTime] IS NOT NULL" : $"{(isFirstCondition ? "WHERE" : "AND")} BS.Typ='Wareneingang'";
						isFirstCondition = false;
						break;
					default:
						break;
				}
				string query = $@"SELECT Id,[OrderNumber],SupplierName,Amount-ISNULL(Discount,0) as Amount,PoPaymentTypeName,OrderType,Status FROM
								(
								SELECT B.[OrderId] AS Id,B.[OrderNumber],B.SupplierName,
								CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
							 	END
							   AS Amount,B.PoPaymentTypeName,B.OrderType,B.Status,B.Discount FROM
                               [__FNC_BestellungenExtension] B INNER JOIN [__FNC_BestellteArtikelExtension] A
                               ON B.[OrderId]=A.[OrderId]
                               {(filter == 4 && !placed ? "INNER JOIN Bestellungen BS ON B.OrderId=BS.[Bestellung-Nr]" : "")}
							   {filterQuery}";

				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} YEAR(B.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} B.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} B.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} B.IssuerId={employeeId}";
					isFirstCondition = false;
				}
				if(!string.IsNullOrEmpty(value: month) && !string.IsNullOrWhiteSpace(month))
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} DATENAME(MONTH, CreationDate)='{month}'";
					isFirstCondition = false;
				}
				query += $"{(filter != 3 ? $"{(isFirstCondition ? "WHERE" : "AND")} B.Status=4" : "")}";
				query += " GROUP BY B.[OrderId],B.[OrderNumber],B.SupplierName,B.PoPaymentTypeName,B.OrderType,B.Status,B.Discount,B.LeasingNbMonths,B.LeasingMonthAmount ) X";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Statistics.OrdersOverviewEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static List<int> SupplementForBestSuppliers(int? year, int? companyId, int? departmentId, int? employeeId, string supplier)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				string query = @"select distinct Id from (
                                select A.*,DATEDIFF(DAY,A.DeliveryWishDate,A.DeliveryActualDate) as Diff from 
								(
								select X.*,Datum as DeliveryActualDate from Bestellungen b inner join 
								(
								select bae.OrderId as Id,be.OrderNumber,be.SupplierName as Supplier,MIN(COALESCE(ba.Bestätigter_Termin,ba.Liefertermin)) as DeliveryWishDate
								from __FNC_BestellteArtikelExtension bae 
								inner join __FNC_BestellungenExtension be on be.OrderId=bae.OrderId
								inner join [bestellte Artikel] ba on be.OrderId=ba.[Bestellung-Nr]";

				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} YEAR(CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? " WHERE" : " AND")} IssuerId={employeeId}";
					isFirstCondition = false;
				}
				query += $@" group by bae.OrderId,be.OrderNumber,be.SupplierName
								) X
								on b.[Bestellung-Nr]=x.Id
								where b.[Typ]='Wareneingang'
								) A where DATEDIFF(DAY,A.DeliveryWishDate,A.DeliveryActualDate)>0
								) Z
								where Supplier='{supplier}'";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(selector: x =>
				Convert.ToInt32(x["Id"])).ToList();
			}
			else
			{
				return null;
			}
		}
		public static decimal SupplementForMonthlyOrders(int? year, int? companyId, int? departmentId, int? employeeId, int filter, string type, string poPayement, int month)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = @$"SELECT 
								CASE 
									WHEN B.PoPaymentTypeName = 'Purchase' THEN SUM(A.[TotalCostDefaultCurrency])-B.Discount
									WHEN B.PoPaymentTypeName = 'Leasing' THEN B.LeasingNbMonths*B.LeasingMonthAmount
									ELSE 0
								END
							   AS Amount
                               FROM [__FNC_BestellungenExtension] B
                               INNER JOIN [__FNC_BestellteArtikelExtension] A
                               ON B.[OrderId]=A.OrderId";
				var isFirstCondition = true;
				if(year.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} YEAR(B.CreationDate)={year}";
					isFirstCondition = false;
				}
				if(companyId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} B.CompanyId={companyId}";
					isFirstCondition = false;
				}
				if(departmentId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} B.DepartmentId={departmentId}";
					isFirstCondition = false;
				}
				if(employeeId.HasValue)
				{
					query += $" {(isFirstCondition ? "WHERE" : "AND")} B.IssuerId={employeeId}";
					isFirstCondition = false;
				}
				query += $" {(isFirstCondition ? "WHERE" : "AND")} {(filter == 1 ? $"B.OrderType='{type}'" : $"B.PoPaymentTypeName='{poPayement}'")} AND Status=4 AND MONTH(B.CreationDate)={month}";
				query += " GROUP BY B.Discount,B.PoPaymentTypeName,B.LeasingNbMonths,B.LeasingMonthAmount";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows[0][0] == System.DBNull.Value
					? 0
					: Convert.ToDecimal(dataTable.Rows[0][0]);
			}
			else
			{
				return 0;
			}
		}
	}
}