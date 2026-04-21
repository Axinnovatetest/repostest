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
	public class GetProjectTypeByMonthV2Handler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<ProjectTypeByMonthResponseModel>>>
	{
		public Psz.Core.Identity.Models.UserModel _user { get; set; }
		public int? _data { get; set; }

		public GetProjectTypeByMonthV2Handler(UserModel user, int? year)
		{
			this._user = user;
			this._data = year;
		}

		public ResponseModel<List<ProjectTypeByMonthResponseModel>> Handle()
		{
			try
			{
				var ValidationResponse = this.Validate();
				if(!ValidationResponse.Success)
				{
					return ValidationResponse;
				}

				var projectTypeByMonthsEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByMonth(_data);

				var groupedByProjectType = projectTypeByMonthsEntity
					.GroupBy(x => x.ProjectTypeName)
					.Select(group => new ProjectTypeByMonthResponseModel(group.ToList()))
					.ToList();

				return ResponseModel<List<ProjectTypeByMonthResponseModel>>.SuccessResponse(groupedByProjectType);

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<List<ProjectTypeByMonthResponseModel>> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<List<ProjectTypeByMonthResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<ProjectTypeByMonthResponseModel>>.SuccessResponse();
		}
	}
}


