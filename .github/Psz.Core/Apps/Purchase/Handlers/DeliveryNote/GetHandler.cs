
namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	using Psz.Core.Apps.Purchase.Models.DeliveryNote;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<GetModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }

		public GetHandler(Identity.Models.UserModel user, int id)
		{
			_user = user;
			_data = id;
		}

		public ResponseModel<GetModel> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			GetModel responseBody = null;
			var deliveryEntitiy = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetDeliveryNotesByNr(this._data);
			if(deliveryEntitiy != null)
			{
				responseBody = new GetModel
				{
					AngebotNr = deliveryEntitiy.Angebot_Nr ?? 0,
					Bezug = deliveryEntitiy.Bezug,
					KundenNr = (int)deliveryEntitiy.Kunden_Nr,
					VornameFirma = deliveryEntitiy.Vorname_NameFirma,
					Datum = deliveryEntitiy.Datum,
					Falligkeit = deliveryEntitiy.Falligkeit
				};
			}

			return new ResponseModel<GetModel>
			{
				Success = true,
				Errors = null,
				Body = responseBody
			};
		}

		public ResponseModel<GetModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<GetModel>.AccessDeniedResponse();
			}

			return ResponseModel<GetModel>.SuccessResponse();
		}
	}
}
