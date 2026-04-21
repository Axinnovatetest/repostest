using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CapitalRequests.Models
{
	public class AccessProfileModel
	{
		public string AccessProfileName { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public bool? ModuleActivated { get; set; }
		public bool? RequestAdmin { get; set; }
		public bool? RequestCapital { get; set; }
		public bool? RequestCreation { get; set; }
		public bool? RequestEngeneering { get; set; }
		public AccessProfileModel()
		{

		}
		public AccessProfileModel(Infrastructure.Data.Entities.Tables.CPL.AccessProfileEntity entity)
		{
			Id = entity.Id;
			AccessProfileName = entity.AccessProfileName;
			CreationTime = entity.CreationTime;
			CreationUserId = entity.CreationUserId;
			ModuleActivated = entity.ModuleActivated;
			RequestAdmin = entity.RequestAdmin;
			RequestCapital = entity.RequestCapital;
			RequestCreation = entity.RequestCreation;
			RequestEngeneering = entity.RequestEngeneering;
		}
		public Infrastructure.Data.Entities.Tables.CPL.AccessProfileEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CPL.AccessProfileEntity
			{
				Id = Id,
				AccessProfileName = AccessProfileName,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				ModuleActivated = ModuleActivated,
				RequestAdmin = RequestAdmin,
				RequestCapital = RequestCapital,
				RequestCreation = RequestCreation,
				RequestEngeneering = RequestEngeneering,
			};
		}
	}
}
