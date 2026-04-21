using Infrastructure.Data.Entities.Tables.COR;

namespace Psz.Core.MaterialManagement.Orders.Models.Users
{
	public class GetResponseModel
	{
		public string UserName { get; set; }
		public int UserId { get; set; }

		public GetResponseModel() { }
		public GetResponseModel(string entity)
		{
			UserName = entity;
			UserName = entity;
		}

		public GetResponseModel(UserEntity x)
		{
			UserName = !string.IsNullOrWhiteSpace(x.LegacyUsername) ? x.LegacyUsername : x.Username;
			UserId = x.Nummer != 0 ? x.Nummer : x.Id;
		}
	}
	public class GetRequestModel
	{
		public string Filter { get; set; }

	}
}
