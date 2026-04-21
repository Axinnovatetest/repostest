using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetBestellungenLinkBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<BestellungenLinkBlanketModel>>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBestellungenLinkBlanketHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<BestellungenLinkBlanketModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var response = new List<BestellungenLinkBlanketModel>();
				var rahmenItemsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data, false);
				var linkedBestellungenPositions = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetbyRahmenPositions(rahmenItemsEntities.Select(x => x.Nr).ToList());
				var BestellungenRahmenLinks = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetBelegfluss(linkedBestellungenPositions?.Select(x => x.Bestellung_Nr ?? -1).Distinct().ToList());
				if(BestellungenRahmenLinks != null && BestellungenRahmenLinks.Count > 0)
				{
					response = BestellungenRahmenLinks.Select(x => new BestellungenLinkBlanketModel(x)).ToList();
				}

				return ResponseModel<List<BestellungenLinkBlanketModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<BestellungenLinkBlanketModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<BestellungenLinkBlanketModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<BestellungenLinkBlanketModel>>.SuccessResponse();
		}

	}
}
