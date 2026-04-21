using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetForecastHeaderHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Delfor.GetForecastModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetForecastHeaderHandler(Identity.Models.UserModel user, int customerId)
		{
			this._user = user;
			this._data = customerId;
		}

		public ResponseModel<Models.Delfor.GetForecastModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var documentEntity = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(this._data);
				var customerEntity = documentEntity.ManualCreation.HasValue && documentEntity.ManualCreation.Value
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(documentEntity.PSZCustomernumber ?? -1)
					: Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByDunsNumber(documentEntity.BuyerDUNS);
				var postionsCounts = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetPositionsCount(new List<string> { documentEntity.DocumentNumber });
				var documentPrevVersionEntity = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetPreviousVersion(this._data);
				var documentNextVersionEntity = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetNextVersion(this._data);
				var documentVersionEntities = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetDocumentVersions(customerEntity.Duns, documentEntity.DocumentNumber) ?? new List<KeyValuePair<long, string>>();

				return ResponseModel<Models.Delfor.GetForecastModel>.SuccessResponse(new Models.Delfor.GetForecastModel(customerEntity, postionsCounts?[0].Value ?? 0, documentEntity, documentPrevVersionEntity, documentNextVersionEntity, documentVersionEntities));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Delfor.GetForecastModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Delfor.GetForecastModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(this._data) == null)
			{
				return ResponseModel<Models.Delfor.GetForecastModel>.FailureResponse("Forecast document not found");
			}

			return ResponseModel<Models.Delfor.GetForecastModel>.SuccessResponse();
		}
	}
}