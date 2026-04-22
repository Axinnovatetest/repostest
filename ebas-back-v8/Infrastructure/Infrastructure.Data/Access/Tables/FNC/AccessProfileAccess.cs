using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class AccessProfileAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_AccessProfile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_AccessProfile] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_AccessProfile] ([AccessProfileName],[Accounting],[AddExternalCommande],[AddExternalProject],[AddInternalCommande],[AddInternalProject],[AddKontenrahmenAccounting],[Administration],[AdministrationAccessProfiles],[AdministrationAccessProfilesUpdate],[AdministrationUser],[AdministrationUserUpdate],[Article],[Assign],[AssignAllDepartments],[AssignAllEmployees],[AssignAllSites],[AssignCreateDept],[AssignCreateLand],[AssignCreateUser],[AssignDeleteDept],[AssignDeleteLand],[AssignDeleteUser],[AssignEditDept],[AssignEditLand],[AssignEditUser],[AssignViewDept],[AssignViewLand],[AssignViewUser],[Budget],[CashLiquidity],[Commande],[CommandeExternalEditAllGroup],[CommandeExternalEditAllSite],[CommandeExternalViewAllGroup],[CommandeExternalViewAllSite],[CommandeExternalViewInvoice],[CommandeExternalViewInvoiceAllGroup],[CommandeInternalEditAllDepartment],[CommandeInternalEditAllGroup],[CommandeInternalEditAllSite],[CommandeInternalViewAllDepartment],[CommandeInternalViewAllGroup],[CommandeInternalViewAllSite],[CommandeInternalViewInvoice],[CommandeInternalViewInvoiceAllGroup],[Config],[ConfigCreateArtikel],[ConfigCreateDept],[ConfigCreateLand],[ConfigCreateSupplier],[ConfigDeleteArtikel],[ConfigDeleteDept],[ConfigDeleteLand],[ConfigDeleteSupplier],[ConfigEditArtikel],[ConfigEditDept],[ConfigEditLand],[ConfigEditSupplier],[CreationTime],[CreationUserId],[CreditManagement],[DeleteExternalCommande],[DeleteExternalProject],[DeleteInternalCommande],[DeleteInternalProject],[DeleteKontenrahmenAccounting],[DeleteZahlungskonditionenKundenAccounting],[DeleteZahlungskonditionenLieferantenAccounting],[FinanceOrder],[FinanceProject],[IsDefault],[LastEditTime],[LastEditUserId],[MainAccessProfileId],[ModuleActivated],[Project],[ProjectExternalEditAllGroup],[ProjectExternalEditAllSite],[ProjectExternalViewAllGroup],[ProjectExternalViewAllSite],[ProjectInternalEditAllGroup],[ProjectInternalEditAllSite],[ProjectInternalViewAllGroup],[ProjectInternalViewAllSite],[Receptions],[ReceptionsEdit],[ReceptionsView],[Statistics],[StatisticsViewAll],[Suppliers],[Units],[UpdateExternalCommande],[UpdateExternalProject],[UpdateInternalCommande],[UpdateInternalProject],[UpdateKontenrahmenAccounting],[UpdateZahlungskonditionenKundenAccounting],[UpdateZahlungskonditionenLieferantenAccounting],[ViewAusfuhrAccounting],[ViewEinfuhrAccounting],[ViewExternalCommande],[ViewExternalProject],[ViewFactoringRgGutschriftlistAccounting],[ViewInternalCommande],[ViewInternalProject],[ViewKontenrahmenAccounting],[ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting],[ViewLiquiditatsplanungSkontozahlerAccounting],[ViewRechnungsTransferAccounting],[ViewRMDCZAccounting],[ViewStammdatenkontrolleWareneingangeAccounting],[ViewZahlungskonditionenKundenAccounting],[ViewZahlungskonditionenLieferantenAccounting]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileName,@Accounting,@AddExternalCommande,@AddExternalProject,@AddInternalCommande,@AddInternalProject,@AddKontenrahmenAccounting,@Administration,@AdministrationAccessProfiles,@AdministrationAccessProfilesUpdate,@AdministrationUser,@AdministrationUserUpdate,@Article,@Assign,@AssignAllDepartments,@AssignAllEmployees,@AssignAllSites,@AssignCreateDept,@AssignCreateLand,@AssignCreateUser,@AssignDeleteDept,@AssignDeleteLand,@AssignDeleteUser,@AssignEditDept,@AssignEditLand,@AssignEditUser,@AssignViewDept,@AssignViewLand,@AssignViewUser,@Budget,@CashLiquidity,@Commande,@CommandeExternalEditAllGroup,@CommandeExternalEditAllSite,@CommandeExternalViewAllGroup,@CommandeExternalViewAllSite,@CommandeExternalViewInvoice,@CommandeExternalViewInvoiceAllGroup,@CommandeInternalEditAllDepartment,@CommandeInternalEditAllGroup,@CommandeInternalEditAllSite,@CommandeInternalViewAllDepartment,@CommandeInternalViewAllGroup,@CommandeInternalViewAllSite,@CommandeInternalViewInvoice,@CommandeInternalViewInvoiceAllGroup,@Config,@ConfigCreateArtikel,@ConfigCreateDept,@ConfigCreateLand,@ConfigCreateSupplier,@ConfigDeleteArtikel,@ConfigDeleteDept,@ConfigDeleteLand,@ConfigDeleteSupplier,@ConfigEditArtikel,@ConfigEditDept,@ConfigEditLand,@ConfigEditSupplier,@CreationTime,@CreationUserId,@CreditManagement,@DeleteExternalCommande,@DeleteExternalProject,@DeleteInternalCommande,@DeleteInternalProject,@DeleteKontenrahmenAccounting,@DeleteZahlungskonditionenKundenAccounting,@DeleteZahlungskonditionenLieferantenAccounting,@FinanceOrder,@FinanceProject,@IsDefault,@LastEditTime,@LastEditUserId,@MainAccessProfileId,@ModuleActivated,@Project,@ProjectExternalEditAllGroup,@ProjectExternalEditAllSite,@ProjectExternalViewAllGroup,@ProjectExternalViewAllSite,@ProjectInternalEditAllGroup,@ProjectInternalEditAllSite,@ProjectInternalViewAllGroup,@ProjectInternalViewAllSite,@Receptions,@ReceptionsEdit,@ReceptionsView,@Statistics,@StatisticsViewAll,@Suppliers,@Units,@UpdateExternalCommande,@UpdateExternalProject,@UpdateInternalCommande,@UpdateInternalProject,@UpdateKontenrahmenAccounting,@UpdateZahlungskonditionenKundenAccounting,@UpdateZahlungskonditionenLieferantenAccounting,@ViewAusfuhrAccounting,@ViewEinfuhrAccounting,@ViewExternalCommande,@ViewExternalProject,@ViewFactoringRgGutschriftlistAccounting,@ViewInternalCommande,@ViewInternalProject,@ViewKontenrahmenAccounting,@ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting,@ViewLiquiditatsplanungSkontozahlerAccounting,@ViewRechnungsTransferAccounting,@ViewRMDCZAccounting,@ViewStammdatenkontrolleWareneingangeAccounting,@ViewZahlungskonditionenKundenAccounting,@ViewZahlungskonditionenLieferantenAccounting); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Accounting", item.Accounting == null ? (object)DBNull.Value : item.Accounting);
					sqlCommand.Parameters.AddWithValue("AddExternalCommande", item.AddExternalCommande == null ? (object)DBNull.Value : item.AddExternalCommande);
					sqlCommand.Parameters.AddWithValue("AddExternalProject", item.AddExternalProject == null ? (object)DBNull.Value : item.AddExternalProject);
					sqlCommand.Parameters.AddWithValue("AddInternalCommande", item.AddInternalCommande == null ? (object)DBNull.Value : item.AddInternalCommande);
					sqlCommand.Parameters.AddWithValue("AddInternalProject", item.AddInternalProject == null ? (object)DBNull.Value : item.AddInternalProject);
					sqlCommand.Parameters.AddWithValue("AddKontenrahmenAccounting", item.AddKontenrahmenAccounting == null ? (object)DBNull.Value : item.AddKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles", item.AdministrationAccessProfiles == null ? (object)DBNull.Value : item.AdministrationAccessProfiles);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate", item.AdministrationAccessProfilesUpdate == null ? (object)DBNull.Value : item.AdministrationAccessProfilesUpdate);
					sqlCommand.Parameters.AddWithValue("AdministrationUser", item.AdministrationUser == null ? (object)DBNull.Value : item.AdministrationUser);
					sqlCommand.Parameters.AddWithValue("AdministrationUserUpdate", item.AdministrationUserUpdate == null ? (object)DBNull.Value : item.AdministrationUserUpdate);
					sqlCommand.Parameters.AddWithValue("Article", item.Article == null ? (object)DBNull.Value : item.Article);
					sqlCommand.Parameters.AddWithValue("Assign", item.Assign == null ? (object)DBNull.Value : item.Assign);
					sqlCommand.Parameters.AddWithValue("AssignAllDepartments", item.AssignAllDepartments == null ? (object)DBNull.Value : item.AssignAllDepartments);
					sqlCommand.Parameters.AddWithValue("AssignAllEmployees", item.AssignAllEmployees == null ? (object)DBNull.Value : item.AssignAllEmployees);
					sqlCommand.Parameters.AddWithValue("AssignAllSites", item.AssignAllSites == null ? (object)DBNull.Value : item.AssignAllSites);
					sqlCommand.Parameters.AddWithValue("AssignCreateDept", item.AssignCreateDept == null ? (object)DBNull.Value : item.AssignCreateDept);
					sqlCommand.Parameters.AddWithValue("AssignCreateLand", item.AssignCreateLand == null ? (object)DBNull.Value : item.AssignCreateLand);
					sqlCommand.Parameters.AddWithValue("AssignCreateUser", item.AssignCreateUser == null ? (object)DBNull.Value : item.AssignCreateUser);
					sqlCommand.Parameters.AddWithValue("AssignDeleteDept", item.AssignDeleteDept == null ? (object)DBNull.Value : item.AssignDeleteDept);
					sqlCommand.Parameters.AddWithValue("AssignDeleteLand", item.AssignDeleteLand == null ? (object)DBNull.Value : item.AssignDeleteLand);
					sqlCommand.Parameters.AddWithValue("AssignDeleteUser", item.AssignDeleteUser == null ? (object)DBNull.Value : item.AssignDeleteUser);
					sqlCommand.Parameters.AddWithValue("AssignEditDept", item.AssignEditDept == null ? (object)DBNull.Value : item.AssignEditDept);
					sqlCommand.Parameters.AddWithValue("AssignEditLand", item.AssignEditLand == null ? (object)DBNull.Value : item.AssignEditLand);
					sqlCommand.Parameters.AddWithValue("AssignEditUser", item.AssignEditUser == null ? (object)DBNull.Value : item.AssignEditUser);
					sqlCommand.Parameters.AddWithValue("AssignViewDept", item.AssignViewDept == null ? (object)DBNull.Value : item.AssignViewDept);
					sqlCommand.Parameters.AddWithValue("AssignViewLand", item.AssignViewLand == null ? (object)DBNull.Value : item.AssignViewLand);
					sqlCommand.Parameters.AddWithValue("AssignViewUser", item.AssignViewUser == null ? (object)DBNull.Value : item.AssignViewUser);
					sqlCommand.Parameters.AddWithValue("Budget", item.Budget);
					sqlCommand.Parameters.AddWithValue("CashLiquidity", item.CashLiquidity);
					sqlCommand.Parameters.AddWithValue("Commande", item.Commande == null ? (object)DBNull.Value : item.Commande);
					sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllGroup", item.CommandeExternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeExternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllSite", item.CommandeExternalEditAllSite == null ? (object)DBNull.Value : item.CommandeExternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllGroup", item.CommandeExternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllSite", item.CommandeExternalViewAllSite == null ? (object)DBNull.Value : item.CommandeExternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoice", item.CommandeExternalViewInvoice == null ? (object)DBNull.Value : item.CommandeExternalViewInvoice);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoiceAllGroup", item.CommandeExternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewInvoiceAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllDepartment", item.CommandeInternalEditAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalEditAllDepartment);
					sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllGroup", item.CommandeInternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeInternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllSite", item.CommandeInternalEditAllSite == null ? (object)DBNull.Value : item.CommandeInternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllDepartment", item.CommandeInternalViewAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalViewAllDepartment);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllGroup", item.CommandeInternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllSite", item.CommandeInternalViewAllSite == null ? (object)DBNull.Value : item.CommandeInternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoice", item.CommandeInternalViewInvoice == null ? (object)DBNull.Value : item.CommandeInternalViewInvoice);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoiceAllGroup", item.CommandeInternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewInvoiceAllGroup);
					sqlCommand.Parameters.AddWithValue("Config", item.Config == null ? (object)DBNull.Value : item.Config);
					sqlCommand.Parameters.AddWithValue("ConfigCreateArtikel", item.ConfigCreateArtikel == null ? (object)DBNull.Value : item.ConfigCreateArtikel);
					sqlCommand.Parameters.AddWithValue("ConfigCreateDept", item.ConfigCreateDept == null ? (object)DBNull.Value : item.ConfigCreateDept);
					sqlCommand.Parameters.AddWithValue("ConfigCreateLand", item.ConfigCreateLand == null ? (object)DBNull.Value : item.ConfigCreateLand);
					sqlCommand.Parameters.AddWithValue("ConfigCreateSupplier", item.ConfigCreateSupplier == null ? (object)DBNull.Value : item.ConfigCreateSupplier);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteArtikel", item.ConfigDeleteArtikel == null ? (object)DBNull.Value : item.ConfigDeleteArtikel);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteDept", item.ConfigDeleteDept == null ? (object)DBNull.Value : item.ConfigDeleteDept);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteLand", item.ConfigDeleteLand == null ? (object)DBNull.Value : item.ConfigDeleteLand);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteSupplier", item.ConfigDeleteSupplier == null ? (object)DBNull.Value : item.ConfigDeleteSupplier);
					sqlCommand.Parameters.AddWithValue("ConfigEditArtikel", item.ConfigEditArtikel == null ? (object)DBNull.Value : item.ConfigEditArtikel);
					sqlCommand.Parameters.AddWithValue("ConfigEditDept", item.ConfigEditDept == null ? (object)DBNull.Value : item.ConfigEditDept);
					sqlCommand.Parameters.AddWithValue("ConfigEditLand", item.ConfigEditLand == null ? (object)DBNull.Value : item.ConfigEditLand);
					sqlCommand.Parameters.AddWithValue("ConfigEditSupplier", item.ConfigEditSupplier == null ? (object)DBNull.Value : item.ConfigEditSupplier);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreditManagement", item.CreditManagement);
					sqlCommand.Parameters.AddWithValue("DeleteExternalCommande", item.DeleteExternalCommande == null ? (object)DBNull.Value : item.DeleteExternalCommande);
					sqlCommand.Parameters.AddWithValue("DeleteExternalProject", item.DeleteExternalProject == null ? (object)DBNull.Value : item.DeleteExternalProject);
					sqlCommand.Parameters.AddWithValue("DeleteInternalCommande", item.DeleteInternalCommande == null ? (object)DBNull.Value : item.DeleteInternalCommande);
					sqlCommand.Parameters.AddWithValue("DeleteInternalProject", item.DeleteInternalProject == null ? (object)DBNull.Value : item.DeleteInternalProject);
					sqlCommand.Parameters.AddWithValue("DeleteKontenrahmenAccounting", item.DeleteKontenrahmenAccounting == null ? (object)DBNull.Value : item.DeleteKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenKundenAccounting", item.DeleteZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenKundenAccounting);
					sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenLieferantenAccounting", item.DeleteZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenLieferantenAccounting);
					sqlCommand.Parameters.AddWithValue("FinanceOrder", item.FinanceOrder == null ? (object)DBNull.Value : item.FinanceOrder);
					sqlCommand.Parameters.AddWithValue("FinanceProject", item.FinanceProject == null ? (object)DBNull.Value : item.FinanceProject);
					sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
					sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("MainAccessProfileId", item.MainAccessProfileId == null ? (object)DBNull.Value : item.MainAccessProfileId);
					sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("Project", item.Project == null ? (object)DBNull.Value : item.Project);
					sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllGroup", item.ProjectExternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectExternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllSite", item.ProjectExternalEditAllSite == null ? (object)DBNull.Value : item.ProjectExternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllGroup", item.ProjectExternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectExternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllSite", item.ProjectExternalViewAllSite == null ? (object)DBNull.Value : item.ProjectExternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllGroup", item.ProjectInternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectInternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllSite", item.ProjectInternalEditAllSite == null ? (object)DBNull.Value : item.ProjectInternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllGroup", item.ProjectInternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectInternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllSite", item.ProjectInternalViewAllSite == null ? (object)DBNull.Value : item.ProjectInternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("Receptions", item.Receptions == null ? (object)DBNull.Value : item.Receptions);
					sqlCommand.Parameters.AddWithValue("ReceptionsEdit", item.ReceptionsEdit == null ? (object)DBNull.Value : item.ReceptionsEdit);
					sqlCommand.Parameters.AddWithValue("ReceptionsView", item.ReceptionsView == null ? (object)DBNull.Value : item.ReceptionsView);
					sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatisticsViewAll", item.StatisticsViewAll == null ? (object)DBNull.Value : item.StatisticsViewAll);
					sqlCommand.Parameters.AddWithValue("Suppliers", item.Suppliers == null ? (object)DBNull.Value : item.Suppliers);
					sqlCommand.Parameters.AddWithValue("Units", item.Units == null ? (object)DBNull.Value : item.Units);
					sqlCommand.Parameters.AddWithValue("UpdateExternalCommande", item.UpdateExternalCommande == null ? (object)DBNull.Value : item.UpdateExternalCommande);
					sqlCommand.Parameters.AddWithValue("UpdateExternalProject", item.UpdateExternalProject == null ? (object)DBNull.Value : item.UpdateExternalProject);
					sqlCommand.Parameters.AddWithValue("UpdateInternalCommande", item.UpdateInternalCommande == null ? (object)DBNull.Value : item.UpdateInternalCommande);
					sqlCommand.Parameters.AddWithValue("UpdateInternalProject", item.UpdateInternalProject == null ? (object)DBNull.Value : item.UpdateInternalProject);
					sqlCommand.Parameters.AddWithValue("UpdateKontenrahmenAccounting", item.UpdateKontenrahmenAccounting == null ? (object)DBNull.Value : item.UpdateKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenKundenAccounting", item.UpdateZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenKundenAccounting);
					sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenLieferantenAccounting", item.UpdateZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenLieferantenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewAusfuhrAccounting", item.ViewAusfuhrAccounting == null ? (object)DBNull.Value : item.ViewAusfuhrAccounting);
					sqlCommand.Parameters.AddWithValue("ViewEinfuhrAccounting", item.ViewEinfuhrAccounting == null ? (object)DBNull.Value : item.ViewEinfuhrAccounting);
					sqlCommand.Parameters.AddWithValue("ViewExternalCommande", item.ViewExternalCommande == null ? (object)DBNull.Value : item.ViewExternalCommande);
					sqlCommand.Parameters.AddWithValue("ViewExternalProject", item.ViewExternalProject == null ? (object)DBNull.Value : item.ViewExternalProject);
					sqlCommand.Parameters.AddWithValue("ViewFactoringRgGutschriftlistAccounting", item.ViewFactoringRgGutschriftlistAccounting == null ? (object)DBNull.Value : item.ViewFactoringRgGutschriftlistAccounting);
					sqlCommand.Parameters.AddWithValue("ViewInternalCommande", item.ViewInternalCommande == null ? (object)DBNull.Value : item.ViewInternalCommande);
					sqlCommand.Parameters.AddWithValue("ViewInternalProject", item.ViewInternalProject == null ? (object)DBNull.Value : item.ViewInternalProject);
					sqlCommand.Parameters.AddWithValue("ViewKontenrahmenAccounting", item.ViewKontenrahmenAccounting == null ? (object)DBNull.Value : item.ViewKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting", item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungSkontozahlerAccounting", item.ViewLiquiditatsplanungSkontozahlerAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungSkontozahlerAccounting);
					sqlCommand.Parameters.AddWithValue("ViewRechnungsTransferAccounting", item.ViewRechnungsTransferAccounting == null ? (object)DBNull.Value : item.ViewRechnungsTransferAccounting);
					sqlCommand.Parameters.AddWithValue("ViewRMDCZAccounting", item.ViewRMDCZAccounting == null ? (object)DBNull.Value : item.ViewRMDCZAccounting);
					sqlCommand.Parameters.AddWithValue("ViewStammdatenkontrolleWareneingangeAccounting", item.ViewStammdatenkontrolleWareneingangeAccounting == null ? (object)DBNull.Value : item.ViewStammdatenkontrolleWareneingangeAccounting);
					sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenKundenAccounting", item.ViewZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenKundenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenLieferantenAccounting", item.ViewZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenLieferantenAccounting);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 115; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insert(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}
				return results;
			}

			return -1;
		}
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__FNC_AccessProfile] ([AccessProfileName],[Accounting],[AddExternalCommande],[AddExternalProject],[AddInternalCommande],[AddInternalProject],[AddKontenrahmenAccounting],[Administration],[AdministrationAccessProfiles],[AdministrationAccessProfilesUpdate],[AdministrationUser],[AdministrationUserUpdate],[Article],[Assign],[AssignAllDepartments],[AssignAllEmployees],[AssignAllSites],[AssignCreateDept],[AssignCreateLand],[AssignCreateUser],[AssignDeleteDept],[AssignDeleteLand],[AssignDeleteUser],[AssignEditDept],[AssignEditLand],[AssignEditUser],[AssignViewDept],[AssignViewLand],[AssignViewUser],[Budget],[CashLiquidity],[Commande],[CommandeExternalEditAllGroup],[CommandeExternalEditAllSite],[CommandeExternalViewAllGroup],[CommandeExternalViewAllSite],[CommandeExternalViewInvoice],[CommandeExternalViewInvoiceAllGroup],[CommandeInternalEditAllDepartment],[CommandeInternalEditAllGroup],[CommandeInternalEditAllSite],[CommandeInternalViewAllDepartment],[CommandeInternalViewAllGroup],[CommandeInternalViewAllSite],[CommandeInternalViewInvoice],[CommandeInternalViewInvoiceAllGroup],[Config],[ConfigCreateArtikel],[ConfigCreateDept],[ConfigCreateLand],[ConfigCreateSupplier],[ConfigDeleteArtikel],[ConfigDeleteDept],[ConfigDeleteLand],[ConfigDeleteSupplier],[ConfigEditArtikel],[ConfigEditDept],[ConfigEditLand],[ConfigEditSupplier],[CreationTime],[CreationUserId],[CreditManagement],[DeleteExternalCommande],[DeleteExternalProject],[DeleteInternalCommande],[DeleteInternalProject],[DeleteKontenrahmenAccounting],[DeleteZahlungskonditionenKundenAccounting],[DeleteZahlungskonditionenLieferantenAccounting],[FinanceOrder],[FinanceProject],[IsDefault],[LastEditTime],[LastEditUserId],[MainAccessProfileId],[ModuleActivated],[Project],[ProjectExternalEditAllGroup],[ProjectExternalEditAllSite],[ProjectExternalViewAllGroup],[ProjectExternalViewAllSite],[ProjectInternalEditAllGroup],[ProjectInternalEditAllSite],[ProjectInternalViewAllGroup],[ProjectInternalViewAllSite],[Receptions],[ReceptionsEdit],[ReceptionsView],[Statistics],[StatisticsViewAll],[Suppliers],[Units],[UpdateExternalCommande],[UpdateExternalProject],[UpdateInternalCommande],[UpdateInternalProject],[UpdateKontenrahmenAccounting],[UpdateZahlungskonditionenKundenAccounting],[UpdateZahlungskonditionenLieferantenAccounting],[ViewAusfuhrAccounting],[ViewEinfuhrAccounting],[ViewExternalCommande],[ViewExternalProject],[ViewFactoringRgGutschriftlistAccounting],[ViewInternalCommande],[ViewInternalProject],[ViewKontenrahmenAccounting],[ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting],[ViewLiquiditatsplanungSkontozahlerAccounting],[ViewRechnungsTransferAccounting],[ViewRMDCZAccounting],[ViewStammdatenkontrolleWareneingangeAccounting],[ViewZahlungskonditionenKundenAccounting],[ViewZahlungskonditionenLieferantenAccounting]) VALUES ( "

							+ "@AccessProfileName" + i + ","
							+ "@Accounting" + i + ","
							+ "@AddExternalCommande" + i + ","
							+ "@AddExternalProject" + i + ","
							+ "@AddInternalCommande" + i + ","
							+ "@AddInternalProject" + i + ","
							+ "@AddKontenrahmenAccounting" + i + ","
							+ "@Administration" + i + ","
							+ "@AdministrationAccessProfiles" + i + ","
							+ "@AdministrationAccessProfilesUpdate" + i + ","
							+ "@AdministrationUser" + i + ","
							+ "@AdministrationUserUpdate" + i + ","
							+ "@Article" + i + ","
							+ "@Assign" + i + ","
							+ "@AssignAllDepartments" + i + ","
							+ "@AssignAllEmployees" + i + ","
							+ "@AssignAllSites" + i + ","
							+ "@AssignCreateDept" + i + ","
							+ "@AssignCreateLand" + i + ","
							+ "@AssignCreateUser" + i + ","
							+ "@AssignDeleteDept" + i + ","
							+ "@AssignDeleteLand" + i + ","
							+ "@AssignDeleteUser" + i + ","
							+ "@AssignEditDept" + i + ","
							+ "@AssignEditLand" + i + ","
							+ "@AssignEditUser" + i + ","
							+ "@AssignViewDept" + i + ","
							+ "@AssignViewLand" + i + ","
							+ "@AssignViewUser" + i + ","
							+ "@Budget" + i + ","
							+ "@CashLiquidity" + i + ","
							+ "@Commande" + i + ","
							+ "@CommandeExternalEditAllGroup" + i + ","
							+ "@CommandeExternalEditAllSite" + i + ","
							+ "@CommandeExternalViewAllGroup" + i + ","
							+ "@CommandeExternalViewAllSite" + i + ","
							+ "@CommandeExternalViewInvoice" + i + ","
							+ "@CommandeExternalViewInvoiceAllGroup" + i + ","
							+ "@CommandeInternalEditAllDepartment" + i + ","
							+ "@CommandeInternalEditAllGroup" + i + ","
							+ "@CommandeInternalEditAllSite" + i + ","
							+ "@CommandeInternalViewAllDepartment" + i + ","
							+ "@CommandeInternalViewAllGroup" + i + ","
							+ "@CommandeInternalViewAllSite" + i + ","
							+ "@CommandeInternalViewInvoice" + i + ","
							+ "@CommandeInternalViewInvoiceAllGroup" + i + ","
							+ "@Config" + i + ","
							+ "@ConfigCreateArtikel" + i + ","
							+ "@ConfigCreateDept" + i + ","
							+ "@ConfigCreateLand" + i + ","
							+ "@ConfigCreateSupplier" + i + ","
							+ "@ConfigDeleteArtikel" + i + ","
							+ "@ConfigDeleteDept" + i + ","
							+ "@ConfigDeleteLand" + i + ","
							+ "@ConfigDeleteSupplier" + i + ","
							+ "@ConfigEditArtikel" + i + ","
							+ "@ConfigEditDept" + i + ","
							+ "@ConfigEditLand" + i + ","
							+ "@ConfigEditSupplier" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@CreditManagement" + i + ","
							+ "@DeleteExternalCommande" + i + ","
							+ "@DeleteExternalProject" + i + ","
							+ "@DeleteInternalCommande" + i + ","
							+ "@DeleteInternalProject" + i + ","
							+ "@DeleteKontenrahmenAccounting" + i + ","
							+ "@DeleteZahlungskonditionenKundenAccounting" + i + ","
							+ "@DeleteZahlungskonditionenLieferantenAccounting" + i + ","
							+ "@FinanceOrder" + i + ","
							+ "@FinanceProject" + i + ","
							+ "@IsDefault" + i + ","
							+ "@LastEditTime" + i + ","
							+ "@LastEditUserId" + i + ","
							+ "@MainAccessProfileId" + i + ","
							+ "@ModuleActivated" + i + ","
							+ "@Project" + i + ","
							+ "@ProjectExternalEditAllGroup" + i + ","
							+ "@ProjectExternalEditAllSite" + i + ","
							+ "@ProjectExternalViewAllGroup" + i + ","
							+ "@ProjectExternalViewAllSite" + i + ","
							+ "@ProjectInternalEditAllGroup" + i + ","
							+ "@ProjectInternalEditAllSite" + i + ","
							+ "@ProjectInternalViewAllGroup" + i + ","
							+ "@ProjectInternalViewAllSite" + i + ","
							+ "@Receptions" + i + ","
							+ "@ReceptionsEdit" + i + ","
							+ "@ReceptionsView" + i + ","
							+ "@Statistics" + i + ","
							+ "@StatisticsViewAll" + i + ","
							+ "@Suppliers" + i + ","
							+ "@Units" + i + ","
							+ "@UpdateExternalCommande" + i + ","
							+ "@UpdateExternalProject" + i + ","
							+ "@UpdateInternalCommande" + i + ","
							+ "@UpdateInternalProject" + i + ","
							+ "@UpdateKontenrahmenAccounting" + i + ","
							+ "@UpdateZahlungskonditionenKundenAccounting" + i + ","
							+ "@UpdateZahlungskonditionenLieferantenAccounting" + i + ","
							+ "@ViewAusfuhrAccounting" + i + ","
							+ "@ViewEinfuhrAccounting" + i + ","
							+ "@ViewExternalCommande" + i + ","
							+ "@ViewExternalProject" + i + ","
							+ "@ViewFactoringRgGutschriftlistAccounting" + i + ","
							+ "@ViewInternalCommande" + i + ","
							+ "@ViewInternalProject" + i + ","
							+ "@ViewKontenrahmenAccounting" + i + ","
							+ "@ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting" + i + ","
							+ "@ViewLiquiditatsplanungSkontozahlerAccounting" + i + ","
							+ "@ViewRechnungsTransferAccounting" + i + ","
							+ "@ViewRMDCZAccounting" + i + ","
							+ "@ViewStammdatenkontrolleWareneingangeAccounting" + i + ","
							+ "@ViewZahlungskonditionenKundenAccounting" + i + ","
							+ "@ViewZahlungskonditionenLieferantenAccounting" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("Accounting" + i, item.Accounting == null ? (object)DBNull.Value : item.Accounting);
						sqlCommand.Parameters.AddWithValue("AddExternalCommande" + i, item.AddExternalCommande == null ? (object)DBNull.Value : item.AddExternalCommande);
						sqlCommand.Parameters.AddWithValue("AddExternalProject" + i, item.AddExternalProject == null ? (object)DBNull.Value : item.AddExternalProject);
						sqlCommand.Parameters.AddWithValue("AddInternalCommande" + i, item.AddInternalCommande == null ? (object)DBNull.Value : item.AddInternalCommande);
						sqlCommand.Parameters.AddWithValue("AddInternalProject" + i, item.AddInternalProject == null ? (object)DBNull.Value : item.AddInternalProject);
						sqlCommand.Parameters.AddWithValue("AddKontenrahmenAccounting" + i, item.AddKontenrahmenAccounting == null ? (object)DBNull.Value : item.AddKontenrahmenAccounting);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
						sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles" + i, item.AdministrationAccessProfiles == null ? (object)DBNull.Value : item.AdministrationAccessProfiles);
						sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate" + i, item.AdministrationAccessProfilesUpdate == null ? (object)DBNull.Value : item.AdministrationAccessProfilesUpdate);
						sqlCommand.Parameters.AddWithValue("AdministrationUser" + i, item.AdministrationUser == null ? (object)DBNull.Value : item.AdministrationUser);
						sqlCommand.Parameters.AddWithValue("AdministrationUserUpdate" + i, item.AdministrationUserUpdate == null ? (object)DBNull.Value : item.AdministrationUserUpdate);
						sqlCommand.Parameters.AddWithValue("Article" + i, item.Article == null ? (object)DBNull.Value : item.Article);
						sqlCommand.Parameters.AddWithValue("Assign" + i, item.Assign == null ? (object)DBNull.Value : item.Assign);
						sqlCommand.Parameters.AddWithValue("AssignAllDepartments" + i, item.AssignAllDepartments == null ? (object)DBNull.Value : item.AssignAllDepartments);
						sqlCommand.Parameters.AddWithValue("AssignAllEmployees" + i, item.AssignAllEmployees == null ? (object)DBNull.Value : item.AssignAllEmployees);
						sqlCommand.Parameters.AddWithValue("AssignAllSites" + i, item.AssignAllSites == null ? (object)DBNull.Value : item.AssignAllSites);
						sqlCommand.Parameters.AddWithValue("AssignCreateDept" + i, item.AssignCreateDept == null ? (object)DBNull.Value : item.AssignCreateDept);
						sqlCommand.Parameters.AddWithValue("AssignCreateLand" + i, item.AssignCreateLand == null ? (object)DBNull.Value : item.AssignCreateLand);
						sqlCommand.Parameters.AddWithValue("AssignCreateUser" + i, item.AssignCreateUser == null ? (object)DBNull.Value : item.AssignCreateUser);
						sqlCommand.Parameters.AddWithValue("AssignDeleteDept" + i, item.AssignDeleteDept == null ? (object)DBNull.Value : item.AssignDeleteDept);
						sqlCommand.Parameters.AddWithValue("AssignDeleteLand" + i, item.AssignDeleteLand == null ? (object)DBNull.Value : item.AssignDeleteLand);
						sqlCommand.Parameters.AddWithValue("AssignDeleteUser" + i, item.AssignDeleteUser == null ? (object)DBNull.Value : item.AssignDeleteUser);
						sqlCommand.Parameters.AddWithValue("AssignEditDept" + i, item.AssignEditDept == null ? (object)DBNull.Value : item.AssignEditDept);
						sqlCommand.Parameters.AddWithValue("AssignEditLand" + i, item.AssignEditLand == null ? (object)DBNull.Value : item.AssignEditLand);
						sqlCommand.Parameters.AddWithValue("AssignEditUser" + i, item.AssignEditUser == null ? (object)DBNull.Value : item.AssignEditUser);
						sqlCommand.Parameters.AddWithValue("AssignViewDept" + i, item.AssignViewDept == null ? (object)DBNull.Value : item.AssignViewDept);
						sqlCommand.Parameters.AddWithValue("AssignViewLand" + i, item.AssignViewLand == null ? (object)DBNull.Value : item.AssignViewLand);
						sqlCommand.Parameters.AddWithValue("AssignViewUser" + i, item.AssignViewUser == null ? (object)DBNull.Value : item.AssignViewUser);
						sqlCommand.Parameters.AddWithValue("Budget" + i, item.Budget);
						sqlCommand.Parameters.AddWithValue("CashLiquidity" + i, item.CashLiquidity);
						sqlCommand.Parameters.AddWithValue("Commande" + i, item.Commande == null ? (object)DBNull.Value : item.Commande);
						sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllGroup" + i, item.CommandeExternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeExternalEditAllGroup);
						sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllSite" + i, item.CommandeExternalEditAllSite == null ? (object)DBNull.Value : item.CommandeExternalEditAllSite);
						sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllGroup" + i, item.CommandeExternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewAllGroup);
						sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllSite" + i, item.CommandeExternalViewAllSite == null ? (object)DBNull.Value : item.CommandeExternalViewAllSite);
						sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoice" + i, item.CommandeExternalViewInvoice == null ? (object)DBNull.Value : item.CommandeExternalViewInvoice);
						sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoiceAllGroup" + i, item.CommandeExternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewInvoiceAllGroup);
						sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllDepartment" + i, item.CommandeInternalEditAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalEditAllDepartment);
						sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllGroup" + i, item.CommandeInternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeInternalEditAllGroup);
						sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllSite" + i, item.CommandeInternalEditAllSite == null ? (object)DBNull.Value : item.CommandeInternalEditAllSite);
						sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllDepartment" + i, item.CommandeInternalViewAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalViewAllDepartment);
						sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllGroup" + i, item.CommandeInternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewAllGroup);
						sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllSite" + i, item.CommandeInternalViewAllSite == null ? (object)DBNull.Value : item.CommandeInternalViewAllSite);
						sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoice" + i, item.CommandeInternalViewInvoice == null ? (object)DBNull.Value : item.CommandeInternalViewInvoice);
						sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoiceAllGroup" + i, item.CommandeInternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewInvoiceAllGroup);
						sqlCommand.Parameters.AddWithValue("Config" + i, item.Config == null ? (object)DBNull.Value : item.Config);
						sqlCommand.Parameters.AddWithValue("ConfigCreateArtikel" + i, item.ConfigCreateArtikel == null ? (object)DBNull.Value : item.ConfigCreateArtikel);
						sqlCommand.Parameters.AddWithValue("ConfigCreateDept" + i, item.ConfigCreateDept == null ? (object)DBNull.Value : item.ConfigCreateDept);
						sqlCommand.Parameters.AddWithValue("ConfigCreateLand" + i, item.ConfigCreateLand == null ? (object)DBNull.Value : item.ConfigCreateLand);
						sqlCommand.Parameters.AddWithValue("ConfigCreateSupplier" + i, item.ConfigCreateSupplier == null ? (object)DBNull.Value : item.ConfigCreateSupplier);
						sqlCommand.Parameters.AddWithValue("ConfigDeleteArtikel" + i, item.ConfigDeleteArtikel == null ? (object)DBNull.Value : item.ConfigDeleteArtikel);
						sqlCommand.Parameters.AddWithValue("ConfigDeleteDept" + i, item.ConfigDeleteDept == null ? (object)DBNull.Value : item.ConfigDeleteDept);
						sqlCommand.Parameters.AddWithValue("ConfigDeleteLand" + i, item.ConfigDeleteLand == null ? (object)DBNull.Value : item.ConfigDeleteLand);
						sqlCommand.Parameters.AddWithValue("ConfigDeleteSupplier" + i, item.ConfigDeleteSupplier == null ? (object)DBNull.Value : item.ConfigDeleteSupplier);
						sqlCommand.Parameters.AddWithValue("ConfigEditArtikel" + i, item.ConfigEditArtikel == null ? (object)DBNull.Value : item.ConfigEditArtikel);
						sqlCommand.Parameters.AddWithValue("ConfigEditDept" + i, item.ConfigEditDept == null ? (object)DBNull.Value : item.ConfigEditDept);
						sqlCommand.Parameters.AddWithValue("ConfigEditLand" + i, item.ConfigEditLand == null ? (object)DBNull.Value : item.ConfigEditLand);
						sqlCommand.Parameters.AddWithValue("ConfigEditSupplier" + i, item.ConfigEditSupplier == null ? (object)DBNull.Value : item.ConfigEditSupplier);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreditManagement" + i, item.CreditManagement);
						sqlCommand.Parameters.AddWithValue("DeleteExternalCommande" + i, item.DeleteExternalCommande == null ? (object)DBNull.Value : item.DeleteExternalCommande);
						sqlCommand.Parameters.AddWithValue("DeleteExternalProject" + i, item.DeleteExternalProject == null ? (object)DBNull.Value : item.DeleteExternalProject);
						sqlCommand.Parameters.AddWithValue("DeleteInternalCommande" + i, item.DeleteInternalCommande == null ? (object)DBNull.Value : item.DeleteInternalCommande);
						sqlCommand.Parameters.AddWithValue("DeleteInternalProject" + i, item.DeleteInternalProject == null ? (object)DBNull.Value : item.DeleteInternalProject);
						sqlCommand.Parameters.AddWithValue("DeleteKontenrahmenAccounting" + i, item.DeleteKontenrahmenAccounting == null ? (object)DBNull.Value : item.DeleteKontenrahmenAccounting);
						sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenKundenAccounting" + i, item.DeleteZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenKundenAccounting);
						sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenLieferantenAccounting" + i, item.DeleteZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenLieferantenAccounting);
						sqlCommand.Parameters.AddWithValue("FinanceOrder" + i, item.FinanceOrder == null ? (object)DBNull.Value : item.FinanceOrder);
						sqlCommand.Parameters.AddWithValue("FinanceProject" + i, item.FinanceProject == null ? (object)DBNull.Value : item.FinanceProject);
						sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("MainAccessProfileId" + i, item.MainAccessProfileId == null ? (object)DBNull.Value : item.MainAccessProfileId);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("Project" + i, item.Project == null ? (object)DBNull.Value : item.Project);
						sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllGroup" + i, item.ProjectExternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectExternalEditAllGroup);
						sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllSite" + i, item.ProjectExternalEditAllSite == null ? (object)DBNull.Value : item.ProjectExternalEditAllSite);
						sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllGroup" + i, item.ProjectExternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectExternalViewAllGroup);
						sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllSite" + i, item.ProjectExternalViewAllSite == null ? (object)DBNull.Value : item.ProjectExternalViewAllSite);
						sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllGroup" + i, item.ProjectInternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectInternalEditAllGroup);
						sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllSite" + i, item.ProjectInternalEditAllSite == null ? (object)DBNull.Value : item.ProjectInternalEditAllSite);
						sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllGroup" + i, item.ProjectInternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectInternalViewAllGroup);
						sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllSite" + i, item.ProjectInternalViewAllSite == null ? (object)DBNull.Value : item.ProjectInternalViewAllSite);
						sqlCommand.Parameters.AddWithValue("Receptions" + i, item.Receptions == null ? (object)DBNull.Value : item.Receptions);
						sqlCommand.Parameters.AddWithValue("ReceptionsEdit" + i, item.ReceptionsEdit == null ? (object)DBNull.Value : item.ReceptionsEdit);
						sqlCommand.Parameters.AddWithValue("ReceptionsView" + i, item.ReceptionsView == null ? (object)DBNull.Value : item.ReceptionsView);
						sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
						sqlCommand.Parameters.AddWithValue("StatisticsViewAll" + i, item.StatisticsViewAll == null ? (object)DBNull.Value : item.StatisticsViewAll);
						sqlCommand.Parameters.AddWithValue("Suppliers" + i, item.Suppliers == null ? (object)DBNull.Value : item.Suppliers);
						sqlCommand.Parameters.AddWithValue("Units" + i, item.Units == null ? (object)DBNull.Value : item.Units);
						sqlCommand.Parameters.AddWithValue("UpdateExternalCommande" + i, item.UpdateExternalCommande == null ? (object)DBNull.Value : item.UpdateExternalCommande);
						sqlCommand.Parameters.AddWithValue("UpdateExternalProject" + i, item.UpdateExternalProject == null ? (object)DBNull.Value : item.UpdateExternalProject);
						sqlCommand.Parameters.AddWithValue("UpdateInternalCommande" + i, item.UpdateInternalCommande == null ? (object)DBNull.Value : item.UpdateInternalCommande);
						sqlCommand.Parameters.AddWithValue("UpdateInternalProject" + i, item.UpdateInternalProject == null ? (object)DBNull.Value : item.UpdateInternalProject);
						sqlCommand.Parameters.AddWithValue("UpdateKontenrahmenAccounting" + i, item.UpdateKontenrahmenAccounting == null ? (object)DBNull.Value : item.UpdateKontenrahmenAccounting);
						sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenKundenAccounting" + i, item.UpdateZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenKundenAccounting);
						sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenLieferantenAccounting" + i, item.UpdateZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenLieferantenAccounting);
						sqlCommand.Parameters.AddWithValue("ViewAusfuhrAccounting" + i, item.ViewAusfuhrAccounting == null ? (object)DBNull.Value : item.ViewAusfuhrAccounting);
						sqlCommand.Parameters.AddWithValue("ViewEinfuhrAccounting" + i, item.ViewEinfuhrAccounting == null ? (object)DBNull.Value : item.ViewEinfuhrAccounting);
						sqlCommand.Parameters.AddWithValue("ViewExternalCommande" + i, item.ViewExternalCommande == null ? (object)DBNull.Value : item.ViewExternalCommande);
						sqlCommand.Parameters.AddWithValue("ViewExternalProject" + i, item.ViewExternalProject == null ? (object)DBNull.Value : item.ViewExternalProject);
						sqlCommand.Parameters.AddWithValue("ViewFactoringRgGutschriftlistAccounting" + i, item.ViewFactoringRgGutschriftlistAccounting == null ? (object)DBNull.Value : item.ViewFactoringRgGutschriftlistAccounting);
						sqlCommand.Parameters.AddWithValue("ViewInternalCommande" + i, item.ViewInternalCommande == null ? (object)DBNull.Value : item.ViewInternalCommande);
						sqlCommand.Parameters.AddWithValue("ViewInternalProject" + i, item.ViewInternalProject == null ? (object)DBNull.Value : item.ViewInternalProject);
						sqlCommand.Parameters.AddWithValue("ViewKontenrahmenAccounting" + i, item.ViewKontenrahmenAccounting == null ? (object)DBNull.Value : item.ViewKontenrahmenAccounting);
						sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting" + i, item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting);
						sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungSkontozahlerAccounting" + i, item.ViewLiquiditatsplanungSkontozahlerAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungSkontozahlerAccounting);
						sqlCommand.Parameters.AddWithValue("ViewRechnungsTransferAccounting" + i, item.ViewRechnungsTransferAccounting == null ? (object)DBNull.Value : item.ViewRechnungsTransferAccounting);
						sqlCommand.Parameters.AddWithValue("ViewRMDCZAccounting" + i, item.ViewRMDCZAccounting == null ? (object)DBNull.Value : item.ViewRMDCZAccounting);
						sqlCommand.Parameters.AddWithValue("ViewStammdatenkontrolleWareneingangeAccounting" + i, item.ViewStammdatenkontrolleWareneingangeAccounting == null ? (object)DBNull.Value : item.ViewStammdatenkontrolleWareneingangeAccounting);
						sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenKundenAccounting" + i, item.ViewZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenKundenAccounting);
						sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenLieferantenAccounting" + i, item.ViewZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenLieferantenAccounting);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_AccessProfile] SET [AccessProfileName]=@AccessProfileName, [Accounting]=@Accounting, [AddExternalCommande]=@AddExternalCommande, [AddExternalProject]=@AddExternalProject, [AddInternalCommande]=@AddInternalCommande, [AddInternalProject]=@AddInternalProject, [AddKontenrahmenAccounting]=@AddKontenrahmenAccounting, [Administration]=@Administration, [AdministrationAccessProfiles]=@AdministrationAccessProfiles, [AdministrationAccessProfilesUpdate]=@AdministrationAccessProfilesUpdate, [AdministrationUser]=@AdministrationUser, [AdministrationUserUpdate]=@AdministrationUserUpdate, [Article]=@Article, [Assign]=@Assign, [AssignAllDepartments]=@AssignAllDepartments, [AssignAllEmployees]=@AssignAllEmployees, [AssignAllSites]=@AssignAllSites, [AssignCreateDept]=@AssignCreateDept, [AssignCreateLand]=@AssignCreateLand, [AssignCreateUser]=@AssignCreateUser, [AssignDeleteDept]=@AssignDeleteDept, [AssignDeleteLand]=@AssignDeleteLand, [AssignDeleteUser]=@AssignDeleteUser, [AssignEditDept]=@AssignEditDept, [AssignEditLand]=@AssignEditLand, [AssignEditUser]=@AssignEditUser, [AssignViewDept]=@AssignViewDept, [AssignViewLand]=@AssignViewLand, [AssignViewUser]=@AssignViewUser, [Budget]=@Budget, [CashLiquidity]=@CashLiquidity, [Commande]=@Commande, [CommandeExternalEditAllGroup]=@CommandeExternalEditAllGroup, [CommandeExternalEditAllSite]=@CommandeExternalEditAllSite, [CommandeExternalViewAllGroup]=@CommandeExternalViewAllGroup, [CommandeExternalViewAllSite]=@CommandeExternalViewAllSite, [CommandeExternalViewInvoice]=@CommandeExternalViewInvoice, [CommandeExternalViewInvoiceAllGroup]=@CommandeExternalViewInvoiceAllGroup, [CommandeInternalEditAllDepartment]=@CommandeInternalEditAllDepartment, [CommandeInternalEditAllGroup]=@CommandeInternalEditAllGroup, [CommandeInternalEditAllSite]=@CommandeInternalEditAllSite, [CommandeInternalViewAllDepartment]=@CommandeInternalViewAllDepartment, [CommandeInternalViewAllGroup]=@CommandeInternalViewAllGroup, [CommandeInternalViewAllSite]=@CommandeInternalViewAllSite, [CommandeInternalViewInvoice]=@CommandeInternalViewInvoice, [CommandeInternalViewInvoiceAllGroup]=@CommandeInternalViewInvoiceAllGroup, [Config]=@Config, [ConfigCreateArtikel]=@ConfigCreateArtikel, [ConfigCreateDept]=@ConfigCreateDept, [ConfigCreateLand]=@ConfigCreateLand, [ConfigCreateSupplier]=@ConfigCreateSupplier, [ConfigDeleteArtikel]=@ConfigDeleteArtikel, [ConfigDeleteDept]=@ConfigDeleteDept, [ConfigDeleteLand]=@ConfigDeleteLand, [ConfigDeleteSupplier]=@ConfigDeleteSupplier, [ConfigEditArtikel]=@ConfigEditArtikel, [ConfigEditDept]=@ConfigEditDept, [ConfigEditLand]=@ConfigEditLand, [ConfigEditSupplier]=@ConfigEditSupplier, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CreditManagement]=@CreditManagement, [DeleteExternalCommande]=@DeleteExternalCommande, [DeleteExternalProject]=@DeleteExternalProject, [DeleteInternalCommande]=@DeleteInternalCommande, [DeleteInternalProject]=@DeleteInternalProject, [DeleteKontenrahmenAccounting]=@DeleteKontenrahmenAccounting, [DeleteZahlungskonditionenKundenAccounting]=@DeleteZahlungskonditionenKundenAccounting, [DeleteZahlungskonditionenLieferantenAccounting]=@DeleteZahlungskonditionenLieferantenAccounting, [FinanceOrder]=@FinanceOrder, [FinanceProject]=@FinanceProject, [IsDefault]=@IsDefault, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [MainAccessProfileId]=@MainAccessProfileId, [ModuleActivated]=@ModuleActivated, [Project]=@Project, [ProjectExternalEditAllGroup]=@ProjectExternalEditAllGroup, [ProjectExternalEditAllSite]=@ProjectExternalEditAllSite, [ProjectExternalViewAllGroup]=@ProjectExternalViewAllGroup, [ProjectExternalViewAllSite]=@ProjectExternalViewAllSite, [ProjectInternalEditAllGroup]=@ProjectInternalEditAllGroup, [ProjectInternalEditAllSite]=@ProjectInternalEditAllSite, [ProjectInternalViewAllGroup]=@ProjectInternalViewAllGroup, [ProjectInternalViewAllSite]=@ProjectInternalViewAllSite, [Receptions]=@Receptions, [ReceptionsEdit]=@ReceptionsEdit, [ReceptionsView]=@ReceptionsView, [Statistics]=@Statistics, [StatisticsViewAll]=@StatisticsViewAll, [Suppliers]=@Suppliers, [Units]=@Units, [UpdateExternalCommande]=@UpdateExternalCommande, [UpdateExternalProject]=@UpdateExternalProject, [UpdateInternalCommande]=@UpdateInternalCommande, [UpdateInternalProject]=@UpdateInternalProject, [UpdateKontenrahmenAccounting]=@UpdateKontenrahmenAccounting, [UpdateZahlungskonditionenKundenAccounting]=@UpdateZahlungskonditionenKundenAccounting, [UpdateZahlungskonditionenLieferantenAccounting]=@UpdateZahlungskonditionenLieferantenAccounting, [ViewAusfuhrAccounting]=@ViewAusfuhrAccounting, [ViewEinfuhrAccounting]=@ViewEinfuhrAccounting, [ViewExternalCommande]=@ViewExternalCommande, [ViewExternalProject]=@ViewExternalProject, [ViewFactoringRgGutschriftlistAccounting]=@ViewFactoringRgGutschriftlistAccounting, [ViewInternalCommande]=@ViewInternalCommande, [ViewInternalProject]=@ViewInternalProject, [ViewKontenrahmenAccounting]=@ViewKontenrahmenAccounting, [ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting]=@ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting, [ViewLiquiditatsplanungSkontozahlerAccounting]=@ViewLiquiditatsplanungSkontozahlerAccounting, [ViewRechnungsTransferAccounting]=@ViewRechnungsTransferAccounting, [ViewRMDCZAccounting]=@ViewRMDCZAccounting, [ViewStammdatenkontrolleWareneingangeAccounting]=@ViewStammdatenkontrolleWareneingangeAccounting, [ViewZahlungskonditionenKundenAccounting]=@ViewZahlungskonditionenKundenAccounting, [ViewZahlungskonditionenLieferantenAccounting]=@ViewZahlungskonditionenLieferantenAccounting WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
				sqlCommand.Parameters.AddWithValue("Accounting", item.Accounting == null ? (object)DBNull.Value : item.Accounting);
				sqlCommand.Parameters.AddWithValue("AddExternalCommande", item.AddExternalCommande == null ? (object)DBNull.Value : item.AddExternalCommande);
				sqlCommand.Parameters.AddWithValue("AddExternalProject", item.AddExternalProject == null ? (object)DBNull.Value : item.AddExternalProject);
				sqlCommand.Parameters.AddWithValue("AddInternalCommande", item.AddInternalCommande == null ? (object)DBNull.Value : item.AddInternalCommande);
				sqlCommand.Parameters.AddWithValue("AddInternalProject", item.AddInternalProject == null ? (object)DBNull.Value : item.AddInternalProject);
				sqlCommand.Parameters.AddWithValue("AddKontenrahmenAccounting", item.AddKontenrahmenAccounting == null ? (object)DBNull.Value : item.AddKontenrahmenAccounting);
				sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
				sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles", item.AdministrationAccessProfiles == null ? (object)DBNull.Value : item.AdministrationAccessProfiles);
				sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate", item.AdministrationAccessProfilesUpdate == null ? (object)DBNull.Value : item.AdministrationAccessProfilesUpdate);
				sqlCommand.Parameters.AddWithValue("AdministrationUser", item.AdministrationUser == null ? (object)DBNull.Value : item.AdministrationUser);
				sqlCommand.Parameters.AddWithValue("AdministrationUserUpdate", item.AdministrationUserUpdate == null ? (object)DBNull.Value : item.AdministrationUserUpdate);
				sqlCommand.Parameters.AddWithValue("Article", item.Article == null ? (object)DBNull.Value : item.Article);
				sqlCommand.Parameters.AddWithValue("Assign", item.Assign == null ? (object)DBNull.Value : item.Assign);
				sqlCommand.Parameters.AddWithValue("AssignAllDepartments", item.AssignAllDepartments == null ? (object)DBNull.Value : item.AssignAllDepartments);
				sqlCommand.Parameters.AddWithValue("AssignAllEmployees", item.AssignAllEmployees == null ? (object)DBNull.Value : item.AssignAllEmployees);
				sqlCommand.Parameters.AddWithValue("AssignAllSites", item.AssignAllSites == null ? (object)DBNull.Value : item.AssignAllSites);
				sqlCommand.Parameters.AddWithValue("AssignCreateDept", item.AssignCreateDept == null ? (object)DBNull.Value : item.AssignCreateDept);
				sqlCommand.Parameters.AddWithValue("AssignCreateLand", item.AssignCreateLand == null ? (object)DBNull.Value : item.AssignCreateLand);
				sqlCommand.Parameters.AddWithValue("AssignCreateUser", item.AssignCreateUser == null ? (object)DBNull.Value : item.AssignCreateUser);
				sqlCommand.Parameters.AddWithValue("AssignDeleteDept", item.AssignDeleteDept == null ? (object)DBNull.Value : item.AssignDeleteDept);
				sqlCommand.Parameters.AddWithValue("AssignDeleteLand", item.AssignDeleteLand == null ? (object)DBNull.Value : item.AssignDeleteLand);
				sqlCommand.Parameters.AddWithValue("AssignDeleteUser", item.AssignDeleteUser == null ? (object)DBNull.Value : item.AssignDeleteUser);
				sqlCommand.Parameters.AddWithValue("AssignEditDept", item.AssignEditDept == null ? (object)DBNull.Value : item.AssignEditDept);
				sqlCommand.Parameters.AddWithValue("AssignEditLand", item.AssignEditLand == null ? (object)DBNull.Value : item.AssignEditLand);
				sqlCommand.Parameters.AddWithValue("AssignEditUser", item.AssignEditUser == null ? (object)DBNull.Value : item.AssignEditUser);
				sqlCommand.Parameters.AddWithValue("AssignViewDept", item.AssignViewDept == null ? (object)DBNull.Value : item.AssignViewDept);
				sqlCommand.Parameters.AddWithValue("AssignViewLand", item.AssignViewLand == null ? (object)DBNull.Value : item.AssignViewLand);
				sqlCommand.Parameters.AddWithValue("AssignViewUser", item.AssignViewUser == null ? (object)DBNull.Value : item.AssignViewUser);
				sqlCommand.Parameters.AddWithValue("Budget", item.Budget);
				sqlCommand.Parameters.AddWithValue("CashLiquidity", item.CashLiquidity);
				sqlCommand.Parameters.AddWithValue("Commande", item.Commande == null ? (object)DBNull.Value : item.Commande);
				sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllGroup", item.CommandeExternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeExternalEditAllGroup);
				sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllSite", item.CommandeExternalEditAllSite == null ? (object)DBNull.Value : item.CommandeExternalEditAllSite);
				sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllGroup", item.CommandeExternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewAllGroup);
				sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllSite", item.CommandeExternalViewAllSite == null ? (object)DBNull.Value : item.CommandeExternalViewAllSite);
				sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoice", item.CommandeExternalViewInvoice == null ? (object)DBNull.Value : item.CommandeExternalViewInvoice);
				sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoiceAllGroup", item.CommandeExternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewInvoiceAllGroup);
				sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllDepartment", item.CommandeInternalEditAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalEditAllDepartment);
				sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllGroup", item.CommandeInternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeInternalEditAllGroup);
				sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllSite", item.CommandeInternalEditAllSite == null ? (object)DBNull.Value : item.CommandeInternalEditAllSite);
				sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllDepartment", item.CommandeInternalViewAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalViewAllDepartment);
				sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllGroup", item.CommandeInternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewAllGroup);
				sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllSite", item.CommandeInternalViewAllSite == null ? (object)DBNull.Value : item.CommandeInternalViewAllSite);
				sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoice", item.CommandeInternalViewInvoice == null ? (object)DBNull.Value : item.CommandeInternalViewInvoice);
				sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoiceAllGroup", item.CommandeInternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewInvoiceAllGroup);
				sqlCommand.Parameters.AddWithValue("Config", item.Config == null ? (object)DBNull.Value : item.Config);
				sqlCommand.Parameters.AddWithValue("ConfigCreateArtikel", item.ConfigCreateArtikel == null ? (object)DBNull.Value : item.ConfigCreateArtikel);
				sqlCommand.Parameters.AddWithValue("ConfigCreateDept", item.ConfigCreateDept == null ? (object)DBNull.Value : item.ConfigCreateDept);
				sqlCommand.Parameters.AddWithValue("ConfigCreateLand", item.ConfigCreateLand == null ? (object)DBNull.Value : item.ConfigCreateLand);
				sqlCommand.Parameters.AddWithValue("ConfigCreateSupplier", item.ConfigCreateSupplier == null ? (object)DBNull.Value : item.ConfigCreateSupplier);
				sqlCommand.Parameters.AddWithValue("ConfigDeleteArtikel", item.ConfigDeleteArtikel == null ? (object)DBNull.Value : item.ConfigDeleteArtikel);
				sqlCommand.Parameters.AddWithValue("ConfigDeleteDept", item.ConfigDeleteDept == null ? (object)DBNull.Value : item.ConfigDeleteDept);
				sqlCommand.Parameters.AddWithValue("ConfigDeleteLand", item.ConfigDeleteLand == null ? (object)DBNull.Value : item.ConfigDeleteLand);
				sqlCommand.Parameters.AddWithValue("ConfigDeleteSupplier", item.ConfigDeleteSupplier == null ? (object)DBNull.Value : item.ConfigDeleteSupplier);
				sqlCommand.Parameters.AddWithValue("ConfigEditArtikel", item.ConfigEditArtikel == null ? (object)DBNull.Value : item.ConfigEditArtikel);
				sqlCommand.Parameters.AddWithValue("ConfigEditDept", item.ConfigEditDept == null ? (object)DBNull.Value : item.ConfigEditDept);
				sqlCommand.Parameters.AddWithValue("ConfigEditLand", item.ConfigEditLand == null ? (object)DBNull.Value : item.ConfigEditLand);
				sqlCommand.Parameters.AddWithValue("ConfigEditSupplier", item.ConfigEditSupplier == null ? (object)DBNull.Value : item.ConfigEditSupplier);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreditManagement", item.CreditManagement);
				sqlCommand.Parameters.AddWithValue("DeleteExternalCommande", item.DeleteExternalCommande == null ? (object)DBNull.Value : item.DeleteExternalCommande);
				sqlCommand.Parameters.AddWithValue("DeleteExternalProject", item.DeleteExternalProject == null ? (object)DBNull.Value : item.DeleteExternalProject);
				sqlCommand.Parameters.AddWithValue("DeleteInternalCommande", item.DeleteInternalCommande == null ? (object)DBNull.Value : item.DeleteInternalCommande);
				sqlCommand.Parameters.AddWithValue("DeleteInternalProject", item.DeleteInternalProject == null ? (object)DBNull.Value : item.DeleteInternalProject);
				sqlCommand.Parameters.AddWithValue("DeleteKontenrahmenAccounting", item.DeleteKontenrahmenAccounting == null ? (object)DBNull.Value : item.DeleteKontenrahmenAccounting);
				sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenKundenAccounting", item.DeleteZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenKundenAccounting);
				sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenLieferantenAccounting", item.DeleteZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenLieferantenAccounting);
				sqlCommand.Parameters.AddWithValue("FinanceOrder", item.FinanceOrder == null ? (object)DBNull.Value : item.FinanceOrder);
				sqlCommand.Parameters.AddWithValue("FinanceProject", item.FinanceProject == null ? (object)DBNull.Value : item.FinanceProject);
				sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("MainAccessProfileId", item.MainAccessProfileId == null ? (object)DBNull.Value : item.MainAccessProfileId);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("Project", item.Project == null ? (object)DBNull.Value : item.Project);
				sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllGroup", item.ProjectExternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectExternalEditAllGroup);
				sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllSite", item.ProjectExternalEditAllSite == null ? (object)DBNull.Value : item.ProjectExternalEditAllSite);
				sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllGroup", item.ProjectExternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectExternalViewAllGroup);
				sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllSite", item.ProjectExternalViewAllSite == null ? (object)DBNull.Value : item.ProjectExternalViewAllSite);
				sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllGroup", item.ProjectInternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectInternalEditAllGroup);
				sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllSite", item.ProjectInternalEditAllSite == null ? (object)DBNull.Value : item.ProjectInternalEditAllSite);
				sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllGroup", item.ProjectInternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectInternalViewAllGroup);
				sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllSite", item.ProjectInternalViewAllSite == null ? (object)DBNull.Value : item.ProjectInternalViewAllSite);
				sqlCommand.Parameters.AddWithValue("Receptions", item.Receptions == null ? (object)DBNull.Value : item.Receptions);
				sqlCommand.Parameters.AddWithValue("ReceptionsEdit", item.ReceptionsEdit == null ? (object)DBNull.Value : item.ReceptionsEdit);
				sqlCommand.Parameters.AddWithValue("ReceptionsView", item.ReceptionsView == null ? (object)DBNull.Value : item.ReceptionsView);
				sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
				sqlCommand.Parameters.AddWithValue("StatisticsViewAll", item.StatisticsViewAll == null ? (object)DBNull.Value : item.StatisticsViewAll);
				sqlCommand.Parameters.AddWithValue("Suppliers", item.Suppliers == null ? (object)DBNull.Value : item.Suppliers);
				sqlCommand.Parameters.AddWithValue("Units", item.Units == null ? (object)DBNull.Value : item.Units);
				sqlCommand.Parameters.AddWithValue("UpdateExternalCommande", item.UpdateExternalCommande == null ? (object)DBNull.Value : item.UpdateExternalCommande);
				sqlCommand.Parameters.AddWithValue("UpdateExternalProject", item.UpdateExternalProject == null ? (object)DBNull.Value : item.UpdateExternalProject);
				sqlCommand.Parameters.AddWithValue("UpdateInternalCommande", item.UpdateInternalCommande == null ? (object)DBNull.Value : item.UpdateInternalCommande);
				sqlCommand.Parameters.AddWithValue("UpdateInternalProject", item.UpdateInternalProject == null ? (object)DBNull.Value : item.UpdateInternalProject);
				sqlCommand.Parameters.AddWithValue("UpdateKontenrahmenAccounting", item.UpdateKontenrahmenAccounting == null ? (object)DBNull.Value : item.UpdateKontenrahmenAccounting);
				sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenKundenAccounting", item.UpdateZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenKundenAccounting);
				sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenLieferantenAccounting", item.UpdateZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenLieferantenAccounting);
				sqlCommand.Parameters.AddWithValue("ViewAusfuhrAccounting", item.ViewAusfuhrAccounting == null ? (object)DBNull.Value : item.ViewAusfuhrAccounting);
				sqlCommand.Parameters.AddWithValue("ViewEinfuhrAccounting", item.ViewEinfuhrAccounting == null ? (object)DBNull.Value : item.ViewEinfuhrAccounting);
				sqlCommand.Parameters.AddWithValue("ViewExternalCommande", item.ViewExternalCommande == null ? (object)DBNull.Value : item.ViewExternalCommande);
				sqlCommand.Parameters.AddWithValue("ViewExternalProject", item.ViewExternalProject == null ? (object)DBNull.Value : item.ViewExternalProject);
				sqlCommand.Parameters.AddWithValue("ViewFactoringRgGutschriftlistAccounting", item.ViewFactoringRgGutschriftlistAccounting == null ? (object)DBNull.Value : item.ViewFactoringRgGutschriftlistAccounting);
				sqlCommand.Parameters.AddWithValue("ViewInternalCommande", item.ViewInternalCommande == null ? (object)DBNull.Value : item.ViewInternalCommande);
				sqlCommand.Parameters.AddWithValue("ViewInternalProject", item.ViewInternalProject == null ? (object)DBNull.Value : item.ViewInternalProject);
				sqlCommand.Parameters.AddWithValue("ViewKontenrahmenAccounting", item.ViewKontenrahmenAccounting == null ? (object)DBNull.Value : item.ViewKontenrahmenAccounting);
				sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting", item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting);
				sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungSkontozahlerAccounting", item.ViewLiquiditatsplanungSkontozahlerAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungSkontozahlerAccounting);
				sqlCommand.Parameters.AddWithValue("ViewRechnungsTransferAccounting", item.ViewRechnungsTransferAccounting == null ? (object)DBNull.Value : item.ViewRechnungsTransferAccounting);
				sqlCommand.Parameters.AddWithValue("ViewRMDCZAccounting", item.ViewRMDCZAccounting == null ? (object)DBNull.Value : item.ViewRMDCZAccounting);
				sqlCommand.Parameters.AddWithValue("ViewStammdatenkontrolleWareneingangeAccounting", item.ViewStammdatenkontrolleWareneingangeAccounting == null ? (object)DBNull.Value : item.ViewStammdatenkontrolleWareneingangeAccounting);
				sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenKundenAccounting", item.ViewZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenKundenAccounting);
				sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenLieferantenAccounting", item.ViewZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenLieferantenAccounting);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 115; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = update(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}

				return results;
			}

			return -1;
		}
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__FNC_AccessProfile] SET "

							+ "[AccessProfileName]=@AccessProfileName" + i + ","
							+ "[Accounting]=@Accounting" + i + ","
							+ "[AddExternalCommande]=@AddExternalCommande" + i + ","
							+ "[AddExternalProject]=@AddExternalProject" + i + ","
							+ "[AddInternalCommande]=@AddInternalCommande" + i + ","
							+ "[AddInternalProject]=@AddInternalProject" + i + ","
							+ "[AddKontenrahmenAccounting]=@AddKontenrahmenAccounting" + i + ","
							+ "[Administration]=@Administration" + i + ","
							+ "[AdministrationAccessProfiles]=@AdministrationAccessProfiles" + i + ","
							+ "[AdministrationAccessProfilesUpdate]=@AdministrationAccessProfilesUpdate" + i + ","
							+ "[AdministrationUser]=@AdministrationUser" + i + ","
							+ "[AdministrationUserUpdate]=@AdministrationUserUpdate" + i + ","
							+ "[Article]=@Article" + i + ","
							+ "[Assign]=@Assign" + i + ","
							+ "[AssignAllDepartments]=@AssignAllDepartments" + i + ","
							+ "[AssignAllEmployees]=@AssignAllEmployees" + i + ","
							+ "[AssignAllSites]=@AssignAllSites" + i + ","
							+ "[AssignCreateDept]=@AssignCreateDept" + i + ","
							+ "[AssignCreateLand]=@AssignCreateLand" + i + ","
							+ "[AssignCreateUser]=@AssignCreateUser" + i + ","
							+ "[AssignDeleteDept]=@AssignDeleteDept" + i + ","
							+ "[AssignDeleteLand]=@AssignDeleteLand" + i + ","
							+ "[AssignDeleteUser]=@AssignDeleteUser" + i + ","
							+ "[AssignEditDept]=@AssignEditDept" + i + ","
							+ "[AssignEditLand]=@AssignEditLand" + i + ","
							+ "[AssignEditUser]=@AssignEditUser" + i + ","
							+ "[AssignViewDept]=@AssignViewDept" + i + ","
							+ "[AssignViewLand]=@AssignViewLand" + i + ","
							+ "[AssignViewUser]=@AssignViewUser" + i + ","
							+ "[Budget]=@Budget" + i + ","
							+ "[CashLiquidity]=@CashLiquidity" + i + ","
							+ "[Commande]=@Commande" + i + ","
							+ "[CommandeExternalEditAllGroup]=@CommandeExternalEditAllGroup" + i + ","
							+ "[CommandeExternalEditAllSite]=@CommandeExternalEditAllSite" + i + ","
							+ "[CommandeExternalViewAllGroup]=@CommandeExternalViewAllGroup" + i + ","
							+ "[CommandeExternalViewAllSite]=@CommandeExternalViewAllSite" + i + ","
							+ "[CommandeExternalViewInvoice]=@CommandeExternalViewInvoice" + i + ","
							+ "[CommandeExternalViewInvoiceAllGroup]=@CommandeExternalViewInvoiceAllGroup" + i + ","
							+ "[CommandeInternalEditAllDepartment]=@CommandeInternalEditAllDepartment" + i + ","
							+ "[CommandeInternalEditAllGroup]=@CommandeInternalEditAllGroup" + i + ","
							+ "[CommandeInternalEditAllSite]=@CommandeInternalEditAllSite" + i + ","
							+ "[CommandeInternalViewAllDepartment]=@CommandeInternalViewAllDepartment" + i + ","
							+ "[CommandeInternalViewAllGroup]=@CommandeInternalViewAllGroup" + i + ","
							+ "[CommandeInternalViewAllSite]=@CommandeInternalViewAllSite" + i + ","
							+ "[CommandeInternalViewInvoice]=@CommandeInternalViewInvoice" + i + ","
							+ "[CommandeInternalViewInvoiceAllGroup]=@CommandeInternalViewInvoiceAllGroup" + i + ","
							+ "[Config]=@Config" + i + ","
							+ "[ConfigCreateArtikel]=@ConfigCreateArtikel" + i + ","
							+ "[ConfigCreateDept]=@ConfigCreateDept" + i + ","
							+ "[ConfigCreateLand]=@ConfigCreateLand" + i + ","
							+ "[ConfigCreateSupplier]=@ConfigCreateSupplier" + i + ","
							+ "[ConfigDeleteArtikel]=@ConfigDeleteArtikel" + i + ","
							+ "[ConfigDeleteDept]=@ConfigDeleteDept" + i + ","
							+ "[ConfigDeleteLand]=@ConfigDeleteLand" + i + ","
							+ "[ConfigDeleteSupplier]=@ConfigDeleteSupplier" + i + ","
							+ "[ConfigEditArtikel]=@ConfigEditArtikel" + i + ","
							+ "[ConfigEditDept]=@ConfigEditDept" + i + ","
							+ "[ConfigEditLand]=@ConfigEditLand" + i + ","
							+ "[ConfigEditSupplier]=@ConfigEditSupplier" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[CreditManagement]=@CreditManagement" + i + ","
							+ "[DeleteExternalCommande]=@DeleteExternalCommande" + i + ","
							+ "[DeleteExternalProject]=@DeleteExternalProject" + i + ","
							+ "[DeleteInternalCommande]=@DeleteInternalCommande" + i + ","
							+ "[DeleteInternalProject]=@DeleteInternalProject" + i + ","
							+ "[DeleteKontenrahmenAccounting]=@DeleteKontenrahmenAccounting" + i + ","
							+ "[DeleteZahlungskonditionenKundenAccounting]=@DeleteZahlungskonditionenKundenAccounting" + i + ","
							+ "[DeleteZahlungskonditionenLieferantenAccounting]=@DeleteZahlungskonditionenLieferantenAccounting" + i + ","
							+ "[FinanceOrder]=@FinanceOrder" + i + ","
							+ "[FinanceProject]=@FinanceProject" + i + ","
							+ "[IsDefault]=@IsDefault" + i + ","
							+ "[LastEditTime]=@LastEditTime" + i + ","
							+ "[LastEditUserId]=@LastEditUserId" + i + ","
							+ "[MainAccessProfileId]=@MainAccessProfileId" + i + ","
							+ "[ModuleActivated]=@ModuleActivated" + i + ","
							+ "[Project]=@Project" + i + ","
							+ "[ProjectExternalEditAllGroup]=@ProjectExternalEditAllGroup" + i + ","
							+ "[ProjectExternalEditAllSite]=@ProjectExternalEditAllSite" + i + ","
							+ "[ProjectExternalViewAllGroup]=@ProjectExternalViewAllGroup" + i + ","
							+ "[ProjectExternalViewAllSite]=@ProjectExternalViewAllSite" + i + ","
							+ "[ProjectInternalEditAllGroup]=@ProjectInternalEditAllGroup" + i + ","
							+ "[ProjectInternalEditAllSite]=@ProjectInternalEditAllSite" + i + ","
							+ "[ProjectInternalViewAllGroup]=@ProjectInternalViewAllGroup" + i + ","
							+ "[ProjectInternalViewAllSite]=@ProjectInternalViewAllSite" + i + ","
							+ "[Receptions]=@Receptions" + i + ","
							+ "[ReceptionsEdit]=@ReceptionsEdit" + i + ","
							+ "[ReceptionsView]=@ReceptionsView" + i + ","
							+ "[Statistics]=@Statistics" + i + ","
							+ "[StatisticsViewAll]=@StatisticsViewAll" + i + ","
							+ "[Suppliers]=@Suppliers" + i + ","
							+ "[Units]=@Units" + i + ","
							+ "[UpdateExternalCommande]=@UpdateExternalCommande" + i + ","
							+ "[UpdateExternalProject]=@UpdateExternalProject" + i + ","
							+ "[UpdateInternalCommande]=@UpdateInternalCommande" + i + ","
							+ "[UpdateInternalProject]=@UpdateInternalProject" + i + ","
							+ "[UpdateKontenrahmenAccounting]=@UpdateKontenrahmenAccounting" + i + ","
							+ "[UpdateZahlungskonditionenKundenAccounting]=@UpdateZahlungskonditionenKundenAccounting" + i + ","
							+ "[UpdateZahlungskonditionenLieferantenAccounting]=@UpdateZahlungskonditionenLieferantenAccounting" + i + ","
							+ "[ViewAusfuhrAccounting]=@ViewAusfuhrAccounting" + i + ","
							+ "[ViewEinfuhrAccounting]=@ViewEinfuhrAccounting" + i + ","
							+ "[ViewExternalCommande]=@ViewExternalCommande" + i + ","
							+ "[ViewExternalProject]=@ViewExternalProject" + i + ","
							+ "[ViewFactoringRgGutschriftlistAccounting]=@ViewFactoringRgGutschriftlistAccounting" + i + ","
							+ "[ViewInternalCommande]=@ViewInternalCommande" + i + ","
							+ "[ViewInternalProject]=@ViewInternalProject" + i + ","
							+ "[ViewKontenrahmenAccounting]=@ViewKontenrahmenAccounting" + i + ","
							+ "[ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting]=@ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting" + i + ","
							+ "[ViewLiquiditatsplanungSkontozahlerAccounting]=@ViewLiquiditatsplanungSkontozahlerAccounting" + i + ","
							+ "[ViewRechnungsTransferAccounting]=@ViewRechnungsTransferAccounting" + i + ","
							+ "[ViewRMDCZAccounting]=@ViewRMDCZAccounting" + i + ","
							+ "[ViewStammdatenkontrolleWareneingangeAccounting]=@ViewStammdatenkontrolleWareneingangeAccounting" + i + ","
							+ "[ViewZahlungskonditionenKundenAccounting]=@ViewZahlungskonditionenKundenAccounting" + i + ","
							+ "[ViewZahlungskonditionenLieferantenAccounting]=@ViewZahlungskonditionenLieferantenAccounting" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("Accounting" + i, item.Accounting == null ? (object)DBNull.Value : item.Accounting);
						sqlCommand.Parameters.AddWithValue("AddExternalCommande" + i, item.AddExternalCommande == null ? (object)DBNull.Value : item.AddExternalCommande);
						sqlCommand.Parameters.AddWithValue("AddExternalProject" + i, item.AddExternalProject == null ? (object)DBNull.Value : item.AddExternalProject);
						sqlCommand.Parameters.AddWithValue("AddInternalCommande" + i, item.AddInternalCommande == null ? (object)DBNull.Value : item.AddInternalCommande);
						sqlCommand.Parameters.AddWithValue("AddInternalProject" + i, item.AddInternalProject == null ? (object)DBNull.Value : item.AddInternalProject);
						sqlCommand.Parameters.AddWithValue("AddKontenrahmenAccounting" + i, item.AddKontenrahmenAccounting == null ? (object)DBNull.Value : item.AddKontenrahmenAccounting);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
						sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles" + i, item.AdministrationAccessProfiles == null ? (object)DBNull.Value : item.AdministrationAccessProfiles);
						sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate" + i, item.AdministrationAccessProfilesUpdate == null ? (object)DBNull.Value : item.AdministrationAccessProfilesUpdate);
						sqlCommand.Parameters.AddWithValue("AdministrationUser" + i, item.AdministrationUser == null ? (object)DBNull.Value : item.AdministrationUser);
						sqlCommand.Parameters.AddWithValue("AdministrationUserUpdate" + i, item.AdministrationUserUpdate == null ? (object)DBNull.Value : item.AdministrationUserUpdate);
						sqlCommand.Parameters.AddWithValue("Article" + i, item.Article == null ? (object)DBNull.Value : item.Article);
						sqlCommand.Parameters.AddWithValue("Assign" + i, item.Assign == null ? (object)DBNull.Value : item.Assign);
						sqlCommand.Parameters.AddWithValue("AssignAllDepartments" + i, item.AssignAllDepartments == null ? (object)DBNull.Value : item.AssignAllDepartments);
						sqlCommand.Parameters.AddWithValue("AssignAllEmployees" + i, item.AssignAllEmployees == null ? (object)DBNull.Value : item.AssignAllEmployees);
						sqlCommand.Parameters.AddWithValue("AssignAllSites" + i, item.AssignAllSites == null ? (object)DBNull.Value : item.AssignAllSites);
						sqlCommand.Parameters.AddWithValue("AssignCreateDept" + i, item.AssignCreateDept == null ? (object)DBNull.Value : item.AssignCreateDept);
						sqlCommand.Parameters.AddWithValue("AssignCreateLand" + i, item.AssignCreateLand == null ? (object)DBNull.Value : item.AssignCreateLand);
						sqlCommand.Parameters.AddWithValue("AssignCreateUser" + i, item.AssignCreateUser == null ? (object)DBNull.Value : item.AssignCreateUser);
						sqlCommand.Parameters.AddWithValue("AssignDeleteDept" + i, item.AssignDeleteDept == null ? (object)DBNull.Value : item.AssignDeleteDept);
						sqlCommand.Parameters.AddWithValue("AssignDeleteLand" + i, item.AssignDeleteLand == null ? (object)DBNull.Value : item.AssignDeleteLand);
						sqlCommand.Parameters.AddWithValue("AssignDeleteUser" + i, item.AssignDeleteUser == null ? (object)DBNull.Value : item.AssignDeleteUser);
						sqlCommand.Parameters.AddWithValue("AssignEditDept" + i, item.AssignEditDept == null ? (object)DBNull.Value : item.AssignEditDept);
						sqlCommand.Parameters.AddWithValue("AssignEditLand" + i, item.AssignEditLand == null ? (object)DBNull.Value : item.AssignEditLand);
						sqlCommand.Parameters.AddWithValue("AssignEditUser" + i, item.AssignEditUser == null ? (object)DBNull.Value : item.AssignEditUser);
						sqlCommand.Parameters.AddWithValue("AssignViewDept" + i, item.AssignViewDept == null ? (object)DBNull.Value : item.AssignViewDept);
						sqlCommand.Parameters.AddWithValue("AssignViewLand" + i, item.AssignViewLand == null ? (object)DBNull.Value : item.AssignViewLand);
						sqlCommand.Parameters.AddWithValue("AssignViewUser" + i, item.AssignViewUser == null ? (object)DBNull.Value : item.AssignViewUser);
						sqlCommand.Parameters.AddWithValue("Budget" + i, item.Budget);
						sqlCommand.Parameters.AddWithValue("CashLiquidity" + i, item.CashLiquidity);
						sqlCommand.Parameters.AddWithValue("Commande" + i, item.Commande == null ? (object)DBNull.Value : item.Commande);
						sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllGroup" + i, item.CommandeExternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeExternalEditAllGroup);
						sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllSite" + i, item.CommandeExternalEditAllSite == null ? (object)DBNull.Value : item.CommandeExternalEditAllSite);
						sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllGroup" + i, item.CommandeExternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewAllGroup);
						sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllSite" + i, item.CommandeExternalViewAllSite == null ? (object)DBNull.Value : item.CommandeExternalViewAllSite);
						sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoice" + i, item.CommandeExternalViewInvoice == null ? (object)DBNull.Value : item.CommandeExternalViewInvoice);
						sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoiceAllGroup" + i, item.CommandeExternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewInvoiceAllGroup);
						sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllDepartment" + i, item.CommandeInternalEditAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalEditAllDepartment);
						sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllGroup" + i, item.CommandeInternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeInternalEditAllGroup);
						sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllSite" + i, item.CommandeInternalEditAllSite == null ? (object)DBNull.Value : item.CommandeInternalEditAllSite);
						sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllDepartment" + i, item.CommandeInternalViewAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalViewAllDepartment);
						sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllGroup" + i, item.CommandeInternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewAllGroup);
						sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllSite" + i, item.CommandeInternalViewAllSite == null ? (object)DBNull.Value : item.CommandeInternalViewAllSite);
						sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoice" + i, item.CommandeInternalViewInvoice == null ? (object)DBNull.Value : item.CommandeInternalViewInvoice);
						sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoiceAllGroup" + i, item.CommandeInternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewInvoiceAllGroup);
						sqlCommand.Parameters.AddWithValue("Config" + i, item.Config == null ? (object)DBNull.Value : item.Config);
						sqlCommand.Parameters.AddWithValue("ConfigCreateArtikel" + i, item.ConfigCreateArtikel == null ? (object)DBNull.Value : item.ConfigCreateArtikel);
						sqlCommand.Parameters.AddWithValue("ConfigCreateDept" + i, item.ConfigCreateDept == null ? (object)DBNull.Value : item.ConfigCreateDept);
						sqlCommand.Parameters.AddWithValue("ConfigCreateLand" + i, item.ConfigCreateLand == null ? (object)DBNull.Value : item.ConfigCreateLand);
						sqlCommand.Parameters.AddWithValue("ConfigCreateSupplier" + i, item.ConfigCreateSupplier == null ? (object)DBNull.Value : item.ConfigCreateSupplier);
						sqlCommand.Parameters.AddWithValue("ConfigDeleteArtikel" + i, item.ConfigDeleteArtikel == null ? (object)DBNull.Value : item.ConfigDeleteArtikel);
						sqlCommand.Parameters.AddWithValue("ConfigDeleteDept" + i, item.ConfigDeleteDept == null ? (object)DBNull.Value : item.ConfigDeleteDept);
						sqlCommand.Parameters.AddWithValue("ConfigDeleteLand" + i, item.ConfigDeleteLand == null ? (object)DBNull.Value : item.ConfigDeleteLand);
						sqlCommand.Parameters.AddWithValue("ConfigDeleteSupplier" + i, item.ConfigDeleteSupplier == null ? (object)DBNull.Value : item.ConfigDeleteSupplier);
						sqlCommand.Parameters.AddWithValue("ConfigEditArtikel" + i, item.ConfigEditArtikel == null ? (object)DBNull.Value : item.ConfigEditArtikel);
						sqlCommand.Parameters.AddWithValue("ConfigEditDept" + i, item.ConfigEditDept == null ? (object)DBNull.Value : item.ConfigEditDept);
						sqlCommand.Parameters.AddWithValue("ConfigEditLand" + i, item.ConfigEditLand == null ? (object)DBNull.Value : item.ConfigEditLand);
						sqlCommand.Parameters.AddWithValue("ConfigEditSupplier" + i, item.ConfigEditSupplier == null ? (object)DBNull.Value : item.ConfigEditSupplier);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreditManagement" + i, item.CreditManagement);
						sqlCommand.Parameters.AddWithValue("DeleteExternalCommande" + i, item.DeleteExternalCommande == null ? (object)DBNull.Value : item.DeleteExternalCommande);
						sqlCommand.Parameters.AddWithValue("DeleteExternalProject" + i, item.DeleteExternalProject == null ? (object)DBNull.Value : item.DeleteExternalProject);
						sqlCommand.Parameters.AddWithValue("DeleteInternalCommande" + i, item.DeleteInternalCommande == null ? (object)DBNull.Value : item.DeleteInternalCommande);
						sqlCommand.Parameters.AddWithValue("DeleteInternalProject" + i, item.DeleteInternalProject == null ? (object)DBNull.Value : item.DeleteInternalProject);
						sqlCommand.Parameters.AddWithValue("DeleteKontenrahmenAccounting" + i, item.DeleteKontenrahmenAccounting == null ? (object)DBNull.Value : item.DeleteKontenrahmenAccounting);
						sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenKundenAccounting" + i, item.DeleteZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenKundenAccounting);
						sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenLieferantenAccounting" + i, item.DeleteZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenLieferantenAccounting);
						sqlCommand.Parameters.AddWithValue("FinanceOrder" + i, item.FinanceOrder == null ? (object)DBNull.Value : item.FinanceOrder);
						sqlCommand.Parameters.AddWithValue("FinanceProject" + i, item.FinanceProject == null ? (object)DBNull.Value : item.FinanceProject);
						sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("MainAccessProfileId" + i, item.MainAccessProfileId == null ? (object)DBNull.Value : item.MainAccessProfileId);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("Project" + i, item.Project == null ? (object)DBNull.Value : item.Project);
						sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllGroup" + i, item.ProjectExternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectExternalEditAllGroup);
						sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllSite" + i, item.ProjectExternalEditAllSite == null ? (object)DBNull.Value : item.ProjectExternalEditAllSite);
						sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllGroup" + i, item.ProjectExternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectExternalViewAllGroup);
						sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllSite" + i, item.ProjectExternalViewAllSite == null ? (object)DBNull.Value : item.ProjectExternalViewAllSite);
						sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllGroup" + i, item.ProjectInternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectInternalEditAllGroup);
						sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllSite" + i, item.ProjectInternalEditAllSite == null ? (object)DBNull.Value : item.ProjectInternalEditAllSite);
						sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllGroup" + i, item.ProjectInternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectInternalViewAllGroup);
						sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllSite" + i, item.ProjectInternalViewAllSite == null ? (object)DBNull.Value : item.ProjectInternalViewAllSite);
						sqlCommand.Parameters.AddWithValue("Receptions" + i, item.Receptions == null ? (object)DBNull.Value : item.Receptions);
						sqlCommand.Parameters.AddWithValue("ReceptionsEdit" + i, item.ReceptionsEdit == null ? (object)DBNull.Value : item.ReceptionsEdit);
						sqlCommand.Parameters.AddWithValue("ReceptionsView" + i, item.ReceptionsView == null ? (object)DBNull.Value : item.ReceptionsView);
						sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
						sqlCommand.Parameters.AddWithValue("StatisticsViewAll" + i, item.StatisticsViewAll == null ? (object)DBNull.Value : item.StatisticsViewAll);
						sqlCommand.Parameters.AddWithValue("Suppliers" + i, item.Suppliers == null ? (object)DBNull.Value : item.Suppliers);
						sqlCommand.Parameters.AddWithValue("Units" + i, item.Units == null ? (object)DBNull.Value : item.Units);
						sqlCommand.Parameters.AddWithValue("UpdateExternalCommande" + i, item.UpdateExternalCommande == null ? (object)DBNull.Value : item.UpdateExternalCommande);
						sqlCommand.Parameters.AddWithValue("UpdateExternalProject" + i, item.UpdateExternalProject == null ? (object)DBNull.Value : item.UpdateExternalProject);
						sqlCommand.Parameters.AddWithValue("UpdateInternalCommande" + i, item.UpdateInternalCommande == null ? (object)DBNull.Value : item.UpdateInternalCommande);
						sqlCommand.Parameters.AddWithValue("UpdateInternalProject" + i, item.UpdateInternalProject == null ? (object)DBNull.Value : item.UpdateInternalProject);
						sqlCommand.Parameters.AddWithValue("UpdateKontenrahmenAccounting" + i, item.UpdateKontenrahmenAccounting == null ? (object)DBNull.Value : item.UpdateKontenrahmenAccounting);
						sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenKundenAccounting" + i, item.UpdateZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenKundenAccounting);
						sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenLieferantenAccounting" + i, item.UpdateZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenLieferantenAccounting);
						sqlCommand.Parameters.AddWithValue("ViewAusfuhrAccounting" + i, item.ViewAusfuhrAccounting == null ? (object)DBNull.Value : item.ViewAusfuhrAccounting);
						sqlCommand.Parameters.AddWithValue("ViewEinfuhrAccounting" + i, item.ViewEinfuhrAccounting == null ? (object)DBNull.Value : item.ViewEinfuhrAccounting);
						sqlCommand.Parameters.AddWithValue("ViewExternalCommande" + i, item.ViewExternalCommande == null ? (object)DBNull.Value : item.ViewExternalCommande);
						sqlCommand.Parameters.AddWithValue("ViewExternalProject" + i, item.ViewExternalProject == null ? (object)DBNull.Value : item.ViewExternalProject);
						sqlCommand.Parameters.AddWithValue("ViewFactoringRgGutschriftlistAccounting" + i, item.ViewFactoringRgGutschriftlistAccounting == null ? (object)DBNull.Value : item.ViewFactoringRgGutschriftlistAccounting);
						sqlCommand.Parameters.AddWithValue("ViewInternalCommande" + i, item.ViewInternalCommande == null ? (object)DBNull.Value : item.ViewInternalCommande);
						sqlCommand.Parameters.AddWithValue("ViewInternalProject" + i, item.ViewInternalProject == null ? (object)DBNull.Value : item.ViewInternalProject);
						sqlCommand.Parameters.AddWithValue("ViewKontenrahmenAccounting" + i, item.ViewKontenrahmenAccounting == null ? (object)DBNull.Value : item.ViewKontenrahmenAccounting);
						sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting" + i, item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting);
						sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungSkontozahlerAccounting" + i, item.ViewLiquiditatsplanungSkontozahlerAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungSkontozahlerAccounting);
						sqlCommand.Parameters.AddWithValue("ViewRechnungsTransferAccounting" + i, item.ViewRechnungsTransferAccounting == null ? (object)DBNull.Value : item.ViewRechnungsTransferAccounting);
						sqlCommand.Parameters.AddWithValue("ViewRMDCZAccounting" + i, item.ViewRMDCZAccounting == null ? (object)DBNull.Value : item.ViewRMDCZAccounting);
						sqlCommand.Parameters.AddWithValue("ViewStammdatenkontrolleWareneingangeAccounting" + i, item.ViewStammdatenkontrolleWareneingangeAccounting == null ? (object)DBNull.Value : item.ViewStammdatenkontrolleWareneingangeAccounting);
						sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenKundenAccounting" + i, item.ViewZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenKundenAccounting);
						sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenLieferantenAccounting" + i, item.ViewZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenLieferantenAccounting);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					string query = "DELETE FROM [__FNC_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_AccessProfile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_AccessProfile]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [__FNC_AccessProfile] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__FNC_AccessProfile] ([AccessProfileName],[Accounting],[AddExternalCommande],[AddExternalProject],[AddInternalCommande],[AddInternalProject],[AddKontenrahmenAccounting],[Administration],[AdministrationAccessProfiles],[AdministrationAccessProfilesUpdate],[AdministrationUser],[AdministrationUserUpdate],[Article],[Assign],[AssignAllDepartments],[AssignAllEmployees],[AssignAllSites],[AssignCreateDept],[AssignCreateLand],[AssignCreateUser],[AssignDeleteDept],[AssignDeleteLand],[AssignDeleteUser],[AssignEditDept],[AssignEditLand],[AssignEditUser],[AssignViewDept],[AssignViewLand],[AssignViewUser],[Budget],[CashLiquidity],[Commande],[CommandeExternalEditAllGroup],[CommandeExternalEditAllSite],[CommandeExternalViewAllGroup],[CommandeExternalViewAllSite],[CommandeExternalViewInvoice],[CommandeExternalViewInvoiceAllGroup],[CommandeInternalEditAllDepartment],[CommandeInternalEditAllGroup],[CommandeInternalEditAllSite],[CommandeInternalViewAllDepartment],[CommandeInternalViewAllGroup],[CommandeInternalViewAllSite],[CommandeInternalViewInvoice],[CommandeInternalViewInvoiceAllGroup],[Config],[ConfigCreateArtikel],[ConfigCreateDept],[ConfigCreateLand],[ConfigCreateSupplier],[ConfigDeleteArtikel],[ConfigDeleteDept],[ConfigDeleteLand],[ConfigDeleteSupplier],[ConfigEditArtikel],[ConfigEditDept],[ConfigEditLand],[ConfigEditSupplier],[CreationTime],[CreationUserId],[CreditManagement],[DeleteExternalCommande],[DeleteExternalProject],[DeleteInternalCommande],[DeleteInternalProject],[DeleteKontenrahmenAccounting],[DeleteZahlungskonditionenKundenAccounting],[DeleteZahlungskonditionenLieferantenAccounting],[FinanceOrder],[FinanceProject],[IsDefault],[LastEditTime],[LastEditUserId],[MainAccessProfileId],[ModuleActivated],[Project],[ProjectExternalEditAllGroup],[ProjectExternalEditAllSite],[ProjectExternalViewAllGroup],[ProjectExternalViewAllSite],[ProjectInternalEditAllGroup],[ProjectInternalEditAllSite],[ProjectInternalViewAllGroup],[ProjectInternalViewAllSite],[Receptions],[ReceptionsEdit],[ReceptionsView],[Statistics],[StatisticsViewAll],[Suppliers],[Units],[UpdateExternalCommande],[UpdateExternalProject],[UpdateInternalCommande],[UpdateInternalProject],[UpdateKontenrahmenAccounting],[UpdateZahlungskonditionenKundenAccounting],[UpdateZahlungskonditionenLieferantenAccounting],[ViewAusfuhrAccounting],[ViewEinfuhrAccounting],[ViewExternalCommande],[ViewExternalProject],[ViewFactoringRgGutschriftlistAccounting],[ViewInternalCommande],[ViewInternalProject],[ViewKontenrahmenAccounting],[ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting],[ViewLiquiditatsplanungSkontozahlerAccounting],[ViewRechnungsTransferAccounting],[ViewRMDCZAccounting],[ViewStammdatenkontrolleWareneingangeAccounting],[ViewZahlungskonditionenKundenAccounting],[ViewZahlungskonditionenLieferantenAccounting]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileName,@Accounting,@AddExternalCommande,@AddExternalProject,@AddInternalCommande,@AddInternalProject,@AddKontenrahmenAccounting,@Administration,@AdministrationAccessProfiles,@AdministrationAccessProfilesUpdate,@AdministrationUser,@AdministrationUserUpdate,@Article,@Assign,@AssignAllDepartments,@AssignAllEmployees,@AssignAllSites,@AssignCreateDept,@AssignCreateLand,@AssignCreateUser,@AssignDeleteDept,@AssignDeleteLand,@AssignDeleteUser,@AssignEditDept,@AssignEditLand,@AssignEditUser,@AssignViewDept,@AssignViewLand,@AssignViewUser,@Budget,@CashLiquidity,@Commande,@CommandeExternalEditAllGroup,@CommandeExternalEditAllSite,@CommandeExternalViewAllGroup,@CommandeExternalViewAllSite,@CommandeExternalViewInvoice,@CommandeExternalViewInvoiceAllGroup,@CommandeInternalEditAllDepartment,@CommandeInternalEditAllGroup,@CommandeInternalEditAllSite,@CommandeInternalViewAllDepartment,@CommandeInternalViewAllGroup,@CommandeInternalViewAllSite,@CommandeInternalViewInvoice,@CommandeInternalViewInvoiceAllGroup,@Config,@ConfigCreateArtikel,@ConfigCreateDept,@ConfigCreateLand,@ConfigCreateSupplier,@ConfigDeleteArtikel,@ConfigDeleteDept,@ConfigDeleteLand,@ConfigDeleteSupplier,@ConfigEditArtikel,@ConfigEditDept,@ConfigEditLand,@ConfigEditSupplier,@CreationTime,@CreationUserId,@CreditManagement,@DeleteExternalCommande,@DeleteExternalProject,@DeleteInternalCommande,@DeleteInternalProject,@DeleteKontenrahmenAccounting,@DeleteZahlungskonditionenKundenAccounting,@DeleteZahlungskonditionenLieferantenAccounting,@FinanceOrder,@FinanceProject,@IsDefault,@LastEditTime,@LastEditUserId,@MainAccessProfileId,@ModuleActivated,@Project,@ProjectExternalEditAllGroup,@ProjectExternalEditAllSite,@ProjectExternalViewAllGroup,@ProjectExternalViewAllSite,@ProjectInternalEditAllGroup,@ProjectInternalEditAllSite,@ProjectInternalViewAllGroup,@ProjectInternalViewAllSite,@Receptions,@ReceptionsEdit,@ReceptionsView,@Statistics,@StatisticsViewAll,@Suppliers,@Units,@UpdateExternalCommande,@UpdateExternalProject,@UpdateInternalCommande,@UpdateInternalProject,@UpdateKontenrahmenAccounting,@UpdateZahlungskonditionenKundenAccounting,@UpdateZahlungskonditionenLieferantenAccounting,@ViewAusfuhrAccounting,@ViewEinfuhrAccounting,@ViewExternalCommande,@ViewExternalProject,@ViewFactoringRgGutschriftlistAccounting,@ViewInternalCommande,@ViewInternalProject,@ViewKontenrahmenAccounting,@ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting,@ViewLiquiditatsplanungSkontozahlerAccounting,@ViewRechnungsTransferAccounting,@ViewRMDCZAccounting,@ViewStammdatenkontrolleWareneingangeAccounting,@ViewZahlungskonditionenKundenAccounting,@ViewZahlungskonditionenLieferantenAccounting); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("Accounting", item.Accounting == null ? (object)DBNull.Value : item.Accounting);
			sqlCommand.Parameters.AddWithValue("AddExternalCommande", item.AddExternalCommande == null ? (object)DBNull.Value : item.AddExternalCommande);
			sqlCommand.Parameters.AddWithValue("AddExternalProject", item.AddExternalProject == null ? (object)DBNull.Value : item.AddExternalProject);
			sqlCommand.Parameters.AddWithValue("AddInternalCommande", item.AddInternalCommande == null ? (object)DBNull.Value : item.AddInternalCommande);
			sqlCommand.Parameters.AddWithValue("AddInternalProject", item.AddInternalProject == null ? (object)DBNull.Value : item.AddInternalProject);
			sqlCommand.Parameters.AddWithValue("AddKontenrahmenAccounting", item.AddKontenrahmenAccounting == null ? (object)DBNull.Value : item.AddKontenrahmenAccounting);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
			sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles", item.AdministrationAccessProfiles == null ? (object)DBNull.Value : item.AdministrationAccessProfiles);
			sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate", item.AdministrationAccessProfilesUpdate == null ? (object)DBNull.Value : item.AdministrationAccessProfilesUpdate);
			sqlCommand.Parameters.AddWithValue("AdministrationUser", item.AdministrationUser == null ? (object)DBNull.Value : item.AdministrationUser);
			sqlCommand.Parameters.AddWithValue("AdministrationUserUpdate", item.AdministrationUserUpdate == null ? (object)DBNull.Value : item.AdministrationUserUpdate);
			sqlCommand.Parameters.AddWithValue("Article", item.Article == null ? (object)DBNull.Value : item.Article);
			sqlCommand.Parameters.AddWithValue("Assign", item.Assign == null ? (object)DBNull.Value : item.Assign);
			sqlCommand.Parameters.AddWithValue("AssignAllDepartments", item.AssignAllDepartments == null ? (object)DBNull.Value : item.AssignAllDepartments);
			sqlCommand.Parameters.AddWithValue("AssignAllEmployees", item.AssignAllEmployees == null ? (object)DBNull.Value : item.AssignAllEmployees);
			sqlCommand.Parameters.AddWithValue("AssignAllSites", item.AssignAllSites == null ? (object)DBNull.Value : item.AssignAllSites);
			sqlCommand.Parameters.AddWithValue("AssignCreateDept", item.AssignCreateDept == null ? (object)DBNull.Value : item.AssignCreateDept);
			sqlCommand.Parameters.AddWithValue("AssignCreateLand", item.AssignCreateLand == null ? (object)DBNull.Value : item.AssignCreateLand);
			sqlCommand.Parameters.AddWithValue("AssignCreateUser", item.AssignCreateUser == null ? (object)DBNull.Value : item.AssignCreateUser);
			sqlCommand.Parameters.AddWithValue("AssignDeleteDept", item.AssignDeleteDept == null ? (object)DBNull.Value : item.AssignDeleteDept);
			sqlCommand.Parameters.AddWithValue("AssignDeleteLand", item.AssignDeleteLand == null ? (object)DBNull.Value : item.AssignDeleteLand);
			sqlCommand.Parameters.AddWithValue("AssignDeleteUser", item.AssignDeleteUser == null ? (object)DBNull.Value : item.AssignDeleteUser);
			sqlCommand.Parameters.AddWithValue("AssignEditDept", item.AssignEditDept == null ? (object)DBNull.Value : item.AssignEditDept);
			sqlCommand.Parameters.AddWithValue("AssignEditLand", item.AssignEditLand == null ? (object)DBNull.Value : item.AssignEditLand);
			sqlCommand.Parameters.AddWithValue("AssignEditUser", item.AssignEditUser == null ? (object)DBNull.Value : item.AssignEditUser);
			sqlCommand.Parameters.AddWithValue("AssignViewDept", item.AssignViewDept == null ? (object)DBNull.Value : item.AssignViewDept);
			sqlCommand.Parameters.AddWithValue("AssignViewLand", item.AssignViewLand == null ? (object)DBNull.Value : item.AssignViewLand);
			sqlCommand.Parameters.AddWithValue("AssignViewUser", item.AssignViewUser == null ? (object)DBNull.Value : item.AssignViewUser);
			sqlCommand.Parameters.AddWithValue("Budget", item.Budget);
			sqlCommand.Parameters.AddWithValue("CashLiquidity", item.CashLiquidity);
			sqlCommand.Parameters.AddWithValue("Commande", item.Commande == null ? (object)DBNull.Value : item.Commande);
			sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllGroup", item.CommandeExternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeExternalEditAllGroup);
			sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllSite", item.CommandeExternalEditAllSite == null ? (object)DBNull.Value : item.CommandeExternalEditAllSite);
			sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllGroup", item.CommandeExternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewAllGroup);
			sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllSite", item.CommandeExternalViewAllSite == null ? (object)DBNull.Value : item.CommandeExternalViewAllSite);
			sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoice", item.CommandeExternalViewInvoice == null ? (object)DBNull.Value : item.CommandeExternalViewInvoice);
			sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoiceAllGroup", item.CommandeExternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewInvoiceAllGroup);
			sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllDepartment", item.CommandeInternalEditAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalEditAllDepartment);
			sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllGroup", item.CommandeInternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeInternalEditAllGroup);
			sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllSite", item.CommandeInternalEditAllSite == null ? (object)DBNull.Value : item.CommandeInternalEditAllSite);
			sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllDepartment", item.CommandeInternalViewAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalViewAllDepartment);
			sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllGroup", item.CommandeInternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewAllGroup);
			sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllSite", item.CommandeInternalViewAllSite == null ? (object)DBNull.Value : item.CommandeInternalViewAllSite);
			sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoice", item.CommandeInternalViewInvoice == null ? (object)DBNull.Value : item.CommandeInternalViewInvoice);
			sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoiceAllGroup", item.CommandeInternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewInvoiceAllGroup);
			sqlCommand.Parameters.AddWithValue("Config", item.Config == null ? (object)DBNull.Value : item.Config);
			sqlCommand.Parameters.AddWithValue("ConfigCreateArtikel", item.ConfigCreateArtikel == null ? (object)DBNull.Value : item.ConfigCreateArtikel);
			sqlCommand.Parameters.AddWithValue("ConfigCreateDept", item.ConfigCreateDept == null ? (object)DBNull.Value : item.ConfigCreateDept);
			sqlCommand.Parameters.AddWithValue("ConfigCreateLand", item.ConfigCreateLand == null ? (object)DBNull.Value : item.ConfigCreateLand);
			sqlCommand.Parameters.AddWithValue("ConfigCreateSupplier", item.ConfigCreateSupplier == null ? (object)DBNull.Value : item.ConfigCreateSupplier);
			sqlCommand.Parameters.AddWithValue("ConfigDeleteArtikel", item.ConfigDeleteArtikel == null ? (object)DBNull.Value : item.ConfigDeleteArtikel);
			sqlCommand.Parameters.AddWithValue("ConfigDeleteDept", item.ConfigDeleteDept == null ? (object)DBNull.Value : item.ConfigDeleteDept);
			sqlCommand.Parameters.AddWithValue("ConfigDeleteLand", item.ConfigDeleteLand == null ? (object)DBNull.Value : item.ConfigDeleteLand);
			sqlCommand.Parameters.AddWithValue("ConfigDeleteSupplier", item.ConfigDeleteSupplier == null ? (object)DBNull.Value : item.ConfigDeleteSupplier);
			sqlCommand.Parameters.AddWithValue("ConfigEditArtikel", item.ConfigEditArtikel == null ? (object)DBNull.Value : item.ConfigEditArtikel);
			sqlCommand.Parameters.AddWithValue("ConfigEditDept", item.ConfigEditDept == null ? (object)DBNull.Value : item.ConfigEditDept);
			sqlCommand.Parameters.AddWithValue("ConfigEditLand", item.ConfigEditLand == null ? (object)DBNull.Value : item.ConfigEditLand);
			sqlCommand.Parameters.AddWithValue("ConfigEditSupplier", item.ConfigEditSupplier == null ? (object)DBNull.Value : item.ConfigEditSupplier);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreditManagement", item.CreditManagement);
			sqlCommand.Parameters.AddWithValue("DeleteExternalCommande", item.DeleteExternalCommande == null ? (object)DBNull.Value : item.DeleteExternalCommande);
			sqlCommand.Parameters.AddWithValue("DeleteExternalProject", item.DeleteExternalProject == null ? (object)DBNull.Value : item.DeleteExternalProject);
			sqlCommand.Parameters.AddWithValue("DeleteInternalCommande", item.DeleteInternalCommande == null ? (object)DBNull.Value : item.DeleteInternalCommande);
			sqlCommand.Parameters.AddWithValue("DeleteInternalProject", item.DeleteInternalProject == null ? (object)DBNull.Value : item.DeleteInternalProject);
			sqlCommand.Parameters.AddWithValue("DeleteKontenrahmenAccounting", item.DeleteKontenrahmenAccounting == null ? (object)DBNull.Value : item.DeleteKontenrahmenAccounting);
			sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenKundenAccounting", item.DeleteZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenKundenAccounting);
			sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenLieferantenAccounting", item.DeleteZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenLieferantenAccounting);
			sqlCommand.Parameters.AddWithValue("FinanceOrder", item.FinanceOrder == null ? (object)DBNull.Value : item.FinanceOrder);
			sqlCommand.Parameters.AddWithValue("FinanceProject", item.FinanceProject == null ? (object)DBNull.Value : item.FinanceProject);
			sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("MainAccessProfileId", item.MainAccessProfileId == null ? (object)DBNull.Value : item.MainAccessProfileId);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("Project", item.Project == null ? (object)DBNull.Value : item.Project);
			sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllGroup", item.ProjectExternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectExternalEditAllGroup);
			sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllSite", item.ProjectExternalEditAllSite == null ? (object)DBNull.Value : item.ProjectExternalEditAllSite);
			sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllGroup", item.ProjectExternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectExternalViewAllGroup);
			sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllSite", item.ProjectExternalViewAllSite == null ? (object)DBNull.Value : item.ProjectExternalViewAllSite);
			sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllGroup", item.ProjectInternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectInternalEditAllGroup);
			sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllSite", item.ProjectInternalEditAllSite == null ? (object)DBNull.Value : item.ProjectInternalEditAllSite);
			sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllGroup", item.ProjectInternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectInternalViewAllGroup);
			sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllSite", item.ProjectInternalViewAllSite == null ? (object)DBNull.Value : item.ProjectInternalViewAllSite);
			sqlCommand.Parameters.AddWithValue("Receptions", item.Receptions == null ? (object)DBNull.Value : item.Receptions);
			sqlCommand.Parameters.AddWithValue("ReceptionsEdit", item.ReceptionsEdit == null ? (object)DBNull.Value : item.ReceptionsEdit);
			sqlCommand.Parameters.AddWithValue("ReceptionsView", item.ReceptionsView == null ? (object)DBNull.Value : item.ReceptionsView);
			sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
			sqlCommand.Parameters.AddWithValue("StatisticsViewAll", item.StatisticsViewAll == null ? (object)DBNull.Value : item.StatisticsViewAll);
			sqlCommand.Parameters.AddWithValue("Suppliers", item.Suppliers == null ? (object)DBNull.Value : item.Suppliers);
			sqlCommand.Parameters.AddWithValue("Units", item.Units == null ? (object)DBNull.Value : item.Units);
			sqlCommand.Parameters.AddWithValue("UpdateExternalCommande", item.UpdateExternalCommande == null ? (object)DBNull.Value : item.UpdateExternalCommande);
			sqlCommand.Parameters.AddWithValue("UpdateExternalProject", item.UpdateExternalProject == null ? (object)DBNull.Value : item.UpdateExternalProject);
			sqlCommand.Parameters.AddWithValue("UpdateInternalCommande", item.UpdateInternalCommande == null ? (object)DBNull.Value : item.UpdateInternalCommande);
			sqlCommand.Parameters.AddWithValue("UpdateInternalProject", item.UpdateInternalProject == null ? (object)DBNull.Value : item.UpdateInternalProject);
			sqlCommand.Parameters.AddWithValue("UpdateKontenrahmenAccounting", item.UpdateKontenrahmenAccounting == null ? (object)DBNull.Value : item.UpdateKontenrahmenAccounting);
			sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenKundenAccounting", item.UpdateZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenKundenAccounting);
			sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenLieferantenAccounting", item.UpdateZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenLieferantenAccounting);
			sqlCommand.Parameters.AddWithValue("ViewAusfuhrAccounting", item.ViewAusfuhrAccounting == null ? (object)DBNull.Value : item.ViewAusfuhrAccounting);
			sqlCommand.Parameters.AddWithValue("ViewEinfuhrAccounting", item.ViewEinfuhrAccounting == null ? (object)DBNull.Value : item.ViewEinfuhrAccounting);
			sqlCommand.Parameters.AddWithValue("ViewExternalCommande", item.ViewExternalCommande == null ? (object)DBNull.Value : item.ViewExternalCommande);
			sqlCommand.Parameters.AddWithValue("ViewExternalProject", item.ViewExternalProject == null ? (object)DBNull.Value : item.ViewExternalProject);
			sqlCommand.Parameters.AddWithValue("ViewFactoringRgGutschriftlistAccounting", item.ViewFactoringRgGutschriftlistAccounting == null ? (object)DBNull.Value : item.ViewFactoringRgGutschriftlistAccounting);
			sqlCommand.Parameters.AddWithValue("ViewInternalCommande", item.ViewInternalCommande == null ? (object)DBNull.Value : item.ViewInternalCommande);
			sqlCommand.Parameters.AddWithValue("ViewInternalProject", item.ViewInternalProject == null ? (object)DBNull.Value : item.ViewInternalProject);
			sqlCommand.Parameters.AddWithValue("ViewKontenrahmenAccounting", item.ViewKontenrahmenAccounting == null ? (object)DBNull.Value : item.ViewKontenrahmenAccounting);
			sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting", item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting);
			sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungSkontozahlerAccounting", item.ViewLiquiditatsplanungSkontozahlerAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungSkontozahlerAccounting);
			sqlCommand.Parameters.AddWithValue("ViewRechnungsTransferAccounting", item.ViewRechnungsTransferAccounting == null ? (object)DBNull.Value : item.ViewRechnungsTransferAccounting);
			sqlCommand.Parameters.AddWithValue("ViewRMDCZAccounting", item.ViewRMDCZAccounting == null ? (object)DBNull.Value : item.ViewRMDCZAccounting);
			sqlCommand.Parameters.AddWithValue("ViewStammdatenkontrolleWareneingangeAccounting", item.ViewStammdatenkontrolleWareneingangeAccounting == null ? (object)DBNull.Value : item.ViewStammdatenkontrolleWareneingangeAccounting);
			sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenKundenAccounting", item.ViewZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenKundenAccounting);
			sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenLieferantenAccounting", item.ViewZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenLieferantenAccounting);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 115; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}

			return -1;
		}
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__FNC_AccessProfile] ([AccessProfileName],[Accounting],[AddExternalCommande],[AddExternalProject],[AddInternalCommande],[AddInternalProject],[AddKontenrahmenAccounting],[Administration],[AdministrationAccessProfiles],[AdministrationAccessProfilesUpdate],[AdministrationUser],[AdministrationUserUpdate],[Article],[Assign],[AssignAllDepartments],[AssignAllEmployees],[AssignAllSites],[AssignCreateDept],[AssignCreateLand],[AssignCreateUser],[AssignDeleteDept],[AssignDeleteLand],[AssignDeleteUser],[AssignEditDept],[AssignEditLand],[AssignEditUser],[AssignViewDept],[AssignViewLand],[AssignViewUser],[Budget],[CashLiquidity],[Commande],[CommandeExternalEditAllGroup],[CommandeExternalEditAllSite],[CommandeExternalViewAllGroup],[CommandeExternalViewAllSite],[CommandeExternalViewInvoice],[CommandeExternalViewInvoiceAllGroup],[CommandeInternalEditAllDepartment],[CommandeInternalEditAllGroup],[CommandeInternalEditAllSite],[CommandeInternalViewAllDepartment],[CommandeInternalViewAllGroup],[CommandeInternalViewAllSite],[CommandeInternalViewInvoice],[CommandeInternalViewInvoiceAllGroup],[Config],[ConfigCreateArtikel],[ConfigCreateDept],[ConfigCreateLand],[ConfigCreateSupplier],[ConfigDeleteArtikel],[ConfigDeleteDept],[ConfigDeleteLand],[ConfigDeleteSupplier],[ConfigEditArtikel],[ConfigEditDept],[ConfigEditLand],[ConfigEditSupplier],[CreationTime],[CreationUserId],[CreditManagement],[DeleteExternalCommande],[DeleteExternalProject],[DeleteInternalCommande],[DeleteInternalProject],[DeleteKontenrahmenAccounting],[DeleteZahlungskonditionenKundenAccounting],[DeleteZahlungskonditionenLieferantenAccounting],[FinanceOrder],[FinanceProject],[IsDefault],[LastEditTime],[LastEditUserId],[MainAccessProfileId],[ModuleActivated],[Project],[ProjectExternalEditAllGroup],[ProjectExternalEditAllSite],[ProjectExternalViewAllGroup],[ProjectExternalViewAllSite],[ProjectInternalEditAllGroup],[ProjectInternalEditAllSite],[ProjectInternalViewAllGroup],[ProjectInternalViewAllSite],[Receptions],[ReceptionsEdit],[ReceptionsView],[Statistics],[StatisticsViewAll],[Suppliers],[Units],[UpdateExternalCommande],[UpdateExternalProject],[UpdateInternalCommande],[UpdateInternalProject],[UpdateKontenrahmenAccounting],[UpdateZahlungskonditionenKundenAccounting],[UpdateZahlungskonditionenLieferantenAccounting],[ViewAusfuhrAccounting],[ViewEinfuhrAccounting],[ViewExternalCommande],[ViewExternalProject],[ViewFactoringRgGutschriftlistAccounting],[ViewInternalCommande],[ViewInternalProject],[ViewKontenrahmenAccounting],[ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting],[ViewLiquiditatsplanungSkontozahlerAccounting],[ViewRechnungsTransferAccounting],[ViewRMDCZAccounting],[ViewStammdatenkontrolleWareneingangeAccounting],[ViewZahlungskonditionenKundenAccounting],[ViewZahlungskonditionenLieferantenAccounting]) VALUES ( "

						+ "@AccessProfileName" + i + ","
						+ "@Accounting" + i + ","
						+ "@AddExternalCommande" + i + ","
						+ "@AddExternalProject" + i + ","
						+ "@AddInternalCommande" + i + ","
						+ "@AddInternalProject" + i + ","
						+ "@AddKontenrahmenAccounting" + i + ","
						+ "@Administration" + i + ","
						+ "@AdministrationAccessProfiles" + i + ","
						+ "@AdministrationAccessProfilesUpdate" + i + ","
						+ "@AdministrationUser" + i + ","
						+ "@AdministrationUserUpdate" + i + ","
						+ "@Article" + i + ","
						+ "@Assign" + i + ","
						+ "@AssignAllDepartments" + i + ","
						+ "@AssignAllEmployees" + i + ","
						+ "@AssignAllSites" + i + ","
						+ "@AssignCreateDept" + i + ","
						+ "@AssignCreateLand" + i + ","
						+ "@AssignCreateUser" + i + ","
						+ "@AssignDeleteDept" + i + ","
						+ "@AssignDeleteLand" + i + ","
						+ "@AssignDeleteUser" + i + ","
						+ "@AssignEditDept" + i + ","
						+ "@AssignEditLand" + i + ","
						+ "@AssignEditUser" + i + ","
						+ "@AssignViewDept" + i + ","
						+ "@AssignViewLand" + i + ","
						+ "@AssignViewUser" + i + ","
						+ "@Budget" + i + ","
						+ "@CashLiquidity" + i + ","
						+ "@Commande" + i + ","
						+ "@CommandeExternalEditAllGroup" + i + ","
						+ "@CommandeExternalEditAllSite" + i + ","
						+ "@CommandeExternalViewAllGroup" + i + ","
						+ "@CommandeExternalViewAllSite" + i + ","
						+ "@CommandeExternalViewInvoice" + i + ","
						+ "@CommandeExternalViewInvoiceAllGroup" + i + ","
						+ "@CommandeInternalEditAllDepartment" + i + ","
						+ "@CommandeInternalEditAllGroup" + i + ","
						+ "@CommandeInternalEditAllSite" + i + ","
						+ "@CommandeInternalViewAllDepartment" + i + ","
						+ "@CommandeInternalViewAllGroup" + i + ","
						+ "@CommandeInternalViewAllSite" + i + ","
						+ "@CommandeInternalViewInvoice" + i + ","
						+ "@CommandeInternalViewInvoiceAllGroup" + i + ","
						+ "@Config" + i + ","
						+ "@ConfigCreateArtikel" + i + ","
						+ "@ConfigCreateDept" + i + ","
						+ "@ConfigCreateLand" + i + ","
						+ "@ConfigCreateSupplier" + i + ","
						+ "@ConfigDeleteArtikel" + i + ","
						+ "@ConfigDeleteDept" + i + ","
						+ "@ConfigDeleteLand" + i + ","
						+ "@ConfigDeleteSupplier" + i + ","
						+ "@ConfigEditArtikel" + i + ","
						+ "@ConfigEditDept" + i + ","
						+ "@ConfigEditLand" + i + ","
						+ "@ConfigEditSupplier" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@CreditManagement" + i + ","
						+ "@DeleteExternalCommande" + i + ","
						+ "@DeleteExternalProject" + i + ","
						+ "@DeleteInternalCommande" + i + ","
						+ "@DeleteInternalProject" + i + ","
						+ "@DeleteKontenrahmenAccounting" + i + ","
						+ "@DeleteZahlungskonditionenKundenAccounting" + i + ","
						+ "@DeleteZahlungskonditionenLieferantenAccounting" + i + ","
						+ "@FinanceOrder" + i + ","
						+ "@FinanceProject" + i + ","
						+ "@IsDefault" + i + ","
						+ "@LastEditTime" + i + ","
						+ "@LastEditUserId" + i + ","
						+ "@MainAccessProfileId" + i + ","
						+ "@ModuleActivated" + i + ","
						+ "@Project" + i + ","
						+ "@ProjectExternalEditAllGroup" + i + ","
						+ "@ProjectExternalEditAllSite" + i + ","
						+ "@ProjectExternalViewAllGroup" + i + ","
						+ "@ProjectExternalViewAllSite" + i + ","
						+ "@ProjectInternalEditAllGroup" + i + ","
						+ "@ProjectInternalEditAllSite" + i + ","
						+ "@ProjectInternalViewAllGroup" + i + ","
						+ "@ProjectInternalViewAllSite" + i + ","
						+ "@Receptions" + i + ","
						+ "@ReceptionsEdit" + i + ","
						+ "@ReceptionsView" + i + ","
						+ "@Statistics" + i + ","
						+ "@StatisticsViewAll" + i + ","
						+ "@Suppliers" + i + ","
						+ "@Units" + i + ","
						+ "@UpdateExternalCommande" + i + ","
						+ "@UpdateExternalProject" + i + ","
						+ "@UpdateInternalCommande" + i + ","
						+ "@UpdateInternalProject" + i + ","
						+ "@UpdateKontenrahmenAccounting" + i + ","
						+ "@UpdateZahlungskonditionenKundenAccounting" + i + ","
						+ "@UpdateZahlungskonditionenLieferantenAccounting" + i + ","
						+ "@ViewAusfuhrAccounting" + i + ","
						+ "@ViewEinfuhrAccounting" + i + ","
						+ "@ViewExternalCommande" + i + ","
						+ "@ViewExternalProject" + i + ","
						+ "@ViewFactoringRgGutschriftlistAccounting" + i + ","
						+ "@ViewInternalCommande" + i + ","
						+ "@ViewInternalProject" + i + ","
						+ "@ViewKontenrahmenAccounting" + i + ","
						+ "@ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting" + i + ","
						+ "@ViewLiquiditatsplanungSkontozahlerAccounting" + i + ","
						+ "@ViewRechnungsTransferAccounting" + i + ","
						+ "@ViewRMDCZAccounting" + i + ","
						+ "@ViewStammdatenkontrolleWareneingangeAccounting" + i + ","
						+ "@ViewZahlungskonditionenKundenAccounting" + i + ","
						+ "@ViewZahlungskonditionenLieferantenAccounting" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Accounting" + i, item.Accounting == null ? (object)DBNull.Value : item.Accounting);
					sqlCommand.Parameters.AddWithValue("AddExternalCommande" + i, item.AddExternalCommande == null ? (object)DBNull.Value : item.AddExternalCommande);
					sqlCommand.Parameters.AddWithValue("AddExternalProject" + i, item.AddExternalProject == null ? (object)DBNull.Value : item.AddExternalProject);
					sqlCommand.Parameters.AddWithValue("AddInternalCommande" + i, item.AddInternalCommande == null ? (object)DBNull.Value : item.AddInternalCommande);
					sqlCommand.Parameters.AddWithValue("AddInternalProject" + i, item.AddInternalProject == null ? (object)DBNull.Value : item.AddInternalProject);
					sqlCommand.Parameters.AddWithValue("AddKontenrahmenAccounting" + i, item.AddKontenrahmenAccounting == null ? (object)DBNull.Value : item.AddKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles" + i, item.AdministrationAccessProfiles == null ? (object)DBNull.Value : item.AdministrationAccessProfiles);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate" + i, item.AdministrationAccessProfilesUpdate == null ? (object)DBNull.Value : item.AdministrationAccessProfilesUpdate);
					sqlCommand.Parameters.AddWithValue("AdministrationUser" + i, item.AdministrationUser == null ? (object)DBNull.Value : item.AdministrationUser);
					sqlCommand.Parameters.AddWithValue("AdministrationUserUpdate" + i, item.AdministrationUserUpdate == null ? (object)DBNull.Value : item.AdministrationUserUpdate);
					sqlCommand.Parameters.AddWithValue("Article" + i, item.Article == null ? (object)DBNull.Value : item.Article);
					sqlCommand.Parameters.AddWithValue("Assign" + i, item.Assign == null ? (object)DBNull.Value : item.Assign);
					sqlCommand.Parameters.AddWithValue("AssignAllDepartments" + i, item.AssignAllDepartments == null ? (object)DBNull.Value : item.AssignAllDepartments);
					sqlCommand.Parameters.AddWithValue("AssignAllEmployees" + i, item.AssignAllEmployees == null ? (object)DBNull.Value : item.AssignAllEmployees);
					sqlCommand.Parameters.AddWithValue("AssignAllSites" + i, item.AssignAllSites == null ? (object)DBNull.Value : item.AssignAllSites);
					sqlCommand.Parameters.AddWithValue("AssignCreateDept" + i, item.AssignCreateDept == null ? (object)DBNull.Value : item.AssignCreateDept);
					sqlCommand.Parameters.AddWithValue("AssignCreateLand" + i, item.AssignCreateLand == null ? (object)DBNull.Value : item.AssignCreateLand);
					sqlCommand.Parameters.AddWithValue("AssignCreateUser" + i, item.AssignCreateUser == null ? (object)DBNull.Value : item.AssignCreateUser);
					sqlCommand.Parameters.AddWithValue("AssignDeleteDept" + i, item.AssignDeleteDept == null ? (object)DBNull.Value : item.AssignDeleteDept);
					sqlCommand.Parameters.AddWithValue("AssignDeleteLand" + i, item.AssignDeleteLand == null ? (object)DBNull.Value : item.AssignDeleteLand);
					sqlCommand.Parameters.AddWithValue("AssignDeleteUser" + i, item.AssignDeleteUser == null ? (object)DBNull.Value : item.AssignDeleteUser);
					sqlCommand.Parameters.AddWithValue("AssignEditDept" + i, item.AssignEditDept == null ? (object)DBNull.Value : item.AssignEditDept);
					sqlCommand.Parameters.AddWithValue("AssignEditLand" + i, item.AssignEditLand == null ? (object)DBNull.Value : item.AssignEditLand);
					sqlCommand.Parameters.AddWithValue("AssignEditUser" + i, item.AssignEditUser == null ? (object)DBNull.Value : item.AssignEditUser);
					sqlCommand.Parameters.AddWithValue("AssignViewDept" + i, item.AssignViewDept == null ? (object)DBNull.Value : item.AssignViewDept);
					sqlCommand.Parameters.AddWithValue("AssignViewLand" + i, item.AssignViewLand == null ? (object)DBNull.Value : item.AssignViewLand);
					sqlCommand.Parameters.AddWithValue("AssignViewUser" + i, item.AssignViewUser == null ? (object)DBNull.Value : item.AssignViewUser);
					sqlCommand.Parameters.AddWithValue("Budget" + i, item.Budget);
					sqlCommand.Parameters.AddWithValue("CashLiquidity" + i, item.CashLiquidity);
					sqlCommand.Parameters.AddWithValue("Commande" + i, item.Commande == null ? (object)DBNull.Value : item.Commande);
					sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllGroup" + i, item.CommandeExternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeExternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllSite" + i, item.CommandeExternalEditAllSite == null ? (object)DBNull.Value : item.CommandeExternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllGroup" + i, item.CommandeExternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllSite" + i, item.CommandeExternalViewAllSite == null ? (object)DBNull.Value : item.CommandeExternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoice" + i, item.CommandeExternalViewInvoice == null ? (object)DBNull.Value : item.CommandeExternalViewInvoice);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoiceAllGroup" + i, item.CommandeExternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewInvoiceAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllDepartment" + i, item.CommandeInternalEditAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalEditAllDepartment);
					sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllGroup" + i, item.CommandeInternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeInternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllSite" + i, item.CommandeInternalEditAllSite == null ? (object)DBNull.Value : item.CommandeInternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllDepartment" + i, item.CommandeInternalViewAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalViewAllDepartment);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllGroup" + i, item.CommandeInternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllSite" + i, item.CommandeInternalViewAllSite == null ? (object)DBNull.Value : item.CommandeInternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoice" + i, item.CommandeInternalViewInvoice == null ? (object)DBNull.Value : item.CommandeInternalViewInvoice);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoiceAllGroup" + i, item.CommandeInternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewInvoiceAllGroup);
					sqlCommand.Parameters.AddWithValue("Config" + i, item.Config == null ? (object)DBNull.Value : item.Config);
					sqlCommand.Parameters.AddWithValue("ConfigCreateArtikel" + i, item.ConfigCreateArtikel == null ? (object)DBNull.Value : item.ConfigCreateArtikel);
					sqlCommand.Parameters.AddWithValue("ConfigCreateDept" + i, item.ConfigCreateDept == null ? (object)DBNull.Value : item.ConfigCreateDept);
					sqlCommand.Parameters.AddWithValue("ConfigCreateLand" + i, item.ConfigCreateLand == null ? (object)DBNull.Value : item.ConfigCreateLand);
					sqlCommand.Parameters.AddWithValue("ConfigCreateSupplier" + i, item.ConfigCreateSupplier == null ? (object)DBNull.Value : item.ConfigCreateSupplier);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteArtikel" + i, item.ConfigDeleteArtikel == null ? (object)DBNull.Value : item.ConfigDeleteArtikel);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteDept" + i, item.ConfigDeleteDept == null ? (object)DBNull.Value : item.ConfigDeleteDept);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteLand" + i, item.ConfigDeleteLand == null ? (object)DBNull.Value : item.ConfigDeleteLand);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteSupplier" + i, item.ConfigDeleteSupplier == null ? (object)DBNull.Value : item.ConfigDeleteSupplier);
					sqlCommand.Parameters.AddWithValue("ConfigEditArtikel" + i, item.ConfigEditArtikel == null ? (object)DBNull.Value : item.ConfigEditArtikel);
					sqlCommand.Parameters.AddWithValue("ConfigEditDept" + i, item.ConfigEditDept == null ? (object)DBNull.Value : item.ConfigEditDept);
					sqlCommand.Parameters.AddWithValue("ConfigEditLand" + i, item.ConfigEditLand == null ? (object)DBNull.Value : item.ConfigEditLand);
					sqlCommand.Parameters.AddWithValue("ConfigEditSupplier" + i, item.ConfigEditSupplier == null ? (object)DBNull.Value : item.ConfigEditSupplier);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreditManagement" + i, item.CreditManagement);
					sqlCommand.Parameters.AddWithValue("DeleteExternalCommande" + i, item.DeleteExternalCommande == null ? (object)DBNull.Value : item.DeleteExternalCommande);
					sqlCommand.Parameters.AddWithValue("DeleteExternalProject" + i, item.DeleteExternalProject == null ? (object)DBNull.Value : item.DeleteExternalProject);
					sqlCommand.Parameters.AddWithValue("DeleteInternalCommande" + i, item.DeleteInternalCommande == null ? (object)DBNull.Value : item.DeleteInternalCommande);
					sqlCommand.Parameters.AddWithValue("DeleteInternalProject" + i, item.DeleteInternalProject == null ? (object)DBNull.Value : item.DeleteInternalProject);
					sqlCommand.Parameters.AddWithValue("DeleteKontenrahmenAccounting" + i, item.DeleteKontenrahmenAccounting == null ? (object)DBNull.Value : item.DeleteKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenKundenAccounting" + i, item.DeleteZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenKundenAccounting);
					sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenLieferantenAccounting" + i, item.DeleteZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenLieferantenAccounting);
					sqlCommand.Parameters.AddWithValue("FinanceOrder" + i, item.FinanceOrder == null ? (object)DBNull.Value : item.FinanceOrder);
					sqlCommand.Parameters.AddWithValue("FinanceProject" + i, item.FinanceProject == null ? (object)DBNull.Value : item.FinanceProject);
					sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("MainAccessProfileId" + i, item.MainAccessProfileId == null ? (object)DBNull.Value : item.MainAccessProfileId);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("Project" + i, item.Project == null ? (object)DBNull.Value : item.Project);
					sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllGroup" + i, item.ProjectExternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectExternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllSite" + i, item.ProjectExternalEditAllSite == null ? (object)DBNull.Value : item.ProjectExternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllGroup" + i, item.ProjectExternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectExternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllSite" + i, item.ProjectExternalViewAllSite == null ? (object)DBNull.Value : item.ProjectExternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllGroup" + i, item.ProjectInternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectInternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllSite" + i, item.ProjectInternalEditAllSite == null ? (object)DBNull.Value : item.ProjectInternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllGroup" + i, item.ProjectInternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectInternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllSite" + i, item.ProjectInternalViewAllSite == null ? (object)DBNull.Value : item.ProjectInternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("Receptions" + i, item.Receptions == null ? (object)DBNull.Value : item.Receptions);
					sqlCommand.Parameters.AddWithValue("ReceptionsEdit" + i, item.ReceptionsEdit == null ? (object)DBNull.Value : item.ReceptionsEdit);
					sqlCommand.Parameters.AddWithValue("ReceptionsView" + i, item.ReceptionsView == null ? (object)DBNull.Value : item.ReceptionsView);
					sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatisticsViewAll" + i, item.StatisticsViewAll == null ? (object)DBNull.Value : item.StatisticsViewAll);
					sqlCommand.Parameters.AddWithValue("Suppliers" + i, item.Suppliers == null ? (object)DBNull.Value : item.Suppliers);
					sqlCommand.Parameters.AddWithValue("Units" + i, item.Units == null ? (object)DBNull.Value : item.Units);
					sqlCommand.Parameters.AddWithValue("UpdateExternalCommande" + i, item.UpdateExternalCommande == null ? (object)DBNull.Value : item.UpdateExternalCommande);
					sqlCommand.Parameters.AddWithValue("UpdateExternalProject" + i, item.UpdateExternalProject == null ? (object)DBNull.Value : item.UpdateExternalProject);
					sqlCommand.Parameters.AddWithValue("UpdateInternalCommande" + i, item.UpdateInternalCommande == null ? (object)DBNull.Value : item.UpdateInternalCommande);
					sqlCommand.Parameters.AddWithValue("UpdateInternalProject" + i, item.UpdateInternalProject == null ? (object)DBNull.Value : item.UpdateInternalProject);
					sqlCommand.Parameters.AddWithValue("UpdateKontenrahmenAccounting" + i, item.UpdateKontenrahmenAccounting == null ? (object)DBNull.Value : item.UpdateKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenKundenAccounting" + i, item.UpdateZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenKundenAccounting);
					sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenLieferantenAccounting" + i, item.UpdateZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenLieferantenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewAusfuhrAccounting" + i, item.ViewAusfuhrAccounting == null ? (object)DBNull.Value : item.ViewAusfuhrAccounting);
					sqlCommand.Parameters.AddWithValue("ViewEinfuhrAccounting" + i, item.ViewEinfuhrAccounting == null ? (object)DBNull.Value : item.ViewEinfuhrAccounting);
					sqlCommand.Parameters.AddWithValue("ViewExternalCommande" + i, item.ViewExternalCommande == null ? (object)DBNull.Value : item.ViewExternalCommande);
					sqlCommand.Parameters.AddWithValue("ViewExternalProject" + i, item.ViewExternalProject == null ? (object)DBNull.Value : item.ViewExternalProject);
					sqlCommand.Parameters.AddWithValue("ViewFactoringRgGutschriftlistAccounting" + i, item.ViewFactoringRgGutschriftlistAccounting == null ? (object)DBNull.Value : item.ViewFactoringRgGutschriftlistAccounting);
					sqlCommand.Parameters.AddWithValue("ViewInternalCommande" + i, item.ViewInternalCommande == null ? (object)DBNull.Value : item.ViewInternalCommande);
					sqlCommand.Parameters.AddWithValue("ViewInternalProject" + i, item.ViewInternalProject == null ? (object)DBNull.Value : item.ViewInternalProject);
					sqlCommand.Parameters.AddWithValue("ViewKontenrahmenAccounting" + i, item.ViewKontenrahmenAccounting == null ? (object)DBNull.Value : item.ViewKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting" + i, item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungSkontozahlerAccounting" + i, item.ViewLiquiditatsplanungSkontozahlerAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungSkontozahlerAccounting);
					sqlCommand.Parameters.AddWithValue("ViewRechnungsTransferAccounting" + i, item.ViewRechnungsTransferAccounting == null ? (object)DBNull.Value : item.ViewRechnungsTransferAccounting);
					sqlCommand.Parameters.AddWithValue("ViewRMDCZAccounting" + i, item.ViewRMDCZAccounting == null ? (object)DBNull.Value : item.ViewRMDCZAccounting);
					sqlCommand.Parameters.AddWithValue("ViewStammdatenkontrolleWareneingangeAccounting" + i, item.ViewStammdatenkontrolleWareneingangeAccounting == null ? (object)DBNull.Value : item.ViewStammdatenkontrolleWareneingangeAccounting);
					sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenKundenAccounting" + i, item.ViewZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenKundenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenLieferantenAccounting" + i, item.ViewZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenLieferantenAccounting);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__FNC_AccessProfile] SET [AccessProfileName]=@AccessProfileName, [Accounting]=@Accounting, [AddExternalCommande]=@AddExternalCommande, [AddExternalProject]=@AddExternalProject, [AddInternalCommande]=@AddInternalCommande, [AddInternalProject]=@AddInternalProject, [AddKontenrahmenAccounting]=@AddKontenrahmenAccounting, [Administration]=@Administration, [AdministrationAccessProfiles]=@AdministrationAccessProfiles, [AdministrationAccessProfilesUpdate]=@AdministrationAccessProfilesUpdate, [AdministrationUser]=@AdministrationUser, [AdministrationUserUpdate]=@AdministrationUserUpdate, [Article]=@Article, [Assign]=@Assign, [AssignAllDepartments]=@AssignAllDepartments, [AssignAllEmployees]=@AssignAllEmployees, [AssignAllSites]=@AssignAllSites, [AssignCreateDept]=@AssignCreateDept, [AssignCreateLand]=@AssignCreateLand, [AssignCreateUser]=@AssignCreateUser, [AssignDeleteDept]=@AssignDeleteDept, [AssignDeleteLand]=@AssignDeleteLand, [AssignDeleteUser]=@AssignDeleteUser, [AssignEditDept]=@AssignEditDept, [AssignEditLand]=@AssignEditLand, [AssignEditUser]=@AssignEditUser, [AssignViewDept]=@AssignViewDept, [AssignViewLand]=@AssignViewLand, [AssignViewUser]=@AssignViewUser, [Budget]=@Budget, [CashLiquidity]=@CashLiquidity, [Commande]=@Commande, [CommandeExternalEditAllGroup]=@CommandeExternalEditAllGroup, [CommandeExternalEditAllSite]=@CommandeExternalEditAllSite, [CommandeExternalViewAllGroup]=@CommandeExternalViewAllGroup, [CommandeExternalViewAllSite]=@CommandeExternalViewAllSite, [CommandeExternalViewInvoice]=@CommandeExternalViewInvoice, [CommandeExternalViewInvoiceAllGroup]=@CommandeExternalViewInvoiceAllGroup, [CommandeInternalEditAllDepartment]=@CommandeInternalEditAllDepartment, [CommandeInternalEditAllGroup]=@CommandeInternalEditAllGroup, [CommandeInternalEditAllSite]=@CommandeInternalEditAllSite, [CommandeInternalViewAllDepartment]=@CommandeInternalViewAllDepartment, [CommandeInternalViewAllGroup]=@CommandeInternalViewAllGroup, [CommandeInternalViewAllSite]=@CommandeInternalViewAllSite, [CommandeInternalViewInvoice]=@CommandeInternalViewInvoice, [CommandeInternalViewInvoiceAllGroup]=@CommandeInternalViewInvoiceAllGroup, [Config]=@Config, [ConfigCreateArtikel]=@ConfigCreateArtikel, [ConfigCreateDept]=@ConfigCreateDept, [ConfigCreateLand]=@ConfigCreateLand, [ConfigCreateSupplier]=@ConfigCreateSupplier, [ConfigDeleteArtikel]=@ConfigDeleteArtikel, [ConfigDeleteDept]=@ConfigDeleteDept, [ConfigDeleteLand]=@ConfigDeleteLand, [ConfigDeleteSupplier]=@ConfigDeleteSupplier, [ConfigEditArtikel]=@ConfigEditArtikel, [ConfigEditDept]=@ConfigEditDept, [ConfigEditLand]=@ConfigEditLand, [ConfigEditSupplier]=@ConfigEditSupplier, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CreditManagement]=@CreditManagement, [DeleteExternalCommande]=@DeleteExternalCommande, [DeleteExternalProject]=@DeleteExternalProject, [DeleteInternalCommande]=@DeleteInternalCommande, [DeleteInternalProject]=@DeleteInternalProject, [DeleteKontenrahmenAccounting]=@DeleteKontenrahmenAccounting, [DeleteZahlungskonditionenKundenAccounting]=@DeleteZahlungskonditionenKundenAccounting, [DeleteZahlungskonditionenLieferantenAccounting]=@DeleteZahlungskonditionenLieferantenAccounting, [FinanceOrder]=@FinanceOrder, [FinanceProject]=@FinanceProject, [IsDefault]=@IsDefault, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [MainAccessProfileId]=@MainAccessProfileId, [ModuleActivated]=@ModuleActivated, [Project]=@Project, [ProjectExternalEditAllGroup]=@ProjectExternalEditAllGroup, [ProjectExternalEditAllSite]=@ProjectExternalEditAllSite, [ProjectExternalViewAllGroup]=@ProjectExternalViewAllGroup, [ProjectExternalViewAllSite]=@ProjectExternalViewAllSite, [ProjectInternalEditAllGroup]=@ProjectInternalEditAllGroup, [ProjectInternalEditAllSite]=@ProjectInternalEditAllSite, [ProjectInternalViewAllGroup]=@ProjectInternalViewAllGroup, [ProjectInternalViewAllSite]=@ProjectInternalViewAllSite, [Receptions]=@Receptions, [ReceptionsEdit]=@ReceptionsEdit, [ReceptionsView]=@ReceptionsView, [Statistics]=@Statistics, [StatisticsViewAll]=@StatisticsViewAll, [Suppliers]=@Suppliers, [Units]=@Units, [UpdateExternalCommande]=@UpdateExternalCommande, [UpdateExternalProject]=@UpdateExternalProject, [UpdateInternalCommande]=@UpdateInternalCommande, [UpdateInternalProject]=@UpdateInternalProject, [UpdateKontenrahmenAccounting]=@UpdateKontenrahmenAccounting, [UpdateZahlungskonditionenKundenAccounting]=@UpdateZahlungskonditionenKundenAccounting, [UpdateZahlungskonditionenLieferantenAccounting]=@UpdateZahlungskonditionenLieferantenAccounting, [ViewAusfuhrAccounting]=@ViewAusfuhrAccounting, [ViewEinfuhrAccounting]=@ViewEinfuhrAccounting, [ViewExternalCommande]=@ViewExternalCommande, [ViewExternalProject]=@ViewExternalProject, [ViewFactoringRgGutschriftlistAccounting]=@ViewFactoringRgGutschriftlistAccounting, [ViewInternalCommande]=@ViewInternalCommande, [ViewInternalProject]=@ViewInternalProject, [ViewKontenrahmenAccounting]=@ViewKontenrahmenAccounting, [ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting]=@ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting, [ViewLiquiditatsplanungSkontozahlerAccounting]=@ViewLiquiditatsplanungSkontozahlerAccounting, [ViewRechnungsTransferAccounting]=@ViewRechnungsTransferAccounting, [ViewRMDCZAccounting]=@ViewRMDCZAccounting, [ViewStammdatenkontrolleWareneingangeAccounting]=@ViewStammdatenkontrolleWareneingangeAccounting, [ViewZahlungskonditionenKundenAccounting]=@ViewZahlungskonditionenKundenAccounting, [ViewZahlungskonditionenLieferantenAccounting]=@ViewZahlungskonditionenLieferantenAccounting WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("Accounting", item.Accounting == null ? (object)DBNull.Value : item.Accounting);
			sqlCommand.Parameters.AddWithValue("AddExternalCommande", item.AddExternalCommande == null ? (object)DBNull.Value : item.AddExternalCommande);
			sqlCommand.Parameters.AddWithValue("AddExternalProject", item.AddExternalProject == null ? (object)DBNull.Value : item.AddExternalProject);
			sqlCommand.Parameters.AddWithValue("AddInternalCommande", item.AddInternalCommande == null ? (object)DBNull.Value : item.AddInternalCommande);
			sqlCommand.Parameters.AddWithValue("AddInternalProject", item.AddInternalProject == null ? (object)DBNull.Value : item.AddInternalProject);
			sqlCommand.Parameters.AddWithValue("AddKontenrahmenAccounting", item.AddKontenrahmenAccounting == null ? (object)DBNull.Value : item.AddKontenrahmenAccounting);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
			sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles", item.AdministrationAccessProfiles == null ? (object)DBNull.Value : item.AdministrationAccessProfiles);
			sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate", item.AdministrationAccessProfilesUpdate == null ? (object)DBNull.Value : item.AdministrationAccessProfilesUpdate);
			sqlCommand.Parameters.AddWithValue("AdministrationUser", item.AdministrationUser == null ? (object)DBNull.Value : item.AdministrationUser);
			sqlCommand.Parameters.AddWithValue("AdministrationUserUpdate", item.AdministrationUserUpdate == null ? (object)DBNull.Value : item.AdministrationUserUpdate);
			sqlCommand.Parameters.AddWithValue("Article", item.Article == null ? (object)DBNull.Value : item.Article);
			sqlCommand.Parameters.AddWithValue("Assign", item.Assign == null ? (object)DBNull.Value : item.Assign);
			sqlCommand.Parameters.AddWithValue("AssignAllDepartments", item.AssignAllDepartments == null ? (object)DBNull.Value : item.AssignAllDepartments);
			sqlCommand.Parameters.AddWithValue("AssignAllEmployees", item.AssignAllEmployees == null ? (object)DBNull.Value : item.AssignAllEmployees);
			sqlCommand.Parameters.AddWithValue("AssignAllSites", item.AssignAllSites == null ? (object)DBNull.Value : item.AssignAllSites);
			sqlCommand.Parameters.AddWithValue("AssignCreateDept", item.AssignCreateDept == null ? (object)DBNull.Value : item.AssignCreateDept);
			sqlCommand.Parameters.AddWithValue("AssignCreateLand", item.AssignCreateLand == null ? (object)DBNull.Value : item.AssignCreateLand);
			sqlCommand.Parameters.AddWithValue("AssignCreateUser", item.AssignCreateUser == null ? (object)DBNull.Value : item.AssignCreateUser);
			sqlCommand.Parameters.AddWithValue("AssignDeleteDept", item.AssignDeleteDept == null ? (object)DBNull.Value : item.AssignDeleteDept);
			sqlCommand.Parameters.AddWithValue("AssignDeleteLand", item.AssignDeleteLand == null ? (object)DBNull.Value : item.AssignDeleteLand);
			sqlCommand.Parameters.AddWithValue("AssignDeleteUser", item.AssignDeleteUser == null ? (object)DBNull.Value : item.AssignDeleteUser);
			sqlCommand.Parameters.AddWithValue("AssignEditDept", item.AssignEditDept == null ? (object)DBNull.Value : item.AssignEditDept);
			sqlCommand.Parameters.AddWithValue("AssignEditLand", item.AssignEditLand == null ? (object)DBNull.Value : item.AssignEditLand);
			sqlCommand.Parameters.AddWithValue("AssignEditUser", item.AssignEditUser == null ? (object)DBNull.Value : item.AssignEditUser);
			sqlCommand.Parameters.AddWithValue("AssignViewDept", item.AssignViewDept == null ? (object)DBNull.Value : item.AssignViewDept);
			sqlCommand.Parameters.AddWithValue("AssignViewLand", item.AssignViewLand == null ? (object)DBNull.Value : item.AssignViewLand);
			sqlCommand.Parameters.AddWithValue("AssignViewUser", item.AssignViewUser == null ? (object)DBNull.Value : item.AssignViewUser);
			sqlCommand.Parameters.AddWithValue("Budget", item.Budget);
			sqlCommand.Parameters.AddWithValue("CashLiquidity", item.CashLiquidity);
			sqlCommand.Parameters.AddWithValue("Commande", item.Commande == null ? (object)DBNull.Value : item.Commande);
			sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllGroup", item.CommandeExternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeExternalEditAllGroup);
			sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllSite", item.CommandeExternalEditAllSite == null ? (object)DBNull.Value : item.CommandeExternalEditAllSite);
			sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllGroup", item.CommandeExternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewAllGroup);
			sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllSite", item.CommandeExternalViewAllSite == null ? (object)DBNull.Value : item.CommandeExternalViewAllSite);
			sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoice", item.CommandeExternalViewInvoice == null ? (object)DBNull.Value : item.CommandeExternalViewInvoice);
			sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoiceAllGroup", item.CommandeExternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewInvoiceAllGroup);
			sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllDepartment", item.CommandeInternalEditAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalEditAllDepartment);
			sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllGroup", item.CommandeInternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeInternalEditAllGroup);
			sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllSite", item.CommandeInternalEditAllSite == null ? (object)DBNull.Value : item.CommandeInternalEditAllSite);
			sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllDepartment", item.CommandeInternalViewAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalViewAllDepartment);
			sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllGroup", item.CommandeInternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewAllGroup);
			sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllSite", item.CommandeInternalViewAllSite == null ? (object)DBNull.Value : item.CommandeInternalViewAllSite);
			sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoice", item.CommandeInternalViewInvoice == null ? (object)DBNull.Value : item.CommandeInternalViewInvoice);
			sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoiceAllGroup", item.CommandeInternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewInvoiceAllGroup);
			sqlCommand.Parameters.AddWithValue("Config", item.Config == null ? (object)DBNull.Value : item.Config);
			sqlCommand.Parameters.AddWithValue("ConfigCreateArtikel", item.ConfigCreateArtikel == null ? (object)DBNull.Value : item.ConfigCreateArtikel);
			sqlCommand.Parameters.AddWithValue("ConfigCreateDept", item.ConfigCreateDept == null ? (object)DBNull.Value : item.ConfigCreateDept);
			sqlCommand.Parameters.AddWithValue("ConfigCreateLand", item.ConfigCreateLand == null ? (object)DBNull.Value : item.ConfigCreateLand);
			sqlCommand.Parameters.AddWithValue("ConfigCreateSupplier", item.ConfigCreateSupplier == null ? (object)DBNull.Value : item.ConfigCreateSupplier);
			sqlCommand.Parameters.AddWithValue("ConfigDeleteArtikel", item.ConfigDeleteArtikel == null ? (object)DBNull.Value : item.ConfigDeleteArtikel);
			sqlCommand.Parameters.AddWithValue("ConfigDeleteDept", item.ConfigDeleteDept == null ? (object)DBNull.Value : item.ConfigDeleteDept);
			sqlCommand.Parameters.AddWithValue("ConfigDeleteLand", item.ConfigDeleteLand == null ? (object)DBNull.Value : item.ConfigDeleteLand);
			sqlCommand.Parameters.AddWithValue("ConfigDeleteSupplier", item.ConfigDeleteSupplier == null ? (object)DBNull.Value : item.ConfigDeleteSupplier);
			sqlCommand.Parameters.AddWithValue("ConfigEditArtikel", item.ConfigEditArtikel == null ? (object)DBNull.Value : item.ConfigEditArtikel);
			sqlCommand.Parameters.AddWithValue("ConfigEditDept", item.ConfigEditDept == null ? (object)DBNull.Value : item.ConfigEditDept);
			sqlCommand.Parameters.AddWithValue("ConfigEditLand", item.ConfigEditLand == null ? (object)DBNull.Value : item.ConfigEditLand);
			sqlCommand.Parameters.AddWithValue("ConfigEditSupplier", item.ConfigEditSupplier == null ? (object)DBNull.Value : item.ConfigEditSupplier);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreditManagement", item.CreditManagement);
			sqlCommand.Parameters.AddWithValue("DeleteExternalCommande", item.DeleteExternalCommande == null ? (object)DBNull.Value : item.DeleteExternalCommande);
			sqlCommand.Parameters.AddWithValue("DeleteExternalProject", item.DeleteExternalProject == null ? (object)DBNull.Value : item.DeleteExternalProject);
			sqlCommand.Parameters.AddWithValue("DeleteInternalCommande", item.DeleteInternalCommande == null ? (object)DBNull.Value : item.DeleteInternalCommande);
			sqlCommand.Parameters.AddWithValue("DeleteInternalProject", item.DeleteInternalProject == null ? (object)DBNull.Value : item.DeleteInternalProject);
			sqlCommand.Parameters.AddWithValue("DeleteKontenrahmenAccounting", item.DeleteKontenrahmenAccounting == null ? (object)DBNull.Value : item.DeleteKontenrahmenAccounting);
			sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenKundenAccounting", item.DeleteZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenKundenAccounting);
			sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenLieferantenAccounting", item.DeleteZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenLieferantenAccounting);
			sqlCommand.Parameters.AddWithValue("FinanceOrder", item.FinanceOrder == null ? (object)DBNull.Value : item.FinanceOrder);
			sqlCommand.Parameters.AddWithValue("FinanceProject", item.FinanceProject == null ? (object)DBNull.Value : item.FinanceProject);
			sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("MainAccessProfileId", item.MainAccessProfileId == null ? (object)DBNull.Value : item.MainAccessProfileId);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("Project", item.Project == null ? (object)DBNull.Value : item.Project);
			sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllGroup", item.ProjectExternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectExternalEditAllGroup);
			sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllSite", item.ProjectExternalEditAllSite == null ? (object)DBNull.Value : item.ProjectExternalEditAllSite);
			sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllGroup", item.ProjectExternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectExternalViewAllGroup);
			sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllSite", item.ProjectExternalViewAllSite == null ? (object)DBNull.Value : item.ProjectExternalViewAllSite);
			sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllGroup", item.ProjectInternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectInternalEditAllGroup);
			sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllSite", item.ProjectInternalEditAllSite == null ? (object)DBNull.Value : item.ProjectInternalEditAllSite);
			sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllGroup", item.ProjectInternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectInternalViewAllGroup);
			sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllSite", item.ProjectInternalViewAllSite == null ? (object)DBNull.Value : item.ProjectInternalViewAllSite);
			sqlCommand.Parameters.AddWithValue("Receptions", item.Receptions == null ? (object)DBNull.Value : item.Receptions);
			sqlCommand.Parameters.AddWithValue("ReceptionsEdit", item.ReceptionsEdit == null ? (object)DBNull.Value : item.ReceptionsEdit);
			sqlCommand.Parameters.AddWithValue("ReceptionsView", item.ReceptionsView == null ? (object)DBNull.Value : item.ReceptionsView);
			sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
			sqlCommand.Parameters.AddWithValue("StatisticsViewAll", item.StatisticsViewAll == null ? (object)DBNull.Value : item.StatisticsViewAll);
			sqlCommand.Parameters.AddWithValue("Suppliers", item.Suppliers == null ? (object)DBNull.Value : item.Suppliers);
			sqlCommand.Parameters.AddWithValue("Units", item.Units == null ? (object)DBNull.Value : item.Units);
			sqlCommand.Parameters.AddWithValue("UpdateExternalCommande", item.UpdateExternalCommande == null ? (object)DBNull.Value : item.UpdateExternalCommande);
			sqlCommand.Parameters.AddWithValue("UpdateExternalProject", item.UpdateExternalProject == null ? (object)DBNull.Value : item.UpdateExternalProject);
			sqlCommand.Parameters.AddWithValue("UpdateInternalCommande", item.UpdateInternalCommande == null ? (object)DBNull.Value : item.UpdateInternalCommande);
			sqlCommand.Parameters.AddWithValue("UpdateInternalProject", item.UpdateInternalProject == null ? (object)DBNull.Value : item.UpdateInternalProject);
			sqlCommand.Parameters.AddWithValue("UpdateKontenrahmenAccounting", item.UpdateKontenrahmenAccounting == null ? (object)DBNull.Value : item.UpdateKontenrahmenAccounting);
			sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenKundenAccounting", item.UpdateZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenKundenAccounting);
			sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenLieferantenAccounting", item.UpdateZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenLieferantenAccounting);
			sqlCommand.Parameters.AddWithValue("ViewAusfuhrAccounting", item.ViewAusfuhrAccounting == null ? (object)DBNull.Value : item.ViewAusfuhrAccounting);
			sqlCommand.Parameters.AddWithValue("ViewEinfuhrAccounting", item.ViewEinfuhrAccounting == null ? (object)DBNull.Value : item.ViewEinfuhrAccounting);
			sqlCommand.Parameters.AddWithValue("ViewExternalCommande", item.ViewExternalCommande == null ? (object)DBNull.Value : item.ViewExternalCommande);
			sqlCommand.Parameters.AddWithValue("ViewExternalProject", item.ViewExternalProject == null ? (object)DBNull.Value : item.ViewExternalProject);
			sqlCommand.Parameters.AddWithValue("ViewFactoringRgGutschriftlistAccounting", item.ViewFactoringRgGutschriftlistAccounting == null ? (object)DBNull.Value : item.ViewFactoringRgGutschriftlistAccounting);
			sqlCommand.Parameters.AddWithValue("ViewInternalCommande", item.ViewInternalCommande == null ? (object)DBNull.Value : item.ViewInternalCommande);
			sqlCommand.Parameters.AddWithValue("ViewInternalProject", item.ViewInternalProject == null ? (object)DBNull.Value : item.ViewInternalProject);
			sqlCommand.Parameters.AddWithValue("ViewKontenrahmenAccounting", item.ViewKontenrahmenAccounting == null ? (object)DBNull.Value : item.ViewKontenrahmenAccounting);
			sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting", item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting);
			sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungSkontozahlerAccounting", item.ViewLiquiditatsplanungSkontozahlerAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungSkontozahlerAccounting);
			sqlCommand.Parameters.AddWithValue("ViewRechnungsTransferAccounting", item.ViewRechnungsTransferAccounting == null ? (object)DBNull.Value : item.ViewRechnungsTransferAccounting);
			sqlCommand.Parameters.AddWithValue("ViewRMDCZAccounting", item.ViewRMDCZAccounting == null ? (object)DBNull.Value : item.ViewRMDCZAccounting);
			sqlCommand.Parameters.AddWithValue("ViewStammdatenkontrolleWareneingangeAccounting", item.ViewStammdatenkontrolleWareneingangeAccounting == null ? (object)DBNull.Value : item.ViewStammdatenkontrolleWareneingangeAccounting);
			sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenKundenAccounting", item.ViewZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenKundenAccounting);
			sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenLieferantenAccounting", item.ViewZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenLieferantenAccounting);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 115; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [__FNC_AccessProfile] SET "

					+ "[AccessProfileName]=@AccessProfileName" + i + ","
					+ "[Accounting]=@Accounting" + i + ","
					+ "[AddExternalCommande]=@AddExternalCommande" + i + ","
					+ "[AddExternalProject]=@AddExternalProject" + i + ","
					+ "[AddInternalCommande]=@AddInternalCommande" + i + ","
					+ "[AddInternalProject]=@AddInternalProject" + i + ","
					+ "[AddKontenrahmenAccounting]=@AddKontenrahmenAccounting" + i + ","
					+ "[Administration]=@Administration" + i + ","
					+ "[AdministrationAccessProfiles]=@AdministrationAccessProfiles" + i + ","
					+ "[AdministrationAccessProfilesUpdate]=@AdministrationAccessProfilesUpdate" + i + ","
					+ "[AdministrationUser]=@AdministrationUser" + i + ","
					+ "[AdministrationUserUpdate]=@AdministrationUserUpdate" + i + ","
					+ "[Article]=@Article" + i + ","
					+ "[Assign]=@Assign" + i + ","
					+ "[AssignAllDepartments]=@AssignAllDepartments" + i + ","
					+ "[AssignAllEmployees]=@AssignAllEmployees" + i + ","
					+ "[AssignAllSites]=@AssignAllSites" + i + ","
					+ "[AssignCreateDept]=@AssignCreateDept" + i + ","
					+ "[AssignCreateLand]=@AssignCreateLand" + i + ","
					+ "[AssignCreateUser]=@AssignCreateUser" + i + ","
					+ "[AssignDeleteDept]=@AssignDeleteDept" + i + ","
					+ "[AssignDeleteLand]=@AssignDeleteLand" + i + ","
					+ "[AssignDeleteUser]=@AssignDeleteUser" + i + ","
					+ "[AssignEditDept]=@AssignEditDept" + i + ","
					+ "[AssignEditLand]=@AssignEditLand" + i + ","
					+ "[AssignEditUser]=@AssignEditUser" + i + ","
					+ "[AssignViewDept]=@AssignViewDept" + i + ","
					+ "[AssignViewLand]=@AssignViewLand" + i + ","
					+ "[AssignViewUser]=@AssignViewUser" + i + ","
					+ "[Budget]=@Budget" + i + ","
					+ "[CashLiquidity]=@CashLiquidity" + i + ","
					+ "[Commande]=@Commande" + i + ","
					+ "[CommandeExternalEditAllGroup]=@CommandeExternalEditAllGroup" + i + ","
					+ "[CommandeExternalEditAllSite]=@CommandeExternalEditAllSite" + i + ","
					+ "[CommandeExternalViewAllGroup]=@CommandeExternalViewAllGroup" + i + ","
					+ "[CommandeExternalViewAllSite]=@CommandeExternalViewAllSite" + i + ","
					+ "[CommandeExternalViewInvoice]=@CommandeExternalViewInvoice" + i + ","
					+ "[CommandeExternalViewInvoiceAllGroup]=@CommandeExternalViewInvoiceAllGroup" + i + ","
					+ "[CommandeInternalEditAllDepartment]=@CommandeInternalEditAllDepartment" + i + ","
					+ "[CommandeInternalEditAllGroup]=@CommandeInternalEditAllGroup" + i + ","
					+ "[CommandeInternalEditAllSite]=@CommandeInternalEditAllSite" + i + ","
					+ "[CommandeInternalViewAllDepartment]=@CommandeInternalViewAllDepartment" + i + ","
					+ "[CommandeInternalViewAllGroup]=@CommandeInternalViewAllGroup" + i + ","
					+ "[CommandeInternalViewAllSite]=@CommandeInternalViewAllSite" + i + ","
					+ "[CommandeInternalViewInvoice]=@CommandeInternalViewInvoice" + i + ","
					+ "[CommandeInternalViewInvoiceAllGroup]=@CommandeInternalViewInvoiceAllGroup" + i + ","
					+ "[Config]=@Config" + i + ","
					+ "[ConfigCreateArtikel]=@ConfigCreateArtikel" + i + ","
					+ "[ConfigCreateDept]=@ConfigCreateDept" + i + ","
					+ "[ConfigCreateLand]=@ConfigCreateLand" + i + ","
					+ "[ConfigCreateSupplier]=@ConfigCreateSupplier" + i + ","
					+ "[ConfigDeleteArtikel]=@ConfigDeleteArtikel" + i + ","
					+ "[ConfigDeleteDept]=@ConfigDeleteDept" + i + ","
					+ "[ConfigDeleteLand]=@ConfigDeleteLand" + i + ","
					+ "[ConfigDeleteSupplier]=@ConfigDeleteSupplier" + i + ","
					+ "[ConfigEditArtikel]=@ConfigEditArtikel" + i + ","
					+ "[ConfigEditDept]=@ConfigEditDept" + i + ","
					+ "[ConfigEditLand]=@ConfigEditLand" + i + ","
					+ "[ConfigEditSupplier]=@ConfigEditSupplier" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[CreditManagement]=@CreditManagement" + i + ","
					+ "[DeleteExternalCommande]=@DeleteExternalCommande" + i + ","
					+ "[DeleteExternalProject]=@DeleteExternalProject" + i + ","
					+ "[DeleteInternalCommande]=@DeleteInternalCommande" + i + ","
					+ "[DeleteInternalProject]=@DeleteInternalProject" + i + ","
					+ "[DeleteKontenrahmenAccounting]=@DeleteKontenrahmenAccounting" + i + ","
					+ "[DeleteZahlungskonditionenKundenAccounting]=@DeleteZahlungskonditionenKundenAccounting" + i + ","
					+ "[DeleteZahlungskonditionenLieferantenAccounting]=@DeleteZahlungskonditionenLieferantenAccounting" + i + ","
					+ "[FinanceOrder]=@FinanceOrder" + i + ","
					+ "[FinanceProject]=@FinanceProject" + i + ","
					+ "[IsDefault]=@IsDefault" + i + ","
					+ "[LastEditTime]=@LastEditTime" + i + ","
					+ "[LastEditUserId]=@LastEditUserId" + i + ","
					+ "[MainAccessProfileId]=@MainAccessProfileId" + i + ","
					+ "[ModuleActivated]=@ModuleActivated" + i + ","
					+ "[Project]=@Project" + i + ","
					+ "[ProjectExternalEditAllGroup]=@ProjectExternalEditAllGroup" + i + ","
					+ "[ProjectExternalEditAllSite]=@ProjectExternalEditAllSite" + i + ","
					+ "[ProjectExternalViewAllGroup]=@ProjectExternalViewAllGroup" + i + ","
					+ "[ProjectExternalViewAllSite]=@ProjectExternalViewAllSite" + i + ","
					+ "[ProjectInternalEditAllGroup]=@ProjectInternalEditAllGroup" + i + ","
					+ "[ProjectInternalEditAllSite]=@ProjectInternalEditAllSite" + i + ","
					+ "[ProjectInternalViewAllGroup]=@ProjectInternalViewAllGroup" + i + ","
					+ "[ProjectInternalViewAllSite]=@ProjectInternalViewAllSite" + i + ","
					+ "[Receptions]=@Receptions" + i + ","
					+ "[ReceptionsEdit]=@ReceptionsEdit" + i + ","
					+ "[ReceptionsView]=@ReceptionsView" + i + ","
					+ "[Statistics]=@Statistics" + i + ","
					+ "[StatisticsViewAll]=@StatisticsViewAll" + i + ","
					+ "[Suppliers]=@Suppliers" + i + ","
					+ "[Units]=@Units" + i + ","
					+ "[UpdateExternalCommande]=@UpdateExternalCommande" + i + ","
					+ "[UpdateExternalProject]=@UpdateExternalProject" + i + ","
					+ "[UpdateInternalCommande]=@UpdateInternalCommande" + i + ","
					+ "[UpdateInternalProject]=@UpdateInternalProject" + i + ","
					+ "[UpdateKontenrahmenAccounting]=@UpdateKontenrahmenAccounting" + i + ","
					+ "[UpdateZahlungskonditionenKundenAccounting]=@UpdateZahlungskonditionenKundenAccounting" + i + ","
					+ "[UpdateZahlungskonditionenLieferantenAccounting]=@UpdateZahlungskonditionenLieferantenAccounting" + i + ","
					+ "[ViewAusfuhrAccounting]=@ViewAusfuhrAccounting" + i + ","
					+ "[ViewEinfuhrAccounting]=@ViewEinfuhrAccounting" + i + ","
					+ "[ViewExternalCommande]=@ViewExternalCommande" + i + ","
					+ "[ViewExternalProject]=@ViewExternalProject" + i + ","
					+ "[ViewFactoringRgGutschriftlistAccounting]=@ViewFactoringRgGutschriftlistAccounting" + i + ","
					+ "[ViewInternalCommande]=@ViewInternalCommande" + i + ","
					+ "[ViewInternalProject]=@ViewInternalProject" + i + ","
					+ "[ViewKontenrahmenAccounting]=@ViewKontenrahmenAccounting" + i + ","
					+ "[ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting]=@ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting" + i + ","
					+ "[ViewLiquiditatsplanungSkontozahlerAccounting]=@ViewLiquiditatsplanungSkontozahlerAccounting" + i + ","
					+ "[ViewRechnungsTransferAccounting]=@ViewRechnungsTransferAccounting" + i + ","
					+ "[ViewRMDCZAccounting]=@ViewRMDCZAccounting" + i + ","
					+ "[ViewStammdatenkontrolleWareneingangeAccounting]=@ViewStammdatenkontrolleWareneingangeAccounting" + i + ","
					+ "[ViewZahlungskonditionenKundenAccounting]=@ViewZahlungskonditionenKundenAccounting" + i + ","
					+ "[ViewZahlungskonditionenLieferantenAccounting]=@ViewZahlungskonditionenLieferantenAccounting" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Accounting" + i, item.Accounting == null ? (object)DBNull.Value : item.Accounting);
					sqlCommand.Parameters.AddWithValue("AddExternalCommande" + i, item.AddExternalCommande == null ? (object)DBNull.Value : item.AddExternalCommande);
					sqlCommand.Parameters.AddWithValue("AddExternalProject" + i, item.AddExternalProject == null ? (object)DBNull.Value : item.AddExternalProject);
					sqlCommand.Parameters.AddWithValue("AddInternalCommande" + i, item.AddInternalCommande == null ? (object)DBNull.Value : item.AddInternalCommande);
					sqlCommand.Parameters.AddWithValue("AddInternalProject" + i, item.AddInternalProject == null ? (object)DBNull.Value : item.AddInternalProject);
					sqlCommand.Parameters.AddWithValue("AddKontenrahmenAccounting" + i, item.AddKontenrahmenAccounting == null ? (object)DBNull.Value : item.AddKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles" + i, item.AdministrationAccessProfiles == null ? (object)DBNull.Value : item.AdministrationAccessProfiles);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate" + i, item.AdministrationAccessProfilesUpdate == null ? (object)DBNull.Value : item.AdministrationAccessProfilesUpdate);
					sqlCommand.Parameters.AddWithValue("AdministrationUser" + i, item.AdministrationUser == null ? (object)DBNull.Value : item.AdministrationUser);
					sqlCommand.Parameters.AddWithValue("AdministrationUserUpdate" + i, item.AdministrationUserUpdate == null ? (object)DBNull.Value : item.AdministrationUserUpdate);
					sqlCommand.Parameters.AddWithValue("Article" + i, item.Article == null ? (object)DBNull.Value : item.Article);
					sqlCommand.Parameters.AddWithValue("Assign" + i, item.Assign == null ? (object)DBNull.Value : item.Assign);
					sqlCommand.Parameters.AddWithValue("AssignAllDepartments" + i, item.AssignAllDepartments == null ? (object)DBNull.Value : item.AssignAllDepartments);
					sqlCommand.Parameters.AddWithValue("AssignAllEmployees" + i, item.AssignAllEmployees == null ? (object)DBNull.Value : item.AssignAllEmployees);
					sqlCommand.Parameters.AddWithValue("AssignAllSites" + i, item.AssignAllSites == null ? (object)DBNull.Value : item.AssignAllSites);
					sqlCommand.Parameters.AddWithValue("AssignCreateDept" + i, item.AssignCreateDept == null ? (object)DBNull.Value : item.AssignCreateDept);
					sqlCommand.Parameters.AddWithValue("AssignCreateLand" + i, item.AssignCreateLand == null ? (object)DBNull.Value : item.AssignCreateLand);
					sqlCommand.Parameters.AddWithValue("AssignCreateUser" + i, item.AssignCreateUser == null ? (object)DBNull.Value : item.AssignCreateUser);
					sqlCommand.Parameters.AddWithValue("AssignDeleteDept" + i, item.AssignDeleteDept == null ? (object)DBNull.Value : item.AssignDeleteDept);
					sqlCommand.Parameters.AddWithValue("AssignDeleteLand" + i, item.AssignDeleteLand == null ? (object)DBNull.Value : item.AssignDeleteLand);
					sqlCommand.Parameters.AddWithValue("AssignDeleteUser" + i, item.AssignDeleteUser == null ? (object)DBNull.Value : item.AssignDeleteUser);
					sqlCommand.Parameters.AddWithValue("AssignEditDept" + i, item.AssignEditDept == null ? (object)DBNull.Value : item.AssignEditDept);
					sqlCommand.Parameters.AddWithValue("AssignEditLand" + i, item.AssignEditLand == null ? (object)DBNull.Value : item.AssignEditLand);
					sqlCommand.Parameters.AddWithValue("AssignEditUser" + i, item.AssignEditUser == null ? (object)DBNull.Value : item.AssignEditUser);
					sqlCommand.Parameters.AddWithValue("AssignViewDept" + i, item.AssignViewDept == null ? (object)DBNull.Value : item.AssignViewDept);
					sqlCommand.Parameters.AddWithValue("AssignViewLand" + i, item.AssignViewLand == null ? (object)DBNull.Value : item.AssignViewLand);
					sqlCommand.Parameters.AddWithValue("AssignViewUser" + i, item.AssignViewUser == null ? (object)DBNull.Value : item.AssignViewUser);
					sqlCommand.Parameters.AddWithValue("Budget" + i, item.Budget);
					sqlCommand.Parameters.AddWithValue("CashLiquidity" + i, item.CashLiquidity);
					sqlCommand.Parameters.AddWithValue("Commande" + i, item.Commande == null ? (object)DBNull.Value : item.Commande);
					sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllGroup" + i, item.CommandeExternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeExternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeExternalEditAllSite" + i, item.CommandeExternalEditAllSite == null ? (object)DBNull.Value : item.CommandeExternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllGroup" + i, item.CommandeExternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewAllSite" + i, item.CommandeExternalViewAllSite == null ? (object)DBNull.Value : item.CommandeExternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoice" + i, item.CommandeExternalViewInvoice == null ? (object)DBNull.Value : item.CommandeExternalViewInvoice);
					sqlCommand.Parameters.AddWithValue("CommandeExternalViewInvoiceAllGroup" + i, item.CommandeExternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeExternalViewInvoiceAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllDepartment" + i, item.CommandeInternalEditAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalEditAllDepartment);
					sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllGroup" + i, item.CommandeInternalEditAllGroup == null ? (object)DBNull.Value : item.CommandeInternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeInternalEditAllSite" + i, item.CommandeInternalEditAllSite == null ? (object)DBNull.Value : item.CommandeInternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllDepartment" + i, item.CommandeInternalViewAllDepartment == null ? (object)DBNull.Value : item.CommandeInternalViewAllDepartment);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllGroup" + i, item.CommandeInternalViewAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewAllSite" + i, item.CommandeInternalViewAllSite == null ? (object)DBNull.Value : item.CommandeInternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoice" + i, item.CommandeInternalViewInvoice == null ? (object)DBNull.Value : item.CommandeInternalViewInvoice);
					sqlCommand.Parameters.AddWithValue("CommandeInternalViewInvoiceAllGroup" + i, item.CommandeInternalViewInvoiceAllGroup == null ? (object)DBNull.Value : item.CommandeInternalViewInvoiceAllGroup);
					sqlCommand.Parameters.AddWithValue("Config" + i, item.Config == null ? (object)DBNull.Value : item.Config);
					sqlCommand.Parameters.AddWithValue("ConfigCreateArtikel" + i, item.ConfigCreateArtikel == null ? (object)DBNull.Value : item.ConfigCreateArtikel);
					sqlCommand.Parameters.AddWithValue("ConfigCreateDept" + i, item.ConfigCreateDept == null ? (object)DBNull.Value : item.ConfigCreateDept);
					sqlCommand.Parameters.AddWithValue("ConfigCreateLand" + i, item.ConfigCreateLand == null ? (object)DBNull.Value : item.ConfigCreateLand);
					sqlCommand.Parameters.AddWithValue("ConfigCreateSupplier" + i, item.ConfigCreateSupplier == null ? (object)DBNull.Value : item.ConfigCreateSupplier);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteArtikel" + i, item.ConfigDeleteArtikel == null ? (object)DBNull.Value : item.ConfigDeleteArtikel);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteDept" + i, item.ConfigDeleteDept == null ? (object)DBNull.Value : item.ConfigDeleteDept);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteLand" + i, item.ConfigDeleteLand == null ? (object)DBNull.Value : item.ConfigDeleteLand);
					sqlCommand.Parameters.AddWithValue("ConfigDeleteSupplier" + i, item.ConfigDeleteSupplier == null ? (object)DBNull.Value : item.ConfigDeleteSupplier);
					sqlCommand.Parameters.AddWithValue("ConfigEditArtikel" + i, item.ConfigEditArtikel == null ? (object)DBNull.Value : item.ConfigEditArtikel);
					sqlCommand.Parameters.AddWithValue("ConfigEditDept" + i, item.ConfigEditDept == null ? (object)DBNull.Value : item.ConfigEditDept);
					sqlCommand.Parameters.AddWithValue("ConfigEditLand" + i, item.ConfigEditLand == null ? (object)DBNull.Value : item.ConfigEditLand);
					sqlCommand.Parameters.AddWithValue("ConfigEditSupplier" + i, item.ConfigEditSupplier == null ? (object)DBNull.Value : item.ConfigEditSupplier);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreditManagement" + i, item.CreditManagement);
					sqlCommand.Parameters.AddWithValue("DeleteExternalCommande" + i, item.DeleteExternalCommande == null ? (object)DBNull.Value : item.DeleteExternalCommande);
					sqlCommand.Parameters.AddWithValue("DeleteExternalProject" + i, item.DeleteExternalProject == null ? (object)DBNull.Value : item.DeleteExternalProject);
					sqlCommand.Parameters.AddWithValue("DeleteInternalCommande" + i, item.DeleteInternalCommande == null ? (object)DBNull.Value : item.DeleteInternalCommande);
					sqlCommand.Parameters.AddWithValue("DeleteInternalProject" + i, item.DeleteInternalProject == null ? (object)DBNull.Value : item.DeleteInternalProject);
					sqlCommand.Parameters.AddWithValue("DeleteKontenrahmenAccounting" + i, item.DeleteKontenrahmenAccounting == null ? (object)DBNull.Value : item.DeleteKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenKundenAccounting" + i, item.DeleteZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenKundenAccounting);
					sqlCommand.Parameters.AddWithValue("DeleteZahlungskonditionenLieferantenAccounting" + i, item.DeleteZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.DeleteZahlungskonditionenLieferantenAccounting);
					sqlCommand.Parameters.AddWithValue("FinanceOrder" + i, item.FinanceOrder == null ? (object)DBNull.Value : item.FinanceOrder);
					sqlCommand.Parameters.AddWithValue("FinanceProject" + i, item.FinanceProject == null ? (object)DBNull.Value : item.FinanceProject);
					sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("MainAccessProfileId" + i, item.MainAccessProfileId == null ? (object)DBNull.Value : item.MainAccessProfileId);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("Project" + i, item.Project == null ? (object)DBNull.Value : item.Project);
					sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllGroup" + i, item.ProjectExternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectExternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectExternalEditAllSite" + i, item.ProjectExternalEditAllSite == null ? (object)DBNull.Value : item.ProjectExternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllGroup" + i, item.ProjectExternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectExternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectExternalViewAllSite" + i, item.ProjectExternalViewAllSite == null ? (object)DBNull.Value : item.ProjectExternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllGroup" + i, item.ProjectInternalEditAllGroup == null ? (object)DBNull.Value : item.ProjectInternalEditAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectInternalEditAllSite" + i, item.ProjectInternalEditAllSite == null ? (object)DBNull.Value : item.ProjectInternalEditAllSite);
					sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllGroup" + i, item.ProjectInternalViewAllGroup == null ? (object)DBNull.Value : item.ProjectInternalViewAllGroup);
					sqlCommand.Parameters.AddWithValue("ProjectInternalViewAllSite" + i, item.ProjectInternalViewAllSite == null ? (object)DBNull.Value : item.ProjectInternalViewAllSite);
					sqlCommand.Parameters.AddWithValue("Receptions" + i, item.Receptions == null ? (object)DBNull.Value : item.Receptions);
					sqlCommand.Parameters.AddWithValue("ReceptionsEdit" + i, item.ReceptionsEdit == null ? (object)DBNull.Value : item.ReceptionsEdit);
					sqlCommand.Parameters.AddWithValue("ReceptionsView" + i, item.ReceptionsView == null ? (object)DBNull.Value : item.ReceptionsView);
					sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatisticsViewAll" + i, item.StatisticsViewAll == null ? (object)DBNull.Value : item.StatisticsViewAll);
					sqlCommand.Parameters.AddWithValue("Suppliers" + i, item.Suppliers == null ? (object)DBNull.Value : item.Suppliers);
					sqlCommand.Parameters.AddWithValue("Units" + i, item.Units == null ? (object)DBNull.Value : item.Units);
					sqlCommand.Parameters.AddWithValue("UpdateExternalCommande" + i, item.UpdateExternalCommande == null ? (object)DBNull.Value : item.UpdateExternalCommande);
					sqlCommand.Parameters.AddWithValue("UpdateExternalProject" + i, item.UpdateExternalProject == null ? (object)DBNull.Value : item.UpdateExternalProject);
					sqlCommand.Parameters.AddWithValue("UpdateInternalCommande" + i, item.UpdateInternalCommande == null ? (object)DBNull.Value : item.UpdateInternalCommande);
					sqlCommand.Parameters.AddWithValue("UpdateInternalProject" + i, item.UpdateInternalProject == null ? (object)DBNull.Value : item.UpdateInternalProject);
					sqlCommand.Parameters.AddWithValue("UpdateKontenrahmenAccounting" + i, item.UpdateKontenrahmenAccounting == null ? (object)DBNull.Value : item.UpdateKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenKundenAccounting" + i, item.UpdateZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenKundenAccounting);
					sqlCommand.Parameters.AddWithValue("UpdateZahlungskonditionenLieferantenAccounting" + i, item.UpdateZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.UpdateZahlungskonditionenLieferantenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewAusfuhrAccounting" + i, item.ViewAusfuhrAccounting == null ? (object)DBNull.Value : item.ViewAusfuhrAccounting);
					sqlCommand.Parameters.AddWithValue("ViewEinfuhrAccounting" + i, item.ViewEinfuhrAccounting == null ? (object)DBNull.Value : item.ViewEinfuhrAccounting);
					sqlCommand.Parameters.AddWithValue("ViewExternalCommande" + i, item.ViewExternalCommande == null ? (object)DBNull.Value : item.ViewExternalCommande);
					sqlCommand.Parameters.AddWithValue("ViewExternalProject" + i, item.ViewExternalProject == null ? (object)DBNull.Value : item.ViewExternalProject);
					sqlCommand.Parameters.AddWithValue("ViewFactoringRgGutschriftlistAccounting" + i, item.ViewFactoringRgGutschriftlistAccounting == null ? (object)DBNull.Value : item.ViewFactoringRgGutschriftlistAccounting);
					sqlCommand.Parameters.AddWithValue("ViewInternalCommande" + i, item.ViewInternalCommande == null ? (object)DBNull.Value : item.ViewInternalCommande);
					sqlCommand.Parameters.AddWithValue("ViewInternalProject" + i, item.ViewInternalProject == null ? (object)DBNull.Value : item.ViewInternalProject);
					sqlCommand.Parameters.AddWithValue("ViewKontenrahmenAccounting" + i, item.ViewKontenrahmenAccounting == null ? (object)DBNull.Value : item.ViewKontenrahmenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting" + i, item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewLiquiditatsplanungSkontozahlerAccounting" + i, item.ViewLiquiditatsplanungSkontozahlerAccounting == null ? (object)DBNull.Value : item.ViewLiquiditatsplanungSkontozahlerAccounting);
					sqlCommand.Parameters.AddWithValue("ViewRechnungsTransferAccounting" + i, item.ViewRechnungsTransferAccounting == null ? (object)DBNull.Value : item.ViewRechnungsTransferAccounting);
					sqlCommand.Parameters.AddWithValue("ViewRMDCZAccounting" + i, item.ViewRMDCZAccounting == null ? (object)DBNull.Value : item.ViewRMDCZAccounting);
					sqlCommand.Parameters.AddWithValue("ViewStammdatenkontrolleWareneingangeAccounting" + i, item.ViewStammdatenkontrolleWareneingangeAccounting == null ? (object)DBNull.Value : item.ViewStammdatenkontrolleWareneingangeAccounting);
					sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenKundenAccounting" + i, item.ViewZahlungskonditionenKundenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenKundenAccounting);
					sqlCommand.Parameters.AddWithValue("ViewZahlungskonditionenLieferantenAccounting" + i, item.ViewZahlungskonditionenLieferantenAccounting == null ? (object)DBNull.Value : item.ViewZahlungskonditionenLieferantenAccounting);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__FNC_AccessProfile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}
			return -1;
		}
		private static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [__FNC_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Entities.Tables.FNC.AccessProfileEntity> GetByMainAccessProfilesIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.FNC.AccessProfileEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					response = getByMainAccessProfilesIds(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					response = new List<Entities.Tables.FNC.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(getByMainAccessProfilesIds(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					response.AddRange(getByMainAccessProfilesIds(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return response;
			}
			return new List<Entities.Tables.FNC.AccessProfileEntity>();
		}
		private static List<Entities.Tables.FNC.AccessProfileEntity> getByMainAccessProfilesIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [__FNC_AccessProfile] WHERE [MainAccessProfileId] IN (" + queryIds + ")";

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.FNC.AccessProfileEntity>();
				}
			}
			return new List<Entities.Tables.FNC.AccessProfileEntity>();
		}
		public static Entities.Tables.FNC.AccessProfileEntity GetByName(string name)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_AccessProfile] WHERE TRIM([AccessProfileName])=@name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name.Trim());

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int UpdateMinimal(Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_AccessProfile] SET  [Administration]=@Administration, [AdministrationAccessProfiles]=@AdministrationAccessProfiles, "
						+ "[AdministrationAccessProfilesUpdate]=@AdministrationAccessProfilesUpdate, [AdministrationUser]=@AdministrationUser, "
						+ "[AdministrationUserUpdate]=@AdministrationUserUpdate, [Budget]=@Budget, [CreditManagement]=@CreditManagement,"
						+ "[CashLiquidity]=@CashLiquidity, [AccessProfileName]=@AccessProfileName, [ModuleActivated]=@ModuleActivated,[Accounting]=@Accounting WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
				sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles", item.AdministrationAccessProfiles == null ? (object)DBNull.Value : item.AdministrationAccessProfiles);
				sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate", item.AdministrationAccessProfilesUpdate == null ? (object)DBNull.Value : item.AdministrationAccessProfilesUpdate);
				sqlCommand.Parameters.AddWithValue("AdministrationUser", item.AdministrationUser == null ? (object)DBNull.Value : item.AdministrationUser);
				sqlCommand.Parameters.AddWithValue("AdministrationUserUpdate", item.AdministrationUserUpdate == null ? (object)DBNull.Value : item.AdministrationUserUpdate);
				sqlCommand.Parameters.AddWithValue("Budget", item.Budget);
				sqlCommand.Parameters.AddWithValue("CashLiquidity", item.CashLiquidity);
				sqlCommand.Parameters.AddWithValue("CreditManagement", item.CreditManagement);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("Accounting", item.Accounting == null ? (object)DBNull.Value : item.Accounting);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateName(Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_AccessProfile] SET [AccessProfileName]=@AccessProfileName, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateDefault(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_AccessProfile] SET [IsDefault]=1-ISNULL([IsDefault],0) WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", id);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> GetDefaultProfiles(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__FNC_AccessProfile] WHERE [IsDefault]=1", connection, transaction))
			{
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>();
			}
		}
		#endregion Custom Methods
	}
}