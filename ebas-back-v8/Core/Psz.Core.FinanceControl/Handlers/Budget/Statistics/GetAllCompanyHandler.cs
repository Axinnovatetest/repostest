using Infrastructure.Data.Entities.Tables.FNC;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetAllCompanyHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CompanyExtensionEntity>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetAllCompanyHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}

		public ResponseModel<List<CompanyExtensionEntity>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<CompanyExtensionEntity> CompanyAll = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get();

				return ResponseModel<List<CompanyExtensionEntity>>.SuccessResponse(CompanyAll);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<CompanyExtensionEntity>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<CompanyExtensionEntity>>.AccessDeniedResponse();
			}

			return ResponseModel<List<CompanyExtensionEntity>>.SuccessResponse();
		}
	}
}
