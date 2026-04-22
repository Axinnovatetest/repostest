using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Logistics.CountryISO
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public DeleteHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.Delete(this._data));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.Get(this._data) == null)
			{
				return new ResponseModel<int>
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
						new ResponseModel<int>.ResponseError
						{
							Key = "1",
							Value = "Country not found"
						}
					}
				};
			}


			return ResponseModel<int>.SuccessResponse();
		}
	}
}
