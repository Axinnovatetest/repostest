using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetBestandSperrlagerHandle: IHandle<Identity.Models.UserModel, ResponseModel<List<BestandSperrlagerListReportModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetBestandSperrlagerHandle(Identity.Models.UserModel user)
		{
			this._user = user;

		}
		public ResponseModel<List<BestandSperrlagerListReportModel>> Handle()
		{


			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetBestandSperrlagerReport();

				var response = PackingListEntity?.Select(x => new BestandSperrlagerListReportModel(x)).ToList();





				return ResponseModel<List<BestandSperrlagerListReportModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<BestandSperrlagerListReportModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<BestandSperrlagerListReportModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<BestandSperrlagerListReportModel>>.SuccessResponse();
		}
	}
}
