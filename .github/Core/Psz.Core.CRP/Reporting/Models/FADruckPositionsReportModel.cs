using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Reporting.Models
{
	public class FADruckPositionsReportModel
	{
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public string Anzahl { get; set; }
		public string Arbeitsanweisung { get; set; }
		public string Fertiger { get; set; }
		public string Termin_Soll { get; set; }
		public string Bemerkungen { get; set; }
		public string Lagerort { get; set; }
		public bool ESD_Schutz { get; set; }
		public string img { get; set; }


		public FADruckPositionsReportModel(Infrastructure.Data.Entities.Joins.FADruck.FAReport1PositionsEntity positionsEntity)
		{
			Artikelnummer = positionsEntity.Artikelnummer;
			Bezeichnung_1 = positionsEntity.Bezeichnung_1;
			Bezeichnung_2 = positionsEntity.Bezeichnung_2;
			Anzahl = positionsEntity.Anzahl.HasValue ? positionsEntity.Anzahl.Value.ToString("F2") : "";
			Arbeitsanweisung = positionsEntity.Arbeitsanweisung;
			Fertiger = positionsEntity.Fertiger;
			Termin_Soll = positionsEntity.Termin_Soll.HasValue ? (positionsEntity.Termin_Soll.Value.ToString("dd/MM/yyyy")) : "";
			Bemerkungen = positionsEntity.Bemerkungen;
			Lagerort = positionsEntity.Lagerort;
			ESD_Schutz = positionsEntity.ESD_Schutz ?? false;
		}
	}
}
