using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CapitalRequests.Models
{
	public class CapitalRequestsLogsRequestModel
	{
		public int? RequestId { get; set; } = null;
		public int? PlantId { get; set; } = null;
		public string? SearchTerms { get; set; } = null;
	}
	public class CapitalRequestsLogsResponseModel
	{
		public string Changes { get; set; }
		public DateTime? DateTime { get; set; }
		public int Id { get; set; }
		public int? IdPosition { get; set; }
		public int? IdRequest { get; set; }
		public string Plant { get; set; }
		public int? PlantId { get; set; }
		public string Status { get; set; }
		public int? StatusId { get; set; }
		public string User { get; set; }
		public int? Fertigungsnummer { get; set; }
		public CapitalRequestsLogsResponseModel()
		{

		}
		public CapitalRequestsLogsResponseModel(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_logEntity entity)
		{
			Id = entity.Id;
			IdRequest = entity.IdRequest ?? -1;
			User = entity.User;
			Changes = entity.Changes;
			DateTime = entity.DateTime;
			IdPosition = entity.IdPosition;
			IdRequest = entity.IdRequest;
			Plant = entity.Plant;
			PlantId = entity.PlantId;
			Status = entity.Status;
			StatusId = entity.StatusId;
			Fertigungsnummer = entity.Fertigungsnummer;
		}
	}
}