namespace Psz.Core.FinanceControl.Models.Supplier
{
	public class MinimalSupplierModel
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int? SupplierNumber { get; set; }
		public int AdressId { get; set; }

		public string Name { get; set; }
		public string AdressText { get; set; }
		public string SuppliersGroup { get; set; }
		public string Industry { get; set; }

		public MinimalSupplierModel()
		{ }
		public MinimalSupplierModel(Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity lieferantenEntity,
			Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenEntity)
		{
			// > Main Ids
			this.Id = lieferantenEntity.Nr;
			this.Number = lieferantenEntity.Nummer ?? -1;
			this.AdressId = -1;
			this.SupplierNumber = -1;

			// > Common
			this.Name = string.Empty;
			this.AdressText = string.Empty;
			this.SuppliersGroup = lieferantenEntity.Lieferantengruppe;
			this.Industry = lieferantenEntity.Branche;
			if(adressenEntity != null)
			{
				this.SupplierNumber = adressenEntity.Lieferantennummer;
				this.AdressId = adressenEntity.Nr;
				this.Name = !string.IsNullOrEmpty(adressenEntity.Name1)
					? adressenEntity.Name1
					: !string.IsNullOrEmpty(adressenEntity.Name2)
						? adressenEntity.Name2
						: adressenEntity.Name3;
				this.AdressText = $"{adressenEntity.StraBe} {adressenEntity.PLZ_StraBe}, {adressenEntity.Ort}, {adressenEntity.Land}".Trim().Trim(',');
			}
		}
	}
}
