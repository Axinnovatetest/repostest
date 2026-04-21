using System;

namespace Psz.Core.Apps.WorkPlan.Models.WorkScheduleDetails
{
	public class WorkScheduleDetailsViewModel
	{
		public int CountryId { get; set; }
		public int CreationUserId { get; set; }
		public int DepartementId { get; set; }
		public int HallId { get; set; }
		public int WorkAreaId { get; set; }
		public int? WorkStationMachineId { get; set; }
		public int? OperationDescriptionId { get; set; }
		public int StandardOperationId { get; set; }
		public DateTime CreationDate { get; set; }
		public int Id { get; set; }

		public int WorkScheduleId { get; set; }
		public bool? OperationValueAdding { get; set; }

		public int OperationNumber { get; set; }
		public double OperationTimeValueAdding { get; set; }
		public int LotSizeSTD { get; set; }
		public decimal TotalTimeOperation { get; set; }
		public int OrderDisplayId { get; set; }

		public string FromToolInsert { get; set; }
		public string FromToolInsert2 { get; set; }
		public decimal Amount { get; set; }
		public decimal OperationTimeSeconds { get; set; }
		public int PredecessorOperation { get; set; }
		public string PredecessorSubOperation { get; set; }
		public double RelationOperationTime { get; set; }
		public decimal SetupTimeMinutes { get; set; }
		public double StandardOccupancy { get; set; }
		public string SubOperationNumber { get; set; }
		public string Comment { get; set; }
		//names
		public string CountryName { get; set; }
		public string HallName { get; set; }
		public string DepartmentName { get; set; }
		public string WorkAreaName { get; set; }
		public string WorkStationMachineName { get; set; }
		public string StandardOperationName { get; set; }
		public string OperationDescriptionName { get; set; }

		public WorkScheduleDetailsViewModel() { }
		public WorkScheduleDetailsViewModel(Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity workScheduleDetailsDb, string workArea)
		{
			this.Amount = workScheduleDetailsDb.Amount;
			this.CountryId = workScheduleDetailsDb.CountryId;
			this.CreationDate = workScheduleDetailsDb.CreationTime;
			this.CreationUserId = workScheduleDetailsDb.CreationUserId;
			this.DepartementId = workScheduleDetailsDb.DepartementId;
			this.FromToolInsert = workScheduleDetailsDb.FromToolInsert;
			this.FromToolInsert2 = workScheduleDetailsDb.FromToolInsert2;
			this.HallId = workScheduleDetailsDb.HallId;
			this.Id = workScheduleDetailsDb.Id;
			this.LotSizeSTD = workScheduleDetailsDb.LotSizeSTD;
			this.OperationDescriptionId = workScheduleDetailsDb.OperationDescriptionId;
			this.OperationNumber = workScheduleDetailsDb.OperationNumber;
			this.OperationTimeSeconds = Math.Round(workScheduleDetailsDb.OperationTimeSeconds, Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
			this.OperationTimeValueAdding = workScheduleDetailsDb.OperationTimeValueAdding;
			this.PredecessorOperation = workScheduleDetailsDb.PredecessorOperation;
			this.PredecessorSubOperation = workScheduleDetailsDb.PredecessorSubOperation;
			this.RelationOperationTime = Math.Round(workScheduleDetailsDb.RelationOperationTime, Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
			this.SetupTimeMinutes = Math.Round(workScheduleDetailsDb.SetupTimeMinutes, Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
			this.StandardOccupancy = workScheduleDetailsDb.StandardOccupancy;
			this.StandardOperationId = workScheduleDetailsDb.StandardOperationId;
			this.SubOperationNumber = workScheduleDetailsDb.SubOperationNumber;
			this.TotalTimeOperation = Math.Round(workScheduleDetailsDb.TotalTimeOperation, Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
			this.WorkAreaId = workScheduleDetailsDb.WorkAreaId;
			this.WorkStationMachineId = workScheduleDetailsDb.WorkStationMachineId;
			this.WorkScheduleId = workScheduleDetailsDb.WorkScheduleId;
			this.OperationValueAdding = workScheduleDetailsDb.OperationValueAdding ?? null;
			this.Comment = workScheduleDetailsDb.Comment;
			this.OrderDisplayId = workScheduleDetailsDb.OrderDisplayId;
			this.CountryName = workScheduleDetailsDb.CountryName;
			this.HallName = workScheduleDetailsDb.HallName;
			this.DepartmentName = workScheduleDetailsDb.DepartmentName;
			this.WorkAreaName = workArea;
			this.WorkStationMachineName = workScheduleDetailsDb.WorkStationMachineName;
			this.StandardOperationName = workScheduleDetailsDb.StandardOperationName;
			this.OperationDescriptionName = workScheduleDetailsDb.OperationDescriptionName;
		}
	}
}
