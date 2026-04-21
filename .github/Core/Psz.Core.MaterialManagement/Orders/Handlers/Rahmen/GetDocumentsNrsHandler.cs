using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Rahmen
{
	public class GetDocumentsNrsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{

		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetDocumentsNrsHandler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<KeyValuePair<string, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var response = new List<KeyValuePair<string, string>>();

				var nrs = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetProjectNrsNrs(_data);
				response = nrs?.Select(x => new KeyValuePair<string, string>(x, x)).ToList();

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}

	}
}
