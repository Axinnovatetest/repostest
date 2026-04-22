using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Reporting.Models
{
	public class FAWerkUpdateReportModel
	{
		public int IdUpdate { get; set; }
		public List<FAUpdatedFromExcelModel> Updated { get; set; }
		public List<FANotUpdatedFromExcelModel> NotUpdated { get; set; }
		public string Logo { get; set; } = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
		public FAWerkUpdateReportModel()
		{

		}
	}
}