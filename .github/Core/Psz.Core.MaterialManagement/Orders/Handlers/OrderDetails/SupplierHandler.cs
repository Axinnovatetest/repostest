using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class SupplierHandler: IHandle<SupplierRequestModel, ResponseModel<List<SupplierResponseModel>>>
	{


		private SupplierRequestModel data { get; set; }
		private UserModel user { get; set; }

		public SupplierHandler(UserModel user, SupplierRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<SupplierResponseModel>> Handle()
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

		private ResponseModel<List<SupplierResponseModel>> Perform(UserModel user, SupplierRequestModel data)
		{
			var result = Infrastructure.Data.Access.Joins.MTM.Order.SupplierAccess.GetSuppliersInfoByArticle(data.ArticleId);
			//return ResponseModel<List<SupplierResponseModel>>.FailureResponse("");

			return ResponseModel<List<SupplierResponseModel>>.SuccessResponse(result?.Select(x => new SupplierResponseModel { SupplierName = x.Name, IsDefault = x.IsDefault, Id = x.Id, MinimumOrderQuantity = x.MinimumQuantity, PurchasePrice = x.PurchasePrice, ShippingDays = x.ShippingDays }).ToList());
		}

		public ResponseModel<List<SupplierResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<SupplierResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<SupplierResponseModel>>.SuccessResponse();
		}
	}
}
