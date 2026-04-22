using Infrastructure.Services.Utils;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers
{
	public partial class CapitalRequestsService
	{
		public ResponseModel<int> UnvalidatePositionCapital(UserModel user, int positionId)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var position = Infrastructure.Data.Access.Tables.CPL.Capital_requests_positionsAccess.Get(positionId);
			if(position is null)
			{
				return ResponseModel<int>.FailureResponse($"Position not found.");
			}
			//if(position.CapitalClose == false)
			//{
			//	return ResponseModel<int>.FailureResponse($"Position already open.");
			//}
			if(position.EngeneeringValidation == true)
			{
				return ResponseModel<int>.FailureResponse($"Position already validated by Engineering Team.");
			}
			var header = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.Get(position.HeaderId ?? 0);
			if(header is null)
			{
				return ResponseModel<int>.FailureResponse($"Request not found.");
			}

			var transaction = new TransactionsManager();
			try
			{
				transaction.beginTransaction();
				position.CapitalClose = false;
				position.CapitalDate = DateTime.Now;
				var reeponse = Infrastructure.Data.Access.Tables.CPL.Capital_requests_positionsAccess.UpdateWithTransaction(position, transaction.connection, transaction.transaction);
				Infrastructure.Data.Access.Tables.CPL.Capital_requests_logAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity
				{
					Changes = $"Position [{position.PositionId}] unvalidated in Capital",
					DateTime = DateTime.Now,
					IdPosition = positionId,
					IdRequest = header.Id,
					Plant = header.Plant,
					PlantId = header.PlantId,
					Status = header.Status,
					StatusId = header.StatusId,
					User = user.Name,
					Fertigungsnummer = header.Fertigungsnummer,
				}, transaction.connection, transaction.transaction);
				Helpers.MailHelper.SendRequestUpdateMail(position.HeaderId ?? -1, new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> { position });

				if(transaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(reeponse);
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