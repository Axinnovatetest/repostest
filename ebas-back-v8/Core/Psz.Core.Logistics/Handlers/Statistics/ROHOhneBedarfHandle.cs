using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class ROHOhneBedarfHandle: IHandle<Identity.Models.UserModel, ResponseModel<List<ROHOhneBedarfModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ROHOhneBedarfSearch _data { get; set; }
		public ROHOhneBedarfHandle(Identity.Models.UserModel user, ROHOhneBedarfSearch _data)
		{
			this._user = user;
			this._data = _data;

		}
		public ResponseModel<List<ROHOhneBedarfModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetROHOhneBedarfEntity(_data.lagerOrt, _data.Days, null, null);
				var response = PackingListEntity?.Select(x => new ROHOhneBedarfModel(x)).ToList();
				return ResponseModel<List<ROHOhneBedarfModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<ROHOhneBedarfModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<ROHOhneBedarfModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<ROHOhneBedarfModel>>.SuccessResponse();
		}
	}
}
