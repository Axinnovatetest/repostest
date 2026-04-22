using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class Dispo120Handler: IHandle<GetDispows120RequestModel, ResponseModel<IPaginatedResponseModel<GetDispows120ResponseModel>>>
	{

		private GetDispows120RequestModel _data { get; set; }
		private UserModel _user { get; set; }
		public Dispo120Handler(UserModel user, GetDispows120RequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<IPaginatedResponseModel<GetDispows120ResponseModel>> Handle()
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

		private ResponseModel<IPaginatedResponseModel<GetDispows120ResponseModel>> Perform(UserModel user, GetDispows120RequestModel data)
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
						"Name1" => "Name1",
						"Stucklisten_Artikelnummer" => "Stücklisten_Artikelnummer",
						"Bezeichnung" => "[Bezeichnung des Bauteils]",
						"SummevonBruttobedarf" => "SummevonBruttobedarf",
						"MaxvonTermin_Materialbedarf" => "MaxvonTermin_Materialbedarf",
						"Bestand" => "Bestand",
						"Differenz" => "Bestand - SummevonBruttobedarf",
						"Mindestbestellmenge" => "Mindestbestellmenge",
						_ => "Name1"
					};
					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}
				List<Dispows120Entity> fetchedData = new List<Dispows120Entity>();

				fetchedData = _data.Dispo switch
				{
					1 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws90(dataPaging, dataSorting, data.Filter),
					2 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws40New(dataPaging, dataSorting, data.Filter),
					3 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetTN90(dataPaging, dataSorting, data.Filter),
					4 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetTN40(dataPaging, dataSorting, data.Filter),
					5 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getbetn90(dataPaging, dataSorting, _data.Filter),
					6 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getbetn40(dataPaging, dataSorting, data.Filter),
					7 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetCZ90(dataPaging, dataSorting, data.Filter),
					8 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetCZ30(dataPaging, dataSorting, data.Filter),
					9 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getal90(dataPaging, dataSorting, data.Filter),
					10 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getal30(dataPaging, dataSorting, data.Filter),
					11 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetGZ90(dataPaging, dataSorting, data.Filter),
					12 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetGZ40(dataPaging, dataSorting, data.Filter),
					13 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetDE90(dataPaging, dataSorting, data.Filter),
					_ => null

				};

				var restoreturn = fetchedData.Select(x => new GetDispows120ResponseModel(x)).ToList();
				int TotalCount = 0;
				if(restoreturn is not null && restoreturn.Count > 0)
				{
					TotalCount = restoreturn.FirstOrDefault().TotalCount;
				}

				return ResponseModel<IPaginatedResponseModel<GetDispows120ResponseModel>>.SuccessResponse(
			 new IPaginatedResponseModel<GetDispows120ResponseModel>
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
		public ResponseModel<IPaginatedResponseModel<GetDispows120ResponseModel>> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<IPaginatedResponseModel<GetDispows120ResponseModel>>.AccessDeniedResponse();
			}
			return ResponseModel<IPaginatedResponseModel<GetDispows120ResponseModel>>.SuccessResponse();
		}
	}
}
