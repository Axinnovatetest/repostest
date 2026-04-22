using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class CancelCommandHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Production.CancelCommandModel _data { get; set; }

		public CancelCommandHandler(Identity.Models.UserModel user, Models.Production.CancelCommandModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			lock(Locks.ProductionLock)
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

					var fertigungEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Id);
					var angeboteArtikel = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(fertigungEntity.Angebot_Artikel_Nr != null ? fertigungEntity.Angebot_Artikel_Nr.Value : -1);

					////
					Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.DeleteByIdFertigung(fertigungEntity.ID);
					//
					fertigungEntity.Kennzeichen = "STORNO";
					fertigungEntity.Bemerkung = $"STORNO am {DateTime.Now}, GRUND: [{this._data.Notes}]  / {fertigungEntity.Bemerkung}";  // <<<< Stornogrund
					fertigungEntity.Angebot_nr = 0;
					fertigungEntity.Angebot_Artikel_Nr = 0;
					fertigungEntity.Anzahl = 0;
					Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateAfterCancel(fertigungEntity, botransaction.connection, botransaction.transaction);
					//
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateFertigungsnummer(angeboteArtikel.Nr, 0, botransaction.connection, botransaction.transaction);
					#endregion // -- transaction-based logic -- //


					//TODO: handle transaction state (success or failure)
					if(botransaction.commit())
					{
						return ResponseModel<int>.SuccessResponse(fertigungEntity.ID);
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
		}

		public ResponseModel<int> Validate()
		{
			try
			{
				if(this._user == null/*
                || this._user.Access.____*/)
				{
					return ResponseModel<int>.AccessDeniedResponse();
				}

				var errors = new List<ResponseModel<int>.ResponseError>();

				var fertigungEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Id);
				if(fertigungEntity == null)
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Production command not found" });
					return new ResponseModel<int>() { Errors = errors, Success = false };
				}
				var lagers = Program.BSD.ProductionLagerIds;// new List<int> { 7, 60, 42, 6, 26 };
				if(lagers?.Contains((int)fertigungEntity.Lagerort_id) == true && fertigungEntity?.FA_Gestartet != null && fertigungEntity?.FA_Gestartet == true)
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Production command is 'Gestartet'" });
				}
				if(fertigungEntity.Anzahl_erledigt != 0)
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "FA has closed quantity" });
					return new ResponseModel<int>() { Errors = errors, Success = false };
				}


				var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(fertigungEntity.Artikel_Nr != null ? fertigungEntity.Artikel_Nr.Value : -1);
				if(artikelEntity == null)
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Article not found" });
					return new ResponseModel<int>() { Errors = errors, Success = false };
				}
				if(!string.IsNullOrEmpty(artikelEntity?.Freigabestatus) && artikelEntity?.Freigabestatus?.ToUpper() == "O")
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Article is 'Obsolete'" });
				}

				var angeboteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(fertigungEntity.Angebot_nr != null ? fertigungEntity.Angebot_nr.Value : -1);
				if(angeboteEntity == null)
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Order not found" });
					return new ResponseModel<int>() { Errors = errors, Success = false };
				}
				if(angeboteEntity?.Typ == null || angeboteEntity?.Typ.ToLower() != Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Confirmation).ToLower()) // "Auftragsbestätigung"
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Order not 'Auftragsbestätigung'" });
				}
				if(angeboteEntity?.Nr == 0)
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Order 'Nr' is zero." });
				}

				var angeboteneArtikel = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(fertigungEntity.Angebot_Artikel_Nr != null ? fertigungEntity.Angebot_Artikel_Nr.Value : -1);
				if(angeboteneArtikel == null)
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Item not found" });
					return new ResponseModel<int>() { Errors = errors, Success = false };
				}
				if(angeboteneArtikel?.Fertigungsnummer == null || angeboteneArtikel?.Fertigungsnummer.Value == 0)
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Item 'Fertigungsnummer' is zero." });
				}

				// - 2022-05-25 - allow cancelFA when AB has delivered items
				//if (angeboteneArtikel.Geliefert.HasValue && angeboteneArtikel.Geliefert.Value>0)
				//{
				//    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Item has delivred quantity cancellation impossible ." });
				//}

				// - 2023-02-03
				var frZone = DateTime.Today.AddDays(Program.CTS.FAHorizons.H1LengthInDays); // 2024-01-25 - Khelil change H1 to 41 days
				if(this._user?.Access?.CustomerService?.FAWerkWunshAdmin != true && this._user.Access?.CustomerService?.FaAdmin != true && fertigungEntity.Lagerort_id != 6 && fertigungEntity.Technik != true && !Program.BSD.TechnicArticleIds.Exists(x => x == fertigungEntity.Artikel_Nr))
				{
					var _newDate = fertigungEntity.Termin_Bestatigt1 ?? new DateTime(1900, 1, 1);
					if(_newDate < DateTime.Today)
					{
						errors.Add(new ResponseModel<int>.ResponseError($"Production date invalid: can not cancel FA [{_newDate.ToString("dd/MM/yyyy")}] in the past."));
					}

					if(_newDate <= frZone)
					{
						errors.Add(new ResponseModel<int>.ResponseError($"Production date invalid: can not cancel FA in Frozen Zone limit [{frZone.ToString("dd/MM/yyyy")}]."));
					}
				}

				// >>>
				if(errors.Count > 0)
				{
					return new ResponseModel<int>() { Errors = errors, Success = false };
				}

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, e.StackTrace);
				throw;
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
