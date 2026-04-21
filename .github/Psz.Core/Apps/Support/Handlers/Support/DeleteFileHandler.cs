using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.IO;

namespace Psz.Core.Apps.Support.Handlers.Request
{
	public class DeleteFileHandler: IHandle<int, ResponseModel<bool>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }
		public DeleteFileHandler(Identity.Models.UserModel user, int data)
		{
			_data = data;
			_user = user;
		}

		public ResponseModel<bool> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var file = Infrastructure.Data.Access.Tables.Support.Request.FilesAccess.Get(_data);
				var filePath = Path.Combine(@"c:\files", file.FileName);
				if(!File.Exists(filePath))
				{
					return ResponseModel<bool>.SuccessResponse(true);
				}

				// Delete the file
				File.Delete(filePath);

				if(!File.Exists(filePath))
				{
					return ResponseModel<bool>.SuccessResponse(true);
				}

				return ResponseModel<bool>.SuccessResponse(false);
			} catch(Exception e)
			{
				throw;
			}
		}

		public ResponseModel<bool> Validate()
		{
			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
