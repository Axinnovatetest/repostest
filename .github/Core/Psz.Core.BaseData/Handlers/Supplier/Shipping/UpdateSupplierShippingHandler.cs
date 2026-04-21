using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier.Shipping
{
	public class UpdateSupplierShippingHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Supplier.SupplierShippingModel _data { get; set; }

		public UpdateSupplierShippingHandler(Models.Supplier.SupplierShippingModel data, Identity.Models.UserModel user)
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

				var adressenEntity = this._data.ToAdressenEntity();
				var leferantenEntity = this._data.ToLieferantenEntity();
				var logs = LogChanges();
				// save update logs
				if(logs.Count > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
				}
				Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateShipping(adressenEntity);
				Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.UpdateShipping(leferantenEntity);
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

			var lieferantnEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.Id);
			if(lieferantnEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Supplier not found"}
					}
				};
			}
			if(string.IsNullOrEmpty(this._data.ShippingMethod) || string.IsNullOrWhiteSpace(this._data.ShippingMethod))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Shipping Method is required"}
					}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}

		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)this._data.AdressId);
			var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.Id);
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Edit;
			//
			var Versandkosten = float.TryParse(this._data.ShippingCosts.ToString(), out var val) ? val : 0;
			var ExpressSurcharge = Convert.ToDecimal(lieferantenEntity.Eilzuschlag ?? 0);
			var FreightAllowance = Convert.ToDecimal(lieferantenEntity.Frachtfreigrenze ?? 0);
			if(lieferantenEntity.Versandkosten != Versandkosten)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Versandkosten", lieferantenEntity.Versandkosten.ToString(), Versandkosten.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Shipping.GetDescription(), logTypeEdit));
			}

			if(lieferantenEntity.Wochentag_Anlieferung != this._data.ShippingWeekDay)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Wochentag_Anlieferung", lieferantenEntity.Wochentag_Anlieferung.ToString(), this._data.ShippingWeekDay.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Shipping.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Montag_Anliefertag != this._data.MondayIsDeliveryDay)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Montag_Anliefertag", adressenEntity.Montag_Anliefertag.ToString(), this._data.MondayIsDeliveryDay.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Shipping.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Freitag_Anliefertag != this._data.FridayIsDeliveryDay)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Freitag_Anliefertag", adressenEntity.Freitag_Anliefertag.ToString(), this._data.FridayIsDeliveryDay.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Shipping.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Dienstag_Anliefertag != this._data.TuesdayIsDeliveryDay)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Dienstag_Anliefertag", adressenEntity.Dienstag_Anliefertag.ToString(), this._data.TuesdayIsDeliveryDay.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Shipping.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Donnerstag_Anliefertag != this._data.ThursdayIsDeliveryDay)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Donnerstag_Anliefertag", adressenEntity.Donnerstag_Anliefertag.ToString(), this._data.ThursdayIsDeliveryDay.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Shipping.GetDescription(), logTypeEdit));
			}

			if(adressenEntity.Mittwoch_Anliefertag != this._data.WednesdayIsDeliveryDay)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Mittwoch_Anliefertag", adressenEntity.Mittwoch_Anliefertag.ToString(), this._data.WednesdayIsDeliveryDay.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Shipping.GetDescription(), logTypeEdit));
			}

			if(lieferantenEntity.Versandart != this._data.ShippingMethod)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Versandart", lieferantenEntity.Versandart.ToString(), this._data.ShippingMethod.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Shipping.GetDescription(), logTypeEdit));
			}

			if(ExpressSurcharge != this._data.ExpressSurcharge)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Eilzuschlag", ExpressSurcharge.ToString(), this._data.ExpressSurcharge.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Shipping.GetDescription(), logTypeEdit));
			}

			if(FreightAllowance != this._data.FreightAllowance)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Frachtfreigrenze", FreightAllowance.ToString(), this._data.FreightAllowance.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Shipping.GetDescription(), logTypeEdit));
			}
			//
			return logs;
		}
	}
}
