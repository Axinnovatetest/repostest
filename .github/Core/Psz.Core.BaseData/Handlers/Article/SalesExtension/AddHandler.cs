using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.SalesExtension.SalesItemModel _data { get; set; }
		public AddHandler(UserModel user, Models.Article.SalesExtension.SalesItemModel projectType)
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

				var articleLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var salesLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var pricingGLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var arbeitskostenLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

				var _salesItemEntity = this._data.ToEntity(new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity(), this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, salesLogs, Enums.ObjectLogEnums.LogType.Add);
				if(_salesItemEntity == null)
					return ResponseModel<int>.SuccessResponse();

				if(this._data.TypeId == (int)Common.Enums.ArticleEnums.SalesItemType.Serie)
				{
					// -
					var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
					var preisGruppenEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(this._data.ArticleId);
					var artikelKalkulEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetArbeitskostenByArtikelNr(this._data.ArticleId);

					if(artikelEntity != null)
					{
						//artikelEntity.Produktionszeit = _salesItemEntity.Profuktionszeit;
						//artikelEntity.Stundensatz = _salesItemEntity.Stundensatz;
						//artikelEntity.Verpackungsart = _salesItemEntity.Verpackungsart;
						//artikelEntity.Verpackungsmenge = (int?)_salesItemEntity.Verpackungsmenge;
						//artikelEntity.Losgroesse = _salesItemEntity.Losgroesse;

						// -
						var artikelData = this._data.ToArtikelEntity(artikelEntity, this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, articleLogs, Enums.ObjectLogEnums.LogType.Edit);
						artikelEntity.Produktionszeit = artikelData.Produktionszeit;
						artikelEntity.Stundensatz = artikelData.Stundensatz;
						artikelEntity.Verpackungsart = artikelData.Verpackungsart;
						artikelEntity.Verpackungsmenge = artikelData.Verpackungsmenge;
						artikelEntity.Losgroesse = artikelData.Losgroesse;
						Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Edit(artikelEntity);
					}

					if(preisGruppenEntity != null)
					{
						//preisGruppenEntity.Verkaufspreis = this._data.StandardPrice;
						//preisGruppenEntity.Einkaufspreis = this._data.Einkaufspreis;
						//preisGruppenEntity.Aufschlagsatz = this._data.Aufschlagsatz;
						//preisGruppenEntity.Aufschlag = this._data.Aufschlag;
						//preisGruppenEntity.brutto = this._data.brutto;
						//preisGruppenEntity.Bemerkung = this._data.Bemerkung;
						//preisGruppenEntity.kalk_kosten = (_salesItemEntity.Produktionskosten != null) ? UpdateSalesItemsHandler.ParseTextToDecimal(_salesItemEntity.Produktionskosten?.ToString()) : 0/* decimal.TryParse(_salesItemEntity.Produktionskosten, out var betrag) ? betrag : 0 : 0*/;
						//preisGruppenEntity.letzte_Aktualisierung = DateTime.Now;

						// -
						var preisGruppenData = this._data.ToPreisgruppenEntity(new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity(), this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, pricingGLogs);
						preisGruppenEntity.Verkaufspreis = preisGruppenData.Verkaufspreis;
						preisGruppenEntity.Einkaufspreis = preisGruppenData.Einkaufspreis;
						preisGruppenEntity.Aufschlagsatz = preisGruppenData.Aufschlagsatz;
						preisGruppenEntity.Aufschlag = preisGruppenData.Aufschlag;
						preisGruppenEntity.brutto = preisGruppenData.brutto;
						preisGruppenEntity.Bemerkung = preisGruppenData.Bemerkung;
						preisGruppenEntity.kalk_kosten = preisGruppenData.kalk_kosten;
						preisGruppenEntity.letzte_Aktualisierung = DateTime.Now;
						Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.Update(preisGruppenEntity);
					}
					else
					{
						Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.Insert(this._data.ToPreisgruppenEntity(new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity(), this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, pricingGLogs, Enums.ObjectLogEnums.LogType.Add));
					}

					if(artikelKalkulEntity != null)
					{
						//artikelKalkulEntity.Betrag = /*decimal.Parse(_salesItemEntity.Produktionskosten)*/(_salesItemEntity.Produktionskosten != null) ? UpdateSalesItemsHandler.ParseTextToDecimal(_salesItemEntity.Produktionskosten?.ToString()) : 0/*decimal.TryParse(_salesItemEntity.Produktionskosten.Replace(",", "."), out var betrag) ? betrag : 0:0*/;

						// -
						var arbeitskostenEntity = this._data.ToArbeitskostenEntity(artikelKalkulEntity, this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, arbeitskostenLogs);
						artikelKalkulEntity.Betrag = arbeitskostenEntity.Betrag;
						Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.Update(artikelKalkulEntity);
					}
					else
					{
						Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.InsertArbeitskostenForArtikel(this._data.ToArbeitskostenEntity(new Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity(), this._user, Enums.ObjectLogEnums.Objects.Article, this._data.ArticleId, arbeitskostenLogs, Enums.ObjectLogEnums.LogType.Add));
					}
				}

				#region >>> Logs & Notifications <<<
				// -
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_salesItemEntity.ArticleNr);

				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(articleLogs);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(salesLogs);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(pricingGLogs);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(arbeitskostenLogs);
				//Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
				//    ObjectLogHelper.getLog(this._user, articleEntity.ArtikelNr,
				//    $"Article Sales Price",
				//    $"{_salesItemEntity.Verkaufspreis}",
				//    $"{_salesItemEntity.Verkaufspreis}",
				//    Enums.ObjectLogEnums.Objects.Article.GetDescription(),
				//    Enums.ObjectLogEnums.LogType.Add));

				// - 
				var title = $"[{articleEntity.ArtikelNummer}] New Purchase";
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
					+ $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just added a new sales price of <strong>{_salesItemEntity.Verkaufspreis} | {_salesItemEntity.ArticleSalesType}</strong> for article <strong>{articleEntity.ArtikelNummer?.ToUpper()}</strong>."
					+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles/{articleEntity.ArtikelNr}/sales'>here</a>"
					+ "<br/><br/>Regards, <br/>IT Department </div>";

				var addresses = (Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetSales()
							?? new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>())
							.Select(x => x.UserEmail)?.ToList();
				Module.EmailingService.SendEmailAsync(title, content, addresses, null);
				#endregion Logs & Notifications

				this._data.PricingGroup = 1;

				// - 2022-03-30
				CreateHandler.generateFileDAT(this._data.ArticleId);

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Insert(_salesItemEntity));
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

			if(!this._data.Einkaufspreis.HasValue || this._data.Einkaufspreis.Value <= 0)
				return ResponseModel<int>.FailureResponse("Purchase Price invalid.");

			if(!this._data.StandardPrice.HasValue || this._data.StandardPrice.Value <= 0)
				return ResponseModel<int>.FailureResponse("Sales Price invalid.");

			if(this._data.DeliveryTimeInWorkingDays.HasValue && this._data.DeliveryTimeInWorkingDays.Value <= 0)
				return ResponseModel<int>.FailureResponse("Delivery Time is invalid.");

			var PreisgruppenEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(this._data.ArticleId);
			var articleItems = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndTypeId(this._data.ArticleId, (int)this._data.TypeId);
			if(this._data.Type == Common.Enums.ArticleEnums.SalesItemType.Serie.GetDescription())
			{
				if(PreisgruppenEntity != null)
				{
					return ResponseModel<int>.FailureResponse($"Article cannot have multiple {this._data.Type}");
				}
			}
			else
			{
				if(articleItems != null)
				{
					return ResponseModel<int>.FailureResponse($"Article cannot have multiple {this._data.Type}");
				}
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
