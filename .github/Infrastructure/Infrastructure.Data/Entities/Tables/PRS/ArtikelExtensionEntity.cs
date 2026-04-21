using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class ArtikelExtensionEntity
	{
		public int ArtikelNr { get; set; }
		public string Consumption12Months { get; set; }
		public string CreatorOrder { get; set; }
		public int? CustomerContactPersonId { get; set; }
		public string CustomerInquiryNumber { get; set; }
		public string CustomerName { get; set; }
		public int Id { get; set; }
		public int ImageId { get; set; }
		public string OrderNumber { get; set; }
		public DateTime? OrderValidity { get; set; }
		public int? ProjectTypeId { get; set; }
		public decimal? QuotationsBased12Months { get; set; }
		public string Sales12MonthsPerItem { get; set; }
		public DateTime? SOPAppointmentCustomer { get; set; }
		public decimal? CopperCostBasis { get; set; }
		public decimal? CopperCostBasis150 { get; set; }
		public int? CreatorID { get; set; }
		public DateTime? DateCreation { get; set; }

		public ArtikelExtensionEntity() { }

		public ArtikelExtensionEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
			Consumption12Months = (dataRow["Consumption12Months"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Consumption12Months"]);
			CreatorOrder = (dataRow["CreatorOrder"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreatorOrder"]);
			CustomerContactPersonId = (dataRow["CustomerContactPersonId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerContactPersonId"]);
			CustomerInquiryNumber = (dataRow["CustomerInquiryNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerInquiryNumber"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ImageId = Convert.ToInt32(dataRow["ImageId"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderNumber"]);
			OrderValidity = (dataRow["OrderValidity"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["OrderValidity"]);
			ProjectTypeId = (dataRow["ProjectTypeId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjectTypeId"]);
			QuotationsBased12Months = (dataRow["QuotationsBased12Months"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["QuotationsBased12Months"]);
			Sales12MonthsPerItem = (dataRow["Sales12MonthsPerItem"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sales12MonthsPerItem"]);
			SOPAppointmentCustomer = (dataRow["SOPAppointmentCustomer"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SOPAppointmentCustomer"]);
			CopperCostBasis = (dataRow["CopperCostBasis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CopperCostBasis"]);
			CopperCostBasis150 = (dataRow["CopperCostBasis150"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CopperCostBasis150"]);
			CreatorID = (dataRow["CreatorID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreatorID"]);
			DateCreation = (dataRow["DateCreation"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DateCreation"]);
		}
	}
}

