using System.Data;
using System;

namespace Infrastructure.Data.Entities.Tables.Support.Request
{
	public class User_RequestEntity
	{
		public string Department { get; set; }
		public string Email { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public int RequestId { get; set; }

		public User_RequestEntity() { }

		public User_RequestEntity(DataRow dataRow)
		{
			Department = Convert.ToString(dataRow["Department"]);
			Email = Convert.ToString(dataRow["Email"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Name = Convert.ToString(dataRow["Name"]);
			Phone = Convert.ToString(dataRow["Phone"]);
			RequestId = Convert.ToInt32(dataRow["RequestId"]);
		}

		public User_RequestEntity ShallowClone()
		{
			return new User_RequestEntity
			{
				Department = Department,
				Email = Email,
				Id = Id,
				Name = Name,
				Phone = Phone,
				RequestId = RequestId
			};
		}
	}
}
