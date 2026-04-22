using System;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class AbLinkBlanketModel
	{
		public int? Nr { get; set; }
		public string ProjektNr { get; set; }
		public int? Vorfallnr { get; set; }
		public DateTime? Falligkeit { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public int? NrRA { get; set; }
		public string Beleg { get; set; }
		public DateTime? Am { get; set; }
		public bool? Status { get; set; }

		public AbLinkBlanketModel()
		{

		}
		public AbLinkBlanketModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity Entity)
		{
			Nr = Entity.Nr;
			ProjektNr = Entity.Projekt_Nr;
			Vorfallnr = Entity.Angebot_Nr;
			Falligkeit = Entity.Falligkeit;
			Name = Entity.Vorname_NameFirma;
			Type = Entity.Typ;
			NrRA = Entity.Nr_RA;
			Beleg = Entity.Bezug;
			Am = Entity.Datum;
			Status = Entity.Erledigt;

		}

		public AbLinkBlanketModel(Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity Entity)
		{
			Nr = Entity.Nr;
			ProjektNr = Entity.Projekt_Nr;
			Vorfallnr = Entity.Bestellung_Nr;
			Falligkeit = Entity.Liefertermin;
			Name = Entity.Vorname_NameFirma;
			Type = Entity.Typ;
			Beleg = Entity.Bezug;
			Am = Entity.Datum;
			Status = Entity.Offnen;
		}

	}
}
