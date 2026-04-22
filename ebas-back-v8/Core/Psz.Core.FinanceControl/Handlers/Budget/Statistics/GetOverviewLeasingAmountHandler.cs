using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetOverviewLeasingAmountHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetOverviewLeasingAmountHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var januaryAmount = 0m;
				var februaryAmount = 0m;
				var marchAmount = 0m;
				var AprilAmount = 0m;
				var MayAmount = 0m;
				var juneAmount = 0m;
				/// 
				var julyAmount = 0m;
				var augustAmount = 0m;
				var septemberAmount = 0m;
				var novemberAmount = 0m;
				var octoberAmount = 0m;
				var decemberAmount = 0m;

				var year = DateTime.Today.Year;
				var januaryDate = new DateTime(year, 1, 1);
				var februaryDate = new DateTime(year, 2, 1);
				var marchDate = new DateTime(year, 3, 1);
				var aprilDate = new DateTime(year, 4, 1);
				var mayDate = new DateTime(year, 5, 1);
				var juneDate = new DateTime(year, 6, 1);
				var julyDate = new DateTime(year, 7, 1);
				var augutsDate = new DateTime(year, 8, 1);
				var septemberDate = new DateTime(year, 9, 1);
				var octoberDate = new DateTime(year, 10, 1);
				var novemberDate = new DateTime(year, 11, 1);
				var decemberDate = new DateTime(year, 12, 1);
				if(this._user.IsGlobalDirector)
				{
					januaryAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(januaryDate);
					februaryAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(februaryDate);
					marchAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(marchDate);
					AprilAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(aprilDate);
					MayAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(mayDate);
					juneAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(juneDate);
					//-
					julyAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(julyDate);
					augustAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(augutsDate);
					septemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(septemberDate);
					octoberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(octoberDate);
					novemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(novemberDate);
					decemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(decemberDate);
				}
				else
				{
					var userEntiy = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);

					// - if user is director, show all PO in company
					var companyDirectorEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { userEntiy.Id });
					if(companyDirectorEntities != null && companyDirectorEntities.Count > 0)
					{
						januaryAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(januaryDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						februaryAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(februaryDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						marchAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(marchDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						AprilAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(aprilDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						MayAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(mayDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						juneAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(juneDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						// -
						julyAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(julyDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						augustAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(augutsDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						septemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(septemberDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						octoberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(octoberDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						novemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(novemberDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						decemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(decemberDate, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
					}

					// - if user has Purchase profile, show all PO in Company
					var userFNCProfileIds = (Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { this._user.Id })
						?? new List<Infrastructure.Data.Entities.Tables.FNC.UserAccessProfilesEntity>()).Select(x => x.AccessProfileId).ToList();
					var userProfileIds = (Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(userFNCProfileIds)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>())?.Select(x => x.MainAccessProfileId).ToList();
					var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntiy.CompanyId ?? -1);
					if(companyExtensionEntity != null && userProfileIds.Exists(x => x == companyExtensionEntity.PurchaseProfileId))
					{
						januaryAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(januaryDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						februaryAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(februaryDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						marchAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(marchDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						AprilAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(aprilDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						MayAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(mayDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						juneAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(juneDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						// -
						julyAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(julyDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						augustAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(augutsDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						septemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(septemberDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						octoberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(octoberDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						novemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(novemberDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						decemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(decemberDate, companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
					}

					// - if user is Head of Dept, show all PO in department
					var departmentHeadEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntiy.Id);
					if(departmentHeadEntities != null && departmentHeadEntities.Count > 0)
					{
						januaryAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(januaryDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						februaryAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(februaryDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						marchAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(marchDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						AprilAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(aprilDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						MayAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(mayDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						juneAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(juneDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						// - 
						julyAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(julyDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						augustAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(augutsDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						septemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(septemberDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						octoberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(octoberDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						novemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(novemberDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						decemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(decemberDate, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
					}

					// - add user issued PO
					januaryAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(januaryDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
					februaryAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(februaryDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
					marchAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(marchDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
					AprilAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(aprilDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
					MayAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(mayDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
					juneAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(juneDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
					// -
					julyAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(julyDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
					augustAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(augutsDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
					septemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(septemberDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
					octoberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(octoberDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
					novemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(novemberDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
					decemberAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingMonth_Amount(decemberDate, companyIds: null, departmentIds: null, employeeId: this._user.Id);
				}

				var responseBody = new List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel> {
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{januaryDate.ToString("MMM-yy")}", Value=$"{januaryAmount}", Count=0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{februaryDate.ToString("MMM-yy")}", Value=$"{februaryAmount}", Count=0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{marchDate.ToString("MMM-yy")}", Value=$"{marchAmount}", Count=0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{aprilDate.ToString("MMM-yy")}", Value=$"{AprilAmount}", Count=0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{mayDate.ToString("MMM-yy")}", Value=$"{MayAmount}", Count=0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{juneDate.ToString("MMM-yy")}", Value=$"{juneAmount}", Count=0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{julyDate.ToString("MMM-yy")}", Value=$"{julyAmount}", Count=0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{augutsDate.ToString("MMM-yy")}", Value=$"{augustAmount}", Count=0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{septemberDate.ToString("MMM-yy")}", Value=$"{septemberAmount}", Count=0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{octoberDate.ToString("MMM-yy")}", Value=$"{novemberAmount}", Count=0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{novemberDate.ToString("MMM-yy")}", Value=$"{octoberAmount}", Count=0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name=$"{decemberDate.ToString("MMM-yy")}", Value=$"{decemberAmount}", Count=0 },
				};
				return ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>>.SuccessResponse();
		}
	}

}
