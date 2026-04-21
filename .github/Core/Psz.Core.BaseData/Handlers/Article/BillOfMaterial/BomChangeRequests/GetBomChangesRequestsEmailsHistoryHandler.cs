using Psz.Core.BaseData.Models.Article.BillOfMaterial.BomChangeRequests;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.BomChangeRequests
{
	public class GetBomChangesRequestsEmailsHistoryHandler: IHandle<UserModel, ResponseModel<List<BomChangesRequestsEmailHistoryModel>>>
	{
		private GetBomChangesRequestsEmailsHistoryModel _request { get; set; }

		public GetBomChangesRequestsEmailsHistoryHandler(UserModel user, GetBomChangesRequestsEmailsHistoryModel request)
		{
			_user = user;
			this._request = request;
		}

		public UserModel _user { get; }

		public ResponseModel<List<BomChangesRequestsEmailHistoryModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return this.Validate();
				}
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(this._request.SortField))
				{
					var sortFieldName = "";
					switch(this._request.SortField.ToLower())
					{
						default:
						case "requestenName":
							sortFieldName = "[Requester_name]";
							break;
						case "requesteremail":
							sortFieldName = "[Requester_email]";
							break;
						case "mailsubject":
							sortFieldName = "[Mail_subject]";
							break;
						case "sendingdate":
							sortFieldName = "[Sending_date]";
							break;
						case "status":
							sortFieldName = "[Status]";
							break;
						case "validatoremail":
							sortFieldName = "[Validator_email]";
							break;

					}
					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._request.SortDesc,
					};
				}
				var entities = Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryAccess.GetAllEmailsHistory(dataSorting);

				var response = entities?.Select(x => new BomChangesRequestsEmailHistoryModel(x)).ToList();
				return ResponseModel<List<BomChangesRequestsEmailHistoryModel>>.SuccessResponse(response);


			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<List<BomChangesRequestsEmailHistoryModel>> Validate()
		{
			if(this._user == null/* || !this._user.Access.MasterData.ViewBCR.Value*/)
			{
				return ResponseModel<List<BomChangesRequestsEmailHistoryModel>>.AccessDeniedResponse();

			}
			return ResponseModel<List<BomChangesRequestsEmailHistoryModel>>.SuccessResponse();
		}

	}
}
