using Psz.Core.Common;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class DeleteFileHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteFileHandler(Identity.Models.UserModel user, int FileId)
		{
			this._user = user;
			this._data = FileId;
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
				if(this._data > 0)
					Program.FilesManager.DeleteFile(this._data);
				var FileExtensionEntity = Infrastructure.Data.Access.Tables.CTS.CTSBlanketFilesAccess.GetFile(this._data);

				Infrastructure.Data.Access.Tables.CTS.CTSBlanketFilesAccess.DeleteFile(this._data);

				//logging
				var _log = new Helpers.LogHelper((int)FileExtensionEntity.Id, 0, 0, "Blanket", Helpers.LogHelper.LogType.DELETIONOBJECT, "CTS", _user)
			 .LogCTS(null, null, null, FileExtensionEntity.Id);
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}


		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// - 
			var FileExtensionEntity = Infrastructure.Data.Access.Tables.CTS.CTSBlanketFilesAccess.GetFile(this._data);


			//if (FileExtensionEntity == null)
			//    return ResponseModel<int>.FailureResponse("File not found");


			return ResponseModel<int>.SuccessResponse();
		}

	}
}
