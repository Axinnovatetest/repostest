using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<Models.Order.SearchResponseModel> Search(Models.Order.SearchModel data,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null /*|| !user.Access.EDI.Order*/)
				{
					return Core.Models.ResponseModel<Models.Order.SearchResponseModel>.AccessDeniedResponse();
				}

				return Core.Models.ResponseModel<Models.Order.SearchResponseModel>.SuccessResponse(SearchInternal(data, user));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static Models.Order.SearchResponseModel SearchInternal(Models.Order.SearchModel data, Core.Identity.Models.UserModel user)
		{
			try
			{
				var allCount = 0;

				var dataTypes = new List<string>();
				var exdataTypes = new List<string>();
				if(data.Types != null)
				{
					foreach(var typeInt in data.Types)
					{
						dataTypes.Add(Enums.OrderEnums.TypeToData((Enums.OrderEnums.Types)typeInt));
					}
				}

				// - 2023-07-23
				if(!data.Types.Contains((int)Enums.OrderEnums.Types.Contract))
				{
					data.BlanketTypeId = null;
				}
				else
				{
					data.BlanketTypeId = 0;
				}

				// - 2023-10-30 - remove Types not accessible by user - Heidenreich 
				if(user?.SuperAdministrator == false || user?.IsGlobalDirector == false)
				{
					// - AB
					if(user?.Access?.CustomerService.ConfirmationView == false)
					{
						exdataTypes.Add(Enums.OrderEnums.Types.Confirmation.GetDescription());
					}
					// - contract
					if(user?.Access?.CustomerService.Rahmen == false)
					{
						exdataTypes.Add(Enums.OrderEnums.Types.Contract.GetDescription());
					}
					//  - GS
					if(user?.Access?.CustomerService.GutschriftView == false)
					{
						exdataTypes.Add(Enums.OrderEnums.Types.Credit.GetDescription());
					}
					// - RG
					if(user?.Access?.CustomerService.Rechnung == false)
					{
						exdataTypes.Add(Enums.OrderEnums.Types.Invoice.GetDescription());
					}
					// - LP
					if(user?.Access?.CustomerService.DelforView == false && user?.Access?.CustomerService.ForcastView == false)
					{
						exdataTypes.Add(Enums.OrderEnums.Types.Forecast.GetDescription());
					}
					// - LS
					if(user?.Access?.CustomerService.DeliveryNoteView == false)
					{
						exdataTypes.Add(Enums.OrderEnums.Types.Delivery.GetDescription());
					}
				}

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.ItemsPerPage > 0 ? (data.RequestedPage * data.ItemsPerPage) : 0,
					RequestRows = data.ItemsPerPage
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data.SortFieldKey))
				{
					var sortFieldName = "";
					switch(data.SortFieldKey.ToLower())
					{
						default:
						case "duedate":
							sortFieldName = "[Fälligkeit]";
							break;
						case "documentnumber":
							sortFieldName = "Bezug";
							break;
						case "type":
							sortFieldName = "[Typ]";
							break;
						case "name":
							sortFieldName = "[Vorname/NameFirma]";
							break;
						case "projectnumber":
							sortFieldName = "[Projekt-Nr]";
							break;
						case "vorfailnr":
							sortFieldName = "[Angebot-Nr]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion

				#region > Customers Filter
				var searchCustomerNumbers = new List<int>();

				if(user.Access.Purchase.AllCustomers)
				{
					searchCustomerNumbers = data.CustomersIds;
				}
				else
				{
					var userCustomerNumbers = new List<int>();

					var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(user);
					userCustomerNumbers = userCustomers.Select(e => e.CustomerNumber).ToList();

					if(data.CustomersIds != null && data.CustomersIds.Count > 0)
					{
						searchCustomerNumbers = data.CustomersIds.FindAll(e => userCustomerNumbers.Contains(e));
					}
					else
					{
						searchCustomerNumbers = userCustomerNumbers;
					}
				}
				//rechnung filtering
				if(data.RechnungCustomerId.HasValue && data.RechnungCustomerId.Value != 0)
					searchCustomerNumbers.Add(data.RechnungCustomerId.Value);
				var typ = data.RechnungType ?? -1;
				var searchedRechnungType = ((Psz.Core.CustomerService.Enums.E_RechnungEnums.RechnungTyp)typ).GetDescription();
				#endregion

				var ordersDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetBy_Bezug_ProjectNr_AngebotNr_Typs_KundenNrs(data.DocumentNumber,
					data.Unbooked,
					data.OnlyInProgress,
					data.ProjectNumber,
					data.PrefailNumber,
					dataTypes,
					searchCustomerNumbers,
					searchedRechnungType,
					data.BlanketTypeId,
					data.GultigAb,
					data.GultigBis,
					data.CreatedFrom,
					data.CreatedTo,
					exdataTypes,
					dataSorting,
					dataPaging);
				allCount = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.CountBy_Bezug_ProjectNr_AngebotNr_Typs_KundenNrs(data.DocumentNumber,
					data.Unbooked,
					data.OnlyInProgress,
					data.ProjectNumber,
					data.PrefailNumber,
					searchedRechnungType,
					data.BlanketTypeId,
					data.GultigAb,
					data.GultigBis,
					data.CreatedFrom,
					data.CreatedTo,
					dataTypes,
					searchCustomerNumbers,
					   exdataTypes);

				//filtering rahmens
				var rahmens = ordersDb.Where(o => o.Typ == Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Contract)).ToList();
				if(rahmens != null && rahmens.Count > 0)
				{
					foreach(var item in rahmens)
					{
						var rahmenextension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(item.Nr);
						var rahmenPositionsExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenNr(item.Nr);
						if(rahmenextension != null && data.BlanketTypeId.HasValue && data.BlanketTypeId.Value != rahmenextension.BlanketTypeId)
						{
							var itemToRemove = ordersDb.Single(r => r.Nr == item.Nr);
							ordersDb.Remove(itemToRemove);
						}

						if(rahmenPositionsExtension != null && rahmenPositionsExtension.Count > 0 && data.GultigAb.HasValue && data.GultigBis.HasValue)
						{
							var _check = rahmenPositionsExtension.Any(e => e.GultigAb == data.GultigAb && e.GultigBis == data.GultigBis);
							if(!_check)
							{
								var itemToRemove = ordersDb.Single(r => r.Nr == item.Nr);
								ordersDb.Remove(itemToRemove);
							}
						}
					}
				}


				var orders = Get(ordersDb, false, includeActions: false);

				return new Models.Order.SearchResponseModel()
				{
					Orders = orders,
					RequestedPage = data.RequestedPage,
					ItemsPerPage = data.ItemsPerPage,
					AllCount = orders != null && orders.Count > 0 ? allCount : 0,
					AllPagesCount = orders != null && orders.Count > 0 ?
					data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)allCount / data.ItemsPerPage)) : 0 : 0,
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
