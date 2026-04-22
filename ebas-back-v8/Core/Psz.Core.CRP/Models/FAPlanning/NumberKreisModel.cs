using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FAPlanning
{
	public class NumberKreisResponseModel
	{
		public int AdressCustomerNumber { get; set; }
		public string Name { get; set; }
		public string NumberKreis { get; set; }
		public NumberKreisResponseModel(Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity kundeEntity)
		{
			if(kundeEntity is null)
			{
				return;
			}

			// - 
			AdressCustomerNumber = kundeEntity.Kundennummer ?? 0;
			Name = kundeEntity.Kunde;
			NumberKreis = kundeEntity.Nummerschlüssel ?? "";
		}
	}
}
