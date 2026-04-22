using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetArtikelNummersHandler: IHandle<GetArtikelNummerRequestModel, ResponseModel<List<GetArtikelNummerModel>>>
	{

		private GetArtikelNummerRequestModel _data { get; set; }
		private UserModel _user { get; set; }
		public GetArtikelNummersHandler(UserModel user, GetArtikelNummerRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<GetArtikelNummerModel>> Handle()
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

		private ResponseModel<List<GetArtikelNummerModel>> Perform(UserModel user, GetArtikelNummerRequestModel data)
		{
			try
			{
				List<GetFaultyArtikelNummerEntity> fetcheddata = new();

				if(data.dispo <= 0 || data.dispo >= 14)
					return ResponseModel<List<GetArtikelNummerModel>>.NotFoundResponse();


				fetcheddata = data.dispo switch
				{
					1 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummers120(data.filter),
					2 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummers40(data.filter),
					3 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummersTN90(data.filter),
					4 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummersTN30(data.filter),
					5 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummersBETN90(data.filter),
					6 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummersBETN40(data.filter),
					7 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummersCZ90(data.filter),
					8 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummersCZ30(data.filter),
					9 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummersAL90(data.filter),
					10 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummersAL30(data.filter),
					11 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummersGZ90(data.filter),
					12 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummersGZ40(data.filter),
					13 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetFaultyArtikelNummersDE90(data.filter),
					_ => null
				};

				if(fetcheddata is null || fetcheddata.Count == 0)
					return ResponseModel<List<GetArtikelNummerModel>>.NotFoundResponse();

				var restoreturn = fetcheddata.Select(x => new GetArtikelNummerModel(x)).ToList();
				return ResponseModel<List<GetArtikelNummerModel>>.SuccessResponse(restoreturn);



			} catch(Exception ex)
			{
				throw;
			}
		}
		public ResponseModel<List<GetArtikelNummerModel>> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<List<GetArtikelNummerModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<GetArtikelNummerModel>>.SuccessResponse();
		}
	}
}
