namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class GetArtikelIdPlantBookingHandler: IHandle<Identity.Models.UserModel, ResponseModel<string>>
	{

		private Core.Identity.Models.UserModel _user;
		private string _ArtikelNr;
		public GetArtikelIdPlantBookingHandler(Core.Identity.Models.UserModel user, string artikelNr)
		{
			this._user = user;
			this._ArtikelNr = artikelNr;
		}

		public ResponseModel<string> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				if(_ArtikelNr is null || string.IsNullOrEmpty(_ArtikelNr) || _ArtikelNr == "" )
					 return ResponseModel<string>.SuccessResponse(null);

				var fetchedData = Infrastructure.Data.Access.Tables.ArtikelAccess.GetArtikelId(this._ArtikelNr);
					return ResponseModel<string>.SuccessResponse(fetchedData);
			

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
					throw;
			}
		}

		public ResponseModel<string> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}

			return ResponseModel<string>.SuccessResponse();
		}


	}
}
