using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FA.Update
{
	public partial class FAService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> GetArticlesForCRPFAUpdate(UserModel user, string artikelnummer)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();

			try
			{
				var entities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetForCRPFAUpdate(artikelnummer);
				var response = entities?.Select(e => new KeyValuePair<int, string>(e.ArtikelNr, e.ArtikelNummer)).ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}