using Geocoding.Microsoft.Json;
using MoreLinq;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetProjetStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Project.StatusResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		//private int _data { get; set; }
		public GetProjetStatusHandler(Identity.Models.UserModel user /*, int value*/ )
		{
			this._user = user;
			//this._data = value;
		}

		public ResponseModel<List<StatusResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var projectEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();

				if(projectEntities == null) // erreur
				{
					return ResponseModel<List<Models.Budget.Project.StatusResponseModel>>.SuccessResponse();
				}

				var response = new List<Models.Budget.Project.StatusResponseModel>();

				foreach(var projectEntity in projectEntities)
				{
					//for project Status
					response.Add(new Models.Budget.Project.StatusResponseModel(projectEntity, projectEntity.ProjectStatus ?? -1, projectEntity.ProjectStatusName));
					;

					// here it's/ this is  projectApprovalStatus 
					response.Add(new Models.Budget.Project.StatusResponseModel(projectEntity, projectEntity.Id_State, ((Enums.BudgetEnums.ProjectApprovalStatuses)projectEntity.Id_State).GetDescription()));
				}

				return ResponseModel<List<Models.Budget.Project.StatusResponseModel>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<StatusResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Project.StatusResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Project.StatusResponseModel>>.SuccessResponse();
		}
	}
}
