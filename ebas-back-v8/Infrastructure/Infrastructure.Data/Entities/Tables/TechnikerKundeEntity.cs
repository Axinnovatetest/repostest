using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.Data.Entities.Tables
{
	public class TechnikerKundeEntity
	{
		public TechnikerKundeEntity(DataRow dataRow)
		{

			Id = (dataRow["Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Nr"]);
			TechnikerKundeName = (dataRow["Ansprechpartner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ansprechpartner"]);

		}
		public int Id { get; set; }
		public string TechnikerKundeName { get; set; }
	}
}
