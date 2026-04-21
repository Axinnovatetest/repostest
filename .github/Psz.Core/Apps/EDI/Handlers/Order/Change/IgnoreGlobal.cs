using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public partial class Change
		{
			public static Core.Models.ResponseModel<object> IgnoreGlobal(int globalChangeId,
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

						var globalChangeDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.Get(globalChangeId);
						if(globalChangeDb == null)
						{
							throw new Core.Exceptions.NotFoundException();
						}

						if((Enums.OrderEnums.GlobalOrderChangeStatus)globalChangeDb.GlobalStatus
							!= Enums.OrderEnums.GlobalOrderChangeStatus.Pending)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Global Change already used" }
							};
						}

						globalChangeDb.GlobalStatus = (int)Enums.OrderEnums.GlobalOrderChangeStatus.Ignored;
						globalChangeDb.ActionTime = DateTime.Now;
						globalChangeDb.ActionUserId = user.Id;
						globalChangeDb.ActionUsername = user.Username;

						Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.Update(globalChangeDb);

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
