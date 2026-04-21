using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Verpackung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class GetListKundenKurtzVDAHandler: IHandle<Identity.Models.UserModel, ResponseModel<ListKundenKurtzModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int _typeVDA { get; set; }
		public GetListKundenKurtzVDAHandler(int typeVDA, Identity.Models.UserModel user)
		{
			this._user = user;
			this._typeVDA = typeVDA;
		}
		public ResponseModel<ListKundenKurtzModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new ListKundenKurtzModel();
				var listeFirma = new List<string>();


				var KundenListEntity = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.GetListeKundenKurtzVDA();
				if(KundenListEntity != null && KundenListEntity.Count > 0)
					listeFirma = KundenListEntity.Select(k => k.lVornameNameFirma).ToList();
				response.ListelVornameNameFirma = listeFirma;
				return ResponseModel<ListKundenKurtzModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<ListKundenKurtzModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<ListKundenKurtzModel>.AccessDeniedResponse();
			}

			return ResponseModel<ListKundenKurtzModel>.SuccessResponse();
		}
	}
}

