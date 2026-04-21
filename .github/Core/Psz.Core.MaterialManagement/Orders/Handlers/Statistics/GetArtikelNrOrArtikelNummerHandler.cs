using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetArtikelNrOrArtikelNummerHandler: IHandle<GetArtikleNrOrArtikelNummerRequestModel, ResponseModel<GetArtikleNrOrArtikelNummerModel>>
	{

		private GetArtikleNrOrArtikelNummerRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetArtikelNrOrArtikelNummerHandler(Identity.Models.UserModel user, GetArtikleNrOrArtikelNummerRequestModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<GetArtikleNrOrArtikelNummerModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var entites = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetArtikelNrOrArtikelNummer(_data.ArtikelNr, _data.Artiklenummer, false);

				if(entites is null || entites.Count() == 0)
					return ResponseModel<GetArtikleNrOrArtikelNummerModel>.NotFoundResponse();

				var response = entites.Select(x => new GetArtikleNrOrArtikelNummerModel(x)).ToList();

				return ResponseModel<GetArtikleNrOrArtikelNummerModel>.SuccessResponse(response.FirstOrDefault());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<GetArtikleNrOrArtikelNummerModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<GetArtikleNrOrArtikelNummerModel>.AccessDeniedResponse();
			}

			return ResponseModel<GetArtikleNrOrArtikelNummerModel>.SuccessResponse();
		}

	}
}
