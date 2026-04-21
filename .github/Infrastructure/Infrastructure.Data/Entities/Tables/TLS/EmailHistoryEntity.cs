namespace Infrastructure.Data.Entities.Tables.TLS;

public class EmailHistoryEntity
{
	public string AttachmentIds { get; set; }
	public string CCEmails { get; set; }
	public string EmailMessage { get; set; }
	public string EmailTitle { get; set; }
	public int Id { get; set; }
	public bool? SenderCC { get; set; }
	public string SenderUserEmail { get; set; }
	public int? SenderUserId { get; set; }
	public string SenderUserName { get; set; }
	public DateTime? SendingTime { get; set; }
	public string ToEmail { get; set; }

	public EmailHistoryEntity() { }

	public EmailHistoryEntity(DataRow dataRow)
	{
		AttachmentIds = (dataRow["AttachmentIds"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AttachmentIds"]);
		CCEmails = (dataRow["CCEmails"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CCEmails"]);
		EmailMessage = (dataRow["EmailMessage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailMessage"]);
		EmailTitle = (dataRow["EmailTitle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailTitle"]);
		Id = Convert.ToInt32(dataRow["Id"]);
		SenderCC = (dataRow["SenderCC"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SenderCC"]);
		SenderUserEmail = (dataRow["SenderUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SenderUserEmail"]);
		SenderUserId = (dataRow["SenderUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SenderUserId"]);
		SenderUserName = (dataRow["SenderUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SenderUserName"]);
		SendingTime = (dataRow["SendingTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SendingTime"]);
		ToEmail = (dataRow["ToEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ToEmail"]);
	}

}
public static class EmailHistoryEntityExtensions
{

	// const limts 
	private const int MAX_ATTACHMENT_IDS = 255;
	private const int MAX_CC_EMAILS = 750;
	private const int MAX_EMAIL_MESSAGE = 1073741823;
	private const int MAX_EMAIL_TITLE = 500;
	private const int MAX_SENDER_USER_EMAIL = 255;
	private const int MAX_SENDER_USER_NAME = 255;
	private const int MAX_TO_EMAIL = 1000;
	public static void TruncateToColumnLimits(this EmailHistoryEntity entity)
	{
		if(entity == null)
			return;

		entity.AttachmentIds = string.IsNullOrEmpty(entity.AttachmentIds)
			? entity.AttachmentIds
			: (entity.AttachmentIds.Length > MAX_ATTACHMENT_IDS
				? entity.AttachmentIds.Substring(0, MAX_ATTACHMENT_IDS)
				: entity.AttachmentIds);

		entity.CCEmails = string.IsNullOrEmpty(entity.CCEmails)
			? entity.AttachmentIds
			: (entity.CCEmails.Length > MAX_CC_EMAILS
				? entity.CCEmails.Substring(0, MAX_CC_EMAILS)
				: entity.CCEmails);

		entity.EmailMessage = string.IsNullOrEmpty(entity.EmailMessage)
			? entity.AttachmentIds
			: (entity.EmailMessage.Length > MAX_EMAIL_MESSAGE
				? entity.EmailMessage.Substring(0, MAX_EMAIL_MESSAGE)
				: entity.EmailMessage);

		entity.EmailTitle = string.IsNullOrEmpty(entity.EmailTitle)
			? entity.AttachmentIds
			: (entity.EmailTitle.Length > MAX_EMAIL_TITLE
				? entity.EmailTitle.Substring(0, MAX_EMAIL_TITLE)
				: entity.EmailTitle);

		entity.SenderUserEmail = string.IsNullOrEmpty(entity.SenderUserEmail)
			? entity.AttachmentIds
			: (entity.SenderUserEmail.Length > MAX_SENDER_USER_EMAIL
				? entity.SenderUserEmail.Substring(0, MAX_SENDER_USER_EMAIL)
				: entity.SenderUserEmail);

		entity.SenderUserName = string.IsNullOrEmpty(entity.SenderUserName)
			? entity.AttachmentIds
			: (entity.SenderUserName.Length > MAX_SENDER_USER_NAME
				? entity.SenderUserName.Substring(0, MAX_SENDER_USER_NAME)
				: entity.SenderUserName);

		entity.ToEmail = string.IsNullOrEmpty(entity.ToEmail)
			? entity.AttachmentIds
			: (entity.ToEmail.Length > MAX_TO_EMAIL
				? entity.ToEmail.Substring(0, MAX_TO_EMAIL)
				: entity.ToEmail);
	}
}


