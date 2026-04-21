using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using static Psz.Core.CustomerService.Models.Statistics.OrderProcessingResponseModel;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetOrderProcessingByYearByMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<OrderProcessingResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.StatRequestModel _data { get; set; }

		public GetOrderProcessingByYearByMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<OrderProcessingResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var nb1 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats2(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var nb2 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats3();
				var statEntityABLS = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats4(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var statEntityPay = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats5(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var statEntityEDI = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats6(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				OrderProcessingResponseModel response = new OrderProcessingResponseModel();
				var _AllABLS = new List<Item> { new Item { Type = "Auftragsbestätigung", Count = 0 }, new Item { Type = "Lieferschein", Count = 0 }, new Item { Type = "Rahmenauftrag", Count = 0 }, new Item { Type = "Gutschrift", Count = 0 } };
				if(statEntityABLS != null && statEntityABLS.Count > 0)
				{
					foreach(var item in statEntityABLS)
					{
						_AllABLS = UpdateList(_AllABLS, item);
					}

				}
				response.AllABLS = _AllABLS;
				if(statEntityPay != null && statEntityPay.Count > 0)
				{
					response.AllPay = statEntityPay.Select(x => new Item1(x)).ToList();
				}
				else
				{
					response.AllPay = new List<Item1> { new Item1 { Pay = "Rechnung", Count = 0 }, new Item1 { Pay = "Vorkasse", Count = 0 }
					, new Item1 { Pay = "Vorauskasse", Count = 0 }, new Item1 { Pay = "Gutschriftverfahren", Count = 0 }, new Item1 { Pay = "Lastschrift", Count = 0 }
					, new Item1 { Pay = "Überweisung", Count = 0 }};
				}

				if(statEntityEDI != null && statEntityEDI.Count > 0)
				{
					response.AllEDI = statEntityEDI.Select(x => new Item2(x)).ToList();
				}
				else
				{
					response.AllEDI = new List<Item2> { new Item2 { Type = "Auftragsbestätigung", Count = 0 }, new Item2 { Type = "Lieferschein", Count = 0 } };
				}
				response.TotalProjects = nb1;
				response.LastCreatedProject = nb2;
				return ResponseModel<OrderProcessingResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<OrderProcessingResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<OrderProcessingResponseModel>.AccessDeniedResponse();
			}
			return ResponseModel<OrderProcessingResponseModel>.SuccessResponse();
		}
		public List<Item> UpdateList(
			List<Item> list,
			Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTypEntity item)
		{
			foreach(var v in list)
			{
				if(v.Type == item.Type)
					v.Count = item.Count;
			}
			return list;
		}
	}
}
