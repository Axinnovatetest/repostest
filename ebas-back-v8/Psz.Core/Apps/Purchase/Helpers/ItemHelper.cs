using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Helpers
{
	public class ItemHelper
	{
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity GetItemByOrderIds(string itemNumber,
			string customerItemNumber)
		{
			var itemDb = !string.IsNullOrEmpty(itemNumber)
				? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(itemNumber)
				 : null;

			if(itemDb == null || itemDb.Freigabestatus == "O")
			{
				itemDb = !string.IsNullOrEmpty(customerItemNumber)
					? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByBezeichnung1(new List<string>() { customerItemNumber?.TrimStart('0') }).FirstOrDefault()
					: null;
			}

			if(itemDb == null || itemDb.Freigabestatus == "O")
			{
				return null;
			}

			return itemDb;
		}

		public static bool CanDeleteOrder(int id, int angeboteNr, out string errorMessage)
		{
			errorMessage = "";
			var ab = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(id);
			if(ab == null)
			{
				return true;
			}
			if(ab.Gebucht == true)
			{
				errorMessage = $"Order is booked";
				return false;
			}
			if(ab.Erledigt == true)
			{
				errorMessage = $"Order is complete";
				return false;
			}
			var ls = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetDeliveryNotesByAB(ab.Nr);
			if(ls != null && ls.Count > 0)
			{
				errorMessage = $"Order has LS [{string.Join(", ", ls.Take(5).Select(x => x.Angebot_Nr ?? 0))}]";
				return false;
			}
			var re = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetInvoiceByAB(ab.Nr);
			if(re != null && re.Count > 0)
			{
				errorMessage = $"Order has Rechnung [{string.Join(", ", re.Take(5).Select(x => x.Angebot_Nr ?? 0))}]";
				return false;
			}
			var gs = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCreditByAB(ab.Nr);
			if(gs != null && gs.Count > 0)
			{
				errorMessage = $"Order has Gutschrifte [{string.Join(", ", gs.Take(5).Select(x => x.Angebot_Nr ?? 0))}]";
				return false;
			}

			var fas = angeboteNr == 0 ? null : Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByAngeboteNr(id);
			if(fas != null && fas.Count > 0)
			{
				errorMessage = $"Order has FAs [{string.Join(", ", fas.Take(5).Select(x => x.Fertigungsnummer ?? 0))}]";
				return false;
			}
			var pos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(id);
			if(pos != null && pos.Count > 0)
			{
				errorMessage = $"Order has {pos.Count} positions, please remove before deleting order";
				return false;
			}
			var canDelete1 = (pos == null || pos.Count <= 0) || Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.CanDelete(id);
			var canDelete2 = (fas == null || fas.Count <= 0);

			// -
			return true;
		}
		public static bool CanArchiveOrderByAngebote(int? id)
		{
			if(!id.HasValue)
				return true;

			var fas = id.Value == 0 ? null : Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByAngeboteNr(id.Value);
			return fas == null || fas.Count <= 0;
		}

		public static int GetVorfallNrFromRange(Enums.OrderEnums.Types type, string user)
		{
			int FinalVofallNr = 0;
			int MaxCurrentValue = 0;
			int MinNewValue = 0;
			int MaxNewValue = 0;
			switch(type)
			{
				case Enums.OrderEnums.Types.Confirmation:
					MaxCurrentValue = Program.CTS.abMaxCurrentValue;
					MinNewValue = Program.CTS.abMinNewValue;
					MaxNewValue = Program.CTS.abMaxNewValue;
					break;
				case Enums.OrderEnums.Types.Forecast:
					break;
				case Enums.OrderEnums.Types.Contract:
					MaxCurrentValue = Program.CTS.raMaxCurrentValue;
					MinNewValue = Program.CTS.raMinNewValue;
					MaxNewValue = Program.CTS.raMaxNewValue;
					break;
				case Enums.OrderEnums.Types.Kanban:
					break;
				case Enums.OrderEnums.Types.Delivery:
					MaxCurrentValue = Program.CTS.lsMaxCurrentValue;
					MinNewValue = Program.CTS.lsMinNewValue;
					MaxNewValue = Program.CTS.lsMaxNewValue;
					break;
				case Enums.OrderEnums.Types.Invoice:
					MaxCurrentValue = Program.CTS.reMaxCurrentValue;
					MinNewValue = Program.CTS.reMinNewValue;
					MaxNewValue = Program.CTS.reMaxNewValue;
					break;
				case Enums.OrderEnums.Types.Credit:
					MaxCurrentValue = Program.CTS.gsMaxCurrentValue;
					MinNewValue = Program.CTS.gsMinNewValue;
					MaxNewValue = Program.CTS.gsMaxNewValue;
					break;
				default:
					break;
			}
			var maxNr = Handlers.Order.getNextAngebotNr(type);
			if((int.TryParse(maxNr, out var val) ? val : 0) < MaxCurrentValue)
				FinalVofallNr = int.TryParse(maxNr, out var val2) ? val2 : 0;
			else
				FinalVofallNr = MinNewValue;
			//alert max reached
			if(FinalVofallNr == MaxNewValue - Program.CTS.Delta)
			{
				string title = "MAX VALUE VORFALL NR LS REACHED";
				var addresses = new List<string>();
				addresses.Add("Mohamed.Souilmi@psz-electronic.com");
				addresses.Add(Program.EmailingService.EmailParamtersModel.AdminEmail);
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
				+ $"<br/><span style='font-size:1.15em;'><strong>{user.ToUpper()}</strong> has reached the naximum range in vorfall nr in {type} creation [{FinalVofallNr}]</strong>."
				+ $"</span><br/><br/>The change is applyed and Logged"
				+ "<br/><br/>Regards, <br/>IT Department </div>";

				Program.EmailingService.SendEmailAsync(title, content, addresses);
			}
			return FinalVofallNr;
		}
	}
}
