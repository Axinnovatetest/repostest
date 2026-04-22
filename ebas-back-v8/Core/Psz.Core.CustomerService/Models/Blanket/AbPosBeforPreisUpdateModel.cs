using System;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class AbPosBeforPreisUpdateModel
	{
		public int? Nr { get; set; }
		public int? ABForfallNr { get; set; }
		public int? ABProjektNr { get; set; }
		public decimal? Einzelpreis { get; set; }
		public int? Position { get; set; }
		public DateTime? Liefertermin { get; set; }


		public AbPosBeforPreisUpdateModel()
		{

		}
		public AbPosBeforPreisUpdateModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity Entity)
		{
			if(Entity == null)
				return;
			var abEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(Entity.AngebotNr ?? -1);
			//
			Nr = abEntity?.Nr;
			ABForfallNr = abEntity?.Angebot_Nr;
			ABProjektNr = int.TryParse(abEntity?.Projekt_Nr, out var v) ? v : 0;
			Einzelpreis = Entity.Einzelpreis;
			Position = Entity.Position;
			Liefertermin = Entity.Liefertermin;
		}
	}
}
