using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.OrderError
{
	public partial class OrderError
	{
		public static Core.Models.ResponseModel<object> ReloadFile(int id,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.OrdersLock)
			{
				if(user == null)
				{
					return Core.Models.ResponseModel<object>.AccessDeniedResponse();
				}

				var orderErrorDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.Get(id);
				if(orderErrorDb == null)
				{
					return new Core.Models.ResponseModel<object>()
					{
						Errors = new List<string>()
						{
							"order error not found"
						}
					};
				}

				if(orderErrorDb.Validated)
				{
					// don't do anything
					return Core.Models.ResponseModel<object>.SuccessResponse();
				}

				//if (orderErrorDb.Validated)
				//{
				//    return new Core.Models.ResponseModel<object>()
				//    {
				//        Errors = new List<string>()
				//        {
				//            "Error already validated"
				//        }
				//    };
				//}

				// > Update
				orderErrorDb.Validated = true;
				orderErrorDb.ValidationUserId = user.Id;
				orderErrorDb.ValidationTime = DateTime.Now;
				Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.Update(orderErrorDb);

				// Move file to New Orders' Directory
				Program.PurchaseEdiFileWatcher.moveErrorToNewFile(orderErrorDb.FileName);

				// > Notify
				Core.Program.Notifier.PushEdiImportedOrdersNotification(new Models.HubMessage.ImportedOrdersNotificationModel()
				{
					Payload = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.CountCustomerOrdersByIsNew(true).ToString(),
					Type = "Success"
				});

				return Core.Models.ResponseModel<object>.SuccessResponse();
			}
		}
	}
}
