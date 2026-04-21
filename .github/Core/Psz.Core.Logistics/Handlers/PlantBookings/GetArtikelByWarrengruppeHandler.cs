namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class GetArtikelByWarrengruppeHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{

		private Core.Identity.Models.UserModel _user;
		private string _data;
		public GetArtikelByWarrengruppeHandler(Core.Identity.Models.UserModel user, string data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				List<KeyValuePair<int, string>> ArtikelListByWarengruppe = new List<KeyValuePair<int, string>>();


				var LGTList = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByWarengruppe(this._data).ToList();

				foreach(var ArtikelWarengruppe in LGTList)
				{
					ArtikelListByWarengruppe.Add(new KeyValuePair<int, string>(
								ArtikelWarengruppe.ArtikelNr,
								ArtikelWarengruppe.ArtikelNummer
								));
				}

				if(ArtikelListByWarengruppe.Count == 0)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.FailureResponse("ArtikelWarengruppe List is empty !");
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(ArtikelListByWarengruppe);


			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
