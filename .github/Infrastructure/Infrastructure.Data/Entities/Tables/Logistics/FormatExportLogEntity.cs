using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.LGT
{
    public class FormatExportLogEntity
    {
		public DateTime? ExportDate { get; set; }
		public int? ExportUserId { get; set; }
		public string ExportUserName { get; set; }
		public int Id { get; set; }
		public int? LagerBewegungId { get; set; }
		public DateTime? SelectedDate { get; set; }
		public int? SelectedLagerFrom { get; set; }
		public int? SelectedLagerTo { get; set; }

        public FormatExportLogEntity() { }

        public FormatExportLogEntity(DataRow dataRow)
        {
			ExportDate = (dataRow["ExportDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ExportDate"]);
			ExportUserId = (dataRow["ExportUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ExportUserId"]);
			ExportUserName = (dataRow["ExportUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ExportUserName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LagerBewegungId = (dataRow["LagerBewegungId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerBewegungId"]);
			SelectedDate = (dataRow["SelectedDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SelectedDate"]);
			SelectedLagerFrom = (dataRow["SelectedLagerFrom"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SelectedLagerFrom"]);
			SelectedLagerTo = (dataRow["SelectedLagerTo"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SelectedLagerTo"]);
        }
    
        public FormatExportLogEntity ShallowClone()
        {
            return new FormatExportLogEntity
            {
			ExportDate = ExportDate,
			ExportUserId = ExportUserId,
			ExportUserName = ExportUserName,
			Id = Id,
			LagerBewegungId = LagerBewegungId,
			SelectedDate = SelectedDate,
			SelectedLagerFrom = SelectedLagerFrom,
			SelectedLagerTo = SelectedLagerTo
            };
        }
    }
}

