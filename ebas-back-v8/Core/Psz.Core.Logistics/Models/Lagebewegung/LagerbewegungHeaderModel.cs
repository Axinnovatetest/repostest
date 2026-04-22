using System;

namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class LagerbewegungHeaderModel
	{
		public long id { get; set; }
		public string typ { get; set; }
		public DateTime? datum { get; set; }
		public bool gebucht { get; set; }
		public string gebuchtVon { get; set; }
		public LagerbewegungHeaderModel()
		{

		}
		public LagerbewegungHeaderModel(Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity lagerbewegungHeaderEntity)
		{
			if(lagerbewegungHeaderEntity == null)
				return;
			this.id = lagerbewegungHeaderEntity.id;
			this.typ = lagerbewegungHeaderEntity.typ;
			this.datum = lagerbewegungHeaderEntity.datum;
			this.gebucht = lagerbewegungHeaderEntity.gebucht;
			this.gebuchtVon = lagerbewegungHeaderEntity.gebuchtVon;
		}
	}
}
