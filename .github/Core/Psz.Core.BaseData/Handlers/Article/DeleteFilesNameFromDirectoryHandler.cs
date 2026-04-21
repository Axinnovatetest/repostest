using Psz.Core.BaseData.Models.Article;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Psz.Core.BaseData.Handlers.Article
{
	public class DeleteFilesNameFromDirectoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<bool>>
	{

		private Identity.Models.UserModel _user { get; set; }
		private DeleteArticleFileModel _data { get; set; }
		public DeleteFilesNameFromDirectoryHandler(Identity.Models.UserModel user, DeleteArticleFileModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<bool> Handle()
		{
			try
			{
				if(_data is null)
				{
					return ResponseModel<bool>.FailureResponse("null data");
				}
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(_data.ArtiKleNummer);
				if(articleEntity is null)
				{
					return ResponseModel<bool>.FailureResponse($"Article [{_data.ArtiKleNummer}] is not found.");
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
				var result = Infrastructure.Services.Files.FilesManager.DeleteFilesNameFromDirectory(Bp, _data.ArtiKleNummer, _data.KundenIndex, _data.FileName, impersonate);


				if(result)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(ObjectLogHelper.getLog(this._user, articleEntity.ArtikelNr, "KundenDoc", $"{_data.FileName}", "", Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Delete));
					return ResponseModel<bool>.SuccessResponse(result);
				}
				else

					return ResponseModel<bool>.FailureResponse("Failed");


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return ResponseModel<bool>.FailureResponse(e.Message);
				throw;
			}

		}

		public ResponseModel<bool> Validate()
		{
			if(this._user == null || !(this._user.Access?.MasterData?.ArticleDeleteCustomerDocument == true || this._user.IsGlobalDirector == true || this._user.SuperAdministrator == true))
			{
				return ResponseModel<bool>.AccessDeniedResponse();
			}

			return ResponseModel<bool>.SuccessResponse();
		}

	}
}
