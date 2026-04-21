using Infrastructure.Services.Files;
using Psz.Core.BaseData.Models.Article;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.IO;

namespace Psz.Core.BaseData.Handlers.Article
{
	public class ArticleFilesUploadHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ArticleFilesUploadModel _data { get; set; }
		public ArticleFilesUploadHandler(Identity.Models.UserModel user, ArticleFilesUploadModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				if(_data is null || _data.AttachmentFile is null || _data.AttachmentFile.Count == 0)
				{
					return ResponseModel<int>.FailureResponse("null data");
				}
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(_data.ArtiKleNummer);
				if(articleEntity is null)
				{
					return ResponseModel<int>.FailureResponse($"Article [{_data.ArtiKleNummer}] is not found.");
				}
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var restrictedExtensions = MimeHelper.GetrestrictedExtensions();
				foreach(var item in _data.AttachmentFile)
				{
					if(restrictedExtensions.Contains(Path.GetExtension(item.FileName)))
					{
						return ResponseModel<int>.FailureResponse("this extension is not allowed on the server");

					}
				}
				bool result = true;
				string Bp = Module.serverPath.BasePath.ToString();
				var impersonate = new Impersonate()
				{
					ImpersonateUsername = Module.impersonate.ImpersonateUsername,
					ImpersonatePassword = Module.impersonate.ImpersonatePassword,
					ImpersonateDomain = Module.impersonate.ImpersonateDomain
				};
				string basePath = Infrastructure.Services.Files.FilesManager.PrepareDirectory(Bp, _data.ArtiKleNummer, _data.KundenIndex, impersonate);
				foreach(var file in _data.AttachmentFile)
				{
					var fileName = ArticleFilesUploadModel.getFileName(file);
					var byteData = ArticleFilesUploadModel.getBytes(file);
					result = result && Infrastructure.Services.Files.FilesManager.SaveFileToRemoteDirectory(byteData, basePath, fileName);
				}


				if(result)
				{
					// -
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(ObjectLogHelper.getLog(this._user, articleEntity.ArtikelNr, "KundenDoc", "", $"[{_data.AttachmentFile.Count}] new files", Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Add));
					return ResponseModel<int>.SuccessResponse(1);
				}
				else
					return ResponseModel<int>.FailureResponse("Failed");


			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				return ResponseModel<int>.FailureResponse(e.Message);
				throw;
			}

		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null || !(this._user.Access?.MasterData?.ArticleAddCustomerDocument == true || this._user.IsGlobalDirector == true || this._user.SuperAdministrator == true))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
