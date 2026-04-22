using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class AddDepartmentEmployeeHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private DepartementEmployeeModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AddDepartmentEmployeeHandler(Identity.Models.UserModel user, DepartementEmployeeModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var Entity = _data.ToEntity();
				var resposne = Infrastructure.Data.Access.Tables.CTS.AV_Abteilung_MitarbeiterAccess.Insert(Entity);
				return ResponseModel<int>.SuccessResponse(resposne);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
