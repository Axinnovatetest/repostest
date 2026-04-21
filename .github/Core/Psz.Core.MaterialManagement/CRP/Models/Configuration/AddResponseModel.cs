using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Models.Configuration
{
	public class AddResponseModel
	{
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int HallId { get; set; }
		public string HallName { get; set; }
		public int Id { get; set; }
		public decimal ProductionOrderThreshold { get; set; }

		// - Details
		public List<Department> Departments { get; set; }

		// - 
		public AddResponseModel(Infrastructure.Data.Entities.Tables.MTM.ConfigurationHeaderEntity configurationHeaderEntity,
			List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity> configurationDetailsEntities)
		{
			if(configurationHeaderEntity == null)
				return;

			Id = configurationHeaderEntity.Id;
			CountryId = configurationHeaderEntity.CountryId;
			CountryName = configurationHeaderEntity.CountryName;
			HallId = configurationHeaderEntity.HallId;
			HallName = configurationHeaderEntity.HallName;
			ProductionOrderThreshold = configurationHeaderEntity.ProductionOrderThreshold;
			//
			CreationUserId = configurationHeaderEntity.CreationUserId;
			CreationTime = configurationHeaderEntity.CreationTime;
			//-
			Departments = configurationDetailsEntities.Select(x => new Department(x))?.ToList();
		}

		public Infrastructure.Data.Entities.Tables.MTM.ConfigurationHeaderEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.MTM.ConfigurationHeaderEntity
			{
				Id = Id,
				CountryId = CountryId,
				CountryName = CountryName,
				HallId = HallId,
				HallName = HallName,
				ProductionOrderThreshold = ProductionOrderThreshold,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId
			};
		}

	}

	public class Department
	{
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public int DepartmentWeekNumber { get; set; }
		public int HeaderId { get; set; }
		public int Id { get; set; }
		public bool IsLowerThanThreshold { get; set; }
		// - 
		public bool? Validated { get; set; }
		public DateTime? ValidatedDate { get; set; }
		public int? ValidatedUserId { get; set; }
		public Department(Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity configurationDetailsEntity)
		{
			if(configurationDetailsEntity == null)
				return;

			Id = configurationDetailsEntity.Id;
			HeaderId = configurationDetailsEntity.HeaderId;
			DepartmentId = configurationDetailsEntity.DepartmentId;
			DepartmentName = configurationDetailsEntity.DepartmentName;
			DepartmentWeekNumber = configurationDetailsEntity.DepartmentWeekNumber;
			CreationUserId = configurationDetailsEntity.CreationUserId;
			CreationTime = configurationDetailsEntity.CreationTime;
			IsLowerThanThreshold = configurationDetailsEntity.IsLowerThanThreshold;
			Validated = configurationDetailsEntity.Validated;
			ValidatedDate = configurationDetailsEntity.ValidatedDate;
			ValidatedUserId = configurationDetailsEntity.ValidatedUserId;
		}

		public Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity
			{
				Id = Id,
				HeaderId = HeaderId,
				DepartmentId = DepartmentId,
				DepartmentName = DepartmentName,
				DepartmentWeekNumber = DepartmentWeekNumber,
				CreationUserId = CreationUserId,
				CreationTime = CreationTime,
				IsLowerThanThreshold = IsLowerThanThreshold,
				Validated = Validated,
				ValidatedDate = ValidatedDate,
				ValidatedUserId = ValidatedUserId
			};
		}
	}
}
