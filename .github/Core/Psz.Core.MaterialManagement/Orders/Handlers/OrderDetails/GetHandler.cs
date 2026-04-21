using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
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

			Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
			if(!string.IsNullOrWhiteSpace(this.data.SortField))
			{
				var sortFieldName = "";
				switch(this.data.SortField.ToLower())
				{
					default:
						sortFieldName = null;
						break;
					case "nr":
						sortFieldName = "[Nr]";
						break;
					case "position":
						sortFieldName = "[Position]";
						break;
					case "bestellnummer":
						sortFieldName = "[Bestellnummer]";
						break;
					case "bezeichnung_1":
						sortFieldName = "[Bezeichnung 1]";
						break;
					case "lagerortname":
						sortFieldName = "[Lagerort_id]";
						break;
					case "liefertermin":
						sortFieldName = "[Liefertermin]";
						break;
					case "anzahl":
						sortFieldName = "[Anzahl]";
						break;
					case "rabatt":
						sortFieldName = "[Rabatt]";
						break;
					case "einzelpreis":
						sortFieldName = "[Einzelpreis]";
						break;
					case "gesamtpreis":
						sortFieldName = "[Gesamtpreis]";
						break;
				}


				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = this.data.SortDesc,
				};
			}
			#endregion
			List<GetResponseModel> result = new List<GetResponseModel>();
			var orderDb = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByOrderIdPaged(data.Id, dataSorting, dataPaging);

			if(orderDb != null && orderDb.Count != 0)
			{
				var orderArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderDb.Select(x => x.Artikel_Nr ?? -1).ToList());


				var LagerorteList = Infrastructure.Data.Access.Tables.MTM.LagerorteAccess.Get(Module.LagerortIds);

				var countWE = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetCountWE(orderDb.Select(x => x.Nr).ToList());
				foreach(var order in orderDb)
				{
					var articles = orderArticles.FindAll(A => A.ArtikelNr == order.Artikel_Nr);
					countWE.TryGetValue(order.Nr, out int weCount);

					foreach(var article in articles)
					{
						result.Add(new GetResponseModel(order, article, LagerorteList, weCount));
					}
				}
			}

			int totalCount = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByOrderIdCount(data.Id);
			return ResponseModel<IPaginatedResponseModel<GetResponseModel>>.SuccessResponse(
				new IPaginatedResponseModel<GetResponseModel>
				{
					Items = result.OrderBy(x => x.Position).ToList(),
					PageRequested = this.data.RequestedPage,
					PageSize = this.data.PageSize,
					TotalCount = totalCount,
					TotalPageCount = this.data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(totalCount > 0 ? totalCount : 0) / this.data.PageSize)) : 0
				});
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
