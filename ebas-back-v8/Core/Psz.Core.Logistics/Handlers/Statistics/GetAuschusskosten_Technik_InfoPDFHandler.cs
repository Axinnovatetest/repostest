using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetAuschusskosten_Technik_InfoPDFHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private AusschusskostenInfoSearchModel _data { get; set; }
		public GetAuschusskosten_Technik_InfoPDFHandler(AusschusskostenInfoSearchModel _data, Identity.Models.UserModel user)
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
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.GetAuschusskosten_Technik_InfoAcess.GetAuschusskosten_Technik_Info(
					this._data.LagerFertigung,
					this._data.LagerP,
					this._data.DateBegin,
					this._data.DateEnd,
					null,
					null,
					null
					);
				//var __PackingListEntity = PackingListEntity.Where(x => x.Datum == _data.DateBegin).Take(10).ToList()
				//        .Concat(PackingListEntity.Where(x => x.Datum == _data.DateEnd).Take(10).ToList()).ToList();
				//if(PackingListEntity == null || PackingListEntity.Count() == 0)
				//	return ResponseModel<byte[]>.FailureResponse("Empty file sent.");
				var details = PackingListEntity?.Select(x => new GetAuschusskosten_Technik_InfoPDFDetailsModel(x)).ToList();


				//sums caluclation
				var sums = new List<GetAuschusskosten_Technik_InfoPDFSumModel>();
				var days = details.Select(x => x.Datum).Distinct().ToList();
				var totalKosten = details.Sum(k => k.Kosten);
				days.ForEach(x =>
				{
					var sumkosten = details.Where(d => d.Datum == x).Sum(y => y.Kosten);
					var percentageKosten = (sumkosten / totalKosten);
					sums.Add(new GetAuschusskosten_Technik_InfoPDFSumModel
					{
						FooterText = $"Zusammenfassung für {x} ({details.Where(d => d.Datum == x).Count()} Deataildatensätze)",
						PercentageKosten = percentageKosten,
						SumKosten = sumkosten,
						Date = x,
					});
				});
				var header = new GetAuschusskosten_Technik_InfoPDFHeaderModel
				{
					Bis = _data.DateEnd.ToString("dd.MM.yyyy"),
					Von = _data.DateBegin.ToString("dd.MM.yyyy"),
					LagerNummer = _data.LagerFertigung.ToString(),
					TotalKosten = totalKosten,
					CurrentDate = DateTime.Now.ToString("dd MMMM yyyy")
				};
				var ReportData = new GetAuschusskosten_Technik_InfoPDFModel { Details = details, Header = new List<GetAuschusskosten_Technik_InfoPDFHeaderModel> { header }, Sums = sums };

				response = Module.Logistic_ReportingService.GenerateAuschusskosten_Technik_InfoReport(Enums.ReportingEnums.ReportType.AUSCHUSSKOSTEN_TECHNIK_INFO, ReportData);

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
