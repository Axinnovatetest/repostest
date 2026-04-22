using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<List<LevelTwoDescriptionOrderModel>> GetPropLevelTwoOrders(UserModel user, int idLevelOne)
		{
			if(user == null)
				return ResponseModel<List<LevelTwoDescriptionOrderModel>>.AccessDeniedResponse();

			try
			{
				var levelTwoEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.GetByLevelOneId(idLevelOne);
				var response = levelTwoEntities?.Select(x => new LevelTwoDescriptionOrderModel
				{
					Id = x.Id,
					Name = x.Name,
					OrderInDescription = x.OrderInDescription ?? -1,
				}).ToList();

				return ResponseModel<List<LevelTwoDescriptionOrderModel>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}