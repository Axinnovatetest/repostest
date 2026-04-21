using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<int> AddLevelTwoRangeValue(UserModel user, ROHLevelTwoRangesModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var entity = data.ToEntity();
			var response = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesAccess.Insert(entity);
			return ResponseModel<int>.SuccessResponse(response);
		}

		public ResponseModel<int> UpdateLevelTwoRangeValue(UserModel user, ROHLevelTwoRangesModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var entity = data.ToEntity();
			var response = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesAccess.Update(entity);
			return ResponseModel<int>.SuccessResponse(response);
		}

		public ResponseModel<int> DeleteLevelTwoRangeValue(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var response = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesAccess.Delete(id);
			return ResponseModel<int>.SuccessResponse(response);
		}

		public ResponseModel<List<ROHLevelTwoRangesModel>> GetROHLevelTwoRangesById(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<List<ROHLevelTwoRangesModel>>.AccessDeniedResponse();

			var entities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesAccess.GetByLevelTwoId(id);
			var response = entities?.Select(x => new ROHLevelTwoRangesModel(x)).ToList();
			return ResponseModel<List<ROHLevelTwoRangesModel>>.SuccessResponse(response);
		}
	}
}