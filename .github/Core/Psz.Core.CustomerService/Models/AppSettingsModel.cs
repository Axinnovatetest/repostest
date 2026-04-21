using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models
{
	public class AppSettingsModel
	{
		public class CTS
		{
			public List<int> Hauplager { get; set; }
			public List<OpenCustomer> OpenCsCustomers { get; set; }
			public string DeliveryNoteFilesPath { get; set; }
			//rahmen
			public int raMaxCurrentValue { get; set; }
			public int raMinNewValue { get; set; }
			public int raMaxNewValue { get; set; }
			//gutshrift
			public int gsMaxCurrentValue { get; set; }
			public int gsMinNewValue { get; set; }
			public int gsMaxNewValue { get; set; }
			//AB
			public int abMaxCurrentValue { get; set; }
			public int abMinNewValue { get; set; }
			public int abMaxNewValue { get; set; }
			//LS
			public int lsMaxCurrentValue { get; set; }
			public int lsMinNewValue { get; set; }
			public int lsMaxNewValue { get; set; }
			//Rechnung
			public int reMaxCurrentValue { get; set; }
			public int reMinNewValue { get; set; }
			public int reMaxNewValue { get; set; }
			//Forcast
			public int bvMaxCurrentValue { get; set; }
			public int bvMinNewValue { get; set; }
			public int bvMaxNewValue { get; set; }
			public int Delta { get; set; }
			public FooterFactoring FooterFactoring { get; set; }
			public FooterFactoring FooterNotFactoring { get; set; }
		}
		public class OpenCustomer
		{
			public int Number { get; set; }
			public string Name { get; set; }
		}
		public class BSD
		{
			public List<int> ProductionLagerIds { get; set; }
		}
		public class FooterFactoring
		{
			public string Bank { get; set; }
			public string Konto { get; set; }
			public string BLZ { get; set; }
			public string IBAN { get; set; }
			public string SWOFT_BIC { get; set; }
		}
	}
}
