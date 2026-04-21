using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.FinanceControl.Models.Accounting;

public class GetZahlungskonditionenLieferantenModel
{
	public string Name1 { get; set; }
	public int KonditionszuordnungstabelleNr { get; set; }
	public int adressenNr { get; set; }
	public string PLZ_Strabe { get; set; }
	public string Ort { get; set; }
	public string Land { get; set; }
	public int Lieferantennummer { get; set; }
	//public int TotalCount { get; set; }
	public string Text { get; set; }
	public GetZahlungskonditionenLieferantenModel(ZahlungskonditionenLieferantenEntity data)
	{
		Name1 = data.Name1;
		PLZ_Strabe = data.PLZ_Strabe;
		Ort = data.Ort;
		Land = data.Land;
		Lieferantennummer = data.Lieferantennummer;
		//TotalCount = data.TotalCount;
		Text = data.Text;
		KonditionszuordnungstabelleNr = data.KonditionszuordnungstabelleNr;
		adressenNr = data.adressenNr;
	}
	public GetZahlungskonditionenLieferantenModel()
	{

	}
}
public class ZahlungskonditionenLieferantenUpdateModel
{
	[Required]
	public string Name1 { get; set; }
	[Required]
	public int KonditionszuordnungstabelleNr { get; set; }
	[Required]
	public int adressenNr { get; set; }
	[Required]
	public string PLZ_Strabe { get; set; }
	[Required]
	public string Ort { get; set; }
	[Required]
	public string Land { get; set; }
	[Required]
	public int Lieferantennummer { get; set; }
	public int TotalCount { get; set; }
	[Required]
	public string Text { get; set; }
	public class GetZahlungskonditionenLieferantenRequestModel
	{

	}
	public List<string> GetUnMatchingUpdateAttributes(Infrastructure.Data.Entities.Joins.FNC.Accounting.ZahlungskonditionenLieferantenEntity data)
	{
		var unmatchingItems = new List<string>();
		if(data is not null)
		{
			if(Name1 != data.Name1)
			{
				unmatchingItems.Add("Name1");
			}
			if(PLZ_Strabe != data.PLZ_Strabe)
			{
				unmatchingItems.Add("PLZ_Strabe");
			}
			if(Ort != data.Ort)
			{
				unmatchingItems.Add("Ort");
			}
			if(Land != data.Land)
			{
				unmatchingItems.Add("Land");
			}
			if(Lieferantennummer != data.Lieferantennummer)
			{
				unmatchingItems.Add("Lieferantennummer");
			}
			if(Text != data.Text)
			{
				unmatchingItems.Add("Text");
			}

		}
		return unmatchingItems;
	}
}
