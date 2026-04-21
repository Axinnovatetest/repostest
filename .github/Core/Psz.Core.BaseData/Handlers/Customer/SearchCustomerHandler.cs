using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class SearchCustomerHandler: IHandle<Models.Customer.SearchCustomerModel, ResponseModel<Models.Customer.SearchCustomerResponseModel>>
	{
		private Models.Customer.SearchCustomerModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public SearchCustomerHandler(Models.Customer.SearchCustomerModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<Models.Customer.SearchCustomerResponseModel> Handle()
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
					FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0,
					RequestRows = this._data.ItemsPerPage
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(this._data.SortFieldKey))
				{
					var sortFieldName = "";
					switch(this._data.SortFieldKey.ToLower())
					{
						default:
						case "number":
							sortFieldName = "[kundennummer]";
							break;
						case "name":
							sortFieldName = "Name1";
							break;
						case "address":
							sortFieldName = "[Straße]";
							break;
						case "customersgroup":
							sortFieldName = "[kundengruppe]";
							break;
						case "industry":
							sortFieldName = "[Branche]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}

				#endregion

				var customers = new List<Models.Customer.CustomerModel>();
				int allCount = 0;

				var kundenEntities = (!this._data.ArchivedOnly) ? Infrastructure.Data.Access.Tables.PRS.KundenAccess.SearchByNumberName(
				int.TryParse(this._data.CustomerNumber, out int nummer) ? nummer : (int?)null,
					this._data.CustomerName,
					dataSorting,
					dataPaging) : Infrastructure.Data.Access.Tables.PRS.KundenAccess.SearchArchived(
						int.TryParse(this._data.CustomerNumber, out int nummer3) ? nummer3 : (int?)null,
					this._data.CustomerName,
					dataSorting,
					dataPaging);

				if(kundenEntities != null && kundenEntities.Count > 0)
				{
					allCount = Infrastructure.Data.Access.Tables.PRS.KundenAccess.SearchByNumberName_CountAll(
						int.TryParse(this._data.CustomerNumber, out int nummer2) ? nummer2 : (int?)null,
						this._data.CustomerName);

					var nrs = kundenEntities.Select(x => (int)x.Nummer).ToList();
					var addresses = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(nrs);

					for(int i = 0; i < kundenEntities.Count; i++)
					{
						var kundenEntity = kundenEntities[i];
						var addressEntity = addresses.FirstOrDefault(a => a.Nr == kundenEntity.Nummer);
						var contactPersonsEntities = addressEntity != null
						? Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(addressEntity.Nr)
						: new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();

						var kundenExtensionEntity = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(kundenEntity.Nr);

						customers.Add(new Models.Customer.CustomerModel(kundenEntity, addressEntity, contactPersonsEntities, kundenExtensionEntity));
					}
				}
				//filter archived customers
				if(!this._data.ArchivedOnly)
				{
					var kundenExtensionEntiy = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.Get();
					var customersInExtension = from cust in customers
											   join ext in kundenExtensionEntiy
											   on cust.Id equals ext.Nr
											   select ext;
					var ArchivedNrs = customersInExtension?.Where(a => a.IsArchived.HasValue && a.IsArchived.Value).Select(x => x.Nr).ToList();
					for(int i = 0; i < customers.Count; i++)
					{
						if(ArchivedNrs.Contains(customers[i].Id))
						{
							customers.RemoveAt(i);
						}
					}
				}
				if(customers != null && customers.Count > 0)
				{
					for(int i = 0; i < customers.Count; i++)
					{
						//get induytry and group names
						var industry = customers[i].Industry;
						var parsed_1 = int.TryParse(industry, out var val) ? val : 0;
						var industryEntity = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Get(parsed_1);
						customers[i].Industry = (industryEntity != null) ? industryEntity.Name : customers[i].Industry;
						//
						var group = customers[i].CustomerGroup;
						var parsed_2 = int.TryParse(group, out var val1) ? val1 : 0;
						var groupEntity = Infrastructure.Data.Access.Tables.BSD.PSZ_KundengruppenAccess.Get(parsed_2);
						customers[i].CustomerGroup = (groupEntity != null) ? groupEntity.Kundengruppe : customers[i].CustomerGroup;
					}
				}
				return ResponseModel<Models.Customer.SearchCustomerResponseModel>.SuccessResponse(
					new Models.Customer.SearchCustomerResponseModel()
					{
						Customers = customers,
						RequestedPage = this._data.RequestedPage,
						ItemsPerPage = this._data.ItemsPerPage,
						AllCount = allCount > 0 ? allCount : 0,
						AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
					});

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Customer.SearchCustomerResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Customer.SearchCustomerResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Customer.SearchCustomerResponseModel>.SuccessResponse();
		}
	}
}
