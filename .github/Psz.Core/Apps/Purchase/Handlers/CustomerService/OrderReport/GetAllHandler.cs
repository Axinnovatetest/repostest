using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.CustomerService
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.CustomerService.OrderReport.CreateModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetAllHandler(Identity.Models.UserModel user)
		{
			_user = user;
		}
		public ResponseModel<List<Models.CustomerService.OrderReport.CreateModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.CustomerService.OrderReport.CreateModel>();

				var orderReportEntities = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.Get();
				foreach(var orderReportEntity in orderReportEntities)
				{
					responseBody.Add(new Models.CustomerService.OrderReport.CreateModel(orderReportEntity));
				}

				return ResponseModel<List<Models.CustomerService.OrderReport.CreateModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.CustomerService.OrderReport.CreateModel>> Validate()
		{
			return ResponseModel<List<Models.CustomerService.OrderReport.CreateModel>>.SuccessResponse();
		}
	}
}
