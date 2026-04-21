using Psz.Core.Common.Models;
using Psz.Core.CRP.Models;
using Psz.Core.CRP.Models.Delfor;

namespace Psz.Core.CRP.Interfaces
{
	public interface IDelforService
	{
		ResponseModel<byte[]> DelforDraft(Identity.Models.UserModel user);
		ResponseModel<DeliveryForcastHeaderModel> GetCustomerInformations(Identity.Models.UserModel user, int data);
		ResponseModel<List<CustomerModel>> GetCustomersForDelfor(Identity.Models.UserModel user);
		ResponseModel<List<DeliveryForcastLineItemModel>> ImportDeflorFromExcel(Identity.Models.UserModel user, ImportFileModel data);
		ResponseModel<int> ReloadDelforFile(Identity.Models.UserModel user, int data);
		ResponseModel<int> SaveDelfor(Identity.Models.UserModel user, DeliveryForcastModel data);
	}
}