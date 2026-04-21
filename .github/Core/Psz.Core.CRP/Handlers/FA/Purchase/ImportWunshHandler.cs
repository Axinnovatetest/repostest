using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class ImportWunshHandler: IHandle<Identity.Models.UserModel, ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Core.Common.Models.ImportFileModel _data { get; set; }
		public ImportWunshHandler(Identity.Models.UserModel user, Core.Common.Models.ImportFileModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var _right = this._user.Access.CustomerService.FAWerkWunshAdmin;
				Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel response = new Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel();
				if(_right)
				{
					var adminImport = Psz.Core.CRP.Handlers.FA.Purchase.WunshUpdate.ImportWunshAdmin(this._data, this._user);
					if(adminImport.Success)
						response = new Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel { AdminUpdate = adminImport.Body, Admin = true };
					else
					{
						var errors = adminImport.Errors.Select(x => x.Value).ToList();
						return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel>.FailureResponse(errors);
					}
				}
				else
				{
					var userImport = Psz.Core.CRP.Handlers.FA.Purchase.WunshUpdate.ImportWunshUser(this._data, this._user, Module.CTS.FAHorizons.H1LengthInDays);
					if(userImport.Success)
						response = new Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel { UserUpdate = userImport.Body, Admin = false };
					else
					{
						var errors = userImport.Errors.Select(x => x.Value).ToList();
						return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel>.FailureResponse(errors);
					}
				}

				return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel>.AccessDeniedResponse();
			}

			return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel>.SuccessResponse();
		}
	}
}