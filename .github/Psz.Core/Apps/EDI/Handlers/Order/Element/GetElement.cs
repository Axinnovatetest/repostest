using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public static Models.Order.Element.OrderElementModel GetElement(int elementId, bool forUpdate = false)
		{
			try
			{
				var element = GetElements(new List<int>() { elementId }).FirstOrDefault();
				if(forUpdate)
				{
					var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(element.OrderId);
					element.StoragesAndIndexes = GetStoragesAndIndexes(element.ItemId);
					element.LinkToABPosition = GetLinkToAB(element.ItemId, Convert.ToInt32(orderEntity.Kunden_Nr), Convert.ToInt32( element.OpenQuantity_Quantity));
				}
				return element;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static CustomerService.Models.OrderProcessing.StoragesAndIndexesModel GetStoragesAndIndexes(int articleId)
		{
			try
			{
				var KundenIndexList = new List<CustomerService.Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex>();
				var StorageLocations = new List<CustomerService.Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation>();
				var articleTypes = new List<CustomerService.Models.OrderProcessing.StoragesAndIndexesModel.ArticleType> { new CustomerService.Models.OrderProcessing.StoragesAndIndexesModel.ArticleType { Key = 3, Value = "Serie" } };
				var _showed = CustomerService.Module.Hauplager;

				//storages
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleId);
				var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { articleEntity?.ArtikelNr ?? -1 }, false);
				var lagerExtEntities = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleAndLagerIds(articleEntity?.ArtikelNr ?? -1, _showed);
				var lagerortEntities = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get() ?? new List<Infrastructure.Data.Entities.Tables.INV.LagerorteEntity>();

				var _lastIndex = CustomerService.Handlers.OrderProcessing.CreateOrderItemHandler.GetLastIndex(articleEntity);
				lagerEntities = lagerEntities?.Where(x => _showed.Exists(y => y == x?.Lagerort_id))?.ToList();
				if(lagerEntities != null && lagerEntities.Count > 0)
				{
					foreach(var item in lagerEntities.OrderBy(x => x.Lagerort_id))
					{
						var lagarName = lagerortEntities.Where(x => x.LagerortId == item.Lagerort_id).FirstOrDefault()?.Lagerort;
						StorageLocations.Add(new CustomerService.Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation
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
							StorageLocations.Add(new CustomerService.Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation
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
						?.Select(x => new CustomerService.Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex(x)));

					// - 2022-06-14 - only add Current Article Index, if no BOM
					if(KundenIndexList.Count == 0) // 
					{
						KundenIndexList.Add(new CustomerService.Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex
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
					(Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNr(articleEntity.ArtikelNr)
					?? new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>())
					?.Select(x => new CustomerService.Models.OrderProcessing.StoragesAndIndexesModel.ArticleType
					{
						Key = (int)CustomerService.Enums.OrderEnums.GetItemType(x.ArticleSalesType)/*x.ArticleSalesTypeId ?? 0*/, // - 2023-10-23 - Types from MTD & CTS do NOT match, NEED conversion!!!!
						Value = CustomerService.Enums.OrderEnums.GetItemType(x.ArticleSalesType).GetDescription()
					})?.ToList();
				}
				// -
				KundenIndexList = KundenIndexList?.DistinctBy(x => x.Value)?.OrderByDescending(x => x.Key)?.ToList();
				return new CustomerService.Models.OrderProcessing.StoragesAndIndexesModel { KundenIndexList = KundenIndexList, storageLocations = StorageLocations, ArticleTypeList = articleTypes?.DistinctBy(x => x.Key)?.ToList() };

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static List<CustomerService.Models.Blanket.LinkToABPositionModel> GetLinkToAB(int articleId, int customerId, int quantity)
		{
			try
			{
				var abEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetRahmenLinkAB(articleId, customerId, quantity).OrderBy(x => x.Position).ToList();
				List<CustomerService.Models.Blanket.LinkToABPositionModel> response = new List<CustomerService.Models.Blanket.LinkToABPositionModel>();

				if(abEntity != null && abEntity.Count > 0)
					response = abEntity.Select(x => new CustomerService.Models.Blanket.LinkToABPositionModel(x)).ToList();

				return response;
			} catch
			{
				throw;
			}
		}
	}
}
