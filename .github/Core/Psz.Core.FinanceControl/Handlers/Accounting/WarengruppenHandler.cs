using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting
{
	public class GetAllWarengruppenHandler: IHandle<UserModel, ResponseModel<List<WarengruppenModel>>>
	{

		private UserModel _user { get; set; }
		public GetAllWarengruppenHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<WarengruppenModel>> Handle()
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

		private ResponseModel<List<WarengruppenModel>> Perform(UserModel user)
		{
			try
			{
				var fetchedData = Infrastructure.Data.Access.Tables.FNC.WarengruppenAccess.Get();
				var restoreturn = fetchedData.Select(x => new WarengruppenModel(x)).ToList();

				return ResponseModel<List<WarengruppenModel>>.SuccessResponse(restoreturn);

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}
		public ResponseModel<List<WarengruppenModel>> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<List<WarengruppenModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<WarengruppenModel>>.SuccessResponse();
		}
	}
}
