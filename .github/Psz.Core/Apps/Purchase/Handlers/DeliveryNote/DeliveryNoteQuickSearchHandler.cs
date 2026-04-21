using Psz.Core.Apps.Purchase.Models.DeliveryNote;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class DeliveryNoteQuickSearchHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<QuickSearchResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private QuickSearchInputModel _data { get; set; }

		public DeliveryNoteQuickSearchHandler(Identity.Models.UserModel user, QuickSearchInputModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<List<QuickSearchResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				List<QuickSearchResponseModel> response = new List<QuickSearchResponseModel>();
				var angeboteEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.QuickSearch(this._data.ProjectNr, this._data.VorfailNr);
				if(angeboteEntities != null && angeboteEntities.Count > 0)
				{
					response = angeboteEntities.Select(x => new QuickSearchResponseModel(x)).ToList();
				}
				return ResponseModel<List<QuickSearchResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<QuickSearchResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<QuickSearchResponseModel>>.AccessDeniedResponse();
			}
			if((string.IsNullOrEmpty(this._data.ProjectNr) || string.IsNullOrWhiteSpace(this._data.ProjectNr)) && (string.IsNullOrEmpty(this._data.VorfailNr) || string.IsNullOrWhiteSpace(this._data.VorfailNr)))
				return ResponseModel<List<QuickSearchResponseModel>>.FailureResponse(key: "1", value: $"Please fill one search item");
			return ResponseModel<List<QuickSearchResponseModel>>.SuccessResponse();
		}
	}
}
