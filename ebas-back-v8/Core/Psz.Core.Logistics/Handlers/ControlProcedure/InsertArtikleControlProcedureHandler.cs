using System;
using System.Collections.Generic;
using Infrastructure.Data.Entities.Tables.Logistics;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.Logistics.Handlers.ControlProcedure
{
	public class InsertArtikleControlProcedureHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{


		private Core.Identity.Models.UserModel _user;
		private CreateControlProcedureVM _data;

		public InsertArtikleControlProcedureHandler(Core.Identity.Models.UserModel user, CreateControlProcedureVM data)
		{
			_user = user;
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
					return validationResponse;
				}

				var validationRes = validateVM();
				if(validationRes.Count > 0)
				{
					return ResponseModel<int>.FailureResponse(validationRes);
				}
				var artiklecontrolprocedurevalue = new PlantBookingsArticleControlledProceduresEntity()
				{
					ArticleId = this._data.ArticleId,
					ArticleNumber = this._data.ArticleNumber,
					CreateTime = DateTime.Now,
					CreateUserId = this._user.Id,
					ProcedureDescription = this._data.ProcedureDescription,
					ProcedureName = this._data.ProcedureName,
					SupplierId = this._data.SupplierId,
					SupplierName = this._data.SupplierName,
					ControlledAverage = _data.ControlledAverage,
					ControlledFailedQuantity = _data.ControlledFailedQuantity,
					ControlledMeasuredValue = _data.ControlledMeasuredValue,
					ControlledQuantity = _data.ControlledQuantity,
					ControlledSum = _data.ControlledSum,
					ControlledTotalQuantity = _data.ControlledTotalQuantity,
					ProcedureType = this._data.ProcedureType,
				};
				botransaction.beginTransaction();
				#region Create Script
				var data = Infrastructure.Data.Access.Tables.Logistics.PlantBookingArticleControlProcedureAccess.InsertWithTransaction(artiklecontrolprocedurevalue, botransaction.connection, botransaction.transaction);
				#endregion

				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(data);
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
		private List<KeyValuePair<string, string>> validateVM()
		{
			var res = new List<KeyValuePair<string, string>>();
			
			int keyCounter = 1;

			if(this._data.ArticleId <= 0)
			{
				res.Add(new KeyValuePair<string,string>($"{keyCounter}", "Invalid Artikel"));
			}
			if(this._data.SupplierId <= 0)
			{
				res.Add(new KeyValuePair<string, string>($"{keyCounter}", "Invalid Supplier"));
			}
			if(this._data.ArticleNumber is null || string.IsNullOrEmpty(this._data.ArticleNumber) || this._data.ArticleNumber?.Length <= 3)
			{
				res.Add(new KeyValuePair<string, string>($"{keyCounter}", "Invalid Artikel"));
			}
			if(this._data.SupplierName is null || string.IsNullOrEmpty(this._data.SupplierName) || this._data.SupplierName?.Length <= 3)
			{
				res.Add(new KeyValuePair<string, string>($"{keyCounter}", "Invalid Supplier"));
			}
			if(this._data.ProcedureDescription is null || string.IsNullOrEmpty(this._data.ProcedureDescription) || this._data.ProcedureDescription?.Length <= 3)
			{
				res.Add(new KeyValuePair<string, string>($"{keyCounter}", "Invalid Procedure Description"));
			}
			if(this._data.ProcedureName is null || string.IsNullOrEmpty(this._data.ProcedureName) || this._data.ProcedureName?.Length <= 3)
			{
				res.Add(new KeyValuePair<string, string>($"{keyCounter}", "Invalid Procedure Name"));
			}
			if(this._data.ProcedureType is null || string.IsNullOrEmpty(this._data.ProcedureType) || this._data.ProcedureType?.Length <= 3)
			{
				res.Add(new KeyValuePair<string, string>($"{keyCounter}", "Invalid Procedure Type"));
			}

			return res;
		}
	}

}

