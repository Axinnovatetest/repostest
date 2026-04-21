using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class State_BudgetEntity
	{
		public int IdS { get; set; }
		public string State { get; set; }

		public State_BudgetEntity() { }

		public State_BudgetEntity(DataRow dataRow)
		{
			IdS = Convert.ToInt32(dataRow["IdS"]);
			State = Convert.ToString(dataRow["State"]);
		}
	}
}

