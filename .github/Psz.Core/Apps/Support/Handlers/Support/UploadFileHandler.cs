using Psz.Core.Apps.Support.Models.Request;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.Support.Handlers.Request
{
	public class UploadFileHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ProjectRequest>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public Models.Request.FilesModel _data { get; set; }
		public UploadFileHandler(Identity.Models.UserModel user, FilesModel data)
		{
			_data = data;
			_user = user;
		}

		public ResponseModel<List<ProjectRequest>> Handle()
		{
			var files = new List<Infrastructure.Data.Entities.Tables.Support.Request.FilesEntity>();

			if(_data.Files is not null)
			{
				foreach(var item in _data.Files)
				{
					byte[] fileBytes;
					using(var ms = new MemoryStream())
					{
						item.CopyTo(ms);
						fileBytes = ms.ToArray();
						var fileId = Program.FilesManager.NewFile(fileBytes, Path.GetExtension(item.FileName), 10);
						Infrastructure.Data.Entities.Tables.Support.Request.FilesEntity filesEntity = new Infrastructure.Data.Entities.Tables.Support.Request.FilesEntity
						{
							ContentType = item.ContentType,
							FileExtention = Path.GetExtension(item.FileName),
							FileName = item.FileName,
							RequestId = _data.RequestId,
							FileId = fileId,
						};
						files.Add(filesEntity);

					}
				}
			}



			var filesDb = Infrastructure.Data.Access.Tables.Support.Request.FilesAccess.GetByRequestId(_data.RequestId);

			var removedFiles = filesDb.Where(p => !_data.FileIds.Any(p2 => p2 == p.Id)).Select(x => x.Id).ToList();
			var removedFileId = filesDb.Where(p => !_data.FileIds.Any(p2 => p2 == p.Id)).Select(x => x.FileId).ToList();

			Infrastructure.Data.Access.Tables.Support.Request.FilesAccess.Delete(removedFiles);
			foreach(var file in removedFileId)
			{
				Program.FilesManager.DeleteFile(file);
			}

			Infrastructure.Data.Access.Tables.Support.Request.FilesAccess.Insert(files);

			return new ResponseModel<List<ProjectRequest>>();
		}
		private string getNextFileName(string fileName)
		{
			string extension = Path.GetExtension(fileName);

			int i = 0;
			while(File.Exists(Path.Combine(@"C:\files", fileName)))
			{
				if(i == 0)
					fileName = fileName.Replace(extension, "(" + ++i + ")" + extension);
				else
					fileName = fileName.Replace("(" + i + ")" + extension, "(" + ++i + ")" + extension);
			}

			return fileName;
		}
		public ResponseModel<List<ProjectRequest>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
