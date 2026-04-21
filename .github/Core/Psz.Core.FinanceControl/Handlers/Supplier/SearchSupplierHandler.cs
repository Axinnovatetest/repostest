using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Supplier
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class SearchSupplierHandler: IHandle<Models.Supplier.SearchSupplierModel, ResponseModel<Models.Supplier.SearchSupplierResponseModel>>
	{
		private Models.Supplier.SearchSupplierModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public SearchSupplierHandler(Models.Supplier.SearchSupplierModel data,
			Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<Models.Supplier.SearchSupplierResponseModel> Handle()
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
							sortFieldName = "[Lieferantennummer]";
							break;
						case "name":
							sortFieldName = "Name1";
							break;
						case "address":
							sortFieldName = "[Straße]";
							break;
						case "suppliersgroup":
							sortFieldName = "[Lieferantengruppe]";
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

				var suppliers = new List<Models.Supplier.MinimalSupplierModel>();
				int allCount = 0;

				var lieferantenEntities = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.SearchByNumberName(
					int.TryParse(this._data.SupplierNumber, out int nummer) ? nummer : (int?)null,
					this._data.SupplierName,
					dataSorting,
					dataPaging);

				if(lieferantenEntities != null && lieferantenEntities.Count > 0)
				{
					allCount = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.SearchByNumberName_CountAll(
						int.TryParse(this._data.SupplierNumber, out int nummer2) ? nummer2 : (int?)null,
						this._data.SupplierName);

					var nrs = lieferantenEntities.Select(x => (int)x.Nummer).ToList();
					var addresses = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(nrs);

					for(int i = 0; i < lieferantenEntities.Count; i++)
					{
						var lieferantenEntity = lieferantenEntities[i];
						var addressEntity = addresses.FirstOrDefault(a => a.Nr == lieferantenEntity.Nummer);

						suppliers.Add(new Models.Supplier.MinimalSupplierModel(lieferantenEntity, addressEntity));
					}
				}

				return ResponseModel<Models.Supplier.SearchSupplierResponseModel>.SuccessResponse(
					new Models.Supplier.SearchSupplierResponseModel()
					{
						Suppliers = suppliers,
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

		public ResponseModel<Models.Supplier.SearchSupplierResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Supplier.SearchSupplierResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Supplier.SearchSupplierResponseModel>.SuccessResponse();
		}
	}
}
