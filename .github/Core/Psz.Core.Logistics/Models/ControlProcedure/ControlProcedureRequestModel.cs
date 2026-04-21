using Psz.Core.Logistics.Models.ControlProcedure;

namespace Psz.Core.Logistics.Models.ControlProcedure
{
	public class ControlProcedureRequestModel: IPaginatedRequestModel
	   {
			public string SearchValue { get; set; }
		}
		public class ControlProcedureResponseModel
	     {
		public int? ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public decimal? ControlledAverage { get; set; }
		public decimal? ControlledFailedQuantity { get; set; }
		public decimal? ControlledMeasuredValue { get; set; }
		public decimal? ControlledQuantity { get; set; }
		public decimal? ControlledSum { get; set; }
		public decimal? ControlledTotalQuantity { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? CreateUserId { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public string ProcedureDescription { get; set; }
		public string ProcedureName { get; set; }
		public string ProcedureType { get; set; }
		public int? SupplierId { get; set; }
		public string SupplierName { get; set; }


		public ControlProcedureResponseModel() { }
		public ControlProcedureResponseModel(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleControlledProceduresEntity __lgtcontrolprocedureEntity)
			{
				if(__lgtcontrolprocedureEntity == null)
					return;

			    ArticleId = __lgtcontrolprocedureEntity.ArticleId;
				ArticleNumber = __lgtcontrolprocedureEntity.ArticleNumber;
				CreateTime = __lgtcontrolprocedureEntity.CreateTime;
				CreateUserId = __lgtcontrolprocedureEntity.CreateUserId;
				Id = __lgtcontrolprocedureEntity.Id;
				LastEditTime = __lgtcontrolprocedureEntity.LastEditTime;
				LastEditUserId = __lgtcontrolprocedureEntity.LastEditUserId;
				ProcedureDescription = __lgtcontrolprocedureEntity.ProcedureDescription;
				ProcedureName = __lgtcontrolprocedureEntity.ProcedureName;
				SupplierId = __lgtcontrolprocedureEntity.SupplierId;
				SupplierName = __lgtcontrolprocedureEntity.SupplierName;
				ControlledAverage = __lgtcontrolprocedureEntity.ControlledAverage;
				ControlledFailedQuantity = __lgtcontrolprocedureEntity.ControlledFailedQuantity;
				ControlledMeasuredValue = __lgtcontrolprocedureEntity.ControlledMeasuredValue;
				ControlledQuantity = __lgtcontrolprocedureEntity.ControlledQuantity;
				ControlledSum = __lgtcontrolprocedureEntity.ControlledSum;
				ControlledTotalQuantity = __lgtcontrolprocedureEntity.ControlledTotalQuantity;
				ProcedureType = __lgtcontrolprocedureEntity.ProcedureType;

	}
		}
	}
	public class GetControlProcedureResponseModel: IPaginatedResponseModel<ControlProcedureResponseModel>
	{
	}


