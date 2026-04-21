using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class SearchInRAPositionsColorsHandler: IHandle<Identity.Models.UserModel, ResponseModel<RADashboardResponseModel>>
	{

		private RAPositionsColorsSearchModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public SearchInRAPositionsColorsHandler(Identity.Models.UserModel user, RAPositionsColorsSearchModel data)
		{
			this._user = user;
			this._data = data;
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
				var openRahmen = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByStatus((int)Enums.BlanketEnums.RAStatus.Validated, _data.TypeId);
				if(openRahmen == null || openRahmen.Count <= 0)
				{
					return ResponseModel<RADashboardResponseModel>.SuccessResponse(emptyResponseBody);
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
				//var raLsPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyABLSPositions(raAbPos?.Select(x=> x.Nr).ToList());


				// - RED Pos
				var redPos = new List<BlanketItem>();
				var oraPos = new List<BlanketItem>();
				var grePos = new List<BlanketItem>();
				foreach(var raItem in openRahmenPos)
				{
					var raExt = openRaPosExtension.FirstOrDefault(x => x.AngeboteArtikelNr == raItem.Nr);
					if(raExt == null)
						continue;

					// items to consider
					if(raExt.GultigBis.HasValue && raExt.GultigBis.Value <= thresholdDate)
					{
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
								grePos.Add(new BlanketItem(raItem, raExt));
							}
						}
					}
				}

				// -
				emptyResponseBody.RedPositions = redPos;
				emptyResponseBody.OrangePositions = oraPos;
				emptyResponseBody.GreenPositions = grePos;

				////var rahmenPosWithAbThatHasLS = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetABLinkedTorahmens(raNrs);
				////var PosThatHasExtensions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngebotNrs(rahmenPosWithAbThatHasLS);
				////var Colors = Helpers.BlanketHelper.GetPositionsColors(PosThatHasExtensions.Select(x => x.AngeboteArtikelNr).ToList());

				////var posList = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(Colors?.Where(a => a.Item3 == _data.SelectedColor)?.Select(x => x.Item2).ToList());
				////var posListExtensions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngebotNrs(Colors?.Where(a => a.Item3 == _data.SelectedColor)?.Select(x => x.Item2).ToList());

				//var _list = new List<BlanketItem>();
				//var _count = 0;
				//foreach (var item in posList)
				//{
				//    var extension = posListExtensions.Find(x => x.AngeboteArtikelNr == item.Nr);
				//    var colorItem = Colors.Find(x => x.Item2 == item.Nr);
				//    _list.Add(new BlanketItem(item, extension));
				//    _count++;
				//}

				//var FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0;

				//_list = _list.Skip(FirstRowNumber).Take(_data.ItemsPerPage).ToList();
				//if (!string.IsNullOrEmpty(_data.Artikelnummer) && !string.IsNullOrWhiteSpace(_data.Artikelnummer))
				//    _list = _list.Where(x => x.Material.Trim().ToLower().Contains(_data.Artikelnummer.Trim().ToLower())).ToList();
				//if (_data.DateFrom.HasValue && _data.DateTo.HasValue)
				//    _list = _list.Where(x => x.ValidFrom >= _data.DateFrom && x.DateOfExpiry <= _data.DateTo).ToList();

				//var response = new RADashboardResponseModel
				//{
				//    Positions = _list,
				//    RequestedPage = this._data.RequestedPage,
				//    ItemsPerPage = this._data.ItemsPerPage,
				//    AllCount = _count,
				//    AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(_count > 0 ? _count : 0) / this._data.ItemsPerPage)) : 0,
				//};

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
