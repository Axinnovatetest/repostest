using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStock
{
		public class InsertReporttwoEntity
		{
			public int ArtikelNr { get; set; }
			public string Artikelnummer { get; set; }
			public decimal GefundeneMengeInProduktion { get; set; }
			public int Id { get; set; }
			public decimal Lagerbestand { get; set; }
			public decimal MengeInProduktion { get; set; }
			public string RueckbuchungBestaetigt { get; set; }

			public InsertReporttwoEntity() { }

			public InsertReporttwoEntity(DataRow dataRow)
			{
				ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
				Artikelnummer = Convert.ToString(dataRow["Artikelnummer"]);
				GefundeneMengeInProduktion = Convert.ToDecimal(dataRow["GefundeneMengeInProduktion"]);
				Lagerbestand = Convert.ToDecimal(dataRow["Lagerbestand"]);
				MengeInProduktion = Convert.ToDecimal(dataRow["MengeInProduktion"]);
				RueckbuchungBestaetigt = Convert.ToString(dataRow["RueckbuchungBestaetigt"]);
	
			}

			public InsertReporttwoEntity ShallowClone()
			{
				return new InsertReporttwoEntity
				{
					ArtikelNr = ArtikelNr,
					Artikelnummer = Artikelnummer,
					GefundeneMengeInProduktion = GefundeneMengeInProduktion,
					Lagerbestand = Lagerbestand,
					MengeInProduktion = MengeInProduktion,
					RueckbuchungBestaetigt = RueckbuchungBestaetigt
				};
			}
		}
	}

