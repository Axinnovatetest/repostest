using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Quality.MHDTag
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Configuration.Quality.MHDTagModel _data { get; set; }
		public AddHandler(UserModel user, Models.Article.Configuration.Quality.MHDTagModel tag)
		{
			_user = user;
			_data = tag;
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

				// 
				this._data.CreateTime = DateTime.Now;
				this._data.CreateUserId = this._user.Id;

				var vdEntity = this._data.ToEntity();
				var insertedId = Infrastructure.Data.Access.Tables.BSD.ArtikelMHD_TagAccess.Insert(vdEntity);
				if(insertedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, insertedId, "Tag", "",
						vdEntity.Tag,
						Enums.ObjectLogEnums.Objects.ArticleConfig_MHDTag.GetDescription(),
						Enums.ObjectLogEnums.LogType.Add));
				}
				return ResponseModel<int>.SuccessResponse(insertedId);
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

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
