using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	using Psz.Core.BaseData.Models.Supplier;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetGeoLocationHandler: IHandle<UserModel, ResponseModel<GetGeoLocationModel>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetGeoLocationHandler(UserModel user, int id)
		{
			_user = user;
			_data = id;
		}

		public ResponseModel<GetGeoLocationModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return new ResponseModel<GetGeoLocationModel>
				{
					Success = true,
					Body = new GetGeoLocationModel(Infrastructure.Data.Access.Tables.PRS.AdressenGeocodingExtensionAccess.Get(this._data))
				};
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<GetGeoLocationModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<GetGeoLocationModel>.AccessDeniedResponse();
			}

			var loc = Infrastructure.Data.Access.Tables.PRS.AdressenGeocodingExtensionAccess.Get(this._data);
			if(loc == null)
			{
				return new ResponseModel<GetGeoLocationModel>
				{
					Success = false,
					Errors = new List<ResponseModel<GetGeoLocationModel>.ResponseError>
					{
						new ResponseModel<GetGeoLocationModel>.ResponseError
						{
							Key = "",
							Value = "Address not found"
						}
					}
				};
			}

			return ResponseModel<GetGeoLocationModel>.SuccessResponse();
		}
	}
}
