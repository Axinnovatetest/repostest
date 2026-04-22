using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetOffeneMatBstHandler: IHandle<OffenematbstRequestModel, ResponseModel<IPaginatedResponseModel<OffenematbstModel>>>
	{

		private OffenematbstRequestModel _data { get; set; }
		private UserModel _user { get; set; }
		public GetOffeneMatBstHandler(UserModel user, OffenematbstRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<IPaginatedResponseModel<OffenematbstModel>> Handle()
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

		private ResponseModel<IPaginatedResponseModel<OffenematbstModel>> Perform(UserModel user, OffenematbstRequestModel data)
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
				var fetchedData = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.Offene_mat_bst_access.GetOffeneMatBst(dataPaging, dataSorting, null, data.fromdate, data.todate, 0);
				var restoreturn = fetchedData.Select(x => new OffenematbstModel(x)).ToList();

				int TotalCount = 0;
				if(restoreturn is not null && restoreturn.Count > 0)
				{
					TotalCount = restoreturn.FirstOrDefault().TotalCount;
				}
				return ResponseModel<IPaginatedResponseModel<OffenematbstModel>>.SuccessResponse(
			 new IPaginatedResponseModel<OffenematbstModel>
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
		public ResponseModel<IPaginatedResponseModel<OffenematbstModel>> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<IPaginatedResponseModel<OffenematbstModel>>.AccessDeniedResponse();
			}
			return ResponseModel<IPaginatedResponseModel<OffenematbstModel>>.SuccessResponse();
		}
	}
}
