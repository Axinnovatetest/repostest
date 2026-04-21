using System;

namespace Psz.Core.Apps.Settings.Handlers.Currency
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Settings.Models.Currency.GetModel _data { get; set; }
		public AddHandler(Identity.Models.UserModel user, Settings.Models.Currency.GetModel data)
		{
			_user = user;
			_data = data;
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

				var currencyEntity = this._data.ToEntity();
				currencyEntity.Stand = DateTime.Now;

				var insertedId = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Insert(currencyEntity);

				return ResponseModel<int>.SuccessResponse(insertedId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
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

			if(string.IsNullOrWhiteSpace(this._data.Name) || string.IsNullOrEmpty(this._data.Name))
				return ResponseModel<int>.FailureResponse($"Invalid value [{this._data.Name}] for currency name");

			if(string.IsNullOrWhiteSpace(this._data.Symbol) || string.IsNullOrEmpty(this._data.Symbol))
				return ResponseModel<int>.FailureResponse($"Invalid value [{this._data.Name}] for currency symbol");

			if(this._data.Rate <= 0)
				return ResponseModel<int>.FailureResponse($"Invalid value [{this._data.Name}] for rate symbol");

			var currName = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.GetByName(this._data.Name);
			if(currName != null && currName.Count > 0)
				return ResponseModel<int>.FailureResponse($"Currency name already exists");

			var currSymbol = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.GetBySymbol(this._data.Symbol);
			if(currSymbol != null && currSymbol.Count > 0)
				return ResponseModel<int>.FailureResponse($"Currency symbol already exists");


			return ResponseModel<int>.SuccessResponse();
		}
	}

}
