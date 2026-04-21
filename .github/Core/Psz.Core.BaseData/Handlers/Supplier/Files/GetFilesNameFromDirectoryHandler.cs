using Infrastructure.Data.Access.Tables.BSD;
using Psz.Core.BaseData.Models.Article;
using Psz.Core.BaseData.Models.Files;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Psz.Core.BaseData.Handlers.Supplier.Files;

[SupportedOSPlatform("windows")]
public class GetFilesNameFromDirectoryAsyncHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
{

	private Identity.Models.UserModel _user { get; set; }
	private GetSupplierAttachmentModel _data { get; set; }
	private System.Threading.CancellationToken? _token { get; set; }
	public GetFilesNameFromDirectoryAsyncHandler(Identity.Models.UserModel user, GetSupplierAttachmentModel data, System.Threading.CancellationToken? token = null)
	{
		_user = user;
		_data = data;
		_token = token;
	}
	public async Task<ResponseModel<List<KeyValuePair<int, string>>>> HandleAsync()
	{
		try
		{
			if(_data is null)
			{
				return await ResponseModel<List<KeyValuePair<int, string>>>.FailureResponseAsync("null data");
			}
			var validationResponse = await ValidateAsync();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			////
			var allEntities = Infrastructure.Data.Access.Tables._Commun.__BSD_Attachements_TrackingAccess.GetByModuleAndModuleIdAllFiles(_data.ModuleId,_data.Module);
			//var filesPerSupplier = _BSD_Suppliers_Files_IdsAccess.GetBySupplierNr(_data.SupplierNr);
			//var files = Infrastructure.Data.Access.Tables.FileAccess.Get(filesPerSupplier.Select(x => x.ID).ToList());
			//var restoreturn = files.Select(x => new KeyValuePair<int, string>(x.Id, filesPerSupplier.Where(y => y.FileId == x.Id).First().FileName)).ToList();
			var restoreturn = allEntities.Select(x=> new KeyValuePair<int,string>(x.ID,x.FileName)).ToList();

			if(restoreturn is not null || restoreturn.Count > 0)
				return await ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponseAsync(restoreturn);
			else
				return await ResponseModel<List<KeyValuePair<int, string>>>.FailureResponseAsync("Failed");

		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			return await ResponseModel<List<KeyValuePair<int, string>>>.FailureResponseAsync(e.Message);
			throw;
		}
	}
	public Task<ResponseModel<List<KeyValuePair<int, string>>>> ValidateAsync()
	{
		if(_user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponseAsync();
		}
		return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponseAsync();
	}
}
