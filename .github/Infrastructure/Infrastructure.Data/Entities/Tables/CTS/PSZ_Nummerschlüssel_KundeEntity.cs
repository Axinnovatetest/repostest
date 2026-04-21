using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class PSZ_Nummerschlüssel_KundeEntity
	{
		public bool? Analyse { get; set; }
		public string AnalyseName { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public string CreationUserName { get; set; }
		public int? CS_ID { get; set; }
		public string CS_Kontakt { get; set; }
		public int ID { get; set; }
		public string Kunde { get; set; }
		public int? Kundennummer { get; set; }
		public int? LastEditUserId { get; set; }
		public string LastEditUserName { get; set; }
		public string Nummerschlüssel { get; set; }
		public string Projektbetreuer_D { get; set; }
		public string Stufe { get; set; }
		public string Technik_Kontakt { get; set; }
		public string Technik_Kontakt_TN { get; set; }

		public PSZ_Nummerschlüssel_KundeEntity() { }

		public PSZ_Nummerschlüssel_KundeEntity(DataRow dataRow)
		{
			Analyse = (dataRow["Analyse"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Analyse"]);
			AnalyseName = (dataRow["AnalyseName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AnalyseName"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			CreationUserName = (dataRow["CreationUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreationUserName"]);
			CS_ID = (dataRow["CS ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CS ID"]);
			CS_Kontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
			Kundennummer = (dataRow["Kundennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kundennummer"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			LastEditUserName = (dataRow["LastEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastEditUserName"]);
			Nummerschlüssel = (dataRow["Nummerschlüssel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nummerschlüssel"]);
			Projektbetreuer_D = (dataRow["Projektbetreuer D"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projektbetreuer D"]);
			Stufe = (dataRow["Stufe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Stufe"]);
			Technik_Kontakt = (dataRow["Technik Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Technik Kontakt"]);
			Technik_Kontakt_TN = (dataRow["Technik Kontakt TN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Technik Kontakt TN"]);
		}

		public PSZ_Nummerschlüssel_KundeEntity ShallowClone()
		{
			return new PSZ_Nummerschlüssel_KundeEntity
			{
				Analyse = Analyse,
				AnalyseName = AnalyseName,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				CreationUserName = CreationUserName,
				CS_ID = CS_ID,
				CS_Kontakt = CS_Kontakt,
				ID = ID,
				Kunde = Kunde,
				Kundennummer = Kundennummer,
				LastEditUserId = LastEditUserId,
				LastEditUserName = LastEditUserName,
				Nummerschlüssel = Nummerschlüssel,
				Projektbetreuer_D = Projektbetreuer_D,
				Stufe = Stufe,
				Technik_Kontakt = Technik_Kontakt,
				Technik_Kontakt_TN = Technik_Kontakt_TN
			};
		}
	}
}

