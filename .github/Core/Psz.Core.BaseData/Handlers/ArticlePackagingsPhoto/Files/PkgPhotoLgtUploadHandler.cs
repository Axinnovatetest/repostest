using Infrastructure.Data.Entities.Tables._Commun;
using Infrastructure.Services.Files;
using Psz.Core.BaseData.Models.Files;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.ArticlePackagingsPhoto.Files;

public class PkgPhotoLgtUploadHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<int>>
{
	private Identity.Models.UserModel _user { get; set; }
	private PkgPhotoLgtUploadModel _data { get; set; }
	private CancellationToken _token { get; set; }
	public PkgPhotoLgtUploadHandler(Identity.Models.UserModel user, PkgPhotoLgtUploadModel data, CancellationToken token)
	{
		_user = user;
		_data = data;
		_token = token;
	}
	public async Task<ResponseModel<int>> HandleAsync()
	{
		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
		try
		{
			var validationResponse = await this.ValidateAsync();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			botransaction.beginTransaction();


			var alloweddExtensions = MimeHelper.GetAllowedPhotosExtensions();
			foreach(var item in _data.AttachmentFile)
			{
				if(!alloweddExtensions.Contains(Path.GetExtension(item.FileName).ToLower()))
				{
					return await ResponseModel<int>.FailureResponseAsync("this extension is not allowed !").ConfigureAwait(false);
				}
			}
			bool result = true;
			var packagingsDataToSave = new List<_MTD_PckgPhoto_attachementsEntity>();
			this._token.ThrowIfCancellationRequested();
			foreach(var file in _data.AttachmentFile)
			{
				var fileName = PkgPhotoLgtUploadModel.getFileName(file);
				var byteData = PkgPhotoLgtUploadModel.getBytes(file);

				var newFileId = await Psz.Core.Common.Program.FilesManager.NewFileWithMinioWithStatusCode(this._user.Id, byteData, Path.GetExtension(file.FileName), 1, fileName).ConfigureAwait(false);
				if(newFileId.Item2.Etag is null || newFileId.Item2.Etag.Length <= 10)
					return await ResponseModel<int>.FailureResponseAsync("file server is not accessible !").ConfigureAwait(false);

				packagingsDataToSave.Add(new Infrastructure.Data.Entities.Tables._Commun._MTD_PckgPhoto_attachementsEntity() { FileName = fileName, UserId = this._user.Id, ArticleId = _data.ArticleId, FileId = newFileId.Item1, UploadedDate = DateTime.Now, isActive = true });
			}

			Infrastructure.Data.Access.Tables._Commun._MTD_PckgPhoto_attachementsAccess.InsertWithTransaction(packagingsDataToSave, botransaction.connection, botransaction.transaction);

			if(result)
			{
				//SaveLogs(supplierDataToSave, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{
					return await ResponseModel<int>.SuccessResponseAsync(1);
				}
			}
			botransaction.rollback();
			return await ResponseModel<int>.FailureResponseAsync("problem Occurred");
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			return await ResponseModel<int>.FailureResponseAsync(e.Message);
			throw;
		}
	}

	//private void GenerateLogForSupplierFiles(List<__BSD_Attachements_TrackingEntity> data, SqlConnection connection, SqlTransaction transaction)
	//{
	//	/* Generating Logs*/
	//	var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
	//	foreach(var item in data)
	//	{
	//		logs.Add(
	//			ObjectLogHelper.getLog(this._user, item.ModuleId ?? -1, "Attachments", "", $"[{_data.AttachmentFile.Count}] new files", Enums.ObjectLogEnums.Objects.Supplier.GetDescription(), Enums.ObjectLogEnums.LogType.Add)
	//			);
	//	}
	//	Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, connection, transaction);
	//	/* End Generating Logs*/
	//}

	public Task<ResponseModel<int>> ValidateAsync()
	{
		if(this._user == null || !(this._user.IsAdministrator || this._user.SuperAdministrator || this._user.IsGlobalDirector == true || this._user.Access?.MasterData?.PackagingsLgtPhotoAdd == true))
		{
			return ResponseModel<int>.AccessDeniedResponseAsync();
		}
		if(_data is null || _data.AttachmentFile is null || _data.AttachmentFile.Count == 0)
		{
			return ResponseModel<int>.FailureResponseAsync("null data");
		}
		return ResponseModel<int>.SuccessResponseAsync();
	}
}
