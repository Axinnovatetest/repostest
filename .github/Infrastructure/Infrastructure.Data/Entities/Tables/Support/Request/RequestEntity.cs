using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Support.Request
{
	public class RequestEntity
	{
		public int Application { get; set; }
		public string Benefits { get; set; }
		public string BuisnessProcess { get; set; }
		public string Consequences { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public DateTime Date { get; set; }
		public string Department { get; set; }
		public string Dependencies { get; set; }
		public string Goals { get; set; }
		public int Id { get; set; }
		public string ItConcept { get; set; }
		public string ItConditions { get; set; }
		public string ItEffort { get; set; }
		public string ItFeasibility { get; set; }
		public string ItNr { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public string OtherApplication { get; set; }
		public string OtherReason { get; set; }
		public int? Priority { get; set; }
		public string Problem { get; set; }
		public int Reason { get; set; }
		public bool? Released { get; set; }
		public string Requester { get; set; }
		public string Requirement { get; set; }
		public int? Status { get; set; }
		public string Theme { get; set; }
		public bool? Validated { get; set; }
		public DateTime? ValidationDate { get; set; }
		public int? ValidationUserId { get; set; }

		public RequestEntity() { }

		public RequestEntity(DataRow dataRow)
		{
			Application = Convert.ToInt32(dataRow["Application"]);
			Benefits = (dataRow["Benefits"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Benefits"]);
			BuisnessProcess = (dataRow["BuisnessProcess"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BuisnessProcess"]);
			Consequences = (dataRow["Consequences"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Consequences"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			Date = Convert.ToDateTime(dataRow["Date"]);
			Department = Convert.ToString(dataRow["Department"]);
			Dependencies = (dataRow["Dependencies"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dependencies"]);
			Goals = (dataRow["Goals"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Goals"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ItConcept = (dataRow["ItConcept"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ItConcept"]);
			ItConditions = (dataRow["ItConditions"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ItConditions"]);
			ItEffort = (dataRow["ItEffort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ItEffort"]);
			ItFeasibility = (dataRow["ItFeasibility"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ItFeasibility"]);
			ItNr = (dataRow["ItNr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ItNr"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			OtherApplication = (dataRow["OtherApplication"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OtherApplication"]);
			OtherReason = (dataRow["OtherReason"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OtherReason"]);
			Priority = (dataRow["Priority"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Priority"]);
			Problem = (dataRow["Problem"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Problem"]);
			Reason = Convert.ToInt32(dataRow["Reason"]);
			Requester = Convert.ToString(dataRow["Requester"]);
			Requirement = (dataRow["Requirement"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Requirement"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Status"]);
			Theme = (dataRow["Theme"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Theme"]);
			Validated = (dataRow["Validated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Validated"]);
			ValidationDate = (dataRow["ValidationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ValidationDate"]);
			ValidationUserId = (dataRow["ValidationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ValidationUserId"]);

		}

		public RequestEntity ShallowClone()
		{
			return new RequestEntity
			{
				Application = Application,
				Benefits = Benefits,
				BuisnessProcess = BuisnessProcess,
				Consequences = Consequences,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				Date = Date,
				Department = Department,
				Dependencies = Dependencies,
				Goals = Goals,
				Id = Id,
				ItConcept = ItConcept,
				ItConditions = ItConditions,
				ItEffort = ItEffort,
				ItFeasibility = ItFeasibility,
				ItNr = ItNr,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				OtherApplication = OtherApplication,
				OtherReason = OtherReason,
				Priority = Priority,
				Problem = Problem,
				Reason = Reason,
				Requester = Requester,
				Requirement = Requirement,
				Status = Status,
				Theme = Theme,
				ValidationDate = ValidationDate,
				ValidationUserId = ValidationUserId,
				Validated = Validated
			};
		}
	}
}
