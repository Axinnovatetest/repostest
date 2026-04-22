using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<int> UpdatePropLevelTwoOrders(UserModel user, LevelTwoDescriptionOrderUpdateModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			try
			{
				var levelTwoEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(data.PropsWithOrders.Select(x => x.Id).ToList());
				foreach(var levelTwoEntity in levelTwoEntities)
				{
					var dataEntity = data.PropsWithOrders.FirstOrDefault(x => x.Id == levelTwoEntity.Id);
					levelTwoEntity.OrderInDescription = dataEntity.OrderInDescription;
				}
				var response = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Update(levelTwoEntities);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}