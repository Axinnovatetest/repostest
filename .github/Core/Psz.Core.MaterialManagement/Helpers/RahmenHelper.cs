
namespace Psz.Core.MaterialManagement.Helpers
{
	public class RahmenHelper
	{
		//public static void StatusEmailNotification(Enums.BlanketEnums.ActionStatus action, Identity.Models.UserModel _user, string reason, int RahmenId)
		//{
		//	try
		//	{
		//		var body = "";
		//		var content = "";
		//		var title = "";
		//		var addresses = new List<string>();
		//		var RahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(RahmenId);
		//		var RahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(RahmenId);

		//		#region // - 2022-12-01 - send Mail to Assigned Employee
		//		var assignedEmployeeAddresses = new List<string>();
		//		var pCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_CustomerNumber(true, RahmenExtensionEntity.CustomerId ?? -1);
		//		var npCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_CustomerNumber(false, RahmenExtensionEntity.CustomerId ?? -1);
		//		if(npCustomersNumbers != null)
		//		{
		//			if(DateTime.Now >= npCustomersNumbers.ValidFromTime.Date && DateTime.Now <= npCustomersNumbers.ValidIntoTime.Date.AddDays(1))
		//			{
		//				if(pCustomersNumbers != null)
		//				{
		//					assignedEmployeeAddresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(npCustomersNumbers.UserId)?.Email ?? "");
		//				}
		//			}
		//			else
		//			{
		//				npCustomersNumbers = null;
		//			}
		//		}
		//		if(pCustomersNumbers != null)
		//		{
		//			assignedEmployeeAddresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(pCustomersNumbers.UserId)?.Email ?? "");
		//		}
		//		#endregion

		//		switch(action)
		//		{
		//			case Enums.BlanketEnums.ActionStatus.SubmitValidate:
		//				body = Template("request_validation", RahmenEntity, null, _user.Name);
		//				title = $"Validation Request – Rahmen | Document No. {RahmenEntity.Bezug}";
		//				addresses.AddRange(
		//				Infrastructure.Data.Access.Tables.COR.UserAccess.Get(
		//			Infrastructure.Data.Access.Tables.CTS.AccessProfileUsersAccess.GetByAccessProfileIds(
		//  Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.GetAdminBlanket()?.Select(x => x.Id)?.ToList())?.Select(x => x.UserId)?.ToList())?.Select(x => x.Email)?.ToList());
		//				break;
		//			case Enums.BlanketEnums.ActionStatus.Valider:
		//				body = Template("confirm_validation", RahmenEntity, null, _user.Name);
		//				title = $"Confirmation of Validation – Rahmen | Document No. {RahmenEntity.Bezug}";
		//				addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
		//				// - 2022-12-01 send Mail to Assigned Employee
		//				if(assignedEmployeeAddresses.Count > 0)
		//				{
		//					addresses.AddRange(assignedEmployeeAddresses);
		//				}
		//				break;
		//			case Enums.BlanketEnums.ActionStatus.Rejeter:
		//				body = Template("reject_validation", RahmenEntity, "rejected", _user.Name);
		//				title = $"Cancellation Notice – Rahmen | Document No. {RahmenEntity.Bezug}";
		//				addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
		//				// - 2022-12-01 send Mail to Assigned Employee
		//				if(assignedEmployeeAddresses.Count > 0)
		//				{
		//					addresses.AddRange(assignedEmployeeAddresses);
		//				}
		//				break;
		//			case Enums.BlanketEnums.ActionStatus.Fermer:
		//				body = Template("closing", RahmenEntity, null, _user.Name);
		//				title = $"Closing Notice – Rahmen | Document No. {RahmenEntity.Bezug}";
		//				addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
		//				// - 2022-12-01 send Mail to Assigned Employee
		//				if(assignedEmployeeAddresses.Count > 0)
		//				{
		//					addresses.AddRange(assignedEmployeeAddresses);
		//				}
		//				break;
		//			case Enums.BlanketEnums.ActionStatus.Annuler:
		//				body = Template("reject_validation", RahmenEntity, "cancelled", _user.Name);
		//				title = $"Cancellation Notice – Rahmen | Document No. {RahmenEntity.Bezug}";
		//				addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
		//				// - 2022-12-01 send Mail to Assigned Employee
		//				if(assignedEmployeeAddresses.Count > 0)
		//				{
		//					addresses.AddRange(assignedEmployeeAddresses);
		//				}
		//				break;
		//			default:
		//				break;
		//		}
		//		//content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
		//		//   + $"<span style='font-size:1.5em;'> Good {(DateTime.Now.Hour <= 12 ? "Morning" : " Afternoon")},</span><br/>"
		//		//   + $"{body}"
		//		//   + $"</span><br/><br/>"
		//		//   + "</div>";
		//		//content += "<br/><br/>";
		//		//content += $"<br/><span style='font-size:1em;'>Sincerely,</span>";
		//		//content += $"<br/><span style='font-size:1em;font-weight:bold'>IT Department </span></br>";
		//		Module.EmailingService.SendEmailAsync(title, content, addresses, null);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		throw;
		//	}
		//}
	}
}
