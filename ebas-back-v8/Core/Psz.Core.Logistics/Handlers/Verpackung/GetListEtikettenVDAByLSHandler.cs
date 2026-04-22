using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Verpackung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class GetListEtikettenVDAByLSHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<VDAModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public long _ls { get; set; }


		public GetListEtikettenVDAByLSHandler(long ls, Identity.Models.UserModel user)
		{

			this._ls = ls;
			this._user = user;
		}
		public ResponseModel<List<VDAModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<VDAModel>();

				var VDAListEntity = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.GetListeVDAByLS(this._ls);
				if(VDAListEntity != null && VDAListEntity.Count > 0)
				{
					foreach(var item in VDAListEntity)
					{
						response.Add(new VDAModel((item)));
					}

				}


				return ResponseModel<List<VDAModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<VDAModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<VDAModel>>.AccessDeniedResponse();
			}
			if(Infrastructure.Data.Access.Tables.Logistics.LSAccess.GetLS(this._ls) == null)
				return ResponseModel<List<VDAModel>>.FailureResponse("LS not found");

			return ResponseModel<List<VDAModel>>.SuccessResponse();
		}
	}
}

