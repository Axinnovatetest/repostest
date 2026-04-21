using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<List<RohArtikelnummerLevel3Model>> GetRohArtikelnummerLevel3(UserModel user, int idLevelOne, int idLevelTwo)
		{
			if(user == null)
				return ResponseModel<List<RohArtikelnummerLevel3Model>>.AccessDeniedResponse();
			var entities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level3Access.GetByLevels(idLevelOne, idLevelTwo);
			var response = entities?.Select(x => new RohArtikelnummerLevel3Model(x)).ToList();
			return ResponseModel<List<RohArtikelnummerLevel3Model>>.SuccessResponse(response);
		}
	}
}