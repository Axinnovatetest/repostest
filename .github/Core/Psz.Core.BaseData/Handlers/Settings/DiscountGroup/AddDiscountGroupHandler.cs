using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.DiscountGroup
{
	public class AddDiscountGroupHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.DiscountGroup.DiscountGroupModel _data { get; set; }
		public AddDiscountGroupHandler(Identity.Models.UserModel user, Models.DiscountGroup.DiscountGroupModel data)
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
				var MaxId = Infrastructure.Data.Access.Tables.BSD.RabatthauptgruppenAccess.GetMaxGroupName();
				var _entity = this._data.ToEntity(MaxId + 1);
				var response = Infrastructure.Data.Access.Tables.BSD.RabatthauptgruppenAccess.Insert(_entity);
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
			var rabatthauptgruppenEntities = Infrastructure.Data.Access.Tables.BSD.RabatthauptgruppenAccess.Get();
			var check = rabatthauptgruppenEntities.Where(x => x.Beschreibung == this._data.Description);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A discount group with the same description already exsists" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
