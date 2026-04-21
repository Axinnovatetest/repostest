using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	using Psz.Core.Apps.Purchase.Models.DeliveryNote;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetAllHandler(Identity.Models.UserModel user)
		{
			_user = user;
		}
		public ResponseModel<List<GetModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var deliveryEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetDeliveryNotes();

			return new ResponseModel<List<GetModel>>
			{
				Success = true,
				Errors = null,
				Body = deliveryEntities?.Select(x => new GetModel
				{
					AngebotNr = x.Nr,
					Bezug = x.Bezug,
					KundenNr = (int)x.Kunden_Nr,
					VornameFirma = x.Vorname_NameFirma,
					Datum = x.Datum,
					Falligkeit = x.Falligkeit
				})?.ToList()
			};
		}

		public ResponseModel<List<GetModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<GetModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<GetModel>>.SuccessResponse();
		}
	}
}
