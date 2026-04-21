using Infrastructure.Services.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.Win32.SafeHandles;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Minio.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Services.Files
{
	public class FilesManager
	{

		[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern bool LogonUser(String Username, String Domain, String Password, int LogonType, int LogonProvider, out SafeAccessTokenHandle Token);
		private object _lock { get; set; } = new object();
		private object _lockTemp { get; set; } = new object();
		private string _programFilesPath { get; set; }
		private string _programFilesTempPath { get; set; }
		private string _accesskey { get; set; }
		private string _secretkey { get; set; }
		private string _endpoint { get; set; }
		private string _bucket { get; set; }
		private string BasePath { get; set; }
		private const int LOGON32_PROVIDER_DEFAULT = 0;
		private const int LOGON32_LOGON_INTERACTIVE = 2;
		public static Impersonate Impersonate { get; set; }
		public FilesManager(string programFilesPath, string programFilesTempPath, string ImpersonateUserName = null, string ImpresonatePassword = null, string ImpersonateDomain = null)
		{
			this._programFilesPath = programFilesPath;
			this._programFilesTempPath = programFilesTempPath;

			if(ImpersonateUserName is not null && ImpresonatePassword is not null)
			{
				Impersonate = new Impersonate()
				{
					ImpersonateUsername = ImpersonateUserName,
					ImpersonatePassword = ImpresonatePassword,
					ImpersonateDomain = ImpersonateDomain
				};
			}
		}
		public FilesManager(string programFilesPath, string programFilesTempPath, string accesskey, string secretkey, string endpoint, string bucket)
		{
			this._programFilesPath = programFilesPath;
			this._programFilesTempPath = programFilesTempPath;
			this._accesskey = accesskey;
			this._secretkey = secretkey;
			this._endpoint = endpoint;
			this._bucket = bucket.ToLower();
		}
		// Minio Experiment
		/// <summary>
		/// Use FilePath to save Data on remote server
		/// </summary>
		/// <param name="accesskey"></param>
		/// <param name="secretkey"></param>
		/// <param name="endpoint"></param>
		/// <param name="bucketName"></param>
		/// <param name="objectName"></param>
		/// <param name="filePath"></param>
		/// <param name="contentType"></param>
		/// <returns></returns>
		static async Task<PutObjectResponse> SendFileFromDisk(string accesskey, string secretkey, string endpoint, string bucketName, string objectName, string filePath, string contentType)
		{
			try
			{
				using var minio = new MinioClient().WithEndpoint(endpoint).WithCredentials(accesskey, secretkey).Build();

				var beArgs = new BucketExistsArgs()
					.WithBucket(bucketName);
				bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
				if(!found)
				{
					var mbArgs = new MakeBucketArgs()
						.WithBucket(bucketName);
					await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
				}

				// Upload a file to bucket.
				var putObjectArgs = new PutObjectArgs()
					.WithBucket(bucketName)
					.WithObject(objectName)
					.WithFileName(filePath)
					.WithContentType(contentType);
				return await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public async Task<PutObjectResponse> ReSendFailedFilesFromDisk(int Id)
		{

			var data = GetFailedFileById(Id);
			if(data is null)
				return null;
			var objectstat = await RetrySendFileFromBytesArray(_accesskey, _secretkey, _endpoint, _bucket, data.FileName, data.FileBytes, data.FileExtension);
			if(!string.IsNullOrWhiteSpace(objectstat.ObjectName))
			{
				Infrastructure.Data.Access.Tables.FNC.MinIO.PSZ_FileServer_RetryDataAccess.UpdateErrorLevel(Id, 1);
			}
			return objectstat;
		}
		/// <summary>
		/// Use byte Array  to save Data on remote server
		/// </summary>
		/// <param name="accesskey"></param>
		/// <param name="secretkey"></param>
		/// <param name="endpoint"></param>
		/// <param name="bucketName"></param>
		/// <param name="objectName"></param>
		/// <param name="data"></param>
		/// <param name="contentType"></param>
		/// <returns>return a PutObjectResponse</returns>
		static async Task<PutObjectResponse> SendFileFromBytesArray(int UserId, string accesskey, string secretkey, string endpoint, string bucketName, string objectName, byte[] data, string extension)
		{
			try
			{
				using var minio = new MinioClient().WithEndpoint(endpoint).WithCredentials(accesskey, secretkey).Build();

				using var stream = new MemoryStream(data);
				// Make a bucket on the server, if not already present.
				var beArgs = new BucketExistsArgs()
					.WithBucket(bucketName);
				bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
				if(!found)
				{
					var mbArgs = new MakeBucketArgs()
						.WithBucket(bucketName);
					await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
				}

				// Upload a file to bucket.
				var putObjectArgs = new PutObjectArgs()
					.WithBucket(bucketName)
					.WithObject(objectName)
					.WithStreamData(stream)
					.WithObjectSize(stream.Length)
					.WithContentType(Infrastructure.Services.FileServer.MimeHelper.GetMimeType(extension));

				return await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

			} catch(MinioException e)
			{
				Infrastructure.Data.Access.Tables.FNC.MinIO.PSZ_FileServer_RetryDataAccess.Insert(
					new Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity()
					{
						AddedOn = DateTime.Now,
						UserId = UserId,
						Exception = e.Message,
						FileName = objectName,
						FileExtension = extension,
						ErrorLevel = -1
					});
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return null;
		}

		async Task<PutObjectResponse> RetrySendFileFromBytesArray(string accesskey, string secretkey, string endpoint, string bucketName, string objectName, byte[] data, string extension)
		{
			try
			{
				using var minio = new MinioClient().WithEndpoint(endpoint).WithCredentials(accesskey, secretkey).Build();

				var stream = new MemoryStream(data);
				// Make a bucket on the server, if not already present.
				var beArgs = new BucketExistsArgs()
					.WithBucket(bucketName);
				bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
				if(!found)
				{
					var mbArgs = new MakeBucketArgs()
						.WithBucket(bucketName);
					await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
				}

				// Upload a file to bucket.
				var putObjectArgs = new PutObjectArgs()
					.WithBucket(bucketName)
					.WithObject(objectName)
					.WithStreamData(stream)
					.WithObjectSize(stream.Length)
					.WithContentType(Infrastructure.Services.FileServer.MimeHelper.GetMimeType(extension));

				return await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

			} catch(MinioException e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		/// <summary>
		/// confirm the file exist on Minio Server
		/// </summary>
		/// <param name="accesskey"></param>
		/// <param name="secretkey"></param>
		/// <param name="endpoint"></param>
		/// <param name="bucketName"></param>
		/// <param name="objectName"></param>
		/// <returns>return a PutObjectResponse</returns>
		public static async Task<bool> ConfirmFileExist(int UserId, string accesskey, string secretkey, string endpoint, string bucketName, string objectName, string extension = null)
		{
			try
			{
				using var minio = new MinioClient().WithEndpoint(endpoint).WithCredentials(accesskey, secretkey).Build();

				// verify Upload : 
				var statObjectArgs = new StatObjectArgs()
					.WithBucket(bucketName)
					.WithObject(objectName);

				var objectstat = await minio.StatObjectAsync(statObjectArgs).ConfigureAwait(false);
				return objectstat.Size != 0 || objectstat.ETag != null || objectstat.ContentType is not null;

			} catch(MinioException e)
			{
				Infrastructure.Data.Access.Tables.FNC.MinIO.PSZ_FileServer_RetryDataAccess.Insert(
					new Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity()
					{
						AddedOn = DateTime.Now,
						Exception = e.Message,
						FileName = objectName,
						UserId = UserId,
						FileExtension = extension,
						ErrorLevel = -2
					});
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return false;
		}
		public static async Task<bool> DeleteFileOnMinioAsync(int UserId, string accesskey, string secretkey, string endpoint, string bucketName, string objectName, string extension = null, System.Threading.CancellationToken _token = default)
		{
			try
			{
				using var minio = new MinioClient().WithEndpoint(endpoint).WithCredentials(accesskey, secretkey).Build();

				// verify Upload : 
				var statObjectArgs = new StatObjectArgs()
					.WithBucket(bucketName)
					.WithObject(objectName);

				var objectstat = await minio.StatObjectAsync(statObjectArgs).ConfigureAwait(false);
				if(objectstat.Size == 0 || objectstat.ETag == null || objectstat.ContentType is null)
					throw new InvalidOperationException("Can not remove not existing object , File does not exist on minio server!");

				var removeArgument = new RemoveObjectArgs()
					.WithObject(objectName)
					.WithBucket(bucketName);
				await minio.RemoveObjectAsync(removeArgument, _token).ConfigureAwait(false);

			} catch(MinioException e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
			return true;
		}
		private static string ValidateMetadata(string bucket, string subbucket, string ObjectName)
		{
			if(string.IsNullOrWhiteSpace(bucket) || string.IsNullOrEmpty(bucket))
				throw new InvalidBucketNameException("Bucket Name can not be null");

			return string.Concat(subbucket, "/", ObjectName);
		}
		/// <summary>
		/// Get File From Minio Server
		/// </summary>
		/// <param name="accesskey"></param>
		/// <param name="secretkey"></param>
		/// <param name="endpoint"></param>
		/// <param name="bucketName"></param>
		/// <param name="objectName"></param>
		/// <returns>return a memory stream </returns>
		static async Task<MemoryStream> GetObject(
			string accesskey,
			string secretkey,
			string endpoint,
			string bucketName,
			string objectName
			)
		{
			var memoryStream = new MemoryStream();

			try
			{
				using IMinioClient minio = new MinioClient().WithEndpoint(endpoint).WithCredentials(accesskey, secretkey).Build();

				var args = new GetObjectArgs()
					.WithBucket(bucketName)
					.WithObject(objectName)
					.WithCallbackStream((stream) =>
					{
						stream.CopyTo(memoryStream);
					});
				await minio.GetObjectAsync(args).ConfigureAwait(false);

				memoryStream.Position = 0;
				return memoryStream;

			} catch(Exception e)
			{
				throw;
			}
		}
		// end experiment 

		public int NewFile(
			byte[] fileBytes,
			string extension,
			int? module,
			string fileName = null,
			string title = null)
		{
			#region > Check nulls
			if(fileBytes.Count() == 0 || string.IsNullOrEmpty(extension))
			{
				throw new NullReferenceException();
			}
			#endregion

			lock(_lock)
			{
				fileName = ValidateFileName(fileName);

				if(!Directory.Exists(_programFilesPath))
					Directory.CreateDirectory(_programFilesPath);

				System.IO.File.WriteAllBytes(_programFilesPath + fileName + extension,
					fileBytes);

				int insertedFileId = Data.Access.Tables.FileAccess.Insert(new Data.Entities.Tables.FileEntity()
				{
					Name = fileName,
					Path = "", // "Files\\",
					Extension = extension,
					CreationTime = DateTime.Now,
					Title = !string.IsNullOrEmpty(title) ? title : fileName,
					CreationUserId = -1,
					Module = module
				});

				return insertedFileId;
			}
		}
		/// <summary>
		/// upload file to minio server and save the log to File table 
		/// </summary>
		/// <param name="UserId"></param>
		/// <param name="fileBytes"></param>
		/// <param name="extension"></param>
		/// <param name="module"></param>
		/// <param name="fileName"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		public async Task<int> NewFileWithMinio(
			int UserId,
			byte[] fileBytes,
			string extension,
			int? module,
			string fileName = null,
			string title = null)
		{
			#region > Check nulls
			if(fileBytes.Count() == 0 || string.IsNullOrEmpty(extension))
			{
				throw new NullReferenceException();
			}
			#endregion

			fileName = ValidateFileName(fileName);

			var fileinfor = await SendFileFromBytesArray(UserId, _accesskey, _secretkey, _endpoint, _bucket, fileName, fileBytes, extension);

			#region localBackUp1
			if(!Directory.Exists(_programFilesPath))
				Directory.CreateDirectory(_programFilesPath);
			#endregion

			lock(_lock)
			{
				#region localBackUp2
				System.IO.File.WriteAllBytes(_programFilesPath + fileName + extension, fileBytes);
				#endregion

				int insertedFileId = Data.Access.Tables.FileAccess.Insert(new Data.Entities.Tables.FileEntity()
				{
					Name = fileName,
					Path = "", // "Files\\",
					Extension = extension,
					CreationTime = DateTime.Now,
					Title = !string.IsNullOrEmpty(title) ? title : fileName,
					CreationUserId = -1,
					Module = module
				});

				return insertedFileId;
			}
		}
		public async Task<Tuple<int, PutObjectResponse>> NewFileWithMinioWithStatusCode(
			int UserId,
			byte[] fileBytes,
			string extension,
			int? module,
			string fileName = null,
			string title = null)
		{

			 fileName = generateFileName().ToLower();
			#region > Check nulls
			if(fileBytes.Count() == 0 || string.IsNullOrEmpty(extension))
			{
				throw new NullReferenceException();
			}
			#endregion
			fileName = ValidateFileName(fileName);
			var fileinfor = await SendFileFromBytesArray(UserId, _accesskey, _secretkey, _endpoint, _bucket, fileName, fileBytes, extension);
			#region localBackUp1
			if(!Directory.Exists(_programFilesPath))
				Directory.CreateDirectory(_programFilesPath);
			#endregion

			lock(_lock)
			{
				#region localBackUp2
				System.IO.File.WriteAllBytes(_programFilesPath + fileName, fileBytes);
				#endregion

				int insertedFileId = Data.Access.Tables.FileAccess.Insert(new Data.Entities.Tables.FileEntity()
				{
					Name = fileName,
					Path = "", // "Files\\",
					Extension = extension,
					CreationTime = DateTime.Now,
					Title = !string.IsNullOrEmpty(title) ? title : fileName,
					CreationUserId = -1,
					Module = module
				});

				return new Tuple<int, PutObjectResponse>(insertedFileId, fileinfor);
			}
		}
		public int NewFileBlanket(
			byte[] fileBytes,
			string extension,
			int? module,
			string fileName = null,
			string title = null)
		{
			#region > Check nulls
			if(fileBytes.Count() == 0 || string.IsNullOrEmpty(extension))
			{
				throw new NullReferenceException();
			}
			#endregion

			lock(_lock)
			{
				fileName = ValidateFileNameBlanket(fileName);

				System.IO.File.WriteAllBytes(_programFilesPath + fileName + extension,
					fileBytes);

				int insertedFileId = Data.Access.Tables.CTS.CTSBlanketFilesAccess.Insert(new Data.Entities.Tables.CTS.CTSBlanketFilesEntity()
				{
					FileName = fileName,
					FileExtension = extension,
					CreationTime = DateTime.Now,
					CreationUserId = -1,
				});

				return insertedFileId;
			}
		}

		public int NewTempFile(byte[] fileBytes, string fileExtension, string fileName = null, string title = null)
		{
			#region > Check nulls
			if(fileBytes.Count() == 0 || string.IsNullOrEmpty(fileExtension))
			{
				throw new NullReferenceException();
			}
			#endregion

			lock(_lockTemp)
			{
				fileName = ValidateFileName(fileName);
				int fileNumber;
				System.IO.File.WriteAllBytes(Path.Combine(_programFilesTempPath, getTempFileName(fileName, fileExtension, title, out fileNumber)), fileBytes);
				return fileNumber;
			}
		}
		public async Task<int> NewTempFileMinio(byte[] fileBytes, string fileExtension, string fileName = null, string title = null)
		{
			#region > Check nulls
			if(fileBytes.Count() == 0 || string.IsNullOrEmpty(fileExtension))
			{
				throw new NullReferenceException();
			}
			#endregion

			int fileNumber;
			fileName = ValidateFileName(fileName);
			var tempfileName = getTempFileNameMinio(fileName, fileExtension, title, out fileNumber);
			await SendFileFromBytesArray(0, _accesskey, _secretkey, _endpoint, _bucket, tempfileName, fileBytes, fileExtension);
			Infrastructure.Data.Access.Tables.BSD.__BSD_TempFilesAccess.Insert(
				new Data.Entities.Tables.BSD.__BSD_TempFilesEntity()
				{
					TempFileName = tempfileName,
					CreationTime = DateTime.Now,
					FileId = fileNumber,
					FileExtension = fileExtension,
					LastModifiedDate = DateTime.Now,
				}
				);

			/*lock(_lockTemp)
			{
				fileName = ValidateFileName(fileName);
				int fileNumber;
				System.IO.File.WriteAllBytes(Path.Combine(_programFilesTempPath, getTempFileName(fileName, fileExtension, title, out fileNumber)), fileBytes);
				return fileNumber;
			}*/

			return fileNumber;
		}
		public int PersistTempFile(int tempFileId, int? module)
		{
			getTempFileProps(tempFileId, out string fullFileName, out string fileName, out string fileTitle, out string fileExtension);
			if(!string.IsNullOrWhiteSpace(fullFileName))
			{
				var tempFileBytes = System.IO.File.ReadAllBytes(Path.Combine(_programFilesTempPath, fullFileName));
				deleteTempFile(fullFileName);

				return NewFile(tempFileBytes, fileExtension, module, fileName, fileTitle);
			}
			return -1;
		}
		public async Task<int> PersistTempFileasync(int UserId, int tempFileId, int? module)
		{
			getTempFileProps(tempFileId, out string fullFileName, out string fileName, out string fileTitle, out string fileExtension);
			if(!string.IsNullOrWhiteSpace(fullFileName))
			{
				var tempFileBytes = System.IO.File.ReadAllBytes(Path.Combine(_programFilesTempPath, fullFileName));
				deleteTempFile(fullFileName);

				return await NewFileWithMinio(UserId, tempFileBytes, fileExtension, module, fileName, fileTitle);
			}
			return -1;
		}
		public Models.FileContent GetTempFile(int tempFileId)
		{
			getTempFileProps(tempFileId, out string fullFileName, out string fileName, out string fileTitle, out string fileExtension);
			var tempFileBytes = System.IO.File.ReadAllBytes(Path.Combine(_programFilesTempPath, fullFileName));

			if(tempFileBytes == null || tempFileBytes.Length == 0)
			{
				return null;
			}

			return new Models.FileContent()
			{
				FileName = fileName,
				FileExtension = fileExtension,
				FileBytes = tempFileBytes
			};
		}
		private string getTempFileName(string fileName, string fileExtension, string title, out int fileNumber)
		{
			fileNumber = 1;
			try
			{
				var tempFiles = Directory.GetFiles(this._programFilesTempPath, "*__PSZERP_DATA_*");
				if(tempFiles != null && tempFiles.Count() > 0)
				{
					var fileNumbers = tempFiles.Select(x => int.TryParse(Path.GetFileName(x).Split('_')[0], out var nb) ? nb : 0).ToList();
					fileNumber = fileNumbers.Max() + 1;
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return $"{fileNumber}_{fileName}_{title}_PSZERP_DATA_{fileExtension}";
		}
		private string getTempFileNameMinio(string fileName, string fileExtension, string title, out int fileNumber)
		{
			fileNumber = 1;
			try
			{
				/*var tempFiles = Directory.GetFiles(this._programFilesTempPath, "*__PSZERP_DATA_*");
				if(tempFiles != null && tempFiles.Count() > 0)
				{
					var fileNumbers = tempFiles.Select(x => int.TryParse(Path.GetFileName(x).Split('_')[0], out var nb) ? nb : 0).ToList();
					fileNumber = fileNumbers.Max() + 1;
				}*/

				getDbTempFileNameMinio(out fileNumber);

				// minio 

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return $"{fileNumber}_{fileName}_{title}_PSZERP_DATA_{fileExtension}";
		}
		private void getDbTempFileNameMinio(out int fileNumber)
		{

			if(Infrastructure.Data.Access.Tables.BSD.__BSD_TempFilesAccess.GetLastItem() is null)
			{
				fileNumber = 1;
			}
			else
			{
				fileNumber = (int)Infrastructure.Data.Access.Tables.BSD.__BSD_TempFilesAccess.GetLastItem()?.FileId + 1;
			}
		}

		private long getTempFileId(string tempFileName)
		{
			if(string.IsNullOrWhiteSpace(tempFileName))
				return -1;

			try
			{
				return long.TryParse(tempFileName.Split('_')[0], out var nb) ? nb : -1;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return -1;
			}
		}
		private string getTempFileName(string tempFileName)
		{
			if(string.IsNullOrWhiteSpace(tempFileName))
				return tempFileName;

			try
			{
				return Path.GetFileNameWithoutExtension(tempFileName).Split('_')[1];
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return tempFileName;
			}
		}
		private string getTempFileTitle(string tempFileName)
		{
			if(string.IsNullOrWhiteSpace(tempFileName))
				return tempFileName;

			try
			{
				return Path.GetFileNameWithoutExtension(tempFileName).Split('_')[2];
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return tempFileName;
			}
		}
		private string getTempFileProps(int tempFileId, out string fullFileName, out string fileName, out string fileTitle, out string fileExtension)
		{
			fullFileName = "";
			fileName = "";
			fileTitle = "";
			fileExtension = "";
			try
			{
				var tempFiles = Directory.GetFiles(this._programFilesTempPath, "*__PSZERP_DATA_*");
				if(tempFiles != null && tempFiles.Count() > 0)
				{
					fullFileName = tempFiles.Select(x => Path.GetFileName(x)).FirstOrDefault(x => x.StartsWith($"{tempFileId}_"));
					fileName = Path.GetFileNameWithoutExtension(fullFileName).Split('_')[1];
					fileTitle = Path.GetFileNameWithoutExtension(fullFileName).Split('_')[2];
					fileExtension = Path.GetExtension(fullFileName);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return fileName;
		}
		private void deleteTempFile(string fileName)
		{
			if(File.Exists(Path.Combine(this._programFilesTempPath, fileName)))
				File.Delete(Path.Combine(this._programFilesTempPath, fileName));
		}
		public static void cleanTempFolder(string programFilesTempPath)
		{
			try
			{
				System.IO.DirectoryInfo di = new DirectoryInfo(programFilesTempPath);
				Logging.Logger.Log($"[{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}] Cleaning temp folder: {di.GetFiles("*__PSZERP_DATA_*").Length} | {di.FullName}");
				foreach(FileInfo file in di.GetFiles("*__PSZERP_DATA_*"))
				{
					if(file.LastWriteTime < DateTime.Today.AddDays(-7))
						file.Delete();
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
		}
		private Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity2 GetFailedFileById(int id)
		{
			var file = Data.Access.Tables.FNC.MinIO.PSZ_FileServer_RetryDataAccess.Get(id);
			if(file == null)
			{
				return null;
			}

			var path = Path.Combine(_programFilesPath, file.FileName + file.FileExtension);

			byte[] fileBytes = System.IO.File.ReadAllBytes(path);
			if(fileBytes == null || fileBytes.Length == 0)
			{
				return null;
			}

			return new Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity2()
			{
				FileName = file.FileName,
				FileExtension = file.FileExtension,
				FileBytes = fileBytes,
				Id = file.Id
			};
		}
		public Models.FileContent GetFile(int id)
		{

			var file = Data.Access.Tables.FileAccess.Get(id);
			if(file == null)
			{
				return null;
			}

			var path = Path.Combine(_programFilesPath, file.Name + file.Extension);
			byte[] fileBytes = null;
			try
			{
				fileBytes = System.IO.File.ReadAllBytes(path);

			} catch
			{

			}
			if(fileBytes == null || fileBytes.Length == 0)
			{
				return null;
			}

			return new Models.FileContent()
			{
				FileName = file.Name,
				FileExtension = file.Extension,
				FileBytes = fileBytes
			};
		}
		/// <summary>
		///  Get the file name from the database and remove it form remote server  .
		/// </summary>
		/// <param name="UserId"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<bool> DeleteFileMinio(int UserId, int id, System.Threading.CancellationToken _token = default)
		{
			var file = Data.Access.Tables.FileAccess.Get(id);

			if(file == null)
			{
				return false;
			}
			return await DeleteFileOnMinioAsync(UserId, _accesskey, _secretkey, _endpoint, _bucket, file.Name, file.Extension, _token);
		}
		public async Task<Models.FileContent> GetFileMinio(int UserId, int id)
		{
			var file = Data.Access.Tables.FileAccess.Get(id);

			if(file == null)
			{
				return null;
			}

			var filexist = await ConfirmFileExist(UserId, _accesskey, _secretkey, _endpoint, _bucket, file.Name, file.Extension);

			if(!filexist)
				return GetFile(id);

			MemoryStream FetchedData = await GetObject(_accesskey, _secretkey, _endpoint, _bucket, file.Name);

			var fileBytes = FetchedData.ToArray();

			if(fileBytes == null || fileBytes.Length == 0)
			{
				return null;
			}
			return new Models.FileContent()
			{
				FileName = file.Name,
				FileExtension = file.Extension,
				FileBytes = fileBytes
			};
		}
		public async Task<Tuple<bool, Models.FileContent>> GetFileMinioWithStatus(int UserId, int id)
		{
			var file = Data.Access.Tables.FileAccess.Get(id);

			if(file == null)
			{
				return null;
			}

			var filexist = await ConfirmFileExist(UserId, _accesskey, _secretkey, _endpoint, _bucket, file.Name , file.Extension);

			if(!filexist)
			{
				var fetchfileFromBackup = GetFile(id);
				if(fetchfileFromBackup is null || fetchfileFromBackup.FileBytes is null || fetchfileFromBackup.FileBytes.Length == 0)
				{
					return new Tuple<bool, Models.FileContent>(false, null);
				}
				return new Tuple<bool, Models.FileContent>(true, fetchfileFromBackup);
			}
			MemoryStream FetchedData = await GetObject(_accesskey, _secretkey, _endpoint, _bucket, file.Name );

			var fileBytes = FetchedData.ToArray();

			if(fileBytes == null || fileBytes.Length == 0)
			{
				return new Tuple<bool, Models.FileContent>(false, null);
			}

			return new Tuple<bool, Models.FileContent>(true, new Models.FileContent()
			{
				FileName = file.Name,
				FileExtension = file.Extension,
				FileBytes = fileBytes
			});
		}
		public void DeleteFile(int id)
		{
			DeleteFile(Data.Access.Tables.FileAccess.Get(id));
		}
		public void DeleteFile(int id, TransactionsManager botransaction)
		{
			DeleteFile(Data.Access.Tables.FileAccess.GetWithTransaction(id, botransaction.connection, botransaction.transaction), botransaction);
		}
		public void DeleteFile(string name)
		{
			if(string.IsNullOrEmpty(name))
			{
				throw new Exception("name must have a value");
			}

			DeleteFile(Data.Access.Tables.FileAccess.GetByName(name));
		}
		public void DeleteFile(Data.Entities.Tables.FileEntity file)
		{
			lock(_lock)
			{
				if(file == null)
				{
					return;
				}

				Data.Access.Tables.FileAccess.Delete(file.Id);

				System.IO.File.Delete(_programFilesPath + file.Name + file.Extension);
			}
		}
		public void DeleteFile(Data.Entities.Tables.FileEntity file, TransactionsManager botransaction)
		{
			lock(_lock)
			{
				if(file == null)
				{
					return;
				}

				Data.Access.Tables.FileAccess.DeleteWithTransaction(file.Id, botransaction.connection, botransaction.transaction);

				System.IO.File.Delete(_programFilesPath + file.Name + file.Extension);
			}
		}
		#region > Helpers
		private string generateFileName()
		{
			string result = generateRandomKey(50);

			while(Data.Access.Tables.FileAccess.GetByName(result) != null)
			{
				result = generateFileName();
			}

			return result;
		}
		private string generateFileNameBlanket()
		{
			string result = generateRandomKey(50);

			while(Data.Access.Tables.CTS.CTSBlanketFilesAccess.GetByName(result) != null)
			{
				result = generateFileName();
			}

			return result;
		}
		private string generateRandomKey(int lenght,
			bool allowNumbers = true,
			bool allowLetters = true,
			bool uppers = true,
			bool lowers = true)
		{
			if(!allowNumbers && !allowLetters)
			{
				allowNumbers = true;
				allowLetters = true;
			}

			if(allowLetters && !allowNumbers
				&& !uppers && !lowers)
			{
				uppers = true;
			}

			var chars = "";
			if(allowNumbers)
			{
				chars += "0123456789";
			}
			if(allowLetters)
			{
				if(uppers)
				{
					chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
				}
				if(lowers)
				{
					chars += "abcdefghijklmnopqrstuvwxyz";
				}
			}

			var data = new byte[lenght];
			using(var crypto = new RNGCryptoServiceProvider())
			{
				crypto.GetBytes(data);
			}
			var result = new StringBuilder(lenght);
			foreach(byte b in data)
			{
				result.Append(chars[b % (chars.Length)]);
			}
			return result.ToString();
		}

		private string ValidateFileName(string name)
		{
			string result;

			if(string.IsNullOrEmpty(name))
			{
				result = generateFileName();
			}
			else
			{
				result = System.IO.Path.GetFileNameWithoutExtension(name);
			}

			result = cleanFileNameForDataBase(result);
			result = cleanFileNameForUrlRouting(result);

			while(Data.Access.Tables.FileAccess.GetByName(result) != null)
			{
				result = result + "_";
			}

			return result;
		}
		private string ValidateFileNameBlanket(string name)
		{
			string result;

			if(string.IsNullOrEmpty(name))
			{
				result = generateFileName();
			}
			else
			{
				result = System.IO.Path.GetFileNameWithoutExtension(name);
			}

			result = cleanFileNameForDataBase(result);
			result = cleanFileNameForUrlRouting(result);

			while(Data.Access.Tables.CTS.CTSBlanketFilesAccess.GetByName(result) != null)
			{
				result = result + "_";
			}

			return result;
		}
		private string cleanFileNameForDataBase(string fileName)
		{
			try
			{
				if(fileName.Length > 99)
				{
					fileName = fileName.Substring(0, 99);
				}

				string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

				var regex = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));

				return regex.Replace(fileName, "_");
			} catch(RegexMatchTimeoutException)
			{
				return string.Empty;
			}
		}
		private string cleanFileNameForUrlRouting(string filename)
		{
			if(filename == null)
				return "";

			const int maxlen = 80;
			int len = filename.Length;
			bool prevdash = false;
			var sb = new StringBuilder(len);
			char c;

			for(int i = 0; i < len; i++)
			{
				c = filename[i];
				if((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
				{
					sb.Append(c);
					prevdash = false;
				}
				else if(c >= 'A' && c <= 'Z')
				{
					// tricky way to convert to lowercase
					sb.Append((char)(c | 32));
					prevdash = false;
				}
				else if(c == ' ' || c == ',' || c == '.' || c == '/' ||
					  c == '\\' || c == '-' || c == '_' || c == '=')
				{
					if(!prevdash && sb.Length > 0)
					{
						sb.Append('_');
						prevdash = true;
					}
				}
				else if((int)c >= 128)
				{
					int prevlen = sb.Length;
					sb.Append(remapInternationalCharToAscii(c));
					if(prevlen != sb.Length)
						prevdash = false;
				}
				if(i == maxlen)
					break;
			}

			if(prevdash)
				return sb.ToString().Substring(0, sb.Length - 1);
			else
				return sb.ToString();
		}
		private string remapInternationalCharToAscii(char c)
		{
			string s = c.ToString().ToLowerInvariant();
			if("àåáâäãåą".Contains(s))
			{
				return "a";
			}
			else if("èéêëę".Contains(s))
			{
				return "e";
			}
			else if("ìíîïı".Contains(s))
			{
				return "i";
			}
			else if("òóôõöøőð".Contains(s))
			{
				return "o";
			}
			else if("ùúûüŭů".Contains(s))
			{
				return "u";
			}
			else if("çćčĉ".Contains(s))
			{
				return "c";
			}
			else if("żźž".Contains(s))
			{
				return "z";
			}
			else if("śşšŝ".Contains(s))
			{
				return "s";
			}
			else if("ñń".Contains(s))
			{
				return "n";
			}
			else if("ýÿ".Contains(s))
			{
				return "y";
			}
			else if("ğĝ".Contains(s))
			{
				return "g";
			}
			else if(c == 'ř')
			{
				return "r";
			}
			else if(c == 'ł')
			{
				return "l";
			}
			else if(c == 'đ')
			{
				return "d";
			}
			else if(c == 'ß')
			{
				return "ss";
			}
			else if(c == 'Þ')
			{
				return "th";
			}
			else if(c == 'ĥ')
			{
				return "h";
			}
			else if(c == 'ĵ')
			{
				return "j";
			}
			else
			{
				return "";
			}
		}
		#endregion

		public static string PrepareDirectory(string basePath, string ArticleNummer, string Kundenindex, Impersonate _impersonate)
		{
			try
			{
				if(!ValidateString(ArticleNummer))
					throw new InvalidOperationException("Invalid params");

				string Custemer = ArticleNummer.Split('-')[0];

				if(string.IsNullOrEmpty(Custemer) || string.IsNullOrEmpty(ArticleNummer))
					throw new InvalidOperationException("Invalid params");

				string combinePath = Path.Combine(new string[] { basePath, Custemer, ArticleNummer });
				using(var _safeAccessTokenHandle = GenerateSafeAccessTokenHandle())
				{
					if(_safeAccessTokenHandle is null || _safeAccessTokenHandle.IsInvalid)
						throw new InvalidOperationException("unable to impersonate or Invalid SafeAccessTokenHandle provided");

					WindowsIdentity.RunImpersonated(_safeAccessTokenHandle, () =>
					{

						if(!Directory.Exists(combinePath))
						{
							Directory.CreateDirectory(combinePath);
						}

					});
				}

				return combinePath;
			} catch(Exception caughtException)
			{
				Infrastructure.Services.Logging.Logger.Log(caughtException);
				throw;
			}
		}
		[SupportedOSPlatform("windows")]
		public static string PrepareDirectoryForSuppliersFiles(string basePath, string SupplierNr, string SupplierName)
		{
			try
			{
				SupplierName = FileHelper.ReplaceInvalidChars(SupplierName);
				if(SupplierName.Length == 0)
					throw new InvalidOperationException("Invalid Supplier Name");

				string combinePath = Path.Combine(new string[] { basePath, FileHelper.GenerateFolderNameForSupplierFiles(SupplierNr, SupplierName) });

				if(!Directory.Exists(combinePath))
					Directory.CreateDirectory(combinePath);

				using(var _safeAccessTokenHandle = GenerateSafeAccessTokenHandle())
				{
					if(_safeAccessTokenHandle is null || _safeAccessTokenHandle.IsInvalid)
						throw new InvalidOperationException("unable to impersonate or Invalid SafeAccessTokenHandle provided");

					WindowsIdentity.RunImpersonated(_safeAccessTokenHandle, () =>
					{
						if(!Directory.Exists(combinePath))
						{
							Directory.CreateDirectory(combinePath);
						}
					});
				}
				return combinePath;

			} catch(Exception caughtException)
			{
				Infrastructure.Services.Logging.Logger.Log(caughtException);
				throw;
			}
		}
		public static string PrepareDirectory(string path, Impersonate impersonate)
		{
			try
			{
				const int LOGON32_PROVIDER_DEFAULT = 0;
				//This parameter causes LogonUser to create a primary token.     
				const int LOGON32_LOGON_INTERACTIVE = 2;
				// Call LogonUser to obtain a handle to an access token.     
				SafeAccessTokenHandle safeAccessTokenHandle;
				Impersonate = impersonate;

				bool returnValue = LogonUser(Impersonate.ImpersonateUsername, Impersonate.ImpersonateDomain, Impersonate.ImpersonatePassword, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out safeAccessTokenHandle);
				WindowsIdentity.RunImpersonated(safeAccessTokenHandle, () =>
				{
					if(!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
				});
				return path;
			} catch(Exception caughtException)
			{
				Infrastructure.Services.Logging.Logger.Log(caughtException);
				throw;
			}
		}
		static bool ValidateString(string input)
		{
			string pattern = @"^[A-Za-z0-9]+-[A-Za-z0-9]+-[A-Za-z0-9]+$";
			return Regex.IsMatch(input, pattern);
		}
		[SupportedOSPlatform("windows")]
		public static bool SaveFileToRemoteDirectory(byte[] file, string basePath, string FileName)
		{
			if(file is null || string.IsNullOrWhiteSpace(basePath) || string.IsNullOrWhiteSpace(FileName))
			{
				return false;
			}
			try
			{
				using(var _safeAccessTokenHandle = GenerateSafeAccessTokenHandle())
				{
					if(_safeAccessTokenHandle is null || _safeAccessTokenHandle.IsInvalid)
						throw new InvalidOperationException("unable to impersonate or Invalid SafeAccessTokenHandle provided");

					SaveFilesToDirectory(new string[] { basePath, FileName }, file);
				}
				return true;
			} catch(Exception caughtException)
			{
				Infrastructure.Services.Logging.Logger.Log(caughtException);
				throw;
			}
		}
		private static void SaveFilesToDirectory(string[] paths, byte[] data)
		{
			File.WriteAllBytes(Path.Combine(paths), data);
		}
		[SupportedOSPlatform("windows")]
		public static List<string> GetFilesNameFromDirectory(string basePath, string artikelnummer, string kundenIndex, Impersonate _impersonate)
		{
			try
			{
				List<string> result = new List<string>();
				string combinePath = PrepareDirectory(basePath, artikelnummer, kundenIndex, _impersonate);
				Impersonate = _impersonate;
				const int LOGON32_PROVIDER_DEFAULT = 0;
				//This parameter causes LogonUser to create a primary token.     
				const int LOGON32_LOGON_INTERACTIVE = 2;
				// Call LogonUser to obtain a handle to an access token.     
				SafeAccessTokenHandle safeAccessTokenHandle;
				bool returnValue = LogonUser(Impersonate.ImpersonateUsername, Impersonate.ImpersonateDomain, Impersonate.ImpersonatePassword, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out safeAccessTokenHandle);
				WindowsIdentity.RunImpersonated(safeAccessTokenHandle, () =>
				{

					var files = Directory.GetFiles(combinePath);
					foreach(var item in files)
					{
						result.Add(GetFileName(item));
					}

				});

				return result;
			} catch(Exception caughtException)
			{
				Infrastructure.Services.Logging.Logger.Log(caughtException);
				throw;
			}

		}
		public static List<string> GetFilesNameFromDirectory(string basePath, string SupplierNr, string SupplierName)
		{
			try
			{

				string combinePath = Path.Combine(new string[] { basePath, FileHelper.GenerateFolderNameForSupplierFiles(SupplierNr, SupplierName) });
				if(!Directory.Exists(combinePath))
					Directory.CreateDirectory(combinePath);

				List<string> result = new List<string>();
				using(var _safeAccessTokenHandle = GenerateSafeAccessTokenHandle())
				{
					if(_safeAccessTokenHandle is null || _safeAccessTokenHandle.IsInvalid)
						throw new InvalidOperationException("unable to impersonate or Invalid SafeAccessTokenHandle provided");

					WindowsIdentity.RunImpersonated(_safeAccessTokenHandle, () =>
					{
						var files = Directory.GetFiles(combinePath);
						foreach(var item in files)
						{
							result.Add(GetFileName(item));
						}
					});
				}
				return result;
			} catch(Exception caughtException)
			{
				Infrastructure.Services.Logging.Logger.Log(caughtException);
				throw;
			}
		}
		private static string GetFileName(string path)
		{
			try
			{
				var filename = Path.GetFileNameWithoutExtension(path);
				var extension = Path.GetExtension(path);
				return filename + extension;
			} catch(Exception caughtException)
			{
				Infrastructure.Services.Logging.Logger.Log(caughtException);
				throw;
			}
		}
		[SupportedOSPlatform("windows")]
		public static byte[] ReadFileFromDisk(string basePath, string artikleNummer, string kudenIndex, string fileName, Impersonate _impersonate)
		{

			try
			{
				string combinePath = PrepareDirectory(basePath, artikleNummer, kudenIndex, _impersonate);
				string combineFilePath = Path.Combine(combinePath, fileName);
				byte[] fileBytes = null;

				using(var _safeAccessTokenHandle = GenerateSafeAccessTokenHandle())
				{
					if(_safeAccessTokenHandle is null || _safeAccessTokenHandle.IsInvalid)
						throw new InvalidOperationException("unable to impersonate or Invalid SafeAccessTokenHandle provided");

					WindowsIdentity.RunImpersonated(_safeAccessTokenHandle, () =>
					{
						if(!File.Exists(combineFilePath))
						{
							Infrastructure.Services.Logging.Logger.Log("File not existes = " + combineFilePath);
							throw new FileNotFoundException("File not found", combineFilePath);
						}
						else
						{
							fileBytes = File.ReadAllBytes(combineFilePath);
						}
					});
				}

				return fileBytes;
			} catch(Exception caughtException)
			{
				Infrastructure.Services.Logging.Logger.Log(caughtException);
				throw;
			}
		}
		[SupportedOSPlatform("windows")]
		public static byte[] ReadFileFromDisk(string basePath, string SupplierNr, string SuppplierName, string fileName)
		{

			try
			{
				byte[] fileBytes = null;
				string combinePath = Path.Combine(new string[] { basePath, FileHelper.GenerateFolderNameForSupplierFiles(SupplierNr, SuppplierName, fileName) });



				using(var _safeAccessTokenHandle = GenerateSafeAccessTokenHandle())
				{
					if(_safeAccessTokenHandle is null || _safeAccessTokenHandle.IsInvalid)
						throw new InvalidOperationException("unable to impersonate or Invalid SafeAccessTokenHandle provided");

					WindowsIdentity.RunImpersonated(_safeAccessTokenHandle, () =>
					{
						if(!File.Exists(combinePath))
						{
							Infrastructure.Services.Logging.Logger.Log("File not existes = " + combinePath);
							throw new FileNotFoundException("File not found", combinePath);
						}
						else
						{
							fileBytes = File.ReadAllBytes(combinePath);
						}
					});
				}

				return fileBytes;
			} catch(Exception caughtException)
			{
				Infrastructure.Services.Logging.Logger.Log(caughtException);
				throw;
			}
		}
		public static bool DeleteFilesNameFromDirectory(string basePath, string artikelnummer, string kundenIndex, string FileName, Impersonate _impersonate)
		{
			try
			{
				bool result = false;
				string combinePath = PrepareDirectory(basePath, artikelnummer, kundenIndex, _impersonate);
				string combineFilePath = Path.Combine(combinePath, FileName);
				Impersonate = _impersonate;
				const int LOGON32_PROVIDER_DEFAULT = 0;
				//This parameter causes LogonUser to create a primary token.     
				const int LOGON32_LOGON_INTERACTIVE = 2;
				// Call LogonUser to obtain a handle to an access token.     
				SafeAccessTokenHandle safeAccessTokenHandle;
				bool returnValue = LogonUser(Impersonate.ImpersonateUsername, Impersonate.ImpersonateDomain, Impersonate.ImpersonatePassword, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out safeAccessTokenHandle);
				WindowsIdentity.RunImpersonated(safeAccessTokenHandle, () =>
				{

					File.Delete(combineFilePath);
					result = true;
				});

				return result;
			} catch(Exception caughtException)
			{
				Infrastructure.Services.Logging.Logger.Log(caughtException);
				throw;
			}

		}
		[SupportedOSPlatform("wnidows")]
		public static bool DeleteFilesNameFromDirectory(string basePath, string SupplierNr, string SupplierName, string FileName)
		{
			try
			{
				bool result = false;

				string combinePath = Path.Combine(new string[] { basePath, FileHelper.GenerateFolderNameForSupplierFiles(SupplierNr, SupplierName, FileName) });


				using(var _safeAccessTokenHandle = GenerateSafeAccessTokenHandle())
				{
					if(_safeAccessTokenHandle is null || _safeAccessTokenHandle.IsInvalid)
						throw new InvalidOperationException("unable to impersonate or Invalid SafeAccessTokenHandle provided");

					WindowsIdentity.RunImpersonated(_safeAccessTokenHandle, () =>
					{
						if(File.Exists(combinePath))
						{
							File.Delete(combinePath);
						}
						result = true;
					});
				}

				return result;
			} catch(Exception caughtException)
			{
				Infrastructure.Services.Logging.Logger.Log(caughtException);
				throw;
			}

		}
		[SupportedOSPlatform("windows")]
		public static SafeAccessTokenHandle GenerateSafeAccessTokenHandle()
		{
			if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				throw new PlatformNotSupportedException("This operation is valid only for windows based OS !");

			SafeAccessTokenHandle safeAccessTokenHandle;

			if(LogonUser(Impersonate.ImpersonateUsername, Impersonate.ImpersonateDomain, Impersonate.ImpersonatePassword, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out safeAccessTokenHandle))
			{
				return safeAccessTokenHandle;
			}
			else
			{
				return null;
			}
		}
	}
	public static class Utils
	{
		public static string SanitizePath(string path)
		{
			// Keep the drive part intact (C:/ or D:/) and sanitize the rest
			string sanitizedFilePath = Regex.Replace(path.Substring(3), @"[<>:""\|/?*]", "_");

			// Rebuild the path with the original drive letter
			sanitizedFilePath = path.Substring(0, 3) + sanitizedFilePath;

			return sanitizedFilePath;
		}
	}

}
public class Impersonate
{
	public string ImpersonateUsername { get; set; }
	public string ImpersonatePassword { get; set; }
	public string ImpersonateDomain { get; set; }
}
