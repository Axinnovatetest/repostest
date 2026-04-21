namespace Psz.Core.FinanceControl.Models.ConditionAssignment
{
	public class ConditionAssignementModel
	{
		public int Id { get; set; } // Nr
		public string Text { get; set; }
		public ConditionAssignementModel()
		{

		}
		public ConditionAssignementModel(Infrastructure.Data.Entities.Tables.FNC.KonditionsZuordnungsTabelleEntity konditionsZuordnungsTabelleEntity)
		{
			Id = konditionsZuordnungsTabelleEntity.Nr;
			Text = konditionsZuordnungsTabelleEntity.Text;
		}
	}
}
