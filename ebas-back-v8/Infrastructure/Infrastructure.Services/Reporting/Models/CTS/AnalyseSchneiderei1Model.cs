namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class AnalyseSchneiderei1Model
	{
		public string T_F { get; set; }
		public string T_B1 { get; set; }
		public int Fer { get; set; }
		public string Anz { get; set; }
		public string Artik_Nr { get; set; }
		public string B1 { get; set; }
		public string UmCZ { get; set; }
		public string Prod { get; set; }
		public string Artik_Nr2 { get; set; }
		public string Ver { get; set; }
		public string SummFAB { get; set; }
		public string SummBe { get; set; }
		public string BZ2 { get; set; }
		public AnalyseSchneiderei1Model()
		{

		}
		public AnalyseSchneiderei1Model(Infrastructure.Data.Entities.Joins.FAUpdate.FAAnalyse1Entity entity)
		{
			T_F = entity.T_F.HasValue ? entity.T_F.Value.ToString("dd.MM.yyyy") : "";
			T_B1 = entity.T_B1.HasValue ? entity.T_B1.Value.ToString("dd.MM.yyyy") : "";
			Fer = entity.Fer ?? 0;
			Anz = entity.Anz.HasValue ? entity.Anz.ToString() : "";
			Artik_Nr = entity.Artik_Nr;
			B1 = entity.B1;
			UmCZ = entity.UmCZ.HasValue ? entity.UmCZ.ToString() + " €" : "";
			Prod = entity.Prod.HasValue ? string.Format("{0:n}", entity.Prod.Value) : "";
			Artik_Nr2 = entity.Artik_Nr2;
			Ver = entity.Ver.HasValue ? string.Format("{0:n}", entity.Ver.Value) : "";
			SummFAB = entity.SummFAB.HasValue ? string.Format("{0:n}", entity.SummFAB.Value) : "";
			SummBe = entity.SummBe.HasValue ? string.Format("{0:n}", entity.SummBe.Value) : "";
			BZ2 = entity.BZ2;
		}
	}
}