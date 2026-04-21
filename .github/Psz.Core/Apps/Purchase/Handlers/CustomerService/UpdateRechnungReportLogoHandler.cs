using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.CustomerService
{
	public class UpdateRechnungReportLogoHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private ReportLogoImportedImage _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateRechnungReportLogoHandler(Identity.Models.UserModel user, ReportLogoImportedImage data)
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

				var rechenungReportEntity = Infrastructure.Data.Access.Tables.CTS.RechnungReportingAccess.GetByLagerIdAndType(_data.LagerId, _data.Typ);
				if(rechenungReportEntity == null)
					return new ResponseModel<int>()
					{
						Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Report not found" }
						}
					};

				var newImageId = Core.Helpers.ImageFileHelper.updateImage(rechenungReportEntity.LogoId ?? 0, this._data.LogoImage, this._data.LogoImageExtension);

				if(Infrastructure.Data.Access.Tables.CTS.RechnungReportingAccess.UpdateLogoId(newImageId, rechenungReportEntity.Id, this._user.Id) > 0)
				{
					return ResponseModel<int>.SuccessResponse(newImageId);
				}

				return ResponseModel<int>.SuccessResponse(-1);

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
