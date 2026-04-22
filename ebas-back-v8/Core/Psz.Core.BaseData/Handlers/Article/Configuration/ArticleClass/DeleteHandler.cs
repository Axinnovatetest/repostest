using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.ArticleClass
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public DeleteHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}



				// 
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelstammKlassifizierungAccess.Get(this._data);
				var deletedId = Infrastructure.Data.Access.Tables.PRS.ArtikelstammKlassifizierungAccess.Delete(this._data);
				if(deletedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, deletedId, "Bezeichnung",
						articleEntity.Bezeichnung, "",
						Enums.ObjectLogEnums.Objects.ArticleConfig_Class.GetDescription(),
						Enums.ObjectLogEnums.LogType.Delete));
				}
				return ResponseModel<int>.SuccessResponse(deletedId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var articleClassEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelstammKlassifizierungAccess.Get(this._data);
			if(articleClassEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Article classification not found");
			}

			var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByClassId(this._data);
			if(articles != null && articles.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Article Class [{articleClassEntity.Klassifizierung}] is used for articles '{string.Join("', '", articles.Select(x => x.ArtikelNummer).Distinct().Take(5).ToList())}'");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
