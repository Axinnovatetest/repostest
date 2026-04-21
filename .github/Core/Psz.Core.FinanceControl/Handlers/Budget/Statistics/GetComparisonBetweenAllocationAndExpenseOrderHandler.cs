using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{

	public class GetComparisonBetweenAllocationAndExpenseOrderHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<MonthlyOrderExpenseResponseModel>>>
	{

		public Psz.Core.Identity.Models.UserModel _user { get; set; }

		public int _year { get; set; }
		public int _companyId { get; set; }

		public int _departmentId { get; set; }

		public GetComparisonBetweenAllocationAndExpenseOrderHandler(UserModel user, int year, int companyId, int departmentId)
		{
			_user = user;
			_year = year;
			_departmentId = departmentId;
			_companyId = companyId;
		}

		public ResponseModel<List<MonthlyOrderExpenseResponseModel>> Handle()
		{
			try
			{
				var ValidationResponse = this.Validate();

				if(!ValidationResponse.Success)
				{
					return ValidationResponse;
				}

				// -- Create Query at level ADO.NET API



				return null;

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<List<MonthlyOrderExpenseResponseModel>> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<List<OrderExpenseResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<MonthlyOrderExpenseResponseModel>>.SuccessResponse();
		}
	}
}


