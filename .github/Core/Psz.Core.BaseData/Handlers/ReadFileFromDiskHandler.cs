using Psz.Core.BaseData.Models.Article;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers
{
	public class ReadFileFromDiskHandler: IHandle<Identity.Models.UserModel, ResponseModel<DownloadArticleFileResponseModel>>
	{

		private Identity.Models.UserModel _user { get; set; }
		private DownloadArticleFileModel _data { get; set; }
		public ReadFileFromDiskHandler(Identity.Models.UserModel user, DownloadArticleFileModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<DownloadArticleFileResponseModel> Handle()
		{
			try
			{
				if(_data is null)
				{
					return ResponseModel<DownloadArticleFileResponseModel>.FailureResponse("null data");
				}
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				string Bp = Module.serverPath.BasePath.ToString();
				var impersonate = new Impersonate()
				{
					ImpersonateUsername = Module.impersonate.ImpersonateUsername,
					ImpersonatePassword = Module.impersonate.ImpersonatePassword,
					ImpersonateDomain = Module.impersonate.ImpersonateDomain
				};
				var result = Infrastructure.Services.Files.FilesManager.ReadFileFromDisk(Bp, _data.ArtiKleNummer, _data.KundenIndex, _data.FileName, impersonate);

				if(result is not null)
				{
					DownloadArticleFileResponseModel downloadArticleFileResponseModel = new DownloadArticleFileResponseModel()
					{
						file = result,
						extention = Path.GetExtension(_data.FileName),
						MIMEtype = Infrastructure.Services.Files.MimeHelper.GetMimeType(Path.GetExtension(_data.FileName)),
						FileName = _data.FileName,
					};
					return ResponseModel<DownloadArticleFileResponseModel>.SuccessResponse(downloadArticleFileResponseModel);
				}
				else

					return ResponseModel<DownloadArticleFileResponseModel>.FailureResponse("Failed");


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return ResponseModel<DownloadArticleFileResponseModel>.FailureResponse(e.Message);
				throw;
			}

		}

		public ResponseModel<DownloadArticleFileResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<DownloadArticleFileResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<DownloadArticleFileResponseModel>.SuccessResponse();
		}


	}
}

