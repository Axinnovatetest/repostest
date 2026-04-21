using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Support.Request
{
	public class SignatureEntity
	{
		public DateTime Date { get; set; }
		public string FirstName { get; set; }
		public string Function { get; set; }
		public int Id { get; set; }
		public string LastName { get; set; }
		public string Signature { get; set; }
		public int RequestId { get; set; }

		public SignatureEntity() { }

		public SignatureEntity(DataRow dataRow)
		{
			Date = Convert.ToDateTime(dataRow["Date"]);
			FirstName = Convert.ToString(dataRow["FirstName"]);
			Function = Convert.ToString(dataRow["Function"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastName = Convert.ToString(dataRow["LastName"]);
			Signature = Convert.ToString(dataRow["Signature"]);
			RequestId = Convert.ToInt32(dataRow["RequestId"]);
		}

		public SignatureEntity ShallowClone()
		{
			return new SignatureEntity
			{
				Date = Date,
				FirstName = FirstName,
				Function = Function,
				Id = Id,
				LastName = LastName,
				Signature = Signature,
				RequestId = RequestId
			};
		}
	}
}
