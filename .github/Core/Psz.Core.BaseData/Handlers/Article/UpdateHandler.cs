using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class UpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.ArticleModel _data { get; set; }


		public UpdateHandler(Identity.Models.UserModel user, Models.Article.ArticleModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				lock((Locks.ArticleEditLock.GetOrAdd(this._data.ArtikelNr, new object())))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					var logs = LogChanges();
					var artikelUpdated = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Edit(this._data.ToEntity());
					if(artikelUpdated > 0)
					{
						// save update logs
						if(logs.Count > 0)
						{
							Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
						}

						var _artikelExtensionEntity = this._data.ToExtensionEntity();
						var artikelExtensionEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(this._data.ArtikelNr);
						if(artikelExtensionEntity != null)
						{
							_artikelExtensionEntity.Id = artikelExtensionEntity.Id;
							Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.Update(_artikelExtensionEntity);
						}
						else
						{
							_artikelExtensionEntity.CreatorID = this._user.Id;
							_artikelExtensionEntity.DateCreation = DateTime.Now;
							Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.Insert(_artikelExtensionEntity);
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

			// Copper attributes
			if(articleEntity.CuGewicht != this._data.CuGewicht)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CuGewicht", articleEntity.CuGewicht.ToString(), this._data.CuGewicht.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.DEL != this._data.DEL)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "DEL", articleEntity.DEL.ToString(), this._data.DEL.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.VKFestpreis != this._data.VKFestpreis)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "VKFestpreis", articleEntity.VKFestpreis.ToString(), this._data.VKFestpreis.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			if(articleExtension != null)
			{
				if(articleExtension.CopperCostBasis != this._data.CopperCostBasis)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CopperCostBasis", articleExtension.CopperCostBasis.ToString(), this._data.CopperCostBasis.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
				if(articleExtension.CopperCostBasis150 != this._data.CopperCostBasis150)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CopperCostBasis150", articleExtension.CopperCostBasis150.ToString(), this._data.CopperCostBasis150.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
			}
			else
			{
				if(this._data.CopperCostBasis.HasValue)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CopperCostBasis", "", this._data.CopperCostBasis.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
				if(this._data.CopperCostBasis150.HasValue)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CopperCostBasis150", "", this._data.CopperCostBasis150.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
			}
			//
			return logs;
		}
	}

}
