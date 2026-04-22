using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Models.Users
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public int AccessProfileId { get; set; }
		public string AccessProfileName { get; set; }

		//- 
		public int? CompanyId { get; set; }
		public int? CountryId { get; set; }
		public int CreationUserId { get; set; }
		public DateTime CreationTime { get; set; }
		public DateTime? DeleteDate { get; set; }
		public int? DeleteUserId { get; set; }
		public int? DepartmentId { get; set; }
		public bool IsArchived { get; set; }
		public bool IsActivated { get; set; }
		public bool? IsGlobalDirector { get; set; }
		public bool? IsCorporateDirector { get; set; }
		public bool? IsAdministrator { get; set; }
		public DateTime? LastEditDate { get; set; }
		public int? LastEditUserId { get; set; }
		public string Password { get; set; }
		public string SelectedLanguage { get; set; }
		public bool SuperAdministrator { get; set; }


		public bool SalesDistributionApp { get; set; }
		public bool CustomerServiceApp { get; set; }
		public bool FinanceControlApp { get; set; }
		public bool LogisticsApp { get; set; }
		public bool HumanResourcesApp { get; set; }
		public bool MaterialManagementApp { get; set; }
		public bool MasterDataApp { get; set; }
		public bool SettingsApp { get; set; }


		public string CompanyName { get; set; }
		public string DepartmentName { get; set; }

		public string TelephoneMobile { get; set; }
		public string TelephoneHome { get; set; }
		public string TelephoneIP { get; set; }
		public string Fax { get; set; }
		// - 2023-05-02
		public string LegacyUsername { get; set; }
		public int? UserNumber { get; set; }
		//rami 22/09/2025
		public List<KeyValuePair<int, string>> Halls { get; set; } = new List<KeyValuePair<int, string>>();
		public List<KeyValuePair<int, string>> Countries { get; set; } = new List<KeyValuePair<int, string>>();
		public UserModel(Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity)
		{
			Id = userEntity.Id;
			AccessProfileId = userEntity.AccessProfileId;
			Name = userEntity.Name;
			Username = userEntity.Username;
			Email = userEntity.Email;
			CompanyId = userEntity?.CompanyId;
			CountryId = userEntity?.CountryId;
			CreationUserId = userEntity?.CreationUserId ?? -1;
			CreationTime = userEntity?.CreationTime ?? DateTime.MinValue;
			DeleteDate = userEntity?.DeleteDate;
			DeleteUserId = userEntity?.DeleteUserId;
			DepartmentId = userEntity?.DepartmentId;
			IsArchived = userEntity?.IsArchived ?? false;
			IsActivated = userEntity?.IsActivated ?? false;
			IsGlobalDirector = userEntity?.IsGlobalDirector;
			IsCorporateDirector = userEntity?.IsCorporateDirector;
			IsAdministrator = userEntity?.IsAdministrator;
			LastEditDate = userEntity?.LastEditDate;
			LastEditUserId = userEntity?.LastEditUserId;
			SelectedLanguage = userEntity?.SelectedLanguage;
			SuperAdministrator = userEntity?.SuperAdministrator ?? false;
			SalesDistributionApp = userEntity?.SalesDistributionApp ?? false;
			CustomerServiceApp = userEntity?.CustomerServiceApp ?? false;
			FinanceControlApp = userEntity?.FinanceControlApp ?? false;
			LogisticsApp = userEntity?.LogisticsApp ?? false;
			HumanResourcesApp = userEntity?.HumanResourcesApp ?? false;
			MaterialManagementApp = userEntity?.MaterialManagementApp ?? false;
			MasterDataApp = userEntity?.MasterDataApp ?? false;
			SettingsApp = userEntity?.SettingsApp ?? false;

			TelephoneMobile = userEntity?.TelephoneMobile;
			TelephoneHome = userEntity?.TelephoneHome;
			TelephoneIP = userEntity?.TelephoneIP;
			Fax = userEntity?.Fax;
			LegacyUsername = userEntity?.LegacyUsername;
			UserNumber = userEntity?.Nummer;
		}
		public UserModel(Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity,
			Infrastructure.Data.Entities.Tables.AccessProfileEntity accessProfileEntity,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity,
			Infrastructure.Data.Entities.Tables.STG.DepartmentEntity departmentEntity,
			List<KeyValuePair<int,string>> halls, List<KeyValuePair<int, string>> coutries)
		{
			Id = userEntity.Id;
			AccessProfileId = userEntity.AccessProfileId;
			AccessProfileName = accessProfileEntity?.Name;
			Name = userEntity.Name;
			Username = userEntity.Username;
			Email = userEntity.Email;
			// -
			CompanyId = userEntity?.CompanyId;
			CountryId = userEntity?.CountryId;
			CreationUserId = userEntity?.CreationUserId ?? -1;
			CreationTime = userEntity?.CreationTime ?? DateTime.MinValue;
			DeleteDate = userEntity?.DeleteDate;
			DeleteUserId = userEntity?.DeleteUserId;
			DepartmentId = userEntity?.DepartmentId;
			IsArchived = userEntity?.IsArchived ?? false;
			IsActivated = userEntity?.IsActivated ?? false;
			IsGlobalDirector = userEntity?.IsGlobalDirector;
			IsCorporateDirector = userEntity?.IsCorporateDirector;
			IsAdministrator = userEntity?.IsAdministrator;
			LastEditDate = userEntity?.LastEditDate;
			LastEditUserId = userEntity?.LastEditUserId;
			//Password = userEntity?.Password;
			SelectedLanguage = userEntity?.SelectedLanguage;
			SuperAdministrator = userEntity?.SuperAdministrator ?? false;

			CompanyName = companyEntity?.Name;
			DepartmentName = departmentEntity?.Name;

			SalesDistributionApp = userEntity?.SalesDistributionApp ?? false;
			CustomerServiceApp = userEntity?.CustomerServiceApp ?? false;
			FinanceControlApp = userEntity?.FinanceControlApp ?? false;
			LogisticsApp = userEntity?.LogisticsApp ?? false;
			HumanResourcesApp = userEntity?.HumanResourcesApp ?? false;
			MaterialManagementApp = userEntity?.MaterialManagementApp ?? false;
			MasterDataApp = userEntity?.MasterDataApp ?? false;
			SettingsApp = userEntity?.SettingsApp ?? false;

			TelephoneMobile = userEntity?.TelephoneMobile;
			TelephoneHome = userEntity?.TelephoneHome;
			TelephoneIP = userEntity?.TelephoneIP;
			Fax = userEntity?.Fax;

			// -
			LegacyUsername = userEntity?.LegacyUsername;
			UserNumber = userEntity?.Nummer;
			//
			Halls = halls;
			Countries = coutries;
		}
	}
	public class UserADModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public int AccessProfileId { get; set; }
		public string AccessProfileName { get; set; }
	}
}
