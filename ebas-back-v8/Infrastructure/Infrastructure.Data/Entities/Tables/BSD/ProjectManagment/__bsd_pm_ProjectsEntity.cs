using System;
using System.ComponentModel;
using System.Data;
using System.ComponentModel.DataAnnotations;
namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class __bsd_pm_ProjectsEntity
	{
		public int Id { get; set; }
		public string ProjectName { get; set; }
		public int? CustomerNumber { get; set; }
		public string CustomerName { get; set; }
		public string Status { get; set; }
		public int? StatusId { get; set; }
		public int? PMManagerUserId { get; set; }
		public string PMManagerUsername { get; set; }
		public int? PMManagerFactoryUserId { get; set; }
		public string PMManagerFactoryUsername { get; set; }
		public int? CSManagerUserId { get; set; }
		public string CSManagerUsername { get; set; }
		public string OfferNumber { get; set; }
		public decimal? QuantityKS { get; set; }
		public int? Factory { get; set; }
		public string Type { get; set; }
		public int? TypeId { get; set; }
		public int? CreationUserId { get; set; }
		public DateTime? CreationTime { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public string CustomerRefrence { get; set; }
		public __bsd_pm_ProjectsEntity() { }
		public __bsd_pm_ProjectsEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			ProjectName = (dataRow["ProjectName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectName"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
			StatusId = (dataRow["StatusId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StatusId"]);
			PMManagerUserId = (dataRow["PMManagerUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PMManagerUserId"]);
			PMManagerUsername = (dataRow["PMManagerUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PMManagerUsername"]);
			PMManagerFactoryUserId = (dataRow["PMManagerFactoryUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PMManagerFactoryUserId"]);
			PMManagerFactoryUsername = (dataRow["PMManagerFactoryUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PMManagerFactoryUsername"]);
			CSManagerUserId = (dataRow["CSManagerUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CSManagerUserId"]);
			CSManagerUsername = (dataRow["CSManagerUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CSManagerUsername"]);
			OfferNumber = (dataRow["OfferNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OfferNumber"]);
			QuantityKS = (dataRow["QuantityKS"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["QuantityKS"]);
			Factory = (dataRow["Factory"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Factory"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
			TypeId = (dataRow["TypeId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TypeId"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			DeliveryDate = (dataRow["DeliveryDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DeliveryDate"]);
			CustomerRefrence = (dataRow["CustomerRefrence"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerRefrence"]);
		}
		public __bsd_pm_ProjectsEntity ShallowClone()
		{
			return new __bsd_pm_ProjectsEntity
			{
				Id = Id,
				ProjectName = ProjectName,
				CustomerNumber = CustomerNumber,
				CustomerName = CustomerName,
				Status = Status,
				StatusId = StatusId,
				PMManagerUserId = PMManagerUserId,
				PMManagerUsername = PMManagerUsername,
				PMManagerFactoryUserId = PMManagerFactoryUserId,
				PMManagerFactoryUsername = PMManagerFactoryUsername,
				CSManagerUserId = CSManagerUserId,
				CSManagerUsername = CSManagerUsername,
				OfferNumber = OfferNumber,
				QuantityKS = QuantityKS,
				Factory = Factory,
				Type = Type,
				TypeId = TypeId,
				CreationUserId = CreationUserId,
				CreationTime = CreationTime,
				DeliveryDate = DeliveryDate,
				CustomerRefrence = CustomerRefrence,
			};
		}
	}
}