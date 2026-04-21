using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class UpdateOverviewHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.ArticleOverviewMinimalModel _data { get; set; }
		public UpdateOverviewHandler(Identity.Models.UserModel user, Models.Article.ArticleOverviewMinimalModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				var logs = LogChanges();
				var articleData = this._data.ToEntity();
				var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(_data.ArtikelNr, botransaction.connection, botransaction.transaction);
				if(artikelEntity?.Warengruppe?.ToLower()?.Trim() != "roh")
				{
					articleData.Manufacturer = artikelEntity?.Manufacturer;
					articleData.ManufacturerNumber = artikelEntity?.ManufacturerNumber;
				}
				var artikelUpdated = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditOverview(articleData, botransaction.connection, botransaction.transaction);
				// - 
				var articleExtensionEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(this._data.ArtikelNr);
				if(articleExtensionEntity != null)
				{
					var extension = this._data.ToExtensionEntity();
					extension.Id = articleExtensionEntity.Id;
					Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.EditOverview(extension, botransaction.connection, botransaction.transaction);
				}
				else
				{
					Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.InsertWithTransaction(this._data.ToExtensionEntity(), botransaction.connection, botransaction.transaction);
				}

				if(artikelUpdated > 0)
				{
					// save update logs
					if(logs.Count > 0)
					{
						Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
					}
				}
				#endregion // -- transaction-based logic -- //


				//handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data.ArtikelNr);

					return ResponseModel<int>.SuccessResponse(artikelUpdated);
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
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(string.IsNullOrWhiteSpace(this._data.Bezeichnung1))
				return ResponseModel<int>.FailureResponse("[Bezeichnung1] should not be empty");

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr);
			if(articleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Article not found"}
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

			// - 2023-06-02 - CustomerItemNumber has its own field, so no checks needed here
			//var artikelSameBezeichnung1 = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByKundeBezeichnung(articleEntity.ArtikelNummer.Trim(), this._data.Bezeichnung1.Trim());
			//// - 2022-12-05 - accept articles from same customer
			//if(!string.IsNullOrWhiteSpace(articleEntity.CustomerItemNumber))
			//{
			//	// - keep articles with diff CustomerItemNumber (CIN)
			//	artikelSameBezeichnung1 = artikelSameBezeichnung1.Where(x => x.CustomerItemNumber?.ToLower() != articleEntity.CustomerItemNumber.ToLower())?.ToList();
			//	if(artikelSameBezeichnung1 != null && artikelSameBezeichnung1.Count > 0)
			//	{
			//		var idx = new List<int>();
			//		var oparts = articleEntity.ArtikelNummer.Split('-');
			//		if(oparts != null && oparts.Count() > 2)
			//		{
			//			foreach(var item in artikelSameBezeichnung1)
			//			{
			//				var parts = item.ArtikelNummer.Split('-');
			//				if(parts != null && parts.Count() > 2)
			//				{
			//					if(parts[0] == oparts[0] && parts[1] == oparts[1])
			//					{
			//						// - same customer parts
			//						idx.Add(item.ArtikelNr);
			//					}
			//				}
			//			}
			//		}

			//		// - remove article with same CIN
			//		if(idx.Count > 0)
			//		{
			//			artikelSameBezeichnung1.RemoveAll(x => idx.Contains(x.ArtikelNr) == true);
			//		}
			//	}
			//}

			//if(artikelSameBezeichnung1 != null && artikelSameBezeichnung1.Count > 0 && artikelSameBezeichnung1.Exists(x => x.ArtikelNr != this._data.ArtikelNr) == true)
			//	return ResponseModel<int>.FailureResponse($"[Herstellernummer/Manufacturer Number] exists in [{string.Join(", ", artikelSameBezeichnung1.Take(5).Select(x => x.ArtikelNummer).ToList())}{(artikelSameBezeichnung1.Count > 5 ? " ..." : "")}]");

			return ResponseModel<int>.SuccessResponse();
		}

		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr);
			//var managersEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetByArtikelNr(this._data.ArtikelNr);
			//var articleExtension = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(this._data.ArtikelNr);
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Edit;

			//if (articleEntity.ArtikelNummer != this._data.ArtikelNummer)
			//{
			//    logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr,  "ArtikelNummer", articleEntity.ArtikelNummer, this._data.ArtikelNummer, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			//}

			if(articleEntity.Bezeichnung1 != this._data.Bezeichnung1)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Bezeichnung1", articleEntity.Bezeichnung1, this._data.Bezeichnung1, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			if(articleEntity.Bezeichnung2 != this._data.Bezeichnung2)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Bezeichnung2", articleEntity.Bezeichnung2, this._data.Bezeichnung2, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Bezeichnung3 != this._data.Bezeichnung3)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Bezeichnung3", articleEntity.Bezeichnung3, this._data.Bezeichnung3, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			// - 2024-04-11
			if(articleEntity.Verpackung != this._data.Verpackung)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Verpackung", articleEntity.Verpackung, this._data.Verpackung, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Artikelbezeichnung != this._data.Artikelbezeichnung)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Artikelbezeichnung", articleEntity.Artikelbezeichnung, this._data.Artikelbezeichnung, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.OrderNumber != this._data.OrderNumber)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "OrderNumber", articleEntity.OrderNumber, this._data.OrderNumber, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Consumption12Months != this._data.Consumption12Months)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Consumption12Months", articleEntity.Consumption12Months, this._data.Consumption12Months, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Losgroesse != this._data.Losgroesse)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Losgroesse", $"{articleEntity.Losgroesse}", $"{this._data.Losgroesse}", Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Produktionlosgrosse != this._data.Produktionlosgrosse)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Produktionlosgrosse", $"{articleEntity.Produktionlosgrosse}", $"{this._data.Produktionlosgrosse}", Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Langtext != this._data.Langtext)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Langtext", articleEntity.Langtext, this._data.Langtext, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Lieferzeit != this._data.Lieferzeit)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Lieferzeit", articleEntity.Lieferzeit, this._data.Lieferzeit, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Manufacturer != this._data.Manufacturer)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Manufacturer", articleEntity.Manufacturer, this._data.Manufacturer, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.ManufacturerNumber != this._data.ManufacturerNumber)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "ManufacturerNumber", articleEntity.ManufacturerNumber, this._data.ManufacturerNumber, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			//
			return logs;
		}
	}

}
