
namespace Psz.Core.CRP.Models.FA
{
	public class FATechnikModel
	{
		public int ID { get; set; }
		public string? Aktion { get; set; }
		public string? Details { get; set; }
		public string? Mitarbeiter { get; set; }
		public DateTime? Termin { get; set; }
		public bool? Status { get; set; }
		public int? ID_Fertigung { get; set; }
		public FATechnikModel()
		{

		}
		public FATechnikModel(Infrastructure.Data.Entities.Tables.CTS.Fertigung_PlanungsdetailsEntity planungEntity)
		{
			if(planungEntity == null)
				return;

			ID = planungEntity.ID;
			ID_Fertigung = planungEntity.ID_Fertigung;
			Aktion = planungEntity.Aktion;
			Details = planungEntity.Details;
			Mitarbeiter = planungEntity.Mitarbeiter;
			Termin = planungEntity.Termin;
			Status = planungEntity.Status;
		}
		public Infrastructure.Data.Entities.Tables.CTS.Fertigung_PlanungsdetailsEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.Fertigung_PlanungsdetailsEntity
			{
				ID = ID,
				Aktion = Aktion,
				ID_Fertigung = ID_Fertigung,
				Details = Details,
				Mitarbeiter = Mitarbeiter,
				Termin = Termin,
				Status = Status,
			};
		}
	}
}