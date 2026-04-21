using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetLieferPlannungHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<LieferPlannungModel>>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetLieferPlannungHandler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<LieferPlannungModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<LieferPlannungModel>();
				var lieferPlannungEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetLieferPlannung(_data);
				if(lieferPlannungEntity != null && lieferPlannungEntity.Count > 0)
					response = lieferPlannungEntity.Select(a => new LieferPlannungModel(a)).ToList();

				return ResponseModel<List<LieferPlannungModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<LieferPlannungModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<LieferPlannungModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<LieferPlannungModel>>.SuccessResponse();
		}
	}
}
