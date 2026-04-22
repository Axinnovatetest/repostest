using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	public class GetSupplierHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.ObjectLog.ObjectLogModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetSupplierHistoryHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.ObjectLog.ObjectLogModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				List<Models.ObjectLog.ObjectLogModel> results = null;
				List<string> LogObjects = new List<string> {
					Enums.ObjectLogEnums.Objects.Supplier_Overview.GetDescription(),
					Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(),
					Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(),
					Enums.ObjectLogEnums.Objects.Supplier_Shipping.GetDescription(),
					Enums.ObjectLogEnums.Objects.Supplier_Communication.GetDescription(),
					Enums.ObjectLogEnums.Objects.Supplier_ContcatPerson.GetDescription(),
				};
				var logEntities = Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.GetByObjetsAndId_2(LogObjects, this._data);

				if(logEntities != null && logEntities.Count > 0)
				{
					results = new List<Models.ObjectLog.ObjectLogModel>();
					foreach(var logEntity in logEntities)
					{
						results.Add(new Models.ObjectLog.ObjectLogModel(logEntity));
					}
				}

				return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.ObjectLog.ObjectLogModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.SuccessResponse();
		}
	}
}
