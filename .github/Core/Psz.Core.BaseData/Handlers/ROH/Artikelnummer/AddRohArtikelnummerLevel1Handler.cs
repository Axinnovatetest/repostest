using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<int> AddRohArtikelnummerLevel1(RohArtikelnummerLevel1Model data, UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			var exsist = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level1Access.GetByNamePartOrderNadPart(data.Name, data.PartOrder ?? 1, data.Part);
			if(exsist != null)
				return ResponseModel<int>.FailureResponse("Article type with the same details already exsists.");

			if(user.Access?.MasterData?.AddRohArtikelNummer != true)
			{
				return ResponseModel<int>.FailureResponse("User does not have access");
			}
			var entity = data.ToEntity();
			var response = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level1Access.Insert(entity);
			//Logging 
			var log = ObjectLogHelper.getLog(user, response, "ROH Artikel nummer level 1", "", data.Name, Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(), Enums.ObjectLogEnums.LogType.Add);
			Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(log);

			return ResponseModel<int>.SuccessResponse(response);
		}
	}
}