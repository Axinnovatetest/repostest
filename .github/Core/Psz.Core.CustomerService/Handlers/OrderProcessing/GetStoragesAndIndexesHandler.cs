using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class GetStoragesAndIndexesHandler: IHandle<Identity.Models.UserModel, ResponseModel<StoragesAndIndexesModel>>
	{

		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetStoragesAndIndexesHandler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<StoragesAndIndexesModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var KundenIndexList = new List<StoragesAndIndexesModel.KundeIndex>();
				var StorageLocations = new List<StoragesAndIndexesModel.StorageLocation>();
				var articleTypes = new List<StoragesAndIndexesModel.ArticleType> { new StoragesAndIndexesModel.ArticleType { Key = 3, Value = "Serie" } };
				var _showed = Module.Hauplager;
				//new List<int> { 3, 4, 8, 13, 15, 24, 41, 58, 825, 928 };

				//storages
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data ?? "");
				var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { articleEntity?.ArtikelNr ?? -1 }, false);
				var lagerExtEntities = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleAndLagerIds(articleEntity?.ArtikelNr ?? -1, _showed);
				var lagerortEntities = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get() ?? new List<Infrastructure.Data.Entities.Tables.INV.LagerorteEntity>();
				//if (lagerExtEntities != null && lagerExtEntities.Count > 0)
				//{
				//    foreach (var item in lagerExtEntities.OrderBy(x => x.Lagerort_id))
				//    {
				//        var lagarName = lagerortEntities.Where(x => x.LagerortId == item.Lagerort_id).FirstOrDefault()?.Lagerort;
				//        StorageLocations.Add(new StoragesAndIndexesModel.StorageLocation
				//        {
				//            Key = item.Lagerort_id,
				//            Value = $"{lagarName} || {item.Bestand?.ToString("0.###")}",
				//            KundenIndex = item.Index_Kunde
				//        });
				//    }
				//}


				var _lastIndex = CreateOrderItemHandler.GetLastIndex(articleEntity);
				lagerEntities = lagerEntities?.Where(x => _showed.Exists(y => y == x?.Lagerort_id))?.ToList();
				if(lagerEntities != null && lagerEntities.Count > 0)
				{
					foreach(var item in lagerEntities.OrderBy(x => x.Lagerort_id))
					{
						var lagarName = lagerortEntities.Where(x => x.LagerortId == item.Lagerort_id).FirstOrDefault()?.Lagerort;
						StorageLocations.Add(new StoragesAndIndexesModel.StorageLocation
						{
							Key = item.Lagerort_id ?? -1,
							Value = $"{lagarName} || {item.Bestand?.ToString("0.###")}",
							KundenIndex = _lastIndex.KundenIndex
						});
					}

					// - 2022-07-29 - add old INdexes w empty Bestand
					var oldIndexes = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetKundenIndexByArticle(articleEntity.ArtikelNr)
						?.Where(x => x?.Trim() != _lastIndex.KundenIndex?.Trim())?.ToList();
					foreach(var item in lagerEntities.OrderBy(x => x.Lagerort_id))
					{
						var lagarName = lagerortEntities.Where(x => x.LagerortId == item.Lagerort_id).FirstOrDefault()?.Lagerort;
						foreach(var x in oldIndexes)
						{
							StorageLocations.Add(new StoragesAndIndexesModel.StorageLocation
							{
								Key = item.Lagerort_id ?? -1,
								Value = $"{lagarName} || 0.000",
								KundenIndex = x
							});
						}
					}
				}
				//indexes
				if(articleEntity != null)
				{
					KundenIndexList.AddRange(
						Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetKundenIndexSnapshotTimeByArticle(articleEntity.ArtikelNr)
						?.Select(x => new StoragesAndIndexesModel.KundeIndex(x)));

					// - 2022-06-14 - only add Current Article Index, if no BOM
					if(KundenIndexList.Count == 0) // (KundenIndexList.FindIndex(x => x.Value == articleEntity.Index_Kunde) <= 0)
					{
						KundenIndexList.Add(new StoragesAndIndexesModel.KundeIndex
						{
							Key = articleEntity.Index_Kunde_Datum,
							Value = articleEntity.Index_Kunde,
							SnapshotTime = DateTime.MaxValue
						});
					}
				}
				// - Article Types 
				if(articleEntity != null)
				{
					articleTypes =
					// - Heidenreich
					//Enum.GetValues(typeof(Enums.OrderEnums.ItemType)).Cast<Enums.OrderEnums.ItemType>().ToList()
					//?.Select(x => new StoragesAndIndexesModel.ArticleType { Key = (int)x, Value = x.GetDescription() })?.ToList();
					(Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNr(articleEntity.ArtikelNr)
					?? new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>())
					?.Select(x => new StoragesAndIndexesModel.ArticleType
					{
						Key = (int)Enums.OrderEnums.GetItemType(x.ArticleSalesType)/*x.ArticleSalesTypeId ?? 0*/, // - 2023-10-23 - Types from MTD & CTS do NOT match, NEED conversion!!!!
						Value = Enums.OrderEnums.GetItemType(x.ArticleSalesType).GetDescription()
					})?.ToList();
				}
				// -
				KundenIndexList = KundenIndexList?.DistinctBy(x => x.Value)?.OrderByDescending(x => x.Key)?.ToList();
				var response = new StoragesAndIndexesModel { KundenIndexList = KundenIndexList, storageLocations = StorageLocations, ArticleTypeList = articleTypes?.DistinctBy(x => x.Key)?.ToList() };

				return ResponseModel<StoragesAndIndexesModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<StoragesAndIndexesModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<StoragesAndIndexesModel>.AccessDeniedResponse();
			}

			return ResponseModel<StoragesAndIndexesModel>.SuccessResponse();
		}
	}
}
