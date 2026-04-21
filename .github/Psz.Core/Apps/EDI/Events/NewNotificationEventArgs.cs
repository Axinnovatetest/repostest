using System;

namespace Psz.Core.Apps.EDI.Events
{
	public class ImportedOrdersNotificationEventArgs: EventArgs
	{
		public ImportedOrdersNotificationEventArgs(Models.HubMessage.ImportedOrdersNotificationModel content)
		{
			_content = content;
		}

		private readonly Models.HubMessage.ImportedOrdersNotificationModel _content;
		public Models.HubMessage.ImportedOrdersNotificationModel Content { get { return _content; } }
	}
}
