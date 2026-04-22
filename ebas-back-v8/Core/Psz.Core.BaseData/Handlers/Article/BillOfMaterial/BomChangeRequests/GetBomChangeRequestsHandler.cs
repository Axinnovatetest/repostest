using Psz.Core.BaseData.Models.Article.BillOfMaterial.BomChangeRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.BomChangeRequests
{
	public class GetBomChangeRequestsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<BomChangeRequestsModel>>>

	{
		private Identity.Models.UserModel _user { get; set; }
		private GetAllBomChangeRequestModel _request { get; set; }

		public GetBomChangeRequestsHandler(Identity.Models.UserModel user, GetAllBomChangeRequestModel request)
		{
			this._user = user;
			this._request = request;
		}

		public BomChangeRequestsModel Data { get; }

		public ResponseModel<List<BomChangeRequestsModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return this.Validate();
				}
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;

				if(!string.IsNullOrWhiteSpace(this._request.SortField))
				{
					var sortFieldName = "";
					switch(this._request.SortField.ToLower())
					{
						default:
						case "artikelnummer":
							sortFieldName = "[Artikelnummer]";
							break;
						case "reason":
							sortFieldName = "[Reason]";
							break;
						case "requester_name":
							sortFieldName = "[Requester_name]";
							break;
						case "requestdate":
							sortFieldName = "[RequestDate]";
							break;
						case "status":
							sortFieldName = "[Status]";
							break;

					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._request.SortDesc,
					};
				}

				List<int> bcrIds = Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsAccess.Get().Select(x => x.Id).ToList();
				List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> entities = new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();

				entities = Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsAccess.Get(this._request.ArticleNumber, dataSorting);

				var response = entities?.Select(x => new BomChangeRequestsModel(x)).ToList();
				return ResponseModel<List<BomChangeRequestsModel>>.SuccessResponse(response);


			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<List<BomChangeRequestsModel>> Validate()
		{
			if(this._user == null /*|| !this._user.Access.MasterData.ViewBCR.Value*/)
			{
				return ResponseModel<List<BomChangeRequestsModel>>.AccessDeniedResponse();

			}
			return ResponseModel<List<BomChangeRequestsModel>>.SuccessResponse();
		}
	}
}
