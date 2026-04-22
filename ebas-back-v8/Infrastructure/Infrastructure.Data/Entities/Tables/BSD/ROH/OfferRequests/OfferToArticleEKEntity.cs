using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests
{
	public class OfferToArticleEKEntity
	{
		public int? ArtikelNr { get; set; }
		public int? BestellnummernNr { get; set; }
		public int Id { get; set; }
		public DateTime? LastUpdate { get; set; }
		public int? OfferId { get; set; }
		public string RequestUI { get; set; }
		public int? SupplierId { get; set; }

		public OfferToArticleEKEntity() { }

		public OfferToArticleEKEntity(DataRow dataRow)
		{
			ArtikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArtikelNr"]);
			BestellnummernNr = (dataRow["BestellnummernNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BestellnummernNr"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastUpdate = (dataRow["LastUpdate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdate"]);
			OfferId = (dataRow["OfferId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OfferId"]);
			RequestUI = (dataRow["RequestUI"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RequestUI"]);
			SupplierId = (dataRow["SupplierId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierId"]);
		}

		public OfferToArticleEKEntity ShallowClone()
		{
			return new OfferToArticleEKEntity
			{
				ArtikelNr = ArtikelNr,
				BestellnummernNr = BestellnummernNr,
				Id = Id,
				LastUpdate = LastUpdate,
				OfferId = OfferId,
				RequestUI = RequestUI,
				SupplierId = SupplierId
			};
		}
	}
}
