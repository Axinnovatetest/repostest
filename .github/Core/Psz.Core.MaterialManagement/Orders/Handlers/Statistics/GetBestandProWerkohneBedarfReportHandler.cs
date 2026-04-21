using Psz.Core.SharedKernel.Interfaces;
using System.IO;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetBestandProWerkohneBedarfReportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int data { get; set; }
		public GetBestandProWerkohneBedarfReportHandler(Identity.Models.UserModel user, int plant)
		{
			this._user = user;
			this.data = plant;
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
				var lagers = Psz.Core.MaterialManagement.Helpers.SpecialHelper.GetLagersPerMainLager(data);

				if(lagers is null)
					throw new InvalidDataException("No Lager Found");

				var bestandprowerk = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.Offene_mat_bst_access.GetBestandProWerkohneBedarf(lagers.ElementAt(0), lagers.ElementAt(1), lagers.ElementAt(2), lagers.ElementAt(3), lagers.ElementAt(4), lagers.ElementAt(5), lagers.ElementAt(6), lagers.ElementAt(7));
				/*	if(bestandprowerk is null || bestandprowerk.Count == 0)
						return ResponseModel<byte[]>.FailureResponse("No Data Found");*/


				byte[] responseBody = null;

				//	var datatoreturn = bestellungohnefa.Select(x => new Infrastructure.Services.Reporting.Models.MTM.BestellungohneFAModel(x)).ToList();

				List<Infrastructure.Services.Reporting.Models.MTM.UserModelBestelle> users = new() { new Infrastructure.Services.Reporting.Models.MTM.UserModelBestelle(_user.Username) };

				var fastReport = new Infrastructure.Services.Reporting.FastReport(Module.BestellungTemplateFolder);
				if(data == 6)
				{
					responseBody = fastReport.GenerateBestandProWerkohneBedarfReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_BestandProWerkohneBedarfcz, bestandprowerk, users);
				}
				if(data == 42)
				{
					responseBody = fastReport.GenerateBestandProWerkohneBedarfReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_BestandProWerkohneBedarfws, bestandprowerk, users);
				}
				if(data == 26)
				{
					responseBody = fastReport.GenerateBestandProWerkohneBedarfReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_BestandProWerkohneBedarfal, bestandprowerk, users);
				}
				if(data == 15)
				{
					responseBody = fastReport.GenerateBestandProWerkohneBedarfReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_BestandProWerkohneBedarfde, bestandprowerk, users);
				}
				if(data == 7)
				{
					responseBody = fastReport.GenerateBestandProWerkohneBedarfReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_BestandProWerkohneBedarftn, bestandprowerk, users);
				}
				if(data == 60)
				{
					responseBody = fastReport.GenerateBestandProWerkohneBedarfReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_BestandProWerkohneBedarfbetn, bestandprowerk, users);
				}
				if(data == 102)
				{
					responseBody = fastReport.GenerateBestandProWerkohneBedarfReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_BestandProWerkohneBedarfgztn, bestandprowerk, users);
				}


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
