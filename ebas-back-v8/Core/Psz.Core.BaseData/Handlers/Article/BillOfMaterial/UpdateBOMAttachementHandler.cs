using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	public class UpdateBOMAttachementHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.UpdateAttachmentModel _data { get; set; }
		public UpdateBOMAttachementHandler(Identity.Models.UserModel user, Models.UpdateAttachmentModel data)
		{
			_user = user;
			_data = data;
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

				//var newAttachmentId = Psz.Core.Common.Helpers.ImageFileHelper.updateImage(-1, this._data.AttachmentFile, this._data.AttachmentFileExtension,(int)Enums.ArticleEnums.ArticleFileType.BOMPosition);
				var newAttachmentId = Psz.Core.Common.Helpers.ImageFileHelper.NewTempFile(this._data.AttachmentFile, this._data.AttachmentFileExtension);
				return ResponseModel<int>.SuccessResponse(newAttachmentId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.Id);
			if(articleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Article not found" }
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
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
