using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{

	public class Dispows120DetailsLieferantenHandler: IHandle<GetDispows120DetailsLieferantenRequestModel, ResponseModel<IPaginatedResponseModel<Dispows120DetailsLieferantenModel>>>
	{

		private GetDispows120DetailsLieferantenRequestModel _data { get; set; }
		private UserModel _user { get; set; }
		public Dispows120DetailsLieferantenHandler(UserModel user, GetDispows120DetailsLieferantenRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		/// <summary>
		/// takes ArtikelNr And it return Infromations about the Supplier based on Dispo plant
		/// </summary>
		/// <returns></returns>
		public ResponseModel<IPaginatedResponseModel<Dispows120DetailsLieferantenModel>> Handle()
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

		private ResponseModel<IPaginatedResponseModel<Dispows120DetailsLieferantenModel>> Perform(UserModel user, GetDispows120DetailsLieferantenRequestModel data)
		{
			try
			{


				var dataPagingob = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = 0,
					RequestRows = 1
				};

				if(data.ArtikelNr <= 0)
					return ResponseModel<IPaginatedResponseModel<Dispows120DetailsLieferantenModel>>.NotFoundResponse();


				List<Dispows120DetailsLieferantenEntity> fetchedData = new();
				var entites = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetArtikelNrOrArtikelNummer(_data.ArtikelNr, "", false);

				if(entites is null || entites.Count == 0)
					return ResponseModel<IPaginatedResponseModel<Dispows120DetailsLieferantenModel>>.NotFoundResponse();

				var artikelNr = entites.FirstOrDefault();
				fetchedData = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws120Lieferanten(artikelNr.Artiklenummer);



				if(fetchedData is null || fetchedData.Count == 0)
					return ResponseModel<IPaginatedResponseModel<Dispows120DetailsLieferantenModel>>.NotFoundResponse();


				var restoreturn = fetchedData.Select(x => new Dispows120DetailsLieferantenModel(x)).ToList();

				foreach(var item in restoreturn)
				{
					var x = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws120OffneBestellungen(artikelNr.Artiklenummer, item.Lieferanten_Nr, dataPagingob);
					if(x is null || x.Count == 0)
					{
						item.offnenbestellecount = 0;
					}
					else
					{
						item.offnenbestellecount = x.FirstOrDefault().TotalCount;
					}
				}

				int TotalCount = 0;
				if(restoreturn is not null && restoreturn.Count > 0)
				{
					TotalCount = restoreturn.FirstOrDefault().TotalCount;
				}

				return ResponseModel<IPaginatedResponseModel<Dispows120DetailsLieferantenModel>>.SuccessResponse(
			 new IPaginatedResponseModel<Dispows120DetailsLieferantenModel>
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
		public ResponseModel<IPaginatedResponseModel<Dispows120DetailsLieferantenModel>> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<IPaginatedResponseModel<Dispows120DetailsLieferantenModel>>.AccessDeniedResponse();
			}
			return ResponseModel<IPaginatedResponseModel<Dispows120DetailsLieferantenModel>>.SuccessResponse();
		}
	}
}
