using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Packaging
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

				var packaging = Infrastructure.Data.Access.Tables.BSD.Verpackungseinheiten_DefinitionenAccess.Get(this._data);
				var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(packaging.Artikelnummer);
				if(articles != null)
				{
					return new ResponseModel<int>
					{
						Success = false,
						Errors = new List<ResponseModel<int>.ResponseError>
						{
							new ResponseModel<int>.ResponseError
							{
								Key ="",
								Value = $"Packaging is associated with Article {articles.ArtikelNummer} ({articles.Bezeichnung1})"
							}
						}
					};
				}

				// 

				var deletedId = Infrastructure.Data.Access.Tables.BSD.Verpackungseinheiten_DefinitionenAccess.Delete(this._data);
				if(deletedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, deletedId, "Masse LxBxH (in mm)",
						packaging.Masse_LxBxH__in_mm_, "",
						Enums.ObjectLogEnums.Objects.ArticleConfig_Packaging.GetDescription(),
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

			if(Infrastructure.Data.Access.Tables.BSD.Verpackungseinheiten_DefinitionenAccess.Get(this._data) == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Packaging Type not found"}
					}
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
