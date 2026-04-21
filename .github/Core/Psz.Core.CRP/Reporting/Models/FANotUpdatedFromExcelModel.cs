using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Reporting.Models
{
	public class FANotUpdatedFromExcelModel
	{
		public int Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Produktionstermin { get; set; }
		public string Kennzeich { get; set; }
		public int Id { get; set; }
		// - 2023-02-17 
		public string Comments { get; set; }
		public FANotUpdatedFromExcelModel()
		{

		}
		public FANotUpdatedFromExcelModel(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity FAEntity, string comments)
		{
			Comments = comments;
			if(FAEntity == null)
			{
				return;
			}
			var ArtikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)FAEntity.Artikel_Nr);
			Fertigungsnummer = FAEntity.Fertigungsnummer ?? 0;
			Artikelnummer = ArtikelEntity?.ArtikelNummer;
			Produktionstermin = FAEntity.Termin_Bestatigt2.HasValue ? FAEntity.Termin_Bestatigt2.Value.ToString("dd/MM/yyyy") : "";
			Kennzeich = FAEntity.Kennzeichen;
			Id = FAEntity.ID;
		}
	}
}