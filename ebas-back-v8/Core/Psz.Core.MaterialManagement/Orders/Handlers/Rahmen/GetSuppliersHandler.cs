using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Rahmen
{
	public class GetSuppliersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetSuppliersHandler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<KeyValuePair<int, string>>();

				//var raSppliersNrs = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetSearchNrs("", "Kunden-Nr", false);
				var raSppliersNrs = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get()?.Select(x => x.Nummer ?? -1)?.ToList();
				var raSuppliersAdresses = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(raSppliersNrs);
				if(!string.IsNullOrEmpty(_data) && !string.IsNullOrWhiteSpace(_data))
				{
					raSuppliersAdresses = raSuppliersAdresses.Where(x => x.Lieferantennummer.ToString().Contains(_data) || x.Name1.ToLower().Contains(_data)).ToList();
				}
				response = raSuppliersAdresses?.Select(x => new KeyValuePair<int, string>(x.Nr, $"{x.Lieferantennummer} | {x.Name1}")).ToList();


				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}

	}
}
