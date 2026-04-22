using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetCableWithoutOrderHandle: IHandle<Identity.Models.UserModel, ResponseModel<List<CableWithoutOrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetCableWithoutOrderHandle(Identity.Models.UserModel user, int _data)
		{
			this._user = user;
			this._data = _data;

		}
		public ResponseModel<List<CableWithoutOrderModel>> Handle()
		{


			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetCableWithoutOrder(_data);

				var response = PackingListEntity?.Select(x => new CableWithoutOrderModel(x)).ToList();





				return ResponseModel<List<CableWithoutOrderModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<CableWithoutOrderModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<CableWithoutOrderModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<CableWithoutOrderModel>>.SuccessResponse();
		}
	}
}
