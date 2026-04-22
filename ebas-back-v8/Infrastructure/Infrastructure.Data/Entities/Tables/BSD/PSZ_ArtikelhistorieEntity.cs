using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class PSZ_ArtikelhistorieEntity
	{
		public string Anderung_von { get; set; }
		public string Anderungsbereich { get; set; }
		public string Anderungsbeschreibung { get; set; }
		public int? Artikel_Nr { get; set; }
		public DateTime? Datum_Anderung { get; set; }
		public int ID { get; set; }

		public PSZ_ArtikelhistorieEntity() { }

		public PSZ_ArtikelhistorieEntity(DataRow dataRow)
		{
			Anderung_von = (dataRow["Änderung von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Änderung von"]);
			Anderungsbereich = (dataRow["Änderungsbereich"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Änderungsbereich"]);
			Anderungsbeschreibung = (dataRow["Änderungsbeschreibung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Änderungsbeschreibung"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Datum_Anderung = (dataRow["Datum Änderung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum Änderung"]);
			ID = Convert.ToInt32(dataRow["ID"]);
		}

		public PSZ_ArtikelhistorieEntity ShallowClone()
		{
			return new PSZ_ArtikelhistorieEntity
			{
				Anderung_von = Anderung_von,
				Anderungsbereich = Anderungsbereich,
				Anderungsbeschreibung = Anderungsbeschreibung,
				Artikel_Nr = Artikel_Nr,
				Datum_Anderung = Datum_Anderung,
				ID = ID
			};
		}
	}
}

