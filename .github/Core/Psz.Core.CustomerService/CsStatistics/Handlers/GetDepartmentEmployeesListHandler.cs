using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetDepartmentEmployeesListHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DepartementEmployeeModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetDepartmentEmployeesListHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<DepartementEmployeeModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var resposne = new List<DepartementEmployeeModel>();
				var EmplyeesEntity = Infrastructure.Data.Access.Tables.CTS.AV_Abteilung_MitarbeiterAccess.Get();
				if(EmplyeesEntity != null && EmplyeesEntity.Count > 0)
					resposne = EmplyeesEntity.Select(e => new DepartementEmployeeModel(e)).ToList();

				return ResponseModel<List<DepartementEmployeeModel>>.SuccessResponse(resposne);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<DepartementEmployeeModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<DepartementEmployeeModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<DepartementEmployeeModel>>.SuccessResponse();
		}
	}
}
