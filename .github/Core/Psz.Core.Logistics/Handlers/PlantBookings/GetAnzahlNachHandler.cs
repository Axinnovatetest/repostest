using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Psz.Core.Logistics.Handlers.PlantBookings;

public class GetAnzahlNachHandler: IHandle<Identity.Models.UserModel, ResponseModel<decimal>>
{

	private Core.Identity.Models.UserModel _user;
	private int _LagerId;
	private int _WereingangId;
	private int _ID;
	public GetAnzahlNachHandler(Core.Identity.Models.UserModel user, int LagerId, int WereingangId, int ID)
	{
		this._user = user;
		this._LagerId = LagerId;
		this._WereingangId = WereingangId;
		this._ID = ID;
	}

	public ResponseModel<decimal> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			if(_WereingangId == 0  || _LagerId == 0)
				return ResponseModel<decimal>.SuccessResponse(0);

			var anzahlNach = Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetAnzahlNach(this._LagerId,this._WereingangId,this._ID);
			return ResponseModel<decimal>.SuccessResponse(anzahlNach);


		} catch(Exception ex)
		{
			Infrastructure.Services.Logging.Logger.Log(ex);
			throw;
		}
	}

	public ResponseModel<decimal> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<decimal>.AccessDeniedResponse();
		}

		return ResponseModel<decimal>.SuccessResponse();
	}


}
