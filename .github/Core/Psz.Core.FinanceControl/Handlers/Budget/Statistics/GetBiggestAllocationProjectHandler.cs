using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetBiggestAllocationProjectHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<BiggestAllocationResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetBiggestAllocationProjectHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<BiggestAllocationResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// 1 --Biggest Allocation

				// get project  
				var projectEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();

				//filtered project by Biggest Allocation

				var filteredBiggestAllocationProject = projectEntitiesAll?.OrderByDescending(x => x.ProjectBudget);

				// take five top biggest budget 
				var topFiveBiggestAllocation = filteredBiggestAllocationProject?.Take(5).ToList(); //--

				var topFiveBiggestAllocation_1 = topFiveBiggestAllocation.Select(x => new BiggestAllocationResponseModel
				{
					ProjectId = x.Id,

					ProjectName = x.ProjectName,

					BudgetAllocatedForProject = x.ProjectBudget,

					FormattedBudgetAllocatedForProject = (x.ProjectBudget / 1000).ToString("#,##0.##") + "K",

					CurrencyName = x.CurrencyName,

				}).ToList();


				///

				return ResponseModel<List<BiggestAllocationResponseModel>>.SuccessResponse(topFiveBiggestAllocation_1);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<BiggestAllocationResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<BiggestAllocationResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<BiggestAllocationResponseModel>>.SuccessResponse();
		}
	}
}
