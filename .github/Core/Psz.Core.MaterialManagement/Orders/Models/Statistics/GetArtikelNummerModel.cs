using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class GetArtikelNummerModel
	{
		public string ArtikelNummer { get; set; }
		public GetArtikelNummerModel(GetFaultyArtikelNummerEntity data)
		{
			ArtikelNummer = data.Artikelnummer ?? string.Empty;
		}
	}
	public class GetArtikelNummerRequestModel
	{
		[Required]
		public string filter { get; set; }
		[Required]
		public int dispo { get; set; }
	}
}
