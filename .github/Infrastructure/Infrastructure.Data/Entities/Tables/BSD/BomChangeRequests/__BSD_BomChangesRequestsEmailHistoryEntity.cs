using System;
using System.Data;


namespace Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests
{
	public class __BSD_BomChangesRequestsEmailHistoryEntity
	{
		public int Id { get; set; }
		public string Mail_subject { get; set; }
		public string Requester_email { get; set; }
		public int Requester_id { get; set; }
		public string Requester_name { get; set; }
		public DateTime? Sending_date { get; set; }
		public string Status { get; set; }
		public string Validator_email { get; set; }
		public int Validator_id { get; set; }

		public __BSD_BomChangesRequestsEmailHistoryEntity() { }

		public __BSD_BomChangesRequestsEmailHistoryEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			Mail_subject = Convert.ToString(dataRow["Mail_subject"]);
			Requester_email = (dataRow["Requester_email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Requester_email"]);
			Requester_id = Convert.ToInt32(dataRow["Requester_id"]);
			Requester_name = (dataRow["Requester_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Requester_name"]);
			Sending_date = (dataRow["Sending_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Sending_date"]);
			Status = Convert.ToString(dataRow["Status"]);
			Validator_email = (dataRow["Validator_email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Validator_email"]);
			Validator_id = Convert.ToInt32(dataRow["Validator_id"]);
		}

		public __BSD_BomChangesRequestsEmailHistoryEntity ShallowClone()
		{
			return new __BSD_BomChangesRequestsEmailHistoryEntity
			{
				Id = Id,
				Mail_subject = Mail_subject,
				Requester_email = Requester_email,
				Requester_id = Requester_id,
				Requester_name = Requester_name,
				Sending_date = Sending_date,
				Status = Status,
				Validator_email = Validator_email,
				Validator_id = Validator_id
			};
		}
	}
}
