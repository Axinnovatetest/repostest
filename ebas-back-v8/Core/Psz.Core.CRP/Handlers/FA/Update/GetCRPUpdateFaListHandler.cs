using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models.FA;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FA.Update
{
	public partial class FAService: IFAService
	{
		public ResponseModel<List<FACRPUpdateListModel>> GetCRPUpdateFaList(UserModel user, string artikelnummer)
		{
			if(user == null)
				return ResponseModel<List<FACRPUpdateListModel>>.AccessDeniedResponse();

			try
			{
				var horizons = Module.CTS.FAHorizons;
				var h1EndDate = DateTime.Today.AddDays(horizons.H1LengthInDays);

				var entities = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetOpenFAForCRPUpdate(artikelnummer, h1EndDate);				

				return ResponseModel<List<FACRPUpdateListModel>>.SuccessResponse(
						entities?.Select(e => new FACRPUpdateListModel(e)).ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}