using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.FinanceControl.Models.Budget.Order;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<OrderListResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Order.OrderListRequestModel _data { get; set; }

		public GetAllHandler(Identity.Models.UserModel user, Models.Budget.Order.OrderListRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<OrderListResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.PageSize
				};

				#endregion
				//FIXME: Redesign this process!!!
				var ordersExtensionEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
				var ext1 = GetByUserHandler.GetOrderExtensions(this._user, this._data.ShowCompletelyBooked, out List<string> errorsuser);
				if(ext1 != null && ext1.Count > 0)
					ordersExtensionEntities.AddRange(ext1);
				var ext2 = GetAllByUserHandler.GetOrderExtensions(this._user, this._data.ShowCompletelyBooked, out List<string> errorsAll, this._data.Year);
				if(ext2 != null && ext2.Count > 0)
					ordersExtensionEntities.AddRange(ext2);

				ordersExtensionEntities = ordersExtensionEntities?.DistinctBy(x => x.Id)?.ToList();
				// - 2025-02-19 filter request frm AK
				if(this._data.ShowOnlyInProgress == true)
				{
					ordersExtensionEntities = ordersExtensionEntities?.Where(x => x.ApprovalUserId is null)?.ToList();
				}
				//filter finance orders that user cannot see
				var financeOrders = ordersExtensionEntities?.Where(x => x.OrderType == Enums.BudgetEnums.ProjectTypes.Finance.GetDescription()).ToList();
				if(financeOrders != null && financeOrders.Count > 0)
				{
					if(!_user.Access.Financial.Budget.FinanceOrder)
						ordersExtensionEntities = ordersExtensionEntities.Except(financeOrders).ToList();
					else
					{
						var visibiltyProjectsEnities = Infrastructure.Data.Access.Tables.FNC.FinanceProjectsVisibiltyUsersAccess.GetByUserId(_user.Id);
						if(visibiltyProjectsEnities == null || visibiltyProjectsEnities.Count <= 0)
							ordersExtensionEntities = ordersExtensionEntities.Except(financeOrders).ToList();
						else
						{
							var visibilityProjects = visibiltyProjectsEnities.Select(x => x.ProjectId).ToList();
							financeOrders.ForEach(x =>
							{
								if(!visibilityProjects.Contains(x.ProjectId))
								{
									var ordersToRemove = ordersExtensionEntities.Where(o => o.ProjectId == x.ProjectId).ToList();
									ordersExtensionEntities.Except(ordersToRemove);
								}
							});
						}
					}
				}
				//filtering
				var filtredResponse = ordersExtensionEntities?.Where(x =>
				!this._data.Year.HasValue ||
				(this._data.Year.HasValue
				&& x.CreationDate.Value.Year == this._data.Year.Value)).ToList();
				filtredResponse = filtredResponse?.Where(x =>
				!this._data.Month.HasValue ||
				(this._data.Month.HasValue
				&& x.CreationDate.Value.Month == this._data.Month.Value)).ToList();
				if(!this._data.Type.IsnullOrEmptyOrWhiteSpaces())
					filtredResponse = filtredResponse?.Where(x => x.OrderType.Trim().ToLower() == this._data.Type.ToLower().Trim()).ToList();
				if(!_data.Searchtext.IsnullOrEmptyOrWhiteSpaces())
				{
					var filter = _data.Searchtext.ToLower().Trim();
					filtredResponse = filtredResponse?.Where(x =>
					x.OrderNumber.Trim().ToLower().Contains(filter) ||
					x.OrderType.Trim().ToLower().Contains(filter) ||
					x.ProjectName.Trim().ToLower().Contains(filter) ||
					x.SupplierName.Trim().ToLower().Contains(filter) ||
					x.IssuerName.Trim().ToLower().Contains(filter) ||
					x.DepartmentName.Trim().ToLower().Contains(filter)).ToList();
				}
				if(this._data.Filter is not null)
				{
					switch(this._data.Filter)
					{
						case 0:
							//all
							break;
						case 1:// - My orders
							filtredResponse = filtredResponse?.Where(x => x.IssuerId == this._user.Id).ToList();
							break;
						case 2: // - My Draft
							filtredResponse = filtredResponse?.Where(x => x.IssuerId == this._user.Id && x.Level == 0).ToList();
							break;
						case 5: // - Placed
							filtredResponse = filtredResponse?.Where(x => x.OrderPlacedUserId > 0 || x.OrderPlacedTime is not null).ToList();
							break;
						case 6: // - Booked
							filtredResponse = filtredResponse?.Where(x => x.Level == 0).ToList();
							break;
						default:
							break;
					}
				}
				//ordering
				switch(this._data.SortField?.ToLower())
				{
					case "order_number":
						filtredResponse = _data.SortDesc
							? filtredResponse.OrderByDescending(x => x.OrderNumber).ToList()
							: filtredResponse.OrderBy(x => x.OrderNumber).ToList();
						break;
					case "type_order":
						filtredResponse = _data.SortDesc
							? filtredResponse.OrderByDescending(x => x.OrderType).ToList()
							: filtredResponse.OrderBy(x => x.OrderType).ToList();
						break;
					case "name_project":
						filtredResponse = _data.SortDesc
							? filtredResponse.OrderByDescending(x => x.ProjectName).ToList()
							: filtredResponse.OrderBy(x => x.ProjectName).ToList();
						break;
					case "id_supplier":
						filtredResponse = _data.SortDesc
							? filtredResponse.OrderByDescending(x => x.SupplierName).ToList()
							: filtredResponse.OrderBy(x => x.SupplierName).ToList();
						break;
					case "responsable_id":
						filtredResponse = _data.SortDesc
							? filtredResponse.OrderByDescending(x => x.IssuerName).ToList()
							: filtredResponse.OrderBy(x => x.IssuerName).ToList();
						break;
					case "id_dept":
						filtredResponse = _data.SortDesc
							? filtredResponse.OrderByDescending(x => x.DepartmentName).ToList()
							: filtredResponse.OrderBy(x => x.DepartmentName).ToList();
						break;
					case "order_date":
						filtredResponse = _data.SortDesc
							? filtredResponse.OrderByDescending(x => x.CreationDate).ToList()
							: filtredResponse.OrderBy(x => x.CreationDate).ToList();
						break;
					default:
						break;

				}
				//paginating
				var paginatedesponse = filtredResponse.Skip(dataPaging.FirstRowNumber).Take(dataPaging.RequestRows).ToList();

				List<OrderModel> data = Helpers.Processings.Budget.Order.GetOrderModelsNew(paginatedesponse, this._user, out List<string> errorsModel)
					?.Where(x => !this._data.Year.HasValue || (this._data.Year.HasValue && x.OrderDate.Value.Year == this._data.Year.Value))?.ToList();
				var __financeOrders = data?.Where(x => x.Type == Enums.BudgetEnums.ProjectTypes.Finance.GetDescription()).ToList();
				// - update Validators
				if(financeOrders != null && financeOrders.Count > 0 && data != null && data.Count > 0)
				{
					var user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
					var userCompany = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(this._user.CompanyId);
					var lastValidator1 = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(userCompany.FinanceValidatorOneId ?? 0)
					?? Infrastructure.Data.Access.Tables.COR.UserAccess.Get(userCompany.FinanceValidatorTowId ?? 0);
					for(var i = 0; i < data.Count; i++)
					{
						if(data[i].Type == Enums.BudgetEnums.ProjectTypes.Finance.GetDescription())
						{
							data[i].Validators = new List<Models.Budget.Order.ValidatorModel>
							{
								new Models.Budget.Order.ValidatorModel(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity{ Id_Validator=this._user.Id, Level=0}, user),
								new Models.Budget.Order.ValidatorModel(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity {  Level=1}, lastValidator1 )
							};
						}
					}
				}
				var errors = new List<string>();
				if(errorsAll != null && errorsAll.Count > 0)
				{
					errors.AddRange(errorsAll);
				}
				if(errorsuser != null && errorsuser.Count > 0)
				{
					errors.AddRange(errorsuser);
				}
				if(errorsModel != null && errorsModel.Count > 0)
				{
					errors.AddRange(errorsModel);
				}

				var response = new OrderListResponseModel
				{
					Items = data,
					PageRequested = this._data.RequestedPage,
					PageSize = this._data.PageSize,
					TotalCount = filtredResponse.Count > 0 ? filtredResponse.Count : 0,
					TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(filtredResponse.Count > 0 ? filtredResponse.Count : 0) / this._data.PageSize)) : 0,
				};
				if(errors != null && errors.Count > 0)
					return ResponseModel<OrderListResponseModel>.FailureResponse(errors);
				return ResponseModel<OrderListResponseModel>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<OrderListResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<OrderListResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<OrderListResponseModel>.SuccessResponse();
		}
		//public ResponseModel<List<Models.Budget.Order.OrderModel>> HandleOLD()
		//{
		//	try
		//	{
		//		var validationResponse = this.Validate();
		//		if(!validationResponse.Success)
		//		{
		//			return validationResponse;
		//		}

		//		//FIXME: Redesign this process!!!
		//		var response = new List<Models.Budget.Order.OrderModel>();
		//		response.AddRange(GetByUserHandler.GetOrders(this._user, this._data.ShowCompletelyBooked, out List<string> errorsuser)
		//			?.Where(x => !this._data.Year.HasValue || (this._data.Year.HasValue && x.OrderDate.Value.Year == this._data.Year.Value))?.ToList() ?? new List<Models.Budget.Order.OrderModel>());
		//		response.AddRange(GetAllByUserHandler.GetOrders(this._user, this._data.ShowCompletelyBooked, out List<string> errorsAll, this._data.Year)
		//			?.Where(x => !this._data.Year.HasValue || (this._data.Year.HasValue && x.OrderDate.Value.Year == this._data.Year.Value))?.ToList() ?? new List<Models.Budget.Order.OrderModel>());
		//		response = response?.DistinctBy(x => x.Id)?.ToList();

		//		var errors = new List<string>();
		//		if(errorsAll != null && errorsAll.Count > 0)
		//		{
		//			errors.AddRange(errorsAll);
		//		}
		//		if(errorsuser != null && errorsuser.Count > 0)
		//		{
		//			errors.AddRange(errorsuser);
		//		}

		//		return new ResponseModel<List<Models.Budget.Order.OrderModel>>()
		//		{
		//			Success = true,
		//			Body = response,
		//			Errors = errors.Count <= 0 ? null : errors.Distinct().Select(x => new ResponseModel<List<Models.Budget.Order.OrderModel>>.ResponseError(new KeyValuePair<string, string>("", x))).ToList()
		//		};
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		throw;
		//	}
		//}
	}

}
