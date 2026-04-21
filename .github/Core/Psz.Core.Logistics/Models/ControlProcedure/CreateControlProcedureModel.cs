using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.ControlProcedure;

namespace Psz.Core.Logistics.Models.ControlProcedure
{
	public class GetAllControlProcedureRequestModel: IPaginatedRequestModel

	{
		public string SearchValue { get; set; }
	}
	public class CreateControlProcedureRequestModel: IPaginatedRequestModel

	{
		public string SearchValue { get; set; }
		public int ArticleId { get; set; }
		public int SupplierId { get; set; }
	}
	public class CreateControlProcedureResponseModel
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
	

		public CreateControlProcedureResponseModel() { }
		public CreateControlProcedureResponseModel(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity __lgtcontrolprocedureEntity)
		{
			if(__lgtcontrolprocedureEntity == null)
				return;

			ArticleId = __lgtcontrolprocedureEntity.Id;
			ArticleNumber = __lgtcontrolprocedureEntity.ArticleNumber;
			Id = __lgtcontrolprocedureEntity.Id;
			LastEditUserId = __lgtcontrolprocedureEntity.LastEditUserId;
			ProcedureDescription = __lgtcontrolprocedureEntity.ProcedureDescription;
			ProcedureName = __lgtcontrolprocedureEntity.ProcedureName;
			SupplierId = __lgtcontrolprocedureEntity.SupplierId;
			SupplierName = __lgtcontrolprocedureEntity.SupplierName;


		}
	}
}
public class CrerateControlProcedureResponseModel: IPaginatedResponseModel<CreateControlProcedureResponseModel>
{
}



public class CreateControlProcedureVM
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
	public CreateControlProcedureVM() { }

}