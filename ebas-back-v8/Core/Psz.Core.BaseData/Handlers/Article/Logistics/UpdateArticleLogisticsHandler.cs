using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class UpdateArticleLogisticsHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.Logistics.ArticleLogisticsModel _data { get; set; }
		public UpdateArticleLogisticsHandler(Identity.Models.UserModel user, Models.Article.Logistics.ArticleLogisticsModel data)
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
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleID);
					var articleLogisticsEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.GetByArticleId(this._data.ArticleID);

					// - logging - 2022-02-25
					var articleLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
					var extensionLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
					if(articleLogisticsEntity != null)
					{
						var updateEntity = this._data.ToEntity(articleLogisticsEntity, this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleID, extensionLogs);
						updateEntity.UpdateUserId = this._user.Id;
						updateEntity.UpdateTime = DateTime.Now;
						//
						updateEntity.CreateUserId = articleLogisticsEntity.CreateUserId;
						updateEntity.CreateTime = articleLogisticsEntity.CreateTime;
						Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.Update(updateEntity);
					}
					else
					{
						var updateEntity = this._data.ToEntity(articleLogisticsEntity, this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleID, extensionLogs);
						updateEntity.CreateUserId = this._user.Id;
						updateEntity.CreateTime = DateTime.Now;
						Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.Insert(updateEntity);
					}

					//- update Artikel
					var articleData = this._data.ToArtikelEntity(articleEntity, this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleID, articleLogs);

					// - save logs 
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(articleLogs);
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(extensionLogs);

					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data.ArticleID);

					// -
					return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Edit(articleData));
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleID);
			if(articleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "2", Value = "Article not found"}
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
			if(this._data.VDALabel.HasValue && this._data.VDALabel.Value && this._data.ULLabel.HasValue && this._data.ULLabel.Value)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "2", Value = "Both VDA and UL labels cannot be true"}
					}
				};
			}

			if(string.IsNullOrWhiteSpace(this._data.ZolltarifNummer))
			{
				return ResponseModel<int>.FailureResponse("[Customs Number (Zolltariffnummer)]: invalid data.");
			}
			if(this._data.ZolltarifNummer.Length != 11)
			{
				return ResponseModel<int>.FailureResponse("[Customs Number (Zolltariffnummer)]: invalid data length. Length must be 11 characters.");
			}
			if(string.IsNullOrWhiteSpace(this._data.UrsprungslandName))
			{
				return ResponseModel<int>.FailureResponse("[Origin Country (Ursprungsland)]: invalid data.");
			}
			// - 2023-08-20 - Original country slinked to production country for EF
			//if(articleEntity.Warengruppe?.Trim()?.ToLower() == "ef")
			//{
			//	if(articleEntity.ProductionCountryCode?.ToLower()?.Trim() != _data.UrsprungslandName?.ToLower()?.Trim())
			//	{
			//		return ResponseModel<int>.FailureResponse($"[Origin Country (Ursprungsland)]: selected value [{_data.UrsprungslandName?.Trim()}] is different from production country [{articleEntity.ProductionCountryCode?.Trim()}].");
			//	}
			//}
			var productionExtension = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(articleEntity.ArtikelNr);
			// - 2023-06-26 - add Teams
			if(this._data.TeamsId > 0 && this._data.TeamsSiteId > 0 && productionExtension?.ProductionPlace1_Id != this._data.TeamsSiteId)
			{
				return ResponseModel<int>.FailureResponse("Team should belong to selected Production place.");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
