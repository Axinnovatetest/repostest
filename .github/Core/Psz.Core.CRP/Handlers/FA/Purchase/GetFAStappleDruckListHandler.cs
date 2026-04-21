using Infrastructure.Services.Reporting.Models.CTS;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetFAStappleDruckListHandler: IHandle<Identity.Models.UserModel, ResponseModel<FAUpdateByArticleFinalModel>>
	{
		private StappleDruckInputModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public GetFAStappleDruckListHandler(StappleDruckInputModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<FAUpdateByArticleFinalModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				FAUpdateByArticleFinalModel response = new FAUpdateByArticleFinalModel();
				List<FAUpdateByArticleListModel> _list = new List<FAUpdateByArticleListModel>();
				var listFAStappleDruck = Infrastructure.Data.Access.Joins.FADruck.FADruckAccess.GetListFAStappleDruck(this._data.Artikelnummer, (DateTime)this._data.Produktionstrmin, (int)this._data.Produktionsort);
				if(listFAStappleDruck == null || listFAStappleDruck.Count == 0)
					return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"No FA to print");
				else
				{
					_list = listFAStappleDruck.Select(x => new FAUpdateByArticleListModel((int)x.Fertigungsnummer, (int)x.Lagerort_id)).ToList();
					response.Updated = _list;
				}

				return ResponseModel<FAUpdateByArticleFinalModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<FAUpdateByArticleFinalModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<FAUpdateByArticleFinalModel>.AccessDeniedResponse();
			}
			List<int> lagers = new List<int> { 15, 26 };
			// List<int> lagers = new List<int> { 15 }; //fur albania
			//if (string.IsNullOrEmpty(this._data.Artikelnummer) || string.IsNullOrWhiteSpace(this._data.Artikelnummer))
			//    return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"Please put an article Or NummerKreise");

			if(!this._data.Produktionstrmin.HasValue)
				return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"Please put a date");

			if(!this._data.Produktionsort.HasValue)
				return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"Please put a warehouse");

			if(this._data.Produktionsort.HasValue && !lagers.Contains(this._data.Produktionsort.Value))
				return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"this function is only available for lager 26 and 15");
			//return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"this function is only available for lager 15"); //fur albania


			if(this._data.Produktionstrmin.HasValue && this._data.Produktionstrmin.Value > DateTime.Now.AddDays(28) && this._data.Produktionsort.HasValue && this._data.Produktionsort.Value == 26)
				return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"Date cannot exceed actual date + 28 days for lager 26");

			if(this._data.Produktionstrmin.HasValue && this._data.Produktionstrmin.Value > DateTime.Now.AddDays(14) && this._data.Produktionsort.HasValue && this._data.Produktionsort.Value == 15)
				return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"Date cannot exceed actual date + 14 days for lager 15");

			return ResponseModel<FAUpdateByArticleFinalModel>.SuccessResponse();
		}
	}
}
