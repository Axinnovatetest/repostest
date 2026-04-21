using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetExternalOrdersByCompanyNotFullValidatedHandler: IHandle<UserModel, ResponseModel<List<Core.FinanceControl.Models.Accounting.CompanyExtOrdersNotFullValidatedModel>>>
	{
		private readonly UserModel _user;
		private readonly int _id;

		public GetExternalOrdersByCompanyNotFullValidatedHandler(UserModel user, int id)
		{
			this._user = user;
			this._id = id;
		}
		public ResponseModel<List<CompanyExtOrdersNotFullValidatedModel>> Handle()
		{
			var validationResult = this.Validate();
			if(!validationResult.Success)
				return validationResult;
			var Entities = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetExternalOrdersNotFullyValidated(_id);
			var response = Entities?.Select(x => new CompanyExtOrdersNotFullValidatedModel(x)).ToList();
			return ResponseModel<List<Core.FinanceControl.Models.Accounting.CompanyExtOrdersNotFullValidatedModel>>.SuccessResponse(response);
		}

		public ResponseModel<List<CompanyExtOrdersNotFullValidatedModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<CompanyExtOrdersNotFullValidatedModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<CompanyExtOrdersNotFullValidatedModel>>.SuccessResponse();
		}
	}
}
