namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class FAGeneralWunchUpdateModel
	{
		public FAWunshUpdateReporAdmintModel AdminUpdate { get; set; }
		public FAWunshUpdateReporUsertModel UserUpdate { get; set; }
		public bool Admin { get; set; }

		public FAGeneralWunchUpdateModel()
		{

		}
	}
}
