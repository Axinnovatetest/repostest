namespace Psz.Core.FinanceControl.Models.SlipCircle
{
	public class SlipCircleModel
	{
		public string Description { get; set; } //Bezeichnung
		public int Id { get; set; }
		public SlipCircleModel()
		{

		}
		public SlipCircleModel(Infrastructure.Data.Entities.Tables.FNC.BelegkreiseVorgabenEntity belegkreiseVorgabenEntity)
		{
			Id = belegkreiseVorgabenEntity.ID;
			Description = belegkreiseVorgabenEntity.Bezeichnung;
		}
	}
}
