using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	using Psz.Core.CustomerService.CsStatistics.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using Psz.Core.Common.Models;

	public class GetCapacityStatusFAHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CapacityAbFaResponseModel>>>
	{
		private Models.CapacityAbFaRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetCapacityStatusFAHandler(Identity.Models.UserModel user, Models.CapacityAbFaRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<CapacityAbFaResponseModel>> Handle()
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
			switch(_data.Horizon)
			{
				case 1:
					{
						dateFrom = null;
						dateTo = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays); // 2024-01-25 - Khelil change H1 to 41 days
						title = $"bis {dateTo.Value.ToString("dd.MM.yyyy")}";
						break;
					}
				case 2:
					{
						dateFrom = _data.Cumulative == true ? null : DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + 1); // 2024-01-25 - Khelil change H1 to 41 days
						dateTo = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7); // 2024-01-25 - Khelil change H1 to 41 days
						title = $"{(_data.Cumulative == true ? "" : $"von {dateFrom.Value.ToString("dd.MM.yyyy")} - ")}bis {dateTo.Value.ToString("dd.MM.yyyy")}";
						break;
					}
				case 3:
					{
						dateFrom = _data.Cumulative == true ? null : DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7 + 1); // 2024-01-25 - Khelil change H1 to 41 days
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

			return ResponseModel<List<CapacityAbFaResponseModel>>.SuccessResponse(
				   Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetCapacityStatus_FA(_data.ArticleId, dateFrom, dateTo)
				   ?.Select(x => new CapacityAbFaResponseModel(x))
				   ?.ToList());

		}

		public ResponseModel<List<CapacityAbFaResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.access.____*/)
			{
				return ResponseModel<List<CapacityAbFaResponseModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<CapacityAbFaResponseModel>>.SuccessResponse();
		}
	}
}
