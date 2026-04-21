using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Type_Project_BudgetEntity
	{
		public int IdT { get; set; }
		public string Type_Project { get; set; }

		public Type_Project_BudgetEntity() { }

		public Type_Project_BudgetEntity(DataRow dataRow)
		{
			IdT = Convert.ToInt32(dataRow["IdT"]);
			Type_Project = Convert.ToString(dataRow["Type_Project"]);
		}
	}
}

