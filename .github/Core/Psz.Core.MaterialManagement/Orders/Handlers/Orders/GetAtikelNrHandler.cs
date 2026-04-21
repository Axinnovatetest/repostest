using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class GetAtikelNrHandler: IHandle<GetArtikelNrRequestModel, ResponseModel<List<GetArtikelNrReponseModel>>>
	{
		private GetArtikelNrRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetAtikelNrHandler(UserModel user, GetArtikelNrRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<GetArtikelNrReponseModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<List<GetArtikelNrReponseModel>> Perform(UserModel user, GetArtikelNrRequestModel data)
		{
			var orderNumbers = Infrastructure.Data.Access.Joins.MTM.Order.ArtikelFilterAccess.GetLikeArtikelnummer(data.Filter, data.IncludeDone ?? false);
			if(orderNumbers == null || orderNumbers.Count == 0)
				return ResponseModel<List<GetArtikelNrReponseModel>>.NotFoundResponse();

			return ResponseModel<List<GetArtikelNrReponseModel>>.SuccessResponse(orderNumbers?.Select(x => new GetArtikelNrReponseModel(x)).Take(10)?.ToList());
		}

		public ResponseModel<List<GetArtikelNrReponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<GetArtikelNrReponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetArtikelNrReponseModel>>.SuccessResponse();
		}
	}
}
