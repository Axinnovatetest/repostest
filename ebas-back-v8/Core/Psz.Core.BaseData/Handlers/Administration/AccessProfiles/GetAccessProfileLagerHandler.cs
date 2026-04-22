
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Administration.AccessProfiles
{
	public class GetAccessProfileLagerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private List<int> _data { get; set; }

		public GetAccessProfileLagerHandler(Identity.Models.UserModel user, List<int> ids)
		{
			this._user = user;
			this._data = ids;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				/// 
				var bsdLagerEntity = Enum.GetValues(typeof(Identity.Enums.BaseDataEmuns.BaseDataLager)).Cast<Identity.Enums.BaseDataEmuns.BaseDataLager>().ToList();
				var Lagers = bsdLagerEntity
							.Select(x => new KeyValuePair<int, string>((int)x, $"{x.GetDescription()}".Trim())).Distinct().ToList();

				var profileLagerEntity = Infrastructure.Data.Access.Tables.BSD.BSD_AccessProfileLagerAccess.GetByAccessProfileIds(this._data)
						?.DistinctBy(x => x.LagerId)
						?.Select(x => new KeyValuePair<int, string>((int)x.LagerId, Lagers.FirstOrDefault(y => y.Key == x.LagerId).Value))
						?.ToList();


				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(profileLagerEntity);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
