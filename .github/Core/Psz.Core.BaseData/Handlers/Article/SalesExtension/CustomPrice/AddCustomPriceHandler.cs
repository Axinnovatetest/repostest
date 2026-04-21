using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension.CustomPrice
{
	public class AddCustomPriceHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.SalesExtension.CustomPrice.CustomPriceModel _data { get; set; }
		public AddCustomPriceHandler(UserModel user, Models.Article.SalesExtension.CustomPrice.CustomPriceModel projectType)
		{
			_user = user;
			_data = projectType;
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

				var responseBody = -1;
				var priesgruppen = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrAndType(this._data.ArticleId, (int)this._data.PricingGroup);

				// - 2023-04-17 - update if already exists
				var staffelPrice = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.GetByArtikelNrAndType(this._data.ArticleId, this._data.Type?.Trim().Replace(" ", ""), false);
				if(staffelPrice != null)
				{
					var data = this._data.ToEntity_2();
					data.Nr_Staffel = staffelPrice.Nr_Staffel;
					responseBody = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.Update(data);
				}
				else
				{
					responseBody = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.Insert(this._data.ToEntity_2());
				}
				switch((Enums.ArticleEnums.CustomPriceType)this._data.TypeId)
				{
					case Enums.ArticleEnums.CustomPriceType.S2:
						priesgruppen.Staffelpreis2 = this._data.StandardPrice;
						priesgruppen.ME2 = this._data.Quantity;
						priesgruppen.Preisgruppe = 1/* this._data.PricingGroup*/;
						priesgruppen.Artikel_Nr = this._data.ArticleId;
						priesgruppen.PM2 = ((priesgruppen.Verkaufspreis - this._data.StandardPrice) * 100) / priesgruppen.Verkaufspreis;
						priesgruppen.ME1 = this._data.PrevPriceQuantity;// - update prev from Inf
						break;

					case Enums.ArticleEnums.CustomPriceType.S3:
						priesgruppen.Staffelpreis3 = this._data.StandardPrice;
						priesgruppen.ME3 = this._data.Quantity;
						priesgruppen.Preisgruppe = 1/* this._data.PricingGroup*/;
						priesgruppen.Artikel_Nr = this._data.ArticleId;
						priesgruppen.PM3 = ((priesgruppen.Verkaufspreis - this._data.StandardPrice) * 100) / priesgruppen.Verkaufspreis;
						priesgruppen.ME2 = this._data.PrevPriceQuantity; // - update prev from Inf
						break;

					case Enums.ArticleEnums.CustomPriceType.S4:
						priesgruppen.Staffelpreis4 = this._data.StandardPrice;
						priesgruppen.ME4 = this._data.Quantity;
						priesgruppen.Preisgruppe = 1/* this._data.PricingGroup*/;
						priesgruppen.Artikel_Nr = this._data.ArticleId;
						priesgruppen.PM4 = ((priesgruppen.Verkaufspreis - this._data.StandardPrice) * 100) / priesgruppen.Verkaufspreis;
						priesgruppen.ME3 = this._data.PrevPriceQuantity;// - update prev from Inf
						break;

					case Enums.ArticleEnums.CustomPriceType.S1:
					default:
						priesgruppen.Staffelpreis1 = this._data.StandardPrice;
						priesgruppen.ME1 = this._data.Quantity;
						priesgruppen.Preisgruppe = 1/* this._data.PricingGroup*/;
						priesgruppen.Artikel_Nr = this._data.ArticleId;
						priesgruppen.PM1 = ((priesgruppen.Verkaufspreis - this._data.StandardPrice) * 100) / priesgruppen.Verkaufspreis;
						break;
				}
				priesgruppen.letzte_Aktualisierung = DateTime.Now;
				Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.Update(priesgruppen);

				// -
				SetLastCustomPriceQuantityToInf(this._data.ArticleId);

				#region >>> Logs & Notifications <<<
				var staffelLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
				var newEntity = this._data.ToStaffelEntity_2(new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2(), this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, staffelLogs, Enums.ObjectLogEnums.LogType.Add);
				var newPrice = 0m;
				var priceType = 0;
				switch((Enums.ArticleEnums.CustomPriceType)this._data.TypeId)
				{
					case Enums.ArticleEnums.CustomPriceType.S2:
						{
							priceType = 2;
							newPrice = this._data.StandardPrice ?? 0;
						}
						break;
					case Enums.ArticleEnums.CustomPriceType.S3:
						{
							priceType = 3;
							newPrice = this._data.StandardPrice ?? 0;
						}
						break;

					case Enums.ArticleEnums.CustomPriceType.S4:
						{
							priceType = 4;
							newPrice = this._data.StandardPrice ?? 0;
						}
						break;
					case Enums.ArticleEnums.CustomPriceType.S1:
					default:
						{
							priceType = 1;
							newPrice = this._data.StandardPrice ?? 0;
						}
						break;
				}

				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(staffelLogs);
				if(priceType != 0)
				{
					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, articleEntity.ArtikelNr,
						$"Article Custom Sales Price {priceType}",
						$"{newPrice}",
						$"{newPrice}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Add));

					// - 
					var title = $"[{articleEntity.ArtikelNummer}] new Custom Price";
					var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
						+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
						+ $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just added a new custom sales price <strong>{this._data.Type}</strong> of <strong>{newPrice}</strong> for article <strong>{articleEntity.ArtikelNummer?.ToUpper()}</strong>."
						+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles/{articleEntity.ArtikelNr}/sales'>here</a>"
						+ "<br/><br/>Regards, <br/>IT Department </div>";

					var addresses = (Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetSales()
								?? new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>())
								.Select(x => x.UserEmail)?.ToList();

					Module.EmailingService.SendEmailAsync(title, content, addresses, null);
				}
				#endregion Logs & Notifications

				return ResponseModel<int>.SuccessResponse(responseBody);

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

			if(this._data.PricingGroup == null)
			{
				return ResponseModel<int>.FailureResponse("Price group should not be null!");
			}

			var priesgruppen = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrAndType(this._data.ArticleId, this._data.PricingGroup.Value);
			if(priesgruppen == null)
			{
				return ResponseModel<int>.FailureResponse("Price group not found!");
			}
			//if (priesgruppen == null)
			//{
			//    ResponseModel<int>.FailureResponse("You should add a sale item before adding a custom price !");
			//}

			if(this._data.StandardPrice == null || this._data.Quantity == null)
			{
				return ResponseModel<int>.FailureResponse("price and quantity should not be null !");
			}
			if(this._data.StandardPrice <= 0 || this._data.Quantity <= 0)
			{
				return ResponseModel<int>.FailureResponse("price and quantity should not be negative !");
			}
			if(this._data.StandardPrice > priesgruppen.Verkaufspreis)
			{
				return ResponseModel<int>.FailureResponse("Custom price should not be bigger then article standard price [" + priesgruppen.Verkaufspreis + "€]!");
			}
			// -  2025-06-11 - Betrag is required
			if(this._data.ProductionCost.HasValue != true || this._data.ProductionCost.Value<0)
			{
				return ResponseModel<int>.FailureResponse($"Production Cost: invalid value [{this._data.ProductionCost}]!");
			}

			var customPrices = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.GetByArticleNr(this._data.ArticleId);
			var PricesList = new List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>();

			if(customPrices != null)
			{
				if(customPrices.Count > 0)
				{
					if(customPrices.Count == 4)
					{
						return ResponseModel<int>.FailureResponse("You have reached the maximum custom prices for this article !");
					}

					foreach(var cp in customPrices)
					{
						PricesList.Add(new Models.Article.SalesExtension.CustomPrice.CustomPriceModel(cp, priesgruppen));
					}
				}


				var previous_price = PricesList.Find(a => a.TypeId == this._data.TypeId - 1);
				if(previous_price != null /*&& this._data.Quantity <= previous_price.Quantity*/)
				{
					// - Adding S3
					if(this._data.TypeId == (int)Enums.ArticleEnums.CustomPriceType.S3)
					{
						var s2 = PricesList.Find(x => x.TypeId == (int)Enums.ArticleEnums.CustomPriceType.S2);
						if(s2 == null)
						{
							return ResponseModel<int>.FailureResponse("Cannot add S3 without S2");
						}

						var s1 = PricesList.Find(x => x.TypeId == (int)Enums.ArticleEnums.CustomPriceType.S1);
						if(s1 == null)
						{
							return ResponseModel<int>.FailureResponse("Cannot add S3 without S1");
						}

						// - new S2 qty <= S1
						if(this._data.PrevPriceQuantity <= s1.Quantity)
						{
							return ResponseModel<int>.FailureResponse("previous quantity should be bigger then [" + s1.Type + "] quantity (" + s1.Quantity + ")!");
						}
					}

					// - Adding S4
					if(this._data.TypeId == (int)Enums.ArticleEnums.CustomPriceType.S4)
					{
						var s3 = PricesList.Find(x => x.TypeId == (int)Enums.ArticleEnums.CustomPriceType.S3);
						if(s3 == null)
						{
							return ResponseModel<int>.FailureResponse("Cannot add S4 without S3");
						}

						var s2 = PricesList.Find(x => x.TypeId == (int)Enums.ArticleEnums.CustomPriceType.S2);
						if(s2 == null)
						{
							return ResponseModel<int>.FailureResponse("Cannot add S4 without S2");
						}

						// - new S3 qty <= S2
						if(this._data.PrevPriceQuantity <= s2.Quantity)
						{
							return ResponseModel<int>.FailureResponse("previous quantity should be bigger then [" + s2.Type + "] quantity (" + s2.Quantity + ")!");
						}
					}

					// - return ResponseModel<int>.FailureResponse("quantity should be bigger then [" + previous_price.Type + "] quantity ("+previous_price.Quantity+")!");
				}

				if(previous_price != null && this._data.StandardPrice >= previous_price.StandardPrice)
				{
					return ResponseModel<int>.FailureResponse("price should be less then [" + previous_price.Type + "] price (" + previous_price.StandardPrice + "€)!");
				}
			}

			// - 2024-04-24  - check HourlyRate
			var articleExtension = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(this._data.ArticleId);
			var hourlyRate = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.GetByProdutionSiteId(articleExtension?.ProductionPlace1_Id ?? -1);
			if(hourlyRate?.Count > 0)
			{
				if(hourlyRate[0].HourlyRate != this._data.HourlyRate)
				{
					return ResponseModel<int>.FailureResponse($"Hourly Rate [{this._data.HourlyRate}] does not match production plant definition [{hourlyRate[0].HourlyRate}].");
				}
			}


			// -
			return ResponseModel<int>.SuccessResponse();
		}
		public static void SetLastCustomPriceQuantityToInf(int articleNr)
		{
			var entity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(articleNr);
			if(entity != null)
			{
				//if ((!entity.Staffelpreis2.HasValue || entity.Staffelpreis2 == 0) && (!entity.ME2.HasValue || entity.ME2.Value <= 0))
				//{
				//if (entity.Staffelpreis1.HasValue && entity.Staffelpreis1.Value > 0)
				//    entity.ME1 = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.PRICE_INFINITY;
				//} else 
				if((!entity.Staffelpreis3.HasValue || entity.Staffelpreis3 == 0) && (!entity.ME3.HasValue || entity.ME3.Value <= 0))
				{
					if(entity.Staffelpreis2.HasValue && entity.Staffelpreis2.Value > 0)
						entity.ME2 = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.PRICE_INFINITY;
				}
				else if((!entity.Staffelpreis4.HasValue || entity.Staffelpreis4 == 0) && (!entity.ME4.HasValue || entity.ME4.Value <= 0))
				{
					if(entity.Staffelpreis3.HasValue && entity.Staffelpreis3.Value > 0)
						entity.ME3 = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.PRICE_INFINITY;
				}
				else if(entity.Staffelpreis4.HasValue && entity.Staffelpreis4 != 0 && entity.ME4.HasValue)
				{
					entity.ME4 = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.PRICE_INFINITY;
				}

				// -
				Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.Update(entity);
			}
		}
	}
}
