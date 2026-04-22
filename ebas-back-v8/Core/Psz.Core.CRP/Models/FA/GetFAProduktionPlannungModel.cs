using System;

namespace Psz.Core.CRP.Models.FA
{
	public class GetFAProduktionPlannungModel
	{
		public DateTime? Datum_bis { get; set; }
		public int? Produktionsort { get; set; }
		public bool? Technikauftrag { get; set; }
		public string Artikelnummer { get; set; }

		public GetFAProduktionPlannungModel()
		{

		}
	}
}
