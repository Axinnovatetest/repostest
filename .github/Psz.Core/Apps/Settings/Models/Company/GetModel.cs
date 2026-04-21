using System;

namespace Psz.Core.Apps.Settings.Models.Company
{
	public class GetModel
	{
		public string Address { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public DateTime CreateTime { get; set; }
		public int CreateUserId { get; set; }
		public string Description { get; set; }
		public string DirectorEmail { get; set; }
		public int? DirectorId { get; set; }
		public string DirectorName { get; set; }
		public string Email { get; set; }
		public string Fax { get; set; }
		public int Id { get; set; }
		public bool? IsActive { get; set; }
		public string LagalName { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUserId { get; set; }
		public byte[] Logo { get; set; }
		public string LogoExtension { get; set; }
		public string Name { get; set; }
		public string PostalCode { get; set; }
		public string Telephone { get; set; }
		public string Telephone2 { get; set; }
		public string Type { get; set; }

		public GetModel(Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity)
		{
			Address = companyEntity?.Address;
			Address2 = companyEntity?.Address2;
			City = companyEntity?.City;
			Country = companyEntity?.Country;
			CreateTime = companyEntity?.CreateTime ?? DateTime.MinValue;
			CreateUserId = companyEntity?.CreateUserId ?? -1;
			Description = companyEntity?.Description;
			DirectorEmail = companyEntity?.DirectorEmail;
			DirectorId = companyEntity?.DirectorId;
			DirectorName = companyEntity?.DirectorName;
			Email = companyEntity?.Email;
			Fax = companyEntity?.Fax;
			Id = companyEntity?.Id ?? -1;
			IsActive = companyEntity?.IsActive;
			LagalName = companyEntity?.LagalName;
			LastUpdateTime = companyEntity?.LastUpdateTime;
			LastUpdateUserId = companyEntity?.LastUpdateUserId;
			Logo = companyEntity?.Logo;
			LogoExtension = companyEntity?.LogoExtension;
			Name = companyEntity?.Name;
			PostalCode = companyEntity?.PostalCode;
			Telephone = companyEntity?.Telephone;
			Telephone2 = companyEntity?.Telephone2;
			Type = companyEntity?.Type;
		}
		public Infrastructure.Data.Entities.Tables.STG.CompanyEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.STG.CompanyEntity
			{
				Address = Address,
				Address2 = Address2,
				City = City,
				Country = Country,
				CreateTime = CreateTime,
				CreateUserId = CreateUserId,
				Description = Description,
				DirectorEmail = DirectorEmail,
				DirectorId = DirectorId,
				DirectorName = DirectorName,
				Email = Email,
				Fax = Fax,
				Id = Id,
				IsActive = IsActive,
				LagalName = LagalName,
				LastUpdateTime = LastUpdateTime,
				LastUpdateUserId = LastUpdateUserId,
				Logo = Logo,
				LogoExtension = LogoExtension,
				Name = Name,
				PostalCode = PostalCode,
				Telephone = Telephone,
				Telephone2 = Telephone2,
				Type = Type,
			};
		}
	}
}
