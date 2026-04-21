using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetHauptLagersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public GetHauptLagersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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

				// -
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetHauptLagers()
					?.Where(l => l.Lagerort_id != 58)
					?.Select(x => new KeyValuePair<int, string>(x.Lagerort_id, x.Lagerort))
					?.ToList());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user is null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}

	}
}
