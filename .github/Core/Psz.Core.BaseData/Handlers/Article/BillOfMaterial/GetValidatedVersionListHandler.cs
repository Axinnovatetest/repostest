using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	public class GetValidatedVersionListHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.BillOfMaterial.VersionListModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetValidatedVersionListHandler(Identity.Models.UserModel user, int articleID)
		{
			_user = user;
			_data = articleID;
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.VersionListModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var bomExtensionEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtension_SnapshotAccess.GetByArticleId(this._data);


				return ResponseModel<List<Models.Article.BillOfMaterial.VersionListModel>>.SuccessResponse(
					bomExtensionEntities?.Select(x =>
					new Models.Article.BillOfMaterial.VersionListModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.VersionListModel>> Validate()
		{
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return new ResponseModel<List<Models.Article.BillOfMaterial.VersionListModel>>()
				{
					Errors = new List<ResponseModel<List<Models.Article.BillOfMaterial.VersionListModel>>.ResponseError>{
							new ResponseModel<List<Models.Article.BillOfMaterial.VersionListModel>>.ResponseError() { Key = "", Value = "Article not found" }
						}
				};
			}
			return ResponseModel<List<Models.Article.BillOfMaterial.VersionListModel>>.SuccessResponse();
		}
	}
}
