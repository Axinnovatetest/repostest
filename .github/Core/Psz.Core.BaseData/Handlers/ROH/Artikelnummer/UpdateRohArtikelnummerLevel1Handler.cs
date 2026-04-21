using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<int> UpdateRohArtikelnummerLevel1(RohArtikelnummerLevel1Model data, UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			if(user.Access?.MasterData?.EditRohArtikelNummer != true)
			{
				return ResponseModel<int>.FailureResponse("User does not have access");
			}
			var entity = data.ToEntity();
			var response = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level1Access.Update(entity);
			return ResponseModel<int>.SuccessResponse(response);
		}
	}
}