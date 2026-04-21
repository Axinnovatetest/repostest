namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetLandsModel
	{
		public int ID { get; set; }
		public string Land_name { get; set; }
		public int? SiteDirectorId { get; set; }
		public string SiteDirectorName { get; set; }
		public string SiteDirectorEmail { get; set; }

		public string PurchaseEmail { get; set; }
		public string PurchaseGroupName { get; set; }
		public int? PurchaseId { get; set; }
		public string PurchaseName { get; set; }
		public GetLandsModel() { }

		public GetLandsModel(Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity budget_LandsEntity)


		{
			ID = budget_LandsEntity.ID;
			Land_name = budget_LandsEntity.Land_name;
			SiteDirectorId = budget_LandsEntity.SiteDirectorId;
			SiteDirectorName = budget_LandsEntity.SiteDirectorName;
			SiteDirectorEmail = budget_LandsEntity.SiteDirectorEmail;
			PurchaseEmail = budget_LandsEntity.PurchaseEmail;
			PurchaseGroupName = budget_LandsEntity.PurchaseGroupName;
			PurchaseId = budget_LandsEntity.PurchaseId;
			PurchaseName = budget_LandsEntity.PurchaseName;

			// to Do
			//this.CreationTime = countryDb.CreationTime;
			//this.Designation = countryDb.Designation;
			//this.LastEditUsername = countryDb.LastEditUserId.HasValue ? Helpers.User.GetUserNameById(countryDb.LastEditUserId.Value) : "";
			//this.CreationUsername = Helpers.User.GetUserNameById(countryDb.CreationUserId);
			//this.LastEditTime = countryDb.LastEditTime.HasValue ? countryDb.LastEditTime.Value : (DateTime?)null;
			//this.CanSafeDelete = Psz.Core.Apps.WorkPlan.Helpers.DeleteCheck.CanSafeDeleteCountry(countryDb.Id);

		}
		public Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity ToBudgetLands()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity
			{
				ID = ID,
				Land_name = Land_name,
				SiteDirectorId = SiteDirectorId,
				SiteDirectorName = SiteDirectorName,
				SiteDirectorEmail = SiteDirectorEmail,
				PurchaseEmail = PurchaseEmail,
				PurchaseGroupName = PurchaseGroupName,
				PurchaseId = PurchaseId,
				PurchaseName = PurchaseName,
			};
		}
	}
}
