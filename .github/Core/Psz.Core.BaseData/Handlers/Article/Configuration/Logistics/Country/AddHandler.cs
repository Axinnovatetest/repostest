using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Logistics.Country
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Configuration.Logistics.CountryModel _data { get; set; }
		public AddHandler(UserModel user, Models.Article.Configuration.Logistics.CountryModel data)
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

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenAccess.Insert(this._data.ToEntity()));
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

			if(string.IsNullOrWhiteSpace(this._data.Land))
				return ResponseModel<int>.FailureResponse("Land cannot be empty");

			if(Infrastructure.Data.Access.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenAccess.GetByLand(this._data.Land) != null)
				return ResponseModel<int>.FailureResponse($"Country [{this._data.Land}] exists");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
