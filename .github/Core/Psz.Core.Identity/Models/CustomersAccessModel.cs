using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class CustomersAccessModel
	{
		public bool Administration { get; set; }
		public bool Configuration { get; set; }
		public bool EDI { get; set; }
		public int Id { get; set; }
		public int MainAccessProfileId { get; set; }
		public bool ModuleActivated { get; set; }
		public bool OrderProcessing { get; set; }
		public bool Statistics { get; set; }
		public CustomersAccessModel()
		{

		}
		public CustomersAccessModel(List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> accessProfileEntities)
		{
			if(accessProfileEntities == null || accessProfileEntities.Count <= 0)
				return;

			foreach(var accessItem in accessProfileEntities)
			{
				Administration = Administration || (accessItem?.Administration ?? false);
				Configuration = Configuration || (accessItem?.Configuration ?? false);
				EDI = EDI || (accessItem?.EDI ?? false);
				ModuleActivated = ModuleActivated || (accessItem?.ModuleActivated ?? false);
				OrderProcessing = OrderProcessing || (accessItem?.OrderProcessing ?? false);
				Statistics = Statistics || (accessItem?.Statistics ?? false);
			}
		}
	}
}
