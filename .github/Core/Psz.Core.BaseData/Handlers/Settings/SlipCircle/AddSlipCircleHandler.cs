using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.SlipCircle
{
	public class AddSlipCircleHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.SlipCircle.SlipCircleModel _data { get; set; }
		public AddSlipCircleHandler(Identity.Models.UserModel user, Models.SlipCircle.SlipCircleModel data)
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
				var MaxCircle = Infrastructure.Data.Access.Tables.BSD.BelegkreiseVorgabenAccess.GetMaxCircle();
				var _entity = this._data.ToEntity(MaxCircle + 1);
				var response = Infrastructure.Data.Access.Tables.BSD.BelegkreiseVorgabenAccess.Insert(_entity);
				return ResponseModel<int>.SuccessResponse(response);
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
			if(string.IsNullOrEmpty(this._data.Description))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Description should not be empty" }
						}
				};
			}
			var slipCirlcesEntities = Infrastructure.Data.Access.Tables.BSD.BelegkreiseVorgabenAccess.Get();
			var check = slipCirlcesEntities.Where(x => x.Bezeichnung == this._data.Description);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A Slip circle with the same description exsists" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
