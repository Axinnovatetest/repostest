using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class GetOrderNrHandler: IHandle<GetOrderNrRequestModel, ResponseModel<List<GetOrderNrReponseModel>>>
	{
		private GetOrderNrRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetOrderNrHandler(UserModel user, GetOrderNrRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<GetOrderNrReponseModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<List<GetOrderNrReponseModel>> Perform(UserModel user, GetOrderNrRequestModel data)
		{

			var dataFilter = new List<Infrastructure.Data.Access.Settings.FilterModel>();
			var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
			{
				FirstRowNumber = 0,
				RequestRows = 10
			};
			dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "ba.erledigt_pos", FirstFilterValue = "0", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Boolean, QueryLevel = 1 });
			dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.erledigt", FirstFilterValue = "0", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Boolean });
			dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.gebucht", FirstFilterValue = "1", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Boolean });
			dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Rahmenbestellung]", SecondFilterValue = "1", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Number });
			dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Bestellung-Nr]", FirstFilterValue = data.Filter, FilterType = Infrastructure.Data.Access.Settings.FilterTypes.String });
			dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[typ]", SecondFilterValue = "Rahmenbestellung", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.String });

			if(data.CanCreateWareneingang)
			{
				var orderNumbers = Infrastructure.Data.Access.Joins.MTM.Order.BestellungenAccess.Filter(null, dataPaging, filters: dataFilter);
				return ResponseModel<List<GetOrderNrReponseModel>>.SuccessResponse(orderNumbers?.Select(x => new GetOrderNrReponseModel(x))?.ToList());
			}
			else
			{
				var orderNumbers = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetFiltered(data.Filter);
				return ResponseModel<List<GetOrderNrReponseModel>>.SuccessResponse(orderNumbers?.Select(x => new GetOrderNrReponseModel(x))?.ToList());

			}
		}

		public ResponseModel<List<GetOrderNrReponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<GetOrderNrReponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetOrderNrReponseModel>>.SuccessResponse();
		}
	}
}
