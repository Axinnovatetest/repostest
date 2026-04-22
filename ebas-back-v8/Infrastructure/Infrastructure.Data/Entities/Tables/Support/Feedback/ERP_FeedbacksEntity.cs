using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Support.Feedback
{
	public class ERP_FeedbacksEntity
	{
		public string Comment { get; set; }
		public DateTime? CreationDate { get; set; }
		public string FeedbackType { get; set; }
		public int Id { get; set; }
		public byte[] Image { get; set; }
		public string Module { get; set; }
		public string PageUrl { get; set; }
		public string priority { get; set; }
		public int? Rating { get; set; }
		public string Submodule { get; set; }
		public bool? Treated { get; set; } = false;
		public DateTime? TreatedDate { get; set; }
		public string TreatedUser { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }

		public ERP_FeedbacksEntity() { }

		public ERP_FeedbacksEntity(DataRow dataRow)
		{
			Comment = (dataRow["Comment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Comment"]);
			CreationDate = (dataRow["CreationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]);
			FeedbackType = (dataRow["FeedbackType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FeedbackType"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Image = (dataRow["Image"] == System.DBNull.Value) ? new byte[0] : (byte[])dataRow["Image"];
			Module = (dataRow["Module"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Module"]);
			PageUrl = (dataRow["PageUrl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PageUrl"]);
			priority = (dataRow["priority"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["priority"]);
			Rating = (dataRow["Rating"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Rating"]);
			Submodule = (dataRow["Submodule"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Submodule"]);
			Treated = (dataRow["Treated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Treated"]);
			TreatedDate = (dataRow["TreatedDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["TreatedDate"]);
			TreatedUser = (dataRow["TreatedUser"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TreatedUser"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
		}
	}
}

