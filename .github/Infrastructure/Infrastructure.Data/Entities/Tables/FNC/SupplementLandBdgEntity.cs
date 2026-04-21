using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class SupplementLandBdgEntity
	{
		public int? B_year { get; set; }
		public double? budget { get; set; }
		public int ID { get; set; }
		public string Land_Name { get; set; }

		public double? SOMME_Supplement_Land_Budget { get; set; }

		public SupplementLandBdgEntity() { }

		public SupplementLandBdgEntity(DataRow dataRow)
		{
			B_year = (dataRow["B_year"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["B_year"]);
			budget = (dataRow["budget"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["budget"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Land_Name = (dataRow["Land_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_Name"]);
			SOMME_Supplement_Land_Budget = (dataRow["SOMME_Supplement_Land_Budget"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["SOMME_Supplement_Land_Budget"]);


		}
		public SupplementLandBdgEntity(int Id, string land, float? landbudget, float? somme_supplement_land_budget, int? year)
		{
			this.ID = Id;
			this.Land_Name = land;
			this.budget = landbudget;
			this.SOMME_Supplement_Land_Budget = somme_supplement_land_budget;
			this.B_year = year;
		}
	}
}
