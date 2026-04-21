using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class XML_OrdersScheduleLineEntity
	{
		public DateTime? CreateTime { get; set; }
		public DateTime? EditTime { get; set; }
		public int Id { get; set; }
		public int IdOrderElement { get; set; }
		public int IdOrdersLineItem { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier { get; set; }
		public string Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime { get; set; }

		public XML_OrdersScheduleLineEntity() { }

		public XML_OrdersScheduleLineEntity(DataRow dataRow)
		{
			CreateTime = (dataRow["CreateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateTime"]);
			EditTime = (dataRow["EditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EditTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdOrderElement = Convert.ToInt32(dataRow["IdOrderElement"]);
			IdOrdersLineItem = Convert.ToInt32(dataRow["IdOrdersLineItem"]);
			Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity"]);
			Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity"]);
			Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier"]);
			Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime = (dataRow["Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime"]);
		}
	}
}

