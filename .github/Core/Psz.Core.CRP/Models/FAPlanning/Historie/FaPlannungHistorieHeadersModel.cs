using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Models.FAPlanning.Historie
{
	public class FaPlannungHistorieHeadersModel
	{
		public int Id { get; set; }
		public DateTime? DateImport { get; set; }
		public DateTime? DateHistorie { get; set; }
		public string ImportUser { get; set; }
		public string ImportType { get; set; }
		public FaPlannungHistorieHeadersModel()
		{

		}
		public FaPlannungHistorieHeadersModel(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity entity)
		{
			Id = entity.Id;
			DateImport = entity.DateImport;
			DateHistorie = entity.DateHistorie;
			ImportUser = entity.ImportUsername;
			ImportType = entity.ImportTyeName;
		}
	}
	public class FaPlannungHistorieHeadersRequestModel: IPaginatedRequestModel
	{
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
	}
	public class FaPlannungHistorieHeadersResponseModel: IPaginatedResponseModel<FaPlannungHistorieHeadersModel> { }
}
