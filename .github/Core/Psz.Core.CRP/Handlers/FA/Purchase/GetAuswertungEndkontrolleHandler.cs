using Infrastructure.Services.Reporting.Models.CTS;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetAuswertungEndkontrolleHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetAuswertungEndkontrolleHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				byte[] responseBody = null;
				var auswetungEndkontrolleEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetAuswertingEndkontrolle();
				if(auswetungEndkontrolleEntity != null && auswetungEndkontrolleEntity.Count > 0)
				{
					var ausweruntEndkontrolle = auswetungEndkontrolleEntity.Select(x => new AuswertungEndkontrolleReportModel(x)).ToList();
					responseBody = Module.CRP_ReportingService.GenerateAuswerungEndkontrolleReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_AUSWERTUNG_ENDKONTROLLE, ausweruntEndkontrolle);
				}
				return ResponseModel<byte[]>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}