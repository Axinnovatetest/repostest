using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class DownloadErrorFileHandler: IHandle<Identity.Models.UserModel, ResponseModel<KeyValuePair<byte[], string>>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DownloadErrorFileHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<KeyValuePair<byte[], string>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new KeyValuePair<byte[], string>();
				var error = Infrastructure.Data.Access.Tables.CTS.ErrorAccess.Get(_data);
				if(error != null)
					response = new KeyValuePair<byte[], string>(System.IO.File.ReadAllBytes(error.FileName), Path.GetFileName(error.FileName));
				else
					return ResponseModel<KeyValuePair<byte[], string>>.FailureResponse("file not found .");

				return ResponseModel<KeyValuePair<byte[], string>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<KeyValuePair<byte[], string>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<KeyValuePair<byte[], string>>.AccessDeniedResponse();
			}

			return ResponseModel<KeyValuePair<byte[], string>>.SuccessResponse();
		}

	}
}
