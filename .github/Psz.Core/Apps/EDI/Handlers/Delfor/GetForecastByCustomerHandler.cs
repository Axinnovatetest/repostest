using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetForecastByCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Delfor.GetForecastModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetForecastByCustomerHandler(Identity.Models.UserModel user, int customerId)
		{
			this._user = user;
			this._data = customerId;
		}

		public ResponseModel<List<Models.Delfor.GetForecastModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var customerEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data);
				var documentEntities = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetLastByBuyerDUNS(customerEntity.Duns) ?? new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
				var documentPrevVersionEntities = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetBeforeLastByBuyerDUNS(customerEntity.Duns) ?? new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
				//var documentNextVersionEntities = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetBeforeLastByBuyerDUNS(customerEntity.Duns) ?? new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
				var documentVersionEntities = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetDocumentVersions(customerEntity.Duns, documentEntities?.Select(x => x.DocumentNumber)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
				var postionsCounts = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetPositionsCount(documentEntities?.Select(x => x.DocumentNumber)?.ToList());

				var response = new List<Models.Delfor.GetForecastModel>();
				foreach(var documentEntity in documentEntities)
				{
					var documentPos = postionsCounts.Where(x => x.Key.Trim() == documentEntity.DocumentNumber.Trim())?.FirstOrDefault();
					var docPrevVersion = documentPrevVersionEntities?.Find(x => x.ReferenceNumber == documentEntity.ReferenceNumber && x.SenderId == documentEntity.SenderId);
					var docVersions = documentVersionEntities?.Where(x => x.ReferenceNumber == documentEntity.ReferenceNumber)?.Select(x => new KeyValuePair<long, string>(x.Id, x.ReferenceVersionNumber?.ToString()))?.ToList();
					response.Add(new Models.Delfor.GetForecastModel(customerEntity, documentPos.HasValue ? documentPos.Value.Value : 0, documentEntity, docPrevVersion, null, docVersions));
				}

				return ResponseModel<List<Models.Delfor.GetForecastModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Delfor.GetForecastModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Delfor.GetForecastModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data) == null)
			{
				return ResponseModel<List<Models.Delfor.GetForecastModel>>.FailureResponse("Customer not found");
			}

			return ResponseModel<List<Models.Delfor.GetForecastModel>>.SuccessResponse();
		}
	}
}