using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class Get120DetailsHandler: IHandle<GetDispows120DetailsRequestModel, ResponseModel<Dispows120DetailsModel>>
	{

		private GetDispows120DetailsRequestModel _data { get; set; }
		private UserModel _user { get; set; }
		public Get120DetailsHandler(UserModel user, GetDispows120DetailsRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Dispows120DetailsModel> Handle()
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

		private ResponseModel<Dispows120DetailsModel> Perform(UserModel user, GetDispows120DetailsRequestModel data)
		{
			try
			{
				if(data.ArtikelNr <= 0)
					return ResponseModel<Dispows120DetailsModel>.NotFoundResponse();

				var entites = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetArtikelNrOrArtikelNummer(_data.ArtikelNr, "", false);

				if(entites is null || entites.Count == 0)
					return ResponseModel<Dispows120DetailsModel>.NotFoundResponse();

				var artikelNr = entites.FirstOrDefault();
				List<Dispows120DetailsEntity> fetchedData = new();
				List<Dispows120DetailsBestandEntity> fetchedData2 = new();
				fetchedData = data.Dispo switch
				{
					1 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws120Details(artikelNr.Artiklenummer),
					2 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws40Details(artikelNr.Artiklenummer),
					3 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetTN90Details(artikelNr.Artiklenummer),
					4 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetTN30Details(artikelNr.Artiklenummer),
					5 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetBeTn90Details(artikelNr.Artiklenummer),
					6 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetBeTn40Details(artikelNr.Artiklenummer),
					7 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetCZ90Details(artikelNr.Artiklenummer),
					8 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetCZ30Details(artikelNr.Artiklenummer),
					9 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetAl90Details(artikelNr.Artiklenummer),
					10 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetAl30Details(artikelNr.Artiklenummer),
					11 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetGZ90Details(artikelNr.Artiklenummer),
					12 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetGZ40Details(artikelNr.Artiklenummer),
					13 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetDEDetails(artikelNr.Artiklenummer),
					_ => null
				};
				fetchedData2 = data.Dispo switch
				{
					1 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws120Bestand(artikelNr.Artiklenummer),
					2 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws40Bestand(artikelNr.Artiklenummer),
					3 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Gettn90Bestand(artikelNr.Artiklenummer),
					4 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Gettn30Bestand(artikelNr.Artiklenummer),
					5 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getbetn90Bestand(artikelNr.Artiklenummer),
					6 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getbetn40Bestand(artikelNr.Artiklenummer),
					7 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetCZ90Bestand(artikelNr.Artiklenummer),
					8 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetCZ30Bestand(artikelNr.Artiklenummer),
					9 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetAl90Bestand(artikelNr.Artiklenummer),
					10 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetAl30Bestand(artikelNr.Artiklenummer),
					11 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetGZ90Bestand(artikelNr.Artiklenummer),
					12 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetGZ40Bestand(artikelNr.Artiklenummer),
					13 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetDE90Bestand(artikelNr.Artiklenummer),
					_ => null
				};

				if(fetchedData is null || fetchedData.Count == 0)
					return ResponseModel<Dispows120DetailsModel>.NotFoundResponse();


				var restoreturn = fetchedData.Select(x => new Dispows120Details12Model(x)).FirstOrDefault();

				var restoreturn2 = fetchedData2.Select(x => new Dispows120DetailsBestandModel(x)).ToList();


				return ResponseModel<Dispows120DetailsModel>.SuccessResponse(
			 new Dispows120DetailsModel
			 {
				 dispows120Details12Models = restoreturn,
				 dispows120Details15Models = restoreturn2,

			 });
			} catch(Exception ex)
			{
				throw;
			}
		}

		public ResponseModel<Dispows120DetailsModel> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<Dispows120DetailsModel>.AccessDeniedResponse();
			}

			return ResponseModel<Dispows120DetailsModel>.SuccessResponse();
		}
	}
}
