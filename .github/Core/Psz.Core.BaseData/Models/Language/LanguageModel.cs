namespace Psz.Core.BaseData.Models.Language
{
	public class LanguageModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public LanguageModel()
		{

		}
		public LanguageModel(Infrastructure.Data.Entities.Tables.STG.LanguageEntity languageEntity)
		{
			Id = languageEntity.Id;
			Name = languageEntity.Name;
			Code = languageEntity.Code;
			Description = languageEntity.Description;
		}
	}
}
