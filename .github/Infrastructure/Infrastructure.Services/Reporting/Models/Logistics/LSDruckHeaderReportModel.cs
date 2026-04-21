namespace Infrastructure.Services.Reporting.Models.Logistics
{
	public class LSDruckHeaderReportModel
	{
		public long angeboteNr { get; set; }
		public long projektNr { get; set; }
		public string liefertermin { get; set; }
		public int kundenNr { get; set; }
		public string anrede { get; set; }
		public string vornameNameFirma { get; set; }
		public string name2 { get; set; }
		public string name3 { get; set; }
		public string ansprechpartner { get; set; }
		public string abteilung { get; set; }
		public string landPLZOrt { get; set; }

		public string briefanrede { get; set; }
		public string lanrede { get; set; }
		public string lVornameNameFirma { get; set; }
		public string lName2 { get; set; }
		public string lName3 { get; set; }
		public string lAnsprechpartner { get; set; }
		public string labteilung { get; set; }
		public string lLandPLZOrt { get; set; }
		public string lStrassePostfach { get; set; }
		public string lBriefanrede { get; set; }
		public int personelNr { get; set; }
		public string ihrZeichen { get; set; }
		public string bezug { get; set; }
		public string versandart { get; set; }
		public string datum { get; set; }
		public string freitext { get; set; }
		public string typ { get; set; }
		public string ablastelle { get; set; }
		public string textLieferschein { get; set; }
		public string strassePostfach { get; set; }
		public string unserZeichen { get; set; }
		public decimal ust { get; set; }
		public bool rp { get; set; }
		public bool PoBarCode { get; set; }
		public bool DocumentNoBarCode { get; set; }
		public LSDruckHeaderReportModel()
		{

		}
		public LSDruckHeaderReportModel(Infrastructure.Data.Entities.Tables.Logistics.LSEntity lsEntity, bool poBarCode, bool documentNoBarCode)
		{
			angeboteNr = lsEntity.angeboteNr;
			projektNr = lsEntity.projektNr;
			liefertermin = lsEntity.liefertermin != null ? lsEntity.liefertermin.Value.ToString("dd-MM-yyyy") : "";
			kundenNr = lsEntity.kundenNr;
			anrede = lsEntity.anrede;
			vornameNameFirma = lsEntity.vornameNameFirma;
			name2 = lsEntity.name2;
			name3 = lsEntity.name3;
			ansprechpartner = lsEntity.ansprechpartner;
			abteilung = lsEntity.abteilung;
			landPLZOrt = lsEntity.landPLZOrt;
			strassePostfach = lsEntity.strassePostfach;
			briefanrede = lsEntity.briefanrede;
			lanrede = lsEntity.lanrede;
			lVornameNameFirma = lsEntity.lVornameNameFirma;
			lName2 = lsEntity.lName2;
			lName3 = lsEntity.lName3;
			lAnsprechpartner = lsEntity.lAnsprechpartner;
			labteilung = lsEntity.labteilung;
			lLandPLZOrt = lsEntity.lLandPLZOrt;
			lStrassePostfach = lsEntity.lStrassePostfach;
			lBriefanrede = lsEntity.lBriefanrede;
			personelNr = lsEntity.personelNr;
			ihrZeichen = lsEntity.ihrZeichen;
			bezug = lsEntity.bezug;
			versandart = lsEntity.versandart;
			datum = lsEntity.datum != null ? lsEntity.datum.Value.ToString("dd-MM-yyyy") : "";
			freitext = lsEntity.freitext;
			typ = lsEntity.typ;
			ablastelle = lsEntity.ablastelle;
			textLieferschein = lsEntity.textLieferschein;
			unserZeichen = lsEntity.unserZeichen;
			ust = lsEntity.ust;
			rp = lsEntity.rp;
			PoBarCode = poBarCode;
			DocumentNoBarCode = documentNoBarCode;
		}
	}
}
