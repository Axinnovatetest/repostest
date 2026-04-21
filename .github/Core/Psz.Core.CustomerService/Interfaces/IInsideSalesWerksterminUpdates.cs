using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.InsideSalesWerksterminUpdates;
using Psz.Core.Identity.Models;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Interfaces
{
	public interface IInsideSalesWerksterminUpdates
	{
		ResponseModel<List<InsideSalesWerkserminUpdatesModel>> getFAWithChangedWerkstermin(UserModel user, bool? insConfirmation);
		ResponseModel<int> UpdateInsConfirmation(InsConfirmationWerkterminModel model, UserModel user);
		ResponseModel<byte[]> getFAWithChangedWerkterminHistory_XLS(UserModel user, bool? insConfirmation);

	}
}
