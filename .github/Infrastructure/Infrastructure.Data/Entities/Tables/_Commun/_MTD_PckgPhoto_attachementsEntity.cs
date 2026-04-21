using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables._Commun
{
	public class _MTD_PckgPhoto_attachementsEntity
	{
		public int? ArticleId { get; set; }
		public int FileId { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public bool? isActive { get; set; }
		public DateTime? UploadedDate { get; set; }
		public int? UserId { get; set; }

		public _MTD_PckgPhoto_attachementsEntity() { }

		public _MTD_PckgPhoto_attachementsEntity(DataRow dataRow)
		{
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			FileId = Convert.ToInt32(dataRow["FileId"]);
			FileName = Convert.ToString(dataRow["FileName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			isActive = (dataRow["isActive"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["isActive"]);
			UploadedDate = (dataRow["UploadedDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UploadedDate"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
		}

		public _MTD_PckgPhoto_attachementsEntity ShallowClone()
		{
			return new _MTD_PckgPhoto_attachementsEntity
			{
				ArticleId = ArticleId,
				FileId = FileId,
				FileName = FileName,
				Id = Id,
				isActive = isActive,
				UploadedDate = UploadedDate,
				UserId = UserId
			};
		}
	}
}
