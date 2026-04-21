using Psz.Core.BaseData.Models.Article.BillOfMaterial.BomChangeRequests;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.BomChangeRequests
{
	public class GetBomChangeRequestByIdHandler: IHandle<UserModel, ResponseModel<BomChangeRequestsModel>>
	{
		private int _id;
		private UserModel _user;
		public GetBomChangeRequestByIdHandler(UserModel user, int id)
		{
			this._user = user;
			this._id = id;
		}
		public ResponseModel<BomChangeRequestsModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return this.Validate();
				}

				var bomChangeById = Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsAccess.Get(this._id);

				if(bomChangeById is not null)
				{
					return ResponseModel<BomChangeRequestsModel>.SuccessResponse(new BomChangeRequestsModel
					{
						AcceptanceDate = bomChangeById.AcceptanceDate,
						Artikel_Nr = bomChangeById.Artikel_Nr,
						Artikelnummer = bomChangeById.Artikelnummer,
						Comments = bomChangeById.Comments,
						Deleted_date = bomChangeById.Deleted_date,
						Deleted_user_id = bomChangeById.Deleted_user_id,
						Deleted_username = bomChangeById.Deleted_username,
						Id = bomChangeById.Id,
						Is_deleted = bomChangeById.Is_deleted,
						Reason = bomChangeById.Reason,
						RejectionDate = bomChangeById.RejectionDate,
						RejectionReason = bomChangeById.RejectionReason,
						RequestDate = bomChangeById.RequestDate,
						Requester_email = bomChangeById.Requester_email,
						Requester_id = bomChangeById.Requester_id,
						Requester_name = bomChangeById.Requester_name,
						Status = bomChangeById.Status,
						SubmissionDate = bomChangeById.SubmissionDate,
						Validator_id = bomChangeById.Validator_id
					});
				}

				return ResponseModel<BomChangeRequestsModel>.FailureResponse("Error when getting bom change request");

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<BomChangeRequestsModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<BomChangeRequestsModel>.AccessDeniedResponse();

			}
			return ResponseModel<BomChangeRequestsModel>.SuccessResponse();
		}
	}
}
