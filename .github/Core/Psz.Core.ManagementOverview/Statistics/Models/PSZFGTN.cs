using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Statistics.Models
{
	public enum CountryCode
	{
		[Description("GZTN")]
		GZTN,
		[Description("BETN")]
		BETN,
		[Description("KsarHelal")]
		KHTN,
		[Description("TN")]
		TN,
		[Description("AL")]
		AL,
		[Description("")]
		CZ
	}
	public record PSZFGTNRequestModel
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public CountryCode? CountryCode { get; set; }

	}

	public record PSZFGTNResponseModel
	{
		public decimal? Arbeitskosten_PSZ_TN { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Marge_mit_CU { get; set; }
		public decimal? Marge_ohne_CU { get; set; }
		public decimal? Produktionszeit { get; set; }
		public decimal? Produktivitat__FA_Zeit_ { get; set; }
		public decimal? Produktivitat_Artikelzeit_ { get; set; }
		public decimal? Stundensatz { get; set; }
		public decimal? Umsatz_PSZ_TN { get; set; }
		public PSZFGTNResponseModel(Infrastructure.Data.Entities.Tables.Statistics.MGO.PSZFGTNEntity entity)
		{
			if(entity == null)
			{
				return;
			}


			// - 
			Arbeitskosten_PSZ_TN = entity.Arbeitskosten_PSZ_TN;
			Artikelnummer = entity.Artikelnummer;
			Marge_mit_CU = entity.Marge_mit_CU;
			Marge_ohne_CU = entity.Marge_ohne_CU;
			Produktionszeit = (decimal?)entity.Produktionszeit;
			Produktivitat__FA_Zeit_ = entity.Produktivitat__FA_Zeit_;
			Produktivitat_Artikelzeit_ = entity.Produktivitat_Artikelzeit_;
			Stundensatz = entity.Stundensatz;
			Umsatz_PSZ_TN = entity.Umsatz_PSZ_TN;
		}
	}
}
