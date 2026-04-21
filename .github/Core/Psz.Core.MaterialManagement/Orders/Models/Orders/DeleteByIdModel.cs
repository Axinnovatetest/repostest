namespace Psz.Core.MaterialManagement.Orders.Models.Orders
{
	public class DeleteByIdRequestModel
	{
		public int OrderId { get; set; }
	}
	public class DeleteByIdResponseModel
	{
		public bool Deleted { get; set; }
		public DeleteByIdResponseModel() { }
		public DeleteByIdResponseModel(bool deleted)
		{
			Deleted = deleted;
		}
	}
}
