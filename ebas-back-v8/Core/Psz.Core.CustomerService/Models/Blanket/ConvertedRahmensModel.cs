using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class ConvertedRahmensModel
	{
		public string ArtikelNummer { get; set; }
		public int Artikel_Nr { get; set; }
		public int? RahmenNr { get; set; }
		public int? RahmenAngebotNr { get; set; }
		public string Supplier { get; set; }
		public List<string> Errors { get; set; }
		public ConvertedRahmensModel()
		{

		}
	}
}
