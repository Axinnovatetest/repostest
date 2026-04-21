using System;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class UpdateFileBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Psz.Core.CustomerService.Models.Blanket.BlanketFilesModel _data { get; set; }
		public UpdateFileBlanketHandler(Identity.Models.UserModel user, Psz.Core.CustomerService.Models.Blanket.BlanketFilesModel data)
		{
			_user = user;
			_data = data;
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

				var insertedId = 0;
				/// 
				//var fileEntity = Infrastructure.Data.Access.Tables.CTS.CTSBlanketFilesAccess.DeleteByAngeboteNrwExceptIds(this._data.AngeboteNr);

				if(this._data.Files != null && this._data.Files.Count > 0)
				{
					foreach(var fileItem in this._data.Files)
					{
						if(fileItem != null)
						{
							fileItem.AngeboteNr = this._data.AngeboteNr;

							insertedId = Infrastructure.Data.Access.Tables.CTS.CTSBlanketFilesAccess.Insert(fileItem.ToFile_OrderEntity(_user.Id));
						}
					}
				}

				return ResponseModel<int>.SuccessResponse(1);
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
			var blanketEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.AngeboteNr);
			if(blanketEntity == null)
				return ResponseModel<int>.FailureResponse("Order not found");

			return ResponseModel<int>.SuccessResponse();
		}


	}

}
