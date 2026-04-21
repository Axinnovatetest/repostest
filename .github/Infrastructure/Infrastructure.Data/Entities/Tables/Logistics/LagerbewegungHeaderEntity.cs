using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class LagerbewegungHeaderEntity
	{
		public long id { get; set; }
		public string typ { get; set; }
		public DateTime? datum { get; set; }
		public bool gebucht { get; set; }
		public string gebuchtVon { get; set; }
		public LagerbewegungHeaderEntity()
		{

		}
		public LagerbewegungHeaderEntity(DataRow dr)
		{
			id = (dr["ID"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["ID"]);
			typ = (dr["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Typ"]);
			datum = (dr["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Datum"]);
			gebucht = (dr["gebucht"] == System.DBNull.Value) ? false : Convert.ToBoolean(dr["gebucht"]);
			gebuchtVon = (dr["Gebucht von"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Gebucht von"]);


		}
	}
	public class LagerbewegungHeaderFormatEntity
	{
		public long id { get; set; }
		public DateTime? datum { get; set; }
		public string CountryFrom { get; set; }
		public string CountryTo { get; set; }
		public int? LogUserId { get; set; }
		public LagerbewegungHeaderFormatEntity()
		{

		}
		public LagerbewegungHeaderFormatEntity(DataRow dr)
		{
			id = (dr["ID"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["ID"]);
			datum = (dr["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Datum"]);
			CountryFrom = (dr["CountryFrom"] == System.DBNull.Value) ? "": Convert.ToString(dr["CountryFrom"]);
			CountryTo = (dr["CountryTo"] == System.DBNull.Value) ? "": Convert.ToString(dr["CountryTo"]);
			LogUserId = (dr["LogUserId"] == System.DBNull.Value) ? null : Convert.ToInt32(dr["LogUserId"]);
		}
	}
	public class LagerbewegungHeaderFormatExtendedEntity
	{
		public long id { get; set; }
		public DateTime? datum { get; set; }
		public string CountryFrom { get; set; }
		public string CountryTo { get; set; }
		public int? LogUserId { get; set; }
		public int? LagerId { get; set; }
		public LagerbewegungHeaderFormatExtendedEntity()
		{

		}
		public LagerbewegungHeaderFormatExtendedEntity(DataRow dr)
		{
			id = (dr["ID"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["ID"]);
			datum = (dr["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Datum"]);
			CountryFrom = (dr["CountryFrom"] == System.DBNull.Value) ? "" : Convert.ToString(dr["CountryFrom"]);
			CountryTo = (dr["CountryTo"] == System.DBNull.Value) ? "" : Convert.ToString(dr["CountryTo"]);
			LogUserId = (dr["LogUserId"] == System.DBNull.Value) ? null : Convert.ToInt32(dr["LogUserId"]);
			LagerId = (dr["LagerId"] == System.DBNull.Value) ? null : Convert.ToInt32(dr["LagerId"]);
		}
	}
}
