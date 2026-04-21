using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class BestandSperrlagerPDFHandle: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public BestandSperrlagerPDFHandle(Identity.Models.UserModel user)
		{
			this._user = user;

		}
		public ResponseModel<byte[]> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{

				byte[] response = null;
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetBestandSperrlagerReport();

				var details = PackingListEntity?.Select(x => new BestandSperrlagerListReportModel(x)).ToList();



				var ReportData = new BestandSperrlagerListReportDetails { Details = details };

				response = Module.Logistic_ReportingService.BestandSperrlagerReport(Enums.ReportingEnums.ReportType.BESTANDSPERRLAGER, ReportData);

				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
