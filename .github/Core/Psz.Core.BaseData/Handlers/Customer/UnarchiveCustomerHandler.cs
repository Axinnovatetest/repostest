using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Customer
{
	public class UnarchiveCustomerHandler: IHandle<Models.Customer.CreateModel, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UnarchiveCustomerHandler(int Id, Identity.Models.UserModel user)
		{
			this._data = Id;
			this._user = user;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				lock(Locks.CostumerEditLock.GetOrAdd(this._data, new object()))
				{
					var kundenExtensionEntity = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(this._data);
					kundenExtensionEntity.IsArchived = false;
					kundenExtensionEntity.UpdateTime = DateTime.Now;
					kundenExtensionEntity.UpdateUserId = this._user.Id;
					var response = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.Update(kundenExtensionEntity);
					return ResponseModel<int>.SuccessResponse(response);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
