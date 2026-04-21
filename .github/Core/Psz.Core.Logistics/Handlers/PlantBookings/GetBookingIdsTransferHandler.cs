using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Handlers.PlantBookings;
public class GetBookingIdsTransferHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, TransferWeVohModel>>>>
{
	private int _filter;
	private int _lagerNach;
	private Core.Identity.Models.UserModel _user;
	public GetBookingIdsTransferHandler(int filter,int lagerNach, Core.Identity.Models.UserModel user)
	{
		_filter = filter;
		_lagerNach = lagerNach;
		_user = user;

	}
	public ResponseModel<List<KeyValuePair<int, TransferWeVohModel>>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success || _filter < 0)
			{
				return validationResponse;
			}
			var data = Infrastructure.Data.Access.Joins.Logistics.WeVOHIncomingAccess.GetWohTransferBezeichungList(_filter,_lagerNach);

			return ResponseModel<List<KeyValuePair<int, TransferWeVohModel>>>.SuccessResponse(
			(data ?? new List<TransferWEArtikelEntity>())
			?.Select(x => new KeyValuePair<int, TransferWeVohModel>(x.weId, new TransferWeVohModel(x)))
			.ToList());
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<KeyValuePair<int, TransferWeVohModel>>> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<List<KeyValuePair<int, TransferWeVohModel>>>.AccessDeniedResponse();
		}

		return ResponseModel<List<KeyValuePair<int, TransferWeVohModel>>>.SuccessResponse();
	}
}

