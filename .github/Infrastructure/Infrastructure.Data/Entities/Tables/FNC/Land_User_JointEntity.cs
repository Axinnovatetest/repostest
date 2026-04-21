using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Land_User_JointEntity
	{
		public int ID { get; set; }
		public int ID_land { get; set; }
		public int ID_user { get; set; }

		public Land_User_JointEntity() { }

		public Land_User_JointEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_land = Convert.ToInt32(dataRow["ID_land"]);
			ID_user = Convert.ToInt32(dataRow["ID_user"]);
		}
	}
}

