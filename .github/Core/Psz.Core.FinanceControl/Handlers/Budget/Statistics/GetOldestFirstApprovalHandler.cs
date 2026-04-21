using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetOldestFirstApprovalHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<OldestFirstApprovalResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetOldestFirstApprovalHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}

		public string ConvertToYearsMonthsDaysHoursMinutesSeconds(TimeSpan timeSpan)
		{
			int days = (int)timeSpan.TotalDays;
			int years = days / 365;
			int remainingDays = days % 365;

			int months = remainingDays / 30; // Approximation, 30 days per month
			int remainingDaysInMonth = remainingDays % 30;

			int hours = timeSpan.Hours;
			int minutes = timeSpan.Minutes;
			int seconds = timeSpan.Seconds;

			string TimeTakenForApproval = "";

			if(years != 0)
				TimeTakenForApproval += $"{years} année(s) ";

			if(months != 0)
				TimeTakenForApproval += $"{months} mois ";

			if(remainingDaysInMonth != 0 && years == 0 && months == 0)
				TimeTakenForApproval += $"{remainingDaysInMonth} jour(s) ";

			if(days == 0 && hours == 0 && years == 0 && months == 0 && minutes != 0)
				TimeTakenForApproval += $"{minutes} minutes ";

			return TimeTakenForApproval.Trim();


		}

		public ResponseModel<List<OldestFirstApprovalResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// get project
				var projectEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();

				var openProject = projectEntitiesAll.Where(x => x.Closed == false).ToList();

				var oldestFirstApproval = openProject.OrderBy(project => project.ApprovalTime).ToList();

				var oldestFirstApprovalTimeSpan = oldestFirstApproval?.Where(a => a.ApprovalTime is not null).Select(x => new OldestFirstApprovalResponseModel
				{
					ProjectId = x.Id,

					ProjectName = x.ProjectName,

					CreationDate = x.CreationDate,

					ApprovalTime = x.ApprovalTime,

					Date = (((TimeSpan)(x.ApprovalTime - x.CreationDate))),

					Date_Time = ConvertToYearsMonthsDaysHoursMinutesSeconds((x.ApprovalTime - x.CreationDate).Value)

				}).ToList();

				// Take Five Top 

				var topFiveOldestFirstApprovalTimeSpan = oldestFirstApprovalTimeSpan.Take(5).ToList();

				return ResponseModel<List<OldestFirstApprovalResponseModel>>.SuccessResponse(topFiveOldestFirstApprovalTimeSpan);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<OldestFirstApprovalResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<OldestFirstApprovalResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<OldestFirstApprovalResponseModel>>.SuccessResponse();
		}
	}
}
