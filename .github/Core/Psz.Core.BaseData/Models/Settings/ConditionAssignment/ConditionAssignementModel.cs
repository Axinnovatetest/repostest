namespace Psz.Core.BaseData.Models.ConditionAssignment
{
	public class ConditionAssignementModel
	{
		public int Id { get; set; } // Nr
		public string Text { get; set; }//Text
		public string Description { get; set; }//Bemerkung
		public double? Discount { get; set; }
		public int? Discount_days { get; set; }//skontotage
		public int? Net_days { get; set; }//Nettotage
		public ConditionAssignementModel()
		{

		}
		public ConditionAssignementModel(Infrastructure.Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity konditionsZuordnungsTabelleEntity)
		{
			Id = konditionsZuordnungsTabelleEntity.Nr;
			Text = konditionsZuordnungsTabelleEntity.Text;
			Description = konditionsZuordnungsTabelleEntity.Bemerkung;
			Discount = konditionsZuordnungsTabelleEntity.Skonto;
			Discount_days = konditionsZuordnungsTabelleEntity.Skontotage;
			Net_days = konditionsZuordnungsTabelleEntity.Nettotage;
		}

		public Infrastructure.Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity
			{
				Nr = Id,
				Text = Text,
				Bemerkung = Description,
				Skonto = Discount,
				Skontotage = Discount_days,
				Nettotage = Net_days,
			};
		}
	}
}
