using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class NeededQuantityOrderedQuantityHandler: IHandle<NeededQuantityOrderedQuantityRequestModel, ResponseModel<NeededAndOrderedQunatityWithStockResponseModel>>
	{
		private NeededQuantityOrderedQuantityRequestModel data { get; set; }
		private UserModel user { get; set; }

		public NeededQuantityOrderedQuantityHandler(UserModel user, NeededQuantityOrderedQuantityRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<NeededAndOrderedQunatityWithStockResponseModel> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}
				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private ResponseModel<NeededAndOrderedQunatityWithStockResponseModel> Perform(UserModel user, NeededQuantityOrderedQuantityRequestModel data)
		{
			var fetchedneed = Infrastructure.Data.Access.Joins.MTM.Order.NeededQuantityInFasAccess.GetNeededQuantityInFas4(Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryID, data.UnitId), data.Months, data.ArtikelNr);
			var fetchedorders = Infrastructure.Data.Access.Joins.MTM.Order.OrderedArticlesAccess.GetOrderedQuantitiy(Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryID, data.UnitId), data.Months, data.ArtikelNr);
			var fetchedStock = Infrastructure.Data.Access.Tables.MTM.LagerorteAccess.GetActualStockInLagerPerArticleByType(
				Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryID, data.UnitId)
				, data.ArtikelNr
				, Psz.Core.MaterialManagement.Helpers.SpecialHelper.GetMainAndProductionLagers(data.CountryID, data.UnitId).Item1,
				Psz.Core.MaterialManagement.Helpers.SpecialHelper.GetMainAndProductionLagers(data.CountryID, data.UnitId).Item2
				);
			var fetchedMinStock = Infrastructure.Data.Access.Tables.MTM.LagerorteAccess.GetMinimumStockInLagerPerArticle(Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryID, data.UnitId), data.ArtikelNr);
			if(fetchedneed is null && fetchedorders is null)
				return ResponseModel<NeededAndOrderedQunatityWithStockResponseModel>.NotFoundResponse();
			var emptyMerger = new List<MergedQuantityAndOrdersResponseModel>();
			var resulttoret = new List<MergedQuantityAndOrdersResponseModel>();
			// make empty list of merged data
			var date = DateTime.Now;
			while(date <= DateTime.Now.AddMonths(6))
			{
				emptyMerger.Add(new MergedQuantityAndOrdersResponseModel(date));
				date = date.AddDays(7);
			}
			// add the quantity to merge
			foreach(var item in fetchedneed)
			{
				resulttoret.Add(new MergedQuantityAndOrdersResponseModel(item));
			}
			// create seprte set of orders same form as merged form 
			var orders = new List<MergedQuantityAndOrdersResponseModel>();
			foreach(var item in fetchedorders)
			{
				orders.Add(new MergedQuantityAndOrdersResponseModel(item));
			}
			foreach(var item in orders.ToList())
			{
				foreach(var item2 in resulttoret)
				{
					if(Psz.Core.MaterialManagement.Helpers.SpecialHelper.CompareWeekPattern(item.Week, item2.Week))
					{
						if(orders.IndexOf(item) >= 0)
						{
							item2.orderedQuantity = item.orderedQuantity;
							orders.RemoveAt(orders.IndexOf(item));
						}
					}
				}
			}
			if(orders.Count() > 0 && orders is not null)
			{
				resulttoret = resulttoret.Concat(orders).ToList();
			}
			foreach(var item in emptyMerger.ToList())
			{
				foreach(var item2 in resulttoret)
				{
					if(Psz.Core.MaterialManagement.Helpers.SpecialHelper.CompareWeekPattern(item.Week, item2.Week))
					{
						if(emptyMerger.IndexOf(item) >= 0)
						{
							emptyMerger.RemoveAt(emptyMerger.IndexOf(item));
						}

					}
				}
			}
			if(emptyMerger.Count > 0)
			{
				resulttoret = resulttoret.Concat(emptyMerger).ToList();
			}
			resulttoret.Sort();
			var supplierArtikle = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetByArticleIdDefaultSupplier(data.ArtikelNr);
			var newResponse = new NeededAndOrderedQunatityWithStockResponseModel(resulttoret, fetchedStock.Stock ?? 0, fetchedMinStock.Mindestbestand ?? 0, supplierArtikle?.Mindestbestellmenge ?? 0, supplierArtikle?.Wiederbeschaffungszeitraum ?? 0);
			return ResponseModel<NeededAndOrderedQunatityWithStockResponseModel>.SuccessResponse(newResponse);
		}
		public ResponseModel<NeededAndOrderedQunatityWithStockResponseModel> Validate()
		{
			if(this.user is null)
			{
				return ResponseModel<NeededAndOrderedQunatityWithStockResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<NeededAndOrderedQunatityWithStockResponseModel>.SuccessResponse();
		}
	}
}
