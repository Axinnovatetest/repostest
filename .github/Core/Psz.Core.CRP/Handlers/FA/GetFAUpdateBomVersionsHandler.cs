using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFAUpdateBomVersionsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, int>>>>
	{
		private string _data { get; set; }
		private string _data2 { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAUpdateBomVersionsHandler(string data, string data2, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
			this._data2 = data2;
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

				List<KeyValuePair<int, int>> response = new List<KeyValuePair<int, int>>();
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data);
				var allBonVersion = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetAllBOMVersions(articleEntity?.ArtikelNr ?? -1);
				var biggestBomVersion = allBonVersion?.Max();
				var bomVersionsEntities = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAUpdateBOMVersions(articleEntity?.ArtikelNr ?? -1, this._data2);
				if(bomVersionsEntities != null && bomVersionsEntities.Count > 0)
				{
					foreach(var item in bomVersionsEntities)
					{
						if(articleEntity.CP_required == true)
						{
							var cpVersion = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetMaxCPVersionByBomVersion(articleEntity?.ArtikelNr ?? -1, item);
							if(cpVersion != null)
								response.Add(new KeyValuePair<int, int>(item, item));
							else
							{
								if(biggestBomVersion.HasValue && item == biggestBomVersion.Value)
									response.Add(new KeyValuePair<int, int>(item, item));
							}
						}
						else
						{
							response.Add(new KeyValuePair<int, int>(item, item));
						}
					}
				}


				return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data},_data2:{_data2}");
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
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data);
			if(articleEntity == null)
				return ResponseModel<List<KeyValuePair<int, int>>>.FailureResponse(key: "1", value: $"Article not found");
			return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse();
		}
	}
}