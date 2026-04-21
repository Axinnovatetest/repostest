using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<int> DeleteRohArtikelnummerLevel2(int id, UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			if(user.Access?.MasterData?.DeleteRohArtikelNummer != true)
			{
				return ResponseModel<int>.FailureResponse("User does not have access");
			}
			var response = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Delete(id);
			//logging
			var log = ObjectLogHelper.getLog(user, id, "ROH artikelnumme Level 2 Id", "", "", Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(), Enums.ObjectLogEnums.LogType.Delete);
			Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(log);
			return ResponseModel<int>.SuccessResponse(response);
		}
	}
}