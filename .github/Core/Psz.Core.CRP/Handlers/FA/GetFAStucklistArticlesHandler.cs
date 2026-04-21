using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFAStucklistArticlesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAStucklistArticlesHandler(string data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// - 2022-10-12 - filter Benchmark Customer
				var benchmarkKreis = (Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetForBenchmark()
					?? new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>()).Select(x => x.Nummerschlüssel).ToList();

				var articlesEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetForFAStcklistSelect(this._data);
				List<KeyValuePair<int, string>> response = new List<KeyValuePair<int, string>>();
				if(articlesEntities != null && articlesEntities.Count > 0)
					response = articlesEntities
						.Where(x => !benchmarkKreis.Exists(y => y == x.CustomerPrefix))
						.Select(x => new KeyValuePair<int, string>(x.ArtikelNr, $"{x.ArtikelNummer}||     {x.Bezeichnung1}||     {x.Freigabestatus}")).ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}