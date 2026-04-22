using Psz.Core.FinanceControl.Enums;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class SmallestCustomersLowestPSZOfferHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<BiggestOfferResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public SmallestCustomersLowestPSZOfferHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}

		public ResponseModel<List<BiggestOfferResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var projectEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();

				var biggestProfitEntities = projectEntitiesAll.Where(x => x.Id_Type == (int)BudgetEnums.ProjectTypes.External && x.PSZOffer is not null).ToList();

				//10 - Smallest customers: lowest PSZOffer in external projects

				var lowestPSZOffer = biggestProfitEntities.OrderBy(x => x.PSZOffer).Take(5).ToList();

				//--transformed data

				var tansformedLowestPSZOffer = lowestPSZOffer.Select(x => new BiggestOfferResponseModel
				{
					ProjectId = x.Id,

					ProjectName = x.ProjectName,

					CustomerName = x.CustomerName,

					CustomerNr = x.CustomerNr,

					PSZOffer = x.PSZOffer,

					FormattedPSZOffer = ((decimal)x.PSZOffer / 1000).ToString("#,##0.##") + "K",

					ProjectBudget = x.ProjectBudget,

					FormattedProjectBudget = (x.ProjectBudget / 1000).ToString("#,##0.##") + "K",
					CurrencyName = x.CurrencyName,


				}).ToList();


				return ResponseModel<List<BiggestOfferResponseModel>>.SuccessResponse(tansformedLowestPSZOffer);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<BiggestOfferResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<BiggestOfferResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<BiggestOfferResponseModel>>.SuccessResponse();
		}
	}
}
