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
	public class GetProjectTypeByMonthHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<ProjectTypeByMonthResponseModel>>>
	{
		public Psz.Core.Identity.Models.UserModel _user { get; set; }
		public int? _data { get; set; }

		public GetProjectTypeByMonthHandler(UserModel user, int? year)
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

				////if enumeration, then several option and each option stand for KeyValuePair<key, description>; 
				var project_Type = new List<KeyValuePair<int, string>>();
				var getProjectType = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectTypes)).Cast<Enums.BudgetEnums.ProjectTypes>().ToList();
				project_Type.AddRange(getProjectType.Select(enumOption => new KeyValuePair<int, string>((int)enumOption, enumOption.GetDescription())));
				// Get Tuple list  ==> transform it into a more suitable models 
				var projectTypeByMonthsEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByMonth(_data);
				//separation of Project types
				var resultByProjectTypeDto = new List<List<ProjectTypeByMonthEntity>>();
				if(projectTypeByMonthsEntity.Count > 0)
				{
					foreach(var item in project_Type)
					{
						var filteredProjects = projectTypeByMonthsEntity.Where(x => x.ProjectTypeName == item.Value).ToList();

						resultByProjectTypeDto.Add(filteredProjects);
					}
				}
				// - 
				return ResponseModel<List<ProjectTypeByMonthResponseModel>>.SuccessResponse(
					resultByProjectTypeDto?.Select(x => new ProjectTypeByMonthResponseModel(x))
					?.ToList());

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


