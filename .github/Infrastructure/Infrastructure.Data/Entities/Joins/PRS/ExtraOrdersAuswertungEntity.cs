using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.Data.Entities.Joins.PRS
{
	public class ExtraOrdersAuswertungEntity
	{
		public string Artikelnummer { get; set; }
		public string Name1 { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal? Stock { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? SumOrders { get; set; }
		public decimal? ValueWithoutRequirement { get; set; }
		public decimal? OtherPlantsRequirement { get; set; }
		public string OtherPlants { get; set; }
		public ExtraOrdersAuswertungEntity()
		{

		}
		public ExtraOrdersAuswertungEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Stock = (dataRow["Stock"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stock"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			SumOrders = (dataRow["SumOrders"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumOrders"]);
			ValueWithoutRequirement = (dataRow["ValueWithoutRequirement"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ValueWithoutRequirement"]);
			OtherPlantsRequirement = (dataRow["OtherPlantsRequirement"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["OtherPlantsRequirement"]);
			OtherPlants = (dataRow["OtherPlants"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OtherPlants"]);
		}
	}
}