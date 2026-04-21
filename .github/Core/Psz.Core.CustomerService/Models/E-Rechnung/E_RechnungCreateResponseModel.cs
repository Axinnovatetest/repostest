using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.E_Rechnung
{
	public class E_RechnungCreated
	{
		public int Nr { get; set; }
		public int ForfallNr { get; set; }
		public int? RechnungProjectNr { get; set; }
		public int? LSForfallNr { get; set; }
		public int? LSNr { get; set; }
		public string Type { get; set; }
		public string Customer { get; set; }
		public int CustomerNr { get; set; }
		public decimal? Betreg { get; set; }
		public bool? Validated { get; set; }
		public DateTime? SentDate { get; set; }
		public List<SammelList> SammelList { get; set; }
		public DateTime? CreationDate { get; set; }

	}

	public class SammelList
	{
		public int RechnungProjectNr { get; set; }
		public int LSForfallNr { get; set; }
		public int LSNr { get; set; }

	}

	public class E_RechnungCreatedResponseModel
	{
		public List<E_RechnungCreated> Created { get; set; }
		public bool ForceCreationAllowed { get; set; }
		public string Message { get; set; }
	}
}
