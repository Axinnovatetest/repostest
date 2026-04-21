using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Gutshrift;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class GetGutschriftListFromRechnungHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<QuickSearchResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _Nr { get; set; }

		public GetGutschriftListFromRechnungHandler(Identity.Models.UserModel user, int nr)
		{
			_user = user;
			_Nr = nr;
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
				var ABItem = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._Nr);
				if(ABItem.Typ == "Rechnung")
				{
					var GutschriftList = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByProjectNr2(int.TryParse(ABItem.Projekt_Nr, out var val0) ? val0 : 0, "Gutschrift");
					if(GutschriftList != null && GutschriftList.Count > 0)
						response = GutschriftList.Select(x => new QuickSearchResponseModel(x)).OrderByDescending(y => y.DueDate).ToList();
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
			var ABItem = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._Nr);
			if(ABItem == null)
				return ResponseModel<List<QuickSearchResponseModel>>.FailureResponse(key: "1", value: $"Rechnung not found");
			return ResponseModel<List<QuickSearchResponseModel>>.SuccessResponse();
		}
	}
}
