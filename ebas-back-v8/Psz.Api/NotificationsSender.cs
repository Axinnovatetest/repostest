using Microsoft.AspNetCore.SignalR;
using System;

namespace Psz.Api
{
	public class NotificationsSender
	{
		public NotificationsSender()
		{
			Core.Program.Notifier.NewNotificationEventHandler += this.newNotificationCaptured;
		}

		private void newNotificationCaptured(object sender, Core.Apps.EDI.Events.ImportedOrdersNotificationEventArgs e)
		{
			Infrastructure.Services.Logging.Logger.Log("newNotificationCaptured...");
			notify(e.Content);
		}

		private void notify(Core.Apps.EDI.Models.HubMessage.ImportedOrdersNotificationModel notification)
		{
			try
			{
				if(notification == null)
				{
					return;
				}

				Hubs.OrderHubService.HubContext.Clients.All.SendAsync("BroadcastMessage",
					notification.Type,
					notification.Payload);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}

			//var connectedUserDevices = Solars.Hubs.MainHub.Users.GetDevices(notification.ForUserId.Value);
			//if (connectedUserDevices == null || connectedUserDevices.Count() == 0)
			//{
			//    return;
			//}

			//lock (_mainHubContext)
			//{
			//    foreach (var connectedDevice in connectedUserDevices)
			//    {
			//        var time = notification.CreationTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture);

			//        var desktopNotificationSent = false;

			//        foreach (var connectionId in connectedDevice.ConnectionIds)
			//        {
			//            _mainHubContext.Clients.Client(connectionId).newNotification(notification.Id,
			//               notification.LinkId,
			//               notification.GeneratedDescription,
			//               notification.Type,
			//               notification.TypeText,
			//               time);

			//            if (!desktopNotificationSent)
			//            {
			//                _mainHubContext.Clients.Client(connectionId).newDesktopNotification(notification.Id,
			//                    notification.LinkId,
			//                    notification.GeneratedDescription,
			//                    notification.Type,
			//                    notification.TypeText,
			//                    time);

			//                desktopNotificationSent = true;
			//            }
			//        }
			//    }
			//}
		}

		//public void CreateAndNotify(Core.Apps.Notifications.Models.NewNotificationModel newNotification,
		//    int senderId,
		//    List<int> forUsersIds)
		//{
		//    var insertedIds = new List<int>();
		//    foreach (var forUserId in forUsersIds)
		//    {
		//        newNotification.ForUserId = forUserId;
		//        newNotification.CreationUserId = senderId;

		//        newNotification.Link = !string.IsNullOrEmpty(newNotification.Link)
		//            ? newNotification.Link
		//            : "#";

		//        var response = Core.Apps.Notifications.Handlers.New(newNotification);
		//        int insertedNotificationId;
		//        int.TryParse(response.Id, out insertedNotificationId);

		//        insertedIds.Add(insertedNotificationId);
		//    }

		//    var notifications = Core.Apps.Notifications.Handlers.Get(insertedIds);

		//    foreach (var notification in notifications)
		//    {
		//        notify(notification);
		//    }
		//}
		//public void CreateAndNotify(Core.Apps.Notifications.Models.NewNotificationModel notification,
		//    int senderId,
		//    int receiverId)
		//{
		//    CreateAndNotify(notification, senderId, new List<int> { receiverId });
		//}
		//public void CreateAndNotify(Core.Apps.Notifications.Models.NewNotificationModel notification)
		//{
		//    var usersIds = Infrastructure.Data.Access.Tables.User.Get(true)
		//        .Select(e => e.Id)
		//        .ToList();

		//    CreateAndNotify(notification, -1, usersIds);
		//}

		//public void CreateAndNotify(string message,
		//    int senderId,
		//    List<int> usersIds)
		//{
		//    CreateAndNotify(new Core.Apps.Notifications.Models.NewNotificationModel { Description = message },
		//        senderId,
		//        usersIds);
		//}
		//public void CreateAndNotify(string message, int senderId, int userId)
		//{
		//    CreateAndNotify(message, senderId, new List<int>() { userId });
		//}
		//public void CreateAndNotify(string message, int senderId)
		//{
		//    var usersIds = Infrastructure.Data.Access.Tables.User.Get(true)
		//        .Select(e => e.Id)
		//        .ToList();

		//    CreateAndNotify(message, senderId, usersIds);
		//}
	}
}