using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.OrderError
{
	public partial class OrderError
	{
		public static Core.Models.ResponseModel<object> Validate(int id,
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
					return new Core.Models.ResponseModel<object>()
					{
						Errors = new List<string>()
						{
							"Error already validated"
						}
					};
				}

				// > Update
				orderErrorDb.Validated = true;
				orderErrorDb.ValidationUserId = user.Id;
				orderErrorDb.ValidationTime = DateTime.Now;
				Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.Update(orderErrorDb);

				// > Notify
				Core.Program.Notifier.PushEdiImportedOrdersNotification(new Models.HubMessage.ImportedOrdersNotificationModel()
				{
					Payload = CountNotValidated(null).ToString(),
					Type = "Error"
				});

				return Core.Models.ResponseModel<object>.SuccessResponse();
			}
		}
	}
}
