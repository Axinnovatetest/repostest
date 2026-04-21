using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class CTS_StatsRG_DocFooterModel
	{
		public string Footer1 { get; set; }
		public string Footer2 { get; set; }
		public string Footer3 { get; set; }
		public string Footer4 { get; set; }
		public string Footer5 { get; set; }
		public string Footer6 { get; set; }
		public string Footer7 { get; set; }
		public string Footer8 { get; set; }
		public string Footer9 { get; set; }
		public string Footer10 { get; set; }
		public string Footer11 { get; set; }
		public string Footer12 { get; set; }
		public string Footer13 { get; set; }
		public string Footer14 { get; set; }
		public string Footer15 { get; set; }
		public string Footer16 { get; set; }
		public string Footer17 { get; set; }
		public int Lager { get; set; }
		public CTS_StatsRG_DocFooterModel()
		{

		}
		public CTS_StatsRG_DocFooterModel(Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity entity, int lager)
		{
			Footer1 = entity.Footer1;
			Footer2 = entity.Footer2;
			Footer3 = entity.Footer3;
			Footer4 = entity.Footer4;
			Footer5 = entity.Footer5;
			Footer6 = entity.Footer6;
			Footer7 = entity.Footer7;
			Footer8 = entity.Footer8;
			Footer9 = entity.Footer9;
			Footer10 = entity.Footer10;
			Footer11 = entity.Footer11;
			Footer12 = entity.Footer12;
			Footer13 = entity.Footer13;
			Footer14 = entity.Footer14;
			Footer15 = entity.Footer15;
			Footer16 = entity.Footer16;
			Footer17 = entity.Footer17;
			Lager = lager;
		}
	}
}
