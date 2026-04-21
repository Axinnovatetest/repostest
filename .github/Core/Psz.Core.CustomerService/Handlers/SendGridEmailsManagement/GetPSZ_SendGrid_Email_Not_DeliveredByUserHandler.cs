using Infrastructure.Services.Email.Models;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.SendGridEmailsManagement;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.CustomerService.Handlers.SendGridEmailsManagement
{
	public class GetPSZ_SendGrid_Email_Not_DeliveredByUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<GetPSZ_SendGrid_Email_Not_DeliveredByUserResponseModel>>
	{
		private GetPSZ_SendGrid_Email_Not_DeliveredByUserModel data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetPSZ_SendGrid_Email_Not_DeliveredByUserHandler(Identity.Models.UserModel user, GetPSZ_SendGrid_Email_Not_DeliveredByUserModel _data)
		{
			this._user = user;
			this.data = _data;
		}

		public ResponseModel<GetPSZ_SendGrid_Email_Not_DeliveredByUserResponseModel> Handle()
		{
			try
			{

				var datatoreturn = new List<Infrastructure.Services.Email.Models.FilteredUndeliveredEmails>();

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				#region  Data  paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};
				#endregion
				var fetchedData = Infrastructure.Data.Access.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredAccess.GetByMessagesIdsWithPagination(this._user.Id, filter: data.filter, paging: dataPaging);
				// - 
				var restoreturn = fetchedData?.Select(x => new PSZ_SendGrid_Email_Not_DeliveredModel(x)).ToList();
				var messagesID = fetchedData.Select(x => x.MessageId).Distinct().ToList();
				foreach(var item in messagesID)
				{
					var databyMessageId = restoreturn.Where(x => x.MessageId == item).ToList();
					datatoreturn.Add(new FilteredUndeliveredEmails(databyMessageId, this._user.Username));
				}
				var allCount = fetchedData.First().TotalCount ?? 0;

				return ResponseModel<GetPSZ_SendGrid_Email_Not_DeliveredByUserResponseModel>.SuccessResponse(new GetPSZ_SendGrid_Email_Not_DeliveredByUserResponseModel
				{
					Items = datatoreturn,
					PageRequested = this.data.RequestedPage,
					PageSize = this.data.PageSize,
					TotalCount = allCount,
					TotalPageCount = this.data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this.data.PageSize)) : 0
				});

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<GetPSZ_SendGrid_Email_Not_DeliveredByUserResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<GetPSZ_SendGrid_Email_Not_DeliveredByUserResponseModel>.AccessDeniedResponse();
			}
			return ResponseModel<GetPSZ_SendGrid_Email_Not_DeliveredByUserResponseModel>.SuccessResponse();
		}
	}
}
