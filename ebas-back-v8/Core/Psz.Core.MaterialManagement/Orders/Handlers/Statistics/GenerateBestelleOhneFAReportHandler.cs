using Infrastructure.Services.Reporting.Models.MTM;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;


namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GenerateBestelleOhneFAReportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public string data { get; set; }
		public GenerateBestelleOhneFAReportHandler(Identity.Models.UserModel user, string plant)
		{
			this._user = user;
			this.data = plant;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				if(string.IsNullOrWhiteSpace(data))
				{
					return ResponseModel<byte[]>.FailureResponse("Invalid Data");
				}
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var plantData = MaterialManagement.Helpers.SpecialHelper.GetPlant(data);

				if(plantData is null || !plantData.IsValidPlantLager())
					return ResponseModel<byte[]>.FailureResponse("No Data Found");

				List<Infrastructure.Services.Reporting.Models.MTM.PlantRegionModel> plantregions = new() { new Infrastructure.Services.Reporting.Models.MTM.PlantRegionModel(plantData.PlantFull) { } };
				var bestellungohnefa = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.Offene_mat_bst_access.GetBestellungohneFA(plantData.LagerHaupt, plantData.Lager_fert, plantData.Lager_fert_2);

				if(bestellungohnefa is null || bestellungohnefa.Count == 0)
					return ResponseModel<byte[]>.FailureResponse("No Data Found");


				byte[] responseBody = null;

				var datatoreturn = bestellungohnefa.Select(x => new Infrastructure.Services.Reporting.Models.MTM.BestellungohneFAModel(x)).ToList();
				var articles = bestellungohnefa.Select(x => x.Artikelnummer).ToList();
				articles = articles.Distinct().ToList();

				var reportarticles = articles.Select(x => new Articles(x)).ToList();

				var fastReport = new Infrastructure.Services.Reporting.FastReport(Module.BestellungTemplateFolder);
				responseBody = fastReport.GenerateBestelleOhneFAReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_BestellungohneFA, datatoreturn, plantregions, reportarticles);

				return ResponseModel<byte[]>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user is null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
