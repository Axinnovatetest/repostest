using Infrastructure.Data.Entities.Tables.FNC;
using System;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class PSZ_BH_Kontenrahmen_UpdateHandler: IHandle<PSZ_BH_Kontenrahmen_CreateModel, ResponseModel<int>>
{

	private PSZ_BH_Kontenrahmen_CreateModel _data { get; set; }
	private UserModel _user { get; set; }
	public PSZ_BH_Kontenrahmen_UpdateHandler(UserModel user, PSZ_BH_Kontenrahmen_CreateModel data)
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

	private ResponseModel<int> Perform(UserModel user, PSZ_BH_Kontenrahmen_CreateModel data)
	{
		lock(Locks.OrderLock)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

			int response = -1;
			try
			{
				int IdLastItem = 0;
				var getItems = Infrastructure.Data.Access.Tables.FNC.PSZ_BH_KontenrahmenAccess.GetNexPossibleId();
				var item = Infrastructure.Data.Access.Tables.FNC.PSZ_BH_KontenrahmenAccess.GetbyId(data.ID);

				var changedItems = data.GetUnMatchingUpdateAttributes(item);

				if(getItems is not null && getItems.Count > 0)
				{
					IdLastItem = getItems[0].ID;
				}

				var creatmdl = new PSZ_BH_KontenrahmenEntity()
				{
					ID = data.ID,
					Habenkto = data.Habenkto,
					Rahmen = data.Rahmen,
					Warengruppe = data.Warengruppe
				};
				botransaction.beginTransaction();
				var creditDbAfterInsert = Infrastructure.Data.Access.Tables.FNC.PSZ_BH_KontenrahmenAccess.UpdateWithTransaction(creatmdl, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{
					//Logging
					foreach(var items in changedItems)
					{
						if(items == "Habenkto")
						{
							var logs = new Psz.Core.FinanceControl.Helpers.LogHelper("FNC", Helpers.LogHelper.LogType.UPDATEOBJECT, _user);
							Infrastructure.Data.Access.Tables.FNC.PSZ_BH_Kontenrahmen_FNC_LogAccess.Insert(logs.LogFNC(items, item.Habenkto, data.Habenkto, data.ID));
						}
						if(items == "Rahmen")
						{
							var logs = new Psz.Core.FinanceControl.Helpers.LogHelper("FNC", Helpers.LogHelper.LogType.UPDATEOBJECT, _user);
							Infrastructure.Data.Access.Tables.FNC.PSZ_BH_Kontenrahmen_FNC_LogAccess.Insert(logs.LogFNC(items, item.Rahmen, data.Rahmen, data.ID));
						}
						if(items == "Warengruppe")
						{
							var logs = new Psz.Core.FinanceControl.Helpers.LogHelper("FNC", Helpers.LogHelper.LogType.UPDATEOBJECT, _user);
							Infrastructure.Data.Access.Tables.FNC.PSZ_BH_Kontenrahmen_FNC_LogAccess.Insert(logs.LogFNC(items, item.Warengruppe, data.Warengruppe, data.ID));
						}

					}
					return new ResponseModel<int> { Body = response, Success = true, Errors = null };
				}
				else
				{
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
