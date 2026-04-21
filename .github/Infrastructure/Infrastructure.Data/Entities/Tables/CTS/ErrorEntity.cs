using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class ErrorEntity
	{
		public string BuyerDuns { get; set; }
		public string Documentnumber { get; set; }
		public string ErrorMessage { get; set; }
		public string ErrorTrace { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public DateTime ProcessTime { get; set; }
		public string RecipientId { get; set; }
		public string SenderId { get; set; }
		public bool? Validated { get; set; }
		public DateTime? ValidationTime { get; set; }
		public int? ValidationUserId { get; set; }

		public ErrorEntity() { }

		public ErrorEntity(DataRow dataRow)
		{
			BuyerDuns = (dataRow["BuyerDuns"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BuyerDuns"]);
			Documentnumber = Convert.ToString(dataRow["Documentnumber"]);
			ErrorMessage = (dataRow["ErrorMessage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ErrorMessage"]);
			ErrorTrace = (dataRow["ErrorTrace"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ErrorTrace"]);
			FileName = Convert.ToString(dataRow["FileName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ProcessTime = Convert.ToDateTime(dataRow["ProcessTime"]);
			RecipientId = Convert.ToString(dataRow["RecipientId"]);
			SenderId = Convert.ToString(dataRow["SenderId"]);
			Validated = (dataRow["Validated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Validated"]);
			ValidationTime = (dataRow["ValidationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ValidationTime"]);
			ValidationUserId = (dataRow["ValidationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ValidationUserId"]);
		}

		public ErrorEntity ShallowClone()
		{
			return new ErrorEntity
			{
				BuyerDuns = BuyerDuns,
				Documentnumber = Documentnumber,
				ErrorMessage = ErrorMessage,
				ErrorTrace = ErrorTrace,
				FileName = FileName,
				Id = Id,
				ProcessTime = ProcessTime,
				RecipientId = RecipientId,
				SenderId = SenderId,
				Validated = Validated,
				ValidationTime = ValidationTime,
				ValidationUserId = ValidationUserId
			};
		}
	}
}

