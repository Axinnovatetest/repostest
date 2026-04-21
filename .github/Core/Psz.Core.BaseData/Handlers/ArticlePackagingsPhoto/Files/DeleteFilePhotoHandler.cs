using System;
using Psz.Core.BaseData.Models.Files;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.ArticlePackagingsPhoto.Files;

public class DeleteFilePhotoHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<bool>>
{
	private Identity.Models.UserModel _user { get; set; }
	private DeletePkgPhotoFileModel _data { get; set; }
	private System.Threading.CancellationToken _token { get; set; }
	public DeleteFilePhotoHandler(Identity.Models.UserModel user, DeletePkgPhotoFileModel data, System.Threading.CancellationToken token = default)
	{
		_user = user;
		_data = data;
		_token = token;
	}

	public async Task<ResponseModel<bool>> HandleAsync()
	{

		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
		try
		{
			botransaction.beginTransaction();

			if(_data is null)
			{
				return await ResponseModel<bool>.FailureResponseAsync("null data");
			}


			var validationResponse = await this.ValidateAsync();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var allEntities = Infrastructure.Data.Access.Tables._Commun._MTD_PckgPhoto_attachementsAccess.GetWithTransaction(_data.Id, botransaction.connection, botransaction.transaction);

			await Psz.Core.Common.Program.FilesManager.DeleteFileMinio(_user.Id, allEntities.FileId, _token);
			Infrastructure.Data.Access.Tables.FileAccess.DeleteWithTransaction(allEntities.FileId, botransaction.connection, botransaction.transaction);
			Infrastructure.Data.Access.Tables._Commun._MTD_PckgPhoto_attachementsAccess.DeleteWithTransaction(allEntities.Id,allEntities.ArticleId ?? -1, botransaction.connection, botransaction.transaction);

			if(botransaction.commit())
			{

				return await ResponseModel<bool>.SuccessResponseAsync(true);
			}
			else
			{
				return await ResponseModel<bool>.FailureResponseAsync("problem occured");
			}


		} catch(Exception e)
		{
			botransaction.rollback();
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}


	public async Task<ResponseModel<bool>> ValidateAsync()
	{
		if(this._user == null || !(this._user.Access?.MasterData?.PackagingsLgtPhotoDelete == true || this._user.IsGlobalDirector == true || this._user.IsAdministrator || this._user.SuperAdministrator))
		{
			return await ResponseModel<bool>.AccessDeniedResponseAsync();
		}

		return await ResponseModel<bool>.SuccessResponseAsync();
	}
}
