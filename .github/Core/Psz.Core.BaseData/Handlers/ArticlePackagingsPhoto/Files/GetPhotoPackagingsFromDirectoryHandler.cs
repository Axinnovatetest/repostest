
using Psz.Core.BaseData.Models.Article.Statistics.CustomerService;
using Psz.Core.BaseData.Models.Files;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;


namespace Psz.Core.BaseData.Handlers.ArticlePackagingsPhoto.Files;

[SupportedOSPlatform("windows")]
public class GetPhotoPackagingsFromDirectoryHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<List<ResponsePkgPhotoFileModelWithId>>>
{

	private Identity.Models.UserModel _user { get; set; }
	private System.Threading.CancellationToken? _token { get; set; }
	private int _articleId;
	public GetPhotoPackagingsFromDirectoryHandler(Identity.Models.UserModel user, int articleId, System.Threading.CancellationToken? token = null)
	{
		_user = user;
		_articleId = articleId;
		_token = token;
	}
	public async Task<ResponseModel<List<ResponsePkgPhotoFileModelWithId>>> HandleAsync()
	{
		try
		{
			if(_articleId == 0)
			{
				return await ResponseModel<List<ResponsePkgPhotoFileModelWithId>>.FailureResponseAsync("null article");
			}
			var validationResponse = await ValidateAsync();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var allEntities = Infrastructure.Data.Access.Tables._Commun._MTD_PckgPhoto_attachementsAccess.GetWithArticle(_articleId);
			var restoreturn = allEntities.Select(x => new  ResponsePkgPhotoFileModelWithId(
				x.FileId,
				x.FileName ?? "unknown_file",
				x.Id,
				x.UploadedDate
				)).ToList();

			if(restoreturn is not null)
				return await ResponseModel<List<ResponsePkgPhotoFileModelWithId>>.SuccessResponseAsync(restoreturn);
			else
				return await ResponseModel<List<ResponsePkgPhotoFileModelWithId>>.FailureResponseAsync("Failed");

		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			return await ResponseModel<List<ResponsePkgPhotoFileModelWithId>>.FailureResponseAsync(e.Message);
			throw;
		}
	}
	public Task<ResponseModel<List<ResponsePkgPhotoFileModelWithId>>> ValidateAsync()
	{
		if(_user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<List<ResponsePkgPhotoFileModelWithId>>.AccessDeniedResponseAsync();
		}
		return ResponseModel<List<ResponsePkgPhotoFileModelWithId>>.SuccessResponseAsync();
	}
}
