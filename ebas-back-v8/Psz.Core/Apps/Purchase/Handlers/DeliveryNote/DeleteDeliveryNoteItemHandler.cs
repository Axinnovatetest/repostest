using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class DeleteDeliveryNoteItemHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		private bool _logIt { get; set; }
		public DeleteDeliveryNoteItemHandler(Identity.Models.UserModel user, int data, bool logIt)
		{
			_user = user;
			_data = data;
			_logIt = logIt;
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

				var errors = new List<KeyValuePair<string, string>>();
				var LSItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data);
				var LSEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(LSItemEntity.AngebotNr ?? -1);
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(LSItemEntity.ArtikelNr ?? -1);

				var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { LSItemEntity.ArtikelNr ?? -1 });
				var lagerEntity = lagerEntities?.Find(x => x.Lagerort_id == LSItemEntity.Lagerort_id);
				if(lagerEntity != null)
				{
					lagerEntity.Bestand += LSItemEntity.Anzahl;
					Infrastructure.Data.Access.Tables.PRS.LagerAccess.Update(lagerEntity);
					Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtension(this._user,
					 new Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtensionModel
					 {
						 ArticleId = LSItemEntity.ArtikelNr ?? -1,
						 OldKundenIndex = LSItemEntity.Index_Kunde,
						 NewKundenIndex = LSItemEntity.Index_Kunde,
						 OldLagerorId = LSItemEntity.Lagerort_id ?? -1,
						 NewLagerorId = LSItemEntity.Lagerort_id ?? -1,
						 OldBestand = LSItemEntity.Anzahl ?? 0,
						 NewBestand = 0m,
					 });
				}
				if(LSItemEntity.LSPoszuABPos.HasValue && LSItemEntity.LSPoszuABPos.Value != 0 && LSItemEntity.LSPoszuABPos.Value != -1)
				{
					if(LSItemEntity != null && LSItemEntity?.erledigt_pos.Value == false)
					{ UpdateDeliveryNote(errors, LSEntity, LSItemEntity, articleEntity); }
				}
				if(_logIt)
				{
					//logging
					var _log = new LogHelper(LSEntity.Nr, (int)LSEntity.Angebot_Nr, int.TryParse(LSEntity.Projekt_Nr, out var v2) ? v2 : 0, LSEntity.Typ, LogHelper.LogType.DELETIONPOS, "CTS", _user)
						.LogCTS(null, null, null, (int)LSItemEntity.Position, LSItemEntity.Nr);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);
				}

				var response = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Delete(this._data);
				var LSItemsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(LSEntity.Nr);
				var LSItemsArticlesEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(LSItemsEntities.Select(l => (int)l.ArtikelNr).ToList() ?? new List<int> { -1 });
				generateDATFile(LSEntity, LSItemsEntities, LSItemsArticlesEntities);
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var angebotItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data);
			if(angebotItemEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"Position not found"}
					}
				};
			}
			//var technicArticles = Program.BSD.TechnicArticleIds;
			if(!Core.CustomerService.Helpers.HorizonsHelper.ArticleIsTechnic(angebotItemEntity.ArtikelNr ?? -1))
			{
				DateTime _newDate, _oldDate;
				_newDate = _oldDate = angebotItemEntity.Liefertermin ?? new DateTime(1900, 1, 1);
				// - 2023-11-03 - Reil - accept position in the past
				if(_newDate < DateTime.Today)
				{
					_newDate = DateTime.Today;
				}
				var horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasLSPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
				if(!horizonCheck)
					return new ResponseModel<int>()
					{
						Success = false,
						Errors = messages.Select(x => new ResponseModel<int>.ResponseError { Key = "", Value = x }).ToList()
					};
			}
			return ResponseModel<int>.SuccessResponse();
		}

		internal void UpdateDeliveryNote(
			List<KeyValuePair<string, string>> errors,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity deliveryNoteEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity angeboteneArtikelEntities,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntities)
		{
			lock(Locks.DeliveryNotesLock)
			{

				var originalABposEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(angeboteneArtikelEntities.LSPoszuABPos ?? -1);


				var angeboteneArtikelEntities_not_223 = (originalABposEntity?.ArtikelNr != 223) ? originalABposEntity : null;
				if(angeboteneArtikelEntities_not_223 != null)
				{
					if(!angeboteneArtikelEntities_not_223.Preiseinheit.HasValue || angeboteneArtikelEntities_not_223.Preiseinheit.Value == 0)
					{
						errors.Add(new KeyValuePair<string, string>("", $"{angeboteneArtikelEntities_not_223.Position}. Preiseinheit: invalid value {angeboteneArtikelEntities_not_223.Preiseinheit.Value}"));
					}
					angeboteneArtikelEntities_not_223.Anzahl = angeboteneArtikelEntities_not_223.Anzahl + angeboteneArtikelEntities.Anzahl;
					angeboteneArtikelEntities_not_223.Geliefert = angeboteneArtikelEntities_not_223.Geliefert - angeboteneArtikelEntities.Anzahl;
					angeboteneArtikelEntities_not_223.Gesamtpreis = (angeboteneArtikelEntities_not_223.Anzahl + angeboteneArtikelEntities.Anzahl) / angeboteneArtikelEntities_not_223.Preiseinheit * angeboteneArtikelEntities_not_223.Einzelpreis * (1 - angeboteneArtikelEntities_not_223.Rabatt);
					angeboteneArtikelEntities_not_223.erledigt_pos = angeboteneArtikelEntities_not_223.Anzahl + angeboteneArtikelEntities.Anzahl > 0 ? false : true;

					// 1.5
					angeboteneArtikelEntities_not_223.Einzelkupferzuschlag = Math.Round((decimal)(((angeboteneArtikelEntities_not_223.DEL * 1.01m) - angeboteneArtikelEntities_not_223.Kupferbasis)
																			  / 100
																			  * (decimal?)angeboteneArtikelEntities_not_223.EinzelCuGewicht), 2);

					// 1.6 
					angeboteneArtikelEntities_not_223.GesamtCuGewicht = angeboteneArtikelEntities_not_223.Anzahl * angeboteneArtikelEntities_not_223.EinzelCuGewicht;
					angeboteneArtikelEntities_not_223.Einzelpreis = angeboteneArtikelEntities_not_223.VKFestpreis.HasValue && angeboteneArtikelEntities_not_223.VKFestpreis.Value == true
						? angeboteneArtikelEntities_not_223.VKEinzelpreis
						: angeboteneArtikelEntities_not_223.Einzelkupferzuschlag * angeboteneArtikelEntities_not_223.Preiseinheit + angeboteneArtikelEntities_not_223.VKEinzelpreis;

					// 1.7
					angeboteneArtikelEntities_not_223.Gesamtpreis = angeboteneArtikelEntities_not_223.Einzelpreis / angeboteneArtikelEntities_not_223.Preiseinheit * angeboteneArtikelEntities_not_223.Anzahl * (1 - angeboteneArtikelEntities_not_223.Rabatt);
					angeboteneArtikelEntities_not_223.Gesamtkupferzuschlag = angeboteneArtikelEntities_not_223.VKFestpreis.HasValue && angeboteneArtikelEntities_not_223.VKFestpreis.Value == true
						? 0
						: angeboteneArtikelEntities_not_223.Anzahl * angeboteneArtikelEntities_not_223.Einzelkupferzuschlag;
					angeboteneArtikelEntities_not_223.VKGesamtpreis = angeboteneArtikelEntities_not_223.Anzahl * angeboteneArtikelEntities_not_223.VKEinzelpreis / angeboteneArtikelEntities_not_223.Preiseinheit;

					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(angeboteneArtikelEntities_not_223);
				}

			}
		}

		internal void generateDATFile(
Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity angeboteEntity,
List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> angeboteneArtikelEntities,
List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> artikelEntities)
		{
			var addressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(angeboteEntity.Kunden_Nr.Value);
			var WmsAngebotNr = angeboteEntity.Angebot_Nr;
			var path = $@"{Program.CTS.DeliveryNoteFilesPath}\WA{DateTime.Now.ToString("yyyyMMddhhmmss")}.dat";
			var content = $"AG;1;1;{WmsAngebotNr};{angeboteneArtikelEntities?.Count};50;{angeboteEntity.Datum.Value.ToString("yyyyMMdd")};{angeboteEntity.Versanddatum_Auswahl?.ToString("yyyyMMdd")};1;0;0;1;1;{addressenEntity.Kundennummer.Value};{angeboteEntity.Vorname_NameFirma.Substring(0, Math.Min(angeboteEntity.Vorname_NameFirma.Length, 37))}";
			foreach(var angeboteneArtikelEntity in angeboteneArtikelEntities)
			{
				var artikelEntity = artikelEntities.Where(x => x.ArtikelNr == angeboteneArtikelEntity.ArtikelNr)?.ToList().FirstOrDefault();

				if(artikelEntity != null && (artikelEntity.Warengruppe.ToUpper() == "EF" || artikelEntity.Warengruppe.ToUpper() == "ROH"))
				{
					content += $"\nAG;2;1;{WmsAngebotNr};{angeboteneArtikelEntity.Position};{artikelEntity.ArtikelNr};{angeboteneArtikelEntity.Anzahl}";
				}
			}

			File.WriteAllText(path, content);
		}
	}
}
