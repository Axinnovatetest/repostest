using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class DeleteDelforItemPlanHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteDelforItemPlanHandler(Identity.Models.UserModel user, int data)
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

				var lineItemPlan = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Get(_data);
				var response = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Delete(_data);

				//logging
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
				{
					AngebotNr = null,
					DateTime = DateTime.Now,
					LogObject = "DeliveryForcat",
					LogText = $"Line item plan Num {lineItemPlan.PositionNumber} deleted",
					LogType = "DELETE_DLF_POSITION",
					Nr = _data,
					Origin = "CTS",
					ProjektNr = null,
					UserId = _user.Id,
					Username = _user.Name
				});

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
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
}
