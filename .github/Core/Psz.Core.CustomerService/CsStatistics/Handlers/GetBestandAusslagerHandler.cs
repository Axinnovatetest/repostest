using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetBestandAusslagerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<BestandReportDetailsModel>>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBestandAusslagerHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<BestandReportDetailsModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<BestandReportDetailsModel>();
				var bestandAusslagerEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetBestandAusenlager(_data);
				if(bestandAusslagerEntity != null && bestandAusslagerEntity.Count > 0)
					response = bestandAusslagerEntity.Select(a => new BestandReportDetailsModel(a)).ToList();

				return ResponseModel<List<BestandReportDetailsModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<BestandReportDetailsModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<BestandReportDetailsModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<BestandReportDetailsModel>>.SuccessResponse();
		}
	}
}
