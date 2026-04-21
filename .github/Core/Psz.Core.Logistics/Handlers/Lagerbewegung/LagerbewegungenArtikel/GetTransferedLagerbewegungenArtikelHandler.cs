using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Logistics.Models.Lagebewegung;


namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class GetTransferedLagerbewegungenArtikelHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int _wereingangId { get; set; }
		public int _Lagernach { get; set; }

		public GetTransferedLagerbewegungenArtikelHandler(Identity.Models.UserModel user, int wereingangId, int Lagernach)
		{
			this._user = user;
			this._wereingangId = wereingangId;
			this._Lagernach = Lagernach;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				int response ;
				var resulttransferedQuantity = Infrastructure.Data.Access.Tables.CTS.Lagerbewegungen_ArtikelAccess.GetTransferdQuantity(this._wereingangId,this._Lagernach);
				if(resulttransferedQuantity == 0)
					ResponseModel<int>.SuccessResponse();

					response = resulttransferedQuantity;
	
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}