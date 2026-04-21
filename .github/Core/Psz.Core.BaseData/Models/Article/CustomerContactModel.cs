namespace Psz.Core.BaseData.Models.Article
{
	public class CustomerContactModel
	{
		public int Nr { get; set; }
		public string Ansprechpartner { get; set; }

		public CustomerContactModel()
		{

		}
		public CustomerContactModel(Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity contactPersonEntity)
		{

			Nr = contactPersonEntity.Nr;
			Ansprechpartner = contactPersonEntity.Ansprechpartner;

		}
	}
}
