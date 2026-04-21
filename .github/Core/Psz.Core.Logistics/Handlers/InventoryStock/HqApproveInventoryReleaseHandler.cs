
using Infrastructure.Data.Entities.Tables.Logistics.InventroyStock;
using Psz.Core.Identity.Models;
using SendGrid.Helpers.Mail;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class HqApproveInventoryReleaseHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly int _data;
		public HqApproveInventoryReleaseHandler(UserModel user, int data)
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
				bool sendEmailNotification = false;
				var id = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.HQApproveInventoryRelease(_data, _user.Id, _user.Username, botransaction.connection, botransaction.transaction);
				sendEmailNotification = true;
				#region add logs
				Infrastructure.Data.Access.Tables.Logistics.InventoryStock.LogsAccess.InsertWithTransaction(new LogsEntity
				{
					LogTime = DateTime.Now,
					LogUserId = _user.Id,
					ObjectId = _data,
					ObjectName = "InventoryStock",
					LogDescription = $"The inventory stock has been [HQ] validated in Lager [{_data}] at [{DateTime.Now:yyyy-MM-dd HH:mm}] by [{_user.Name}]",
					LogsType = 2,
					LogUserName = _user.Name,
					LagerId = _data
				}, botransaction.connection, botransaction.transaction);
				#endregion add logs

				if(botransaction.commit())
				{
					if(sendEmailNotification)
					{
						// - 
						sendApprovalEmail();
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
		void sendApprovalEmail()
		{
			try
			{
				var inventoryStats = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.GetByWarehouse(_data);
				var requester = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(inventoryStats?.WarehouseValidatorId ?? -2);
				var warehouse = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data);
				var title = "[EBAS] Inventory Release Approval";
				var content = $@"";
				content += $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>";
				content += $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/><br/>";
				content += $"</div>";
				content += $"<div style='font-family: Arial, sans-serif; font-size: 14px; color: #333333; line-height: 1.5;'>";
				//content += $"<div style='margin-bottom: 10px;'>Inventory Release Request Approved</div>";
				content += $"<div style='margin-bottom: 10px;'>The following inventory release request has been <strong>approved</strong>:</div>";
				content += $"<table cellpadding='4' cellspacing='0' style='border-collapse: collapse; margin-top: 8px; margin-bottom: 16px;'>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Requester:</td>";
				content += $"<td>{requester?.Name}</td>";
				content += $"</tr>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Site:</td>";
				content += $"<td>{warehouse?.Lagerort}</td>";
				content += $"</tr>";
				//content += $"<tr>";
				//content += $"<td style='font-weight: bold; padding-right: 8px;'>Submitted At:</td>";
				//content += $"<td>timestamp</td>";
				//content += $"</tr>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Approved By:</td>";
				content += $"<td>{_user.Name}</td>";
				content += $"</tr>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Approval Time:</td>";
				content += $"<td>{DateTime.Now:dd.MM.yyyy HH:mm}</td>";
				content += $"</tr>";
				content += $"</table>";
				content += $"<div style='margin-bottom: 10px;'>";
				content += $"The inventory release can now proceed according to the defined workflow.";
				content += $"</div>";
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
