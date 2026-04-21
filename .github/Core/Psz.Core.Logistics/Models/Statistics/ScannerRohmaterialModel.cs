using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class ScannerRohmaterialModel
	{
		public ScannerRohmaterialModel(Infrastructure.Data.Entities.Joins.Logistics.ScannerRohmaterialEntity ScannerRohmaterialEntity)
		{

			if(ScannerRohmaterialEntity == null)
				return;

			IdVersand = ScannerRohmaterialEntity.IdVersand;
			Transferlager = ScannerRohmaterialEntity.Transferlager;
			Artikelnummer = ScannerRohmaterialEntity.Artikelnummer;
			Lagerplatz_pos = ScannerRohmaterialEntity.Lagerplatz_pos;
			Menge = ScannerRohmaterialEntity.Menge;
			Scanndatum = ScannerRohmaterialEntity.Scanndatum.Value.ToString("dd/MM/yyyy HH:mm:ss");
			Datum = ScannerRohmaterialEntity.Datum;
		}

		public int IdVersand { get; set; }
		public int Transferlager { get; set; }
		public string Artikelnummer { get; set; }
		public int Lagerplatz_pos { get; set; }
		public int Menge { get; set; }
		public string Scanndatum { get; set; }
		public string Datum { get; set; }


	}
	public class Title
	{
		public int Transferlager { get; set; }
		public Title()
		{

		}
		public Title(ScannerRohmaterialModel model)
		{
			Transferlager = model.Transferlager;
		}
	}
	public class SubTitle
	{
		public int Transferlager { get; set; }
		public int Lagerplatz_pos { get; set; }


	}
	public class TitleDatum
	{

		public int Transferlager { get; set; }
		public int Lagerplatz_pos { get; set; }
		public string Datum { get; set; }
		public TitleDatum()
		{

		}
		public TitleDatum(ScannerRohmaterialModel model)
		{
			Transferlager = model.Transferlager;
			Lagerplatz_pos = model.Lagerplatz_pos;
			Datum = model.Datum;
		}

	}

	public class ScannerRohmaterialPDFModel
	{
		public List<Title> Title { get; set; }
		public List<ScannerRohmaterialModel> Details { get; set; }
		//public List<SubTitle> SubTitle { get; set; }
		public List<TitleDatum> TitleDatum { get; set; }
	}

}
