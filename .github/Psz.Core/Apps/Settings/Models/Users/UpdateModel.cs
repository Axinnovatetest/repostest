namespace Psz.Core.Apps.Settings.Models.Users
{
	public class UpdateModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public int AccessProfileId { get; set; }
		//- 
		public int CompanyId { get; set; }
		public int CountryId { get; set; }
		public int DepartmentId { get; set; }


		// -
		public bool? SalesDistributionApp { get; set; }
		public bool? CustomerServiceApp { get; set; }
		public bool? FinanceControlApp { get; set; }
		public bool? LogisticsApp { get; set; }
		public bool? HumanResourcesApp { get; set; }
		public bool? MaterialManagementApp { get; set; }
		public bool? MasterDataApp { get; set; }
		public bool? SettingsApp { get; set; }
		// - 

		public string Fax { get; set; }
		public string TelephoneHome { get; set; }
		public string TelephoneIP { get; set; }
		public string TelephoneMobile { get; set; }

		// souilmi 27-07-2023
		public bool? SameProfileAs { get; set; }
		public int? SameProfileUserId { get; set; }
		public UpdateModel()
		{

		}
		public UpdateModel(Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity,
			Infrastructure.Data.Entities.Tables.STG.DepartmentEntity departmentEntity,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity)
		{
			Id = userEntity.Id;
			Username = userEntity?.Username;
			Name = userEntity?.Name;
			Email = userEntity?.Email;

			CompanyId = userEntity.CompanyId ?? (int?)companyEntity?.Id ?? -1;
			CountryId = userEntity.CountryId ?? -1;
			DepartmentId = userEntity.DepartmentId ?? (int?)departmentEntity?.Id ?? -1;

			AccessProfileId = userEntity.AccessProfileId;

			SalesDistributionApp = userEntity?.SalesDistributionApp ?? false;
			CustomerServiceApp = userEntity?.CustomerServiceApp ?? false;
			FinanceControlApp = userEntity?.FinanceControlApp ?? false;
			LogisticsApp = userEntity?.LogisticsApp ?? false;
			HumanResourcesApp = userEntity?.HumanResourcesApp ?? false;
			MaterialManagementApp = userEntity?.MaterialManagementApp ?? false;
			MasterDataApp = userEntity?.MasterDataApp ?? false;
			SettingsApp = userEntity?.SettingsApp ?? false;

			Fax = userEntity?.Fax;
			TelephoneHome = userEntity?.TelephoneHome;
			TelephoneMobile = userEntity?.TelephoneMobile;
			TelephoneIP = userEntity?.TelephoneIP;
		}

		public Infrastructure.Data.Entities.Tables.COR.UserEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.COR.UserEntity
			{
				Id = Id,
				Username = Username,
				Name = Name,
				Email = Email,

				CompanyId = CompanyId,
				CountryId = CountryId,
				DepartmentId = DepartmentId,

				AccessProfileId = AccessProfileId,

				SalesDistributionApp = SalesDistributionApp ?? false,
				CustomerServiceApp = CustomerServiceApp ?? false,
				FinanceControlApp = FinanceControlApp ?? false,
				LogisticsApp = LogisticsApp ?? false,
				HumanResourcesApp = HumanResourcesApp ?? false,
				MaterialManagementApp = MaterialManagementApp ?? false,
				MasterDataApp = MasterDataApp ?? false,
				SettingsApp = SettingsApp ?? false,

				Fax = Fax,
				TelephoneHome = TelephoneHome,
				TelephoneMobile = TelephoneMobile,
				TelephoneIP = TelephoneIP
			};
		}
	}
}
