using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.PRS
{
	public class StockWarningsNeedsInOtherPlantsEntity
	{
		public string Lager { get; set; }
		public decimal? Needed { get; set; }
		public decimal? Bedarf { get; set; }
		public decimal? OpenOrders { get; set; }
		public StockWarningsNeedsInOtherPlantsEntity()
		{

		}
		public StockWarningsNeedsInOtherPlantsEntity(DataRow dataRow)
		{
			Lager = dataRow["Unit"] == DBNull.Value ? null : Convert.ToString(dataRow["Unit"]);
			Needed = dataRow["Needed"] == DBNull.Value ? null : Convert.ToDecimal(dataRow["Needed"]);
			Bedarf = dataRow["Bedarf"] == DBNull.Value ? null : Convert.ToDecimal(dataRow["Bedarf"]);
			OpenOrders = dataRow["OpenOrders"] == DBNull.Value ? null : Convert.ToDecimal(dataRow["OpenOrders"]);

		}
	}

	public class ExtraOrdersNeedsInOtherPlantsEntity
	{
		public int Bestellung_Nr { get; set; }
		public int Nr { get; set; }
		public string Lieferant { get; set; }
		public decimal? Anzahl { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public ExtraOrdersNeedsInOtherPlantsEntity()
		{

		}
		public ExtraOrdersNeedsInOtherPlantsEntity(DataRow dataRow)
		{
			Bestellung_Nr = Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Lieferant = dataRow["Lieferant"] == DBNull.Value ? null : Convert.ToString(dataRow["Lieferant"]);
			Anzahl = dataRow["Anzahl"] == DBNull.Value ? null : Convert.ToDecimal(dataRow["Anzahl"]);
			Lagerort_id = dataRow["Lagerort_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Wunschtermin = dataRow["Wünschtermin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Wünschtermin"]);
			Bestatigter_Termin = dataRow["Bestätigter_Termin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
		}
	}
}
