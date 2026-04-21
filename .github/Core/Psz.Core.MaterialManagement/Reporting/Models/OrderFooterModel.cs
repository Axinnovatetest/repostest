using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Reporting.Models
{
	public class OrderFooterModel
	{
		public static OrderFooterModel CreateFooter()
		{
			return new OrderFooterModel
			{
				FooterAddress1 = "Im Gstaudach 6",
				FooterAddress2 = "92648 Vohenstrau\u00df",
				FooterAddress3 = "Tel.: +49 9651 924 117-0",

				FooterBankLabel = "Bankverbindung:",
				FooterBankValue1 = "VR Bank Mittlere Oberpfalz eG",
				FooterBankValue2 = "",
				FooterBankValue3 = "",
				FooterBankValue4 = "",

				FooterLabelUst = "Ust.-Id-Nr.:",
				FooterValueUst = "DE 813 706 578",
				FooterLabelSite = "Sitz:",
				FooterValueSite = "Vohenstrau\u00df",
				FooterLabelFax = "Fax:",
				FooterValueFax = "+49 9651 924 117-212",

				FooterLabelManager = "Gesch\u00e4ftsf\u00fchrer:",
				FooterValueManager = "Werner Steinbacher",
				FooterLabelManager2 = "",
				FooterValueManager2 = "",
				FooterLabelTaxId = "Steuernummer:",
				FooterValueTaxId = "255/135/40526",

				FooterLabelHRB = "HRB:",
				FooterValueHRB = "2907 AG Weiden",
				FooterLabelEmail = "E-mail:",
				FooterValueEmail = "info@psz-electronic.com",
				FooterLabelCustomsId = "Zollnummer:",
				FooterValueCustomsId = "488 26 28",

				FooterAccountLabel = "Konto:",
				FooterAccountValue1 = "3 22 66 03",
				FooterAccountValue2 = "",
				FooterAccountValue3 = "",
				FooterAccountValue4 = "",

				FooterBLZLabel = "BLZ:",
				FooterBLZValue1 = "750 691 71",
				FooterBLZValue2 = "",
				FooterBLZValue3 = "",
				FooterBLZValue4 = "",

				FooterIBANLabel = "IBAN:",
				FooterIBANValue1 = "DE04 7506 9171 0003 2266 03",
				FooterIBANValue2 = "",
				FooterIBANValue3 = "",
				FooterIBANValue4 = "",

				FooterSWIFTLabel = "SWIFT-BIC:",
				FooterSWIFTValue1 = "GENODEF1SWD",
				FooterSWIFTValue2 = "",
				FooterSWIFTValue3 = "",
				FooterSWIFTValue4 = ""
			};
		}

		public string FooterAddress1 { get; set; }
		public string FooterAddress2 { get; set; }
		public string FooterAddress3 { get; set; }

		public string FooterBankLabel { get; set; }
		public string FooterBankValue1 { get; set; }
		public string FooterBankValue2 { get; set; }
		public string FooterBankValue3 { get; set; }
		public string FooterBankValue4 { get; set; }

		public string FooterLabelUst { get; set; }
		public string FooterValueUst { get; set; }
		public string FooterLabelSite { get; set; }
		public string FooterValueSite { get; set; }
		public string FooterLabelFax { get; set; }
		public string FooterValueFax { get; set; }

		public string FooterLabelManager { get; set; }
		public string FooterValueManager { get; set; }
		public string FooterLabelManager2 { get; set; }
		public string FooterValueManager2 { get; set; }
		public string FooterLabelTaxId { get; set; }
		public string FooterValueTaxId { get; set; }

		public string FooterLabelHRB { get; set; }
		public string FooterValueHRB { get; set; }
		public string FooterLabelEmail { get; set; }
		public string FooterValueEmail { get; set; }
		public string FooterLabelCustomsId { get; set; }
		public string FooterValueCustomsId { get; set; }

		public string FooterAccountLabel { get; set; }
		public string FooterAccountValue1 { get; set; }
		public string FooterAccountValue2 { get; set; }
		public string FooterAccountValue3 { get; set; }
		public string FooterAccountValue4 { get; set; }

		public string FooterBLZLabel { get; set; }
		public string FooterBLZValue1 { get; set; }
		public string FooterBLZValue2 { get; set; }
		public string FooterBLZValue3 { get; set; }
		public string FooterBLZValue4 { get; set; }

		public string FooterIBANLabel { get; set; }
		public string FooterIBANValue1 { get; set; }
		public string FooterIBANValue2 { get; set; }
		public string FooterIBANValue3 { get; set; }
		public string FooterIBANValue4 { get; set; }

		public string FooterSWIFTLabel { get; set; }
		public string FooterSWIFTValue1 { get; set; }
		public string FooterSWIFTValue2 { get; set; }
		public string FooterSWIFTValue3 { get; set; }
		public string FooterSWIFTValue4 { get; set; }
	}
}
