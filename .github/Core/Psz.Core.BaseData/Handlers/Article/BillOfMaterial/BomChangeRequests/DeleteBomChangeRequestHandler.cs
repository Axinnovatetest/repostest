using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.BomChangeRequests
{
	public class DeleteBomChangeRequestHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteBomChangeRequestHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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

				// 
				var deletedId = Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsAccess.Delete(this._data);
				return ResponseModel<int>.SuccessResponse(deletedId);

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/* || !this._user.Access.MasterData.DeleteBCR.Value*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();

			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
