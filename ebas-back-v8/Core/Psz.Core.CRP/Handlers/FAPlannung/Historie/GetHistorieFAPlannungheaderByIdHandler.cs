using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning.Historie;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<FaPlannungHistorieHeadersModel> GetHistorieFAPlannungheaderById(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<FaPlannungHistorieHeadersModel>.AccessDeniedResponse();

			try
			{
				var entity = Infrastructure.Data.Access.Tables.CRP.__crp_historie_fa_plannung_headerAccess.Get(id);
				return ResponseModel<FaPlannungHistorieHeadersModel>.SuccessResponse(new FaPlannungHistorieHeadersModel(entity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}