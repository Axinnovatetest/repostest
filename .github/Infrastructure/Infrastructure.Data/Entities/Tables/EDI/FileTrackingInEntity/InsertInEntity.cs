namespace Infrastructure.Data.Entities.Tables.EDI.FileTrackingInEntity
{

	public class InsertInEntity
	{
		public string FileName { get; set; }
		public string Number { get; set; }
		public string Segment2 { get; set; }
		public string DeliveryNoteNumber { get; set; }
		public DateTime? FileDateTime { get; set; }
		public DateTime? LastUpdateTime { get; set; }

	}

}
