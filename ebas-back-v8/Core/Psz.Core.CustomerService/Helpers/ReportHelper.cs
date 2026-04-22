using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Helpers
{
	public class ReportHelper
	{
		public static Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity SetBanksFooterByCustomerFactoring(Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity data,bool factoring)
		{
			var footerFactoring = Module.CTS.FooterFactoring;
			var footerNotFactoring = Module.CTS.FooterNotFactoring;

			if(factoring)
			{
				data.Footer67 = "";
				data.Footer65 = "";
				data.Footer64 = "";
				data.Footer61 = "";
				data.Footer77 = "";
				data.Footer75 = "";
				data.Footer74 = "";
				data.Footer71 = "";
				data.Footer63 = "";
				data.Footer73 = "";
				data.Footer78 = "";
				data.Footer79 = "";
				data.Footer80 = "";
				data.Footer81 = "";
				data.Footer82 = "";
				//
				if(footerFactoring is not null)
				{
					data.Footer57 = footerFactoring?.SWOFT_BIC ?? "";
					data.Footer55 = footerFactoring?.IBAN ?? "";
					data.Footer54 = footerFactoring?.BLZ ?? "";
					data.Footer51 = footerFactoring?.Bank ?? "";
					data.Footer53 = footerFactoring?.Konto ?? "";
				}
			}
			else
			{
				data.Footer78 = footerNotFactoring?.Bank ?? "";
				data.Footer79 = footerNotFactoring?.Konto ?? "";
				data.Footer80 = footerNotFactoring?.BLZ ?? "";
				data.Footer81 = footerNotFactoring?.IBAN ?? "";
				data.Footer82 = footerNotFactoring?.SWOFT_BIC ?? "";
			}
			return data;
		}
	}
}
