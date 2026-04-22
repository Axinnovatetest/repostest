using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Apps.WorkPlan.Models.WorkPlan
{
	public class WorkPlanHeaderModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int ArticleId { get; set; }
		public int HallId { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
	}
	public class WorkScheduleDetailsLineItemModel
	{
		public int WorkScheduleId { get; set; }
		public decimal Amount { get; set; }
		public int CountryId { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public int DepartementId { get; set; }
		public string FromToolInsert { get; set; }
		public string FromToolInsert2 { get; set; }
		public int HallId { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public int LotSizeSTD { get; set; }
		public int? OperationDescriptionId { get; set; }
		public int OperationNumber { get; set; }
		public decimal OperationTimeSeconds { get; set; }
		public double OperationTimeValueAdding { get; set; }
		public int PredecessorOperation { get; set; }
		public string PredecessorSubOperation { get; set; }
		public double RelationOperationTime { get; set; }
		public decimal SetupTimeMinutes { get; set; }
		public double StandardOccupancy { get; set; }
		public int StandardOperationId { get; set; }
		public string SubOperationNumber { get; set; }
		public decimal TotalTimeOperation { get; set; }
		public int WorkAreaId { get; set; }
		public string Comment { get; set; }
		public int? WorkStationMachineId { get; set; }
		public bool? OperationValueAdding { get; set; }
		public int OrderDisplayId { get; set; }
		//names
		public string CountryName { get; set; }
		public string HallName { get; set; }
		public string DepartmentName { get; set; }
		public string WorkAreaName { get; set; }
		public string WorkStationMachineName { get; set; }
		public string StandardOperationName { get; set; }
		public string OperationDescriptionName { get; set; }
	}
		public class WorkPlanCreateModel
	{
		
			public WorkPlanHeaderModel Header { get; set; }
			public List<WorkScheduleDetailsLineItemModel> LineItems { get; set; }
		
	}
}
