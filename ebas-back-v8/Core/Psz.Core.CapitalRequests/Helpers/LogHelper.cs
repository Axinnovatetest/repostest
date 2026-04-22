using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Helpers
{
	partial class LogHelper
	{
		public static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity> GetPositionChanges(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> entities,
			List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> Oldentities,
			UserModel user)
		{
			var request = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.Get(entities[0].HeaderId ?? -1);
			var positionChanges = new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity>();
			foreach(var entity in entities)
			{

				var oldEntity = Oldentities.FirstOrDefault(x => x.Id == entity.Id);

				if(oldEntity.CapitalReply != entity.CapitalReply)
					positionChanges.Add(new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity
					{
						Changes = $"Position [{entity.PositionId}] CapitalReply changed from [{oldEntity.CapitalReply}] to [{entity.CapitalReply}]",
						IdPosition = entity.PositionId,
						IdRequest = entity.HeaderId,
						DateTime = DateTime.Now,
						User = user.Name,
						Plant = request.Plant,
						PlantId = request.PlantId,
						Status = request.Status,
						StatusId = request.StatusId,
						Fertigungsnummer = request.Fertigungsnummer,
					});
				if(oldEntity.CapitalStatus != entity.CapitalStatus)
					positionChanges.Add(new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity
					{
						Changes = $"Position [{entity.PositionId}] CapitalStatus changed from [{oldEntity.CapitalStatus}] to [{entity.CapitalStatus}]",
						IdPosition = entity.PositionId,
						IdRequest = entity.HeaderId,
						DateTime = DateTime.Now,
						User = user.Name,
						Plant = request.Plant,
						PlantId = request.PlantId,
						Status = request.Status,
						StatusId = request.StatusId,
						Fertigungsnummer = request.Fertigungsnummer,
					});
				if(oldEntity.CapitalBOM != entity.CapitalBOM)
					positionChanges.Add(new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity
					{
						Changes = $"Position [{entity.PositionId}] CapitalBOM changed from [{oldEntity.CapitalBOM}] to [{entity.CapitalBOM}]",
						IdPosition = entity.PositionId,
						IdRequest = entity.HeaderId,
						DateTime = DateTime.Now,
						User = user.Name,
						Plant = request.Plant,
						PlantId = request.PlantId,
						Status = request.Status,
						StatusId = request.StatusId,
						Fertigungsnummer = request.Fertigungsnummer,
					});
				if(oldEntity.CapitalCP != entity.CapitalCP)
					positionChanges.Add(new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity
					{
						Changes = $"Position [{entity.PositionId}] CapitalCP changed from [{oldEntity.CapitalCP}] to [{entity.CapitalCP}]",
						IdPosition = entity.PositionId,
						IdRequest = entity.HeaderId,
						DateTime = DateTime.Now,
						User = user.Name,
						Plant = request.Plant,
						PlantId = request.PlantId,
						Status = request.Status,
						StatusId = request.StatusId,
						Fertigungsnummer = request.Fertigungsnummer,
					});
				if(oldEntity.CapitalFB != entity.CapitalFB)
					positionChanges.Add(new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity
					{
						Changes = $"Position [{entity.PositionId}] CapitalFB changed from [{oldEntity.CapitalFB}] to [{entity.CapitalFB}]",
						IdPosition = entity.PositionId,
						IdRequest = entity.HeaderId,
						DateTime = DateTime.Now,
						User = user.Name,
						Plant = request.Plant,
						PlantId = request.PlantId,
						Status = request.Status,
						StatusId = request.StatusId,
						Fertigungsnummer = request.Fertigungsnummer,
					});
				if(oldEntity.CapitalDOC != entity.CapitalDOC)
					positionChanges.Add(new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity
					{
						Changes = $"Position [{entity.PositionId}] CapitalDOC changed from [{oldEntity.CapitalDOC}] to [{entity.CapitalDOC}]",
						IdPosition = entity.PositionId,
						IdRequest = entity.HeaderId,
						DateTime = DateTime.Now,
						User = user.Name,
						Plant = request.Plant,
						PlantId = request.PlantId,
						Status = request.Status,
						StatusId = request.StatusId,
						Fertigungsnummer = request.Fertigungsnummer,
					});
				if(oldEntity.CapitalClose != entity.CapitalClose)
					positionChanges.Add(new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity
					{
						Changes = $"Position [{entity.PositionId}] {(entity.CapitalClose.Value ? "Validated" : "Unvalidated")} in Capital",
						IdPosition = entity.PositionId,
						IdRequest = entity.HeaderId,
						DateTime = DateTime.Now,
						User = user.Name,
						Plant = request.Plant,
						PlantId = request.PlantId,
						Status = request.Status,
						StatusId = request.StatusId,
						Fertigungsnummer = request.Fertigungsnummer,
					});
				if(oldEntity.EngeneeringValidation != entity.EngeneeringValidation)
					positionChanges.Add(new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity
					{
						Changes = $"Position [{entity.PositionId}] {(entity.EngeneeringValidation.Value ? "Validated" : "Unvalidated")} in Engeneering",
						IdPosition = entity.PositionId,
						IdRequest = entity.HeaderId,
						DateTime = DateTime.Now,
						User = user.Name,
						Plant = request.Plant,
						PlantId = request.PlantId,
						Status = request.Status,
						StatusId = request.StatusId,
						Fertigungsnummer = request.Fertigungsnummer,
					});
			}
			return positionChanges;
		}
		public static Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity GetStatusChange(int requestId, string oldStatus, string newStatus, UserModel user)
		{
			var request = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.Get(requestId);
			return new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity
			{
				Changes = $"Request Number [{requestId}] Status changed from [{oldStatus}] to [{newStatus}]",
				IdRequest = requestId,
				DateTime = DateTime.Now,
				IdPosition = null,
				User = user.Name,
				Plant = request.Plant,
				PlantId = request.PlantId,
				Status = request.Status,
				StatusId = request.StatusId,
				Fertigungsnummer = request.Fertigungsnummer,
			};
		}
	}
}
