using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CPL
{
	public class Capital_requests_headerEntity
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
		public string UserName { get; set; }

		public Capital_requests_headerEntity() { }

		public Capital_requests_headerEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			CloseDate = (dataRow["CloseDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CloseDate"]);
			Date = (dataRow["Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Date"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Plant = (dataRow["Plant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Plant"]);
			PlantId = (dataRow["PlantId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PlantId"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
			StatusId = (dataRow["StatusId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StatusId"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			UserName = (dataRow["UserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserName"]);
		}

		public Capital_requests_headerEntity ShallowClone()
		{
			return new Capital_requests_headerEntity
			{
				Artikelnummer = Artikelnummer,
				CloseDate = CloseDate,
				Date = Date,
				Fertigungsnummer = Fertigungsnummer,
				Id = Id,
				Plant = Plant,
				PlantId = PlantId,
				Status = Status,
				StatusId = StatusId,
				UserId = UserId,
				UserName = UserName
			};
		}
	}
}

