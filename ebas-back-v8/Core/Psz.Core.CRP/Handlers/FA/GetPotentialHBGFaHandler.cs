using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;
namespace Psz.Core.CRP.Handlers.FA
{
	public class GetPotentialHBGFaHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<PotentialHBGFaResponseModel>>>
	{
		private PotentialHBGFaRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetPotentialHBGFaHandler(PotentialHBGFaRequestModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<PotentialHBGFaResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var hbgFaPositions = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetAvailableHBGByArticleId(this._data.ArticleId)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
				var hbgFaEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(hbgFaPositions.Select(x => x.ID_Fertigung ?? -1).ToList())
					?.Where(x => x.Kennzeichen?.Trim()?.ToLower() == "offen")?.ToList();

				// - filter by Offen FAs
				hbgFaPositions = hbgFaPositions.Where(x => hbgFaEntities.Exists(y => y.ID == x.ID_Fertigung) == true)?.ToList();
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(hbgFaPositions?.Select(x => x.Artikel_Nr ?? -1)?.ToList());
				var responseBody = new List<PotentialHBGFaResponseModel>();
				foreach(var faItem in hbgFaEntities)
				{
					var faPos = hbgFaPositions.Where(x => x.ID_Fertigung == faItem.ID);
					foreach(var faPosItem in faPos)
					{
						var article = articleEntities.FirstOrDefault(x => x.ArtikelNr == faPosItem.Artikel_Nr);
						responseBody.Add(new PotentialHBGFaResponseModel(faItem, faPosItem, article));
					}
				}

				responseBody = responseBody?.OrderBy(x => x.FaNummer)?.ThenBy(x => x.FaPositionId)?.ToList();
				// -
				return ResponseModel<List<PotentialHBGFaResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<PotentialHBGFaResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<PotentialHBGFaResponseModel>>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
			if(articleEntity == null)
			{
				return ResponseModel<List<PotentialHBGFaResponseModel>>.FailureResponse("Article not found");
			}
			if(articleEntity.aktiv != true)
			{
				return ResponseModel<List<PotentialHBGFaResponseModel>>.FailureResponse("Article not active");
			}
			if(articleEntity.UBG != true)
			{
				return ResponseModel<List<PotentialHBGFaResponseModel>>.FailureResponse("Article not UBG");
			}

			return ResponseModel<List<PotentialHBGFaResponseModel>>.SuccessResponse();
		}
	}
}