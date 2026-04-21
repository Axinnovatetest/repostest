using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.FNC.MinIO
{
	public class PSZ_FileServer_RetryDataEntity
	{
		public DateTime? AddedOn { get; set; }
		public int? ErrorLevel { get; set; }
		public string Exception { get; set; }
		public string FileExtension { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public int? UserId { get; set; }

		public PSZ_FileServer_RetryDataEntity() { }

		public PSZ_FileServer_RetryDataEntity(DataRow dataRow)
		{
			AddedOn = (dataRow["AddedOn"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AddedOn"]);
			ErrorLevel = (dataRow["ErrorLevel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ErrorLevel"]);
			Exception = (dataRow["Exception"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Exception"]);
			FileExtension = (dataRow["FileExtension"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FileExtension"]);
			FileName = (dataRow["FileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FileName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
		}

		public PSZ_FileServer_RetryDataEntity ShallowClone()
		{
			return new PSZ_FileServer_RetryDataEntity
			{
				AddedOn = AddedOn,
				ErrorLevel = ErrorLevel,
				Exception = Exception,
				FileExtension = FileExtension,
				FileName = FileName,
				Id = Id,
				UserId = UserId
			};
		}
	}
	public class PSZ_FileServer_RetryDataEntity2: PSZ_FileServer_RetryDataEntity
	{
		public byte[] FileBytes { get; set; }
		public PSZ_FileServer_RetryDataEntity2(DataRow dataRow) : base(dataRow)
		{

		}
		public PSZ_FileServer_RetryDataEntity2()
		{

		}
	}
	public class PSZ_FileServer_RetryDataEntity3: PSZ_FileServer_RetryDataEntity
	{
		public int? TotalCount { get; set; }
		public PSZ_FileServer_RetryDataEntity3(DataRow dataRow) : base(dataRow)
		{
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCount"]);
		}
		public PSZ_FileServer_RetryDataEntity3()
		{

		}
	}
	public class PSZ_FileServer_RetryDataEntity4
	{
		public int? TotalCount { get; set; }
		public PSZ_FileServer_RetryDataEntity4(DataRow dataRow)
		{
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCount"]);
		}

	}
}
