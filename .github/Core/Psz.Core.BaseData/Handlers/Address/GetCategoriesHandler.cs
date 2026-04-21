using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Address
{
	public class GetCategoriesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCategoriesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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
				var responseBody = new List<KeyValuePair<int, string>> { };
				foreach(var item in Enum.GetValues(typeof(Enums.AddressEnums.Categories)).Cast<Enums.AddressEnums.Categories>().ToList())
				{
					responseBody.Add(new KeyValuePair<int, string>((int)item, item.GetDescription()));
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(responseBody);
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
