using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Administration.AccessProfiles;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Handlers.Administration.AccessProfiles
{
	public class UpdateProfileHorizonsHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private Horizonsmodel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateProfileHorizonsHandler(Identity.Models.UserModel user, Horizonsmodel data)
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

				var profile = Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.Get(_data.Id);

				profile.FACreateHorizon1 = _data.FACreateHorizon1;
				profile.FACreateHorizon2 = _data.FACreateHorizon2;
				profile.FACreateHorizon3 = _data.FACreateHorizon3;

				profile.FAUpdateTerminHorizon1 = _data.FAUpdateTerminHorizon1;
				profile.FAUpdateTerminHorizon2 = _data.FAUpdateTerminHorizon2;
				profile.FAUpdateTerminHorizon3 = _data.FAUpdateTerminHorizon3;

				profile.FACancelHorizon1 = _data.FACancelHorizon1;
				profile.FACancelHorizon2 = _data.FACancelHorizon2;
				profile.FACancelHorizon3 = _data.FACancelHorizon3;

				profile.ABPosHorizon1 = _data.ABPosHorizon1;
				profile.ABPosHorizon2 = _data.ABPosHorizon2;
				profile.ABPosHorizon3 = _data.ABPosHorizon3;

				profile.GSPosHorizon1 = _data.GSPosHorizon1;
				profile.GSPosHorizon2 = _data.GSPosHorizon2;
				profile.GSPosHorizon3 = _data.GSPosHorizon3;

				profile.LSPosHorizon1 = _data.LSPosHorizon1;
				profile.LSPosHorizon2 = _data.LSPosHorizon2;
				profile.LSPosHorizon3 = _data.LSPosHorizon3;

				profile.RAPosHorizon1 = _data.RAPosHorizon1;
				profile.RAPosHorizon2 = _data.RAPosHorizon2;
				profile.RAPosHorizon3 = _data.RAPosHorizon3;

				profile.RGPosHorizon1 = _data.RGPosHorizon1;
				profile.RGPosHorizon2 = _data.RGPosHorizon2;
				profile.RGPosHorizon3 = _data.RGPosHorizon3;

				profile.DLFPosHorizon1 = _data.DLFPosHorizon1;
				profile.DLFPosHorizon2 = _data.DLFPosHorizon2;
				profile.DLFPosHorizon3 = _data.DLFPosHorizon3;

				profile.FRCPosHorizon1 = _data.FRCPosHorizon1;
				profile.FRCPosHorizon2 = _data.FRCPosHorizon2;
				profile.FRCPosHorizon3 = _data.FRCPosHorizon3;

				var result = Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.Update(profile);

				return ResponseModel<int>.SuccessResponse(result);
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
