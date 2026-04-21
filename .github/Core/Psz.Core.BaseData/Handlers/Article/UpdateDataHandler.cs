using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class UpdateDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.ArticleDataModel _data { get; set; }


		public UpdateDataHandler(Identity.Models.UserModel user, Models.Article.ArticleDataModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				lock(Locks.ArticleEditLock.GetOrAdd(this._data.ArtikelNr, new object()))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					var logs = LogChanges();
					var artikelUpdated = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditData(this._data.ToEntity());
					var artikelExtensionUpdated = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.EditExtensionData(this._data.ToExtensionEntity());
					if(artikelUpdated > 0)
					{
						// save update logs
						if(logs.Count > 0)
						{
							Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
						}

						// - 2022-03-30
						CreateHandler.generateFileDAT(this._data.ArtikelNr);

						return ResponseModel<int>.SuccessResponse(artikelUpdated);
					}

					return ResponseModel<int>.SuccessResponse(0);
				}
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

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr);
			if(articleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}

			if(string.IsNullOrWhiteSpace(this._data.Bezeichnung1))
				return ResponseModel<int>.FailureResponse("[Herstellernummer/Bezeichnung 1] should not be empty");

			return ResponseModel<int>.SuccessResponse();
		}

		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr);
			var articleExtension = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(this._data.ArtikelNr);
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Edit;

			if(articleEntity.ArtikelNummer != this._data.ArtikelNummer)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "ArtikelNummer", articleEntity.ArtikelNummer, this._data.ArtikelNummer, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			if(articleEntity.Bezeichnung1 != this._data.Bezeichnung1)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Bezeichnung1", articleEntity.Bezeichnung1, this._data.Bezeichnung1, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			if(articleEntity.Bezeichnung2 != this._data.Bezeichnung2)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Bezeichnung2", articleEntity.Bezeichnung2, this._data.Bezeichnung2, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}


			//
			return logs;
		}
	}

}
