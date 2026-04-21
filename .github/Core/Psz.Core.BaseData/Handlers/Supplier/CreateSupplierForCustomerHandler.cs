using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	public class CreateSupplierForCustomerHandler: IHandle<Models.Supplier.UpdateModel, ResponseModel<int>>
	{
		//private Models.Supplier.UpdateModel _data { get; set; }
		private Models.Supplier.SupplierCreateModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public CreateSupplierForCustomerHandler(Models.Supplier.SupplierCreateModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
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

				var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.AddressNr ?? -1, botransaction.connection, botransaction.transaction);
				if(!addressEntity.Lieferantennummer.HasValue)
				{
					Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateLieferantenNummer(new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
					{
						Nr = addressEntity.Nr,
						Lieferantennummer = this._data.Lieferantennummer,
					}, botransaction.connection, botransaction.transaction);
				}
				var insertedSupplierId = -1;
				var Lieferant = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByNummer(this._data.AddressNr ?? -1, botransaction.connection, botransaction.transaction);
				if(Lieferant == null)
				{
					insertedSupplierId = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity
					{
						Nummer = this._data.AddressNr,
						Konditionszuordnungs_Nr = this._data.Konditionszuordnung,
						Branche = this._data.Branche,
						Lieferantengruppe = this._data.Lieferantengruppe,
						Umsatzsteuer_berechnen = this._data.Umsatzsteuer,
						Sprache = this._data.Sprache,
						Wahrung = this._data.Wahrung,
						Zahlungsweise = this._data.Zahlungsweise,
						Versandart = this._data.Versandart,
						// - 2023-04-19 - from compare w P3000
						Belegkreis = 0,
						Versandkosten = 0,
						Mahnsperre = false,
						Frachtfreigrenze = 0,
						Bestellimit = 0,
						Zielaufschlag = 0,
						Eilzuschlag = 0,
						Mindestbestellwert = 0,
						Zuschlag_Mindestbestellwert = 0,
						Rabattgruppe = 1,
						Mahnsperre_Lieferant = false,
						Gesperrt_fur_weitere_Bestellungen = false,
						Bestellbestatigung_anmahnen = false,
						LH = false,
						LH_Datum = new DateTime(2999, 12, 31)
					}, botransaction.connection, botransaction.transaction);
				}
				else
				{
					insertedSupplierId = Lieferant.Nr;
					Lieferant.Nummer = this._data.AddressNr;
					Lieferant.Konditionszuordnungs_Nr = this._data.Konditionszuordnung;
					Lieferant.Branche = this._data.Branche;
					Lieferant.Lieferantengruppe = this._data.Lieferantengruppe;
					Lieferant.Umsatzsteuer_berechnen = this._data.Umsatzsteuer;
					Lieferant.Sprache = this._data.Sprache;
					Lieferant.Wahrung = this._data.Wahrung;
					Lieferant.Zahlungsweise = this._data.Zahlungsweise;
					Lieferant.Versandart = this._data.Versandart;
					// - 2023-04-19 - from compare w P3000
					Lieferant.Belegkreis = 0;
					Lieferant.Versandkosten = 0;
					Lieferant.Mahnsperre = false;
					Lieferant.Frachtfreigrenze = 0;
					Lieferant.Bestellimit = 0;
					Lieferant.Zielaufschlag = 0;
					Lieferant.Eilzuschlag = 0;
					Lieferant.Mindestbestellwert = 0;
					Lieferant.Zuschlag_Mindestbestellwert = 0;
					Lieferant.Rabattgruppe = 1;
					Lieferant.Mahnsperre_Lieferant = false;
					Lieferant.Gesperrt_fur_weitere_Bestellungen = false;
					Lieferant.Bestellbestatigung_anmahnen = false;
					Lieferant.LH = false;
					Lieferant.LH_Datum = new DateTime(2999, 12, 31);
					insertedSupplierId = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.InsertWithTransaction(Lieferant, botransaction.connection, botransaction.transaction);
				}
				Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.BSD.LieferantenExtensionEntity
				{
					Nr = insertedSupplierId,
					IsArchived = false,
					ArchiveUserId = null,
					ArchiveTime = null,
				}, botransaction.connection, botransaction.transaction);
				//logging
				var log = ObjectLogHelper.getLog(this._user, insertedSupplierId, "Supplier", null, this._data.Name1, Enums.ObjectLogEnums.Objects.Supplier.GetDescription(), Enums.ObjectLogEnums.LogType.Add);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(insertedSupplierId);
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
			if(
				this._data.Branche == null || //string.IsNullOrEmpty(this._data.Branche) || string.IsNullOrWhiteSpace(this._data.Branche) ||
				string.IsNullOrEmpty(this._data.Lieferantengruppe) || string.IsNullOrWhiteSpace(this._data.Lieferantengruppe) ||
				string.IsNullOrEmpty(this._data.Zahlungsweise) || string.IsNullOrWhiteSpace(this._data.Zahlungsweise) ||
				string.IsNullOrEmpty(this._data.Versandart) || string.IsNullOrWhiteSpace(this._data.Versandart) ||
				!this._data.Lieferantennummer.HasValue || (this._data.Lieferantennummer.HasValue && this._data.Lieferantennummer.Value == -1) ||
				!this._data.Konditionszuordnung.HasValue || (this._data.Konditionszuordnung.HasValue && this._data.Konditionszuordnung.Value == -1) ||
				!this._data.Sprache.HasValue || (this._data.Sprache.HasValue && this._data.Sprache.Value == -1) ||
				!this._data.Wahrung.HasValue || (this._data.Wahrung.HasValue && this._data.Wahrung.Value == -1)
				)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Please fill all required fields" }
						}
				};
			}
			var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.AddressNr ?? -1);
			if(addressEntity != null)
			{
				if(addressEntity.Lieferantennummer.HasValue)
				{
					return new ResponseModel<int>()
					{
						Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Address [{addressEntity.Name1}] with the same Address Type is already used" }
						}
					};
				}
			}
			else
			{
				return ResponseModel<int>.FailureResponse("Address not found");
			}

			var Lieferantennummer_exsist = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetCountLieferantennummer((int)this._data.Lieferantennummer);
			if(Lieferantennummer_exsist)
			{
				return ResponseModel<int>.FailureResponse($"Lieferantennummer [{this._data.Lieferantennummer}] already exsists.");
			}
			var Lieferant = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByNummer(this._data.AddressNr ?? -1);
			if(Lieferant != null && addressEntity.Lieferantennummer != null)
			{
				return ResponseModel<int>.FailureResponse($"Lieferanten [{addressEntity.Lieferantennummer}] already exists for [{addressEntity.Name1}].");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
