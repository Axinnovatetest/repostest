using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.FNC
{

	public class UserEntity
	{
		public int AccessProfileId { get; set; }
		public int? CompanyId { get; set; }
		public int? CountryId { get; set; }
		public int Creation_User_Id { get; set; }
		public DateTime CreationTime { get; set; }
		public bool? CustomerServiceApp { get; set; }
		public DateTime? Delete_Date { get; set; }
		public int? Delete_User_Id { get; set; }
		public int? DepartmentId { get; set; }
		public string Email { get; set; }
		public string Fax { get; set; }
		public bool? FinanceControlApp { get; set; }
		public bool? HumanResourcesApp { get; set; }
		public int Id { get; set; }
		public bool Is_Archived { get; set; }
		public bool IsActivated { get; set; }
		public bool? IsGlobalDirector { get; set; }
		public DateTime? Last_Edit_Date { get; set; }
		public int? Last_Edit_User_Id { get; set; }
		public int? LastConnectErrorCount { get; set; }
		public DateTime? LastConnectErrorTime { get; set; }
		public DateTime? LastConnectTime { get; set; }
		public string LegacyUsername { get; set; }
		public bool? LogisticsApp { get; set; }
		public bool? MasterDataApp { get; set; }
		public bool? MaterialManagementApp { get; set; }
		public string Name { get; set; }
		public int? Nummer { get; set; }
		public string Password { get; set; }
		public bool? SalesDistributionApp { get; set; }
		public string SelectedLanguage { get; set; }
		public bool? SettingsApp { get; set; }
		public bool SuperAdministrator { get; set; }
		public string TelephoneHome { get; set; }
		public string TelephoneIP { get; set; }
		public string TelephoneMobile { get; set; }
		public string Username { get; set; }

		public UserEntity() { }

		public UserEntity(DataRow dataRow)
		{
			AccessProfileId = Convert.ToInt32(dataRow["AccessProfileId"]);
			CompanyId = (dataRow["CompanyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CompanyId"]);
			CountryId = (dataRow["CountryId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CountryId"]);
			Creation_User_Id = Convert.ToInt32(dataRow["Creation_User_Id"]);
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CustomerServiceApp = (dataRow["CustomerServiceApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerServiceApp"]);
			Delete_Date = (dataRow["Delete_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Delete_Date"]);
			Delete_User_Id = (dataRow["Delete_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Delete_User_Id"]);
			DepartmentId = (dataRow["DepartmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DepartmentId"]);
			Email = (dataRow["Email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Email"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			FinanceControlApp = (dataRow["FinanceControlApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FinanceControlApp"]);
			HumanResourcesApp = (dataRow["HumanResourcesApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["HumanResourcesApp"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Is_Archived = Convert.ToBoolean(dataRow["Is_Archived"]);
			IsActivated = Convert.ToBoolean(dataRow["IsActivated"]);
			IsGlobalDirector = (dataRow["IsGlobalDirector"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsGlobalDirector"]);
			Last_Edit_Date = (dataRow["Last_Edit_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Last_Edit_Date"]);
			Last_Edit_User_Id = (dataRow["Last_Edit_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Last_Edit_User_Id"]);
			LastConnectErrorCount = (dataRow["LastConnectErrorCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastConnectErrorCount"]);
			LastConnectErrorTime = (dataRow["LastConnectErrorTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastConnectErrorTime"]);
			LastConnectTime = (dataRow["LastConnectTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastConnectTime"]);
			LegacyUsername = (dataRow["LegacyUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LegacyUsername"]);
			LogisticsApp = (dataRow["LogisticsApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LogisticsApp"]);
			MasterDataApp = (dataRow["MasterDataApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MasterDataApp"]);
			MaterialManagementApp = (dataRow["MaterialManagementApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MaterialManagementApp"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			Nummer = (dataRow["Nummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nummer"]);
			Password = Convert.ToString(dataRow["Password"]);
			SalesDistributionApp = (dataRow["SalesDistributionApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SalesDistributionApp"]);
			SelectedLanguage = (dataRow["SelectedLanguage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SelectedLanguage"]);
			SettingsApp = (dataRow["SettingsApp"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SettingsApp"]);
			SuperAdministrator = Convert.ToBoolean(dataRow["SuperAdministrator"]);
			TelephoneHome = (dataRow["TelephoneHome"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TelephoneHome"]);
			TelephoneIP = (dataRow["TelephoneIP"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TelephoneIP"]);
			TelephoneMobile = (dataRow["TelephoneMobile"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TelephoneMobile"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
		}

		public UserEntity ShallowClone()
		{
			return new UserEntity
			{
				AccessProfileId = AccessProfileId,
				CompanyId = CompanyId,
				CountryId = CountryId,
				Creation_User_Id = Creation_User_Id,
				CreationTime = CreationTime,
				CustomerServiceApp = CustomerServiceApp,
				Delete_Date = Delete_Date,
				Delete_User_Id = Delete_User_Id,
				DepartmentId = DepartmentId,
				Email = Email,
				Fax = Fax,
				FinanceControlApp = FinanceControlApp,
				HumanResourcesApp = HumanResourcesApp,
				Id = Id,
				Is_Archived = Is_Archived,
				IsActivated = IsActivated,
				IsGlobalDirector = IsGlobalDirector,
				Last_Edit_Date = Last_Edit_Date,
				Last_Edit_User_Id = Last_Edit_User_Id,
				LastConnectErrorCount = LastConnectErrorCount,
				LastConnectErrorTime = LastConnectErrorTime,
				LastConnectTime = LastConnectTime,
				LegacyUsername = LegacyUsername,
				LogisticsApp = LogisticsApp,
				MasterDataApp = MasterDataApp,
				MaterialManagementApp = MaterialManagementApp,
				Name = Name,
				Nummer = Nummer,
				Password = Password,
				SalesDistributionApp = SalesDistributionApp,
				SelectedLanguage = SelectedLanguage,
				SettingsApp = SettingsApp,
				SuperAdministrator = SuperAdministrator,
				TelephoneHome = TelephoneHome,
				TelephoneIP = TelephoneIP,
				TelephoneMobile = TelephoneMobile,
				Username = Username
			};
		}
	}
}
