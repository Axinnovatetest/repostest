using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<List<RohArtikelnummerLevel2Model>> GetRohArtikelnummerLevel2(UserModel user, int idLevelOne)
		{
			if(user == null)
				return ResponseModel<List<RohArtikelnummerLevel2Model>>.AccessDeniedResponse();
			var entities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.GetByLevelOneId(idLevelOne);
			var response = entities?.Select(x => new RohArtikelnummerLevel2Model(x)).ToList();
			return ResponseModel<List<RohArtikelnummerLevel2Model>>.SuccessResponse(response);
		}
	}
}