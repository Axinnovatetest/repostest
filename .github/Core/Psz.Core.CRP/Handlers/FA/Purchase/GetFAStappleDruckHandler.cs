using Infrastructure.Services.Reporting.Models.CTS;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetFAStappleDruckHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<byte[]>>>
	{
		private StappleDruckInputModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public GetFAStappleDruckHandler(StappleDruckInputModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<byte[]>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<byte[]> carrier = new List<byte[]>();
				List<int> faListToPrint = new List<int>();
				List<FAStapelFinalReportModel> finalReport = new List<FAStapelFinalReportModel>();
				var listFAStappleDruck = Infrastructure.Data.Access.Joins.FADruck.FADruckAccess.GetListFAStappleDruck(this._data.Artikelnummer, (DateTime)this._data.Produktionstrmin, (int)this._data.Produktionsort);
				if(listFAStappleDruck == null || listFAStappleDruck.Count == 0)
					return ResponseModel<List<byte[]>>.FailureResponse(key: "1", value: $"No FA to print");

				else
				{
					faListToPrint = listFAStappleDruck.Select(X => (int)X.Fertigungsnummer).ToList();
					var toUpdate = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					var faEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(faListToPrint);
					foreach(var item in faListToPrint)
					{
						//var faItem = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(item);
						var faItem = faEntities.Find(x => x.Fertigungsnummer == item);
						if(faItem != null)
						{
							var artikelItem = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)faItem.Artikel_Nr);
							var stapple = new Psz.Core.CRP.Handlers.FA.Purchase.GetFADruckHandler(this._user, item).Handle();
							if(stapple.Success)
							{
								var result = stapple.Body;
								carrier.Add(result);
							}
							finalReport.Add(new FAStapelFinalReportModel(item, artikelItem.ArtikelNummer, faItem.Termin_Bestatigt1));
							faItem.Gedruckt = true;
							faItem.FA_Druckdatum = DateTime.Now;
							//Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Update(faItem);
							toUpdate.Add(faItem);
						}
					}
					// - 2022-03-16 - refactor 
					Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Update(toUpdate);

					var finalReportBuffer = Module.CRP_ReportingService.GenerateFAStapelFinalReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FASTAPLEREPORT, finalReport);
					carrier.Add(finalReportBuffer);
				}

				return ResponseModel<List<byte[]>>.SuccessResponse(carrier);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<byte[]>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<byte[]>>.AccessDeniedResponse();
			}
			List<int> lagers = new List<int> { 15, 26 };
			if(!this._data.Produktionstrmin.HasValue)
				return ResponseModel<List<byte[]>>.FailureResponse(key: "1", value: $"Please put a date");

			if(!this._data.Produktionsort.HasValue)
				return ResponseModel<List<byte[]>>.FailureResponse(key: "1", value: $"Please put a warehouse");

			if(this._data.Produktionsort.HasValue && !lagers.Contains(this._data.Produktionsort.Value))
				return ResponseModel<List<byte[]>>.FailureResponse(key: "1", value: $"this function is only available for lager 26 and 15");


			if(this._data.Produktionstrmin.HasValue && this._data.Produktionstrmin.Value > DateTime.Now.AddDays(28) && this._data.Produktionsort.HasValue && this._data.Produktionsort.Value == 26)
				return ResponseModel<List<byte[]>>.FailureResponse(key: "1", value: $"Date cannot exceed actual date + 28 days for lager 26");

			if(this._data.Produktionstrmin.HasValue && this._data.Produktionstrmin.Value > DateTime.Now.AddDays(14) && this._data.Produktionsort.HasValue && this._data.Produktionsort.Value == 15)
				return ResponseModel<List<byte[]>>.FailureResponse(key: "1", value: $"Date cannot exceed actual date + 14 days for lager 15");

			return ResponseModel<List<byte[]>>.SuccessResponse();
		}

	}
}
