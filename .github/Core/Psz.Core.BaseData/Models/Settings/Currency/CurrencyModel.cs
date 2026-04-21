using System;

namespace Psz.Core.BaseData.Models.Currency
{
	public class CurrencyModel
	{
		public bool? EU { get; set; }//EU
		public string Country { get; set; } // Land
		public int Id { get; set; } // Nr
		public string Symbol { get; set; }//Symbol
		public string Name { get; set; } // Wahrung
		public double? Foreign_currency_amount { get; set; }//Betrag Fremdwährung
		public decimal? corresponds_to_DM { get; set; }//entspricht DM
		public DateTime? Stand { get; set; }//Stand
		public int? Decimal_places { get; set; }//Dezimalstellen

		public CurrencyModel()
		{

		}
		public CurrencyModel(Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity wahrungenEntity)
		{
			Id = wahrungenEntity.Nr;
			EU = wahrungenEntity.EU;
			Country = wahrungenEntity.Land;
			Symbol = wahrungenEntity.Symbol;
			Name = wahrungenEntity.Wahrung;
			Foreign_currency_amount = wahrungenEntity.Betrag_Fremdwahrung;
			corresponds_to_DM = wahrungenEntity.Entspricht_DM;
			Stand = wahrungenEntity.Stand;
			Decimal_places = wahrungenEntity.Dezimalstellen;
		}

		public Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity
			{
				Nr = Id,
				EU = EU,
				Land = Country,
				Symbol = Symbol,
				Wahrung = Name,
				Betrag_Fremdwahrung = Foreign_currency_amount,
				Entspricht_DM = corresponds_to_DM,
				Stand = Stand,
				Dezimalstellen = Decimal_places,
			};
		}
	}
}
