using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class XML_OrdersLineItemEntity
	{
		public DateTime? EditTime { get; set; }
		public int Id { get; set; }
		public int IdFile { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis { get; set; }


		public XML_OrdersLineItemEntity() { }

		public XML_OrdersLineItemEntity(DataRow dataRow)
		{
			EditTime = (dataRow["EditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EditTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdFile = Convert.ToInt32(dataRow["IdFile"]);
			Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet"]);
			Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber"]);
			Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text"]);
			Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage"]);
			Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier"]);
			Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription"]);
			Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest"]);
			Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount"]);
			Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier"]);
			Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity"]);
			Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber"]);
			Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier"]);
			Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime"]);
			Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber"]);
			Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber"]);
			Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier"]);
			Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber"]);
			Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis"]);
		}
	}
}

