using MoreLinq.Extensions;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Statistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.ManagementOverview.Statistics.Handlers
{
	public class GetDelKupferHandler: IHandle<Identity.Models.UserModel, ResponseModel<KeyValuePair<int, int>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetDelKupferHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<KeyValuePair<int, int>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				///

				return ResponseModel<KeyValuePair<int, int>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.Statistics.MGO.StatisticsAccess.GetDelKupferbasis());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<KeyValuePair<int, int>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<KeyValuePair<int, int>>.AccessDeniedResponse();
			}
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<KeyValuePair<int, int>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<KeyValuePair<int, int>>.SuccessResponse();
		}
	}
}
