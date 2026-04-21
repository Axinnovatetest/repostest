using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetFAFehlematerialHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAFehlematerialHandler(Identity.Models.UserModel user, int data)
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
				List<Infrastructure.Services.Reporting.Models.CTS.AnalyseSchneiderei1Model> list = new List<Infrastructure.Services.Reporting.Models.CTS.AnalyseSchneiderei1Model>();
				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data);
				var date = (faEntity.Termin_Bestatigt1.HasValue) ? $"<= '{faEntity.Termin_Bestatigt1.Value.AddDays(10).ToString("yyyyMMdd")}'" : "IS NULL";
				var fehlematerialEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseFehlmaterial(new List<int> { (int)faEntity.Lagerort_id }, new List<int> { (int)faEntity.Lagerort_id }, this._data, date);
				if(fehlematerialEntity != null && fehlematerialEntity.Count > 0)
				{
					list = fehlematerialEntity.Select(x => new Infrastructure.Services.Reporting.Models.CTS.AnalyseSchneiderei1Model(x)).ToList();
				}
				responseBody = Module.CRP_ReportingService.GenerateFAFehlematerialReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FEHLEMATERIAL, list);
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
