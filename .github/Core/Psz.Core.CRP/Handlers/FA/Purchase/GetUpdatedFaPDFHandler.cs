using Infrastructure.Services.Reporting.Models.CTS;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetUpdatedFaPDFHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private FAUpdateByArticleFinalModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetUpdatedFaPDFHandler(FAUpdateByArticleFinalModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
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
				responseBody = Module.CRP_ReportingService.GenerateFAUpdateReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FA_UPDATE, this._data);
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
