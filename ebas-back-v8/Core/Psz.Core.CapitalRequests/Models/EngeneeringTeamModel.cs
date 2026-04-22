using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CapitalRequests.Models
{
	public class EngeneeringTeamModel
	{
		public int Id { get; set; }
		public int? UserId { get; set; }
		public string? Username { get; set; }
		public string? Fullname { get; set; }
		public int? PlantId { get; set; }
		public string? Plant { get; set; }
		public int? AddUserId { get; set; }
		public DateTime? AddTime { get; set; }
		public int? UpdateUserId { get; set; }
		public DateTime? UpdateTime { get; set; }
		public string? Email { get; set; }
		public EngeneeringTeamModel()
		{

		}
		public EngeneeringTeamModel(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_engeneering_teamsEntity entity)
		{
			Id = entity.Id;
			UserId = entity.UserId;
			Username = entity.Username;
			PlantId = entity.PlantId;
			Plant = entity.Plant;
			AddUserId = entity.AddUserId;
			AddTime = entity.AddTime;
			UpdateUserId = entity.UpdateUserId;
			UpdateTime = entity.UpdateTime;
		}
		public Infrastructure.Data.Entities.Tables.CPL.Capital_requests_engeneering_teamsEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_engeneering_teamsEntity
			{
				Id = Id,
				UserId = UserId,
				Username = Username,
				PlantId = PlantId,
				Plant = Plant,
				AddUserId = AddUserId,
				AddTime = AddTime,
				UpdateUserId = UpdateUserId,
				UpdateTime = UpdateTime,
			};
		}
	}
}