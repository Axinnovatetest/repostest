using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class CustomerUserEntity
	{
		public int CustomerNumber { get; set; }
		public int Id { get; set; }
		public bool IsPrimary { get; set; }
		public int UserId { get; set; }
		public DateTime ValidIntoTime { get; set; }
		public DateTime ValidFromTime { get; set; }

		public CustomerUserEntity() { }
		public CustomerUserEntity(DataRow dataRow)
		{
			CustomerNumber = Convert.ToInt32(dataRow["CustomerId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsPrimary = Convert.ToBoolean(dataRow["IsPrimary"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
			ValidIntoTime = Convert.ToDateTime(dataRow["ValidIntoTime"]);
			ValidFromTime = Convert.ToDateTime(dataRow["ValidFromTime"]);
		}
	}
}

