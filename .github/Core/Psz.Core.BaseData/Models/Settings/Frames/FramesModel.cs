namespace Psz.Core.BaseData.Models.Frames
{
	public class FramesModel
	{
		public int ID { get; set; }
		public string Frame { get; set; }

		public FramesModel()
		{

		}
		public FramesModel(Infrastructure.Data.Entities.Tables.BSD.Fibu_kunden_rahmenEntity frameEntity)
		{
			ID = frameEntity.ID;
			Frame = frameEntity.Rahmen;
		}

		public Infrastructure.Data.Entities.Tables.BSD.Fibu_kunden_rahmenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Fibu_kunden_rahmenEntity
			{
				ID = ID,
				Rahmen = Frame,
			};
		}
	}
}
