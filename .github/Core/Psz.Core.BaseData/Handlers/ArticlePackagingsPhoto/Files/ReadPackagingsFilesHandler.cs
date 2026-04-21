using Psz.Core.BaseData.Models.Files;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.ArticlePackagingsPhoto.Files;

public class ReadPackagingsFilesHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<DownloadPkgPhotoFileResponseModel>>
{
	private Identity.Models.UserModel _user { get; set; }
	private int? _Id { get; set; }
	private CancellationToken _token { get; set; }
	public ReadPackagingsFilesHandler(Identity.Models.UserModel user,int Id, CancellationToken token)
	{
		_user = user;
		_Id = Id;
		_token = token;
	}
	public async Task<ResponseModel<DownloadPkgPhotoFileResponseModel>> HandleAsync()
	{
		try
		{
			if(_Id is null)
			{
				return await ResponseModel<DownloadPkgPhotoFileResponseModel>.FailureResponseAsync("invalid  data !");
			}
			var validationResponse = await this.ValidateAsync();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			//var fileEntity = _BSD_Suppliers_Files_IdsAccess.Get(_data.FileName);
			var fileEntity = Infrastructure.Data.Access.Tables._Commun._MTD_PckgPhoto_attachementsAccess.Get(_Id ?? -1);

			if(fileEntity is null || fileEntity.Id <= 0)
				return await ResponseModel<DownloadPkgPhotoFileResponseModel>.FailureResponseAsync("Failed");
			// cancel operation before initiate server call (Db & Minio)
			_token.ThrowIfCancellationRequested();
			var file = await Psz.Core.Common.Program.FilesManager.GetFileMinioWithStatus(this._user.Id, fileEntity.FileId).ConfigureAwait(false);

			if(file is null || !file.Item1)
			{
				return await ResponseModel<DownloadPkgPhotoFileResponseModel>.FailureResponseAsync("Corrupted file or file server inaccessible");
			}

			DownloadPkgPhotoFileResponseModel downloadArticleFileResponseModel = new DownloadPkgPhotoFileResponseModel()
			{
				file = file.Item2.FileBytes,
				extension = Path.GetExtension(fileEntity.FileName),
				MIMEtype = Infrastructure.Services.Files.MimeHelper.GetMimeType(Path.GetExtension(fileEntity.FileName)),
				FileName = fileEntity.FileName,
			};

			return await ResponseModel<DownloadPkgPhotoFileResponseModel>.SuccessResponseAsync(downloadArticleFileResponseModel);



		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			return await ResponseModel<DownloadPkgPhotoFileResponseModel>.FailureResponseAsync(e.Message);
			throw;
		}
	}

	public Task<ResponseModel<DownloadPkgPhotoFileResponseModel>> ValidateAsync()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<DownloadPkgPhotoFileResponseModel>.AccessDeniedResponseAsync();
		}

		return ResponseModel<DownloadPkgPhotoFileResponseModel>.SuccessResponseAsync();
	}
}

