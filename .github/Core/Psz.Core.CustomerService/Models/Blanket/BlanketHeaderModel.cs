namespace Psz.Core.CustomerService.Models.Blanket
{
	public class BlanketHeaderModel
	{

		public int Id { get; set; }
		public int AngeboteNr { get; set; }
		public string Auftraggeber { get; set; }
		public string Warenemfanger { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public int? Anhage { get; set; }

		public BlanketHeaderModel()
		{

		}
		public BlanketHeaderModel(Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity extensionEntity)
		{
			if(extensionEntity == null)
				return;
			Id = extensionEntity.Id;
			AngeboteNr = extensionEntity.AngeboteNr;
			Auftraggeber = extensionEntity.Auftraggeber;
			Warenemfanger = extensionEntity.Warenemfanger;
			Gesamtpreis = (decimal)extensionEntity.Gesamtpreis;
			Anhage = extensionEntity.Anhage;
		}
	}
}
