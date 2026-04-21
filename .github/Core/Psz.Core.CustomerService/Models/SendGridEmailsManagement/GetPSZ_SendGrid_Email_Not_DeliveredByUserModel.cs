using Psz.Core.Common.Models;

namespace Psz.Core.CustomerService.Models.SendGridEmailsManagement
{
	public class GetPSZ_SendGrid_Email_Not_DeliveredByUserModel: IPaginatedRequestModel
	{
		public string filter { get; set; }
	}

	public class GetPSZ_SendGrid_Email_Not_DeliveredByUserResponseModel: IPaginatedResponseModel<Infrastructure.Services.Email.Models.FilteredUndeliveredEmails>
	{
	}
}
