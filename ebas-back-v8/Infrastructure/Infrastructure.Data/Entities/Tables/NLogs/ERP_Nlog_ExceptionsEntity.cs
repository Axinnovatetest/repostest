using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.NLogs
{
	public class ERP_Nlog_ExceptionsEntity
	{
		public int Id { get; set; }
		public DateTime? Date { get; set; }
		public string Level { get; set; }
		public int EventId { get; set; }
		public string MemberName { get; set; }
		public string SourceFilePath { get; set; }
		public int? SourceLineNumber { get; set; }
		public string Message { get; set; }
		public string Body { get; set; }
		public bool? IsCall { get; set; }
		public ERP_Nlog_ExceptionsEntity()
		{

		}
		public ERP_Nlog_ExceptionsEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			Date = (dataRow["Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Date"]);
			Level = (dataRow["Level"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Level"]);
			EventId = Convert.ToInt32(dataRow["Id"]);
			MemberName = (dataRow["MemberName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["MemberName"]);
			SourceFilePath = (dataRow["SourceFilePath"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["SourceFilePath"]);
			SourceLineNumber = (dataRow["SourceLineNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SourceLineNumber"]);
			Message = (dataRow["Message"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Message"]);
			Body = (dataRow["Body"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Body"]);
			IsCall = (dataRow["IsCall"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsCall"]);
		}
	}
}