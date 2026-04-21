using Geocoding.Microsoft.Json;
using Infrastructure.Data.Access.Tables.FNC;
using Infrastructure.Data.Entities.Tables.FNC;
using iText.StyledXmlParser.Jsoup.Select;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Psz.Core.FinanceControl.Enums.BudgetEnums;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{

	public class GetProjectByMonthHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<ProjectByMonthResponseModel>>>
	{
		public Psz.Core.Identity.Models.UserModel _user { get; set; }
		public int? _data { get; set; }
		public GetProjectByMonthHandler(UserModel user, int? year)
		{
			_user = user;
			this._data = year;
		}

		public ResponseModel<List<ProjectByMonthResponseModel>> Handle()
		{
			try
			{
				var ValidationResponse = this.Validate();
				if(!ValidationResponse.Success)
				{
					return ValidationResponse;
				}
				var projectByMonth = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectPerMonth(_data);

				return ResponseModel<List<ProjectByMonthResponseModel>>.SuccessResponse(
					projectByMonth.Select(x => new ProjectByMonthResponseModel(x)).ToList());

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<List<ProjectByMonthResponseModel>> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<List<ProjectByMonthResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<ProjectByMonthResponseModel>>.SuccessResponse();
		}
	}
}
