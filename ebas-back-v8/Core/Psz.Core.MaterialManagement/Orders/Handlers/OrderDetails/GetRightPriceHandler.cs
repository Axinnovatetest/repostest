using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class GetRightPriceHandler: IHandle<PriceRequestModel, ResponseModel<decimal>>
	{
		private PriceRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetRightPriceHandler(UserModel user, PriceRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<decimal> Handle()
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

		private ResponseModel<decimal> Perform(UserModel user, PriceRequestModel data)
		{
			var response = 0m;
			var articleNr = data.ArtikelNr.HasValue && data.ArtikelNr.Value != 0
				? data.ArtikelNr.Value
				: Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.Nr.Value).Artikel_Nr ?? -1;
			if(data.RahmenPosId.HasValue)
			{
				var rahmenPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.RahmenPosId.Value);
				response = rahmenPos.Einzelpreis ?? 0m;
			}
			else
			{
				var bestellnummern = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetBySupplierIdArticleId(data.SupplierID, articleNr);
				response = bestellnummern.Einkaufspreis ?? 0m;
			}

			return ResponseModel<decimal>.SuccessResponse(response);
		}

		public ResponseModel<decimal> Validate()
		{
			if(user == null)
			{
				return ResponseModel<decimal>.AccessDeniedResponse();
			}

			return ResponseModel<decimal>.SuccessResponse();
		}
	}
}
