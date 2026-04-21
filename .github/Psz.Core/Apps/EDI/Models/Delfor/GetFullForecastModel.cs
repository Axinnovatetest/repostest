namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class GetFullForecastModel
	{
		public int CustomerId { get; set; }
		public string CustomerNumber { get; set; }
		public string CustomerName { get; set; }

		public XMLHeaderModel Document { get; set; }
		public XMLLineItemModel LineItem { get; set; }
		public GetFullForecastModel(
			Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressenEntity,
			Infrastructure.Data.Entities.Tables.CTS.HeaderEntity headerModel,
			Infrastructure.Data.Entities.Tables.CTS.LineItemEntity lineItemEntity
			)
		{
			CustomerId = adressenEntity.Nr;
			CustomerName = adressenEntity.Name1;
			CustomerNumber = (adressenEntity.Kundennummer ?? -1).ToString();
			Document = new XMLHeaderModel(headerModel);
			LineItem = new XMLLineItemModel(lineItemEntity);
		}
	}
}
