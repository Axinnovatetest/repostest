using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Statistics
{
	public class ProjectsOverviewByTypeEntity
	{
		public int Id { get; set; }
		public string ProjectName { get; set; }
		public string Type { get; set; }
		public decimal? ProjectBudget { get; set; }
		public ProjectsOverviewByTypeEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Id"]);
			ProjectName = (dataRow["ProjectName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectName"]);
			ProjectBudget = (dataRow["ProjectBudget"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProjectBudget"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
		}
	}
}
