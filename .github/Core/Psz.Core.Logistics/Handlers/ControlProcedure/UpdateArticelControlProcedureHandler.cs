using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.BaseData.Handlers;
using Psz.Core.SharedKernel.Interfaces;
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Helpers;


namespace Psz.Core.Logistics.Handlers.PlantBookings;

public class UpdateArticelControlProcedureHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
{
	private Identity.Models.UserModel _user { get; set; }
	private Psz.Core.Logistics.Models.ControlProcedure.UpdateArticleControlProcedureModel _data { get; set; }


	public UpdateArticelControlProcedureHandler(Identity.Models.UserModel user, Psz.Core.Logistics.Models.ControlProcedure.UpdateArticleControlProcedureModel data)
	{
		this._user = user;
		this._data = data;
	}

	public ResponseModel<int> Handle()
	{

		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var currentData = Infrastructure.Data.Access.Tables.Logistics.PlantBookingArticleControlProcedureAccess.Get(_data.Id);

			var entity = new Infrastructure.Data.Entities.Tables.Logistics.UpdateArticelControlProcedureEntity()
			{
				Id=_data.Id,
				LastEditTime = DateTime.Now,
				LastEditUserId = this._user.Id,
				ProcedureDescription = _data.ProcedureDescription,
				ProcedureName = _data.ProcedureName,
				ControlledAverage = _data.ControlledAverage,
				ControlledFailedQuantity = _data.ControlledFailedQuantity,
				ControlledMeasuredValue = _data.ControlledMeasuredValue,
				ControlledQuantity = _data.ControlledQuantity,
				ControlledSum = _data.ControlledSum,
				ControlledTotalQuantity = _data.ControlledTotalQuantity,
				ProcedureType = _data.ProcedureType
			};

			botransaction.beginTransaction();


			
			var updateResult = Infrastructure.Data.Access.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresAccess.UpdateArticleControlProcedureTrans(entity, botransaction.connection, botransaction.transaction);
			
			//var logs = PlantBookingLogHelper.GenerateLogForUpdates(_user,currentData, _data);
			//var InsertLogsResult = Infrastructure.Data.Access.Tables.Logistics.PlantBookingsLogsAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);

			if(botransaction.commit())
			{
				return ResponseModel<int>.SuccessResponse(updateResult);
			}
			else
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
			}
		} catch(Exception e)
		{
			botransaction.rollback();
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<int> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<int>.AccessDeniedResponse();
		}

		return ResponseModel<int>.SuccessResponse();
	}
}
