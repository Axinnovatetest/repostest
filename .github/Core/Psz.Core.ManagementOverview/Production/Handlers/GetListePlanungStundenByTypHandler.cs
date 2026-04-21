using Infrastructure.Data.Access.Joins.MGO;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Production.Interfaces;
using Psz.Core.ManagementOverview.Production.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Handlers
{
	public partial class ProductionService: IProductionService
	{
		public ResponseModel<PackGeplantStundenByTypModel> GetListePlanungStundenByTyp(Identity.Models.UserModel user, int lager, int typeLoading)
		{
			try
			{
				var validationResponse = this.ValidationListePlanungStundenByTyp(user, lager);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				if(typeLoading == 2)
				{
					string nameAgent = "PlanungStundenByTyp";
					int? resultExecute=ProductionAccess.ExecuteAgent(nameAgent);
					Thread.Sleep(4000);
				}

				PackGeplantStundenByTypModel packGeplantStundenByTyp = new PackGeplantStundenByTypModel();

				var listPlanungStundenEntity = ProductionAccess.GetListPlanungStundenByLagerAndTyp(lager);

				var listPlanungStundenModel = new List<Models.GeplantStundenByTypModel>();
				if(listPlanungStundenEntity != null && listPlanungStundenEntity.Count > 0)
				{
				
					listPlanungStundenModel = listPlanungStundenEntity.Select(k => new Models.GeplantStundenByTypModel(k)).ToList();
					packGeplantStundenByTyp.lastUpdate = listPlanungStundenModel[0].datum;
					
				}

				packGeplantStundenByTyp.listGeplantStunden = listPlanungStundenModel;



				//---------Liste Kunde-----------------------------

				return ResponseModel<PackGeplantStundenByTypModel>.SuccessResponse(packGeplantStundenByTyp);


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<PackGeplantStundenByTypModel> ValidationListePlanungStundenByTyp(Identity.Models.UserModel user, int lager)
		{
			if(user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<PackGeplantStundenByTypModel>.AccessDeniedResponse();
			}

			return ResponseModel<PackGeplantStundenByTypModel>.SuccessResponse();
		}
	}
}
