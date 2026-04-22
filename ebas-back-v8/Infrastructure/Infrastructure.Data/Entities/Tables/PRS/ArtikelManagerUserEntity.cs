using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class ArtikelManagerUserEntity
	{
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public int Id { get; set; }
		public string UserFullName { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }

		public ArtikelManagerUserEntity() { }

		public ArtikelManagerUserEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserFullName = (dataRow["UserFullName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserFullName"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
			UserName = (dataRow["UserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserName"]);
		}
	}
}

