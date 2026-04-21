namespace Infrastructure.Data.Access.Tables.BSD
{
	public class AccessProfileAccess
	{
		#region Default Methods
		public static Entities.Tables.BSD.AccessProfileEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.BSD.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.BSD.AccessProfileEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_AccessProfile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.BSD.AccessProfileEntity>();
			}
		}
		public static List<Entities.Tables.BSD.AccessProfileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.BSD.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.BSD.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Entities.Tables.BSD.AccessProfileEntity>();
		}
		private static List<Entities.Tables.BSD.AccessProfileEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_AccessProfile] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.BSD.AccessProfileEntity>();
				}
			}
			return new List<Entities.Tables.BSD.AccessProfileEntity>();
		}

		public static int Insert(Entities.Tables.BSD.AccessProfileEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_AccessProfile] ([AccessProfileName],[AddArticle],[AddArticleReference],[AddBCR],[AddCocType],[AddConditionAssignment],[AddContactAddress],[AddContactSalutation],[AddCurrencies],[AddCustomer],[AddCustomerGroup],[AddCustomerItemNumber],[AddDiscountGroup],[AddEdiConcern],[AddFibuFrame],[AddFiles],[AddHourlyRate],[AddIndustry],[AddPayementPractises],[AddPricingGroup],[AddRohArtikelNummer],[AddShippingMethods],[AddSlipCircle],[AddSupplier],[AddSupplierGroup],[AddTermsOfPayment],[Administration],[AltPositionsArticleBOM],[ArchiveArticle],[ArchiveCustomer],[ArchiveSupplier],[ArticleAddCustomerDocument],[ArticleBOM],[ArticleCts],[ArticleData],[ArticleDeleteCustomerDocument],[ArticleLogistics],[ArticleLogisticsPrices],[ArticleOverview],[ArticleProduction],[ArticlePurchase],[ArticleQuality],[Articles],[ArticleSales],[ArticleSalesCustom],[ArticleSalesItem],[ArticlesBOMCPControlEngineering],[ArticlesBOMCPControlHistory],[ArticlesBOMCPControlQuality],[ArticleStatistics],[ArticleStatisticsEngineering],[ArticleStatisticsEngineeringEdit],[ArticleStatisticsFinanceAccounting],[ArticleStatisticsFinanceAccountingEdit],[ArticleStatisticsLogistics],[ArticleStatisticsLogisticsEdit],[ArticleStatisticsPurchase],[ArticleStatisticsPurchaseEdit],[ArticleStatisticsTechnic],[ArticleStatisticsTechnicEdit],[BomChangeRequests],[CocType],[ConditionAssignment],[ConfigArticle],[ConfigCustomer],[ConfigSupplier],[ContactAddress],[ContactSalutation],[CreateArticlePurchase],[CreateArticleSalesCustom],[CreateArticleSalesItem],[CreateCustomerContactPerson],[CreateSupplierContactPerson],[CreationTime],[CreationUserId],[Currencies],[CustomerAddress],[CustomerCommunication],[CustomerContactPerson],[CustomerData],[CustomerGroup],[CustomerHistory],[CustomerItemNumber],[CustomerOverview],[Customers],[CustomerShipping],[DeleteAltPositionsArticleBOM],[DeleteArticle],[DeleteArticleBOM],[DeleteArticlePurchase],[DeleteArticleSalesCustom],[DeleteArticleSalesItem],[DeleteBCR],[DeleteCustomerContactPerson],[DeleteFiles],[DeleteRohArtikelNummer],[DeleteSupplierContactPerson],[DiscountGroup],[DownloadAllOutdatedEinkaufsPreis],[DownloadFiles],[DownloadOutdatedEinkaufsPreis],[EdiConcern],[EditAltPositionsArticleBOM],[EditArticle],[EditArticleBOM],[EditArticleBOMPosition],[EditArticleCts],[EditArticleData],[EditArticleDesignation],[EditArticleImage],[EditArticleLogistics],[EditArticleManager],[EditArticleProduction],[EditArticlePurchase],[EditArticleQuality],[EditArticleReference],[EditArticleSalesCustom],[EditArticleSalesItem],[EditCocType],[EditConditionAssignment],[EditContactAddress],[EditContactSalutation],[EditCurrencies],[EditCustomer],[EditCustomerAddress],[EditCustomerCommunication],[EditCustomerContactPerson],[EditCustomerCoordination],[EditCustomerData],[EditCustomerGroup],[EditCustomerImage],[EditCustomerItemNumber],[EditCustomerShipping],[EditDiscountGroup],[EditEdiConcern],[EditFibuFrame],[EditHourlyRate],[EditIndustry],[EditLagerCCID],[EditLagerMinStock],[EditLagerOrderProposal],[EditLagerStock],[EditPayementPractises],[EditPricingGroup],[EditRohArtikelNummer],[EditShippingMethods],[EditSlipCircle],[EditSupplier],[EditSupplierAddress],[EditSupplierCommunication],[EditSupplierContactPerson],[EditSupplierCoordination],[EditSupplierData],[EditSupplierGroup],[EditSupplierImage],[EditSupplierShipping],[EditTermsOfPayment],[EDrawingEdit],[EinkaufsPreisUpdate],[FibuFrame],[GetRohArtikelNummer],[HourlyRate],[ImportArticleBOM],[Industry],[isDefault],[LagerArticleLogistics],[ModuleActivated],[ModuleAdministrator],[offer],[OfferRequestADD],[OfferRequestApplyPrice],[OfferRequestDelete],[OfferRequestEdit],[OfferRequestEditEmail],[OfferRequestSendEmail],[OfferRequestView],[PackagingsLgtPhotoAdd],[PackagingsLgtPhotoDelete],[PackagingsLgtPhotoView],[PayementPractises],[PMAddCable],[PMAddMileStone],[PMAddProject],[PMDeleteCable],[PMDeleteMileStone],[PMDeleteProject],[PMEditCable],[PMEditMileStone],[PMEditProject],[PMModule],[PMViewProjectsCompact],[PMViewProjectsDetail],[PMViewProjectsMedium],[PricingGroup],[RemoveArticleReference],[Settings],[ShippingMethods],[SlipCircle],[SupplierAddress],[SupplierAttachementAddFile],[SupplierAttachementGetFile],[SupplierAttachementRemoveFile],[SupplierCommunication],[SupplierContactPerson],[SupplierData],[SupplierGroup],[SupplierHistory],[SupplierOverview],[Suppliers],[SupplierShipping],[TermsOfPayment],[UploadAltPositionsArticleBOM],[UploadArticleBOM],[ValidateArticleBOM],[ValidateBCR],[ViewAltPositionsArticleBOM],[ViewArticleLog],[ViewArticleReference],[ViewArticles],[ViewBCR],[ViewCustomers],[ViewLPCustomer],[ViewLPSupplier],[ViewSupplierAddressComments],[ViewSuppliers]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileName,@AddArticle,@AddArticleReference,@AddBCR,@AddCocType,@AddConditionAssignment,@AddContactAddress,@AddContactSalutation,@AddCurrencies,@AddCustomer,@AddCustomerGroup,@AddCustomerItemNumber,@AddDiscountGroup,@AddEdiConcern,@AddFibuFrame,@AddFiles,@AddHourlyRate,@AddIndustry,@AddPayementPractises,@AddPricingGroup,@AddRohArtikelNummer,@AddShippingMethods,@AddSlipCircle,@AddSupplier,@AddSupplierGroup,@AddTermsOfPayment,@Administration,@AltPositionsArticleBOM,@ArchiveArticle,@ArchiveCustomer,@ArchiveSupplier,@ArticleAddCustomerDocument,@ArticleBOM,@ArticleCts,@ArticleData,@ArticleDeleteCustomerDocument,@ArticleLogistics,@ArticleLogisticsPrices,@ArticleOverview,@ArticleProduction,@ArticlePurchase,@ArticleQuality,@Articles,@ArticleSales,@ArticleSalesCustom,@ArticleSalesItem,@ArticlesBOMCPControlEngineering,@ArticlesBOMCPControlHistory,@ArticlesBOMCPControlQuality,@ArticleStatistics,@ArticleStatisticsEngineering,@ArticleStatisticsEngineeringEdit,@ArticleStatisticsFinanceAccounting,@ArticleStatisticsFinanceAccountingEdit,@ArticleStatisticsLogistics,@ArticleStatisticsLogisticsEdit,@ArticleStatisticsPurchase,@ArticleStatisticsPurchaseEdit,@ArticleStatisticsTechnic,@ArticleStatisticsTechnicEdit,@BomChangeRequests,@CocType,@ConditionAssignment,@ConfigArticle,@ConfigCustomer,@ConfigSupplier,@ContactAddress,@ContactSalutation,@CreateArticlePurchase,@CreateArticleSalesCustom,@CreateArticleSalesItem,@CreateCustomerContactPerson,@CreateSupplierContactPerson,@CreationTime,@CreationUserId,@Currencies,@CustomerAddress,@CustomerCommunication,@CustomerContactPerson,@CustomerData,@CustomerGroup,@CustomerHistory,@CustomerItemNumber,@CustomerOverview,@Customers,@CustomerShipping,@DeleteAltPositionsArticleBOM,@DeleteArticle,@DeleteArticleBOM,@DeleteArticlePurchase,@DeleteArticleSalesCustom,@DeleteArticleSalesItem,@DeleteBCR,@DeleteCustomerContactPerson,@DeleteFiles,@DeleteRohArtikelNummer,@DeleteSupplierContactPerson,@DiscountGroup,@DownloadAllOutdatedEinkaufsPreis,@DownloadFiles,@DownloadOutdatedEinkaufsPreis,@EdiConcern,@EditAltPositionsArticleBOM,@EditArticle,@EditArticleBOM,@EditArticleBOMPosition,@EditArticleCts,@EditArticleData,@EditArticleDesignation,@EditArticleImage,@EditArticleLogistics,@EditArticleManager,@EditArticleProduction,@EditArticlePurchase,@EditArticleQuality,@EditArticleReference,@EditArticleSalesCustom,@EditArticleSalesItem,@EditCocType,@EditConditionAssignment,@EditContactAddress,@EditContactSalutation,@EditCurrencies,@EditCustomer,@EditCustomerAddress,@EditCustomerCommunication,@EditCustomerContactPerson,@EditCustomerCoordination,@EditCustomerData,@EditCustomerGroup,@EditCustomerImage,@EditCustomerItemNumber,@EditCustomerShipping,@EditDiscountGroup,@EditEdiConcern,@EditFibuFrame,@EditHourlyRate,@EditIndustry,@EditLagerCCID,@EditLagerMinStock,@EditLagerOrderProposal,@EditLagerStock,@EditPayementPractises,@EditPricingGroup,@EditRohArtikelNummer,@EditShippingMethods,@EditSlipCircle,@EditSupplier,@EditSupplierAddress,@EditSupplierCommunication,@EditSupplierContactPerson,@EditSupplierCoordination,@EditSupplierData,@EditSupplierGroup,@EditSupplierImage,@EditSupplierShipping,@EditTermsOfPayment,@EDrawingEdit,@EinkaufsPreisUpdate,@FibuFrame,@GetRohArtikelNummer,@HourlyRate,@ImportArticleBOM,@Industry,@isDefault,@LagerArticleLogistics,@ModuleActivated,@ModuleAdministrator,@offer,@OfferRequestADD,@OfferRequestApplyPrice,@OfferRequestDelete,@OfferRequestEdit,@OfferRequestEditEmail,@OfferRequestSendEmail,@OfferRequestView,@PackagingsLgtPhotoAdd,@PackagingsLgtPhotoDelete,@PackagingsLgtPhotoView,@PayementPractises,@PMAddCable,@PMAddMileStone,@PMAddProject,@PMDeleteCable,@PMDeleteMileStone,@PMDeleteProject,@PMEditCable,@PMEditMileStone,@PMEditProject,@PMModule,@PMViewProjectsCompact,@PMViewProjectsDetail,@PMViewProjectsMedium,@PricingGroup,@RemoveArticleReference,@Settings,@ShippingMethods,@SlipCircle,@SupplierAddress,@SupplierAttachementAddFile,@SupplierAttachementGetFile,@SupplierAttachementRemoveFile,@SupplierCommunication,@SupplierContactPerson,@SupplierData,@SupplierGroup,@SupplierHistory,@SupplierOverview,@Suppliers,@SupplierShipping,@TermsOfPayment,@UploadAltPositionsArticleBOM,@UploadArticleBOM,@ValidateArticleBOM,@ValidateBCR,@ViewAltPositionsArticleBOM,@ViewArticleLog,@ViewArticleReference,@ViewArticles,@ViewBCR,@ViewCustomers,@ViewLPCustomer,@ViewLPSupplier,@ViewSupplierAddressComments,@ViewSuppliers); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("AddArticle", item.AddArticle);
					sqlCommand.Parameters.AddWithValue("AddArticleReference", item.AddArticleReference == null ? (object)DBNull.Value : item.AddArticleReference);
					sqlCommand.Parameters.AddWithValue("AddBCR", item.AddBCR == null ? (object)DBNull.Value : item.AddBCR);
					sqlCommand.Parameters.AddWithValue("AddCocType", item.AddCocType == null ? (object)DBNull.Value : item.AddCocType);
					sqlCommand.Parameters.AddWithValue("AddConditionAssignment", item.AddConditionAssignment == null ? (object)DBNull.Value : item.AddConditionAssignment);
					sqlCommand.Parameters.AddWithValue("AddContactAddress", item.AddContactAddress == null ? (object)DBNull.Value : item.AddContactAddress);
					sqlCommand.Parameters.AddWithValue("AddContactSalutation", item.AddContactSalutation == null ? (object)DBNull.Value : item.AddContactSalutation);
					sqlCommand.Parameters.AddWithValue("AddCurrencies", item.AddCurrencies == null ? (object)DBNull.Value : item.AddCurrencies);
					sqlCommand.Parameters.AddWithValue("AddCustomer", item.AddCustomer == null ? (object)DBNull.Value : item.AddCustomer);
					sqlCommand.Parameters.AddWithValue("AddCustomerGroup", item.AddCustomerGroup == null ? (object)DBNull.Value : item.AddCustomerGroup);
					sqlCommand.Parameters.AddWithValue("AddCustomerItemNumber", item.AddCustomerItemNumber == null ? (object)DBNull.Value : item.AddCustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("AddDiscountGroup", item.AddDiscountGroup == null ? (object)DBNull.Value : item.AddDiscountGroup);
					sqlCommand.Parameters.AddWithValue("AddEdiConcern", item.AddEdiConcern == null ? (object)DBNull.Value : item.AddEdiConcern);
					sqlCommand.Parameters.AddWithValue("AddFibuFrame", item.AddFibuFrame == null ? (object)DBNull.Value : item.AddFibuFrame);
					sqlCommand.Parameters.AddWithValue("AddFiles", item.AddFiles == null ? (object)DBNull.Value : item.AddFiles);
					sqlCommand.Parameters.AddWithValue("AddHourlyRate", item.AddHourlyRate == null ? (object)DBNull.Value : item.AddHourlyRate);
					sqlCommand.Parameters.AddWithValue("AddIndustry", item.AddIndustry == null ? (object)DBNull.Value : item.AddIndustry);
					sqlCommand.Parameters.AddWithValue("AddPayementPractises", item.AddPayementPractises == null ? (object)DBNull.Value : item.AddPayementPractises);
					sqlCommand.Parameters.AddWithValue("AddPricingGroup", item.AddPricingGroup == null ? (object)DBNull.Value : item.AddPricingGroup);
					sqlCommand.Parameters.AddWithValue("AddRohArtikelNummer", item.AddRohArtikelNummer == null ? (object)DBNull.Value : item.AddRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("AddShippingMethods", item.AddShippingMethods == null ? (object)DBNull.Value : item.AddShippingMethods);
					sqlCommand.Parameters.AddWithValue("AddSlipCircle", item.AddSlipCircle == null ? (object)DBNull.Value : item.AddSlipCircle);
					sqlCommand.Parameters.AddWithValue("AddSupplier", item.AddSupplier == null ? (object)DBNull.Value : item.AddSupplier);
					sqlCommand.Parameters.AddWithValue("AddSupplierGroup", item.AddSupplierGroup == null ? (object)DBNull.Value : item.AddSupplierGroup);
					sqlCommand.Parameters.AddWithValue("AddTermsOfPayment", item.AddTermsOfPayment == null ? (object)DBNull.Value : item.AddTermsOfPayment);
					sqlCommand.Parameters.AddWithValue("Administration", item.Administration);
					sqlCommand.Parameters.AddWithValue("AltPositionsArticleBOM", item.AltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("ArchiveArticle", item.ArchiveArticle);
					sqlCommand.Parameters.AddWithValue("ArchiveCustomer", item.ArchiveCustomer == null ? (object)DBNull.Value : item.ArchiveCustomer);
					sqlCommand.Parameters.AddWithValue("ArchiveSupplier", item.ArchiveSupplier == null ? (object)DBNull.Value : item.ArchiveSupplier);
					sqlCommand.Parameters.AddWithValue("ArticleAddCustomerDocument", item.ArticleAddCustomerDocument == null ? (object)DBNull.Value : item.ArticleAddCustomerDocument);
					sqlCommand.Parameters.AddWithValue("ArticleBOM", item.ArticleBOM);
					sqlCommand.Parameters.AddWithValue("ArticleCts", item.ArticleCts);
					sqlCommand.Parameters.AddWithValue("ArticleData", item.ArticleData);
					sqlCommand.Parameters.AddWithValue("ArticleDeleteCustomerDocument", item.ArticleDeleteCustomerDocument == null ? (object)DBNull.Value : item.ArticleDeleteCustomerDocument);
					sqlCommand.Parameters.AddWithValue("ArticleLogistics", item.ArticleLogistics);
					sqlCommand.Parameters.AddWithValue("ArticleLogisticsPrices", item.ArticleLogisticsPrices == null ? (object)DBNull.Value : item.ArticleLogisticsPrices);
					sqlCommand.Parameters.AddWithValue("ArticleOverview", item.ArticleOverview);
					sqlCommand.Parameters.AddWithValue("ArticleProduction", item.ArticleProduction);
					sqlCommand.Parameters.AddWithValue("ArticlePurchase", item.ArticlePurchase);
					sqlCommand.Parameters.AddWithValue("ArticleQuality", item.ArticleQuality);
					sqlCommand.Parameters.AddWithValue("Articles", item.Articles);
					sqlCommand.Parameters.AddWithValue("ArticleSales", item.ArticleSales);
					sqlCommand.Parameters.AddWithValue("ArticleSalesCustom", item.ArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("ArticleSalesItem", item.ArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlEngineering", item.ArticlesBOMCPControlEngineering == null ? (object)DBNull.Value : item.ArticlesBOMCPControlEngineering);
					sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlHistory", item.ArticlesBOMCPControlHistory == null ? (object)DBNull.Value : item.ArticlesBOMCPControlHistory);
					sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlQuality", item.ArticlesBOMCPControlQuality == null ? (object)DBNull.Value : item.ArticlesBOMCPControlQuality);
					sqlCommand.Parameters.AddWithValue("ArticleStatistics", item.ArticleStatistics);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineering", item.ArticleStatisticsEngineering);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineeringEdit", item.ArticleStatisticsEngineeringEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccounting", item.ArticleStatisticsFinanceAccounting);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccountingEdit", item.ArticleStatisticsFinanceAccountingEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogistics", item.ArticleStatisticsLogistics);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogisticsEdit", item.ArticleStatisticsLogisticsEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchase", item.ArticleStatisticsPurchase);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchaseEdit", item.ArticleStatisticsPurchaseEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnic", item.ArticleStatisticsTechnic);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnicEdit", item.ArticleStatisticsTechnicEdit);
					sqlCommand.Parameters.AddWithValue("BomChangeRequests", item.BomChangeRequests == null ? (object)DBNull.Value : item.BomChangeRequests);
					sqlCommand.Parameters.AddWithValue("CocType", item.CocType == null ? (object)DBNull.Value : item.CocType);
					sqlCommand.Parameters.AddWithValue("ConditionAssignment", item.ConditionAssignment == null ? (object)DBNull.Value : item.ConditionAssignment);
					sqlCommand.Parameters.AddWithValue("ConfigArticle", item.ConfigArticle);
					sqlCommand.Parameters.AddWithValue("ConfigCustomer", item.ConfigCustomer == null ? (object)DBNull.Value : item.ConfigCustomer);
					sqlCommand.Parameters.AddWithValue("ConfigSupplier", item.ConfigSupplier == null ? (object)DBNull.Value : item.ConfigSupplier);
					sqlCommand.Parameters.AddWithValue("ContactAddress", item.ContactAddress == null ? (object)DBNull.Value : item.ContactAddress);
					sqlCommand.Parameters.AddWithValue("ContactSalutation", item.ContactSalutation == null ? (object)DBNull.Value : item.ContactSalutation);
					sqlCommand.Parameters.AddWithValue("CreateArticlePurchase", item.CreateArticlePurchase);
					sqlCommand.Parameters.AddWithValue("CreateArticleSalesCustom", item.CreateArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("CreateArticleSalesItem", item.CreateArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("CreateCustomerContactPerson", item.CreateCustomerContactPerson == null ? (object)DBNull.Value : item.CreateCustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("CreateSupplierContactPerson", item.CreateSupplierContactPerson == null ? (object)DBNull.Value : item.CreateSupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Currencies", item.Currencies == null ? (object)DBNull.Value : item.Currencies);
					sqlCommand.Parameters.AddWithValue("CustomerAddress", item.CustomerAddress == null ? (object)DBNull.Value : item.CustomerAddress);
					sqlCommand.Parameters.AddWithValue("CustomerCommunication", item.CustomerCommunication == null ? (object)DBNull.Value : item.CustomerCommunication);
					sqlCommand.Parameters.AddWithValue("CustomerContactPerson", item.CustomerContactPerson == null ? (object)DBNull.Value : item.CustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("CustomerData", item.CustomerData == null ? (object)DBNull.Value : item.CustomerData);
					sqlCommand.Parameters.AddWithValue("CustomerGroup", item.CustomerGroup == null ? (object)DBNull.Value : item.CustomerGroup);
					sqlCommand.Parameters.AddWithValue("CustomerHistory", item.CustomerHistory == null ? (object)DBNull.Value : item.CustomerHistory);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("CustomerOverview", item.CustomerOverview == null ? (object)DBNull.Value : item.CustomerOverview);
					sqlCommand.Parameters.AddWithValue("Customers", item.Customers);
					sqlCommand.Parameters.AddWithValue("CustomerShipping", item.CustomerShipping == null ? (object)DBNull.Value : item.CustomerShipping);
					sqlCommand.Parameters.AddWithValue("DeleteAltPositionsArticleBOM", item.DeleteAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("DeleteArticle", item.DeleteArticle == null ? (object)DBNull.Value : item.DeleteArticle);
					sqlCommand.Parameters.AddWithValue("DeleteArticleBOM", item.DeleteArticleBOM);
					sqlCommand.Parameters.AddWithValue("DeleteArticlePurchase", item.DeleteArticlePurchase);
					sqlCommand.Parameters.AddWithValue("DeleteArticleSalesCustom", item.DeleteArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("DeleteArticleSalesItem", item.DeleteArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("DeleteBCR", item.DeleteBCR == null ? (object)DBNull.Value : item.DeleteBCR);
					sqlCommand.Parameters.AddWithValue("DeleteCustomerContactPerson", item.DeleteCustomerContactPerson == null ? (object)DBNull.Value : item.DeleteCustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("DeleteFiles", item.DeleteFiles == null ? (object)DBNull.Value : item.DeleteFiles);
					sqlCommand.Parameters.AddWithValue("DeleteRohArtikelNummer", item.DeleteRohArtikelNummer == null ? (object)DBNull.Value : item.DeleteRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("DeleteSupplierContactPerson", item.DeleteSupplierContactPerson == null ? (object)DBNull.Value : item.DeleteSupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("DiscountGroup", item.DiscountGroup == null ? (object)DBNull.Value : item.DiscountGroup);
					sqlCommand.Parameters.AddWithValue("DownloadAllOutdatedEinkaufsPreis", item.DownloadAllOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadAllOutdatedEinkaufsPreis);
					sqlCommand.Parameters.AddWithValue("DownloadFiles", item.DownloadFiles == null ? (object)DBNull.Value : item.DownloadFiles);
					sqlCommand.Parameters.AddWithValue("DownloadOutdatedEinkaufsPreis", item.DownloadOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadOutdatedEinkaufsPreis);
					sqlCommand.Parameters.AddWithValue("EdiConcern", item.EdiConcern == null ? (object)DBNull.Value : item.EdiConcern);
					sqlCommand.Parameters.AddWithValue("EditAltPositionsArticleBOM", item.EditAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("EditArticle", item.EditArticle);
					sqlCommand.Parameters.AddWithValue("EditArticleBOM", item.EditArticleBOM);
					sqlCommand.Parameters.AddWithValue("EditArticleBOMPosition", item.EditArticleBOMPosition);
					sqlCommand.Parameters.AddWithValue("EditArticleCts", item.EditArticleCts);
					sqlCommand.Parameters.AddWithValue("EditArticleData", item.EditArticleData);
					sqlCommand.Parameters.AddWithValue("EditArticleDesignation", item.EditArticleDesignation);
					sqlCommand.Parameters.AddWithValue("EditArticleImage", item.EditArticleImage);
					sqlCommand.Parameters.AddWithValue("EditArticleLogistics", item.EditArticleLogistics);
					sqlCommand.Parameters.AddWithValue("EditArticleManager", item.EditArticleManager);
					sqlCommand.Parameters.AddWithValue("EditArticleProduction", item.EditArticleProduction);
					sqlCommand.Parameters.AddWithValue("EditArticlePurchase", item.EditArticlePurchase);
					sqlCommand.Parameters.AddWithValue("EditArticleQuality", item.EditArticleQuality);
					sqlCommand.Parameters.AddWithValue("EditArticleReference", item.EditArticleReference == null ? (object)DBNull.Value : item.EditArticleReference);
					sqlCommand.Parameters.AddWithValue("EditArticleSalesCustom", item.EditArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("EditArticleSalesItem", item.EditArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("EditCocType", item.EditCocType == null ? (object)DBNull.Value : item.EditCocType);
					sqlCommand.Parameters.AddWithValue("EditConditionAssignment", item.EditConditionAssignment == null ? (object)DBNull.Value : item.EditConditionAssignment);
					sqlCommand.Parameters.AddWithValue("EditContactAddress", item.EditContactAddress == null ? (object)DBNull.Value : item.EditContactAddress);
					sqlCommand.Parameters.AddWithValue("EditContactSalutation", item.EditContactSalutation == null ? (object)DBNull.Value : item.EditContactSalutation);
					sqlCommand.Parameters.AddWithValue("EditCurrencies", item.EditCurrencies == null ? (object)DBNull.Value : item.EditCurrencies);
					sqlCommand.Parameters.AddWithValue("EditCustomer", item.EditCustomer == null ? (object)DBNull.Value : item.EditCustomer);
					sqlCommand.Parameters.AddWithValue("EditCustomerAddress", item.EditCustomerAddress == null ? (object)DBNull.Value : item.EditCustomerAddress);
					sqlCommand.Parameters.AddWithValue("EditCustomerCommunication", item.EditCustomerCommunication == null ? (object)DBNull.Value : item.EditCustomerCommunication);
					sqlCommand.Parameters.AddWithValue("EditCustomerContactPerson", item.EditCustomerContactPerson == null ? (object)DBNull.Value : item.EditCustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("EditCustomerCoordination", item.EditCustomerCoordination == null ? (object)DBNull.Value : item.EditCustomerCoordination);
					sqlCommand.Parameters.AddWithValue("EditCustomerData", item.EditCustomerData == null ? (object)DBNull.Value : item.EditCustomerData);
					sqlCommand.Parameters.AddWithValue("EditCustomerGroup", item.EditCustomerGroup == null ? (object)DBNull.Value : item.EditCustomerGroup);
					sqlCommand.Parameters.AddWithValue("EditCustomerImage", item.EditCustomerImage == null ? (object)DBNull.Value : item.EditCustomerImage);
					sqlCommand.Parameters.AddWithValue("EditCustomerItemNumber", item.EditCustomerItemNumber == null ? (object)DBNull.Value : item.EditCustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("EditCustomerShipping", item.EditCustomerShipping == null ? (object)DBNull.Value : item.EditCustomerShipping);
					sqlCommand.Parameters.AddWithValue("EditDiscountGroup", item.EditDiscountGroup == null ? (object)DBNull.Value : item.EditDiscountGroup);
					sqlCommand.Parameters.AddWithValue("EditEdiConcern", item.EditEdiConcern == null ? (object)DBNull.Value : item.EditEdiConcern);
					sqlCommand.Parameters.AddWithValue("EditFibuFrame", item.EditFibuFrame == null ? (object)DBNull.Value : item.EditFibuFrame);
					sqlCommand.Parameters.AddWithValue("EditHourlyRate", item.EditHourlyRate == null ? (object)DBNull.Value : item.EditHourlyRate);
					sqlCommand.Parameters.AddWithValue("EditIndustry", item.EditIndustry == null ? (object)DBNull.Value : item.EditIndustry);
					sqlCommand.Parameters.AddWithValue("EditLagerCCID", item.EditLagerCCID == null ? (object)DBNull.Value : item.EditLagerCCID);
					sqlCommand.Parameters.AddWithValue("EditLagerMinStock", item.EditLagerMinStock);
					sqlCommand.Parameters.AddWithValue("EditLagerOrderProposal", item.EditLagerOrderProposal == null ? (object)DBNull.Value : item.EditLagerOrderProposal);
					sqlCommand.Parameters.AddWithValue("EditLagerStock", item.EditLagerStock == null ? (object)DBNull.Value : item.EditLagerStock);
					sqlCommand.Parameters.AddWithValue("EditPayementPractises", item.EditPayementPractises == null ? (object)DBNull.Value : item.EditPayementPractises);
					sqlCommand.Parameters.AddWithValue("EditPricingGroup", item.EditPricingGroup == null ? (object)DBNull.Value : item.EditPricingGroup);
					sqlCommand.Parameters.AddWithValue("EditRohArtikelNummer", item.EditRohArtikelNummer == null ? (object)DBNull.Value : item.EditRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("EditShippingMethods", item.EditShippingMethods == null ? (object)DBNull.Value : item.EditShippingMethods);
					sqlCommand.Parameters.AddWithValue("EditSlipCircle", item.EditSlipCircle == null ? (object)DBNull.Value : item.EditSlipCircle);
					sqlCommand.Parameters.AddWithValue("EditSupplier", item.EditSupplier == null ? (object)DBNull.Value : item.EditSupplier);
					sqlCommand.Parameters.AddWithValue("EditSupplierAddress", item.EditSupplierAddress == null ? (object)DBNull.Value : item.EditSupplierAddress);
					sqlCommand.Parameters.AddWithValue("EditSupplierCommunication", item.EditSupplierCommunication == null ? (object)DBNull.Value : item.EditSupplierCommunication);
					sqlCommand.Parameters.AddWithValue("EditSupplierContactPerson", item.EditSupplierContactPerson == null ? (object)DBNull.Value : item.EditSupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("EditSupplierCoordination", item.EditSupplierCoordination == null ? (object)DBNull.Value : item.EditSupplierCoordination);
					sqlCommand.Parameters.AddWithValue("EditSupplierData", item.EditSupplierData == null ? (object)DBNull.Value : item.EditSupplierData);
					sqlCommand.Parameters.AddWithValue("EditSupplierGroup", item.EditSupplierGroup == null ? (object)DBNull.Value : item.EditSupplierGroup);
					sqlCommand.Parameters.AddWithValue("EditSupplierImage", item.EditSupplierImage == null ? (object)DBNull.Value : item.EditSupplierImage);
					sqlCommand.Parameters.AddWithValue("EditSupplierShipping", item.EditSupplierShipping == null ? (object)DBNull.Value : item.EditSupplierShipping);
					sqlCommand.Parameters.AddWithValue("EditTermsOfPayment", item.EditTermsOfPayment == null ? (object)DBNull.Value : item.EditTermsOfPayment);
					sqlCommand.Parameters.AddWithValue("EDrawingEdit", item.EDrawingEdit == null ? (object)DBNull.Value : item.EDrawingEdit);
					sqlCommand.Parameters.AddWithValue("EinkaufsPreisUpdate", item.EinkaufsPreisUpdate == null ? (object)DBNull.Value : item.EinkaufsPreisUpdate);
					sqlCommand.Parameters.AddWithValue("FibuFrame", item.FibuFrame == null ? (object)DBNull.Value : item.FibuFrame);
					sqlCommand.Parameters.AddWithValue("GetRohArtikelNummer", item.GetRohArtikelNummer == null ? (object)DBNull.Value : item.GetRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("HourlyRate", item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
					sqlCommand.Parameters.AddWithValue("ImportArticleBOM", item.ImportArticleBOM);
					sqlCommand.Parameters.AddWithValue("Industry", item.Industry == null ? (object)DBNull.Value : item.Industry);
					sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
					sqlCommand.Parameters.AddWithValue("LagerArticleLogistics", item.LagerArticleLogistics);
					sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("ModuleAdministrator", item.ModuleAdministrator == null ? (object)DBNull.Value : item.ModuleAdministrator);
					sqlCommand.Parameters.AddWithValue("offer", item.offer == null ? (object)DBNull.Value : item.offer);
					sqlCommand.Parameters.AddWithValue("OfferRequestADD", item.OfferRequestADD == null ? (object)DBNull.Value : item.OfferRequestADD);
					sqlCommand.Parameters.AddWithValue("OfferRequestApplyPrice", item.OfferRequestApplyPrice == null ? (object)DBNull.Value : item.OfferRequestApplyPrice);
					sqlCommand.Parameters.AddWithValue("OfferRequestDelete", item.OfferRequestDelete == null ? (object)DBNull.Value : item.OfferRequestDelete);
					sqlCommand.Parameters.AddWithValue("OfferRequestEdit", item.OfferRequestEdit == null ? (object)DBNull.Value : item.OfferRequestEdit);
					sqlCommand.Parameters.AddWithValue("OfferRequestEditEmail", item.OfferRequestEditEmail == null ? (object)DBNull.Value : item.OfferRequestEditEmail);
					sqlCommand.Parameters.AddWithValue("OfferRequestSendEmail", item.OfferRequestSendEmail == null ? (object)DBNull.Value : item.OfferRequestSendEmail);
					sqlCommand.Parameters.AddWithValue("OfferRequestView", item.OfferRequestView == null ? (object)DBNull.Value : item.OfferRequestView);
					sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoAdd", item.PackagingsLgtPhotoAdd == null ? (object)DBNull.Value : item.PackagingsLgtPhotoAdd);
					sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoDelete", item.PackagingsLgtPhotoDelete == null ? (object)DBNull.Value : item.PackagingsLgtPhotoDelete);
					sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoView", item.PackagingsLgtPhotoView == null ? (object)DBNull.Value : item.PackagingsLgtPhotoView);
					sqlCommand.Parameters.AddWithValue("PayementPractises", item.PayementPractises == null ? (object)DBNull.Value : item.PayementPractises);
					sqlCommand.Parameters.AddWithValue("PMAddCable", item.PMAddCable == null ? (object)DBNull.Value : item.PMAddCable);
					sqlCommand.Parameters.AddWithValue("PMAddMileStone", item.PMAddMileStone == null ? (object)DBNull.Value : item.PMAddMileStone);
					sqlCommand.Parameters.AddWithValue("PMAddProject", item.PMAddProject == null ? (object)DBNull.Value : item.PMAddProject);
					sqlCommand.Parameters.AddWithValue("PMDeleteCable", item.PMDeleteCable == null ? (object)DBNull.Value : item.PMDeleteCable);
					sqlCommand.Parameters.AddWithValue("PMDeleteMileStone", item.PMDeleteMileStone == null ? (object)DBNull.Value : item.PMDeleteMileStone);
					sqlCommand.Parameters.AddWithValue("PMDeleteProject", item.PMDeleteProject == null ? (object)DBNull.Value : item.PMDeleteProject);
					sqlCommand.Parameters.AddWithValue("PMEditCable", item.PMEditCable == null ? (object)DBNull.Value : item.PMEditCable);
					sqlCommand.Parameters.AddWithValue("PMEditMileStone", item.PMEditMileStone == null ? (object)DBNull.Value : item.PMEditMileStone);
					sqlCommand.Parameters.AddWithValue("PMEditProject", item.PMEditProject == null ? (object)DBNull.Value : item.PMEditProject);
					sqlCommand.Parameters.AddWithValue("PMModule", item.PMModule == null ? (object)DBNull.Value : item.PMModule);
					sqlCommand.Parameters.AddWithValue("PMViewProjectsCompact", item.PMViewProjectsCompact == null ? (object)DBNull.Value : item.PMViewProjectsCompact);
					sqlCommand.Parameters.AddWithValue("PMViewProjectsDetail", item.PMViewProjectsDetail == null ? (object)DBNull.Value : item.PMViewProjectsDetail);
					sqlCommand.Parameters.AddWithValue("PMViewProjectsMedium", item.PMViewProjectsMedium == null ? (object)DBNull.Value : item.PMViewProjectsMedium);
					sqlCommand.Parameters.AddWithValue("PricingGroup", item.PricingGroup == null ? (object)DBNull.Value : item.PricingGroup);
					sqlCommand.Parameters.AddWithValue("RemoveArticleReference", item.RemoveArticleReference == null ? (object)DBNull.Value : item.RemoveArticleReference);
					sqlCommand.Parameters.AddWithValue("Settings", item.Settings == null ? (object)DBNull.Value : item.Settings);
					sqlCommand.Parameters.AddWithValue("ShippingMethods", item.ShippingMethods == null ? (object)DBNull.Value : item.ShippingMethods);
					sqlCommand.Parameters.AddWithValue("SlipCircle", item.SlipCircle == null ? (object)DBNull.Value : item.SlipCircle);
					sqlCommand.Parameters.AddWithValue("SupplierAddress", item.SupplierAddress == null ? (object)DBNull.Value : item.SupplierAddress);
					sqlCommand.Parameters.AddWithValue("SupplierAttachementAddFile", item.SupplierAttachementAddFile == null ? (object)DBNull.Value : item.SupplierAttachementAddFile);
					sqlCommand.Parameters.AddWithValue("SupplierAttachementGetFile", item.SupplierAttachementGetFile == null ? (object)DBNull.Value : item.SupplierAttachementGetFile);
					sqlCommand.Parameters.AddWithValue("SupplierAttachementRemoveFile", item.SupplierAttachementRemoveFile == null ? (object)DBNull.Value : item.SupplierAttachementRemoveFile);
					sqlCommand.Parameters.AddWithValue("SupplierCommunication", item.SupplierCommunication == null ? (object)DBNull.Value : item.SupplierCommunication);
					sqlCommand.Parameters.AddWithValue("SupplierContactPerson", item.SupplierContactPerson == null ? (object)DBNull.Value : item.SupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("SupplierData", item.SupplierData == null ? (object)DBNull.Value : item.SupplierData);
					sqlCommand.Parameters.AddWithValue("SupplierGroup", item.SupplierGroup == null ? (object)DBNull.Value : item.SupplierGroup);
					sqlCommand.Parameters.AddWithValue("SupplierHistory", item.SupplierHistory == null ? (object)DBNull.Value : item.SupplierHistory);
					sqlCommand.Parameters.AddWithValue("SupplierOverview", item.SupplierOverview == null ? (object)DBNull.Value : item.SupplierOverview);
					sqlCommand.Parameters.AddWithValue("Suppliers", item.Suppliers);
					sqlCommand.Parameters.AddWithValue("SupplierShipping", item.SupplierShipping == null ? (object)DBNull.Value : item.SupplierShipping);
					sqlCommand.Parameters.AddWithValue("TermsOfPayment", item.TermsOfPayment == null ? (object)DBNull.Value : item.TermsOfPayment);
					sqlCommand.Parameters.AddWithValue("UploadAltPositionsArticleBOM", item.UploadAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("UploadArticleBOM", item.UploadArticleBOM);
					sqlCommand.Parameters.AddWithValue("ValidateArticleBOM", item.ValidateArticleBOM == null ? (object)DBNull.Value : item.ValidateArticleBOM);
					sqlCommand.Parameters.AddWithValue("ValidateBCR", item.ValidateBCR == null ? (object)DBNull.Value : item.ValidateBCR);
					sqlCommand.Parameters.AddWithValue("ViewAltPositionsArticleBOM", item.ViewAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("ViewArticleLog", item.ViewArticleLog);
					sqlCommand.Parameters.AddWithValue("ViewArticleReference", item.ViewArticleReference == null ? (object)DBNull.Value : item.ViewArticleReference);
					sqlCommand.Parameters.AddWithValue("ViewArticles", item.ViewArticles);
					sqlCommand.Parameters.AddWithValue("ViewBCR", item.ViewBCR == null ? (object)DBNull.Value : item.ViewBCR);
					sqlCommand.Parameters.AddWithValue("ViewCustomers", item.ViewCustomers == null ? (object)DBNull.Value : item.ViewCustomers);
					sqlCommand.Parameters.AddWithValue("ViewLPCustomer", item.ViewLPCustomer == null ? (object)DBNull.Value : item.ViewLPCustomer);
					sqlCommand.Parameters.AddWithValue("ViewLPSupplier", item.ViewLPSupplier == null ? (object)DBNull.Value : item.ViewLPSupplier);
					sqlCommand.Parameters.AddWithValue("ViewSupplierAddressComments", item.ViewSupplierAddressComments == null ? (object)DBNull.Value : item.ViewSupplierAddressComments);
					sqlCommand.Parameters.AddWithValue("ViewSuppliers", item.ViewSuppliers == null ? (object)DBNull.Value : item.ViewSuppliers);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Entities.Tables.BSD.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 226; // Nb params per query
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
		private static int insert(List<Entities.Tables.BSD.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__BSD_AccessProfile] ([AccessProfileName],[AddArticle],[AddArticleReference],[AddBCR],[AddCocType],[AddConditionAssignment],[AddContactAddress],[AddContactSalutation],[AddCurrencies],[AddCustomer],[AddCustomerGroup],[AddCustomerItemNumber],[AddDiscountGroup],[AddEdiConcern],[AddFibuFrame],[AddFiles],[AddHourlyRate],[AddIndustry],[AddPayementPractises],[AddPricingGroup],[AddRohArtikelNummer],[AddShippingMethods],[AddSlipCircle],[AddSupplier],[AddSupplierGroup],[AddTermsOfPayment],[Administration],[AltPositionsArticleBOM],[ArchiveArticle],[ArchiveCustomer],[ArchiveSupplier],[ArticleAddCustomerDocument],[ArticleBOM],[ArticleCts],[ArticleData],[ArticleDeleteCustomerDocument],[ArticleLogistics],[ArticleLogisticsPrices],[ArticleOverview],[ArticleProduction],[ArticlePurchase],[ArticleQuality],[Articles],[ArticleSales],[ArticleSalesCustom],[ArticleSalesItem],[ArticlesBOMCPControlEngineering],[ArticlesBOMCPControlHistory],[ArticlesBOMCPControlQuality],[ArticleStatistics],[ArticleStatisticsEngineering],[ArticleStatisticsEngineeringEdit],[ArticleStatisticsFinanceAccounting],[ArticleStatisticsFinanceAccountingEdit],[ArticleStatisticsLogistics],[ArticleStatisticsLogisticsEdit],[ArticleStatisticsPurchase],[ArticleStatisticsPurchaseEdit],[ArticleStatisticsTechnic],[ArticleStatisticsTechnicEdit],[BomChangeRequests],[CocType],[ConditionAssignment],[ConfigArticle],[ConfigCustomer],[ConfigSupplier],[ContactAddress],[ContactSalutation],[CreateArticlePurchase],[CreateArticleSalesCustom],[CreateArticleSalesItem],[CreateCustomerContactPerson],[CreateSupplierContactPerson],[CreationTime],[CreationUserId],[Currencies],[CustomerAddress],[CustomerCommunication],[CustomerContactPerson],[CustomerData],[CustomerGroup],[CustomerHistory],[CustomerItemNumber],[CustomerOverview],[Customers],[CustomerShipping],[DeleteAltPositionsArticleBOM],[DeleteArticle],[DeleteArticleBOM],[DeleteArticlePurchase],[DeleteArticleSalesCustom],[DeleteArticleSalesItem],[DeleteBCR],[DeleteCustomerContactPerson],[DeleteFiles],[DeleteRohArtikelNummer],[DeleteSupplierContactPerson],[DiscountGroup],[DownloadAllOutdatedEinkaufsPreis],[DownloadFiles],[DownloadOutdatedEinkaufsPreis],[EdiConcern],[EditAltPositionsArticleBOM],[EditArticle],[EditArticleBOM],[EditArticleBOMPosition],[EditArticleCts],[EditArticleData],[EditArticleDesignation],[EditArticleImage],[EditArticleLogistics],[EditArticleManager],[EditArticleProduction],[EditArticlePurchase],[EditArticleQuality],[EditArticleReference],[EditArticleSalesCustom],[EditArticleSalesItem],[EditCocType],[EditConditionAssignment],[EditContactAddress],[EditContactSalutation],[EditCurrencies],[EditCustomer],[EditCustomerAddress],[EditCustomerCommunication],[EditCustomerContactPerson],[EditCustomerCoordination],[EditCustomerData],[EditCustomerGroup],[EditCustomerImage],[EditCustomerItemNumber],[EditCustomerShipping],[EditDiscountGroup],[EditEdiConcern],[EditFibuFrame],[EditHourlyRate],[EditIndustry],[EditLagerCCID],[EditLagerMinStock],[EditLagerOrderProposal],[EditLagerStock],[EditPayementPractises],[EditPricingGroup],[EditRohArtikelNummer],[EditShippingMethods],[EditSlipCircle],[EditSupplier],[EditSupplierAddress],[EditSupplierCommunication],[EditSupplierContactPerson],[EditSupplierCoordination],[EditSupplierData],[EditSupplierGroup],[EditSupplierImage],[EditSupplierShipping],[EditTermsOfPayment],[EDrawingEdit],[EinkaufsPreisUpdate],[FibuFrame],[GetRohArtikelNummer],[HourlyRate],[ImportArticleBOM],[Industry],[isDefault],[LagerArticleLogistics],[ModuleActivated],[ModuleAdministrator],[offer],[OfferRequestADD],[OfferRequestApplyPrice],[OfferRequestDelete],[OfferRequestEdit],[OfferRequestEditEmail],[OfferRequestSendEmail],[OfferRequestView],[PackagingsLgtPhotoAdd],[PackagingsLgtPhotoDelete],[PackagingsLgtPhotoView],[PayementPractises],[PMAddCable],[PMAddMileStone],[PMAddProject],[PMDeleteCable],[PMDeleteMileStone],[PMDeleteProject],[PMEditCable],[PMEditMileStone],[PMEditProject],[PMModule],[PMViewProjectsCompact],[PMViewProjectsDetail],[PMViewProjectsMedium],[PricingGroup],[RemoveArticleReference],[Settings],[ShippingMethods],[SlipCircle],[SupplierAddress],[SupplierAttachementAddFile],[SupplierAttachementGetFile],[SupplierAttachementRemoveFile],[SupplierCommunication],[SupplierContactPerson],[SupplierData],[SupplierGroup],[SupplierHistory],[SupplierOverview],[Suppliers],[SupplierShipping],[TermsOfPayment],[UploadAltPositionsArticleBOM],[UploadArticleBOM],[ValidateArticleBOM],[ValidateBCR],[ViewAltPositionsArticleBOM],[ViewArticleLog],[ViewArticleReference],[ViewArticles],[ViewBCR],[ViewCustomers],[ViewLPCustomer],[ViewLPSupplier],[ViewSupplierAddressComments],[ViewSuppliers]) VALUES ( "

							+ "@AccessProfileName" + i + ","
							+ "@AddArticle" + i + ","
							+ "@AddArticleReference" + i + ","
							+ "@AddBCR" + i + ","
							+ "@AddCocType" + i + ","
							+ "@AddConditionAssignment" + i + ","
							+ "@AddContactAddress" + i + ","
							+ "@AddContactSalutation" + i + ","
							+ "@AddCurrencies" + i + ","
							+ "@AddCustomer" + i + ","
							+ "@AddCustomerGroup" + i + ","
							+ "@AddCustomerItemNumber" + i + ","
							+ "@AddDiscountGroup" + i + ","
							+ "@AddEdiConcern" + i + ","
							+ "@AddFibuFrame" + i + ","
							+ "@AddFiles" + i + ","
							+ "@AddHourlyRate" + i + ","
							+ "@AddIndustry" + i + ","
							+ "@AddPayementPractises" + i + ","
							+ "@AddPricingGroup" + i + ","
							+ "@AddRohArtikelNummer" + i + ","
							+ "@AddShippingMethods" + i + ","
							+ "@AddSlipCircle" + i + ","
							+ "@AddSupplier" + i + ","
							+ "@AddSupplierGroup" + i + ","
							+ "@AddTermsOfPayment" + i + ","
							+ "@Administration" + i + ","
							+ "@AltPositionsArticleBOM" + i + ","
							+ "@ArchiveArticle" + i + ","
							+ "@ArchiveCustomer" + i + ","
							+ "@ArchiveSupplier" + i + ","
							+ "@ArticleAddCustomerDocument" + i + ","
							+ "@ArticleBOM" + i + ","
							+ "@ArticleCts" + i + ","
							+ "@ArticleData" + i + ","
							+ "@ArticleDeleteCustomerDocument" + i + ","
							+ "@ArticleLogistics" + i + ","
							+ "@ArticleLogisticsPrices" + i + ","
							+ "@ArticleOverview" + i + ","
							+ "@ArticleProduction" + i + ","
							+ "@ArticlePurchase" + i + ","
							+ "@ArticleQuality" + i + ","
							+ "@Articles" + i + ","
							+ "@ArticleSales" + i + ","
							+ "@ArticleSalesCustom" + i + ","
							+ "@ArticleSalesItem" + i + ","
							+ "@ArticlesBOMCPControlEngineering" + i + ","
							+ "@ArticlesBOMCPControlHistory" + i + ","
							+ "@ArticlesBOMCPControlQuality" + i + ","
							+ "@ArticleStatistics" + i + ","
							+ "@ArticleStatisticsEngineering" + i + ","
							+ "@ArticleStatisticsEngineeringEdit" + i + ","
							+ "@ArticleStatisticsFinanceAccounting" + i + ","
							+ "@ArticleStatisticsFinanceAccountingEdit" + i + ","
							+ "@ArticleStatisticsLogistics" + i + ","
							+ "@ArticleStatisticsLogisticsEdit" + i + ","
							+ "@ArticleStatisticsPurchase" + i + ","
							+ "@ArticleStatisticsPurchaseEdit" + i + ","
							+ "@ArticleStatisticsTechnic" + i + ","
							+ "@ArticleStatisticsTechnicEdit" + i + ","
							+ "@BomChangeRequests" + i + ","
							+ "@CocType" + i + ","
							+ "@ConditionAssignment" + i + ","
							+ "@ConfigArticle" + i + ","
							+ "@ConfigCustomer" + i + ","
							+ "@ConfigSupplier" + i + ","
							+ "@ContactAddress" + i + ","
							+ "@ContactSalutation" + i + ","
							+ "@CreateArticlePurchase" + i + ","
							+ "@CreateArticleSalesCustom" + i + ","
							+ "@CreateArticleSalesItem" + i + ","
							+ "@CreateCustomerContactPerson" + i + ","
							+ "@CreateSupplierContactPerson" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@Currencies" + i + ","
							+ "@CustomerAddress" + i + ","
							+ "@CustomerCommunication" + i + ","
							+ "@CustomerContactPerson" + i + ","
							+ "@CustomerData" + i + ","
							+ "@CustomerGroup" + i + ","
							+ "@CustomerHistory" + i + ","
							+ "@CustomerItemNumber" + i + ","
							+ "@CustomerOverview" + i + ","
							+ "@Customers" + i + ","
							+ "@CustomerShipping" + i + ","
							+ "@DeleteAltPositionsArticleBOM" + i + ","
							+ "@DeleteArticle" + i + ","
							+ "@DeleteArticleBOM" + i + ","
							+ "@DeleteArticlePurchase" + i + ","
							+ "@DeleteArticleSalesCustom" + i + ","
							+ "@DeleteArticleSalesItem" + i + ","
							+ "@DeleteBCR" + i + ","
							+ "@DeleteCustomerContactPerson" + i + ","
							+ "@DeleteFiles" + i + ","
							+ "@DeleteRohArtikelNummer" + i + ","
							+ "@DeleteSupplierContactPerson" + i + ","
							+ "@DiscountGroup" + i + ","
							+ "@DownloadAllOutdatedEinkaufsPreis" + i + ","
							+ "@DownloadFiles" + i + ","
							+ "@DownloadOutdatedEinkaufsPreis" + i + ","
							+ "@EdiConcern" + i + ","
							+ "@EditAltPositionsArticleBOM" + i + ","
							+ "@EditArticle" + i + ","
							+ "@EditArticleBOM" + i + ","
							+ "@EditArticleBOMPosition" + i + ","
							+ "@EditArticleCts" + i + ","
							+ "@EditArticleData" + i + ","
							+ "@EditArticleDesignation" + i + ","
							+ "@EditArticleImage" + i + ","
							+ "@EditArticleLogistics" + i + ","
							+ "@EditArticleManager" + i + ","
							+ "@EditArticleProduction" + i + ","
							+ "@EditArticlePurchase" + i + ","
							+ "@EditArticleQuality" + i + ","
							+ "@EditArticleReference" + i + ","
							+ "@EditArticleSalesCustom" + i + ","
							+ "@EditArticleSalesItem" + i + ","
							+ "@EditCocType" + i + ","
							+ "@EditConditionAssignment" + i + ","
							+ "@EditContactAddress" + i + ","
							+ "@EditContactSalutation" + i + ","
							+ "@EditCurrencies" + i + ","
							+ "@EditCustomer" + i + ","
							+ "@EditCustomerAddress" + i + ","
							+ "@EditCustomerCommunication" + i + ","
							+ "@EditCustomerContactPerson" + i + ","
							+ "@EditCustomerCoordination" + i + ","
							+ "@EditCustomerData" + i + ","
							+ "@EditCustomerGroup" + i + ","
							+ "@EditCustomerImage" + i + ","
							+ "@EditCustomerItemNumber" + i + ","
							+ "@EditCustomerShipping" + i + ","
							+ "@EditDiscountGroup" + i + ","
							+ "@EditEdiConcern" + i + ","
							+ "@EditFibuFrame" + i + ","
							+ "@EditHourlyRate" + i + ","
							+ "@EditIndustry" + i + ","
							+ "@EditLagerCCID" + i + ","
							+ "@EditLagerMinStock" + i + ","
							+ "@EditLagerOrderProposal" + i + ","
							+ "@EditLagerStock" + i + ","
							+ "@EditPayementPractises" + i + ","
							+ "@EditPricingGroup" + i + ","
							+ "@EditRohArtikelNummer" + i + ","
							+ "@EditShippingMethods" + i + ","
							+ "@EditSlipCircle" + i + ","
							+ "@EditSupplier" + i + ","
							+ "@EditSupplierAddress" + i + ","
							+ "@EditSupplierCommunication" + i + ","
							+ "@EditSupplierContactPerson" + i + ","
							+ "@EditSupplierCoordination" + i + ","
							+ "@EditSupplierData" + i + ","
							+ "@EditSupplierGroup" + i + ","
							+ "@EditSupplierImage" + i + ","
							+ "@EditSupplierShipping" + i + ","
							+ "@EditTermsOfPayment" + i + ","
							+ "@EDrawingEdit" + i + ","
							+ "@EinkaufsPreisUpdate" + i + ","
							+ "@FibuFrame" + i + ","
							+ "@GetRohArtikelNummer" + i + ","
							+ "@HourlyRate" + i + ","
							+ "@ImportArticleBOM" + i + ","
							+ "@Industry" + i + ","
							+ "@isDefault" + i + ","
							+ "@LagerArticleLogistics" + i + ","
							+ "@ModuleActivated" + i + ","
							+ "@ModuleAdministrator" + i + ","
							+ "@offer" + i + ","
							+ "@OfferRequestADD" + i + ","
							+ "@OfferRequestApplyPrice" + i + ","
							+ "@OfferRequestDelete" + i + ","
							+ "@OfferRequestEdit" + i + ","
							+ "@OfferRequestEditEmail" + i + ","
							+ "@OfferRequestSendEmail" + i + ","
							+ "@OfferRequestView" + i + ","
							+ "@PackagingsLgtPhotoAdd" + i + ","
							+ "@PackagingsLgtPhotoDelete" + i + ","
							+ "@PackagingsLgtPhotoView" + i + ","
							+ "@PayementPractises" + i + ","
							+ "@PMAddCable" + i + ","
							+ "@PMAddMileStone" + i + ","
							+ "@PMAddProject" + i + ","
							+ "@PMDeleteCable" + i + ","
							+ "@PMDeleteMileStone" + i + ","
							+ "@PMDeleteProject" + i + ","
							+ "@PMEditCable" + i + ","
							+ "@PMEditMileStone" + i + ","
							+ "@PMEditProject" + i + ","
							+ "@PMModule" + i + ","
							+ "@PMViewProjectsCompact" + i + ","
							+ "@PMViewProjectsDetail" + i + ","
							+ "@PMViewProjectsMedium" + i + ","
							+ "@PricingGroup" + i + ","
							+ "@RemoveArticleReference" + i + ","
							+ "@Settings" + i + ","
							+ "@ShippingMethods" + i + ","
							+ "@SlipCircle" + i + ","
							+ "@SupplierAddress" + i + ","
							+ "@SupplierAttachementAddFile" + i + ","
							+ "@SupplierAttachementGetFile" + i + ","
							+ "@SupplierAttachementRemoveFile" + i + ","
							+ "@SupplierCommunication" + i + ","
							+ "@SupplierContactPerson" + i + ","
							+ "@SupplierData" + i + ","
							+ "@SupplierGroup" + i + ","
							+ "@SupplierHistory" + i + ","
							+ "@SupplierOverview" + i + ","
							+ "@Suppliers" + i + ","
							+ "@SupplierShipping" + i + ","
							+ "@TermsOfPayment" + i + ","
							+ "@UploadAltPositionsArticleBOM" + i + ","
							+ "@UploadArticleBOM" + i + ","
							+ "@ValidateArticleBOM" + i + ","
							+ "@ValidateBCR" + i + ","
							+ "@ViewAltPositionsArticleBOM" + i + ","
							+ "@ViewArticleLog" + i + ","
							+ "@ViewArticleReference" + i + ","
							+ "@ViewArticles" + i + ","
							+ "@ViewBCR" + i + ","
							+ "@ViewCustomers" + i + ","
							+ "@ViewLPCustomer" + i + ","
							+ "@ViewLPSupplier" + i + ","
							+ "@ViewSupplierAddressComments" + i + ","
							+ "@ViewSuppliers" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("AddArticle" + i, item.AddArticle);
						sqlCommand.Parameters.AddWithValue("AddArticleReference" + i, item.AddArticleReference == null ? (object)DBNull.Value : item.AddArticleReference);
						sqlCommand.Parameters.AddWithValue("AddBCR" + i, item.AddBCR == null ? (object)DBNull.Value : item.AddBCR);
						sqlCommand.Parameters.AddWithValue("AddCocType" + i, item.AddCocType == null ? (object)DBNull.Value : item.AddCocType);
						sqlCommand.Parameters.AddWithValue("AddConditionAssignment" + i, item.AddConditionAssignment == null ? (object)DBNull.Value : item.AddConditionAssignment);
						sqlCommand.Parameters.AddWithValue("AddContactAddress" + i, item.AddContactAddress == null ? (object)DBNull.Value : item.AddContactAddress);
						sqlCommand.Parameters.AddWithValue("AddContactSalutation" + i, item.AddContactSalutation == null ? (object)DBNull.Value : item.AddContactSalutation);
						sqlCommand.Parameters.AddWithValue("AddCurrencies" + i, item.AddCurrencies == null ? (object)DBNull.Value : item.AddCurrencies);
						sqlCommand.Parameters.AddWithValue("AddCustomer" + i, item.AddCustomer == null ? (object)DBNull.Value : item.AddCustomer);
						sqlCommand.Parameters.AddWithValue("AddCustomerGroup" + i, item.AddCustomerGroup == null ? (object)DBNull.Value : item.AddCustomerGroup);
						sqlCommand.Parameters.AddWithValue("AddCustomerItemNumber" + i, item.AddCustomerItemNumber == null ? (object)DBNull.Value : item.AddCustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("AddDiscountGroup" + i, item.AddDiscountGroup == null ? (object)DBNull.Value : item.AddDiscountGroup);
						sqlCommand.Parameters.AddWithValue("AddEdiConcern" + i, item.AddEdiConcern == null ? (object)DBNull.Value : item.AddEdiConcern);
						sqlCommand.Parameters.AddWithValue("AddFibuFrame" + i, item.AddFibuFrame == null ? (object)DBNull.Value : item.AddFibuFrame);
						sqlCommand.Parameters.AddWithValue("AddFiles" + i, item.AddFiles == null ? (object)DBNull.Value : item.AddFiles);
						sqlCommand.Parameters.AddWithValue("AddHourlyRate" + i, item.AddHourlyRate == null ? (object)DBNull.Value : item.AddHourlyRate);
						sqlCommand.Parameters.AddWithValue("AddIndustry" + i, item.AddIndustry == null ? (object)DBNull.Value : item.AddIndustry);
						sqlCommand.Parameters.AddWithValue("AddPayementPractises" + i, item.AddPayementPractises == null ? (object)DBNull.Value : item.AddPayementPractises);
						sqlCommand.Parameters.AddWithValue("AddPricingGroup" + i, item.AddPricingGroup == null ? (object)DBNull.Value : item.AddPricingGroup);
						sqlCommand.Parameters.AddWithValue("AddRohArtikelNummer" + i, item.AddRohArtikelNummer == null ? (object)DBNull.Value : item.AddRohArtikelNummer);
						sqlCommand.Parameters.AddWithValue("AddShippingMethods" + i, item.AddShippingMethods == null ? (object)DBNull.Value : item.AddShippingMethods);
						sqlCommand.Parameters.AddWithValue("AddSlipCircle" + i, item.AddSlipCircle == null ? (object)DBNull.Value : item.AddSlipCircle);
						sqlCommand.Parameters.AddWithValue("AddSupplier" + i, item.AddSupplier == null ? (object)DBNull.Value : item.AddSupplier);
						sqlCommand.Parameters.AddWithValue("AddSupplierGroup" + i, item.AddSupplierGroup == null ? (object)DBNull.Value : item.AddSupplierGroup);
						sqlCommand.Parameters.AddWithValue("AddTermsOfPayment" + i, item.AddTermsOfPayment == null ? (object)DBNull.Value : item.AddTermsOfPayment);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration);
						sqlCommand.Parameters.AddWithValue("AltPositionsArticleBOM" + i, item.AltPositionsArticleBOM);
						sqlCommand.Parameters.AddWithValue("ArchiveArticle" + i, item.ArchiveArticle);
						sqlCommand.Parameters.AddWithValue("ArchiveCustomer" + i, item.ArchiveCustomer == null ? (object)DBNull.Value : item.ArchiveCustomer);
						sqlCommand.Parameters.AddWithValue("ArchiveSupplier" + i, item.ArchiveSupplier == null ? (object)DBNull.Value : item.ArchiveSupplier);
						sqlCommand.Parameters.AddWithValue("ArticleAddCustomerDocument" + i, item.ArticleAddCustomerDocument == null ? (object)DBNull.Value : item.ArticleAddCustomerDocument);
						sqlCommand.Parameters.AddWithValue("ArticleBOM" + i, item.ArticleBOM);
						sqlCommand.Parameters.AddWithValue("ArticleCts" + i, item.ArticleCts);
						sqlCommand.Parameters.AddWithValue("ArticleData" + i, item.ArticleData);
						sqlCommand.Parameters.AddWithValue("ArticleDeleteCustomerDocument" + i, item.ArticleDeleteCustomerDocument == null ? (object)DBNull.Value : item.ArticleDeleteCustomerDocument);
						sqlCommand.Parameters.AddWithValue("ArticleLogistics" + i, item.ArticleLogistics);
						sqlCommand.Parameters.AddWithValue("ArticleLogisticsPrices" + i, item.ArticleLogisticsPrices == null ? (object)DBNull.Value : item.ArticleLogisticsPrices);
						sqlCommand.Parameters.AddWithValue("ArticleOverview" + i, item.ArticleOverview);
						sqlCommand.Parameters.AddWithValue("ArticleProduction" + i, item.ArticleProduction);
						sqlCommand.Parameters.AddWithValue("ArticlePurchase" + i, item.ArticlePurchase);
						sqlCommand.Parameters.AddWithValue("ArticleQuality" + i, item.ArticleQuality);
						sqlCommand.Parameters.AddWithValue("Articles" + i, item.Articles);
						sqlCommand.Parameters.AddWithValue("ArticleSales" + i, item.ArticleSales);
						sqlCommand.Parameters.AddWithValue("ArticleSalesCustom" + i, item.ArticleSalesCustom);
						sqlCommand.Parameters.AddWithValue("ArticleSalesItem" + i, item.ArticleSalesItem);
						sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlEngineering" + i, item.ArticlesBOMCPControlEngineering == null ? (object)DBNull.Value : item.ArticlesBOMCPControlEngineering);
						sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlHistory" + i, item.ArticlesBOMCPControlHistory == null ? (object)DBNull.Value : item.ArticlesBOMCPControlHistory);
						sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlQuality" + i, item.ArticlesBOMCPControlQuality == null ? (object)DBNull.Value : item.ArticlesBOMCPControlQuality);
						sqlCommand.Parameters.AddWithValue("ArticleStatistics" + i, item.ArticleStatistics);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineering" + i, item.ArticleStatisticsEngineering);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineeringEdit" + i, item.ArticleStatisticsEngineeringEdit);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccounting" + i, item.ArticleStatisticsFinanceAccounting);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccountingEdit" + i, item.ArticleStatisticsFinanceAccountingEdit);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogistics" + i, item.ArticleStatisticsLogistics);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogisticsEdit" + i, item.ArticleStatisticsLogisticsEdit);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchase" + i, item.ArticleStatisticsPurchase);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchaseEdit" + i, item.ArticleStatisticsPurchaseEdit);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnic" + i, item.ArticleStatisticsTechnic);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnicEdit" + i, item.ArticleStatisticsTechnicEdit);
						sqlCommand.Parameters.AddWithValue("BomChangeRequests" + i, item.BomChangeRequests == null ? (object)DBNull.Value : item.BomChangeRequests);
						sqlCommand.Parameters.AddWithValue("CocType" + i, item.CocType == null ? (object)DBNull.Value : item.CocType);
						sqlCommand.Parameters.AddWithValue("ConditionAssignment" + i, item.ConditionAssignment == null ? (object)DBNull.Value : item.ConditionAssignment);
						sqlCommand.Parameters.AddWithValue("ConfigArticle" + i, item.ConfigArticle);
						sqlCommand.Parameters.AddWithValue("ConfigCustomer" + i, item.ConfigCustomer == null ? (object)DBNull.Value : item.ConfigCustomer);
						sqlCommand.Parameters.AddWithValue("ConfigSupplier" + i, item.ConfigSupplier == null ? (object)DBNull.Value : item.ConfigSupplier);
						sqlCommand.Parameters.AddWithValue("ContactAddress" + i, item.ContactAddress == null ? (object)DBNull.Value : item.ContactAddress);
						sqlCommand.Parameters.AddWithValue("ContactSalutation" + i, item.ContactSalutation == null ? (object)DBNull.Value : item.ContactSalutation);
						sqlCommand.Parameters.AddWithValue("CreateArticlePurchase" + i, item.CreateArticlePurchase);
						sqlCommand.Parameters.AddWithValue("CreateArticleSalesCustom" + i, item.CreateArticleSalesCustom);
						sqlCommand.Parameters.AddWithValue("CreateArticleSalesItem" + i, item.CreateArticleSalesItem);
						sqlCommand.Parameters.AddWithValue("CreateCustomerContactPerson" + i, item.CreateCustomerContactPerson == null ? (object)DBNull.Value : item.CreateCustomerContactPerson);
						sqlCommand.Parameters.AddWithValue("CreateSupplierContactPerson" + i, item.CreateSupplierContactPerson == null ? (object)DBNull.Value : item.CreateSupplierContactPerson);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Currencies" + i, item.Currencies == null ? (object)DBNull.Value : item.Currencies);
						sqlCommand.Parameters.AddWithValue("CustomerAddress" + i, item.CustomerAddress == null ? (object)DBNull.Value : item.CustomerAddress);
						sqlCommand.Parameters.AddWithValue("CustomerCommunication" + i, item.CustomerCommunication == null ? (object)DBNull.Value : item.CustomerCommunication);
						sqlCommand.Parameters.AddWithValue("CustomerContactPerson" + i, item.CustomerContactPerson == null ? (object)DBNull.Value : item.CustomerContactPerson);
						sqlCommand.Parameters.AddWithValue("CustomerData" + i, item.CustomerData == null ? (object)DBNull.Value : item.CustomerData);
						sqlCommand.Parameters.AddWithValue("CustomerGroup" + i, item.CustomerGroup == null ? (object)DBNull.Value : item.CustomerGroup);
						sqlCommand.Parameters.AddWithValue("CustomerHistory" + i, item.CustomerHistory == null ? (object)DBNull.Value : item.CustomerHistory);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("CustomerOverview" + i, item.CustomerOverview == null ? (object)DBNull.Value : item.CustomerOverview);
						sqlCommand.Parameters.AddWithValue("Customers" + i, item.Customers);
						sqlCommand.Parameters.AddWithValue("CustomerShipping" + i, item.CustomerShipping == null ? (object)DBNull.Value : item.CustomerShipping);
						sqlCommand.Parameters.AddWithValue("DeleteAltPositionsArticleBOM" + i, item.DeleteAltPositionsArticleBOM);
						sqlCommand.Parameters.AddWithValue("DeleteArticle" + i, item.DeleteArticle == null ? (object)DBNull.Value : item.DeleteArticle);
						sqlCommand.Parameters.AddWithValue("DeleteArticleBOM" + i, item.DeleteArticleBOM);
						sqlCommand.Parameters.AddWithValue("DeleteArticlePurchase" + i, item.DeleteArticlePurchase);
						sqlCommand.Parameters.AddWithValue("DeleteArticleSalesCustom" + i, item.DeleteArticleSalesCustom);
						sqlCommand.Parameters.AddWithValue("DeleteArticleSalesItem" + i, item.DeleteArticleSalesItem);
						sqlCommand.Parameters.AddWithValue("DeleteBCR" + i, item.DeleteBCR == null ? (object)DBNull.Value : item.DeleteBCR);
						sqlCommand.Parameters.AddWithValue("DeleteCustomerContactPerson" + i, item.DeleteCustomerContactPerson == null ? (object)DBNull.Value : item.DeleteCustomerContactPerson);
						sqlCommand.Parameters.AddWithValue("DeleteFiles" + i, item.DeleteFiles == null ? (object)DBNull.Value : item.DeleteFiles);
						sqlCommand.Parameters.AddWithValue("DeleteRohArtikelNummer" + i, item.DeleteRohArtikelNummer == null ? (object)DBNull.Value : item.DeleteRohArtikelNummer);
						sqlCommand.Parameters.AddWithValue("DeleteSupplierContactPerson" + i, item.DeleteSupplierContactPerson == null ? (object)DBNull.Value : item.DeleteSupplierContactPerson);
						sqlCommand.Parameters.AddWithValue("DiscountGroup" + i, item.DiscountGroup == null ? (object)DBNull.Value : item.DiscountGroup);
						sqlCommand.Parameters.AddWithValue("DownloadAllOutdatedEinkaufsPreis" + i, item.DownloadAllOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadAllOutdatedEinkaufsPreis);
						sqlCommand.Parameters.AddWithValue("DownloadFiles" + i, item.DownloadFiles == null ? (object)DBNull.Value : item.DownloadFiles);
						sqlCommand.Parameters.AddWithValue("DownloadOutdatedEinkaufsPreis" + i, item.DownloadOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadOutdatedEinkaufsPreis);
						sqlCommand.Parameters.AddWithValue("EdiConcern" + i, item.EdiConcern == null ? (object)DBNull.Value : item.EdiConcern);
						sqlCommand.Parameters.AddWithValue("EditAltPositionsArticleBOM" + i, item.EditAltPositionsArticleBOM);
						sqlCommand.Parameters.AddWithValue("EditArticle" + i, item.EditArticle);
						sqlCommand.Parameters.AddWithValue("EditArticleBOM" + i, item.EditArticleBOM);
						sqlCommand.Parameters.AddWithValue("EditArticleBOMPosition" + i, item.EditArticleBOMPosition);
						sqlCommand.Parameters.AddWithValue("EditArticleCts" + i, item.EditArticleCts);
						sqlCommand.Parameters.AddWithValue("EditArticleData" + i, item.EditArticleData);
						sqlCommand.Parameters.AddWithValue("EditArticleDesignation" + i, item.EditArticleDesignation);
						sqlCommand.Parameters.AddWithValue("EditArticleImage" + i, item.EditArticleImage);
						sqlCommand.Parameters.AddWithValue("EditArticleLogistics" + i, item.EditArticleLogistics);
						sqlCommand.Parameters.AddWithValue("EditArticleManager" + i, item.EditArticleManager);
						sqlCommand.Parameters.AddWithValue("EditArticleProduction" + i, item.EditArticleProduction);
						sqlCommand.Parameters.AddWithValue("EditArticlePurchase" + i, item.EditArticlePurchase);
						sqlCommand.Parameters.AddWithValue("EditArticleQuality" + i, item.EditArticleQuality);
						sqlCommand.Parameters.AddWithValue("EditArticleReference" + i, item.EditArticleReference == null ? (object)DBNull.Value : item.EditArticleReference);
						sqlCommand.Parameters.AddWithValue("EditArticleSalesCustom" + i, item.EditArticleSalesCustom);
						sqlCommand.Parameters.AddWithValue("EditArticleSalesItem" + i, item.EditArticleSalesItem);
						sqlCommand.Parameters.AddWithValue("EditCocType" + i, item.EditCocType == null ? (object)DBNull.Value : item.EditCocType);
						sqlCommand.Parameters.AddWithValue("EditConditionAssignment" + i, item.EditConditionAssignment == null ? (object)DBNull.Value : item.EditConditionAssignment);
						sqlCommand.Parameters.AddWithValue("EditContactAddress" + i, item.EditContactAddress == null ? (object)DBNull.Value : item.EditContactAddress);
						sqlCommand.Parameters.AddWithValue("EditContactSalutation" + i, item.EditContactSalutation == null ? (object)DBNull.Value : item.EditContactSalutation);
						sqlCommand.Parameters.AddWithValue("EditCurrencies" + i, item.EditCurrencies == null ? (object)DBNull.Value : item.EditCurrencies);
						sqlCommand.Parameters.AddWithValue("EditCustomer" + i, item.EditCustomer == null ? (object)DBNull.Value : item.EditCustomer);
						sqlCommand.Parameters.AddWithValue("EditCustomerAddress" + i, item.EditCustomerAddress == null ? (object)DBNull.Value : item.EditCustomerAddress);
						sqlCommand.Parameters.AddWithValue("EditCustomerCommunication" + i, item.EditCustomerCommunication == null ? (object)DBNull.Value : item.EditCustomerCommunication);
						sqlCommand.Parameters.AddWithValue("EditCustomerContactPerson" + i, item.EditCustomerContactPerson == null ? (object)DBNull.Value : item.EditCustomerContactPerson);
						sqlCommand.Parameters.AddWithValue("EditCustomerCoordination" + i, item.EditCustomerCoordination == null ? (object)DBNull.Value : item.EditCustomerCoordination);
						sqlCommand.Parameters.AddWithValue("EditCustomerData" + i, item.EditCustomerData == null ? (object)DBNull.Value : item.EditCustomerData);
						sqlCommand.Parameters.AddWithValue("EditCustomerGroup" + i, item.EditCustomerGroup == null ? (object)DBNull.Value : item.EditCustomerGroup);
						sqlCommand.Parameters.AddWithValue("EditCustomerImage" + i, item.EditCustomerImage == null ? (object)DBNull.Value : item.EditCustomerImage);
						sqlCommand.Parameters.AddWithValue("EditCustomerItemNumber" + i, item.EditCustomerItemNumber == null ? (object)DBNull.Value : item.EditCustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("EditCustomerShipping" + i, item.EditCustomerShipping == null ? (object)DBNull.Value : item.EditCustomerShipping);
						sqlCommand.Parameters.AddWithValue("EditDiscountGroup" + i, item.EditDiscountGroup == null ? (object)DBNull.Value : item.EditDiscountGroup);
						sqlCommand.Parameters.AddWithValue("EditEdiConcern" + i, item.EditEdiConcern == null ? (object)DBNull.Value : item.EditEdiConcern);
						sqlCommand.Parameters.AddWithValue("EditFibuFrame" + i, item.EditFibuFrame == null ? (object)DBNull.Value : item.EditFibuFrame);
						sqlCommand.Parameters.AddWithValue("EditHourlyRate" + i, item.EditHourlyRate == null ? (object)DBNull.Value : item.EditHourlyRate);
						sqlCommand.Parameters.AddWithValue("EditIndustry" + i, item.EditIndustry == null ? (object)DBNull.Value : item.EditIndustry);
						sqlCommand.Parameters.AddWithValue("EditLagerCCID" + i, item.EditLagerCCID == null ? (object)DBNull.Value : item.EditLagerCCID);
						sqlCommand.Parameters.AddWithValue("EditLagerMinStock" + i, item.EditLagerMinStock);
						sqlCommand.Parameters.AddWithValue("EditLagerOrderProposal" + i, item.EditLagerOrderProposal == null ? (object)DBNull.Value : item.EditLagerOrderProposal);
						sqlCommand.Parameters.AddWithValue("EditLagerStock" + i, item.EditLagerStock == null ? (object)DBNull.Value : item.EditLagerStock);
						sqlCommand.Parameters.AddWithValue("EditPayementPractises" + i, item.EditPayementPractises == null ? (object)DBNull.Value : item.EditPayementPractises);
						sqlCommand.Parameters.AddWithValue("EditPricingGroup" + i, item.EditPricingGroup == null ? (object)DBNull.Value : item.EditPricingGroup);
						sqlCommand.Parameters.AddWithValue("EditRohArtikelNummer" + i, item.EditRohArtikelNummer == null ? (object)DBNull.Value : item.EditRohArtikelNummer);
						sqlCommand.Parameters.AddWithValue("EditShippingMethods" + i, item.EditShippingMethods == null ? (object)DBNull.Value : item.EditShippingMethods);
						sqlCommand.Parameters.AddWithValue("EditSlipCircle" + i, item.EditSlipCircle == null ? (object)DBNull.Value : item.EditSlipCircle);
						sqlCommand.Parameters.AddWithValue("EditSupplier" + i, item.EditSupplier == null ? (object)DBNull.Value : item.EditSupplier);
						sqlCommand.Parameters.AddWithValue("EditSupplierAddress" + i, item.EditSupplierAddress == null ? (object)DBNull.Value : item.EditSupplierAddress);
						sqlCommand.Parameters.AddWithValue("EditSupplierCommunication" + i, item.EditSupplierCommunication == null ? (object)DBNull.Value : item.EditSupplierCommunication);
						sqlCommand.Parameters.AddWithValue("EditSupplierContactPerson" + i, item.EditSupplierContactPerson == null ? (object)DBNull.Value : item.EditSupplierContactPerson);
						sqlCommand.Parameters.AddWithValue("EditSupplierCoordination" + i, item.EditSupplierCoordination == null ? (object)DBNull.Value : item.EditSupplierCoordination);
						sqlCommand.Parameters.AddWithValue("EditSupplierData" + i, item.EditSupplierData == null ? (object)DBNull.Value : item.EditSupplierData);
						sqlCommand.Parameters.AddWithValue("EditSupplierGroup" + i, item.EditSupplierGroup == null ? (object)DBNull.Value : item.EditSupplierGroup);
						sqlCommand.Parameters.AddWithValue("EditSupplierImage" + i, item.EditSupplierImage == null ? (object)DBNull.Value : item.EditSupplierImage);
						sqlCommand.Parameters.AddWithValue("EditSupplierShipping" + i, item.EditSupplierShipping == null ? (object)DBNull.Value : item.EditSupplierShipping);
						sqlCommand.Parameters.AddWithValue("EditTermsOfPayment" + i, item.EditTermsOfPayment == null ? (object)DBNull.Value : item.EditTermsOfPayment);
						sqlCommand.Parameters.AddWithValue("EDrawingEdit" + i, item.EDrawingEdit == null ? (object)DBNull.Value : item.EDrawingEdit);
						sqlCommand.Parameters.AddWithValue("EinkaufsPreisUpdate" + i, item.EinkaufsPreisUpdate == null ? (object)DBNull.Value : item.EinkaufsPreisUpdate);
						sqlCommand.Parameters.AddWithValue("FibuFrame" + i, item.FibuFrame == null ? (object)DBNull.Value : item.FibuFrame);
						sqlCommand.Parameters.AddWithValue("GetRohArtikelNummer" + i, item.GetRohArtikelNummer == null ? (object)DBNull.Value : item.GetRohArtikelNummer);
						sqlCommand.Parameters.AddWithValue("HourlyRate" + i, item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
						sqlCommand.Parameters.AddWithValue("ImportArticleBOM" + i, item.ImportArticleBOM);
						sqlCommand.Parameters.AddWithValue("Industry" + i, item.Industry == null ? (object)DBNull.Value : item.Industry);
						sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
						sqlCommand.Parameters.AddWithValue("LagerArticleLogistics" + i, item.LagerArticleLogistics);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("ModuleAdministrator" + i, item.ModuleAdministrator == null ? (object)DBNull.Value : item.ModuleAdministrator);
						sqlCommand.Parameters.AddWithValue("offer" + i, item.offer == null ? (object)DBNull.Value : item.offer);
						sqlCommand.Parameters.AddWithValue("OfferRequestADD" + i, item.OfferRequestADD == null ? (object)DBNull.Value : item.OfferRequestADD);
						sqlCommand.Parameters.AddWithValue("OfferRequestApplyPrice" + i, item.OfferRequestApplyPrice == null ? (object)DBNull.Value : item.OfferRequestApplyPrice);
						sqlCommand.Parameters.AddWithValue("OfferRequestDelete" + i, item.OfferRequestDelete == null ? (object)DBNull.Value : item.OfferRequestDelete);
						sqlCommand.Parameters.AddWithValue("OfferRequestEdit" + i, item.OfferRequestEdit == null ? (object)DBNull.Value : item.OfferRequestEdit);
						sqlCommand.Parameters.AddWithValue("OfferRequestEditEmail" + i, item.OfferRequestEditEmail == null ? (object)DBNull.Value : item.OfferRequestEditEmail);
						sqlCommand.Parameters.AddWithValue("OfferRequestSendEmail" + i, item.OfferRequestSendEmail == null ? (object)DBNull.Value : item.OfferRequestSendEmail);
						sqlCommand.Parameters.AddWithValue("OfferRequestView" + i, item.OfferRequestView == null ? (object)DBNull.Value : item.OfferRequestView);
						sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoAdd" + i, item.PackagingsLgtPhotoAdd == null ? (object)DBNull.Value : item.PackagingsLgtPhotoAdd);
						sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoDelete" + i, item.PackagingsLgtPhotoDelete == null ? (object)DBNull.Value : item.PackagingsLgtPhotoDelete);
						sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoView" + i, item.PackagingsLgtPhotoView == null ? (object)DBNull.Value : item.PackagingsLgtPhotoView);
						sqlCommand.Parameters.AddWithValue("PayementPractises" + i, item.PayementPractises == null ? (object)DBNull.Value : item.PayementPractises);
						sqlCommand.Parameters.AddWithValue("PMAddCable" + i, item.PMAddCable == null ? (object)DBNull.Value : item.PMAddCable);
						sqlCommand.Parameters.AddWithValue("PMAddMileStone" + i, item.PMAddMileStone == null ? (object)DBNull.Value : item.PMAddMileStone);
						sqlCommand.Parameters.AddWithValue("PMAddProject" + i, item.PMAddProject == null ? (object)DBNull.Value : item.PMAddProject);
						sqlCommand.Parameters.AddWithValue("PMDeleteCable" + i, item.PMDeleteCable == null ? (object)DBNull.Value : item.PMDeleteCable);
						sqlCommand.Parameters.AddWithValue("PMDeleteMileStone" + i, item.PMDeleteMileStone == null ? (object)DBNull.Value : item.PMDeleteMileStone);
						sqlCommand.Parameters.AddWithValue("PMDeleteProject" + i, item.PMDeleteProject == null ? (object)DBNull.Value : item.PMDeleteProject);
						sqlCommand.Parameters.AddWithValue("PMEditCable" + i, item.PMEditCable == null ? (object)DBNull.Value : item.PMEditCable);
						sqlCommand.Parameters.AddWithValue("PMEditMileStone" + i, item.PMEditMileStone == null ? (object)DBNull.Value : item.PMEditMileStone);
						sqlCommand.Parameters.AddWithValue("PMEditProject" + i, item.PMEditProject == null ? (object)DBNull.Value : item.PMEditProject);
						sqlCommand.Parameters.AddWithValue("PMModule" + i, item.PMModule == null ? (object)DBNull.Value : item.PMModule);
						sqlCommand.Parameters.AddWithValue("PMViewProjectsCompact" + i, item.PMViewProjectsCompact == null ? (object)DBNull.Value : item.PMViewProjectsCompact);
						sqlCommand.Parameters.AddWithValue("PMViewProjectsDetail" + i, item.PMViewProjectsDetail == null ? (object)DBNull.Value : item.PMViewProjectsDetail);
						sqlCommand.Parameters.AddWithValue("PMViewProjectsMedium" + i, item.PMViewProjectsMedium == null ? (object)DBNull.Value : item.PMViewProjectsMedium);
						sqlCommand.Parameters.AddWithValue("PricingGroup" + i, item.PricingGroup == null ? (object)DBNull.Value : item.PricingGroup);
						sqlCommand.Parameters.AddWithValue("RemoveArticleReference" + i, item.RemoveArticleReference == null ? (object)DBNull.Value : item.RemoveArticleReference);
						sqlCommand.Parameters.AddWithValue("Settings" + i, item.Settings == null ? (object)DBNull.Value : item.Settings);
						sqlCommand.Parameters.AddWithValue("ShippingMethods" + i, item.ShippingMethods == null ? (object)DBNull.Value : item.ShippingMethods);
						sqlCommand.Parameters.AddWithValue("SlipCircle" + i, item.SlipCircle == null ? (object)DBNull.Value : item.SlipCircle);
						sqlCommand.Parameters.AddWithValue("SupplierAddress" + i, item.SupplierAddress == null ? (object)DBNull.Value : item.SupplierAddress);
						sqlCommand.Parameters.AddWithValue("SupplierAttachementAddFile" + i, item.SupplierAttachementAddFile == null ? (object)DBNull.Value : item.SupplierAttachementAddFile);
						sqlCommand.Parameters.AddWithValue("SupplierAttachementGetFile" + i, item.SupplierAttachementGetFile == null ? (object)DBNull.Value : item.SupplierAttachementGetFile);
						sqlCommand.Parameters.AddWithValue("SupplierAttachementRemoveFile" + i, item.SupplierAttachementRemoveFile == null ? (object)DBNull.Value : item.SupplierAttachementRemoveFile);
						sqlCommand.Parameters.AddWithValue("SupplierCommunication" + i, item.SupplierCommunication == null ? (object)DBNull.Value : item.SupplierCommunication);
						sqlCommand.Parameters.AddWithValue("SupplierContactPerson" + i, item.SupplierContactPerson == null ? (object)DBNull.Value : item.SupplierContactPerson);
						sqlCommand.Parameters.AddWithValue("SupplierData" + i, item.SupplierData == null ? (object)DBNull.Value : item.SupplierData);
						sqlCommand.Parameters.AddWithValue("SupplierGroup" + i, item.SupplierGroup == null ? (object)DBNull.Value : item.SupplierGroup);
						sqlCommand.Parameters.AddWithValue("SupplierHistory" + i, item.SupplierHistory == null ? (object)DBNull.Value : item.SupplierHistory);
						sqlCommand.Parameters.AddWithValue("SupplierOverview" + i, item.SupplierOverview == null ? (object)DBNull.Value : item.SupplierOverview);
						sqlCommand.Parameters.AddWithValue("Suppliers" + i, item.Suppliers);
						sqlCommand.Parameters.AddWithValue("SupplierShipping" + i, item.SupplierShipping == null ? (object)DBNull.Value : item.SupplierShipping);
						sqlCommand.Parameters.AddWithValue("TermsOfPayment" + i, item.TermsOfPayment == null ? (object)DBNull.Value : item.TermsOfPayment);
						sqlCommand.Parameters.AddWithValue("UploadAltPositionsArticleBOM" + i, item.UploadAltPositionsArticleBOM);
						sqlCommand.Parameters.AddWithValue("UploadArticleBOM" + i, item.UploadArticleBOM);
						sqlCommand.Parameters.AddWithValue("ValidateArticleBOM" + i, item.ValidateArticleBOM == null ? (object)DBNull.Value : item.ValidateArticleBOM);
						sqlCommand.Parameters.AddWithValue("ValidateBCR" + i, item.ValidateBCR == null ? (object)DBNull.Value : item.ValidateBCR);
						sqlCommand.Parameters.AddWithValue("ViewAltPositionsArticleBOM" + i, item.ViewAltPositionsArticleBOM);
						sqlCommand.Parameters.AddWithValue("ViewArticleLog" + i, item.ViewArticleLog);
						sqlCommand.Parameters.AddWithValue("ViewArticleReference" + i, item.ViewArticleReference == null ? (object)DBNull.Value : item.ViewArticleReference);
						sqlCommand.Parameters.AddWithValue("ViewArticles" + i, item.ViewArticles);
						sqlCommand.Parameters.AddWithValue("ViewBCR" + i, item.ViewBCR == null ? (object)DBNull.Value : item.ViewBCR);
						sqlCommand.Parameters.AddWithValue("ViewCustomers" + i, item.ViewCustomers == null ? (object)DBNull.Value : item.ViewCustomers);
						sqlCommand.Parameters.AddWithValue("ViewLPCustomer" + i, item.ViewLPCustomer == null ? (object)DBNull.Value : item.ViewLPCustomer);
						sqlCommand.Parameters.AddWithValue("ViewLPSupplier" + i, item.ViewLPSupplier == null ? (object)DBNull.Value : item.ViewLPSupplier);
						sqlCommand.Parameters.AddWithValue("ViewSupplierAddressComments" + i, item.ViewSupplierAddressComments == null ? (object)DBNull.Value : item.ViewSupplierAddressComments);
						sqlCommand.Parameters.AddWithValue("ViewSuppliers" + i, item.ViewSuppliers == null ? (object)DBNull.Value : item.ViewSuppliers);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Entities.Tables.BSD.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_AccessProfile] SET [AccessProfileName]=@AccessProfileName, [AddArticle]=@AddArticle, [AddArticleReference]=@AddArticleReference, [AddBCR]=@AddBCR, [AddCocType]=@AddCocType, [AddConditionAssignment]=@AddConditionAssignment, [AddContactAddress]=@AddContactAddress, [AddContactSalutation]=@AddContactSalutation, [AddCurrencies]=@AddCurrencies, [AddCustomer]=@AddCustomer, [AddCustomerGroup]=@AddCustomerGroup, [AddCustomerItemNumber]=@AddCustomerItemNumber, [AddDiscountGroup]=@AddDiscountGroup, [AddEdiConcern]=@AddEdiConcern, [AddFibuFrame]=@AddFibuFrame, [AddFiles]=@AddFiles, [AddHourlyRate]=@AddHourlyRate, [AddIndustry]=@AddIndustry, [AddPayementPractises]=@AddPayementPractises, [AddPricingGroup]=@AddPricingGroup, [AddRohArtikelNummer]=@AddRohArtikelNummer, [AddShippingMethods]=@AddShippingMethods, [AddSlipCircle]=@AddSlipCircle, [AddSupplier]=@AddSupplier, [AddSupplierGroup]=@AddSupplierGroup, [AddTermsOfPayment]=@AddTermsOfPayment, [Administration]=@Administration, [AltPositionsArticleBOM]=@AltPositionsArticleBOM, [ArchiveArticle]=@ArchiveArticle, [ArchiveCustomer]=@ArchiveCustomer, [ArchiveSupplier]=@ArchiveSupplier, [ArticleAddCustomerDocument]=@ArticleAddCustomerDocument, [ArticleBOM]=@ArticleBOM, [ArticleCts]=@ArticleCts, [ArticleData]=@ArticleData, [ArticleDeleteCustomerDocument]=@ArticleDeleteCustomerDocument, [ArticleLogistics]=@ArticleLogistics, [ArticleLogisticsPrices]=@ArticleLogisticsPrices, [ArticleOverview]=@ArticleOverview, [ArticleProduction]=@ArticleProduction, [ArticlePurchase]=@ArticlePurchase, [ArticleQuality]=@ArticleQuality, [Articles]=@Articles, [ArticleSales]=@ArticleSales, [ArticleSalesCustom]=@ArticleSalesCustom, [ArticleSalesItem]=@ArticleSalesItem, [ArticlesBOMCPControlEngineering]=@ArticlesBOMCPControlEngineering, [ArticlesBOMCPControlHistory]=@ArticlesBOMCPControlHistory, [ArticlesBOMCPControlQuality]=@ArticlesBOMCPControlQuality, [ArticleStatistics]=@ArticleStatistics, [ArticleStatisticsEngineering]=@ArticleStatisticsEngineering, [ArticleStatisticsEngineeringEdit]=@ArticleStatisticsEngineeringEdit, [ArticleStatisticsFinanceAccounting]=@ArticleStatisticsFinanceAccounting, [ArticleStatisticsFinanceAccountingEdit]=@ArticleStatisticsFinanceAccountingEdit, [ArticleStatisticsLogistics]=@ArticleStatisticsLogistics, [ArticleStatisticsLogisticsEdit]=@ArticleStatisticsLogisticsEdit, [ArticleStatisticsPurchase]=@ArticleStatisticsPurchase, [ArticleStatisticsPurchaseEdit]=@ArticleStatisticsPurchaseEdit, [ArticleStatisticsTechnic]=@ArticleStatisticsTechnic, [ArticleStatisticsTechnicEdit]=@ArticleStatisticsTechnicEdit, [BomChangeRequests]=@BomChangeRequests, [CocType]=@CocType, [ConditionAssignment]=@ConditionAssignment, [ConfigArticle]=@ConfigArticle, [ConfigCustomer]=@ConfigCustomer, [ConfigSupplier]=@ConfigSupplier, [ContactAddress]=@ContactAddress, [ContactSalutation]=@ContactSalutation, [CreateArticlePurchase]=@CreateArticlePurchase, [CreateArticleSalesCustom]=@CreateArticleSalesCustom, [CreateArticleSalesItem]=@CreateArticleSalesItem, [CreateCustomerContactPerson]=@CreateCustomerContactPerson, [CreateSupplierContactPerson]=@CreateSupplierContactPerson, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [Currencies]=@Currencies, [CustomerAddress]=@CustomerAddress, [CustomerCommunication]=@CustomerCommunication, [CustomerContactPerson]=@CustomerContactPerson, [CustomerData]=@CustomerData, [CustomerGroup]=@CustomerGroup, [CustomerHistory]=@CustomerHistory, [CustomerItemNumber]=@CustomerItemNumber, [CustomerOverview]=@CustomerOverview, [Customers]=@Customers, [CustomerShipping]=@CustomerShipping, [DeleteAltPositionsArticleBOM]=@DeleteAltPositionsArticleBOM, [DeleteArticle]=@DeleteArticle, [DeleteArticleBOM]=@DeleteArticleBOM, [DeleteArticlePurchase]=@DeleteArticlePurchase, [DeleteArticleSalesCustom]=@DeleteArticleSalesCustom, [DeleteArticleSalesItem]=@DeleteArticleSalesItem, [DeleteBCR]=@DeleteBCR, [DeleteCustomerContactPerson]=@DeleteCustomerContactPerson, [DeleteFiles]=@DeleteFiles, [DeleteRohArtikelNummer]=@DeleteRohArtikelNummer, [DeleteSupplierContactPerson]=@DeleteSupplierContactPerson, [DiscountGroup]=@DiscountGroup, [DownloadAllOutdatedEinkaufsPreis]=@DownloadAllOutdatedEinkaufsPreis, [DownloadFiles]=@DownloadFiles, [DownloadOutdatedEinkaufsPreis]=@DownloadOutdatedEinkaufsPreis, [EdiConcern]=@EdiConcern, [EditAltPositionsArticleBOM]=@EditAltPositionsArticleBOM, [EditArticle]=@EditArticle, [EditArticleBOM]=@EditArticleBOM, [EditArticleBOMPosition]=@EditArticleBOMPosition, [EditArticleCts]=@EditArticleCts, [EditArticleData]=@EditArticleData, [EditArticleDesignation]=@EditArticleDesignation, [EditArticleImage]=@EditArticleImage, [EditArticleLogistics]=@EditArticleLogistics, [EditArticleManager]=@EditArticleManager, [EditArticleProduction]=@EditArticleProduction, [EditArticlePurchase]=@EditArticlePurchase, [EditArticleQuality]=@EditArticleQuality, [EditArticleReference]=@EditArticleReference, [EditArticleSalesCustom]=@EditArticleSalesCustom, [EditArticleSalesItem]=@EditArticleSalesItem, [EditCocType]=@EditCocType, [EditConditionAssignment]=@EditConditionAssignment, [EditContactAddress]=@EditContactAddress, [EditContactSalutation]=@EditContactSalutation, [EditCurrencies]=@EditCurrencies, [EditCustomer]=@EditCustomer, [EditCustomerAddress]=@EditCustomerAddress, [EditCustomerCommunication]=@EditCustomerCommunication, [EditCustomerContactPerson]=@EditCustomerContactPerson, [EditCustomerCoordination]=@EditCustomerCoordination, [EditCustomerData]=@EditCustomerData, [EditCustomerGroup]=@EditCustomerGroup, [EditCustomerImage]=@EditCustomerImage, [EditCustomerItemNumber]=@EditCustomerItemNumber, [EditCustomerShipping]=@EditCustomerShipping, [EditDiscountGroup]=@EditDiscountGroup, [EditEdiConcern]=@EditEdiConcern, [EditFibuFrame]=@EditFibuFrame, [EditHourlyRate]=@EditHourlyRate, [EditIndustry]=@EditIndustry, [EditLagerCCID]=@EditLagerCCID, [EditLagerMinStock]=@EditLagerMinStock, [EditLagerOrderProposal]=@EditLagerOrderProposal, [EditLagerStock]=@EditLagerStock, [EditPayementPractises]=@EditPayementPractises, [EditPricingGroup]=@EditPricingGroup, [EditRohArtikelNummer]=@EditRohArtikelNummer, [EditShippingMethods]=@EditShippingMethods, [EditSlipCircle]=@EditSlipCircle, [EditSupplier]=@EditSupplier, [EditSupplierAddress]=@EditSupplierAddress, [EditSupplierCommunication]=@EditSupplierCommunication, [EditSupplierContactPerson]=@EditSupplierContactPerson, [EditSupplierCoordination]=@EditSupplierCoordination, [EditSupplierData]=@EditSupplierData, [EditSupplierGroup]=@EditSupplierGroup, [EditSupplierImage]=@EditSupplierImage, [EditSupplierShipping]=@EditSupplierShipping, [EditTermsOfPayment]=@EditTermsOfPayment, [EDrawingEdit]=@EDrawingEdit, [EinkaufsPreisUpdate]=@EinkaufsPreisUpdate, [FibuFrame]=@FibuFrame, [GetRohArtikelNummer]=@GetRohArtikelNummer, [HourlyRate]=@HourlyRate, [ImportArticleBOM]=@ImportArticleBOM, [Industry]=@Industry, [isDefault]=@isDefault, [LagerArticleLogistics]=@LagerArticleLogistics, [ModuleActivated]=@ModuleActivated, [ModuleAdministrator]=@ModuleAdministrator, [offer]=@offer, [OfferRequestADD]=@OfferRequestADD, [OfferRequestApplyPrice]=@OfferRequestApplyPrice, [OfferRequestDelete]=@OfferRequestDelete, [OfferRequestEdit]=@OfferRequestEdit, [OfferRequestEditEmail]=@OfferRequestEditEmail, [OfferRequestSendEmail]=@OfferRequestSendEmail, [OfferRequestView]=@OfferRequestView, [PackagingsLgtPhotoAdd]=@PackagingsLgtPhotoAdd, [PackagingsLgtPhotoDelete]=@PackagingsLgtPhotoDelete, [PackagingsLgtPhotoView]=@PackagingsLgtPhotoView, [PayementPractises]=@PayementPractises, [PMAddCable]=@PMAddCable, [PMAddMileStone]=@PMAddMileStone, [PMAddProject]=@PMAddProject, [PMDeleteCable]=@PMDeleteCable, [PMDeleteMileStone]=@PMDeleteMileStone, [PMDeleteProject]=@PMDeleteProject, [PMEditCable]=@PMEditCable, [PMEditMileStone]=@PMEditMileStone, [PMEditProject]=@PMEditProject, [PMModule]=@PMModule, [PMViewProjectsCompact]=@PMViewProjectsCompact, [PMViewProjectsDetail]=@PMViewProjectsDetail, [PMViewProjectsMedium]=@PMViewProjectsMedium, [PricingGroup]=@PricingGroup, [RemoveArticleReference]=@RemoveArticleReference, [Settings]=@Settings, [ShippingMethods]=@ShippingMethods, [SlipCircle]=@SlipCircle, [SupplierAddress]=@SupplierAddress, [SupplierAttachementAddFile]=@SupplierAttachementAddFile, [SupplierAttachementGetFile]=@SupplierAttachementGetFile, [SupplierAttachementRemoveFile]=@SupplierAttachementRemoveFile, [SupplierCommunication]=@SupplierCommunication, [SupplierContactPerson]=@SupplierContactPerson, [SupplierData]=@SupplierData, [SupplierGroup]=@SupplierGroup, [SupplierHistory]=@SupplierHistory, [SupplierOverview]=@SupplierOverview, [Suppliers]=@Suppliers, [SupplierShipping]=@SupplierShipping, [TermsOfPayment]=@TermsOfPayment, [UploadAltPositionsArticleBOM]=@UploadAltPositionsArticleBOM, [UploadArticleBOM]=@UploadArticleBOM, [ValidateArticleBOM]=@ValidateArticleBOM, [ValidateBCR]=@ValidateBCR, [ViewAltPositionsArticleBOM]=@ViewAltPositionsArticleBOM, [ViewArticleLog]=@ViewArticleLog, [ViewArticleReference]=@ViewArticleReference, [ViewArticles]=@ViewArticles, [ViewBCR]=@ViewBCR, [ViewCustomers]=@ViewCustomers, [ViewLPCustomer]=@ViewLPCustomer, [ViewLPSupplier]=@ViewLPSupplier, [ViewSupplierAddressComments]=@ViewSupplierAddressComments, [ViewSuppliers]=@ViewSuppliers WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
				sqlCommand.Parameters.AddWithValue("AddArticle", item.AddArticle);
				sqlCommand.Parameters.AddWithValue("AddArticleReference", item.AddArticleReference == null ? (object)DBNull.Value : item.AddArticleReference);
				sqlCommand.Parameters.AddWithValue("AddBCR", item.AddBCR == null ? (object)DBNull.Value : item.AddBCR);
				sqlCommand.Parameters.AddWithValue("AddCocType", item.AddCocType == null ? (object)DBNull.Value : item.AddCocType);
				sqlCommand.Parameters.AddWithValue("AddConditionAssignment", item.AddConditionAssignment == null ? (object)DBNull.Value : item.AddConditionAssignment);
				sqlCommand.Parameters.AddWithValue("AddContactAddress", item.AddContactAddress == null ? (object)DBNull.Value : item.AddContactAddress);
				sqlCommand.Parameters.AddWithValue("AddContactSalutation", item.AddContactSalutation == null ? (object)DBNull.Value : item.AddContactSalutation);
				sqlCommand.Parameters.AddWithValue("AddCurrencies", item.AddCurrencies == null ? (object)DBNull.Value : item.AddCurrencies);
				sqlCommand.Parameters.AddWithValue("AddCustomer", item.AddCustomer == null ? (object)DBNull.Value : item.AddCustomer);
				sqlCommand.Parameters.AddWithValue("AddCustomerGroup", item.AddCustomerGroup == null ? (object)DBNull.Value : item.AddCustomerGroup);
				sqlCommand.Parameters.AddWithValue("AddCustomerItemNumber", item.AddCustomerItemNumber == null ? (object)DBNull.Value : item.AddCustomerItemNumber);
				sqlCommand.Parameters.AddWithValue("AddDiscountGroup", item.AddDiscountGroup == null ? (object)DBNull.Value : item.AddDiscountGroup);
				sqlCommand.Parameters.AddWithValue("AddEdiConcern", item.AddEdiConcern == null ? (object)DBNull.Value : item.AddEdiConcern);
				sqlCommand.Parameters.AddWithValue("AddFibuFrame", item.AddFibuFrame == null ? (object)DBNull.Value : item.AddFibuFrame);
				sqlCommand.Parameters.AddWithValue("AddFiles", item.AddFiles == null ? (object)DBNull.Value : item.AddFiles);
				sqlCommand.Parameters.AddWithValue("AddHourlyRate", item.AddHourlyRate == null ? (object)DBNull.Value : item.AddHourlyRate);
				sqlCommand.Parameters.AddWithValue("AddIndustry", item.AddIndustry == null ? (object)DBNull.Value : item.AddIndustry);
				sqlCommand.Parameters.AddWithValue("AddPayementPractises", item.AddPayementPractises == null ? (object)DBNull.Value : item.AddPayementPractises);
				sqlCommand.Parameters.AddWithValue("AddPricingGroup", item.AddPricingGroup == null ? (object)DBNull.Value : item.AddPricingGroup);
				sqlCommand.Parameters.AddWithValue("AddRohArtikelNummer", item.AddRohArtikelNummer == null ? (object)DBNull.Value : item.AddRohArtikelNummer);
				sqlCommand.Parameters.AddWithValue("AddShippingMethods", item.AddShippingMethods == null ? (object)DBNull.Value : item.AddShippingMethods);
				sqlCommand.Parameters.AddWithValue("AddSlipCircle", item.AddSlipCircle == null ? (object)DBNull.Value : item.AddSlipCircle);
				sqlCommand.Parameters.AddWithValue("AddSupplier", item.AddSupplier == null ? (object)DBNull.Value : item.AddSupplier);
				sqlCommand.Parameters.AddWithValue("AddSupplierGroup", item.AddSupplierGroup == null ? (object)DBNull.Value : item.AddSupplierGroup);
				sqlCommand.Parameters.AddWithValue("AddTermsOfPayment", item.AddTermsOfPayment == null ? (object)DBNull.Value : item.AddTermsOfPayment);
				sqlCommand.Parameters.AddWithValue("Administration", item.Administration);
				sqlCommand.Parameters.AddWithValue("AltPositionsArticleBOM", item.AltPositionsArticleBOM);
				sqlCommand.Parameters.AddWithValue("ArchiveArticle", item.ArchiveArticle);
				sqlCommand.Parameters.AddWithValue("ArchiveCustomer", item.ArchiveCustomer == null ? (object)DBNull.Value : item.ArchiveCustomer);
				sqlCommand.Parameters.AddWithValue("ArchiveSupplier", item.ArchiveSupplier == null ? (object)DBNull.Value : item.ArchiveSupplier);
				sqlCommand.Parameters.AddWithValue("ArticleAddCustomerDocument", item.ArticleAddCustomerDocument == null ? (object)DBNull.Value : item.ArticleAddCustomerDocument);
				sqlCommand.Parameters.AddWithValue("ArticleBOM", item.ArticleBOM);
				sqlCommand.Parameters.AddWithValue("ArticleCts", item.ArticleCts);
				sqlCommand.Parameters.AddWithValue("ArticleData", item.ArticleData);
				sqlCommand.Parameters.AddWithValue("ArticleDeleteCustomerDocument", item.ArticleDeleteCustomerDocument == null ? (object)DBNull.Value : item.ArticleDeleteCustomerDocument);
				sqlCommand.Parameters.AddWithValue("ArticleLogistics", item.ArticleLogistics);
				sqlCommand.Parameters.AddWithValue("ArticleLogisticsPrices", item.ArticleLogisticsPrices == null ? (object)DBNull.Value : item.ArticleLogisticsPrices);
				sqlCommand.Parameters.AddWithValue("ArticleOverview", item.ArticleOverview);
				sqlCommand.Parameters.AddWithValue("ArticleProduction", item.ArticleProduction);
				sqlCommand.Parameters.AddWithValue("ArticlePurchase", item.ArticlePurchase);
				sqlCommand.Parameters.AddWithValue("ArticleQuality", item.ArticleQuality);
				sqlCommand.Parameters.AddWithValue("Articles", item.Articles);
				sqlCommand.Parameters.AddWithValue("ArticleSales", item.ArticleSales);
				sqlCommand.Parameters.AddWithValue("ArticleSalesCustom", item.ArticleSalesCustom);
				sqlCommand.Parameters.AddWithValue("ArticleSalesItem", item.ArticleSalesItem);
				sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlEngineering", item.ArticlesBOMCPControlEngineering == null ? (object)DBNull.Value : item.ArticlesBOMCPControlEngineering);
				sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlHistory", item.ArticlesBOMCPControlHistory == null ? (object)DBNull.Value : item.ArticlesBOMCPControlHistory);
				sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlQuality", item.ArticlesBOMCPControlQuality == null ? (object)DBNull.Value : item.ArticlesBOMCPControlQuality);
				sqlCommand.Parameters.AddWithValue("ArticleStatistics", item.ArticleStatistics);
				sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineering", item.ArticleStatisticsEngineering);
				sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineeringEdit", item.ArticleStatisticsEngineeringEdit);
				sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccounting", item.ArticleStatisticsFinanceAccounting);
				sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccountingEdit", item.ArticleStatisticsFinanceAccountingEdit);
				sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogistics", item.ArticleStatisticsLogistics);
				sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogisticsEdit", item.ArticleStatisticsLogisticsEdit);
				sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchase", item.ArticleStatisticsPurchase);
				sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchaseEdit", item.ArticleStatisticsPurchaseEdit);
				sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnic", item.ArticleStatisticsTechnic);
				sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnicEdit", item.ArticleStatisticsTechnicEdit);
				sqlCommand.Parameters.AddWithValue("BomChangeRequests", item.BomChangeRequests == null ? (object)DBNull.Value : item.BomChangeRequests);
				sqlCommand.Parameters.AddWithValue("CocType", item.CocType == null ? (object)DBNull.Value : item.CocType);
				sqlCommand.Parameters.AddWithValue("ConditionAssignment", item.ConditionAssignment == null ? (object)DBNull.Value : item.ConditionAssignment);
				sqlCommand.Parameters.AddWithValue("ConfigArticle", item.ConfigArticle);
				sqlCommand.Parameters.AddWithValue("ConfigCustomer", item.ConfigCustomer == null ? (object)DBNull.Value : item.ConfigCustomer);
				sqlCommand.Parameters.AddWithValue("ConfigSupplier", item.ConfigSupplier == null ? (object)DBNull.Value : item.ConfigSupplier);
				sqlCommand.Parameters.AddWithValue("ContactAddress", item.ContactAddress == null ? (object)DBNull.Value : item.ContactAddress);
				sqlCommand.Parameters.AddWithValue("ContactSalutation", item.ContactSalutation == null ? (object)DBNull.Value : item.ContactSalutation);
				sqlCommand.Parameters.AddWithValue("CreateArticlePurchase", item.CreateArticlePurchase);
				sqlCommand.Parameters.AddWithValue("CreateArticleSalesCustom", item.CreateArticleSalesCustom);
				sqlCommand.Parameters.AddWithValue("CreateArticleSalesItem", item.CreateArticleSalesItem);
				sqlCommand.Parameters.AddWithValue("CreateCustomerContactPerson", item.CreateCustomerContactPerson == null ? (object)DBNull.Value : item.CreateCustomerContactPerson);
				sqlCommand.Parameters.AddWithValue("CreateSupplierContactPerson", item.CreateSupplierContactPerson == null ? (object)DBNull.Value : item.CreateSupplierContactPerson);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("Currencies", item.Currencies == null ? (object)DBNull.Value : item.Currencies);
				sqlCommand.Parameters.AddWithValue("CustomerAddress", item.CustomerAddress == null ? (object)DBNull.Value : item.CustomerAddress);
				sqlCommand.Parameters.AddWithValue("CustomerCommunication", item.CustomerCommunication == null ? (object)DBNull.Value : item.CustomerCommunication);
				sqlCommand.Parameters.AddWithValue("CustomerContactPerson", item.CustomerContactPerson == null ? (object)DBNull.Value : item.CustomerContactPerson);
				sqlCommand.Parameters.AddWithValue("CustomerData", item.CustomerData == null ? (object)DBNull.Value : item.CustomerData);
				sqlCommand.Parameters.AddWithValue("CustomerGroup", item.CustomerGroup == null ? (object)DBNull.Value : item.CustomerGroup);
				sqlCommand.Parameters.AddWithValue("CustomerHistory", item.CustomerHistory == null ? (object)DBNull.Value : item.CustomerHistory);
				sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
				sqlCommand.Parameters.AddWithValue("CustomerOverview", item.CustomerOverview == null ? (object)DBNull.Value : item.CustomerOverview);
				sqlCommand.Parameters.AddWithValue("Customers", item.Customers);
				sqlCommand.Parameters.AddWithValue("CustomerShipping", item.CustomerShipping == null ? (object)DBNull.Value : item.CustomerShipping);
				sqlCommand.Parameters.AddWithValue("DeleteAltPositionsArticleBOM", item.DeleteAltPositionsArticleBOM);
				sqlCommand.Parameters.AddWithValue("DeleteArticle", item.DeleteArticle == null ? (object)DBNull.Value : item.DeleteArticle);
				sqlCommand.Parameters.AddWithValue("DeleteArticleBOM", item.DeleteArticleBOM);
				sqlCommand.Parameters.AddWithValue("DeleteArticlePurchase", item.DeleteArticlePurchase);
				sqlCommand.Parameters.AddWithValue("DeleteArticleSalesCustom", item.DeleteArticleSalesCustom);
				sqlCommand.Parameters.AddWithValue("DeleteArticleSalesItem", item.DeleteArticleSalesItem);
				sqlCommand.Parameters.AddWithValue("DeleteBCR", item.DeleteBCR == null ? (object)DBNull.Value : item.DeleteBCR);
				sqlCommand.Parameters.AddWithValue("DeleteCustomerContactPerson", item.DeleteCustomerContactPerson == null ? (object)DBNull.Value : item.DeleteCustomerContactPerson);
				sqlCommand.Parameters.AddWithValue("DeleteFiles", item.DeleteFiles == null ? (object)DBNull.Value : item.DeleteFiles);
				sqlCommand.Parameters.AddWithValue("DeleteRohArtikelNummer", item.DeleteRohArtikelNummer == null ? (object)DBNull.Value : item.DeleteRohArtikelNummer);
				sqlCommand.Parameters.AddWithValue("DeleteSupplierContactPerson", item.DeleteSupplierContactPerson == null ? (object)DBNull.Value : item.DeleteSupplierContactPerson);
				sqlCommand.Parameters.AddWithValue("DiscountGroup", item.DiscountGroup == null ? (object)DBNull.Value : item.DiscountGroup);
				sqlCommand.Parameters.AddWithValue("DownloadAllOutdatedEinkaufsPreis", item.DownloadAllOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadAllOutdatedEinkaufsPreis);
				sqlCommand.Parameters.AddWithValue("DownloadFiles", item.DownloadFiles == null ? (object)DBNull.Value : item.DownloadFiles);
				sqlCommand.Parameters.AddWithValue("DownloadOutdatedEinkaufsPreis", item.DownloadOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadOutdatedEinkaufsPreis);
				sqlCommand.Parameters.AddWithValue("EdiConcern", item.EdiConcern == null ? (object)DBNull.Value : item.EdiConcern);
				sqlCommand.Parameters.AddWithValue("EditAltPositionsArticleBOM", item.EditAltPositionsArticleBOM);
				sqlCommand.Parameters.AddWithValue("EditArticle", item.EditArticle);
				sqlCommand.Parameters.AddWithValue("EditArticleBOM", item.EditArticleBOM);
				sqlCommand.Parameters.AddWithValue("EditArticleBOMPosition", item.EditArticleBOMPosition);
				sqlCommand.Parameters.AddWithValue("EditArticleCts", item.EditArticleCts);
				sqlCommand.Parameters.AddWithValue("EditArticleData", item.EditArticleData);
				sqlCommand.Parameters.AddWithValue("EditArticleDesignation", item.EditArticleDesignation);
				sqlCommand.Parameters.AddWithValue("EditArticleImage", item.EditArticleImage);
				sqlCommand.Parameters.AddWithValue("EditArticleLogistics", item.EditArticleLogistics);
				sqlCommand.Parameters.AddWithValue("EditArticleManager", item.EditArticleManager);
				sqlCommand.Parameters.AddWithValue("EditArticleProduction", item.EditArticleProduction);
				sqlCommand.Parameters.AddWithValue("EditArticlePurchase", item.EditArticlePurchase);
				sqlCommand.Parameters.AddWithValue("EditArticleQuality", item.EditArticleQuality);
				sqlCommand.Parameters.AddWithValue("EditArticleReference", item.EditArticleReference == null ? (object)DBNull.Value : item.EditArticleReference);
				sqlCommand.Parameters.AddWithValue("EditArticleSalesCustom", item.EditArticleSalesCustom);
				sqlCommand.Parameters.AddWithValue("EditArticleSalesItem", item.EditArticleSalesItem);
				sqlCommand.Parameters.AddWithValue("EditCocType", item.EditCocType == null ? (object)DBNull.Value : item.EditCocType);
				sqlCommand.Parameters.AddWithValue("EditConditionAssignment", item.EditConditionAssignment == null ? (object)DBNull.Value : item.EditConditionAssignment);
				sqlCommand.Parameters.AddWithValue("EditContactAddress", item.EditContactAddress == null ? (object)DBNull.Value : item.EditContactAddress);
				sqlCommand.Parameters.AddWithValue("EditContactSalutation", item.EditContactSalutation == null ? (object)DBNull.Value : item.EditContactSalutation);
				sqlCommand.Parameters.AddWithValue("EditCurrencies", item.EditCurrencies == null ? (object)DBNull.Value : item.EditCurrencies);
				sqlCommand.Parameters.AddWithValue("EditCustomer", item.EditCustomer == null ? (object)DBNull.Value : item.EditCustomer);
				sqlCommand.Parameters.AddWithValue("EditCustomerAddress", item.EditCustomerAddress == null ? (object)DBNull.Value : item.EditCustomerAddress);
				sqlCommand.Parameters.AddWithValue("EditCustomerCommunication", item.EditCustomerCommunication == null ? (object)DBNull.Value : item.EditCustomerCommunication);
				sqlCommand.Parameters.AddWithValue("EditCustomerContactPerson", item.EditCustomerContactPerson == null ? (object)DBNull.Value : item.EditCustomerContactPerson);
				sqlCommand.Parameters.AddWithValue("EditCustomerCoordination", item.EditCustomerCoordination == null ? (object)DBNull.Value : item.EditCustomerCoordination);
				sqlCommand.Parameters.AddWithValue("EditCustomerData", item.EditCustomerData == null ? (object)DBNull.Value : item.EditCustomerData);
				sqlCommand.Parameters.AddWithValue("EditCustomerGroup", item.EditCustomerGroup == null ? (object)DBNull.Value : item.EditCustomerGroup);
				sqlCommand.Parameters.AddWithValue("EditCustomerImage", item.EditCustomerImage == null ? (object)DBNull.Value : item.EditCustomerImage);
				sqlCommand.Parameters.AddWithValue("EditCustomerItemNumber", item.EditCustomerItemNumber == null ? (object)DBNull.Value : item.EditCustomerItemNumber);
				sqlCommand.Parameters.AddWithValue("EditCustomerShipping", item.EditCustomerShipping == null ? (object)DBNull.Value : item.EditCustomerShipping);
				sqlCommand.Parameters.AddWithValue("EditDiscountGroup", item.EditDiscountGroup == null ? (object)DBNull.Value : item.EditDiscountGroup);
				sqlCommand.Parameters.AddWithValue("EditEdiConcern", item.EditEdiConcern == null ? (object)DBNull.Value : item.EditEdiConcern);
				sqlCommand.Parameters.AddWithValue("EditFibuFrame", item.EditFibuFrame == null ? (object)DBNull.Value : item.EditFibuFrame);
				sqlCommand.Parameters.AddWithValue("EditHourlyRate", item.EditHourlyRate == null ? (object)DBNull.Value : item.EditHourlyRate);
				sqlCommand.Parameters.AddWithValue("EditIndustry", item.EditIndustry == null ? (object)DBNull.Value : item.EditIndustry);
				sqlCommand.Parameters.AddWithValue("EditLagerCCID", item.EditLagerCCID == null ? (object)DBNull.Value : item.EditLagerCCID);
				sqlCommand.Parameters.AddWithValue("EditLagerMinStock", item.EditLagerMinStock);
				sqlCommand.Parameters.AddWithValue("EditLagerOrderProposal", item.EditLagerOrderProposal == null ? (object)DBNull.Value : item.EditLagerOrderProposal);
				sqlCommand.Parameters.AddWithValue("EditLagerStock", item.EditLagerStock == null ? (object)DBNull.Value : item.EditLagerStock);
				sqlCommand.Parameters.AddWithValue("EditPayementPractises", item.EditPayementPractises == null ? (object)DBNull.Value : item.EditPayementPractises);
				sqlCommand.Parameters.AddWithValue("EditPricingGroup", item.EditPricingGroup == null ? (object)DBNull.Value : item.EditPricingGroup);
				sqlCommand.Parameters.AddWithValue("EditRohArtikelNummer", item.EditRohArtikelNummer == null ? (object)DBNull.Value : item.EditRohArtikelNummer);
				sqlCommand.Parameters.AddWithValue("EditShippingMethods", item.EditShippingMethods == null ? (object)DBNull.Value : item.EditShippingMethods);
				sqlCommand.Parameters.AddWithValue("EditSlipCircle", item.EditSlipCircle == null ? (object)DBNull.Value : item.EditSlipCircle);
				sqlCommand.Parameters.AddWithValue("EditSupplier", item.EditSupplier == null ? (object)DBNull.Value : item.EditSupplier);
				sqlCommand.Parameters.AddWithValue("EditSupplierAddress", item.EditSupplierAddress == null ? (object)DBNull.Value : item.EditSupplierAddress);
				sqlCommand.Parameters.AddWithValue("EditSupplierCommunication", item.EditSupplierCommunication == null ? (object)DBNull.Value : item.EditSupplierCommunication);
				sqlCommand.Parameters.AddWithValue("EditSupplierContactPerson", item.EditSupplierContactPerson == null ? (object)DBNull.Value : item.EditSupplierContactPerson);
				sqlCommand.Parameters.AddWithValue("EditSupplierCoordination", item.EditSupplierCoordination == null ? (object)DBNull.Value : item.EditSupplierCoordination);
				sqlCommand.Parameters.AddWithValue("EditSupplierData", item.EditSupplierData == null ? (object)DBNull.Value : item.EditSupplierData);
				sqlCommand.Parameters.AddWithValue("EditSupplierGroup", item.EditSupplierGroup == null ? (object)DBNull.Value : item.EditSupplierGroup);
				sqlCommand.Parameters.AddWithValue("EditSupplierImage", item.EditSupplierImage == null ? (object)DBNull.Value : item.EditSupplierImage);
				sqlCommand.Parameters.AddWithValue("EditSupplierShipping", item.EditSupplierShipping == null ? (object)DBNull.Value : item.EditSupplierShipping);
				sqlCommand.Parameters.AddWithValue("EditTermsOfPayment", item.EditTermsOfPayment == null ? (object)DBNull.Value : item.EditTermsOfPayment);
				sqlCommand.Parameters.AddWithValue("EDrawingEdit", item.EDrawingEdit == null ? (object)DBNull.Value : item.EDrawingEdit);
				sqlCommand.Parameters.AddWithValue("EinkaufsPreisUpdate", item.EinkaufsPreisUpdate == null ? (object)DBNull.Value : item.EinkaufsPreisUpdate);
				sqlCommand.Parameters.AddWithValue("FibuFrame", item.FibuFrame == null ? (object)DBNull.Value : item.FibuFrame);
				sqlCommand.Parameters.AddWithValue("GetRohArtikelNummer", item.GetRohArtikelNummer == null ? (object)DBNull.Value : item.GetRohArtikelNummer);
				sqlCommand.Parameters.AddWithValue("HourlyRate", item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
				sqlCommand.Parameters.AddWithValue("ImportArticleBOM", item.ImportArticleBOM);
				sqlCommand.Parameters.AddWithValue("Industry", item.Industry == null ? (object)DBNull.Value : item.Industry);
				sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
				sqlCommand.Parameters.AddWithValue("LagerArticleLogistics", item.LagerArticleLogistics);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("ModuleAdministrator", item.ModuleAdministrator == null ? (object)DBNull.Value : item.ModuleAdministrator);
				sqlCommand.Parameters.AddWithValue("offer", item.offer == null ? (object)DBNull.Value : item.offer);
				sqlCommand.Parameters.AddWithValue("OfferRequestADD", item.OfferRequestADD == null ? (object)DBNull.Value : item.OfferRequestADD);
				sqlCommand.Parameters.AddWithValue("OfferRequestApplyPrice", item.OfferRequestApplyPrice == null ? (object)DBNull.Value : item.OfferRequestApplyPrice);
				sqlCommand.Parameters.AddWithValue("OfferRequestDelete", item.OfferRequestDelete == null ? (object)DBNull.Value : item.OfferRequestDelete);
				sqlCommand.Parameters.AddWithValue("OfferRequestEdit", item.OfferRequestEdit == null ? (object)DBNull.Value : item.OfferRequestEdit);
				sqlCommand.Parameters.AddWithValue("OfferRequestEditEmail", item.OfferRequestEditEmail == null ? (object)DBNull.Value : item.OfferRequestEditEmail);
				sqlCommand.Parameters.AddWithValue("OfferRequestSendEmail", item.OfferRequestSendEmail == null ? (object)DBNull.Value : item.OfferRequestSendEmail);
				sqlCommand.Parameters.AddWithValue("OfferRequestView", item.OfferRequestView == null ? (object)DBNull.Value : item.OfferRequestView);
				sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoAdd", item.PackagingsLgtPhotoAdd == null ? (object)DBNull.Value : item.PackagingsLgtPhotoAdd);
				sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoDelete", item.PackagingsLgtPhotoDelete == null ? (object)DBNull.Value : item.PackagingsLgtPhotoDelete);
				sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoView", item.PackagingsLgtPhotoView == null ? (object)DBNull.Value : item.PackagingsLgtPhotoView);
				sqlCommand.Parameters.AddWithValue("PayementPractises", item.PayementPractises == null ? (object)DBNull.Value : item.PayementPractises);
				sqlCommand.Parameters.AddWithValue("PMAddCable", item.PMAddCable == null ? (object)DBNull.Value : item.PMAddCable);
				sqlCommand.Parameters.AddWithValue("PMAddMileStone", item.PMAddMileStone == null ? (object)DBNull.Value : item.PMAddMileStone);
				sqlCommand.Parameters.AddWithValue("PMAddProject", item.PMAddProject == null ? (object)DBNull.Value : item.PMAddProject);
				sqlCommand.Parameters.AddWithValue("PMDeleteCable", item.PMDeleteCable == null ? (object)DBNull.Value : item.PMDeleteCable);
				sqlCommand.Parameters.AddWithValue("PMDeleteMileStone", item.PMDeleteMileStone == null ? (object)DBNull.Value : item.PMDeleteMileStone);
				sqlCommand.Parameters.AddWithValue("PMDeleteProject", item.PMDeleteProject == null ? (object)DBNull.Value : item.PMDeleteProject);
				sqlCommand.Parameters.AddWithValue("PMEditCable", item.PMEditCable == null ? (object)DBNull.Value : item.PMEditCable);
				sqlCommand.Parameters.AddWithValue("PMEditMileStone", item.PMEditMileStone == null ? (object)DBNull.Value : item.PMEditMileStone);
				sqlCommand.Parameters.AddWithValue("PMEditProject", item.PMEditProject == null ? (object)DBNull.Value : item.PMEditProject);
				sqlCommand.Parameters.AddWithValue("PMModule", item.PMModule == null ? (object)DBNull.Value : item.PMModule);
				sqlCommand.Parameters.AddWithValue("PMViewProjectsCompact", item.PMViewProjectsCompact == null ? (object)DBNull.Value : item.PMViewProjectsCompact);
				sqlCommand.Parameters.AddWithValue("PMViewProjectsDetail", item.PMViewProjectsDetail == null ? (object)DBNull.Value : item.PMViewProjectsDetail);
				sqlCommand.Parameters.AddWithValue("PMViewProjectsMedium", item.PMViewProjectsMedium == null ? (object)DBNull.Value : item.PMViewProjectsMedium);
				sqlCommand.Parameters.AddWithValue("PricingGroup", item.PricingGroup == null ? (object)DBNull.Value : item.PricingGroup);
				sqlCommand.Parameters.AddWithValue("RemoveArticleReference", item.RemoveArticleReference == null ? (object)DBNull.Value : item.RemoveArticleReference);
				sqlCommand.Parameters.AddWithValue("Settings", item.Settings == null ? (object)DBNull.Value : item.Settings);
				sqlCommand.Parameters.AddWithValue("ShippingMethods", item.ShippingMethods == null ? (object)DBNull.Value : item.ShippingMethods);
				sqlCommand.Parameters.AddWithValue("SlipCircle", item.SlipCircle == null ? (object)DBNull.Value : item.SlipCircle);
				sqlCommand.Parameters.AddWithValue("SupplierAddress", item.SupplierAddress == null ? (object)DBNull.Value : item.SupplierAddress);
				sqlCommand.Parameters.AddWithValue("SupplierAttachementAddFile", item.SupplierAttachementAddFile == null ? (object)DBNull.Value : item.SupplierAttachementAddFile);
				sqlCommand.Parameters.AddWithValue("SupplierAttachementGetFile", item.SupplierAttachementGetFile == null ? (object)DBNull.Value : item.SupplierAttachementGetFile);
				sqlCommand.Parameters.AddWithValue("SupplierAttachementRemoveFile", item.SupplierAttachementRemoveFile == null ? (object)DBNull.Value : item.SupplierAttachementRemoveFile);
				sqlCommand.Parameters.AddWithValue("SupplierCommunication", item.SupplierCommunication == null ? (object)DBNull.Value : item.SupplierCommunication);
				sqlCommand.Parameters.AddWithValue("SupplierContactPerson", item.SupplierContactPerson == null ? (object)DBNull.Value : item.SupplierContactPerson);
				sqlCommand.Parameters.AddWithValue("SupplierData", item.SupplierData == null ? (object)DBNull.Value : item.SupplierData);
				sqlCommand.Parameters.AddWithValue("SupplierGroup", item.SupplierGroup == null ? (object)DBNull.Value : item.SupplierGroup);
				sqlCommand.Parameters.AddWithValue("SupplierHistory", item.SupplierHistory == null ? (object)DBNull.Value : item.SupplierHistory);
				sqlCommand.Parameters.AddWithValue("SupplierOverview", item.SupplierOverview == null ? (object)DBNull.Value : item.SupplierOverview);
				sqlCommand.Parameters.AddWithValue("Suppliers", item.Suppliers);
				sqlCommand.Parameters.AddWithValue("SupplierShipping", item.SupplierShipping == null ? (object)DBNull.Value : item.SupplierShipping);
				sqlCommand.Parameters.AddWithValue("TermsOfPayment", item.TermsOfPayment == null ? (object)DBNull.Value : item.TermsOfPayment);
				sqlCommand.Parameters.AddWithValue("UploadAltPositionsArticleBOM", item.UploadAltPositionsArticleBOM);
				sqlCommand.Parameters.AddWithValue("UploadArticleBOM", item.UploadArticleBOM);
				sqlCommand.Parameters.AddWithValue("ValidateArticleBOM", item.ValidateArticleBOM == null ? (object)DBNull.Value : item.ValidateArticleBOM);
				sqlCommand.Parameters.AddWithValue("ValidateBCR", item.ValidateBCR == null ? (object)DBNull.Value : item.ValidateBCR);
				sqlCommand.Parameters.AddWithValue("ViewAltPositionsArticleBOM", item.ViewAltPositionsArticleBOM);
				sqlCommand.Parameters.AddWithValue("ViewArticleLog", item.ViewArticleLog);
				sqlCommand.Parameters.AddWithValue("ViewArticleReference", item.ViewArticleReference == null ? (object)DBNull.Value : item.ViewArticleReference);
				sqlCommand.Parameters.AddWithValue("ViewArticles", item.ViewArticles);
				sqlCommand.Parameters.AddWithValue("ViewBCR", item.ViewBCR == null ? (object)DBNull.Value : item.ViewBCR);
				sqlCommand.Parameters.AddWithValue("ViewCustomers", item.ViewCustomers == null ? (object)DBNull.Value : item.ViewCustomers);
				sqlCommand.Parameters.AddWithValue("ViewLPCustomer", item.ViewLPCustomer == null ? (object)DBNull.Value : item.ViewLPCustomer);
				sqlCommand.Parameters.AddWithValue("ViewLPSupplier", item.ViewLPSupplier == null ? (object)DBNull.Value : item.ViewLPSupplier);
				sqlCommand.Parameters.AddWithValue("ViewSupplierAddressComments", item.ViewSupplierAddressComments == null ? (object)DBNull.Value : item.ViewSupplierAddressComments);
				sqlCommand.Parameters.AddWithValue("ViewSuppliers", item.ViewSuppliers == null ? (object)DBNull.Value : item.ViewSuppliers);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Entities.Tables.BSD.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 226; // Nb params per query
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
		private static int update(List<Entities.Tables.BSD.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__BSD_AccessProfile] SET "

							+ "[AccessProfileName]=@AccessProfileName" + i + ","
							+ "[AddArticle]=@AddArticle" + i + ","
							+ "[AddArticleReference]=@AddArticleReference" + i + ","
							+ "[AddBCR]=@AddBCR" + i + ","
							+ "[AddCocType]=@AddCocType" + i + ","
							+ "[AddConditionAssignment]=@AddConditionAssignment" + i + ","
							+ "[AddContactAddress]=@AddContactAddress" + i + ","
							+ "[AddContactSalutation]=@AddContactSalutation" + i + ","
							+ "[AddCurrencies]=@AddCurrencies" + i + ","
							+ "[AddCustomer]=@AddCustomer" + i + ","
							+ "[AddCustomerGroup]=@AddCustomerGroup" + i + ","
							+ "[AddCustomerItemNumber]=@AddCustomerItemNumber" + i + ","
							+ "[AddDiscountGroup]=@AddDiscountGroup" + i + ","
							+ "[AddEdiConcern]=@AddEdiConcern" + i + ","
							+ "[AddFibuFrame]=@AddFibuFrame" + i + ","
							+ "[AddFiles]=@AddFiles" + i + ","
							+ "[AddHourlyRate]=@AddHourlyRate" + i + ","
							+ "[AddIndustry]=@AddIndustry" + i + ","
							+ "[AddPayementPractises]=@AddPayementPractises" + i + ","
							+ "[AddPricingGroup]=@AddPricingGroup" + i + ","
							+ "[AddRohArtikelNummer]=@AddRohArtikelNummer" + i + ","
							+ "[AddShippingMethods]=@AddShippingMethods" + i + ","
							+ "[AddSlipCircle]=@AddSlipCircle" + i + ","
							+ "[AddSupplier]=@AddSupplier" + i + ","
							+ "[AddSupplierGroup]=@AddSupplierGroup" + i + ","
							+ "[AddTermsOfPayment]=@AddTermsOfPayment" + i + ","
							+ "[Administration]=@Administration" + i + ","
							+ "[AltPositionsArticleBOM]=@AltPositionsArticleBOM" + i + ","
							+ "[ArchiveArticle]=@ArchiveArticle" + i + ","
							+ "[ArchiveCustomer]=@ArchiveCustomer" + i + ","
							+ "[ArchiveSupplier]=@ArchiveSupplier" + i + ","
							+ "[ArticleAddCustomerDocument]=@ArticleAddCustomerDocument" + i + ","
							+ "[ArticleBOM]=@ArticleBOM" + i + ","
							+ "[ArticleCts]=@ArticleCts" + i + ","
							+ "[ArticleData]=@ArticleData" + i + ","
							+ "[ArticleDeleteCustomerDocument]=@ArticleDeleteCustomerDocument" + i + ","
							+ "[ArticleLogistics]=@ArticleLogistics" + i + ","
							+ "[ArticleLogisticsPrices]=@ArticleLogisticsPrices" + i + ","
							+ "[ArticleOverview]=@ArticleOverview" + i + ","
							+ "[ArticleProduction]=@ArticleProduction" + i + ","
							+ "[ArticlePurchase]=@ArticlePurchase" + i + ","
							+ "[ArticleQuality]=@ArticleQuality" + i + ","
							+ "[Articles]=@Articles" + i + ","
							+ "[ArticleSales]=@ArticleSales" + i + ","
							+ "[ArticleSalesCustom]=@ArticleSalesCustom" + i + ","
							+ "[ArticleSalesItem]=@ArticleSalesItem" + i + ","
							+ "[ArticlesBOMCPControlEngineering]=@ArticlesBOMCPControlEngineering" + i + ","
							+ "[ArticlesBOMCPControlHistory]=@ArticlesBOMCPControlHistory" + i + ","
							+ "[ArticlesBOMCPControlQuality]=@ArticlesBOMCPControlQuality" + i + ","
							+ "[ArticleStatistics]=@ArticleStatistics" + i + ","
							+ "[ArticleStatisticsEngineering]=@ArticleStatisticsEngineering" + i + ","
							+ "[ArticleStatisticsEngineeringEdit]=@ArticleStatisticsEngineeringEdit" + i + ","
							+ "[ArticleStatisticsFinanceAccounting]=@ArticleStatisticsFinanceAccounting" + i + ","
							+ "[ArticleStatisticsFinanceAccountingEdit]=@ArticleStatisticsFinanceAccountingEdit" + i + ","
							+ "[ArticleStatisticsLogistics]=@ArticleStatisticsLogistics" + i + ","
							+ "[ArticleStatisticsLogisticsEdit]=@ArticleStatisticsLogisticsEdit" + i + ","
							+ "[ArticleStatisticsPurchase]=@ArticleStatisticsPurchase" + i + ","
							+ "[ArticleStatisticsPurchaseEdit]=@ArticleStatisticsPurchaseEdit" + i + ","
							+ "[ArticleStatisticsTechnic]=@ArticleStatisticsTechnic" + i + ","
							+ "[ArticleStatisticsTechnicEdit]=@ArticleStatisticsTechnicEdit" + i + ","
							+ "[BomChangeRequests]=@BomChangeRequests" + i + ","
							+ "[CocType]=@CocType" + i + ","
							+ "[ConditionAssignment]=@ConditionAssignment" + i + ","
							+ "[ConfigArticle]=@ConfigArticle" + i + ","
							+ "[ConfigCustomer]=@ConfigCustomer" + i + ","
							+ "[ConfigSupplier]=@ConfigSupplier" + i + ","
							+ "[ContactAddress]=@ContactAddress" + i + ","
							+ "[ContactSalutation]=@ContactSalutation" + i + ","
							+ "[CreateArticlePurchase]=@CreateArticlePurchase" + i + ","
							+ "[CreateArticleSalesCustom]=@CreateArticleSalesCustom" + i + ","
							+ "[CreateArticleSalesItem]=@CreateArticleSalesItem" + i + ","
							+ "[CreateCustomerContactPerson]=@CreateCustomerContactPerson" + i + ","
							+ "[CreateSupplierContactPerson]=@CreateSupplierContactPerson" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[Currencies]=@Currencies" + i + ","
							+ "[CustomerAddress]=@CustomerAddress" + i + ","
							+ "[CustomerCommunication]=@CustomerCommunication" + i + ","
							+ "[CustomerContactPerson]=@CustomerContactPerson" + i + ","
							+ "[CustomerData]=@CustomerData" + i + ","
							+ "[CustomerGroup]=@CustomerGroup" + i + ","
							+ "[CustomerHistory]=@CustomerHistory" + i + ","
							+ "[CustomerItemNumber]=@CustomerItemNumber" + i + ","
							+ "[CustomerOverview]=@CustomerOverview" + i + ","
							+ "[Customers]=@Customers" + i + ","
							+ "[CustomerShipping]=@CustomerShipping" + i + ","
							+ "[DeleteAltPositionsArticleBOM]=@DeleteAltPositionsArticleBOM" + i + ","
							+ "[DeleteArticle]=@DeleteArticle" + i + ","
							+ "[DeleteArticleBOM]=@DeleteArticleBOM" + i + ","
							+ "[DeleteArticlePurchase]=@DeleteArticlePurchase" + i + ","
							+ "[DeleteArticleSalesCustom]=@DeleteArticleSalesCustom" + i + ","
							+ "[DeleteArticleSalesItem]=@DeleteArticleSalesItem" + i + ","
							+ "[DeleteBCR]=@DeleteBCR" + i + ","
							+ "[DeleteCustomerContactPerson]=@DeleteCustomerContactPerson" + i + ","
							+ "[DeleteFiles]=@DeleteFiles" + i + ","
							+ "[DeleteRohArtikelNummer]=@DeleteRohArtikelNummer" + i + ","
							+ "[DeleteSupplierContactPerson]=@DeleteSupplierContactPerson" + i + ","
							+ "[DiscountGroup]=@DiscountGroup" + i + ","
							+ "[DownloadAllOutdatedEinkaufsPreis]=@DownloadAllOutdatedEinkaufsPreis" + i + ","
							+ "[DownloadFiles]=@DownloadFiles" + i + ","
							+ "[DownloadOutdatedEinkaufsPreis]=@DownloadOutdatedEinkaufsPreis" + i + ","
							+ "[EdiConcern]=@EdiConcern" + i + ","
							+ "[EditAltPositionsArticleBOM]=@EditAltPositionsArticleBOM" + i + ","
							+ "[EditArticle]=@EditArticle" + i + ","
							+ "[EditArticleBOM]=@EditArticleBOM" + i + ","
							+ "[EditArticleBOMPosition]=@EditArticleBOMPosition" + i + ","
							+ "[EditArticleCts]=@EditArticleCts" + i + ","
							+ "[EditArticleData]=@EditArticleData" + i + ","
							+ "[EditArticleDesignation]=@EditArticleDesignation" + i + ","
							+ "[EditArticleImage]=@EditArticleImage" + i + ","
							+ "[EditArticleLogistics]=@EditArticleLogistics" + i + ","
							+ "[EditArticleManager]=@EditArticleManager" + i + ","
							+ "[EditArticleProduction]=@EditArticleProduction" + i + ","
							+ "[EditArticlePurchase]=@EditArticlePurchase" + i + ","
							+ "[EditArticleQuality]=@EditArticleQuality" + i + ","
							+ "[EditArticleReference]=@EditArticleReference" + i + ","
							+ "[EditArticleSalesCustom]=@EditArticleSalesCustom" + i + ","
							+ "[EditArticleSalesItem]=@EditArticleSalesItem" + i + ","
							+ "[EditCocType]=@EditCocType" + i + ","
							+ "[EditConditionAssignment]=@EditConditionAssignment" + i + ","
							+ "[EditContactAddress]=@EditContactAddress" + i + ","
							+ "[EditContactSalutation]=@EditContactSalutation" + i + ","
							+ "[EditCurrencies]=@EditCurrencies" + i + ","
							+ "[EditCustomer]=@EditCustomer" + i + ","
							+ "[EditCustomerAddress]=@EditCustomerAddress" + i + ","
							+ "[EditCustomerCommunication]=@EditCustomerCommunication" + i + ","
							+ "[EditCustomerContactPerson]=@EditCustomerContactPerson" + i + ","
							+ "[EditCustomerCoordination]=@EditCustomerCoordination" + i + ","
							+ "[EditCustomerData]=@EditCustomerData" + i + ","
							+ "[EditCustomerGroup]=@EditCustomerGroup" + i + ","
							+ "[EditCustomerImage]=@EditCustomerImage" + i + ","
							+ "[EditCustomerItemNumber]=@EditCustomerItemNumber" + i + ","
							+ "[EditCustomerShipping]=@EditCustomerShipping" + i + ","
							+ "[EditDiscountGroup]=@EditDiscountGroup" + i + ","
							+ "[EditEdiConcern]=@EditEdiConcern" + i + ","
							+ "[EditFibuFrame]=@EditFibuFrame" + i + ","
							+ "[EditHourlyRate]=@EditHourlyRate" + i + ","
							+ "[EditIndustry]=@EditIndustry" + i + ","
							+ "[EditLagerCCID]=@EditLagerCCID" + i + ","
							+ "[EditLagerMinStock]=@EditLagerMinStock" + i + ","
							+ "[EditLagerOrderProposal]=@EditLagerOrderProposal" + i + ","
							+ "[EditLagerStock]=@EditLagerStock" + i + ","
							+ "[EditPayementPractises]=@EditPayementPractises" + i + ","
							+ "[EditPricingGroup]=@EditPricingGroup" + i + ","
							+ "[EditRohArtikelNummer]=@EditRohArtikelNummer" + i + ","
							+ "[EditShippingMethods]=@EditShippingMethods" + i + ","
							+ "[EditSlipCircle]=@EditSlipCircle" + i + ","
							+ "[EditSupplier]=@EditSupplier" + i + ","
							+ "[EditSupplierAddress]=@EditSupplierAddress" + i + ","
							+ "[EditSupplierCommunication]=@EditSupplierCommunication" + i + ","
							+ "[EditSupplierContactPerson]=@EditSupplierContactPerson" + i + ","
							+ "[EditSupplierCoordination]=@EditSupplierCoordination" + i + ","
							+ "[EditSupplierData]=@EditSupplierData" + i + ","
							+ "[EditSupplierGroup]=@EditSupplierGroup" + i + ","
							+ "[EditSupplierImage]=@EditSupplierImage" + i + ","
							+ "[EditSupplierShipping]=@EditSupplierShipping" + i + ","
							+ "[EditTermsOfPayment]=@EditTermsOfPayment" + i + ","
							+ "[EDrawingEdit]=@EDrawingEdit" + i + ","
							+ "[EinkaufsPreisUpdate]=@EinkaufsPreisUpdate" + i + ","
							+ "[FibuFrame]=@FibuFrame" + i + ","
							+ "[GetRohArtikelNummer]=@GetRohArtikelNummer" + i + ","
							+ "[HourlyRate]=@HourlyRate" + i + ","
							+ "[ImportArticleBOM]=@ImportArticleBOM" + i + ","
							+ "[Industry]=@Industry" + i + ","
							+ "[isDefault]=@isDefault" + i + ","
							+ "[LagerArticleLogistics]=@LagerArticleLogistics" + i + ","
							+ "[ModuleActivated]=@ModuleActivated" + i + ","
							+ "[ModuleAdministrator]=@ModuleAdministrator" + i + ","
							+ "[offer]=@offer" + i + ","
							+ "[OfferRequestADD]=@OfferRequestADD" + i + ","
							+ "[OfferRequestApplyPrice]=@OfferRequestApplyPrice" + i + ","
							+ "[OfferRequestDelete]=@OfferRequestDelete" + i + ","
							+ "[OfferRequestEdit]=@OfferRequestEdit" + i + ","
							+ "[OfferRequestEditEmail]=@OfferRequestEditEmail" + i + ","
							+ "[OfferRequestSendEmail]=@OfferRequestSendEmail" + i + ","
							+ "[OfferRequestView]=@OfferRequestView" + i + ","
							+ "[PackagingsLgtPhotoAdd]=@PackagingsLgtPhotoAdd" + i + ","
							+ "[PackagingsLgtPhotoDelete]=@PackagingsLgtPhotoDelete" + i + ","
							+ "[PackagingsLgtPhotoView]=@PackagingsLgtPhotoView" + i + ","
							+ "[PayementPractises]=@PayementPractises" + i + ","
							+ "[PMAddCable]=@PMAddCable" + i + ","
							+ "[PMAddMileStone]=@PMAddMileStone" + i + ","
							+ "[PMAddProject]=@PMAddProject" + i + ","
							+ "[PMDeleteCable]=@PMDeleteCable" + i + ","
							+ "[PMDeleteMileStone]=@PMDeleteMileStone" + i + ","
							+ "[PMDeleteProject]=@PMDeleteProject" + i + ","
							+ "[PMEditCable]=@PMEditCable" + i + ","
							+ "[PMEditMileStone]=@PMEditMileStone" + i + ","
							+ "[PMEditProject]=@PMEditProject" + i + ","
							+ "[PMModule]=@PMModule" + i + ","
							+ "[PMViewProjectsCompact]=@PMViewProjectsCompact" + i + ","
							+ "[PMViewProjectsDetail]=@PMViewProjectsDetail" + i + ","
							+ "[PMViewProjectsMedium]=@PMViewProjectsMedium" + i + ","
							+ "[PricingGroup]=@PricingGroup" + i + ","
							+ "[RemoveArticleReference]=@RemoveArticleReference" + i + ","
							+ "[Settings]=@Settings" + i + ","
							+ "[ShippingMethods]=@ShippingMethods" + i + ","
							+ "[SlipCircle]=@SlipCircle" + i + ","
							+ "[SupplierAddress]=@SupplierAddress" + i + ","
							+ "[SupplierAttachementAddFile]=@SupplierAttachementAddFile" + i + ","
							+ "[SupplierAttachementGetFile]=@SupplierAttachementGetFile" + i + ","
							+ "[SupplierAttachementRemoveFile]=@SupplierAttachementRemoveFile" + i + ","
							+ "[SupplierCommunication]=@SupplierCommunication" + i + ","
							+ "[SupplierContactPerson]=@SupplierContactPerson" + i + ","
							+ "[SupplierData]=@SupplierData" + i + ","
							+ "[SupplierGroup]=@SupplierGroup" + i + ","
							+ "[SupplierHistory]=@SupplierHistory" + i + ","
							+ "[SupplierOverview]=@SupplierOverview" + i + ","
							+ "[Suppliers]=@Suppliers" + i + ","
							+ "[SupplierShipping]=@SupplierShipping" + i + ","
							+ "[TermsOfPayment]=@TermsOfPayment" + i + ","
							+ "[UploadAltPositionsArticleBOM]=@UploadAltPositionsArticleBOM" + i + ","
							+ "[UploadArticleBOM]=@UploadArticleBOM" + i + ","
							+ "[ValidateArticleBOM]=@ValidateArticleBOM" + i + ","
							+ "[ValidateBCR]=@ValidateBCR" + i + ","
							+ "[ViewAltPositionsArticleBOM]=@ViewAltPositionsArticleBOM" + i + ","
							+ "[ViewArticleLog]=@ViewArticleLog" + i + ","
							+ "[ViewArticleReference]=@ViewArticleReference" + i + ","
							+ "[ViewArticles]=@ViewArticles" + i + ","
							+ "[ViewBCR]=@ViewBCR" + i + ","
							+ "[ViewCustomers]=@ViewCustomers" + i + ","
							+ "[ViewLPCustomer]=@ViewLPCustomer" + i + ","
							+ "[ViewLPSupplier]=@ViewLPSupplier" + i + ","
							+ "[ViewSupplierAddressComments]=@ViewSupplierAddressComments" + i + ","
							+ "[ViewSuppliers]=@ViewSuppliers" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("AddArticle" + i, item.AddArticle);
						sqlCommand.Parameters.AddWithValue("AddArticleReference" + i, item.AddArticleReference == null ? (object)DBNull.Value : item.AddArticleReference);
						sqlCommand.Parameters.AddWithValue("AddBCR" + i, item.AddBCR == null ? (object)DBNull.Value : item.AddBCR);
						sqlCommand.Parameters.AddWithValue("AddCocType" + i, item.AddCocType == null ? (object)DBNull.Value : item.AddCocType);
						sqlCommand.Parameters.AddWithValue("AddConditionAssignment" + i, item.AddConditionAssignment == null ? (object)DBNull.Value : item.AddConditionAssignment);
						sqlCommand.Parameters.AddWithValue("AddContactAddress" + i, item.AddContactAddress == null ? (object)DBNull.Value : item.AddContactAddress);
						sqlCommand.Parameters.AddWithValue("AddContactSalutation" + i, item.AddContactSalutation == null ? (object)DBNull.Value : item.AddContactSalutation);
						sqlCommand.Parameters.AddWithValue("AddCurrencies" + i, item.AddCurrencies == null ? (object)DBNull.Value : item.AddCurrencies);
						sqlCommand.Parameters.AddWithValue("AddCustomer" + i, item.AddCustomer == null ? (object)DBNull.Value : item.AddCustomer);
						sqlCommand.Parameters.AddWithValue("AddCustomerGroup" + i, item.AddCustomerGroup == null ? (object)DBNull.Value : item.AddCustomerGroup);
						sqlCommand.Parameters.AddWithValue("AddCustomerItemNumber" + i, item.AddCustomerItemNumber == null ? (object)DBNull.Value : item.AddCustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("AddDiscountGroup" + i, item.AddDiscountGroup == null ? (object)DBNull.Value : item.AddDiscountGroup);
						sqlCommand.Parameters.AddWithValue("AddEdiConcern" + i, item.AddEdiConcern == null ? (object)DBNull.Value : item.AddEdiConcern);
						sqlCommand.Parameters.AddWithValue("AddFibuFrame" + i, item.AddFibuFrame == null ? (object)DBNull.Value : item.AddFibuFrame);
						sqlCommand.Parameters.AddWithValue("AddFiles" + i, item.AddFiles == null ? (object)DBNull.Value : item.AddFiles);
						sqlCommand.Parameters.AddWithValue("AddHourlyRate" + i, item.AddHourlyRate == null ? (object)DBNull.Value : item.AddHourlyRate);
						sqlCommand.Parameters.AddWithValue("AddIndustry" + i, item.AddIndustry == null ? (object)DBNull.Value : item.AddIndustry);
						sqlCommand.Parameters.AddWithValue("AddPayementPractises" + i, item.AddPayementPractises == null ? (object)DBNull.Value : item.AddPayementPractises);
						sqlCommand.Parameters.AddWithValue("AddPricingGroup" + i, item.AddPricingGroup == null ? (object)DBNull.Value : item.AddPricingGroup);
						sqlCommand.Parameters.AddWithValue("AddRohArtikelNummer" + i, item.AddRohArtikelNummer == null ? (object)DBNull.Value : item.AddRohArtikelNummer);
						sqlCommand.Parameters.AddWithValue("AddShippingMethods" + i, item.AddShippingMethods == null ? (object)DBNull.Value : item.AddShippingMethods);
						sqlCommand.Parameters.AddWithValue("AddSlipCircle" + i, item.AddSlipCircle == null ? (object)DBNull.Value : item.AddSlipCircle);
						sqlCommand.Parameters.AddWithValue("AddSupplier" + i, item.AddSupplier == null ? (object)DBNull.Value : item.AddSupplier);
						sqlCommand.Parameters.AddWithValue("AddSupplierGroup" + i, item.AddSupplierGroup == null ? (object)DBNull.Value : item.AddSupplierGroup);
						sqlCommand.Parameters.AddWithValue("AddTermsOfPayment" + i, item.AddTermsOfPayment == null ? (object)DBNull.Value : item.AddTermsOfPayment);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration);
						sqlCommand.Parameters.AddWithValue("AltPositionsArticleBOM" + i, item.AltPositionsArticleBOM);
						sqlCommand.Parameters.AddWithValue("ArchiveArticle" + i, item.ArchiveArticle);
						sqlCommand.Parameters.AddWithValue("ArchiveCustomer" + i, item.ArchiveCustomer == null ? (object)DBNull.Value : item.ArchiveCustomer);
						sqlCommand.Parameters.AddWithValue("ArchiveSupplier" + i, item.ArchiveSupplier == null ? (object)DBNull.Value : item.ArchiveSupplier);
						sqlCommand.Parameters.AddWithValue("ArticleAddCustomerDocument" + i, item.ArticleAddCustomerDocument == null ? (object)DBNull.Value : item.ArticleAddCustomerDocument);
						sqlCommand.Parameters.AddWithValue("ArticleBOM" + i, item.ArticleBOM);
						sqlCommand.Parameters.AddWithValue("ArticleCts" + i, item.ArticleCts);
						sqlCommand.Parameters.AddWithValue("ArticleData" + i, item.ArticleData);
						sqlCommand.Parameters.AddWithValue("ArticleDeleteCustomerDocument" + i, item.ArticleDeleteCustomerDocument == null ? (object)DBNull.Value : item.ArticleDeleteCustomerDocument);
						sqlCommand.Parameters.AddWithValue("ArticleLogistics" + i, item.ArticleLogistics);
						sqlCommand.Parameters.AddWithValue("ArticleLogisticsPrices" + i, item.ArticleLogisticsPrices == null ? (object)DBNull.Value : item.ArticleLogisticsPrices);
						sqlCommand.Parameters.AddWithValue("ArticleOverview" + i, item.ArticleOverview);
						sqlCommand.Parameters.AddWithValue("ArticleProduction" + i, item.ArticleProduction);
						sqlCommand.Parameters.AddWithValue("ArticlePurchase" + i, item.ArticlePurchase);
						sqlCommand.Parameters.AddWithValue("ArticleQuality" + i, item.ArticleQuality);
						sqlCommand.Parameters.AddWithValue("Articles" + i, item.Articles);
						sqlCommand.Parameters.AddWithValue("ArticleSales" + i, item.ArticleSales);
						sqlCommand.Parameters.AddWithValue("ArticleSalesCustom" + i, item.ArticleSalesCustom);
						sqlCommand.Parameters.AddWithValue("ArticleSalesItem" + i, item.ArticleSalesItem);
						sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlEngineering" + i, item.ArticlesBOMCPControlEngineering == null ? (object)DBNull.Value : item.ArticlesBOMCPControlEngineering);
						sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlHistory" + i, item.ArticlesBOMCPControlHistory == null ? (object)DBNull.Value : item.ArticlesBOMCPControlHistory);
						sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlQuality" + i, item.ArticlesBOMCPControlQuality == null ? (object)DBNull.Value : item.ArticlesBOMCPControlQuality);
						sqlCommand.Parameters.AddWithValue("ArticleStatistics" + i, item.ArticleStatistics);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineering" + i, item.ArticleStatisticsEngineering);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineeringEdit" + i, item.ArticleStatisticsEngineeringEdit);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccounting" + i, item.ArticleStatisticsFinanceAccounting);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccountingEdit" + i, item.ArticleStatisticsFinanceAccountingEdit);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogistics" + i, item.ArticleStatisticsLogistics);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogisticsEdit" + i, item.ArticleStatisticsLogisticsEdit);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchase" + i, item.ArticleStatisticsPurchase);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchaseEdit" + i, item.ArticleStatisticsPurchaseEdit);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnic" + i, item.ArticleStatisticsTechnic);
						sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnicEdit" + i, item.ArticleStatisticsTechnicEdit);
						sqlCommand.Parameters.AddWithValue("BomChangeRequests" + i, item.BomChangeRequests == null ? (object)DBNull.Value : item.BomChangeRequests);
						sqlCommand.Parameters.AddWithValue("CocType" + i, item.CocType == null ? (object)DBNull.Value : item.CocType);
						sqlCommand.Parameters.AddWithValue("ConditionAssignment" + i, item.ConditionAssignment == null ? (object)DBNull.Value : item.ConditionAssignment);
						sqlCommand.Parameters.AddWithValue("ConfigArticle" + i, item.ConfigArticle);
						sqlCommand.Parameters.AddWithValue("ConfigCustomer" + i, item.ConfigCustomer == null ? (object)DBNull.Value : item.ConfigCustomer);
						sqlCommand.Parameters.AddWithValue("ConfigSupplier" + i, item.ConfigSupplier == null ? (object)DBNull.Value : item.ConfigSupplier);
						sqlCommand.Parameters.AddWithValue("ContactAddress" + i, item.ContactAddress == null ? (object)DBNull.Value : item.ContactAddress);
						sqlCommand.Parameters.AddWithValue("ContactSalutation" + i, item.ContactSalutation == null ? (object)DBNull.Value : item.ContactSalutation);
						sqlCommand.Parameters.AddWithValue("CreateArticlePurchase" + i, item.CreateArticlePurchase);
						sqlCommand.Parameters.AddWithValue("CreateArticleSalesCustom" + i, item.CreateArticleSalesCustom);
						sqlCommand.Parameters.AddWithValue("CreateArticleSalesItem" + i, item.CreateArticleSalesItem);
						sqlCommand.Parameters.AddWithValue("CreateCustomerContactPerson" + i, item.CreateCustomerContactPerson == null ? (object)DBNull.Value : item.CreateCustomerContactPerson);
						sqlCommand.Parameters.AddWithValue("CreateSupplierContactPerson" + i, item.CreateSupplierContactPerson == null ? (object)DBNull.Value : item.CreateSupplierContactPerson);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Currencies" + i, item.Currencies == null ? (object)DBNull.Value : item.Currencies);
						sqlCommand.Parameters.AddWithValue("CustomerAddress" + i, item.CustomerAddress == null ? (object)DBNull.Value : item.CustomerAddress);
						sqlCommand.Parameters.AddWithValue("CustomerCommunication" + i, item.CustomerCommunication == null ? (object)DBNull.Value : item.CustomerCommunication);
						sqlCommand.Parameters.AddWithValue("CustomerContactPerson" + i, item.CustomerContactPerson == null ? (object)DBNull.Value : item.CustomerContactPerson);
						sqlCommand.Parameters.AddWithValue("CustomerData" + i, item.CustomerData == null ? (object)DBNull.Value : item.CustomerData);
						sqlCommand.Parameters.AddWithValue("CustomerGroup" + i, item.CustomerGroup == null ? (object)DBNull.Value : item.CustomerGroup);
						sqlCommand.Parameters.AddWithValue("CustomerHistory" + i, item.CustomerHistory == null ? (object)DBNull.Value : item.CustomerHistory);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("CustomerOverview" + i, item.CustomerOverview == null ? (object)DBNull.Value : item.CustomerOverview);
						sqlCommand.Parameters.AddWithValue("Customers" + i, item.Customers);
						sqlCommand.Parameters.AddWithValue("CustomerShipping" + i, item.CustomerShipping == null ? (object)DBNull.Value : item.CustomerShipping);
						sqlCommand.Parameters.AddWithValue("DeleteAltPositionsArticleBOM" + i, item.DeleteAltPositionsArticleBOM);
						sqlCommand.Parameters.AddWithValue("DeleteArticle" + i, item.DeleteArticle == null ? (object)DBNull.Value : item.DeleteArticle);
						sqlCommand.Parameters.AddWithValue("DeleteArticleBOM" + i, item.DeleteArticleBOM);
						sqlCommand.Parameters.AddWithValue("DeleteArticlePurchase" + i, item.DeleteArticlePurchase);
						sqlCommand.Parameters.AddWithValue("DeleteArticleSalesCustom" + i, item.DeleteArticleSalesCustom);
						sqlCommand.Parameters.AddWithValue("DeleteArticleSalesItem" + i, item.DeleteArticleSalesItem);
						sqlCommand.Parameters.AddWithValue("DeleteBCR" + i, item.DeleteBCR == null ? (object)DBNull.Value : item.DeleteBCR);
						sqlCommand.Parameters.AddWithValue("DeleteCustomerContactPerson" + i, item.DeleteCustomerContactPerson == null ? (object)DBNull.Value : item.DeleteCustomerContactPerson);
						sqlCommand.Parameters.AddWithValue("DeleteFiles" + i, item.DeleteFiles == null ? (object)DBNull.Value : item.DeleteFiles);
						sqlCommand.Parameters.AddWithValue("DeleteRohArtikelNummer" + i, item.DeleteRohArtikelNummer == null ? (object)DBNull.Value : item.DeleteRohArtikelNummer);
						sqlCommand.Parameters.AddWithValue("DeleteSupplierContactPerson" + i, item.DeleteSupplierContactPerson == null ? (object)DBNull.Value : item.DeleteSupplierContactPerson);
						sqlCommand.Parameters.AddWithValue("DiscountGroup" + i, item.DiscountGroup == null ? (object)DBNull.Value : item.DiscountGroup);
						sqlCommand.Parameters.AddWithValue("DownloadAllOutdatedEinkaufsPreis" + i, item.DownloadAllOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadAllOutdatedEinkaufsPreis);
						sqlCommand.Parameters.AddWithValue("DownloadFiles" + i, item.DownloadFiles == null ? (object)DBNull.Value : item.DownloadFiles);
						sqlCommand.Parameters.AddWithValue("DownloadOutdatedEinkaufsPreis" + i, item.DownloadOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadOutdatedEinkaufsPreis);
						sqlCommand.Parameters.AddWithValue("EdiConcern" + i, item.EdiConcern == null ? (object)DBNull.Value : item.EdiConcern);
						sqlCommand.Parameters.AddWithValue("EditAltPositionsArticleBOM" + i, item.EditAltPositionsArticleBOM);
						sqlCommand.Parameters.AddWithValue("EditArticle" + i, item.EditArticle);
						sqlCommand.Parameters.AddWithValue("EditArticleBOM" + i, item.EditArticleBOM);
						sqlCommand.Parameters.AddWithValue("EditArticleBOMPosition" + i, item.EditArticleBOMPosition);
						sqlCommand.Parameters.AddWithValue("EditArticleCts" + i, item.EditArticleCts);
						sqlCommand.Parameters.AddWithValue("EditArticleData" + i, item.EditArticleData);
						sqlCommand.Parameters.AddWithValue("EditArticleDesignation" + i, item.EditArticleDesignation);
						sqlCommand.Parameters.AddWithValue("EditArticleImage" + i, item.EditArticleImage);
						sqlCommand.Parameters.AddWithValue("EditArticleLogistics" + i, item.EditArticleLogistics);
						sqlCommand.Parameters.AddWithValue("EditArticleManager" + i, item.EditArticleManager);
						sqlCommand.Parameters.AddWithValue("EditArticleProduction" + i, item.EditArticleProduction);
						sqlCommand.Parameters.AddWithValue("EditArticlePurchase" + i, item.EditArticlePurchase);
						sqlCommand.Parameters.AddWithValue("EditArticleQuality" + i, item.EditArticleQuality);
						sqlCommand.Parameters.AddWithValue("EditArticleReference" + i, item.EditArticleReference == null ? (object)DBNull.Value : item.EditArticleReference);
						sqlCommand.Parameters.AddWithValue("EditArticleSalesCustom" + i, item.EditArticleSalesCustom);
						sqlCommand.Parameters.AddWithValue("EditArticleSalesItem" + i, item.EditArticleSalesItem);
						sqlCommand.Parameters.AddWithValue("EditCocType" + i, item.EditCocType == null ? (object)DBNull.Value : item.EditCocType);
						sqlCommand.Parameters.AddWithValue("EditConditionAssignment" + i, item.EditConditionAssignment == null ? (object)DBNull.Value : item.EditConditionAssignment);
						sqlCommand.Parameters.AddWithValue("EditContactAddress" + i, item.EditContactAddress == null ? (object)DBNull.Value : item.EditContactAddress);
						sqlCommand.Parameters.AddWithValue("EditContactSalutation" + i, item.EditContactSalutation == null ? (object)DBNull.Value : item.EditContactSalutation);
						sqlCommand.Parameters.AddWithValue("EditCurrencies" + i, item.EditCurrencies == null ? (object)DBNull.Value : item.EditCurrencies);
						sqlCommand.Parameters.AddWithValue("EditCustomer" + i, item.EditCustomer == null ? (object)DBNull.Value : item.EditCustomer);
						sqlCommand.Parameters.AddWithValue("EditCustomerAddress" + i, item.EditCustomerAddress == null ? (object)DBNull.Value : item.EditCustomerAddress);
						sqlCommand.Parameters.AddWithValue("EditCustomerCommunication" + i, item.EditCustomerCommunication == null ? (object)DBNull.Value : item.EditCustomerCommunication);
						sqlCommand.Parameters.AddWithValue("EditCustomerContactPerson" + i, item.EditCustomerContactPerson == null ? (object)DBNull.Value : item.EditCustomerContactPerson);
						sqlCommand.Parameters.AddWithValue("EditCustomerCoordination" + i, item.EditCustomerCoordination == null ? (object)DBNull.Value : item.EditCustomerCoordination);
						sqlCommand.Parameters.AddWithValue("EditCustomerData" + i, item.EditCustomerData == null ? (object)DBNull.Value : item.EditCustomerData);
						sqlCommand.Parameters.AddWithValue("EditCustomerGroup" + i, item.EditCustomerGroup == null ? (object)DBNull.Value : item.EditCustomerGroup);
						sqlCommand.Parameters.AddWithValue("EditCustomerImage" + i, item.EditCustomerImage == null ? (object)DBNull.Value : item.EditCustomerImage);
						sqlCommand.Parameters.AddWithValue("EditCustomerItemNumber" + i, item.EditCustomerItemNumber == null ? (object)DBNull.Value : item.EditCustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("EditCustomerShipping" + i, item.EditCustomerShipping == null ? (object)DBNull.Value : item.EditCustomerShipping);
						sqlCommand.Parameters.AddWithValue("EditDiscountGroup" + i, item.EditDiscountGroup == null ? (object)DBNull.Value : item.EditDiscountGroup);
						sqlCommand.Parameters.AddWithValue("EditEdiConcern" + i, item.EditEdiConcern == null ? (object)DBNull.Value : item.EditEdiConcern);
						sqlCommand.Parameters.AddWithValue("EditFibuFrame" + i, item.EditFibuFrame == null ? (object)DBNull.Value : item.EditFibuFrame);
						sqlCommand.Parameters.AddWithValue("EditHourlyRate" + i, item.EditHourlyRate == null ? (object)DBNull.Value : item.EditHourlyRate);
						sqlCommand.Parameters.AddWithValue("EditIndustry" + i, item.EditIndustry == null ? (object)DBNull.Value : item.EditIndustry);
						sqlCommand.Parameters.AddWithValue("EditLagerCCID" + i, item.EditLagerCCID == null ? (object)DBNull.Value : item.EditLagerCCID);
						sqlCommand.Parameters.AddWithValue("EditLagerMinStock" + i, item.EditLagerMinStock);
						sqlCommand.Parameters.AddWithValue("EditLagerOrderProposal" + i, item.EditLagerOrderProposal == null ? (object)DBNull.Value : item.EditLagerOrderProposal);
						sqlCommand.Parameters.AddWithValue("EditLagerStock" + i, item.EditLagerStock == null ? (object)DBNull.Value : item.EditLagerStock);
						sqlCommand.Parameters.AddWithValue("EditPayementPractises" + i, item.EditPayementPractises == null ? (object)DBNull.Value : item.EditPayementPractises);
						sqlCommand.Parameters.AddWithValue("EditPricingGroup" + i, item.EditPricingGroup == null ? (object)DBNull.Value : item.EditPricingGroup);
						sqlCommand.Parameters.AddWithValue("EditRohArtikelNummer" + i, item.EditRohArtikelNummer == null ? (object)DBNull.Value : item.EditRohArtikelNummer);
						sqlCommand.Parameters.AddWithValue("EditShippingMethods" + i, item.EditShippingMethods == null ? (object)DBNull.Value : item.EditShippingMethods);
						sqlCommand.Parameters.AddWithValue("EditSlipCircle" + i, item.EditSlipCircle == null ? (object)DBNull.Value : item.EditSlipCircle);
						sqlCommand.Parameters.AddWithValue("EditSupplier" + i, item.EditSupplier == null ? (object)DBNull.Value : item.EditSupplier);
						sqlCommand.Parameters.AddWithValue("EditSupplierAddress" + i, item.EditSupplierAddress == null ? (object)DBNull.Value : item.EditSupplierAddress);
						sqlCommand.Parameters.AddWithValue("EditSupplierCommunication" + i, item.EditSupplierCommunication == null ? (object)DBNull.Value : item.EditSupplierCommunication);
						sqlCommand.Parameters.AddWithValue("EditSupplierContactPerson" + i, item.EditSupplierContactPerson == null ? (object)DBNull.Value : item.EditSupplierContactPerson);
						sqlCommand.Parameters.AddWithValue("EditSupplierCoordination" + i, item.EditSupplierCoordination == null ? (object)DBNull.Value : item.EditSupplierCoordination);
						sqlCommand.Parameters.AddWithValue("EditSupplierData" + i, item.EditSupplierData == null ? (object)DBNull.Value : item.EditSupplierData);
						sqlCommand.Parameters.AddWithValue("EditSupplierGroup" + i, item.EditSupplierGroup == null ? (object)DBNull.Value : item.EditSupplierGroup);
						sqlCommand.Parameters.AddWithValue("EditSupplierImage" + i, item.EditSupplierImage == null ? (object)DBNull.Value : item.EditSupplierImage);
						sqlCommand.Parameters.AddWithValue("EditSupplierShipping" + i, item.EditSupplierShipping == null ? (object)DBNull.Value : item.EditSupplierShipping);
						sqlCommand.Parameters.AddWithValue("EditTermsOfPayment" + i, item.EditTermsOfPayment == null ? (object)DBNull.Value : item.EditTermsOfPayment);
						sqlCommand.Parameters.AddWithValue("EDrawingEdit" + i, item.EDrawingEdit == null ? (object)DBNull.Value : item.EDrawingEdit);
						sqlCommand.Parameters.AddWithValue("EinkaufsPreisUpdate" + i, item.EinkaufsPreisUpdate == null ? (object)DBNull.Value : item.EinkaufsPreisUpdate);
						sqlCommand.Parameters.AddWithValue("FibuFrame" + i, item.FibuFrame == null ? (object)DBNull.Value : item.FibuFrame);
						sqlCommand.Parameters.AddWithValue("GetRohArtikelNummer" + i, item.GetRohArtikelNummer == null ? (object)DBNull.Value : item.GetRohArtikelNummer);
						sqlCommand.Parameters.AddWithValue("HourlyRate" + i, item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
						sqlCommand.Parameters.AddWithValue("ImportArticleBOM" + i, item.ImportArticleBOM);
						sqlCommand.Parameters.AddWithValue("Industry" + i, item.Industry == null ? (object)DBNull.Value : item.Industry);
						sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
						sqlCommand.Parameters.AddWithValue("LagerArticleLogistics" + i, item.LagerArticleLogistics);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("ModuleAdministrator" + i, item.ModuleAdministrator == null ? (object)DBNull.Value : item.ModuleAdministrator);
						sqlCommand.Parameters.AddWithValue("offer" + i, item.offer == null ? (object)DBNull.Value : item.offer);
						sqlCommand.Parameters.AddWithValue("OfferRequestADD" + i, item.OfferRequestADD == null ? (object)DBNull.Value : item.OfferRequestADD);
						sqlCommand.Parameters.AddWithValue("OfferRequestApplyPrice" + i, item.OfferRequestApplyPrice == null ? (object)DBNull.Value : item.OfferRequestApplyPrice);
						sqlCommand.Parameters.AddWithValue("OfferRequestDelete" + i, item.OfferRequestDelete == null ? (object)DBNull.Value : item.OfferRequestDelete);
						sqlCommand.Parameters.AddWithValue("OfferRequestEdit" + i, item.OfferRequestEdit == null ? (object)DBNull.Value : item.OfferRequestEdit);
						sqlCommand.Parameters.AddWithValue("OfferRequestEditEmail" + i, item.OfferRequestEditEmail == null ? (object)DBNull.Value : item.OfferRequestEditEmail);
						sqlCommand.Parameters.AddWithValue("OfferRequestSendEmail" + i, item.OfferRequestSendEmail == null ? (object)DBNull.Value : item.OfferRequestSendEmail);
						sqlCommand.Parameters.AddWithValue("OfferRequestView" + i, item.OfferRequestView == null ? (object)DBNull.Value : item.OfferRequestView);
						sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoAdd" + i, item.PackagingsLgtPhotoAdd == null ? (object)DBNull.Value : item.PackagingsLgtPhotoAdd);
						sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoDelete" + i, item.PackagingsLgtPhotoDelete == null ? (object)DBNull.Value : item.PackagingsLgtPhotoDelete);
						sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoView" + i, item.PackagingsLgtPhotoView == null ? (object)DBNull.Value : item.PackagingsLgtPhotoView);
						sqlCommand.Parameters.AddWithValue("PayementPractises" + i, item.PayementPractises == null ? (object)DBNull.Value : item.PayementPractises);
						sqlCommand.Parameters.AddWithValue("PMAddCable" + i, item.PMAddCable == null ? (object)DBNull.Value : item.PMAddCable);
						sqlCommand.Parameters.AddWithValue("PMAddMileStone" + i, item.PMAddMileStone == null ? (object)DBNull.Value : item.PMAddMileStone);
						sqlCommand.Parameters.AddWithValue("PMAddProject" + i, item.PMAddProject == null ? (object)DBNull.Value : item.PMAddProject);
						sqlCommand.Parameters.AddWithValue("PMDeleteCable" + i, item.PMDeleteCable == null ? (object)DBNull.Value : item.PMDeleteCable);
						sqlCommand.Parameters.AddWithValue("PMDeleteMileStone" + i, item.PMDeleteMileStone == null ? (object)DBNull.Value : item.PMDeleteMileStone);
						sqlCommand.Parameters.AddWithValue("PMDeleteProject" + i, item.PMDeleteProject == null ? (object)DBNull.Value : item.PMDeleteProject);
						sqlCommand.Parameters.AddWithValue("PMEditCable" + i, item.PMEditCable == null ? (object)DBNull.Value : item.PMEditCable);
						sqlCommand.Parameters.AddWithValue("PMEditMileStone" + i, item.PMEditMileStone == null ? (object)DBNull.Value : item.PMEditMileStone);
						sqlCommand.Parameters.AddWithValue("PMEditProject" + i, item.PMEditProject == null ? (object)DBNull.Value : item.PMEditProject);
						sqlCommand.Parameters.AddWithValue("PMModule" + i, item.PMModule == null ? (object)DBNull.Value : item.PMModule);
						sqlCommand.Parameters.AddWithValue("PMViewProjectsCompact" + i, item.PMViewProjectsCompact == null ? (object)DBNull.Value : item.PMViewProjectsCompact);
						sqlCommand.Parameters.AddWithValue("PMViewProjectsDetail" + i, item.PMViewProjectsDetail == null ? (object)DBNull.Value : item.PMViewProjectsDetail);
						sqlCommand.Parameters.AddWithValue("PMViewProjectsMedium" + i, item.PMViewProjectsMedium == null ? (object)DBNull.Value : item.PMViewProjectsMedium);
						sqlCommand.Parameters.AddWithValue("PricingGroup" + i, item.PricingGroup == null ? (object)DBNull.Value : item.PricingGroup);
						sqlCommand.Parameters.AddWithValue("RemoveArticleReference" + i, item.RemoveArticleReference == null ? (object)DBNull.Value : item.RemoveArticleReference);
						sqlCommand.Parameters.AddWithValue("Settings" + i, item.Settings == null ? (object)DBNull.Value : item.Settings);
						sqlCommand.Parameters.AddWithValue("ShippingMethods" + i, item.ShippingMethods == null ? (object)DBNull.Value : item.ShippingMethods);
						sqlCommand.Parameters.AddWithValue("SlipCircle" + i, item.SlipCircle == null ? (object)DBNull.Value : item.SlipCircle);
						sqlCommand.Parameters.AddWithValue("SupplierAddress" + i, item.SupplierAddress == null ? (object)DBNull.Value : item.SupplierAddress);
						sqlCommand.Parameters.AddWithValue("SupplierAttachementAddFile" + i, item.SupplierAttachementAddFile == null ? (object)DBNull.Value : item.SupplierAttachementAddFile);
						sqlCommand.Parameters.AddWithValue("SupplierAttachementGetFile" + i, item.SupplierAttachementGetFile == null ? (object)DBNull.Value : item.SupplierAttachementGetFile);
						sqlCommand.Parameters.AddWithValue("SupplierAttachementRemoveFile" + i, item.SupplierAttachementRemoveFile == null ? (object)DBNull.Value : item.SupplierAttachementRemoveFile);
						sqlCommand.Parameters.AddWithValue("SupplierCommunication" + i, item.SupplierCommunication == null ? (object)DBNull.Value : item.SupplierCommunication);
						sqlCommand.Parameters.AddWithValue("SupplierContactPerson" + i, item.SupplierContactPerson == null ? (object)DBNull.Value : item.SupplierContactPerson);
						sqlCommand.Parameters.AddWithValue("SupplierData" + i, item.SupplierData == null ? (object)DBNull.Value : item.SupplierData);
						sqlCommand.Parameters.AddWithValue("SupplierGroup" + i, item.SupplierGroup == null ? (object)DBNull.Value : item.SupplierGroup);
						sqlCommand.Parameters.AddWithValue("SupplierHistory" + i, item.SupplierHistory == null ? (object)DBNull.Value : item.SupplierHistory);
						sqlCommand.Parameters.AddWithValue("SupplierOverview" + i, item.SupplierOverview == null ? (object)DBNull.Value : item.SupplierOverview);
						sqlCommand.Parameters.AddWithValue("Suppliers" + i, item.Suppliers);
						sqlCommand.Parameters.AddWithValue("SupplierShipping" + i, item.SupplierShipping == null ? (object)DBNull.Value : item.SupplierShipping);
						sqlCommand.Parameters.AddWithValue("TermsOfPayment" + i, item.TermsOfPayment == null ? (object)DBNull.Value : item.TermsOfPayment);
						sqlCommand.Parameters.AddWithValue("UploadAltPositionsArticleBOM" + i, item.UploadAltPositionsArticleBOM);
						sqlCommand.Parameters.AddWithValue("UploadArticleBOM" + i, item.UploadArticleBOM);
						sqlCommand.Parameters.AddWithValue("ValidateArticleBOM" + i, item.ValidateArticleBOM == null ? (object)DBNull.Value : item.ValidateArticleBOM);
						sqlCommand.Parameters.AddWithValue("ValidateBCR" + i, item.ValidateBCR == null ? (object)DBNull.Value : item.ValidateBCR);
						sqlCommand.Parameters.AddWithValue("ViewAltPositionsArticleBOM" + i, item.ViewAltPositionsArticleBOM);
						sqlCommand.Parameters.AddWithValue("ViewArticleLog" + i, item.ViewArticleLog);
						sqlCommand.Parameters.AddWithValue("ViewArticleReference" + i, item.ViewArticleReference == null ? (object)DBNull.Value : item.ViewArticleReference);
						sqlCommand.Parameters.AddWithValue("ViewArticles" + i, item.ViewArticles);
						sqlCommand.Parameters.AddWithValue("ViewBCR" + i, item.ViewBCR == null ? (object)DBNull.Value : item.ViewBCR);
						sqlCommand.Parameters.AddWithValue("ViewCustomers" + i, item.ViewCustomers == null ? (object)DBNull.Value : item.ViewCustomers);
						sqlCommand.Parameters.AddWithValue("ViewLPCustomer" + i, item.ViewLPCustomer == null ? (object)DBNull.Value : item.ViewLPCustomer);
						sqlCommand.Parameters.AddWithValue("ViewLPSupplier" + i, item.ViewLPSupplier == null ? (object)DBNull.Value : item.ViewLPSupplier);
						sqlCommand.Parameters.AddWithValue("ViewSupplierAddressComments" + i, item.ViewSupplierAddressComments == null ? (object)DBNull.Value : item.ViewSupplierAddressComments);
						sqlCommand.Parameters.AddWithValue("ViewSuppliers" + i, item.ViewSuppliers == null ? (object)DBNull.Value : item.ViewSuppliers);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__BSD_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = sqlCommand.ExecuteNonQuery();
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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					string query = "DELETE FROM [__BSD_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Entities.Tables.BSD.AccessProfileEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_AccessProfile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.BSD.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.BSD.AccessProfileEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_AccessProfile]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.BSD.AccessProfileEntity>();
			}
		}
		public static List<Entities.Tables.BSD.AccessProfileEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.BSD.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.BSD.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Entities.Tables.BSD.AccessProfileEntity>();
		}
		private static List<Entities.Tables.BSD.AccessProfileEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_AccessProfile] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.BSD.AccessProfileEntity>();
				}
			}
			return new List<Entities.Tables.BSD.AccessProfileEntity>();
		}

		public static int InsertWithTransaction(Entities.Tables.BSD.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__BSD_AccessProfile] ([AccessProfileName],[AddArticle],[AddArticleReference],[AddBCR],[AddCocType],[AddConditionAssignment],[AddContactAddress],[AddContactSalutation],[AddCurrencies],[AddCustomer],[AddCustomerGroup],[AddCustomerItemNumber],[AddDiscountGroup],[AddEdiConcern],[AddFibuFrame],[AddFiles],[AddHourlyRate],[AddIndustry],[AddPayementPractises],[AddPricingGroup],[AddRohArtikelNummer],[AddShippingMethods],[AddSlipCircle],[AddSupplier],[AddSupplierGroup],[AddTermsOfPayment],[Administration],[AltPositionsArticleBOM],[ArchiveArticle],[ArchiveCustomer],[ArchiveSupplier],[ArticleAddCustomerDocument],[ArticleBOM],[ArticleCts],[ArticleData],[ArticleDeleteCustomerDocument],[ArticleLogistics],[ArticleLogisticsPrices],[ArticleOverview],[ArticleProduction],[ArticlePurchase],[ArticleQuality],[Articles],[ArticleSales],[ArticleSalesCustom],[ArticleSalesItem],[ArticlesBOMCPControlEngineering],[ArticlesBOMCPControlHistory],[ArticlesBOMCPControlQuality],[ArticleStatistics],[ArticleStatisticsEngineering],[ArticleStatisticsEngineeringEdit],[ArticleStatisticsFinanceAccounting],[ArticleStatisticsFinanceAccountingEdit],[ArticleStatisticsLogistics],[ArticleStatisticsLogisticsEdit],[ArticleStatisticsPurchase],[ArticleStatisticsPurchaseEdit],[ArticleStatisticsTechnic],[ArticleStatisticsTechnicEdit],[BomChangeRequests],[CocType],[ConditionAssignment],[ConfigArticle],[ConfigCustomer],[ConfigSupplier],[ContactAddress],[ContactSalutation],[CreateArticlePurchase],[CreateArticleSalesCustom],[CreateArticleSalesItem],[CreateCustomerContactPerson],[CreateSupplierContactPerson],[CreationTime],[CreationUserId],[Currencies],[CustomerAddress],[CustomerCommunication],[CustomerContactPerson],[CustomerData],[CustomerGroup],[CustomerHistory],[CustomerItemNumber],[CustomerOverview],[Customers],[CustomerShipping],[DeleteAltPositionsArticleBOM],[DeleteArticle],[DeleteArticleBOM],[DeleteArticlePurchase],[DeleteArticleSalesCustom],[DeleteArticleSalesItem],[DeleteBCR],[DeleteCustomerContactPerson],[DeleteFiles],[DeleteRohArtikelNummer],[DeleteSupplierContactPerson],[DiscountGroup],[DownloadAllOutdatedEinkaufsPreis],[DownloadFiles],[DownloadOutdatedEinkaufsPreis],[EdiConcern],[EditAltPositionsArticleBOM],[EditArticle],[EditArticleBOM],[EditArticleBOMPosition],[EditArticleCts],[EditArticleData],[EditArticleDesignation],[EditArticleImage],[EditArticleLogistics],[EditArticleManager],[EditArticleProduction],[EditArticlePurchase],[EditArticleQuality],[EditArticleReference],[EditArticleSalesCustom],[EditArticleSalesItem],[EditCocType],[EditConditionAssignment],[EditContactAddress],[EditContactSalutation],[EditCurrencies],[EditCustomer],[EditCustomerAddress],[EditCustomerCommunication],[EditCustomerContactPerson],[EditCustomerCoordination],[EditCustomerData],[EditCustomerGroup],[EditCustomerImage],[EditCustomerItemNumber],[EditCustomerShipping],[EditDiscountGroup],[EditEdiConcern],[EditFibuFrame],[EditHourlyRate],[EditIndustry],[EditLagerCCID],[EditLagerMinStock],[EditLagerOrderProposal],[EditLagerStock],[EditPayementPractises],[EditPricingGroup],[EditRohArtikelNummer],[EditShippingMethods],[EditSlipCircle],[EditSupplier],[EditSupplierAddress],[EditSupplierCommunication],[EditSupplierContactPerson],[EditSupplierCoordination],[EditSupplierData],[EditSupplierGroup],[EditSupplierImage],[EditSupplierShipping],[EditTermsOfPayment],[EDrawingEdit],[EinkaufsPreisUpdate],[FibuFrame],[GetRohArtikelNummer],[HourlyRate],[ImportArticleBOM],[Industry],[isDefault],[LagerArticleLogistics],[ModuleActivated],[ModuleAdministrator],[offer],[OfferRequestADD],[OfferRequestApplyPrice],[OfferRequestDelete],[OfferRequestEdit],[OfferRequestEditEmail],[OfferRequestSendEmail],[OfferRequestView],[PackagingsLgtPhotoAdd],[PackagingsLgtPhotoDelete],[PackagingsLgtPhotoView],[PayementPractises],[PMAddCable],[PMAddMileStone],[PMAddProject],[PMDeleteCable],[PMDeleteMileStone],[PMDeleteProject],[PMEditCable],[PMEditMileStone],[PMEditProject],[PMModule],[PMViewProjectsCompact],[PMViewProjectsDetail],[PMViewProjectsMedium],[PricingGroup],[RemoveArticleReference],[Settings],[ShippingMethods],[SlipCircle],[SupplierAddress],[SupplierAttachementAddFile],[SupplierAttachementGetFile],[SupplierAttachementRemoveFile],[SupplierCommunication],[SupplierContactPerson],[SupplierData],[SupplierGroup],[SupplierHistory],[SupplierOverview],[Suppliers],[SupplierShipping],[TermsOfPayment],[UploadAltPositionsArticleBOM],[UploadArticleBOM],[ValidateArticleBOM],[ValidateBCR],[ViewAltPositionsArticleBOM],[ViewArticleLog],[ViewArticleReference],[ViewArticles],[ViewBCR],[ViewCustomers],[ViewLPCustomer],[ViewLPSupplier],[ViewSupplierAddressComments],[ViewSuppliers]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileName,@AddArticle,@AddArticleReference,@AddBCR,@AddCocType,@AddConditionAssignment,@AddContactAddress,@AddContactSalutation,@AddCurrencies,@AddCustomer,@AddCustomerGroup,@AddCustomerItemNumber,@AddDiscountGroup,@AddEdiConcern,@AddFibuFrame,@AddFiles,@AddHourlyRate,@AddIndustry,@AddPayementPractises,@AddPricingGroup,@AddRohArtikelNummer,@AddShippingMethods,@AddSlipCircle,@AddSupplier,@AddSupplierGroup,@AddTermsOfPayment,@Administration,@AltPositionsArticleBOM,@ArchiveArticle,@ArchiveCustomer,@ArchiveSupplier,@ArticleAddCustomerDocument,@ArticleBOM,@ArticleCts,@ArticleData,@ArticleDeleteCustomerDocument,@ArticleLogistics,@ArticleLogisticsPrices,@ArticleOverview,@ArticleProduction,@ArticlePurchase,@ArticleQuality,@Articles,@ArticleSales,@ArticleSalesCustom,@ArticleSalesItem,@ArticlesBOMCPControlEngineering,@ArticlesBOMCPControlHistory,@ArticlesBOMCPControlQuality,@ArticleStatistics,@ArticleStatisticsEngineering,@ArticleStatisticsEngineeringEdit,@ArticleStatisticsFinanceAccounting,@ArticleStatisticsFinanceAccountingEdit,@ArticleStatisticsLogistics,@ArticleStatisticsLogisticsEdit,@ArticleStatisticsPurchase,@ArticleStatisticsPurchaseEdit,@ArticleStatisticsTechnic,@ArticleStatisticsTechnicEdit,@BomChangeRequests,@CocType,@ConditionAssignment,@ConfigArticle,@ConfigCustomer,@ConfigSupplier,@ContactAddress,@ContactSalutation,@CreateArticlePurchase,@CreateArticleSalesCustom,@CreateArticleSalesItem,@CreateCustomerContactPerson,@CreateSupplierContactPerson,@CreationTime,@CreationUserId,@Currencies,@CustomerAddress,@CustomerCommunication,@CustomerContactPerson,@CustomerData,@CustomerGroup,@CustomerHistory,@CustomerItemNumber,@CustomerOverview,@Customers,@CustomerShipping,@DeleteAltPositionsArticleBOM,@DeleteArticle,@DeleteArticleBOM,@DeleteArticlePurchase,@DeleteArticleSalesCustom,@DeleteArticleSalesItem,@DeleteBCR,@DeleteCustomerContactPerson,@DeleteFiles,@DeleteRohArtikelNummer,@DeleteSupplierContactPerson,@DiscountGroup,@DownloadAllOutdatedEinkaufsPreis,@DownloadFiles,@DownloadOutdatedEinkaufsPreis,@EdiConcern,@EditAltPositionsArticleBOM,@EditArticle,@EditArticleBOM,@EditArticleBOMPosition,@EditArticleCts,@EditArticleData,@EditArticleDesignation,@EditArticleImage,@EditArticleLogistics,@EditArticleManager,@EditArticleProduction,@EditArticlePurchase,@EditArticleQuality,@EditArticleReference,@EditArticleSalesCustom,@EditArticleSalesItem,@EditCocType,@EditConditionAssignment,@EditContactAddress,@EditContactSalutation,@EditCurrencies,@EditCustomer,@EditCustomerAddress,@EditCustomerCommunication,@EditCustomerContactPerson,@EditCustomerCoordination,@EditCustomerData,@EditCustomerGroup,@EditCustomerImage,@EditCustomerItemNumber,@EditCustomerShipping,@EditDiscountGroup,@EditEdiConcern,@EditFibuFrame,@EditHourlyRate,@EditIndustry,@EditLagerCCID,@EditLagerMinStock,@EditLagerOrderProposal,@EditLagerStock,@EditPayementPractises,@EditPricingGroup,@EditRohArtikelNummer,@EditShippingMethods,@EditSlipCircle,@EditSupplier,@EditSupplierAddress,@EditSupplierCommunication,@EditSupplierContactPerson,@EditSupplierCoordination,@EditSupplierData,@EditSupplierGroup,@EditSupplierImage,@EditSupplierShipping,@EditTermsOfPayment,@EDrawingEdit,@EinkaufsPreisUpdate,@FibuFrame,@GetRohArtikelNummer,@HourlyRate,@ImportArticleBOM,@Industry,@isDefault,@LagerArticleLogistics,@ModuleActivated,@ModuleAdministrator,@offer,@OfferRequestADD,@OfferRequestApplyPrice,@OfferRequestDelete,@OfferRequestEdit,@OfferRequestEditEmail,@OfferRequestSendEmail,@OfferRequestView,@PackagingsLgtPhotoAdd,@PackagingsLgtPhotoDelete,@PackagingsLgtPhotoView,@PayementPractises,@PMAddCable,@PMAddMileStone,@PMAddProject,@PMDeleteCable,@PMDeleteMileStone,@PMDeleteProject,@PMEditCable,@PMEditMileStone,@PMEditProject,@PMModule,@PMViewProjectsCompact,@PMViewProjectsDetail,@PMViewProjectsMedium,@PricingGroup,@RemoveArticleReference,@Settings,@ShippingMethods,@SlipCircle,@SupplierAddress,@SupplierAttachementAddFile,@SupplierAttachementGetFile,@SupplierAttachementRemoveFile,@SupplierCommunication,@SupplierContactPerson,@SupplierData,@SupplierGroup,@SupplierHistory,@SupplierOverview,@Suppliers,@SupplierShipping,@TermsOfPayment,@UploadAltPositionsArticleBOM,@UploadArticleBOM,@ValidateArticleBOM,@ValidateBCR,@ViewAltPositionsArticleBOM,@ViewArticleLog,@ViewArticleReference,@ViewArticles,@ViewBCR,@ViewCustomers,@ViewLPCustomer,@ViewLPSupplier,@ViewSupplierAddressComments,@ViewSuppliers); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("AddArticle", item.AddArticle);
			sqlCommand.Parameters.AddWithValue("AddArticleReference", item.AddArticleReference == null ? (object)DBNull.Value : item.AddArticleReference);
			sqlCommand.Parameters.AddWithValue("AddBCR", item.AddBCR == null ? (object)DBNull.Value : item.AddBCR);
			sqlCommand.Parameters.AddWithValue("AddCocType", item.AddCocType == null ? (object)DBNull.Value : item.AddCocType);
			sqlCommand.Parameters.AddWithValue("AddConditionAssignment", item.AddConditionAssignment == null ? (object)DBNull.Value : item.AddConditionAssignment);
			sqlCommand.Parameters.AddWithValue("AddContactAddress", item.AddContactAddress == null ? (object)DBNull.Value : item.AddContactAddress);
			sqlCommand.Parameters.AddWithValue("AddContactSalutation", item.AddContactSalutation == null ? (object)DBNull.Value : item.AddContactSalutation);
			sqlCommand.Parameters.AddWithValue("AddCurrencies", item.AddCurrencies == null ? (object)DBNull.Value : item.AddCurrencies);
			sqlCommand.Parameters.AddWithValue("AddCustomer", item.AddCustomer == null ? (object)DBNull.Value : item.AddCustomer);
			sqlCommand.Parameters.AddWithValue("AddCustomerGroup", item.AddCustomerGroup == null ? (object)DBNull.Value : item.AddCustomerGroup);
			sqlCommand.Parameters.AddWithValue("AddCustomerItemNumber", item.AddCustomerItemNumber == null ? (object)DBNull.Value : item.AddCustomerItemNumber);
			sqlCommand.Parameters.AddWithValue("AddDiscountGroup", item.AddDiscountGroup == null ? (object)DBNull.Value : item.AddDiscountGroup);
			sqlCommand.Parameters.AddWithValue("AddEdiConcern", item.AddEdiConcern == null ? (object)DBNull.Value : item.AddEdiConcern);
			sqlCommand.Parameters.AddWithValue("AddFibuFrame", item.AddFibuFrame == null ? (object)DBNull.Value : item.AddFibuFrame);
			sqlCommand.Parameters.AddWithValue("AddFiles", item.AddFiles == null ? (object)DBNull.Value : item.AddFiles);
			sqlCommand.Parameters.AddWithValue("AddHourlyRate", item.AddHourlyRate == null ? (object)DBNull.Value : item.AddHourlyRate);
			sqlCommand.Parameters.AddWithValue("AddIndustry", item.AddIndustry == null ? (object)DBNull.Value : item.AddIndustry);
			sqlCommand.Parameters.AddWithValue("AddPayementPractises", item.AddPayementPractises == null ? (object)DBNull.Value : item.AddPayementPractises);
			sqlCommand.Parameters.AddWithValue("AddPricingGroup", item.AddPricingGroup == null ? (object)DBNull.Value : item.AddPricingGroup);
			sqlCommand.Parameters.AddWithValue("AddRohArtikelNummer", item.AddRohArtikelNummer == null ? (object)DBNull.Value : item.AddRohArtikelNummer);
			sqlCommand.Parameters.AddWithValue("AddShippingMethods", item.AddShippingMethods == null ? (object)DBNull.Value : item.AddShippingMethods);
			sqlCommand.Parameters.AddWithValue("AddSlipCircle", item.AddSlipCircle == null ? (object)DBNull.Value : item.AddSlipCircle);
			sqlCommand.Parameters.AddWithValue("AddSupplier", item.AddSupplier == null ? (object)DBNull.Value : item.AddSupplier);
			sqlCommand.Parameters.AddWithValue("AddSupplierGroup", item.AddSupplierGroup == null ? (object)DBNull.Value : item.AddSupplierGroup);
			sqlCommand.Parameters.AddWithValue("AddTermsOfPayment", item.AddTermsOfPayment == null ? (object)DBNull.Value : item.AddTermsOfPayment);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration);
			sqlCommand.Parameters.AddWithValue("AltPositionsArticleBOM", item.AltPositionsArticleBOM);
			sqlCommand.Parameters.AddWithValue("ArchiveArticle", item.ArchiveArticle);
			sqlCommand.Parameters.AddWithValue("ArchiveCustomer", item.ArchiveCustomer == null ? (object)DBNull.Value : item.ArchiveCustomer);
			sqlCommand.Parameters.AddWithValue("ArchiveSupplier", item.ArchiveSupplier == null ? (object)DBNull.Value : item.ArchiveSupplier);
			sqlCommand.Parameters.AddWithValue("ArticleAddCustomerDocument", item.ArticleAddCustomerDocument == null ? (object)DBNull.Value : item.ArticleAddCustomerDocument);
			sqlCommand.Parameters.AddWithValue("ArticleBOM", item.ArticleBOM);
			sqlCommand.Parameters.AddWithValue("ArticleCts", item.ArticleCts);
			sqlCommand.Parameters.AddWithValue("ArticleData", item.ArticleData);
			sqlCommand.Parameters.AddWithValue("ArticleDeleteCustomerDocument", item.ArticleDeleteCustomerDocument == null ? (object)DBNull.Value : item.ArticleDeleteCustomerDocument);
			sqlCommand.Parameters.AddWithValue("ArticleLogistics", item.ArticleLogistics);
			sqlCommand.Parameters.AddWithValue("ArticleLogisticsPrices", item.ArticleLogisticsPrices == null ? (object)DBNull.Value : item.ArticleLogisticsPrices);
			sqlCommand.Parameters.AddWithValue("ArticleOverview", item.ArticleOverview);
			sqlCommand.Parameters.AddWithValue("ArticleProduction", item.ArticleProduction);
			sqlCommand.Parameters.AddWithValue("ArticlePurchase", item.ArticlePurchase);
			sqlCommand.Parameters.AddWithValue("ArticleQuality", item.ArticleQuality);
			sqlCommand.Parameters.AddWithValue("Articles", item.Articles);
			sqlCommand.Parameters.AddWithValue("ArticleSales", item.ArticleSales);
			sqlCommand.Parameters.AddWithValue("ArticleSalesCustom", item.ArticleSalesCustom);
			sqlCommand.Parameters.AddWithValue("ArticleSalesItem", item.ArticleSalesItem);
			sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlEngineering", item.ArticlesBOMCPControlEngineering == null ? (object)DBNull.Value : item.ArticlesBOMCPControlEngineering);
			sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlHistory", item.ArticlesBOMCPControlHistory == null ? (object)DBNull.Value : item.ArticlesBOMCPControlHistory);
			sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlQuality", item.ArticlesBOMCPControlQuality == null ? (object)DBNull.Value : item.ArticlesBOMCPControlQuality);
			sqlCommand.Parameters.AddWithValue("ArticleStatistics", item.ArticleStatistics);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineering", item.ArticleStatisticsEngineering);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineeringEdit", item.ArticleStatisticsEngineeringEdit);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccounting", item.ArticleStatisticsFinanceAccounting);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccountingEdit", item.ArticleStatisticsFinanceAccountingEdit);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogistics", item.ArticleStatisticsLogistics);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogisticsEdit", item.ArticleStatisticsLogisticsEdit);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchase", item.ArticleStatisticsPurchase);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchaseEdit", item.ArticleStatisticsPurchaseEdit);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnic", item.ArticleStatisticsTechnic);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnicEdit", item.ArticleStatisticsTechnicEdit);
			sqlCommand.Parameters.AddWithValue("BomChangeRequests", item.BomChangeRequests == null ? (object)DBNull.Value : item.BomChangeRequests);
			sqlCommand.Parameters.AddWithValue("CocType", item.CocType == null ? (object)DBNull.Value : item.CocType);
			sqlCommand.Parameters.AddWithValue("ConditionAssignment", item.ConditionAssignment == null ? (object)DBNull.Value : item.ConditionAssignment);
			sqlCommand.Parameters.AddWithValue("ConfigArticle", item.ConfigArticle);
			sqlCommand.Parameters.AddWithValue("ConfigCustomer", item.ConfigCustomer == null ? (object)DBNull.Value : item.ConfigCustomer);
			sqlCommand.Parameters.AddWithValue("ConfigSupplier", item.ConfigSupplier == null ? (object)DBNull.Value : item.ConfigSupplier);
			sqlCommand.Parameters.AddWithValue("ContactAddress", item.ContactAddress == null ? (object)DBNull.Value : item.ContactAddress);
			sqlCommand.Parameters.AddWithValue("ContactSalutation", item.ContactSalutation == null ? (object)DBNull.Value : item.ContactSalutation);
			sqlCommand.Parameters.AddWithValue("CreateArticlePurchase", item.CreateArticlePurchase);
			sqlCommand.Parameters.AddWithValue("CreateArticleSalesCustom", item.CreateArticleSalesCustom);
			sqlCommand.Parameters.AddWithValue("CreateArticleSalesItem", item.CreateArticleSalesItem);
			sqlCommand.Parameters.AddWithValue("CreateCustomerContactPerson", item.CreateCustomerContactPerson == null ? (object)DBNull.Value : item.CreateCustomerContactPerson);
			sqlCommand.Parameters.AddWithValue("CreateSupplierContactPerson", item.CreateSupplierContactPerson == null ? (object)DBNull.Value : item.CreateSupplierContactPerson);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Currencies", item.Currencies == null ? (object)DBNull.Value : item.Currencies);
			sqlCommand.Parameters.AddWithValue("CustomerAddress", item.CustomerAddress == null ? (object)DBNull.Value : item.CustomerAddress);
			sqlCommand.Parameters.AddWithValue("CustomerCommunication", item.CustomerCommunication == null ? (object)DBNull.Value : item.CustomerCommunication);
			sqlCommand.Parameters.AddWithValue("CustomerContactPerson", item.CustomerContactPerson == null ? (object)DBNull.Value : item.CustomerContactPerson);
			sqlCommand.Parameters.AddWithValue("CustomerData", item.CustomerData == null ? (object)DBNull.Value : item.CustomerData);
			sqlCommand.Parameters.AddWithValue("CustomerGroup", item.CustomerGroup == null ? (object)DBNull.Value : item.CustomerGroup);
			sqlCommand.Parameters.AddWithValue("CustomerHistory", item.CustomerHistory == null ? (object)DBNull.Value : item.CustomerHistory);
			sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
			sqlCommand.Parameters.AddWithValue("CustomerOverview", item.CustomerOverview == null ? (object)DBNull.Value : item.CustomerOverview);
			sqlCommand.Parameters.AddWithValue("Customers", item.Customers);
			sqlCommand.Parameters.AddWithValue("CustomerShipping", item.CustomerShipping == null ? (object)DBNull.Value : item.CustomerShipping);
			sqlCommand.Parameters.AddWithValue("DeleteAltPositionsArticleBOM", item.DeleteAltPositionsArticleBOM);
			sqlCommand.Parameters.AddWithValue("DeleteArticle", item.DeleteArticle == null ? (object)DBNull.Value : item.DeleteArticle);
			sqlCommand.Parameters.AddWithValue("DeleteArticleBOM", item.DeleteArticleBOM);
			sqlCommand.Parameters.AddWithValue("DeleteArticlePurchase", item.DeleteArticlePurchase);
			sqlCommand.Parameters.AddWithValue("DeleteArticleSalesCustom", item.DeleteArticleSalesCustom);
			sqlCommand.Parameters.AddWithValue("DeleteArticleSalesItem", item.DeleteArticleSalesItem);
			sqlCommand.Parameters.AddWithValue("DeleteBCR", item.DeleteBCR == null ? (object)DBNull.Value : item.DeleteBCR);
			sqlCommand.Parameters.AddWithValue("DeleteCustomerContactPerson", item.DeleteCustomerContactPerson == null ? (object)DBNull.Value : item.DeleteCustomerContactPerson);
			sqlCommand.Parameters.AddWithValue("DeleteFiles", item.DeleteFiles == null ? (object)DBNull.Value : item.DeleteFiles);
			sqlCommand.Parameters.AddWithValue("DeleteRohArtikelNummer", item.DeleteRohArtikelNummer == null ? (object)DBNull.Value : item.DeleteRohArtikelNummer);
			sqlCommand.Parameters.AddWithValue("DeleteSupplierContactPerson", item.DeleteSupplierContactPerson == null ? (object)DBNull.Value : item.DeleteSupplierContactPerson);
			sqlCommand.Parameters.AddWithValue("DiscountGroup", item.DiscountGroup == null ? (object)DBNull.Value : item.DiscountGroup);
			sqlCommand.Parameters.AddWithValue("DownloadAllOutdatedEinkaufsPreis", item.DownloadAllOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadAllOutdatedEinkaufsPreis);
			sqlCommand.Parameters.AddWithValue("DownloadFiles", item.DownloadFiles == null ? (object)DBNull.Value : item.DownloadFiles);
			sqlCommand.Parameters.AddWithValue("DownloadOutdatedEinkaufsPreis", item.DownloadOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadOutdatedEinkaufsPreis);
			sqlCommand.Parameters.AddWithValue("EdiConcern", item.EdiConcern == null ? (object)DBNull.Value : item.EdiConcern);
			sqlCommand.Parameters.AddWithValue("EditAltPositionsArticleBOM", item.EditAltPositionsArticleBOM);
			sqlCommand.Parameters.AddWithValue("EditArticle", item.EditArticle);
			sqlCommand.Parameters.AddWithValue("EditArticleBOM", item.EditArticleBOM);
			sqlCommand.Parameters.AddWithValue("EditArticleBOMPosition", item.EditArticleBOMPosition);
			sqlCommand.Parameters.AddWithValue("EditArticleCts", item.EditArticleCts);
			sqlCommand.Parameters.AddWithValue("EditArticleData", item.EditArticleData);
			sqlCommand.Parameters.AddWithValue("EditArticleDesignation", item.EditArticleDesignation);
			sqlCommand.Parameters.AddWithValue("EditArticleImage", item.EditArticleImage);
			sqlCommand.Parameters.AddWithValue("EditArticleLogistics", item.EditArticleLogistics);
			sqlCommand.Parameters.AddWithValue("EditArticleManager", item.EditArticleManager);
			sqlCommand.Parameters.AddWithValue("EditArticleProduction", item.EditArticleProduction);
			sqlCommand.Parameters.AddWithValue("EditArticlePurchase", item.EditArticlePurchase);
			sqlCommand.Parameters.AddWithValue("EditArticleQuality", item.EditArticleQuality);
			sqlCommand.Parameters.AddWithValue("EditArticleReference", item.EditArticleReference == null ? (object)DBNull.Value : item.EditArticleReference);
			sqlCommand.Parameters.AddWithValue("EditArticleSalesCustom", item.EditArticleSalesCustom);
			sqlCommand.Parameters.AddWithValue("EditArticleSalesItem", item.EditArticleSalesItem);
			sqlCommand.Parameters.AddWithValue("EditCocType", item.EditCocType == null ? (object)DBNull.Value : item.EditCocType);
			sqlCommand.Parameters.AddWithValue("EditConditionAssignment", item.EditConditionAssignment == null ? (object)DBNull.Value : item.EditConditionAssignment);
			sqlCommand.Parameters.AddWithValue("EditContactAddress", item.EditContactAddress == null ? (object)DBNull.Value : item.EditContactAddress);
			sqlCommand.Parameters.AddWithValue("EditContactSalutation", item.EditContactSalutation == null ? (object)DBNull.Value : item.EditContactSalutation);
			sqlCommand.Parameters.AddWithValue("EditCurrencies", item.EditCurrencies == null ? (object)DBNull.Value : item.EditCurrencies);
			sqlCommand.Parameters.AddWithValue("EditCustomer", item.EditCustomer == null ? (object)DBNull.Value : item.EditCustomer);
			sqlCommand.Parameters.AddWithValue("EditCustomerAddress", item.EditCustomerAddress == null ? (object)DBNull.Value : item.EditCustomerAddress);
			sqlCommand.Parameters.AddWithValue("EditCustomerCommunication", item.EditCustomerCommunication == null ? (object)DBNull.Value : item.EditCustomerCommunication);
			sqlCommand.Parameters.AddWithValue("EditCustomerContactPerson", item.EditCustomerContactPerson == null ? (object)DBNull.Value : item.EditCustomerContactPerson);
			sqlCommand.Parameters.AddWithValue("EditCustomerCoordination", item.EditCustomerCoordination == null ? (object)DBNull.Value : item.EditCustomerCoordination);
			sqlCommand.Parameters.AddWithValue("EditCustomerData", item.EditCustomerData == null ? (object)DBNull.Value : item.EditCustomerData);
			sqlCommand.Parameters.AddWithValue("EditCustomerGroup", item.EditCustomerGroup == null ? (object)DBNull.Value : item.EditCustomerGroup);
			sqlCommand.Parameters.AddWithValue("EditCustomerImage", item.EditCustomerImage == null ? (object)DBNull.Value : item.EditCustomerImage);
			sqlCommand.Parameters.AddWithValue("EditCustomerItemNumber", item.EditCustomerItemNumber == null ? (object)DBNull.Value : item.EditCustomerItemNumber);
			sqlCommand.Parameters.AddWithValue("EditCustomerShipping", item.EditCustomerShipping == null ? (object)DBNull.Value : item.EditCustomerShipping);
			sqlCommand.Parameters.AddWithValue("EditDiscountGroup", item.EditDiscountGroup == null ? (object)DBNull.Value : item.EditDiscountGroup);
			sqlCommand.Parameters.AddWithValue("EditEdiConcern", item.EditEdiConcern == null ? (object)DBNull.Value : item.EditEdiConcern);
			sqlCommand.Parameters.AddWithValue("EditFibuFrame", item.EditFibuFrame == null ? (object)DBNull.Value : item.EditFibuFrame);
			sqlCommand.Parameters.AddWithValue("EditHourlyRate", item.EditHourlyRate == null ? (object)DBNull.Value : item.EditHourlyRate);
			sqlCommand.Parameters.AddWithValue("EditIndustry", item.EditIndustry == null ? (object)DBNull.Value : item.EditIndustry);
			sqlCommand.Parameters.AddWithValue("EditLagerCCID", item.EditLagerCCID == null ? (object)DBNull.Value : item.EditLagerCCID);
			sqlCommand.Parameters.AddWithValue("EditLagerMinStock", item.EditLagerMinStock);
			sqlCommand.Parameters.AddWithValue("EditLagerOrderProposal", item.EditLagerOrderProposal == null ? (object)DBNull.Value : item.EditLagerOrderProposal);
			sqlCommand.Parameters.AddWithValue("EditLagerStock", item.EditLagerStock == null ? (object)DBNull.Value : item.EditLagerStock);
			sqlCommand.Parameters.AddWithValue("EditPayementPractises", item.EditPayementPractises == null ? (object)DBNull.Value : item.EditPayementPractises);
			sqlCommand.Parameters.AddWithValue("EditPricingGroup", item.EditPricingGroup == null ? (object)DBNull.Value : item.EditPricingGroup);
			sqlCommand.Parameters.AddWithValue("EditRohArtikelNummer", item.EditRohArtikelNummer == null ? (object)DBNull.Value : item.EditRohArtikelNummer);
			sqlCommand.Parameters.AddWithValue("EditShippingMethods", item.EditShippingMethods == null ? (object)DBNull.Value : item.EditShippingMethods);
			sqlCommand.Parameters.AddWithValue("EditSlipCircle", item.EditSlipCircle == null ? (object)DBNull.Value : item.EditSlipCircle);
			sqlCommand.Parameters.AddWithValue("EditSupplier", item.EditSupplier == null ? (object)DBNull.Value : item.EditSupplier);
			sqlCommand.Parameters.AddWithValue("EditSupplierAddress", item.EditSupplierAddress == null ? (object)DBNull.Value : item.EditSupplierAddress);
			sqlCommand.Parameters.AddWithValue("EditSupplierCommunication", item.EditSupplierCommunication == null ? (object)DBNull.Value : item.EditSupplierCommunication);
			sqlCommand.Parameters.AddWithValue("EditSupplierContactPerson", item.EditSupplierContactPerson == null ? (object)DBNull.Value : item.EditSupplierContactPerson);
			sqlCommand.Parameters.AddWithValue("EditSupplierCoordination", item.EditSupplierCoordination == null ? (object)DBNull.Value : item.EditSupplierCoordination);
			sqlCommand.Parameters.AddWithValue("EditSupplierData", item.EditSupplierData == null ? (object)DBNull.Value : item.EditSupplierData);
			sqlCommand.Parameters.AddWithValue("EditSupplierGroup", item.EditSupplierGroup == null ? (object)DBNull.Value : item.EditSupplierGroup);
			sqlCommand.Parameters.AddWithValue("EditSupplierImage", item.EditSupplierImage == null ? (object)DBNull.Value : item.EditSupplierImage);
			sqlCommand.Parameters.AddWithValue("EditSupplierShipping", item.EditSupplierShipping == null ? (object)DBNull.Value : item.EditSupplierShipping);
			sqlCommand.Parameters.AddWithValue("EditTermsOfPayment", item.EditTermsOfPayment == null ? (object)DBNull.Value : item.EditTermsOfPayment);
			sqlCommand.Parameters.AddWithValue("EDrawingEdit", item.EDrawingEdit == null ? (object)DBNull.Value : item.EDrawingEdit);
			sqlCommand.Parameters.AddWithValue("EinkaufsPreisUpdate", item.EinkaufsPreisUpdate == null ? (object)DBNull.Value : item.EinkaufsPreisUpdate);
			sqlCommand.Parameters.AddWithValue("FibuFrame", item.FibuFrame == null ? (object)DBNull.Value : item.FibuFrame);
			sqlCommand.Parameters.AddWithValue("GetRohArtikelNummer", item.GetRohArtikelNummer == null ? (object)DBNull.Value : item.GetRohArtikelNummer);
			sqlCommand.Parameters.AddWithValue("HourlyRate", item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
			sqlCommand.Parameters.AddWithValue("ImportArticleBOM", item.ImportArticleBOM);
			sqlCommand.Parameters.AddWithValue("Industry", item.Industry == null ? (object)DBNull.Value : item.Industry);
			sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
			sqlCommand.Parameters.AddWithValue("LagerArticleLogistics", item.LagerArticleLogistics);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("ModuleAdministrator", item.ModuleAdministrator == null ? (object)DBNull.Value : item.ModuleAdministrator);
			sqlCommand.Parameters.AddWithValue("offer", item.offer == null ? (object)DBNull.Value : item.offer);
			sqlCommand.Parameters.AddWithValue("OfferRequestADD", item.OfferRequestADD == null ? (object)DBNull.Value : item.OfferRequestADD);
			sqlCommand.Parameters.AddWithValue("OfferRequestApplyPrice", item.OfferRequestApplyPrice == null ? (object)DBNull.Value : item.OfferRequestApplyPrice);
			sqlCommand.Parameters.AddWithValue("OfferRequestDelete", item.OfferRequestDelete == null ? (object)DBNull.Value : item.OfferRequestDelete);
			sqlCommand.Parameters.AddWithValue("OfferRequestEdit", item.OfferRequestEdit == null ? (object)DBNull.Value : item.OfferRequestEdit);
			sqlCommand.Parameters.AddWithValue("OfferRequestEditEmail", item.OfferRequestEditEmail == null ? (object)DBNull.Value : item.OfferRequestEditEmail);
			sqlCommand.Parameters.AddWithValue("OfferRequestSendEmail", item.OfferRequestSendEmail == null ? (object)DBNull.Value : item.OfferRequestSendEmail);
			sqlCommand.Parameters.AddWithValue("OfferRequestView", item.OfferRequestView == null ? (object)DBNull.Value : item.OfferRequestView);
			sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoAdd", item.PackagingsLgtPhotoAdd == null ? (object)DBNull.Value : item.PackagingsLgtPhotoAdd);
			sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoDelete", item.PackagingsLgtPhotoDelete == null ? (object)DBNull.Value : item.PackagingsLgtPhotoDelete);
			sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoView", item.PackagingsLgtPhotoView == null ? (object)DBNull.Value : item.PackagingsLgtPhotoView);
			sqlCommand.Parameters.AddWithValue("PayementPractises", item.PayementPractises == null ? (object)DBNull.Value : item.PayementPractises);
			sqlCommand.Parameters.AddWithValue("PMAddCable", item.PMAddCable == null ? (object)DBNull.Value : item.PMAddCable);
			sqlCommand.Parameters.AddWithValue("PMAddMileStone", item.PMAddMileStone == null ? (object)DBNull.Value : item.PMAddMileStone);
			sqlCommand.Parameters.AddWithValue("PMAddProject", item.PMAddProject == null ? (object)DBNull.Value : item.PMAddProject);
			sqlCommand.Parameters.AddWithValue("PMDeleteCable", item.PMDeleteCable == null ? (object)DBNull.Value : item.PMDeleteCable);
			sqlCommand.Parameters.AddWithValue("PMDeleteMileStone", item.PMDeleteMileStone == null ? (object)DBNull.Value : item.PMDeleteMileStone);
			sqlCommand.Parameters.AddWithValue("PMDeleteProject", item.PMDeleteProject == null ? (object)DBNull.Value : item.PMDeleteProject);
			sqlCommand.Parameters.AddWithValue("PMEditCable", item.PMEditCable == null ? (object)DBNull.Value : item.PMEditCable);
			sqlCommand.Parameters.AddWithValue("PMEditMileStone", item.PMEditMileStone == null ? (object)DBNull.Value : item.PMEditMileStone);
			sqlCommand.Parameters.AddWithValue("PMEditProject", item.PMEditProject == null ? (object)DBNull.Value : item.PMEditProject);
			sqlCommand.Parameters.AddWithValue("PMModule", item.PMModule == null ? (object)DBNull.Value : item.PMModule);
			sqlCommand.Parameters.AddWithValue("PMViewProjectsCompact", item.PMViewProjectsCompact == null ? (object)DBNull.Value : item.PMViewProjectsCompact);
			sqlCommand.Parameters.AddWithValue("PMViewProjectsDetail", item.PMViewProjectsDetail == null ? (object)DBNull.Value : item.PMViewProjectsDetail);
			sqlCommand.Parameters.AddWithValue("PMViewProjectsMedium", item.PMViewProjectsMedium == null ? (object)DBNull.Value : item.PMViewProjectsMedium);
			sqlCommand.Parameters.AddWithValue("PricingGroup", item.PricingGroup == null ? (object)DBNull.Value : item.PricingGroup);
			sqlCommand.Parameters.AddWithValue("RemoveArticleReference", item.RemoveArticleReference == null ? (object)DBNull.Value : item.RemoveArticleReference);
			sqlCommand.Parameters.AddWithValue("Settings", item.Settings == null ? (object)DBNull.Value : item.Settings);
			sqlCommand.Parameters.AddWithValue("ShippingMethods", item.ShippingMethods == null ? (object)DBNull.Value : item.ShippingMethods);
			sqlCommand.Parameters.AddWithValue("SlipCircle", item.SlipCircle == null ? (object)DBNull.Value : item.SlipCircle);
			sqlCommand.Parameters.AddWithValue("SupplierAddress", item.SupplierAddress == null ? (object)DBNull.Value : item.SupplierAddress);
			sqlCommand.Parameters.AddWithValue("SupplierAttachementAddFile", item.SupplierAttachementAddFile == null ? (object)DBNull.Value : item.SupplierAttachementAddFile);
			sqlCommand.Parameters.AddWithValue("SupplierAttachementGetFile", item.SupplierAttachementGetFile == null ? (object)DBNull.Value : item.SupplierAttachementGetFile);
			sqlCommand.Parameters.AddWithValue("SupplierAttachementRemoveFile", item.SupplierAttachementRemoveFile == null ? (object)DBNull.Value : item.SupplierAttachementRemoveFile);
			sqlCommand.Parameters.AddWithValue("SupplierCommunication", item.SupplierCommunication == null ? (object)DBNull.Value : item.SupplierCommunication);
			sqlCommand.Parameters.AddWithValue("SupplierContactPerson", item.SupplierContactPerson == null ? (object)DBNull.Value : item.SupplierContactPerson);
			sqlCommand.Parameters.AddWithValue("SupplierData", item.SupplierData == null ? (object)DBNull.Value : item.SupplierData);
			sqlCommand.Parameters.AddWithValue("SupplierGroup", item.SupplierGroup == null ? (object)DBNull.Value : item.SupplierGroup);
			sqlCommand.Parameters.AddWithValue("SupplierHistory", item.SupplierHistory == null ? (object)DBNull.Value : item.SupplierHistory);
			sqlCommand.Parameters.AddWithValue("SupplierOverview", item.SupplierOverview == null ? (object)DBNull.Value : item.SupplierOverview);
			sqlCommand.Parameters.AddWithValue("Suppliers", item.Suppliers);
			sqlCommand.Parameters.AddWithValue("SupplierShipping", item.SupplierShipping == null ? (object)DBNull.Value : item.SupplierShipping);
			sqlCommand.Parameters.AddWithValue("TermsOfPayment", item.TermsOfPayment == null ? (object)DBNull.Value : item.TermsOfPayment);
			sqlCommand.Parameters.AddWithValue("UploadAltPositionsArticleBOM", item.UploadAltPositionsArticleBOM);
			sqlCommand.Parameters.AddWithValue("UploadArticleBOM", item.UploadArticleBOM);
			sqlCommand.Parameters.AddWithValue("ValidateArticleBOM", item.ValidateArticleBOM == null ? (object)DBNull.Value : item.ValidateArticleBOM);
			sqlCommand.Parameters.AddWithValue("ValidateBCR", item.ValidateBCR == null ? (object)DBNull.Value : item.ValidateBCR);
			sqlCommand.Parameters.AddWithValue("ViewAltPositionsArticleBOM", item.ViewAltPositionsArticleBOM);
			sqlCommand.Parameters.AddWithValue("ViewArticleLog", item.ViewArticleLog);
			sqlCommand.Parameters.AddWithValue("ViewArticleReference", item.ViewArticleReference == null ? (object)DBNull.Value : item.ViewArticleReference);
			sqlCommand.Parameters.AddWithValue("ViewArticles", item.ViewArticles);
			sqlCommand.Parameters.AddWithValue("ViewBCR", item.ViewBCR == null ? (object)DBNull.Value : item.ViewBCR);
			sqlCommand.Parameters.AddWithValue("ViewCustomers", item.ViewCustomers == null ? (object)DBNull.Value : item.ViewCustomers);
			sqlCommand.Parameters.AddWithValue("ViewLPCustomer", item.ViewLPCustomer == null ? (object)DBNull.Value : item.ViewLPCustomer);
			sqlCommand.Parameters.AddWithValue("ViewLPSupplier", item.ViewLPSupplier == null ? (object)DBNull.Value : item.ViewLPSupplier);
			sqlCommand.Parameters.AddWithValue("ViewSupplierAddressComments", item.ViewSupplierAddressComments == null ? (object)DBNull.Value : item.ViewSupplierAddressComments);
			sqlCommand.Parameters.AddWithValue("ViewSuppliers", item.ViewSuppliers == null ? (object)DBNull.Value : item.ViewSuppliers);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Entities.Tables.BSD.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 226; // Nb params per query
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
		private static int insertWithTransaction(List<Entities.Tables.BSD.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_AccessProfile] ([AccessProfileName],[AddArticle],[AddArticleReference],[AddBCR],[AddCocType],[AddConditionAssignment],[AddContactAddress],[AddContactSalutation],[AddCurrencies],[AddCustomer],[AddCustomerGroup],[AddCustomerItemNumber],[AddDiscountGroup],[AddEdiConcern],[AddFibuFrame],[AddFiles],[AddHourlyRate],[AddIndustry],[AddPayementPractises],[AddPricingGroup],[AddRohArtikelNummer],[AddShippingMethods],[AddSlipCircle],[AddSupplier],[AddSupplierGroup],[AddTermsOfPayment],[Administration],[AltPositionsArticleBOM],[ArchiveArticle],[ArchiveCustomer],[ArchiveSupplier],[ArticleAddCustomerDocument],[ArticleBOM],[ArticleCts],[ArticleData],[ArticleDeleteCustomerDocument],[ArticleLogistics],[ArticleLogisticsPrices],[ArticleOverview],[ArticleProduction],[ArticlePurchase],[ArticleQuality],[Articles],[ArticleSales],[ArticleSalesCustom],[ArticleSalesItem],[ArticlesBOMCPControlEngineering],[ArticlesBOMCPControlHistory],[ArticlesBOMCPControlQuality],[ArticleStatistics],[ArticleStatisticsEngineering],[ArticleStatisticsEngineeringEdit],[ArticleStatisticsFinanceAccounting],[ArticleStatisticsFinanceAccountingEdit],[ArticleStatisticsLogistics],[ArticleStatisticsLogisticsEdit],[ArticleStatisticsPurchase],[ArticleStatisticsPurchaseEdit],[ArticleStatisticsTechnic],[ArticleStatisticsTechnicEdit],[BomChangeRequests],[CocType],[ConditionAssignment],[ConfigArticle],[ConfigCustomer],[ConfigSupplier],[ContactAddress],[ContactSalutation],[CreateArticlePurchase],[CreateArticleSalesCustom],[CreateArticleSalesItem],[CreateCustomerContactPerson],[CreateSupplierContactPerson],[CreationTime],[CreationUserId],[Currencies],[CustomerAddress],[CustomerCommunication],[CustomerContactPerson],[CustomerData],[CustomerGroup],[CustomerHistory],[CustomerItemNumber],[CustomerOverview],[Customers],[CustomerShipping],[DeleteAltPositionsArticleBOM],[DeleteArticle],[DeleteArticleBOM],[DeleteArticlePurchase],[DeleteArticleSalesCustom],[DeleteArticleSalesItem],[DeleteBCR],[DeleteCustomerContactPerson],[DeleteFiles],[DeleteRohArtikelNummer],[DeleteSupplierContactPerson],[DiscountGroup],[DownloadAllOutdatedEinkaufsPreis],[DownloadFiles],[DownloadOutdatedEinkaufsPreis],[EdiConcern],[EditAltPositionsArticleBOM],[EditArticle],[EditArticleBOM],[EditArticleBOMPosition],[EditArticleCts],[EditArticleData],[EditArticleDesignation],[EditArticleImage],[EditArticleLogistics],[EditArticleManager],[EditArticleProduction],[EditArticlePurchase],[EditArticleQuality],[EditArticleReference],[EditArticleSalesCustom],[EditArticleSalesItem],[EditCocType],[EditConditionAssignment],[EditContactAddress],[EditContactSalutation],[EditCurrencies],[EditCustomer],[EditCustomerAddress],[EditCustomerCommunication],[EditCustomerContactPerson],[EditCustomerCoordination],[EditCustomerData],[EditCustomerGroup],[EditCustomerImage],[EditCustomerItemNumber],[EditCustomerShipping],[EditDiscountGroup],[EditEdiConcern],[EditFibuFrame],[EditHourlyRate],[EditIndustry],[EditLagerCCID],[EditLagerMinStock],[EditLagerOrderProposal],[EditLagerStock],[EditPayementPractises],[EditPricingGroup],[EditRohArtikelNummer],[EditShippingMethods],[EditSlipCircle],[EditSupplier],[EditSupplierAddress],[EditSupplierCommunication],[EditSupplierContactPerson],[EditSupplierCoordination],[EditSupplierData],[EditSupplierGroup],[EditSupplierImage],[EditSupplierShipping],[EditTermsOfPayment],[EDrawingEdit],[EinkaufsPreisUpdate],[FibuFrame],[GetRohArtikelNummer],[HourlyRate],[ImportArticleBOM],[Industry],[isDefault],[LagerArticleLogistics],[ModuleActivated],[ModuleAdministrator],[offer],[OfferRequestADD],[OfferRequestApplyPrice],[OfferRequestDelete],[OfferRequestEdit],[OfferRequestEditEmail],[OfferRequestSendEmail],[OfferRequestView],[PackagingsLgtPhotoAdd],[PackagingsLgtPhotoDelete],[PackagingsLgtPhotoView],[PayementPractises],[PMAddCable],[PMAddMileStone],[PMAddProject],[PMDeleteCable],[PMDeleteMileStone],[PMDeleteProject],[PMEditCable],[PMEditMileStone],[PMEditProject],[PMModule],[PMViewProjectsCompact],[PMViewProjectsDetail],[PMViewProjectsMedium],[PricingGroup],[RemoveArticleReference],[Settings],[ShippingMethods],[SlipCircle],[SupplierAddress],[SupplierAttachementAddFile],[SupplierAttachementGetFile],[SupplierAttachementRemoveFile],[SupplierCommunication],[SupplierContactPerson],[SupplierData],[SupplierGroup],[SupplierHistory],[SupplierOverview],[Suppliers],[SupplierShipping],[TermsOfPayment],[UploadAltPositionsArticleBOM],[UploadArticleBOM],[ValidateArticleBOM],[ValidateBCR],[ViewAltPositionsArticleBOM],[ViewArticleLog],[ViewArticleReference],[ViewArticles],[ViewBCR],[ViewCustomers],[ViewLPCustomer],[ViewLPSupplier],[ViewSupplierAddressComments],[ViewSuppliers]) VALUES ( "

						+ "@AccessProfileName" + i + ","
						+ "@AddArticle" + i + ","
						+ "@AddArticleReference" + i + ","
						+ "@AddBCR" + i + ","
						+ "@AddCocType" + i + ","
						+ "@AddConditionAssignment" + i + ","
						+ "@AddContactAddress" + i + ","
						+ "@AddContactSalutation" + i + ","
						+ "@AddCurrencies" + i + ","
						+ "@AddCustomer" + i + ","
						+ "@AddCustomerGroup" + i + ","
						+ "@AddCustomerItemNumber" + i + ","
						+ "@AddDiscountGroup" + i + ","
						+ "@AddEdiConcern" + i + ","
						+ "@AddFibuFrame" + i + ","
						+ "@AddFiles" + i + ","
						+ "@AddHourlyRate" + i + ","
						+ "@AddIndustry" + i + ","
						+ "@AddPayementPractises" + i + ","
						+ "@AddPricingGroup" + i + ","
						+ "@AddRohArtikelNummer" + i + ","
						+ "@AddShippingMethods" + i + ","
						+ "@AddSlipCircle" + i + ","
						+ "@AddSupplier" + i + ","
						+ "@AddSupplierGroup" + i + ","
						+ "@AddTermsOfPayment" + i + ","
						+ "@Administration" + i + ","
						+ "@AltPositionsArticleBOM" + i + ","
						+ "@ArchiveArticle" + i + ","
						+ "@ArchiveCustomer" + i + ","
						+ "@ArchiveSupplier" + i + ","
						+ "@ArticleAddCustomerDocument" + i + ","
						+ "@ArticleBOM" + i + ","
						+ "@ArticleCts" + i + ","
						+ "@ArticleData" + i + ","
						+ "@ArticleDeleteCustomerDocument" + i + ","
						+ "@ArticleLogistics" + i + ","
						+ "@ArticleLogisticsPrices" + i + ","
						+ "@ArticleOverview" + i + ","
						+ "@ArticleProduction" + i + ","
						+ "@ArticlePurchase" + i + ","
						+ "@ArticleQuality" + i + ","
						+ "@Articles" + i + ","
						+ "@ArticleSales" + i + ","
						+ "@ArticleSalesCustom" + i + ","
						+ "@ArticleSalesItem" + i + ","
						+ "@ArticlesBOMCPControlEngineering" + i + ","
						+ "@ArticlesBOMCPControlHistory" + i + ","
						+ "@ArticlesBOMCPControlQuality" + i + ","
						+ "@ArticleStatistics" + i + ","
						+ "@ArticleStatisticsEngineering" + i + ","
						+ "@ArticleStatisticsEngineeringEdit" + i + ","
						+ "@ArticleStatisticsFinanceAccounting" + i + ","
						+ "@ArticleStatisticsFinanceAccountingEdit" + i + ","
						+ "@ArticleStatisticsLogistics" + i + ","
						+ "@ArticleStatisticsLogisticsEdit" + i + ","
						+ "@ArticleStatisticsPurchase" + i + ","
						+ "@ArticleStatisticsPurchaseEdit" + i + ","
						+ "@ArticleStatisticsTechnic" + i + ","
						+ "@ArticleStatisticsTechnicEdit" + i + ","
						+ "@BomChangeRequests" + i + ","
						+ "@CocType" + i + ","
						+ "@ConditionAssignment" + i + ","
						+ "@ConfigArticle" + i + ","
						+ "@ConfigCustomer" + i + ","
						+ "@ConfigSupplier" + i + ","
						+ "@ContactAddress" + i + ","
						+ "@ContactSalutation" + i + ","
						+ "@CreateArticlePurchase" + i + ","
						+ "@CreateArticleSalesCustom" + i + ","
						+ "@CreateArticleSalesItem" + i + ","
						+ "@CreateCustomerContactPerson" + i + ","
						+ "@CreateSupplierContactPerson" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@Currencies" + i + ","
						+ "@CustomerAddress" + i + ","
						+ "@CustomerCommunication" + i + ","
						+ "@CustomerContactPerson" + i + ","
						+ "@CustomerData" + i + ","
						+ "@CustomerGroup" + i + ","
						+ "@CustomerHistory" + i + ","
						+ "@CustomerItemNumber" + i + ","
						+ "@CustomerOverview" + i + ","
						+ "@Customers" + i + ","
						+ "@CustomerShipping" + i + ","
						+ "@DeleteAltPositionsArticleBOM" + i + ","
						+ "@DeleteArticle" + i + ","
						+ "@DeleteArticleBOM" + i + ","
						+ "@DeleteArticlePurchase" + i + ","
						+ "@DeleteArticleSalesCustom" + i + ","
						+ "@DeleteArticleSalesItem" + i + ","
						+ "@DeleteBCR" + i + ","
						+ "@DeleteCustomerContactPerson" + i + ","
						+ "@DeleteFiles" + i + ","
						+ "@DeleteRohArtikelNummer" + i + ","
						+ "@DeleteSupplierContactPerson" + i + ","
						+ "@DiscountGroup" + i + ","
						+ "@DownloadAllOutdatedEinkaufsPreis" + i + ","
						+ "@DownloadFiles" + i + ","
						+ "@DownloadOutdatedEinkaufsPreis" + i + ","
						+ "@EdiConcern" + i + ","
						+ "@EditAltPositionsArticleBOM" + i + ","
						+ "@EditArticle" + i + ","
						+ "@EditArticleBOM" + i + ","
						+ "@EditArticleBOMPosition" + i + ","
						+ "@EditArticleCts" + i + ","
						+ "@EditArticleData" + i + ","
						+ "@EditArticleDesignation" + i + ","
						+ "@EditArticleImage" + i + ","
						+ "@EditArticleLogistics" + i + ","
						+ "@EditArticleManager" + i + ","
						+ "@EditArticleProduction" + i + ","
						+ "@EditArticlePurchase" + i + ","
						+ "@EditArticleQuality" + i + ","
						+ "@EditArticleReference" + i + ","
						+ "@EditArticleSalesCustom" + i + ","
						+ "@EditArticleSalesItem" + i + ","
						+ "@EditCocType" + i + ","
						+ "@EditConditionAssignment" + i + ","
						+ "@EditContactAddress" + i + ","
						+ "@EditContactSalutation" + i + ","
						+ "@EditCurrencies" + i + ","
						+ "@EditCustomer" + i + ","
						+ "@EditCustomerAddress" + i + ","
						+ "@EditCustomerCommunication" + i + ","
						+ "@EditCustomerContactPerson" + i + ","
						+ "@EditCustomerCoordination" + i + ","
						+ "@EditCustomerData" + i + ","
						+ "@EditCustomerGroup" + i + ","
						+ "@EditCustomerImage" + i + ","
						+ "@EditCustomerItemNumber" + i + ","
						+ "@EditCustomerShipping" + i + ","
						+ "@EditDiscountGroup" + i + ","
						+ "@EditEdiConcern" + i + ","
						+ "@EditFibuFrame" + i + ","
						+ "@EditHourlyRate" + i + ","
						+ "@EditIndustry" + i + ","
						+ "@EditLagerCCID" + i + ","
						+ "@EditLagerMinStock" + i + ","
						+ "@EditLagerOrderProposal" + i + ","
						+ "@EditLagerStock" + i + ","
						+ "@EditPayementPractises" + i + ","
						+ "@EditPricingGroup" + i + ","
						+ "@EditRohArtikelNummer" + i + ","
						+ "@EditShippingMethods" + i + ","
						+ "@EditSlipCircle" + i + ","
						+ "@EditSupplier" + i + ","
						+ "@EditSupplierAddress" + i + ","
						+ "@EditSupplierCommunication" + i + ","
						+ "@EditSupplierContactPerson" + i + ","
						+ "@EditSupplierCoordination" + i + ","
						+ "@EditSupplierData" + i + ","
						+ "@EditSupplierGroup" + i + ","
						+ "@EditSupplierImage" + i + ","
						+ "@EditSupplierShipping" + i + ","
						+ "@EditTermsOfPayment" + i + ","
						+ "@EDrawingEdit" + i + ","
						+ "@EinkaufsPreisUpdate" + i + ","
						+ "@FibuFrame" + i + ","
						+ "@GetRohArtikelNummer" + i + ","
						+ "@HourlyRate" + i + ","
						+ "@ImportArticleBOM" + i + ","
						+ "@Industry" + i + ","
						+ "@isDefault" + i + ","
						+ "@LagerArticleLogistics" + i + ","
						+ "@ModuleActivated" + i + ","
						+ "@ModuleAdministrator" + i + ","
						+ "@offer" + i + ","
						+ "@OfferRequestADD" + i + ","
						+ "@OfferRequestApplyPrice" + i + ","
						+ "@OfferRequestDelete" + i + ","
						+ "@OfferRequestEdit" + i + ","
						+ "@OfferRequestEditEmail" + i + ","
						+ "@OfferRequestSendEmail" + i + ","
						+ "@OfferRequestView" + i + ","
						+ "@PackagingsLgtPhotoAdd" + i + ","
						+ "@PackagingsLgtPhotoDelete" + i + ","
						+ "@PackagingsLgtPhotoView" + i + ","
						+ "@PayementPractises" + i + ","
						+ "@PMAddCable" + i + ","
						+ "@PMAddMileStone" + i + ","
						+ "@PMAddProject" + i + ","
						+ "@PMDeleteCable" + i + ","
						+ "@PMDeleteMileStone" + i + ","
						+ "@PMDeleteProject" + i + ","
						+ "@PMEditCable" + i + ","
						+ "@PMEditMileStone" + i + ","
						+ "@PMEditProject" + i + ","
						+ "@PMModule" + i + ","
						+ "@PMViewProjectsCompact" + i + ","
						+ "@PMViewProjectsDetail" + i + ","
						+ "@PMViewProjectsMedium" + i + ","
						+ "@PricingGroup" + i + ","
						+ "@RemoveArticleReference" + i + ","
						+ "@Settings" + i + ","
						+ "@ShippingMethods" + i + ","
						+ "@SlipCircle" + i + ","
						+ "@SupplierAddress" + i + ","
						+ "@SupplierAttachementAddFile" + i + ","
						+ "@SupplierAttachementGetFile" + i + ","
						+ "@SupplierAttachementRemoveFile" + i + ","
						+ "@SupplierCommunication" + i + ","
						+ "@SupplierContactPerson" + i + ","
						+ "@SupplierData" + i + ","
						+ "@SupplierGroup" + i + ","
						+ "@SupplierHistory" + i + ","
						+ "@SupplierOverview" + i + ","
						+ "@Suppliers" + i + ","
						+ "@SupplierShipping" + i + ","
						+ "@TermsOfPayment" + i + ","
						+ "@UploadAltPositionsArticleBOM" + i + ","
						+ "@UploadArticleBOM" + i + ","
						+ "@ValidateArticleBOM" + i + ","
						+ "@ValidateBCR" + i + ","
						+ "@ViewAltPositionsArticleBOM" + i + ","
						+ "@ViewArticleLog" + i + ","
						+ "@ViewArticleReference" + i + ","
						+ "@ViewArticles" + i + ","
						+ "@ViewBCR" + i + ","
						+ "@ViewCustomers" + i + ","
						+ "@ViewLPCustomer" + i + ","
						+ "@ViewLPSupplier" + i + ","
						+ "@ViewSupplierAddressComments" + i + ","
						+ "@ViewSuppliers" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("AddArticle" + i, item.AddArticle);
					sqlCommand.Parameters.AddWithValue("AddArticleReference" + i, item.AddArticleReference == null ? (object)DBNull.Value : item.AddArticleReference);
					sqlCommand.Parameters.AddWithValue("AddBCR" + i, item.AddBCR == null ? (object)DBNull.Value : item.AddBCR);
					sqlCommand.Parameters.AddWithValue("AddCocType" + i, item.AddCocType == null ? (object)DBNull.Value : item.AddCocType);
					sqlCommand.Parameters.AddWithValue("AddConditionAssignment" + i, item.AddConditionAssignment == null ? (object)DBNull.Value : item.AddConditionAssignment);
					sqlCommand.Parameters.AddWithValue("AddContactAddress" + i, item.AddContactAddress == null ? (object)DBNull.Value : item.AddContactAddress);
					sqlCommand.Parameters.AddWithValue("AddContactSalutation" + i, item.AddContactSalutation == null ? (object)DBNull.Value : item.AddContactSalutation);
					sqlCommand.Parameters.AddWithValue("AddCurrencies" + i, item.AddCurrencies == null ? (object)DBNull.Value : item.AddCurrencies);
					sqlCommand.Parameters.AddWithValue("AddCustomer" + i, item.AddCustomer == null ? (object)DBNull.Value : item.AddCustomer);
					sqlCommand.Parameters.AddWithValue("AddCustomerGroup" + i, item.AddCustomerGroup == null ? (object)DBNull.Value : item.AddCustomerGroup);
					sqlCommand.Parameters.AddWithValue("AddCustomerItemNumber" + i, item.AddCustomerItemNumber == null ? (object)DBNull.Value : item.AddCustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("AddDiscountGroup" + i, item.AddDiscountGroup == null ? (object)DBNull.Value : item.AddDiscountGroup);
					sqlCommand.Parameters.AddWithValue("AddEdiConcern" + i, item.AddEdiConcern == null ? (object)DBNull.Value : item.AddEdiConcern);
					sqlCommand.Parameters.AddWithValue("AddFibuFrame" + i, item.AddFibuFrame == null ? (object)DBNull.Value : item.AddFibuFrame);
					sqlCommand.Parameters.AddWithValue("AddFiles" + i, item.AddFiles == null ? (object)DBNull.Value : item.AddFiles);
					sqlCommand.Parameters.AddWithValue("AddHourlyRate" + i, item.AddHourlyRate == null ? (object)DBNull.Value : item.AddHourlyRate);
					sqlCommand.Parameters.AddWithValue("AddIndustry" + i, item.AddIndustry == null ? (object)DBNull.Value : item.AddIndustry);
					sqlCommand.Parameters.AddWithValue("AddPayementPractises" + i, item.AddPayementPractises == null ? (object)DBNull.Value : item.AddPayementPractises);
					sqlCommand.Parameters.AddWithValue("AddPricingGroup" + i, item.AddPricingGroup == null ? (object)DBNull.Value : item.AddPricingGroup);
					sqlCommand.Parameters.AddWithValue("AddRohArtikelNummer" + i, item.AddRohArtikelNummer == null ? (object)DBNull.Value : item.AddRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("AddShippingMethods" + i, item.AddShippingMethods == null ? (object)DBNull.Value : item.AddShippingMethods);
					sqlCommand.Parameters.AddWithValue("AddSlipCircle" + i, item.AddSlipCircle == null ? (object)DBNull.Value : item.AddSlipCircle);
					sqlCommand.Parameters.AddWithValue("AddSupplier" + i, item.AddSupplier == null ? (object)DBNull.Value : item.AddSupplier);
					sqlCommand.Parameters.AddWithValue("AddSupplierGroup" + i, item.AddSupplierGroup == null ? (object)DBNull.Value : item.AddSupplierGroup);
					sqlCommand.Parameters.AddWithValue("AddTermsOfPayment" + i, item.AddTermsOfPayment == null ? (object)DBNull.Value : item.AddTermsOfPayment);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration);
					sqlCommand.Parameters.AddWithValue("AltPositionsArticleBOM" + i, item.AltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("ArchiveArticle" + i, item.ArchiveArticle);
					sqlCommand.Parameters.AddWithValue("ArchiveCustomer" + i, item.ArchiveCustomer == null ? (object)DBNull.Value : item.ArchiveCustomer);
					sqlCommand.Parameters.AddWithValue("ArchiveSupplier" + i, item.ArchiveSupplier == null ? (object)DBNull.Value : item.ArchiveSupplier);
					sqlCommand.Parameters.AddWithValue("ArticleAddCustomerDocument" + i, item.ArticleAddCustomerDocument == null ? (object)DBNull.Value : item.ArticleAddCustomerDocument);
					sqlCommand.Parameters.AddWithValue("ArticleBOM" + i, item.ArticleBOM);
					sqlCommand.Parameters.AddWithValue("ArticleCts" + i, item.ArticleCts);
					sqlCommand.Parameters.AddWithValue("ArticleData" + i, item.ArticleData);
					sqlCommand.Parameters.AddWithValue("ArticleDeleteCustomerDocument" + i, item.ArticleDeleteCustomerDocument == null ? (object)DBNull.Value : item.ArticleDeleteCustomerDocument);
					sqlCommand.Parameters.AddWithValue("ArticleLogistics" + i, item.ArticleLogistics);
					sqlCommand.Parameters.AddWithValue("ArticleLogisticsPrices" + i, item.ArticleLogisticsPrices == null ? (object)DBNull.Value : item.ArticleLogisticsPrices);
					sqlCommand.Parameters.AddWithValue("ArticleOverview" + i, item.ArticleOverview);
					sqlCommand.Parameters.AddWithValue("ArticleProduction" + i, item.ArticleProduction);
					sqlCommand.Parameters.AddWithValue("ArticlePurchase" + i, item.ArticlePurchase);
					sqlCommand.Parameters.AddWithValue("ArticleQuality" + i, item.ArticleQuality);
					sqlCommand.Parameters.AddWithValue("Articles" + i, item.Articles);
					sqlCommand.Parameters.AddWithValue("ArticleSales" + i, item.ArticleSales);
					sqlCommand.Parameters.AddWithValue("ArticleSalesCustom" + i, item.ArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("ArticleSalesItem" + i, item.ArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlEngineering" + i, item.ArticlesBOMCPControlEngineering == null ? (object)DBNull.Value : item.ArticlesBOMCPControlEngineering);
					sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlHistory" + i, item.ArticlesBOMCPControlHistory == null ? (object)DBNull.Value : item.ArticlesBOMCPControlHistory);
					sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlQuality" + i, item.ArticlesBOMCPControlQuality == null ? (object)DBNull.Value : item.ArticlesBOMCPControlQuality);
					sqlCommand.Parameters.AddWithValue("ArticleStatistics" + i, item.ArticleStatistics);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineering" + i, item.ArticleStatisticsEngineering);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineeringEdit" + i, item.ArticleStatisticsEngineeringEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccounting" + i, item.ArticleStatisticsFinanceAccounting);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccountingEdit" + i, item.ArticleStatisticsFinanceAccountingEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogistics" + i, item.ArticleStatisticsLogistics);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogisticsEdit" + i, item.ArticleStatisticsLogisticsEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchase" + i, item.ArticleStatisticsPurchase);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchaseEdit" + i, item.ArticleStatisticsPurchaseEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnic" + i, item.ArticleStatisticsTechnic);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnicEdit" + i, item.ArticleStatisticsTechnicEdit);
					sqlCommand.Parameters.AddWithValue("BomChangeRequests" + i, item.BomChangeRequests == null ? (object)DBNull.Value : item.BomChangeRequests);
					sqlCommand.Parameters.AddWithValue("CocType" + i, item.CocType == null ? (object)DBNull.Value : item.CocType);
					sqlCommand.Parameters.AddWithValue("ConditionAssignment" + i, item.ConditionAssignment == null ? (object)DBNull.Value : item.ConditionAssignment);
					sqlCommand.Parameters.AddWithValue("ConfigArticle" + i, item.ConfigArticle);
					sqlCommand.Parameters.AddWithValue("ConfigCustomer" + i, item.ConfigCustomer == null ? (object)DBNull.Value : item.ConfigCustomer);
					sqlCommand.Parameters.AddWithValue("ConfigSupplier" + i, item.ConfigSupplier == null ? (object)DBNull.Value : item.ConfigSupplier);
					sqlCommand.Parameters.AddWithValue("ContactAddress" + i, item.ContactAddress == null ? (object)DBNull.Value : item.ContactAddress);
					sqlCommand.Parameters.AddWithValue("ContactSalutation" + i, item.ContactSalutation == null ? (object)DBNull.Value : item.ContactSalutation);
					sqlCommand.Parameters.AddWithValue("CreateArticlePurchase" + i, item.CreateArticlePurchase);
					sqlCommand.Parameters.AddWithValue("CreateArticleSalesCustom" + i, item.CreateArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("CreateArticleSalesItem" + i, item.CreateArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("CreateCustomerContactPerson" + i, item.CreateCustomerContactPerson == null ? (object)DBNull.Value : item.CreateCustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("CreateSupplierContactPerson" + i, item.CreateSupplierContactPerson == null ? (object)DBNull.Value : item.CreateSupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Currencies" + i, item.Currencies == null ? (object)DBNull.Value : item.Currencies);
					sqlCommand.Parameters.AddWithValue("CustomerAddress" + i, item.CustomerAddress == null ? (object)DBNull.Value : item.CustomerAddress);
					sqlCommand.Parameters.AddWithValue("CustomerCommunication" + i, item.CustomerCommunication == null ? (object)DBNull.Value : item.CustomerCommunication);
					sqlCommand.Parameters.AddWithValue("CustomerContactPerson" + i, item.CustomerContactPerson == null ? (object)DBNull.Value : item.CustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("CustomerData" + i, item.CustomerData == null ? (object)DBNull.Value : item.CustomerData);
					sqlCommand.Parameters.AddWithValue("CustomerGroup" + i, item.CustomerGroup == null ? (object)DBNull.Value : item.CustomerGroup);
					sqlCommand.Parameters.AddWithValue("CustomerHistory" + i, item.CustomerHistory == null ? (object)DBNull.Value : item.CustomerHistory);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("CustomerOverview" + i, item.CustomerOverview == null ? (object)DBNull.Value : item.CustomerOverview);
					sqlCommand.Parameters.AddWithValue("Customers" + i, item.Customers);
					sqlCommand.Parameters.AddWithValue("CustomerShipping" + i, item.CustomerShipping == null ? (object)DBNull.Value : item.CustomerShipping);
					sqlCommand.Parameters.AddWithValue("DeleteAltPositionsArticleBOM" + i, item.DeleteAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("DeleteArticle" + i, item.DeleteArticle == null ? (object)DBNull.Value : item.DeleteArticle);
					sqlCommand.Parameters.AddWithValue("DeleteArticleBOM" + i, item.DeleteArticleBOM);
					sqlCommand.Parameters.AddWithValue("DeleteArticlePurchase" + i, item.DeleteArticlePurchase);
					sqlCommand.Parameters.AddWithValue("DeleteArticleSalesCustom" + i, item.DeleteArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("DeleteArticleSalesItem" + i, item.DeleteArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("DeleteBCR" + i, item.DeleteBCR == null ? (object)DBNull.Value : item.DeleteBCR);
					sqlCommand.Parameters.AddWithValue("DeleteCustomerContactPerson" + i, item.DeleteCustomerContactPerson == null ? (object)DBNull.Value : item.DeleteCustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("DeleteFiles" + i, item.DeleteFiles == null ? (object)DBNull.Value : item.DeleteFiles);
					sqlCommand.Parameters.AddWithValue("DeleteRohArtikelNummer" + i, item.DeleteRohArtikelNummer == null ? (object)DBNull.Value : item.DeleteRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("DeleteSupplierContactPerson" + i, item.DeleteSupplierContactPerson == null ? (object)DBNull.Value : item.DeleteSupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("DiscountGroup" + i, item.DiscountGroup == null ? (object)DBNull.Value : item.DiscountGroup);
					sqlCommand.Parameters.AddWithValue("DownloadAllOutdatedEinkaufsPreis" + i, item.DownloadAllOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadAllOutdatedEinkaufsPreis);
					sqlCommand.Parameters.AddWithValue("DownloadFiles" + i, item.DownloadFiles == null ? (object)DBNull.Value : item.DownloadFiles);
					sqlCommand.Parameters.AddWithValue("DownloadOutdatedEinkaufsPreis" + i, item.DownloadOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadOutdatedEinkaufsPreis);
					sqlCommand.Parameters.AddWithValue("EdiConcern" + i, item.EdiConcern == null ? (object)DBNull.Value : item.EdiConcern);
					sqlCommand.Parameters.AddWithValue("EditAltPositionsArticleBOM" + i, item.EditAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("EditArticle" + i, item.EditArticle);
					sqlCommand.Parameters.AddWithValue("EditArticleBOM" + i, item.EditArticleBOM);
					sqlCommand.Parameters.AddWithValue("EditArticleBOMPosition" + i, item.EditArticleBOMPosition);
					sqlCommand.Parameters.AddWithValue("EditArticleCts" + i, item.EditArticleCts);
					sqlCommand.Parameters.AddWithValue("EditArticleData" + i, item.EditArticleData);
					sqlCommand.Parameters.AddWithValue("EditArticleDesignation" + i, item.EditArticleDesignation);
					sqlCommand.Parameters.AddWithValue("EditArticleImage" + i, item.EditArticleImage);
					sqlCommand.Parameters.AddWithValue("EditArticleLogistics" + i, item.EditArticleLogistics);
					sqlCommand.Parameters.AddWithValue("EditArticleManager" + i, item.EditArticleManager);
					sqlCommand.Parameters.AddWithValue("EditArticleProduction" + i, item.EditArticleProduction);
					sqlCommand.Parameters.AddWithValue("EditArticlePurchase" + i, item.EditArticlePurchase);
					sqlCommand.Parameters.AddWithValue("EditArticleQuality" + i, item.EditArticleQuality);
					sqlCommand.Parameters.AddWithValue("EditArticleReference" + i, item.EditArticleReference == null ? (object)DBNull.Value : item.EditArticleReference);
					sqlCommand.Parameters.AddWithValue("EditArticleSalesCustom" + i, item.EditArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("EditArticleSalesItem" + i, item.EditArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("EditCocType" + i, item.EditCocType == null ? (object)DBNull.Value : item.EditCocType);
					sqlCommand.Parameters.AddWithValue("EditConditionAssignment" + i, item.EditConditionAssignment == null ? (object)DBNull.Value : item.EditConditionAssignment);
					sqlCommand.Parameters.AddWithValue("EditContactAddress" + i, item.EditContactAddress == null ? (object)DBNull.Value : item.EditContactAddress);
					sqlCommand.Parameters.AddWithValue("EditContactSalutation" + i, item.EditContactSalutation == null ? (object)DBNull.Value : item.EditContactSalutation);
					sqlCommand.Parameters.AddWithValue("EditCurrencies" + i, item.EditCurrencies == null ? (object)DBNull.Value : item.EditCurrencies);
					sqlCommand.Parameters.AddWithValue("EditCustomer" + i, item.EditCustomer == null ? (object)DBNull.Value : item.EditCustomer);
					sqlCommand.Parameters.AddWithValue("EditCustomerAddress" + i, item.EditCustomerAddress == null ? (object)DBNull.Value : item.EditCustomerAddress);
					sqlCommand.Parameters.AddWithValue("EditCustomerCommunication" + i, item.EditCustomerCommunication == null ? (object)DBNull.Value : item.EditCustomerCommunication);
					sqlCommand.Parameters.AddWithValue("EditCustomerContactPerson" + i, item.EditCustomerContactPerson == null ? (object)DBNull.Value : item.EditCustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("EditCustomerCoordination" + i, item.EditCustomerCoordination == null ? (object)DBNull.Value : item.EditCustomerCoordination);
					sqlCommand.Parameters.AddWithValue("EditCustomerData" + i, item.EditCustomerData == null ? (object)DBNull.Value : item.EditCustomerData);
					sqlCommand.Parameters.AddWithValue("EditCustomerGroup" + i, item.EditCustomerGroup == null ? (object)DBNull.Value : item.EditCustomerGroup);
					sqlCommand.Parameters.AddWithValue("EditCustomerImage" + i, item.EditCustomerImage == null ? (object)DBNull.Value : item.EditCustomerImage);
					sqlCommand.Parameters.AddWithValue("EditCustomerItemNumber" + i, item.EditCustomerItemNumber == null ? (object)DBNull.Value : item.EditCustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("EditCustomerShipping" + i, item.EditCustomerShipping == null ? (object)DBNull.Value : item.EditCustomerShipping);
					sqlCommand.Parameters.AddWithValue("EditDiscountGroup" + i, item.EditDiscountGroup == null ? (object)DBNull.Value : item.EditDiscountGroup);
					sqlCommand.Parameters.AddWithValue("EditEdiConcern" + i, item.EditEdiConcern == null ? (object)DBNull.Value : item.EditEdiConcern);
					sqlCommand.Parameters.AddWithValue("EditFibuFrame" + i, item.EditFibuFrame == null ? (object)DBNull.Value : item.EditFibuFrame);
					sqlCommand.Parameters.AddWithValue("EditHourlyRate" + i, item.EditHourlyRate == null ? (object)DBNull.Value : item.EditHourlyRate);
					sqlCommand.Parameters.AddWithValue("EditIndustry" + i, item.EditIndustry == null ? (object)DBNull.Value : item.EditIndustry);
					sqlCommand.Parameters.AddWithValue("EditLagerCCID" + i, item.EditLagerCCID == null ? (object)DBNull.Value : item.EditLagerCCID);
					sqlCommand.Parameters.AddWithValue("EditLagerMinStock" + i, item.EditLagerMinStock);
					sqlCommand.Parameters.AddWithValue("EditLagerOrderProposal" + i, item.EditLagerOrderProposal == null ? (object)DBNull.Value : item.EditLagerOrderProposal);
					sqlCommand.Parameters.AddWithValue("EditLagerStock" + i, item.EditLagerStock == null ? (object)DBNull.Value : item.EditLagerStock);
					sqlCommand.Parameters.AddWithValue("EditPayementPractises" + i, item.EditPayementPractises == null ? (object)DBNull.Value : item.EditPayementPractises);
					sqlCommand.Parameters.AddWithValue("EditPricingGroup" + i, item.EditPricingGroup == null ? (object)DBNull.Value : item.EditPricingGroup);
					sqlCommand.Parameters.AddWithValue("EditRohArtikelNummer" + i, item.EditRohArtikelNummer == null ? (object)DBNull.Value : item.EditRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("EditShippingMethods" + i, item.EditShippingMethods == null ? (object)DBNull.Value : item.EditShippingMethods);
					sqlCommand.Parameters.AddWithValue("EditSlipCircle" + i, item.EditSlipCircle == null ? (object)DBNull.Value : item.EditSlipCircle);
					sqlCommand.Parameters.AddWithValue("EditSupplier" + i, item.EditSupplier == null ? (object)DBNull.Value : item.EditSupplier);
					sqlCommand.Parameters.AddWithValue("EditSupplierAddress" + i, item.EditSupplierAddress == null ? (object)DBNull.Value : item.EditSupplierAddress);
					sqlCommand.Parameters.AddWithValue("EditSupplierCommunication" + i, item.EditSupplierCommunication == null ? (object)DBNull.Value : item.EditSupplierCommunication);
					sqlCommand.Parameters.AddWithValue("EditSupplierContactPerson" + i, item.EditSupplierContactPerson == null ? (object)DBNull.Value : item.EditSupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("EditSupplierCoordination" + i, item.EditSupplierCoordination == null ? (object)DBNull.Value : item.EditSupplierCoordination);
					sqlCommand.Parameters.AddWithValue("EditSupplierData" + i, item.EditSupplierData == null ? (object)DBNull.Value : item.EditSupplierData);
					sqlCommand.Parameters.AddWithValue("EditSupplierGroup" + i, item.EditSupplierGroup == null ? (object)DBNull.Value : item.EditSupplierGroup);
					sqlCommand.Parameters.AddWithValue("EditSupplierImage" + i, item.EditSupplierImage == null ? (object)DBNull.Value : item.EditSupplierImage);
					sqlCommand.Parameters.AddWithValue("EditSupplierShipping" + i, item.EditSupplierShipping == null ? (object)DBNull.Value : item.EditSupplierShipping);
					sqlCommand.Parameters.AddWithValue("EditTermsOfPayment" + i, item.EditTermsOfPayment == null ? (object)DBNull.Value : item.EditTermsOfPayment);
					sqlCommand.Parameters.AddWithValue("EDrawingEdit" + i, item.EDrawingEdit == null ? (object)DBNull.Value : item.EDrawingEdit);
					sqlCommand.Parameters.AddWithValue("EinkaufsPreisUpdate" + i, item.EinkaufsPreisUpdate == null ? (object)DBNull.Value : item.EinkaufsPreisUpdate);
					sqlCommand.Parameters.AddWithValue("FibuFrame" + i, item.FibuFrame == null ? (object)DBNull.Value : item.FibuFrame);
					sqlCommand.Parameters.AddWithValue("GetRohArtikelNummer" + i, item.GetRohArtikelNummer == null ? (object)DBNull.Value : item.GetRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("HourlyRate" + i, item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
					sqlCommand.Parameters.AddWithValue("ImportArticleBOM" + i, item.ImportArticleBOM);
					sqlCommand.Parameters.AddWithValue("Industry" + i, item.Industry == null ? (object)DBNull.Value : item.Industry);
					sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
					sqlCommand.Parameters.AddWithValue("LagerArticleLogistics" + i, item.LagerArticleLogistics);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("ModuleAdministrator" + i, item.ModuleAdministrator == null ? (object)DBNull.Value : item.ModuleAdministrator);
					sqlCommand.Parameters.AddWithValue("offer" + i, item.offer == null ? (object)DBNull.Value : item.offer);
					sqlCommand.Parameters.AddWithValue("OfferRequestADD" + i, item.OfferRequestADD == null ? (object)DBNull.Value : item.OfferRequestADD);
					sqlCommand.Parameters.AddWithValue("OfferRequestApplyPrice" + i, item.OfferRequestApplyPrice == null ? (object)DBNull.Value : item.OfferRequestApplyPrice);
					sqlCommand.Parameters.AddWithValue("OfferRequestDelete" + i, item.OfferRequestDelete == null ? (object)DBNull.Value : item.OfferRequestDelete);
					sqlCommand.Parameters.AddWithValue("OfferRequestEdit" + i, item.OfferRequestEdit == null ? (object)DBNull.Value : item.OfferRequestEdit);
					sqlCommand.Parameters.AddWithValue("OfferRequestEditEmail" + i, item.OfferRequestEditEmail == null ? (object)DBNull.Value : item.OfferRequestEditEmail);
					sqlCommand.Parameters.AddWithValue("OfferRequestSendEmail" + i, item.OfferRequestSendEmail == null ? (object)DBNull.Value : item.OfferRequestSendEmail);
					sqlCommand.Parameters.AddWithValue("OfferRequestView" + i, item.OfferRequestView == null ? (object)DBNull.Value : item.OfferRequestView);
					sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoAdd" + i, item.PackagingsLgtPhotoAdd == null ? (object)DBNull.Value : item.PackagingsLgtPhotoAdd);
					sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoDelete" + i, item.PackagingsLgtPhotoDelete == null ? (object)DBNull.Value : item.PackagingsLgtPhotoDelete);
					sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoView" + i, item.PackagingsLgtPhotoView == null ? (object)DBNull.Value : item.PackagingsLgtPhotoView);
					sqlCommand.Parameters.AddWithValue("PayementPractises" + i, item.PayementPractises == null ? (object)DBNull.Value : item.PayementPractises);
					sqlCommand.Parameters.AddWithValue("PMAddCable" + i, item.PMAddCable == null ? (object)DBNull.Value : item.PMAddCable);
					sqlCommand.Parameters.AddWithValue("PMAddMileStone" + i, item.PMAddMileStone == null ? (object)DBNull.Value : item.PMAddMileStone);
					sqlCommand.Parameters.AddWithValue("PMAddProject" + i, item.PMAddProject == null ? (object)DBNull.Value : item.PMAddProject);
					sqlCommand.Parameters.AddWithValue("PMDeleteCable" + i, item.PMDeleteCable == null ? (object)DBNull.Value : item.PMDeleteCable);
					sqlCommand.Parameters.AddWithValue("PMDeleteMileStone" + i, item.PMDeleteMileStone == null ? (object)DBNull.Value : item.PMDeleteMileStone);
					sqlCommand.Parameters.AddWithValue("PMDeleteProject" + i, item.PMDeleteProject == null ? (object)DBNull.Value : item.PMDeleteProject);
					sqlCommand.Parameters.AddWithValue("PMEditCable" + i, item.PMEditCable == null ? (object)DBNull.Value : item.PMEditCable);
					sqlCommand.Parameters.AddWithValue("PMEditMileStone" + i, item.PMEditMileStone == null ? (object)DBNull.Value : item.PMEditMileStone);
					sqlCommand.Parameters.AddWithValue("PMEditProject" + i, item.PMEditProject == null ? (object)DBNull.Value : item.PMEditProject);
					sqlCommand.Parameters.AddWithValue("PMModule" + i, item.PMModule == null ? (object)DBNull.Value : item.PMModule);
					sqlCommand.Parameters.AddWithValue("PMViewProjectsCompact" + i, item.PMViewProjectsCompact == null ? (object)DBNull.Value : item.PMViewProjectsCompact);
					sqlCommand.Parameters.AddWithValue("PMViewProjectsDetail" + i, item.PMViewProjectsDetail == null ? (object)DBNull.Value : item.PMViewProjectsDetail);
					sqlCommand.Parameters.AddWithValue("PMViewProjectsMedium" + i, item.PMViewProjectsMedium == null ? (object)DBNull.Value : item.PMViewProjectsMedium);
					sqlCommand.Parameters.AddWithValue("PricingGroup" + i, item.PricingGroup == null ? (object)DBNull.Value : item.PricingGroup);
					sqlCommand.Parameters.AddWithValue("RemoveArticleReference" + i, item.RemoveArticleReference == null ? (object)DBNull.Value : item.RemoveArticleReference);
					sqlCommand.Parameters.AddWithValue("Settings" + i, item.Settings == null ? (object)DBNull.Value : item.Settings);
					sqlCommand.Parameters.AddWithValue("ShippingMethods" + i, item.ShippingMethods == null ? (object)DBNull.Value : item.ShippingMethods);
					sqlCommand.Parameters.AddWithValue("SlipCircle" + i, item.SlipCircle == null ? (object)DBNull.Value : item.SlipCircle);
					sqlCommand.Parameters.AddWithValue("SupplierAddress" + i, item.SupplierAddress == null ? (object)DBNull.Value : item.SupplierAddress);
					sqlCommand.Parameters.AddWithValue("SupplierAttachementAddFile" + i, item.SupplierAttachementAddFile == null ? (object)DBNull.Value : item.SupplierAttachementAddFile);
					sqlCommand.Parameters.AddWithValue("SupplierAttachementGetFile" + i, item.SupplierAttachementGetFile == null ? (object)DBNull.Value : item.SupplierAttachementGetFile);
					sqlCommand.Parameters.AddWithValue("SupplierAttachementRemoveFile" + i, item.SupplierAttachementRemoveFile == null ? (object)DBNull.Value : item.SupplierAttachementRemoveFile);
					sqlCommand.Parameters.AddWithValue("SupplierCommunication" + i, item.SupplierCommunication == null ? (object)DBNull.Value : item.SupplierCommunication);
					sqlCommand.Parameters.AddWithValue("SupplierContactPerson" + i, item.SupplierContactPerson == null ? (object)DBNull.Value : item.SupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("SupplierData" + i, item.SupplierData == null ? (object)DBNull.Value : item.SupplierData);
					sqlCommand.Parameters.AddWithValue("SupplierGroup" + i, item.SupplierGroup == null ? (object)DBNull.Value : item.SupplierGroup);
					sqlCommand.Parameters.AddWithValue("SupplierHistory" + i, item.SupplierHistory == null ? (object)DBNull.Value : item.SupplierHistory);
					sqlCommand.Parameters.AddWithValue("SupplierOverview" + i, item.SupplierOverview == null ? (object)DBNull.Value : item.SupplierOverview);
					sqlCommand.Parameters.AddWithValue("Suppliers" + i, item.Suppliers);
					sqlCommand.Parameters.AddWithValue("SupplierShipping" + i, item.SupplierShipping == null ? (object)DBNull.Value : item.SupplierShipping);
					sqlCommand.Parameters.AddWithValue("TermsOfPayment" + i, item.TermsOfPayment == null ? (object)DBNull.Value : item.TermsOfPayment);
					sqlCommand.Parameters.AddWithValue("UploadAltPositionsArticleBOM" + i, item.UploadAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("UploadArticleBOM" + i, item.UploadArticleBOM);
					sqlCommand.Parameters.AddWithValue("ValidateArticleBOM" + i, item.ValidateArticleBOM == null ? (object)DBNull.Value : item.ValidateArticleBOM);
					sqlCommand.Parameters.AddWithValue("ValidateBCR" + i, item.ValidateBCR == null ? (object)DBNull.Value : item.ValidateBCR);
					sqlCommand.Parameters.AddWithValue("ViewAltPositionsArticleBOM" + i, item.ViewAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("ViewArticleLog" + i, item.ViewArticleLog);
					sqlCommand.Parameters.AddWithValue("ViewArticleReference" + i, item.ViewArticleReference == null ? (object)DBNull.Value : item.ViewArticleReference);
					sqlCommand.Parameters.AddWithValue("ViewArticles" + i, item.ViewArticles);
					sqlCommand.Parameters.AddWithValue("ViewBCR" + i, item.ViewBCR == null ? (object)DBNull.Value : item.ViewBCR);
					sqlCommand.Parameters.AddWithValue("ViewCustomers" + i, item.ViewCustomers == null ? (object)DBNull.Value : item.ViewCustomers);
					sqlCommand.Parameters.AddWithValue("ViewLPCustomer" + i, item.ViewLPCustomer == null ? (object)DBNull.Value : item.ViewLPCustomer);
					sqlCommand.Parameters.AddWithValue("ViewLPSupplier" + i, item.ViewLPSupplier == null ? (object)DBNull.Value : item.ViewLPSupplier);
					sqlCommand.Parameters.AddWithValue("ViewSupplierAddressComments" + i, item.ViewSupplierAddressComments == null ? (object)DBNull.Value : item.ViewSupplierAddressComments);
					sqlCommand.Parameters.AddWithValue("ViewSuppliers" + i, item.ViewSuppliers == null ? (object)DBNull.Value : item.ViewSuppliers);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Entities.Tables.BSD.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_AccessProfile] SET [AccessProfileName]=@AccessProfileName, [AddArticle]=@AddArticle, [AddArticleReference]=@AddArticleReference, [AddBCR]=@AddBCR, [AddCocType]=@AddCocType, [AddConditionAssignment]=@AddConditionAssignment, [AddContactAddress]=@AddContactAddress, [AddContactSalutation]=@AddContactSalutation, [AddCurrencies]=@AddCurrencies, [AddCustomer]=@AddCustomer, [AddCustomerGroup]=@AddCustomerGroup, [AddCustomerItemNumber]=@AddCustomerItemNumber, [AddDiscountGroup]=@AddDiscountGroup, [AddEdiConcern]=@AddEdiConcern, [AddFibuFrame]=@AddFibuFrame, [AddFiles]=@AddFiles, [AddHourlyRate]=@AddHourlyRate, [AddIndustry]=@AddIndustry, [AddPayementPractises]=@AddPayementPractises, [AddPricingGroup]=@AddPricingGroup, [AddRohArtikelNummer]=@AddRohArtikelNummer, [AddShippingMethods]=@AddShippingMethods, [AddSlipCircle]=@AddSlipCircle, [AddSupplier]=@AddSupplier, [AddSupplierGroup]=@AddSupplierGroup, [AddTermsOfPayment]=@AddTermsOfPayment, [Administration]=@Administration, [AltPositionsArticleBOM]=@AltPositionsArticleBOM, [ArchiveArticle]=@ArchiveArticle, [ArchiveCustomer]=@ArchiveCustomer, [ArchiveSupplier]=@ArchiveSupplier, [ArticleAddCustomerDocument]=@ArticleAddCustomerDocument, [ArticleBOM]=@ArticleBOM, [ArticleCts]=@ArticleCts, [ArticleData]=@ArticleData, [ArticleDeleteCustomerDocument]=@ArticleDeleteCustomerDocument, [ArticleLogistics]=@ArticleLogistics, [ArticleLogisticsPrices]=@ArticleLogisticsPrices, [ArticleOverview]=@ArticleOverview, [ArticleProduction]=@ArticleProduction, [ArticlePurchase]=@ArticlePurchase, [ArticleQuality]=@ArticleQuality, [Articles]=@Articles, [ArticleSales]=@ArticleSales, [ArticleSalesCustom]=@ArticleSalesCustom, [ArticleSalesItem]=@ArticleSalesItem, [ArticlesBOMCPControlEngineering]=@ArticlesBOMCPControlEngineering, [ArticlesBOMCPControlHistory]=@ArticlesBOMCPControlHistory, [ArticlesBOMCPControlQuality]=@ArticlesBOMCPControlQuality, [ArticleStatistics]=@ArticleStatistics, [ArticleStatisticsEngineering]=@ArticleStatisticsEngineering, [ArticleStatisticsEngineeringEdit]=@ArticleStatisticsEngineeringEdit, [ArticleStatisticsFinanceAccounting]=@ArticleStatisticsFinanceAccounting, [ArticleStatisticsFinanceAccountingEdit]=@ArticleStatisticsFinanceAccountingEdit, [ArticleStatisticsLogistics]=@ArticleStatisticsLogistics, [ArticleStatisticsLogisticsEdit]=@ArticleStatisticsLogisticsEdit, [ArticleStatisticsPurchase]=@ArticleStatisticsPurchase, [ArticleStatisticsPurchaseEdit]=@ArticleStatisticsPurchaseEdit, [ArticleStatisticsTechnic]=@ArticleStatisticsTechnic, [ArticleStatisticsTechnicEdit]=@ArticleStatisticsTechnicEdit, [BomChangeRequests]=@BomChangeRequests, [CocType]=@CocType, [ConditionAssignment]=@ConditionAssignment, [ConfigArticle]=@ConfigArticle, [ConfigCustomer]=@ConfigCustomer, [ConfigSupplier]=@ConfigSupplier, [ContactAddress]=@ContactAddress, [ContactSalutation]=@ContactSalutation, [CreateArticlePurchase]=@CreateArticlePurchase, [CreateArticleSalesCustom]=@CreateArticleSalesCustom, [CreateArticleSalesItem]=@CreateArticleSalesItem, [CreateCustomerContactPerson]=@CreateCustomerContactPerson, [CreateSupplierContactPerson]=@CreateSupplierContactPerson, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [Currencies]=@Currencies, [CustomerAddress]=@CustomerAddress, [CustomerCommunication]=@CustomerCommunication, [CustomerContactPerson]=@CustomerContactPerson, [CustomerData]=@CustomerData, [CustomerGroup]=@CustomerGroup, [CustomerHistory]=@CustomerHistory, [CustomerItemNumber]=@CustomerItemNumber, [CustomerOverview]=@CustomerOverview, [Customers]=@Customers, [CustomerShipping]=@CustomerShipping, [DeleteAltPositionsArticleBOM]=@DeleteAltPositionsArticleBOM, [DeleteArticle]=@DeleteArticle, [DeleteArticleBOM]=@DeleteArticleBOM, [DeleteArticlePurchase]=@DeleteArticlePurchase, [DeleteArticleSalesCustom]=@DeleteArticleSalesCustom, [DeleteArticleSalesItem]=@DeleteArticleSalesItem, [DeleteBCR]=@DeleteBCR, [DeleteCustomerContactPerson]=@DeleteCustomerContactPerson, [DeleteFiles]=@DeleteFiles, [DeleteRohArtikelNummer]=@DeleteRohArtikelNummer, [DeleteSupplierContactPerson]=@DeleteSupplierContactPerson, [DiscountGroup]=@DiscountGroup, [DownloadAllOutdatedEinkaufsPreis]=@DownloadAllOutdatedEinkaufsPreis, [DownloadFiles]=@DownloadFiles, [DownloadOutdatedEinkaufsPreis]=@DownloadOutdatedEinkaufsPreis, [EdiConcern]=@EdiConcern, [EditAltPositionsArticleBOM]=@EditAltPositionsArticleBOM, [EditArticle]=@EditArticle, [EditArticleBOM]=@EditArticleBOM, [EditArticleBOMPosition]=@EditArticleBOMPosition, [EditArticleCts]=@EditArticleCts, [EditArticleData]=@EditArticleData, [EditArticleDesignation]=@EditArticleDesignation, [EditArticleImage]=@EditArticleImage, [EditArticleLogistics]=@EditArticleLogistics, [EditArticleManager]=@EditArticleManager, [EditArticleProduction]=@EditArticleProduction, [EditArticlePurchase]=@EditArticlePurchase, [EditArticleQuality]=@EditArticleQuality, [EditArticleReference]=@EditArticleReference, [EditArticleSalesCustom]=@EditArticleSalesCustom, [EditArticleSalesItem]=@EditArticleSalesItem, [EditCocType]=@EditCocType, [EditConditionAssignment]=@EditConditionAssignment, [EditContactAddress]=@EditContactAddress, [EditContactSalutation]=@EditContactSalutation, [EditCurrencies]=@EditCurrencies, [EditCustomer]=@EditCustomer, [EditCustomerAddress]=@EditCustomerAddress, [EditCustomerCommunication]=@EditCustomerCommunication, [EditCustomerContactPerson]=@EditCustomerContactPerson, [EditCustomerCoordination]=@EditCustomerCoordination, [EditCustomerData]=@EditCustomerData, [EditCustomerGroup]=@EditCustomerGroup, [EditCustomerImage]=@EditCustomerImage, [EditCustomerItemNumber]=@EditCustomerItemNumber, [EditCustomerShipping]=@EditCustomerShipping, [EditDiscountGroup]=@EditDiscountGroup, [EditEdiConcern]=@EditEdiConcern, [EditFibuFrame]=@EditFibuFrame, [EditHourlyRate]=@EditHourlyRate, [EditIndustry]=@EditIndustry, [EditLagerCCID]=@EditLagerCCID, [EditLagerMinStock]=@EditLagerMinStock, [EditLagerOrderProposal]=@EditLagerOrderProposal, [EditLagerStock]=@EditLagerStock, [EditPayementPractises]=@EditPayementPractises, [EditPricingGroup]=@EditPricingGroup, [EditRohArtikelNummer]=@EditRohArtikelNummer, [EditShippingMethods]=@EditShippingMethods, [EditSlipCircle]=@EditSlipCircle, [EditSupplier]=@EditSupplier, [EditSupplierAddress]=@EditSupplierAddress, [EditSupplierCommunication]=@EditSupplierCommunication, [EditSupplierContactPerson]=@EditSupplierContactPerson, [EditSupplierCoordination]=@EditSupplierCoordination, [EditSupplierData]=@EditSupplierData, [EditSupplierGroup]=@EditSupplierGroup, [EditSupplierImage]=@EditSupplierImage, [EditSupplierShipping]=@EditSupplierShipping, [EditTermsOfPayment]=@EditTermsOfPayment, [EDrawingEdit]=@EDrawingEdit, [EinkaufsPreisUpdate]=@EinkaufsPreisUpdate, [FibuFrame]=@FibuFrame, [GetRohArtikelNummer]=@GetRohArtikelNummer, [HourlyRate]=@HourlyRate, [ImportArticleBOM]=@ImportArticleBOM, [Industry]=@Industry, [isDefault]=@isDefault, [LagerArticleLogistics]=@LagerArticleLogistics, [ModuleActivated]=@ModuleActivated, [ModuleAdministrator]=@ModuleAdministrator, [offer]=@offer, [OfferRequestADD]=@OfferRequestADD, [OfferRequestApplyPrice]=@OfferRequestApplyPrice, [OfferRequestDelete]=@OfferRequestDelete, [OfferRequestEdit]=@OfferRequestEdit, [OfferRequestEditEmail]=@OfferRequestEditEmail, [OfferRequestSendEmail]=@OfferRequestSendEmail, [OfferRequestView]=@OfferRequestView, [PackagingsLgtPhotoAdd]=@PackagingsLgtPhotoAdd, [PackagingsLgtPhotoDelete]=@PackagingsLgtPhotoDelete, [PackagingsLgtPhotoView]=@PackagingsLgtPhotoView, [PayementPractises]=@PayementPractises, [PMAddCable]=@PMAddCable, [PMAddMileStone]=@PMAddMileStone, [PMAddProject]=@PMAddProject, [PMDeleteCable]=@PMDeleteCable, [PMDeleteMileStone]=@PMDeleteMileStone, [PMDeleteProject]=@PMDeleteProject, [PMEditCable]=@PMEditCable, [PMEditMileStone]=@PMEditMileStone, [PMEditProject]=@PMEditProject, [PMModule]=@PMModule, [PMViewProjectsCompact]=@PMViewProjectsCompact, [PMViewProjectsDetail]=@PMViewProjectsDetail, [PMViewProjectsMedium]=@PMViewProjectsMedium, [PricingGroup]=@PricingGroup, [RemoveArticleReference]=@RemoveArticleReference, [Settings]=@Settings, [ShippingMethods]=@ShippingMethods, [SlipCircle]=@SlipCircle, [SupplierAddress]=@SupplierAddress, [SupplierAttachementAddFile]=@SupplierAttachementAddFile, [SupplierAttachementGetFile]=@SupplierAttachementGetFile, [SupplierAttachementRemoveFile]=@SupplierAttachementRemoveFile, [SupplierCommunication]=@SupplierCommunication, [SupplierContactPerson]=@SupplierContactPerson, [SupplierData]=@SupplierData, [SupplierGroup]=@SupplierGroup, [SupplierHistory]=@SupplierHistory, [SupplierOverview]=@SupplierOverview, [Suppliers]=@Suppliers, [SupplierShipping]=@SupplierShipping, [TermsOfPayment]=@TermsOfPayment, [UploadAltPositionsArticleBOM]=@UploadAltPositionsArticleBOM, [UploadArticleBOM]=@UploadArticleBOM, [ValidateArticleBOM]=@ValidateArticleBOM, [ValidateBCR]=@ValidateBCR, [ViewAltPositionsArticleBOM]=@ViewAltPositionsArticleBOM, [ViewArticleLog]=@ViewArticleLog, [ViewArticleReference]=@ViewArticleReference, [ViewArticles]=@ViewArticles, [ViewBCR]=@ViewBCR, [ViewCustomers]=@ViewCustomers, [ViewLPCustomer]=@ViewLPCustomer, [ViewLPSupplier]=@ViewLPSupplier, [ViewSupplierAddressComments]=@ViewSupplierAddressComments, [ViewSuppliers]=@ViewSuppliers WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("AddArticle", item.AddArticle);
			sqlCommand.Parameters.AddWithValue("AddArticleReference", item.AddArticleReference == null ? (object)DBNull.Value : item.AddArticleReference);
			sqlCommand.Parameters.AddWithValue("AddBCR", item.AddBCR == null ? (object)DBNull.Value : item.AddBCR);
			sqlCommand.Parameters.AddWithValue("AddCocType", item.AddCocType == null ? (object)DBNull.Value : item.AddCocType);
			sqlCommand.Parameters.AddWithValue("AddConditionAssignment", item.AddConditionAssignment == null ? (object)DBNull.Value : item.AddConditionAssignment);
			sqlCommand.Parameters.AddWithValue("AddContactAddress", item.AddContactAddress == null ? (object)DBNull.Value : item.AddContactAddress);
			sqlCommand.Parameters.AddWithValue("AddContactSalutation", item.AddContactSalutation == null ? (object)DBNull.Value : item.AddContactSalutation);
			sqlCommand.Parameters.AddWithValue("AddCurrencies", item.AddCurrencies == null ? (object)DBNull.Value : item.AddCurrencies);
			sqlCommand.Parameters.AddWithValue("AddCustomer", item.AddCustomer == null ? (object)DBNull.Value : item.AddCustomer);
			sqlCommand.Parameters.AddWithValue("AddCustomerGroup", item.AddCustomerGroup == null ? (object)DBNull.Value : item.AddCustomerGroup);
			sqlCommand.Parameters.AddWithValue("AddCustomerItemNumber", item.AddCustomerItemNumber == null ? (object)DBNull.Value : item.AddCustomerItemNumber);
			sqlCommand.Parameters.AddWithValue("AddDiscountGroup", item.AddDiscountGroup == null ? (object)DBNull.Value : item.AddDiscountGroup);
			sqlCommand.Parameters.AddWithValue("AddEdiConcern", item.AddEdiConcern == null ? (object)DBNull.Value : item.AddEdiConcern);
			sqlCommand.Parameters.AddWithValue("AddFibuFrame", item.AddFibuFrame == null ? (object)DBNull.Value : item.AddFibuFrame);
			sqlCommand.Parameters.AddWithValue("AddFiles", item.AddFiles == null ? (object)DBNull.Value : item.AddFiles);
			sqlCommand.Parameters.AddWithValue("AddHourlyRate", item.AddHourlyRate == null ? (object)DBNull.Value : item.AddHourlyRate);
			sqlCommand.Parameters.AddWithValue("AddIndustry", item.AddIndustry == null ? (object)DBNull.Value : item.AddIndustry);
			sqlCommand.Parameters.AddWithValue("AddPayementPractises", item.AddPayementPractises == null ? (object)DBNull.Value : item.AddPayementPractises);
			sqlCommand.Parameters.AddWithValue("AddPricingGroup", item.AddPricingGroup == null ? (object)DBNull.Value : item.AddPricingGroup);
			sqlCommand.Parameters.AddWithValue("AddRohArtikelNummer", item.AddRohArtikelNummer == null ? (object)DBNull.Value : item.AddRohArtikelNummer);
			sqlCommand.Parameters.AddWithValue("AddShippingMethods", item.AddShippingMethods == null ? (object)DBNull.Value : item.AddShippingMethods);
			sqlCommand.Parameters.AddWithValue("AddSlipCircle", item.AddSlipCircle == null ? (object)DBNull.Value : item.AddSlipCircle);
			sqlCommand.Parameters.AddWithValue("AddSupplier", item.AddSupplier == null ? (object)DBNull.Value : item.AddSupplier);
			sqlCommand.Parameters.AddWithValue("AddSupplierGroup", item.AddSupplierGroup == null ? (object)DBNull.Value : item.AddSupplierGroup);
			sqlCommand.Parameters.AddWithValue("AddTermsOfPayment", item.AddTermsOfPayment == null ? (object)DBNull.Value : item.AddTermsOfPayment);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration);
			sqlCommand.Parameters.AddWithValue("AltPositionsArticleBOM", item.AltPositionsArticleBOM);
			sqlCommand.Parameters.AddWithValue("ArchiveArticle", item.ArchiveArticle);
			sqlCommand.Parameters.AddWithValue("ArchiveCustomer", item.ArchiveCustomer == null ? (object)DBNull.Value : item.ArchiveCustomer);
			sqlCommand.Parameters.AddWithValue("ArchiveSupplier", item.ArchiveSupplier == null ? (object)DBNull.Value : item.ArchiveSupplier);
			sqlCommand.Parameters.AddWithValue("ArticleAddCustomerDocument", item.ArticleAddCustomerDocument == null ? (object)DBNull.Value : item.ArticleAddCustomerDocument);
			sqlCommand.Parameters.AddWithValue("ArticleBOM", item.ArticleBOM);
			sqlCommand.Parameters.AddWithValue("ArticleCts", item.ArticleCts);
			sqlCommand.Parameters.AddWithValue("ArticleData", item.ArticleData);
			sqlCommand.Parameters.AddWithValue("ArticleDeleteCustomerDocument", item.ArticleDeleteCustomerDocument == null ? (object)DBNull.Value : item.ArticleDeleteCustomerDocument);
			sqlCommand.Parameters.AddWithValue("ArticleLogistics", item.ArticleLogistics);
			sqlCommand.Parameters.AddWithValue("ArticleLogisticsPrices", item.ArticleLogisticsPrices == null ? (object)DBNull.Value : item.ArticleLogisticsPrices);
			sqlCommand.Parameters.AddWithValue("ArticleOverview", item.ArticleOverview);
			sqlCommand.Parameters.AddWithValue("ArticleProduction", item.ArticleProduction);
			sqlCommand.Parameters.AddWithValue("ArticlePurchase", item.ArticlePurchase);
			sqlCommand.Parameters.AddWithValue("ArticleQuality", item.ArticleQuality);
			sqlCommand.Parameters.AddWithValue("Articles", item.Articles);
			sqlCommand.Parameters.AddWithValue("ArticleSales", item.ArticleSales);
			sqlCommand.Parameters.AddWithValue("ArticleSalesCustom", item.ArticleSalesCustom);
			sqlCommand.Parameters.AddWithValue("ArticleSalesItem", item.ArticleSalesItem);
			sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlEngineering", item.ArticlesBOMCPControlEngineering == null ? (object)DBNull.Value : item.ArticlesBOMCPControlEngineering);
			sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlHistory", item.ArticlesBOMCPControlHistory == null ? (object)DBNull.Value : item.ArticlesBOMCPControlHistory);
			sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlQuality", item.ArticlesBOMCPControlQuality == null ? (object)DBNull.Value : item.ArticlesBOMCPControlQuality);
			sqlCommand.Parameters.AddWithValue("ArticleStatistics", item.ArticleStatistics);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineering", item.ArticleStatisticsEngineering);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineeringEdit", item.ArticleStatisticsEngineeringEdit);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccounting", item.ArticleStatisticsFinanceAccounting);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccountingEdit", item.ArticleStatisticsFinanceAccountingEdit);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogistics", item.ArticleStatisticsLogistics);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogisticsEdit", item.ArticleStatisticsLogisticsEdit);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchase", item.ArticleStatisticsPurchase);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchaseEdit", item.ArticleStatisticsPurchaseEdit);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnic", item.ArticleStatisticsTechnic);
			sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnicEdit", item.ArticleStatisticsTechnicEdit);
			sqlCommand.Parameters.AddWithValue("BomChangeRequests", item.BomChangeRequests == null ? (object)DBNull.Value : item.BomChangeRequests);
			sqlCommand.Parameters.AddWithValue("CocType", item.CocType == null ? (object)DBNull.Value : item.CocType);
			sqlCommand.Parameters.AddWithValue("ConditionAssignment", item.ConditionAssignment == null ? (object)DBNull.Value : item.ConditionAssignment);
			sqlCommand.Parameters.AddWithValue("ConfigArticle", item.ConfigArticle);
			sqlCommand.Parameters.AddWithValue("ConfigCustomer", item.ConfigCustomer == null ? (object)DBNull.Value : item.ConfigCustomer);
			sqlCommand.Parameters.AddWithValue("ConfigSupplier", item.ConfigSupplier == null ? (object)DBNull.Value : item.ConfigSupplier);
			sqlCommand.Parameters.AddWithValue("ContactAddress", item.ContactAddress == null ? (object)DBNull.Value : item.ContactAddress);
			sqlCommand.Parameters.AddWithValue("ContactSalutation", item.ContactSalutation == null ? (object)DBNull.Value : item.ContactSalutation);
			sqlCommand.Parameters.AddWithValue("CreateArticlePurchase", item.CreateArticlePurchase);
			sqlCommand.Parameters.AddWithValue("CreateArticleSalesCustom", item.CreateArticleSalesCustom);
			sqlCommand.Parameters.AddWithValue("CreateArticleSalesItem", item.CreateArticleSalesItem);
			sqlCommand.Parameters.AddWithValue("CreateCustomerContactPerson", item.CreateCustomerContactPerson == null ? (object)DBNull.Value : item.CreateCustomerContactPerson);
			sqlCommand.Parameters.AddWithValue("CreateSupplierContactPerson", item.CreateSupplierContactPerson == null ? (object)DBNull.Value : item.CreateSupplierContactPerson);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Currencies", item.Currencies == null ? (object)DBNull.Value : item.Currencies);
			sqlCommand.Parameters.AddWithValue("CustomerAddress", item.CustomerAddress == null ? (object)DBNull.Value : item.CustomerAddress);
			sqlCommand.Parameters.AddWithValue("CustomerCommunication", item.CustomerCommunication == null ? (object)DBNull.Value : item.CustomerCommunication);
			sqlCommand.Parameters.AddWithValue("CustomerContactPerson", item.CustomerContactPerson == null ? (object)DBNull.Value : item.CustomerContactPerson);
			sqlCommand.Parameters.AddWithValue("CustomerData", item.CustomerData == null ? (object)DBNull.Value : item.CustomerData);
			sqlCommand.Parameters.AddWithValue("CustomerGroup", item.CustomerGroup == null ? (object)DBNull.Value : item.CustomerGroup);
			sqlCommand.Parameters.AddWithValue("CustomerHistory", item.CustomerHistory == null ? (object)DBNull.Value : item.CustomerHistory);
			sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
			sqlCommand.Parameters.AddWithValue("CustomerOverview", item.CustomerOverview == null ? (object)DBNull.Value : item.CustomerOverview);
			sqlCommand.Parameters.AddWithValue("Customers", item.Customers);
			sqlCommand.Parameters.AddWithValue("CustomerShipping", item.CustomerShipping == null ? (object)DBNull.Value : item.CustomerShipping);
			sqlCommand.Parameters.AddWithValue("DeleteAltPositionsArticleBOM", item.DeleteAltPositionsArticleBOM);
			sqlCommand.Parameters.AddWithValue("DeleteArticle", item.DeleteArticle == null ? (object)DBNull.Value : item.DeleteArticle);
			sqlCommand.Parameters.AddWithValue("DeleteArticleBOM", item.DeleteArticleBOM);
			sqlCommand.Parameters.AddWithValue("DeleteArticlePurchase", item.DeleteArticlePurchase);
			sqlCommand.Parameters.AddWithValue("DeleteArticleSalesCustom", item.DeleteArticleSalesCustom);
			sqlCommand.Parameters.AddWithValue("DeleteArticleSalesItem", item.DeleteArticleSalesItem);
			sqlCommand.Parameters.AddWithValue("DeleteBCR", item.DeleteBCR == null ? (object)DBNull.Value : item.DeleteBCR);
			sqlCommand.Parameters.AddWithValue("DeleteCustomerContactPerson", item.DeleteCustomerContactPerson == null ? (object)DBNull.Value : item.DeleteCustomerContactPerson);
			sqlCommand.Parameters.AddWithValue("DeleteFiles", item.DeleteFiles == null ? (object)DBNull.Value : item.DeleteFiles);
			sqlCommand.Parameters.AddWithValue("DeleteRohArtikelNummer", item.DeleteRohArtikelNummer == null ? (object)DBNull.Value : item.DeleteRohArtikelNummer);
			sqlCommand.Parameters.AddWithValue("DeleteSupplierContactPerson", item.DeleteSupplierContactPerson == null ? (object)DBNull.Value : item.DeleteSupplierContactPerson);
			sqlCommand.Parameters.AddWithValue("DiscountGroup", item.DiscountGroup == null ? (object)DBNull.Value : item.DiscountGroup);
			sqlCommand.Parameters.AddWithValue("DownloadAllOutdatedEinkaufsPreis", item.DownloadAllOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadAllOutdatedEinkaufsPreis);
			sqlCommand.Parameters.AddWithValue("DownloadFiles", item.DownloadFiles == null ? (object)DBNull.Value : item.DownloadFiles);
			sqlCommand.Parameters.AddWithValue("DownloadOutdatedEinkaufsPreis", item.DownloadOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadOutdatedEinkaufsPreis);
			sqlCommand.Parameters.AddWithValue("EdiConcern", item.EdiConcern == null ? (object)DBNull.Value : item.EdiConcern);
			sqlCommand.Parameters.AddWithValue("EditAltPositionsArticleBOM", item.EditAltPositionsArticleBOM);
			sqlCommand.Parameters.AddWithValue("EditArticle", item.EditArticle);
			sqlCommand.Parameters.AddWithValue("EditArticleBOM", item.EditArticleBOM);
			sqlCommand.Parameters.AddWithValue("EditArticleBOMPosition", item.EditArticleBOMPosition);
			sqlCommand.Parameters.AddWithValue("EditArticleCts", item.EditArticleCts);
			sqlCommand.Parameters.AddWithValue("EditArticleData", item.EditArticleData);
			sqlCommand.Parameters.AddWithValue("EditArticleDesignation", item.EditArticleDesignation);
			sqlCommand.Parameters.AddWithValue("EditArticleImage", item.EditArticleImage);
			sqlCommand.Parameters.AddWithValue("EditArticleLogistics", item.EditArticleLogistics);
			sqlCommand.Parameters.AddWithValue("EditArticleManager", item.EditArticleManager);
			sqlCommand.Parameters.AddWithValue("EditArticleProduction", item.EditArticleProduction);
			sqlCommand.Parameters.AddWithValue("EditArticlePurchase", item.EditArticlePurchase);
			sqlCommand.Parameters.AddWithValue("EditArticleQuality", item.EditArticleQuality);
			sqlCommand.Parameters.AddWithValue("EditArticleReference", item.EditArticleReference == null ? (object)DBNull.Value : item.EditArticleReference);
			sqlCommand.Parameters.AddWithValue("EditArticleSalesCustom", item.EditArticleSalesCustom);
			sqlCommand.Parameters.AddWithValue("EditArticleSalesItem", item.EditArticleSalesItem);
			sqlCommand.Parameters.AddWithValue("EditCocType", item.EditCocType == null ? (object)DBNull.Value : item.EditCocType);
			sqlCommand.Parameters.AddWithValue("EditConditionAssignment", item.EditConditionAssignment == null ? (object)DBNull.Value : item.EditConditionAssignment);
			sqlCommand.Parameters.AddWithValue("EditContactAddress", item.EditContactAddress == null ? (object)DBNull.Value : item.EditContactAddress);
			sqlCommand.Parameters.AddWithValue("EditContactSalutation", item.EditContactSalutation == null ? (object)DBNull.Value : item.EditContactSalutation);
			sqlCommand.Parameters.AddWithValue("EditCurrencies", item.EditCurrencies == null ? (object)DBNull.Value : item.EditCurrencies);
			sqlCommand.Parameters.AddWithValue("EditCustomer", item.EditCustomer == null ? (object)DBNull.Value : item.EditCustomer);
			sqlCommand.Parameters.AddWithValue("EditCustomerAddress", item.EditCustomerAddress == null ? (object)DBNull.Value : item.EditCustomerAddress);
			sqlCommand.Parameters.AddWithValue("EditCustomerCommunication", item.EditCustomerCommunication == null ? (object)DBNull.Value : item.EditCustomerCommunication);
			sqlCommand.Parameters.AddWithValue("EditCustomerContactPerson", item.EditCustomerContactPerson == null ? (object)DBNull.Value : item.EditCustomerContactPerson);
			sqlCommand.Parameters.AddWithValue("EditCustomerCoordination", item.EditCustomerCoordination == null ? (object)DBNull.Value : item.EditCustomerCoordination);
			sqlCommand.Parameters.AddWithValue("EditCustomerData", item.EditCustomerData == null ? (object)DBNull.Value : item.EditCustomerData);
			sqlCommand.Parameters.AddWithValue("EditCustomerGroup", item.EditCustomerGroup == null ? (object)DBNull.Value : item.EditCustomerGroup);
			sqlCommand.Parameters.AddWithValue("EditCustomerImage", item.EditCustomerImage == null ? (object)DBNull.Value : item.EditCustomerImage);
			sqlCommand.Parameters.AddWithValue("EditCustomerItemNumber", item.EditCustomerItemNumber == null ? (object)DBNull.Value : item.EditCustomerItemNumber);
			sqlCommand.Parameters.AddWithValue("EditCustomerShipping", item.EditCustomerShipping == null ? (object)DBNull.Value : item.EditCustomerShipping);
			sqlCommand.Parameters.AddWithValue("EditDiscountGroup", item.EditDiscountGroup == null ? (object)DBNull.Value : item.EditDiscountGroup);
			sqlCommand.Parameters.AddWithValue("EditEdiConcern", item.EditEdiConcern == null ? (object)DBNull.Value : item.EditEdiConcern);
			sqlCommand.Parameters.AddWithValue("EditFibuFrame", item.EditFibuFrame == null ? (object)DBNull.Value : item.EditFibuFrame);
			sqlCommand.Parameters.AddWithValue("EditHourlyRate", item.EditHourlyRate == null ? (object)DBNull.Value : item.EditHourlyRate);
			sqlCommand.Parameters.AddWithValue("EditIndustry", item.EditIndustry == null ? (object)DBNull.Value : item.EditIndustry);
			sqlCommand.Parameters.AddWithValue("EditLagerCCID", item.EditLagerCCID == null ? (object)DBNull.Value : item.EditLagerCCID);
			sqlCommand.Parameters.AddWithValue("EditLagerMinStock", item.EditLagerMinStock);
			sqlCommand.Parameters.AddWithValue("EditLagerOrderProposal", item.EditLagerOrderProposal == null ? (object)DBNull.Value : item.EditLagerOrderProposal);
			sqlCommand.Parameters.AddWithValue("EditLagerStock", item.EditLagerStock == null ? (object)DBNull.Value : item.EditLagerStock);
			sqlCommand.Parameters.AddWithValue("EditPayementPractises", item.EditPayementPractises == null ? (object)DBNull.Value : item.EditPayementPractises);
			sqlCommand.Parameters.AddWithValue("EditPricingGroup", item.EditPricingGroup == null ? (object)DBNull.Value : item.EditPricingGroup);
			sqlCommand.Parameters.AddWithValue("EditRohArtikelNummer", item.EditRohArtikelNummer == null ? (object)DBNull.Value : item.EditRohArtikelNummer);
			sqlCommand.Parameters.AddWithValue("EditShippingMethods", item.EditShippingMethods == null ? (object)DBNull.Value : item.EditShippingMethods);
			sqlCommand.Parameters.AddWithValue("EditSlipCircle", item.EditSlipCircle == null ? (object)DBNull.Value : item.EditSlipCircle);
			sqlCommand.Parameters.AddWithValue("EditSupplier", item.EditSupplier == null ? (object)DBNull.Value : item.EditSupplier);
			sqlCommand.Parameters.AddWithValue("EditSupplierAddress", item.EditSupplierAddress == null ? (object)DBNull.Value : item.EditSupplierAddress);
			sqlCommand.Parameters.AddWithValue("EditSupplierCommunication", item.EditSupplierCommunication == null ? (object)DBNull.Value : item.EditSupplierCommunication);
			sqlCommand.Parameters.AddWithValue("EditSupplierContactPerson", item.EditSupplierContactPerson == null ? (object)DBNull.Value : item.EditSupplierContactPerson);
			sqlCommand.Parameters.AddWithValue("EditSupplierCoordination", item.EditSupplierCoordination == null ? (object)DBNull.Value : item.EditSupplierCoordination);
			sqlCommand.Parameters.AddWithValue("EditSupplierData", item.EditSupplierData == null ? (object)DBNull.Value : item.EditSupplierData);
			sqlCommand.Parameters.AddWithValue("EditSupplierGroup", item.EditSupplierGroup == null ? (object)DBNull.Value : item.EditSupplierGroup);
			sqlCommand.Parameters.AddWithValue("EditSupplierImage", item.EditSupplierImage == null ? (object)DBNull.Value : item.EditSupplierImage);
			sqlCommand.Parameters.AddWithValue("EditSupplierShipping", item.EditSupplierShipping == null ? (object)DBNull.Value : item.EditSupplierShipping);
			sqlCommand.Parameters.AddWithValue("EditTermsOfPayment", item.EditTermsOfPayment == null ? (object)DBNull.Value : item.EditTermsOfPayment);
			sqlCommand.Parameters.AddWithValue("EDrawingEdit", item.EDrawingEdit == null ? (object)DBNull.Value : item.EDrawingEdit);
			sqlCommand.Parameters.AddWithValue("EinkaufsPreisUpdate", item.EinkaufsPreisUpdate == null ? (object)DBNull.Value : item.EinkaufsPreisUpdate);
			sqlCommand.Parameters.AddWithValue("FibuFrame", item.FibuFrame == null ? (object)DBNull.Value : item.FibuFrame);
			sqlCommand.Parameters.AddWithValue("GetRohArtikelNummer", item.GetRohArtikelNummer == null ? (object)DBNull.Value : item.GetRohArtikelNummer);
			sqlCommand.Parameters.AddWithValue("HourlyRate", item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
			sqlCommand.Parameters.AddWithValue("ImportArticleBOM", item.ImportArticleBOM);
			sqlCommand.Parameters.AddWithValue("Industry", item.Industry == null ? (object)DBNull.Value : item.Industry);
			sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
			sqlCommand.Parameters.AddWithValue("LagerArticleLogistics", item.LagerArticleLogistics);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("ModuleAdministrator", item.ModuleAdministrator == null ? (object)DBNull.Value : item.ModuleAdministrator);
			sqlCommand.Parameters.AddWithValue("offer", item.offer == null ? (object)DBNull.Value : item.offer);
			sqlCommand.Parameters.AddWithValue("OfferRequestADD", item.OfferRequestADD == null ? (object)DBNull.Value : item.OfferRequestADD);
			sqlCommand.Parameters.AddWithValue("OfferRequestApplyPrice", item.OfferRequestApplyPrice == null ? (object)DBNull.Value : item.OfferRequestApplyPrice);
			sqlCommand.Parameters.AddWithValue("OfferRequestDelete", item.OfferRequestDelete == null ? (object)DBNull.Value : item.OfferRequestDelete);
			sqlCommand.Parameters.AddWithValue("OfferRequestEdit", item.OfferRequestEdit == null ? (object)DBNull.Value : item.OfferRequestEdit);
			sqlCommand.Parameters.AddWithValue("OfferRequestEditEmail", item.OfferRequestEditEmail == null ? (object)DBNull.Value : item.OfferRequestEditEmail);
			sqlCommand.Parameters.AddWithValue("OfferRequestSendEmail", item.OfferRequestSendEmail == null ? (object)DBNull.Value : item.OfferRequestSendEmail);
			sqlCommand.Parameters.AddWithValue("OfferRequestView", item.OfferRequestView == null ? (object)DBNull.Value : item.OfferRequestView);
			sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoAdd", item.PackagingsLgtPhotoAdd == null ? (object)DBNull.Value : item.PackagingsLgtPhotoAdd);
			sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoDelete", item.PackagingsLgtPhotoDelete == null ? (object)DBNull.Value : item.PackagingsLgtPhotoDelete);
			sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoView", item.PackagingsLgtPhotoView == null ? (object)DBNull.Value : item.PackagingsLgtPhotoView);
			sqlCommand.Parameters.AddWithValue("PayementPractises", item.PayementPractises == null ? (object)DBNull.Value : item.PayementPractises);
			sqlCommand.Parameters.AddWithValue("PMAddCable", item.PMAddCable == null ? (object)DBNull.Value : item.PMAddCable);
			sqlCommand.Parameters.AddWithValue("PMAddMileStone", item.PMAddMileStone == null ? (object)DBNull.Value : item.PMAddMileStone);
			sqlCommand.Parameters.AddWithValue("PMAddProject", item.PMAddProject == null ? (object)DBNull.Value : item.PMAddProject);
			sqlCommand.Parameters.AddWithValue("PMDeleteCable", item.PMDeleteCable == null ? (object)DBNull.Value : item.PMDeleteCable);
			sqlCommand.Parameters.AddWithValue("PMDeleteMileStone", item.PMDeleteMileStone == null ? (object)DBNull.Value : item.PMDeleteMileStone);
			sqlCommand.Parameters.AddWithValue("PMDeleteProject", item.PMDeleteProject == null ? (object)DBNull.Value : item.PMDeleteProject);
			sqlCommand.Parameters.AddWithValue("PMEditCable", item.PMEditCable == null ? (object)DBNull.Value : item.PMEditCable);
			sqlCommand.Parameters.AddWithValue("PMEditMileStone", item.PMEditMileStone == null ? (object)DBNull.Value : item.PMEditMileStone);
			sqlCommand.Parameters.AddWithValue("PMEditProject", item.PMEditProject == null ? (object)DBNull.Value : item.PMEditProject);
			sqlCommand.Parameters.AddWithValue("PMModule", item.PMModule == null ? (object)DBNull.Value : item.PMModule);
			sqlCommand.Parameters.AddWithValue("PMViewProjectsCompact", item.PMViewProjectsCompact == null ? (object)DBNull.Value : item.PMViewProjectsCompact);
			sqlCommand.Parameters.AddWithValue("PMViewProjectsDetail", item.PMViewProjectsDetail == null ? (object)DBNull.Value : item.PMViewProjectsDetail);
			sqlCommand.Parameters.AddWithValue("PMViewProjectsMedium", item.PMViewProjectsMedium == null ? (object)DBNull.Value : item.PMViewProjectsMedium);
			sqlCommand.Parameters.AddWithValue("PricingGroup", item.PricingGroup == null ? (object)DBNull.Value : item.PricingGroup);
			sqlCommand.Parameters.AddWithValue("RemoveArticleReference", item.RemoveArticleReference == null ? (object)DBNull.Value : item.RemoveArticleReference);
			sqlCommand.Parameters.AddWithValue("Settings", item.Settings == null ? (object)DBNull.Value : item.Settings);
			sqlCommand.Parameters.AddWithValue("ShippingMethods", item.ShippingMethods == null ? (object)DBNull.Value : item.ShippingMethods);
			sqlCommand.Parameters.AddWithValue("SlipCircle", item.SlipCircle == null ? (object)DBNull.Value : item.SlipCircle);
			sqlCommand.Parameters.AddWithValue("SupplierAddress", item.SupplierAddress == null ? (object)DBNull.Value : item.SupplierAddress);
			sqlCommand.Parameters.AddWithValue("SupplierAttachementAddFile", item.SupplierAttachementAddFile == null ? (object)DBNull.Value : item.SupplierAttachementAddFile);
			sqlCommand.Parameters.AddWithValue("SupplierAttachementGetFile", item.SupplierAttachementGetFile == null ? (object)DBNull.Value : item.SupplierAttachementGetFile);
			sqlCommand.Parameters.AddWithValue("SupplierAttachementRemoveFile", item.SupplierAttachementRemoveFile == null ? (object)DBNull.Value : item.SupplierAttachementRemoveFile);
			sqlCommand.Parameters.AddWithValue("SupplierCommunication", item.SupplierCommunication == null ? (object)DBNull.Value : item.SupplierCommunication);
			sqlCommand.Parameters.AddWithValue("SupplierContactPerson", item.SupplierContactPerson == null ? (object)DBNull.Value : item.SupplierContactPerson);
			sqlCommand.Parameters.AddWithValue("SupplierData", item.SupplierData == null ? (object)DBNull.Value : item.SupplierData);
			sqlCommand.Parameters.AddWithValue("SupplierGroup", item.SupplierGroup == null ? (object)DBNull.Value : item.SupplierGroup);
			sqlCommand.Parameters.AddWithValue("SupplierHistory", item.SupplierHistory == null ? (object)DBNull.Value : item.SupplierHistory);
			sqlCommand.Parameters.AddWithValue("SupplierOverview", item.SupplierOverview == null ? (object)DBNull.Value : item.SupplierOverview);
			sqlCommand.Parameters.AddWithValue("Suppliers", item.Suppliers);
			sqlCommand.Parameters.AddWithValue("SupplierShipping", item.SupplierShipping == null ? (object)DBNull.Value : item.SupplierShipping);
			sqlCommand.Parameters.AddWithValue("TermsOfPayment", item.TermsOfPayment == null ? (object)DBNull.Value : item.TermsOfPayment);
			sqlCommand.Parameters.AddWithValue("UploadAltPositionsArticleBOM", item.UploadAltPositionsArticleBOM);
			sqlCommand.Parameters.AddWithValue("UploadArticleBOM", item.UploadArticleBOM);
			sqlCommand.Parameters.AddWithValue("ValidateArticleBOM", item.ValidateArticleBOM == null ? (object)DBNull.Value : item.ValidateArticleBOM);
			sqlCommand.Parameters.AddWithValue("ValidateBCR", item.ValidateBCR == null ? (object)DBNull.Value : item.ValidateBCR);
			sqlCommand.Parameters.AddWithValue("ViewAltPositionsArticleBOM", item.ViewAltPositionsArticleBOM);
			sqlCommand.Parameters.AddWithValue("ViewArticleLog", item.ViewArticleLog);
			sqlCommand.Parameters.AddWithValue("ViewArticleReference", item.ViewArticleReference == null ? (object)DBNull.Value : item.ViewArticleReference);
			sqlCommand.Parameters.AddWithValue("ViewArticles", item.ViewArticles);
			sqlCommand.Parameters.AddWithValue("ViewBCR", item.ViewBCR == null ? (object)DBNull.Value : item.ViewBCR);
			sqlCommand.Parameters.AddWithValue("ViewCustomers", item.ViewCustomers == null ? (object)DBNull.Value : item.ViewCustomers);
			sqlCommand.Parameters.AddWithValue("ViewLPCustomer", item.ViewLPCustomer == null ? (object)DBNull.Value : item.ViewLPCustomer);
			sqlCommand.Parameters.AddWithValue("ViewLPSupplier", item.ViewLPSupplier == null ? (object)DBNull.Value : item.ViewLPSupplier);
			sqlCommand.Parameters.AddWithValue("ViewSupplierAddressComments", item.ViewSupplierAddressComments == null ? (object)DBNull.Value : item.ViewSupplierAddressComments);
			sqlCommand.Parameters.AddWithValue("ViewSuppliers", item.ViewSuppliers == null ? (object)DBNull.Value : item.ViewSuppliers);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Entities.Tables.BSD.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 226; // Nb params per query
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
		private static int updateWithTransaction(List<Entities.Tables.BSD.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__BSD_AccessProfile] SET "

					+ "[AccessProfileName]=@AccessProfileName" + i + ","
					+ "[AddArticle]=@AddArticle" + i + ","
					+ "[AddArticleReference]=@AddArticleReference" + i + ","
					+ "[AddBCR]=@AddBCR" + i + ","
					+ "[AddCocType]=@AddCocType" + i + ","
					+ "[AddConditionAssignment]=@AddConditionAssignment" + i + ","
					+ "[AddContactAddress]=@AddContactAddress" + i + ","
					+ "[AddContactSalutation]=@AddContactSalutation" + i + ","
					+ "[AddCurrencies]=@AddCurrencies" + i + ","
					+ "[AddCustomer]=@AddCustomer" + i + ","
					+ "[AddCustomerGroup]=@AddCustomerGroup" + i + ","
					+ "[AddCustomerItemNumber]=@AddCustomerItemNumber" + i + ","
					+ "[AddDiscountGroup]=@AddDiscountGroup" + i + ","
					+ "[AddEdiConcern]=@AddEdiConcern" + i + ","
					+ "[AddFibuFrame]=@AddFibuFrame" + i + ","
					+ "[AddFiles]=@AddFiles" + i + ","
					+ "[AddHourlyRate]=@AddHourlyRate" + i + ","
					+ "[AddIndustry]=@AddIndustry" + i + ","
					+ "[AddPayementPractises]=@AddPayementPractises" + i + ","
					+ "[AddPricingGroup]=@AddPricingGroup" + i + ","
					+ "[AddRohArtikelNummer]=@AddRohArtikelNummer" + i + ","
					+ "[AddShippingMethods]=@AddShippingMethods" + i + ","
					+ "[AddSlipCircle]=@AddSlipCircle" + i + ","
					+ "[AddSupplier]=@AddSupplier" + i + ","
					+ "[AddSupplierGroup]=@AddSupplierGroup" + i + ","
					+ "[AddTermsOfPayment]=@AddTermsOfPayment" + i + ","
					+ "[Administration]=@Administration" + i + ","
					+ "[AltPositionsArticleBOM]=@AltPositionsArticleBOM" + i + ","
					+ "[ArchiveArticle]=@ArchiveArticle" + i + ","
					+ "[ArchiveCustomer]=@ArchiveCustomer" + i + ","
					+ "[ArchiveSupplier]=@ArchiveSupplier" + i + ","
					+ "[ArticleAddCustomerDocument]=@ArticleAddCustomerDocument" + i + ","
					+ "[ArticleBOM]=@ArticleBOM" + i + ","
					+ "[ArticleCts]=@ArticleCts" + i + ","
					+ "[ArticleData]=@ArticleData" + i + ","
					+ "[ArticleDeleteCustomerDocument]=@ArticleDeleteCustomerDocument" + i + ","
					+ "[ArticleLogistics]=@ArticleLogistics" + i + ","
					+ "[ArticleLogisticsPrices]=@ArticleLogisticsPrices" + i + ","
					+ "[ArticleOverview]=@ArticleOverview" + i + ","
					+ "[ArticleProduction]=@ArticleProduction" + i + ","
					+ "[ArticlePurchase]=@ArticlePurchase" + i + ","
					+ "[ArticleQuality]=@ArticleQuality" + i + ","
					+ "[Articles]=@Articles" + i + ","
					+ "[ArticleSales]=@ArticleSales" + i + ","
					+ "[ArticleSalesCustom]=@ArticleSalesCustom" + i + ","
					+ "[ArticleSalesItem]=@ArticleSalesItem" + i + ","
					+ "[ArticlesBOMCPControlEngineering]=@ArticlesBOMCPControlEngineering" + i + ","
					+ "[ArticlesBOMCPControlHistory]=@ArticlesBOMCPControlHistory" + i + ","
					+ "[ArticlesBOMCPControlQuality]=@ArticlesBOMCPControlQuality" + i + ","
					+ "[ArticleStatistics]=@ArticleStatistics" + i + ","
					+ "[ArticleStatisticsEngineering]=@ArticleStatisticsEngineering" + i + ","
					+ "[ArticleStatisticsEngineeringEdit]=@ArticleStatisticsEngineeringEdit" + i + ","
					+ "[ArticleStatisticsFinanceAccounting]=@ArticleStatisticsFinanceAccounting" + i + ","
					+ "[ArticleStatisticsFinanceAccountingEdit]=@ArticleStatisticsFinanceAccountingEdit" + i + ","
					+ "[ArticleStatisticsLogistics]=@ArticleStatisticsLogistics" + i + ","
					+ "[ArticleStatisticsLogisticsEdit]=@ArticleStatisticsLogisticsEdit" + i + ","
					+ "[ArticleStatisticsPurchase]=@ArticleStatisticsPurchase" + i + ","
					+ "[ArticleStatisticsPurchaseEdit]=@ArticleStatisticsPurchaseEdit" + i + ","
					+ "[ArticleStatisticsTechnic]=@ArticleStatisticsTechnic" + i + ","
					+ "[ArticleStatisticsTechnicEdit]=@ArticleStatisticsTechnicEdit" + i + ","
					+ "[BomChangeRequests]=@BomChangeRequests" + i + ","
					+ "[CocType]=@CocType" + i + ","
					+ "[ConditionAssignment]=@ConditionAssignment" + i + ","
					+ "[ConfigArticle]=@ConfigArticle" + i + ","
					+ "[ConfigCustomer]=@ConfigCustomer" + i + ","
					+ "[ConfigSupplier]=@ConfigSupplier" + i + ","
					+ "[ContactAddress]=@ContactAddress" + i + ","
					+ "[ContactSalutation]=@ContactSalutation" + i + ","
					+ "[CreateArticlePurchase]=@CreateArticlePurchase" + i + ","
					+ "[CreateArticleSalesCustom]=@CreateArticleSalesCustom" + i + ","
					+ "[CreateArticleSalesItem]=@CreateArticleSalesItem" + i + ","
					+ "[CreateCustomerContactPerson]=@CreateCustomerContactPerson" + i + ","
					+ "[CreateSupplierContactPerson]=@CreateSupplierContactPerson" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[Currencies]=@Currencies" + i + ","
					+ "[CustomerAddress]=@CustomerAddress" + i + ","
					+ "[CustomerCommunication]=@CustomerCommunication" + i + ","
					+ "[CustomerContactPerson]=@CustomerContactPerson" + i + ","
					+ "[CustomerData]=@CustomerData" + i + ","
					+ "[CustomerGroup]=@CustomerGroup" + i + ","
					+ "[CustomerHistory]=@CustomerHistory" + i + ","
					+ "[CustomerItemNumber]=@CustomerItemNumber" + i + ","
					+ "[CustomerOverview]=@CustomerOverview" + i + ","
					+ "[Customers]=@Customers" + i + ","
					+ "[CustomerShipping]=@CustomerShipping" + i + ","
					+ "[DeleteAltPositionsArticleBOM]=@DeleteAltPositionsArticleBOM" + i + ","
					+ "[DeleteArticle]=@DeleteArticle" + i + ","
					+ "[DeleteArticleBOM]=@DeleteArticleBOM" + i + ","
					+ "[DeleteArticlePurchase]=@DeleteArticlePurchase" + i + ","
					+ "[DeleteArticleSalesCustom]=@DeleteArticleSalesCustom" + i + ","
					+ "[DeleteArticleSalesItem]=@DeleteArticleSalesItem" + i + ","
					+ "[DeleteBCR]=@DeleteBCR" + i + ","
					+ "[DeleteCustomerContactPerson]=@DeleteCustomerContactPerson" + i + ","
					+ "[DeleteFiles]=@DeleteFiles" + i + ","
					+ "[DeleteRohArtikelNummer]=@DeleteRohArtikelNummer" + i + ","
					+ "[DeleteSupplierContactPerson]=@DeleteSupplierContactPerson" + i + ","
					+ "[DiscountGroup]=@DiscountGroup" + i + ","
					+ "[DownloadAllOutdatedEinkaufsPreis]=@DownloadAllOutdatedEinkaufsPreis" + i + ","
					+ "[DownloadFiles]=@DownloadFiles" + i + ","
					+ "[DownloadOutdatedEinkaufsPreis]=@DownloadOutdatedEinkaufsPreis" + i + ","
					+ "[EdiConcern]=@EdiConcern" + i + ","
					+ "[EditAltPositionsArticleBOM]=@EditAltPositionsArticleBOM" + i + ","
					+ "[EditArticle]=@EditArticle" + i + ","
					+ "[EditArticleBOM]=@EditArticleBOM" + i + ","
					+ "[EditArticleBOMPosition]=@EditArticleBOMPosition" + i + ","
					+ "[EditArticleCts]=@EditArticleCts" + i + ","
					+ "[EditArticleData]=@EditArticleData" + i + ","
					+ "[EditArticleDesignation]=@EditArticleDesignation" + i + ","
					+ "[EditArticleImage]=@EditArticleImage" + i + ","
					+ "[EditArticleLogistics]=@EditArticleLogistics" + i + ","
					+ "[EditArticleManager]=@EditArticleManager" + i + ","
					+ "[EditArticleProduction]=@EditArticleProduction" + i + ","
					+ "[EditArticlePurchase]=@EditArticlePurchase" + i + ","
					+ "[EditArticleQuality]=@EditArticleQuality" + i + ","
					+ "[EditArticleReference]=@EditArticleReference" + i + ","
					+ "[EditArticleSalesCustom]=@EditArticleSalesCustom" + i + ","
					+ "[EditArticleSalesItem]=@EditArticleSalesItem" + i + ","
					+ "[EditCocType]=@EditCocType" + i + ","
					+ "[EditConditionAssignment]=@EditConditionAssignment" + i + ","
					+ "[EditContactAddress]=@EditContactAddress" + i + ","
					+ "[EditContactSalutation]=@EditContactSalutation" + i + ","
					+ "[EditCurrencies]=@EditCurrencies" + i + ","
					+ "[EditCustomer]=@EditCustomer" + i + ","
					+ "[EditCustomerAddress]=@EditCustomerAddress" + i + ","
					+ "[EditCustomerCommunication]=@EditCustomerCommunication" + i + ","
					+ "[EditCustomerContactPerson]=@EditCustomerContactPerson" + i + ","
					+ "[EditCustomerCoordination]=@EditCustomerCoordination" + i + ","
					+ "[EditCustomerData]=@EditCustomerData" + i + ","
					+ "[EditCustomerGroup]=@EditCustomerGroup" + i + ","
					+ "[EditCustomerImage]=@EditCustomerImage" + i + ","
					+ "[EditCustomerItemNumber]=@EditCustomerItemNumber" + i + ","
					+ "[EditCustomerShipping]=@EditCustomerShipping" + i + ","
					+ "[EditDiscountGroup]=@EditDiscountGroup" + i + ","
					+ "[EditEdiConcern]=@EditEdiConcern" + i + ","
					+ "[EditFibuFrame]=@EditFibuFrame" + i + ","
					+ "[EditHourlyRate]=@EditHourlyRate" + i + ","
					+ "[EditIndustry]=@EditIndustry" + i + ","
					+ "[EditLagerCCID]=@EditLagerCCID" + i + ","
					+ "[EditLagerMinStock]=@EditLagerMinStock" + i + ","
					+ "[EditLagerOrderProposal]=@EditLagerOrderProposal" + i + ","
					+ "[EditLagerStock]=@EditLagerStock" + i + ","
					+ "[EditPayementPractises]=@EditPayementPractises" + i + ","
					+ "[EditPricingGroup]=@EditPricingGroup" + i + ","
					+ "[EditRohArtikelNummer]=@EditRohArtikelNummer" + i + ","
					+ "[EditShippingMethods]=@EditShippingMethods" + i + ","
					+ "[EditSlipCircle]=@EditSlipCircle" + i + ","
					+ "[EditSupplier]=@EditSupplier" + i + ","
					+ "[EditSupplierAddress]=@EditSupplierAddress" + i + ","
					+ "[EditSupplierCommunication]=@EditSupplierCommunication" + i + ","
					+ "[EditSupplierContactPerson]=@EditSupplierContactPerson" + i + ","
					+ "[EditSupplierCoordination]=@EditSupplierCoordination" + i + ","
					+ "[EditSupplierData]=@EditSupplierData" + i + ","
					+ "[EditSupplierGroup]=@EditSupplierGroup" + i + ","
					+ "[EditSupplierImage]=@EditSupplierImage" + i + ","
					+ "[EditSupplierShipping]=@EditSupplierShipping" + i + ","
					+ "[EditTermsOfPayment]=@EditTermsOfPayment" + i + ","
					+ "[EDrawingEdit]=@EDrawingEdit" + i + ","
					+ "[EinkaufsPreisUpdate]=@EinkaufsPreisUpdate" + i + ","
					+ "[FibuFrame]=@FibuFrame" + i + ","
					+ "[GetRohArtikelNummer]=@GetRohArtikelNummer" + i + ","
					+ "[HourlyRate]=@HourlyRate" + i + ","
					+ "[ImportArticleBOM]=@ImportArticleBOM" + i + ","
					+ "[Industry]=@Industry" + i + ","
					+ "[isDefault]=@isDefault" + i + ","
					+ "[LagerArticleLogistics]=@LagerArticleLogistics" + i + ","
					+ "[ModuleActivated]=@ModuleActivated" + i + ","
					+ "[ModuleAdministrator]=@ModuleAdministrator" + i + ","
					+ "[offer]=@offer" + i + ","
					+ "[OfferRequestADD]=@OfferRequestADD" + i + ","
					+ "[OfferRequestApplyPrice]=@OfferRequestApplyPrice" + i + ","
					+ "[OfferRequestDelete]=@OfferRequestDelete" + i + ","
					+ "[OfferRequestEdit]=@OfferRequestEdit" + i + ","
					+ "[OfferRequestEditEmail]=@OfferRequestEditEmail" + i + ","
					+ "[OfferRequestSendEmail]=@OfferRequestSendEmail" + i + ","
					+ "[OfferRequestView]=@OfferRequestView" + i + ","
					+ "[PackagingsLgtPhotoAdd]=@PackagingsLgtPhotoAdd" + i + ","
					+ "[PackagingsLgtPhotoDelete]=@PackagingsLgtPhotoDelete" + i + ","
					+ "[PackagingsLgtPhotoView]=@PackagingsLgtPhotoView" + i + ","
					+ "[PayementPractises]=@PayementPractises" + i + ","
					+ "[PMAddCable]=@PMAddCable" + i + ","
					+ "[PMAddMileStone]=@PMAddMileStone" + i + ","
					+ "[PMAddProject]=@PMAddProject" + i + ","
					+ "[PMDeleteCable]=@PMDeleteCable" + i + ","
					+ "[PMDeleteMileStone]=@PMDeleteMileStone" + i + ","
					+ "[PMDeleteProject]=@PMDeleteProject" + i + ","
					+ "[PMEditCable]=@PMEditCable" + i + ","
					+ "[PMEditMileStone]=@PMEditMileStone" + i + ","
					+ "[PMEditProject]=@PMEditProject" + i + ","
					+ "[PMModule]=@PMModule" + i + ","
					+ "[PMViewProjectsCompact]=@PMViewProjectsCompact" + i + ","
					+ "[PMViewProjectsDetail]=@PMViewProjectsDetail" + i + ","
					+ "[PMViewProjectsMedium]=@PMViewProjectsMedium" + i + ","
					+ "[PricingGroup]=@PricingGroup" + i + ","
					+ "[RemoveArticleReference]=@RemoveArticleReference" + i + ","
					+ "[Settings]=@Settings" + i + ","
					+ "[ShippingMethods]=@ShippingMethods" + i + ","
					+ "[SlipCircle]=@SlipCircle" + i + ","
					+ "[SupplierAddress]=@SupplierAddress" + i + ","
					+ "[SupplierAttachementAddFile]=@SupplierAttachementAddFile" + i + ","
					+ "[SupplierAttachementGetFile]=@SupplierAttachementGetFile" + i + ","
					+ "[SupplierAttachementRemoveFile]=@SupplierAttachementRemoveFile" + i + ","
					+ "[SupplierCommunication]=@SupplierCommunication" + i + ","
					+ "[SupplierContactPerson]=@SupplierContactPerson" + i + ","
					+ "[SupplierData]=@SupplierData" + i + ","
					+ "[SupplierGroup]=@SupplierGroup" + i + ","
					+ "[SupplierHistory]=@SupplierHistory" + i + ","
					+ "[SupplierOverview]=@SupplierOverview" + i + ","
					+ "[Suppliers]=@Suppliers" + i + ","
					+ "[SupplierShipping]=@SupplierShipping" + i + ","
					+ "[TermsOfPayment]=@TermsOfPayment" + i + ","
					+ "[UploadAltPositionsArticleBOM]=@UploadAltPositionsArticleBOM" + i + ","
					+ "[UploadArticleBOM]=@UploadArticleBOM" + i + ","
					+ "[ValidateArticleBOM]=@ValidateArticleBOM" + i + ","
					+ "[ValidateBCR]=@ValidateBCR" + i + ","
					+ "[ViewAltPositionsArticleBOM]=@ViewAltPositionsArticleBOM" + i + ","
					+ "[ViewArticleLog]=@ViewArticleLog" + i + ","
					+ "[ViewArticleReference]=@ViewArticleReference" + i + ","
					+ "[ViewArticles]=@ViewArticles" + i + ","
					+ "[ViewBCR]=@ViewBCR" + i + ","
					+ "[ViewCustomers]=@ViewCustomers" + i + ","
					+ "[ViewLPCustomer]=@ViewLPCustomer" + i + ","
					+ "[ViewLPSupplier]=@ViewLPSupplier" + i + ","
					+ "[ViewSupplierAddressComments]=@ViewSupplierAddressComments" + i + ","
					+ "[ViewSuppliers]=@ViewSuppliers" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("AddArticle" + i, item.AddArticle);
					sqlCommand.Parameters.AddWithValue("AddArticleReference" + i, item.AddArticleReference == null ? (object)DBNull.Value : item.AddArticleReference);
					sqlCommand.Parameters.AddWithValue("AddBCR" + i, item.AddBCR == null ? (object)DBNull.Value : item.AddBCR);
					sqlCommand.Parameters.AddWithValue("AddCocType" + i, item.AddCocType == null ? (object)DBNull.Value : item.AddCocType);
					sqlCommand.Parameters.AddWithValue("AddConditionAssignment" + i, item.AddConditionAssignment == null ? (object)DBNull.Value : item.AddConditionAssignment);
					sqlCommand.Parameters.AddWithValue("AddContactAddress" + i, item.AddContactAddress == null ? (object)DBNull.Value : item.AddContactAddress);
					sqlCommand.Parameters.AddWithValue("AddContactSalutation" + i, item.AddContactSalutation == null ? (object)DBNull.Value : item.AddContactSalutation);
					sqlCommand.Parameters.AddWithValue("AddCurrencies" + i, item.AddCurrencies == null ? (object)DBNull.Value : item.AddCurrencies);
					sqlCommand.Parameters.AddWithValue("AddCustomer" + i, item.AddCustomer == null ? (object)DBNull.Value : item.AddCustomer);
					sqlCommand.Parameters.AddWithValue("AddCustomerGroup" + i, item.AddCustomerGroup == null ? (object)DBNull.Value : item.AddCustomerGroup);
					sqlCommand.Parameters.AddWithValue("AddCustomerItemNumber" + i, item.AddCustomerItemNumber == null ? (object)DBNull.Value : item.AddCustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("AddDiscountGroup" + i, item.AddDiscountGroup == null ? (object)DBNull.Value : item.AddDiscountGroup);
					sqlCommand.Parameters.AddWithValue("AddEdiConcern" + i, item.AddEdiConcern == null ? (object)DBNull.Value : item.AddEdiConcern);
					sqlCommand.Parameters.AddWithValue("AddFibuFrame" + i, item.AddFibuFrame == null ? (object)DBNull.Value : item.AddFibuFrame);
					sqlCommand.Parameters.AddWithValue("AddFiles" + i, item.AddFiles == null ? (object)DBNull.Value : item.AddFiles);
					sqlCommand.Parameters.AddWithValue("AddHourlyRate" + i, item.AddHourlyRate == null ? (object)DBNull.Value : item.AddHourlyRate);
					sqlCommand.Parameters.AddWithValue("AddIndustry" + i, item.AddIndustry == null ? (object)DBNull.Value : item.AddIndustry);
					sqlCommand.Parameters.AddWithValue("AddPayementPractises" + i, item.AddPayementPractises == null ? (object)DBNull.Value : item.AddPayementPractises);
					sqlCommand.Parameters.AddWithValue("AddPricingGroup" + i, item.AddPricingGroup == null ? (object)DBNull.Value : item.AddPricingGroup);
					sqlCommand.Parameters.AddWithValue("AddRohArtikelNummer" + i, item.AddRohArtikelNummer == null ? (object)DBNull.Value : item.AddRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("AddShippingMethods" + i, item.AddShippingMethods == null ? (object)DBNull.Value : item.AddShippingMethods);
					sqlCommand.Parameters.AddWithValue("AddSlipCircle" + i, item.AddSlipCircle == null ? (object)DBNull.Value : item.AddSlipCircle);
					sqlCommand.Parameters.AddWithValue("AddSupplier" + i, item.AddSupplier == null ? (object)DBNull.Value : item.AddSupplier);
					sqlCommand.Parameters.AddWithValue("AddSupplierGroup" + i, item.AddSupplierGroup == null ? (object)DBNull.Value : item.AddSupplierGroup);
					sqlCommand.Parameters.AddWithValue("AddTermsOfPayment" + i, item.AddTermsOfPayment == null ? (object)DBNull.Value : item.AddTermsOfPayment);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration);
					sqlCommand.Parameters.AddWithValue("AltPositionsArticleBOM" + i, item.AltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("ArchiveArticle" + i, item.ArchiveArticle);
					sqlCommand.Parameters.AddWithValue("ArchiveCustomer" + i, item.ArchiveCustomer == null ? (object)DBNull.Value : item.ArchiveCustomer);
					sqlCommand.Parameters.AddWithValue("ArchiveSupplier" + i, item.ArchiveSupplier == null ? (object)DBNull.Value : item.ArchiveSupplier);
					sqlCommand.Parameters.AddWithValue("ArticleAddCustomerDocument" + i, item.ArticleAddCustomerDocument == null ? (object)DBNull.Value : item.ArticleAddCustomerDocument);
					sqlCommand.Parameters.AddWithValue("ArticleBOM" + i, item.ArticleBOM);
					sqlCommand.Parameters.AddWithValue("ArticleCts" + i, item.ArticleCts);
					sqlCommand.Parameters.AddWithValue("ArticleData" + i, item.ArticleData);
					sqlCommand.Parameters.AddWithValue("ArticleDeleteCustomerDocument" + i, item.ArticleDeleteCustomerDocument == null ? (object)DBNull.Value : item.ArticleDeleteCustomerDocument);
					sqlCommand.Parameters.AddWithValue("ArticleLogistics" + i, item.ArticleLogistics);
					sqlCommand.Parameters.AddWithValue("ArticleLogisticsPrices" + i, item.ArticleLogisticsPrices == null ? (object)DBNull.Value : item.ArticleLogisticsPrices);
					sqlCommand.Parameters.AddWithValue("ArticleOverview" + i, item.ArticleOverview);
					sqlCommand.Parameters.AddWithValue("ArticleProduction" + i, item.ArticleProduction);
					sqlCommand.Parameters.AddWithValue("ArticlePurchase" + i, item.ArticlePurchase);
					sqlCommand.Parameters.AddWithValue("ArticleQuality" + i, item.ArticleQuality);
					sqlCommand.Parameters.AddWithValue("Articles" + i, item.Articles);
					sqlCommand.Parameters.AddWithValue("ArticleSales" + i, item.ArticleSales);
					sqlCommand.Parameters.AddWithValue("ArticleSalesCustom" + i, item.ArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("ArticleSalesItem" + i, item.ArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlEngineering" + i, item.ArticlesBOMCPControlEngineering == null ? (object)DBNull.Value : item.ArticlesBOMCPControlEngineering);
					sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlHistory" + i, item.ArticlesBOMCPControlHistory == null ? (object)DBNull.Value : item.ArticlesBOMCPControlHistory);
					sqlCommand.Parameters.AddWithValue("ArticlesBOMCPControlQuality" + i, item.ArticlesBOMCPControlQuality == null ? (object)DBNull.Value : item.ArticlesBOMCPControlQuality);
					sqlCommand.Parameters.AddWithValue("ArticleStatistics" + i, item.ArticleStatistics);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineering" + i, item.ArticleStatisticsEngineering);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsEngineeringEdit" + i, item.ArticleStatisticsEngineeringEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccounting" + i, item.ArticleStatisticsFinanceAccounting);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsFinanceAccountingEdit" + i, item.ArticleStatisticsFinanceAccountingEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogistics" + i, item.ArticleStatisticsLogistics);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsLogisticsEdit" + i, item.ArticleStatisticsLogisticsEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchase" + i, item.ArticleStatisticsPurchase);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsPurchaseEdit" + i, item.ArticleStatisticsPurchaseEdit);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnic" + i, item.ArticleStatisticsTechnic);
					sqlCommand.Parameters.AddWithValue("ArticleStatisticsTechnicEdit" + i, item.ArticleStatisticsTechnicEdit);
					sqlCommand.Parameters.AddWithValue("BomChangeRequests" + i, item.BomChangeRequests == null ? (object)DBNull.Value : item.BomChangeRequests);
					sqlCommand.Parameters.AddWithValue("CocType" + i, item.CocType == null ? (object)DBNull.Value : item.CocType);
					sqlCommand.Parameters.AddWithValue("ConditionAssignment" + i, item.ConditionAssignment == null ? (object)DBNull.Value : item.ConditionAssignment);
					sqlCommand.Parameters.AddWithValue("ConfigArticle" + i, item.ConfigArticle);
					sqlCommand.Parameters.AddWithValue("ConfigCustomer" + i, item.ConfigCustomer == null ? (object)DBNull.Value : item.ConfigCustomer);
					sqlCommand.Parameters.AddWithValue("ConfigSupplier" + i, item.ConfigSupplier == null ? (object)DBNull.Value : item.ConfigSupplier);
					sqlCommand.Parameters.AddWithValue("ContactAddress" + i, item.ContactAddress == null ? (object)DBNull.Value : item.ContactAddress);
					sqlCommand.Parameters.AddWithValue("ContactSalutation" + i, item.ContactSalutation == null ? (object)DBNull.Value : item.ContactSalutation);
					sqlCommand.Parameters.AddWithValue("CreateArticlePurchase" + i, item.CreateArticlePurchase);
					sqlCommand.Parameters.AddWithValue("CreateArticleSalesCustom" + i, item.CreateArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("CreateArticleSalesItem" + i, item.CreateArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("CreateCustomerContactPerson" + i, item.CreateCustomerContactPerson == null ? (object)DBNull.Value : item.CreateCustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("CreateSupplierContactPerson" + i, item.CreateSupplierContactPerson == null ? (object)DBNull.Value : item.CreateSupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Currencies" + i, item.Currencies == null ? (object)DBNull.Value : item.Currencies);
					sqlCommand.Parameters.AddWithValue("CustomerAddress" + i, item.CustomerAddress == null ? (object)DBNull.Value : item.CustomerAddress);
					sqlCommand.Parameters.AddWithValue("CustomerCommunication" + i, item.CustomerCommunication == null ? (object)DBNull.Value : item.CustomerCommunication);
					sqlCommand.Parameters.AddWithValue("CustomerContactPerson" + i, item.CustomerContactPerson == null ? (object)DBNull.Value : item.CustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("CustomerData" + i, item.CustomerData == null ? (object)DBNull.Value : item.CustomerData);
					sqlCommand.Parameters.AddWithValue("CustomerGroup" + i, item.CustomerGroup == null ? (object)DBNull.Value : item.CustomerGroup);
					sqlCommand.Parameters.AddWithValue("CustomerHistory" + i, item.CustomerHistory == null ? (object)DBNull.Value : item.CustomerHistory);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("CustomerOverview" + i, item.CustomerOverview == null ? (object)DBNull.Value : item.CustomerOverview);
					sqlCommand.Parameters.AddWithValue("Customers" + i, item.Customers);
					sqlCommand.Parameters.AddWithValue("CustomerShipping" + i, item.CustomerShipping == null ? (object)DBNull.Value : item.CustomerShipping);
					sqlCommand.Parameters.AddWithValue("DeleteAltPositionsArticleBOM" + i, item.DeleteAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("DeleteArticle" + i, item.DeleteArticle == null ? (object)DBNull.Value : item.DeleteArticle);
					sqlCommand.Parameters.AddWithValue("DeleteArticleBOM" + i, item.DeleteArticleBOM);
					sqlCommand.Parameters.AddWithValue("DeleteArticlePurchase" + i, item.DeleteArticlePurchase);
					sqlCommand.Parameters.AddWithValue("DeleteArticleSalesCustom" + i, item.DeleteArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("DeleteArticleSalesItem" + i, item.DeleteArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("DeleteBCR" + i, item.DeleteBCR == null ? (object)DBNull.Value : item.DeleteBCR);
					sqlCommand.Parameters.AddWithValue("DeleteCustomerContactPerson" + i, item.DeleteCustomerContactPerson == null ? (object)DBNull.Value : item.DeleteCustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("DeleteFiles" + i, item.DeleteFiles == null ? (object)DBNull.Value : item.DeleteFiles);
					sqlCommand.Parameters.AddWithValue("DeleteRohArtikelNummer" + i, item.DeleteRohArtikelNummer == null ? (object)DBNull.Value : item.DeleteRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("DeleteSupplierContactPerson" + i, item.DeleteSupplierContactPerson == null ? (object)DBNull.Value : item.DeleteSupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("DiscountGroup" + i, item.DiscountGroup == null ? (object)DBNull.Value : item.DiscountGroup);
					sqlCommand.Parameters.AddWithValue("DownloadAllOutdatedEinkaufsPreis" + i, item.DownloadAllOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadAllOutdatedEinkaufsPreis);
					sqlCommand.Parameters.AddWithValue("DownloadFiles" + i, item.DownloadFiles == null ? (object)DBNull.Value : item.DownloadFiles);
					sqlCommand.Parameters.AddWithValue("DownloadOutdatedEinkaufsPreis" + i, item.DownloadOutdatedEinkaufsPreis == null ? (object)DBNull.Value : item.DownloadOutdatedEinkaufsPreis);
					sqlCommand.Parameters.AddWithValue("EdiConcern" + i, item.EdiConcern == null ? (object)DBNull.Value : item.EdiConcern);
					sqlCommand.Parameters.AddWithValue("EditAltPositionsArticleBOM" + i, item.EditAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("EditArticle" + i, item.EditArticle);
					sqlCommand.Parameters.AddWithValue("EditArticleBOM" + i, item.EditArticleBOM);
					sqlCommand.Parameters.AddWithValue("EditArticleBOMPosition" + i, item.EditArticleBOMPosition);
					sqlCommand.Parameters.AddWithValue("EditArticleCts" + i, item.EditArticleCts);
					sqlCommand.Parameters.AddWithValue("EditArticleData" + i, item.EditArticleData);
					sqlCommand.Parameters.AddWithValue("EditArticleDesignation" + i, item.EditArticleDesignation);
					sqlCommand.Parameters.AddWithValue("EditArticleImage" + i, item.EditArticleImage);
					sqlCommand.Parameters.AddWithValue("EditArticleLogistics" + i, item.EditArticleLogistics);
					sqlCommand.Parameters.AddWithValue("EditArticleManager" + i, item.EditArticleManager);
					sqlCommand.Parameters.AddWithValue("EditArticleProduction" + i, item.EditArticleProduction);
					sqlCommand.Parameters.AddWithValue("EditArticlePurchase" + i, item.EditArticlePurchase);
					sqlCommand.Parameters.AddWithValue("EditArticleQuality" + i, item.EditArticleQuality);
					sqlCommand.Parameters.AddWithValue("EditArticleReference" + i, item.EditArticleReference == null ? (object)DBNull.Value : item.EditArticleReference);
					sqlCommand.Parameters.AddWithValue("EditArticleSalesCustom" + i, item.EditArticleSalesCustom);
					sqlCommand.Parameters.AddWithValue("EditArticleSalesItem" + i, item.EditArticleSalesItem);
					sqlCommand.Parameters.AddWithValue("EditCocType" + i, item.EditCocType == null ? (object)DBNull.Value : item.EditCocType);
					sqlCommand.Parameters.AddWithValue("EditConditionAssignment" + i, item.EditConditionAssignment == null ? (object)DBNull.Value : item.EditConditionAssignment);
					sqlCommand.Parameters.AddWithValue("EditContactAddress" + i, item.EditContactAddress == null ? (object)DBNull.Value : item.EditContactAddress);
					sqlCommand.Parameters.AddWithValue("EditContactSalutation" + i, item.EditContactSalutation == null ? (object)DBNull.Value : item.EditContactSalutation);
					sqlCommand.Parameters.AddWithValue("EditCurrencies" + i, item.EditCurrencies == null ? (object)DBNull.Value : item.EditCurrencies);
					sqlCommand.Parameters.AddWithValue("EditCustomer" + i, item.EditCustomer == null ? (object)DBNull.Value : item.EditCustomer);
					sqlCommand.Parameters.AddWithValue("EditCustomerAddress" + i, item.EditCustomerAddress == null ? (object)DBNull.Value : item.EditCustomerAddress);
					sqlCommand.Parameters.AddWithValue("EditCustomerCommunication" + i, item.EditCustomerCommunication == null ? (object)DBNull.Value : item.EditCustomerCommunication);
					sqlCommand.Parameters.AddWithValue("EditCustomerContactPerson" + i, item.EditCustomerContactPerson == null ? (object)DBNull.Value : item.EditCustomerContactPerson);
					sqlCommand.Parameters.AddWithValue("EditCustomerCoordination" + i, item.EditCustomerCoordination == null ? (object)DBNull.Value : item.EditCustomerCoordination);
					sqlCommand.Parameters.AddWithValue("EditCustomerData" + i, item.EditCustomerData == null ? (object)DBNull.Value : item.EditCustomerData);
					sqlCommand.Parameters.AddWithValue("EditCustomerGroup" + i, item.EditCustomerGroup == null ? (object)DBNull.Value : item.EditCustomerGroup);
					sqlCommand.Parameters.AddWithValue("EditCustomerImage" + i, item.EditCustomerImage == null ? (object)DBNull.Value : item.EditCustomerImage);
					sqlCommand.Parameters.AddWithValue("EditCustomerItemNumber" + i, item.EditCustomerItemNumber == null ? (object)DBNull.Value : item.EditCustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("EditCustomerShipping" + i, item.EditCustomerShipping == null ? (object)DBNull.Value : item.EditCustomerShipping);
					sqlCommand.Parameters.AddWithValue("EditDiscountGroup" + i, item.EditDiscountGroup == null ? (object)DBNull.Value : item.EditDiscountGroup);
					sqlCommand.Parameters.AddWithValue("EditEdiConcern" + i, item.EditEdiConcern == null ? (object)DBNull.Value : item.EditEdiConcern);
					sqlCommand.Parameters.AddWithValue("EditFibuFrame" + i, item.EditFibuFrame == null ? (object)DBNull.Value : item.EditFibuFrame);
					sqlCommand.Parameters.AddWithValue("EditHourlyRate" + i, item.EditHourlyRate == null ? (object)DBNull.Value : item.EditHourlyRate);
					sqlCommand.Parameters.AddWithValue("EditIndustry" + i, item.EditIndustry == null ? (object)DBNull.Value : item.EditIndustry);
					sqlCommand.Parameters.AddWithValue("EditLagerCCID" + i, item.EditLagerCCID == null ? (object)DBNull.Value : item.EditLagerCCID);
					sqlCommand.Parameters.AddWithValue("EditLagerMinStock" + i, item.EditLagerMinStock);
					sqlCommand.Parameters.AddWithValue("EditLagerOrderProposal" + i, item.EditLagerOrderProposal == null ? (object)DBNull.Value : item.EditLagerOrderProposal);
					sqlCommand.Parameters.AddWithValue("EditLagerStock" + i, item.EditLagerStock == null ? (object)DBNull.Value : item.EditLagerStock);
					sqlCommand.Parameters.AddWithValue("EditPayementPractises" + i, item.EditPayementPractises == null ? (object)DBNull.Value : item.EditPayementPractises);
					sqlCommand.Parameters.AddWithValue("EditPricingGroup" + i, item.EditPricingGroup == null ? (object)DBNull.Value : item.EditPricingGroup);
					sqlCommand.Parameters.AddWithValue("EditRohArtikelNummer" + i, item.EditRohArtikelNummer == null ? (object)DBNull.Value : item.EditRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("EditShippingMethods" + i, item.EditShippingMethods == null ? (object)DBNull.Value : item.EditShippingMethods);
					sqlCommand.Parameters.AddWithValue("EditSlipCircle" + i, item.EditSlipCircle == null ? (object)DBNull.Value : item.EditSlipCircle);
					sqlCommand.Parameters.AddWithValue("EditSupplier" + i, item.EditSupplier == null ? (object)DBNull.Value : item.EditSupplier);
					sqlCommand.Parameters.AddWithValue("EditSupplierAddress" + i, item.EditSupplierAddress == null ? (object)DBNull.Value : item.EditSupplierAddress);
					sqlCommand.Parameters.AddWithValue("EditSupplierCommunication" + i, item.EditSupplierCommunication == null ? (object)DBNull.Value : item.EditSupplierCommunication);
					sqlCommand.Parameters.AddWithValue("EditSupplierContactPerson" + i, item.EditSupplierContactPerson == null ? (object)DBNull.Value : item.EditSupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("EditSupplierCoordination" + i, item.EditSupplierCoordination == null ? (object)DBNull.Value : item.EditSupplierCoordination);
					sqlCommand.Parameters.AddWithValue("EditSupplierData" + i, item.EditSupplierData == null ? (object)DBNull.Value : item.EditSupplierData);
					sqlCommand.Parameters.AddWithValue("EditSupplierGroup" + i, item.EditSupplierGroup == null ? (object)DBNull.Value : item.EditSupplierGroup);
					sqlCommand.Parameters.AddWithValue("EditSupplierImage" + i, item.EditSupplierImage == null ? (object)DBNull.Value : item.EditSupplierImage);
					sqlCommand.Parameters.AddWithValue("EditSupplierShipping" + i, item.EditSupplierShipping == null ? (object)DBNull.Value : item.EditSupplierShipping);
					sqlCommand.Parameters.AddWithValue("EditTermsOfPayment" + i, item.EditTermsOfPayment == null ? (object)DBNull.Value : item.EditTermsOfPayment);
					sqlCommand.Parameters.AddWithValue("EDrawingEdit" + i, item.EDrawingEdit == null ? (object)DBNull.Value : item.EDrawingEdit);
					sqlCommand.Parameters.AddWithValue("EinkaufsPreisUpdate" + i, item.EinkaufsPreisUpdate == null ? (object)DBNull.Value : item.EinkaufsPreisUpdate);
					sqlCommand.Parameters.AddWithValue("FibuFrame" + i, item.FibuFrame == null ? (object)DBNull.Value : item.FibuFrame);
					sqlCommand.Parameters.AddWithValue("GetRohArtikelNummer" + i, item.GetRohArtikelNummer == null ? (object)DBNull.Value : item.GetRohArtikelNummer);
					sqlCommand.Parameters.AddWithValue("HourlyRate" + i, item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
					sqlCommand.Parameters.AddWithValue("ImportArticleBOM" + i, item.ImportArticleBOM);
					sqlCommand.Parameters.AddWithValue("Industry" + i, item.Industry == null ? (object)DBNull.Value : item.Industry);
					sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
					sqlCommand.Parameters.AddWithValue("LagerArticleLogistics" + i, item.LagerArticleLogistics);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("ModuleAdministrator" + i, item.ModuleAdministrator == null ? (object)DBNull.Value : item.ModuleAdministrator);
					sqlCommand.Parameters.AddWithValue("offer" + i, item.offer == null ? (object)DBNull.Value : item.offer);
					sqlCommand.Parameters.AddWithValue("OfferRequestADD" + i, item.OfferRequestADD == null ? (object)DBNull.Value : item.OfferRequestADD);
					sqlCommand.Parameters.AddWithValue("OfferRequestApplyPrice" + i, item.OfferRequestApplyPrice == null ? (object)DBNull.Value : item.OfferRequestApplyPrice);
					sqlCommand.Parameters.AddWithValue("OfferRequestDelete" + i, item.OfferRequestDelete == null ? (object)DBNull.Value : item.OfferRequestDelete);
					sqlCommand.Parameters.AddWithValue("OfferRequestEdit" + i, item.OfferRequestEdit == null ? (object)DBNull.Value : item.OfferRequestEdit);
					sqlCommand.Parameters.AddWithValue("OfferRequestEditEmail" + i, item.OfferRequestEditEmail == null ? (object)DBNull.Value : item.OfferRequestEditEmail);
					sqlCommand.Parameters.AddWithValue("OfferRequestSendEmail" + i, item.OfferRequestSendEmail == null ? (object)DBNull.Value : item.OfferRequestSendEmail);
					sqlCommand.Parameters.AddWithValue("OfferRequestView" + i, item.OfferRequestView == null ? (object)DBNull.Value : item.OfferRequestView);
					sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoAdd" + i, item.PackagingsLgtPhotoAdd == null ? (object)DBNull.Value : item.PackagingsLgtPhotoAdd);
					sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoDelete" + i, item.PackagingsLgtPhotoDelete == null ? (object)DBNull.Value : item.PackagingsLgtPhotoDelete);
					sqlCommand.Parameters.AddWithValue("PackagingsLgtPhotoView" + i, item.PackagingsLgtPhotoView == null ? (object)DBNull.Value : item.PackagingsLgtPhotoView);
					sqlCommand.Parameters.AddWithValue("PayementPractises" + i, item.PayementPractises == null ? (object)DBNull.Value : item.PayementPractises);
					sqlCommand.Parameters.AddWithValue("PMAddCable" + i, item.PMAddCable == null ? (object)DBNull.Value : item.PMAddCable);
					sqlCommand.Parameters.AddWithValue("PMAddMileStone" + i, item.PMAddMileStone == null ? (object)DBNull.Value : item.PMAddMileStone);
					sqlCommand.Parameters.AddWithValue("PMAddProject" + i, item.PMAddProject == null ? (object)DBNull.Value : item.PMAddProject);
					sqlCommand.Parameters.AddWithValue("PMDeleteCable" + i, item.PMDeleteCable == null ? (object)DBNull.Value : item.PMDeleteCable);
					sqlCommand.Parameters.AddWithValue("PMDeleteMileStone" + i, item.PMDeleteMileStone == null ? (object)DBNull.Value : item.PMDeleteMileStone);
					sqlCommand.Parameters.AddWithValue("PMDeleteProject" + i, item.PMDeleteProject == null ? (object)DBNull.Value : item.PMDeleteProject);
					sqlCommand.Parameters.AddWithValue("PMEditCable" + i, item.PMEditCable == null ? (object)DBNull.Value : item.PMEditCable);
					sqlCommand.Parameters.AddWithValue("PMEditMileStone" + i, item.PMEditMileStone == null ? (object)DBNull.Value : item.PMEditMileStone);
					sqlCommand.Parameters.AddWithValue("PMEditProject" + i, item.PMEditProject == null ? (object)DBNull.Value : item.PMEditProject);
					sqlCommand.Parameters.AddWithValue("PMModule" + i, item.PMModule == null ? (object)DBNull.Value : item.PMModule);
					sqlCommand.Parameters.AddWithValue("PMViewProjectsCompact" + i, item.PMViewProjectsCompact == null ? (object)DBNull.Value : item.PMViewProjectsCompact);
					sqlCommand.Parameters.AddWithValue("PMViewProjectsDetail" + i, item.PMViewProjectsDetail == null ? (object)DBNull.Value : item.PMViewProjectsDetail);
					sqlCommand.Parameters.AddWithValue("PMViewProjectsMedium" + i, item.PMViewProjectsMedium == null ? (object)DBNull.Value : item.PMViewProjectsMedium);
					sqlCommand.Parameters.AddWithValue("PricingGroup" + i, item.PricingGroup == null ? (object)DBNull.Value : item.PricingGroup);
					sqlCommand.Parameters.AddWithValue("RemoveArticleReference" + i, item.RemoveArticleReference == null ? (object)DBNull.Value : item.RemoveArticleReference);
					sqlCommand.Parameters.AddWithValue("Settings" + i, item.Settings == null ? (object)DBNull.Value : item.Settings);
					sqlCommand.Parameters.AddWithValue("ShippingMethods" + i, item.ShippingMethods == null ? (object)DBNull.Value : item.ShippingMethods);
					sqlCommand.Parameters.AddWithValue("SlipCircle" + i, item.SlipCircle == null ? (object)DBNull.Value : item.SlipCircle);
					sqlCommand.Parameters.AddWithValue("SupplierAddress" + i, item.SupplierAddress == null ? (object)DBNull.Value : item.SupplierAddress);
					sqlCommand.Parameters.AddWithValue("SupplierAttachementAddFile" + i, item.SupplierAttachementAddFile == null ? (object)DBNull.Value : item.SupplierAttachementAddFile);
					sqlCommand.Parameters.AddWithValue("SupplierAttachementGetFile" + i, item.SupplierAttachementGetFile == null ? (object)DBNull.Value : item.SupplierAttachementGetFile);
					sqlCommand.Parameters.AddWithValue("SupplierAttachementRemoveFile" + i, item.SupplierAttachementRemoveFile == null ? (object)DBNull.Value : item.SupplierAttachementRemoveFile);
					sqlCommand.Parameters.AddWithValue("SupplierCommunication" + i, item.SupplierCommunication == null ? (object)DBNull.Value : item.SupplierCommunication);
					sqlCommand.Parameters.AddWithValue("SupplierContactPerson" + i, item.SupplierContactPerson == null ? (object)DBNull.Value : item.SupplierContactPerson);
					sqlCommand.Parameters.AddWithValue("SupplierData" + i, item.SupplierData == null ? (object)DBNull.Value : item.SupplierData);
					sqlCommand.Parameters.AddWithValue("SupplierGroup" + i, item.SupplierGroup == null ? (object)DBNull.Value : item.SupplierGroup);
					sqlCommand.Parameters.AddWithValue("SupplierHistory" + i, item.SupplierHistory == null ? (object)DBNull.Value : item.SupplierHistory);
					sqlCommand.Parameters.AddWithValue("SupplierOverview" + i, item.SupplierOverview == null ? (object)DBNull.Value : item.SupplierOverview);
					sqlCommand.Parameters.AddWithValue("Suppliers" + i, item.Suppliers);
					sqlCommand.Parameters.AddWithValue("SupplierShipping" + i, item.SupplierShipping == null ? (object)DBNull.Value : item.SupplierShipping);
					sqlCommand.Parameters.AddWithValue("TermsOfPayment" + i, item.TermsOfPayment == null ? (object)DBNull.Value : item.TermsOfPayment);
					sqlCommand.Parameters.AddWithValue("UploadAltPositionsArticleBOM" + i, item.UploadAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("UploadArticleBOM" + i, item.UploadArticleBOM);
					sqlCommand.Parameters.AddWithValue("ValidateArticleBOM" + i, item.ValidateArticleBOM == null ? (object)DBNull.Value : item.ValidateArticleBOM);
					sqlCommand.Parameters.AddWithValue("ValidateBCR" + i, item.ValidateBCR == null ? (object)DBNull.Value : item.ValidateBCR);
					sqlCommand.Parameters.AddWithValue("ViewAltPositionsArticleBOM" + i, item.ViewAltPositionsArticleBOM);
					sqlCommand.Parameters.AddWithValue("ViewArticleLog" + i, item.ViewArticleLog);
					sqlCommand.Parameters.AddWithValue("ViewArticleReference" + i, item.ViewArticleReference == null ? (object)DBNull.Value : item.ViewArticleReference);
					sqlCommand.Parameters.AddWithValue("ViewArticles" + i, item.ViewArticles);
					sqlCommand.Parameters.AddWithValue("ViewBCR" + i, item.ViewBCR == null ? (object)DBNull.Value : item.ViewBCR);
					sqlCommand.Parameters.AddWithValue("ViewCustomers" + i, item.ViewCustomers == null ? (object)DBNull.Value : item.ViewCustomers);
					sqlCommand.Parameters.AddWithValue("ViewLPCustomer" + i, item.ViewLPCustomer == null ? (object)DBNull.Value : item.ViewLPCustomer);
					sqlCommand.Parameters.AddWithValue("ViewLPSupplier" + i, item.ViewLPSupplier == null ? (object)DBNull.Value : item.ViewLPSupplier);
					sqlCommand.Parameters.AddWithValue("ViewSupplierAddressComments" + i, item.ViewSupplierAddressComments == null ? (object)DBNull.Value : item.ViewSupplierAddressComments);
					sqlCommand.Parameters.AddWithValue("ViewSuppliers" + i, item.ViewSuppliers == null ? (object)DBNull.Value : item.ViewSuppliers);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_AccessProfile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = sqlCommand.ExecuteNonQuery();


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

				string query = "DELETE FROM [__BSD_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods



		#region Custom Methods
		public static List<Entities.Tables.BSD.AccessProfileEntity> GetByMainAccessProfilesIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.BSD.AccessProfileEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					response = getByMainAccessProfilesIds(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					response = new List<Entities.Tables.BSD.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(getByMainAccessProfilesIds(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					response.AddRange(getByMainAccessProfilesIds(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return response;
			}
			return new List<Entities.Tables.BSD.AccessProfileEntity>();
		}
		private static List<Entities.Tables.BSD.AccessProfileEntity> getByMainAccessProfilesIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [__BSD_AccessProfile] WHERE [Id] IN (" + queryIds + ")";

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.BSD.AccessProfileEntity>();
				}
			}
			return new List<Entities.Tables.BSD.AccessProfileEntity>();
		}
		public static Entities.Tables.BSD.AccessProfileEntity GetByMainAccessProfilesId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.BSD.AccessProfileEntity GetByName(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_AccessProfile] WHERE [AccessProfileName]=@name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name.Trim());

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity> GetDefaultProfiles(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__BSD_AccessProfile] WHERE [isDefault]=1", connection, transaction))
			{
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity>();
			}
		}
		public static int UpdateName(Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_AccessProfile] SET [AccessProfileName]=@AccessProfileName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		#endregion Custom Methods

	}
}
