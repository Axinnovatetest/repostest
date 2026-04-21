using Infrastructure.Data.Entities.Tables.WPL;
using System;
using System.Linq;

namespace Psz.Core.Apps.WorkPlan.Handlers.WorkPlan
{
	public partial class WorkPlan
	{
		internal class Helpers
		{
			public static Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity ToDataEntity(Models.WorkPlan.WorkPlanModel workplan)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity
				{
					ArticleId = workplan.ArticleId,
					CreationTime = workplan.CreationTime,
					CreationUserId = workplan.CreationUserId,
					HallId = workplan.HallId,
					Id = workplan.Id,
					IsActive = workplan.IsActive,
					Name = workplan.Name,
					LastEditTime = workplan.LastEditTime,
					LastEditUserId = workplan.LastEditUserId,
				};
			}

			public static Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity PrepareToDb(Models.WorkPlan.WorkPlanModel workplan)
			{

				var workPlansDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetByHallIdArticleId(workplan.HallId, workplan.ArticleId);

				if(string.IsNullOrEmpty(workplan.Name)) // If it's an add it will be empty else it will be an edit.
				{
					var name = 1;
					if(workPlansDb.Count != 0)
						name = workPlansDb.Max(ws => int.Parse(ws.Name.Substring(2, ws.Name.Length - 2)));
					workplan.Name = "AP" + name;
				}
				if(workplan.IsActive == true)
				{
					workPlansDb.ForEach(ws => ws = edit(ws, workplan.CreationUserId));
					Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Update(workPlansDb);
				}

				return ToDataEntity(workplan);
			}

			private static WorkPlanEntity edit(WorkPlanEntity ws, int UserId)
			{
				ws.IsActive = false;
				ws.LastEditTime = DateTime.Now;
				ws.LastEditUserId = UserId;

				return ws;
			}
		}
	}

}
