using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Customer.Data
{
	public class UpdateCustomerDataHandler: IHandle<Models.Customer.CustomerDataModel, ResponseModel<int>>
	{
		private Models.Customer.CustomerDataModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateCustomerDataHandler(Identity.Models.UserModel user, Models.Customer.CustomerDataModel data)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				lock(Locks.CostumerEditLock.GetOrAdd(this._data.Id, new object()))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}
					if(this._data.BlockedForFurtherDeliveries.HasValue && !this._data.BlockedForFurtherDeliveries.Value)
					{
						this._data.ReasonForLock = "";
					}
					var logs = LogChanges();
					// save update logs
					if(logs.Count > 0)
					{
						Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
					}
					var kundenEntity = this._data.ToKundenEntity();
					var pszLieferantengruppenEntities = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Get();
					//var actulGroup = int.TryParse(kundenEntity.Kundengruppe, out var val) ? val : 0;
					//var group = pszLieferantengruppenEntities?.Where(x => x.ID == actulGroup).ToList();
					//kundenEntity.Kundengruppe = (group != null && group.Count > 0)?group[0].Lieferantengruppe: kundenEntity.Kundengruppe;
					var response = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Update(kundenEntity);
					return ResponseModel<int>.SuccessResponse(response);
				}
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
						new ResponseModel<int>.ResponseError {Key = "", Value = "Customer not found"}
					}
				};
			}
			if(
				this._data.Industry == null || //string.IsNullOrEmpty(this._data.Industry) || string.IsNullOrWhiteSpace(this._data.Industry) ||
				string.IsNullOrEmpty(this._data.CustomerGroup) || string.IsNullOrWhiteSpace(this._data.CustomerGroup) ||
				string.IsNullOrEmpty(this._data.PaymentMethod) || string.IsNullOrWhiteSpace(this._data.PaymentMethod) ||
				string.IsNullOrEmpty(this._data.ShippingMethod) || string.IsNullOrWhiteSpace(this._data.ShippingMethod) ||
				!this._data.ConditionAssignmentId.HasValue || (this._data.ConditionAssignmentId.HasValue && this._data.ConditionAssignmentId.Value == -1) ||
				!this._data.CurrencyId.HasValue || (this._data.CurrencyId.HasValue && this._data.CurrencyId.Value == -1) ||
				!this._data.SlipCircleId.HasValue || (this._data.SlipCircleId.HasValue && this._data.SlipCircleId.Value == -1) ||
				!this._data.DiscountGroupId.HasValue || (this._data.DiscountGroupId.HasValue && this._data.DiscountGroupId.Value == -1)
				)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Please fill all the required feilds"}
					}
				};
			}
			if(this._data.BlockedForFurtherDeliveries.HasValue && this._data.BlockedForFurtherDeliveries.Value && (this._data.ReasonForLock == null || this._data.ReasonForLock == ""))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "please type a reason for locking the customer"}
					}
				};
			}
			if(this._data.DefaultInteresFromMonitoringLevel < 0 || this._data.DefaultInteresFromMonitoringLevel > 3)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "the Default Interest should be between 0 and 3"}
					}
				};
			}
			// - 2023-11-06
			if(this._data.ConditionAssignmentId != kundenEntity.Konditionszuordnungs_Nr)
			{
				var condition = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(this._data.ConditionAssignmentId ?? -1);
				if(condition is null)
				{
					ResponseModel<int>.FailureResponse("Update aborted: selected Payment Conditions does not exist.");
				}

				var openOrdersCount = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetConfirmationForKonditionUpdate(kundenEntity.Nummer ?? -1, condition.Text);
				if(openOrdersCount > 0)
				{
					var conditionsBody = new Settings.ConditionAssignment.GetConditionAssignmentsHandler(this._user)
				   .Handle();
					string conditionsNew = "", conditionsOld = "";
					if(conditionsBody != null && conditionsBody.Success && conditionsBody.Body?.Count > 0)
					{
						conditionsOld = conditionsBody.Body.FirstOrDefault(x => x.Key == kundenEntity.Konditionszuordnungs_Nr).Value;
						conditionsNew = conditionsBody.Body.FirstOrDefault(x => x.Key == this._data.ConditionAssignmentId).Value;
					}
					return ResponseModel<int>.FailureResponse($"{openOrdersCount} open AB! conditions {(!string.IsNullOrWhiteSpace(conditionsOld) && !string.IsNullOrWhiteSpace(conditionsNew) ? $"from [{conditionsOld}] to [{conditionsNew}]" : "")} cannot be changed for customer.");
				}
			}
			return ResponseModel<int>.SuccessResponse();
		}

		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data.Id);
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Edit;

			if(kundenEntity.RG_Abteilung != this._data.RG_Department)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "RG_Abteilung", kundenEntity.RG_Abteilung, this._data.RG_Department, Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.RG_Land_PLZ_ORT != this._data.RG_Country_ZIPLocation)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "RG_Land_PLZ_ORT", kundenEntity.RG_Land_PLZ_ORT, this._data.RG_Country_ZIPLocation, Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.RG_Strasse_Postfach != this._data.RG_Street_postbox)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "RG_Strasse_Postfach", kundenEntity.RG_Strasse_Postfach, this._data.RG_Street_postbox, Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Preisgruppe != this._data.PriceGroup)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Preisgruppe", kundenEntity.Preisgruppe.ToString(), this._data.PriceGroup.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Preisgruppe2 != this._data.PriceGroup2)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Preisgruppe2", kundenEntity.Preisgruppe2.ToString(), this._data.PriceGroup2.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Rabattgruppe != this._data.DiscountGroupId)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Rabattgruppe", kundenEntity.Rabattgruppe.ToString(), this._data.DiscountGroupId.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Branche != this._data.Industry)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Branche", kundenEntity.Branche, this._data.Industry, Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Kundengruppe != this._data.CustomerGroup)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Kundengruppe", kundenEntity.Kundengruppe, this._data.CustomerGroup, Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Lieferantenummer__Kunden_ != this._data.SupplierNumber_customers)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Lieferantenummer_Kunden", kundenEntity.Lieferantenummer__Kunden_, this._data.SupplierNumber_customers, Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Zahlungsweise != this._data.PaymentMethod)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Zahlungsweise", kundenEntity.Zahlungsweise, this._data.PaymentMethod, Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Versandart != this._data.ShippingMethod)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Versandart", kundenEntity.Versandart, this._data.ShippingMethod, Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Konditionszuordnungs_Nr != this._data.ConditionAssignmentId)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Konditionszuordnungs_Nr", kundenEntity.Konditionszuordnungs_Nr.ToString(), this._data.ConditionAssignmentId.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Zahlungskondition != this._data.TermsOfPaymentId)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Zahlungskondition", kundenEntity.Zahlungskondition.ToString(), this._data.TermsOfPaymentId.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Factoring != this._data.Factoring)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Factoring", kundenEntity.Factoring.ToString(), this._data.Factoring.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Debitoren_Nr != this._data.DebtorsNr)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Debitoren_Nr", kundenEntity.Debitoren_Nr, this._data.DebtorsNr, Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.fibu_rahmen != this._data.FibuFrame)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Fibu_rahmen", kundenEntity.fibu_rahmen.ToString(), this._data.FibuFrame.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Umsatzsteuer_berechnen != this._data.CalculateSalesTax)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Umsatzsteuer_berechnen", kundenEntity.Umsatzsteuer_berechnen.ToString(), this._data.CalculateSalesTax.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.EG___Identifikationsnummer != this._data.EG_IdentificationNumber)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "EG___Identifikationsnummer", kundenEntity.EG___Identifikationsnummer, this._data.EG_IdentificationNumber, Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Bruttofakturierung != this._data.GrossInvoicing)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Bruttofakturierung", kundenEntity.Bruttofakturierung.ToString(), this._data.GrossInvoicing.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Währung != this._data.CurrencyId)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Währung", kundenEntity.Währung.ToString(), this._data.CurrencyId.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Belegkreis != this._data.SlipCircleId)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Belegkreis", kundenEntity.Belegkreis.ToString(), this._data.SlipCircleId.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.zolltarif_nr != this._data.CustomerFee_Nr)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "zolltarif_nr", kundenEntity.zolltarif_nr.ToString(), this._data.CustomerFee_Nr.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Mahngebühr_1 != this._data.DunningFee_1)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Mahngebühr_1", kundenEntity.Mahngebühr_1.ToString(), this._data.DunningFee_1.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Mahngebühr_2 != this._data.DunningFee_2)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Mahngebühr_2", kundenEntity.Mahngebühr_2.ToString(), this._data.DunningFee_2.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Mahngebühr_3 != this._data.DunningFee_3)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Mahngebühr_3", kundenEntity.Mahngebühr_3.ToString(), this._data.DunningFee_3.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Zahlung_erwartet_nach != this._data.PaymentExpectedAfter)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Zahlung_erwartet_nach", kundenEntity.Zahlung_erwartet_nach.ToString(), this._data.PaymentExpectedAfter.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Verzugszinsen != this._data.LatePaymentInterest)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Verzugszinsen", kundenEntity.Verzugszinsen.ToString(), this._data.LatePaymentInterest.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Verzugszinsen_ab_Mahnstufe != this._data.DefaultInteresFromMonitoringLevel)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Verzugszinsen_ab_Mahnstufe", kundenEntity.Verzugszinsen_ab_Mahnstufe.ToString(), this._data.DefaultInteresFromMonitoringLevel.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Grund_für_Sperre != this._data.ReasonForLock)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Grund_für_Sperre", kundenEntity.Grund_für_Sperre, this._data.ReasonForLock, Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Zahlungsmoral != this._data.PaymentPractices)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Zahlungsmoral", kundenEntity.Zahlungsmoral.ToString(), this._data.PaymentPractices.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Kreditlimit != this._data.CreditLimit)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Kreditlimit", kundenEntity.Kreditlimit.ToString(), this._data.CreditLimit.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.OPOS != this._data.OPOS)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "OPOS", kundenEntity.OPOS.ToString(), this._data.OPOS.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Mahnsperre != this._data.DunningBlock)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Mahnsperre", kundenEntity.Mahnsperre.ToString(), this._data.DunningBlock.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.Karenztage != this._data.WaitingDays)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "Karenztage", kundenEntity.Karenztage.ToString(), this._data.WaitingDays.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}

			if(kundenEntity.gesperrt_für_weitere_Lieferungen != this._data.BlockedForFurtherDeliveries)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.Id, "gesperrt_für_weitere_Lieferungen", kundenEntity.gesperrt_für_weitere_Lieferungen.ToString(), this._data.BlockedForFurtherDeliveries.ToString(), Enums.ObjectLogEnums.Objects.Customer_Data.GetDescription(), logTypeEdit));
			}
			//
			return logs;
		}
	}
}
