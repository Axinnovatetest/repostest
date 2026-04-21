using Newtonsoft.Json;
using Psz.Core.FinanceControl.Handlers.Budget.Order;
using Psz.Core.FinanceControl.Models.Budget.Order;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Helpers
{
	public class Processings
	{
		public class Budget
		{
			public class Order
			{
				public static List<Models.Budget.Order.OrderModel> GetOrderLeasingModels(List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> ordersExtensionEntities, Core.Identity.Models.UserModel currentUser, out List<string> errors, int year)
				{
					//errors = new List<string>();
					var orderModels = GetOrderModels(ordersExtensionEntities, currentUser, out errors);

					if(orderModels != null && orderModels.Count > 0)
					{
						for(int i = 0; i < orderModels.Count; i++)
						{
							orderModels[i].LeasingCurrentYearAmount = (orderModels[i].LeasingMonthAmount ?? 0) * getOrderLeasingYearNbMonths(orderModels[i].Id_Order, year);
						}
					}

					return orderModels;
				}
				public static List<OrderOptimisedModel> GetOrderOptimisedModels(
	List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> ordersExtensionEntities,
	out List<string> errors)
				{
					errors = new List<string>();
					if(ordersExtensionEntities == null || ordersExtensionEntities.Count <= 0)
						return null;

					var orderIds = ordersExtensionEntities.Select(x => x.OrderId)?.ToList();
					var projectIds = ordersExtensionEntities.Select(x => x.ProjectId ?? -1).ToList();

					// Fetch required entities
					var projectsEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(projectIds);

					var response = new List<OrderOptimisedModel>();
					ordersExtensionEntities = ordersExtensionEntities?.DistinctBy(x => x.Id)?.ToList();
					ordersExtensionEntities = ordersExtensionEntities.OrderByDescending(x => x.Id)?.ToList();
					var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderIds);

					try
					{
						foreach(var orderEntity in ordersExtensionEntities)
						{
							var projectEntity = projectsEntities?.Find(e => e.Id == orderEntity?.ProjectId);
							var articleEntity = articleEntites?.FindAll(x => x.OrderId == orderEntity.OrderId)?.ToList();

							// Create and add the optimised model
							var optimisedModel = new OrderOptimisedModel(orderEntity, projectEntity, articleEntity);
							response.Add(optimisedModel);
						}
					} catch(Exception exx)
					{
						Infrastructure.Services.Logging.Logger.Log(exx);
						throw;
					}

					// Log errors for debugging
					if(errors.Count > 0)
					{
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Debug, JsonConvert.SerializeObject(errors));
					}

					return response;
				}
				public static List<Models.Budget.Order.OrderModel> GetOrderModels(List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> ordersExtensionEntities, Core.Identity.Models.UserModel currentUser, out List<string> errors)
				{
					errors = new List<string>();
					if(ordersExtensionEntities == null || ordersExtensionEntities.Count <= 0)
						return null;

					var orderIds = ordersExtensionEntities.Select(x => x.OrderId)?.ToList();
					var projectIds = ordersExtensionEntities.Select(x => x.ProjectId ?? -1).ToList();
					var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(orderIds);
					var projectsEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(projectIds);

					Helpers.Processings.Budget.Order.updateArticlePrices(orderIds); // - FIXME: HEAVY processing

					var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderIds);
					List<int> BestellteArtikelNrs = articleEntites.Select(x => x.BestellteArtikelNr).ToList();
					var bestellteArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(BestellteArtikelNrs);
					var fileEntities = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdsOrder(orderIds);

					var response = new List<Models.Budget.Order.OrderModel>();
					Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenCustomerEntity = null;
					ordersExtensionEntities = ordersExtensionEntities?.DistinctBy(x => x.Id)?.ToList();
					ordersExtensionEntities = ordersExtensionEntities.OrderByDescending(x => x.Id)?.ToList();
					var _errors = new List<string>();
					try
					{
						foreach(var orderEntity in ordersExtensionEntities)
						{

							var bestellungEntity = bestellungEntities?.Find(x => x.Nr == orderEntity.OrderId);
							var projectEntity = projectsEntities?.Find(e => e.Id == orderEntity?.ProjectId);
							if(projectEntity != null)
							{
								var customer = (projectEntity.CustomerId.HasValue && projectEntity.Id_Type != 2)
							   ? Infrastructure.Data.Access.Tables.FNC.KundenAccess.Get(projectEntity.CustomerId.Value)
								  : null;

								if(customer != null)
								{
									adressenCustomerEntity = customer.Nummer.HasValue
									? Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(customer.Nummer.Value)
									: null;
								}
							}

							var articleEntity = articleEntites?.FindAll(x => x.OrderId == orderEntity.OrderId)?.ToList();
							var bestellteArticelEntity = bestellteArticleEntities?.FindAll(x => articleEntity.Select(y => y.BestellteArtikelNr)?.ToList()?.Exists(a => a == x.Nr) == true)?.ToList();
							var articlesEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(articleEntity?.Select(x => x.ArticleId)?.ToList());
							var fileEntity = fileEntities?.FindAll(x => x.Id_Order == orderEntity.OrderId);

							// Validators
							var validators = Validators.getByOrderId(orderEntity.OrderId, out _errors);
							if(_errors != null && _errors.Count > 0)
							{
								errors.AddRange(_errors);
								continue;
								//return null;
							}

							var uValidators = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validators?.Select(x => x.Id_Validator)?.ToList());

							// Handle last VALIDATOR as Profile NOT User
							response.Add(new Models.Budget.Order.OrderModel(orderEntity, bestellungEntity, projectEntity, articleEntity, bestellteArticelEntity, articlesEntities, fileEntity, validators, uValidators,
								currentUser,
								Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetReceptionsByOrderId_Count(orderEntity.OrderId)));
						}
					} catch(Exception exx)
					{
						Infrastructure.Services.Logging.Logger.Log(exx);
						throw;
					}
					// - log out errors for debugging
					if(errors.Count > 0)
					{
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Debug, JsonConvert.SerializeObject(errors));
					}

					return response;
				}
				public static List<Models.Budget.Order.OrderModel> GetOrderModelsNew(List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> ordersExtensionEntities, Core.Identity.Models.UserModel currentUser, out List<string> errors)
				{
					errors = new List<string>();
					try
					{

						if(ordersExtensionEntities == null || ordersExtensionEntities.Count <= 0)
							return null;

						var orderIds = ordersExtensionEntities.Select(x => x.OrderId)?.ToList();
						var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(orderIds);
						var projectsEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(ordersExtensionEntities.Select(x => x.ProjectId ?? -1)?.ToList());

						Helpers.Processings.Budget.Order.updateArticlePrices(orderIds); // - FIXME: HEAVY processing

						var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderIds);
						var bestellteArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(articleEntites?.Select(x => x.BestellteArtikelNr)?.ToList());
						var fileEntities = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdsOrder(orderIds);

						// - 2022-11-18 - customers
						var cutomers = Infrastructure.Data.Access.Tables.FNC.KundenAccess.Get(projectsEntities.Select(x => x.CustomerId ?? -1)?.ToList());
						var addresses = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(cutomers?.Select(x => x.Nummer ?? -1)?.ToList());

						var departments = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(ordersExtensionEntities.Select(x => (long)(x.DepartmentId ?? -1)).ToList());
						var companies = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(ordersExtensionEntities.Select(x => (long)(x.CompanyId ?? -1)).ToList());
						var companyExtensions = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompanyIds(companies?.Select(x => x.Id)?.ToList());
						var prjCompanyExtensions = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompanyIds(projectsEntities?.Select(x => x.CompanyId ?? -1)?.ToList());

						var issuers = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(ordersExtensionEntities.Select(x => x.IssuerId).ToList());
						var departmentHeadOfs = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(departments?.Select(x => (int)x.HeadUserId)?.ToList());
						var siteDirectors = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(companies?.Select(x => x.DirectorId ?? -1)?.ToList());
						var projectValidatorEntities = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectIds(projectsEntities?.Select(x => x.Id)?.ToList());
						var projectValidatorUsers = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(projectValidatorEntities?.Select(x => x.Id_Validator)?.ToList());
						var bookingCounts = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetReceptionsByOrderIds_Count(orderIds);
						var validatorEntities = new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
						if(issuers != null && issuers.Count > 0)
						{
							validatorEntities.AddRange(issuers);
						}
						if(departmentHeadOfs != null && departmentHeadOfs.Count > 0)
						{
							validatorEntities.AddRange(departmentHeadOfs);
						}
						if(siteDirectors != null && siteDirectors.Count > 0)
						{
							validatorEntities.AddRange(siteDirectors);
						}
						if(projectValidatorUsers != null && projectValidatorUsers.Count > 0)
						{
							validatorEntities.AddRange(projectValidatorUsers);
						}

						var response = new List<Models.Budget.Order.OrderModel>();
						Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenCustomerEntity = null;
						ordersExtensionEntities = ordersExtensionEntities?.DistinctBy(x => x.Id)?.ToList();
						ordersExtensionEntities = ordersExtensionEntities.OrderByDescending(x => x.Id)?.ToList();
						var _errors = new List<string>();

						Infrastructure.Data.Entities.Tables.COR.UserEntity issuer = null;
						Infrastructure.Data.Entities.Tables.COR.UserEntity departmentHeadOf = null;
						Infrastructure.Data.Entities.Tables.COR.UserEntity siteDirector = null;

						Infrastructure.Data.Entities.Tables.STG.DepartmentEntity departmentEntity = null;
						Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity = null;
						Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity companyExtensionEntity = null;
						Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity = null;
						Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity projectCompanyExtensionEntity = null;
						List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> projectValidators = null;
						int bookingCount = 0;

						try
						{
							foreach(var orderEntity in ordersExtensionEntities)
							{

								var bestellungEntity = bestellungEntities?.Find(x => x.Nr == orderEntity.OrderId);
								projectEntity = projectsEntities?.Find(e => e.Id == orderEntity?.ProjectId);
								if(projectEntity != null)
								{
									var customer = (projectEntity.CustomerId.HasValue && projectEntity.Id_Type != 2)
								   ? cutomers.FirstOrDefault(x => x.Nr == projectEntity.CustomerId.Value) // - Infrastructure.Data.Access.Tables.FNC.KundenAccess.Get(projectEntity.CustomerId.Value)
									  : null;

									if(customer != null)
									{
										adressenCustomerEntity = customer.Nummer.HasValue
										? addresses.FirstOrDefault(x => x.Nr == customer.Nummer.Value) // - Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(customer.Nummer.Value)
										: null;
									}

									// - 2022-11-18 -
									projectCompanyExtensionEntity = prjCompanyExtensions?.Find(x => x.CompanyId == projectEntity.CompanyId);
									projectValidators = projectValidatorEntities?.Where(x => x.Id_Project == projectEntity.Id)?.ToList();
								}

								var articleEntity = articleEntites?.Where(x => x.OrderId == orderEntity.OrderId)?.ToList();
								var bestellteArticelEntity = bestellteArticleEntities?.Where(x => x.Bestellung_Nr == orderEntity.OrderId)?.ToList();
								var articlesEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(articleEntity?.Select(x => x.ArticleId)?.ToList());
								var fileEntity = fileEntities?.Where(x => x.Id_Order == orderEntity.OrderId)?.ToList();

								// Validators
								departmentEntity = departments.Find(x => x.Id == orderEntity.DepartmentId);
								companyEntity = companies.Find(x => x.Id == orderEntity.CompanyId);
								companyExtensionEntity = companyExtensions.Find(x => x.CompanyId == orderEntity.CompanyId);

								issuer = issuers.FirstOrDefault(x => x.Id == orderEntity.IssuerId);
								departmentHeadOf = departmentHeadOfs.FirstOrDefault(x => x.Id == departmentEntity.HeadUserId);
								siteDirector = siteDirectors.FirstOrDefault(x => x.Id == companyEntity.DirectorId);
								var validators = Validators.getByOrderId(orderEntity, issuer, departmentHeadOf, siteDirector, departmentEntity, companyEntity, companyExtensionEntity, projectEntity, projectCompanyExtensionEntity, projectValidators, out _errors);
								if(_errors != null && _errors.Count > 0)
								{
									errors.AddRange(_errors);
									continue;
									//return null;
								}

								var uValidators = validatorEntities?.Where(y => validators?.Exists(x => x.Id_Validator == y.Id) == true)?.ToList();// - Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validators?.Select(x => x.Id_Validator)?.ToList());
								bookingCount = bookingCounts?.FirstOrDefault(x => x.Item1 == orderEntity.OrderId)?.Item2 ?? 0;

								// Handle last VALIDATOR as Profile NOT User
								response.Add(new Models.Budget.Order.OrderModel(orderEntity, bestellungEntity, projectEntity, articleEntity, bestellteArticelEntity, articlesEntities, fileEntity, validators, uValidators,
									currentUser, bookingCount));
							}
						} catch(Exception exx)
						{
							Infrastructure.Services.Logging.Logger.Log(exx);
							throw;
						}
						// - log out errors for debugging
						if(errors.Count > 0)
						{
							Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Debug, JsonConvert.SerializeObject(errors));
						}

						return response;
					} catch(Exception e)
					{

						throw;
					}
				}

				public static decimal getItemsAmount(List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> entities, bool? wVAT = true, bool defaultCurrency = false, decimal discount = 0)
				{
					if(defaultCurrency)
						return (entities?.Select(x => (x?.UnitPriceDefaultCurrency * x.Quantity * (1 + (wVAT.HasValue && wVAT.Value ? x.VAT : 0))) ?? 0)?.Sum() ?? 0) * (1m - discount / 100);
					else
						return (entities?.Select(x => (x?.UnitPrice * x.Quantity * (1 + (wVAT.HasValue && wVAT.Value ? x.VAT : 0))) ?? 0)?.Sum() ?? 0) * (1m - discount / 100);
				}
				public static decimal getOrderLeasingMonthAmount(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity, bool? wVAT = true, bool defaultCurrency = false)
				{
					if(defaultCurrency)
					{
						var orderCurrency = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(orderEntity.CurrencyId ?? -1)
							?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
						return (orderEntity?.LeasingMonthAmount ?? 0) * (decimal)(orderCurrency?.entspricht_DM ?? 1);
					}
					else
						return (orderEntity?.LeasingMonthAmount ?? 0);
				}
				public static int getOrderLeasingFirstYearNbMonths(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity)
				{
					return Math.Min((orderEntity.LeasingNbMonths ?? 0), 12 - (orderEntity.LeasingStartMonth ?? 1) + 1);
				}
				public static int getOrderLeasingLastYearNbMonths(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity)
				{
					return ((orderEntity.LeasingNbMonths ?? 0) - getOrderLeasingFirstYearNbMonths(orderEntity)) % 12;
				}
				public static int getOrderLeasingYearNbMonths(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity, int year)
				{
					var startDate = new DateTime((int)orderEntity.LeasingStartYear, (int)orderEntity.LeasingStartMonth, 1);
					var endDate = startDate.AddMonths(orderEntity.LeasingNbMonths ?? 0);
					// - 2024-12-06
					if(endDate.Year < year)
					{ return 0; }

					var nbMonthsInYear = 12; // - default 12 months in year

					var firstNbMonth = getOrderLeasingFirstYearNbMonths(orderEntity);
					if(year == startDate.Year)
					{
						nbMonthsInYear = firstNbMonth;
					}
					else if(year > startDate.Year)
					{
						if(year == endDate.Year)
						{
							var reminder = ((orderEntity.LeasingNbMonths ?? 0) - firstNbMonth) % 12;
							nbMonthsInYear = reminder == 0 ? 12 : ((orderEntity.LeasingNbMonths ?? 0) - firstNbMonth) % 12;
						}
					}

					return nbMonthsInYear;
				}
				public static int getOrderLeasingYearNbMonths(int orderId, int year)
				{
					var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(orderId);
					var startDate = new DateTime((int)orderEntity.LeasingStartYear, (int)orderEntity.LeasingStartMonth, 1);
					var endDate = startDate.AddMonths(orderEntity.LeasingNbMonths ?? 0);
					var nbMonthsInYear = 12; // - default 12 months in year

					var firstNbMonth = getOrderLeasingFirstYearNbMonths(orderEntity);
					if(year == startDate.Year)
					{
						nbMonthsInYear = firstNbMonth;
					}
					else if(year > startDate.Year)
					{
						if(year == endDate.Year)
						{
							var reminder = ((orderEntity.LeasingNbMonths ?? 0) - firstNbMonth) % 12;
							nbMonthsInYear = reminder == 0 ? 12 : ((orderEntity.LeasingNbMonths ?? 0) - firstNbMonth) % 12;
						}
					}

					return nbMonthsInYear;
				}
				public static void HandleArticles(Models.Budget.Order.OrderModel data, Identity.Models.UserModel user)
				{
					try
					{
						var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(data.Id_Order);
						Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.DeleteByOrderId(data.Id_Order);
						Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.DeleteByOrderId(data.Id_Order);

						if(data.Articles != null && data.Articles.Count > 0)
						{
							var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity?.CompanyId ?? -1);
							var currencyEntity = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(orderEntity?.CurrencyId ?? -1)
								?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
							var n = 1;
							foreach(var item in data.Articles)
							{
								item.Id_Order = data.Id_Order;
								item.Location_Id = orderEntity?.StorageLocationId ?? -1;
								item.Location_Name = orderEntity?.StorageLocationName;

								item.Id_Currency_Article = orderEntity?.CurrencyId;
								item.Currency_Article = orderEntity?.CurrencyName;

								item.DefaultCurrencyId = companyExtensionEntity?.DefaultCurrencyId;
								item.DefaultCurrencyName = companyExtensionEntity?.DefaultCurrencyName;
								item.DefaultCurrencyDecimals = currencyEntity?.Dezimalstellen;
								item.DefaultCurrencyRate = (decimal?)currencyEntity?.entspricht_DM;
								if(item.Id_Currency_Article > 0
									&& item.DefaultCurrencyId > 0)
								{
									item.UnitPriceDefaultCurrency = (bool)currencyEntity?.entspricht_DM.HasValue && currencyEntity?.entspricht_DM.Value > 0
										? item.Unit_Price * (decimal?)currencyEntity?.entspricht_DM
										: 1;
									item.TotalCostDefaultCurrency = item.UnitPriceDefaultCurrency * item.Quantity;
								}

								item.Position = n;
								// - -- -
								var id = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Insert(item.ToBestellteArtikelEntity());
								var bestellteItem = item.ToBestellteExtensionEntity();
								bestellteItem.BestellteArtikelNr = id;
								Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.Insert(bestellteItem);
								n++;
							}
						}
					} catch(Exception)
					{

						throw;
					}
				}
				public static void updateArticlePrices(List<int> orderIds)
				{
					// Fetch all necessary data in one go
					var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderIds(orderIds);
					var articleEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderIds);

					if(orderEntities == null || !orderEntities.Any() || articleEntitiesAll == null || !articleEntitiesAll.Any())
						return;

					List<int> companyIds = orderEntities
						.Where(o => o.CompanyId.HasValue)
						.Select(o => o.CompanyId.Value).Distinct().ToList();
					List<int> currencyIds = orderEntities
						.Where(o => o.CurrencyId.HasValue)
						.Select(o => o.CurrencyId.Value)
						.Distinct()
						.ToList();

					// Fetch company and currency data in bulk
					var companyExtensions = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get(companyIds)
						.ToDictionary(c => c.CompanyId);
					var currencyEntities = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(currencyIds)
						.ToDictionary(c => c.Nr);

					var updatedArticles = new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();

					foreach(var orderEntity in orderEntities)
					{
						if((orderEntity.Archived == null || orderEntity.Archived == false) &&
							(orderEntity.Deleted == null || orderEntity.Deleted == false) &&
							(orderEntity.Level <= (int)Enums.BudgetEnums.ValidationLevels.Draft))
						{
							var articleEntities = articleEntitiesAll.Where(x => x.OrderId == orderEntity.OrderId).ToList();

							if(articleEntities.Any())
							{
								var companyExtensionEntity = companyExtensions.GetValueOrDefault<int, Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>(orderEntity.CompanyId ?? -1);
								var currencyEntity = currencyEntities.GetValueOrDefault<int, Infrastructure.Data.Entities.Tables.STG.WahrungenEntity>(orderEntity.CurrencyId ?? -1) ?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity();

								foreach(var item in articleEntities)
								{
									item.DefaultCurrencyId = companyExtensionEntity?.DefaultCurrencyId;
									item.DefaultCurrencyName = companyExtensionEntity?.DefaultCurrencyName;
									item.DefaultCurrencyDecimals = currencyEntity?.Dezimalstellen;
									item.DefaultCurrencyRate = (decimal?)currencyEntity?.entspricht_DM;

									if(item.CurrencyId > 0 && item.DefaultCurrencyId > 0)
									{
										item.UnitPriceDefaultCurrency = currencyEntity?.entspricht_DM.HasValue == true && currencyEntity.entspricht_DM.Value > 0
											? item.UnitPrice * (decimal?)currencyEntity.entspricht_DM
											: 1;
										item.TotalCostDefaultCurrency = item.UnitPriceDefaultCurrency * item.Quantity;
									}

									updatedArticles.Add(item);
								}
							}
						}
					}

					// Bulk update articles
					Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.Update(updatedArticles);
				}
				//public static void updateArticlePrices(List<int> orderIds)
				//{
				//	var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderIds(orderIds);
				//	var articleEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderIds);

				//	if(orderEntities == null || orderEntities.Count <= 0 || articleEntitiesAll == null || articleEntitiesAll.Count <= 0)
				//		return;

				//	var updatedArticles = new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
				//	foreach(var orderEntity in orderEntities)
				//	{
				//		var articleEntities = articleEntitiesAll?.FindAll(x => x.OrderId == orderEntity.OrderId)?.ToList();

				//		if((orderEntity.Archived == null || orderEntity.Archived == false)  // - not archived
				//			&& (orderEntity.Deleted == null || orderEntity.Deleted == false) // - not deleted
				//			&& (orderEntity.Level <= (int)Enums.BudgetEnums.ValidationLevels.Draft)) // - not yet validated
				//		{
				//			if(articleEntities != null && articleEntities.Count > 0)
				//			{
				//				var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity?.CompanyId ?? -1);
				//				var currencyEntity = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(orderEntity?.CurrencyId ?? -1)
				//					?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
				//				foreach(var item in articleEntities)
				//				{
				//					item.DefaultCurrencyId = companyExtensionEntity?.DefaultCurrencyId;
				//					item.DefaultCurrencyName = companyExtensionEntity?.DefaultCurrencyName;
				//					item.DefaultCurrencyDecimals = currencyEntity?.Dezimalstellen;
				//					item.DefaultCurrencyRate = (decimal?)currencyEntity?.entspricht_DM;
				//					if(item.CurrencyId > 0 && item.DefaultCurrencyId > 0)
				//					{
				//						item.UnitPriceDefaultCurrency = (bool)currencyEntity?.entspricht_DM.HasValue && currencyEntity?.entspricht_DM.Value > 0
				//							? item.UnitPrice * (decimal?)currencyEntity?.entspricht_DM
				//							: 1;
				//						item.TotalCostDefaultCurrency = item.UnitPriceDefaultCurrency * item.Quantity;
				//					}
				//					// - -- -
				//					updatedArticles.Add(item);
				//				}
				//			}
				//		}
				//	}
				//	Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.Update(updatedArticles);
				//}
				public static List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> getPurchaseItems(int userId, int year, int? month, bool? validated = false, bool? filterOrdersByUser = true)
				{
					// -
					var orderEntites = month.HasValue
						? Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUserAndYearAndMonth(userId, year, month.Value,
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
						: Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUserAndYear(userId, year,
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase);

					if(validated == true)
						orderEntites = orderEntites.Where(x => x.Level > (int)Enums.BudgetEnums.ValidationLevels.Draft)?.ToList();

					// -
					if(filterOrdersByUser == true)
					{
						orderEntites = filterByUser(userId, orderEntites);
					}

					// -
					orderEntites = orderEntites
						?.Where(x =>
						(x.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower()
						|| x.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Finance.GetDescription().ToLower())
						&& x.Level > (int)Enums.BudgetEnums.ValidationLevels.Draft)?.ToList(); // Internal & validated

					// - 2024-03-22 use discount
					var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntites?.Select(x => x.OrderId)?.ToList());

					return articleEntities ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
				}
				public static decimal getPurchaseAmount(int userId, int year, int? month, bool? validated = false, bool? filterOrdersByUser = true)
				{
					var articleEntities = getPurchaseItems(userId, year, month, validated, filterOrdersByUser);
					var orderEntites = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderIds(articleEntities?.Select(x => x.OrderId)?.ToList());

					// - 2024-03-22 use discount
					foreach(var item in articleEntities)
					{
						var order = orderEntites?.FirstOrDefault(x => x.OrderId == item.OrderId);
						if(order != null && order.Discount.HasValue && order.Discount.Value > 0)
						{
							item.TotalCost = (1m - order.Discount / 100) * item.TotalCost;
							item.TotalCostDefaultCurrency = (1m - order.Discount / 100) * item.TotalCostDefaultCurrency;
							item.UnitPrice = (1m - order.Discount / 100) * item.UnitPrice;
							item.UnitPriceDefaultCurrency = (1m - order.Discount / 100) * item.UnitPriceDefaultCurrency;
						}
					}

					return articleEntities?.Select(x => x.TotalCostDefaultCurrency ?? 0)?.Sum() ?? 0;
				}
				public static decimal GetLeasingAmount(int userId, int year, int? month)
				{
					var orderEntites = month.HasValue
						? Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUserAndYearAndMonth(userId, year, month.Value,
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
						: Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUserAndYear(userId, year,
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing);

					// -
					orderEntites = filterByUser(userId, orderEntites);

					// - 
					return orderEntites
						?.Where(x =>
						(x.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower()
						|| x.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Finance.GetDescription().ToLower())
						&& x.Level > (int)Enums.BudgetEnums.ValidationLevels.Draft) // Internal & validated
						?.Select(x => ((x.LeasingTotalAmount ?? 0) / (x.LeasingNbMonths ?? 1)) * (month.HasValue ? 1 : getOrderLeasingYearNbMonths(x, year)))?.Sum() ?? 0;
				}
				public static decimal getLeasingUserAmount(int orderId, int year, int month)
				{
					var orderAmountValidatedEntity = Infrastructure.Data.Access.Tables.FNC.OrderValidationLeasingAccess.GetByOrderId(orderId)
							?.Where(x => x.ValidationLevel == 0 && x.ValidationTime.Year == year && x.ValidationTime.Month == month)
							?.FirstOrDefault();

					// - Return first year amount
					return orderAmountValidatedEntity?.OrderLeasingMonthAmount ?? 0;
				}
				public static decimal getLeasingAmount(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity extensionEntity, int year)
				{
					// - Return first year amount
					var nbMonth = getOrderLeasingYearNbMonths(extensionEntity, year);

					//var orderAmountValidatedEntity = Infrastructure.Data.Access.Tables.FNC.OrderValidationLeasingAccess.GetByOrderId(extensionEntity.OrderId)
					//        ?.Where(x => x.ValidationLevel == 0 && x.ValidationTime.Year == (extensionEntity.ValidationRequestTime ?? DateTime.MinValue).Year && x.ValidationTime.Month == (extensionEntity.ValidationRequestTime ?? DateTime.MinValue).Month)
					//        ?.FirstOrDefault();
					//// - 
					//return (orderAmountValidatedEntity?.OrderLeasingMonthAmount ?? 0) / (orderAmountValidatedEntity?.OrderLeasingYearTotalMonths ?? 1) * nbMonth;

					// -
					if(extensionEntity.ApprovalTime.HasValue && extensionEntity.ApprovalUserId.HasValue)
						return (extensionEntity.LeasingMonthAmount ?? 0m) * nbMonth;

					return 0m;
				}

				/// <summary>
				/// Apply Leasing fees periodiquely, given a time ( week, month, ...)
				/// For each order, if last validation is lower than current date, process the missing months
				/// </summary>
				public static void applyLeasingFees(bool sendSummaryEmail = true)
				{
					var inProgressLeasingOrderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByYearAndMonth(DateTime.Today.Year, DateTime.Today.Month)
						?.Where(x => x.ApprovalTime.HasValue && x.ApprovalUserId.HasValue)?.ToList();

					var emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
							+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

					if(inProgressLeasingOrderEntities == null || inProgressLeasingOrderEntities.Count <= 0)
					{
						#region >>>> Summary Notification <<<<
						try
						{
							emailContent += $"<br/>{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")} | No pending Leasing Orders.";
							// Send email notification
							Module.EmailingService.SendEmailAsync("[Budget] Leasing Order Summary - (0)", emailContent, new List<string> { Module.EmailingService.EmailParamtersModel.AdminEmail }, null,
								saveHistory: true, senderEmail: "", senderName: "", senderId: 0, senderCC: false, attachmentIds: null);
						} catch(Exception ex)
						{
							Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{Module.EmailingService.EmailParamtersModel.AdminEmail}]"));
							Infrastructure.Services.Logging.Logger.Log(ex);
						}
						#endregion Summary Notification
						return;
					}

					try
					{
						var orderIds = inProgressLeasingOrderEntities.Select(x => x.OrderId).Distinct().ToList();
						var leasingValidationHistory = Infrastructure.Data.Access.Tables.FNC.OrderValidationLeasingAccess.GetFirstByOrderId(orderIds);
						foreach(var orderItem in inProgressLeasingOrderEntities)
						{
							// get last validation date
							var lastValidation = leasingValidationHistory
								?.Where(x => x.OrderId == orderItem.OrderId)
								?.Aggregate((y, z) => y.ValidationTime > z.ValidationTime ? y : z);

							if(lastValidation != null && (DateTime.Today.Year - lastValidation.ValidationTime.Year) > 0)
							{
								applyOrderLeasingFees(orderItem, lastValidation, sendSummaryEmail);
							}
						}
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(ex);
					}

					#region >>>> Summary Notification <<<<
					try
					{
						emailContent += $"<br/>{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")} | {inProgressLeasingOrderEntities.Count} pending Leasing Orders processed.";

						// Send email notification
						Module.EmailingService.SendEmailAsync($"[Budget] Leasing Order Summary - ({inProgressLeasingOrderEntities.Count})", emailContent, new List<string> { Module.EmailingService.EmailParamtersModel.AdminEmail }, null,
						saveHistory: true, senderEmail: "", senderName: "", senderId: 0, senderCC: false, attachmentIds: null);
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{Module.EmailingService.EmailParamtersModel.AdminEmail}]"));
						Infrastructure.Services.Logging.Logger.Log(ex);
					}
					#endregion Summary Notification
				}
				internal static void applyOrderLeasingFees(
					Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity,
					Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity lastLeasingEntity, bool sendSummaryEmail = true)
				{
					{
						#region >>>> Budget <<<<
						// -- Decrease Budget
						var nbYearUnpaid = DateTime.Today.Year - lastLeasingEntity.ValidationTime.Year;
						var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderEntity.IssuerId);
						var department = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderEntity.DepartmentId ?? -1);
						var company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderEntity.CompanyId ?? -1);
						var companyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);

						var nextValidatorEmail = new List<string> { };
						string emailTitle, emailContent;

						if(nbYearUnpaid > 0)
						{
							var defaultCurrency = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(orderEntity.CurrencyId ?? -1)
								?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
							var orderArticleExtensionEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(orderEntity.OrderId);
							var validationHsitoryEntities = new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity> { };

							for(int i = 1; i < nbYearUnpaid + 1; i++)
							{
								var currentDate = new DateTime(lastLeasingEntity.ValidationTime.Year + i, 1, 1);
								var startDate = new DateTime((int)orderEntity.LeasingStartYear, (int)orderEntity.LeasingStartMonth, 1);
								var endDate = startDate.AddMonths(orderEntity.LeasingNbMonths ?? 0);
								var nbMonthsInYear = 12; // - default 12 months in year

								var firstNbMonth = getOrderLeasingFirstYearNbMonths(orderEntity);
								if(currentDate.Year == startDate.Year)
								{
									nbMonthsInYear = firstNbMonth;
								}
								else if(currentDate.Year > startDate.Year)
								{
									if(currentDate.Year == endDate.Year)
									{
										var reminder = ((orderEntity.LeasingNbMonths ?? 0) - firstNbMonth) % 12;
										nbMonthsInYear = reminder == 0 ? 12 : ((orderEntity.LeasingNbMonths ?? 0) - firstNbMonth) % 12;
									}
								}

								var orderAmount = nbMonthsInYear * lastLeasingEntity.OrderLeasingMonthAmount; // MonthAmount computed on FIRST Validation

								orderEntity.ValidationRequestTime = DateTime.Now;
								orderEntity.BudgetYear = orderEntity.ValidationRequestTime?.Year ?? -1;

								// - Always remove from User 
								var userBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userEntity.Id, currentDate.Year);
								if(userBudgetEntity != null)
								{
									userBudgetEntity.AmountSpent = userBudgetEntity.AmountSpent + ((decimal?)orderAmount ?? 0);
									Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Update(userBudgetEntity);

									nextValidatorEmail.Add(userEntity?.Email);
									// -
									validationHsitoryEntities.Add(new Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity
									{
										Id = -1,
										OrderId = orderEntity?.OrderId ?? -1,
										OrderArticleCount = orderArticleExtensionEntites?.Count ?? 0,
										OrderProjectId = orderEntity?.ProjectId ?? -1,
										OrderType = orderEntity.OrderType,
										OrderIssuerId = orderEntity.IssuerId,
										UserId = userEntity.Id,
										ValidationLevel = 0,
										ValidationNotes = $"Auto deduction by System Agent [{userEntity?.Email}].",
										ValidationTime = DateTime.Now,
										DefaultCurrencyId = defaultCurrency.Nr,
										DefaultCurrencyDecimals = defaultCurrency.Dezimalstellen,
										DefaultCurrencyName = defaultCurrency.Symbol,
										DefaultCurrencyRate = (decimal?)defaultCurrency.entspricht_DM,
										OrderTotalAmount = orderEntity.LeasingTotalAmount ?? 0,
										OrderTotalAmountDefaultCurrency = (orderEntity.LeasingTotalAmount ?? 0) * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingMonthAmount = lastLeasingEntity.OrderLeasingMonthAmount,
										OrderLeasingMonthAmountDefaultCurrency = lastLeasingEntity.OrderLeasingMonthAmount * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingYear = DateTime.Today.Year,
										OrderLeasingYearTotalAmount = nbMonthsInYear * lastLeasingEntity.OrderLeasingMonthAmount,
										OrderLeasingYearTotalAmountDefaultCurrency = nbMonthsInYear * lastLeasingEntity.OrderLeasingMonthAmount * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingYearTotalMonths = nbMonthsInYear
									});
								}

								// - validate from HeadOf
								var deptBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(orderEntity.DepartmentId ?? -1, currentDate.Year);
								if(deptBudgetEntity != null)
								{
									deptBudgetEntity.AmountSpent = deptBudgetEntity.AmountSpent + orderAmount;
									Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(deptBudgetEntity);

									nextValidatorEmail.Add(department?.HeadUserEmail);
									// -
									validationHsitoryEntities.Add(new Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity
									{
										Id = -1,
										OrderId = orderEntity?.OrderId ?? -1,
										OrderArticleCount = orderArticleExtensionEntites?.Count ?? 0,
										OrderProjectId = orderEntity?.ProjectId ?? -1,
										OrderType = orderEntity.OrderType,
										OrderIssuerId = orderEntity.IssuerId,
										UserId = (int?)department?.HeadUserId ?? -1,
										ValidationLevel = 1,
										ValidationNotes = $"Auto deduction by System Agent [{department?.HeadUserEmail}].",
										ValidationTime = DateTime.Now,
										DefaultCurrencyId = defaultCurrency.Nr,
										DefaultCurrencyDecimals = defaultCurrency.Dezimalstellen,
										DefaultCurrencyName = defaultCurrency.Symbol,
										DefaultCurrencyRate = (decimal?)defaultCurrency.entspricht_DM,
										OrderTotalAmount = orderEntity.LeasingTotalAmount ?? 0,
										OrderTotalAmountDefaultCurrency = (orderEntity.LeasingTotalAmount ?? 0) * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingMonthAmount = lastLeasingEntity.OrderLeasingMonthAmount,
										OrderLeasingMonthAmountDefaultCurrency = lastLeasingEntity.OrderLeasingMonthAmount * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingYear = DateTime.Today.Year,
										OrderLeasingYearTotalAmount = nbMonthsInYear * lastLeasingEntity.OrderLeasingMonthAmount,
										OrderLeasingYearTotalAmountDefaultCurrency = nbMonthsInYear * lastLeasingEntity.OrderLeasingMonthAmount * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingYearTotalMonths = nbMonthsInYear
									});
								}

								// - validate from Site dir
								var siteBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(orderEntity.CompanyId ?? -1, currentDate.Year);
								if(siteBudgetEntity != null)
								{
									siteBudgetEntity.AmountSpent = siteBudgetEntity.AmountSpent + orderAmount;
									Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(siteBudgetEntity);

									nextValidatorEmail.Add(company?.DirectorEmail);
									nextValidatorEmail.Add(companyExtension?.PurchaseGroupEmail);
									var fncAccessProfile = Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.GetByMainAccessProfilesIds(new List<int> { companyExtension?.PurchaseProfileId ?? -1 });
									var purchaseUserProfiles = Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByAccessProfileIds(fncAccessProfile?.Select(x => x.Id).ToList());
									nextValidatorEmail.AddRange(
										Infrastructure.Data.Access.Tables.COR.UserAccess.Get(purchaseUserProfiles?.Select(x => x.UserId)?.ToList())
										?.Where(x => x.CompanyId == companyExtension.CompanyId)?.Select(x => x.Email.Trim())?.ToList());

									// - Director
									validationHsitoryEntities.Add(new Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity
									{
										Id = -1,
										OrderId = orderEntity?.OrderId ?? -1,
										OrderArticleCount = orderArticleExtensionEntites?.Count ?? 0,
										OrderProjectId = orderEntity?.ProjectId ?? -1,
										OrderType = orderEntity.OrderType,
										OrderIssuerId = orderEntity.IssuerId,
										UserId = company?.DirectorId ?? -1,
										ValidationLevel = 2,
										ValidationNotes = $"Auto deduction by System Agent [{company?.DirectorEmail}].",
										ValidationTime = DateTime.Now,
										DefaultCurrencyId = defaultCurrency.Nr,
										DefaultCurrencyDecimals = defaultCurrency.Dezimalstellen,
										DefaultCurrencyName = defaultCurrency.Symbol,
										DefaultCurrencyRate = (decimal?)defaultCurrency.entspricht_DM,
										OrderTotalAmount = orderEntity.LeasingTotalAmount ?? 0,
										OrderTotalAmountDefaultCurrency = (orderEntity.LeasingTotalAmount ?? 0) * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingMonthAmount = lastLeasingEntity.OrderLeasingMonthAmount,
										OrderLeasingMonthAmountDefaultCurrency = lastLeasingEntity.OrderLeasingMonthAmount * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingYear = DateTime.Today.Year,
										OrderLeasingYearTotalAmount = nbMonthsInYear * lastLeasingEntity.OrderLeasingMonthAmount,
										OrderLeasingYearTotalAmountDefaultCurrency = nbMonthsInYear * lastLeasingEntity.OrderLeasingMonthAmount * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingYearTotalMonths = nbMonthsInYear
									});
									// - Purchase
									validationHsitoryEntities.Add(new Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity
									{
										Id = -1,
										OrderId = orderEntity?.OrderId ?? -1,
										OrderArticleCount = orderArticleExtensionEntites?.Count ?? 0,
										OrderProjectId = orderEntity?.ProjectId ?? -1,
										OrderType = orderEntity.OrderType,
										OrderIssuerId = orderEntity.IssuerId,
										UserId = -1,
										ValidationLevel = 3,
										ValidationNotes = "Auto deduction by System Agent [Purchase].",
										ValidationTime = DateTime.Now,
										DefaultCurrencyId = defaultCurrency.Nr,
										DefaultCurrencyDecimals = defaultCurrency.Dezimalstellen,
										DefaultCurrencyName = defaultCurrency.Symbol,
										DefaultCurrencyRate = (decimal?)defaultCurrency.entspricht_DM,
										OrderTotalAmount = orderEntity.LeasingTotalAmount ?? 0,
										OrderTotalAmountDefaultCurrency = (orderEntity.LeasingTotalAmount ?? 0) * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingMonthAmount = lastLeasingEntity.OrderLeasingMonthAmount,
										OrderLeasingMonthAmountDefaultCurrency = lastLeasingEntity.OrderLeasingMonthAmount * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingYear = DateTime.Today.Year,
										OrderLeasingYearTotalAmount = nbMonthsInYear * lastLeasingEntity.OrderLeasingMonthAmount,
										OrderLeasingYearTotalAmountDefaultCurrency = nbMonthsInYear * lastLeasingEntity.OrderLeasingMonthAmount * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingYearTotalMonths = nbMonthsInYear
									});
								}

								#region >>>> Leasing fees history <<<<
								Infrastructure.Data.Access.Tables.FNC.OrderLeasingFeesHistoryAccess.Insert(
									new Infrastructure.Data.Entities.Tables.FNC.OrderLeasingFeesHistoryEntity
									{
										DefaultCurrencyDecimals = orderEntity.DefaultCurrencyDecimals,
										DefaultCurrencyId = orderEntity.DefaultCurrencyId,
										DefaultCurrencyName = orderEntity.DefaultCurrencyName,
										DefaultCurrencyRate = orderEntity.DefaultCurrencyRate,
										Id = -1,
										InsertTime = DateTime.Now,
										OrderArticleCount = -1,
										OrderId = orderEntity.OrderId,
										OrderIssuerId = orderEntity.IssuerId,
										OrderLeasingMonthAmount = lastLeasingEntity.OrderLeasingMonthAmount,
										OrderLeasingMonthAmountDefaultCurrency = lastLeasingEntity.OrderLeasingMonthAmountDefaultCurrency,
										OrderLeasingYear = currentDate.Year,
										OrderLeasingYearTotalAmount = orderAmount,
										OrderLeasingYearTotalAmountDefaultCurrency = orderAmount * (decimal)(defaultCurrency.entspricht_DM ?? 0),
										OrderLeasingYearTotalMonths = nbMonthsInYear,
										OrderProjectId = orderEntity.ProjectId ?? -1,
										OrderTotalAmount = -1,
										OrderTotalAmountDefaultCurrency = -1,
										OrderType = orderEntity.OrderType,
										UserId = -2
									});

								//- validationhistory
								Infrastructure.Data.Access.Tables.FNC.OrderValidationLeasingAccess.Insert(validationHsitoryEntities);
								#endregion
							}
						}

						// - 
						#endregion Budget

						#region >>>> Email Notification <<<<

						if(sendSummaryEmail == true && nextValidatorEmail.Count > 0)
						{
							getEmailBody(orderEntity, userEntity, out emailTitle, out emailContent);
							var attachments = new List<KeyValuePair<string, System.IO.Stream>>();
							var attachmentIds = new List<int>();
							try
							{
								var fileEntities = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdOrder(orderEntity.OrderId);
								if(fileEntities != null && fileEntities.Count > 0)
								{
									var n = 0;
									foreach(var fileItem in fileEntities)
									{
										if(fileItem.Id_File > 0)
										{
											var data = Psz.Core.Common.Program.FilesManager.GetFile(fileItem.FileId);
											if(data != null)
											{
												attachments.Add(new KeyValuePair<string, System.IO.Stream>($"AttachedFile{n++}{data.FileExtension}", new System.IO.MemoryStream(data.FileBytes)));
												attachmentIds.Add(fileItem.FileId);
											}
										}
									}
								}
							} catch(Exception e)
							{
								Infrastructure.Services.Logging.Logger.Log(e);
							}

							try
							{
								var reportData = ReportHandler.generateReportData(orderEntity.OrderId, companyExtension.ReportDefaultLanguageId);
								if(reportData != null)
								{
									attachments.Add(new KeyValuePair<string, System.IO.Stream>($"{orderEntity.OrderNumber}.pdf", new System.IO.MemoryStream(reportData)));
								}
							} catch(Exception e)
							{
								Infrastructure.Services.Logging.Logger.Log(e);
							}

							try
							{
								// - 2025-04-03 - Khelil(mail) - remove Steinbacher from mail list
								nextValidatorEmail = nextValidatorEmail?.Where(x => x?.Trim()?.ToLower() != "werner.steinbacher@psz-electronic.com")?.ToList();

								// Send email notification
								Module.EmailingService.SendEmailAsync(emailTitle, emailContent, nextValidatorEmail, attachments, new List<string> { Module.EmailingService.EmailParamtersModel.AdminEmail },
									saveHistory: true, senderEmail: userEntity.Email, senderName: userEntity.Username, senderId: userEntity.Id, senderCC: false, attachmentIds: attachmentIds);
							} catch(Exception ex)
							{
								Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", nextValidatorEmail)}]"));
								Infrastructure.Services.Logging.Logger.Log(ex);
								//throw new Exception($"Unable to send email to [{string.Join(", ", nextValidatorEmail)}]");
							}
						}
						#endregion Email Notification
					}
				}
				private static void getEmailBody(
					Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity,
					Infrastructure.Data.Entities.Tables.COR.UserEntity issuerEntity, out string emailTitle, out string emailContent)
				{
					emailTitle = "[Budget] Leasing Order";
					emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
							+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

					emailContent += $"<br/><span style='font-size:1.15em;'>This is budget update for the leasing order <strong>{orderEntity.OrderNumber?.ToUpper()}</strong>";
					emailContent += $" issued by <strong>{issuerEntity.Name?.ToUpper()}</strong> on {orderEntity.CreationDate.Value.ToString("dd.MM.yyyy HH:mm")}.";

					emailContent += $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/budget-rebuild/orders/edit/{orderEntity.OrderId}'>here</a>";
					emailContent += "<br/><br/>Regards, <br/>IT Department </div>";
				}
				public static void SaveOrderHistory(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity,
					Enums.BudgetEnums.OrderWorkflowActions action,
					Core.Identity.Models.UserModel userEntity,
					string comments = null)
				{
					var issuerEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderEntity.IssuerId);
					Infrastructure.Data.Access.Tables.FNC.OrderWorkflowHistoryAccess.Insert(
						new Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity
						{
							Id = -1,
							OrderId = orderEntity.OrderId,
							OrderIssuerUserEmail = issuerEntity?.Email,
							OrderIssuerUserId = orderEntity.IssuerId,
							OrderIssuerUserName = orderEntity.IssuerName,
							OrderNumber = orderEntity.OrderNumber,
							WorkflowActionId = (int)action,
							WorkflowActionName = action.GetDescription(),
							WorkflowActionTime = DateTime.Now,
							WorkflowActionUserEmail = userEntity.Email,
							WorkflowActionUserId = userEntity.Id,
							WorkflowActionUserName = userEntity.Name,
							WorkflowActionComments = comments
						});
				}
				public static void SaveOrderHistory(Infrastructure.Services.Utils.TransactionsManager botransactionFNC, Infrastructure.Services.Utils.TransactionsManager botransactionDefault, Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity,
					Enums.BudgetEnums.OrderWorkflowActions action,
					Core.Identity.Models.UserModel userEntity,
					string comments = null)
				{
					var issuerEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.GetWithTransaction(orderEntity.IssuerId, botransactionDefault.connection, botransactionDefault.transaction);
					Infrastructure.Data.Access.Tables.FNC.OrderWorkflowHistoryAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity
						{
							Id = -1,
							OrderId = orderEntity.OrderId,
							OrderIssuerUserEmail = issuerEntity?.Email,
							OrderIssuerUserId = orderEntity.IssuerId,
							OrderIssuerUserName = orderEntity.IssuerName,
							OrderNumber = orderEntity.OrderNumber,
							WorkflowActionId = (int)action,
							WorkflowActionName = action.GetDescription(),
							WorkflowActionTime = DateTime.Now,
							WorkflowActionUserEmail = userEntity.Email,
							WorkflowActionUserId = userEntity.Id,
							WorkflowActionUserName = userEntity.Name,
							WorkflowActionComments = comments
						}, botransactionFNC.connection, botransactionFNC.transaction);
				}

				private static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> filterByUser(int userId, List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> orderEntites)
				{
					// - filter orders by uer company and department
					var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(userId);
					var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(userEntity.CompanyId ?? -1);
					var departmentEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(userEntity.DepartmentId ?? -1);
					if(companyEntity != null)
					{
						orderEntites = orderEntites
							?.Where(x => x.CompanyId.HasValue == false || (x.CompanyId.HasValue == true && x.CompanyId.Value == companyEntity.Id))
							?.ToList();
					}
					if(departmentEntity != null)
					{
						orderEntites = orderEntites
							?.Where(x => x.DepartmentId.HasValue == false || (x.DepartmentId.HasValue == true && x.DepartmentId.Value == departmentEntity.Id))
							?.ToList();
					}

					return orderEntites;
				}

				public static decimal getAmountSpent_User(int userId, bool includeLeasing = true)
				{
					var amount = 0m;
					amount = getPurchaseAmount(userId, DateTime.Today.Year, month: null, validated: true, false);
					if(includeLeasing)
					{
						amount += GetLeasingAmount(userId, DateTime.Today.Year, month: null);
					}

					return amount;
				}

				public static decimal GetPurchaseAmount_Department(int departmentId, int year)
				{
					return getPurchaseAmount_Department(departmentId, year, true);
				}
				public static decimal GetAmountSpent_Department(int departmentId, bool includeLeasing = true)
				{
					var amount = 0m;
					amount = getPurchaseAmount_Department(departmentId, DateTime.Today.Year, validated: true);
					if(includeLeasing)
					{
						amount += getLeasingAmount_Department(departmentId, DateTime.Today.Year, true);
					}

					return amount;
				}
				private static decimal getPurchaseAmount_Department(int departmentId, int year, bool? validated)
				{
					var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartmentAndYear(departmentId, year,
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase);

					orderEntities = orderEntities
						?.Where(x => x.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower() && x.Level > (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector)?.ToList(); // Internal & validated

					if(validated == true)
						orderEntities = orderEntities.Where(x => x.Level > (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector)?.ToList();

					var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities?.Select(x => x.OrderId)?.ToList());
					// - 2024-03-22 use discount
					foreach(var item in articleEntities)
					{
						var order = orderEntities?.FirstOrDefault(x => x.OrderId == item.OrderId);
						if(order != null && order.Discount.HasValue && order.Discount.Value > 0)
						{
							item.TotalCost = (1m - order.Discount / 100) * item.TotalCost;
							item.TotalCostDefaultCurrency = (1m - order.Discount / 100) * item.TotalCostDefaultCurrency;
							item.UnitPrice = (1m - order.Discount / 100) * item.UnitPrice;
							item.UnitPriceDefaultCurrency = (1m - order.Discount / 100) * item.UnitPriceDefaultCurrency;
						}
					}
					return articleEntities?.Select(x => x.TotalCostDefaultCurrency ?? 0)?.Sum() ?? 0;
				}
				private static decimal getLeasingAmount_Department(int departmentId, int year, bool? validated)
				{
					var orderEntites = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartmentAndYear(departmentId, year,
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing);

					if(validated == true)
						orderEntites = orderEntites.Where(x => x.Level > (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector)?.ToList();

					// - 
					return orderEntites
						?.Where(x => x.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower() && x.Level > (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector) // Internal & validated
						?.Select(x => ((x.LeasingTotalAmount ?? 0) / (x.LeasingNbMonths ?? 1)) * (getOrderLeasingYearNbMonths(x, year)))?.Sum() ?? 0;
				}

				public static decimal GetPurchaseAmount_Company(int companyId, int year)
				{
					return getPurchaseAmount_Company(companyId, year, true);
				}
				public static decimal GetAmountSpent_Company(int companyId, bool includeLeasing = true)
				{
					var amount = 0m;
					amount = getPurchaseAmount_Company(companyId, DateTime.Today.Year, validated: true);
					if(includeLeasing)
					{
						amount += getLeasingAmount_Company(companyId, DateTime.Today.Year, true);
					}

					return amount;
				}
				private static decimal getPurchaseAmount_Company(int companyId, int year, bool? validated)
				{
					var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanyAndYear(companyId, year,
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase);

					orderEntities = orderEntities
						?.Where(x => x.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower() && x.Level > (int)Enums.BudgetEnums.ValidationLevels.SiteDirector)?.ToList(); // Internal & validated

					if(validated == true)
						orderEntities = orderEntities.Where(x => x.Level > (int)Enums.BudgetEnums.ValidationLevels.SiteDirector)?.ToList();

					var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities?.Select(x => x.OrderId)?.ToList());
					// - 2024-03-22 use discount
					foreach(var item in articleEntities)
					{
						var order = orderEntities?.FirstOrDefault(x => x.OrderId == item.OrderId);
						if(order != null && order.Discount.HasValue && order.Discount.Value > 0)
						{
							item.TotalCost = (1m - order.Discount / 100) * item.TotalCost;
							item.TotalCostDefaultCurrency = (1m - order.Discount / 100) * item.TotalCostDefaultCurrency;
							item.UnitPrice = (1m - order.Discount / 100) * item.UnitPrice;
							item.UnitPriceDefaultCurrency = (1m - order.Discount / 100) * item.UnitPriceDefaultCurrency;
						}
					}
					return articleEntities?.Select(x => x.TotalCostDefaultCurrency ?? 0)?.Sum() ?? 0;
				}
				private static decimal getLeasingAmount_Company(int companyId, int year, bool? validated)
				{
					var orderEntites = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanyAndYear(companyId, year,
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing);

					if(validated == true)
						orderEntites = orderEntites.Where(x => x.Level > (int)Enums.BudgetEnums.ValidationLevels.SiteDirector)?.ToList();

					// - 
					return orderEntites
						?.Where(x => x.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower() && x.Level > (int)Enums.BudgetEnums.ValidationLevels.SiteDirector) // Internal & validated
						?.Select(x => ((x.LeasingTotalAmount ?? 0) / (x.LeasingNbMonths ?? 1)) * (getOrderLeasingYearNbMonths(x, year)))?.Sum() ?? 0;
				}
			}

			public class Project
			{
				public static void SaveProjectHistory(Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
					Enums.BudgetEnums.ProjectWorkflowActions action,
					Core.Identity.Models.UserModel userEntity,
					string comments = null)
				{
					var issuerEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(projectEntity.CreationUserId ?? -1);
					Infrastructure.Data.Access.Tables.FNC.ProjectWorkflowHistoryAccess.Insert(
						new Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity
						{
							Id = -1,
							ProjectId = projectEntity.Id,
							ProjectOwnerUserEmail = issuerEntity?.Email,
							ProjectOwnerUserId = projectEntity.CreationUserId,
							ProjectOwnerUserName = projectEntity.CreationUserName,
							ProjectName = projectEntity.ProjectName,
							WorkflowActionId = (int)action,
							WorkflowActionName = action.GetDescription(),
							WorkflowActionTime = DateTime.Now,
							WorkflowActionUserEmail = userEntity.Email,
							WorkflowActionUserId = userEntity.Id,
							WorkflowActionUserName = userEntity.Name,
							WorkflowActionComments = comments
						});
				}
			}
			public static bool HasPurchaseProfile(int userId, Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity companyExtensionEntity)
			{
				var purchaseUserEntities = Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { userId });
				var fncAccessProfileEntities = Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(purchaseUserEntities?.Select(x => x.AccessProfileId).ToList());
				return fncAccessProfileEntities.FindIndex(x => x.MainAccessProfileId == companyExtensionEntity?.PurchaseProfileId) >= 0;
			}

			public class User
			{
				public static bool HasDifferentAllocation(int userId, int? newDepartment, int? newCompany)
				{
					var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(userId);
					if(userEntity == null)
						return false;

					var userAllocationEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userId, DateTime.Today.Year);
					if(userAllocationEntity == null)
						return false;

					var userDepartmentEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(userEntity.DepartmentId ?? -1);
					return userDepartmentEntity.CompanyId != newCompany || userDepartmentEntity.Id != newDepartment;
				}
			}
		}
	}
}
