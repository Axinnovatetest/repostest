using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetFehlerMaterialFAHandler: IHandle<Identity.Models.UserModel, ResponseModel<FehlerMaterialFAResponseModel>>
	{

		private FehlerMaterialFARequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFehlerMaterialFAHandler(Identity.Models.UserModel user, FehlerMaterialFARequestModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<FehlerMaterialFAResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var lagerort = Infrastructure.Data.Access.Tables.MTM.LagerorteAccess.Get(_data.Lager);
				var artikel = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data.Artikel_nr);
				var entities = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.FehlerMaterialFAAccess.GetFehlermaterialFA(artikel.ArtikelNummer, _data.Menge, lagerort.Lagerort, _data.Date);
				var response = new FehlerMaterialFAResponseModel
				{
					Artikelnummer = artikel.ArtikelNummer,
					Lagerort = lagerort.Lagerort,
					FehlerMaterialList = entities?.Select(x => new FehlerMaterialFAList(x)).ToList()
				};


				return ResponseModel<FehlerMaterialFAResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<FehlerMaterialFAResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<FehlerMaterialFAResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<FehlerMaterialFAResponseModel>.SuccessResponse();
		}

	}
}
