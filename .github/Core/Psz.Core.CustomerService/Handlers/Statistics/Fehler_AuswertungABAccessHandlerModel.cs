using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Layout.Font;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class Fehler_AuswertungABAccessHandlerModel
	{
		public string Kunde { get; set; }
		public string DokumenetenNr { get; set; }
		public int Position { get; set; }
		public int VorfallNr { get; set; }
		public int ArtikelNr { get; set; }
		public string Bezeichnung1 { get; set; }
		public int Mengeoffen { get; set; }
		public DateTime? liefertermin { get; set; }
		public int Auslieferlager { get; set; }
		public Fehler_AuswertungABAccessHandlerModel()
		{

		}
		//public Fehler_AuswertungABAccessHandlerModel(Infrastructure.Data.Entities.Joins.CTS.Fehler_AuswertungABEntity data)
		//{
		//	Kunde = data.Kunde;
		//	DokumenetenNr = data.DokumenetenNr;
		//	Position = data.Position ;
		//	VorfallNr = data.VorfallNr ;
		//	ArtikelNr = data.ArtikelNr ;
		//	Bezeichnung1 = data.Bezeichnung1;
		//	Mengeoffen = data.Mengeoffen ;
		//	liefertermin = data.liefertermin;
		//	Auslieferlager = data.Auslieferlager ;

		//}
	}
}
