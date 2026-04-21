using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.ManagementOverview.ProductionWorkload.Handlers
{
	public class GetProductionWarehousesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetProductionWarehousesHandler(Identity.Models.UserModel user)
		{
			_user = user;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				// -
				var entities = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(Module.AppSettingsBSD.ProductionLagerIds);
				var responseBody = new List<KeyValuePair<int, string>>();
				if(entities != null && entities.Count > 0)
				{
					foreach(var item in entities)
					{
						responseBody.Add(new KeyValuePair<int, string>(item.Lagerort_id, item.Lagerort));
					}
				}

				if(!_user.IsGlobalDirector && !_user.IsAdministrator && !_user.IsCorporateDirector && !_user.Access.ManagementOverview.AllProductionWarehouses)
				{
					var werke = Infrastructure.Data.Access.Joins.CapitalRequestsJointsAccess.GetWerkeId(_user.CompanyId);
					var companyLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetByWerke(werke);
					var companyLagerIds = companyLager?.Select(x => x.Lagerort_id).ToList();
					responseBody = responseBody.Where(r => companyLagerIds.Exists(s => s == r.Key)).ToList();
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(_user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			// - 
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}

	}
}
