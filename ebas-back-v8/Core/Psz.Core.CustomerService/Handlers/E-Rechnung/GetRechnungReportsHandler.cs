using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class GetRechnungReportsHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{

		private List<RechnungReportRequestModel> _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRechnungReportsHandler(Identity.Models.UserModel user, List<RechnungReportRequestModel> data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<byte[]> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			List<byte[]> responseBody = new List<byte[]>();
			List<string> errors = new List<string>();
			foreach(var item in _data)
			{
				var interResult = new GetRechnungReportHandler(_user, item).Handle();
				if(interResult.Success)
				{
					responseBody.Add(interResult.Body);
				}
				else
				{
					errors.Add(string.Join("|", interResult.Errors.Select(e => e.Value)));
				}
			}

			return ResponseModel<byte[]>.SuccessResponse(responseBody.Count > 0 ? Infrastructure.Services.Reporting.PdfSharp.CombinePDFs(responseBody) : null);
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			if(_data.Count > 100)
			{
				return ResponseModel<byte[]>.FailureResponse("Too many invoices, please do not select more than 100.");
			}
			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
