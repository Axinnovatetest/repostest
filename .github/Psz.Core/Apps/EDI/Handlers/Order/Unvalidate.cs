using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> Unvalidate(int orderId, Core.Identity.Models.UserModel user)
		{

			if(user == null || (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
			{
				throw new Core.Exceptions.UnauthorizedException();
			}

			lock(Locks.OrdersLock)
			{
				#region > Validation
				if(user == null
					|| !user.Access.CustomerService.EDIOrderEdit)
				{
					return Core.Models.ResponseModel<object>.AccessDeniedResponse();
				}

				var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderId);
				if(orderDb == null)
				{
					return new Core.Models.ResponseModel<object>()
					{
						Success = false,
						Errors = new List<string>()
						{
							"Order not found"
						}
					};
				}

				if(orderDb.Neu_Order == true)
				{
					return new Core.Models.ResponseModel<object>()
					{
						Success = false,
						Errors = new List<string>()
						{
							"Order already unvalidated"
						}
					};
				}
				#endregion

				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateNeuOrder(orderDb.Nr, true);

				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateGebucht(orderDb.Nr, false);

				var orderExtensionDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrderId(orderDb.Nr);
				if(orderExtensionDb == null)
				{
					Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity()
					{
						Id = -1,
						OrderId = orderDb.Nr,
						EdiValidationTime = DateTime.Now,
						EdiValidationUserId = user.Id,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserId = user.Id,
						LastUpdateUsername = user.Username,
					});
				}
				else
				{
					orderExtensionDb.EdiValidationUserId = user.Id;
					orderExtensionDb.EdiValidationTime = DateTime.Now;
					Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.Update(orderExtensionDb);
				}

				// > Notify
				Core.Program.Notifier.PushEdiImportedOrdersNotification(new Core.Apps.EDI.Models.HubMessage.ImportedOrdersNotificationModel()
				{
					Type = "Success",
					Payload = Order.CountUnvalidated(null).ToString()
				});

				return Core.Models.ResponseModel<object>.SuccessResponse();
			}
		}
	}
}
