using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.WorkPlan.Helpers
{
	public class DeleteCheck
	{
		public static List<string> CanSafeDeleteStandardOperation(int StandardOperationId)
		{
			var operationDescriptionDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.GetByOperationId(StandardOperationId);
			if(operationDescriptionDb != null && operationDescriptionDb.Count > 0)
				return operationDescriptionDb.Select(x => x.Description).Take(5).ToList();

			var WorkScheduleDetailDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.GetByStandardOperationId(StandardOperationId);
			if(WorkScheduleDetailDb != null && WorkScheduleDetailDb.Count > 0)
			{
				var workScheduleEntities = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(WorkScheduleDetailDb.Select(x => x.WorkScheduleId).ToList());
				if(workScheduleEntities != null && workScheduleEntities.Count > 0)
				{
					var articleEntities = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(workScheduleEntities.Select(x => x.ArticleId).ToList());
					if(articleEntities != null && articleEntities.Count > 0)
					{
						return articleEntities.Select(x => x.Name).Take(5).ToList();
					}
				}
			}

			return null;
		}
		public static List<string> CanSafeDeleteOperationDescription(int OperationDescriptionId)
		{
			var WorkScheduleDetailDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.GetByOperationDescriptionId(OperationDescriptionId);

			if(WorkScheduleDetailDb != null && WorkScheduleDetailDb.Count > 0)
			{
				var workScheduleEntities = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(WorkScheduleDetailDb.Select(x => x.WorkScheduleId).ToList());
				if(workScheduleEntities != null && workScheduleEntities.Count > 0)
				{
					var articleEntities = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(workScheduleEntities.Select(x => x.ArticleId).ToList());
					if(articleEntities != null && articleEntities.Count > 0)
					{
						return articleEntities.Select(x => $"{x.Name}").Take(5).ToList();
					}
				}
			}

			return null;
		}

		public static Boolean CanSafeDeleteCountry(int countryId)
		{
			if(Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.CountByCountryId(countryId) > 0)
				return false;

			if(Infrastructure.Data.Access.Tables.WPL.HallAccess.CountByCountryId(countryId) > 0)
				return false;

			if(Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.CountByCountryId(countryId) > 0)
				return false;

			if(Infrastructure.Data.Access.Tables.WPL.UserCountryAccess.CountByCountryId(countryId) > 0)
				return false;

			return true;
		}
		public static Boolean CanSafeDeleteHall(int hallId)
		{
			if(Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.CountByHallId(hallId) > 0)
				return false;

			if(Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.CountByHallId(hallId) > 0)
				return false;

			if(Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.CountByHallId(hallId) > 0)
				return false;

			if(Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.CountByHallId(hallId) > 0)
				return false;

			return true;
		}

		public static Boolean CanSafeDeleteWorkStationMachine(int WorkStationMachineId)
		{
			var WorkScheduleDetailDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.CountByWorkStationMachineId(WorkStationMachineId);

			if(WorkScheduleDetailDb == 0)
				return true;
			return false;
		}
		public static Boolean CanSafeDeleteWorkArea(int WorkAreaId)
		{
			var WorkScheduleDetailDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.CountByWorkAreaId(WorkAreaId);

			if(WorkScheduleDetailDb == 0)
				return true;
			return false;

		}
		public static Boolean CanSafeDeleteDepartement(int DepartmentId)
		{
			var WorkScheduleDetailDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.CountByDepartementId(DepartmentId);

			if(WorkScheduleDetailDb != 0)
				return false;

			return Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.CountByDepartmentId(DepartmentId) == 0;
		}

		public static Boolean CanSafeDeleteWorkSchedule(int workScheduleId)
		{
			var workScheduleDetailsDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.CountByWorkScheduleId(workScheduleId);
			if(workScheduleDetailsDb == 0)
				return true;
			return false;
		}
		public static Boolean CanSafeDeleteArticle(int ArticleId)
		{
			var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.CountByArticleId(ArticleId);
			if(workScheduleDb == 0)
				return true;
			return false;
		}
	}
}
