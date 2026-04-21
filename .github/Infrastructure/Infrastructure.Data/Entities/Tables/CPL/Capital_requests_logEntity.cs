using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CPL
{
	public class Capital_requests_logEntity
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
		public Capital_requests_logEntity() { }

		public Capital_requests_logEntity(DataRow dataRow)
		{
			Changes = (dataRow["Changes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Changes"]);
			DateTime = (dataRow["DateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DateTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdPosition = (dataRow["IdPosition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IdPosition"]);
			IdRequest = (dataRow["IdRequest"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IdRequest"]);
			Plant = (dataRow["Plant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Plant"]);
			PlantId = (dataRow["PlantId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PlantId"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
			StatusId = (dataRow["StatusId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StatusId"]);
			User = (dataRow["User"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["User"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
		}

		public Capital_requests_logEntity ShallowClone()
		{
			return new Capital_requests_logEntity
			{
				Changes = Changes,
				DateTime = DateTime,
				Id = Id,
				IdPosition = IdPosition,
				IdRequest = IdRequest,
				Plant = Plant,
				PlantId = PlantId,
				Status = Status,
				StatusId = StatusId,
				User = User,
				Fertigungsnummer = Fertigungsnummer,
			};
		}
	}
}