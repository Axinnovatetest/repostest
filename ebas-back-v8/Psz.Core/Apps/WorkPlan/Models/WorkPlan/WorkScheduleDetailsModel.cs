using iText.Layout.Element;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Models.WorkPlan
{
	public class WorkScheduleDetailsModel
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

		public WorkScheduleDetailsModel(Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity entity)
		{
			WorkScheduleId = entity.WorkScheduleId;
			Amount = entity.Amount;
			CountryId = entity.CountryId;
			CreationTime = entity.CreationTime;
			CreationUserId = entity.CreationUserId;
			DepartementId = entity.DepartementId;
			FromToolInsert = entity.FromToolInsert;
			FromToolInsert2 = entity.FromToolInsert2;
			HallId = entity.HallId;
			Id = entity.Id;
			LastEditTime = entity.LastEditTime;
			LastEditUserId = entity.LastEditUserId;
			LotSizeSTD = entity.LotSizeSTD;
			OperationDescriptionId = entity.OperationDescriptionId;
			OperationNumber = entity.OperationNumber;
			OperationTimeSeconds = entity.OperationTimeSeconds;
			OperationTimeValueAdding = entity.OperationTimeValueAdding;
			PredecessorOperation = entity.PredecessorOperation;
			PredecessorSubOperation = entity.PredecessorSubOperation;
			RelationOperationTime = entity.RelationOperationTime;
			SetupTimeMinutes = entity.SetupTimeMinutes;
			StandardOccupancy = entity.StandardOccupancy;
			StandardOperationId = entity.StandardOperationId;
			SubOperationNumber = entity.SubOperationNumber;
			TotalTimeOperation = entity.TotalTimeOperation;
			WorkAreaId = entity.WorkAreaId;
			Comment = entity.Comment;
			WorkStationMachineId = entity.WorkStationMachineId;
			OperationValueAdding = entity.OperationValueAdding;
			OrderDisplayId = entity.OrderDisplayId;

			CountryName = entity.CountryName;
			HallName = entity.HallName;
			DepartmentName = entity.DepartmentName;
			WorkAreaName = entity.WorkAreaName;
			WorkStationMachineName = entity.WorkStationMachineName;
			StandardOperationName = entity.StandardOperationName;
			OperationDescriptionName = entity.OperationDescriptionName;
		}
	}
	public class WorkScheduleExcelModel
	{
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public List<WorkScheduleDetailsModel> Positions { get; set; }
	}
}
