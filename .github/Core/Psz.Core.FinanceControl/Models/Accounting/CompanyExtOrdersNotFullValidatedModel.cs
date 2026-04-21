using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Models.Accounting
{
	public class CompanyExtOrdersNotFullValidatedModel
	{
		public int Count { get; set; }
		public int ValidationLevel { get; set; }
		public string CompanyName { get; set; }
		public string Username { get; set; }

		public CompanyExtOrdersNotFullValidatedModel(Infrastructure.Data.Entities.Joins.FNC.Accounting.CompanyExtOrdersNotFullValidatedEntity Entity)
		{
			Count = Entity.Count;
			ValidationLevel = Entity.ValidationLevel;
			CompanyName = Entity.CompanyName;
			Username = Entity.Username;
		}
	}
}
