using Infrastructure.Services.Utils;
using Psz.Core.CapitalRequests.Models;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers
{
	public partial class CapitalRequestsService
	{
		public ResponseModel<int> ValidateAddRequest(UserModel user, CapitalRequestModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			if(data.Header.Fertigungsnummer is null)
				return ResponseModel<int>.FailureResponse("FA nummer is required.");
			if(data.Header.PlantId is null)
				return ResponseModel<int>.FailureResponse("Plant is required.");
			if(data.Positions == null || data.Positions.Count() == 0)
				return ResponseModel<int>.FailureResponse("No positions to add.");
			var werke = Infrastructure.Data.Access.Joins.CapitalRequestsJointsAccess.GetWerkeId(user.CompanyId);
			var companyLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetByWerke(werke);
			var companyLagerIds = companyLager?.Select(x => x.Lagerort_id).ToList();
			var fa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(data.Header.Fertigungsnummer ?? -1);
			if(!companyLagerIds.Contains(fa.Lagerort_id ?? -1))
				return ResponseModel<int>.FailureResponse("Order is not in user company.");
			return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> AddRequest(UserModel user, CapitalRequestModel data)
		{
			var transaction = new TransactionsManager();
			try
			{
				var validationResponse = ValidateAddRequest(user, data);
				if(!validationResponse.Success)
					return validationResponse;
				lock(Locks.Locks.TicketLock)
				{
					transaction.beginTransaction();
					var entity = data.Header.ToEntity();
					var company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(user.CompanyId);
					entity.Status = Enums.RequestEnums.RequestStatus.Open.GetDescription();
					entity.StatusId = (int)Enums.RequestEnums.RequestStatus.Open;
					entity.Date = DateTime.Now;
					entity.UserId = user.Id;
					entity.UserName = user.Name;
					entity.PlantId = company.Id;
					entity.Plant = company.Name;
					var insertedId = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.InsertWithTransaction(entity,
						transaction.connection, transaction.transaction);
					var i = 1;
					data.Positions.ToList().ForEach(p =>
					{
						p.IncidentDate = DateTime.Now;
						p.HeaderId = insertedId;
						p.IncidentCategory = ((Enums.RequestEnums.RequestCategories)p.IncidentCategoryId).GetDescription();
						p.PositionId = i;
						i++;
					});

					var response = Infrastructure.Data.Access.Tables.CPL.Capital_requests_positionsAccess.InsertWithTransaction(data.Positions.Select(p => p.ToEntity()).ToList(),
						transaction.connection, transaction.transaction);
					Infrastructure.Data.Access.Tables.CPL.Capital_requests_logAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity
					{
						Changes = $"New request made with number [{insertedId}]",
						DateTime = DateTime.Now,
						IdRequest = insertedId,
						Status = Enums.RequestEnums.RequestStatus.Open.GetDescription(),
						StatusId = (int)Enums.RequestEnums.RequestStatus.Open,
						PlantId = company.Id,
						Plant = company.Name,
						User = user.Name,
						Fertigungsnummer = entity.Fertigungsnummer
					}, transaction.connection, transaction.transaction);

					if(!transaction.commit())
						return ResponseModel<int>.FailureResponse("Error addig request.");
					Helpers.MailHelper.SendNewRequestMail(insertedId);
					return ResponseModel<int>.SuccessResponse(insertedId);
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