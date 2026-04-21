using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension.CustomPrice
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

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//// 
				var customPrice = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.Get(this._data);
				var priesgruppen = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr((int)customPrice.Artikel_Nr);
				var response = ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.Delete(this._data, botransaction.connection, botransaction.transaction));
				// -
				var newPrice = 0m;
				var priceType = 0;

				// - Fix Price Type in case price added from P3000
				if(!customPrice.TypeId.HasValue)
				{
					if(customPrice.Staffelpreis_Typ.Trim().Replace(" ", "").ToLower() == "s1")
					{
						customPrice.TypeId = (int)Enums.ArticleEnums.CustomPriceType.S1;
						customPrice.Type = Enums.ArticleEnums.CustomPriceType.S1.GetDescription();
					}
					else if(customPrice.Staffelpreis_Typ.Trim().Replace(" ", "").ToLower() == "s2")
					{
						customPrice.TypeId = (int)Enums.ArticleEnums.CustomPriceType.S2;
						customPrice.Type = Enums.ArticleEnums.CustomPriceType.S2.GetDescription();
					}
					else if(customPrice.Staffelpreis_Typ.Trim().Replace(" ", "").ToLower() == "s3")
					{
						customPrice.TypeId = (int)Enums.ArticleEnums.CustomPriceType.S3;
						customPrice.Type = Enums.ArticleEnums.CustomPriceType.S3.GetDescription();
					}
					else if(customPrice.Staffelpreis_Typ.Trim().Replace(" ", "").ToLower() == "s4")
					{
						customPrice.TypeId = (int)Enums.ArticleEnums.CustomPriceType.S4;
						customPrice.Type = Enums.ArticleEnums.CustomPriceType.S4.GetDescription();
					}
				}


				switch((Enums.ArticleEnums.CustomPriceType)customPrice.TypeId)
				{
					case Enums.ArticleEnums.CustomPriceType.S2:
						priesgruppen.Staffelpreis2 = null;
						priesgruppen.ME2 = null;
						priesgruppen.PM2 = null;
						// -
						newPrice = priesgruppen.Staffelpreis2 ?? 0;
						priceType = 2;
						break;
					case Enums.ArticleEnums.CustomPriceType.S3:

						priesgruppen.Staffelpreis3 = null;
						priesgruppen.ME3 = null;
						priesgruppen.PM3 = null;
						// -
						newPrice = priesgruppen.Staffelpreis3 ?? 0;
						priceType = 3;
						break;

					case Enums.ArticleEnums.CustomPriceType.S4:
						priesgruppen.Staffelpreis4 = null;
						priesgruppen.ME4 = null;
						priesgruppen.PM4 = null;
						// -
						newPrice = priesgruppen.Staffelpreis4 ?? 0;
						priceType = 4;

						break;
					case Enums.ArticleEnums.CustomPriceType.S1:
					default:
						priesgruppen.Staffelpreis1 = null;
						priesgruppen.ME1 = null;
						priesgruppen.PM1 = null;
						// -
						newPrice = priesgruppen.Staffelpreis1 ?? 0;
						priceType = 1;
						break;
				}
				Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.UpdateWithTransaction(priesgruppen, botransaction.connection, botransaction.transaction);

				// -
				AddCustomPriceHandler.SetLastCustomPriceQuantityToInf((int)customPrice.Artikel_Nr, botransaction);

				#region >>> Logs & Notifications <<<
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(priesgruppen.Artikel_Nr ?? -1);

				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					ObjectLogHelper.getLog(this._user, articleEntity.ArtikelNr,
					$"Article Custom Sales Price {priceType}",
					$"{newPrice}",
					$"{newPrice}",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Delete), botransaction.connection, botransaction.transaction);
				#endregion Logs & Notifications


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					if(priceType != 0)
					{
						// - 
						var title = $"[{articleEntity.ArtikelNummer}] delete Custom Price";
						var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
							+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
							+ $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just deleted the custom sales price <strong>{newPrice}</strong> of <strong>{customPrice.Type}</strong> from article <strong>{articleEntity.ArtikelNummer?.ToUpper()}</strong>."
							+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles/{articleEntity.ArtikelNr}/sales'>here</a>"
							+ "<br/><br/>Regards, <br/>IT Department </div>";

						var addresses = (Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetSales()
									?? new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>())
									.Select(x => x.UserEmail)?.ToList();

						Module.EmailingService.SendEmailAsync(title, content, addresses, null);
					}

					return response;
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			var customPrice = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.Get(this._data);
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			if(customPrice == null)
			{
				return ResponseModel<int>.FailureResponse("Custom price not found !");
			}

			var allCustomPrices = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.GetByArticleNr((int)customPrice.Artikel_Nr);
			var typesNames = new List<string>();
			if(allCustomPrices != null && allCustomPrices.Count > 0)
			{
				foreach(var item in allCustomPrices)
				{
					if(item.TypeId.HasValue && item.TypeId.Value != customPrice.TypeId)
					{
						if(item.TypeId.Value > customPrice.TypeId && item.Betrag.HasValue)
						{
							typesNames.Add(((Enums.ArticleEnums.CustomPriceType)item.TypeId).GetDescription());
						}
					}
					else
					{
						if(string.Compare(customPrice.Staffelpreis_Typ.Trim().ToLower(), item.Staffelpreis_Typ.Trim().ToLower()) < 0 && item.Betrag.HasValue)
						{
							typesNames.Add(item.Staffelpreis_Typ);
						}
					}
				}

				if(typesNames.Count > 0)
				{
					return ResponseModel<int>.FailureResponse("You have to delete the custum prices [" + String.Join(",", typesNames) + "] before !");
				}
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
