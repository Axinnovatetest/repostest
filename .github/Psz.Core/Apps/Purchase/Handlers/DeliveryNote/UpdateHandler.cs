using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class UpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.CustomerService.DeliveryNote.CreateDelieveryNoteModel _data { get; set; }
		public UpdateHandler(Identity.Models.UserModel user, Models.CustomerService.DeliveryNote.CreateDelieveryNoteModel data)
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

				var orderDelieveryReportEntity = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.GetByLanguageAndType(this._data.LanguageId, this._data.OrderTypeId);
				if(orderDelieveryReportEntity == null)
					return new ResponseModel<int>()
					{
						Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Report not found" }
						}
					};

				this._data.Id = orderDelieveryReportEntity.Id;
				this._data.LastUpdateTime = DateTime.Now;
				this._data.LastUpdateUserId = this._user.Id;
				var responseBody = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.Update(this._data.ToDelieveryNoteReportEntity());

				return ResponseModel<int>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);

				Infrastructure.Services.Logging.Logger.Log(e, e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
