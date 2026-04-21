using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.MGO
{
	public class DayOff
	{
		public DateTime? Date { get; set; }
		public string Name { get; set; }
		public bool? BW { get; set; }
		public bool? AllStates { get; set; }
		public bool? HB { get; set; }
		public bool? BB { get; set; }
		public bool? BE { get; set; }
		public bool? BY { get; set; }
		public bool? NW { get; set; }
		public bool? NI { get; set; }
		public bool? MV { get; set; }
		public bool? HH { get; set; }
		public bool? HE { get; set; }
		public bool? RP { get; set; }
		public bool? TH { get; set; }
		public bool? SH { get; set; }
		public bool? ST { get; set; }
		public bool? SN { get; set; }
		public bool? SL { get; set; }
		public short? KW { get; set; }
		public DayOff() { }
		public DayOff(DataRow dataRow)
		{
			Date = (dataRow["Date"] == DBNull.Value) ? null : Convert.ToDateTime(dataRow["Date"]);
			Name = (dataRow["Name"] == DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			BY = (dataRow["BY"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["BY"]);
			BE = (dataRow["BE"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["BE"]);
			BW = (dataRow["BW"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["BW"]);
			BB = (dataRow["BB"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["BB"]);
			HB = (dataRow["HB"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["HB"]);
			HH = (dataRow["HH"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["HH"]);
			BW = (dataRow["BW"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["BW"]);
			BW = (dataRow["BW"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["BW"]);
			HE = (dataRow["HE"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["HE"]);
			AllStates = (dataRow["AllStates"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AllStates"]);
			NI = (dataRow["NI"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["NI"]);
			NW = (dataRow["NW"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["NW"]);
			RP = (dataRow["RP"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RP"]);
			MV = (dataRow["MV"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MV"]);
			SL = (dataRow["SL"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SL"]);
			SN = (dataRow["SN"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SN"]);
			ST = (dataRow["ST"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ST"]);
			SH = (dataRow["SH"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SH"]);
			TH = (dataRow["TH"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["TH"]);
			KW = (dataRow["KW"] == DBNull.Value) ? (short?)null : Convert.ToInt16(dataRow["KW"]);
		}
	}
}
