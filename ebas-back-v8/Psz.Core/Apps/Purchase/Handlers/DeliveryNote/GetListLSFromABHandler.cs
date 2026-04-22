using Psz.Core.Apps.Purchase.Models.DeliveryNote;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class GetListLSFromABHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<QuickSearchResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _Nr { get; set; }

		public GetListLSFromABHandler(Identity.Models.UserModel user, int nr)
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
				// - 2022-11-28 - get LS by AB thru AB pos not AB project
				var abPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._Nr, false);
				if(abPos != null && abPos.Count > 0)
				{
					var lsPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyABLSPositions(abPos.Select(x => x.Nr).ToList());
					if(lsPos != null && lsPos.Count > 0)
					{
						var ls = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(lsPos.Select(x => x.AngebotNr ?? -1).ToList());
						response = ls?.Select(x => new QuickSearchResponseModel(x)).ToList();
					}
				}

				// - old
				//var ABItem = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._Nr);
				//if (ABItem.Typ == "Auftragsbestätigung")
				//{
				//	var LSlist = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByProjectNr2(int.TryParse(ABItem.Projekt_Nr , out var val0) ? val0 : 0 , "Lieferschein");
				//	if (LSlist != null && LSlist.Count > 0)
				//	{
				//		response = LSlist.Select(x => new QuickSearchResponseModel(x)).ToList();
				//	}
				//}
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
				return ResponseModel<List<QuickSearchResponseModel>>.FailureResponse(key: "1", value: $"AB not found");
			return ResponseModel<List<QuickSearchResponseModel>>.SuccessResponse();
		}
	}
}
