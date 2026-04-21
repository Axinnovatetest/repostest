namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class AngeboteArticleBlanketExtensionEntity
	{
		public int Id { get; set; }
		public int AngeboteArtikelNr { get; set; }
		public int RahmenNr { get; set; }
		public string Material { get; set; }
		public decimal? Zielmenge { get; set; }
		public string Bezeichnung { get; set; }
		public decimal? Preis { get; set; }
		public string ME { get; set; }
		public DateTime? GultigAb { get; set; }
		public DateTime? GultigBis { get; set; }
		public string KundenMatNummer { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public int? MaterialNr { get; set; }
		public string WahrungName { get; set; }
		public string WahrungSymbole { get; set; }
		public int? WahrungId { get; set; }
		public DateTime? ExtensionDate { get; set; }
		public decimal? PreisDefault { get; set; }
		public decimal? GesamtpreisDefault { get; set; }
		public decimal? BasePrice { get; set; }
		public DateTime? AckDate { get; set; }
		public string ReasonNewPosition { get; set; }
		public string Comment { get; set; }
		public string AB_nummer { get; set; }
		public AngeboteArticleBlanketExtensionEntity() { }
		public AngeboteArticleBlanketExtensionEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			AngeboteArtikelNr = Convert.ToInt32(dataRow["AngeboteArtikelNr"]);
			RahmenNr = Convert.ToInt32(dataRow["RahmenNr"]);
			Material = (dataRow["Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material"]);
			Zielmenge = (dataRow["Zielmenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zielmenge"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			Preis = (dataRow["Preis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis"]);
			ME = (dataRow["ME"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ME"]);
			GultigAb = (dataRow["GultigAb"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["GultigAb"]);
			GultigBis = (dataRow["GultigBis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["GultigBis"]);
			KundenMatNummer = (dataRow["KundenMatNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["KundenMatNummer"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			MaterialNr = (dataRow["MaterialNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MaterialNr"]);
			WahrungName = (dataRow["WahrungName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WahrungName"]);
			WahrungSymbole = (dataRow["WahrungSymbole"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WahrungSymbole"]);
			WahrungId = (dataRow["WahrungId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WahrungId"]);
			ExtensionDate = (dataRow["ExtensionDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ExtensionDate"]);
			PreisDefault = (dataRow["PreisDefault"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PreisDefault"]);
			GesamtpreisDefault = (dataRow["GesamtpreisDefault"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["GesamtpreisDefault"]);
			BasePrice = (dataRow["BasePrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["BasePrice"]);
			AckDate = (dataRow["AckDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AckDate"]);
			ReasonNewPosition = (dataRow["ReasonNewPosition"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ReasonNewPosition"]);
			Comment = (dataRow["Comment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Comment"]);
			AB_nummer = (dataRow["AB_nummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AB_nummer"]);
		}
		public AngeboteArticleBlanketExtensionEntity ShallowClone()
		{
			return new AngeboteArticleBlanketExtensionEntity
			{
				Id = Id,
				AngeboteArtikelNr = AngeboteArtikelNr,
				RahmenNr = RahmenNr,
				Material = Material,
				Zielmenge = Zielmenge,
				Bezeichnung = Bezeichnung,
				Preis = Preis,
				ME = ME,
				GultigAb = GultigAb,
				GultigBis = GultigBis,
				KundenMatNummer = KundenMatNummer,
				Gesamtpreis = Gesamtpreis,
				MaterialNr = MaterialNr,
				WahrungName = WahrungName,
				WahrungSymbole = WahrungSymbole,
				WahrungId = WahrungId,
				ExtensionDate = ExtensionDate,
				PreisDefault = PreisDefault,
				GesamtpreisDefault = GesamtpreisDefault,
				BasePrice = BasePrice,
				AckDate = AckDate,
				ReasonNewPosition = ReasonNewPosition,
				Comment = Comment,
				AB_nummer = AB_nummer,
			};
		}
	}
}
