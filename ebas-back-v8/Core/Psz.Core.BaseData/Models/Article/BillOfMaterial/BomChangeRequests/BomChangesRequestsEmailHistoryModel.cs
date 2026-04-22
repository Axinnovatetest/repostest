using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial.BomChangeRequests
{
	public class BomChangesRequestsEmailHistoryModel
	{
		public BomChangesRequestsEmailHistoryModel()
		{
		}

		public int Id { get; set; }
		public string Mail_subject { get; set; }
		public string Requester_email { get; set; }
		public int Requester_id { get; set; }
		public string Requester_name { get; set; }
		public DateTime? Sending_date { get; set; }
		public string Status { get; set; }
		public string Validator_email { get; set; }
		public int Validator_id { get; set; }

		public BomChangesRequestsEmailHistoryModel(Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryEntity entity) {
			Id = entity.Id;
			Mail_subject = entity.Mail_subject;
			Requester_email = entity.Requester_email;
			Requester_id = entity.Requester_id;
			Requester_name = entity.Requester_name;
			Sending_date = entity.Sending_date;
			Status = entity.Status;
			Validator_email = entity.Validator_email;
			Validator_id = entity.Validator_id;

		}
		public Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryEntity
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
