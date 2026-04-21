using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class GetForecastModel
	{
		public int CustomerId { get; set; }
		public string CustomerNumber { get; set; }
		public string CustomerName { get; set; }
		public int PositionsCount { get; set; }

		public XMLHeaderModel Document { get; set; }
		public XMLHeaderModel DocumentPreviousVersion { get; set; }
		public XMLHeaderModel DocumentNextVersion { get; set; }
		public List<KeyValuePair<long, string>> DocumentVersionNumbers { get; set; }
		public GetForecastModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressenEntity, int positionsCount, Infrastructure.Data.Entities.Tables.CTS.HeaderEntity headerModel, Infrastructure.Data.Entities.Tables.CTS.HeaderEntity headerModelPrev, Infrastructure.Data.Entities.Tables.CTS.HeaderEntity headerModelNext, List<KeyValuePair<long, string>> versionNumbers)
		{
			CustomerId = adressenEntity.Nr;
			CustomerName = adressenEntity.Name1;
			CustomerNumber = (adressenEntity.Kundennummer ?? -1).ToString();
			PositionsCount = positionsCount;
			Document = new XMLHeaderModel(headerModel);
			DocumentPreviousVersion = new XMLHeaderModel(headerModelPrev);
			DocumentNextVersion = new XMLHeaderModel(headerModelNext);
			DocumentVersionNumbers = versionNumbers;
		}
	}
}
