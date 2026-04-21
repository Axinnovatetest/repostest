using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension
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

				var articleExtensions = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Get(this._data);
				var PreisgruppenEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(articleExtensions.ArticleNr);


				if(articleExtensions.ArticleSalesType == Common.Enums.ArticleEnums.SalesItemType.Serie.GetDescription())
				{
					Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.DeleteByArticleNr((int)PreisgruppenEntity.Artikel_Nr);
					Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.DeleteArbeitskostenByArtikelNr(articleExtensions.ArticleNr);
				}

				#region >>> Logs & Notifications <<<
				// -
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleExtensions.ArticleNr);

				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
					ObjectLogHelper.getLog(this._user, articleEntity?.ArtikelNr ?? -1,
					$"Article Sales Price",
					$"{articleExtensions.Verkaufspreis}",
					$"{articleExtensions.Verkaufspreis}",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Delete));

				// - 
				if(articleEntity != null)
				{
					var title = $"[{articleEntity.ArtikelNummer}] Delete Sales";
					var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
						+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
						+ $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just deleted the sales price <strong>{articleExtensions.Verkaufspreis} | {articleExtensions.ArticleSalesType}</strong> from article <strong>{articleEntity.ArtikelNummer?.ToUpper()}</strong>."
						+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles/{articleEntity.ArtikelNr}/sales'>here</a>"
						+ "<br/><br/>Regards, <br/>IT Department </div>";

					var addresses = (Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetSales()
								?? new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>())
								.Select(x => x.UserEmail)?.ToList();
					Module.EmailingService.SendEmailAsync(title, content, addresses, null);
				}
				#endregion Logs & Notifications

				// - 2022-03-30
				CreateHandler.generateFileDAT(articleExtensions.ArticleNr);

				// -
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Delete(this._data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var articleExtensions = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Get(this._data);
			if(articleExtensions == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Sales item not found"}
					}
				};
			}
			if(articleExtensions != null)
			{
				var staffelPreiEntities = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.GetByArticleNr(articleExtensions.ArticleNr);
				if(articleExtensions.ArticleSalesType == Common.Enums.ArticleEnums.SalesItemType.Serie.GetDescription() && staffelPreiEntities != null)
				{
					return new ResponseModel<int>()
					{
						Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "2", Value = "Cannot delete Serie Sale Item if it have custom Price(s)"}
					}
					};
				}
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
