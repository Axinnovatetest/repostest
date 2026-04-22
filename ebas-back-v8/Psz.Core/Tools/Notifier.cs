using System;

namespace Psz.Core.Tools
{
	public class Notifier
	{
		#region > EDI
		public event EventHandler<Apps.EDI.Events.ImportedOrdersNotificationEventArgs> NewNotificationEventHandler;

		public void PushEdiImportedOrdersNotification(Apps.EDI.Models.HubMessage.ImportedOrdersNotificationModel content)
		{
			if(content == null)
			{
				return;
			}

			NewNotificationEventHandler?.Invoke(this, new Apps.EDI.Events.ImportedOrdersNotificationEventArgs(content));
		}
		#endregion
	}
}
