using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables._Commun
{
	public class __BSD_Attachements_Tracking_LogEntity
	{
		public int FileId { get; set; }
		public string FileName { get; set; }
		public int ID { get; set; }
		public int? Module { get; set; }
		public int? ModuleId { get; set; }
		public string Operation { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UserId { get; set; }

		public __BSD_Attachements_Tracking_LogEntity() { }

		public __BSD_Attachements_Tracking_LogEntity(DataRow dataRow)
		{
			FileId = Convert.ToInt32(dataRow["FileId"]);
			FileName = dataRow["FileName"] == DBNull.Value ? "" : Convert.ToString(dataRow["FileName"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Module = dataRow["Module"] == DBNull.Value ? null : Convert.ToInt32(dataRow["Module"]);
			ModuleId = dataRow["ModuleId"] == DBNull.Value ? null : Convert.ToInt32(dataRow["ModuleId"]);
			Operation = dataRow["Operation"] == DBNull.Value ? "" : Convert.ToString(dataRow["Operation"]);
			UpdateTime = dataRow["UpdateTime"] == DBNull.Value ? null : Convert.ToDateTime(dataRow["UpdateTime"]);
			UserId = dataRow["UserId"] == DBNull.Value ? null : Convert.ToInt32(dataRow["UserId"]);
		}

		public __BSD_Attachements_Tracking_LogEntity ShallowClone()
		{
			return new __BSD_Attachements_Tracking_LogEntity
			{
				FileId = FileId,
				FileName = FileName,
				ID = ID,
				Module = Module,
				ModuleId = ModuleId,
				Operation = Operation,
				UpdateTime = UpdateTime,
				UserId = UserId
			};
		}
	}
}
