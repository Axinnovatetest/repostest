namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class InsideSalesChecksArchiveEntity
	{
		public int? ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public bool? CheckCRP { get; set; }
		public string CheckCRPComments { get; set; }
		public DateTime? CheckCRPDate { get; set; }
		public bool? CheckCRPDateAdjusted { get; set; }
		public int? CheckCRPUserId { get; set; }
		public string CheckCRPUserName { get; set; }
		public int? CheckCRPWeek { get; set; }
		public bool? CheckCRPWTCompliedOk { get; set; }
		public bool? CheckFa { get; set; }
		public bool? CheckFaAvaialable { get; set; }
		public string CheckFaComments { get; set; }
		public DateTime? CheckFaDate { get; set; }
		public bool? CheckFaDateOk { get; set; }
		public int? CheckFaUserId { get; set; }
		public string CheckFaUserName { get; set; }
		public int? CheckFaWeek { get; set; }
		public bool? CheckFST { get; set; }
		public string CheckFSTComments { get; set; }
		public DateTime? CheckFSTDate { get; set; }
		public bool? CheckFSTKapaOk { get; set; }
		public string CheckFSTKapaReason { get; set; }
		public int? CheckFSTKapaWeek { get; set; }
		public int? CheckFSTUserId { get; set; }
		public string CheckFSTUserName { get; set; }
		public bool? CheckINS { get; set; }
		public bool? CheckINSAbConfirmed { get; set; }
		public string CheckINSComments { get; set; }
		public DateTime? CheckINSDate { get; set; }
		public int? CheckINSUserId { get; set; }
		public string CheckINSUserName { get; set; }
		public bool? CheckPRS { get; set; }
		public string CheckPRSComments { get; set; }
		public DateTime? CheckPRSDate { get; set; }
		public DateTime? CheckPRSMaterialLastDeliveryDate { get; set; }
		public string CheckPRSMaterialMissing { get; set; }
		public bool? CheckPRSMaterialOk { get; set; }
		public int? CheckPRSUserId { get; set; }
		public string CheckPRSUserName { get; set; }
		public bool? CheckStock { get; set; }
		public string CheckStockComments { get; set; }
		public DateTime? CheckStockDate { get; set; }
		public int? CheckStockUserId { get; set; }
		public string CheckStockUserName { get; set; }
		public string CustomerName { get; set; }
		public int? CustomerNumber { get; set; }
		public string CustomerOrderNumber { get; set; }
		public int Id { get; set; }
		public bool? IsCheckedStock { get; set; }
		public DateTime? OrderDeliveryDate { get; set; }
		public int? OrderId { get; set; }
		public int? OrderNumber { get; set; }
		public decimal? OrderOpenQuantity { get; set; }
		public int? OrderPositionId { get; set; }
		public DateTime? RevertArchiveDate { get; set; }
		public int? RevertArchiveUserId { get; set; }
		public string RevertArchiveUserName { get; set; }

		public InsideSalesChecksArchiveEntity() { }

		public InsideSalesChecksArchiveEntity(DataRow dataRow)
		{
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			CheckCRP = (dataRow["CheckCRP"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckCRP"]);
			CheckCRPComments = (dataRow["CheckCRPComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckCRPComments"]);
			CheckCRPDate = (dataRow["CheckCRPDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CheckCRPDate"]);
			CheckCRPDateAdjusted = (dataRow["CheckCRPDateAdjusted"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckCRPDateAdjusted"]);
			CheckCRPUserId = (dataRow["CheckCRPUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CheckCRPUserId"]);
			CheckCRPUserName = (dataRow["CheckCRPUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckCRPUserName"]);
			CheckCRPWeek = (dataRow["CheckCRPWeek"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CheckCRPWeek"]);
			CheckCRPWTCompliedOk = (dataRow["CheckCRPWTCompliedOk"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckCRPWTCompliedOk"]);
			CheckFa = (dataRow["CheckFa"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckFa"]);
			CheckFaAvaialable = (dataRow["CheckFaAvaialable"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckFaAvaialable"]);
			CheckFaComments = (dataRow["CheckFaComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckFaComments"]);
			CheckFaDate = (dataRow["CheckFaDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CheckFaDate"]);
			CheckFaDateOk = (dataRow["CheckFaDateOk"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckFaDateOk"]);
			CheckFaUserId = (dataRow["CheckFaUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CheckFaUserId"]);
			CheckFaUserName = (dataRow["CheckFaUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckFaUserName"]);
			CheckFaWeek = (dataRow["CheckFaWeek"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CheckFaWeek"]);
			CheckFST = (dataRow["CheckFST"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckFST"]);
			CheckFSTComments = (dataRow["CheckFSTComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckFSTComments"]);
			CheckFSTDate = (dataRow["CheckFSTDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CheckFSTDate"]);
			CheckFSTKapaOk = (dataRow["CheckFSTKapaOk"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckFSTKapaOk"]);
			CheckFSTKapaReason = (dataRow["CheckFSTKapaReason"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckFSTKapaReason"]);
			CheckFSTKapaWeek = (dataRow["CheckFSTKapaWeek"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CheckFSTKapaWeek"]);
			CheckFSTUserId = (dataRow["CheckFSTUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CheckFSTUserId"]);
			CheckFSTUserName = (dataRow["CheckFSTUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckFSTUserName"]);
			CheckINS = (dataRow["CheckINS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckINS"]);
			CheckINSAbConfirmed = (dataRow["CheckINSAbConfirmed"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckINSAbConfirmed"]);
			CheckINSComments = (dataRow["CheckINSComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckINSComments"]);
			CheckINSDate = (dataRow["CheckINSDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CheckINSDate"]);
			CheckINSUserId = (dataRow["CheckINSUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CheckINSUserId"]);
			CheckINSUserName = (dataRow["CheckINSUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckINSUserName"]);
			CheckPRS = (dataRow["CheckPRS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckPRS"]);
			CheckPRSComments = (dataRow["CheckPRSComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckPRSComments"]);
			CheckPRSDate = (dataRow["CheckPRSDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CheckPRSDate"]);
			CheckPRSMaterialLastDeliveryDate = (dataRow["CheckPRSMaterialLastDeliveryDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CheckPRSMaterialLastDeliveryDate"]);
			CheckPRSMaterialMissing = (dataRow["CheckPRSMaterialMissing"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckPRSMaterialMissing"]);
			CheckPRSMaterialOk = (dataRow["CheckPRSMaterialOk"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckPRSMaterialOk"]);
			CheckPRSUserId = (dataRow["CheckPRSUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CheckPRSUserId"]);
			CheckPRSUserName = (dataRow["CheckPRSUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckPRSUserName"]);
			CheckStock = (dataRow["CheckStock"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CheckStock"]);
			CheckStockComments = (dataRow["CheckStockComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckStockComments"]);
			CheckStockDate = (dataRow["CheckStockDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CheckStockDate"]);
			CheckStockUserId = (dataRow["CheckStockUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CheckStockUserId"]);
			CheckStockUserName = (dataRow["CheckStockUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckStockUserName"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
			CustomerOrderNumber = (dataRow["CustomerOrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerOrderNumber"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsCheckedStock = (dataRow["IsCheckedStock"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsCheckedStock"]);
			OrderDeliveryDate = (dataRow["OrderDeliveryDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["OrderDeliveryDate"]);
			OrderId = (dataRow["OrderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderId"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderNumber"]);
			OrderOpenQuantity = (dataRow["OrderOpenQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["OrderOpenQuantity"]);
			OrderPositionId = (dataRow["OrderPositionId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderPositionId"]);
			RevertArchiveDate = (dataRow["RevertArchiveDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["RevertArchiveDate"]);
			RevertArchiveUserId = (dataRow["RevertArchiveUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RevertArchiveUserId"]);
			RevertArchiveUserName = (dataRow["RevertArchiveUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RevertArchiveUserName"]);
		}

		public InsideSalesChecksArchiveEntity ShallowClone()
		{
			return new InsideSalesChecksArchiveEntity
			{
				ArticleId = ArticleId,
				ArticleNumber = ArticleNumber,
				CheckCRP = CheckCRP,
				CheckCRPComments = CheckCRPComments,
				CheckCRPDate = CheckCRPDate,
				CheckCRPDateAdjusted = CheckCRPDateAdjusted,
				CheckCRPUserId = CheckCRPUserId,
				CheckCRPUserName = CheckCRPUserName,
				CheckCRPWeek = CheckCRPWeek,
				CheckCRPWTCompliedOk = CheckCRPWTCompliedOk,
				CheckFa = CheckFa,
				CheckFaAvaialable = CheckFaAvaialable,
				CheckFaComments = CheckFaComments,
				CheckFaDate = CheckFaDate,
				CheckFaDateOk = CheckFaDateOk,
				CheckFaUserId = CheckFaUserId,
				CheckFaUserName = CheckFaUserName,
				CheckFaWeek = CheckFaWeek,
				CheckFST = CheckFST,
				CheckFSTComments = CheckFSTComments,
				CheckFSTDate = CheckFSTDate,
				CheckFSTKapaOk = CheckFSTKapaOk,
				CheckFSTKapaReason = CheckFSTKapaReason,
				CheckFSTKapaWeek = CheckFSTKapaWeek,
				CheckFSTUserId = CheckFSTUserId,
				CheckFSTUserName = CheckFSTUserName,
				CheckINS = CheckINS,
				CheckINSAbConfirmed = CheckINSAbConfirmed,
				CheckINSComments = CheckINSComments,
				CheckINSDate = CheckINSDate,
				CheckINSUserId = CheckINSUserId,
				CheckINSUserName = CheckINSUserName,
				CheckPRS = CheckPRS,
				CheckPRSComments = CheckPRSComments,
				CheckPRSDate = CheckPRSDate,
				CheckPRSMaterialLastDeliveryDate = CheckPRSMaterialLastDeliveryDate,
				CheckPRSMaterialMissing = CheckPRSMaterialMissing,
				CheckPRSMaterialOk = CheckPRSMaterialOk,
				CheckPRSUserId = CheckPRSUserId,
				CheckPRSUserName = CheckPRSUserName,
				CheckStock = CheckStock,
				CheckStockComments = CheckStockComments,
				CheckStockDate = CheckStockDate,
				CheckStockUserId = CheckStockUserId,
				CheckStockUserName = CheckStockUserName,
				CustomerName = CustomerName,
				CustomerNumber = CustomerNumber,
				CustomerOrderNumber = CustomerOrderNumber,
				Id = Id,
				IsCheckedStock = IsCheckedStock,
				OrderDeliveryDate = OrderDeliveryDate,
				OrderId = OrderId,
				OrderNumber = OrderNumber,
				OrderOpenQuantity = OrderOpenQuantity,
				OrderPositionId = OrderPositionId,
				RevertArchiveDate = RevertArchiveDate,
				RevertArchiveUserId = RevertArchiveUserId,
				RevertArchiveUserName = RevertArchiveUserName
			};
		}
	}
}

