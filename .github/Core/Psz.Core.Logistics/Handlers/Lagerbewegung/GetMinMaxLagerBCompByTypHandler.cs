using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class GetMinMaxLagerBCompByTypHandler: IHandle<Identity.Models.UserModel, ResponseModel<LagerbewegungCompletModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public string _typ { get; set; }
		public int _order { get; set; }

		public GetMinMaxLagerBCompByTypHandler(Identity.Models.UserModel user, string typ, int order)
		{
			this._user = user;
			this._typ = typ;
			this._order = order;

		}
		public ResponseModel<LagerbewegungCompletModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var response = new LagerbewegungCompletModel();
				var lagerbwegungHeaderEntity = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetLagerbewegungMinMaxByTyp(this._typ, this._order);
				if(lagerbwegungHeaderEntity != null && lagerbwegungHeaderEntity.id > 0)
				{
					response.id = lagerbwegungHeaderEntity.id;
					response.typ = lagerbwegungHeaderEntity.typ;
					response.gebucht = lagerbwegungHeaderEntity.gebucht;
					response.datum = lagerbwegungHeaderEntity.datum;
					response.gebuchtVon = lagerbwegungHeaderEntity.gebuchtVon;
					var lagerbewegungDetailsEntity = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetLagerbewegungDetailsByID(lagerbwegungHeaderEntity.id);

					response.listPosition = lagerbewegungDetailsEntity.Select(k => new LagerbewegungDetailsModel(k)).ToList();
					;
				}




				return ResponseModel<LagerbewegungCompletModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<LagerbewegungCompletModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<LagerbewegungCompletModel>.AccessDeniedResponse();
			}

			return ResponseModel<LagerbewegungCompletModel>.SuccessResponse();
		}
	}
}