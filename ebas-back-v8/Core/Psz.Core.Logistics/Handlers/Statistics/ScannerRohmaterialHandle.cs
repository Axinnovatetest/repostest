using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class ScannerRohmaterialHandle: IHandle<Identity.Models.UserModel, ResponseModel<List<ScannerRohmaterialModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ScannerRohmaterialSearch _data { get; set; }
		public ScannerRohmaterialHandle(Identity.Models.UserModel user, ScannerRohmaterialSearch _data)
		{
			this._user = user;
			this._data = _data;

		}
		public ResponseModel<List<ScannerRohmaterialModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetScannerRohmaterialEntity(_data.From, _data.To, null, null, null, null);
				var response = PackingListEntity?.Select(x => new ScannerRohmaterialModel(x)).OrderBy(y => y.Scanndatum).ToList();
				return ResponseModel<List<ScannerRohmaterialModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<ScannerRohmaterialModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<ScannerRohmaterialModel>>.AccessDeniedResponse();
			}
			if(!IsValidDate(_data.From))
				return ResponseModel<List<ScannerRohmaterialModel>>.FailureResponse($"Wrong From Date [{_data.From}]");
			if(!IsValidDate(_data.To))
				return ResponseModel<List<ScannerRohmaterialModel>>.FailureResponse($"Wrong To Date [{_data.To}]");
			return ResponseModel<List<ScannerRohmaterialModel>>.SuccessResponse();
		}
		public static bool IsValidDate(DateTime date)
		{
			var year = date.Year;
			if(year < 1753 || year > 9999)
				return false;
			else
				return true;
		}
	}
}