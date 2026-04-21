using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FAPlanning.Historie
{
	public class FAPlannungHistorieAgentLogModel
	{
		public DateTime? DateHistorie { get; set; }
		public DateTime? DateImport { get; set; }
		public string Username { get; set; }
		public string ImportType { get; set; }
		public FAPlannungHistorieAgentLogModel()
		{

		}
		public FAPlannungHistorieAgentLogModel(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity entity)
		{
			DateHistorie = entity.DateHistorie;
			DateImport = entity.DateImport;
			Username = entity.ImportUsername;
			ImportType = entity.ImportTyeName;
		}
	}
}