using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class SetGeolocationHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Supplier.SetGeolocationModel _data { get; set; }
		public SetGeolocationHandler(UserModel user, Models.Supplier.SetGeolocationModel newLocation)
		{
			_user = user;
			_data = newLocation;
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

				var newLocExtension = new Infrastructure.Data.Entities.Tables.PRS.AdressenGeocodingExtensionEntity
				{
					Nr = this._data.Nr,
					Longitude = this._data.Longitude,
					Latitude = this._data.Latitude,
					Confidence = 2, // manual == max confidence
					UpdateDate = DateTime.Now
				};

				if(Infrastructure.Data.Access.Tables.PRS.AdressenGeocodingExtensionAccess.Get(this._data.Nr) == null)
				{
					Infrastructure.Data.Access.Tables.PRS.AdressenGeocodingExtensionAccess.Insert(newLocExtension);
				}
				else
				{
					Infrastructure.Data.Access.Tables.PRS.AdressenGeocodingExtensionAccess.Update(newLocExtension);
				}

				return new ResponseModel<int>
				{
					Success = true,
					Body = this._data.Nr
				};
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

			var address = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.Nr);
			if(address == null)
			{
				return new ResponseModel<int>
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
						new ResponseModel<int>.ResponseError
						{
							Key = "",
							Value = "Address not found"
						}
					}
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
