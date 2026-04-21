using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetOriginalAritclesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Production.OriginalArticleModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetOriginalAritclesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Production.OriginalArticleModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var wahrungenEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetProductionOriginalArticles();

				var response = new List<Models.Production.OriginalArticleModel>();

				foreach(var wahrungenEntity in wahrungenEntities)
				{
					response.Add(new Models.Production.OriginalArticleModel(wahrungenEntity));
				}

				return ResponseModel<List<Models.Production.OriginalArticleModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Production.OriginalArticleModel>> Handle(string name)
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var wahrungenEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetProductionOriginalArticles(name)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();

				var response = new List<Models.Production.OriginalArticleModel>();

				foreach(var wahrungenEntity in wahrungenEntities)
				{
					response.Add(new Models.Production.OriginalArticleModel(wahrungenEntity));
				}

				return ResponseModel<List<Models.Production.OriginalArticleModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Production.OriginalArticleModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Production.OriginalArticleModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Production.OriginalArticleModel>>.SuccessResponse();
		}
	}
}
