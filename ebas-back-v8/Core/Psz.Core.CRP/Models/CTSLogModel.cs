using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models
{
	public class CTSLogModel
	{
		public int? AngebotNr { get; set; }
		public DateTime? DateTime { get; set; }
		public string LogText { get; set; }
		public string Origin { get; set; }
		public int? ProjektNr { get; set; }
		public string Username { get; set; }
		public string LogObject { get; set; }
		public CTSLogModel()
		{

		}
		public CTSLogModel(Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity entity)
		{
			AngebotNr = entity.AngebotNr;
			DateTime = entity.DateTime;
			LogText = entity.LogText;
			Origin = entity.Origin;
			ProjektNr = entity.ProjektNr;
			Username = entity.Username;
			LogObject = entity.LogObject;
		}
	}
}