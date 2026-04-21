using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class GetForSearchHandler: IHandle<SearchRequestModel, ResponseModel<List<SearchResponseModel>>>
	{

		private SearchRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetForSearchHandler(UserModel user, SearchRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<SearchResponseModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<List<SearchResponseModel>> Perform()
		{
			List<string> clause = new List<string>();
			#region data filtering & search
			var dataFilter = new List<Infrastructure.Data.Access.Settings.FilterModel>();
			if(data.Lieferanten_Nr != 0)
				clause.Add($"[Lieferanten-Nr] LIKE '{data.Lieferanten_Nr}%'");
			if(data.Bestellung_Nr != 0)
				clause.Add($"[Bestellung-Nr] LIKE '{data.Bestellung_Nr}%'");
			if(data.Projekt_Nr != 0)
				clause.Add($"[Projekt-Nr] LIKE '{data.Projekt_Nr}%'");
			if(!string.IsNullOrWhiteSpace(data.Benutzer))
				clause.Add($"[Bearbeiter]={data.Benutzer}");
			if(!string.IsNullOrWhiteSpace(data.OrderType))
			{
				if(data.OrderType == "Rahmenbestellung")
				{
					clause.Add($"([Rahmenbestellung=1 OR [typ] LIKE '{data.OrderType}%')");

				}
				else
				{
					clause.Add($"([Rahmenbestellung=1 AND [typ] LIKE '{data.OrderType}%')");
				}
			}

			#endregion


			return ResponseModel<List<SearchResponseModel>>.SuccessResponse(
				BestellungenAccess.Search(clause.Count > 0 ? string.Join(" AND ", clause) : "")
				?.Select(x => new SearchResponseModel(x))
				?.ToList()
				);
		}

		public ResponseModel<List<SearchResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<SearchResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<SearchResponseModel>>.SuccessResponse();
		}
	}
}
