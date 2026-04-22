using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class GetDelforVersionsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DelforItemsResponseModel>>>
	{

		private DelforVersionsRequsetModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetDelforVersionsHandler(Identity.Models.UserModel user, DelforVersionsRequsetModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<DelforItemsResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<DelforItemsResponseModel>();
				var haderVersions = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetDeflorOtherversions(_data.DocumentNumber, _data.CustomerNumber, -1, false, false);
				if(haderVersions != null && haderVersions.Count > 0)
					response = haderVersions.Select(x => new DelforItemsResponseModel
					{
						CustomerNumber = x.PSZCustomernumber ?? -1,
						DocumentNumber = x.DocumentNumber,
						Version = x.ReferenceVersionNumber ?? -1,
						CreatedOn = x.CreationTime,
						ValidFrom = x.ValidFrom,
						ValidTill = x.ValidTill,
						PositionsCount = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetByHeaderId((int)x.Id)?.Count ?? 0,
						HeaderId = (int)x.Id,
					}).ToList();

				return ResponseModel<List<DelforItemsResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<DelforItemsResponseModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<DelforItemsResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<DelforItemsResponseModel>>.SuccessResponse();
		}

	}
}
