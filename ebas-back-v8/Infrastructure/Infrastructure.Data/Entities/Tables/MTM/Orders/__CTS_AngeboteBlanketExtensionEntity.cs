using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class __CTS_AngeboteBlanketExtensionEntity
	{
		public int AngeboteNr { get; set; }
		public int? Anhage { get; set; }
		public bool? Archived { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public string Auftraggeber { get; set; }
		public int? BlanketTypeId { get; set; }
		public string BlanketTypeName { get; set; }
		public DateTime? CreateTime { get; set; }
		public int CreateUserId { get; set; }
		public int? CustomerId { get; set; }
		public string CustomerName { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public decimal? GesamtpreisDefault { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public int? StatusId { get; set; }
		public string StatusName { get; set; }
		public int? SupplierId { get; set; }
		public string SupplierName { get; set; }
		public string Warenemfanger { get; set; }

		public __CTS_AngeboteBlanketExtensionEntity() { }

		public __CTS_AngeboteBlanketExtensionEntity(DataRow dataRow)
		{
			AngeboteNr = Convert.ToInt32(dataRow["AngeboteNr"]);
			Anhage = (dataRow["Anhage"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anhage"]);
			Archived = (dataRow["Archived"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Archived"]);
			ArchiveTime = (dataRow["ArchiveTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArchiveTime"]);
			ArchiveUserId = (dataRow["ArchiveUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArchiveUserId"]);
			Auftraggeber = (dataRow["Auftraggeber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Auftraggeber"]);
			BlanketTypeId = (dataRow["BlanketTypeId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BlanketTypeId"]);
			BlanketTypeName = (dataRow["BlanketTypeName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BlanketTypeName"]);
			CreateTime = (dataRow["CreateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateTime"]);
			CreateUserId = Convert.ToInt32(dataRow["CreateUserId"]);
			CustomerId = (dataRow["CustomerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerId"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			GesamtpreisDefault = (dataRow["GesamtpreisDefault"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["GesamtpreisDefault"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			StatusId = (dataRow["StatusId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StatusId"]);
			StatusName = (dataRow["StatusName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StatusName"]);
			SupplierId = (dataRow["SupplierId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierId"]);
			SupplierName = (dataRow["SupplierName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierName"]);
			Warenemfanger = (dataRow["Warenemfanger"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warenemfanger"]);
		}

		public __CTS_AngeboteBlanketExtensionEntity ShallowClone()
		{
			return new __CTS_AngeboteBlanketExtensionEntity
			{
				AngeboteNr = AngeboteNr,
				Anhage = Anhage,
				Archived = Archived,
				ArchiveTime = ArchiveTime,
				ArchiveUserId = ArchiveUserId,
				Auftraggeber = Auftraggeber,
				BlanketTypeId = BlanketTypeId,
				BlanketTypeName = BlanketTypeName,
				CreateTime = CreateTime,
				CreateUserId = CreateUserId,
				CustomerId = CustomerId,
				CustomerName = CustomerName,
				Gesamtpreis = Gesamtpreis,
				GesamtpreisDefault = GesamtpreisDefault,
				Id = Id,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				StatusId = StatusId,
				StatusName = StatusName,
				SupplierId = SupplierId,
				SupplierName = SupplierName,
				Warenemfanger = Warenemfanger
			};
		}
	}
}

