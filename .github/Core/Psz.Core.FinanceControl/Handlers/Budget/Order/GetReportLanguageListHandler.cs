using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetReportLanguageListHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private UserModel _user { get; set; }
		public GetReportLanguageListHandler(UserModel user)
		{
			_user = user;
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//
				var enumEntites = Enum.GetValues(typeof(Infrastructure.Services.Reporting.Models.FNC.ReportLanguage)).Cast<Infrastructure.Services.Reporting.Models.FNC.ReportLanguage>().ToList();
				if(enumEntites != null && enumEntites.Count > 0)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(enumEntites
							.Select(x => new KeyValuePair<int, string>((int)x, x.GetDescription())).Distinct().ToList());
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}


			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
