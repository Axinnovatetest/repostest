namespace Psz.Core.FinanceControl.Models.Industry
{
	public class IndustryModel
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }

		public IndustryModel()
		{

		}
		public IndustryModel(Infrastructure.Data.Entities.Tables.FNC.IndustryEntity industryEntity)
		{
			Id = industryEntity.Id;
			Description = industryEntity.Description;
			Name = industryEntity.Name;
		}
	}
}
