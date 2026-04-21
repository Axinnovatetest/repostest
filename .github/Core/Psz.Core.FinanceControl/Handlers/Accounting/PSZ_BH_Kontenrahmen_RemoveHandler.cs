using System;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class PSZ_BH_Kontenrahmen_RemoveHandler: IHandle<UserModel, ResponseModel<int>>
{

	private int _data { get; set; }
	private UserModel _user { get; set; }
	public PSZ_BH_Kontenrahmen_RemoveHandler(UserModel user, int data)
	{
		this._user = user;
		this._data = data;
	}
	public ResponseModel<int> Handle()
	{
		try
		{
			var validation = Validate();
			if(!validation.Success)
			{
				return validation;
			}
			return Perform(this._user, this._data);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	private ResponseModel<int> Perform(UserModel user, int data)
	{
		lock(Locks.OrderLock)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

			int response = -1;
			try
			{
				botransaction.beginTransaction();
				var creditDbAfterInsert = Infrastructure.Data.Access.Tables.FNC.PSZ_BH_KontenrahmenAccess.DeleteWithTransaction(data, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{
					//Logging
					var logs = new Psz.Core.FinanceControl.Helpers.LogHelper("FNC", Helpers.LogHelper.LogType.REMOVEOBJECT, _user);

					Infrastructure.Data.Access.Tables.FNC.PSZ_BH_Kontenrahmen_FNC_LogAccess.Insert(logs.LogFNC(null, null, null, data));

					return new ResponseModel<int> { Body = response, Success = true, Errors = null };
				}
				else
				{
					botransaction.rollback();
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Transaction error");
				}

			} catch(Exception ex)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}
	}
	public ResponseModel<int> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<int>.AccessDeniedResponse();
		}
		return ResponseModel<int>.SuccessResponse();
	}
}

