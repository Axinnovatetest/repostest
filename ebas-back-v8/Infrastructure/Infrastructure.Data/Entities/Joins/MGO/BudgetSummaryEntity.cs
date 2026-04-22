using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class BudgetSummaryEntity
	{
		public string Abteilung { get; set; }
		public string Anfrager { get; set; }
		public decimal Betrag_EUR { get; set; }
		public decimal Betrag_TND { get; set; }
		public int? KW { get; set; }
		public string Lieferant { get; set; }
		public string Monat { get; set; }
		public string PO_Nummer { get; set; }
		public string Site { get; set; }

		public BudgetSummaryEntity() { }

		public BudgetSummaryEntity(DataRow dataRow)
		{
			Abteilung = (dataRow["Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abteilung"]);
			Anfrager = (dataRow["Anfrager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anfrager"]);
			Betrag_EUR = (dataRow["Betrag EUR"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Betrag EUR"]);
			Betrag_TND = (dataRow["Betrag TND"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Betrag TND"]);
			KW = (dataRow["KW"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KW"]);
			Lieferant = (dataRow["Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferant"]);
			Monat = (dataRow["Monat"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Monat"]);
			PO_Nummer = (dataRow["PO Nummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PO Nummer"]);
			Site = (dataRow["Site"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Site"]);
		}

		public BudgetSummaryEntity ShallowClone()
		{
			return new BudgetSummaryEntity
			{
				Abteilung = Abteilung,
				Anfrager = Anfrager,
				Betrag_EUR = Betrag_EUR,
				Betrag_TND = Betrag_TND,
				KW = KW,
				Lieferant = Lieferant,
				Monat = Monat,
				PO_Nummer = PO_Nummer,
				Site = Site
			};
		}
	}
}
