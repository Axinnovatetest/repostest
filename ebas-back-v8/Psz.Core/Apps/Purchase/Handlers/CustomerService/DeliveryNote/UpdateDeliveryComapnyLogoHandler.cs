using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.CustomerService.DeliveryNote
{
	public class UpdateDeliveryComapnyLogoHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.CustomerService.OrderReport.LogoImageModel _data { get; set; }
		public UpdateDeliveryComapnyLogoHandler(Identity.Models.UserModel user, Models.CustomerService.OrderReport.LogoImageModel data)
		{
			_user = user;
			_data = data;
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

				var deliveryReportEntity = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.GetByLanguageAndType(this._data.LanguageId, this._data.OrderTypeId);
				if(deliveryReportEntity == null)
					return new ResponseModel<int>()
					{
						Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Report not found" }
						}
					};

				var newImageId = Core.Helpers.ImageFileHelper.updateImage(deliveryReportEntity.CompanyLogoImageId, this._data.LogoImage, this._data.LogoImageExtension);
				if(Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.UpdateComapnyLogo(deliveryReportEntity.Id, newImageId, this._user.Id, DateTime.Now) > 0)
				{
					return ResponseModel<int>.SuccessResponse(newImageId);
				}

				return ResponseModel<int>.SuccessResponse(-1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);

				Infrastructure.Services.Logging.Logger.Log(e, e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
