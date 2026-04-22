using System;
using System.Collections.Generic;

namespace Psz.Core.ManagementOverview.Administration.Models.Users
{
	public class GetResponseModel
	{
		public int AccessProfileId { get; set; }
		public int? CompanyId { get; set; }
		public string CompanyName { get; set; }
		public int? CountryId { get; set; }
		public int CreationUserId { get; set; }
		public DateTime CreationTime { get; set; }
		public bool? CustomerServiceApp { get; set; }
		public DateTime? DeleteDate { get; set; }
		public int? DeleteUserId { get; set; }
		public int? DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public string Email { get; set; }
		public string Fax { get; set; }
		public bool? FinanceControlApp { get; set; }
		public bool? HumanResourcesApp { get; set; }
		public int Id { get; set; }
		public bool IsArchived { get; set; }
		public bool IsActivated { get; set; }
		public bool? IsGlobalDirector { get; set; }
		public bool? IsCorporateDirector { get; set; }
		public bool? IsAdministrator { get; set; }
		public DateTime? LastEditDate { get; set; }
		public int? LastEditUserId { get; set; }
		public bool? LogisticsApp { get; set; }
		public bool? MasterDataApp { get; set; }
		public bool? MaterialManagementApp { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
		public bool? SalesDistributionApp { get; set; }
		public string SelectedLanguage { get; set; }
		public bool? SettingsApp { get; set; }
		public bool SuperAdministrator { get; set; }
		public string TelephoneHome { get; set; }
		public string TelephoneIP { get; set; }
		public string TelephoneMobile { get; set; }
		public string Username { get; set; }

		//
		public int Nummer { get; set; }
		public string LegacyUsername { get; set; }
		public List<string> AccessProfileNames { get; set; }
		public List<AccessProfiles.AccessProfileAddRequestModel> AccessProfiles { get; set; }

		public GetResponseModel(Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity,
			Infrastructure.Data.Entities.Tables.STG.DepartmentEntity departmentEntity,
			List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> accessProfileEntities)
		{
			if(userEntity == null)
				return;

			AccessProfileId = userEntity.AccessProfileId;
			CompanyId = userEntity.CompanyId;
			CompanyName = companyEntity?.Name;
			CountryId = userEntity.CountryId;
			CreationUserId = userEntity.CreationUserId;
			CreationTime = userEntity.CreationTime;
			CustomerServiceApp = userEntity.CustomerServiceApp;
			DeleteDate = userEntity.DeleteDate;
			DeleteUserId = userEntity.DeleteUserId;
			DepartmentId = userEntity.DepartmentId;
			DepartmentName = departmentEntity?.Name;
			Email = userEntity.Email;
			Fax = userEntity.Fax;
			FinanceControlApp = userEntity.FinanceControlApp;
			HumanResourcesApp = userEntity.HumanResourcesApp;
			Id = userEntity.Id;
			IsArchived = userEntity.IsArchived;
			IsActivated = userEntity.IsActivated;
			IsGlobalDirector = userEntity.IsGlobalDirector;
			IsCorporateDirector = userEntity.IsCorporateDirector;
			IsAdministrator = userEntity.IsAdministrator;
			LastEditDate = userEntity.LastEditDate;
			LastEditUserId = userEntity.LastEditUserId;
			LogisticsApp = userEntity.LogisticsApp;
			MasterDataApp = userEntity.MasterDataApp;
			MaterialManagementApp = userEntity.MaterialManagementApp;
			Name = userEntity.Name;
			Password = userEntity.Password;
			SalesDistributionApp = userEntity.SalesDistributionApp;
			SelectedLanguage = userEntity.SelectedLanguage;
			SettingsApp = userEntity.SettingsApp;
			SuperAdministrator = userEntity.SuperAdministrator;
			TelephoneHome = userEntity.TelephoneHome;
			TelephoneIP = userEntity.TelephoneIP;
			TelephoneMobile = userEntity.TelephoneMobile;
			Username = userEntity.Username;
			//Nummer = userEntity.Nummer;
			//LegacyUsername = userEntity.LegacyUsername;
			//- 
			if(accessProfileEntities != null && accessProfileEntities.Count > 0)
			{
				AccessProfileNames = new List<string>();
				AccessProfiles = new List<AccessProfiles.AccessProfileAddRequestModel>();
				accessProfileEntities?.ForEach(x =>
				{
					AccessProfileNames.Add(x.AccessProfileName);
					AccessProfiles.Add(new AccessProfiles.AccessProfileAddRequestModel(x));
				});
			}
		}


	}
}
