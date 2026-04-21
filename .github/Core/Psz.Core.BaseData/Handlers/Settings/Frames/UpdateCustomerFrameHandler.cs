using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.Frames
{
	public class UpdateCustomerFrameHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Frames.FramesModel _data { get; set; }

		public UpdateCustomerFrameHandler(Identity.Models.UserModel user, Models.Frames.FramesModel data)
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
				var responseBody = Infrastructure.Data.Access.Tables.BSD.Fibu_kunden_rahmenAccess.Update(_entity);

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
			var frameEntity = Infrastructure.Data.Access.Tables.BSD.Fibu_kunden_rahmenAccess.Get(this._data.ID);
			if(frameEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Customer frame not found" }
						}
				};
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
			var theRest = customerFramesEntities.Where(x => x.ID != this._data.ID);
			var check = theRest.Where(x => x.Rahmen == this._data.Frame);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A customer Frame with the same name already exsists" }
						}
				};
			}

			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByFibuFrame(this._data.ID);
			if(exsist1 != null && exsist1.Count > 0)
			{
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(exsist1.Select(x => x.Nummer ?? -1)?.Distinct().ToList());
				return ResponseModel<int>.FailureResponse(
					$"Cannot update Customer frame. The following customer(s) use this Customer frame [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{x.Kundennummer} - {x.Name1}")?.ToList())}]");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
