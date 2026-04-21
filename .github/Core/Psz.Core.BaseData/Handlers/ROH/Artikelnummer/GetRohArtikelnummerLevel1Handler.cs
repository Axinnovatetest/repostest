using Psz.Core.BaseData.Interfaces.ROH;
using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer: IRohArtikelnummer
	{
		public ResponseModel<List<RohArtikelnummerLevel1Model>> GetRohArtikelnummerLevel1(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<RohArtikelnummerLevel1Model>>.AccessDeniedResponse();
			var entities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level1Access.Get();
			var response = entities?.Select(x => new RohArtikelnummerLevel1Model(x)).OrderBy(x => x.Name).ToList();
			return ResponseModel<List<RohArtikelnummerLevel1Model>>.SuccessResponse(response);
		}
	}
}