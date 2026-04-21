using Infrastructure.Data.Entities.Tables.CRP;

namespace Psz.Core.CRP.Models.FA
{
	public class FaUserEmailResponseModel
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public int UserId { get; set; }

		public FaUserEmailResponseModel()
		{
		}

		public FaUserEmailResponseModel(CRP_FA_EmailUsersEntity FaEmailUserEntity)
		{
			Id = FaEmailUserEntity.Id;
			UserName = FaEmailUserEntity.UserName;
			UserId = (int)FaEmailUserEntity.UserId;
		}
	}
}
