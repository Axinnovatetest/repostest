namespace Psz.Core.Logistics.Models.Lagebewegung.Fertigung
{
	public class FALagerModel
	{
		public long id { get; set; }
		public long fertigungsnummer { get; set; }
		public string artikelnummer { get; set; }
		public string externStatus { get; set; }
		public int anzahl { get; set; }

		public FALagerModel()
		{

		}

		public FALagerModel(Infrastructure.Data.Entities.Tables.Logistics.FALagerEntity FAEntity)
		{

			if(FAEntity != null)
			{
				fertigungsnummer = FAEntity.fertigungsnummer;
				artikelnummer = FAEntity.artikelnummer;
				externStatus = FAEntity.externStatus;
				anzahl = FAEntity.anzahl;
				id = FAEntity.id;

			}
		}
	}
}
