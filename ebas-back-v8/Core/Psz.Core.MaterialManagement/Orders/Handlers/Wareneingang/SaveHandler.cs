using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.Wareneingang;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Wareneingang
{
	public class SaveHandler: IHandle<SaveRequestModel, ResponseModel<SaveResponseModel>>
	{
		private SaveRequestModel data { get; set; }
		private UserModel user { get; set; }
		public SaveHandler(UserModel user, SaveRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<SaveResponseModel> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private ResponseModel<SaveResponseModel> Perform()
		{


			lock(Locks.Locks.CreateWareneingangLock)
			{
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);

				try
				{

					botransaction.beginTransaction();

					/// Update Artikel Gross 
					// Get All Artikels From Artikel table.

					var artikels = Infrastructure.Data.Access.Tables.MTM.ArtikelAccess.GetWithTransaction(data.UpdatedArticle.Select(x => x.ArtikelNr).ToList(), botransaction.connection, botransaction.transaction);

					// update All Artikel Gross value
					foreach(var item in artikels)
					{
						var gross = data.UpdatedArticle.Find(x => x.ArtikelNr == item.Artikel_Nr).Grosse;

						if(gross != item.Grosse)
						{
							item.Grosse = data.UpdatedArticle.Find(x => x.ArtikelNr == item.Artikel_Nr).Grosse;
							Infrastructure.Data.Access.Tables.MTM.ArtikelAccess.UpdateWithTransaction(item, botransaction.connection, botransaction.transaction);
							var arikelGross = Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.GetArtikelWareingangGewicht(item.Artikel_Nr, botransaction.connection, botransaction.transaction);
							Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.UpdateArtikelGrossWithTransaction(arikelGross, botransaction.connection, botransaction.transaction);
						}
					}

					///Save Wareneingang to Database
					/// 
					var currentMaxWareingangNr = Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.GetMaxWareingangNrByMandant(data.Nr, botransaction.connection, botransaction.transaction);
					var insertedId = Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.InsertWareneingangWithTransaction(data.Nr, string.IsNullOrWhiteSpace(user.LegacyUserName) ? user.Username : user.LegacyUserName, currentMaxWareingangNr + 1, data.LsNummer, botransaction.connection, botransaction.transaction);
					var wareneingangDb = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetWithTransactionByBestellung_Nr(currentMaxWareingangNr + 1, botransaction.connection, botransaction.transaction);


					/// Get Current bestellungen bestellteArtikels 
					/// 
					var bestellteArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetWithTransaction(data.UpdatedArticle.Select(x => x.bestelleteArtikelNr).ToList(), botransaction.connection, botransaction.transaction);


					/// Save New bestellteArtikel for the new wareneingang
					/// 
					List<Infrastructure.Data.Entities.Joins.MTM.Order.BestellteArtikelWareneingangEntity> listBestellteArtikel = new List<Infrastructure.Data.Entities.Joins.MTM.Order.BestellteArtikelWareneingangEntity>();
					var pos = 10;
					data.UpdatedArticle.ForEach(x =>
					{
						var sumQty = data.UpdatedArticle.Where(y => y.bestelleteArtikelNr == x.bestelleteArtikelNr).Select(y => y.AktuelleAnzahl).Sum();
						listBestellteArtikel.Add(new Infrastructure.Data.Entities.Joins.MTM.Order.BestellteArtikelWareneingangEntity()
						{
							ArtikelNr = x.ArtikelNr,
							AktuelleAnzahl = x.AktuelleAnzahl,
							CurrentBestellteArtikelNr = x.bestelleteArtikelNr,
							Lagerort_id = x.Lagerort,
							MhdDatumArtikel = string.IsNullOrWhiteSpace(x.MHDDatumString) ? null : DateTime.ParseExact(x.MHDDatumString, "MM/dd/yyyy", null),
							NewWareneingangNr = wareneingangDb.Nr,
							StartAnzahl = 0,
							//Change erledigt_pos to true if all quantities are received else to false 
							//?
							erledigt_pos = bestellteArtikel.FirstOrDefault(item => item.Nr == x.bestelleteArtikelNr)?.Anzahl <= sumQty ? true : false,
							Position = pos,
						});
						pos = pos + 10;
					}
					);

					Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.InsertBestellteArtikelWareneingangWithTransaction(listBestellteArtikel, botransaction.connection, botransaction.transaction);

					/// Update bestellteArtikel with new data
					/// TODO :: 
					Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.UpdateCurrentBestellungWithTransaction(listBestellteArtikel, user.LegacyUserName, botransaction.connection, botransaction.transaction);

					/// Update Lager with new quantity.
					/// 
					Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.UpdateLagerortQuantitiesWithTransaction(listBestellteArtikel, botransaction.connection, botransaction.transaction);
					var bestellungDb = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetWithTransaction(data.Nr, botransaction.connection, botransaction.transaction);
					var _log = new LogHelper(
							bestellungDb.Nr,
							bestellungDb.Bestellung_Nr ?? -1,
							int.TryParse(bestellungDb.Projekt_Nr, out var val) ? val : 0,
							$"{bestellungDb.Typ}",
							LogHelper.LogType.CREATIONWE,
							"MTM",
							user)
								.LogMTM(bestellungDb.Nr);
					Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

					_log = new LogHelper(
							wareneingangDb.Nr,
							wareneingangDb.Bestellung_Nr ?? -1,
							int.TryParse(wareneingangDb.Projekt_Nr, out val) ? val : 0,
							$"{wareneingangDb.Typ}",
							LogHelper.LogType.CREATIONWE,
							"MTM",
							user)
								.LogMTM(wareneingangDb.Nr);
					Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);
					if(botransaction.commit())
					{
						return ResponseModel<SaveResponseModel>.SuccessResponse(new SaveResponseModel() { WareneingangId = wareneingangDb.Nr });
					}

					/// Rollback and reture error.
					botransaction.rollback();
					return ResponseModel<SaveResponseModel>.FailureResponse("Transaction didn't commit.");
				} catch(Exception e)
				{
					botransaction.rollback();
					throw;
				}
			}
		}
		public ResponseModel<SaveResponseModel> Validate()
		{
			var listErrors = new List<string>();
			// Check Quantities.
			if(data.UpdatedArticle.Where(x => x.AktuelleAnzahl <= 0).ToList().Count > 0)
				listErrors.Add("Received Quantity can't be negative or equal to zero");
			// Check weights
			if(data.UpdatedArticle.Where(x => x.Grosse <= 0).ToList().Count > 0)
				listErrors.Add("Weight can't be negative or equal to zero");
			//Get Wareneingang data
			var WareneingangData = Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.GetWareneingangEntitiesByBestellungNr(data.Nr);
			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.Nr);
			// Check order data
			if(bestellung is null)
				listErrors.Add("Can't create wareneingang, Order not found.");
			// Check Order status
			if(!bestellung.gebucht.HasValue || bestellung.gebucht.Value != true)
				listErrors.Add("Can't create wareneingang, Order is not validated.");
			//Check Artikels Confiramtion status
			foreach(var item in data.UpdatedArticle)
			{
				var bestellteArtikelData = WareneingangData.FirstOrDefault(x => x.Nr == item.bestelleteArtikelNr);
				if(bestellteArtikelData is null)
					continue;

				if(bestellteArtikelData.COF_Pflichtig && !item.COCbestatigung)
				{
					listErrors.Add($"{item.ArtikelNummer}  cannot be booked, COC confirmation is not available! Please contact purchasing departement");
				}
				if(bestellteArtikelData.EMPB && !item.EMPBBestatigung)
				{
					listErrors.Add($"{item.ArtikelNummer} cannot be booked, EMPB confirmation is not available! Please contact QS Departement");
				}

				//Check quantities matching
				var overBookLimit = decimal.Parse(bestellteArtikelData.Start_Anzahl) / 10 + (decimal.TryParse(bestellteArtikelData.Anzahl, out var a) ? a : 0);
				if(item.AktuelleAnzahl > overBookLimit)
				{
					listErrors.Add($"{item.ArtikelNummer} booking quantity [{item.AktuelleAnzahl}] can't be bigger than open quantity (+10%) [{overBookLimit}]. Please contact {(!string.IsNullOrWhiteSpace(bestellung.Benutzer) ? bestellung.Benutzer.Substring(0, bestellung.Benutzer.Length - 20) : "order creator")} to change Order Quantity as needed.");
				}
			}

			// - 2024-09-12 - multiple pos in same WE from one pos from be. - check qty max
			var uniqueWePos = data.UpdatedArticle.Select(x => x.bestelleteArtikelNr).Distinct();
			foreach(var item in uniqueWePos)
			{
				var wePos = data.UpdatedArticle.FirstOrDefault(x => x.bestelleteArtikelNr == item);
				var bePos = WareneingangData.FirstOrDefault(x => x.Nr == item);
				var sumQty = data.UpdatedArticle.Where(x => x.bestelleteArtikelNr == item).Select(x => x.AktuelleAnzahl).Sum();
				//Check quantities matching
				var overBookLimit = decimal.Parse(bePos.Start_Anzahl) / 10 + (decimal.TryParse(bePos.Anzahl, out var a) ? a : 0);
				if(sumQty > overBookLimit)
				{
					listErrors.Add($"{wePos?.ArtikelNummer} booking quantity [{sumQty}] can't be bigger than open quantity (+10%) [{overBookLimit}]. Please contact {(!string.IsNullOrWhiteSpace(bestellung.Benutzer) ? bestellung.Benutzer.Substring(0, bestellung.Benutzer.Length - 20) : "order creator")} to change Order Quantity as needed.");
				}
			}

			//returning errors
			if(listErrors.Count > 0)
				return ResponseModel<SaveResponseModel>.FailureResponse(listErrors);

			//returning succes
			return ResponseModel<SaveResponseModel>.SuccessResponse(new SaveResponseModel());
		}
	}
}