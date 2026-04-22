using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.BomChangeRequests
{
	public class GetValidatedBomChangeRequestsHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private UserModel _user;
		private int _articleId;

		public GetValidatedBomChangeRequestsHandler(UserModel user, int articleId)
		{
			this._user = user;
			this._articleId = articleId;
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{

			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return this.Validate();
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
			(Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsAccess.GetValidatedBCR(this._articleId)
			?? new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>())
			?.Select(x => new KeyValuePair<int, string>(x.Id, x.SubmissionDate.ToString("dd-MM-yyyy") + " | " + x.Reason + " | " + x.Requester_name))
			?.ToList());

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}

		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();

			}
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
