using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.COR
{
	public class UserEntity
	{
		public int AccessProfileId { get; set; }
		public int? CompanyId { get; set; }
		public int? CountryId { get; set; }
		public int CreationUserId { get; set; }
		public DateTime CreationTime { get; set; }
		public bool? CustomerServiceApp { get; set; }
		public DateTime? DeleteDate { get; set; }
		public int? DeleteUserId { get; set; }
		public int? DepartmentId { get; set; }
		public string Email { get; set; }
		public string Fax { get; set; }
		public bool? FinanceControlApp { get; set; }
		public bool? HumanResourcesApp { get; set; }
		public int Id { get; set; }
		public bool IsArchived { get; set; }
		public bool IsActivated { get; set; }
		public bool? IsAdministrator { get; set; }
		public bool? IsCorporateDirector { get; set; }
		public bool? IsGlobalDirector { get; set; }
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
		public int Nummer { get; set; }
		public string LegacyUsername { get; set; }

		public UserEntity() { }

		public UserEntity(DataRow dataRow)
		{
			AccessProfileId = Convert.ToInt32(dataRow["AccessProfileId"]);
			CompanyId = (dataRow["CompanyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CompanyId"]);
			CountryId = (dataRow["CountryId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CountryId"]);
			CreationUserId = Convert.ToInt32(dataRow["Creation_User_Id"]);
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CustomerServiceApp = (dataRow["CustomerServiceApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerServiceApp"]);
			DeleteDate = (dataRow["Delete_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Delete_Date"]);
			DeleteUserId = (dataRow["Delete_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Delete_User_Id"]);
			DepartmentId = (dataRow["DepartmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DepartmentId"]);
			Email = (dataRow["Email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Email"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			FinanceControlApp = (dataRow["FinanceControlApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FinanceControlApp"]);
			HumanResourcesApp = (dataRow["HumanResourcesApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["HumanResourcesApp"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsArchived = Convert.ToBoolean(dataRow["Is_Archived"]);
			IsActivated = Convert.ToBoolean(dataRow["IsActivated"]);
			IsAdministrator = (dataRow["IsAdministrator"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsAdministrator"]);
			IsCorporateDirector = (dataRow["IsCorporateDirector"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsCorporateDirector"]);
			IsGlobalDirector = (dataRow["IsGlobalDirector"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsGlobalDirector"]);
			LastEditDate = (dataRow["Last_Edit_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Last_Edit_Date"]);
			LastEditUserId = (dataRow["Last_Edit_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Last_Edit_User_Id"]);
			LogisticsApp = (dataRow["LogisticsApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LogisticsApp"]);
			MasterDataApp = (dataRow["MasterDataApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MasterDataApp"]);
			MaterialManagementApp = (dataRow["MaterialManagementApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MaterialManagementApp"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			Password = Convert.ToString(dataRow["Password"]);
			SalesDistributionApp = (dataRow["SalesDistributionApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SalesDistributionApp"]);
			SelectedLanguage = (dataRow["SelectedLanguage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SelectedLanguage"]);
			SettingsApp = (dataRow["SettingsApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SettingsApp"]);
			SuperAdministrator = Convert.ToBoolean(dataRow["SuperAdministrator"]);
			TelephoneHome = (dataRow["TelephoneHome"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TelephoneHome"]);
			TelephoneIP = (dataRow["TelephoneIP"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TelephoneIP"]);
			TelephoneMobile = (dataRow["TelephoneMobile"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TelephoneMobile"]);
			Username = Convert.ToString(dataRow["Username"]);
			Nummer = (dataRow["Nummer"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Nummer"].ToString());
			LegacyUsername = (dataRow["LegacyUsername"] == System.DBNull.Value) ? "-" : Convert.ToString(dataRow["LegacyUsername"].ToString());
		}
	}
}

