using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Data.SqlClient;
	using System.Linq;

	public class UpgradeFABomHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.BillOfMaterial.UpgradeFABomRequestModel _data { get; set; }
		public UpgradeFABomHandler(UserModel user, Models.Article.BillOfMaterial.UpgradeFABomRequestModel data)
		{
			_user = user;
			_data = data;
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
				//// -- Article level Logging
				//Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
				//    ObjectLogHelper.getLog(this._user, this._data.ArticleId,
				//    "Article BOM Status from",
				//    $"{Enums.ArticleEnums.BomStatus.InPreparation.GetDescription()}",
				//    $"{Enums.ArticleEnums.BomStatus.Approved.GetDescription()}",
				//    Enums.ObjectLogEnums.Objects.Article.GetDescription(),
				//    Enums.ObjectLogEnums.LogType.Edit));

				upgradeFABOM(botransaction.connection, botransaction.transaction, this._user, this._data.ArticleId, this._data.FaIds, this._data.BomVerion);

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data.ArticleId);
					// -
					return ResponseModel<int>.SuccessResponse();
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

			if(Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticleAndBomVersion(this._data.ArticleId, this._data.BomVerion) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Article BOM Version not found");

			if(!Infrastructure.Data.Access.Tables.PRS.FertigungAccess.ConfirmBOMValidationUpgradable(this._data.BomVerion, this._data.FaIds))
				return ResponseModel<int>.FailureResponse(key: "1", value: "Cannot find all FA");

			return ResponseModel<int>.SuccessResponse();
		}

		public static int upgradeFABOM(SqlConnection connection, SqlTransaction transaction, UserModel user, int articleId, List<int> faIds, int bomVerion, bool isFirstValidation = false)
		{
			var faEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetWithTransaction(faIds, connection, transaction);
			var stucklistenEntities = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetByArticleAndVersion(articleId, isFirstValidation ? (int?)null : bomVerion, connection, transaction);
			var abPositionEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(faEntities.Select(x => x.Angebot_Artikel_Nr ?? -1)?.ToList(), connection, transaction);
			var abEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(abPositionEntities.Select(x => x.AngebotNr ?? -1)?.ToList(), connection, transaction);
			foreach(var faItem in faEntities)
			{
				// - Replace FA OLD Positions by the new
				Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.DeleteByIdFertigung(faItem.ID, connection, transaction);
				Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.InsertWithTransaction(
					stucklistenEntities?.Select(x =>
					new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
					{
						ID_Fertigung_HL = faItem.ID,
						ID_Fertigung = faItem.ID,
						Artikel_Nr = x.Artikel_Nr_des_Bauteils,
						Anzahl = faItem.Originalanzahl * x.Anzahl,
						Lagerort_ID = faItem.Lagerort_id,
						Buchen = true,
						Vorgang_Nr = x.Vorgang_Nr,
						ME_gebucht = false,
						//Arbeitsanweisung = ""
						//Bemerkungen="",
						//Fertiger="",
						//Fertigstellung_Ist = DateTime.Now,
						//Löschen = false,
						//Termin_Soll = DateTime.Now
					})?.ToList(), connection, transaction);

				// - 2022-06-14 - if, FA from AB, then upgrade AB pos - ArtilceId
				if(faItem.Angebot_Artikel_Nr.HasValue && faItem.Angebot_Artikel_Nr.Value > 0)
				{
					var abPos = abPositionEntities.FirstOrDefault(x => x.Nr == faItem.Angebot_Artikel_Nr);
					if(abPos != null)
					{
						if(abPos.Anzahl == abPos.OriginalAnzahl)
						{
							// - 2023-01-27
							var ab = abEntities.FirstOrDefault(x => x.Nr == abPos.AngebotNr);
							Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
							{
								AngebotNr = ab.Angebot_Nr ?? -1,
								Nr = abPos.AngebotNr ?? -1,
								DateTime = DateTime.Now,
								LogObject = ab.Typ,
								LogText = $"AB updated [Pos:{abPos.Position} - Article || IndexKunde || IndexKundedate] to [{articleId} || {faItem.KundenIndex} || {faItem.Kunden_Index_Datum}]",
								LogType = "MODIFICATIONOBJECT",
								Origin = "CTS",
								ProjektNr = int.TryParse(ab.Projekt_Nr, out var v) ? v : 0,
								UserId = user.Id,
								Username = user.Name
							}, connection, transaction);

							abPos.ArtikelNr = articleId;
							abPos.Index_Kunde = faItem.KundenIndex;
							abPos.Index_Kunde_Datum = faItem.Kunden_Index_Datum;
							Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(abPos, connection, transaction);
						}
						else
						{ // - has been partially delivered
							Infrastructure.Services.Logging.Logger.LogWarning($"BOM Validate: cannot upgrade AB Position [{abPos.Nr}] for FA [{faItem.Fertigungsnummer}].");
						}
					}
				}
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(articleId, connection, transaction);

			// -- logging
			Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
				faEntities.Select(x =>
				Psz.Core.BaseData.Handlers.ObjectLogHelper.getLog(user, x.ID, $"[FA:{x.Fertigungsnummer}] Article || BOM || CP || IndexKunde",
				$"{x.Artikel_Nr} || {x.BomVersion} || NULL || {x.KundenIndex}",
				$"{articleId} || {bomVerion} || {articleEntity?.Index_Kunde}",
				"Fertigung",
				Enums.ObjectLogEnums.LogType.Edit)
				)?.ToList(), connection, transaction);
			// -

			return Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpgradeBOM(bomVerion, articleEntity?.Index_Kunde, articleEntity?.Index_Kunde_Datum, articleId, faIds, connection, transaction);
		}
	}
}
