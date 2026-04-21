using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class PlantBookingsTicketLogsEntity
	{
		public string artikelnummer { get; set; }
		public DateTime? CreationDate { get; set; }
		public int Id { get; set; }
		public int? LagerId { get; set; }
		public int? ticketscount { get; set; }
		public string Userfullname { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }
		public int? verpackungnr { get; set; }

		public PlantBookingsTicketLogsEntity() { }
		public PlantBookingsTicketLogsEntity(string verpackungnrstring)
		{
			try
			{
				verpackungnr = int.Parse(verpackungnrstring);
			} catch(Exception)
			{

			}
		}

		public PlantBookingsTicketLogsEntity(DataRow dataRow)
		{
			artikelnummer = (dataRow["artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["artikelnummer"]);
			CreationDate = (dataRow["CreationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LagerId = (dataRow["LagerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerId"]);
			ticketscount = (dataRow["ticketscount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ticketscount"]);
			Userfullname = (dataRow["Userfullname"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Userfullname"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
			verpackungnr = (dataRow["verpackungnr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["verpackungnr"]);
		}

		public PlantBookingsTicketLogsEntity ShallowClone()
		{
			return new PlantBookingsTicketLogsEntity
			{
				artikelnummer = artikelnummer,
				CreationDate = CreationDate,
				Id = Id,
				LagerId = LagerId,
				ticketscount = ticketscount,
				Userfullname = Userfullname,
				UserId = UserId,
				Username = Username,
				verpackungnr = verpackungnr
			};
		}
	}
}
