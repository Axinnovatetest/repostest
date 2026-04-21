namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class InterTransferResponseModel
	{
		public LagerBewegungTreeModel interntransfer { get; set; }
		public int AllCount { get; set; }

		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
