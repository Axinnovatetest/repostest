using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Models.StandardOperationDescription
{
	public class CreateModel
	{
		public int Id { get; set; }
		public int StandardOperationId { get; set; }
		public string Description { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public Boolean IsArchived { get; set; }

		public string NameDE { get; set; }
		public string NameTN { get; set; }
		public string NameCZ { get; set; }
		public string NameAL { get; set; }
		public string CreationUsername { get; set; }
		public string LastEditUsername { get; set; }
		public string Code { get; set; }
		public string LotPiece { get; set; }
		public string MachineToolInsert { get; set; }
		public string ManuelMachinel { get; set; }
		public bool Operation_Value_Adding { get; set; }
		public string ReationSetup { get; set; }
		public string RelationOperationSetup { get; set; }
		public string Remark { get; set; }
		public string Remark2 { get; set; }
		public string SecondsPerSubOperation { get; set; }
		public string Setup { get; set; }
		public int StdOperationId { get; set; }
		public string TechnologieArea { get; set; }
		public string ValueAdding { get; set; }
		public CreateModel()
		{

		}
	}
}
