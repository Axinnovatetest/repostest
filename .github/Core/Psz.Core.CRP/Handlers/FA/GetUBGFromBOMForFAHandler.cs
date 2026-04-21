using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetUBGFromBOMForFAHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.FA.UBGProductionResponseModel>>>
	{
		private UBGProductionRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetUBGFromBOMForFAHandler(UBGProductionRequestModel data, Identity.Models.UserModel user)
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
				var bomEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data.ArticleId);
				var articleIds = bomEntities.Select(x => x.Artikel_Nr_des_Bauteils ?? -1).ToList();
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleIds)
					.Where(x => x.Warengruppe?.ToLower() == "ef" && x.UBG == true)?.ToList();

				var articleExtensions = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(articleIds);
				// -
				return ResponseModel<List<Models.FA.UBGProductionResponseModel>>.SuccessResponse(
					 articleEntities.Select(x =>
				new Models.FA.UBGProductionResponseModel(this._data.FaQuantity ?? 1,
				bomEntities.FirstOrDefault(b => b.Artikel_Nr_des_Bauteils == x.ArtikelNr),
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

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data.ArticleId);
			if(articleEntity == null)
			{
				return ResponseModel<List<Models.FA.UBGProductionResponseModel>>.FailureResponse($"Article not found");
			}
			if(articleEntity.aktiv != true)
			{
				return ResponseModel<List<Models.FA.UBGProductionResponseModel>>.FailureResponse($"Article not active");
			}
			var bomEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(_data.ArticleId);
			if(bomEntities == null || bomEntities.Count <= 0)
			{
				return ResponseModel<List<Models.FA.UBGProductionResponseModel>>.FailureResponse($"Article BOM empty");
			}

			return ResponseModel<List<Models.FA.UBGProductionResponseModel>>.SuccessResponse();
		}
	}
}