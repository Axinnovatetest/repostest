using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Interfaces.ROH
{
	public interface IRohArtikelnummer
	{
		ResponseModel<List<RohArtikelnummerLevel1Model>> GetRohArtikelnummerLevel1(UserModel user);
		ResponseModel<List<RohArtikelnummerLevel2Model>> GetRohArtikelnummerLevel2(UserModel user, int idLevelOne);
		ResponseModel<List<RohArtikelnummerLevel3Model>> GetRohArtikelnummerLevel3(UserModel user, int idLevelOne, int idLevelTwo);

		ResponseModel<int> AddRohArtikelnummerLevel1(RohArtikelnummerLevel1Model data, UserModel user);
		ResponseModel<int> AddRohArtikelnummerLevel2(RohArtikelnummerLevel2Model data, UserModel user);
		ResponseModel<int> AddRohArtikelnummerLevel3(RohArtikelnummerLevel3Model data, UserModel user);

		ResponseModel<int> UpdateRohArtikelnummerLevel1(RohArtikelnummerLevel1Model data, UserModel user);
		ResponseModel<int> UpdateRohArtikelnummerLevel2(RohArtikelnummerLevel2Model data, UserModel user);
		ResponseModel<int> UpdateRohArtikelnummerLevel3(RohArtikelnummerLevel3Model data, UserModel user);

		ResponseModel<int> DeleteRohArtikelnummerLevel1(int id, UserModel user);
		ResponseModel<int> DeleteRohArtikelnummerLevel2(int id, UserModel user);
		ResponseModel<int> DeleteRohArtikelnummerLevel3(int id, UserModel user);
		ResponseModel<List<ROHPropertiesModel>> GetROHPropertiesValues(UserModel user, int idLevelOne);
		ResponseModel<ROHArtikelnummerPreviewResponseModel> GetRohArtikelnummerPreview(UserModel user, RohArtikelnummerPreviewRequestModel data);
		ResponseModel<List<LevelTwoDescriptionOrderModel>> GetPropLevelTwoOrders(UserModel user, int idLevelOne);
		ResponseModel<int> UpdatePropLevelTwoOrders(UserModel user, LevelTwoDescriptionOrderUpdateModel data);

		ResponseModel<int> AddLevelTwoRangeValue(UserModel user, ROHLevelTwoRangesModel data);
		ResponseModel<int> UpdateLevelTwoRangeValue(UserModel user, ROHLevelTwoRangesModel data);
		ResponseModel<int> DeleteLevelTwoRangeValue(UserModel user, int id);
		ResponseModel<List<ROHLevelTwoRangesModel>> GetROHLevelTwoRangesById(UserModel user, int id);
	}
}