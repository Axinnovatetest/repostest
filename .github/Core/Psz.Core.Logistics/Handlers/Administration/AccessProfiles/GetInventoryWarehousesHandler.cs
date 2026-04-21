using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Psz.Core.Logistics.Handlers.Administration.AccessProfiles
{
	public class GetInventoryWarehousesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetInventoryWarehousesHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var accessProfileWarehouses = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessProfileWarehouseAccess.GetByAccessProfiles(new List<int> { _data});
				var warehouses = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(accessProfileWarehouses.Select(x=> x.WarehouseId)?.ToList());
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(warehouses?.Select(x => new KeyValuePair<int, string>(x.Lagerort_id, x.Lagerort))?.ToList());
				
			} catch(Exception e)
			{
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel< List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.Logistics.AccessProfileAccess.Get(this._data);
			if(userEntity == null)
				return ResponseModel< List<KeyValuePair<int, string>>>.FailureResponse("Access Profile not found");

			return ResponseModel< List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
