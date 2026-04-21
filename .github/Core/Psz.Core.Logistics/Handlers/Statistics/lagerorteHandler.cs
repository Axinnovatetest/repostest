using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class lagerorteHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<lagerorteModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public lagerorteHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<lagerorteModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<lagerorteModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.lagerorteAcess.GetlagerorteAcess();
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new lagerorteModel(k)).ToList();

				return ResponseModel<List<lagerorteModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<List<lagerorteModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<lagerorteModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<lagerorteModel>>.SuccessResponse();
		}
	}
}
