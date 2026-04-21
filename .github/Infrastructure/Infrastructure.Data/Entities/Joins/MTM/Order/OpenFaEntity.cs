using System;


namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class FaultyFertigungCountEntity
	{

		public int? faulty_Fa_Count { get; set; }
		public FaultyFertigungCountEntity() { }
		public FaultyFertigungCountEntity(System.Data.DataRow datarow)
		{

			faulty_Fa_Count = (datarow["faulty_Fa"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["faulty_Fa"]);
		}
	}
	public class FaultyArticlesEntity
	{
		public int Artikel_Nr { get; set; }
		public int TotalCount { get; set; }
		public string Artikelnummer { get; set; }

		public FaultyArticlesEntity(System.Data.DataRow datarow)
		{
			Artikelnummer = (datarow["Artikelnummer"] == System.DBNull.Value) ? "" : datarow["Artikelnummer"].ToString();
			Artikel_Nr = (datarow["Artikel_Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["Artikel_Nr"]);
			TotalCount = (datarow["TotalCount"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["TotalCount"]);
		}
	}



}
