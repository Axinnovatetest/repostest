using System;

namespace Psz.Core.FinanceControl.Handlers.Supplier
{
	public class DeleteHandler: IHandle<Models.Supplier.UpdateModel, ResponseModel<object>>
	{
		private int _supplierId { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public DeleteHandler(int supplierId, Identity.Models.UserModel user)
		{
			this._supplierId = supplierId;
			this._user = user;
		}

		public ResponseModel<object> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var supplierEntity = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Get(_supplierId);
				if(supplierEntity == null)
				{
					return ResponseModel<object>.SuccessResponse();
				}

				Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Delete(supplierEntity.Nr);

				var addressEntity = supplierEntity.Nummer.HasValue
					? Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(supplierEntity.Nummer.Value)
					: null;
				if(addressEntity == null)
				{
					return ResponseModel<object>.SuccessResponse();
				}

				Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Delete(addressEntity.Nr);

				return ResponseModel<object>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<object> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<object>.AccessDeniedResponse();
			}

			return ResponseModel<object>.SuccessResponse();
		}
	}
}
