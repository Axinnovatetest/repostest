using Psz.Core.Common.Models;
using Psz.Core.Logistics.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class GetListEtikettenVDAByNrAngArtDruckerHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public long _nrAngeboteArtikel { get; set; }


		public GetListEtikettenVDAByNrAngArtDruckerHandler(long nr, Identity.Models.UserModel user)
		{

			this._nrAngeboteArtikel = nr;
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

				var VDAListEntity = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.GetListeVDAByNrAngeboteArtikel(this._nrAngeboteArtikel);
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
			var ligne = Infrastructure.Data.Access.Tables.Logistics.LSAccess.GetLigneLS(this._nrAngeboteArtikel);
			if(ligne == null || ligne.Count() == 0)
				return ResponseModel<byte[]>.FailureResponse("nrAngeboteArtikel not found");

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}

