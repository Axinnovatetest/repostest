using Psz.Core.BaseData.Helpers;
using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<ROHArtikelnummerPreviewResponseModel> GetRohArtikelnummerPreview(UserModel user, RohArtikelnummerPreviewRequestModel data)
		{
			try
			{
				if(user == null)
					return ResponseModel<ROHArtikelnummerPreviewResponseModel>.AccessDeniedResponse();
				var levelOneEntity = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level1Access.Get(data.IdLevelOne);

				var partOne = "";
				var partTwo = "";
				var partThree = "";

				var Artikelnummer = "";
				var description = "";

				partOne = levelOneEntity.Part;
				var namesOrderd = new List<RohDescritionBuilderModel>();
				if(data.IdsLevelThree == null || data.IdsLevelThree.Count <= 0)
				{
					// artikelnummer preview
					Artikelnummer = Helpers.ROHHelper.GetAvailableArtikelnummer(new Helpers.RohArtikelnummerPreviewProps
					{
						PartOne = partOne,
						PartTwoDefined = false,
						PartThreeDefined = false,
					});
					// description
					if(data.FreeTextLevelTwoValues != null && data.FreeTextLevelTwoValues.Count > 0)
					{
						var levelTwoEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(data.FreeTextLevelTwoValues.Select(x => x.Key).ToList());
						levelTwoEntities = levelTwoEntities.Where(x => x.IncludeInDescription.HasValue && x.IncludeInDescription.Value).ToList();
						foreach(var item in levelTwoEntities)
						{
							var freeTextEntity = data.FreeTextLevelTwoValues.FirstOrDefault(x => x.Key == item.Id);
							namesOrderd.Add(new RohDescritionBuilderModel(item.OrderInDescription ?? -1, freeTextEntity.Value, item.Sepertor, item.Prefix, item.Suffix));
						}
					}
					// -
					if(data.FreeTextLevelThreeValues != null && data.FreeTextLevelThreeValues.Count > 0)
					{
						var levelTwoEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(data.FreeTextLevelThreeValues.Select(x => x.Key).ToList());
						levelTwoEntities = levelTwoEntities.Where(x => x.IncludeInDescription.HasValue && x.IncludeInDescription.Value).ToList();
						foreach(var item in levelTwoEntities)
						{
							var freeTextEntity = data.FreeTextLevelThreeValues.FirstOrDefault(x => x.Key == item.Id);
							namesOrderd.Add(new RohDescritionBuilderModel(item.OrderInDescription ?? -1, freeTextEntity.Value, item.Sepertor, item.Prefix, item.Suffix));
						}
					}
					// -
					if(data.RangesLevelTwoValues != null && data.RangesLevelTwoValues.Count > 0)
					{
						var levelTwoEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(data.RangesLevelTwoValues.Select(x => x.IdLevelTwo).ToList());
						levelTwoEntities = levelTwoEntities.Where(x => x.IncludeInDescription.HasValue && x.IncludeInDescription.Value).ToList();
						foreach(var item in levelTwoEntities)
						{
							var rangeEntity = data.RangesLevelTwoValues.FirstOrDefault(x => x.IdLevelTwo == item.Id);
							namesOrderd.Add(new RohDescritionBuilderModel(item.OrderInDescription ?? -1, rangeEntity.From != rangeEntity.To ? $"{rangeEntity.From}-{rangeEntity.To}" : $"{rangeEntity.From}", item.Sepertor, item.Prefix, item.Suffix));
						}
					}
				}
				else
				{
					var levelThreeEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level3Access.Get(data.IdsLevelThree);
					// description
					foreach(var item in levelThreeEntities)
					{
						var levelTwoEntity = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(item.IdLevelTwo ?? -1);
						if(item.IncludeInDescription.HasValue && item.IncludeInDescription.Value)
						{
							if(item.IsFreeText.HasValue && item.IsFreeText.Value)
							{
								var freeTextLevelThreeItem = data.FreeTextLevelThreeValues.FirstOrDefault(x => x.Key == item.Id);
								namesOrderd.Add(new RohDescritionBuilderModel(levelTwoEntity.OrderInDescription ?? -1, freeTextLevelThreeItem.Value, levelTwoEntity.Sepertor, levelTwoEntity.Prefix, levelTwoEntity.Suffix));
							}
							else
								namesOrderd.Add(new RohDescritionBuilderModel(levelTwoEntity.OrderInDescription ?? -1, item.Name, levelTwoEntity.Sepertor, levelTwoEntity.Prefix, levelTwoEntity.Suffix));
						}
					}
					if(data.FreeTextLevelTwoValues != null && data.FreeTextLevelTwoValues.Count > 0)
					{
						var levelTwoFreeEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(data.FreeTextLevelTwoValues.Select(x => x.Key).ToList());
						foreach(var item in levelTwoFreeEntities.Where(x => x.IncludeInDescription.HasValue && x.IncludeInDescription.Value))
						{
							var freeTextEntity = data.FreeTextLevelTwoValues.FirstOrDefault(x => x.Key == item.Id);
							namesOrderd.Add(new RohDescritionBuilderModel(item.OrderInDescription ?? -1, freeTextEntity.Value, item.Sepertor, item.Prefix, item.Suffix));
						}
					}
					// -
					if(data.FreeTextLevelThreeValues != null && data.FreeTextLevelThreeValues.Count > 0)
					{
						var levelEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(data.FreeTextLevelThreeValues.Select(x => x.Key).ToList());
						levelEntities = levelEntities.Where(x => x.IncludeInDescription.HasValue && x.IncludeInDescription.Value).ToList();
						foreach(var item in levelEntities)
						{
							var freeTextEntity = data.FreeTextLevelThreeValues.FirstOrDefault(x => x.Key == item.Id);
							namesOrderd.Add(new RohDescritionBuilderModel(item.OrderInDescription ?? -1, freeTextEntity.Value, item.Sepertor, item.Prefix, item.Suffix));
						}
					}
					// -
					if(data.RangesLevelTwoValues != null && data.RangesLevelTwoValues.Count > 0)
					{
						var level2Entities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(data.RangesLevelTwoValues.Select(x => x.IdLevelTwo).ToList());
						level2Entities = level2Entities.Where(x => x.IncludeInDescription.HasValue && x.IncludeInDescription.Value).ToList();
						foreach(var item in level2Entities)
						{
							var rangeEntity = data.RangesLevelTwoValues.FirstOrDefault(x => x.IdLevelTwo == item.Id);
							namesOrderd.Add(new RohDescritionBuilderModel(item.OrderInDescription ?? -1, rangeEntity.From != rangeEntity.To ? $"{rangeEntity.From}-{rangeEntity.To}" : $"{rangeEntity.From}", item.Sepertor, item.Prefix, item.Suffix));
						}
					}
					// artikelnummer preview
					var levelThreeEntitiesIdsLevelTwo = levelThreeEntities.Select(x => x.IdLevelTwo ?? -1).ToList();
					var levelTwoEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(levelThreeEntitiesIdsLevelTwo);
					var impactedLevelTwoEntities = levelTwoEntities.Where(x => x.ImpactNumberGeneration.HasValue && x.ImpactNumberGeneration.Value).ToList();
					var impactionLevelTwoEntitiesIds = impactedLevelTwoEntities.Select(x => x.Id).ToList();
					levelThreeEntities = levelThreeEntities.Where(x => impactionLevelTwoEntitiesIds.Contains(x.IdLevelTwo ?? -1)).ToList();

					var partOrders = levelThreeEntities.Select(x => x.PartOrder).ToList();
					var entitiesWithPartOrderOne = levelThreeEntities.Where(x => x.PartOrder == 1).ToList();
					var entitiesWithPartOrderTwo = levelThreeEntities.Where(x => x.PartOrder == 2).ToList();
					var entitiesWithPartOrderThree = levelThreeEntities.Where(x => x.PartOrder == 3).ToList();
					//
					if(data.FreeTextLevelTwoValues != null && data.FreeTextLevelTwoValues.Count > 0)
					{
						var freetextLevelTwoEntities = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(data.FreeTextLevelTwoValues.Select(x => x.Key).ToList());
						foreach(var entity in freetextLevelTwoEntities)
						{
							if(entity.PartOrder == 1)
								entitiesWithPartOrderOne.Add(new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level3Entity
								{
									PartOrder = entity.PartOrder,
									Part = entity.Part,
								});
							if(entity.PartOrder == 2)
								entitiesWithPartOrderTwo.Add(new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level3Entity
								{
									PartOrder = entity.PartOrder,
									Part = entity.Part,
								});
							if(entity.PartOrder == 3)
								entitiesWithPartOrderThree.Add(new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level3Entity
								{
									PartOrder = entity.PartOrder,
									Part = entity.Part,
								});
						}
					}
					var props = new RohArtikelnummerPreviewProps
					{
						PartThreeDefined = partOrders.Contains(3),
						PartTwoDefined = partOrders.Contains(2),
						PartOne = (entitiesWithPartOrderOne != null && entitiesWithPartOrderOne.Count > 0)
						? entitiesWithPartOrderOne[entitiesWithPartOrderOne.Count - 1].Part
						: partOne,
						PartTow = (entitiesWithPartOrderTwo != null && entitiesWithPartOrderTwo.Count > 0)
						? entitiesWithPartOrderTwo[entitiesWithPartOrderTwo.Count - 1].Part
						: partTwo,
						PartThree = (entitiesWithPartOrderThree != null && entitiesWithPartOrderThree.Count > 0)
						? entitiesWithPartOrderThree[entitiesWithPartOrderThree.Count - 1].Part
						: partThree,
					};
					Artikelnummer = Helpers.ROHHelper.GetAvailableArtikelnummer(props);
				}
				if(levelOneEntity.IncludeInDescription.HasValue && levelOneEntity.IncludeInDescription.Value)
				{
					var descriptionValue = levelOneEntity.ValueInDescription.IsNullOrEmptyOrWitheSpaces() ? levelOneEntity.Name : levelOneEntity.ValueInDescription;
					var orderInDescription = levelOneEntity.OrderInDescription is null ? 0 : levelOneEntity.OrderInDescription ?? 0;
					namesOrderd.Add(new RohDescritionBuilderModel(orderInDescription, descriptionValue, levelOneEntity.Seperator, "", ""));
				}
				if(!levelOneEntity.ValueAtBeginningOfDescription.IsNullOrEmptyOrWitheSpaces())
				{
					namesOrderd.Add(new RohDescritionBuilderModel(-1, levelOneEntity.ValueAtBeginningOfDescription, "", "", ""));
				}
				namesOrderd = namesOrderd.OrderBy(x => x.Order).ToList();
				description = Helpers.ROHHelper.ComposeDescription(namesOrderd);
				return ResponseModel<ROHArtikelnummerPreviewResponseModel>.SuccessResponse(new ROHArtikelnummerPreviewResponseModel { Description = description, Preview = Artikelnummer });

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}