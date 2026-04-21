using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class CreateCustomerForSupplierHandler: IHandle<Models.Customer.CreateModel, ResponseModel<int>>
	{
		private Models.Customer.CreateCustomerModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public CreateCustomerForSupplierHandler(Models.Customer.CreateCustomerModel Name, Identity.Models.UserModel user)
		{
			this._data = Name;
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

				#region // -- transaction start -- //

				this._data.Kundennummer = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetNewKundennummer(Module.AppSettings.SpecialCustomerNumberStart, botransaction.connection, botransaction.transaction);
				var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.AddressNr ?? -1, botransaction.connection, botransaction.transaction);
				if(!addressEntity.Kundennummer.HasValue)
				{
					Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateKundenNummer(new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
					{
						Nr = addressEntity.Nr,
						Kundennummer = this._data.Kundennummer,
					}, botransaction.connection, botransaction.transaction);
				}

				var insertedCostumerId = -1;
				var kundeEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(addressEntity.Nr, botransaction.connection, botransaction.transaction);
				if(kundeEntity == null)
				{
					insertedCostumerId = Infrastructure.Data.Access.Tables.PRS.KundenAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.KundenEntity
					{
						Nummer = this._data.AddressNr,
						Konditionszuordnungs_Nr = this._data.Konditionszuordnung,
						Branche = this._data.Branche,
						Kundengruppe = this._data.Kundengruppe,
						Umsatzsteuer_berechnen = this._data.Umsatzsteuer,
						Sprache = this._data.Sprache,
						Währung = this._data.Wahrung,
						Zahlungsweise = this._data.Zahlungsweise,
						Versandart = this._data.Versandart,
						Belegkreis = this._data.Belegkreis,
						Rabattgruppe = this._data.Rabattgruppe,
						Factoring = true,
						Debitoren_Nr = this._data.Debitorennummer,
						fibu_rahmen = this._data.FibuRahmen,
						// - 2023-04-19 - from compare w/ P3000
						Lieferscheinadresse = false,
						Mahnsperre = false,
						Zahlungskondition = 1,
						Preisgruppe = 1,
						Preisgruppe2 = 0,
						Kreditlimit = 0,
						Zielaufschlag = 0,
						Eilzuschlag = 0,
						Mindermengenzuschlag = 0,
						OPOS = true,
						Karenztage = 3,
						Mahngebühr_1 = 0,
						Mahngebühr_2 = 0,
						Mahngebühr_3 = 0,
						Zahlung_erwartet_nach = 10,
						Verzugszinsen = 0,
						Verzugszinsen_ab_Mahnstufe = 2,
						gesperrt_für_weitere_Lieferungen = false,
						Regelmäßig_anschreiben__ = false,
						LSADR = 0, // REM - ????
						LSADRANG = false,
						LSADRAUF = false,
						LSADRRG = false,
						LSADRGUT = false,
						LSADRSTO = false,
						LSADRPROF = false,
						LSRG = false,
						Bruttofakturierung = false,
						zolltarif_nr = false,
						Zahlungsmoral = 1
					}, botransaction.connection, botransaction.transaction);
				}
				else
				{
					kundeEntity.Nummer = this._data.AddressNr;
					kundeEntity.Konditionszuordnungs_Nr = this._data.Konditionszuordnung;
					kundeEntity.Branche = this._data.Branche;
					kundeEntity.Kundengruppe = this._data.Kundengruppe;
					kundeEntity.Umsatzsteuer_berechnen = this._data.Umsatzsteuer;
					kundeEntity.Sprache = this._data.Sprache;
					kundeEntity.Währung = this._data.Wahrung;
					kundeEntity.Zahlungsweise = this._data.Zahlungsweise;
					kundeEntity.Versandart = this._data.Versandart;
					kundeEntity.Belegkreis = this._data.Belegkreis;
					kundeEntity.Rabattgruppe = this._data.Rabattgruppe;
					kundeEntity.Factoring = true;
					kundeEntity.Debitoren_Nr = this._data.Debitorennummer;
					kundeEntity.fibu_rahmen = this._data.FibuRahmen;
					// - 2023-04-19 - from compare w/ P3000
					kundeEntity.Lieferscheinadresse = false;
					kundeEntity.Mahnsperre = false;
					kundeEntity.Zahlungskondition = 1;
					kundeEntity.Preisgruppe = 1;
					kundeEntity.Preisgruppe2 = 0;
					kundeEntity.Kreditlimit = 0;
					kundeEntity.Zielaufschlag = 0;
					kundeEntity.Eilzuschlag = 0;
					kundeEntity.Mindermengenzuschlag = 0;
					kundeEntity.OPOS = true;
					kundeEntity.Karenztage = 3;
					kundeEntity.Mahngebühr_1 = 0;
					kundeEntity.Mahngebühr_2 = 0;
					kundeEntity.Mahngebühr_3 = 0;
					kundeEntity.Zahlung_erwartet_nach = 10;
					kundeEntity.Verzugszinsen = 0;
					kundeEntity.Verzugszinsen_ab_Mahnstufe = 2;
					kundeEntity.gesperrt_für_weitere_Lieferungen = false;
					kundeEntity.Regelmäßig_anschreiben__ = false;
					kundeEntity.LSADR = 0; // REM - ????
					kundeEntity.LSADRANG = false;
					kundeEntity.LSADRAUF = false;
					kundeEntity.LSADRRG = false;
					kundeEntity.LSADRGUT = false;
					kundeEntity.LSADRSTO = false;
					kundeEntity.LSADRPROF = false;
					kundeEntity.LSRG = false;
					kundeEntity.Bruttofakturierung = false;
					kundeEntity.zolltarif_nr = false;
					kundeEntity.Zahlungsmoral = 1;
					Infrastructure.Data.Access.Tables.PRS.KundenAccess.Update(kundeEntity, botransaction.connection, botransaction.transaction);
					insertedCostumerId = kundeEntity.Nr;
				}
				Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.BSD.KundenExtensionEntity
				{
					Nr = insertedCostumerId,
					IsArchived = false,
					UpdateUserId = this._user.Id,
					UpdateTime = DateTime.Now,
				}, botransaction.connection, botransaction.transaction);
				//logging
				var log = ObjectLogHelper.getLog(this._user, insertedCostumerId, "Customer", null, this._data.Name1, Enums.ObjectLogEnums.Objects.Customer.GetDescription(), Enums.ObjectLogEnums.LogType.Add);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction end -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(insertedCostumerId);
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

			var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.AddressNr ?? -1);
			if(addressEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Address not found");
			}
			if(addressEntity.Kundennummer.HasValue && addressEntity.Adresstyp == (int)Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum.Standard)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Customer [{addressEntity.Name1}] with the same Address Type is already used" }
						}
				};
			}
			if(
				this._data.Branche == null || //string.IsNullOrEmpty(this._data.Branche) || string.IsNullOrWhiteSpace(this._data.Branche) ||
				this._data.Kundengruppe == null || //string.IsNullOrEmpty(this._data.Kundengruppe) || string.IsNullOrWhiteSpace(this._data.Kundengruppe) ||
				string.IsNullOrEmpty(this._data.Zahlungsweise) || string.IsNullOrWhiteSpace(this._data.Zahlungsweise) ||
				string.IsNullOrEmpty(this._data.Versandart) || string.IsNullOrWhiteSpace(this._data.Versandart) ||
				!this._data.Konditionszuordnung.HasValue || (this._data.Konditionszuordnung.HasValue && this._data.Konditionszuordnung.Value == -1) ||
				!this._data.Sprache.HasValue || (this._data.Sprache.HasValue && this._data.Sprache.Value == -1) ||
				!this._data.Wahrung.HasValue || (this._data.Wahrung.HasValue && this._data.Wahrung.Value == -1) ||
				!this._data.Belegkreis.HasValue || (this._data.Adresstyp.HasValue && this._data.Belegkreis.Value == -1) ||
				!this._data.Rabattgruppe.HasValue || (this._data.Rabattgruppe.HasValue && this._data.Rabattgruppe.Value == -1)
				)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Please fill all required fields" }
						}
				};
			}

			var kundennummer_exsist = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetCountKundennummer((int)this._data.Kundennummer);
			if(kundennummer_exsist)
			{
				return ResponseModel<int>.FailureResponse($"Kundennummer [{this._data.Kundennummer}] already exsists.");
			}
			var kundeEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(this._data.AddressNr ?? -1);
			if(kundeEntity != null && addressEntity.Kundennummer != null)
			{
				return ResponseModel<int>.FailureResponse($"Kunde [{addressEntity.Kundennummer}] already exists for [{addressEntity.Name1}].");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
