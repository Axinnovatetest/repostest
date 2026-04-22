using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class PlantBookingsArticleSupplierControlProceduresEntity
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
		public string? ProcedureType { get; set; }
		public int? SupplierId { get; set; }
		public string? SupplierName { get; set; }

		public PlantBookingsArticleSupplierControlProceduresEntity() { }

		public PlantBookingsArticleSupplierControlProceduresEntity(DataRow dataRow)
		{
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			ControlledAverage = (dataRow["ControlledAverage"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ControlledAverage"]);
			ControlledFailedQuantity = (dataRow["ControlledFailedQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ControlledFailedQuantity"]);
			ControlledMeasuredValue = (dataRow["ControlledMeasuredValue"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ControlledMeasuredValue"]);
			ControlledQuantity = (dataRow["ControlledQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ControlledQuantity"]);
			ControlledSum = (dataRow["ControlledSum"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ControlledSum"]);
			ControlledTotalQuantity = (dataRow["ControlledTotalQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ControlledTotalQuantity"]);
			CreateTime = (dataRow["CreateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateTime"]);
			CreateUserId = (dataRow["CreateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreateUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			ProcedureDescription = (dataRow["ProcedureDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProcedureDescription"]);
			ProcedureName = (dataRow["ProcedureName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProcedureName"]);
			ProcedureType = (dataRow["ProcedureType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProcedureType"]);
			SupplierId = (dataRow["SupplierId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierId"]);
			SupplierName = (dataRow["SupplierName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierName"]);
		}

		public PlantBookingsArticleSupplierControlProceduresEntity ShallowClone()
		{
			return new PlantBookingsArticleSupplierControlProceduresEntity
			{
				ArticleId = ArticleId,
				ArticleNumber = ArticleNumber,
				ControlledAverage = ControlledAverage,
				ControlledFailedQuantity = ControlledFailedQuantity,
				ControlledMeasuredValue = ControlledMeasuredValue,
				ControlledQuantity = ControlledQuantity,
				ControlledSum = ControlledSum,
				ControlledTotalQuantity = ControlledTotalQuantity,
				CreateTime = CreateTime,
				CreateUserId = CreateUserId,
				Id = Id,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				ProcedureDescription = ProcedureDescription,
				ProcedureName = ProcedureName,
				ProcedureType = ProcedureType,
				SupplierId = SupplierId,
				SupplierName = SupplierName
			};
		}
	}
}

