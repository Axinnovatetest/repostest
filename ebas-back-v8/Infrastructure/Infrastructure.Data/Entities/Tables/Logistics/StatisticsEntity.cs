using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class LagerbewegungFertigungEntity
	{
		public int Fertigungsnummer { get; set; }
		public int FertigungId { get; set; }
		public int FertigungPositionArticleId { get; set; }
		public decimal FertigungPositionArticleQuantity { get; set; }
		public decimal FertigungPositionArticleUnitPrice { get; set; }
		public string FertigungPositionArticleWarengruppe { get; set; }
		public bool FertigungPositionArticleUBG { get; set; }
		public LagerbewegungFertigungEntity()
		{

		}
		public LagerbewegungFertigungEntity(DataRow dr)
		{
			Fertigungsnummer = Convert.ToInt32(dr["Fertigungsnummer"]);
			FertigungId = Convert.ToInt32(dr["FertigungId"]);
			FertigungPositionArticleId = Convert.ToInt32(dr["FertigungPositionArticleId"]);
			FertigungPositionArticleQuantity = (dr["FertigungPositionArticleQuantity"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["FertigungPositionArticleQuantity"]);
			FertigungPositionArticleUnitPrice = (dr["FertigungPositionArticleUnitPrice"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["FertigungPositionArticleUnitPrice"]);
			FertigungPositionArticleWarengruppe = (dr["FertigungPositionArticleWarengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dr["FertigungPositionArticleWarengruppe"]);
			FertigungPositionArticleUBG = (dr["FertigungPositionArticleUBG"] == System.DBNull.Value) ? false : Convert.ToBoolean(dr["FertigungPositionArticleUBG"]);
		}
	}
}
