using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class UpdateLSSingleItemByQteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data1 { get; set; }
		private int _data2 { get; set; }

		public UpdateLSSingleItemByQteHandler(Identity.Models.UserModel user, int id, int Qte)
		{
			_user = user;
			_data1 = id;
			_data2 = Qte;

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

				//lock (Locks.DeliveryNotesLock)
				//{
				var oldLSItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data1);
				var LSEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(oldLSItem?.AngebotNr ?? -1);

				//updating original Order position
				var LSItemOrderdQuantiy = oldLSItem.Anzahl;
				if(oldLSItem.LSPoszuABPos.HasValue && oldLSItem.LSPoszuABPos.Value != 0 && oldLSItem.LSPoszuABPos.Value != -1)
				{
					var orderItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get((int)oldLSItem.LSPoszuABPos);
					if(orderItem == null)
					{
						return new ResponseModel<int>()
						{
							Success = false,
							Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = " Original order position not found"}
					}
						};
					}

					var ActaulOrderRest = orderItem.Anzahl + oldLSItem.Anzahl;

					if(this._data2 > ActaulOrderRest)
					{
						return new ResponseModel<int>()
						{
							Success = false,
							Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = "requested quantity is bigger then Order rest quantity"}
					}
						};
					}
					var newOrderRestQuantity = ActaulOrderRest - this._data2;
					var newOrderSentQuantity = this._data2;

					orderItem.Anzahl = newOrderRestQuantity;
					orderItem.Geliefert = newOrderSentQuantity;

					var OrderItemToUpdateCalcul = (orderItem?.ArtikelNr != 223) ? orderItem : null;
					if(OrderItemToUpdateCalcul != null)
					{
						if(!OrderItemToUpdateCalcul.Preiseinheit.HasValue || OrderItemToUpdateCalcul.Preiseinheit.Value == 0)
						{
							return new ResponseModel<int>()
							{
								Success = false,
								Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"{OrderItemToUpdateCalcul.Position}. Preiseinheit: invalid value {OrderItemToUpdateCalcul.Preiseinheit.Value}"}
					}
							};
						}
						if(OrderItemToUpdateCalcul.erledigt_pos.HasValue && OrderItemToUpdateCalcul.erledigt_pos.Value)
							OrderItemToUpdateCalcul.erledigt_pos = false;
						OrderItemToUpdateCalcul.Gesamtpreis = (OrderItemToUpdateCalcul.Anzahl) / OrderItemToUpdateCalcul.Preiseinheit * OrderItemToUpdateCalcul.Einzelpreis * (1 - OrderItemToUpdateCalcul.Rabatt);
						OrderItemToUpdateCalcul.erledigt_pos = OrderItemToUpdateCalcul.Anzahl > 0 ? false : true;

						// 1.5
						OrderItemToUpdateCalcul.Einzelkupferzuschlag = Math.Round((decimal)(((OrderItemToUpdateCalcul.DEL * 1.01m) - OrderItemToUpdateCalcul.Kupferbasis)
																				  / 100
																				  * (decimal?)OrderItemToUpdateCalcul.EinzelCuGewicht), 2);

						// 1.6 
						OrderItemToUpdateCalcul.GesamtCuGewicht = OrderItemToUpdateCalcul.Anzahl * OrderItemToUpdateCalcul.EinzelCuGewicht;
						OrderItemToUpdateCalcul.Einzelpreis = OrderItemToUpdateCalcul.VKFestpreis.HasValue && OrderItemToUpdateCalcul.VKFestpreis.Value == true
							? OrderItemToUpdateCalcul.VKEinzelpreis
							: OrderItemToUpdateCalcul.Einzelkupferzuschlag * OrderItemToUpdateCalcul.Preiseinheit + OrderItemToUpdateCalcul.VKEinzelpreis;

						// 1.7
						OrderItemToUpdateCalcul.Gesamtpreis = OrderItemToUpdateCalcul.Einzelpreis / OrderItemToUpdateCalcul.Preiseinheit * OrderItemToUpdateCalcul.Anzahl * (1 - OrderItemToUpdateCalcul.Rabatt);
						OrderItemToUpdateCalcul.Gesamtkupferzuschlag = OrderItemToUpdateCalcul.VKFestpreis.HasValue && OrderItemToUpdateCalcul.VKFestpreis.Value == true
							? 0
							: OrderItemToUpdateCalcul.Anzahl * OrderItemToUpdateCalcul.Einzelkupferzuschlag;
						OrderItemToUpdateCalcul.VKGesamtpreis = OrderItemToUpdateCalcul.Anzahl * OrderItemToUpdateCalcul.VKEinzelpreis / OrderItemToUpdateCalcul.Preiseinheit;

						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(OrderItemToUpdateCalcul);
					}
				}
				//----------------------Update Calcul                
				oldLSItem.Anzahl = this._data2;
				oldLSItem.Geliefert = 0;
				if(!oldLSItem.LSPoszuABPos.HasValue || oldLSItem.LSPoszuABPos.Value == 0 || oldLSItem.LSPoszuABPos.Value == -1)
				{
					oldLSItem.OriginalAnzahl = this._data2;
				}

				var LSItemToUpdate = (oldLSItem?.ArtikelNr != 223) ? oldLSItem : null;
				if(LSItemToUpdate != null)
				{
					if(!LSItemToUpdate.Preiseinheit.HasValue || LSItemToUpdate.Preiseinheit.Value == 0)
					{
						return new ResponseModel<int>()
						{
							Success = false,
							Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"{LSItemToUpdate.Position}. Preiseinheit: invalid value {LSItemToUpdate.Preiseinheit.Value}"}
					}
						};
					}
					LSItemToUpdate.Gesamtpreis = (LSItemToUpdate.Anzahl) / LSItemToUpdate.Preiseinheit * LSItemToUpdate.Einzelpreis * (1 - LSItemToUpdate.Rabatt);

					LSItemToUpdate.erledigt_pos = false;
					// 1.5
					LSItemToUpdate.Einzelkupferzuschlag = Math.Round((decimal)(((LSItemToUpdate.DEL * 1.01m) - LSItemToUpdate.Kupferbasis)
																			  / 100
																			  * (decimal?)LSItemToUpdate.EinzelCuGewicht), 2);

					// 1.6 
					LSItemToUpdate.GesamtCuGewicht = LSItemToUpdate.Anzahl * LSItemToUpdate.EinzelCuGewicht;
					LSItemToUpdate.Einzelpreis = LSItemToUpdate.VKFestpreis.HasValue && LSItemToUpdate.VKFestpreis.Value == true
						? LSItemToUpdate.VKEinzelpreis
						: LSItemToUpdate.Einzelkupferzuschlag * LSItemToUpdate.Preiseinheit + LSItemToUpdate.VKEinzelpreis;

					// 1.7
					LSItemToUpdate.Gesamtpreis = LSItemToUpdate.Einzelpreis / LSItemToUpdate.Preiseinheit * LSItemToUpdate.Anzahl * (1 - LSItemToUpdate.Rabatt);
					LSItemToUpdate.Gesamtkupferzuschlag = LSItemToUpdate.VKFestpreis.HasValue && LSItemToUpdate.VKFestpreis.Value == true
						? 0
						: LSItemToUpdate.Anzahl * LSItemToUpdate.Einzelkupferzuschlag;
					LSItemToUpdate.VKGesamtpreis = LSItemToUpdate.OriginalAnzahl * LSItemToUpdate.VKEinzelpreis / LSItemToUpdate.Preiseinheit;

					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(LSItemToUpdate);
				}

				//updating delivery position
				var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(oldLSItem?.ArtikelNr ?? -1);

				//updating lager bestand
				var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { oldLSItem?.ArtikelNr ?? -1 });
				var lagerEntity = lagerEntities?.Find(x => x.Lagerort_id == oldLSItem.Lagerort_id);
				// decimal? _diff = 0m;
				if(LSItemOrderdQuantiy != this._data2)
				{
					lagerEntity.Bestand = lagerEntity.Bestand + LSItemOrderdQuantiy - this._data2;
					Infrastructure.Data.Access.Tables.PRS.LagerAccess.Update(lagerEntity);
					// - 2022-03-11 track KundenIndex for Lager Bestand
					Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtension(this._user,
						new Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtensionModel
						{
							ArticleId = oldLSItem?.ArtikelNr ?? -1,
							OldKundenIndex = oldLSItem?.Index_Kunde,
							NewKundenIndex = oldLSItem?.Index_Kunde,
							OldLagerorId = lagerEntity.Lagerort_id ?? -1,
							NewLagerorId = lagerEntity.Lagerort_id ?? -1,
							OldBestand = LSItemOrderdQuantiy ?? 0,
							NewBestand = this._data2,
						});
				}
				var LSItemsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(LSEntity.Nr);
				var LSItemsArtilesEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(LSItemsEntities?.Select(l => (int)l.ArtikelNr).ToList() ?? new List<int> { -1 });
				generateDATFile(LSEntity, LSItemsEntities, LSItemsArtilesEntities);
				// -
				return ResponseModel<int>.SuccessResponse(1);
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
			var LSItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data1);

			if(LSItem.AngebotNr <= 0)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"Project Number [{LSItem.AngebotNr}] invalid"}
					}
				};
			}

			var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(LSItem?.AngebotNr ?? -1);
			var addressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderEntity.Kunden_Nr.Value);
			if(!addressenEntity.Kundennummer.HasValue)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = "Order Confirmation does not have a customer number"}
					}
				};
			}
			if(orderEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = "Order Confirmation not found"}
					}
				};
			}

			var errors = new List<string> { };
			//if (orderEntity.Typ.ToLower() != "auftragsbestätigung")
			//{
			//    errors.Add($"Order: Type is not Auftragsbestätigung");
			//}
			if(orderEntity.Typ.ToLower() != "lieferschein")
			{
				errors.Add($"Order: Type is not Lieferschein");
			}
			//if (orderEntity.Gebucht == false)
			//{
			//    errors.Add($"Order: Gebucht is false");
			//}
			if(orderEntity.Erledigt == true)
			{
				errors.Add($"Order: Erledigt is true");
			}

			//--

			var angeboteArtikelEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(LSItem?.AngebotNr ?? -1)?
							.Where(x => !x.erledigt_pos.HasValue || x.erledigt_pos.HasValue && x.erledigt_pos.Value == false)?.ToList()
							?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();


			var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { LSItem?.ArtikelNr ?? -1 });
			var lagerEntity = lagerEntities?.Find(x => x.Lagerort_id == LSItem.Lagerort_id);
			var lagerExtensionEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager((int)LSItem.ArtikelNr, LSItem.Index_Kunde, LSItem.Lagerort_id ?? -1);
			var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(LSItem?.ArtikelNr ?? -1);
			var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(LSItem?.ArtikelNr ?? -1);


			if(lagerEntity.Bestand < _data2)
			{
				errors.Add($"Requested quantity [{_data2}] is bigger then quantity [{lagerEntity.Bestand}] in warehouse [{lagerEntity.Lagerort_id}].");
			}
			//if ((lagerExtensionEntity?.Bestand ?? 0) < _data2)
			//{
			//    errors.Add($"Requested quantity [{_data2}] is bigger then quantity [{lagerExtensionEntity.Bestand }] with Index [{lagerExtensionEntity.Index_Kunde}] in warehouse [{lagerEntity.Lagerort_id}].");
			//}
			//-
			if(itemPricingGroupDb == null)
			{
				errors.Add($"Article {artikelEntity.ArtikelNummer} has no verkaufspreis");
			}
			if(string.IsNullOrEmpty(artikelEntity.ArtikelNummer) || string.IsNullOrWhiteSpace(artikelEntity.ArtikelNummer))
			{
				errors.Add($"Article must not be empty");
			}
			if(!LSItem.Lagerort_id.HasValue)
			{
				errors.Add($"Storage must not be empty");
			}
			if(!LSItem.Anzahl.HasValue)
			{
				errors.Add($"Order quantity must not be empty");
			}
			if(!LSItem.Preiseinheit.HasValue || (LSItem.Preiseinheit.HasValue && LSItem.Preiseinheit.Value < 0))
			{
				errors.Add($"Article {artikelEntity.ArtikelNummer}: unit price not valid");
			}
			if(this._data2 <= 0)
			{
				errors.Add($"Article {artikelEntity.ArtikelNummer}: invalid quantity");
			}


			if(errors.Count > 0)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = errors.Select(x => new ResponseModel<int>.ResponseError { Key = "", Value = x }).Distinct().ToList()
				};
			}

			return ResponseModel<int>.SuccessResponse();
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
