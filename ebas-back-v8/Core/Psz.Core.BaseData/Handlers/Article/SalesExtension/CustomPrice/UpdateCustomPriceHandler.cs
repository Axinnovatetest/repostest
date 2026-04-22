using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension.CustomPrice
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.IO;
	using System.Linq;

	public class UpdateCustomPriceHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.SalesExtension.CustomPrice.CustomPriceModel _data { get; set; }
		public UpdateCustomPriceHandler(UserModel user, Models.Article.SalesExtension.CustomPrice.CustomPriceModel projectType)
		{
			_user = user;
			_data = projectType;
		}
		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				lock((Locks.ArticleEditLock.GetOrAdd(this._data.ArticleId, new object())))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					var staffelLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

					var responseBody = -1;
					var priesgruppen = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrAndType(this._data.ArticleId, this._data.PricingGroup ?? 1);
					var oldKonditionzuordnungEntity = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.Get(this._data.CustomPriceId);

					responseBody = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.Update(this._data.ToStaffelEntity_2(oldKonditionzuordnungEntity, this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, staffelLogs, Enums.ObjectLogEnums.LogType.Add), botransaction.connection, botransaction.transaction);
					var presigruppenLogs = getPreisgruppenLogs(priesgruppen);
					switch((Enums.ArticleEnums.CustomPriceType)this._data.TypeId)
					{
						case Enums.ArticleEnums.CustomPriceType.S2:
							priesgruppen.Staffelpreis2 = this._data.StandardPrice;
							priesgruppen.ME2 = this._data.Quantity;
							priesgruppen.Preisgruppe = 1/* this._data.PricingGroup*/;
							priesgruppen.Artikel_Nr = this._data.ArticleId;
							priesgruppen.PM2 = ((priesgruppen.Verkaufspreis - this._data.StandardPrice) * 100) / priesgruppen.Verkaufspreis;
							//priesgruppen.ME1 = this._data.PrevPriceQuantity; // -
							break;
						case Enums.ArticleEnums.CustomPriceType.S3:

							priesgruppen.Staffelpreis3 = this._data.StandardPrice;
							priesgruppen.ME3 = this._data.Quantity;
							priesgruppen.Preisgruppe = 1/* this._data.PricingGroup*/;
							priesgruppen.Artikel_Nr = this._data.ArticleId;
							priesgruppen.PM3 = ((priesgruppen.Verkaufspreis - this._data.StandardPrice) * 100) / priesgruppen.Verkaufspreis;
							//priesgruppen.ME2 = this._data.PrevPriceQuantity; // -
							break;

						case Enums.ArticleEnums.CustomPriceType.S4:
							priesgruppen.Staffelpreis4 = this._data.StandardPrice;
							priesgruppen.ME4 = this._data.Quantity;
							priesgruppen.Preisgruppe = 1/* this._data.PricingGroup*/;
							priesgruppen.Artikel_Nr = this._data.ArticleId;
							priesgruppen.PM4 = ((priesgruppen.Verkaufspreis - this._data.StandardPrice) * 100) / priesgruppen.Verkaufspreis;
							//priesgruppen.ME3 = this._data.PrevPriceQuantity; // -
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
					Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.UpdateWithTransaction(priesgruppen, botransaction.connection, botransaction.transaction);

					// -
					AddCustomPriceHandler.SetLastCustomPriceQuantityToInf(this._data.ArticleId, botransaction);

					#region >>> Logs & Notifications <<<
					var oldEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrAndType(this._data.ArticleId, this._data.PricingGroup ?? 1);
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
					var oldPrice = 0m;
					var newPrice = 0m;
					var priceType = 0;
					switch((Enums.ArticleEnums.CustomPriceType)this._data.TypeId)
					{
						case Enums.ArticleEnums.CustomPriceType.S2:
							if(oldEntity.Staffelpreis2 != this._data.StandardPrice)
							{
								priceType = 2;
								oldPrice = oldEntity.Staffelpreis2 ?? 0;
								newPrice = this._data.StandardPrice ?? 0;
							}
							break;
						case Enums.ArticleEnums.CustomPriceType.S3:
							if(oldEntity.Staffelpreis3 != this._data.StandardPrice)
							{
								priceType = 3;
								oldPrice = oldEntity.Staffelpreis3 ?? 0;
								newPrice = this._data.StandardPrice ?? 0;
							}
							break;
						case Enums.ArticleEnums.CustomPriceType.S4:
							if(oldEntity.Staffelpreis4 != this._data.StandardPrice)
							{
								priceType = 4;
								oldPrice = oldEntity.Staffelpreis4 ?? 0;
								newPrice = this._data.StandardPrice ?? 0;
							}
							break;
						case Enums.ArticleEnums.CustomPriceType.S1:
						default:
							if(oldEntity.Staffelpreis1 != this._data.StandardPrice)
							{
								priceType = 1;
								oldPrice = oldEntity.Staffelpreis1 ?? 0;
								newPrice = this._data.StandardPrice ?? 0;
							}
							break;
					}

					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(staffelLogs, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(presigruppenLogs, botransaction.connection, botransaction.transaction);
					if(oldPrice != newPrice)
					{
						Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
							ObjectLogHelper.getLog(this._user, articleEntity.ArtikelNr,
							$"Article Custom Sales Price {priceType}",
							$"{oldPrice}",
							$"{newPrice}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit), botransaction.connection, botransaction.transaction);
					}
					#endregion Logs & Notifications
					//TODO: handle transaction state (success or failure)
					if(botransaction.commit())
					{
						if(priceType != 0)
						{

							// - 
							var title = $"[{articleEntity.ArtikelNummer}] Custom Price Changes";
							var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
								+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
								+ $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just changed custom sales price <strong>{this._data.Type}</strong> from <strong>{oldPrice}</strong> to <strong>{newPrice}</strong> for article <strong>{articleEntity.ArtikelNummer?.ToUpper()}</strong>."
								+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles/{articleEntity.ArtikelNr}/sales'>here</a>"
								+ "<br/><br/>Regards, <br/>IT Department </div>";

							var addresses = (Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetSales()
										?? new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>())
										.Select(x => x.UserEmail)?.ToList();

							Module.EmailingService.SendEmailAsync(title, content, addresses, null);
						}
						return ResponseModel<int>.SuccessResponse(responseBody);
					}
					else
					{
						return ResponseModel<int>.FailureResponse("Transaction error");
					}
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
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
			if(articleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Article no found"}
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
			if(this._data.PricingGroup == null)
			{
				return ResponseModel<int>.FailureResponse("Price Group should not be null");
			}
			var customerPrice = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.GetByArtikelNrAndType(this._data.ArticleId,
					((Enums.ArticleEnums.CustomPriceType)this._data.TypeId).GetDescription(), false);
			if(customerPrice == null)
			{
				return ResponseModel<int>.FailureResponse("Custom Price not found");
			}
			var customPrices = Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.GetByArticleNr(this._data.ArticleId, false);
			var priesgruppen = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrAndType(this._data.ArticleId, this._data.PricingGroup.Value);
			if(priesgruppen == null)
			{
				return ResponseModel<int>.FailureResponse("Price group not found!");
			}
			if(this._data.TypeId < 1 || this._data.TypeId > 4)
			{
				return ResponseModel<int>.FailureResponse("Invalid price type selected");
			}
			if(this._data.StandardPrice == null || this._data.Quantity == null)
			{
				return ResponseModel<int>.FailureResponse("price and quantity must not be null !");
			}
			if(this._data.StandardPrice <= 0 || this._data.Quantity <= 0)
			{
				return ResponseModel<int>.FailureResponse("price and quantity must not be negative !");
			}
			// -  2025-06-11 - Betrag is required
			if(this._data.ProductionTime.HasValue != true || this._data.ProductionTime.Value < 0)
			{
				return ResponseModel<int>.FailureResponse($"Production Time: invalid value [{this._data.ProductionTime}]!");
			}
			this._data.ProductionCost = this._data.HourlyRate * this._data.ProductionTime / 60;
			var PricesList = new List<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>();
			if(customPrices != null && customPrices.Count > 0)
			{
				foreach(var cp in customPrices)
				{
					PricesList.Add(new Models.Article.SalesExtension.CustomPrice.CustomPriceModel(cp, priesgruppen));
				}
			}
			if(customPrices != null)
			{
				var previous_price = PricesList.FirstOrDefault(a => a.TypeId == this._data.TypeId - 1);
				var next_price = PricesList.FirstOrDefault(a => a.TypeId == this._data.TypeId + 1);
				if(previous_price != null)
				{
					if(this._data.Quantity <= previous_price.Quantity)
					{
						return ResponseModel<int>.FailureResponse("quantity should be bigger then [" + previous_price.Type + "] quantity (" + previous_price.Quantity + ")!");
					}
					if(this._data.StandardPrice >= previous_price.StandardPrice)
					{
						return ResponseModel<int>.FailureResponse("price should be less then [" + previous_price.Type + "] price (" + previous_price.StandardPrice + "€)!");
					}
				}
				if(next_price != null)
				{
					if(this._data.Quantity >= next_price.Quantity)
					{
						return ResponseModel<int>.FailureResponse("quantity should be less then [" + next_price.Type + "] quantity (" + next_price.Quantity + ")!");
					}
					if(this._data.StandardPrice <= next_price.StandardPrice)
					{
						return ResponseModel<int>.FailureResponse("price should be Bigger then [" + next_price.Type + "] price (" + next_price.StandardPrice + "€)!");
					}
				}
			}
			if(this._data.StandardPrice > priesgruppen.Verkaufspreis)
			{
				return ResponseModel<int>.FailureResponse("Custom price should not be bigger then article standard price [" + priesgruppen.Verkaufspreis + "€]!");
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

			return ResponseModel<int>.SuccessResponse();
		}
		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> getPreisgruppenLogs(Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity priesgruppen)
		{
			if(priesgruppen == null)
				return null;


			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var pm = ((priesgruppen.Verkaufspreis - this._data.StandardPrice) * 100) / priesgruppen.Verkaufspreis;
			switch((Enums.ArticleEnums.CustomPriceType)this._data.TypeId)
			{
				case Enums.ArticleEnums.CustomPriceType.S2:
					{
						// - logs
						if(priesgruppen.Staffelpreis2 != this._data.StandardPrice)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"Staffelpreis2",
									$"{priesgruppen.Staffelpreis2}",
									$"{this._data.StandardPrice}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.ME2 != this._data.Quantity)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"ME2",
									$"{priesgruppen.ME2}",
									$"{this._data.Quantity}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.Preisgruppe != 1)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"Preisgruppe",
									$"{priesgruppen.Preisgruppe}",
									$"{1}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.PM2 != pm)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"PM2",
									$"{priesgruppen.PM2}",
									$"{pm}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
					}
					break;
				case Enums.ArticleEnums.CustomPriceType.S3:
					{
						if(priesgruppen.Staffelpreis3 != this._data.StandardPrice)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"Staffelpreis3",
									$"{priesgruppen.Staffelpreis3}",
									$"{this._data.StandardPrice}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.ME3 != this._data.Quantity)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"ME3",
									$"{priesgruppen.ME3}",
									$"{this._data.Quantity}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.Preisgruppe != 1)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"Preisgruppe",
									$"{priesgruppen.Preisgruppe}",
									$"{1}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.PM3 != pm)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"PM3",
									$"{priesgruppen.PM3}",
									$"{pm}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
					}
					break;
				case Enums.ArticleEnums.CustomPriceType.S4:
					{
						if(priesgruppen.Staffelpreis4 != this._data.StandardPrice)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"Staffelpreis4",
									$"{priesgruppen.Staffelpreis4}",
									$"{this._data.StandardPrice}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.ME4 != this._data.Quantity)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"ME4",
									$"{priesgruppen.ME4}",
									$"{this._data.Quantity}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.Preisgruppe != 1)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"Preisgruppe",
									$"{priesgruppen.Preisgruppe}",
									$"{1}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.PM4 != pm)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"PM4",
									$"{priesgruppen.PM4}",
									$"{pm}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
					}
					break;
				case Enums.ArticleEnums.CustomPriceType.S1:
				default:
					{
						if(priesgruppen.Staffelpreis1 != this._data.StandardPrice)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"Staffelpreis1",
									$"{priesgruppen.Staffelpreis1}",
									$"{this._data.StandardPrice}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.ME1 != this._data.Quantity)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"ME1",
									$"{priesgruppen.ME1}",
									$"{this._data.Quantity}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.Preisgruppe != 1)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"Preisgruppe",
									$"{priesgruppen.Preisgruppe}",
									$"{1}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
						if(priesgruppen.PM1 != pm)
						{
							logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleId,
									$"PM1",
									$"{priesgruppen.PM1}",
									$"{pm}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit));
						}
					}
					break;
			}

			return logs;
		}
	}
}
