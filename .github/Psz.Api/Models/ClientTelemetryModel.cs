using System;

namespace Psz.Api.Models
{
	public class ClientTelemetryModel
	{
		public string Url { get; set; }
		public decimal Elapsed { get; set; }
		public string CorrelationId { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
