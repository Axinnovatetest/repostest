using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.Industry
{
	public class AddIndustryHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Industry.IndustryModel _data { get; set; }

		public AddIndustryHandler(Identity.Models.UserModel user, Models.Industry.IndustryModel data)
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
				_entity.CreationTime = DateTime.Now;
				_entity.CreationUserId = this._user.Id;
				var response = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Insert(_entity);
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
			if(string.IsNullOrEmpty(this._data.Name))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Name should not be empty" }
						}
				};
			}
			if(this._data.Type == -1)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Please choose a Type" }
						}
				};
			}
			var indutriesEntities = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Get();
			var check = indutriesEntities.Where(x => x.Name == this._data.Name);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "An industry with the same Name already exsists" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
