using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Models.WorkPlan
{
	public class WorkScheduleViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int ArticleId { get; set; }
		public int HallId { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreationTime { get; set; }
		public string CreationUserName { get; set; }
		public DateTime? LastEditTime { get; set; }
		public string LastEditUserName { get; set; }
		public bool CanSafeDelete { get; set; }
		public string HallName { get; set; }
		public decimal FAQuantity { get; set; }
		public double TotalOperationValueAdding { get; set; }
		public double TotalOperationTime { get; set; }
		public decimal Ratio { get; set; }

		public WorkScheduleViewModel() { }
		public WorkScheduleViewModel(Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity workScheduleDb)
		{
			this.Id = workScheduleDb.Id;
			this.ArticleId = workScheduleDb.ArticleId;
			this.HallId = workScheduleDb.HallId;
			this.HallName = Helpers.User.getHallNameById(this.HallId);
			this.IsActive = workScheduleDb.IsActive;
			this.Name = workScheduleDb.Name;
			this.CreationTime = workScheduleDb.CreationTime;
			this.CreationUserName = Helpers.User.GetUserNameById(workScheduleDb.CreationUserId);
			this.LastEditTime = workScheduleDb.LastEditTime.HasValue ? workScheduleDb.LastEditTime.Value : (DateTime?)null;
			this.LastEditUserName = workScheduleDb.LastEditUserId.HasValue ? Helpers.User.GetUserNameById(workScheduleDb.LastEditUserId.Value) : "";
			this.CanSafeDelete = Psz.Core.Apps.WorkPlan.Helpers.DeleteCheck.CanSafeDeleteWorkSchedule(workScheduleDb.Id);

			// - 
			var details = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.GetByWorkScheduleId(workScheduleDb.Id)
				?? new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			this.FAQuantity = 1;
			this.TotalOperationValueAdding = 0;
			this.TotalOperationTime = 0;
			foreach(var item in details)
			{
				item.LotSizeSTD = Convert.ToInt32(this.FAQuantity);
				var _item = Core.Apps.WorkPlan.Helpers.WorkSchedule.setTotalTimeOperation(item);
				_item.OperationTimeValueAdding = Core.Apps.WorkPlan.Helpers.WorkSchedule.GetOperationTimeValueAddng(_item);
				TotalOperationValueAdding += Math.Round(_item.OperationTimeValueAdding, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
				TotalOperationTime += Math.Round((double)_item.TotalTimeOperation, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
			}
			Ratio = (decimal)(TotalOperationValueAdding > 0
						? Math.Round((TotalOperationTime - TotalOperationValueAdding) / TotalOperationValueAdding, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS)
						: 0);
		}
	}
}

