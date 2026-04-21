//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;

//namespace Infrastructure.Data.Entities.Tables.FNC
//{
//    public class Currency_BudgetEntity
//    {
//		public string Currency { get; set; }
//		public int IdC { get; set; }
//		public string Symol { get; set; }

//        public Currency_BudgetEntity() { }

//        public Currency_BudgetEntity(DataRow dataRow)
//        {
//			Currency = Convert.ToString(dataRow["Currency"]);
//			IdC = Convert.ToInt32(dataRow["IdC"]);
//			Symol = Convert.ToString(dataRow["Symol"]);
//        }
//    }
//}



using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Currency_BudgetEntity
	{
		public string Currency { get; set; }
		public int IdC { get; set; }
		public string Symol { get; set; }

		public Currency_BudgetEntity() { }

		public Currency_BudgetEntity(DataRow dataRow)
		{
			Currency = Convert.ToString(dataRow["Currency"]);
			IdC = Convert.ToInt32(dataRow["IdC"]);
			Symol = Convert.ToString(dataRow["Symol"]);
		}
	}
}

