using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CPL
{
	public class Capital_requests_positionsEntity
	{
		public bool? CapitalBOM { get; set; }
		public bool? CapitalClose { get; set; }
		public bool? CapitalCP { get; set; }
		public DateTime? CapitalDate { get; set; }
		public bool? CapitalDOC { get; set; }
		public bool? CapitalFB { get; set; }
		public string CapitalReply { get; set; }
		public bool? CapitalStatus { get; set; }
		public bool? EngeneeringValidation { get; set; }
		public DateTime? EngeneeringValidationDate { get; set; }
		public int? HeaderId { get; set; }
		public int Id { get; set; }
		public string IncidentCategory { get; set; }
		public int? IncidentCategoryId { get; set; }
		public DateTime? IncidentDate { get; set; }
		public string IncidentDescription { get; set; }
		public int? PositionId { get; set; }

		public Capital_requests_positionsEntity() { }

		public Capital_requests_positionsEntity(DataRow dataRow)
		{
			CapitalBOM = (dataRow["CapitalBOM"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CapitalBOM"]);
			CapitalClose = (dataRow["CapitalClose"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CapitalClose"]);
			CapitalCP = (dataRow["CapitalCP"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CapitalCP"]);
			CapitalDate = (dataRow["CapitalDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CapitalDate"]);
			CapitalDOC = (dataRow["CapitalDOC"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CapitalDOC"]);
			CapitalFB = (dataRow["CapitalFB"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CapitalFB"]);
			CapitalReply = (dataRow["CapitalReply"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CapitalReply"]);
			CapitalStatus = (dataRow["CapitalStatus"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CapitalStatus"]);
			EngeneeringValidation = (dataRow["EngeneeringValidation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EngeneeringValidation"]);
			EngeneeringValidationDate = (dataRow["EngeneeringValidationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EngeneeringValidationDate"]);
			HeaderId = (dataRow["HeaderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["HeaderId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IncidentCategory = (dataRow["IncidentCategory"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IncidentCategory"]);
			IncidentCategoryId = (dataRow["IncidentCategoryId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IncidentCategoryId"]);
			IncidentDate = (dataRow["IncidentDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["IncidentDate"]);
			IncidentDescription = (dataRow["IncidentDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IncidentDescription"]);
			PositionId = (dataRow["PositionId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PositionId"]);
		}

		public Capital_requests_positionsEntity ShallowClone()
		{
			return new Capital_requests_positionsEntity
			{
				CapitalBOM = CapitalBOM,
				CapitalClose = CapitalClose,
				CapitalCP = CapitalCP,
				CapitalDate = CapitalDate,
				CapitalDOC = CapitalDOC,
				CapitalFB = CapitalFB,
				CapitalReply = CapitalReply,
				CapitalStatus = CapitalStatus,
				EngeneeringValidation = EngeneeringValidation,
				EngeneeringValidationDate = EngeneeringValidationDate,
				HeaderId = HeaderId,
				Id = Id,
				IncidentCategory = IncidentCategory,
				IncidentCategoryId = IncidentCategoryId,
				IncidentDate = IncidentDate,
				IncidentDescription = IncidentDescription,
				PositionId = PositionId
			};
		}
	}
}

