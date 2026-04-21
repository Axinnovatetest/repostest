using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	public class IsHBGHandler: IHandle<Identity.Models.UserModel, ResponseModel<bool>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public IsHBGHandler(Identity.Models.UserModel user, int ArticleNr)
		{
			this._user = user;
			this._data = ArticleNr;
		}
		public ResponseModel<bool> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var bomPosEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data);
				var ubgEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(bomPosEntities.Select(x => x.Artikel_Nr_des_Bauteils ?? -1)?.ToList())
				?.Where(x => x.UBG == true && x.Warengruppe?.ToLower()?.Trim() == "ef")?.ToList();
				return ResponseModel<bool>.SuccessResponse(ubgEntities != null && ubgEntities.Count > 0);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<bool> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<bool>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
			{
				return ResponseModel<bool>.FailureResponse("Article not found");
			}

			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
