using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class AutoCompleteArtikelnummerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{

		private AutoCompleteArtikelModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AutoCompleteArtikelnummerHandler(Identity.Models.UserModel user, AutoCompleteArtikelModel data)
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
				var supplierAdress = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(_data.SupplerId);
				var AngeboteArticlesEntity = Infrastructure.Data.Access.Tables.CTS.Angebote_Artikelliste_ODBCAccess.GetArtikelFilterBySupplierId(this._data.Searchtext, supplierAdress.Nummer ?? -1);
				List<KeyValuePair<int, string>> response = new List<KeyValuePair<int, string>>();
				if(AngeboteArticlesEntity != null && AngeboteArticlesEntity.Count > 0)
					response = AngeboteArticlesEntity.Select(x => new KeyValuePair<int, string>(x.Artikel_Nr, x.Artikelnummer))
						.Where(x => !benchmarkKreis.Exists(y => x.Value.StartsWith(y) == true)).ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}

	}
}
