using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	public class GetLevelsHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private readonly UserModel _user;

		public GetLevelsHandler(UserModel user)
		{
			_user = user;
		}
		public ResponseModel<List<KeyValuePair<string, string>>> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var response = Enum.GetValues(typeof(Enums.SupplierEnums.Levels))
				.Cast<Enums.SupplierEnums.Levels>()
				.Select(e => new KeyValuePair<string, string>(
					e.ToString(),
					e.ToString()
				)).ToList();

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(_user == null)
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
	}
}
