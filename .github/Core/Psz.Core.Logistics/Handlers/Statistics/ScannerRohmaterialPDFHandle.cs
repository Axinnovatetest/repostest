using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class ScannerRohmaterialPDFHandle: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ScannerRohmaterialSearch _data { get; set; }
		public ScannerRohmaterialPDFHandle(Identity.Models.UserModel user, ScannerRohmaterialSearch _data)
		{
			this._user = user;
			this._data = _data;

		}
		public ResponseModel<byte[]> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			try
			{
				byte[] response = null;


				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetScannerRohmaterialEntity(_data.From, _data.To, null, null, _data.SearchValue, _data.SearchLager);
				var responseEntity = PackingListEntity?.Select(x => new ScannerRohmaterialModel(x)).ToList();
				var Transferlager = responseEntity.DistinctBy(a => a.Transferlager).Select(x => new Title(x)).ToList();


				var transferLagerAndLagerplatz = responseEntity.GroupBy(c => new
				{
					c.Transferlager,
					c.Lagerplatz_pos,
					c.Datum,
				}).Select(gcs => new TitleDatum()
				{
					Transferlager = gcs.Key.Transferlager,
					Lagerplatz_pos = gcs.Key.Lagerplatz_pos,
					Datum = gcs.Key.Datum,
				}).ToList();
				//var transferLagerAndLagerplatz = responseEntity.Select(x => new TitleDatum(x)).DistinctBy(y=>y.Lagerplatz_pos).DistinctBy(z=>z.Transferlager).DistinctBy(a => a.Datum).ToList();			
				var reportData = new ScannerRohmaterialPDFModel { Title = Transferlager, Details = responseEntity.OrderBy(x => x.Scanndatum).ToList(), TitleDatum = transferLagerAndLagerplatz };
				response = Module.Logistic_ReportingService.GenerateScannerRohmaterialReport(Enums.ReportingEnums.ReportType.ScannerRohmaterial, reportData);

				return ResponseModel<byte[]>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
