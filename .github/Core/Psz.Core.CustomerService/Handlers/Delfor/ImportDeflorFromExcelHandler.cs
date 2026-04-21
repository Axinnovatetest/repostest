using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class ImportDeflorFromExcelHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DeliveryForcastLineItemModel>>>
	{

		private ImportFileModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public ImportDeflorFromExcelHandler(Identity.Models.UserModel user, ImportFileModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<DeliveryForcastLineItemModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var commaSeperator = _data.CommaSeperator == "true" ? true : false;
				var checkFrequency = _data.CheckFrequency == "true" ? true : false;
				var excleResult = Helpers.DelforHelper.ReadDelforFromExcel(_data.FilePath, out List<string> errors, commaSeperator, checkFrequency);
				if(errors != null && errors.Count > 0)
					return ResponseModel<List<DeliveryForcastLineItemModel>>.FailureResponse(errors);

				Helpers.DelforHelper.ValidateDelforData(excleResult, out List<string> warnings, _user);
				var response = new ResponseModel<List<DeliveryForcastLineItemModel>>
				{
					Body = excleResult,
					Errors = null,
					Infos = null,
					Success = true,
					Warnings = warnings
				};

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<DeliveryForcastLineItemModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<DeliveryForcastLineItemModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<DeliveryForcastLineItemModel>>.SuccessResponse();
		}

	}
}
