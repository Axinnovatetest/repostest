using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetLagerIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<LagerOrt_IdModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetLagerIdHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<LagerOrt_IdModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<LagerOrt_IdModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.lagerorteAcess.GetlagerIdeAcess();
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new LagerOrt_IdModel(k)).ToList();

				return ResponseModel<List<LagerOrt_IdModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<List<LagerOrt_IdModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<LagerOrt_IdModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<LagerOrt_IdModel>>.SuccessResponse();
		}
	}
}
