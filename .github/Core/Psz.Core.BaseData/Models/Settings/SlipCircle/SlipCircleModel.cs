namespace Psz.Core.BaseData.Models.SlipCircle
{
	public class SlipCircleModel
	{
		public string Description { get; set; } //Bezeichnung
		public int Id { get; set; }
		public int? Belegkreis { get; set; }
		public SlipCircleModel()
		{

		}
		public SlipCircleModel(Infrastructure.Data.Entities.Tables.BSD.BelegkreiseVorgabenEntity belegkreiseVorgabenEntity)
		{
			Id = belegkreiseVorgabenEntity.ID;
			Belegkreis = belegkreiseVorgabenEntity.Belegkreis;
			Description = belegkreiseVorgabenEntity.Bezeichnung;
		}

		public Infrastructure.Data.Entities.Tables.BSD.BelegkreiseVorgabenEntity ToEntity(int id)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.BelegkreiseVorgabenEntity
			{
				ID = Id,
				Belegkreis = id,
				Bezeichnung = Description,
			};
		}
	}
}
