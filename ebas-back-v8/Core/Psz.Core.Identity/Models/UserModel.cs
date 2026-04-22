using System;

namespace Psz.Core.Identity.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public DateTime CreationTime { get; set; }
		public string Name { get; set; }
		public string SelectedLanguage { get; set; } = "en";
		public bool SuperAdministrator { get; set; } = false;
		public string Email { get; set; }
		public bool IsGlobalDirector { get; set; } = false;
		public bool IsCorporateDirector { get; set; } = false;
		public bool IsAdministrator { get; set; } = false;
		public AccessProfileModel Access { get; set; }

		public string Telephone { get; set; }
		public string Fax { get; set; }
		public int CompanyId { get; set; }
		public long DepartmentId { get; set; }
		public string CompanyName { get; set; }
		public string DepartmentName { get; set; }

		public int? Number { get; set; }
		public string LegacyUserName { get; set; }
		public UserModel() { }
		public UserModel(Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity,
			Models.AccessProfileModel accessProfile)
		{
			var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(userEntity.CompanyId ?? -1);
			var departmentEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(userEntity.DepartmentId ?? -1);

			// -
			this.Id = userEntity.Id;
			this.Username = userEntity.Username;
			this.CreationTime = userEntity.CreationTime;
			this.Name = userEntity.Name;
			this.SelectedLanguage = userEntity.SelectedLanguage;
			this.Email = userEntity.Email;
			this.IsGlobalDirector = userEntity.IsGlobalDirector ?? false;
			this.IsCorporateDirector = userEntity.IsCorporateDirector ?? false;
			this.IsAdministrator = userEntity.IsAdministrator ?? false;
			this.Telephone = $"{userEntity.TelephoneHome} - {userEntity.TelephoneMobile} - {userEntity.TelephoneIP}"?.Trim('-').Trim();
			this.Fax = userEntity.Fax;
			this.Access = accessProfile;

			this.CompanyId = companyEntity?.Id ?? -1;
			this.CompanyName = companyEntity?.Name;
			this.DepartmentId = departmentEntity?.Id ?? -1;
			this.DepartmentName = departmentEntity?.Name;

			this.SuperAdministrator = userEntity.SuperAdministrator;
			this.Number = userEntity.Nummer;
			this.LegacyUserName = userEntity.LegacyUsername;
		}
	}
}
