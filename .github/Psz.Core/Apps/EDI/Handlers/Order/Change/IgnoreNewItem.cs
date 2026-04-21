using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public partial class Change
		{
			public static Core.Models.ResponseModel<object> IgnoreNewItem(int itemChangeId,
				Core.Identity.Models.UserModel user)
			{
				lock(Locks.OrdersLock)
				{
					try
					{
						if(user == null
							|| !user.Access.Purchase.ModuleActivated)
						{
							throw new Core.Exceptions.UnauthorizedException();
						}

						var itemChangeDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeItemAccess.Get(itemChangeId);
						if(itemChangeDb == null)
						{
							throw new Core.Exceptions.NotFoundException();
						}

						if((Enums.OrderEnums.OrderChangeItemStatus)itemChangeDb.Status
							!= Enums.OrderEnums.OrderChangeItemStatus.Pending)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Item Change already used" }
							};
						}

						if((Enums.OrderEnums.OrderChangeItemTypes)itemChangeDb.Type
							!= Enums.OrderEnums.OrderChangeItemTypes.New)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Wrong Item Change Type" }
							};
						}

						var orderChangeDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.Get(itemChangeDb.OrderChangeId);
						if(orderChangeDb == null)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Order Change not found" }
							};
						}

						var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(itemChangeDb.OrderId);
						if(orderDb == null)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Order not found" }
							};
						}

						if(orderDb.Neu_Order == false)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Order is validated" }
							};
						}

						#region > Update Item Change
						itemChangeDb.Status = (int)Enums.OrderEnums.OrderChangeItemStatus.Ignored;
						itemChangeDb.ActionTime = DateTime.Now;
						itemChangeDb.ActionUserId = user.Id;
						itemChangeDb.ActionUsername = user.Username;

						Infrastructure.Data.Access.Tables.PRS.OrderChangeItemAccess.Update(itemChangeDb);
						#endregion

						return Core.Models.ResponseModel<object>.SuccessResponse();
					} catch(Exception e)
					{
						Infrastructure.Services.Logging.Logger.Log(e);
						throw;
					}
				}
			}
		}
	}
}
