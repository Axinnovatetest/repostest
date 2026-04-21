using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class AccessProfileEntity
	{
		public string AccessProfileName { get; set; }
		public bool? Administration { get; set; }
		public DateTime? CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public int Id { get; set; }

		public bool? ModuleActivated { get; set; }
		public bool? Shipping { get; set; }
		public bool? ShippingCreate { get; set; }
		public bool? StatisticsLGT { get; set; }
		public bool? StatiticsAuswertung { get; set; }
		public bool? StatiticsDeleteROHScanned { get; set; }
		public bool? StatiticsArticleCustomsNumber { get; set; }
		//--------------------------------------------------------
		public bool? Materialswirtschaft { get; set; }
		public bool? MaterialswirtschaftUmbuchung { get; set; }
		public bool? MaterialswirtschaftEntnahme { get; set; }
		public bool? MaterialswirtschaftZugang { get; set; }
		public bool? MaterialswirtschaftTranfer { get; set; }
		public bool? Customs { get; set; }
		public bool? IsDefault { get; set; }
		//--------------------------------------
		//----------------------------------
		public bool? Inventur { get; set; }
		public bool? InventurReset { get; set; }
		public bool? InventurFG { get; set; }
		public bool? InventurROH { get; set; }
		//----------------------------------
		public bool? PlantBookings { get; set; }
		public bool? PlantBookingCreateCopy { get; set; }
		public bool? PlantBookingDelete { get; set; }
		public bool? PlantBookingEdit { get; set; }
		public bool? PlantBookingReprint { get; set; }
		public bool? PlantBookingView { get; set; }
		public bool? ViewLagerList { get; set; }
		public bool? ViewLogs { get; set; }
		public bool? TransferUmbuchung2 { get; set; }
		public bool? Xls { get; set; }
		public bool? AddArtikelVOH { get; set; }

		public AccessProfileEntity() { }

		public AccessProfileEntity(DataRow dataRow)
		{
			AccessProfileName = Convert.ToString(dataRow["AccessProfileName"]);
			Administration = (dataRow["Administration"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Administration"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			Customs = (dataRow["Customs"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Customs"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ModuleActivated = (dataRow["ModuleActivated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ModuleActivated"]);
			Shipping = (dataRow["Shipping"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Shipping"]);
			ShippingCreate = (dataRow["ShippingCreate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ShippingCreate"]);
			StatisticsLGT = (dataRow["StatisticsLGT"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsLGT"]);
			StatiticsAuswertung = (dataRow["StatiticsAuswertung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatiticsAuswertung"]);
			StatiticsDeleteROHScanned = (dataRow["StatiticsDeleteROHScanned"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatiticsDeleteROHScanned"]);
			StatiticsArticleCustomsNumber = (dataRow["StatiticsArticleCustomsNumber"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatiticsArticleCustomsNumber"]);
			Materialswirtschaft = (dataRow["Materialswirtschaft"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Materialswirtschaft"]);
			MaterialswirtschaftUmbuchung = (dataRow["MaterialswirtschaftUmbuchung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MaterialswirtschaftUmbuchung"]);
			MaterialswirtschaftEntnahme = (dataRow["MaterialswirtschaftEntnahme"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MaterialswirtschaftEntnahme"]);
			MaterialswirtschaftZugang = (dataRow["MaterialswirtschaftZugang"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MaterialswirtschaftZugang"]);
			MaterialswirtschaftTranfer = (dataRow["MaterialswirtschaftTranfer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MaterialswirtschaftTranfer"]);
			Customs = (dataRow["Customs"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Customs"]);
			IsDefault = (dataRow["isDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["isDefault"]);
			Inventur = (dataRow["Inventur"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Inventur"]);
			InventurReset = (dataRow["InventurReset"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InventurReset"]);
			InventurFG = (dataRow["InventurFG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InventurFG"]);
			InventurROH = (dataRow["InventurROH"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InventurROH"]);
			PlantBookings = (dataRow["PlantBookings"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PlantBookings"]);
			PlantBookingCreateCopy = (dataRow["PlantBookingCreateCopy"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PlantBookingCreateCopy"]);
			PlantBookingDelete = (dataRow["PlantBookingDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PlantBookingDelete"]);
			PlantBookingEdit = (dataRow["PlantBookingEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PlantBookingEdit"]);
			PlantBookingReprint = (dataRow["PlantBookingReprint"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PlantBookingReprint"]);
			PlantBookingView = (dataRow["PlantBookingView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PlantBookingView"]);
			ViewLagerList = (dataRow["ViewLagerList"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewLagerList"]);
			ViewLogs = (dataRow["ViewLogs"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewLogs"]);
			TransferUmbuchung2 = (dataRow["TransferUmbuchung2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["TransferUmbuchung2"]);
			Xls = (dataRow["Xls"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Xls"]);
			AddArtikelVOH = (dataRow["AddArtikelVOH"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddArtikelVOH"]);

		}
	}
}

