using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetWerkUpdateReportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public GetWerkUpdateReportHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				byte[] responseBody = null;
				var Lists = new Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel();
				var updateEntity = Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_UpdateAccess.Get(this._data);
				var UpdateDetails = new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
				List<Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel> Updated = new List<Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel>();
				List<Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedFromExcelModel> Notupdated = new List<Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedFromExcelModel>();
				if(updateEntity != null)
				{
					UpdateDetails = Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_Update_detailsAccess.GetByUpdateId(this._data);
				}
				if(UpdateDetails != null && UpdateDetails.Count > 0)
				{
					var FANummers = UpdateDetails.Select(x => (int)x.FA).ToList();
					var FAEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFAIds(FANummers);
					var UpdatedOrdersEntity = UpdateDetails?.Where(x => x.updated.Value).ToList();
					var NotupdatedOrdersEntity = UpdateDetails?.Where(x => !x.updated.Value).ToList();

					if(UpdatedOrdersEntity != null && UpdatedOrdersEntity.Count > 0)
					{
						foreach(var item in UpdatedOrdersEntity)
						{
							Updated.Add(new Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel(1, FAEntities?.FirstOrDefault(x => x.Fertigungsnummer == item.FA)));
						}
					}
					if(NotupdatedOrdersEntity != null && NotupdatedOrdersEntity.Count > 0)
					{
						foreach(var item in NotupdatedOrdersEntity)
						{
							Notupdated.Add(new Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedFromExcelModel(FAEntities?.FirstOrDefault(x => x.Fertigungsnummer == item.FA), ""));
						}
					}

				}
				Lists = new Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel { IdUpdate = updateEntity.Id, Updated = Updated, NotUpdated = Notupdated };
				responseBody = Module.CRP_ReportingService.GenerateWerkTerminUpdateReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_WERK_TERMIN, Lists);

				return ResponseModel<byte[]>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
