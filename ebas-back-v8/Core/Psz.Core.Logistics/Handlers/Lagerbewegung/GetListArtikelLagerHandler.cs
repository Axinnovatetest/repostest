using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class GetListArtikelLagerHandler: IHandle<Identity.Models.UserModel, ResponseModel<ListeArtikelLager>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int _lagerID { get; set; }

		public GetListArtikelLagerHandler(Identity.Models.UserModel user, int lagerID)
		{
			this._user = user;
			this._lagerID = lagerID;


		}
		public ResponseModel<ListeArtikelLager> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var response = new ListeArtikelLager();
				var listepositions = Infrastructure.Data.Access.Tables.Logistics.LagerArtikelAccess.GetListArtikelLagerPositif(this._lagerID);
				if(listepositions != null && listepositions.Count() > 0)
				{

					response.listeArtikel = listepositions.Select(k => new LagerArtikelModel(k)).ToList();

				}




				return ResponseModel<ListeArtikelLager>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<ListeArtikelLager> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<ListeArtikelLager>.AccessDeniedResponse();
			}

			return ResponseModel<ListeArtikelLager>.SuccessResponse();
		}
	}
}
