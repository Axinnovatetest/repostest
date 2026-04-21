using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins.FNC.Budget
{
	public class StatisticsAccess
	{
		public static List<Tuple<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity, decimal>> GetTopByAmount(bool cheapest = false, int? count = null, int? year = null)
		{
			DataTable dataTable = new DataTable();
			count = count ?? 5;
			using(SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{

				sqlConnection.Open();
				string Query = $"SELECT TOP {count} o.*, p.OrderAmount FROM [__FNC_BestellungenExtension] o " +
					$"JOIN (SELECT OrderId, SUM(ISNULL(TotalCostDefaultCurrency,0)) OrderAmount FROM [__FNC_BestellteArtikelExtension] GROUP BY OrderId) p on p.OrderId=o.OrderId " +
					$"WHERE o.[PoPaymentType]=0 AND o.[ValidationRequestTime] IS NOT NULL{(year.HasValue ? $" AND YEAR(o.CreationDate)={year.Value}" : "")} ORDER BY p.OrderAmount{(cheapest ? " ASC" : " DESC")};";

				SqlCommand command = new SqlCommand(Query, sqlConnection);

				new SqlDataAdapter(command).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Tuple<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity, decimal>(
					new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x),
					decimal.TryParse(x["OrderAmount"].ToString(), out var amount) ? amount
					: 0)).ToList();
			}
			else
			{
				return new List<Tuple<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity, decimal>>();
			}


		}

		// Top 5 desc Order Associating to project
		//public static List<Tuple<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity, decimal>> GetTotalOrderAmountAssociationWithProject(bool cheapest = false, int? count = null, int? year = null)
		//{
		//	DataTable dataTable = new DataTable();
		//	count = count ?? 5;
		//	using(SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
		//	{

		//		sqlConnection.Open();
		//		string Query = $"SELECT TOP {count} o.*, p.OrderAmount FROM [__FNC_BestellungenExtension] o " +
		//			$"JOIN (SELECT OrderId, SUM(ISNULL(TotalCostDefaultCurrency,0)) OrderAmount FROM [__FNC_BestellteArtikelExtension] GROUP BY OrderId) p on p.OrderId=o.OrderId " +
		//			$"WHERE o.[PoPaymentType]=0 AND o.[ValidationRequestTime] IS NOT NULL{(year.HasValue ? $" AND YEAR(o.CreationDate)={year.Value}" : "")} ORDER BY p.OrderAmount{(cheapest ? " ASC" : " DESC")};";

		//		SqlCommand command = new SqlCommand(Query, sqlConnection);

		//		new SqlDataAdapter(command).Fill(dataTable);
		//	}

		//	if(dataTable.Rows.Count > 0)
		//	{
		//		return dataTable.Rows.Cast<DataRow>().Select(x => new Tuple<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity, decimal>(
		//			new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x),
		//			decimal.TryParse(x["OrderAmount"].ToString(), out var amount) ? amount
		//			: 0)).ToList();
		//	}
		//	else
		//	{
		//		return new List<Tuple<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity, decimal>>();
		//	}


		//}
		#region >>>>>> Project Allocation Vs - Exppense 
		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderValidatedByProjectEntity> GetAllProjectWhoseOrderAlreadyValidated(int? year)  // Orders Already Validated 
		{
			DataTable dataTable = new DataTable();

			if(!year.HasValue)
			{
				year = DateTime.Now.Year;
			}

			using(SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = @"
									SELECT 
										Best.OrderId,  
										proj.Id as ProjectId,
										Best.LeasingTotalAmount, 
										Best.PoPaymentType,
										Best.Discount,
										proj.CurrencyName,
										proj.OrderCount,
										proj.ProjectBudget, 
										proj.ProjectName 
									FROM 
										__FNC_BudgetProject AS proj 
									JOIN 
										__FNC_BestellungenExtension AS Best 
									ON 
										proj.Id = Best.ProjectId
									WHERE 
										Best.ApprovalTime IS NOT NULL 
										AND Best.Deleted = 0 -- Assuming Best.Deleted is a boolean, use 0 for False
										AND Best.Archived = 0 -- Assuming Best.Archived is a boolean, use 0 for False
										AND YEAR(proj.CreationDate) = @Year
									GROUP BY 
										proj.Id,
										Best.OrderId,
										Best.LeasingTotalAmount, 
										Best.PoPaymentType,
										Best.Discount,
										proj.CurrencyName,
										proj.OrderCount,
										proj.ProjectBudget, 
										proj.ProjectName";


				SqlCommand command = new SqlCommand(query, sqlConnection);

				command.Parameters.AddWithValue("@Year", year);

				new SqlDataAdapter(command).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(row => new Infrastructure.Data.Entities.Tables.FNC.OrderValidatedByProjectEntity(row)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidatedByProjectEntity>();
			}
		}





		#endregion
	}
}
