using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class AccessProfiles
	{
		public static Core.Models.ResponseModel<Core.Identity.Models.AccessProfileModel> Get(int id, Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null
					|| !user.Access.Settings.AccessProfiles)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var accessProfile = Get(id);
				if(accessProfile == null)
				{
					throw new Core.Exceptions.NotFoundException();
				}

				return Core.Models.ResponseModel<Core.Identity.Models.AccessProfileModel>.SuccessResponse(accessProfile);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static Core.Identity.Models.AccessProfileModel Get(int id)
		{
			try
			{
				return Core.Identity.Handlers.AccessProfile.Get(new List<int>() { id }).FirstOrDefault();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		//public static Core.Models.ResponseModel<List<Core.Identity.Models.AccessProfileModel>> Get(Core.Identity.Models.UserModel user)
		//{
		//    try
		//    {
		//        if (user == null
		//            || (!user.Access.Settings.AccessProfiles && !user.Access.WorkPlan.AdministrationAccessProfiles && !user.Access.Budget.AdministrationAccessProfiles))
		//        {
		//            throw new Core.Exceptions.UnauthorizedException();
		//        }

		//        return Core.Models.ResponseModel<List<Core.Identity.Models.AccessProfileModel>>.SuccessResponse(Get());
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}
		//public static Core.Models.ResponseModel<List<Models.AccessProfiles.AccessProfileModel>> GetBudget(Core.Identity.Models.UserModel user)
		//{
		//    try
		//    {
		//        if (user == null
		//            || (!user.Access.Settings.AccessProfiles && !user.Access.Budget.AdministrationAccessProfiles))
		//        {
		//            throw new Core.Exceptions.UnauthorizedException();
		//        }

		//        return Core.Models.ResponseModel<List<Core.Apps.Settings.Models.AccessProfiles.AccessProfileModel>>.SuccessResponse(Get());
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}
		//internal static List<Core.Identity.Models.AccessProfileModel> Get()
		//{
		//    try
		//    {
		//        return Get(Infrastructure.Data.Access.Tables.AccessProfileAccess.Get());
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}

		//public static Core.Models.ResponseModel<List<Models.AccessProfiles.AccessProfileModel>> Get(List<int> ids, Core.Identity.Models.UserModel user)
		//{
		//    try
		//    {
		//        if (user == null
		//            || (!user.Access.Settings.AccessProfiles && !user.Access.WorkPlan.AdministrationAccessProfiles))
		//        {
		//            throw new Core.Exceptions.UnauthorizedException();
		//        }

		//        return Core.Models.ResponseModel<List<Core.Apps.Settings.Models.AccessProfiles.AccessProfileModel>>.SuccessResponse(Get(ids));
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}
		//internal static List<Models.AccessProfiles.AccessProfileModel> Get(List<int> ids)
		//{
		//    try
		//    {
		//        return Get(Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(ids));
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}

		//internal static List<Core.Identity.Models.AccessProfileModel> Get(List<Infrastructure.Data.Entities.Tables.AccessProfileEntity> accessProfilesDb)
		//{
		//    try
		//    {
		//        //if (accessProfilesDb == null || accessProfilesDb.Count == 0)
		//        //{
		//        //    return new List<Models.AccessProfiles.AccessProfileModel>();
		//        //}

		//        //var usersIds = accessProfilesDb.Select(e => e.CreationUserId).ToList();
		//        //var usersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(usersIds);

		//        //var accessProfilesIds = accessProfilesDb.Select(e => e.Id).ToList();

		//        //var settingsAccessProfilesDb = Infrastructure.Data.Access.Tables.STG.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);
		//        //var ediAccessProfilesDb = Infrastructure.Data.Access.Tables.PRS.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);
		//        //var workPlanAccessProfilesDb = Infrastructure.Data.Access.Tables.WPL.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);

		//        //var financeControlEntities = Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);
		//        //var customerServiceEntities = Infrastructure.Data.Access.Tables.CSV.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);
		//        //var masterDataEntities = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.GetByMainAccessProfilesIds(accessProfilesIds);


		//        //var response = new List<Models.AccessProfiles.AccessProfileModel>();

		//        //foreach (var accessProfileDb in accessProfilesDb)
		//        //{
		//        //    var accessProfile = new Models.AccessProfiles.AccessProfileModel()
		//        //    {
		//        //        Id = accessProfileDb.Id,
		//        //        Name = accessProfileDb.Name,

		//        //        CreationUser = usersDb.Find(e => e.Id == accessProfileDb.CreationUserId)?.Username,

		//        //        Settings = new Models.AccessProfiles.SettingsAccessModel(),
		//        //        WorkPlan = new Models.AccessProfiles.WorkPlanAccessModel(),
		//        //        Purchase = new Models.AccessProfiles.PurchaseAccessModel(),

		//        //        Budget = new Models.AccessProfiles.BudgetAccessModel(),
		//        //        Financal = new Psz.Core.Identity.Models.FinancalAccessModel(),
		//        //        Administration = new Psz.Core.Identity.Models.AdministrationAccessModel(),
		//        //        CustomerService = new Psz.Core.Identity.Models.CustomerServiceAccessModel(),
		//        //        HumanResources = new Psz.Core.Identity.Models.HumanResourcesAccessModel(),
		//        //        MaterialManagement = new Psz.Core.Identity.Models.MaterialManagementAccessModel(),
		//        //        MasterData = new Psz.Core.Identity.Models.MasterDataAccessModel(),
		//        //        Logistics = new Psz.Core.Identity.Models.LogisticsAccessModel(),
		//        //        SalesDistribution = new Psz.Core.Identity.Models.SalesDistributionAccessModel()
		//        //    };

		//        //    var settingsAccessProfileDb = settingsAccessProfilesDb.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
		//        //    if (settingsAccessProfileDb != null)
		//        //    {
		//        //        accessProfile.Settings.ModuleActivated = settingsAccessProfileDb.ModuleActivated;

		//        //        accessProfile.Settings.AccessProfiles = settingsAccessProfileDb.AccessProfiles;
		//        //        accessProfile.Settings.AccessProfilesCreate = settingsAccessProfileDb.AccessProfilesCreate;
		//        //        accessProfile.Settings.AccessProfilesDelete = settingsAccessProfileDb.AccessProfilesDelete;
		//        //        accessProfile.Settings.AccessProfilesUpdate = settingsAccessProfileDb.AccessProfilesUpdate;

		//        //        accessProfile.Settings.Users = settingsAccessProfileDb.Users;
		//        //        accessProfile.Settings.UsersCreate = settingsAccessProfileDb.UsersCreate;
		//        //        accessProfile.Settings.UsersDelete = settingsAccessProfileDb.UsersDelete;
		//        //        accessProfile.Settings.UsersUpdate = settingsAccessProfileDb.UsersUpdate;

		//        //        accessProfile.Settings.SuperAdministrator = settingsAccessProfileDb.SuperAdministrator;
		//        //    }

		//        //    var BudgetAccessProfileDb = financeControlEntities.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
		//        //    if (BudgetAccessProfileDb != null)
		//        //    {
		//        //        accessProfile.Budget = new Models.AccessProfiles.BudgetAccessModel(accessProfileDb.Id, BudgetAccessProfileDb);
		//        //        //accessProfile.Budget.ModuleActivated = BudgetAccessProfileDb.ModuleActivated ?? false;

		//        //        //accessProfile.Budget.Config = BudgetAccessProfileDb.Config ?? false;
		//        //        //accessProfile.Budget.Units = BudgetAccessProfileDb.Units ?? false;
		//        //        //accessProfile.Budget.ConfigCreateLand = BudgetAccessProfileDb.ConfigCreateLand ?? false;
		//        //        //accessProfile.Budget.ConfigEditLand = BudgetAccessProfileDb.ConfigEditLand ?? false;
		//        //        //accessProfile.Budget.ConfigDeleteLand = BudgetAccessProfileDb.ConfigDeleteLand ?? false;
		//        //        //accessProfile.Budget.ConfigCreateDept = BudgetAccessProfileDb.ConfigCreateDept ?? false;
		//        //        //accessProfile.Budget.ConfigEditDept = BudgetAccessProfileDb.ConfigEditDept ?? false;
		//        //        //accessProfile.Budget.ConfigDeleteDept = BudgetAccessProfileDb.ConfigDeleteDept ?? false;
		//        //        //accessProfile.Budget.Suppliers = BudgetAccessProfileDb.Suppliers ?? false;
		//        //        //accessProfile.Budget.ConfigCreateSupplier = BudgetAccessProfileDb.ConfigCreateSupplier ?? false;
		//        //        //accessProfile.Budget.ConfigEditSupplier = BudgetAccessProfileDb.ConfigEditSupplier ?? false;
		//        //        //accessProfile.Budget.ConfigDeleteSupplier = BudgetAccessProfileDb.ConfigDeleteSupplier ?? false;
		//        //        //accessProfile.Budget.Article = BudgetAccessProfileDb.Article ?? false;
		//        //        //accessProfile.Budget.ConfigCreateArtikel = BudgetAccessProfileDb.ConfigCreateArtikel ?? false;
		//        //        //accessProfile.Budget.ConfigEditArtikel = BudgetAccessProfileDb.ConfigEditArtikel ?? false;
		//        //        //accessProfile.Budget.ConfigDeleteArtikel = BudgetAccessProfileDb.ConfigDeleteArtikel ?? false;

		//        //        //accessProfile.Budget.Assign = BudgetAccessProfileDb.Assign ?? false;
		//        //        //accessProfile.Budget.AssignViewLand = BudgetAccessProfileDb.AssignViewLand ?? false;
		//        //        //accessProfile.Budget.AssignCreateLand = BudgetAccessProfileDb.AssignCreateLand ?? false;
		//        //        //accessProfile.Budget.AssignEditLand = BudgetAccessProfileDb.AssignEditLand ?? false;
		//        //        //accessProfile.Budget.AssignDeleteLand = BudgetAccessProfileDb.AssignDeleteLand ?? false;
		//        //        //accessProfile.Budget.AssignViewDept = BudgetAccessProfileDb.AssignViewDept ?? false;
		//        //        //accessProfile.Budget.AssignCreateDept = BudgetAccessProfileDb.AssignCreateDept ?? false;
		//        //        //accessProfile.Budget.AssignEditDept = BudgetAccessProfileDb.AssignEditDept ?? false;
		//        //        //accessProfile.Budget.AssignDeleteDept = BudgetAccessProfileDb.AssignDeleteDept ?? false;
		//        //        //accessProfile.Budget.AssignViewUser = BudgetAccessProfileDb.AssignViewUser ?? false;
		//        //        //accessProfile.Budget.AssignCreateUser = BudgetAccessProfileDb.AssignCreateUser ?? false;
		//        //        //accessProfile.Budget.AssignEditUser = BudgetAccessProfileDb.AssignEditUser ?? false;
		//        //        //accessProfile.Budget.AssignDeleteUser = BudgetAccessProfileDb.AssignDeleteUser ?? false;

		//        //        //accessProfile.Budget.Administration = BudgetAccessProfileDb.Administration ?? false;
		//        //        //accessProfile.Budget.AdministrationUser = BudgetAccessProfileDb.AdministrationUser ?? false;
		//        //        //accessProfile.Budget.AdministrationUserUpdate = BudgetAccessProfileDb.AdministrationUserUpdate ?? false;
		//        //        //accessProfile.Budget.AdministrationAccessProfiles = BudgetAccessProfileDb.AdministrationAccessProfiles ?? false;
		//        //        //accessProfile.Budget.AdministrationAccessProfilesUpdate = BudgetAccessProfileDb.AdministrationAccessProfilesUpdate ?? false;

		//        //        //accessProfile.Budget.Project = BudgetAccessProfileDb.Project ?? false;
		//        //        //accessProfile.Budget.ViewExternalProject = BudgetAccessProfileDb.ViewExternalProject ?? false;
		//        //        //accessProfile.Budget.AddExternalProject = BudgetAccessProfileDb.AddExternalProject ?? false;
		//        //        //accessProfile.Budget.UpdateExternalProject = BudgetAccessProfileDb.UpdateExternalProject ?? false;
		//        //        //accessProfile.Budget.DeleteExternalProject = BudgetAccessProfileDb.DeleteExternalProject ?? false;
		//        //        //accessProfile.Budget.ViewInternalProject = BudgetAccessProfileDb.ViewInternalProject ?? false;
		//        //        //accessProfile.Budget.AddInternalProject = BudgetAccessProfileDb.AddInternalProject ?? false;
		//        //        //accessProfile.Budget.UpdateInternalProject = BudgetAccessProfileDb.UpdateInternalProject ?? false;
		//        //        //accessProfile.Budget.DeleteInternalProject = BudgetAccessProfileDb.DeleteInternalProject ?? false;

		//        //        //accessProfile.Budget.Commande = BudgetAccessProfileDb.Commande ?? false;
		//        //        //accessProfile.Budget.ViewExternalCommande = BudgetAccessProfileDb.ViewExternalCommande ?? false;
		//        //        //accessProfile.Budget.AddExternalCommande = BudgetAccessProfileDb.AddExternalCommande ?? false;
		//        //        //accessProfile.Budget.UpdateExternalCommande = BudgetAccessProfileDb.UpdateExternalCommande ?? false;
		//        //        //accessProfile.Budget.DeleteExternalCommande = BudgetAccessProfileDb.DeleteExternalCommande ?? false;
		//        //        //accessProfile.Budget.ViewInternalCommande = BudgetAccessProfileDb.ViewInternalCommande ?? false;
		//        //        //accessProfile.Budget.AddInternalCommande = BudgetAccessProfileDb.AddInternalCommande ?? false;
		//        //        //accessProfile.Budget.UpdateInternalCommande = BudgetAccessProfileDb.UpdateInternalCommande ?? false;
		//        //        //accessProfile.Budget.DeleteInternalCommande = BudgetAccessProfileDb.DeleteInternalCommande ?? false;

		//        //        //accessProfile.Budget.Receptions = BudgetAccessProfileDb.Receptions ?? false;
		//        //        //accessProfile.Budget.ReceptionsEdit = BudgetAccessProfileDb.ReceptionsEdit ?? false;
		//        //        //accessProfile.Budget.ReceptionsView = BudgetAccessProfileDb.ReceptionsView ?? false;
		//        //    }


		//        //    var workPlanAccessProfileDb = workPlanAccessProfilesDb.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
		//        //    if (workPlanAccessProfileDb != null)
		//        //    {
		//        //        accessProfile.WorkPlan.ModuleActivated = workPlanAccessProfileDb.ModuleActivated;

		//        //        accessProfile.WorkPlan.Access = workPlanAccessProfileDb.Access;
		//        //        accessProfile.WorkPlan.AccessUpdate = workPlanAccessProfileDb.AccessUpdate;

		//        //        accessProfile.WorkPlan.Country = workPlanAccessProfileDb.Country;
		//        //        accessProfile.WorkPlan.CountryCreate = workPlanAccessProfileDb.CountryCreate;
		//        //        accessProfile.WorkPlan.CountryDelete = workPlanAccessProfileDb.CountryDelete;
		//        //        accessProfile.WorkPlan.CountryUpdate = workPlanAccessProfileDb.CountryUpdate;

		//        //        accessProfile.WorkPlan.Departement = workPlanAccessProfileDb.Departement;
		//        //        accessProfile.WorkPlan.DepartementCreate = workPlanAccessProfileDb.DepartementCreate;
		//        //        accessProfile.WorkPlan.DepartementDelete = workPlanAccessProfileDb.DepartementDelete;
		//        //        accessProfile.WorkPlan.DepartementUpdate = workPlanAccessProfileDb.DepartementUpdate;

		//        //        accessProfile.WorkPlan.Hall = workPlanAccessProfileDb.Hall;
		//        //        accessProfile.WorkPlan.HallCreate = workPlanAccessProfileDb.HallCreate;
		//        //        accessProfile.WorkPlan.HallDelete = workPlanAccessProfileDb.HallDelete;
		//        //        accessProfile.WorkPlan.HallUpdate = workPlanAccessProfileDb.HallUpdate;

		//        //        accessProfile.WorkPlan.StandardOperation = workPlanAccessProfileDb.StandardOperation;
		//        //        accessProfile.WorkPlan.StandardOperationCreate = workPlanAccessProfileDb.StandardOperationCreate;
		//        //        accessProfile.WorkPlan.StandardOperationDelete = workPlanAccessProfileDb.StandardOperationDelete;
		//        //        accessProfile.WorkPlan.StandardOperationUpdate = workPlanAccessProfileDb.StandardOperationUpdate;

		//        //        accessProfile.WorkPlan.WorkArea = workPlanAccessProfileDb.WorkArea;
		//        //        accessProfile.WorkPlan.WorkAreaCreate = workPlanAccessProfileDb.WorkAreaCreate;
		//        //        accessProfile.WorkPlan.WorkAreaDelete = workPlanAccessProfileDb.WorkAreaDelete;
		//        //        accessProfile.WorkPlan.WorkAreaUpdate = workPlanAccessProfileDb.WorkAreaUpdate;

		//        //        accessProfile.WorkPlan.WorkPlan = workPlanAccessProfileDb.WorkPlan;
		//        //        accessProfile.WorkPlan.WorkPlanCreate = workPlanAccessProfileDb.WorkPlanCreate;
		//        //        accessProfile.WorkPlan.WorkPlanDelete = workPlanAccessProfileDb.WorkPlanDelete;
		//        //        accessProfile.WorkPlan.WorkPlanUpdate = workPlanAccessProfileDb.WorkPlanUpdate;

		//        //        accessProfile.WorkPlan.WorkPlanReporting = workPlanAccessProfileDb.WorkPlanReporting;
		//        //        accessProfile.WorkPlan.WorkPlanReportingCreate = workPlanAccessProfileDb.WorkPlanReportingCreate;
		//        //        accessProfile.WorkPlan.WorkPlanReportingDelete = workPlanAccessProfileDb.WorkPlanReportingDelete;
		//        //        accessProfile.WorkPlan.WorkPlanReportingUpdate = workPlanAccessProfileDb.WorkPlanReportingUpdate;

		//        //        accessProfile.WorkPlan.WorkStation = workPlanAccessProfileDb.WorkStation;
		//        //        accessProfile.WorkPlan.WorkStationCreate = workPlanAccessProfileDb.WorkStationCreate;
		//        //        accessProfile.WorkPlan.WorkStationDelete = workPlanAccessProfileDb.WorkStationDelete;
		//        //        accessProfile.WorkPlan.WorkStationUpdate = workPlanAccessProfileDb.WorkStationUpdate;


		//        //        accessProfile.WorkPlan.AdministrationUser = workPlanAccessProfileDb.AdministrationUser;
		//        //        accessProfile.WorkPlan.AdministrationUserUpdate = workPlanAccessProfileDb.AdministrationUserUpdate;
		//        //        accessProfile.WorkPlan.AdministrationAccessProfiles = workPlanAccessProfileDb.AdministrationAccessProfiles;
		//        //        accessProfile.WorkPlan.AdministrationAccessProfilesUpdate = workPlanAccessProfileDb.AdministrationAccessProfilesUpdate;

		//        //        accessProfile.WorkPlan.SuperAdministrator = workPlanAccessProfileDb.SuperAdministrator;
		//        //    }

		//        //    var purchaseAccessProfileDb = ediAccessProfilesDb.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
		//        //    if (purchaseAccessProfileDb != null)
		//        //    {
		//        //        accessProfile.Purchase.ModuleActivated = purchaseAccessProfileDb.ModuleActivated;

		//        //        accessProfile.Purchase.Access = purchaseAccessProfileDb.Access;
		//        //        accessProfile.Purchase.AccessUpdate = purchaseAccessProfileDb.AccessUpdate;

		//        //        accessProfile.Purchase.Customer = purchaseAccessProfileDb.Customer;
		//        //        accessProfile.Purchase.CustomerUpdate = purchaseAccessProfileDb.CustomerUpdate;

		//        //        accessProfile.Purchase.Order = purchaseAccessProfileDb.Order;
		//        //        accessProfile.Purchase.OrderUpdate = purchaseAccessProfileDb.OrderUpdate;
		//        //        accessProfile.Purchase.OrderValidate = purchaseAccessProfileDb.OrderValidate;
		//        //        accessProfile.Purchase.OrderHistory = purchaseAccessProfileDb.OrderHistory;

		//        //        accessProfile.Purchase.OrderError = purchaseAccessProfileDb.OrderError;
		//        //        accessProfile.Purchase.OrderErrorValidate = purchaseAccessProfileDb.OrderErrorValidate;
		//        //        accessProfile.Purchase.OrderErrorHistory = purchaseAccessProfileDb.OrderErrorHistory;

		//        //        accessProfile.Purchase.EDI = purchaseAccessProfileDb.EDI;
		//        //        accessProfile.Purchase.AllCustomers = purchaseAccessProfileDb.AllCustomers;

		//        //        accessProfile.Purchase.SuperAdministrator = purchaseAccessProfileDb.SuperAdministrator;
		//        //    }


		//        //    var fncAccessProfileDb = financeControlEntities.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
		//        //    if (fncAccessProfileDb!= null)
		//        //    {
		//        //        accessProfile.Financal = new Psz.Core.Identity.Models.FinancalAccessModel(fncAccessProfileDb);
		//        //    }

		//        //    var bsdAccessProfileDb = masterDataEntities.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
		//        //    if (bsdAccessProfileDb != null)
		//        //    {
		//        //        accessProfile.MasterData = new Psz.Core.Identity.Models.MasterDataAccessModel(bsdAccessProfileDb);
		//        //    }

		//        //    var csvAccessProfileDb = customerServiceEntities.Find(e => e.MainAccessProfileId == accessProfileDb.Id);
		//        //    if (csvAccessProfileDb != null)
		//        //    {
		//        //        accessProfile.CustomerService = new Psz.Core.Identity.Models.CustomerServiceAccessModel(csvAccessProfileDb);
		//        //    }

		//        //    response.Add(accessProfile);
		//        //}

		//        //return response;

		//        return Core.Identity.Handlers.AccessProfile.Get()
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}
	}
}
