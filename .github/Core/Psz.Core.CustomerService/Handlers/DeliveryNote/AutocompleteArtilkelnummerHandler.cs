using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.DeliveryNote
{
	public class AutocompleteArtilkelnummerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private string _data { get; set; }

		private Identity.Models.UserModel _user { get; set; }

		public AutocompleteArtilkelnummerHandler(Identity.Models.UserModel user, string data)
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

				var AngeboteArticlesEntity = Infrastructure.Data.Access.Tables.CTS.Angebote_Artikelliste_ODBCAccess.GetLikeArticle(this._data);
				List<KeyValuePair<int, string>> response = new List<KeyValuePair<int, string>>();
				if(AngeboteArticlesEntity != null && AngeboteArticlesEntity.Count > 0)
					response = AngeboteArticlesEntity.Select(x => new KeyValuePair<int, string>(x.Artikel_Nr, x.Artikelnummer))
						.Where(x => !benchmarkKreis.Exists(y => x.Value.StartsWith(y) == true)).ToList();

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
