using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests
{
	public class __BSD_BomChangesRequestsEntity
	{
		public DateTime? AcceptanceDate { get; set; }
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Comments { get; set; }
		public DateTime Deleted_date { get; set; }
		public int Deleted_user_id { get; set; }
		public string Deleted_username { get; set; }
		public int Id { get; set; }
		public bool? Is_deleted { get; set; }
		public string Reason { get; set; }
		public DateTime? RejectionDate { get; set; }
		public string RejectionReason { get; set; }
		public DateTime? RequestDate { get; set; }
		public string Requester_email { get; set; }
		public int Requester_id { get; set; }
		public string Requester_name { get; set; }
		public string Status { get; set; }
		public DateTime SubmissionDate { get; set; }
		public int Validator_id { get; set; }

		public __BSD_BomChangesRequestsEntity() { }

		public __BSD_BomChangesRequestsEntity(DataRow dataRow)
		{
			AcceptanceDate = (dataRow["AcceptanceDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AcceptanceDate"]);
			Artikel_Nr = Convert.ToInt32(dataRow["Artikel_Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Comments = (dataRow["Comments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Comments"]);
			Deleted_date = Convert.ToDateTime(dataRow["Deleted_date"]);
			Deleted_user_id = Convert.ToInt32(dataRow["Deleted_user_id"]);
			Deleted_username = (dataRow["Deleted_username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Deleted_username"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Is_deleted = (dataRow["Is_deleted"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Is_deleted"]);
			Reason = Convert.ToString(dataRow["Reason"]);
			RejectionDate = (dataRow["RejectionDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["RejectionDate"]);
			RejectionReason = (dataRow["RejectionReason"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RejectionReason"]);
			RequestDate = (dataRow["RequestDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["RequestDate"]);
			Requester_email = (dataRow["Requester_email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Requester_email"]);
			Requester_id = Convert.ToInt32(dataRow["Requester_id"]);
			Requester_name = (dataRow["Requester_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Requester_name"]);
			Status = Convert.ToString(dataRow["Status"]);
			SubmissionDate = Convert.ToDateTime(dataRow["SubmissionDate"]);
			Validator_id = Convert.ToInt32(dataRow["Validator_id"]);
		}

		public __BSD_BomChangesRequestsEntity ShallowClone()
		{
			return new __BSD_BomChangesRequestsEntity
			{
				AcceptanceDate = AcceptanceDate,
				Artikel_Nr = Artikel_Nr,
				Artikelnummer = Artikelnummer,
				Comments = Comments,
				Deleted_date = Deleted_date,
				Deleted_user_id = Deleted_user_id,
				Deleted_username = Deleted_username,
				Id = Id,
				Is_deleted = Is_deleted,
				Reason = Reason,
				RejectionDate = RejectionDate,
				RejectionReason = RejectionReason,
				RequestDate = RequestDate,
				Requester_email = Requester_email,
				Requester_id = Requester_id,
				Requester_name = Requester_name,
				Status = Status,
				SubmissionDate = SubmissionDate,
				Validator_id = Validator_id
			};
		}


	}
}
