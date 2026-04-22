namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class InterTransferKompletModel
	{
		public LagerBewegungTreeModel interntransferWSIN { get; set; }
		public LagerBewegungTreeModel interntransferINWS { get; set; }
		public LagerBewegungTreeModel interntransferTNIN { get; set; }
		public LagerBewegungTreeModel interntransferINTN { get; set; }
	}
}
