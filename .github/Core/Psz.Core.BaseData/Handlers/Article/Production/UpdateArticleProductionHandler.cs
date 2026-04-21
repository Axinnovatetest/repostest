using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Production
{
	public class UpdateArticleProductionHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.Production.ArticleProductionModel _data { get; set; }

		public UpdateArticleProductionHandler(Identity.Models.UserModel user, Models.Article.Production.ArticleProductionModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				lock(Locks.ArticleEditLock.GetOrAdd(this._data.ArticleID, new object()))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}
					var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

					var response = -1;
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleID);
					var articleProductionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(this._data.ArticleID);
					var Entity = this._data.ToEntity(articleProductionEntity, this._user, Enums.ObjectLogEnums.Objects.Article, articleEntity.ArtikelNr, logs, articleProductionEntity == null ? Enums.ObjectLogEnums.LogType.Add : Enums.ObjectLogEnums.LogType.Edit);
					if(Entity.AlternativeProductionPlace.HasValue && !Entity.AlternativeProductionPlace.Value)
					{
						Entity.ProductionPlace2_Name = null;
						Entity.ProductionPlace2_Id = null;
						Entity.ProductionPlace3_Name = null;
						Entity.ProductionPlace3_Id = null;
					}
					if(articleProductionEntity == null)
					{
						Entity.CreateUserId = this._user.Id;
						Entity.CreateTime = DateTime.Now;
						response = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.Insert(Entity);
					}
					else
					{

						Entity.Id = articleProductionEntity.Id;
						Entity.UpdateUserId = this._user.Id;
						Entity.UpdateTime = DateTime.Now;
						Entity.CreateUserId = articleProductionEntity.CreateUserId;
						Entity.CreateTime = articleProductionEntity.CreateTime;
						response = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.Update(Entity);
					}

					// - Artikel
					var artikelEntity = this._data.ToArtikelEntity(articleEntity, this._user, Enums.ObjectLogEnums.Objects.Article, articleEntity.ArtikelNr, logs, Enums.ObjectLogEnums.LogType.Edit);
					Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditProduction(artikelEntity);

					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);

					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data.ArticleID);

					// -
					return ResponseModel<int>.SuccessResponse(response);
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

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleID);
			var prods = new List<int>();
			var Entity = this._data.ToEntity(null);
			if(Entity.AlternativeProductionPlace.HasValue && !Entity.AlternativeProductionPlace.Value)
			{
				Entity.ProductionPlace2_Id = null;
				Entity.ProductionPlace3_Id = null;

				Entity.ProductionPlace2_Name = null;
				Entity.ProductionPlace3_Name = null;
			}
			//detecting duplicate prod sites
			if(Entity.ProductionPlace1_Id != null)
				prods.Add((int)Entity.ProductionPlace1_Id);

			if(Entity.ProductionPlace2_Id != null)
				prods.Add((int)Entity.ProductionPlace2_Id);

			if(Entity.ProductionPlace3_Id != null)
				prods.Add((int)Entity.ProductionPlace3_Id);
			var duplicates = prods.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key);

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
			if(articleEntity.aktiv.HasValue && !articleEntity.aktiv.Value)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Article is not Active"}
					}
				};
			}
			if(Entity.ProductionPlace1_Id == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "2", Value = "Article must have at least one production site"}
					}
				};
			}
			if(prods != null)
			{
				if(duplicates != null && duplicates.Count() > 0)
				{
					return new ResponseModel<int>()
					{
						Success = false,
						Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "2", Value = "Article can't have duplicate production sites"}
					}
					};
				}
			}
			if(Entity.AlternativeProductionPlace.HasValue && Entity.AlternativeProductionPlace.Value && Entity.ProductionPlace2_Id == null && Entity.ProductionPlace3_Id == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "2", Value = "You must select at least one alternative production site"}
					}
				};
			}

			// - 2023-06-26 - add Teams
			if(this._data.TeamsId > 0 && this._data.TeamsSiteId > 0 && this._data.ProductionPlace1_Id != this._data.TeamsSiteId)
			{
				return ResponseModel<int>.FailureResponse("Team should belong to selected Production place.");
			}
			if(articleEntity.ProductionLotSize.HasValue && articleEntity.ProductionLotSize.Value < 0)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Update aborted: [Production Lot Size] must be positive or null."}
					}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
