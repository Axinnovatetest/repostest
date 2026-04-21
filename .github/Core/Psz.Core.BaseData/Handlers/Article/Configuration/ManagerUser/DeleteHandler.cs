using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ManagerUser
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


				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data);
				var deletedId = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.DeleteByUser(this._data);
				if(deletedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, deletedId, "UserName", userEntity.Username,
						"",
						Enums.ObjectLogEnums.Objects.ArticleConfig_ManagerUsers.GetDescription(),
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

			var userEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetByUserId(this._data);
			if(userEntities == null || userEntities.Count <= 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>()
				   {
					   new ResponseModel<int>.ResponseError(){ Key = "", Value= "User not found"}
				   }
				};
			}

			var hasArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetHasArtikels(this._data);
			if(hasArticles != null && hasArticles.Count > 0)
			{
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(hasArticles.Select(x => x.ArtikelNr)?.ToList());
				return new ResponseModel<int>
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
						{
							new ResponseModel<int>.ResponseError
							{
								Key = "",
								Value = $"Manager is assigned to Articles '{string.Join("', '", articleEntities.Select(x => x.ArtikelNummer).Distinct().Take(5).ToList())}'"
							}
						}
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
