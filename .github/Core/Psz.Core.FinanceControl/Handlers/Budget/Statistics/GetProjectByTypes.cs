using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetProjectByTypesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Project.TypeResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		//private int _data { get; set; }
		public GetProjectByTypesHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}

		public ResponseModel<List<TypeResponseModel>> Handle()
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
				if(projectEntities == null)
				{
					return ResponseModel<List<Models.Budget.Project.TypeResponseModel>>.SuccessResponse();
				}

				var response = new List<Models.Budget.Project.TypeResponseModel>();

				foreach(var projectEntity in projectEntities)
				{
					response.Add(new Models.Budget.Project.TypeResponseModel(projectEntity));
				}

				return ResponseModel<List<Models.Budget.Project.TypeResponseModel>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<TypeResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Project.TypeResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Project.TypeResponseModel>>.SuccessResponse();
		}
	}
}
