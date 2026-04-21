using System;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class BestellungenLinkBlanketModel
	{
		public int? OrderNumber { get; set; }
		public string ProjectNumber { get; set; }
		public string Type { get; set; }
		public string CompanyName { get; set; }
		public DateTime? Date { get; set; }
		public string Relation { get; set; }
		public string Condition { get; set; }
		public string User { get; set; }
		public string Status { get; set; }
		public int? Nr { get; set; }

		public BestellungenLinkBlanketModel()
		{

		}

		public BestellungenLinkBlanketModel(Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity bestellungenEntity)
		{
			if(bestellungenEntity == null)
			{
				return;
			}
			var editorName = "";
			if(bestellungenEntity.Bearbeiter.HasValue)
				editorName = Infrastructure.Data.Access.Tables.MTM.PSZ_BearbeiterAccess.GetByNummer(bestellungenEntity.Bearbeiter.Value)?.Name;
			var countPlacements = Infrastructure.Data.Access.Tables.PRS.OrderPlacementHistoryAccess.GetCountByOrderId(bestellungenEntity.Nr);
			//- 

			//Get relation text 
			var relationId = 0;
			var relation = "";

			if(int.TryParse(bestellungenEntity.Bezug, out relationId))
			{
				var betreffEntities = Infrastructure.Data.Access.Tables.MTM.BetreffAccess.Get(relationId);
				relation = betreffEntities.Betreff;
			}
			var status = "";
			if(bestellungenEntity.gebucht.HasValue && bestellungenEntity.gebucht.Value)
				status = "Validated";
			if(!bestellungenEntity.gebucht.HasValue || !bestellungenEntity.gebucht.Value)
				status = "Draft";
			if(countPlacements > 0)
				status = "Placed";

			Relation = relation;
			Type = bestellungenEntity.Rahmenbestellung == true ? "Rahmenbestellung" : bestellungenEntity.Typ;
			OrderNumber = bestellungenEntity.Bestellung_Nr;
			ProjectNumber = bestellungenEntity.Projekt_Nr;
			CompanyName = bestellungenEntity.Vorname_NameFirma;
			Date = bestellungenEntity.Datum;
			Condition = bestellungenEntity.Konditionen;
			User = $"{bestellungenEntity.Bearbeiter} || {editorName}";
			Status = status;
			Nr = bestellungenEntity.Nr;
		}
	}
}
