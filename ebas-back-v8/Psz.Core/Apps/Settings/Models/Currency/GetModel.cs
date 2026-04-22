using System;

namespace Psz.Core.Apps.Settings.Models.Currency
{
	public class GetModel
	{
		public double? Unit { get; set; }
		public int? DecimalPartLength { get; set; }
		public double? Rate { get; set; }
		public bool? EU { get; set; }
		public string Country { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public string Symbol { get; set; }
		public string Name { get; set; }

		public GetModel()
		{

		}
		public GetModel(Infrastructure.Data.Entities.Tables.STG.WahrungenEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			Unit = entity?.Betrag_Fremdwahrung;
			DecimalPartLength = entity?.Dezimalstellen;
			Rate = entity?.entspricht_DM;
			EU = entity?.EU;
			Country = entity?.Land;
			Id = entity?.Nr ?? -1;
			LastEditTime = entity?.Stand;
			Symbol = entity?.Symbol;
			Name = entity?.Wahrung;
		}

		public Infrastructure.Data.Entities.Tables.STG.WahrungenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity
			{
				Betrag_Fremdwahrung = Unit,
				Dezimalstellen = DecimalPartLength,
				entspricht_DM = Rate,
				EU = EU,
				Land = Country?.Trim(),
				Nr = Id,
				Stand = LastEditTime,
				Symbol = Symbol?.Trim(),
				Wahrung = Name?.Trim()
			};
		}
	}
}
