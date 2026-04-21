using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//15 -02- 2024  // it's useless

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetClosedProjectWithRemainingBudgetHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<BudgetLeakResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetClosedProjectWithRemainingBudgetHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}

		public ResponseModel<List<BudgetLeakResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var projectEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();

				var closedProjectWithRemainingBudget = projectEntitiesAll.Where(x => x.Closed == true && x.ProjectBudget > 0 && x.ProjectStatusName.ToLower() == "closed").Take(5).ToList();

				var BudgetLeak = closedProjectWithRemainingBudget.Select(x => new BudgetLeakResponseModel
				{

					ProjectId = x.Id,
					ProjectName = x.ProjectName,
					Status = x.ProjectStatusName,
					ProjectBudget = x.ProjectBudget, // display remaning budget of closed project 
					FormattedProjectBudget = (x.ProjectBudget / 1000).ToString("#,##0.##") + "K",
					CurrencyName = x.CurrencyName,

				}).ToList();

				return ResponseModel<List<BudgetLeakResponseModel>>.SuccessResponse(BudgetLeak);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<BudgetLeakResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<BudgetLeakResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<BudgetLeakResponseModel>>.SuccessResponse();
		}
	}
}
