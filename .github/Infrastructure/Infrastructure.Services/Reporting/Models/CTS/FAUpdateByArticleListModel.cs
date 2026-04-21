namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class FAUpdateByArticleListModel
	{
		public int Fertigungsnummer { get; set; }
		public int Lager { get; set; }
		public FAUpdateByArticleListModel()
		{

		}

		public FAUpdateByArticleListModel(int fertigunsnummer, int lager)
		{
			Fertigungsnummer = fertigunsnummer;
			Lager = lager;
		}
	}

	public class FANotUpdateByArticleListModel
	{
		public int Fertigungsnummer { get; set; }
		public string Raison { get; set; }

		public FANotUpdateByArticleListModel(int fertigunsnummer, string raison)
		{
			Fertigungsnummer = fertigunsnummer;
			Raison = raison;
		}
	}
}
