using System;

namespace Psz.Core.FinanceControl.Models.Budget.LogState
{
	public class LogStateModel
	{
		public DateTime? Action_date { get; set; }
		public int Id_LS { get; set; }
		public int Id_proj { get; set; }
		public int Id_state { get; set; }
		public int Id_user { get; set; }


		public LogStateModel() { }
		public LogStateModel(Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity LogStateProjectEntity)
		{

			Action_date = LogStateProjectEntity.Action_date;
			Id_LS = LogStateProjectEntity.Id_LS;
			Id_proj = LogStateProjectEntity.Id_proj;
			Id_state = LogStateProjectEntity.Id_state;
			Id_user = LogStateProjectEntity.Id_user;

		}

		public Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity ToLog_State_BudgetEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity
			{
				Action_date = Action_date,
				Id_LS = Id_LS,
				Id_proj = Id_proj,
				Id_state = Id_state,
				Id_user = Id_user,
			};
		}
	}
}
