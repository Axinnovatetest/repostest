using Infrastructure.Data.Entities.Joins.CRP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models
{
	public class OrderProcessingSearchLogsModel
	{
		public List<OrderProcessingLogsModel>? OrderProcessingLogsList { get; set; } = new List<OrderProcessingLogsModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

	}
	public class OrderProcessingLogsModel
	{
		public OrderProcessingLogsModel(OrderProcessingLogs Entity, int count)
		{
			VorfallNr = Entity.VorfallNr;
			DokNr = Entity.DokNr;
			Pos = Entity.Pos;
			artikelnummer = Entity.artikelnummer;
			User = Entity.User;
			TYP = Entity.TYP;
			Log = Entity.Log;
			totalRows = count;
			DateTime = Entity.DateTime;

		}
		public int? VorfallNr { get; set; }
		public string DokNr { get; set; }
		public int? Pos { get; set; }
		public string artikelnummer { get; set; }
		public string User { get; set; }
		public string TYP { get; set; }
		public string Log { get; set; }
		public int totalRows { get; set; }
		public DateTime? DateTime { get; set; }
	}
	public class OPSearchLogsModel
	{
		public OPSearchLogsModel()
		{

		}
		public string SearchValueVorfallNr { get; set; }
		public string SearchValuePosition { get; set; }
		public string SearchValueartikelnummer { get; set; }
		public string SearchValueUsername { get; set; }
		public List<string> ListSearchType { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
	public class AutoCompliteOP
	{
		public AutoCompliteOP() { }
		public int columnValue { get; set; }
		public string SearchValue { get; set; }
	}
}
