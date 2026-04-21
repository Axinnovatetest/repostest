using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CRP
{
	public class __crp_historie_fa_plannung_headerEntity
	{
		public DateTime? DateHistorie { get; set; }
		public DateTime? DateImport { get; set; }
		public int Id { get; set; }
		public string ImportTyeName { get; set; }
		public int? ImportTypeId { get; set; }
		public int? importUserId { get; set; }
		public string ImportUsername { get; set; }

		public __crp_historie_fa_plannung_headerEntity() { }

		public __crp_historie_fa_plannung_headerEntity(DataRow dataRow)
		{
			DateHistorie = (dataRow["DateHistorie"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DateHistorie"]);
			DateImport = (dataRow["DateImport"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DateImport"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ImportTyeName = (dataRow["ImportTyeName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ImportTyeName"]);
			ImportTypeId = (dataRow["ImportTypeId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ImportTypeId"]);
			importUserId = (dataRow["importUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["importUserId"]);
			ImportUsername = (dataRow["ImportUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ImportUsername"]);
		}

		public __crp_historie_fa_plannung_headerEntity ShallowClone()
		{
			return new __crp_historie_fa_plannung_headerEntity
			{
				DateHistorie = DateHistorie,
				DateImport = DateImport,
				Id = Id,
				ImportTyeName = ImportTyeName,
				ImportTypeId = ImportTypeId,
				importUserId = importUserId,
				ImportUsername = ImportUsername
			};
		}
	}
}

