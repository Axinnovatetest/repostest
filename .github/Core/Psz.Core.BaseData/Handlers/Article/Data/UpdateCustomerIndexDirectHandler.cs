using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateCustomerIndexDirectHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.UpdateCustomerIndexRequestModel _data { get; set; }

		public UpdateCustomerIndexDirectHandler(Identity.Models.UserModel user, Models.Article.UpdateCustomerIndexRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{

				#region // -- transaction-based logic -- //

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();

				var originalArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.OriginalArticleId);
				var oldIndex = originalArticleEntity.Index_Kunde;
				var sameOldCustomerIndex = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemIndex(originalArticleEntity.CustomerNumber ?? -1, originalArticleEntity.ArtikelNummer?.Substring(0, originalArticleEntity.ArtikelNummer?.IndexOf('-')??0), originalArticleEntity.CustomerItemNumber, originalArticleEntity.CustomerIndex);

				#region Logs                
				// - save KundenIndex History
				Infrastructure.Data.Access.Tables.BSD.PSZ_ArtikelhistorieAccess.InsertWithTransaction(sameOldCustomerIndex.Select(x =>
					new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity
					{
						Artikel_Nr = x.ArtikelNr,
						Datum_Anderung = DateTime.Now,
						ID = -1, // not needed coz AutoIncrement
						Anderungsbereich = $"Index Kunden changed from [{x.Index_Kunde}] to [{this._data.NewCustomerIndex}]",
						Anderungsbeschreibung = this._data.Reason,
						Anderung_von = this._user.Username
					}).ToList(), botransaction.connection, botransaction.transaction);
				// - logs
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					sameOldCustomerIndex.Select(x =>
					ObjectLogHelper.getLog(this._user, x.ArtikelNr, "Kundenindex", oldIndex, this._data.NewCustomerIndex, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit)
					).ToList(),
					botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					sameOldCustomerIndex.Select(x =>
					ObjectLogHelper.getLog(this._user, x.ArtikelNr, "Kundenindexdatum", originalArticleEntity.Index_Kunde_Datum?.ToString("dd/MM/yyyy"), this._data.NewCustomerIndexDate?.ToString("dd/MM/yyyy"), Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit)
					).ToList(),
					botransaction.connection, botransaction.transaction);
				#endregion logs

				// -
				for(var i = 0; i < sameOldCustomerIndex.Count; i++)
				{
					sameOldCustomerIndex[i].CustomerIndex = this._data.NewCustomerIndex;
					sameOldCustomerIndex[i].Index_Kunde = this._data.NewCustomerIndex;
					sameOldCustomerIndex[i].Index_Kunde_Datum = this._data.NewCustomerIndexDate;
				}

				// - save update data
				var insertedNr = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.UpdateCustomerIndexWithTransaction(
					sameOldCustomerIndex, botransaction.connection, botransaction.transaction);

				var openFAs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetNotStartedOpenByArticle(originalArticleEntity.ArtikelNr, null, botransaction.connection, botransaction.transaction);
				if(openFAs != null && openFAs.Count > 0)
				{
					var oldFAPositions = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByIdFertigung(openFAs.Select(x => x.ID).ToList(), botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.DeleteByIdFertigung(openFAs.Select(x => x.ID).ToList(), botransaction.connection, botransaction.transaction);
					var newPositions = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
					var articleBOM = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticleWithTransaction(originalArticleEntity.ArtikelNr, botransaction.connection, botransaction.transaction)
						?? new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
					foreach(var faItem in openFAs)
					{
						foreach(var bomItem in articleBOM)
						{
							var oldPosition = oldFAPositions?.FirstOrDefault(x => x.ID_Fertigung == faItem.ID && x.Artikel_Nr == bomItem.Artikel_Nr_des_Bauteils);
							newPositions.Add(new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
							{
								ID_Fertigung_HL = faItem.ID,
								ID_Fertigung = faItem.ID,
								Artikel_Nr = bomItem.Artikel_Nr_des_Bauteils,
								Anzahl = faItem.Anzahl * bomItem.Anzahl,
								Lagerort_ID = faItem.Lagerort_id,
								Buchen = true,
								Vorgang_Nr = bomItem.Vorgang_Nr,
								ME_gebucht = false,
								Löschen = false,
								IsUBG = oldPosition?.IsUBG,
								UBGFertigungsId = oldPosition?.UBGFertigungsId,
								UBGFertigungsnummer = oldPosition?.UBGFertigungsnummer,
							});
						}
					}
					Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.InsertWithTransaction(newPositions, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateArticleIndexWithTransaction(originalArticleEntity.ArtikelNr, originalArticleEntity.Index_Kunde, originalArticleEntity.Index_Kunde_Datum, botransaction.connection, botransaction.transaction);

					// - 2024-03-11
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(
						 openFAs.Select(x => new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
						 {
							 AngebotNr = 0,
							 DateTime = DateTime.Now,
							 Id = 1,
							 LogObject = "Fertigung",
							 LogText = $"[Fertigung] [{x.Fertigungsnummer}] Modified-[Kundenindex] changed from [{x.KundenIndex}] to [{this._data.NewCustomerIndex}]",
							 LogType = $"Edit",
							 Nr = x.ID,
							 Origin = "MTD",
							 ProjektNr = 0,
							 UserId = this._user.Id,
							 Username = this._user.Username
						 }).ToList());
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(
							 openFAs.Select(x => new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
							 {
								 AngebotNr = 0,
								 DateTime = DateTime.Now,
								 Id = 1,
								 LogObject = "Fertigung",
								 LogText = $"[Fertigung] [{x.Fertigungsnummer}] Modified-[Kundenindexdatum] changed from [{x.Kunden_Index_Datum?.ToString("dd/MM/yyyy")}] to [{this._data.NewCustomerIndexDate?.ToString("dd/MM/yyyy")}]",
								 LogType = $"Edit",
								 Nr = x.ID,
								 Origin = "MTD",
								 ProjektNr = 0,
								 UserId = this._user.Id,
								 Username = this._user.Username
							 }).ToList());
				}

				#endregion transaction-based logic

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - 2022-03-30
					CreateHandler.generateFileDAT(insertedNr, isNew: false);


					// -
					return ResponseModel<int>.SuccessResponse(insertedNr);
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

			#region all Articles
			var originalArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.OriginalArticleId);
			if(originalArticleEntity == null)
			{
				return ResponseModel<int>.FailureResponse($"Article not found.");
			}
			var validatedBOM = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetByArticleAndKundenIndex(this._data.OriginalArticleId, originalArticleEntity.Index_Kunde);
			var bomStatus = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.OriginalArticleId);
			if(validatedBOM?.Count > 0 || bomStatus?.BomStatusId == (int)Enums.ArticleEnums.BomStatus.Approved)
			{
				return ResponseModel<int>.FailureResponse($"Article has validated BOM. Index change not allowed.");
			}
			if(string.IsNullOrWhiteSpace(this._data.NewCustomerIndex))
			{
				return ResponseModel<int>.FailureResponse($"Customer Index [{this._data.NewCustomerIndex}] invalid.");
			}
			if(this._data.NewCustomerIndex == originalArticleEntity.CustomerIndex)
			{
				return ResponseModel<int>.FailureResponse($"Customer Index [{this._data.NewCustomerIndex}] should be different from old [{originalArticleEntity.CustomerIndex}].");
			}
			if(string.IsNullOrWhiteSpace(this._data.Reason))
				return ResponseModel<int>.FailureResponse("4", "[Kunden Index] changed, please add a reason.");
			#endregion - all Articles

			// - Customer ItemNumber
			var sameNewCustomerIndex = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemIndex(originalArticleEntity.CustomerNumber ?? -1, originalArticleEntity.ArtikelNummer?.Substring(0, originalArticleEntity.ArtikelNummer?.IndexOf('-') ?? 0), originalArticleEntity.CustomerItemNumber, this._data.NewCustomerIndex);
			if(sameNewCustomerIndex != null && sameNewCustomerIndex.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Customer Index [{this._data.NewCustomerIndex}] already exists [{string.Join(",", sameNewCustomerIndex.Take(5).Select(x => $"{x.ArtikelNummer}"))}]");
			}
			var sameOldCustomerIndex = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemIndex(originalArticleEntity.CustomerNumber ?? -1, originalArticleEntity.ArtikelNummer?.Substring(0, originalArticleEntity.ArtikelNummer?.IndexOf('-') ?? 0), originalArticleEntity.CustomerItemNumber, originalArticleEntity.CustomerIndex);
			var doneFAs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetDoneByArticleAndIndex(sameOldCustomerIndex.Select(x => x.ArtikelNr).ToList(), originalArticleEntity.Index_Kunde, null);
			if(doneFAs != null && doneFAs.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Customer Index [{originalArticleEntity.CustomerIndex}] has already erledigt FAs [{string.Join(",", doneFAs.Take(5).Select(x => $"{x.Fertigungsnummer}"))}]");
			}
			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
