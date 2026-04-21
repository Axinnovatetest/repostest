using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class UpdateRechnungReportingHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private RechnungReportParametersModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateRechnungReportingHandler(Identity.Models.UserModel user, RechnungReportParametersModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var entity = _data.ToEntity();
				var response = Infrastructure.Data.Access.Tables.CTS.RechnungReportingAccess.Update(entity);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
