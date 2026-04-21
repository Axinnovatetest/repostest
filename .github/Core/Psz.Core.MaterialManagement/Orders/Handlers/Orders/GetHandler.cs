using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class GetHandler: IHandle<GetRequestModel, ResponseModel<IPaginatedResponseModel<GetResponseModel>>>
	{
		private GetRequestModel data { get; set; }
		private UserModel user { get; set; }
		public GetHandler(UserModel user, GetRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<IPaginatedResponseModel<GetResponseModel>> Handle()
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
		private ResponseModel<IPaginatedResponseModel<GetResponseModel>> Perform(UserModel user, GetRequestModel data)
		{
			#region > Data sorting & paging
			var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
			{
				FirstRowNumber = this.data.PageSize > 0 ? (this.data.RequestedPage * this.data.PageSize) : 0,
				RequestRows = this.data.PageSize
			};

			Infrastructure.Data.Access.Settings.SortingModel dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
			{
				SortFieldName = "[Nr]",
				SortDesc = true,
			};

			if(!string.IsNullOrWhiteSpace(this.data.SortField))
			{
				var sortFieldName = "";
				switch(this.data.SortField.ToLower())
				{
					case "projekt_nr":
						sortFieldName = "[Projekt-Nr]";
						break;
					case "bestellung_nr":
						sortFieldName = "[Bestellung-Nr]";
						break;
					case "vorname_namefirma":
						sortFieldName = "[Vorname/NameFirma]";
						break;
					case "datum":
						sortFieldName = "Datum";
						break;
					default:
					case "nr":
						sortFieldName = "[Nr]";
						break;
				}

				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = this.data.SortDesc,
				};
			}
			#endregion

			#region data filtering & search
			var dataFilter = new List<Infrastructure.Data.Access.Settings.FilterModel>();

			if(data.Lieferanten_Nr.HasValue && data.Lieferanten_Nr != 0)
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Lieferanten-Nr]", FirstFilterValue = data.Lieferanten_Nr.ToString(), FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Number });

			if(data.Bestellung_Nr.HasValue && data.Bestellung_Nr != 0)
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Bestellung-Nr]", FirstFilterValue = data.Bestellung_Nr.ToString(), FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Number });

			if(!string.IsNullOrWhiteSpace(data.Benutzer))
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Bearbeiter]", FirstFilterValue = data.Benutzer, FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Number });

			if(data.DraftsOnly)
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[gebucht]", FirstFilterValue = "0", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Boolean });

			if(data.ProjectPurchase == true)
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[ProjectPurchase]", FirstFilterValue = "1", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Boolean });

			if(!data.IncludeDone.HasValue || data.IncludeDone == false)
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[erledigt]", FirstFilterValue = "0", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Boolean });

			if(data.CanCreateWereingang)
			{
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "ba.erledigt_pos", FirstFilterValue = "0", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Boolean, QueryLevel = 1 });
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.erledigt", FirstFilterValue = "0", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Boolean });
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.gebucht", FirstFilterValue = "1", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Boolean });
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Rahmenbestellung]", SecondFilterValue = "1", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Number });
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[typ]", SecondFilterValue = "Rahmenbestellung", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.String });
			}

			if(!string.IsNullOrWhiteSpace(data.OrderType))
			{
				if(data.OrderType == "Rahmenbestellung")
				{
					dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = $"(a.[Rahmenbestellung]=1 OR a.[typ]='{data.OrderType}')", ConnectorType = " AND", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Query });
				}
				else
				{
					dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Rahmenbestellung]", SecondFilterValue = "1", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Number });
					dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[typ]", FirstFilterValue = data.OrderType, ConnectorType = " AND", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.String });
				}
			}

			if(data.ArtikelNr.HasValue && data.ArtikelNr != 0)
			{
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "ba.[Artikel-Nr]", FirstFilterValue = data.ArtikelNr.ToString(), FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Number, QueryLevel = 1 });
			}

			if(!string.IsNullOrWhiteSpace(data.Projekt_Nr))
			{
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Projekt-Nr]", FirstFilterValue = data.Projekt_Nr.ToString(), FilterType = Infrastructure.Data.Access.Settings.FilterTypes.String });
			}

			if(!string.IsNullOrWhiteSpace(data.CreationDateFrom))
			{
				if(!string.IsNullOrWhiteSpace(data.CreationDateTo))
					dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Datum]", FirstFilterValue = data.CreationDateFrom, SecondFilterValue = data.CreationDateTo, FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Date });
				else
					dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Datum]", FirstFilterValue = data.CreationDateFrom, FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Date });
			}
			else
			{
				if(!string.IsNullOrWhiteSpace(data.CreationDateTo))
					dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Datum]", SecondFilterValue = data.CreationDateTo, FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Date });
			}

			if(!user.Access.MaterialManagement.Purchasing.WE)
			{
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "a.[Typ]", SecondFilterValue = "Wareneingang", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.String });
			}

			if(!string.IsNullOrWhiteSpace(data.DesiredDate))
				dataFilter.Add(new Infrastructure.Data.Access.Settings.FilterModel { FilterFieldName = "ba.[Liefertermin]", FirstFilterValue = data.DesiredDate, FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Date });
			#endregion

			var dataResponse = Infrastructure.Data.Access.Joins.MTM.Order.BestellungenAccess.Filter(dataSorting, dataPaging, dataFilter)?.Select(x => new GetResponseModel(x))?.ToList();

			var totalCount = dataResponse != null && dataResponse.Count > 0 ? dataResponse.FirstOrDefault().TotalCount : 0;
			return ResponseModel<IPaginatedResponseModel<GetResponseModel>>.SuccessResponse(
				new IPaginatedResponseModel<GetResponseModel>
				{
					Items = dataResponse,
					PageRequested = this.data.RequestedPage,
					PageSize = this.data.PageSize,
					TotalCount = totalCount,
					TotalPageCount = this.data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(totalCount > 0 ? totalCount : 0) / this.data.PageSize)) : 0
				}
				);
		}
		public ResponseModel<IPaginatedResponseModel<GetResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<IPaginatedResponseModel<GetResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<IPaginatedResponseModel<GetResponseModel>>.SuccessResponse();
		}
	}
}