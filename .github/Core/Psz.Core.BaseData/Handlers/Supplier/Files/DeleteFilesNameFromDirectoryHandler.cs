using Infrastructure.Data.Entities.Tables._Commun;
using Psz.Core.BaseData.Models.Files;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Supplier.Files;

public class DeleteFileAsyncHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<bool>>
{
	private Identity.Models.UserModel _user { get; set; }
	private DeleteSupplierFileModel _data { get; set; }
	private System.Threading.CancellationToken _token { get; set; }
	public DeleteFileAsyncHandler(Identity.Models.UserModel user, DeleteSupplierFileModel data, System.Threading.CancellationToken token = default)
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
			//var lieferentenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.Id);

			var validationResponse = await this.ValidateAsync();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			//var filesPerSupplier = _BSD_Suppliers_Files_IdsAccess.Get(_data.Id);
			var allEntities = Infrastructure.Data.Access.Tables._Commun.__BSD_Attachements_TrackingAccess.GetWithTransaction(_data.Id, botransaction.connection, botransaction.transaction);
			// FIRE AND FORGET --> NO NEED TO AWAIT IT but we will await it :( -->
			await Psz.Core.Common.Program.FilesManager.DeleteFileMinio(_user.Id, allEntities.FileId, _token);
			Infrastructure.Data.Access.Tables.FileAccess.DeleteWithTransaction(allEntities.FileId, botransaction.connection, botransaction.transaction);
			Infrastructure.Data.Access.Tables._Commun.__BSD_Attachements_TrackingAccess.DeleteWithTransaction(allEntities.ID, botransaction.connection, botransaction.transaction);

			/*var logsEntities = Infrastructure.Services.Files.FileFactory.FilesAndModulesNameRelation.GenerateFilesTrackingLogEntity(new List<Infrastructure.Data.Entities.Tables._Commun.__BSD_Attachements_TrackingEntity>() { allEntities }, "Removed", allEntities.Module ?? -1, allEntities.ModuleId ?? -1, _user.Id);
			Infrastructure.Data.Access.Tables._Commun.__BSD_Attachements_Tracking_LogAccess.InsertWithTransaction(logsEntities, botransaction.connection, botransaction.transaction);*/

			SaveLogs(new List<__BSD_Attachements_TrackingEntity>() { allEntities }, allEntities.Module ?? -1, allEntities.ModuleId ?? -1, botransaction.connection, botransaction.transaction);

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

	private void GenerateLogsForOrderFiles(List<__BSD_Attachements_TrackingEntity> data, Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity entity, SqlConnection connection, SqlTransaction transaction)
	{
		/* Generating Logs*/
		var _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
		foreach(var item in data)
		{
			_logs.Add(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity() { Nr = entity.Nr, AngebotNr = entity.Angebot_Nr, DateTime = DateTime.Now, LogObject = $"[PRS]", LogText = $"[Rahmenauftrag][{item.ModuleId}]  File Named [{item.FileName}] Removed ", UserId = _user.Id, Username = _user.Username });
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
				ObjectLogHelper.getLog(this._user, item.ModuleId ?? -1, "File Named  :", "", $"[{item.FileName}] Removed ", Enums.ObjectLogEnums.Objects.Supplier.GetDescription(), Enums.ObjectLogEnums.LogType.Add)
				);
		}
		Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, connection, transaction);
		/* End Generating Logs*/
	}
	private void SaveLogs(List<__BSD_Attachements_TrackingEntity> data,int Module , int ModuleId, SqlConnection connection, SqlTransaction transaction)
	{
		if(Infrastructure.Services.Files.FileFactory.FilesAndModulesNameRelation.GetModuleName(Module) == Infrastructure.Services.Files.FileFactory.ModulesInAttachements.Suppliers)
		{
			GenerateLogForSupplierFiles(data, connection, transaction);
		}
		else if(Infrastructure.Services.Files.FileFactory.FilesAndModulesNameRelation.GetModuleName(Module) == Infrastructure.Services.Files.FileFactory.ModulesInAttachements.Rahmen)
		{
			GenerateLogsForOrderFiles(data, Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(ModuleId), connection, transaction);
		}
	}




	public async Task<ResponseModel<bool>> ValidateAsync()
	{
		if(this._user == null || !(this._user.Access?.MasterData?.ArticleDeleteCustomerDocument == true || this._user.IsGlobalDirector == true || this._user.IsAdministrator || this._user.SuperAdministrator))
		{
			return await ResponseModel<bool>.AccessDeniedResponseAsync();
		}

		return await ResponseModel<bool>.SuccessResponseAsync();
	}
}
