using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Create
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetArticleNumberForCreateHandler: IHandle<Identity.Models.UserModel, ResponseModel<string>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.CreateRequestModel _data { get; set; }

		public GetArticleNumberForCreateHandler(Identity.Models.UserModel user, Models.Article.CreateRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<string> Handle()
		{

			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var itemParts = this._data.ArticleNumber?.Trim().Split('-')?.ToList();
				var newIndexSequence = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerNextIndexSequence(this._data.CustomerNumber, itemParts[0], this._data.CustomerItemNumber);
				var countryEntity = Infrastructure.Data.Access.Tables.WPL.CountryAccess.GetByCode(this._data.ProductionCountryCode);
				var siteEntity = Infrastructure.Data.Access.Tables.WPL.HallAccess.GetByCode(this._data.ProductionSiteCode);
				var rohGoodsGroupEntity = (Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get()
					?? new List<Infrastructure.Data.Entities.Tables.PRS.WarengruppenEntity>()).Find(x => x.Warengruppe.Trim().ToLower() == "roh");
				int customerNumberSeq = 0;
				int customerIndexSeq = 0;

				#region >>> Data sequences <<<
				var maxCustomerSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerMaxNumberSequence(this._data.CustomerNumber, itemParts[0]);
				if(maxCustomerSeq < 0)
				{
					// - no articles for Customer
				}
				else
				{
					// - current ItemNumber
					customerNumberSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerNumberSequence(this._data.CustomerNumber, itemParts[0], this._data.CustomerItemNumber);
					if(customerNumberSeq < 0)
					{
						// - current ItemNumber does not exist
						customerNumberSeq = maxCustomerSeq + 1;
					}
					else
					{
						// - current Index
						customerIndexSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerIndexSequence(this._data.CustomerNumber, itemParts[0], this._data.CustomerItemNumber, this._data.CustomerItemIndex);
						if(customerIndexSeq < 0)
						{
							// - current Index does not exist
							customerIndexSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerNextIndexSequence(this._data.CustomerNumber, itemParts[0], this._data.CustomerItemNumber);
						}
					}
				}
				#endregion

				var newArticleNumber = Data.UpdateCustomerIndexHandler.getNewArticleNumber(this._data.CustomerPrefix, customerNumberSeq,
					customerIndexSeq, this._data.ProductionSiteCode, this._data.ProductionCountryCode);
				return ResponseModel<string>.SuccessResponse(newArticleNumber);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<string> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}

			#region all Articles
			//if (Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber) != null)
			//{
			//    return ResponseModel<string>.FailureResponse($"Article Number [{this._data.ArticleNumber}] exists.");
			//}
			//if (Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get(this._data.GoodsGroupId) == null)
			//{
			//    return ResponseModel<string>.FailureResponse($"Goods Group (Warengruppe) [{this._data.GoodsGroupName}] not found.");
			//}
			//var itemParts = this._data.ArticleNumber?.Trim().Split('-')?.ToList();
			//if (itemParts == null || itemParts.Count < 3)
			//{
			//    return ResponseModel<string>.FailureResponse($"Article Number structure invalid.");
			//}
			#endregion - all Articles

			#region ROH
			//var rohGoodsGroupEntity = (Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get()
			//    ?? new List<Infrastructure.Data.Entities.Tables.PRS.WarengruppenEntity>()).Find(x => x.Warengruppe.Trim().ToLower() == "roh");
			//if (this._data.GoodsGroupName.Trim().ToLower() == rohGoodsGroupEntity?.Warengruppe?.Trim().ToLower()
			//    && !this._data.GoodsTypeId.HasValue)
			//    return ResponseModel<string>.FailureResponse("Article ROH must have a [Waren Type].");
			#endregion - ROH

			// -
			var customers = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByKundeKreis(this._data.CustomerPrefix);

			// - 2022-06-08
			//if (this._data.GoodsGroupName?.Trim()?.ToLower() == "ef")
			{
				#region --- prems ---
				if(/*this._data.ProductionCountryCode != null && */Infrastructure.Data.Access.Tables.WPL.CountryAccess.GetByCode(this._data.ProductionCountryCode) == null)
				{
					return ResponseModel<string>.FailureResponse($"Country [{this._data.ProductionCountryName}] not found.");
				}
				if(/*this._data.ProductionSiteCode != null && */Infrastructure.Data.Access.Tables.WPL.HallAccess.GetByCode(this._data.ProductionSiteCode) == null)
				{
					return ResponseModel<string>.FailureResponse($"Hall [{this._data.ProductionSiteName}] not found.");
				}

				if(customers == null || customers.Count <= 0)
				{
					return ResponseModel<string>.FailureResponse($"EF Article Nummerschlüssel [{this._data.CustomerPrefix}] does not exist.");
				}

				// - 1 - Customer
				if(Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get(this._data.CustomerId) == null)
				{
					return ResponseModel<string>.FailureResponse("Customer not found in Nummerschlüssel.");
				}

				if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(this._data.CustomerNumber) == null)
				{
					return ResponseModel<string>.FailureResponse("Customer not found in Adressen.");
				}

				// - 2 - ItemNumber
				if(string.IsNullOrWhiteSpace(this._data.CustomerItemNumber))
				{
					return ResponseModel<string>.FailureResponse($"Customer Item Number invalid value [{this._data.CustomerItemNumber}].");
				}
				// - 3 - Index
				if(string.IsNullOrWhiteSpace(this._data.CustomerItemIndex))
				{
					return ResponseModel<string>.FailureResponse($"Customer Item Index invalid value [{this._data.CustomerItemIndex}]");
				}
				#endregion prems

				//var itemNumberSeq = itemParts[1];
				//var itemIndexSeq = itemParts[2]?.Substring(0, 2);
				//var warehouseSeq = itemParts[2]?.Length > 2 ? itemParts[2]?.Substring(2, 2) : "";
				//var countrySeq = itemParts[2]?.Length > 4 ? itemParts[2]?.Substring(4) : "";
				// - Customer ItemNumber
				var sameCustomerItems = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(this._data.CustomerNumber, _data.CustomerPrefix, this._data.CustomerItemNumber);
				if(sameCustomerItems == null || sameCustomerItems.Count <= 0)
				{
					// - new CustomerItem - do nothing
				}
				else // - old CustomerItem
				{
					// - check CustomerIndex
					var sameCustomerIndex = sameCustomerItems.Where(x => x.CustomerIndex == this._data.CustomerItemIndex).ToList();
					if(sameCustomerIndex == null || sameCustomerIndex.Count <= 0)
					{
						// - new CustomerIndex - do nothing
					}
					else // - old CustomerIndex
					{
						// - checks unique <Warehouse-Country>
						//var sameData = sameCustomerIndex.FirstOrDefault(x => x.ProductionCountryName?.Trim()?.ToLower() == this._data.ProductionCountryName?.Trim()?.ToLower()
						//    && x.ProductionSiteName?.Trim()?.ToLower() == this._data.ProductionSiteName?.Trim()?.ToLower());
						//if (sameData != null)
						//{
						//    return ResponseModel<string>.FailureResponse($"Another article with same values exists [{sameData.ArtikelNummer}]");
						//}
					}
				}
			}
			//else
			//{
			//    // - not EF article

			//    // - should not start with a Customer kreis
			//    if (customers != null && customers.Count > 0)
			//    {
			//        return ResponseModel<string>.FailureResponse(new List<string> { $"Kreis [{kreis}] exists for customers [{string.Join(", ", customers.Take(5).Select(x => $"{x.Kundennummer} | {x.Kunde}"))}].", $"Change article to EF or use another Nummerkreis." });
			//    }
			//}

			// -
			return ResponseModel<string>.SuccessResponse();
		}
	}
}
