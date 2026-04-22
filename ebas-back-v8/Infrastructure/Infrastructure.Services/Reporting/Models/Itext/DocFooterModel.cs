using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Reporting.Models.Itext
{
	public class DocFooterModel
	{
		public string footerAddress1 { get; set; }
		public string footerAddress2 { get; set; }
		public string footerAddress3 { get; set; }

		public string footerBankLabel { get; set; }
		public string footerBankValue1 { get; set; }
		public string footerBankValue2 { get; set; }
		public string footerBankValue3 { get; set; }
		public string footerBankValue4 { get; set; }

		public string footerLabelUst { get; set; }
		public string footerValueUst { get; set; }
		public string footerLabelSite { get; set; }
		public string footerValueSite { get; set; }
		public string footerLabelFax { get; set; }
		public string footerValueFax { get; set; }


		public string footerLabelManager { get; set; }
		public string footerValueManager { get; set; }
		public string footerLabelManager2 { get; set; }
		public string footerValueManager2 { get; set; }
		public string footerLabelTaxId { get; set; }
		public string footerValueTaxId { get; set; }

		public string footerLabelHRB { get; set; }
		public string footerValueHRB { get; set; }
		public string footerLabelEmail { get; set; }
		public string footerValueEmail { get; set; }
		public string footerLabelCustomsId { get; set; }
		public string footerValueCustomsId { get; set; }
		//Konto
		public string footerAccountLabel { get; set; }
		public string footerAccountValue1 { get; set; }
		public string footerAccountValue2 { get; set; }
		public string footerAccountValue3 { get; set; }
		public string footerAccountValue4 { get; set; }
		//BLZ
		public string footerBLZLabel { get; set; }
		public string footerBLZValue1 { get; set; }
		public string footerBLZValue2 { get; set; }
		public string footerBLZValue3 { get; set; }
		public string footerBLZValue4 { get; set; }
		//IBAN
		public string footerIBANLabel { get; set; }
		public string footerIBANValue1 { get; set; }
		public string footerIBANValue2 { get; set; }
		public string footerIBANValue3 { get; set; }
		public string footerIBANValue4 { get; set; }
		//SWIFT
		public string footerSWIFTLabel { get; set; }
		public string footerSWIFTValue1 { get; set; }
		public string footerSWIFTValue2 { get; set; }
		public string footerSWIFTValue3 { get; set; }
		public string footerSWIFTValue4 { get; set; }
		public string Form { get; set; }
		// -- for customer factoring based footer --souilmi 14/10/2024 ticket#40262--
		public string Footer78 { get; set; }
		public string Footer79 { get; set; }
		public string Footer80 { get; set; }
		public string Footer81 { get; set; }
		public string Footer82 { get; set; }
		public DocFooterModel(Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity entity)
		{
			footerAddress1 = entity.Footer11;
			footerAddress2 = entity.Footer21;
			footerAddress3 = entity.Footer31;

			footerBankLabel = entity.Footer41;
			footerBankValue1 = entity.Footer51;
			footerBankValue2 = entity.Footer61;
			footerBankValue3 = entity.Footer71;
			footerBankValue4 = "";

			footerLabelUst = entity.Footer12;
			footerValueUst = entity.Footer13;
			footerLabelSite = entity.Footer22;
			footerValueSite = entity.Footer23;
			footerLabelFax = entity.Footer32;
			footerValueFax = entity.Footer33;


			footerLabelManager = entity.Footer14;
			footerValueManager = entity.Footer15;
			footerLabelManager2 = "";
			footerValueManager2 = "";
			footerLabelTaxId = entity.Footer34;
			footerValueTaxId = entity.Footer35;

			footerLabelHRB = entity.Footer16;
			footerValueHRB = entity.Footer17;
			footerLabelEmail = entity.Footer26;
			footerValueEmail = entity.Footer27;
			footerLabelCustomsId = entity.Footer36;
			footerValueCustomsId = entity.Footer37;
			//Konto
			footerAccountLabel = entity.Footer43;
			footerAccountValue1 = entity.Footer53;
			footerAccountValue2 = entity.Footer63;
			footerAccountValue3 = entity.Footer73;
			footerAccountValue4 = "";
			//BLZ
			footerBLZLabel = entity.Footer44;
			footerBLZValue1 = entity.Footer54;
			footerBLZValue2 = entity.Footer64;
			footerBLZValue3 = entity.Footer74;
			footerBLZValue4 = "";
			//IBAN
			footerIBANLabel = entity.Footer45;
			footerIBANValue1 = entity.Footer55;
			footerIBANValue2 = entity.Footer65;
			footerIBANValue3 = entity.Footer75;
			footerIBANValue4 = "";
			//SWIFT
			footerSWIFTLabel = entity.Footer47;
			footerSWIFTValue1 = entity.Footer57;
			footerSWIFTValue2 = entity.Footer67;
			footerSWIFTValue3 = entity.Footer77;
			footerSWIFTValue4 = "";

			Footer78 = entity.Footer78;
			Footer79 = entity.Footer79;
			Footer80 = entity.Footer80;
			Footer81 = entity.Footer81;
			Footer82 = entity.Footer82;
		}
	}
}