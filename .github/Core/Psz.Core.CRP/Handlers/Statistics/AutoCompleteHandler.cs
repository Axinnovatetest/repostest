using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.Statistics
{
	public partial class CrpStatisticsService
	{
		public ResponseModel<List<string>> AutoComplete(Identity.Models.UserModel user, int columnValue, string searchValue)
		{
			var validationResponse = this.ValidateAutoComplete(user);
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			try
			{
				var response = new List<string>();
				string value = "";
				switch(columnValue)
				{
					case 1:
						value = "L.[AngebotNr]";
						break;
					case 2:
						value = "AR.Position";
						break;
					case 3:
						value = "ART.Artikelnummer";
						break;
					case 4:
						value = "L.Username";
						break;
				}

				if(value != "")
				{
					response = Infrastructure.Data.Access.Joins.CRP.CRPStatisticsAccess.getAutoComplete(value, searchValue);
				}

				return ResponseModel<List<string>>.SuccessResponse(
					response);



			} catch(System.Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw new NotImplementedException();
			}
		}
		public ResponseModel<List<string>> ValidateAutoComplete(Identity.Models.UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<string>>.AccessDeniedResponse();
			}

			return ResponseModel<List<string>>.SuccessResponse();
		}
	}
}