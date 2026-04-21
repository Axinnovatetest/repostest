using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Assign_User_JointEntity
	{
		public int ID { get; set; }
		public int ID_AssignUser { get; set; }
		public int ID_user { get; set; }

		public Assign_User_JointEntity() { }

		public Assign_User_JointEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_AssignUser = Convert.ToInt32(dataRow["ID_AssignUser"]);
			ID_user = Convert.ToInt32(dataRow["ID_user"]);
		}
	}
}

