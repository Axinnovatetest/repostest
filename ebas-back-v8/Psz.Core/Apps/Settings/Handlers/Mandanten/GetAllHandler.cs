using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.Mandanten
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var ordersEntities = Infrastructure.Data.Access.Tables.STG.PSZ_MandantenAccess.Get();
				if(ordersEntities == null)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
				}

				var response = new List<KeyValuePair<int, string>>();


				foreach(var orderEntity in ordersEntities)
				{
					response.Add(new KeyValuePair<int, string>(orderEntity.ID, orderEntity.Mandant));
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
