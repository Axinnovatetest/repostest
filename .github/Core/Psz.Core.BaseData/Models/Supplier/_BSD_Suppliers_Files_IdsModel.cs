using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Supplier
{
	public class Suppliers_Files_Ids_Model
	{
		public int FileId { get; set; }
		public int ID { get; set; }
		public bool? isActive { get; set; }
		public int? SupplierId { get; set; }
		public DateTime? UploadedDate { get; set; }
		public int? UserId { get; set; }
		public Suppliers_Files_Ids_Model() { }
		public Suppliers_Files_Ids_Model(Infrastructure.Data.Entities.Tables.BSD._BSD_Suppliers_Files_IdsEntity data)
		{
			FileId = data.FileId;
			ID = data.ID;
			isActive = data.isActive;
			SupplierId = data.SupplierId;
			UploadedDate = data.UploadedDate;
			UserId = data.UserId;
		}
	}
	public class Suppliers_Files_Ids_Request_Model
	{
		public int FileId { get; set; }
		public int ID { get; set; }
		public bool? isActive { get; set; }
		public int? SupplierId { get; set; }
		public DateTime? UploadedDate { get; set; }
		public int? UserId { get; set; }
		public Suppliers_Files_Ids_Request_Model() { }
	}
}
