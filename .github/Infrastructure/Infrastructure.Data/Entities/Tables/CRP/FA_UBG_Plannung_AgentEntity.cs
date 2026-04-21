using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CRP
{
	public class FA_UBG_Plannung_AgentEntity
	{
		public string Action { get; set; }
		public int? ActionId { get; set; }
		public DateTime? Date { get; set; }
		public string Error { get; set; }
		public int Id { get; set; }
		public bool? Success { get; set; }
		public string User { get; set; }

		public FA_UBG_Plannung_AgentEntity() { }

		public FA_UBG_Plannung_AgentEntity(DataRow dataRow)
		{
			Action = (dataRow["Action"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Action"]);
			ActionId = (dataRow["ActionId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ActionId"]);
			Date = (dataRow["Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Date"]);
			Error = (dataRow["Error"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Error"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Success = (dataRow["Success"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Success"]);
			User = (dataRow["User"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["User"]);
		}

		public FA_UBG_Plannung_AgentEntity ShallowClone()
		{
			return new FA_UBG_Plannung_AgentEntity
			{
				Action = Action,
				ActionId = ActionId,
				Date = Date,
				Error = Error,
				Id = Id,
				Success = Success,
				User = User
			};
		}
	}
}