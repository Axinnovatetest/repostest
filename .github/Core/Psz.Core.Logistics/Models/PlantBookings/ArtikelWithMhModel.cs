using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.PlantBookings
{
	public  class ArtikelWithMhModel
	{

			public int artikelNr { get; set; }
			public string artikelnummer { get; set; }
			public string bezeichnung1 { get; set; }
			public string einheit { get; set; }
			public bool MHD { get; set; }
			public ArtikelWithMhModel()
			{

			}
			public ArtikelWithMhModel(Infrastructure.Data.Entities.Tables.Logistics.ArtikelWithMhdEntity artikelEntity)
			{
				if(artikelEntity == null)
					return;
				artikelNr = artikelEntity.artikelNr;
				artikelnummer = artikelEntity.artikelnummer;
				bezeichnung1 = artikelEntity.bezeichnung1;
				einheit = artikelEntity.einheit;
				MHD = artikelEntity.MHD;
			}
		}
	}
