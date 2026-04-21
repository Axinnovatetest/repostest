using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetUBGFromBOMHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.FA.UBGProductionResponseModel>>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetUBGFromBOMHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.FA.UBGProductionResponseModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var bomEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data);
				var articleIds = bomEntities.Select(x => x.Artikel_Nr_des_Bauteils ?? -1).ToList();
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleIds)
					.Where(x => x.Warengruppe?.ToLower() == "ef" && x.UBG == true)?.ToList();

				var articleExtensions = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(articleIds);
				// -
				return ResponseModel<List<Models.FA.UBGProductionResponseModel>>.SuccessResponse(
					 articleEntities.Select(x =>
				new Models.FA.UBGProductionResponseModel(bomEntities.FirstOrDefault(b => b.Artikel_Nr_des_Bauteils == x.ArtikelNr),
				x,
				articleExtensions?.FirstOrDefault(e => e.ArticleId == x.ArtikelNr))).ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<Models.FA.UBGProductionResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.FA.UBGProductionResponseModel>>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return ResponseModel<List<Models.FA.UBGProductionResponseModel>>.FailureResponse($"Article not found");
			}
			if(articleEntity.aktiv != true)
			{
				return ResponseModel<List<Models.FA.UBGProductionResponseModel>>.FailureResponse($"Article not active");
			}
			var bomEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data);
			if(bomEntities == null || bomEntities.Count <= 0)
			{
				return ResponseModel<List<Models.FA.UBGProductionResponseModel>>.FailureResponse($"Article BOM empty");
			}

			return ResponseModel<List<Models.FA.UBGProductionResponseModel>>.SuccessResponse();
		}
	}
}