namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class GetUserHallAndCountryModel
	{
		public int Country_Id { get; set; }
		public int Hall_Id { get; set; }
		public string HallName { get; set; }
		public string CountryName { get; set; }
		public GetUserHallAndCountryModel(Infrastructure.Data.Entities.Joins.MTM.Order.GetUserHallAndCountryEntity data)
		{
			Country_Id = data.Country_Id;
			Hall_Id = data.Hall_Id;
			CountryName = data.CountryName;
			HallName = data.HallName;
		}
	}
	public class GetUserHallAndCountryResponseModel
	{
		public int Country_Id { get; set; }
		public string CountryName { get; set; }
		public List<HallDataResponseModel> Halls { get; set; }

		public GetUserHallAndCountryResponseModel()
		{
			Halls = new List<HallDataResponseModel>();
		}
	}
	public class HallDataResponseModel
	{
		public string HallName { get; set; }
		public int Hall_Id { get; set; }
	}
	public class GetUserHallAndCountryRequestModel
	{
		public int filter { get; set; } = 0;

	}

}
