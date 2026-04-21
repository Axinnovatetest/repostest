namespace Psz.Core.Apps.Purchase.Models.CustomerService
{
	public class LanguageModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public LanguageModel(Infrastructure.Data.Entities.Tables.STG.SprachenEntity languageEntity)
		{
			Id = languageEntity.ID;
			Name = languageEntity.Sprache;
		}
	}
}
