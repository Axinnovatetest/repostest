using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;


namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<int> AddRohArtikelnummerLevel2(RohArtikelnummerLevel2Model data, UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			var exsist = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.GetByLevelOneIdAndName(data.IdLevelOne ?? -1, data.Name);
			if(exsist != null)
				return ResponseModel<int>.FailureResponse("Property part type with the same details already exsists.");

			if(user.Access?.MasterData?.AddRohArtikelNummer != true)
			{
				return ResponseModel<int>.FailureResponse("User does not have access");
			}

			var entity = data.ToEntity();
			var maxOrderInDescription = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.GetMaxOrderInDescrition(data.IdLevelOne ?? -1);
			entity.OrderInDescription = maxOrderInDescription == 0
				? 1
				: maxOrderInDescription + 1;
			var response = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Insert(entity);
			//Logging 
			Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
	new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> {
									ObjectLogHelper.getLog(user, response, "ROH Artikel level 2 name :",
									"",
									$"{data.Name}",
									Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(),
									Enums.ObjectLogEnums.LogType.Add),
									ObjectLogHelper.getLog(user,response, "ROH Artikel Level one name:",
									"",
									$"{data.NameLevelOne}",
									Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(),
									Enums.ObjectLogEnums.LogType.Add),
									ObjectLogHelper.getLog(user,response, "ROH Artikel Id level one :",
									"",
									$"{data.IdLevelOne}",
									Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(),
									Enums.ObjectLogEnums.LogType.Add),
									ObjectLogHelper.getLog(user,response, "ROH Artikel Part number level one :",
									"",
									$"{data.PartNumberLevelOne}",
									Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(),
									Enums.ObjectLogEnums.LogType.Add)
	});
			return ResponseModel<int>.SuccessResponse(response);
		}
	}
}