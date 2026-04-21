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
	public class GetFilesNameFromDirectoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<string>>>
	{

		private Identity.Models.UserModel _user { get; set; }
		private GetArticleAttachmentModel _data { get; set; }
		public GetFilesNameFromDirectoryHandler(Identity.Models.UserModel user, GetArticleAttachmentModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<List<string>> Handle()
		{
			try
			{
				if(_data is null)
				{
					return ResponseModel<List<string>>.FailureResponse("null data");
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
				var result = Infrastructure.Services.Files.FilesManager.GetFilesNameFromDirectory(Bp, _data.ArtiKleNummer, _data.KundenIndex, impersonate);



				if(result is not null || result.Count > 0)
					return ResponseModel<List<string>>.SuccessResponse(result);

				else

					return ResponseModel<List<string>>.FailureResponse("Failed");


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return ResponseModel<List<string>>.FailureResponse(e.Message);
				throw;
			}

		}

		public ResponseModel<List<string>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<string>>.AccessDeniedResponse();
			}

			return ResponseModel<List<string>>.SuccessResponse();
		}


	}
}
