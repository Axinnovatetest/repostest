using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class UpdateGeolocationHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public UpdateGeolocationHandler(UserModel user, int id)
		{
			_user = user;
			_data = id;
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

				var address = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data);

				var newLoc = Infrastructure.Services.Geocoding.Converter.LocationFromAddress($"{address.StraBe} {address.Postfach} {address.Ort} {(address.Land.ToLower() == "d" ? "Deutschland" : address.Land)}");
				var newLocExtension = new Infrastructure.Data.Entities.Tables.PRS.AdressenGeocodingExtensionEntity
				{
					Nr = address.Nr,
					Longitude = newLoc.Longitude,
					Latitude = newLoc.Latitude,
					Confidence = newLoc.Confidence,
					UpdateDate = DateTime.Now
				};

				if(Infrastructure.Data.Access.Tables.PRS.AdressenGeocodingExtensionAccess.Get(this._data) == null)
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
					Body = address.Nr
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

			var address = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data);
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
