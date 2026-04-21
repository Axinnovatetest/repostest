using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Rahmen
{
	public class GetVorfailNrsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, int>>>>
	{

		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetVorfailNrsHandler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<KeyValuePair<int, int>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var response = new List<KeyValuePair<int, int>>();

				var nrs = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetSearchNrs(_data, "Angebot-Nr");
				response = nrs?.Select(x => new KeyValuePair<int, int>(x, x)).ToList();

				return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, int>>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, int>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse();
		}

	}
}
