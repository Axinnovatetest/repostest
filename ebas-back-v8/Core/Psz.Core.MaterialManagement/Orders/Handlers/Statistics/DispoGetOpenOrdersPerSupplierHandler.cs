using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class DispoGetOpenOrdersPerSupplierHandler: IHandle<GetOpenOrdersPerSupplierRequestModel, ResponseModel<IPaginatedResponseModel<GetOpenOrdersPerSupplierResponseModel>>>
	{

		private GetOpenOrdersPerSupplierRequestModel _data { get; set; }
		private UserModel _user { get; set; }
		public DispoGetOpenOrdersPerSupplierHandler(UserModel user, GetOpenOrdersPerSupplierRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<IPaginatedResponseModel<GetOpenOrdersPerSupplierResponseModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this._user, this._data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<IPaginatedResponseModel<GetOpenOrdersPerSupplierResponseModel>> Perform(UserModel user, GetOpenOrdersPerSupplierRequestModel data)
		{
			try
			{
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};
				List<Dispows120DetailsOffenBestellungenEntity> fetchedOffneBestellungen = new();

				var entites = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetArtikelNrOrArtikelNummer(_data.ArtikelNr, "", false);
				var artikelNr = entites.FirstOrDefault();

				if(entites is null || entites.Count == 0)
					return ResponseModel<IPaginatedResponseModel<GetOpenOrdersPerSupplierResponseModel>>.NotFoundResponse();


				fetchedOffneBestellungen = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws120OffneBestellungen(artikelNr.Artiklenummer, data.Lieferanten_Nr, dataPaging);


				if(fetchedOffneBestellungen is null || fetchedOffneBestellungen.Count == 0)
					return ResponseModel<IPaginatedResponseModel<GetOpenOrdersPerSupplierResponseModel>>.NotFoundResponse();


				var restoreturn = fetchedOffneBestellungen.Select(x => new GetOpenOrdersPerSupplierResponseModel(x)).ToList();

				int TotalCount = 0;

				if(restoreturn.Count > 0 && restoreturn is not null)
				{
					TotalCount = restoreturn.FirstOrDefault().TotalCount;
				}

				return ResponseModel<IPaginatedResponseModel<GetOpenOrdersPerSupplierResponseModel>>.SuccessResponse(
			 new IPaginatedResponseModel<GetOpenOrdersPerSupplierResponseModel>
			 {
				 Items = restoreturn,
				 PageRequested = this._data.RequestedPage,
				 PageSize = this._data.PageSize,
				 TotalCount = TotalCount,
				 TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(TotalCount > 0 ? TotalCount : 0) / this._data.PageSize)) : 0
			 });


			} catch(Exception ex)
			{
				throw;
			}
		}
		public ResponseModel<IPaginatedResponseModel<GetOpenOrdersPerSupplierResponseModel>> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<IPaginatedResponseModel<GetOpenOrdersPerSupplierResponseModel>>.AccessDeniedResponse();
			}
			return ResponseModel<IPaginatedResponseModel<GetOpenOrdersPerSupplierResponseModel>>.SuccessResponse();
		}
	}
}
