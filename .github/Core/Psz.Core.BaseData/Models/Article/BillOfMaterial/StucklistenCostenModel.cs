using System;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class StucklistenCostenModel
	{
		public double? Materiel_cost { get; set; }
		public double? DB { get; set; }
		public double? Prozent { get; set; }

		public StucklistenCostenModel()
		{

		}
		public StucklistenCostenModel(Infrastructure.Data.Entities.Tables.BSD.Stucklisten_costenEntity calculEntity)
		{
			if(calculEntity != null)
			{
				Materiel_cost = calculEntity.Somme_Material.HasValue ? Math.Round(calculEntity.Somme_Material.Value, 2) : 0;
				DB = (calculEntity.VK_PSZ.HasValue && calculEntity.Kalkulatorischekosten.HasValue && calculEntity.Somme_Material.HasValue) ?
				  Math.Round(calculEntity.VK_PSZ.Value - (calculEntity.Kalkulatorischekosten.Value + calculEntity.Somme_Material.Value), 2) : 0;
				Prozent = (calculEntity.Kalkulatorischekosten.HasValue && calculEntity.Kalkulatorischekosten.Value > 0 && calculEntity.Somme_Material.HasValue && calculEntity.Somme_Material.Value > 0) ?
				  Math.Round(Convert.ToDouble((calculEntity.VK_PSZ - (calculEntity.Kalkulatorischekosten + calculEntity.Somme_Material)) / (calculEntity.Kalkulatorischekosten + calculEntity.Somme_Material) * 100), 2) : 0;
			}
		}
	}
}
