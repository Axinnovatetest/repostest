namespace Psz.Core.BaseData.Models.PayementPractice
{
	public class PayementPracticeModel
	{
		public int Id { get; set; }
		public string Description { get; set; }

		public PayementPracticeModel()
		{

		}
		public PayementPracticeModel(Infrastructure.Data.Entities.Tables.BSD.Mahnwesen_zahlungsmoralEntity termsOfPAyementEntity)
		{
			Id = termsOfPAyementEntity.ID;
			Description = termsOfPAyementEntity.Bezeichnung;
		}

		public Infrastructure.Data.Entities.Tables.BSD.Mahnwesen_zahlungsmoralEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Mahnwesen_zahlungsmoralEntity
			{
				ID = Id,
				Bezeichnung = Description,
			};
		}
	}
}
