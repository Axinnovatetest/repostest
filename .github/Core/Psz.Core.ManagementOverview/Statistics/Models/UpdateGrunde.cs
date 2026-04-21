using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Statistics.Models
{
	public record UpdateGrundeRequestModel
	{
		public int Id { get; set; }
		public int IdGrunde { get; set; }
		public int? IdOldGrunde { get; set; }
		public int ArtikelNr { get; set; }

	}


}
