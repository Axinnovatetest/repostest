using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Threading.Tasks;

	public class UpdateFileHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Order.OrderFileModel _data { get; set; }

		public UpdateFileHandler(Identity.Models.UserModel user, Models.Budget.Order.OrderFileModel model)
		{
			this._user = user;
			this._data = model;
		}

		public async Task<ResponseModel<int>> Handleasync()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var insertedId = 0;
				/// 
				var fileEntity = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.DeleteByOrderIdwExceptIds(this._data.Id_Order, this._data.OrderFileIds);

				if(this._data.Files != null && this._data.Files.Count > 0)
				{
					foreach(var fileItem in this._data.Files)
					{
						if(fileItem != null)
						{
							fileItem.idOrder = this._data.Id_Order;
							fileItem.userId = this._user.Id;
							insertedId = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.Insert(await fileItem.ToFile_OrderEntity(_user.Id));
						}
					}
				}

				return ResponseModel<int>.SuccessResponse(insertedId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Id_Order) == null)
				return ResponseModel<int>.FailureResponse("Order not found");

			return ResponseModel<int>.SuccessResponse();
		}

		public ResponseModel<int> Handle()
		{
			throw new NotImplementedException();
		}
	}
}
