using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{

	public class GetDaysOffHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DayOffResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetDaysOffHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<DayOffResponseModel>> Handle()
		{
			try
			{
				//var validationResponse = this.Validate();
				//if(!validationResponse.Success)
				//{
				//	return validationResponse;
				//}



				List<DayOffResponseModel> response = new List<DayOffResponseModel>();

				List<Infrastructure.Data.Entities.Tables.MGO.DayOff> lsAllDaysOff =
					Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetDaysOff("by");
				for(short kwCount = 1; kwCount <= ISOWeek.GetWeeksInYear(System.DateTime.Now.Year);
					kwCount++)
				{
					response.Add(new DayOffResponseModel
					{
						KW = kwCount,
						NbDaysOff = lsAllDaysOff.Count(x => x.KW.HasValue && x.KW.Value == kwCount)
					});
				}


				return ResponseModel<List<DayOffResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<DayOffResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<DayOffResponseModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<DayOffResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<DayOffResponseModel>>.SuccessResponse();
		}

	}
}
