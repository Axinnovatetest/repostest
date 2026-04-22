using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetTypesForFACreationHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private readonly Identity.Models.UserModel _user;
		private readonly int _data;

		public GetTypesForFACreationHandler(Identity.Models.UserModel user, int data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var types = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetTypesForFACreation(_data);
				var response = types?.Select(t => new KeyValuePair<int, string>(t.Key, TranslateType(t.Value))).ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(_user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}

		internal string TranslateType(string type)
		{
			if(type.ToLower() == "serie")
				return "Serie";
			else if(type.ToLower() == "first sample")
				return "Erstmuster";
			else if(type.ToLower() == "prototype")
				return "Prototyp";
			else
				return "";
		}
	}
}