using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//15 -02- 2024  // it's useless

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetDepartmentByCompanyId: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Project.TypeResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetDepartmentByCompanyId(Identity.Models.UserModel user)
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

				//--


				//--

				return null;

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
