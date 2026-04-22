using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Order.OrderModel _data { get; set; }

		public UpdateHandler(Identity.Models.UserModel user, Models.Budget.Order.OrderModel model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// - 2022-01-25 - Allocation type
				if(this._data.AllocationType.HasValue && this._data.AllocationType == (int)Enums.BudgetEnums.AllocationTypes.Invest)
				{
					this._data.AllocationTypeName = Enums.BudgetEnums.AllocationTypes.Invest.GetDescription();
				}
				else
				{ // - default as Fix
					this._data.AllocationType = (int)Enums.BudgetEnums.AllocationTypes.Fix;
					this._data.AllocationTypeName = Enums.BudgetEnums.AllocationTypes.Fix.GetDescription();
				}

				/// 
				//try
				//{
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var companyFNCExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntity?.CompanyId ?? -1);
				var orderCurrency = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(this._data.CurrencyId ?? -1)
				?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };

				//}
				//catch (Exception e)
				//{
				//    Infrastructure.Services.Logging.Logger.Log(e);
				//    Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				//    throw;
				//}
				// - will be inserted after
				//var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Id_Order);
				//if (this._data.File.DocumentData != null  )
				//{  
				//    this._data.File.idOrder = this._data.Id_Order;
				//    this._data.File.userId = this._user.Id;
				//    var fileEntity = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdOrder(this._data.Id_Order);
				//    if (fileEntity != null) {                  
				//        Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.Delete(fileEntity.Id_File);
				//        Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.Insert(this._data.File.ToFile_OrderEntity());
				//    }
				//    else
				//    {
				//        Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.Insert(this._data.File.ToFile_OrderEntity());
				//    }
				//}

				try
				{

					this._data.TotalAmount = this._data.Articles?.Select(x => (decimal)(x.TotalCost_Article ?? 0) * (1 + (x.VAT ?? 0)))?.Sum() ?? 0m;
					this._data.TotalAmountDefaultCurrency = this._data.Articles?.Select(x => (decimal)(x.TotalCost_Article ?? 0) * (decimal)(orderCurrency?.entspricht_DM ?? 1) * (1 + (x.VAT ?? 0)))?.Sum() ?? 0m;

				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				}

				// Update Order
				var supplierEntity = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Get(this._data.SupplierId ?? -1);
				if(supplierEntity != null && supplierEntity.Nummer == 89) // - generic supplier
				{
					this._data.SupplierId = 0;
				}
				try
				{
					Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Update(this._data.ToBestellungenEntity());
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				}
				var updatedId = -1;
				try
				{
					var bestellExt = this._data.ToOrderExtension();
					var defaultCurrency = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(companyFNCExtension.DefaultCurrencyId ?? -1)
						?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
					bestellExt.DefaultCurrencyId = defaultCurrency?.Nr;
					bestellExt.DefaultCurrencyName = defaultCurrency?.Symbol;
					bestellExt.DefaultCurrencyRate = (decimal?)defaultCurrency?.entspricht_DM;
					bestellExt.DefaultCurrencyDecimals = defaultCurrency?.Dezimalstellen;
					updatedId = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Update(bestellExt);

				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				}

				try
				{
					var order = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Id_Order);
					if(this._user.Id == order.IssuerId)
					{
						Helpers.Processings.Budget.Order.HandleArticles(this._data, this._user);
					}
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
					throw;
				}

				#region Article Update DONE A-part
				//// update Articles
				//var oldArticles = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(this._data.Id_Order);

				//var artikelEntites = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(this._data.Articles?.Select(x => x.Id_Article)?.ToList());

				//var newArticles = (oldArticles == null || oldArticles.Count <= 0)
				//    ? this._data.Articles
				//    : this._data.Articles?.FindAll(x => !oldArticles.Exists(y => y.Id_AO == x.Id_AO))?.ToList();
				//var nArticles = new List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity>();
				//foreach (var item in newArticles)
				//{
				//    var artikel = artikelEntites.Find(x => x.Artikel_Nr == item.Id_Article);
				//    nArticles.Add(item.ToArticle_OrderEntity(artikel));
				//}
				//// New Articles
				//Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.Insert(nArticles);
				////Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.Insert(newArticles?.Select(x => x.ToArticle_OrderEntity())?.ToList());


				//// Changed Articles
				//var changedArticles = this._data.Articles == null || this._data.Articles.Count <= 0 
				//    ? new List<Models.Budget.Order.Article.ArticleModel>()
				//    : this._data.Articles.Except(newArticles)?.ToList();
				//var cArticles = new List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity>();
				//foreach (var item in changedArticles)
				//{
				//    var artikel = artikelEntites.Find(x => x.Artikel_Nr == item.Id_Article);
				//    cArticles.Add(item.ToArticle_OrderEntity(artikel));
				//}
				//Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.Update(cArticles);
				////Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.Update(changedArticles?.Select(x => x.ToArticle_OrderEntity())?.ToList());

				//// Deleted Articles
				//if (this._data.Articles != null && this._data.Articles.Count > 0)
				//{
				//    var deletedArticles = oldArticles?.FindAll(x => !this._data.Articles.Exists(y => y.Id_AO == x.Id_AO))?.Select(x => x.Id_AO)?.ToList();
				//    Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.Delete(deletedArticles);
				//} 
				#endregion

				return ResponseModel<int>.SuccessResponse(updatedId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var order = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Id_Order);
			if(order == null)
				return ResponseModel<int>.FailureResponse("Order not found");

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");


			#region >>>> Leasing Date <<<<
			if(this._data.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
			{
				var leasingAllowedDepartments = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetAllowedLeasing();
				if(leasingAllowedDepartments?.Exists(x => x.Id == userEntity.DepartmentId) != true)
					return ResponseModel<int>.FailureResponse(key: "1", value: "user cannot add Leasing PO");

				if(this._data.LeasingStartMonth == null || this._data.LeasingStartMonth <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Start Month invalid");

				if(this._data.LeasingStartYear == null || this._data.LeasingStartYear <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Start Year invalid");

				if(this._data.LeasingMonthAmount == null || this._data.LeasingMonthAmount <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Month Amount invalid");

				if(this._data.LeasingNbMonths == null || this._data.LeasingNbMonths <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Nb months invalid");

				if(this._data.LeasingStartYear < DateTime.Today.Year
					|| this._data.LeasingStartYear == DateTime.Today.Year && this._data.LeasingStartMonth < DateTime.Today.Month)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing cannot start in the past");

				var companyFncExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntity.CompanyId ?? -1);
				if(this._data.CurrencyId != null && this._data.CurrencyId != companyFncExtension.DefaultCurrencyId)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Leasing accepts only [{companyFncExtension.DefaultCurrencyName}] as currency.");
			}
			#endregion Leasing Date

			//REM: Only owner can update order
			if(this._user.Id == order.IssuerId)
			{
				#region >>>> Order Amount <<<<
				var orderCurrency = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(this._data.CurrencyId ?? -1)
					?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
				var orderAmount = 0m;
				if(this._data.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
				{
					orderAmount = this._data.Articles.Select(x => (decimal)(x.TotalCost_Article ?? 0) * (decimal)(orderCurrency?.entspricht_DM ?? 1))?.Sum() ?? 0; // - ignore VAT
				}
				else
				{
					if(this._data.LeasingStartYear == DateTime.Today.Year && this._data.LeasingStartMonth == DateTime.Today.Month)
					{
						orderAmount = (this._data.LeasingMonthAmount ?? 0) * (decimal)(orderCurrency?.entspricht_DM ?? 1) / (this._data.LeasingNbMonths ?? 1);
						//orderAmount = (this._data.LeasingTotalAmount ?? 0)*(decimal)(orderCurrency?.entspricht_DM ?? 1) / (this._data.LeasingNbMonths ?? 1);
					}
					// - if Leasing does not start current year and month leave Amount to Zero!
				}
				#endregion Order Amount

				#region >>>> Budget Checks <<<<
				// - External Order/Project
				if(this._data.Type?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower().ToLower())
				{
					if(this._data.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
						return ResponseModel<int>.FailureResponse(key: "1", value: "External order cannot be leased");

					var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.ProjectId);
					if(projectEntity == null)
						return ResponseModel<int>.FailureResponse(key: "1", value: "External order should have a project");

					//if ((projectEntity.ProjectBudget - projectEntity.TotalSpent) < orderAmount)
					//    return ResponseModel<int>.FailureResponse(key: "1", value: "Insufficient project budget");

					if(projectEntity.Closed.HasValue && projectEntity.Closed.Value == true)
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been closed");

					if(projectEntity.Archived.HasValue && projectEntity.Archived.Value)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Project is archived");

					if(projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive)
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been deactivated");

				}
				else // Internal order
				{
					var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.ProjectId);
					if(projectEntity != null)
					{
						if(projectEntity.Closed.HasValue && projectEntity.Closed.Value == true)
							return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been closed");

						if(projectEntity.Archived.HasValue && projectEntity.Archived.Value)
							return ResponseModel<int>.FailureResponse(key: "1", value: "Project is archived");

						if(projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive)
							return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been deactivated");
					}

					var budgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userEntity.Id, DateTime.Today.Year);
					if(budgetEntity == null)
						return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to user");

					// - 2024-02-06 - allow save w/o enough budget
					//REM: Leasing Amount is added only if it starts on current Month and Year
					//if(budgetEntity.AmountOrder/* - budgetEntity.AmountSpent*/ < orderAmount)
					//	return ResponseModel<int>.FailureResponse(key: "1", value: "Insufficient user Order Budget");


					// - 2024-02-06 - allow save w/o enough budget
					//var monthExpenses =
					//	Helpers.Processings.Budget.Order.getItemsAmount(Helpers.Processings.Budget.Order.getPurchaseItems(this._user.Id, DateTime.Today.Year, DateTime.Today.Month), false, true, this._data.Discount ?? 0) +
					//	Helpers.Processings.Budget.Order.GetLeasingAmount(this._user.Id, DateTime.Today.Year, DateTime.Today.Month);
					//REM: Leasing Amount is added only if it starts on current Month and Year
					//if(budgetEntity.AmountMonth - monthExpenses < orderAmount)
					//	return ResponseModel<int>.FailureResponse(key: "1", value: "Insufficient user Month Budget");

					// - 2024-02-06 - allow save w/o enough budget
					//var yearExpenses =
					//	Helpers.Processings.Budget.Order.getItemsAmount(Helpers.Processings.Budget.Order.getPurchaseItems(this._user.Id, DateTime.Today.Year, null), false, true, this._data.Discount ?? 0) +
					//	Helpers.Processings.Budget.Order.GetLeasingAmount(this._user.Id, DateTime.Today.Year, null);
					//REM: Leasing Amount is added only if it starts on current Month and Year
					//if(budgetEntity.AmountYear - yearExpenses < orderAmount)
					//	return ResponseModel<int>.FailureResponse(key: "1", value: "Insufficient user Year Budget");
				}
				#endregion Budget checks
			}


			return ResponseModel<int>.SuccessResponse();
		}
	}
}
