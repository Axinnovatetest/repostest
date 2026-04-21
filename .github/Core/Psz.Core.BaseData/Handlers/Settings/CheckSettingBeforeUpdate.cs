using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings
{
	public class CheckSettingBeforeUpdate: IHandle<Identity.Models.UserModel, ResponseModel<bool>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private KeyValuePair<int, string> _data { get; set; }

		public CheckSettingBeforeUpdate(Identity.Models.UserModel user, KeyValuePair<int, string> data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<bool> Handle()
		{
			try
			{
				bool result = false;
				switch(_data.Key)
				{
					case 1:
						result = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetCustomerWithIndustry(_data.Value) | Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetSupplierWithIndustry(_data.Value);
						break;
					case 2:
						result = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetCustomerWithShippingMethod(_data.Value);
						break;
					case 3:
						result = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetAdressWithAnrede(_data.Value);
						break;
					case 4:
						result = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetAdressWithSalutation(_data.Value);
						break;

				}
				return ResponseModel<bool>.SuccessResponse(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<bool> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<bool>.AccessDeniedResponse();
			}
			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
