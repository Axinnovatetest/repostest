using System;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class BestellteArtikelWareneingangEntity
	{
		public int ArtikelNr { get; set; }
		public decimal AktuelleAnzahl { get; set; }
		public int NewWareneingangNr { get; set; }
		public int Lagerort_id { get; set; }
		public DateTime? MhdDatumArtikel { get; set; }
		public decimal StartAnzahl { get; set; }
		public int CurrentBestellteArtikelNr { get; set; }
		public bool erledigt_pos { get; set; }
		public int Position { get; set; }
	}
}
