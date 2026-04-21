using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting
{
	public class Fibu_kunden_rahmenHandler: IHandle<UserModel, ResponseModel<List<Fibu_kunden_rahmenModel>>>
	{

		private UserModel _user { get; set; }
		public Fibu_kunden_rahmenHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Fibu_kunden_rahmenModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}
				return Perform(this._user);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<List<Fibu_kunden_rahmenModel>> Perform(UserModel user)
		{
			try
			{
				var fetchedData = Infrastructure.Data.Access.Tables.FNC.Fibu_kunden_rahmen2Access.Get();
				var restoreturn = fetchedData.Select(x => new Fibu_kunden_rahmenModel(x)).ToList();

				return ResponseModel<List<Fibu_kunden_rahmenModel>>.SuccessResponse(restoreturn);

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}
		public ResponseModel<List<Fibu_kunden_rahmenModel>> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<List<Fibu_kunden_rahmenModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<Fibu_kunden_rahmenModel>>.SuccessResponse();
		}
	}
}
