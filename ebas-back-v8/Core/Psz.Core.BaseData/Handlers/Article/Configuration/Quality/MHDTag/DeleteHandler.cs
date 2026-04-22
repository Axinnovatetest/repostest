using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Quality.MHDTag
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

				var tag = Infrastructure.Data.Access.Tables.BSD.ArtikelMHD_TagAccess.Get(this._data);
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByMHDTag(tag.Tag);
				if(articleEntities != null && articleEntities.Count > 0)
				{
					return new ResponseModel<int>
					{
						Success = false,
						Errors = new List<ResponseModel<int>.ResponseError>
							{
								new ResponseModel<int>.ResponseError
								{
									Key ="",
									Value = $"Tag is used by articles '{string.Join("', '", articleEntities.Select(x => x.ArtikelNummer).Distinct().Take(5).ToList())}'"
								}
							}
					};
				}

				// 
				var deletedId = Infrastructure.Data.Access.Tables.BSD.ArtikelMHD_TagAccess.Delete(this._data);
				if(deletedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, deletedId, "Tag",
						tag.Tag, "",
						Enums.ObjectLogEnums.Objects.ArticleConfig_MHDTag.GetDescription(),
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

			if(Infrastructure.Data.Access.Tables.BSD.ArtikelMHD_TagAccess.Get(this._data) == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Tag not found"}
					}
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
