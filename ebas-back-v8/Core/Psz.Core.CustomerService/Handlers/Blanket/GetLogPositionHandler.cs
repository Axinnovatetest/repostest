using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetLogPositionHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<LogPositionResponseModel>>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetLogPositionHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<LogPositionResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var rahmenPosPricelogsEntities = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByPosition(_data);
				var _grouped = GroupList(rahmenPosPricelogsEntities);
				var response = _grouped?.Select(x => new LogPositionResponseModel(x)).ToList();


				return ResponseModel<List<LogPositionResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<LogPositionResponseModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<LogPositionResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<LogPositionResponseModel>>.SuccessResponse();
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> GroupList(List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> _list)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
			if(_list != null && _list.Count > 0)
			{
				var Prices = _list?.DistinctBy(x => new { x.Price, x.ValidFrom })?.ToList();
				foreach(var item in Prices)
				{
					var _samePrice = _list?.Where(x => x.Price == item.Price && x.ValidFrom == item.ValidFrom).ToList();
					var _maxUpdateDate = _samePrice?.Max(x => x.DateUpdate);
					result.Add(_samePrice?.Find(x => x.DateUpdate == _maxUpdateDate));
				}
				return result;
			}
			else
				return new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
		}
	}
}
