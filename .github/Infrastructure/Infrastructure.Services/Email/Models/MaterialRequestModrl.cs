using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Email.Models
{
	public class MaterialRequestbaseModel
	{
		public string? MatNr { get; set; }
		public string Bez { get; set; }
		public string Hersteller { get; set; }
		public decimal? Jahresmenge { get; set; }
		public string Quantitymargin { get; set; }
		public string unit { get; set; }
		public bool StckAzhalExists { get; set; } = false;
		public string UnitName { get; set; }
	}

}
