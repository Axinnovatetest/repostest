using Infrastructure.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Identity.Handlers
{
	public partial class AccessProfile
	{
		public static Common.Models.ResponseModel<Models.AccessProfileModel> Get(int id, Models.UserModel user)
		{
			try
			{
				if(user == null
					|| !user.Access.Settings.AccessProfiles)
				{
					throw new SharedKernel.Exceptions.UnauthorizedException();
				}

				var accessProfile = Get(id);
				if(accessProfile == null)
				{
					throw new SharedKernel.Exceptions.NotFoundException();
				}

				return Common.Models.ResponseModel<Models.AccessProfileModel>.SuccessResponse(accessProfile);
			} catch(Exception e)
			{
				Logger.Log(e);
				throw;
			}
		}
		public static Models.AccessProfileModel Get(int id)
		{
			try
			{
				return Get(new List<int>() { id }).FirstOrDefault();
			} catch(Exception e)
			{
				Logger.Log(e);
				throw;
			}
		}
		public static Models.AccessProfileModel GetUserProfiles(int id)
		{
			try
			{
				var usersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(id);
				var accessProfile = new Models.AccessProfileModel()
				{
					Id = usersDb.Id,
					Name = usersDb.Name,
					CreationUser = usersDb.Username,
					Settings = new Models.SettingsAccessModel(),
					WorkPlan = new Models.WorkPlanAccessModel(),
					Purchase = new Models.PurchaseAccessModel(),
					Budget = new Models.FNC_AccessProfileModel(),
					Financial = new Psz.Core.Identity.Models.FinancialAccessModel(),
					Administration = new Psz.Core.Identity.Models.AdministrationAccessModel(),
					CustomerService = new Psz.Core.Identity.Models.CustomerServiceAccessModel(),
					HumanResources = new Psz.Core.Identity.Models.HumanResourcesAccessModel(),
					MaterialManagement = new Psz.Core.Identity.Models.MaterialManagementAccessModel(),
					MasterData = new Psz.Core.Identity.Models.MasterDataAccessModel(),
					Logistics = new Psz.Core.Identity.Models.LogisticsAccessModel(),
					SalesDistribution = new Psz.Core.Identity.Models.SalesDistributionAccessModel(),
					ManagementOverview = new Models.ManagementOverviewAccessModel(),
					CRP = new Models.CRPAccessModel(),
					CapitalRequests = new Models.CapitalRequestsAccessModel(),
				};

				var _mainProfileEntites = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(usersDb.AccessProfileId);

				#region Admin
				var stgAccessProfileDb = Infrastructure.Data.Access.Tables.STG.AccessProfileAccess.GetByMainAccessProfilesIds(new List<int> { usersDb.AccessProfileId });
				if(stgAccessProfileDb != null && stgAccessProfileDb.Count > 0)
				{
					accessProfile.Administration = new Psz.Core.Identity.Models.AdministrationAccessModel(stgAccessProfileDb);
				}
				#endregion Admin

				#region Customer Service
				var _mainCSVProfileEntites = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(usersDb.AccessProfileId);
				var _customerServiceuserentities = Infrastructure.Data.Access.Tables.CTS.AccessProfileUsersAccess.GetByUersId(id);
				var ctsIds = _customerServiceuserentities?.Select(x => (int)x.AccessProfileId).ToList()
					?? new List<int>();
				var _customerServiceprofilesEntity = Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.Get(ctsIds);
				accessProfile.CustomerService = new Psz.Core.Identity.Models.CustomerServiceAccessModel(_customerServiceprofilesEntity);
				#endregion

				#region Logistics
				// -- Check EDI
				//var _mainCSVProfileEntites = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(usersDb.AccessProfileId);

				var _logisticsuserentities = Infrastructure.Data.Access.Tables.Logistics.AccessProfileUsersAccess.GetByUersId(id);
				var lgtIds = _logisticsuserentities?.Select(x => (int)x.AccessProfileId).ToList()
					?? new List<int>();
				var _logisticsprofilesEntity = Infrastructure.Data.Access.Tables.Logistics.AccessProfileAccess.Get(lgtIds);

				accessProfile.Logistics = new Psz.Core.Identity.Models.LogisticsAccessModel(_logisticsprofilesEntity);
				#endregion

				#region Finance Control
				var _fncUserEntities = Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { id });
				var _fncprofilesEntity = Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(_fncUserEntities?.Select(x => x.AccessProfileId).ToList());
				var fncIds = _fncprofilesEntity?.Select(x => (int)x.Id).ToList()
					?? new List<int>();
				accessProfile.Financial = new Psz.Core.Identity.Models.FinancialAccessModel(_fncprofilesEntity);
				accessProfile.Budget = new Models.FNC_AccessProfileModel(Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(fncIds /*new List<int> { _mainProfileEntites.Id }*/));
				#endregion

				#region Work Plan
				//var _wplUserEntities = Infrastructure.Data.Access.Tables.WPL.UserAccess.GetByUerId(id);
				//var _wplProfilesEntity = Infrastructure.Data.Access.Tables.WPL.AccessProfileAccess.Get(_wplUserEntities?.Select(x => x.AccessProfileId).ToList());
				//accessProfile.SalesDistribution.WorkPlan = new Models.WorkPlan(_purchaseprofilesEntity);
				var _wplEntities = Infrastructure.Data.Access.Tables.WPL.AccessProfileAccess.GetByMainAccessProfilesIds(new List<int> { _mainProfileEntites.Id });
				accessProfile.WorkPlan = new Psz.Core.Identity.Models.WorkPlanAccessModel(_wplEntities);
				accessProfile.SalesDistribution = new Models.SalesDistributionAccessModel();
				accessProfile.SalesDistribution.ModuleActivated = accessProfile.WorkPlan.ModuleActivated;
				#endregion

				#region EDI
				//var _purchaseuserentities = Infrastructure.Data.Access.Tables.PRS.AccessProfileUsersAccess.GetByUersId(id);
				//var _purchaseprofilesEntity = Infrastructure.Data.Access.Tables.PRS.AccessProfileAccess.Get(_purchaseuserentities?.Select(x => (int)x.AccessProfileId).ToList());
				ctsIds.Add(_mainCSVProfileEntites.Id);
				var _purchaseprofilesEntity = Infrastructure.Data.Access.Tables.PRS.AccessProfileAccess.GetByMainAccessProfilesIds(ctsIds);
				accessProfile.Purchase = new Psz.Core.Identity.Models.PurchaseAccessModel(_purchaseprofilesEntity);
				#endregion

				#region Settings
				//var _customerServiceuserentities = Infrastructure.Data.Access.Tables.CSV.AccessProfileAccess.GetByUersId(id);
				//var _customerServiceprofilesEntity = Infrastructure.Data.Access.Tables.CSV.AccessProfileAccess.Get(_customerServiceuserentities?.Select(x => x.AccessProfileId).ToList());
				//var _customerServiceEntities = Infrastructure.Data.Access.Tables.CSV.AccessProfileAccess.GetByMainAccessProfilesIds(_customerServiceuserentities?.Select(x => x.AccessProfileId).ToList());
				var _stgEntities = Infrastructure.Data.Access.Tables.STG.AccessProfileAccess.GetByMainAccessProfilesIds(new List<int> { _mainProfileEntites.Id });
				accessProfile.Settings = new Psz.Core.Identity.Models.SettingsAccessModel(_stgEntities);
				#endregion

				#region Master Data
				var _masterDatauserentities = Infrastructure.Data.Access.Tables.BSD.AccessProfileUsersAccess.GetByUersId(id);
				var _masterDataprofilesEntity = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get(_masterDatauserentities?.Select(x => x.AccessProfileId).ToList());
				//
				var bsdLagerEntity = Enum.GetValues(typeof(Enums.BaseDataEmuns.BaseDataLager)).Cast<Enums.BaseDataEmuns.BaseDataLager>().ToList();
				var Lagers = bsdLagerEntity?
							.Select(x => new KeyValuePair<int, string>((int)x, $"{x.GetDescription()}".Trim())).Distinct().ToList();
				//
				if(_masterDataprofilesEntity != null)
				{
					foreach(var item in _masterDataprofilesEntity)
					{
						var lagers = Infrastructure.Data.Access.Tables.BSD.BSD_AccessProfileLagerAccess.GetByAccessProfileIds(new List<int> { item.Id });
						List<KeyValuePair<int, string>> profileLagers = new List<KeyValuePair<int, string>>();
						foreach(var l in lagers)
						{
							profileLagers.Add(new KeyValuePair<int, string>(
								(int)l.LagerId,
								Lagers.FirstOrDefault(x => x.Key == l.LagerId).Value
								));
						}
						item.LagerIds = profileLagers;
					}
				}
				//var _masterDataEntities = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.GetByMainAccessProfilesIds(_masterDatauserentities?.Select(x => x.AccessProfileId).ToList());
				accessProfile.MasterData = new Psz.Core.Identity.Models.MasterDataAccessModel(_masterDataprofilesEntity);
				#endregion

				#region Material Manamgement
				var _mtmUserEntities = Infrastructure.Data.Access.Tables.MTM.AccessProfileUsersAccess.GetByUserId(id);
				var _mtmEntities = Infrastructure.Data.Access.Tables.MTM.AccessProfileAccess.Get(_mtmUserEntities?.Select(x => x.AccessProfileId).ToList());
				accessProfile.MaterialManagement = new Psz.Core.Identity.Models.MaterialManagementAccessModel(_mtmEntities);
				#endregion

				#region Management Overview
				var _mgoUserEntities = Infrastructure.Data.Access.Tables.MGO.AccessProfileUsersAccess.GetByUserId(id);
				var _mgoEntities = Infrastructure.Data.Access.Tables.MGO.AccessProfileAccess.Get(_mgoUserEntities?.Select(x => x.AccessProfileId ?? -1).ToList());
				accessProfile.ManagementOverview = new Psz.Core.Identity.Models.ManagementOverviewAccessModel(_mgoEntities);
				#endregion Management Overview

				#region Capital Requests
				var _capitalRequestUserEntities = Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.GetByUersId(id);
				var cplIds = _capitalRequestUserEntities?.Select(x => (int)x.AccessProfileId).ToList() ?? new List<int>();
				var _capitalRequestsProfilesEntity = Infrastructure.Data.Access.Tables.CPL.AccessProfileAccess.Get(cplIds);
				accessProfile.CapitalRequests = new Models.CapitalRequestsAccessModel(_capitalRequestsProfilesEntity);
				#endregion

				#region CRP
				var _crpUserEntities = Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.GetByUersId(id);
				var _crpEntities = Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Get(_crpUserEntities.Select(x => x.AccessProfileId ?? -1).ToList());
				accessProfile.CRP = new Models.CRPAccessModel(_crpEntities);
				#endregion

				return accessProfile;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static Common.Models.ResponseModel<List<Models.AccessProfileModel>> Get(Models.UserModel user)
		{
			try
			{
				if(user == null
					/*|| !user.Access.Settings.AccessProfiles*/)
				{
					throw new SharedKernel.Exceptions.UnauthorizedException();
				}

				return Common.Models.ResponseModel<List<Models.AccessProfileModel>>.SuccessResponse(Get());
			} catch(Exception e)
			{
				Logger.Log(e);
				throw;
			}
		}
		public static Common.Models.ResponseModel<List<Models.AccessProfileMinimalModel>> GetMinimal(Models.UserModel user)
		{
			try
			{
				if(user == null
					/*|| !user.Access.Settings.AccessProfiles*/)
				{
					throw new SharedKernel.Exceptions.UnauthorizedException();
				}

				return Common.Models.ResponseModel<List<Models.AccessProfileMinimalModel>>.SuccessResponse(Get()?.Select(x => new Models.AccessProfileMinimalModel(x))?.ToList());
			} catch(Exception e)
			{
				Logger.Log(e);
				throw;
			}
		}
		public static Common.Models.ResponseModel<List<Models.AccessProfileModel>> GetBudget(Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null
					|| (!user.Access.Settings.AccessProfiles && !user.Access.Budget.AdministrationAccessProfiles))
				{
					throw new SharedKernel.Exceptions.UnauthorizedException();
				}

				return Common.Models.ResponseModel<List<Models.AccessProfileModel>>.SuccessResponse(Get());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.AccessProfileModel> Get()
		{
			try
			{
				return Get(Infrastructure.Data.Access.Tables.AccessProfileAccess.Get());
			} catch(Exception e)
			{
				Logger.Log(e);
				throw;
			}
		}
		public static Common.Models.ResponseModel<List<Models.AccessProfileModel>> Get(List<int> ids, Models.UserModel user)
		{
			try
			{
				if(user == null
					|| !user.Access.Settings.AccessProfiles)
				{
					throw new SharedKernel.Exceptions.UnauthorizedException();
				}

				return Common.Models.ResponseModel<List<Models.AccessProfileModel>>.SuccessResponse(Get(ids));
			} catch(Exception e)
			{
				Logger.Log(e);
				throw;
			}
		}
		public static List<Models.AccessProfileModel> Get(List<int> ids)
		{
			try
			{
				return Get(Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(ids));
			} catch(Exception e)
			{
				Logger.Log(e);
				throw;
			}
		}
		public static List<Models.AccessProfileModel> GetByUserId(int id)
		{
			try
			{
				return GetByUserId(id);
			} catch(Exception e)
			{
				Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.AccessProfileModel> Get(List<Infrastructure.Data.Entities.Tables.AccessProfileEntity> accessProfilesDb)
		{
			try
			{
				if(accessProfilesDb == null || accessProfilesDb.Count == 0)
				{
					return new List<Models.AccessProfileModel>();
				}

				var usersIds = accessProfilesDb.Select(e => e.CreationUserId).ToList();
				var usersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(usersIds);

				var accessProfilesIds = accessProfilesDb.Select(e => e.Id).ToList();

				var settingsAccessProfilesDb = Infrastructure.Data.Access.Tables.STG.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);
				var ediAccessProfilesDb = Infrastructure.Data.Access.Tables.PRS.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);
				var workPlanAccessProfilesDb = Infrastructure.Data.Access.Tables.WPL.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);

				var financeControlEntities = Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);
				var customerServiceEntities = Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);
				var masterDataEntities = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);
				var materialManagementEntities = Infrastructure.Data.Access.Tables.MTM.AccessProfileAccess.Get();


				var response = new List<Models.AccessProfileModel>();

				foreach(var accessProfileDb in accessProfilesDb)
				{
					var accessProfile = new Models.AccessProfileModel()
					{
						Id = accessProfileDb.Id,
						Name = accessProfileDb.Name,

						CreationUser = usersDb.Find(e => e.Id == accessProfileDb.CreationUserId)?.Username,

						Settings = new Models.SettingsAccessModel(),
						WorkPlan = new Models.WorkPlanAccessModel(),
						Purchase = new Models.PurchaseAccessModel(),

						Budget = new Models.FNC_AccessProfileModel(),
						Financial = new Psz.Core.Identity.Models.FinancialAccessModel(),
						Administration = new Psz.Core.Identity.Models.AdministrationAccessModel(),
						CustomerService = new Psz.Core.Identity.Models.CustomerServiceAccessModel(),
						HumanResources = new Psz.Core.Identity.Models.HumanResourcesAccessModel(),
						MaterialManagement = new Psz.Core.Identity.Models.MaterialManagementAccessModel(),
						MasterData = new Psz.Core.Identity.Models.MasterDataAccessModel(),
						Logistics = new Psz.Core.Identity.Models.LogisticsAccessModel(),
						SalesDistribution = new Psz.Core.Identity.Models.SalesDistributionAccessModel()
					};

					//FIXME: ---
					if(accessProfileDb.Id == 1)
					{
						accessProfile.SalesDistribution.ModuleActivated = true;
					}

					var settingsAccessProfileDb = settingsAccessProfilesDb.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
					if(settingsAccessProfileDb != null)
					{
						accessProfile.Settings.ModuleActivated = settingsAccessProfileDb.ModuleActivated;

						accessProfile.Settings.AccessProfiles = settingsAccessProfileDb.AccessProfiles;
						accessProfile.Settings.AccessProfilesCreate = settingsAccessProfileDb.AccessProfilesCreate;
						accessProfile.Settings.AccessProfilesDelete = settingsAccessProfileDb.AccessProfilesDelete;
						accessProfile.Settings.AccessProfilesUpdate = settingsAccessProfileDb.AccessProfilesUpdate;

						accessProfile.Settings.Users = settingsAccessProfileDb.Users;
						accessProfile.Settings.UsersCreate = settingsAccessProfileDb.UsersCreate;
						accessProfile.Settings.UsersDelete = settingsAccessProfileDb.UsersDelete;
						accessProfile.Settings.UsersUpdate = settingsAccessProfileDb.UsersUpdate;

						accessProfile.Settings.SuperAdministrator = settingsAccessProfileDb.SuperAdministrator;
					}

					var BudgetAccessProfileDb = financeControlEntities.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
					if(BudgetAccessProfileDb != null)
					{
						accessProfile.Budget = new Models.FNC_AccessProfileModel(accessProfileDb.Id, BudgetAccessProfileDb);
					}


					var workPlanAccessProfileDb = workPlanAccessProfilesDb.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
					if(workPlanAccessProfileDb != null)
					{
						accessProfile.WorkPlan.ModuleActivated = workPlanAccessProfileDb.ModuleActivated;

						accessProfile.WorkPlan.Access = workPlanAccessProfileDb.Access;
						accessProfile.WorkPlan.AccessUpdate = workPlanAccessProfileDb.AccessUpdate;

						accessProfile.WorkPlan.Country = workPlanAccessProfileDb.Country;
						accessProfile.WorkPlan.CountryCreate = workPlanAccessProfileDb.CountryCreate;
						accessProfile.WorkPlan.CountryDelete = workPlanAccessProfileDb.CountryDelete;
						accessProfile.WorkPlan.CountryUpdate = workPlanAccessProfileDb.CountryUpdate;

						accessProfile.WorkPlan.Departement = workPlanAccessProfileDb.Departement;
						accessProfile.WorkPlan.DepartementCreate = workPlanAccessProfileDb.DepartementCreate;
						accessProfile.WorkPlan.DepartementDelete = workPlanAccessProfileDb.DepartementDelete;
						accessProfile.WorkPlan.DepartementUpdate = workPlanAccessProfileDb.DepartementUpdate;

						accessProfile.WorkPlan.Hall = workPlanAccessProfileDb.Hall;
						accessProfile.WorkPlan.HallCreate = workPlanAccessProfileDb.HallCreate;
						accessProfile.WorkPlan.HallDelete = workPlanAccessProfileDb.HallDelete;
						accessProfile.WorkPlan.HallUpdate = workPlanAccessProfileDb.HallUpdate;

						accessProfile.WorkPlan.StandardOperation = workPlanAccessProfileDb.StandardOperation;
						accessProfile.WorkPlan.StandardOperationCreate = workPlanAccessProfileDb.StandardOperationCreate;
						accessProfile.WorkPlan.StandardOperationDelete = workPlanAccessProfileDb.StandardOperationDelete;
						accessProfile.WorkPlan.StandardOperationUpdate = workPlanAccessProfileDb.StandardOperationUpdate;

						accessProfile.WorkPlan.WorkArea = workPlanAccessProfileDb.WorkArea;
						accessProfile.WorkPlan.WorkAreaCreate = workPlanAccessProfileDb.WorkAreaCreate;
						accessProfile.WorkPlan.WorkAreaDelete = workPlanAccessProfileDb.WorkAreaDelete;
						accessProfile.WorkPlan.WorkAreaUpdate = workPlanAccessProfileDb.WorkAreaUpdate;

						accessProfile.WorkPlan.WorkPlan = workPlanAccessProfileDb.WorkPlan;
						accessProfile.WorkPlan.WorkPlanCreate = workPlanAccessProfileDb.WorkPlanCreate;
						accessProfile.WorkPlan.WorkPlanDelete = workPlanAccessProfileDb.WorkPlanDelete;
						accessProfile.WorkPlan.WorkPlanUpdate = workPlanAccessProfileDb.WorkPlanUpdate;

						accessProfile.WorkPlan.WorkPlanReporting = workPlanAccessProfileDb.WorkPlanReporting;
						accessProfile.WorkPlan.WorkPlanReportingCreate = workPlanAccessProfileDb.WorkPlanReportingCreate;
						accessProfile.WorkPlan.WorkPlanReportingDelete = workPlanAccessProfileDb.WorkPlanReportingDelete;
						accessProfile.WorkPlan.WorkPlanReportingUpdate = workPlanAccessProfileDb.WorkPlanReportingUpdate;

						accessProfile.WorkPlan.WorkStation = workPlanAccessProfileDb.WorkStation;
						accessProfile.WorkPlan.WorkStationCreate = workPlanAccessProfileDb.WorkStationCreate;
						accessProfile.WorkPlan.WorkStationDelete = workPlanAccessProfileDb.WorkStationDelete;
						accessProfile.WorkPlan.WorkStationUpdate = workPlanAccessProfileDb.WorkStationUpdate;


						accessProfile.WorkPlan.AdministrationUser = workPlanAccessProfileDb.AdminstrationUser;
						accessProfile.WorkPlan.AdministrationUserUpdate = workPlanAccessProfileDb.AdminstrationUserUpdate;
						accessProfile.WorkPlan.AdministrationAccessProfiles = workPlanAccessProfileDb.AdministrationAccessProfiles;
						accessProfile.WorkPlan.AdministrationAccessProfilesUpdate = workPlanAccessProfileDb.AdministrationAccessProfilesUpdate;

						accessProfile.WorkPlan.SuperAdministrator = workPlanAccessProfileDb.SuperAdministrator;
						accessProfile.WorkPlan.isDefault = workPlanAccessProfileDb.isDefault ?? false;
						{
							accessProfile.SalesDistribution.ModuleActivated = workPlanAccessProfileDb.ModuleActivated;
						}
					}

					var purchaseAccessProfileDb = ediAccessProfilesDb.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
					if(purchaseAccessProfileDb != null)
					{
						accessProfile.Purchase.ModuleActivated = purchaseAccessProfileDb.ModuleActivated;

						accessProfile.Purchase.Access = purchaseAccessProfileDb.Access;
						accessProfile.Purchase.AccessUpdate = purchaseAccessProfileDb.AccessUpdate;

						accessProfile.Purchase.Customer = purchaseAccessProfileDb.Customer;
						accessProfile.Purchase.CustomerUpdate = purchaseAccessProfileDb.CustomerUpdate;

						accessProfile.Purchase.Order = purchaseAccessProfileDb.Order;
						accessProfile.Purchase.OrderUpdate = purchaseAccessProfileDb.OrderUpdate;
						accessProfile.Purchase.OrderValidate = purchaseAccessProfileDb.OrderValidate;
						accessProfile.Purchase.OrderHistory = purchaseAccessProfileDb.OrderHistory;

						accessProfile.Purchase.OrderError = purchaseAccessProfileDb.OrderError;
						accessProfile.Purchase.OrderErrorValidate = purchaseAccessProfileDb.OrderErrorValidate;
						accessProfile.Purchase.OrderErrorHistory = purchaseAccessProfileDb.OrderErrorHistory;

						accessProfile.Purchase.EDI = purchaseAccessProfileDb.EDI;
						accessProfile.Purchase.AllCustomers = purchaseAccessProfileDb.AllCustomers;

						accessProfile.Purchase.SuperAdministrator = purchaseAccessProfileDb.SuperAdministrator;
					}


					var stgAccessProfileDb = settingsAccessProfilesDb.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
					if(stgAccessProfileDb != null)
					{
						accessProfile.Administration = new Psz.Core.Identity.Models.AdministrationAccessModel(stgAccessProfileDb);
					}

					var fncAccessProfileDb = financeControlEntities.FindAll(e => e.MainAccessProfileId == accessProfileDb.Id);
					if(fncAccessProfileDb != null)
					{
						accessProfile.Financial = new Psz.Core.Identity.Models.FinancialAccessModel(fncAccessProfileDb);
					}

					//FIXME: 
					//var bsdAccessProfileDb = masterDataEntities.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
					//if (bsdAccessProfileDb != null)
					//{
					//    accessProfile.MasterData = new Psz.Core.Identity.Models.MasterDataAccessModel(bsdAccessProfileDb);
					//}

					// -
					//var csvAccessProfileDb = customerServiceEntities.FindAll(e => e.MainAccessProfileId == accessProfileDb.Id);
					var csvAccessProfileDb = customerServiceEntities.FindAll(e => e.Id == accessProfileDb.Id);

					if(csvAccessProfileDb != null)
					{
						accessProfile.CustomerService = new Psz.Core.Identity.Models.CustomerServiceAccessModel(csvAccessProfileDb);
					}

					// - 
					accessProfile.MaterialManagement = new Psz.Core.Identity.Models.MaterialManagementAccessModel(/*materialManagementEntities.FindAll(x=> x.MainAccessProfileId == accessProfileDb.Id)*/);

					response.Add(accessProfile);
				}

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		// - 2025-03-31 - same access as a given user
		public static int InitProfilesAsUser(int userId, int destUserId, int editUserId,
			Infrastructure.Services.Utils.TransactionsManager botransaction,
			Infrastructure.Services.Utils.TransactionsManager botransactionFNC,
			Infrastructure.Services.Utils.TransactionsManager botransactionMTM)
		{

			try
			{
				var usersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.GetWithTransaction(userId, botransaction.connection, botransaction.transaction);
				var destUserDb = Infrastructure.Data.Access.Tables.COR.UserAccess.GetWithTransaction(destUserId, botransaction.connection, botransaction.transaction);
				if(usersDb == null || destUserDb == null)
				{
					return 0;
				}

				// -
				Infrastructure.Data.Access.Joins.AccessProfileUserAccess.ReplaceForUser(userId, destUserDb.Id, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Joins.AccessProfileUserAccess.ReplaceForUserFNC(userId, destUserDb.Id, destUserDb.Username, destUserDb.Email, botransactionFNC.connection, botransactionFNC.transaction);
				Infrastructure.Data.Access.Joins.AccessProfileUserAccess.ReplaceForUserMTM(userId, destUserDb.Id, destUserDb.Username, destUserDb.Email, botransactionMTM.connection, botransactionMTM.transaction);

				// -
				Infrastructure.Data.Access.Tables.COR.UserAccess.UpdateAccessProfileId(userId, usersDb.AccessProfileId, editUserId, botransaction.connection, botransaction.transaction);

				// -
					return destUserDb.AccessProfileId;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}