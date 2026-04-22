using Psz.Core.CustomerService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> UpdateGlobalData(Models.Order.UpdateGlobalDataModel data,
		  Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null || (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return UpdateGlobalData(data, false, user);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static Core.Models.ResponseModel<object> UpdateGlobalData(Models.Order.UpdateGlobalDataModel data,
			bool confirm,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.OrdersLock)
			{
				try
				{
					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data.Id);
					if(orderDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order not found" }
						};
					}

					// - 20232-10-30 - Heidenreich prevent edit for AB created from LP
					if(orderDb.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION.Trim().ToLower())
					{
						if(orderDb.nr_dlf.HasValue && orderDb.nr_dlf.Value > 0)
						{
							return Core.Models.ResponseModel<object>.FailureResponse("Cannot edit order created from LP");
						}
					}

					// - 20232-11-04 - Heidenreich prevent edit for AB created from RA
					if(orderDb.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION.Trim().ToLower())
					{
						if(orderDb.Nr_RA.HasValue && orderDb.Nr_RA.Value > 0 && data.HasChangedFromRA(orderDb) == true)
						{
							return Core.Models.ResponseModel<object>.FailureResponse("Cannot edit customer of order created from RA");
						}
					}

					// - 20232-05-10 - Heidenreich prevent edit for RG after their Creation DAY or when Booked
					if(orderDb.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE.Trim().ToLower())
					{
						if(orderDb.Datum < DateTime.Today || orderDb.Gebucht == true)
						{
							return Core.Models.ResponseModel<object>.FailureResponse("Invoice edit is not allowed");
						}
					}
					//var customerDb = orderDb.Kunden_Nr.HasValue
					//    ? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(orderDb.Kunden_Nr.Value)
					//    : null;
					var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(data.CustomerId); // - 2022-05-04 can change customer
					if(customerDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Customer not found" }
						};
					}

					var adressDb = customerDb.Nummer.HasValue
							? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
							: null;
					if(adressDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Address not found" }
						};
					}

					if(customerDb.LSADR.HasValue && customerDb.LSADR > 0)
					{
						var adressLiefDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.LSADR.Value);
						if(adressLiefDb == null)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Delivery Address not found" }
							};
						}
					}

					// - if EDI
					if(orderDb.Neu_Order.HasValue && adressDb.Nr != orderDb.Kunden_Nr)
					{
						return Core.Models.ResponseModel<object>.FailureResponse("Cannot change Customer for EDI Orders");
					}

					var warnings = new List<string> { };
					if(data.DocumentNumber != orderDb.Bezug || adressDb.Nr != orderDb.Kunden_Nr)
					{
						// > Check if DocumentNumber Exists if new Order
						var orderDbByUniqueNumber = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByBezugAndKundenNr(data.DocumentNumber,
							adressDb.Nr,
							orderDb.Typ);

						if(orderDbByUniqueNumber != null && orderDbByUniqueNumber.Nr != orderDb.Nr)
						{
							// - EDI
							if(orderDb.Neu_Order.HasValue)
							{
								return new Core.Models.ResponseModel<object>()
								{
									Errors = new List<string>() { "Document Number Exists" }
								};
							}
							else
							{
								warnings.Add($"Document Number [{data.DocumentNumber}] exists");
							}
						}
					}

					var orderExtensionDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrderId(orderDb.Nr);

					// > Check Version
					//if (orderExtensionDb != null && data.Version != orderExtensionDb.Version)
					//{
					//    return new Core.Models.ResponseModel<object>()
					//    {
					//        Errors = new List<string>() { "You are not using the latest data version" }
					//    };
					//}
					//customer change
					var lieferadressDb = new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity();

					if(customerDb.LSADR.HasValue && customerDb.LSADR > 0)
						lieferadressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)customerDb.LSADR);
					else
						lieferadressDb = customerDb.Nummer.HasValue
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
					: null;

					var newadressDb = customerDb.Nummer.HasValue
						? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
						: null;
					var mailBoxIsPreferred = adressDb?.Postfach_bevorzugt == true;
					var conditionAssignementTableDb = customerDb.Konditionszuordnungs_Nr.HasValue
							? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customerDb.Konditionszuordnungs_Nr.Value)
							: null;
					//
					var orderPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderDb.Nr);
					if(orderDb.Kunden_Nr != newadressDb.Nr)
					{
						var abPositons = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderDb.Nr);
						var abPostionsWithRahmens = abPositons?.Where(x => x.ABPoszuRAPos.HasValue && x.ABPoszuRAPos.Value != 0 && x.ABPoszuRAPos != -1)?.ToList();
						if(abPostionsWithRahmens != null && abPostionsWithRahmens.Count > 0)
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { $"Positions [{string.Join(",", abPostionsWithRahmens.Select(x => x.Position.ToString()).ToList())}] are linked to Rahmens, cannot change customer." }
							};
					}
					if(orderDb.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CREDIT.Trim().ToLower())
					{
						//var technicArticles = Program.BSD.TechnicArticleIds;
						var horizionErrors = new List<string>();
						if(orderPositions != null && orderPositions.Count > 0)
						{
							foreach(var item in orderPositions)
							{
								var horizonCheck = HorizonsHelper.userHasGSPosHorizonRight(item.Liefertermin ?? new DateTime(1900, 1, 1), user, out List<string> messages);
								if(!horizonCheck && !HorizonsHelper.ArticleIsTechnic(item.ArtikelNr ?? -1))
									horizionErrors.AddRange(messages);
							}
						}
						if(horizionErrors != null && horizionErrors.Count > 0)
							return new Core.Models.ResponseModel<object>()
							{
								Errors = horizionErrors
							};
					}

					orderDb.Vorname_NameFirma = data.Name;
					orderDb.Name2 = data.Name2;
					orderDb.Name3 = data.Name3;
					orderDb.Ansprechpartner = data.Contact;
					//

					//

					orderDb.Abteilung = data.Department;
					orderDb.Straße_Postfach = data.StreetPOBox;
					orderDb.Land_PLZ_Ort = data.CountryPostcode;

					orderDb.Bezug = data.DocumentNumber;
					orderDb.Versandart = data.Shipping;
					orderDb.Zahlungsweise = data.Payment;
					orderDb.Konditionen = Common.Helpers.CTS.BlanketHelpers.TrimStartConditionsID( data.Conditions);
					orderDb.USt_Berechnen = data.Vat;

					orderDb.Falligkeit = data.DueDate;
					orderDb.Briefanrede = data.OrderTitle;
					orderDb.Personal_Nr = data.PersonalNumber;
					orderDb.Freitext = data.Freetext;
					orderDb.Lieferadresse = data.ShippingAddress;
					orderDb.Reparatur_nr = data.RepairNumber;
					orderDb.Datum = data.Date;
					orderDb.Wunschtermin = data.DesiredDate;
					orderDb.Liefertermin = data.DeliveryDate;
					//added (souilmi)
					orderDb.Kunden_Nr = newadressDb.Nr;
					orderDb.Unser_Zeichen = newadressDb.Kundennummer.HasValue ? newadressDb.Kundennummer.ToString() : "";
					orderDb.Ihr_Zeichen = customerDb.Lieferantenummer__Kunden_;
					if(confirm)
					{
						orderDb.LAnrede = lieferadressDb?.Anrede;
						orderDb.LVorname_NameFirma = lieferadressDb?.Name1;
						orderDb.LName2 = lieferadressDb?.Name2;
						orderDb.LName3 = lieferadressDb?.Name3;
						orderDb.LAnsprechpartner = lieferadressDb?.Abteilung;
						orderDb.LAbteilung = lieferadressDb?.Abteilung;
						orderDb.LStraße_Postfach = $"{lieferadressDb?.StraBe}";
						orderDb.LLand_PLZ_Ort = $"{lieferadressDb?.PLZ_StraBe}, {lieferadressDb?.Ort}".Trim(new char[] { ',', ' ' });
						orderDb.LBriefanrede = lieferadressDb?.Briefanrede;
					}


					var _oldOrder = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data.Id);
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(orderDb);
					//logging
					var _logs = GetLogs(_oldOrder, orderDb, user);
					if(_logs != null && _logs.Count > 0)
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_logs);

					if(confirm)
					{
						var nextAngeboteNr = getNextAngebotNr(Apps.Purchase.Enums.OrderEnums.Types.Confirmation);
						int FinalVofallNr = Helpers.ItemHelper.GetVorfallNrFromRange(Enums.OrderEnums.Types.Confirmation, user.Username);
						//if ((int.TryParse(nextAngeboteNr, out var val) ? val : 0) < Program.CTS.abMaxCurrentValue)
						//{
						//    FinalVofallNr = int.TryParse(nextAngeboteNr, out var val2) ? val2 : 0;
						//}
						//else
						//    FinalVofallNr = Program.CTS.abMinNewValue;
						////alert max reached
						//if (FinalVofallNr == Program.CTS.abMaxNewValue - Program.CTS.Delta)
						//{
						//    string title = "MAX VALUE VORFALL NR AB REACHED";
						//    var addresses = new List<string>();
						//    addresses.Add("Mohamed.Souilmi@psz-electronic.com");
						//    addresses.Add(Program.EmailingService.EmailParamtersModel.AdminEmail);
						//    var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
						//    + $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
						//    + $"<br/><span style='font-size:1.15em;'><strong>{user.Name?.ToUpper()}</strong> has reached the naximum range in vorfall nr in AB creation [{FinalVofallNr}]</strong>."
						//    + $"</span><br/><br/>The change is applyed and Logged"
						//    + "<br/><br/>Regards, <br/>IT Department </div>";

						//    Program.EmailingService.SendEmail(title, content, addresses);
						//}

						var nextProjektNr = nextAngeboteNr;

						Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(orderDb.Nr,
							FinalVofallNr.ToString(),
							nextProjektNr,
							$"Gebucht, {user.Username}, {DateTime.Now}",
							true);
					}

					if(orderExtensionDb == null)
					{
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity()
						{
							Id = -1,
							Version = 0,
							LastUpdateTime = DateTime.Now,
							LastUpdateUserId = user.Id,
							LastUpdateUsername = user.Username,
							OrderId = orderDb.Nr,
							EdiValidationTime = DateTime.Now,
							EdiValidationUserId = -1,
						});
					}
					else
					{
						orderExtensionDb.LastUpdateTime = DateTime.Now;
						orderExtensionDb.LastUpdateUserId = user.Id;
						orderExtensionDb.LastUpdateUsername = user.Username;
						orderExtensionDb.Version += 1;
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.Update(orderExtensionDb);
					}
					if(orderDb.Typ == "Lieferschein")
					{
						var LSItemsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderDb.Nr);
						var LSItemsArticlesEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(LSItemsEntities.Select(l => (int)l.ArtikelNr).ToList() ?? new List<int> { -1 });
						generateDATFile(orderDb, LSItemsEntities, LSItemsArticlesEntities);
					}

					return new Core.Models.ResponseModel<object>
					{
						Success = true,
						Warnings = warnings.Count > 0 ? warnings : null
					};
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static string getNextAngebotNr(Enums.OrderEnums.Types type)
		{
			var maxAngebotNrString = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.MaxAngebotNrByTyp(Enums.OrderEnums.TypeToData(type));
			return $"{Convert.ToInt32(Convert.ToDecimal(maxAngebotNrString)) + 1}";
		}

		public static string getNextProjektNr(Enums.OrderEnums.Types type)
		{
			var maxProjektNrString = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.MaxProjektNrByTyp(Enums.OrderEnums.TypeToData(type));
			return $"{Convert.ToInt32(Convert.ToDecimal(maxProjektNrString)) + 1}";
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity _old,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity _new, Core.Identity.Models.UserModel user)
		{
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			LogHelper _log = new LogHelper(_old.Nr, (int)_old.Angebot_Nr, int.TryParse(_old.Projekt_Nr, out var val) ? val : 0, _old.Typ, LogHelper.LogType.MODIFICATIONOBJECT, "CTS", user);

			if(_old.Vorname_NameFirma != _new.Vorname_NameFirma)
			{
				_logs.Add(_log.LogCTS("Vorname_NameFirma", _old.Vorname_NameFirma, _new.Vorname_NameFirma, 0));
			}
			if(_old.Name2 != _new.Name2)
			{
				_logs.Add(_log.LogCTS("Name2", _old.Name2, _new.Name2, 0));
			}
			if(_old.Name3 != _new.Name3)
			{
				_logs.Add(_log.LogCTS("Name3", _old.Name3, _new.Name3, 0));
			}
			if(_old.Ansprechpartner != _new.Ansprechpartner)
			{
				_logs.Add(_log.LogCTS("Ansprechpartner", _old.Ansprechpartner, _new.Ansprechpartner, 0));
			}
			if(_old.Abteilung != _new.Abteilung)
			{
				_logs.Add(_log.LogCTS("Abteilung", _old.Abteilung, _new.Abteilung, 0));
			}
			if(_old.Straße_Postfach != _new.Straße_Postfach)
			{
				_logs.Add(_log.LogCTS("Straße_Postfach", _old.Straße_Postfach, _new.Straße_Postfach, 0));
			}
			if(_old.Straße_Postfach != _new.Straße_Postfach)
			{
				_logs.Add(_log.LogCTS("Straße_Postfach", _old.Straße_Postfach, _new.Straße_Postfach, 0));
			}
			if(_old.Land_PLZ_Ort != _new.Land_PLZ_Ort)
			{
				_logs.Add(_log.LogCTS("Land_PLZ_Ort", _old.Land_PLZ_Ort, _new.Land_PLZ_Ort, 0));
			}
			if(_old.Bezug != _new.Bezug)
			{
				_logs.Add(_log.LogCTS("Bezug", _old.Bezug, _new.Bezug, 0));
			}
			if(_old.Versandart != _new.Versandart)
			{
				_logs.Add(_log.LogCTS("Versandart", _old.Versandart, _new.Versandart, 0));
			}
			if(_old.Zahlungsweise != _new.Zahlungsweise)
			{
				_logs.Add(_log.LogCTS("Zahlungsweise", _old.Zahlungsweise, _new.Zahlungsweise, 0));
			}
			if(_old.Zahlungsweise != _new.Zahlungsweise)
			{
				_logs.Add(_log.LogCTS("Zahlungsweise", _old.Zahlungsweise, _new.Zahlungsweise, 0));
			}
			if(_old.Konditionen != _new.Konditionen)
			{
				_logs.Add(_log.LogCTS("Konditionen", _old.Konditionen, _new.Konditionen, 0));
			}
			if(_old.USt_Berechnen != _new.USt_Berechnen)
			{
				_logs.Add(_log.LogCTS("USt_Berechnen", _old.USt_Berechnen.ToString(), _new.USt_Berechnen.ToString(), 0));
			}
			if(_old.Falligkeit != _new.Falligkeit)
			{
				_logs.Add(_log.LogCTS("Falligkeit", _old.Falligkeit.ToString(), _new.Falligkeit.ToString(), 0));
			}
			if(_old.Briefanrede != _new.Briefanrede)
			{
				_logs.Add(_log.LogCTS("Briefanrede", _old.Briefanrede, _new.Briefanrede, 0));
			}
			if(_old.Personal_Nr != _new.Personal_Nr)
			{
				_logs.Add(_log.LogCTS("Personal_Nr", _old.Personal_Nr.ToString(), _new.Personal_Nr.ToString(), 0));
			}
			if(_old.Freitext != _new.Freitext)
			{
				_logs.Add(_log.LogCTS("Freitext", _old.Freitext, _new.Freitext, 0));
			}
			if(_old.Lieferadresse != _new.Lieferadresse)
			{
				_logs.Add(_log.LogCTS("Lieferadresse", _old.Lieferadresse, _new.Lieferadresse, 0));
			}
			if(_old.Reparatur_nr != _new.Reparatur_nr)
			{
				_logs.Add(_log.LogCTS("Reparatur_nr", _old.Reparatur_nr.ToString(), _new.Reparatur_nr.ToString(), 0));
			}
			if(_old.Datum != _new.Datum)
			{
				_logs.Add(_log.LogCTS("Datum", _old.Datum.ToString(), _new.Datum.ToString(), 0));
			}
			return _logs;
		}

	}
}
