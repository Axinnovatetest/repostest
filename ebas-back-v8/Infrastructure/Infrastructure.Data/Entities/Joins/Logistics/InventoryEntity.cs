using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class WorkAreaEntity
	{
		public int LagerId { get; set; }
		public decimal? Percent { get; set; }
		public string Step { get; set; }
		public int? StepOrder { get; set; }

		public WorkAreaEntity() { }

		public WorkAreaEntity(DataRow dataRow)
		{
			LagerId = Convert.ToInt32(dataRow["LagerId"]);
			Percent = (dataRow["Percent"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Percent"]);
			Step = (dataRow["Step"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Step"]);
			StepOrder = (dataRow["StepOrder"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StepOrder"]);
		}

		public WorkAreaEntity ShallowClone()
		{
			return new WorkAreaEntity
			{
				LagerId = LagerId,
				Percent = Percent,
				Step = Step,
				StepOrder = StepOrder
			};
		}
	}
	public class InventoryDifferenceEntity
	{
		public string Artikelnummer { get; set; }
		public decimal BestandL { get; set; }
		public decimal Einkaufspreis { get; set; }
		public int? Jahr { get; set; }
		public decimal Menge { get; set; }

		public InventoryDifferenceEntity() { }

		public InventoryDifferenceEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			BestandL =  Convert.ToDecimal(dataRow["BestandL"]);
			Einkaufspreis = Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Jahr = (dataRow["Jahr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Jahr"]);
			Menge = Convert.ToDecimal(dataRow["Menge"]);
		}

		public InventoryDifferenceEntity ShallowClone()
		{
			return new InventoryDifferenceEntity
			{
				Artikelnummer = Artikelnummer,
				BestandL = BestandL,
				Einkaufspreis = Einkaufspreis,
				Jahr = Jahr,
				Menge = Menge
			};
		}
	}
	public class InventoryDifferenceSumEntity
	{
		public string Warengruppe { get; set; }
		public decimal ScannedValue { get; set; }
		public decimal StockValue { get; set; }

		public InventoryDifferenceSumEntity() { }

		public InventoryDifferenceSumEntity(DataRow dataRow)
		{
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
			ScannedValue = Convert.ToDecimal(dataRow["ScannedValue"]);
			StockValue = Convert.ToDecimal(dataRow["StockValue"]);
		}
	}
	public class RohInProdEntity
	{
		public int? SpuleId { get; set; }
		public decimal? Menge { get; set; }
		public string Artikelnummer { get; set; }
		public string SpuleStatus { get; set; }

		public RohInProdEntity() { }

		public RohInProdEntity(DataRow dataRow)
		{
			SpuleId = (dataRow["SpuleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SpuleId"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Menge"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			SpuleStatus = (dataRow["SpuleStatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SpuleStatus"]);
		}
	}

	public class RohSurplusProdEntity
	{
		public string Artikelnummer { get; set; }
		public decimal? MengeInProduktion { get; set; }
		public decimal? GefundeneMengeInProduktion { get; set; }
		public decimal? BedarfFa { get; set; }

		public RohSurplusProdEntity() { }

		public RohSurplusProdEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			MengeInProduktion = (dataRow["MengeInProduktion"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["MengeInProduktion"]);
			GefundeneMengeInProduktion = (dataRow["GefundeneMengeInProduktion"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["GefundeneMengeInProduktion"]);
			BedarfFa = (dataRow["BedarfFa"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["BedarfFa"]);
		}
	}
	public class RohArticlePricesEntity
	{
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public int? InventoryYear { get; set; }
		public decimal Price { get; set; }

		public RohArticlePricesEntity() { }

		public RohArticlePricesEntity(DataRow dataRow)
		{
			ArticleId = Convert.ToInt32(dataRow["ArticleId"]);
			ArticleNumber = Convert.ToString(dataRow["ArticleNumber"]);
			InventoryYear = (dataRow["InventoryYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["InventoryYear"]);
			Price = Convert.ToDecimal(dataRow["Price"]);
		}
	}
}