using Psz.Core.Common.Models;
using System;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial.BomChangeRequests
{
	public class BomChangeRequestsModel
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
		public string SortFieldKey { get; internal set; }
		public bool SortDesc { get; internal set; }

		public BomChangeRequestsModel()
		{

		}
		public BomChangeRequestsModel(Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity entity)
		{
			AcceptanceDate = entity.AcceptanceDate;
			Artikel_Nr = entity.Artikel_Nr;
			Artikelnummer = entity.Artikelnummer;
			Comments = entity.Comments;
			Deleted_date = entity.Deleted_date;
			Deleted_user_id = entity.Deleted_user_id;
			Deleted_username = entity.Deleted_username;
			Id = entity.Id;
			Is_deleted = entity.Is_deleted;
			Reason = entity.Reason;
			RejectionDate = entity.RejectionDate;
			RejectionReason = entity.RejectionReason;
			RequestDate = entity.RequestDate;
			Requester_email = entity.Requester_email;
			Requester_id = entity.Requester_id;
			Requester_name = entity.Requester_name;
			Status = entity.Status;
			SubmissionDate = entity.SubmissionDate;
			Validator_id = entity.Validator_id;
		}
		public Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity
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
				Validator_id = Validator_id,
			};
		}
	}
	public class ChangeRequestStatusRequestmodel
	{
		public int Idrequest { get; set; }
		public int IdStatus { get; set; }
		public string Reason { get; set; }
	}

	public class GetAllBomChangeRequestModel: IPaginatedRequestModel
	{
		public string ArticleNumber { get; set; }
	}

}
