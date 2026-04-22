using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class CalculateFamodel
	{
		public string Site { get; set; }
		public int? CountFa { get; set; }
		public CalculateFamodel()
		{

		}
		public CalculateFamodel(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.FaCalculateEntity faCalculateEntity)
		{
			if(faCalculateEntity == null)
				return;
			Site = faCalculateEntity.Site;
			CountFa = faCalculateEntity.CountFa ?? -1;
		}
	}
}
