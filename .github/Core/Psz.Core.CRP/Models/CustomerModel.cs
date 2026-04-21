using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models
{
	public class CustomerModel
	{
		public int Id { get; set; }
		public int CustomerNumber { get; set; }
		public int? AdressCustomerNumber { get; set; }
		public string Duns { get; set; }
		public string Type { get; set; }
		public string Name { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Contact { get; set; }
		public string Department { get; set; }
		public string StreetPOBox { get; set; }
		public string CountryPostcode { get; set; }
		public string Street { get; set; }
		public string POBox { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public string Email { get; set; }
		public string EGIdentifikationsnummer { get; set; }
		public CustomerModel()
		{

		}
		public CustomerModel(Infrastructure.Data.Entities.Tables.PRS.KundenEntity kundenEntity,
			Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressenEntity
			)
		{
			if(kundenEntity == null)
			{
				return;
			}
			Id = kundenEntity.Nr;
			Type = adressenEntity?.Anrede;
			CustomerNumber = adressenEntity.Kundennummer ?? -1;
			AdressCustomerNumber = adressenEntity?.Kundennummer;
			Name = adressenEntity?.Name1;
			Name2 = adressenEntity?.Name2;
			Name3 = adressenEntity?.Name3;
			Contact = adressenEntity.Briefanrede;
			Department = adressenEntity.Abteilung;
			CountryPostcode = adressenEntity.PLZ_Postfach;
			StreetPOBox = adressenEntity.StraBe;
			Duns = adressenEntity?.Duns;
			Country = adressenEntity?.Land;
			Email = adressenEntity?.EMail;
			Fax = adressenEntity?.Fax;
			Phone = adressenEntity?.Telefon;
			POBox = adressenEntity?.Postfach;
			Street = adressenEntity?.StraBe;
		}
	}
}