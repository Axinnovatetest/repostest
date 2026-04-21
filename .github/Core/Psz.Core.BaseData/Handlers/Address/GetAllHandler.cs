using Psz.Core.BaseData.Models.Address;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Address
{
	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<GetAdressesListResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private GetAddressesListRequestModel _data { get; set; }

		public GetAllHandler(Identity.Models.UserModel user, GetAddressesListRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<GetAdressesListResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				int totalCount = 0;

				#region > Data sorting & paging
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;


				if(!string.IsNullOrWhiteSpace(_data.SortField))
				{
					var sortFieldName = "";
					switch(_data.SortField.ToLower())
					{
						default:
						case "addresstype":
							sortFieldName = "[Adresstyp]";
							break;
						case "customernumber":
							sortFieldName = "[Kundennummer]";
							break;
						case "suppliernumber":
							sortFieldName = "[Lieferantennummer]";
							break;
						case "name1":
							sortFieldName = "[Name1]";
							break;
						case "name2":
							sortFieldName = "[Name2]";
							break;
						case "street":
							sortFieldName = "[Straße]";
							break;
						case "city":
							sortFieldName = "[Ort]";
							break;
						case "streetzipcode":
							sortFieldName = "[PLZ_Straße]";
							break;
						case "country":
							sortFieldName = "[Land]";
							break;
						case "phonenumber":
							sortFieldName = "[Telefon]";
							break;
						case "faxnumber":
							sortFieldName = "[Fax]";
							break;
						case "id":
							sortFieldName = "[Nr]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};

				}
				totalCount = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.CountAdresses(_data.SearchValue, (int)_data.AdresseType);

				#endregion
				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.FullData ? totalCount : this._data.PageSize
				};
				///
				var entities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(_data.SearchValue, _data.AdresseType, dataSorting, dataPaging);


				List<AddressModel> adressesList = new List<AddressModel>();

				adressesList = entities.Select(x => new AddressModel(x)).ToList();
				var r = this._data.PageSize > 0 ? totalCount / decimal.Parse((this._data.PageSize).ToString()) : 0;

				if(adressesList is null || adressesList.Count == 0)
				{
					return ResponseModel<GetAdressesListResponseModel>.SuccessResponse(new GetAdressesListResponseModel
					{
						Items = new List<AddressModel>(),
						PageRequested = this._data.RequestedPage,
						PageSize = this._data.PageSize,
						TotalCount = totalCount,
						TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
					});
				}

				GetAdressesListResponseModel response = new GetAdressesListResponseModel
				{
					Items = adressesList,
					PageRequested = _data.RequestedPage,
					PageSize = _data.PageSize,
					TotalCount = adressesList != null && adressesList.Count > 0 ? totalCount : 0,
					TotalPageCount = adressesList != null && adressesList.Count > 0 ?
					_data.PageSize > 0 ? (int)Math.Ceiling(((decimal)totalCount / _data.PageSize)) : 0 : 0,
				};
				return ResponseModel<GetAdressesListResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<GetAdressesListResponseModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<GetAdressesListResponseModel>.AccessDeniedResponse();
			}
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<GetAdressesListResponseModel>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<GetAdressesListResponseModel>.SuccessResponse();
		}
	}
}
