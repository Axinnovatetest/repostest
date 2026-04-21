using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class UpdateArticlePricePackagingHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.SalesExtension.SalesItemModel _data { get; set; }
		public UpdateArticlePricePackagingHandler(Identity.Models.UserModel user, Models.Article.SalesExtension.SalesItemModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				lock(Locks.ArticleEditLock.GetOrAdd(this._data.ArticleId, new object()))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}
					// -
					var salesItemEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Get(this._data.Id);
					var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
					var _salesItemEntity = this._data.ToEntity(salesItemEntity, this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, logs, Enums.ObjectLogEnums.LogType.Edit, true);

					if(salesItemEntity.ArticleSalesTypeId == (int)Common.Enums.ArticleEnums.SalesItemType.Serie)
					{
						var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
						if(artikelEntity != null)
						{
							artikelEntity.Verpackungsart = _salesItemEntity.Verpackungsart;
							artikelEntity.Verpackungsmenge = (int?)_salesItemEntity.Verpackungsmenge;
							Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditPackaging(artikelEntity);
						}
					}
					// -
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);

					//- update
					salesItemEntity.VerpackungsartId = _salesItemEntity.VerpackungsartId;
					salesItemEntity.Verpackungsart = _salesItemEntity.Verpackungsart;
					salesItemEntity.Verpackungsmenge = _salesItemEntity.Verpackungsmenge;

					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data.ArticleId);

					// -
					return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.UpdatePackaging(salesItemEntity));
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

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
			if(articleEntity == null)
				return ResponseModel<int>.FailureResponse("Article no found");

			if(articleEntity.aktiv.HasValue && !articleEntity.aktiv.Value)
				return ResponseModel<int>.FailureResponse("Article is not Active");

			if(Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Get(this._data.Id) == null)
				return ResponseModel<int>.FailureResponse("Sales item not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
