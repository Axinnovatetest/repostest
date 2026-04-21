using Infrastructure.Services.Utils;
using iText.StyledXmlParser.Jsoup.Nodes;
using Psz.Core.CapitalRequests.Models;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using RazorLight.Razor;


namespace Psz.Core.CapitalRequests.Handlers
{
	public partial class CapitalRequestsService
	{
		public ResponseModel<int> ValidateUpdateRequest(UserModel user, CapitalRequestModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var headerEntity = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.Get(data.Header.Id);
			if(headerEntity == null)
			{
				return ResponseModel<int>.FailureResponse($"Request not found.");
			}
			var positionsEntities = Infrastructure.Data.Access.Tables.CPL.Capital_requests_positionsAccess.GetByHeaderId(data.Header.Id)
				?? new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
			if(positionsEntities?.Count <= 0)
			{
				return ResponseModel<int>.FailureResponse($"Request positions empty.");
			}

			// - 
			var errors = new List<string>();
			foreach(var x in data.Positions)
			{
				var p = positionsEntities.FirstOrDefault(y => x.Id == y.Id);

				// - prevent Capital form changing closed pos (at Cap. level)
				if(p.CapitalClose == true && (x.CapitalReply != p.CapitalReply || x.CapitalBOM != p.CapitalBOM || x.CapitalFB != p.CapitalFB || x.CapitalDOC != p.CapitalDOC || x.CapitalClose != p.CapitalClose || x.CapitalStatus != p.CapitalStatus))
				{
					errors.Add($"Change aborted: Request Pos[{x.PositionId}] is already closed by Capital Team.");
				}

				// - prevent Capital form changing closed pos (at Eng. level)
				if(p.EngeneeringValidation == true && (x.CapitalReply != p.CapitalReply || x.CapitalBOM != p.CapitalBOM || x.CapitalFB != p.CapitalFB || x.CapitalDOC != p.CapitalDOC || x.CapitalClose != p.CapitalClose || x.CapitalStatus != p.CapitalStatus))
				{
					errors.Add($"Change aborted: Request Pos[{x.PositionId}] is already validated by Engineering Team.");
				}

				// - prevent Eng. form changing open pos (at Capital level) 
				if(x.EngeneeringValidation != p.EngeneeringValidation && p.CapitalClose == false)
				{
					errors.Add($"Change aborted: Request Pos[{x.PositionId}] is not validated by Capital Team.");
				}
			}
			if(errors?.Count > 0)
			{
				return ResponseModel<int>.FailureResponse(errors);
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> UpdateRequest(UserModel user, CapitalRequestModel data)
		{
			var transaction = new TransactionsManager();
			try
			{
				transaction.beginTransaction();
				var validationResponse = ValidateUpdateRequest(user, data);
				if(!validationResponse.Success)
					return validationResponse;

				var oldEntities = Infrastructure.Data.Access.Tables.CPL.Capital_requests_positionsAccess.GetByHeaderId(data.Header.Id, transaction.connection, transaction.transaction);
				var entities = data.Positions.Select(p => p.ToEntity()).ToList();
				var oldHeader = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.GetWithTransaction(entities?[0].HeaderId ?? -1, transaction.connection, transaction.transaction);
				var header = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.GetWithTransaction(entities?[0].HeaderId ?? -1, transaction.connection, transaction.transaction);
				foreach(var item in entities)
				{
					var oldItem = oldEntities.FirstOrDefault(x => x.Id == item.Id);
					if((!oldItem.EngeneeringValidation.HasValue || !oldItem.EngeneeringValidation.Value) && item.EngeneeringValidation.Value)
						item.EngeneeringValidationDate = DateTime.Now;
				}

				var touched = entities?.Where(x => !x.CapitalReply.StringIsNullOrEmptyOrWhiteSpaces()
				|| x.CapitalBOM.Value || x.CapitalFB.Value || x.CapitalDOC.Value || x.CapitalClose.Value || x.CapitalStatus.Value).ToList();
				if(touched != null && touched.Count > 0)
				{
					header.StatusId = (int)Enums.RequestEnums.RequestStatus.InProgress;
					header.Status = Enums.RequestEnums.RequestStatus.InProgress.GetDescription();
				}
				var engFullValidation = entities.Any(x => !x.EngeneeringValidation.Value);
				if(!engFullValidation)
				{
					header.StatusId = (int)Enums.RequestEnums.RequestStatus.Closed;
					header.Status = Enums.RequestEnums.RequestStatus.Closed.GetDescription();
				}
				var PositionsToUpdate = new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
				foreach(var item in oldEntities)
				{
					var newEntity = entities.FirstOrDefault(x => x.Id == item.Id);
					if(item.EngeneeringValidation != newEntity.EngeneeringValidation && newEntity.EngeneeringValidation.HasValue && newEntity.EngeneeringValidation.Value)
						newEntity.EngeneeringValidationDate = DateTime.Now;
					if(item.CapitalReply != newEntity.CapitalReply || item.CapitalBOM != newEntity.CapitalBOM || item.CapitalFB != newEntity.CapitalFB || item.CapitalDOC != newEntity.CapitalDOC || item.CapitalClose != newEntity.CapitalClose || item.CapitalStatus != newEntity.CapitalStatus)
					{
						newEntity.CapitalDate = DateTime.Now;
					}
					PositionsToUpdate.Add(newEntity);
				}
				var oldEngFullValidation = entities.Any(x => !x.EngeneeringValidation.Value);
				if(!engFullValidation && oldEngFullValidation)
					header.CloseDate = DateTime.Now;

				Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.UpdateWithTransaction(header, transaction.connection, transaction.transaction);
				var response = Infrastructure.Data.Access.Tables.CPL.Capital_requests_positionsAccess.UpdateWithTransaction(PositionsToUpdate, transaction.connection, transaction.transaction);
				var logEntities = Helpers.LogHelper.GetPositionChanges(entities, oldEntities, user);
				//prepare mail notifs
				var changedPositionsCapital = new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
				var changedPositionsEng = new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
				foreach(var item in oldEntities)
				{
					var newEntity = entities.FirstOrDefault(x => x.Id == item.Id);
					if(newEntity.CapitalClose != item.CapitalClose || newEntity.CapitalStatus != item.CapitalStatus)
					{
						changedPositionsCapital.Add(item);
					}
					if(newEntity.EngeneeringValidation != item.EngeneeringValidation)
						changedPositionsEng.Add(item);

				}
				if(oldHeader.StatusId != header.StatusId)
				{
					logEntities.Add(Helpers.LogHelper.GetStatusChange(header.Id, oldHeader.Status, header.Status, user));
				}

				if(header.StatusId == (int)Enums.RequestEnums.RequestStatus.Closed)
				{
					Helpers.MailHelper.SendRequestCloseMail(data.Header.Id);
				}
				else
				{
					if(changedPositionsCapital != null && changedPositionsCapital.Count > 0)
						Helpers.MailHelper.SendRequestUpdateMail(data.Header.Id, changedPositionsCapital);
					if(changedPositionsEng != null && changedPositionsEng.Count > 0)
						Helpers.MailHelper.SendRequestUpdateMail(data.Header.Id, changedPositionsEng, true);
				}

				if(response > 0)
					Infrastructure.Data.Access.Tables.CPL.Capital_requests_logAccess.InsertWithTransaction(logEntities, transaction.connection, transaction.transaction);

				if(transaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(data.Header.Id);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error.");
				}

			} catch(Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

	}
}