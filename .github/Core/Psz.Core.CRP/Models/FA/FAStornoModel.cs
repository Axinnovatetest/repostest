namespace Psz.Core.CRP.Models.FA
{
	public class FAStornoModel
	{
		public int fa { get; set; }
		public int FaId { get; set; }
		public string? grund { get; set; }
		public bool? CancelUBGFas { get; set; } = true;
		public FAStornoModel()
		{

		}
	}
}