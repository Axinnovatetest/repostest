using System;

namespace Psz.Core.CustomerService.Models.E_Rechnung
{
	public class ERechnungReprintModel
	{
		public int? RechnungNr { get; set; }
		public int? RechnungForfallNr { get; set; }
		public DateTime? CreationTime { get; set; }
		public string CustomerName { get; set; }
		public string adress { get; set; }
		public string Bezug { get; set; }
		public string KundenType { get; set; }

		public ERechnungReprintModel(Infrastructure.Data.Entities.Joins.CTS.RechnungReprintEntity entity)
		{
			RechnungNr = entity.RechnungNr;
			RechnungForfallNr = entity.RechnungForfallNr;
			CreationTime = entity.CreationTime;
			CustomerName = entity.CustomerName;
			adress = entity.adress;
			Bezug = entity.Bezug;
			KundenType = entity.KundenType;
		}
	}
}
