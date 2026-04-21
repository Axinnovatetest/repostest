using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetFehlermaterialFALagerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public GetFehlermaterialFALagerHandler(Identity.Models.UserModel user)
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

				var lagerOrtEntities = Infrastructure.Data.Access.Tables.MTM.LagerorteAccess.Get();
				var fehlerLager = new List<int>
				{
					(int)Enums.StatisticsEnums.Lager.CZ,
					(int)Enums.StatisticsEnums.Lager.TN,
					(int)Enums.StatisticsEnums.Lager.DE,
					(int)Enums.StatisticsEnums.Lager.AL,
					(int)Enums.StatisticsEnums.Lager.BETN,
					(int)Enums.StatisticsEnums.Lager.WSTN,
					(int)Enums.StatisticsEnums.Lager.GZTN,
				};
				var response = lagerOrtEntities?.Where(x => fehlerLager.Contains(x.Lagerort_id))
					?.Select(y => new KeyValuePair<int, string>(y.Lagerort_id, y.Lagerort)).ToList();

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
