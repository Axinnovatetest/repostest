using System;
using System.ComponentModel;
using System.Data;
using System.ComponentModel.DataAnnotations;
namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class FileTrackingOutEntity
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public DateTime? SentTime { get; set; }
		public string MsgType { get; set; }
		public DateTime? EbasGenerateTime { get; set; }
		public string CustomerNumber { get; set; }
		public string CustomerName { get; set; }
		public string DocumentNumber { get; set; }
		public bool? LastCheckStauts { get; set; }
		public DateTime? LastCheckTime { get; set; }
		public FileTrackingOutEntity() { }
		public FileTrackingOutEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			FileName = (dataRow["FileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FileName"]);
			SentTime = (dataRow["SentTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SentTime"]);
			MsgType = (dataRow["MsgType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["MsgType"]);
			EbasGenerateTime = (dataRow["EbasGenerateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EbasGenerateTime"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerNumber"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			DocumentNumber = (dataRow["DocumentNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DocumentNumber"]);
			LastCheckStauts = (dataRow["LastCheckStauts"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LastCheckStauts"]);
			LastCheckTime = (dataRow["LastCheckTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastCheckTime"]);
		}
		public FileTrackingOutEntity ShallowClone()
		{
			return new FileTrackingOutEntity
			{
				Id = Id,
				FileName = FileName,
				SentTime = SentTime,
				MsgType = MsgType,
				EbasGenerateTime = EbasGenerateTime,
				CustomerNumber = CustomerNumber,
				CustomerName = CustomerName,
				DocumentNumber = DocumentNumber,
				LastCheckStauts = LastCheckStauts,
				LastCheckTime = LastCheckTime,
			};
		}
	}
}
