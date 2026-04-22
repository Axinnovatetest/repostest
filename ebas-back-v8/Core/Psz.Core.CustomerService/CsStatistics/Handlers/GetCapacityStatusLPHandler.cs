using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	using Psz.Core.CustomerService.CsStatistics.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using Psz.Core.Common.Models;

	public class GetCapacityStatusLPHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CapacityLpResponseModel>>>
	{
		private Models.CapacityAbFaRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetCapacityStatusLPHandler(Identity.Models.UserModel user, Models.CapacityAbFaRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<CapacityLpResponseModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			// -
			DateTime? dateFrom = null;
			DateTime? dateTo = null;
			string title = "";
			// 2024-01-25 - Khelil change H1 to 41 days
			switch(_data.Horizon)
			{
				case 1:
					{
						dateFrom = null;
						dateTo = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays);
						title = $"bis {dateTo.Value.ToString("dd.MM.yyyy")}";
						break;
					}
				case 2:
					{
						dateFrom = _data.Cumulative == true ? null : DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + 1);
						dateTo = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7);
						title = $"{(_data.Cumulative == true ? "" : $"von {dateFrom.Value.ToString("dd.MM.yyyy")} - ")}bis {dateTo.Value.ToString("dd.MM.yyyy")}";
						break;
					}
				case 3:
					{
						dateFrom = _data.Cumulative == true ? null : DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7 + 1);
						dateTo = null;
						title = _data.Cumulative == true ? "kein Datumsfilter" : $"von {dateFrom.Value.ToString("dd.MM.yyyy")}";
						break;
					}
				default:
					{
						dateFrom = null;
						dateTo = null;
						title = "kein Datumsfilter";
						break;
					}
			}

			return ResponseModel<List<CapacityLpResponseModel>>.SuccessResponse(
				   Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetCapacityStatus_LP(_data.ArticleId, dateFrom, dateTo)
				   ?.Select(x => new CapacityLpResponseModel(x))
				   ?.ToList());

		}

		public ResponseModel<List<CapacityLpResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.access.____*/)
			{
				return ResponseModel<List<CapacityLpResponseModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<CapacityLpResponseModel>>.SuccessResponse();
		}
	}
}
