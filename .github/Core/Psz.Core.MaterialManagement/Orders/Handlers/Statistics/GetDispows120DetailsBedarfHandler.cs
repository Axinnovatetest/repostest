using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetDispows120DetailsBedarfHandler: IHandle<GetDispows120DetailsBedarfeRequestModel, ResponseModel<IPaginatedResponseModel<Dispows120DetailsBedarfeModel>>>
	{

		private GetDispows120DetailsBedarfeRequestModel _data { get; set; }
		private UserModel _user { get; set; }
		public GetDispows120DetailsBedarfHandler(UserModel user, GetDispows120DetailsBedarfeRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<IPaginatedResponseModel<Dispows120DetailsBedarfeModel>> Handle()
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

		private ResponseModel<IPaginatedResponseModel<Dispows120DetailsBedarfeModel>> Perform(UserModel user, GetDispows120DetailsBedarfeRequestModel data)
		{
			try
			{
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};

				if(data is null || data.ArtikelNr <= 0)
					return ResponseModel<IPaginatedResponseModel<Dispows120DetailsBedarfeModel>>.NotFoundResponse();


				var entites = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetArtikelNrOrArtikelNummer(_data.ArtikelNr, "", false);
				var artikelNr = entites.FirstOrDefault();

				if(entites is null || entites.Count == 0)
					return ResponseModel<IPaginatedResponseModel<Dispows120DetailsBedarfeModel>>.NotFoundResponse();

				List<Dispows120DetailsBedarfeEntity> fetchedData = new();
				fetchedData = data.Dispo switch
				{
					1 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws120Bedarfe(artikelNr.Artiklenummer, dataPaging),
					2 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws40Bedarfe(artikelNr.Artiklenummer, dataPaging),
					3 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetTN90Bedarfe(artikelNr.Artiklenummer, dataPaging),
					4 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetTN40Bedarfe(artikelNr.Artiklenummer, dataPaging),
					5 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetBETN90Bedarfe(artikelNr.Artiklenummer, dataPaging),
					6 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetBETN40Bedarfe(artikelNr.Artiklenummer, dataPaging),
					7 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetCZ90Bedarfe(artikelNr.Artiklenummer, dataPaging),
					8 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetCZ30Bedarfe(artikelNr.Artiklenummer, dataPaging),
					9 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetAL90Bedarfe(artikelNr.Artiklenummer, dataPaging),
					10 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetAL30Bedarfe(artikelNr.Artiklenummer, dataPaging),
					11 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetGZ90Bedarfe(artikelNr.Artiklenummer, dataPaging),
					12 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetGZ40Bedarfe(artikelNr.Artiklenummer, dataPaging),
					13 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getde90Bedarfe(artikelNr.Artiklenummer, dataPaging),
					_ => null
				};

				if(fetchedData is null || fetchedData.Count == 0)
					return ResponseModel<IPaginatedResponseModel<Dispows120DetailsBedarfeModel>>.NotFoundResponse();


				var restoreturn = fetchedData.Select(x => new Dispows120DetailsBedarfeModel(x)).ToList();
				int TotalCount = 0;
				if(restoreturn.Count > 0 && restoreturn is not null)
				{
					TotalCount = (restoreturn is null) ? 0 : restoreturn.FirstOrDefault().TotalCount;
				}

				return ResponseModel<IPaginatedResponseModel<Dispows120DetailsBedarfeModel>>.SuccessResponse(
			 new IPaginatedResponseModel<Dispows120DetailsBedarfeModel>
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
		public ResponseModel<IPaginatedResponseModel<Dispows120DetailsBedarfeModel>> Validate()
		{
			if(_user == null)
			{
				return ResponseModel<IPaginatedResponseModel<Dispows120DetailsBedarfeModel>>.AccessDeniedResponse();
			}
			return ResponseModel<IPaginatedResponseModel<Dispows120DetailsBedarfeModel>>.SuccessResponse();
		}
	}
}
