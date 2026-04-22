using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Supplement_Budget_LandEntity
	{
		public int Id { get; set; }
		public int Id_AL { get; set; }
		public double? Supplement_Budget { get; set; }
		public DateTime Creation_Date { get; set; }

		public Supplement_Budget_LandEntity() { }

		public Supplement_Budget_LandEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			Id_AL = Convert.ToInt32(dataRow["Id_AL"]);
			Supplement_Budget = (dataRow["Supplement_Budget"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Supplement_Budget"]);
			Creation_Date = Convert.ToDateTime(dataRow["Creation_Date"]);
		}
	}
}

