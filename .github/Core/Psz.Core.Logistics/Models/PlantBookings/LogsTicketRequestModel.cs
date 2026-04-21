using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Logistics.Models.ControlProcedure;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Psz.Core.Logistics.Models.PlantBookings
{
	public class LogsTicketRequestModel: IPaginatedRequestModel
	{
		public string SearchValue { get; set; }

	}
	public class LogsTicketResponseModel
	{
		public int? UserId { get; set; }
		public string? UserName { get; set; }

		public string? Userfullname { get; set; }
		public DateTime? CreationDate { get; set; }
		public int? LagerId { get; set; }
		public string? artikelnummer { get; set; }
		public int? ticketscount { get; set; }
		public int? verpackungnr { get; set; }
		public LogsTicketResponseModel(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity __plantBookingsTicketLogsEntity)
		{
			if(__plantBookingsTicketLogsEntity == null)
				return;

			UserId = __plantBookingsTicketLogsEntity.UserId;
			UserName = __plantBookingsTicketLogsEntity.Username;
			Userfullname = __plantBookingsTicketLogsEntity.Userfullname;
			CreationDate = __plantBookingsTicketLogsEntity.CreationDate;
			LagerId = __plantBookingsTicketLogsEntity.LagerId;
			artikelnummer = __plantBookingsTicketLogsEntity.artikelnummer;
			ticketscount = __plantBookingsTicketLogsEntity.ticketscount;
			verpackungnr = __plantBookingsTicketLogsEntity.verpackungnr;
		}
	}
	
}
public class GetLogsTicketResponseModel: IPaginatedResponseModel<LogsTicketResponseModel>
{
}
