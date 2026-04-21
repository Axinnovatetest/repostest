using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class __CTS_AngeboteArticleBlanketExtensionEntity
	{
		public int AngeboteArtikelNr { get; set; }
		public string Bezeichnung { get; set; }
		public DateTime? ExtensionDate { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public decimal? GesamtpreisDefault { get; set; }
		public DateTime? GultigAb { get; set; }
		public DateTime? GultigBis { get; set; }
		public int Id { get; set; }
		public string KundenMatNummer { get; set; }
		public string Material { get; set; }
		public int? MaterialNr { get; set; }
		public string ME { get; set; }
		public decimal? Preis { get; set; }
		public decimal? PreisDefault { get; set; }
		public int RahmenNr { get; set; }
		public int? WahrungId { get; set; }
		public string WahrungName { get; set; }
		public string WahrungSymbole { get; set; }
		public decimal? Zielmenge { get; set; }

		public __CTS_AngeboteArticleBlanketExtensionEntity() { }

		public __CTS_AngeboteArticleBlanketExtensionEntity(DataRow dataRow)
		{
			AngeboteArtikelNr = Convert.ToInt32(dataRow["AngeboteArtikelNr"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			ExtensionDate = (dataRow["ExtensionDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ExtensionDate"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			GesamtpreisDefault = (dataRow["GesamtpreisDefault"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["GesamtpreisDefault"]);
			GultigAb = (dataRow["GultigAb"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["GultigAb"]);
			GultigBis = (dataRow["GultigBis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["GultigBis"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			KundenMatNummer = (dataRow["KundenMatNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["KundenMatNummer"]);
			Material = (dataRow["Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material"]);
			MaterialNr = (dataRow["MaterialNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MaterialNr"]);
			ME = (dataRow["ME"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ME"]);
			Preis = (dataRow["Preis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis"]);
			PreisDefault = (dataRow["PreisDefault"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PreisDefault"]);
			RahmenNr = Convert.ToInt32(dataRow["RahmenNr"]);
			WahrungId = (dataRow["WahrungId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WahrungId"]);
			WahrungName = (dataRow["WahrungName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WahrungName"]);
			WahrungSymbole = (dataRow["WahrungSymbole"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WahrungSymbole"]);
			Zielmenge = (dataRow["Zielmenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zielmenge"]);
		}

		public __CTS_AngeboteArticleBlanketExtensionEntity ShallowClone()
		{
			return new __CTS_AngeboteArticleBlanketExtensionEntity
			{
				AngeboteArtikelNr = AngeboteArtikelNr,
				Bezeichnung = Bezeichnung,
				ExtensionDate = ExtensionDate,
				Gesamtpreis = Gesamtpreis,
				GesamtpreisDefault = GesamtpreisDefault,
				GultigAb = GultigAb,
				GultigBis = GultigBis,
				Id = Id,
				KundenMatNummer = KundenMatNummer,
				Material = Material,
				MaterialNr = MaterialNr,
				ME = ME,
				Preis = Preis,
				PreisDefault = PreisDefault,
				RahmenNr = RahmenNr,
				WahrungId = WahrungId,
				WahrungName = WahrungName,
				WahrungSymbole = WahrungSymbole,
				Zielmenge = Zielmenge
			};
		}
	}
}

