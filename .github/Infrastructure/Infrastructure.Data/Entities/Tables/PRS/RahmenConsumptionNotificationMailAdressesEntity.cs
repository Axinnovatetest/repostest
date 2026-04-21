using System;
using System.ComponentModel;
using System.Data;
using System.ComponentModel.DataAnnotations;
namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class RahmenConsumptionNotificationMailAdressesEntity
	{
		public int Id { get; set; }
		public string Mail { get; set; }
		public DateTime? AddedDate { get; set; }
		public int? AddredUserId { get; set; }
		public string AddedUsername { get; set; }
		public RahmenConsumptionNotificationMailAdressesEntity() { }
		public RahmenConsumptionNotificationMailAdressesEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			Mail = (dataRow["Mail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mail"]);
			AddedDate = (dataRow["AddedDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AddedDate"]);
			AddredUserId = (dataRow["AddredUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AddredUserId"]);
			AddedUsername = (dataRow["AddedUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AddedUsername"]);
		}
		public RahmenConsumptionNotificationMailAdressesEntity ShallowClone()
		{
			return new RahmenConsumptionNotificationMailAdressesEntity
			{
				Id = Id,
				Mail = Mail,
				AddedDate = AddedDate,
				AddredUserId = AddredUserId,
				AddedUsername = AddedUsername,
			};
		}
	}
}
