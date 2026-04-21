using System;
using System.Data;


namespace Infrastructure.Data.Entities.Joins.MTM.Order.Statistics
{
	public class BestandProWerkohneBedarfEntity
	{
		public string Artikelnummer { get; set; }
		public double Bestand { get; set; }
		public double BedarfTN { get; set; }
		public double BedarfBETN { get; set; }
		public double BedarfWS { get; set; }
		public double BedarfAL { get; set; }
		public double BedarfDE { get; set; }
		public double BedarfGZTN { get; set; }
		public BestandProWerkohneBedarfEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Artikelnummer"].ToString();
			BedarfTN = Math.Round((dataRow["BedarfTN"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["BedarfTN"].ToString()), 4);
			Bestand = Math.Round((dataRow["Bestand"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["Bestand"].ToString()), 4);
			BedarfBETN = Math.Round((dataRow["BedarfBETN"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["BedarfBETN"].ToString()), 4);
			BedarfWS = Math.Round((dataRow["BedarfWS"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["BedarfWS"].ToString()), 4);
			BedarfAL = Math.Round((dataRow["BedarfAL"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["BedarfAL"].ToString()), 4);
			BedarfDE = Math.Round((dataRow["BedarfDE"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["BedarfDE"].ToString()), 4);
			BedarfGZTN = Math.Round((dataRow["BedarfGZTN"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["BedarfGZTN"].ToString()), 4);
		}
	}
}
