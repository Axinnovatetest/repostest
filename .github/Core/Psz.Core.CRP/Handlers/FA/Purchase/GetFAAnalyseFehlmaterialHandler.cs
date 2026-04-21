using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetFAAnalyseFehlmaterialHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		public int _Fertigungsnummer { get; set; }
		public DateTime _Date { get; set; }
		public int _Id { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAAnalyseFehlmaterialHandler(Identity.Models.UserModel user, int fertigungsnummer, DateTime date, int id)
		{
			this._user = user;
			this._Fertigungsnummer = fertigungsnummer;
			this._Date = date;
			this._Id = id;
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
				string myTime = $"<= '{this._Date.ToString("yyyyMMdd")}'";
				var param = SetQueryParts((int)this._Id);
				List<Infrastructure.Services.Reporting.Models.CTS.AnalyseSchneiderei1Model> List = new List<Infrastructure.Services.Reporting.Models.CTS.AnalyseSchneiderei1Model>();
				var AnalyseSchneiderei1Entities = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseFehlmaterial(param.Key, param.Value, (int)this._Fertigungsnummer, myTime);
				if(AnalyseSchneiderei1Entities != null && AnalyseSchneiderei1Entities.Count > 0)
				{
					List = AnalyseSchneiderei1Entities.Select(x => new Infrastructure.Services.Reporting.Models.CTS.AnalyseSchneiderei1Model(x)).ToList();
				}
				responseBody = Module.CRP_ReportingService.GenerateFAFehlematerialReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FEHLEMATERIAL, List);
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
		public KeyValuePair<List<int>, List<int>> SetQueryParts(int id)
		{
			List<int> lager = new List<int>();
			List<int> hauptlagers = new List<int>();

			switch((Enums.FAEnums.AnalyseLands)id)
			{
				case Enums.FAEnums.AnalyseLands.TN://TN
					lager = new List<int> { 7 };
					hauptlagers = new List<int> { 7, 10, 23, 29, 30, 56 };
					break;
				case Enums.FAEnums.AnalyseLands.WS://WS
					lager = new List<int> { 42 };
					hauptlagers = new List<int> { 40, 42, 46, 47, 49, 57 };
					break;
				case Enums.FAEnums.AnalyseLands.CZ://CZ
					lager = new List<int> { 6, 3, 20, 21 };
					hauptlagers = new List<int> { 3, 6, 9, 52 };
					break;
				case Enums.FAEnums.AnalyseLands.AL://AL
					lager = new List<int> { 24, 26 };
					hauptlagers = new List<int> { 24, 25, 26, 34, 35, 50 };
					break;
				case Enums.FAEnums.AnalyseLands.DE://DE
					lager = new List<int> { 14, 15 };
					hauptlagers = new List<int> { 14, 15, 16, 22 };
					break;
				case Enums.FAEnums.AnalyseLands.BETN://BETN
					lager = new List<int> { 60 };
					hauptlagers = new List<int> { 59, 60, 63, 64, 61 };
					break;
				case Enums.FAEnums.AnalyseLands.GZTN://GZTN
					lager = new List<int> { 102 };
					hauptlagers = new List<int> { 104, 102, 107, 108, 105 };
					break;
				default:
					break;
			}
			return new KeyValuePair<List<int>, List<int>>(lager, hauptlagers);
		}
	}
}
