using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using System;

namespace Psz.Core.FinanceControl.Models.Accounting;

public class RechnungstransferModel
{
	public string Belegdatum { get; set; }
	public int Belegnummer { get; set; }
	public string Buchungstext { get; set; }
	public double Betrag { get; set; }
	public string Whrg { get; set; }
	public int Sollkto { get; set; }
	public int Habenkto { get; set; }
	public string DocumentNr { get; set; }

	public RechnungstransferModel(RechnungstransferEntity data)
	{
		Belegdatum = data.Belegdatum?.Date.ToString("dd/MM/yyyy");
		Belegnummer = data.Belegnummer;
		Buchungstext = data.Buchungstext;
		Betrag = data.Betrag;
		Whrg = data.Whrg;
		Sollkto = data.Sollkto;
		Habenkto = data.Habenkto;
		DocumentNr = data.Bezug;

	}
}
public class RechnungstransferHistoryRequestModel
{
	public DateTime? DateFrom { get; set; } = DateTime.Today;
	public DateTime? DateTill { get; set; } = DateTime.Today;
}
