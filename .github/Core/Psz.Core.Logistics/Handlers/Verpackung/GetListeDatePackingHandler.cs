using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class GetListeDatePackingHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DateTime?>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetListeDatePackingHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<DateTime?>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<DateTime?>();


				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.GetListeChoose();
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(t => t.versanddatum_Auswahl).Distinct().ToList();
				;



				return ResponseModel<List<DateTime?>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<DateTime?>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<DateTime?>>.AccessDeniedResponse();
			}

			return ResponseModel<List<DateTime?>>.SuccessResponse();
		}
	}
}
