using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;
namespace Psz.Core.CRP.Handlers.FA
{
	public class GetPotentialUBGFaHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<PotentialUBGFaResponseModel>>>
	{
		private PotentialHBGFaRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetPotentialUBGFaHandler(PotentialHBGFaRequestModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<PotentialUBGFaResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var ubgFas = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetAvailableUBGByArticle(this._data.ArticleId)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();

				// -
				return ResponseModel<List<PotentialUBGFaResponseModel>>.SuccessResponse(ubgFas?.Select(x => new PotentialUBGFaResponseModel(x)).ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<PotentialUBGFaResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<PotentialUBGFaResponseModel>>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
			if(articleEntity == null)
			{
				return ResponseModel<List<PotentialUBGFaResponseModel>>.FailureResponse("Article not found");
			}
			if(articleEntity.aktiv != true)
			{
				return ResponseModel<List<PotentialUBGFaResponseModel>>.FailureResponse("Article not active");
			}
			if(articleEntity.UBG != true)
			{
				return ResponseModel<List<PotentialUBGFaResponseModel>>.FailureResponse("Article not UBG");
			}

			return ResponseModel<List<PotentialUBGFaResponseModel>>.SuccessResponse();
		}
	}
}