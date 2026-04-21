namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class GetBySupplierNameCAQHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private string _SupplierName;
		private Core.Identity.Models.UserModel _user;
		public GetBySupplierNameCAQHandler(string SupplierName, Core.Identity.Models.UserModel user)
		{
			_SupplierName = SupplierName;
			_user = user;

		}
	
			public ResponseModel<List<KeyValuePair<int, string>>> Handle()
			{
			try
			{
				List<KeyValuePair<int, string>> ArtikelListByWarengruppe = new List<KeyValuePair<int, string>>();


				var LGTList = Infrastructure.Data.Access.Joins.Logistics.WeVOHIncomingAccess.GetBySupplierNameCAQ(this._SupplierName).ToList();

				foreach(var SupplierList in LGTList)
				{
					ArtikelListByWarengruppe.Add(new KeyValuePair<int, string>(
								SupplierList.SupplierId,
								SupplierList.SupplierName
								));
				}

				if(ArtikelListByWarengruppe.Count == 0)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.FailureResponse("SupplierList List is empty !");
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(ArtikelListByWarengruppe);

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
