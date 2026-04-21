using System;

namespace Psz.Core.CRP.Models.FA
{
	public class StappleDruckInputModel
	{
		public string Artikelnummer { get; set; }
		public DateTime? Produktionstrmin { get; set; }
		public int? Produktionsort { get; set; }

		public StappleDruckInputModel()
		{

		}
	}
}
