namespace Psz.Core.FinanceControl.Models.Accounting
{
	public class Fibu_kunden_rahmenModel
	{
		public int ID { get; set; }
		public string Rahmen { get; set; }
		public Fibu_kunden_rahmenModel(Infrastructure.Data.Entities.Tables.FNC.Fibu_kunden_rahmenEntity data)
		{
			ID = data.ID;
			Rahmen = data.Rahmen ?? string.Empty;
		}
	}
}
