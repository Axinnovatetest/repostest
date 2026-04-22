using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Support.Request
{
	public class Requirement_ProcessEntity
	{
		public string Description { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string TestUseCase { get; set; }
		public int RequestId { get; set; }

		public Requirement_ProcessEntity() { }

		public Requirement_ProcessEntity(DataRow dataRow)
		{
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Name = Convert.ToString(dataRow["Name"]);
			TestUseCase = Convert.ToString(dataRow["TestUseCase"]);
			RequestId = Convert.ToInt32(dataRow["RequestId"]);
		}

		public Requirement_ProcessEntity ShallowClone()
		{
			return new Requirement_ProcessEntity
			{
				Description = Description,
				Id = Id,
				Name = Name,
				TestUseCase = TestUseCase,
				RequestId = RequestId
			};
		}
	}
}
