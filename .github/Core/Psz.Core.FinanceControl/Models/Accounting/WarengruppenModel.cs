namespace Psz.Core.FinanceControl.Models.Accounting;

public class WarengruppenModel
{
	public string Bezeichnung { get; set; }
	public string Hinweis { get; set; }
	public string Warengruppe { get; set; }
	public WarengruppenModel(Infrastructure.Data.Entities.Tables.FNC.WarengruppenEntity data)
	{
		Bezeichnung = data.Bezeichnung ?? string.Empty;
		Hinweis = data.Hinweis ?? string.Empty;
		Warengruppe = data.Warengruppe ?? string.Empty;
	}
}
