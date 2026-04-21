using Infrastructure.Services.Files;
using Psz.Core.Apps.Support.Models.Request;
using Psz.Core.Common;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Apps.Support.Handlers.Request
{
	public class DownloadFileHandler: IHandle<int, ResponseModel<FileDownloadModel>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }
		public DownloadFileHandler(Identity.Models.UserModel user, int data)
		{
			_data = data;
			_user = user;
		}

		public ResponseModel<FileDownloadModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var fileDb = Infrastructure.Data.Access.Tables.Support.Request.FilesAccess.Get(_data);
				var file = Program.FilesManager.GetFile(fileDb.FileId);
				var response = new FileDownloadModel
				{
					ContentType = fileDb.ContentType,
					FileContent = file.FileBytes,
					Name = fileDb.FileName,
				};

				return ResponseModel<FileDownloadModel>.SuccessResponse(response);

			} catch(Exception e)
			{
				throw;
			}
		}

		public ResponseModel<FileDownloadModel> Validate()
		{
			return ResponseModel<FileDownloadModel>.SuccessResponse();
		}
	}
}
