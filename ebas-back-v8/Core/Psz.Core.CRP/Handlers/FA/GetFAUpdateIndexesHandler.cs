using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFAUpdateIndexesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAUpdateIndexesHandler(string data, Identity.Models.UserModel user)
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

				List<KeyValuePair<string, string>> response = new List<KeyValuePair<string, string>>();
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data);
				var indexesEntities = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAUpdateIndexes(articleEntity?.ArtikelNr ?? -1);
				if(indexesEntities != null && indexesEntities.Count > 0)
					response = indexesEntities.Select(x => new KeyValuePair<string, string>(x, x)).ToList();

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
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data);
			if(articleEntity == null)
				return ResponseModel<List<KeyValuePair<string, string>>>.FailureResponse(key: "1", value: $"Article not found");
			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
	}
}