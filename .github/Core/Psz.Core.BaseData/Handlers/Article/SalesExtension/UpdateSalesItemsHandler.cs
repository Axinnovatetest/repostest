using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateSalesItemsHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.SalesExtension.SalesItemModel _data { get; set; }
		public UpdateSalesItemsHandler(UserModel user, Models.Article.SalesExtension.SalesItemModel projectType)
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

				#region // -- transaction-based logic -- //

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// - logs
				var articleLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var salesLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var pricingGLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var arbeitskostenLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

				// 
				var salesItemEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Get(this._data.Id);
				var _salesItemEntity = this._data.ToEntity(salesItemEntity, this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, salesLogs);
				if(salesItemEntity.ArticleSalesType != _salesItemEntity.ArticleSalesType)
				{
					return ResponseModel<int>.FailureResponse($"Cannot change Type from {salesItemEntity.ArticleSalesType} to {_salesItemEntity.ArticleSalesType}");
				}

				if(salesItemEntity.ArticleSalesTypeId == (int)Common.Enums.ArticleEnums.SalesItemType.Serie)
				{
					var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
					var preisGruppenEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(this._data.ArticleId);
					var artikelKalkulEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetArbeitskostenByArtikelNr(this._data.ArticleId);

					if(artikelEntity != null)
					{
						// -
						var artikelData = this._data.ToArtikelEntity(artikelEntity, this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, articleLogs, Enums.ObjectLogEnums.LogType.Edit);
						artikelEntity.Produktionszeit = artikelData.Produktionszeit;
						artikelEntity.Stundensatz = artikelData.Stundensatz;
						artikelEntity.Verpackungsart = artikelData.Verpackungsart;
						artikelEntity.Verpackungsmenge = artikelData.Verpackungsmenge;
						artikelEntity.Losgroesse = artikelData.Losgroesse;

						// -
						Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.UpdateWithTransaction(artikelEntity, botransaction.connection, botransaction.transaction);
					}

					if(preisGruppenEntity != null)
					{
						preisGruppenEntity.Verkaufspreis = this._data.StandardPrice;
						preisGruppenEntity.Einkaufspreis = this._data.Einkaufspreis;
						preisGruppenEntity.Aufschlagsatz = this._data.Aufschlagsatz;
						preisGruppenEntity.Aufschlag = this._data.Aufschlag;
						preisGruppenEntity.brutto = this._data.brutto;
						preisGruppenEntity.Bemerkung = this._data.Bemerkung;
						preisGruppenEntity.kalk_kosten = (_salesItemEntity.Produktionskosten != null) ? ParseTextToDecimal(_salesItemEntity.Produktionskosten?.ToString()) : 0/* decimal.TryParse(_salesItemEntity.Produktionskosten, out var betrag) ? betrag : 0 : 0*/;
						preisGruppenEntity.letzte_Aktualisierung = DateTime.Now;
						Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.UpdateWithTransaction(preisGruppenEntity, botransaction.connection, botransaction.transaction);


						// - update Staffelpreis1 if  any
						var priesgruppen = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrAndType(this._data.ArticleId, (int)this._data.PricingGroup, botransaction.connection, botransaction.transaction);
						if(priesgruppen.Staffelpreis1.HasValue)
						{
							priesgruppen.Staffelpreis1 = this._data.StandardPrice;
							priesgruppen.PM1 = ((priesgruppen.Verkaufspreis - this._data.StandardPrice) * 100) / (priesgruppen.Verkaufspreis ?? 1);
							// - 2025-03-24 - Ticket #43502
							if(priesgruppen.Staffelpreis2.HasValue && priesgruppen.Staffelpreis2.Value > 0)
							{
								priesgruppen.PM2 = ((priesgruppen.Verkaufspreis - priesgruppen.Staffelpreis2.Value) * 100) / (priesgruppen.Verkaufspreis ?? 1);
							}
							if(priesgruppen.Staffelpreis3.HasValue && priesgruppen.Staffelpreis3.Value > 0)
							{
								priesgruppen.PM3 = ((priesgruppen.Verkaufspreis - priesgruppen.Staffelpreis3.Value) * 100) / (priesgruppen.Verkaufspreis ?? 1);
							}
							if(priesgruppen.Staffelpreis4.HasValue && priesgruppen.Staffelpreis4.Value > 0)
							{
								priesgruppen.PM4 = ((priesgruppen.Verkaufspreis - priesgruppen.Staffelpreis4.Value) * 100) / (priesgruppen.Verkaufspreis ?? 1);
							}
							priesgruppen.letzte_Aktualisierung = DateTime.Now;
							Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.UpdateWithTransaction(priesgruppen, botransaction.connection, botransaction.transaction);
						}
					}

					if(artikelKalkulEntity != null)
					{
						// -
						var arbeitskostenEntity = this._data.ToArbeitskostenEntity(artikelKalkulEntity, this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, arbeitskostenLogs);
						artikelKalkulEntity.Betrag = arbeitskostenEntity.Betrag;
						Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.Update(artikelKalkulEntity, botransaction.connection, botransaction.transaction);
					}
					else
					{
						Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.InsertArbeitskostenForArtikel(this._data.ToArbeitskostenEntity(new Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity(), this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, arbeitskostenLogs, Enums.ObjectLogEnums.LogType.Add), botransaction.connection, botransaction.transaction);
					}

				}

				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(articleLogs, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(salesLogs, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(pricingGLogs, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(arbeitskostenLogs, botransaction.connection, botransaction.transaction);

				var updatedId = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Update(_salesItemEntity, botransaction.connection, botransaction.transaction);


				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					#region >>> Logs & Notifications <<<
					if(salesItemEntity.Verkaufspreis != _salesItemEntity.Verkaufspreis)
					{
						var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(salesItemEntity.ArticleNr);

						// - 
						var title = $"[{articleEntity.ArtikelNummer}] Sales price Changes";
						var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
							+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
							+ $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just changed sales price <strong>{salesItemEntity.ArticleSalesType}</strong> from <strong>{salesItemEntity.Verkaufspreis}</strong> to <strong>{_salesItemEntity.Verkaufspreis}</strong> for article <strong>{articleEntity.ArtikelNummer?.ToUpper()}</strong>."
							+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles/{articleEntity.ArtikelNr}/sales'>here</a>"
							+ "<br/><br/>Regards, <br/>IT Department </div>";

						var addresses = (Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetSales()
									?? new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>())
									.Select(x => x.UserEmail)?.ToList();

						Module.EmailingService.SendEmailAsync(title, content, addresses, null);
					}
					#endregion Logs & Notifications

					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data.ArticleId);

					return ResponseModel<int>.SuccessResponse(updatedId);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
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
			if(Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Get(this._data.Id) == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Sales item not found"}
					}
				};
			}

			if(_data.StandardPrice == 0)
			{
				return ResponseModel<int>.FailureResponse("Invalid value: Verkaufspreis cannot be ZERO");
			}
			// - 
			if(this._data.Type?.Trim()?.ToLower() == "serie")
			{
				var priesgruppen = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrAndType(this._data.ArticleId, (int)this._data.PricingGroup);
				if(priesgruppen != null)
				{
					// -
					if(priesgruppen.Staffelpreis2 >= this._data.StandardPrice)
					{
						return ResponseModel<int>.FailureResponse($"Verkaufspreis [{this._data.StandardPrice?.ToString("0.0000")}] can not be smaller or equal than Staffel price 2 [{priesgruppen.Staffelpreis2?.ToString("0.0000")}].");
					}
					if(priesgruppen.Staffelpreis3 >= this._data.StandardPrice)
					{
						return ResponseModel<int>.FailureResponse($"Verkaufspreis [{this._data.StandardPrice?.ToString("0.0000")}] can not be smaller or equal than Staffel price 3 [{priesgruppen.Staffelpreis3?.ToString("0.0000")}].");
					}
					if(priesgruppen.Staffelpreis4 >= this._data.StandardPrice)
					{
						return ResponseModel<int>.FailureResponse($"Verkaufspreis [{this._data.StandardPrice?.ToString("0.0000")}] can not be smaller or equal than Staffel price 4 [{priesgruppen.Staffelpreis4?.ToString("0.0000")}].");
					}
				}
			}
			return ResponseModel<int>.SuccessResponse();
		}

		public static decimal ParseTextToDecimal(string decimalText)
		{
			if(decimalText == String.Empty)
				return 0;
			string temp = decimalText.Replace(',', '.');
			var decText = temp.Split('.');
			if(!Int32.TryParse(decText[0], out int integerPart))
				return 0;
			if(decText.Length == 1)
				return integerPart;
			if(decText.Length == 2)
			{
				if(!Int32.TryParse(decText[1], out int decimalPart))
					return 0;

				decimal powerOfTen = 10m;
				for(int i = 1; i < decText[1].Length; i++)
					powerOfTen *= 10;
				return integerPart + decimalPart / powerOfTen;
			}
			return 0; // there were two or more decimal separators, which is a clear invalid input
		}
	}
}
