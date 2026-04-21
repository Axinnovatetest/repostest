namespace Psz.Core.FinanceControl.Models.Budget.Account
{
	public class AccountModel
	{
		public string Description { get; set; }
		public int ID { get; set; }
		public string Country { get; set; }
		public string Account { get; set; }
		public string Products { get; set; }

		public AccountModel() { }
		public AccountModel(Infrastructure.Data.Entities.Tables.FNC.PSZ_BH_KontenSoll_WKZEntity AccountEntity)
		{
			Description = AccountEntity.Beschreibung;
			ID = AccountEntity.ID;
			Country = AccountEntity.Niederlassung;
			Account = AccountEntity.Sollkto;
			Products = AccountEntity.Warengruppe;

		}
		public Infrastructure.Data.Entities.Tables.FNC.PSZ_BH_KontenSoll_WKZEntity ToBudgetAccount()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.PSZ_BH_KontenSoll_WKZEntity
			{
				Beschreibung = Description,
				ID = ID,
				Niederlassung = Country,
				Sollkto = Account,
				Warengruppe = Products,
			};
		}
	}
}
