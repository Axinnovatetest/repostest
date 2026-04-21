using System;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class ValidatorModel
	{
		public int Step { get; set; }
		public string StepName { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public DateTime? Date { get; set; }
		public ValidatorModel(Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity entity, Infrastructure.Data.Entities.Tables.COR.UserEntity user)
		{
			Step = entity.Level ?? -1;
			StepName = $"{Common.Helpers.Numbers.getNumeral(entity.Level ?? 0)} validation";
			UserId = entity.Id_Validator;
			UserName = user?.Name;
			Date = entity.Validation_date;
		}
	}
}
