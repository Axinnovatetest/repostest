using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_GenericResult
	{
		public PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_GenericResult()
		{

		}
		public List<PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II_Model> List_Details_II { get; set; }
		public List<PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III_Model> List_Details_III { get; set; }
		public List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_IV> List_Details_IV { get; set; }
		public string Lieferent_Details_III { get; set; }
		public decimal EK { get; set; }
		public string Bestell_Nr { get; set; }
		public decimal MOQ { get; set; }
		public int std_Lief_Zeit { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public decimal Bestant_List_Details_IV { get; set; }

	}
}
