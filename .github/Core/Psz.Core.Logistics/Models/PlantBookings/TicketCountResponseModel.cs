using System.Data;
using Psz.Core.BaseData.Models.Article.Packaging;
using static Psz.Core.BaseData.Enums.BomChangeEnums;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class TicketCountResponseModel
	{
		public int? ticketscount { get; set; }
		//public DateTime? CreationDate { get; set; }
		//public DateTime? TicektCountTime { get; set; }
		//public int? UserId { get; set; }
		public int? lagerID { get; set; }
		//public string? Username { get; set; }
		//public string? Artikelnummer { get; set; }
		public TicketCountResponseModel()
		{
		}
		public TicketCountResponseModel(Infrastructure.Data.Entities.Tables.Logistics.TicketCountEntity ticketcountentity)
		{
			if(ticketcountentity == null)
				return;
			ticketscount = ticketcountentity.ticketscount;
			//CreationDate = ticketcountentity.CreationDate;
			//TicektCountTime = ticketcountentity.TicektCountTime;
			//UserId = ticketcountentity.UserId;
			lagerID = ticketcountentity.lagerID;
			//Username = ticketcountentity.Username;
			//Artikelnummer = ticketcountentity.Artikelnummer;
		}
	}
}


