using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.InsideSales
{
	public partial class InsideSalesOveview
	{
		public ResponseModel<List<KeyValuePair<int, string>>> GetArticlesForChartsFilter(UserModel user, string text)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();

			try
			{
				var entities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetForFAStcklistSelect(text);
				var response = entities?.Select(x => new KeyValuePair<int, string>(x.ArtikelNr, x.ArtikelNummer)).ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}