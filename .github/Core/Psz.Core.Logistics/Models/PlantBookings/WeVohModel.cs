
using System.Globalization;

namespace Psz.Core.Logistics.Models.PlantBookings;

public class WeVohModel
{
	public bool MHD { get; set; }
	public int weId { get; set; }
	public int position { get; set; }
	public string bezeichnung1 { get; set; }
	public string artikelnummer { get; set; }
	public int anzahl { get; set; }
	public int startAnzahl { get; set; }
	public string Einheit { get; set; }
	public System.DateTime? liefertermin { get; set; }

	public WeVohModel()
	{

	}
	public WeVohModel(Infrastructure.Data.Entities.Tables.Logistics.WEArtikelEntity artikelEntity)
	{
		if(artikelEntity == null)
			return;
		weId = artikelEntity.weId;
		position = artikelEntity.position;
		bezeichnung1 = artikelEntity.bezeichnung1;
		artikelnummer = artikelEntity.artikelnummer;
		anzahl = artikelEntity.anzahl;
		startAnzahl = artikelEntity.startAnzahl;
		Einheit = artikelEntity.Einheit;
		MHD = artikelEntity.MHD;
		liefertermin = artikelEntity.liefertermin;
	}
}
public class TransferWeVohModel
{
	public bool MHD { get; set; }
	public int weId { get; set; }
	public int lagerNach { get; set; }
	public int lagerVon { get; set; }
	public int ID { get; set; }
	public string? GebuchtVon { get; set; }
	public int position { get; set; }
	public string bezeichnung1 { get; set; }
	public string artikelnummer { get; set; }
	public int anzahl { get; set; }
	public int startAnzahl { get; set; }
	public string Einheit { get; set; }
	public System.DateTime? liefertermin { get; set; }

	public TransferWeVohModel()
	{

	}
	public TransferWeVohModel(Infrastructure.Data.Entities.Tables.Logistics.TransferWEArtikelEntity artikelEntity)
	{
		if(artikelEntity == null)
			return;
		weId = artikelEntity.weId;
		position = artikelEntity.position;
		bezeichnung1 = artikelEntity.bezeichnung1;
		artikelnummer = artikelEntity.artikelnummer;
		anzahl = artikelEntity.anzahl;
		startAnzahl = artikelEntity.startAnzahl;
		Einheit = artikelEntity.Einheit;
		MHD = artikelEntity.MHD;
		liefertermin = artikelEntity.liefertermin;
		GebuchtVon = artikelEntity.GebuchtVon.ToString().Split(" ").ToList()[3] + " " + artikelEntity.GebuchtVon.ToString().Split(" ").ToList()[4];
		lagerNach = artikelEntity.lagerNach;
		lagerVon = artikelEntity.lagerVon;
		ID = artikelEntity.ID;
	}
}

