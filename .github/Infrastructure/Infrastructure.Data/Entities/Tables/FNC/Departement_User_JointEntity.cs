using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Departement_User_JointEntity
	{
		public int ID { get; set; }
		public int ID_Departement { get; set; }
		public int ID_user { get; set; }

		public Departement_User_JointEntity() { }

		public Departement_User_JointEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_Departement = Convert.ToInt32(dataRow["ID_Departement"]);
			ID_user = Convert.ToInt32(dataRow["ID_user"]);


		}
	}
}

