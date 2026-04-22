namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class GetCustomerModel
	{
		public int CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerNumber { get; set; }
		public string CustomerDUNS { get; set; }
		public int UnvalidatedDocumentsCount { get; set; }
		public int ErrorDocumentsCount { get; set; }
		public int ValidatedErrorDocumentCount { get; set; }

		public GetCustomerModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressenEntity, int unvalidatedCount, int errorCount, int validatedErrorCount, string customerDUNS)
		{
			if(adressenEntity == null)
				return;
			CustomerId = adressenEntity.Nr;
			CustomerName = adressenEntity.Name1;
			CustomerNumber = (adressenEntity.Kundennummer ?? -1).ToString();
			UnvalidatedDocumentsCount = unvalidatedCount;
			ErrorDocumentsCount = errorCount;
			ValidatedErrorDocumentCount = validatedErrorCount;
			CustomerDUNS = customerDUNS;
		}
		public GetCustomerModel()
		{

		}
	}
}
