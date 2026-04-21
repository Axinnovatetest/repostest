using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer.Shipping
{
	public class UpdateCustomerShippingHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Customer.CustomerShippingModel _data { get; set; }

		public UpdateCustomerShippingHandler(Models.Customer.CustomerShippingModel data, Identity.Models.UserModel user)
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

				var kundennEntity = this._data.ToKundenEntity();
				var adressenEntity = this._data.ToAdressenEntity();
				var logs = LogChanges();
				// save update logs
				if(logs.Count > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
				}
				Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateShipping(adressenEntity);
				Infrastructure.Data.Access.Tables.PRS.KundenAccess.UpdateShipping(kundennEntity);
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
			return ResponseModel<int>.SuccessResponse();
		}

		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)this._data.AdressId);
			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data.Id);
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Edit;
			//
			if(adressenEntity.Montag_Anliefertag != this._data.MondayIsDeliveryDay)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Montag_Anliefertag", adressenEntity.Montag_Anliefertag.ToString(), this._data.MondayIsDeliveryDay.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Freitag_Anliefertag != this._data.FridayIsDeliveryDay)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Freitag_Anliefertag", adressenEntity.Freitag_Anliefertag.ToString(), this._data.FridayIsDeliveryDay.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Dienstag_Anliefertag != this._data.TuesdayIsDeliveryDay)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Dienstag_Anliefertag", adressenEntity.Dienstag_Anliefertag.ToString(), this._data.TuesdayIsDeliveryDay.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Donnerstag_Anliefertag != this._data.ThursdayIsDeliveryDay)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Donnerstag_Anliefertag", adressenEntity.Donnerstag_Anliefertag.ToString(), this._data.ThursdayIsDeliveryDay.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Mittwoch_Anliefertag != this._data.WednesdayIsDeliveryDay)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Mittwoch_Anliefertag", adressenEntity.Mittwoch_Anliefertag.ToString(), this._data.WednesdayIsDeliveryDay.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}
			//
			if(kundenEntity.LSADR != this._data.LSADR)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "LSADR", kundenEntity.LSADR.ToString(), this._data.LSADR.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.LSADRANG != this._data.LSADRANG)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "LSADRANG", kundenEntity.LSADRANG.ToString(), this._data.LSADRANG.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}
			if(kundenEntity.LSADRAUF != this._data.LSADRAUF)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "LSADRAUF", kundenEntity.LSADRAUF.ToString(), this._data.LSADRAUF.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}
			if(kundenEntity.LSADRGUT != this._data.LSADRGUT)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "LSADRGUT", kundenEntity.LSADRGUT.ToString(), this._data.LSADRGUT.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.LSADRPROF != this._data.LSADRPROF)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "LSADRPROF", kundenEntity.LSADRPROF.ToString(), this._data.LSADRPROF.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}
			if(kundenEntity.LSADRRG != this._data.LSADRRG)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "LSADRRG", kundenEntity.LSADRRG.ToString(), this._data.LSADRRG.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}
			if(kundenEntity.LSADRSTO != this._data.LSADRSTO)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "LSADRSTO", kundenEntity.LSADRSTO.ToString(), this._data.LSADRSTO.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.LSRG != this._data.LSRG)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "LSRG", kundenEntity.LSRG.ToString(), this._data.LSRG.ToString(), Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Standardversand != this._data.StandardShipping)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Standardversand", kundenEntity.Standardversand, this._data.StandardShipping, Enums.ObjectLogEnums.Objects.Customer_Shipping.GetDescription(), logTypeEdit));
			}
			//
			return logs;
		}
	}
}
