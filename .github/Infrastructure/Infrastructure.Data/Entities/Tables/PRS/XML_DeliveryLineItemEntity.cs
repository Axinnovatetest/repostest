using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class XML_DeliveryLineItemEntity
	{
		public int Id { get; set; }
		public int IdFile { get; set; }
		public string Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight { get; set; }
		public string Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber { get; set; }
		public string Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark { get; set; }
		public string Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight { get; set; }

		public XML_DeliveryLineItemEntity() { }

		public XML_DeliveryLineItemEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			IdFile = Convert.ToInt32(dataRow["IdFile"]);
			Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight = Convert.ToString(dataRow["Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight"]);
			Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber = Convert.ToString(dataRow["Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber"]);
			Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark = Convert.ToString(dataRow["Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark"]);
			Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight = Convert.ToString(dataRow["Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight"]);
		}
	}
}

