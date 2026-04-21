using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class UpdateGlobalDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<object>>
	{
		private UpdateGlobalDataModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateGlobalDataHandler(UpdateGlobalDataModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<object> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			try
			{
				lock(Locks.Locks.OrdersLock)
				{
					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.Id);
					var orderExtensionDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrderId(orderDb.Nr);

					orderDb.Vorname_NameFirma = _data.Name;
					orderDb.Name2 = _data.Name2;
					orderDb.Name3 = _data.Name3;
					orderDb.Ansprechpartner = _data.Contact;

					orderDb.Abteilung = _data.Department;
					orderDb.Straße_Postfach = _data.StreetPOBox;
					orderDb.Land_PLZ_Ort = _data.CountryPostcode;

					orderDb.Bezug = _data.DocumentNumber;
					orderDb.Versandart = _data.Shipping;
					orderDb.Zahlungsweise = _data.Payment;
					orderDb.Konditionen = Common.Helpers.CTS.BlanketHelpers.TrimStartConditionsID(_data.Conditions);
					orderDb.USt_Berechnen = _data.Vat;

					orderDb.Falligkeit = _data.DueDate;
					orderDb.Briefanrede = _data.OrderTitle;
					orderDb.Personal_Nr = _data.PersonalNumber;
					orderDb.Freitext = _data.Freetext;
					orderDb.Lieferadresse = _data.ShippingAddress;
					orderDb.Reparatur_nr = _data.RepairNumber;
					orderDb.Datum = _data.Date;
					orderDb.Wunschtermin = _data.DesiredDate;
					orderDb.Liefertermin = _data.DeliveryDate;
					var _oldOrder = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.Id);
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(orderDb);
					//order extension
					if(orderExtensionDb == null)
					{
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity()
						{
							Id = -1,
							Version = 0,
							LastUpdateTime = DateTime.Now,
							LastUpdateUserId = _user.Id,
							LastUpdateUsername = _user.Username,
							OrderId = orderDb.Nr,
							EdiValidationTime = DateTime.Now,
							EdiValidationUserId = -1,
						});
					}
					else
					{
						orderExtensionDb.LastUpdateTime = DateTime.Now;
						orderExtensionDb.LastUpdateUserId = _user.Id;
						orderExtensionDb.LastUpdateUsername = _user.Username;
						orderExtensionDb.Version += 1;
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.Update(orderExtensionDb);
					}
					//logging
					var _logs = GetLogs(_oldOrder, orderDb, _user);
					if(_logs != null && _logs.Count > 0)
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_logs);

					return ResponseModel<object>.SuccessResponse();
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}

		}
		public ResponseModel<object> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<object>.AccessDeniedResponse();
			}
			var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.Id);
			if(orderDb == null)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Order not found");
			var customerDb = orderDb.Kunden_Nr.HasValue
						? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(orderDb.Kunden_Nr.Value)
						: null;
			if(customerDb == null)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Customer not found");
			var adressDb = customerDb.Nummer.HasValue
							? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
							: null;
			if(adressDb == null)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Address not found");
			if(_data.DocumentNumber != orderDb.Bezug)
			{
				// > Check if DocumentNumber Exists if new Order
				var orderDbByUniqueNumber = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByBezugAndKundenNr(_data.DocumentNumber, adressDb.Nr,
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION);
				if(orderDbByUniqueNumber != null && orderDbByUniqueNumber.Nr != orderDb.Nr)
					return ResponseModel<object>.FailureResponse(key: "1", value: $"Document Number Exists");
			}

			return ResponseModel<object>.SuccessResponse();
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity _old,
	Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity _new, Core.Identity.Models.UserModel user)
		{
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			Helpers.LogHelper _log = new Helpers.LogHelper(_old.Nr, (int)_old.Angebot_Nr, int.TryParse(_old.Projekt_Nr, out var val) ? val : 0, _old.Typ, Helpers.LogHelper.LogType.MODIFICATIONOBJECT, "CTS", user);

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
				_logs.Add(_log.LogCTS("USt_Berechnen", _old.USt_Berechnen?.ToString(), _new.USt_Berechnen?.ToString(), 0));
			}
			if(_old.Falligkeit != _new.Falligkeit)
			{
				_logs.Add(_log.LogCTS("Falligkeit", _old.Falligkeit?.ToString(), _new.Falligkeit?.ToString(), 0));
			}
			if(_old.Briefanrede != _new.Briefanrede)
			{
				_logs.Add(_log.LogCTS("Briefanrede", _old.Briefanrede, _new.Briefanrede, 0));
			}
			if(_old.Personal_Nr != _new.Personal_Nr)
			{
				_logs.Add(_log.LogCTS("Personal_Nr", _old.Personal_Nr?.ToString(), _new.Personal_Nr?.ToString(), 0));
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
				_logs.Add(_log.LogCTS("Reparatur_nr", _old.Reparatur_nr?.ToString(), _new.Reparatur_nr?.ToString(), 0));
			}
			if(_old.Datum != _new.Datum)
			{
				_logs.Add(_log.LogCTS("Datum", _old.Datum?.ToString(), _new.Datum?.ToString(), 0));
			}
			return _logs;
		}
	}
}
