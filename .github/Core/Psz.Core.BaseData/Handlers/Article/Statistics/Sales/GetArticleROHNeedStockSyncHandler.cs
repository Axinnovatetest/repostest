using System;
using System.Collections.Generic;


namespace Psz.Core.BaseData.Handlers.Article.Statistics.Sales
{
	using Infrastructure.Data.Entities.Tables.BSD;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Diagnostics;
	using System.Globalization;
	using System.IO;
	using System.Linq;

	public class GetArticleROHNeedStockSyncHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Sales.ArticleROHNeedStockRequestModel _data { get; set; }
		public GetArticleROHNeedStockSyncHandler(UserModel user, Models.Article.Statistics.Sales.ArticleROHNeedStockRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncResponseModel> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//-
				var supplierNames = new List<string>();
				if(this._data.AdressenNr != null && this._data.AdressenNr > 0)
				{
					var supplier = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.AdressenNr.Value);
					if(supplier != null)
					{
						supplierNames.Add(supplier.Name1);
					}
				}
				else
				{
					var suppliers = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByStufe(this._data.Stufe);
					if(suppliers != null)
					{
						supplierNames.AddRange(suppliers.Select(x => x.Name1));
					}
				}

				var responseBody = new List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>();
				var count = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock_Header_count(supplierNames, this._data.ArticleNumber, this._data.Fa30Postive, this._data.OnlyPrioSupplier, _data.ArticleClassification);
				if(count > 0)
				{
					var _ = new Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel();
					responseBody = getResponseBody(supplierNames, out _);
				}

				// -
				return ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncResponseModel>.SuccessResponse(
					new Models.Article.Statistics.Sales.ArticleROHNeedStockSyncResponseModel
					{
						Items = responseBody,
						PageRequested = this._data.RequestedPage,
						PageSize = this._data.PageSize,
						TotalCount = count > 0 ? count : 0,
						TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(count > 0 ? count : 0) / this._data.PageSize)) : 0,
					});
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		private List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel> getResponseBody(List<string> supplierNames, out Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel labels, bool paged = true)
		{
			labels = new Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel();
			List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel> responseBody;
			#region > Data sorting & paging
			var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
			{
				FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
				RequestRows = this._data.PageSize
			};

			Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
			if(!string.IsNullOrWhiteSpace(this._data.SortField))
			{
				var sortFieldName = "";
				switch(this._data.SortField.ToLower())
				{
					default:
					case "mindestbestellmenge":
						sortFieldName = "m.[Mindestbestellmenge]";
						break;
					case "lieferzeit":
						sortFieldName = "m.[Lieferzeit]";
						break;
					case "ek":
						sortFieldName = "m.[EK]";
						break;
					case "vpe_losgroesse":
						sortFieldName = "m.[VPE_Losgroesse]";
						break;
					case "lieferantartikelnummer":
						sortFieldName = "m.[LieferantArtikelnummer]";
						break;
					case "priO1_lieferant":
						sortFieldName = "m.[PRIO1_Lieferant]";
						break;
					case "artikelnummer":
						sortFieldName = "m.[Artikelnummer]";
						break;
					case "gesamtbedarfoffenefa360":
						sortFieldName = "m.[GesamtbedarfOffeneFA360]";
						break;
					case "verfugbarbestand":
						sortFieldName = "m.[Verfugbarbestand]";
						break;
					case "lagerbestand":
						sortFieldName = "m.[Lager]";
						break;
					case "min_lagerbestand":
						sortFieldName = "m.[Min_Lagerbestand]";
						break;
					case "bedarfpo":
						sortFieldName = "m.[BedarfPO]";
						break;
					case "articleclassification":
						sortFieldName = "a.[artikelklassifizierung]";
						break;
					case "description2":
						sortFieldName = "a.[Bezeichnung 2]";
						break;
				}

				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = this._data.SortDesc,
				};
			}

			#endregion
			if(!paged)
			{
				dataPaging = null;
			}
			responseBody = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock_Header(supplierNames, this._data.ArticleNumber, this._data.Fa30Postive, this._data.OnlyPrioSupplier, _data.ArticleClassification, dataSorting, dataPaging)
						?.Select(x => new Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel(x)).ToList()
						?? new List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>();

			var articleNrs = responseBody?.Select(x => x.FaNeedsArtikelNr)?.ToList();
			var poEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock_Po(responseBody.Count > 0 ? responseBody[0].SyncId : -1, articleNrs)
				?? new List<MaterialRequirementsQuantityEntity>();
			var faEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock_Fa(responseBody.Count > 0 ? responseBody[0].SyncId : -1, articleNrs)
				?? new List<MaterialRequirementsQuantityEntity>();
			// - 
			var currentCW = ISOWeek.GetWeekOfYear(DateTime.Today);
			var maxCW = ISOWeek.GetWeekOfYear(new DateTime(DateTime.Today.Year, 12, 31));
			var currYear = DateTime.Today.Year;
			var nextYear = DateTime.Today.Year + 1;

			maxCW = maxCW <= 1 ? 52 : maxCW;

			#region Labels
			int i = 0;
			labels.Label_CW1 = $"{currYear}/KW{currentCW}";
			for(int j = 1; j <= 52; j++)
			{
				var labelProperty = labels.GetType().GetProperty($"Label_CW{j + 1}");
				var labelValue = j < 52
					? currentCW + j <= maxCW ? $"{currYear}/KW{currentCW + j}" : $"{nextYear}/KW{currentCW + j - maxCW}"
					: $"{currYear}/KW{(maxCW - (currentCW + j))}";
				labelProperty.SetValue(labels, labelValue);
			}
			//i++;
			//labels.Label_CW2 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW3 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW4 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW5 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW6 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW7 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW8 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW9 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW10 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW11 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW12 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW13 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW14 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW15 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW16 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW17 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW18 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW19 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW20 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW21 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW22 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW23 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW24 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW25 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW26 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW27 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW28 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW29 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW30 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW31 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW32 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW33 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW34 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW35 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW36 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW37 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW38 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW39 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW40 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW41 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW42 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW43 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW44 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW45 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW46 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW47 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW48 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW49 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW50 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW51 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW52 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{currentCW + i - maxCW}";
			//i++;
			//labels.Label_CW53 = currentCW + i <= maxCW ? $"{currYear}/KW{currentCW + i}" : $"{nextYear}/KW{(maxCW - (currentCW + i))}";
			#endregion labels
			if(responseBody != null && responseBody.Count > 0)
				foreach(var item in responseBody)
				{
					// - Sum
					item.SummePO = poEntities?.Where(x => x.ArtikelNr == item.BsQuantityArtikelNr)?.Select(x => x.Quantity)?.Sum() ?? 0;
					item.BedarfPO = faEntities.Where(x => x.ArtikelNr == item.BsQuantityArtikelNr)?.Select(x => x.Quantity)?.Sum() ?? 0;
					// - Ruckstand
					item.PoDelay = poEntities.Where(x => x.ArtikelNr == item.BsQuantityArtikelNr && (x.Year < DateTime.Today.Year || x.Year == DateTime.Today.Year && x.CW < currentCW))?.Select(x => x.Quantity)?.Sum() ?? 0;
					item.FaDelay = faEntities.Where(x => x.ArtikelNr == item.BsQuantityArtikelNr && (x.Year < DateTime.Today.Year || x.Year == DateTime.Today.Year && x.CW < currentCW))?.Select(x => x.Quantity)?.Sum() ?? 0;

					// -
					setPoData(currentCW, labels, item, poEntities.Where(x => x.ArtikelNr == item.BsQuantityArtikelNr && (x.Year > DateTime.Today.Year || x.Year == DateTime.Today.Year && x.CW >= currentCW))?.OrderBy(x => x.Year)?.ThenBy(x => x.CW)?.ToList()
						, faEntities.Where(x => x.ArtikelNr == item.BsQuantityArtikelNr && (x.Year > DateTime.Today.Year || x.Year == DateTime.Today.Year && x.CW >= currentCW))?.OrderBy(x => x.Year)?.ThenBy(x => x.CW)?.ToList());
				}
			// - set labels
			setPoLabels(labels, responseBody.Count > 0 ? responseBody[0] : new Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel { });

			return responseBody;
		}

		public ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncResponseModel>.AccessDeniedResponse();
			}


			return ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncResponseModel>.SuccessResponse();
		}
		internal void setPoData(int currentCW, Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel labels, Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel item, List<MaterialRequirementsQuantityEntity> poData,
			List<MaterialRequirementsQuantityEntity> faData)
		{
			// -		
			var maxCW = ISOWeek.GetWeekOfYear(new DateTime(DateTime.Today.Year, 12, 31));

			maxCW = maxCW <= 1 ? 52 : maxCW;
			var currYear = DateTime.Today.Year;
			var nextYear = DateTime.Today.Year + 1;
			{
				int i = 0;
				// - 01
				item.Label_CW1 = $"{labels.Label_CW1}";
				item.Order_CW1 = poData?.Where(x => x.CW == currentCW && x.Year == currYear)?.Sum(x => x.Quantity) ?? 0;
				item.Need_CW1 = faData?.Where(x => x.CW == currentCW && x.Year == currYear)?.Sum(x => x.Quantity) ?? 0;
				//!REM: Formula to check 
				item.Lager_CW1 = item.Lager + item.PoDelay - item.FaDelay;
				item.MinToOrder_CW1 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, (decimal)item.VPE_Losgroesse, item.Need_CW1, item.Lager_CW1);

				for(int j = 1; j <= 52; j++)
				{
					var labelProperty = item.GetType().GetProperty($"Label_CW{j + 1}");
					var orderProperty = item.GetType().GetProperty($"Order_CW{j + 1}");
					var needProperty = item.GetType().GetProperty($"Need_CW{j + 1}");
					var lagerProperty = item.GetType().GetProperty($"Lager_CW{j + 1}");
					var minToOrderProperty = item.GetType().GetProperty($"MinToOrder_CW{j + 1}");

					var labelToSetProperty = labels.GetType().GetProperty($"Label_CW{j + 1}");
					var previousMinToOrderValue = (decimal)item.GetType().GetProperty($"MinToOrder_CW{j}").GetValue(item);
					var previousOrderValue = (decimal)item.GetType().GetProperty($"Order_CW{j}").GetValue(item);
					var previousNeedValue = (decimal)item.GetType().GetProperty($"Need_CW{j}").GetValue(item);
					var previousLagerValue = (decimal)item.GetType().GetProperty($"Lager_CW{j}").GetValue(item);

					labelProperty.SetValue(item, labelToSetProperty.GetValue(labels));
					orderProperty.SetValue(item, poData?.Where(x => currentCW + j <= maxCW ? x.CW == currentCW + j && x.Year == currYear : x.CW == currentCW + j - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0);
					needProperty.SetValue(item, faData?.Where(x => currentCW + j <= maxCW ? x.CW == currentCW + j && x.Year == currYear : x.CW == currentCW + j - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0);
					//decimal stockToTakeFrom = previousMinToOrderValue == 0 ? previousLagerValue : previousMinToOrderValue;
					lagerProperty.SetValue(item, (object)((previousMinToOrderValue + previousLagerValue) - previousNeedValue));

					var currentNeedValue = (decimal)item.GetType().GetProperty($"Need_CW{j + 1}").GetValue(item);
					var currentLagerValue = (decimal)item.GetType().GetProperty($"Lager_CW{j + 1}").GetValue(item);

					minToOrderProperty.SetValue(item, (object)GetMinToOrderQty(item.Mindestbestellmenge ?? 0, (decimal)item.VPE_Losgroesse, currentNeedValue, currentLagerValue));
				}

				var itemAsList = item.ToOrdredList();
				Helpers.SpecialHelper.CutOrderWaste(itemAsList, (decimal)item.VPE_Losgroesse);
				item = Helpers.SpecialHelper.ListToInstance(itemAsList, item);

				#region --
				// - 02
				//i = 1;
				//item.Label_CW2 = labels.Label_CW2;
				//item.Order_CW2 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW2 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW2 = item.MinToOrder_CW1 + item.Order_CW1 - item.Need_CW1;
				//item.MinToOrder_CW2 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW2);
				//// - 03
				//i = 2;
				//item.Label_CW3 = labels.Label_CW3;
				//item.Order_CW3 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW3 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW3 = item.MinToOrder_CW2 + item.Order_CW2 - item.Need_CW2;
				//item.MinToOrder_CW3 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW3);

				//// - 04
				//i = 3;
				//item.Label_CW4 = labels.Label_CW4;
				//item.Order_CW4 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW4 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW4 = item.MinToOrder_CW3 + item.Order_CW3 - item.Need_CW3;
				//item.MinToOrder_CW4 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW4);

				//// - 05
				//i = 4;
				//item.Label_CW5 = labels.Label_CW5;
				//item.Order_CW5 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW5 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW5 = item.MinToOrder_CW4 + item.Order_CW4 - item.Need_CW4;
				//item.MinToOrder_CW5 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW5);

				//// - 06
				//i = 5;
				//item.Label_CW6 = labels.Label_CW6;
				//item.Order_CW6 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW6 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW6 = item.MinToOrder_CW5 + item.Order_CW5 - item.Need_CW5;
				//item.MinToOrder_CW6 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW6);

				//// - 07
				//i = 6;
				//item.Label_CW7 = labels.Label_CW7;
				//item.Order_CW7 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW7 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW7 = item.MinToOrder_CW6 + item.Order_CW6 - item.Need_CW6;
				//item.MinToOrder_CW7 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW7);

				//// - 08
				//i = 7;
				//item.Label_CW8 = labels.Label_CW8;
				//item.Order_CW8 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW8 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW8 = item.MinToOrder_CW7 + item.Order_CW7 - item.Need_CW7;
				//item.MinToOrder_CW8 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW8);

				//// - 09
				//i = 8;
				//item.Label_CW9 = labels.Label_CW9;
				//item.Order_CW9 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW9 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW9 = item.MinToOrder_CW8 + item.Order_CW8 - item.Need_CW8;
				//item.MinToOrder_CW9 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW9);

				//// - 10
				//i = 9;
				//item.Label_CW10 = labels.Label_CW10;
				//item.Order_CW10 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW10 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW10 = item.MinToOrder_CW9 + item.Order_CW9 - item.Need_CW9;
				//item.MinToOrder_CW10 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW10);

				//// - 11
				//i = 10;
				//item.Label_CW11 = labels.Label_CW11;
				//item.Order_CW11 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW11 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW11 = item.MinToOrder_CW10 + item.Order_CW10 - item.Need_CW10;
				//item.MinToOrder_CW11 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW11);

				//// - 12
				//i = 11;
				//item.Label_CW12 = labels.Label_CW12;
				//item.Order_CW12 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW12 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW12 = item.MinToOrder_CW11 + item.Order_CW11 - item.Need_CW11;
				//item.MinToOrder_CW12 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW12);

				//// - 13
				//i = 12;
				//item.Label_CW13 = labels.Label_CW13;
				//item.Order_CW13 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW13 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW13 = item.MinToOrder_CW12 + item.Order_CW12 - item.Need_CW12;
				//item.MinToOrder_CW13 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW13);

				//// - 14
				//i = 13;
				//item.Label_CW14 = labels.Label_CW14;
				//item.Order_CW14 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW14 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW14 = item.MinToOrder_CW13 + item.Order_CW13 - item.Need_CW13;
				//item.MinToOrder_CW14 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW14);

				//// - 15
				//i = 14;
				//item.Label_CW15 = labels.Label_CW15;
				//item.Order_CW15 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW15 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW15 = item.MinToOrder_CW14 + item.Order_CW14 - item.Need_CW14;
				//item.MinToOrder_CW15 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW15);

				//// - 16
				//i = 15;
				//item.Label_CW16 = labels.Label_CW16;
				//item.Order_CW16 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW16 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW16 = item.MinToOrder_CW15 + item.Order_CW15 - item.Need_CW15;
				//item.MinToOrder_CW16 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW16);

				//// - 17
				//i = 16;
				//item.Label_CW17 = labels.Label_CW17;
				//item.Order_CW17 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW17 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW17 = item.MinToOrder_CW16 + item.Order_CW16 - item.Need_CW16;
				//item.MinToOrder_CW17 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW17);

				//// - 18
				//i = 17;
				//item.Label_CW18 = labels.Label_CW18;
				//item.Order_CW18 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW18 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW18 = item.MinToOrder_CW17 + item.Order_CW17 - item.Need_CW17;
				//item.MinToOrder_CW18 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW18);

				//// - 19
				//i = 18;
				//item.Label_CW19 = labels.Label_CW19;
				//item.Order_CW19 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW19 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW19 = item.MinToOrder_CW18 + item.Order_CW18 - item.Need_CW18;
				//item.MinToOrder_CW19 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW19);

				//// - 20
				//i = 19;
				//item.Label_CW20 = labels.Label_CW20;
				//item.Order_CW20 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW20 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW20 = item.MinToOrder_CW19 + item.Order_CW19 - item.Need_CW19;
				//item.MinToOrder_CW20 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW20);

				//// - 21
				//i = 20;
				//item.Label_CW21 = labels.Label_CW21;
				//item.Order_CW21 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW21 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW21 = item.MinToOrder_CW20 + item.Order_CW20 - item.Need_CW20;
				//item.MinToOrder_CW21 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW21);

				//// - 22
				//i = 21;
				//item.Label_CW22 = labels.Label_CW22;
				//item.Order_CW22 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW22 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW22 = item.MinToOrder_CW21 + item.Order_CW21 - item.Need_CW21;
				//item.MinToOrder_CW22 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW22);

				//// - 23
				//i = 22;
				//item.Label_CW23 = labels.Label_CW23;
				//item.Order_CW23 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW23 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW23 = item.MinToOrder_CW22 + item.Order_CW22 - item.Need_CW22;
				//item.MinToOrder_CW23 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW23);

				//// - 24
				//i = 23;
				//item.Label_CW24 = labels.Label_CW24;
				//item.Order_CW24 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW24 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW24 = item.MinToOrder_CW23 + item.Order_CW23 - item.Need_CW23;
				//item.MinToOrder_CW24 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW24);

				//// - 25
				//i = 24;
				//item.Label_CW25 = labels.Label_CW25;
				//item.Order_CW25 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW25 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW25 = item.MinToOrder_CW24 + item.Order_CW24 - item.Need_CW24;
				//item.MinToOrder_CW25 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW25);

				//// - 26
				//i = 25;
				//item.Label_CW26 = labels.Label_CW26;
				//item.Order_CW26 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW26 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW26 = item.MinToOrder_CW25 + item.Order_CW25 - item.Need_CW25;
				//item.MinToOrder_CW26 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW26);

				//// - 27
				//i = 26;
				//item.Label_CW27 = labels.Label_CW27;
				//item.Order_CW27 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW27 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW27 = item.MinToOrder_CW26 + item.Order_CW26 - item.Need_CW26;
				//item.MinToOrder_CW27 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW27);

				//// - 28
				//i = 27;
				//item.Label_CW28 = labels.Label_CW28;
				//item.Order_CW28 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW28 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW28 = item.MinToOrder_CW27 + item.Order_CW27 - item.Need_CW27;
				//item.MinToOrder_CW28 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW28);

				//// - 29
				//i = 28;
				//item.Label_CW29 = labels.Label_CW29;
				//item.Order_CW29 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW29 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW29 = item.MinToOrder_CW28 + item.Order_CW28 - item.Need_CW28;
				//item.MinToOrder_CW29 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW29);

				//// - 30
				//i = 29;
				//item.Label_CW30 = labels.Label_CW30;
				//item.Order_CW30 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW30 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW30 = item.MinToOrder_CW29 + item.Order_CW29 - item.Need_CW29;
				//item.MinToOrder_CW30 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW30);

				//// - 31
				//i = 30;
				//item.Label_CW31 = labels.Label_CW31;
				//item.Order_CW31 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW31 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW31 = item.MinToOrder_CW30 + item.Order_CW30 - item.Need_CW30;
				//item.MinToOrder_CW31 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW31);

				//// - 32
				//i = 31;
				//item.Label_CW32 = labels.Label_CW32;
				//item.Order_CW32 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW32 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW32 = item.MinToOrder_CW31 + item.Order_CW31 - item.Need_CW31;
				//item.MinToOrder_CW32 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW32);

				//// - 33
				//i = 32;
				//item.Label_CW33 = labels.Label_CW33;
				//item.Order_CW33 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW33 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW33 = item.MinToOrder_CW32 + item.Order_CW32 - item.Need_CW32;
				//item.MinToOrder_CW33 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW33);

				//// - 34
				//i = 33;
				//item.Label_CW34 = labels.Label_CW34;
				//item.Order_CW34 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW34 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW34 = item.MinToOrder_CW33 + item.Order_CW33 - item.Need_CW33;
				//item.MinToOrder_CW34 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW34);

				//// - 35
				//i = 34;
				//item.Label_CW35 = labels.Label_CW35;
				//item.Order_CW35 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW35 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW35 = item.MinToOrder_CW34 + item.Order_CW34 - item.Need_CW34;
				//item.MinToOrder_CW35 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW35);

				//// - 36
				//i = 35;
				//item.Label_CW36 = labels.Label_CW36;
				//item.Order_CW36 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW36 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW36 = item.MinToOrder_CW35 + item.Order_CW35 - item.Need_CW35;
				//item.MinToOrder_CW36 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW36);

				//// - 37
				//i = 36;
				//item.Label_CW37 = labels.Label_CW37;
				//item.Order_CW37 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW37 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW37 = item.MinToOrder_CW36 + item.Order_CW36 - item.Need_CW36;
				//item.MinToOrder_CW37 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW37);

				//// - 38
				//i = 37;
				//item.Label_CW38 = labels.Label_CW38;
				//item.Order_CW38 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW38 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW38 = item.MinToOrder_CW37 + item.Order_CW37 - item.Need_CW37;
				//item.MinToOrder_CW38 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW38);

				//// - 39
				//i = 38;
				//item.Label_CW39 = labels.Label_CW39;
				//item.Order_CW39 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW39 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW39 = item.MinToOrder_CW38 + item.Order_CW38 - item.Need_CW38;
				//item.MinToOrder_CW39 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW39);

				//// - 40
				//i = 39;
				//item.Label_CW40 = labels.Label_CW40;
				//item.Order_CW40 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW40 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW40 = item.MinToOrder_CW39 + item.Order_CW39 - item.Need_CW39;
				//item.MinToOrder_CW40 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW40);

				//// - 41
				//i = 40;
				//item.Label_CW41 = labels.Label_CW41;
				//item.Order_CW41 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW41 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW41 = item.MinToOrder_CW40 + item.Order_CW40 - item.Need_CW40;
				//item.MinToOrder_CW41 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW41);

				//// - 42
				//i = 41;
				//item.Label_CW42 = labels.Label_CW42;
				//item.Order_CW42 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW42 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW42 = item.MinToOrder_CW41 + item.Order_CW41 - item.Need_CW41;
				//item.MinToOrder_CW42 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW42);

				//// - 43
				//i = 42;
				//item.Label_CW43 = labels.Label_CW43;
				//item.Order_CW43 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW43 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW43 = item.MinToOrder_CW42 + item.Order_CW42 - item.Need_CW42;
				//item.MinToOrder_CW43 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW43);

				//// - 44
				//i = 43;
				//item.Label_CW44 = labels.Label_CW44;
				//item.Order_CW44 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW44 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW44 = item.MinToOrder_CW43 + item.Order_CW43 - item.Need_CW43;
				//item.MinToOrder_CW44 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW44);

				//// - 45
				//i = 44;
				//item.Label_CW45 = labels.Label_CW45;
				//item.Order_CW45 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW45 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW45 = item.MinToOrder_CW44 + item.Order_CW44 - item.Need_CW44;
				//item.MinToOrder_CW45 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW45);

				//// - 46
				//i = 45;
				//item.Label_CW46 = labels.Label_CW46;
				//item.Order_CW46 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW46 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW46 = item.MinToOrder_CW45 + item.Order_CW45 - item.Need_CW45;
				//item.MinToOrder_CW46 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW46);

				//// - 47
				//i = 46;
				//item.Label_CW47 = labels.Label_CW47;
				//item.Order_CW47 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW47 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW47 = item.MinToOrder_CW46 + item.Order_CW46 - item.Need_CW46;
				//item.MinToOrder_CW47 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW47);

				//// - 48
				//i = 47;
				//item.Label_CW48 = labels.Label_CW48;
				//item.Order_CW48 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW48 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW48 = item.MinToOrder_CW47 + item.Order_CW47 - item.Need_CW47;
				//item.MinToOrder_CW48 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW48);

				//// - 49
				//i = 48;
				//item.Label_CW49 = labels.Label_CW49;
				//item.Order_CW49 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW49 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW49 = item.MinToOrder_CW48 + item.Order_CW48 - item.Need_CW48;
				//item.MinToOrder_CW49 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW49);

				//// - 50
				//i = 49;
				//item.Label_CW50 = labels.Label_CW50;
				//item.Order_CW50 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW50 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW50 = item.MinToOrder_CW49 + item.Order_CW49 - item.Need_CW49;
				//item.MinToOrder_CW50 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW50);

				//// - 51
				//i = 50;
				//item.Label_CW51 = labels.Label_CW51;
				//item.Order_CW51 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW51 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW51 = item.MinToOrder_CW50 + item.Order_CW50 - item.Need_CW50;
				//item.MinToOrder_CW51 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW51);

				//// - 52
				//i = 51;
				//item.Label_CW52 = labels.Label_CW52;
				//item.Order_CW52 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW52 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW52 = item.MinToOrder_CW51 + item.Order_CW51 - item.Need_CW51;
				//item.MinToOrder_CW52 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW52);

				//// - 53
				//i = 52;
				//item.Label_CW53 = labels.Label_CW53;
				//item.Order_CW53 = poData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Need_CW53 = faData?.Where(x => currentCW + i <= maxCW ? x.CW == currentCW + i && x.Year == currYear : x.CW == currentCW + i - maxCW && x.Year == nextYear)?.Sum(x => x.Quantity) ?? 0;
				//item.Lager_CW53 = item.MinToOrder_CW52 + item.Order_CW52 - item.Need_CW52;
				//item.MinToOrder_CW53 = GetMinToOrderQty(item.Mindestbestellmenge ?? 0, item.Lager_CW53);
				#endregion
			}
		}
		internal void setPoLabels(Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel labels, Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel item)
		{
			if(item is not null)
			{
				for(int i = 1; i <= 53; i++)
				{
					var labelProperty = item.GetType().GetProperty($"Label_CW{i}");
					item.GetType().GetProperty($"Label_CW{i}").SetValue(labels, labelProperty?.GetValue(item));
				}
				//item.Label_CW1 = labels.Label_CW1;
				//item.Label_CW2 = labels.Label_CW2;
				//item.Label_CW3 = labels.Label_CW3;
				//item.Label_CW4 = labels.Label_CW4;
				//item.Label_CW5 = labels.Label_CW5;
				//item.Label_CW6 = labels.Label_CW6;
				//item.Label_CW7 = labels.Label_CW7;
				//item.Label_CW8 = labels.Label_CW8;
				//item.Label_CW9 = labels.Label_CW9;
				//item.Label_CW10 = labels.Label_CW10;
				//item.Label_CW11 = labels.Label_CW11;
				//item.Label_CW12 = labels.Label_CW12;
				//item.Label_CW13 = labels.Label_CW13;
				//item.Label_CW14 = labels.Label_CW14;
				//item.Label_CW15 = labels.Label_CW15;
				//item.Label_CW16 = labels.Label_CW16;
				//item.Label_CW17 = labels.Label_CW17;
				//item.Label_CW18 = labels.Label_CW18;
				//item.Label_CW19 = labels.Label_CW19;
				//item.Label_CW20 = labels.Label_CW20;
				//item.Label_CW21 = labels.Label_CW21;
				//item.Label_CW22 = labels.Label_CW22;
				//item.Label_CW23 = labels.Label_CW23;
				//item.Label_CW24 = labels.Label_CW24;
				//item.Label_CW25 = labels.Label_CW25;
				//item.Label_CW26 = labels.Label_CW26;
				//item.Label_CW27 = labels.Label_CW27;
				//item.Label_CW28 = labels.Label_CW28;
				//item.Label_CW29 = labels.Label_CW29;
				//item.Label_CW30 = labels.Label_CW30;
				//item.Label_CW31 = labels.Label_CW31;
				//item.Label_CW32 = labels.Label_CW32;
				//item.Label_CW33 = labels.Label_CW33;
				//item.Label_CW34 = labels.Label_CW34;
				//item.Label_CW35 = labels.Label_CW35;
				//item.Label_CW36 = labels.Label_CW36;
				//item.Label_CW37 = labels.Label_CW37;
				//item.Label_CW38 = labels.Label_CW38;
				//item.Label_CW39 = labels.Label_CW39;
				//item.Label_CW40 = labels.Label_CW40;
				//item.Label_CW41 = labels.Label_CW41;
				//item.Label_CW42 = labels.Label_CW42;
				//item.Label_CW43 = labels.Label_CW43;
				//item.Label_CW44 = labels.Label_CW44;
				//item.Label_CW45 = labels.Label_CW45;
				//item.Label_CW46 = labels.Label_CW46;
				//item.Label_CW47 = labels.Label_CW47;
				//item.Label_CW48 = labels.Label_CW48;
				//item.Label_CW49 = labels.Label_CW49;
				//item.Label_CW50 = labels.Label_CW50;
				//item.Label_CW51 = labels.Label_CW51;
				//item.Label_CW52 = labels.Label_CW52;
				//item.Label_CW53 = labels.Label_CW53;
			}
		}
		public byte[] GetXLS(bool external = false)
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return null;
				}

				//-
				var supplierNames = new List<string>();
				if(this._data.AdressenNr != null && this._data.AdressenNr > 0)
				{
					var supplier = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.AdressenNr.Value);
					if(supplier != null)
					{
						supplierNames.Add(supplier.Name1);
					}
				}
				else
				{
					var suppliers = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByStufe(this._data.Stufe);
					if(suppliers != null)
					{
						supplierNames.AddRange(suppliers.Select(x => x.Name1));
					}
				}

				var responseBody = new List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>();
				var count = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock_Header_count(supplierNames, this._data.ArticleNumber, this._data.Fa30Postive, this._data.OnlyPrioSupplier, _data.ArticleClassification);
				var labels = new Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel();
				if(count > 0)
				{
					responseBody = getResponseBody(supplierNames, out labels, false);
				}

				// -
				return external
					? getXlsExtrenalData(responseBody, labels)
					: getXlsData(responseBody, labels);


			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		internal byte[] getXlsData(List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel> data, Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel labels)
		{
			try
			{
				var currentCW = ISOWeek.GetWeekOfYear(DateTime.Today);
				using(var stream = new MemoryStream())
				{
					// FIXME: Replace EPPlus by NPOI, or some other alt
					OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
					// Create the package and make sure you wrap it in a using statement
					using(var package = new OfficeOpenXml.ExcelPackage())
					{
						// add a new worksheet to the empty workbook
						OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"data");
						// add a new worksheet - 2024-08-31 // - 2024-10-10 remove detail - Heidenreich - move to PRS stats
						// - addDetailedHistory(package);

						// Keep track of the row that we're on, but start with four to skip the header
						var headerRowNumber = 2;
						var startColumnNumber = 1;
						var numberOfColumns = 17 + 52;

						// Add some formatting to the worksheet
						worksheet.TabColor = System.Drawing.Color.Yellow;
						worksheet.DefaultRowHeight = 11;
						worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
						worksheet.Row(2).Height = 20;
						worksheet.Row(1).Height = 30;
						worksheet.Row(headerRowNumber).Height = 20;

						// Pre Header
						worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
						worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						worksheet.Cells[1, 1].Value = $"Data";
						worksheet.Cells[1, 1].Style.Font.Size = 16;

						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "PRIO1 Lieferant";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Lieferant Artikelnummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Artikelklassifizierung";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bezeichnung 2";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "EK";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Mindestbestellmenge";
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Lieferzeit";
						worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "VPE/Losgroesse";
						worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Gesamtbedarf Offene FA 360";
						worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Bestand reserviert";
						worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Lagerbestand";
						worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Min Lagerbestand";
						worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Bedarf PO";
						worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Summe PO";
						worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "";
						worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Rückstand";
						// - 
						addWeeksHeader(currentCW, headerRowNumber, startColumnNumber, 16, worksheet, labels);


						var rowNumber = headerRowNumber + 1;
						if(data != null && data.Count > 0)
						{
							// Loop through 
							foreach(var w in data)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
								worksheet.Cells[rowNumber + 1, startColumnNumber + 0].Value = w?.Artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.PRIO1_Lieferant;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.LieferantArtikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.ArticleClassification;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Description2;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.EK;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Mindestbestellmenge;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Lieferzeit;
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.VPE_Losgroesse;
								worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.GesamtbedarfOffeneFA360;
								worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Verfugbarbestand;
								worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Lager;
								worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Min_Lagerbestand;
								worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.BedarfPO;
								worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.SummePO;
								worksheet.Cells[rowNumber, startColumnNumber + 15].Value = "Lager:";
								worksheet.Cells[rowNumber + 1, startColumnNumber + 15].Value = "Bedarf:";
								worksheet.Cells[rowNumber, startColumnNumber + 16].Value = w?.PoDelay;
								worksheet.Cells[rowNumber + 1, startColumnNumber + 16].Value = w?.FaDelay;

								// - formatting
								worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 13].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 14].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber + 0, startColumnNumber + 16].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber + 1, startColumnNumber + 16].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

								addWeeks(currentCW, rowNumber, startColumnNumber, 16, worksheet, w);

								// - 
								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 2;
							}
						}

						//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
						using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
						{
							range.Style.Font.Bold = true;
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
							range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
							range.Style.ShrinkToFit = false;
						}
						// Darker Blue in Top cell
						worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White/*System.Drawing.ColorTranslator.FromHtml("#D9E1F2")*/);

						// Doc content
						if(data != null && data.Count > 0)
						{
							using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
							{
								range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
								range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
								range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
								range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
								range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
								range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							}
							for(int i = headerRowNumber + 1; i < rowNumber; i += 2)
							{

								// - alternate color
								if((i / 2) % 2 == 1)
								{
									using(var range = worksheet.Cells[i, 1, i + 1, numberOfColumns])
									{
										range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
										range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
									}
								}
							}
						}

						// Thick countour
						using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
						{
							range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
						}

						// Fit the columns according to its content
						for(int i = 1; i <= numberOfColumns; i++)
						{
							worksheet.Column(i).AutoFit();
						}

						// Set some document properties
						package.Workbook.Properties.Title = $"Data";
						package.Workbook.Properties.Author = "PSZ ERP";
						package.Workbook.Properties.Company = "PSZ ERP";

						//-	
						return package.GetAsByteArray();
					}
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		internal byte[] getXlsExtrenalData(List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel> data, Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel labels)
		{
			var currentCW = ISOWeek.GetWeekOfYear(DateTime.Today);
			using(var stream = new MemoryStream())
			{
				// FIXME: Replace EPPlus by NPOI, or some other alt
				OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new OfficeOpenXml.ExcelPackage())
				{
					OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"data");

					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 17 + 52;

					worksheet.TabColor = System.Drawing.Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					//pre header
					worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"Data";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					//headers
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "PRIO1 Lieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Lieferant Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Artikelklassifizierung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bezeichnung 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "EK";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Mindestbestellmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Lieferzeit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "VPE/Losgroesse";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Gesamtbedarf Offene FA 360";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Bestand reserviert";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Lagerbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Min Lagerbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Bedarf PO";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Summe PO";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Rückstand FA";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Rückstand PO";

					addWeeksHeader(currentCW, headerRowNumber, startColumnNumber, 16, worksheet, labels);

					var rowNumber = headerRowNumber + 1;
					if(data != null && data.Count > 0)
					{
						foreach(var w in data)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.PRIO1_Lieferant;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.LieferantArtikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.ArticleClassification;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Description2;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.EK;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Mindestbestellmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Lieferzeit;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.VPE_Losgroesse;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.GesamtbedarfOffeneFA360;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Verfugbarbestand;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Lager;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Min_Lagerbestand;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.BedarfPO;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.SummePO;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = w?.FaDelay;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = w?.PoDelay;
							// - formatting
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

							addWeeksExternal(currentCW, rowNumber, startColumnNumber, 16, worksheet, w);

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}

						using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
					}
					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
						range.Style.ShrinkToFit = false;
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White/*System.Drawing.ColorTranslator.FromHtml("#D9E1F2")*/);
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}

					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					package.Workbook.Properties.Title = $"Data";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					//-	
					return package.GetAsByteArray();
				}
			}
		}
		internal void addWeeksHeader(int currentCW, int rowNumber, int startColumnNumber, int fixColumnStart, OfficeOpenXml.ExcelWorksheet worksheet,
			Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel w)
		{
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 1].Value = $"{w?.Label_CW1}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 2].Value = $"{w?.Label_CW2}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 3].Value = $"{w?.Label_CW3}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 4].Value = $"{w?.Label_CW4}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 5].Value = $"{w?.Label_CW5}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 6].Value = $"{w?.Label_CW6}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 7].Value = $"{w?.Label_CW7}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 8].Value = $"{w?.Label_CW8}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 9].Value = $"{w?.Label_CW9}";

			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 10].Value = $"{w?.Label_CW10}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 11].Value = $"{w?.Label_CW11}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 12].Value = $"{w?.Label_CW12}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 13].Value = $"{w?.Label_CW13}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 14].Value = $"{w?.Label_CW14}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 15].Value = $"{w?.Label_CW15}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 16].Value = $"{w?.Label_CW16}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 17].Value = $"{w?.Label_CW17}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 18].Value = $"{w?.Label_CW18}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 19].Value = $"{w?.Label_CW19}";

			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 20].Value = $"{w?.Label_CW20}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 21].Value = $"{w?.Label_CW21}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 22].Value = $"{w?.Label_CW22}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 23].Value = $"{w?.Label_CW23}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 24].Value = $"{w?.Label_CW24}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 25].Value = $"{w?.Label_CW25}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 26].Value = $"{w?.Label_CW26}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 27].Value = $"{w?.Label_CW27}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 28].Value = $"{w?.Label_CW28}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 29].Value = $"{w?.Label_CW29}";

			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 30].Value = $"{w?.Label_CW30}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 31].Value = $"{w?.Label_CW31}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 32].Value = $"{w?.Label_CW32}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 33].Value = $"{w?.Label_CW33}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 34].Value = $"{w?.Label_CW34}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 35].Value = $"{w?.Label_CW35}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 36].Value = $"{w?.Label_CW36}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 37].Value = $"{w?.Label_CW37}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 38].Value = $"{w?.Label_CW38}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 39].Value = $"{w?.Label_CW39}";

			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 40].Value = $"{w?.Label_CW40}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 41].Value = $"{w?.Label_CW41}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 42].Value = $"{w?.Label_CW42}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 43].Value = $"{w?.Label_CW43}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 44].Value = $"{w?.Label_CW44}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 45].Value = $"{w?.Label_CW45}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 46].Value = $"{w?.Label_CW46}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 47].Value = $"{w?.Label_CW47}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 48].Value = $"{w?.Label_CW48}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 49].Value = $"{w?.Label_CW49}";


			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 50].Value = $"{w?.Label_CW50}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 51].Value = $"{w?.Label_CW51}";
			worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 52].Value = $"{w?.Label_CW52}";
			//worksheet.Cells[rowNumber, startColumnNumber + fixColumnStart + 53].Value = $"{w?.Label_CW53}";
		}
		internal void addWeeksExternal(int currentCW, int rowNumber, int startColumnNumber, int fixColumnStart, OfficeOpenXml.ExcelWorksheet worksheet,
			Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel w)
		{
			for(int i = 1; i <= 52; i++)
			{
				var minToOrderPorperty = w.GetType().GetProperty($"MinToOrder_CW{i}");

				var minToOrderValue = (decimal)minToOrderPorperty?.GetValue(w);

				worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + i].Value = minToOrderValue;
				worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + i].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
			}
		}
		internal void addWeeks(int currentCW, int rowNumber, int startColumnNumber, int fixColumnStart, OfficeOpenXml.ExcelWorksheet worksheet,
			Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel w)
		{
			for(int i = 1; i <= 52; i++)
			{
				var lagerProperty = w.GetType().GetProperty($"Lager_CW{i}");
				var needProperty = w.GetType().GetProperty($"Need_CW{i}");

				var lagerValue = (decimal)lagerProperty?.GetValue(w);
				var needValue = (decimal)needProperty?.GetValue(w);

				worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + i].Value = lagerValue;
				worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + i].Value = needValue;

				worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + i].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
				worksheet.Cells[rowNumber + 4, startColumnNumber + fixColumnStart + i].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

			}
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 1].Value = $"{w?.Lager_CW1}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 1].Value = $"{w?.Need_CW1}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 2].Value = $"{w?.Lager_CW2}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 2].Value = $"{w?.Need_CW2}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 3].Value = $"{w?.Lager_CW3}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 3].Value = $"{w?.Need_CW3}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 4].Value = $"{w?.Lager_CW4}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 4].Value = $"{w?.Need_CW4}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 5].Value = $"{w?.Lager_CW5}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 5].Value = $"{w?.Need_CW5}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 6].Value = $"{w?.Lager_CW6}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 6].Value = $"{w?.Need_CW6}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 7].Value = $"{w?.Lager_CW7}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 7].Value = $"{w?.Need_CW7}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 8].Value = $"{w?.Lager_CW8}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 8].Value = $"{w?.Need_CW8}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 9].Value = $"{w?.Lager_CW9}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 9].Value = $"{w?.Need_CW9}";

			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 10].Value = $"{w?.Lager_CW10}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 10].Value = $"{w?.Need_CW10}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 11].Value = $"{w?.Lager_CW11}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 11].Value = $"{w?.Need_CW11}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 12].Value = $"{w?.Lager_CW12}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 12].Value = $"{w?.Need_CW12}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 13].Value = $"{w?.Lager_CW13}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 13].Value = $"{w?.Need_CW13}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 14].Value = $"{w?.Lager_CW14}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 14].Value = $"{w?.Need_CW14}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 15].Value = $"{w?.Lager_CW15}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 15].Value = $"{w?.Need_CW15}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 16].Value = $"{w?.Lager_CW16}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 16].Value = $"{w?.Need_CW16}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 17].Value = $"{w?.Lager_CW17}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 17].Value = $"{w?.Need_CW17}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 18].Value = $"{w?.Lager_CW18}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 18].Value = $"{w?.Need_CW18}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 19].Value = $"{w?.Lager_CW19}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 19].Value = $"{w?.Need_CW19}";

			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 20].Value = $"{w?.Lager_CW20}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 20].Value = $"{w?.Need_CW20}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 21].Value = $"{w?.Lager_CW21}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 21].Value = $"{w?.Need_CW21}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 22].Value = $"{w?.Lager_CW22}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 22].Value = $"{w?.Need_CW22}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 23].Value = $"{w?.Lager_CW23}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 23].Value = $"{w?.Need_CW23}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 24].Value = $"{w?.Lager_CW24}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 24].Value = $"{w?.Need_CW24}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 25].Value = $"{w?.Lager_CW25}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 25].Value = $"{w?.Need_CW25}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 26].Value = $"{w?.Lager_CW26}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 26].Value = $"{w?.Need_CW26}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 27].Value = $"{w?.Lager_CW27}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 27].Value = $"{w?.Need_CW27}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 28].Value = $"{w?.Lager_CW28}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 28].Value = $"{w?.Need_CW28}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 29].Value = $"{w?.Lager_CW29}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 29].Value = $"{w?.Need_CW29}";

			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 30].Value = $"{w?.Lager_CW30}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 30].Value = $"{w?.Need_CW30}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 31].Value = $"{w?.Lager_CW31}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 31].Value = $"{w?.Need_CW31}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 32].Value = $"{w?.Lager_CW32}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 32].Value = $"{w?.Need_CW32}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 33].Value = $"{w?.Lager_CW33}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 33].Value = $"{w?.Need_CW33}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 34].Value = $"{w?.Lager_CW34}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 34].Value = $"{w?.Need_CW34}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 35].Value = $"{w?.Lager_CW35}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 35].Value = $"{w?.Need_CW35}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 36].Value = $"{w?.Lager_CW36}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 36].Value = $"{w?.Need_CW36}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 37].Value = $"{w?.Lager_CW37}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 37].Value = $"{w?.Need_CW37}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 38].Value = $"{w?.Lager_CW38}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 38].Value = $"{w?.Need_CW38}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 39].Value = $"{w?.Lager_CW39}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 39].Value = $"{w?.Need_CW39}";

			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 40].Value = $"{w?.Lager_CW40}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 40].Value = $"{w?.Need_CW40}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 41].Value = $"{w?.Lager_CW41}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 41].Value = $"{w?.Need_CW41}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 42].Value = $"{w?.Lager_CW42}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 42].Value = $"{w?.Need_CW42}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 43].Value = $"{w?.Lager_CW43}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 43].Value = $"{w?.Need_CW43}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 44].Value = $"{w?.Lager_CW44}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 44].Value = $"{w?.Need_CW44}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 45].Value = $"{w?.Lager_CW45}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 45].Value = $"{w?.Need_CW45}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 46].Value = $"{w?.Lager_CW46}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 46].Value = $"{w?.Need_CW46}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 47].Value = $"{w?.Lager_CW47}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 47].Value = $"{w?.Need_CW47}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 48].Value = $"{w?.Lager_CW48}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 48].Value = $"{w?.Need_CW48}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 49].Value = $"{w?.Lager_CW49}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 49].Value = $"{w?.Need_CW49}";

			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 50].Value = $"{w?.Lager_CW50}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 50].Value = $"{w?.Need_CW50}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 51].Value = $"{w?.Lager_CW51}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 51].Value = $"{w?.Need_CW51}";
			//worksheet.Cells[rowNumber + 0, startColumnNumber + fixColumnStart + 52].Value = $"{w?.Lager_CW52}";
			//worksheet.Cells[rowNumber + 1, startColumnNumber + fixColumnStart + 52].Value = $"{w?.Need_CW52}";
		}

		internal decimal GetMinToOrderQty(decimal minOrderQty, decimal lotSize, decimal neededQty, decimal lagerQty)
		{
			var neededToOrder = lagerQty - neededQty;
			if(neededToOrder >= 0)
				return 0; // No need to order anything, we have enough stock
			if(minOrderQty <= 0)
				return Math.Abs(neededToOrder);
			if(Math.Abs(neededToOrder) <= minOrderQty)
				return minOrderQty; // We need to order at least the minimum order quantity

			var requiredQty = Math.Abs(neededToOrder); // How much we need to bring stock to 0
			var multiplier = (int)Math.Ceiling(requiredQty / (lotSize == 0 ? 1 : lotSize));
			return multiplier * lotSize;
		}
	}
}
