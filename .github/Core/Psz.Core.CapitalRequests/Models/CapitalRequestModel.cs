using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CapitalRequests.Models
{
	public class CapitalRequestModel
	{
		public RequestHeaderModel Header { get; set; }
		public IEnumerable<RequestPositionModel> Positions { get; set; }
	}
	public class RequestHeaderModel
	{
		public string Artikelnummer { get; set; }
		public DateTime? CloseDate { get; set; }
		public DateTime? Date { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int Id { get; set; }
		public string Plant { get; set; }
		public int? PlantId { get; set; }
		public string Status { get; set; }
		public int? StatusId { get; set; }
		public int? UserId { get; set; }
		public string? UserName { get; set; }
		public RequestHeaderModel()
		{

		}
		public RequestHeaderModel(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity entity)
		{
			Id = entity.Id;
			Artikelnummer = entity.Artikelnummer;
			Date = entity.Date;
			Fertigungsnummer = entity.Fertigungsnummer;
			Plant = entity.Plant;
			PlantId = entity.PlantId;
			Status = entity.Status;
			StatusId = entity.StatusId;
			UserId = entity.UserId;
			UserName = entity.UserName;
			CloseDate = entity.CloseDate;
		}

		public Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity
			{
				Id = Id,
				Artikelnummer = Artikelnummer,
				Date = Date,
				Fertigungsnummer = Fertigungsnummer,
				Plant = Plant,
				PlantId = PlantId,
				Status = Status,
				StatusId = StatusId,
				CloseDate = CloseDate,
				UserName = UserName,
				UserId = UserId
			};
		}
	}
	public class RequestPositionModel
	{
		public bool? CapitalBOM { get; set; }
		public bool? CapitalClose { get; set; }
		public bool? CapitalCP { get; set; }
		public DateTime? CapitalDate { get; set; }
		public bool? CapitalDOC { get; set; }
		public bool? CapitalFB { get; set; }
		public string CapitalReply { get; set; }
		public bool? CapitalStatus { get; set; }
		public bool? EngeneeringValidation { get; set; }
		public DateTime? EngeneeringValidationDate { get; set; }
		public int? HeaderId { get; set; }
		public int Id { get; set; }
		public int? PositionId { get; set; }
		public string IncidentCategory { get; set; }
		public int? IncidentCategoryId { get; set; }
		public DateTime? IncidentDate { get; set; }
		public string IncidentDescription { get; set; }
		public RequestPositionModel()
		{

		}
		public RequestPositionModel(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity entity)
		{
			Id = entity.Id;
			PositionId = entity.PositionId;
			CapitalBOM = entity.CapitalBOM;
			CapitalClose = entity.CapitalClose;
			CapitalCP = entity.CapitalCP;
			CapitalDate = entity.CapitalDate;
			CapitalDOC = entity.CapitalDOC;
			CapitalFB = entity.CapitalFB;
			CapitalReply = entity.CapitalReply;
			CapitalStatus = entity.CapitalStatus;
			EngeneeringValidation = entity.EngeneeringValidation;
			EngeneeringValidationDate = entity.EngeneeringValidationDate;
			HeaderId = entity.HeaderId;
			IncidentCategory = entity.IncidentCategory;
			IncidentCategoryId = entity.IncidentCategoryId;
			IncidentDate = entity.IncidentDate;
			IncidentDescription = entity.IncidentDescription;
		}
		public Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity
			{
				Id = Id,
				PositionId = PositionId,
				CapitalBOM = CapitalBOM,
				CapitalClose = CapitalClose,
				CapitalCP = CapitalCP,
				CapitalDate = CapitalDate,
				CapitalDOC = CapitalDOC,
				CapitalFB = CapitalFB,
				CapitalReply = CapitalReply,
				CapitalStatus = CapitalStatus,
				EngeneeringValidation = EngeneeringValidation,
				EngeneeringValidationDate = EngeneeringValidationDate,
				HeaderId = HeaderId,
				IncidentCategory = IncidentCategory,
				IncidentCategoryId = IncidentCategoryId,
				IncidentDate = IncidentDate,
				IncidentDescription = IncidentDescription,
			};
		}
	}
}