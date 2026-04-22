using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class ReportHandler: IHandle<UserModel, ResponseModel<string>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		private string _languageCode { get; set; }
		public ReportHandler(int orderId, string langCode, UserModel user)
		{
			_user = user;
			_data = orderId;
			_languageCode = langCode;
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

				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntity?.CompanyId ?? -1);
				var langId = (companyExtensionEntity?.ReportDefaultLanguageId) ?? 0;
				if(!string.IsNullOrWhiteSpace(this._languageCode))
				{
					switch(this._languageCode.ToLower())
					{
						case "de":
							langId = (int)Infrastructure.Services.Reporting.Models.FNC.ReportLanguage.German;
							break;
						case "fr":
						case "tn":
							langId = (int)Infrastructure.Services.Reporting.Models.FNC.ReportLanguage.French;
							break;
						case "al":
							langId = (int)Infrastructure.Services.Reporting.Models.FNC.ReportLanguage.Albanian;
							break;
						case "cz":
							langId = (int)Infrastructure.Services.Reporting.Models.FNC.ReportLanguage.Czech;
							break;
						case "en":
						default:
							break;
					}
				}
				return ResponseModel<string>.SuccessResponse(generateReport(this._data, langId));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<string> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data);
			if(orderEntity == null)
				return ResponseModel<string>.FailureResponse(key: "1", value: "Order not found");

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<string>.FailureResponse(key: "1", value: "User not found");


			return ResponseModel<string>.SuccessResponse();
		}
		internal static string generateReport(int orderId, int? languageId)
		{
			var report = generateReportData(orderId, languageId, true);
			return Convert.ToBase64String(report);
		}
		public static byte[] generateReportData(int orderId, int? languageId, bool isFinal = false)
		{
			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(orderId);
			var bestellungEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(orderId);
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderEntity?.IssuerId ?? -1);

			var departmentEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderEntity.DepartmentId ?? -1);

			int billingCompanyId = userEntity?.CompanyId ?? -1;
			if(orderEntity.BillingCompanyId.HasValue && orderEntity.BillingCompanyId != userEntity?.CompanyId)
				billingCompanyId = orderEntity.BillingCompanyId.Value;

			var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(billingCompanyId);
			var company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(billingCompanyId)
				?? Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get()?[0];
			var shippingCompany = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderEntity?.DeliveryCompanyId ?? -1);

			var textBausteineEnity = Infrastructure.Data.Access.Tables.STG.Textbausteine_AB_LS_RG_GU_BAccess.Get()?[0];

			var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(orderId);
			var bestellteArticleEntites = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(orderId);
			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderEntity?.ProjectId ?? -1);

			var supplierEntity = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Get(orderEntity.SupplierId ?? -1);
			var addressEntity = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(supplierEntity?.Nummer ?? -1);
			var artikelEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(articleEntites.Select(x => x.ArticleId)?.ToList());
			var konditionEntity = Infrastructure.Data.Access.Tables.FNC.KonditionsZuordnungstabelleEntity.Get(supplierEntity?.Konditionszuordnungs_Nr ?? -1);

			var lang = languageId.HasValue
				? (Infrastructure.Services.Reporting.Models.FNC.ReportLanguage)languageId.Value
				: Infrastructure.Services.Reporting.Models.FNC.ReportLanguage.English;

			return Module.ReportingService.GenerateFNCOrderReport(
				new Infrastructure.Services.Reporting.Models.FNC.OrderModel(
					userEntity, companyExtensionEntity, company, shippingCompany, textBausteineEnity, projectEntity, orderEntity, bestellungEntity,
					articleEntites, bestellteArticleEntites, supplierEntity, addressEntity, artikelEntities, konditionEntity),
				new List<Infrastructure.Services.Reporting.Models.FNC.OrderTemplateModel>
				{
					new Infrastructure.Services.Reporting.Models.FNC.OrderTemplateModel(lang)
				}, isFinal);
		}
	}
}