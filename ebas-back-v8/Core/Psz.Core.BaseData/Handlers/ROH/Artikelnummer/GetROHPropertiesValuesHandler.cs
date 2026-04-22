using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<List<ROHPropertiesModel>> GetROHPropertiesValues(UserModel user, int data)
		{
			try
			{
				if(user == null)
					return ResponseModel<List<ROHPropertiesModel>>.AccessDeniedResponse();

				var result = new List<ROHPropertiesModel>();
				var levelTwoEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.GetByLevelOneId(data);
				if(levelTwoEntities != null && levelTwoEntities.Count > 0)
				{
					foreach(var levelTwoEntity in levelTwoEntities)
					{
						var levelThreeEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level3Access.GetByLevels(data, levelTwoEntity.Id);
						var ranges = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesAccess.GetByLevelTwoId(levelTwoEntity.Id);
						result.Add(new ROHPropertiesModel
						{
							IdLevelTwo = levelTwoEntity.Id,
							NameLevelTwo = levelTwoEntity.Name,
							PartLevelTwo = levelTwoEntity.Part,
							Required = levelTwoEntity.Required ?? false,
							IsFreeText = levelTwoEntity.IsFreeText ?? false,
							IsRange = levelTwoEntity.IsRange ?? false,
							ValuesLevelThree = levelThreeEntities?.Select(x => new ValuesLevelThreeModel(x)).OrderBy(x => x.Value).ToList(),
							RangesLevelTwoFrom = ranges?.Where(x => x.FromOrTwo == 1).Select(x => new RangesLevelTwoModel(x)).OrderBy(x => x.Value).ToList(),
							RangesLevelTwoTo = ranges?.Where(x => x.FromOrTwo == 2).Select(x => new RangesLevelTwoModel(x)).OrderBy(x => x.Value).ToList(),
						});
					}
				}
				return ResponseModel<List<ROHPropertiesModel>>.SuccessResponse(result);

			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
