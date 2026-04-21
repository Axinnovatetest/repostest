using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Autocomplete
{
	public class AutocompleteArticleHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AutocompleteArticleHandler(string data, Identity.Models.UserModel user)
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

				var articlesEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetLikeNumber(this._data);
				var response = new List<KeyValuePair<string, string>>();
				if(articlesEntity != null && articlesEntity.Count > 0)
				{
					foreach(var item in articlesEntity)
					{
						response.Add(new KeyValuePair<string, string>(item.ArtikelNummer, item.ArtikelNummer));
					}
				}
				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
	}
}