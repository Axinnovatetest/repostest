using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetGeschlMatBstHandler: IHandle<OffenematbstRequestModel, ResponseModel<IPaginatedResponseModel<GeschlmatbstModel>>>
	{

		private OffenematbstRequestModel _data { get; set; }
		private UserModel _user { get; set; }
		public GetGeschlMatBstHandler(UserModel user, OffenematbstRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<IPaginatedResponseModel<GeschlmatbstModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this._user, this._data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<IPaginatedResponseModel<GeschlmatbstModel>> Perform(UserModel user, OffenematbstRequestModel data)
		{
			try
			{
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.PageSize
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(this._data.SortField))
				{
					string sortFieldName = this._data.SortField switch
					{
						"Benutzer" => "Benutzer",
						"Lieferantennr" => "Lieferantennr",
						"Lieferant" => "Lieferant",
						"Bestellung_Nr" => "[Bestellung-Nr]",
						"Anzahl" => "Anzahl",
						"Mindestbestellmenge" => "Mindestbestellmenge",
						"Verpackungseinheit" => "Verpackungseinheit",
						"Bezeichnung1" => "[Bezeichnung 1]",
						"Fälligkeit" => "Fälligkeit",

						_ => "Bestätigter_Termin"
					};
					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}

				var fetcheData = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.Offene_mat_bst_access.GetGeschlMatBst(dataPaging, dataSorting, null, data.fromdate, data.todate, 0);

				var restoreturn = fetcheData.Select(x => new GeschlmatbstModel(x)).ToList();
				int TotalCount = 0;
				if(restoreturn is not null && restoreturn.Count > 0)
				{
					TotalCount = restoreturn.FirstOrDefault().TotalCount;
				}
				return ResponseModel<IPaginatedResponseModel<GeschlmatbstModel>>.SuccessResponse(
			 new IPaginatedResponseModel<GeschlmatbstModel>
			 {
				 Items = restoreturn,
				 PageRequested = this._data.RequestedPage,
				 PageSize = this._data.PageSize,
				 TotalCount = TotalCount,
				 TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(TotalCount > 0 ? TotalCount : 0) / this._data.PageSize)) : 0
			 });
			} catch(Exception ex)
			{
				throw;
			}
		}
		public ResponseModel<IPaginatedResponseModel<GeschlmatbstModel>> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<IPaginatedResponseModel<GeschlmatbstModel>>.AccessDeniedResponse();
			}
			return ResponseModel<IPaginatedResponseModel<GeschlmatbstModel>>.SuccessResponse();
		}
	}
}
