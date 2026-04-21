using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Validator
{
	public class ValidatorModel
	{
		public int? Id_Project { get; set; }
		public int Id_User { get; set; }
		public int Id_Validator { get; set; }
		public string Validator_Name { get; set; }
		public int ID { get; set; }
		public int? Level { get; set; }
		public string email { get; set; }
		public DateTime? Validation_date { get; set; }
		public ValidatorModel() { }

		public ValidatorModel(Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity validatorProjectEntity,
		 List<Infrastructure.Data.Entities.Tables.COR.UserEntity> userProjectEntities) : this(validatorProjectEntity)
		{
			Validator_Name = userProjectEntities.Find(x => x.Id == validatorProjectEntity.Id_Validator)?.Username;

		}
		//public ValidatorModel(Infrastructure.Data.Entities.Tables.FNC.Validators_ProjectEntity validatorProjectEntity,
		//    Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity,
		//    List<Infrastructure.Data.Entities.Tables.FNC.Validators_ProjectEntity> validatorProjectEntities):this(validatorProjectEntity)

		//{
		//    Id_Validator = validatorProjectEntity.Id_Validator;
		//    foreach (var validator in validatorProjectEntities)
		//    {        
		//            Validator_Name = userEntity.Name;
		//    }
		//}
		public ValidatorModel(Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity validatorProjectEntity)
		{
			Id_Project = validatorProjectEntity.Id_Project == null ? -1 : validatorProjectEntity.Id_Project;
			Id_User = validatorProjectEntity.Id_User;
			Id_Validator = validatorProjectEntity.Id_Validator;
			ID = validatorProjectEntity.ID;
			Level = validatorProjectEntity.Level == null ? -1 : validatorProjectEntity.Level;
			email = validatorProjectEntity.email == null ? "" : validatorProjectEntity.email;
			Validation_date = validatorProjectEntity.Validation_date;
		}

		public Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity ToValidator_ProjectEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
			{
				Id_Project = Id_Project,
				Id_User = Id_User,
				Id_Validator = Id_Validator,
				ID = ID,
				Level = Level,
				email = email,
				Validation_date = Validation_date,

			};
		}
	}
}
