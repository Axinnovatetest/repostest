using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetDashboardDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<RADashboardResponseModel>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetDashboardDataHandler(Identity.Models.UserModel user, int data)
		{
			this._data = data;
			this._user = user;
		}


		public ResponseModel<RADashboardResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var emptyResponseBody = new RADashboardResponseModel { };
				var openRahmen = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByStatus((int)Enums.BlanketEnums.RAStatus.Validated, this._data);
				if(openRahmen == null || openRahmen.Count <= 0)
				{
					return ResponseModel<RADashboardResponseModel>.SuccessResponse(emptyResponseBody);
				}
				// - filter by user customers
				if(this._user.Access != null &&
					(this._user.IsGlobalDirector || this._user.SuperAdministrator
					|| this._user.Access.CustomerService?.RahmenClosure == true
					|| this._user.Access.MasterData?.ArticleSales == true
					|| this._user.Access.MasterData?.ArticlePurchase == true))
				{
					// - Admin - do nothing
				}
				else
				{
					var customersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUserId(this._user.Id)
						.Select(e => e.CustomerNumber)?.Distinct()?.ToList();

					openRahmen = openRahmen.Where(x => customersNumbers.Contains(x.CustomerId ?? -2))?.ToList();
				}

				var raNrs = openRahmen.Select(x => x.AngeboteNr).ToList();
				var openRahmenPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetOpenByRahmen(raNrs);
				if(openRahmenPos == null || openRahmenPos.Count <= 0)
				{
					return ResponseModel<RADashboardResponseModel>.SuccessResponse(emptyResponseBody);
				}

				var thresholdDate = DateTime.Today.AddMonths(3);
				var openRaPosExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNrs(openRahmenPos.Select(x => x.Nr).ToList(), thresholdDate);
				// - re-filter to discard Pos not before threshold
				openRahmenPos = openRahmenPos.Where(x => openRaPosExtension?.Exists(y => y.AngeboteArtikelNr == x.Nr) == true)?.ToList();
				var raAbPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRahmenPositions(openRahmenPos.Select(x => x.Nr).ToList());


				// - RED Pos
				var redPos = new List<BlanketItem>();
				var oraPos = new List<BlanketItem>();
				var grePos = new List<BlanketItem>();
				foreach(var raItem in openRahmenPos)
				{
					var raExt = openRaPosExtension.FirstOrDefault(x => x.AngeboteArtikelNr == raItem.Nr);
					if(raExt == null)
						continue;

					if(raExt.GultigBis < DateTime.Today)
					{
						redPos.Add(new BlanketItem(raItem, raExt));
						continue;
					}
					var abPos = raAbPos.Where(x => x.ABPoszuRAPos == raItem.Nr)?.ToList();
					//var lsPos = raLsPos?.Where(x => abPos?.Exists(y => x.LSPoszuABPos == y.Nr) == true)?.ToList();
					if(abPos != null && abPos.Count > 0 && abPos.Sum(x => x.Anzahl ?? 0) < (raItem.OriginalAnzahl ?? 0) * 0.75m)
					{
						redPos.Add(new BlanketItem(raItem, raExt));
					}
					else
					{
						if(abPos != null && abPos.Count > 0 && abPos.Sum(x => x.Geliefert ?? 0) < (raItem.OriginalAnzahl ?? 0) * 0.75m)
						{
							oraPos.Add(new BlanketItem(raItem, raExt));
						}
						else
						{
							if(abPos != null && abPos.Count > 0 && abPos.Sum(x => x.Geliefert ?? 0) < (raItem.OriginalAnzahl ?? 0) * 0.75m)
							{
								grePos.Add(new BlanketItem(raItem, raExt));
							}
						}
					}
				}

				// -
				emptyResponseBody.RedPositions = redPos;
				emptyResponseBody.OrangePositions = oraPos;
				emptyResponseBody.GreenPositions = grePos;

				return ResponseModel<RADashboardResponseModel>.SuccessResponse(emptyResponseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<RADashboardResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<RADashboardResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<RADashboardResponseModel>.SuccessResponse();
		}

	}
}
