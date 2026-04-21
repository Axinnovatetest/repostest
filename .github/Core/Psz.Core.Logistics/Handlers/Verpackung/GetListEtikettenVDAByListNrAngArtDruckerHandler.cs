using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Verpackung;
using Psz.Core.Logistics.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class GetListEtikettenVDAByListNrAngArtDruckerHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ListeUpdateVDA _listeUpdateVDA { get; set; }


		public GetListEtikettenVDAByListNrAngArtDruckerHandler(ListeUpdateVDA listeUpdateVDA, Identity.Models.UserModel user)
		{

			this._listeUpdateVDA = listeUpdateVDA;
			this._user = user;
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
				var response = new List<VDAEtikettenModel>();

				var VDAListEntity = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.GetListeVDAByListNrAngeboteArtikel(this._listeUpdateVDA.listeNrAngArt);
				if(VDAListEntity != null && VDAListEntity.Count > 0)
				{
					foreach(var item in VDAListEntity)
					{
						response.Add(new VDAEtikettenModel((item)));
					}

				}
				var POSITIONS = VDAListEntity?.Select(x => new Psz.Core.Logistics.Reporting.Models.VDAEtikettenModel(x)).ToList();
				responseBody = Module.Logistic_ReportingService.GenerateLSDruckEtikettenReport(Enums.ReportingEnums.ReportType.LSEtikettenDRUCKER,
				 new VDAEtikettenRportingModel() { listeEtiketten = POSITIONS });

				return ResponseModel<byte[]>.SuccessResponse(responseBody);
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
			foreach(var item in this._listeUpdateVDA.listeNrAngArt)
			{
				var ligne = Infrastructure.Data.Access.Tables.Logistics.LSAccess.GetLigneLS(item);
				if(ligne == null || ligne.Count() == 0)
					return ResponseModel<byte[]>.FailureResponse("nrAngeboteArtikel{" + item + "} not found");
			}


			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}

