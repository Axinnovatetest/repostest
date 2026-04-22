using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Layout.Font;

namespace Psz.Core.Logistics.Models.PlantBookings
{
	public class PlantBookingUpdateModel
	{
		public int? Aktiv { get; set; }
		public decimal? Akzeptierte_Menge { get; set; }
		public string? Artikelnummer { get; set; }
		public int? Clock_Number { get; set; }
		public decimal? Geprufte_Prufmenge { get; set; }
		public decimal? Gesamtmenge { get; set; }
		public string? Inspektor { get; set; }
		public string? Kunde { get; set; }
		public int? Laufende_Nummer { get; set; }
		public decimal? Menge { get; set; }
		public bool? Prufentscheid { get; set; }
		public decimal? Prufmenge { get; set; }
		public decimal? Pruftiefe { get; set; }
		public decimal? Reklamierte_Menge { get; set; }
		public string? Resultat { get; set; }
		public int? Verpackungsnr { get; set; }
		public int? LagerortID { get; set; }
		public decimal? WE_Anzahl_VOH { get; set; }
		public int? Eingangslieferscheinnr { get; set; }

		public int? LagerbewegungenId { get; set; }
		public bool? Choice { get; set; }
		public bool? ChoiceTransfer { get; set; }
	}
}
