namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class InsideSalesWerksterminUpdatesEntity
	{
		public int? ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string CustomerName { get; set; }
		public int? CustomerNumber { get; set; }
		public int? CustomerOrderNumber { get; set; }
		public DateTime? EditDate { get; set; }
		public int? EditUserId { get; set; }
		public string EditUserName { get; set; }
		public int? FertigungId { get; set; }
		public int? FertigungNumber { get; set; }
		public int Id { get; set; }
		public bool? InsConfirmation { get; set; }
		public DateTime? InsConfirmationDate { get; set; }
		public int? InsConfirmationUserId { get; set; }
		public string InsConfirmationUserName { get; set; }
		public DateTime? NewWorkDate { get; set; }
		public DateTime? OldWorkDate { get; set; }
		public bool? ReasonCapacity { get; set; }
		public string ReasonCapacityComments { get; set; }
		public bool? ReasonClarification { get; set; }
		public string? ReasonClarificationComments { get; set; }
		public bool? ReasonDefective { get; set; }
		public string ReasonDefectiveComments { get; set; }
		public bool? ReasonMaterial { get; set; }
		public string ReasonMaterialComments { get; set; }
		public bool? ReasonQuality { get; set; }
		public string ReasonQualityComments { get; set; }
		public bool? ReasonStatusP { get; set; }
		public string ReasonStatusPComments { get; set; }

		public InsideSalesWerksterminUpdatesEntity() { }

		public InsideSalesWerksterminUpdatesEntity(DataRow dataRow)
		{
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
			CustomerOrderNumber = (dataRow["CustomerOrderNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerOrderNumber"]);
			EditDate = (dataRow["EditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EditDate"]);
			EditUserId = (dataRow["EditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EditUserId"]);
			EditUserName = (dataRow["EditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EditUserName"]);
			FertigungId = (dataRow["FertigungId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FertigungId"]);
			FertigungNumber = (dataRow["FertigungNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FertigungNumber"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			InsConfirmation = (dataRow["InsConfirmation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InsConfirmation"]);
			InsConfirmationDate = (dataRow["InsConfirmationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["InsConfirmationDate"]);
			InsConfirmationUserId = (dataRow["InsConfirmationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["InsConfirmationUserId"]);
			InsConfirmationUserName = (dataRow["InsConfirmationUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InsConfirmationUserName"]);
			NewWorkDate = (dataRow["NewWorkDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["NewWorkDate"]);
			OldWorkDate = (dataRow["OldWorkDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["OldWorkDate"]);
			ReasonCapacity = (dataRow["ReasonCapacity"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ReasonCapacity"]);
			ReasonCapacityComments = (dataRow["ReasonCapacityComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ReasonCapacityComments"]);
			ReasonClarification = (dataRow["ReasonClarification"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ReasonClarification"]);
			ReasonClarificationComments = (dataRow["ReasonClarificationComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ReasonClarificationComments"]);
			ReasonDefective = (dataRow["ReasonDefective"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ReasonDefective"]);
			ReasonDefectiveComments = (dataRow["ReasonDefectiveComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ReasonDefectiveComments"]);
			ReasonMaterial = (dataRow["ReasonMaterial"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ReasonMaterial"]);
			ReasonMaterialComments = (dataRow["ReasonMaterialComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ReasonMaterialComments"]);
			ReasonQuality = (dataRow["ReasonQuality"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ReasonQuality"]);
			ReasonQualityComments = (dataRow["ReasonQualityComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ReasonQualityComments"]);
			ReasonStatusP = (dataRow["ReasonStatusP"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ReasonStatusP"]);
			ReasonStatusPComments = (dataRow["ReasonStatusPComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ReasonStatusPComments"]);
		}

		public InsideSalesWerksterminUpdatesEntity ShallowClone()
		{
			return new InsideSalesWerksterminUpdatesEntity
			{
				ArticleId = ArticleId,
				ArticleNumber = ArticleNumber,
				CustomerName = CustomerName,
				CustomerNumber = CustomerNumber,
				CustomerOrderNumber = CustomerOrderNumber,
				EditDate = EditDate,
				EditUserId = EditUserId,
				EditUserName = EditUserName,
				FertigungId = FertigungId,
				FertigungNumber = FertigungNumber,
				Id = Id,
				InsConfirmation = InsConfirmation,
				InsConfirmationDate = InsConfirmationDate,
				InsConfirmationUserId = InsConfirmationUserId,
				InsConfirmationUserName = InsConfirmationUserName,
				NewWorkDate = NewWorkDate,
				OldWorkDate = OldWorkDate,
				ReasonCapacity = ReasonCapacity,
				ReasonCapacityComments = ReasonCapacityComments,
				ReasonClarification = ReasonClarification,
				ReasonClarificationComments = ReasonClarificationComments,
				ReasonDefective = ReasonDefective,
				ReasonDefectiveComments = ReasonDefectiveComments,
				ReasonMaterial = ReasonMaterial,
				ReasonMaterialComments = ReasonMaterialComments,
				ReasonQuality = ReasonQuality,
				ReasonQualityComments = ReasonQualityComments,
				ReasonStatusP = ReasonStatusP,
				ReasonStatusPComments = ReasonStatusPComments
			};
		}
	}
}

