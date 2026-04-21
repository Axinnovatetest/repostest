using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer.Communication
{
	public class UpdateCustomerCommunicationHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Customer.CustomerCommunicationModel _data { get; set; }

		public UpdateCustomerCommunicationHandler(Models.Customer.CustomerCommunicationModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
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

				var adressenEntity = this._data.ToEntity();
				var logs = LogChanges();
				// save update logs
				if(logs.Count > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
				}

				Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateCommunication(adressenEntity);
				Infrastructure.Data.Access.Tables.PRS.KundenAccess.UpdateLanguage(this._data.Id, (int)this._data.LanguageId);

				var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data.Id);
				if(_data.OrderAddress?.Trim()?.ToLower() != kundenEntity.OrderAddress?.Trim()?.ToLower())
				{
					Infrastructure.Data.Access.Tables.PRS.KundenAccess.UpdateOrderAddress(this._data.Id, this._data.OrderAddress);
				}

				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
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

			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data.Id);
			if(kundenEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Customer not found"}
					}
				};
			}

			if(string.IsNullOrEmpty(this._data.AdressPhoneNumber) || string.IsNullOrWhiteSpace(this._data.AdressPhoneNumber) ||
				!Infrastructure.Services.Email.Helpers.IsValidEmail(_data.AdressEmailAdress) ||
				!this._data.LanguageId.HasValue || !Infrastructure.Services.Email.Helpers.IsValidEmail(_data.OrderAddress)
				)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Please fill all the required fields"}
					}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}


		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)this._data.AdressId);
			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data.Id);
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Edit;
			//
			if(kundenEntity.Sprache != this._data.LanguageId)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Sprache", kundenEntity.Sprache.ToString(), this._data.LanguageId.ToString(), Enums.ObjectLogEnums.Objects.Customer_Communication.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.PLZ_Postfach != this._data.AdressMailboxZipCode)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "PLZ_Postfach", adressenEntity.PLZ_Postfach, this._data.AdressMailboxZipCode, Enums.ObjectLogEnums.Objects.Customer_Communication.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Postfach != this._data.AdressMailbox)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Postfach", adressenEntity.Postfach, this._data.AdressMailbox, Enums.ObjectLogEnums.Objects.Customer_Communication.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Postfach_bevorzugt != this._data.AdressMailboxIsPreferred)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Postfach_bevorzugt", adressenEntity.Postfach_bevorzugt.ToString(), this._data.AdressMailboxIsPreferred.ToString(), Enums.ObjectLogEnums.Objects.Customer_Communication.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Telefon != this._data.AdressPhoneNumber)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Telefon", adressenEntity.Telefon, this._data.AdressPhoneNumber, Enums.ObjectLogEnums.Objects.Customer_Communication.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Fax != this._data.AdressFaxNumber)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Fax", adressenEntity.Fax, this._data.AdressFaxNumber, Enums.ObjectLogEnums.Objects.Customer_Communication.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.EMail != this._data.AdressEmailAdress)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "EMail", adressenEntity.EMail, this._data.AdressEmailAdress, Enums.ObjectLogEnums.Objects.Customer_Communication.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.WWW != this._data.AdressWebsite)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "WWW", adressenEntity.WWW, this._data.AdressWebsite, Enums.ObjectLogEnums.Objects.Customer_Communication.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Bemerkung != this._data.AdressNote)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Bemerkung", adressenEntity.Bemerkung, this._data.AdressNote, Enums.ObjectLogEnums.Objects.Customer_Communication.GetDescription(), logTypeEdit));
			}
			//
			return logs;
		}
	}
}
