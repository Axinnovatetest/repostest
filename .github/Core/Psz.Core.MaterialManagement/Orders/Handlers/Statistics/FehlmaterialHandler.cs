using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class FehlmaterialHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private FehlmaterialRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public FehlmaterialHandler(Identity.Models.UserModel user, FehlmaterialRequestModel data)
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

				var fehlmaterialData = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.FehlmaterialAccess.GetSiteFehlamaterial(_data.place, _data.fa, _data.date);
				//if(fehlmaterialData is null || fehlmaterialData.Count == 0)
				//{
				//	return ResponseModel<byte[]>.NotFoundResponse();
				//}
				List<FehlmaterialModel> List = new List<FehlmaterialModel>();
				if(fehlmaterialData != null && fehlmaterialData.Count > 0)
				{
					List = fehlmaterialData.Select(x => new FehlmaterialModel(x)).ToList();
				}
				var fastReport = new Infrastructure.Services.Reporting.FastReport(Module.BestellungTemplateFolder);
				responseBody = fastReport.GenerateMTMFehlematerialReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FEHLEMATERIAL, fehlmaterialData?.Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.FAFehlmaterialModel(x))?.ToList());
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
