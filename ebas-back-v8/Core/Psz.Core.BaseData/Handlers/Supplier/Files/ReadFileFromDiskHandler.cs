using Infrastructure.Data.Access.Tables.BSD;
using Psz.Core.BaseData.Models.Files;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Supplier.Files;

public class ReadFileFromDiskAsyncHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<DownloadSupplierFileResponseModel>>
{
	private Identity.Models.UserModel _user { get; set; }
	private DownloadSupplierFileModel _data { get; set; }
	private CancellationToken _token { get; set; }
	public ReadFileFromDiskAsyncHandler(Identity.Models.UserModel user, DownloadSupplierFileModel data, CancellationToken token)
	{
		_user = user;
		_data = data;
		_token = token;
	}
	public async Task<ResponseModel<DownloadSupplierFileResponseModel>> HandleAsync()
	{
		try
		{
			if(_data is null)
			{
				return await ResponseModel<DownloadSupplierFileResponseModel>.FailureResponseAsync("invalid  data !");
			}
			var validationResponse = await this.ValidateAsync();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			//var fileEntity = _BSD_Suppliers_Files_IdsAccess.Get(_data.FileName);
			var fileEntity = Infrastructure.Data.Access.Tables._Commun.__BSD_Attachements_TrackingAccess.Get(_data.Module);
			
			if(fileEntity is null || fileEntity.ID <= 0)
				return await ResponseModel<DownloadSupplierFileResponseModel>.FailureResponseAsync("Failed");
			// cancel operation before initiate server call (Db & Minio)
			_token.ThrowIfCancellationRequested();
			var file = await Psz.Core.Common.Program.FilesManager.GetFileMinioWithStatus(this._user.Id, fileEntity.FileId).ConfigureAwait(false);

			if(file is null || !file.Item1)
			{
				return await ResponseModel<DownloadSupplierFileResponseModel>.FailureResponseAsync("Corrupted file or file server inaccessible");
			}

			DownloadSupplierFileResponseModel downloadArticleFileResponseModel = new DownloadSupplierFileResponseModel()
			{
				file = file.Item2.FileBytes,
				extension = Path.GetExtension(fileEntity.FileName),
				MIMEtype = Infrastructure.Services.Files.MimeHelper.GetMimeType(Path.GetExtension(fileEntity.FileName)),
				FileName = fileEntity.FileName,
			};

			return await ResponseModel<DownloadSupplierFileResponseModel>.SuccessResponseAsync(downloadArticleFileResponseModel);



		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			return await ResponseModel<DownloadSupplierFileResponseModel>.FailureResponseAsync(e.Message);
			throw;
		}
	}

	public Task<ResponseModel<DownloadSupplierFileResponseModel>> ValidateAsync()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<DownloadSupplierFileResponseModel>.AccessDeniedResponseAsync();
		}

		return ResponseModel<DownloadSupplierFileResponseModel>.SuccessResponseAsync();
	}
}

