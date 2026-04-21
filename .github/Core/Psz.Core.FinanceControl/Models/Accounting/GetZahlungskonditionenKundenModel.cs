using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.FinanceControl.Models.Accounting;

public class GetZahlungskonditionenKundenModel
{
	public string Name1 { get; set; }
	public int adressenNr { get; set; }
	public int KonditionszuordnungstabelleNr { get; set; }
	public string PLZ_Strabe { get; set; }
	public string Ort { get; set; }
	public string Land { get; set; }
	public int Kundennummer { get; set; }
	public int TotalCount { get; set; }
	public string Text { get; set; }
	public GetZahlungskonditionenKundenModel(Infrastructure.Data.Entities.Joins.FNC.Accounting.ZahlungskonditionenKundenEntity data)
	{
		Name1 = data.Name1;
		PLZ_Strabe = data.PLZ_Strabe;
		Ort = data.Ort;
		Land = data.Land;
		Kundennummer = data.Kundennummer;
		TotalCount = data.TotalCount;
		Text = data.Text;
		adressenNr = data.adressenNr;
		KonditionszuordnungstabelleNr = data.KonditionszuordnungstabelleNr;
	}
}
public class ZahlungskonditionenKundenUpdateModel
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
	public int Kundennummer { get; set; }
	public int TotalCount { get; set; }
	[Required]
	public string Text { get; set; }

	public List<string> GetUnMatchingUpdateAttributes(Infrastructure.Data.Entities.Joins.FNC.Accounting.ZahlungskonditionenKundenEntity data)
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
			if(Kundennummer != data.Kundennummer)
			{
				unmatchingItems.Add("Kundennummer");
			}
			if(Text != data.Text)
			{
				unmatchingItems.Add("Text");
			}

		}
		return unmatchingItems;
	}
}
public class GetZahlungskonditionenKundenRequestModel: IPaginatedRequestModel
{

}
