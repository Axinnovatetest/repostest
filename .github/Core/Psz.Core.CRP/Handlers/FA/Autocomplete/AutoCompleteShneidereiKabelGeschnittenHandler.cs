using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA.Autocomplete
{
	public class AutoCompleteShneidereiKabelGeschnittenHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, int>>>>
	{
		private int _data { get; set; }
		private string _dataTerm { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AutoCompleteShneidereiKabelGeschnittenHandler(int data, string term, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
			this._dataTerm = term;
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

				var articlesEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAListShneidereiKabelGeschnitten(this._data, _dataTerm);
				var response = new List<KeyValuePair<int, int>>();
				if(articlesEntity != null && articlesEntity.Count > 0)
					response = articlesEntity.Select(x => new KeyValuePair<int, int>((int)x.Fertigungsnummer, (int)x.Fertigungsnummer)).ToList();
				return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, int>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, int>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse();
		}
	}
}