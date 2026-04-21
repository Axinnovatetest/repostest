using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class RejectForPastBudgetHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Budget.Order.RejectModel _data { get; set; }
		public RejectForPastBudgetHandler(Models.Budget.Order.RejectModel validateData, UserModel user)
		{
			_user = user;
			_data = validateData;
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

				//
				if(saveVersionHistory())
				{
					rejectOrder();
				}

				return ResponseModel<int>.SuccessResponse(0);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null
                || (!this._user.IsGlobalDirector && !this._user.SuperAdministrator && !this._user.IsAdministrator && !this._user.IsCorporateDirector))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// do nothing
			if(this._data.CurrentStep == 0)
				return ResponseModel<int>.FailureResponse("Draft Order can not be rejected");

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse("User not found");

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			if(orderEntity == null)
				return ResponseModel<int>.FailureResponse("Order not found");

			if(orderEntity.Archived.HasValue && orderEntity.Archived.Value)
				return ResponseModel<int>.FailureResponse("Order is archived");

			var orderArticleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);
			if(orderArticleEntities == null || orderArticleEntities.Count <= 0)
				return ResponseModel<int>.FailureResponse("Order articles list empty");

			if(orderEntity.ValidationRequestTime?.Year >= DateTime.Now.Year)
				return ResponseModel<int>.FailureResponse("Cannot reject order in current or next budget");

			// - Multiple currencies - need to update Prices
			Helpers.Processings.Budget.Order.updateArticlePrices(new List<int> { this._data.OrderId });

			// --
			if(orderEntity.OrderType == Enums.BudgetEnums.ProjectTypes.Finance.GetDescription())
			{ // Finance
				if(orderEntity.Status.HasValue && orderEntity.Status.Value > this._data.CurrentStep)
					return ResponseModel<int>.FailureResponse("Cannot reject order that is already validated");
			}
			else
			{
				if(orderEntity.Level.HasValue && orderEntity.Level.Value > this._data.CurrentStep)
					return ResponseModel<int>.FailureResponse("Cannot reject order that is already validated");
			}

			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderEntity.ProjectId ?? -1);
			if(projectEntity != null)
			{
				if(projectEntity.Closed.HasValue && projectEntity.Closed.Value == true)
					return ResponseModel<int>.FailureResponse($"Project [{projectEntity.ProjectName}] has been closed");

				if(projectEntity.Archived.HasValue && projectEntity.Archived.Value)
					return ResponseModel<int>.FailureResponse("Project is archived");

				if(projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive)
					return ResponseModel<int>.FailureResponse($"Project [{projectEntity.ProjectName}] has been deactivated");

			}

			return ResponseModel<int>.SuccessResponse();
		}

		internal bool saveVersionHistory()
		{
			try
			{
				var orderExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
				var orderArticleEntites = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(this._data.OrderId);
				var orderArticleExtensionEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);

				// Save Order
				Infrastructure.Data.Access.Tables.FNC.Budget_Order_VersionAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity
				{
					Id_Order = orderExtensionEntity.OrderId,
					Dept_name = orderExtensionEntity.DepartmentName,
					Id_Currency_Order = orderExtensionEntity.CurrencyId,
					Id_Dept = orderExtensionEntity.DepartmentId,
					Id_Land = orderExtensionEntity.CompanyId,
					Land_name = orderExtensionEntity.CompanyName,
					Id_Level = this._data.CurrentStep,
					Id_Project = orderExtensionEntity.ProjectId,
					Id_Status = (int)Enums.BudgetEnums.ValidationStatuses.Rejection,
					Id_Supplier_VersionOrder = orderExtensionEntity.SupplierId,
					Id_User = this._user.Id,
					Id_VO = -1,
					Nr_version_Order = -1, // >>>>>>>>>>>>>>>
					Step_Order = Enums.BudgetEnums.GetOrderValidationStatus(this._data.CurrentStep), // >>>>>>>>>>
					TotalCost_Order = (double?)orderArticleExtensionEntites.Select(x => x.TotalCost.GetValueOrDefault())?.Sum(),
					Version_Order_date = DateTime.Now
				});

				// Save articles
				Infrastructure.Data.Access.Tables.FNC.Budget_Article_VersionAccess.Insert(
					orderArticleExtensionEntites?.Select(x => new Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity
					{
						Action_Version_Article = Enums.BudgetEnums.ValidationStatuses.Validation.GetDescription(),
						Currency_Version_Article = x.CurrencyName,
						Dept_name_VersionArticle = orderExtensionEntity.DepartmentName,
						Id_AOV = -1,
						Id_Article = x.ArticleId,
						Id_Currency_Version_Article = x.CurrencyId,
						Id_Dept_VersionArticle = orderExtensionEntity.DepartmentId,
						Id_Land_VersionArticle = orderExtensionEntity.CompanyId,
						Id_Level_VersionArticle = this._data.CurrentStep,
						Id_Order_Version = orderExtensionEntity.OrderId,
						Id_Project_VersionArticle = orderExtensionEntity.ProjectId,
						Id_Status_VersionArticle = (int)Enums.BudgetEnums.ValidationStatuses.Rejection,
						Id_Supplier_VersionArticle = orderExtensionEntity.SupplierId,
						Id_User_VersionArticle = this._user.Id,
						Land_name_VersionArticle = orderExtensionEntity.CompanyName,
						Quantity_VersionArticle = x.Quantity,
						TotalCost__VersionArticle = (double?)x.TotalCost,
						Unit_Price_VersionArticle = (double?)x.UnitPrice,
						Version_Article_date = DateTime.Now
					})?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return false;
			}

			return true;
		}
		internal void rejectOrder()
		{
			var orderExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			var orderArticleExtensionEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);

			#region >>>>>> Budget DB

			// - workflow history
			Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtensionEntity, Enums.BudgetEnums.OrderWorkflowActions.Reject, this._user, ((Enums.BudgetEnums.ValidationLevels)this._data.CurrentStep).GetDescription());

			// - Rejection History 
			Infrastructure.Data.Access.Tables.FNC.OrderRejectionAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity
			{
				Id = -1,
				OrderId = orderExtensionEntity?.OrderId ?? -1,
				OrderArticleCount = orderArticleExtensionEntites?.Count ?? 0,
				OrderProjectId = orderExtensionEntity?.ProjectId ?? -1,
				OrderTotalAmount = 0,
				OrderType = orderExtensionEntity.OrderType,
				OrderUserId = orderExtensionEntity.IssuerId,
				UserId = this._user.Id,
				RejectionLevel = this._data.CurrentStep,
				RejectionNotes = this._data.Notes,
				RejectionTime = DateTime.Now
			});
			// - Rejection History 

			// Rejection
			orderExtensionEntity.Level = 0; // reset workflow
			orderExtensionEntity.Status = 0;
			orderExtensionEntity.ApprovalTime = null;
			orderExtensionEntity.ApprovalUserId = null;
			// Set Rejection flags
			orderExtensionEntity.LastRejectionLevel = this._data.CurrentStep;
			orderExtensionEntity.LastRejectionTime = DateTime.Now;
			orderExtensionEntity.LastRejectionUserId = this._user.Id;
			orderExtensionEntity.ValidationRequestTime = null;
			orderExtensionEntity.BudgetYear = -1;

			Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Update(orderExtensionEntity);
			#endregion // >>>>>> Budget DB
		}
	}
}
