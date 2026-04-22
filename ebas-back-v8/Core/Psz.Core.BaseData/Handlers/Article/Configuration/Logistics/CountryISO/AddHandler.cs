using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Logistics.CountryISO
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Configuration.Logistics.CountryISOModel _data { get; set; }
		public AddHandler(UserModel user, Models.Article.Configuration.Logistics.CountryISOModel data)
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

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.Insert(this._data.ToEntity()));
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

			if(string.IsNullOrWhiteSpace(this._data.Name))
				return ResponseModel<int>.FailureResponse("Name cannot be empty");

			if(string.IsNullOrWhiteSpace(this._data.NativeName))
				return ResponseModel<int>.FailureResponse("NativeName cannot be empty");

			if(string.IsNullOrWhiteSpace(this._data.alpha2Code))
				return ResponseModel<int>.FailureResponse("Alpha2Code cannot be empty");

			if(string.IsNullOrWhiteSpace(this._data.alpha3Code))
				return ResponseModel<int>.FailureResponse("Alpha3Code cannot be empty");

			if(Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.CheckName(this._data.Name) != null)
				return ResponseModel<int>.FailureResponse("Country Name already exists.");

			if(Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.CheckNativeName(this._data.NativeName) != null)
				return ResponseModel<int>.FailureResponse("Country NativeName already exists.");

			if(Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.CheckAlpha2Code(this._data.alpha2Code) != null)
				return ResponseModel<int>.FailureResponse("Country Alpha2Code already exists.");

			if(Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.CheckAlpha3Code(this._data.alpha3Code) != null)
				return ResponseModel<int>.FailureResponse("Country Alpha3Code already exists.");

			if(Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.CheckNumericCode(this._data.NumericCode) != null)
				return ResponseModel<int>.FailureResponse("Country NumericCode already exists.");


			return ResponseModel<int>.SuccessResponse();
		}
	}
}
