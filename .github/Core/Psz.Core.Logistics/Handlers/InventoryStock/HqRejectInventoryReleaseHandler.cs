using Psz.Core.Identity.Models;
using Infrastructure.Data.Entities.Tables.Logistics.InventroyStock;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class HqRejectInventoryReleaseHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly KeyValuePair< int, string> _data;
		public HqRejectInventoryReleaseHandler(UserModel user, KeyValuePair<int, string> data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<int> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();
				bool sendRejectEmail = false;
				var id = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.HQRejectInventoryRelease(_data.Key, _user.Id, _user.Username, _data.Value, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.RevertInventoryRelease(_data.Key, botransaction.connection, botransaction.transaction);

				// - send email to Warehouse Approver to check & retry again 
				sendRejectEmail = true;

				#region add logs
				Infrastructure.Data.Access.Tables.Logistics.InventoryStock.LogsAccess.InsertWithTransaction(new LogsEntity
				{
					LogTime = DateTime.Now,
					LogUserId = _user.Id,
					ObjectId = _data.Key,
					ObjectName = "InventoryStock",
					LogDescription = $"The inventory stock has been [HQ] rejected in Lager [{_data.Key}] at [{DateTime.Now:yyyy-MM-dd HH:mm}] by [{_user.Name}]",
					LogsType = 2,
					LogUserName = _user.Name,
					LagerId = _data.Key
				}, botransaction.connection, botransaction.transaction);
				#endregion add logs

				if(botransaction.commit())
				{
					if(sendRejectEmail)
					{
						// - do it :-P
						sendRejectionEmail();
					}
					return ResponseModel<int>.SuccessResponse(id);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch
			{
				botransaction.rollback();
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(_user == null || (!_user.SuperAdministrator && !_user.IsGlobalDirector && !_user.Access.Logistics.InventoryHqValidate))
				return ResponseModel<int>.AccessDeniedResponse();

			return ResponseModel<int>.SuccessResponse();
		}
		void sendRejectionEmail()
		{
			try
			{
				var inventoryStats = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.GetByWarehouse(_data.Key);
				var requester = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(inventoryStats?.WarehouseValidatorId ?? -2);
				var warehouse = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data.Key);
				var title = "[EBAS] Inventory Rejection";
				var content = $@"";
				content += $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>";
				content += $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/><br/>";
				content += $"</div>";
				content += $"<div style='font-family: Arial, sans-serif; font-size: 14px; color: #333333; line-height: 1.5;'>";
				//content += $"<div style='margin-bottom: 10px;'>Inventory Release Request Rejected</div>";
				content += $"<div style='margin-bottom: 10px;'>The following inventory release request has been <strong>rejected</strong>:</div>";
				content += $"<table cellpadding='4' cellspacing='0' style='border-collapse: collapse; margin-top: 8px; margin-bottom: 16px;'>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Requester:</td>";
				content += $"<td>{requester?.Name}</td>";
				content += $"</tr>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Site:</td>";
				content += $"<td>{warehouse?.Lagerort}</td>";
				content += $"</tr>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Submitted At:</td>";
				content += $"<td>{inventoryStats.WarehouseValidatorValidateTime:dd.MM.yyyy HH:mm}</td>";
				content += $"</tr>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Rejected By:</td>";
				content += $"<td>{_user.Name}</td>";
				content += $"</tr>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Rejection Time:</td>";
				content += $"<td>{DateTime.Now:dd.MM.yyyy HH:mm}</td>";
				content += $"</tr>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Rejection Comments:</td>";
				content += $"<td>{_data.Value}</td>";
				content += $"</tr>";
				content += $"</table>";
				content += $"<div style='margin-bottom: 10px;'>If applicable, please review the comments or corrective actions provided in the system.</div>";
				//content += $"<div style='margin-bottom: 10px;'>";
				//content += $"You can view the request details using the link below:";
				//content += $"</div>";
				//content += $"<div style='margin-bottom: 10px;'>";
				//content += $"<a href='/inventory-stock' style='display: inline-block; text-decoration: none; padding: 8px 16px; border-radius: 4px; border: 1px solid #333333;'>";
				//content += $"View Inventory Release";
				//content += $"</a>";
				//content += $"</div>";
				content += $"<div style='margin-top: 16px; font-size: 12px; color: #777777;'>";
				content += $"This is an automated notification. Please do not reply to this email.";
				content += $"</div>";
				content += $"</div>";
				var destinations = new List<string> { requester?.Email };
				Module.EmailingService.SendEmailAsync(title, content, destinations);
			} catch
			{
				throw;
			}
		}
	}
}
