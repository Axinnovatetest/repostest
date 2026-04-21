using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class ArtikelOutilCossEntity
	{
		public int ArtikelNRFG { get; set; }
		public string ArtikelnummerFG { get; set; }
		public int ArtikelNRROH { get; set; }
		public string ArtikelnummerROH { get; set; }
		public string Outil { get; set; }
		public ArtikelOutilCossEntity() { }

		public ArtikelOutilCossEntity(DataRow dataRow)
		{
			ArtikelNRFG = (dataRow["ArtikelNRFG"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ArtikelNRFG"]);
			ArtikelnummerFG = (dataRow["ArtikelnummerFG"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelnummerFG"]);
			ArtikelNRROH = (dataRow["ArtikelNRROH"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ArtikelNRROH"]);
			ArtikelnummerROH = (dataRow["ArtikelnummerROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelnummerROH"]);
			Outil = (dataRow["Outil"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Outil"]);
		}
	}
}

