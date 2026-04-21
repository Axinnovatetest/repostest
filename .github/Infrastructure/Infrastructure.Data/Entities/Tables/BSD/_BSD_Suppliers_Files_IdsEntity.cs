using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class _BSD_Suppliers_Files_IdsEntity
	{
		public int FileId { get; set; }
		public string FileName { get; set; }
		public int ID { get; set; }
		public bool? isActive { get; set; }
		public int? SupplierId { get; set; }
		public DateTime? UploadedDate { get; set; }
		public int? UserId { get; set; }

		public _BSD_Suppliers_Files_IdsEntity() { }

		public _BSD_Suppliers_Files_IdsEntity(DataRow dataRow)
		{
			FileId = Convert.ToInt32(dataRow["FileId"]);
			FileName = (dataRow["FileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FileName"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			isActive = (dataRow["isActive"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["isActive"]);
			SupplierId = (dataRow["SupplierId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierId"]);
			UploadedDate = (dataRow["UploadedDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UploadedDate"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
		}

		public _BSD_Suppliers_Files_IdsEntity ShallowClone()
		{
			return new _BSD_Suppliers_Files_IdsEntity
			{
				FileId = FileId,
				FileName = FileName,
				ID = ID,
				isActive = isActive,
				SupplierId = SupplierId,
				UploadedDate = UploadedDate,
				UserId = UserId
			};
		}
	}
}
