using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables._Commun
{
	public class __BSD_Attachements_TrackingEntity
	{
		public int FileId { get; set; }
		public string FileName { get; set; }
		public int ID { get; set; }
		public bool? isActive { get; set; }
		public int? Module { get; set; }
		public int? ModuleId { get; set; }
		public DateTime? UploadedDate { get; set; }
		public int? UserId { get; set; }

		public __BSD_Attachements_TrackingEntity() { }

		public __BSD_Attachements_TrackingEntity(DataRow dataRow)
		{
			FileId = Convert.ToInt32(dataRow["FileId"]);
			FileName = (dataRow["FileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FileName"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			isActive = (dataRow["isActive"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["isActive"]);
			Module = (dataRow["Module"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Module"]);
			ModuleId = (dataRow["ModuleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ModuleId"]);
			UploadedDate = (dataRow["UploadedDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UploadedDate"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
		}

		public __BSD_Attachements_TrackingEntity ShallowClone()
		{
			return new __BSD_Attachements_TrackingEntity
			{
				FileId = FileId,
				FileName = FileName,
				ID = ID,
				isActive = isActive,
				Module = Module,
				ModuleId = ModuleId,
				UploadedDate = UploadedDate,
				UserId = UserId
			};
		}
	}
}
