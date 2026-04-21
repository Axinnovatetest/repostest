using System;

namespace Psz.Core.BaseData.Handlers.Article.ProjectClass
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

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

				// -
				var articleClassEntity = Infrastructure.Data.Access.Tables.BSD.ProjectClassAccess.Get(this._data);

				// 
				var deletedId = Infrastructure.Data.Access.Tables.BSD.ProjectClassAccess.Delete(this._data);
				if(deletedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, deletedId, "Name",
						articleClassEntity.Name, "",
						Enums.ObjectLogEnums.Objects.ArticleConfig_ProjectClass.GetDescription(),
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

			var articleClassEntity = Infrastructure.Data.Access.Tables.BSD.ProjectClassAccess.Get(this._data);
			if(articleClassEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Project Class not found");
			}

			var articleIds = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByProjectTypeId(articleClassEntity.Id)
				?.Select(x => x.ArtikelNr)
				?.ToList();
			var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleIds);
			if(articles != null && articles.Count > 0)
				return ResponseModel<int>.FailureResponse($"Project Class [{articleClassEntity.Name}] is unsed for articles '{string.Join("', '", articles.Select(x => x.ArtikelNummer).Distinct().Take(5).ToList())}'");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
