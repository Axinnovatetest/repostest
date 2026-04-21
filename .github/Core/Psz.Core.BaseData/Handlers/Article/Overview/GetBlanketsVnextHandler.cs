using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class GetBlanketsVnextHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.Overview.BlanketResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetBlanketsVnextHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Article.Overview.BlanketResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Article.Overview.BlanketResponseModel>();
				// -
				var ras = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
				var raExtensions = new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity>();

				var raPoss = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
				var raPosExtensions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByArticle(this._data)
					?? new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();
				if(raPosExtensions.Count > 0)
				{
					var raIds = raPosExtensions?.Select(x => x.RahmenNr)?.ToList();

					ras = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(raIds);
					raExtensions = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNrs(raIds);
					raPoss = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(
						raPosExtensions?.Select(x => x.AngeboteArtikelNr)?.ToList());

					// - 
					foreach(var raPosExt in raPosExtensions)
					{
						var ra = ras.FirstOrDefault(x => x.Nr == raPosExt.RahmenNr);
						if(ra != null)
						{
							var raExt = raExtensions.FirstOrDefault(x => x.AngeboteNr == ra.Nr);
							var raPos = raPoss.FirstOrDefault(x => x.Nr == raPosExt.AngeboteArtikelNr);
							responseBody.Add(new Models.Article.Overview.BlanketResponseModel(ra, raExt, raPos, raPosExt));
						}
					}
				}

				// -
				return ResponseModel<List<Models.Article.Overview.BlanketResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Overview.BlanketResponseModel>> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Overview.BlanketResponseModel>>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return ResponseModel<List<Models.Article.Overview.BlanketResponseModel>>.FailureResponse("Article not found");
			}

			// -
			return ResponseModel<List<Models.Article.Overview.BlanketResponseModel>>.SuccessResponse();
		}
	}
}
