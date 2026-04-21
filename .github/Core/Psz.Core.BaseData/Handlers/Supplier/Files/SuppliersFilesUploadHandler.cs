using Infrastructure.Data.Entities.Tables._Commun;
using Infrastructure.Data.Entities.Tables.CTS;
using Infrastructure.Services.Files;
using Infrastructure.Services.Files.FileFactory;
using Psz.Core.BaseData.Models.Files;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Supplier.Files;

public class SuppliersFilesUploadAsyncHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<int>>
{
	private Identity.Models.UserModel _user { get; set; }
	private SupplierFilesUploadModel _data { get; set; }
	private CancellationToken _token { get; set; }
	public SuppliersFilesUploadAsyncHandler(Identity.Models.UserModel user, SupplierFilesUploadModel data, CancellationToken token)
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
			if(!Infrastructure.Services.Files.FileFactory.FilesAndModulesNameRelation.ValidateEntityToUpdateExists(_data.ModuleId, _data.Module))
			{
				return await ResponseModel<int>.FailureResponseAsync($"The Requested Module or entity does not exist.").ConfigureAwait(false);
			}

			botransaction.beginTransaction();
			//var lieferentenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.SupplierNr);

			var restrictedExtensions = MimeHelper.GetrestrictedExtensions();
			foreach(var item in _data.AttachmentFile)
			{
				if(restrictedExtensions.Contains(Path.GetExtension(item.FileName)))
				{
					return await ResponseModel<int>.FailureResponseAsync("this extension is not allowed on the server").ConfigureAwait(false);
				}
			}
			bool result = true;
			var supplierDataToSave = new List<__BSD_Attachements_TrackingEntity>();
			this._token.ThrowIfCancellationRequested();
			foreach(var file in _data.AttachmentFile)
			{
				var fileName = SupplierFilesUploadModel.getFileName(file);
				var byteData = SupplierFilesUploadModel.getBytes(file);

				var newFileId = await Psz.Core.Common.Program.FilesManager.NewFileWithMinioWithStatusCode(this._user.Id, byteData, Path.GetExtension(file.FileName), 1, fileName).ConfigureAwait(false);
				if(newFileId.Item2.Etag is null || newFileId.Item2.Etag.Length <= 10)
					return await ResponseModel<int>.FailureResponseAsync("file server is not accessible !").ConfigureAwait(false);

				supplierDataToSave.Add(new Infrastructure.Data.Entities.Tables._Commun.__BSD_Attachements_TrackingEntity() { FileName = fileName, UserId = this._user.Id, FileId = newFileId.Item1, isActive = true, Module = _data.Module, ModuleId = _data.ModuleId, UploadedDate = DateTime.Now, });
			}

			Infrastructure.Data.Access.Tables._Commun.__BSD_Attachements_TrackingAccess.InsertWithTransaction(supplierDataToSave, botransaction.connection, botransaction.transaction);

			if(result)
			{
				SaveLogs(supplierDataToSave, botransaction.connection, botransaction.transaction);

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
	private void GenerateLogsForOrderFiles(List<__BSD_Attachements_TrackingEntity> data, Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity entity, SqlConnection connection, SqlTransaction transaction)
	{
		/* Generating Logs*/
		var _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
		foreach(var item in data)
		{
			_logs.Add(new OrderProcesssing_LogEntity() { Nr = entity.Nr, AngebotNr = entity.Angebot_Nr, DateTime = DateTime.Now, LogObject = "[PRS]", LogText = $" [Rahmenauftrag][{item.ModuleId}] New File Named [{item.FileName}] Uploaded  ", UserId = _user.Id, Username = _user.Username });
		}
		Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_logs, connection, transaction);
		/* End Generating Logs*/
	}
	private void GenerateLogForSupplierFiles(List<__BSD_Attachements_TrackingEntity> data, SqlConnection connection, SqlTransaction transaction)
	{
		/* Generating Logs*/
		var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
		foreach(var item in data)
		{
			logs.Add(
				ObjectLogHelper.getLog(this._user, item.ModuleId ?? -1, "Attachments", "", $"[{_data.AttachmentFile.Count}] new files", Enums.ObjectLogEnums.Objects.Supplier.GetDescription(), Enums.ObjectLogEnums.LogType.Add)
				);
		}
		Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, connection, transaction);
		/* End Generating Logs*/
	}
	private void SaveLogs(List<__BSD_Attachements_TrackingEntity> data, SqlConnection connection, SqlTransaction transaction)
	{
		if(Infrastructure.Services.Files.FileFactory.FilesAndModulesNameRelation.GetModuleName(this._data.Module) == ModulesInAttachements.Suppliers)
		{
			GenerateLogForSupplierFiles(data, connection, transaction);
		}
		else if(Infrastructure.Services.Files.FileFactory.FilesAndModulesNameRelation.GetModuleName(this._data.Module) == ModulesInAttachements.Rahmen)
		{
			GenerateLogsForOrderFiles(data, Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.ModuleId), connection, transaction);
		}
	}
	public Task<ResponseModel<int>> ValidateAsync()
	{
		if(this._user == null || !(this._user.IsAdministrator || this._user.SuperAdministrator || this._user.IsGlobalDirector == true || this._user.Access?.MasterData?.SupplierAttachementAddFile == true))
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
