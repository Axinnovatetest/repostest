using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class UpdateArticelControlProcedureEntity
	{
		public decimal? ControlledAverage { get; set; }
		public decimal? ControlledFailedQuantity { get; set; }
		public decimal? ControlledMeasuredValue { get; set; }
		public decimal? ControlledQuantity { get; set; }
		public decimal? ControlledSum { get; set; }
		public decimal? ControlledTotalQuantity { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public string ProcedureDescription { get; set; }
		public string ProcedureName { get; set; }
		public int Id { get; set; }
		public string ProcedureType { get; set; }
	}
}
