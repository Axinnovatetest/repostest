using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests
{
	public class OfferRequestEmailAttachmentsEntity
	{
		public int? FileId { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public int? OfferId { get; set; }

		public OfferRequestEmailAttachmentsEntity() { }

		public OfferRequestEmailAttachmentsEntity(DataRow dataRow)
		{
			FileId = (dataRow["FileId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FileId"]);
			FileName = (dataRow["FileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FileName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			OfferId = (dataRow["OfferId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OfferId"]);
		}

		public OfferRequestEmailAttachmentsEntity ShallowClone()
		{
			return new OfferRequestEmailAttachmentsEntity
			{
				FileId = FileId,
				FileName = FileName,
				Id = Id,
				OfferId = OfferId
			};
		}
	}
}
