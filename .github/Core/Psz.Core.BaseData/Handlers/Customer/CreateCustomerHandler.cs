using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class CreateCustomerHandler: IHandle<Models.Customer.CreateModel, ResponseModel<int>>
	{
		private Models.Customer.CreateCustomerModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public CreateCustomerHandler(Models.Customer.CreateCustomerModel Name, Identity.Models.UserModel user)
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
				int adressId = -1;
				var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByName1(this._data.Name1.Trim(), this._data.Adresstyp ?? -1, botransaction.connection, botransaction.transaction);
				if(addressEntity != null)
				{
					if(!addressEntity.Kundennummer.HasValue)
					{
						Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateKundenNummer(new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
						{
							Nr = addressEntity.Nr,
							Kundennummer = this._data.Kundennummer,
						}, botransaction.connection, botransaction.transaction);
					}
					adressId = addressEntity.Nr;
				}
				else
				{
					var insertedAddressId = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
						{
							Kundennummer = this._data.Kundennummer,
							Name1 = this._data.Name1,
							Adresstyp = this._data.Adresstyp,
							StraBe = this._data.StraBe,
							Land = this._data.Land,
							PLZ_StraBe = this._data.PLZ_StraBe,
							Ort = this._data.Ort,
							Telefon = this._data.Telefon,
							EMail = this._data.Mail,
							Anrede = this._data.Anrede,
							Sperren = false,
							Erfasst = DateTime.Now, // 2024-02-26
						}, botransaction.connection, botransaction.transaction);
					adressId = insertedAddressId;
				}
				var insertedCostumerId = Infrastructure.Data.Access.Tables.PRS.KundenAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.KundenEntity
				{
					Nummer = adressId,
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
					fibu_rahmen = this._data.FibuRahmen
				}, botransaction.connection, botransaction.transaction);
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
			var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByName1(this._data.Name1.Trim(), this._data.Adresstyp ?? -1);
			if(addressEntity != null)
			{
				if(addressEntity.Kundennummer.HasValue)
				{
					return new ResponseModel<int>()
					{
						Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Address Name 1 with the same Address Type is already used" }
						}
					};
				}
			}
			if(
				string.IsNullOrEmpty(this._data.StraBe) || string.IsNullOrWhiteSpace(this._data.StraBe) ||
				string.IsNullOrEmpty(this._data.Land) || string.IsNullOrWhiteSpace(this._data.Land) ||
				string.IsNullOrEmpty(this._data.PLZ_StraBe) || string.IsNullOrWhiteSpace(this._data.PLZ_StraBe) ||
				string.IsNullOrEmpty(this._data.Ort) || string.IsNullOrWhiteSpace(this._data.Ort) ||
				string.IsNullOrEmpty(this._data.Telefon) || string.IsNullOrWhiteSpace(this._data.Telefon) ||
				string.IsNullOrEmpty(this._data.Mail) || string.IsNullOrWhiteSpace(this._data.Mail) ||
				this._data.Anrede == null || // string.IsNullOrEmpty(this._data.Anrede) || string.IsNullOrWhiteSpace(this._data.Anrede) ||
				string.IsNullOrEmpty(this._data.Name1) || string.IsNullOrWhiteSpace(this._data.Name1) ||
				this._data.Branche == null || //string.IsNullOrEmpty(this._data.Branche) || string.IsNullOrWhiteSpace(this._data.Branche) ||
				this._data.Kundengruppe == null || //string.IsNullOrEmpty(this._data.Kundengruppe) || string.IsNullOrWhiteSpace(this._data.Kundengruppe) ||
				string.IsNullOrEmpty(this._data.Zahlungsweise) || string.IsNullOrWhiteSpace(this._data.Zahlungsweise) ||
				string.IsNullOrEmpty(this._data.Versandart) || string.IsNullOrWhiteSpace(this._data.Versandart) ||
				!this._data.Adresstyp.HasValue || (this._data.Adresstyp.HasValue && this._data.Adresstyp.Value == -1) ||
				!this._data.Kundennummer.HasValue || (this._data.Kundennummer.HasValue && this._data.Kundennummer.Value == -1) ||
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

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
