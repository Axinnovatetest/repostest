namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetSupplierArticleModel
	{
		public string Article_supplier_name { get; set; }
		public int? Lieferantennummer { get; set; }
		public int Nr { get; set; }

		public string Anrede { get; set; }
		public string Vorname { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Ansprechpartner { get; set; }
		public string Abteilung { get; set; }
		public string StrassePostfach { get; set; }
		public string LandPLZ { get; set; }
		public string Briefanrede { get; set; }

		public string LieferantenNummer { get; set; }
		public string Versandart { get; set; }
		public string Zahlungsweise { get; set; }
		public string Konditionszuordnungs { get; set; }
		public string Nummer { get; set; }


		public string Fax { get; set; }
		public string Email { get; set; }

		public string Strasse { get; set; }
		public string PLZ { get; set; }
		public string Ort { get; set; }
		public string Land { get; set; }
		public GetSupplierArticleModel() { }

		public GetSupplierArticleModel(Infrastructure.Data.Entities.Tables.FNC.Supplier_Article_BudgetEntity budget_Supplier_ArticleEntity)
		{
			if(budget_Supplier_ArticleEntity == null)
				return;

			Article_supplier_name = budget_Supplier_ArticleEntity.Article_supplier_name;
			Lieferantennummer = budget_Supplier_ArticleEntity.Lieferantennummer;
			Nr = budget_Supplier_ArticleEntity.Nr;
			Anrede = budget_Supplier_ArticleEntity.Anrede;
			Vorname = budget_Supplier_ArticleEntity.Vorname;
			Name1 = budget_Supplier_ArticleEntity.Name1;
			Name2 = budget_Supplier_ArticleEntity.Name2;
			Name3 = budget_Supplier_ArticleEntity.Name3;
			Ansprechpartner = budget_Supplier_ArticleEntity.Ansprechpartner;
			Abteilung = budget_Supplier_ArticleEntity.Abteilung;
			StrassePostfach = budget_Supplier_ArticleEntity.StrassePostfach;
			LandPLZ = budget_Supplier_ArticleEntity.LandPLZ;
			Briefanrede = budget_Supplier_ArticleEntity.Briefanrede;
			Nummer = budget_Supplier_ArticleEntity.Nummer;

			LieferantenNummer = budget_Supplier_ArticleEntity.LieferantenNummer;
			Versandart = budget_Supplier_ArticleEntity.Versandart;
			Zahlungsweise = budget_Supplier_ArticleEntity.Zahlungsweise;
			Konditionszuordnungs = budget_Supplier_ArticleEntity.Konditionszuordnungs;

			Fax = budget_Supplier_ArticleEntity.Fax;
			Email = budget_Supplier_ArticleEntity.Email;

			Strasse = budget_Supplier_ArticleEntity?.Strasse;
			PLZ = budget_Supplier_ArticleEntity?.PLZ;
			Ort = budget_Supplier_ArticleEntity.Ort;
			Land = budget_Supplier_ArticleEntity.Land;
		}
		public Infrastructure.Data.Entities.Tables.FNC.Supplier_Article_BudgetEntity ToBudgetSupplierArticle()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Supplier_Article_BudgetEntity
			{
				Article_supplier_name = Article_supplier_name,
				Lieferantennummer = Lieferantennummer,
				Nr = Nr,
				Anrede = Anrede,
				Vorname = Vorname,
				Name1 = Name1,
				Name2 = Name2,
				Name3 = Name3,
				Ansprechpartner = Ansprechpartner,
				Abteilung = Abteilung,
				StrassePostfach = StrassePostfach,
				LandPLZ = LandPLZ,
				Briefanrede = Briefanrede,
				LieferantenNummer = LieferantenNummer,
				Versandart = Versandart,
				Zahlungsweise = Zahlungsweise,
				Konditionszuordnungs = Konditionszuordnungs,
				Nummer = Nummer,
				Fax = Fax,
				Email = Email,
				Strasse = Strasse,
				PLZ = PLZ,
				Ort = Ort,
				Land = Land
			};
		}
	}
}
