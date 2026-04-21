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
	// -- To do 11/10/2023  
	public class GetNumberOfOrdersPerProjectTypeHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, List<ProjectTypeOrderCountEntity>>>>>
	{
		public Psz.Core.Identity.Models.UserModel _user { get; set; }
		public int? _data { get; set; }
		public GetNumberOfOrdersPerProjectTypeHandler(UserModel user, int? year)
		{
			_user = user;
			this._data = year;
		}

		public ResponseModel<List<KeyValuePair<string, List<ProjectTypeOrderCountEntity>>>> Handle()
		{
			try
			{
				var ValidationResponse = this.Validate();
				if(!ValidationResponse.Success)
				{
					return ValidationResponse;
				}

				var project_Type = new List<KeyValuePair<int, string>>();

				var getProjectType = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectTypes)).Cast<Enums.BudgetEnums.ProjectTypes>().ToList();

				project_Type.AddRange(getProjectType.Select(enumOption => new KeyValuePair<int, string>((int)enumOption, enumOption.GetDescription())));

				var enumkeysOfProjectType = project_Type.Select(x => x.Key).ToList();
				var projectTypeByMonthsEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByMonth(_data);
				var countOrderPerProjectType = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.CountOrderByProjectType(enumkeysOfProjectType, _data);

				// Filter my projects by type.  07-02-2024
				//-- apply principe : divide and conquer
				var internalProjects = countOrderPerProjectType.Where(x => x.ProjectTypeName == "Internal"); // FIXME 07-02-2024 : Don't use a hard code  review after
				var externalProject = countOrderPerProjectType.Where(x => x.ProjectTypeName == "External"); // FIXME 07-02-2024 : Don't use a hard code  review after

				var projectTypeDescription = project_Type.Select(x => x.Value).ToList(); // FIXME 07-02-2024 ==== SOLVED

				var countOrderPerProjectType2 = new List<KeyValuePair<string, List<ProjectTypeOrderCountEntity>>>();
				foreach(string description in projectTypeDescription)
				{
					var filteredProject = countOrderPerProjectType.Where(x => x.ProjectTypeName == description).ToList();

					var kvp = new KeyValuePair<string, List<ProjectTypeOrderCountEntity>>(description, filteredProject);

					countOrderPerProjectType2.Add(kvp); // i will get a tab of list

				}


				return ResponseModel<List<KeyValuePair<string, List<ProjectTypeOrderCountEntity>>>>.SuccessResponse(countOrderPerProjectType2);

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<string, List<ProjectTypeOrderCountEntity>>>> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<ProjectByMonthAllViewResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<string, List<ProjectTypeOrderCountEntity>>>>.SuccessResponse();
		}
	}
}
