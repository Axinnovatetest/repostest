using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier.Data
{
	public class UpdateSupplierDataHandler: IHandle<Models.Supplier.SupplierDataModel, ResponseModel<int>>
	{
		private Models.Supplier.SupplierDataModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateSupplierDataHandler(Identity.Models.UserModel user, Models.Supplier.SupplierDataModel data)
		{
			this._data = data;
			this._user = user;
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
				if(!this._data.BlockedForFurtherOrders)
				{
					this._data.ReasonForBlocking = "";
				}
				var logs = LogChanges();
				// save update logs
				if(logs.Count > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
				}
				var lieferantenEntity = this._data.ToEntity();
				//var pszLieferantengruppenEntities = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Get();
				//var actulGroup = int.TryParse(lieferantenEntity.Lieferantengruppe, out var val) ? val : 0;
				//var group = pszLieferantengruppenEntities?.Where(x => x.ID == actulGroup).ToList();
				//lieferantenEntity.Lieferantengruppe = (group != null && group.Count > 0) ? group[0].Lieferantengruppe : lieferantenEntity.Lieferantengruppe;
				var response = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.UpdateData(lieferantenEntity);
				return ResponseModel<int>.SuccessResponse(response);
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
			var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.Id);
			if(lieferantenEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Supplier not found"}
					}
				};
			}
			if(string.IsNullOrEmpty(this._data.Industry) || string.IsNullOrWhiteSpace(this._data.Industry) ||
				string.IsNullOrEmpty(this._data.SuppliersGroup) || string.IsNullOrWhiteSpace(this._data.SuppliersGroup) ||
				string.IsNullOrEmpty(this._data.PaymentMethod) || string.IsNullOrWhiteSpace(this._data.PaymentMethod) ||
				!this._data.ConditionAssignmentNumber.HasValue || (this._data.ConditionAssignmentNumber.HasValue && this._data.ConditionAssignmentNumber.Value == -1) ||
				!this._data.CurrencyId.HasValue || (this._data.CurrencyId.HasValue && this._data.CurrencyId.Value == -1)
				)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Please fill all the required fields"}
					}
				};
			}

			if(this._data.BlockedForFurtherOrders && (this._data.ReasonForBlocking == null || this._data.ReasonForBlocking == ""))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Please put reason for blocking the supplier"}
					}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}

		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var lieferentenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.Id);
			var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)this._data.Number);
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Edit;


			var Bestellimit = float.TryParse(this._data.OrderLimit.ToString(), out var val4) ? val4 : 0;
			var Mindestbestellwert = float.TryParse(this._data.MinimumValue.ToString(), out var val6) ? val6 : 0;
			var Zielaufschlag = float.TryParse(this._data.TargetImpact.ToString(), out var val1) ? val1 : 0;
			var Zuschlag_Mindestbestellwert = float.TryParse(this._data.SurchargeMinimumOrderValue.ToString(), out var val3) ? val3 : 0;

			if(lieferentenEntity.Zuschlag_Mindestbestellwert != Zuschlag_Mindestbestellwert)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Zuschlag_Mindestbestellwert", lieferentenEntity.Zuschlag_Mindestbestellwert.ToString(), Zuschlag_Mindestbestellwert.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Zielaufschlag != Zielaufschlag)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Zielaufschlag", lieferentenEntity.Zielaufschlag.ToString(), Zielaufschlag.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Mindestbestellwert != Mindestbestellwert)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Mindestbestellwert", lieferentenEntity.Mindestbestellwert.ToString(), Mindestbestellwert.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}


			if(lieferentenEntity.Bestellimit != Bestellimit)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Bestellimit", lieferentenEntity.Bestellimit.ToString(), Bestellimit.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}


			if(lieferentenEntity.Mahnsperre != this._data.Dunning)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Mahnsperre", lieferentenEntity.Mahnsperre.ToString(), this._data.Dunning.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Grund_fur_Sperre != this._data.ReasonForBlocking)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Grund_fur_Sperre", lieferentenEntity.Grund_fur_Sperre.ToString(), this._data.ReasonForBlocking.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Grund_fur_Sperre != this._data.ReasonForBlocking)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Grund_fur_Sperre", lieferentenEntity.Grund_fur_Sperre.ToString(), this._data.ReasonForBlocking.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Gesperrt_fur_weitere_Bestellungen != this._data.BlockedForFurtherOrders)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Gesperrt_fur_weitere_Bestellungen", lieferentenEntity.Gesperrt_fur_weitere_Bestellungen.ToString(), this._data.BlockedForFurtherOrders.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Rabattgruppe != this._data.DiscountGroupId)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Rabattgruppe", lieferentenEntity.Rabattgruppe.ToString(), this._data.DiscountGroupId.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Branche != this._data.Industry)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Branche", lieferentenEntity.Branche, this._data.Industry, Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Lieferantengruppe != this._data.SuppliersGroup)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Lieferantengruppe", lieferentenEntity.Lieferantengruppe, this._data.SuppliersGroup, Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Zahlungsweise != this._data.PaymentMethod)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Zahlungsweise", lieferentenEntity.Zahlungsweise, this._data.PaymentMethod, Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Konditionszuordnungs_Nr != this._data.ConditionAssignmentNumber)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Konditionszuordnungs_Nr", lieferentenEntity.Konditionszuordnungs_Nr.ToString(), this._data.ConditionAssignmentNumber.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Umsatzsteuer_berechnen != this._data.CalculateSalesTax)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Umsatzsteuer_berechnen", lieferentenEntity.Umsatzsteuer_berechnen.ToString(), this._data.CalculateSalesTax.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Wahrung != this._data.CurrencyId)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Wahrung", lieferentenEntity.Wahrung.ToString(), this._data.CurrencyId.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}

			if(lieferentenEntity.Belegkreis != this._data.SlipCircleId)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Belegkreis", lieferentenEntity.Belegkreis.ToString(), this._data.SlipCircleId.ToString(), Enums.ObjectLogEnums.Objects.Supplier_Data.GetDescription(), logTypeEdit));
			}
			//
			return logs;
		}
	}
}
