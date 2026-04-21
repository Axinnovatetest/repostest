using Psz.Core.SharedKernel.Interfaces;
using System.Linq;
namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class GetUserHallAndCountryHandler: IHandle<GetUserHallAndCountryRequestModel, ResponseModel<List<GetUserHallAndCountryResponseModel>>>
	{
		private GetUserHallAndCountryRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetUserHallAndCountryHandler(UserModel user, GetUserHallAndCountryRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<GetUserHallAndCountryResponseModel>> Handle()
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
		private ResponseModel<List<GetUserHallAndCountryResponseModel>> Perform(UserModel user, GetUserHallAndCountryRequestModel data)
		{
			var manipulatedata = new List<GetUserHallAndCountryModel>();

			var fetchedData = Infrastructure.Data.Access.Joins.MTM.Order.UserCountryAndHallAccess.GetUserHallAndCountry(user.Id);

			if(fetchedData.Count() == 0 || fetchedData is null)
				return ResponseModel<List<GetUserHallAndCountryResponseModel>>.NotFoundResponse();

			foreach(var item in fetchedData)
			{
				manipulatedata.Add(new GetUserHallAndCountryModel(item));
			}


			var results = (from unit in manipulatedata
						   group new HallDataResponseModel() { HallName = unit.HallName, Hall_Id = unit.Hall_Id } by (unit.Country_Id, unit.CountryName) into cluster
						   select new GetUserHallAndCountryResponseModel() { Country_Id = cluster.Key.Country_Id, CountryName = cluster.Key.CountryName, Halls = cluster.ToList() }).ToList();

			return ResponseModel<List<GetUserHallAndCountryResponseModel>>.SuccessResponse(results);

		}
		public ResponseModel<List<GetUserHallAndCountryResponseModel>> Validate()
		{
			if(user is null)
			{
				return ResponseModel<List<GetUserHallAndCountryResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetUserHallAndCountryResponseModel>>.SuccessResponse();
		}
	}
}
