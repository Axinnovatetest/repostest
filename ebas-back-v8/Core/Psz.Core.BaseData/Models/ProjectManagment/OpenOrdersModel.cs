using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.ProjectManagment
{
	public class OpenOrdersModel
	{
		public int FAId { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Status { get; set; }
		public string State { get; set; }
		public DateTime? FertigungDate { get; set; }
		public OpenOrdersModel()
		{

		}
	}
}