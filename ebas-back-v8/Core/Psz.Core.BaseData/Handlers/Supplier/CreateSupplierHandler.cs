using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	public class CreateSupplierHandler: IHandle<Models.Supplier.UpdateModel, ResponseModel<int>>
	{
		//private Models.Supplier.UpdateModel _data { get; set; }
		private Models.Supplier.SupplierCreateModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public CreateSupplierHandler(Models.Supplier.SupplierCreateModel data, Identity.Models.UserModel user)
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

				int adressId = -1;
				var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByName1(this._data.Name1.Trim(), this._data.Adresstyp ?? -1, botransaction.connection, botransaction.transaction);
				if(addressEntity != null)
				{
					if(!addressEntity.Lieferantennummer.HasValue)
					{
						Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateLieferantenNummer(new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
						{
							Nr = addressEntity.Nr,
							Lieferantennummer = this._data.Lieferantennummer,
						}, botransaction.connection, botransaction.transaction);
					}
					adressId = addressEntity.Nr;
				}
				else
				{
					var insertedAddressId = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
						{
							Lieferantennummer = this._data.Lieferantennummer,
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
							Stufe = this._data.Stufe,
						}, botransaction.connection, botransaction.transaction);
					adressId = insertedAddressId;
				}
				var insertedSupplierId = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity
				{
					Nummer = adressId,
					Konditionszuordnungs_Nr = this._data.Konditionszuordnung,
					Branche = this._data.Branche,
					Lieferantengruppe = this._data.Lieferantengruppe,
					Umsatzsteuer_berechnen = this._data.Umsatzsteuer,
					Sprache = this._data.Sprache,
					Wahrung = this._data.Wahrung,
					Zahlungsweise = this._data.Zahlungsweise,
					Versandart = this._data.Versandart,
				}, botransaction.connection, botransaction.transaction);
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
				string.IsNullOrEmpty(this._data.StraBe) || string.IsNullOrWhiteSpace(this._data.StraBe) ||
				string.IsNullOrEmpty(this._data.Land) || string.IsNullOrWhiteSpace(this._data.Land) ||
				string.IsNullOrEmpty(this._data.PLZ_StraBe) || string.IsNullOrWhiteSpace(this._data.PLZ_StraBe) ||
				string.IsNullOrEmpty(this._data.Ort) || string.IsNullOrWhiteSpace(this._data.Ort) ||
				string.IsNullOrEmpty(this._data.Telefon) || string.IsNullOrWhiteSpace(this._data.Telefon) ||
				string.IsNullOrEmpty(this._data.Mail) || string.IsNullOrWhiteSpace(this._data.Mail) ||
				this._data.Anrede == null || // string.IsNullOrEmpty(this._data.Anrede) || string.IsNullOrWhiteSpace(this._data.Anrede) ||
				string.IsNullOrEmpty(this._data.Name1) || string.IsNullOrWhiteSpace(this._data.Name1) ||
				this._data.Branche == null || //string.IsNullOrEmpty(this._data.Branche) || string.IsNullOrWhiteSpace(this._data.Branche) ||
				string.IsNullOrEmpty(this._data.Lieferantengruppe) || string.IsNullOrWhiteSpace(this._data.Lieferantengruppe) ||
				string.IsNullOrEmpty(this._data.Zahlungsweise) || string.IsNullOrWhiteSpace(this._data.Zahlungsweise) ||
				string.IsNullOrEmpty(this._data.Versandart) || string.IsNullOrWhiteSpace(this._data.Versandart) ||
				!this._data.Adresstyp.HasValue || (this._data.Adresstyp.HasValue && this._data.Adresstyp.Value == -1) ||
				!this._data.Lieferantennummer.HasValue || (this._data.Lieferantennummer.HasValue && this._data.Lieferantennummer.Value == -1) ||
				!this._data.Konditionszuordnung.HasValue || (this._data.Konditionszuordnung.HasValue && this._data.Konditionszuordnung.Value == -1) ||
				!this._data.Sprache.HasValue || (this._data.Sprache.HasValue && this._data.Sprache.Value == -1) ||
				!this._data.Wahrung.HasValue || (this._data.Wahrung.HasValue && this._data.Wahrung.Value == -1) ||
				string.IsNullOrEmpty(this._data.Stufe) || string.IsNullOrWhiteSpace(this._data.Stufe)
				)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Please fill all required fields" }
						}
				};
			}
			var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByName1(this._data.Name1.Trim(), this._data.Adresstyp ?? -1);
			if(addressEntity != null)
			{

				if(addressEntity.Lieferantennummer.HasValue)
				{
					return new ResponseModel<int>()
					{
						Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Address Name 1 with the same Address Type is already used" }
						}
					};
				}
			}

			var Lieferantennummer_exsist = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetCountLieferantennummer((int)this._data.Lieferantennummer);
			if(Lieferantennummer_exsist)
			{
				return ResponseModel<int>.FailureResponse($"Lieferantennummer [{this._data.Lieferantennummer}] already exsists.");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
