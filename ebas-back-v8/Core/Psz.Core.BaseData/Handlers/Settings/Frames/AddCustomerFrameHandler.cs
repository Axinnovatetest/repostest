using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.Frames
{
	public class AddCustomerFrameHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Frames.FramesModel _data { get; set; }

		public AddCustomerFrameHandler(Identity.Models.UserModel user, Models.Frames.FramesModel data)
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
				var _entity = this._data.ToEntity();
				var responseBody = Infrastructure.Data.Access.Tables.BSD.Fibu_kunden_rahmenAccess.Insert(_entity);

				return ResponseModel<int>.SuccessResponse(responseBody);
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
			if(string.IsNullOrEmpty(this._data.Frame))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Frame Name should not be empty" }
						}
				};
			}
			var customerFramesEntities = Infrastructure.Data.Access.Tables.BSD.Fibu_kunden_rahmenAccess.Get();
			var check = customerFramesEntities.Where(x => x.Rahmen == this._data.Frame);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A customer Frame with the same name exsists" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
