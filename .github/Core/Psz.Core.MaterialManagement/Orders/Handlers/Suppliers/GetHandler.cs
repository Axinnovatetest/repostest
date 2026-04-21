using Psz.Core.MaterialManagement.Orders.Models.Suppliers;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Suppliers
{
	public class GetHandler: IHandle<Models.Suppliers.GetRequestModel, ResponseModel<List<Models.Suppliers.GetResponseModel>>>
	{

		private Models.Suppliers.GetRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetHandler(UserModel user, Models.Suppliers.GetRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<Models.Suppliers.GetResponseModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<List<Models.Suppliers.GetResponseModel>> Perform(UserModel user, Models.Suppliers.GetRequestModel data)
		{
			#region Prepare Data
			var allAddresses = Infrastructure.Data.Access.Tables.MTM.AdressenAccess.GetForOrders();
			var allSuppliers = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.GetForOrders(allAddresses);

			var filteredAddresses = Infrastructure.Data.Access.Tables.MTM.AdressenAccess.GetForOrdersFiltered(allSuppliers, data.Filter)?.DistinctBy(x => x.Nr)?.OrderBy(x => x.Lieferantennummer).ToList();
			#endregion

			if(filteredAddresses == null || filteredAddresses.Count == 0)
				return ResponseModel<List<Models.Suppliers.GetResponseModel>>.NotFoundResponse();

			return ResponseModel<List<GetResponseModel>>.SuccessResponse(filteredAddresses.Select(x => new GetResponseModel(x)).ToList());

		}

		public ResponseModel<List<Models.Suppliers.GetResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<Models.Suppliers.GetResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Suppliers.GetResponseModel>>.SuccessResponse();
		}
	}
}
