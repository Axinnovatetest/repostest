using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> ChangeCustomer(Models.Order.ChangeCustomerModel data,
		   Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null/* || !user.Access.EDI.Order*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return ChangeCustomer(data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static Core.Models.ResponseModel<object> ChangeCustomer(Models.Order.ChangeCustomerModel data)
		{
			lock(Locks.OrdersLock)
			{
				try
				{
					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data.OrderId);
					if(orderDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order not found" }
						};
					}

					var newCustomerDb = orderDb.Kunden_Nr.HasValue
						? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(orderDb.Kunden_Nr.Value)
						: null;
					if(newCustomerDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Customer not found" }
						};
					}

					var adressDb = newCustomerDb.Nummer.HasValue
						? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(newCustomerDb.Nummer.Value)
						: null;
					if(adressDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Address not found" }
						};
					}

					// > 
					var conditionAssignementTableDb = newCustomerDb.Konditionszuordnungs_Nr.HasValue
						? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(newCustomerDb.Konditionszuordnungs_Nr.Value)
						: null;

					var mailBoxIsPreferred = adressDb.Postfach_bevorzugt == true;

					orderDb.ABSENDER = adressDb.Name1;
					orderDb.Kunden_Nr = adressDb.Nr;
					orderDb.EDI_Order_Neu = true;
					orderDb.Vorname_NameFirma = adressDb.Name1;
					orderDb.Name2 = adressDb.Name2;
					orderDb.Name3 = adressDb.Name3;
					orderDb.Ansprechpartner = adressDb.Abteilung;
					orderDb.Abteilung = adressDb.Abteilung;
					orderDb.Straße_Postfach = mailBoxIsPreferred
								? $"Postfach {adressDb.Postfach}"
								: $"{adressDb.PLZ_StraBe} {adressDb.StraBe}";
					orderDb.Land_PLZ_Ort = mailBoxIsPreferred
								? $"{adressDb.PLZ_Postfach} {adressDb.Ort}"
								: $"{adressDb.PLZ_StraBe} {adressDb.StraBe}, {adressDb.Ort} {adressDb.Postfach}";
					orderDb.Versandart = newCustomerDb.Versandart;
					orderDb.Zahlungsweise = newCustomerDb.Zahlungsweise;
					orderDb.Konditionen = conditionAssignementTableDb?.Text;
					orderDb.Unser_Zeichen = adressDb.Kundennummer.HasValue ? adressDb.Kundennummer.ToString() : "";
					orderDb.Ihr_Zeichen = newCustomerDb.Lieferantenummer__Kunden_;
					orderDb.USt_Berechnen = newCustomerDb.Umsatzsteuer_berechnen;
					orderDb.Falligkeit = DateTime.Now.AddDays(+30);
					orderDb.Datum = DateTime.Now;
					orderDb.Briefanrede = adressDb.Briefanrede;
					orderDb.Personal_Nr = 0;
					orderDb.Freitext = $"USt - ID - Nr.: {newCustomerDb.EG___Identifikationsnummer}";
					orderDb.LAnrede = adressDb.Anrede;
					orderDb.LVorname_NameFirma = adressDb.Name1;
					orderDb.LName2 = adressDb.Name2;
					orderDb.LName3 = adressDb.Name3;
					orderDb.LAnsprechpartner = adressDb.Abteilung;
					orderDb.LAbteilung = adressDb.Abteilung;
					orderDb.LStraße_Postfach = $"{adressDb.PLZ_StraBe} {adressDb.StraBe} {adressDb.Postfach}";
					orderDb.LLand_PLZ_Ort = $"{adressDb.PLZ_StraBe} {adressDb.StraBe} {adressDb.Postfach}, {adressDb.Ort}";
					orderDb.LBriefanrede = adressDb.Briefanrede;

					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(orderDb);

					return Core.Models.ResponseModel<object>.SuccessResponse();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}
