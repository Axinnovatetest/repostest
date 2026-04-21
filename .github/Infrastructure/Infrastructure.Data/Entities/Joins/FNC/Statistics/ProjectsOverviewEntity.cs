using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Statistics
{
	public class ProjectsOverviewEntity
	{
		public int Id { get; set; }
		public string ProjectName { get; set; }
		public int? StatusId { get; set; }
		public string Type { get; set; }
		public string ProjectStatus { get; set; }
		public decimal? ProjectBudget { get; set; }
		public ProjectsOverviewEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Id"]);
			ProjectName = (dataRow["ProjectName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectName"]);
			ProjectBudget = (dataRow["ProjectBudget"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProjectBudget"]);
			ProjectStatus = (dataRow["ProjectStatusName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectStatusName"]);
			StatusId = (dataRow["Id_State"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_State"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
		}
	}
}