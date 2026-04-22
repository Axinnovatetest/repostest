using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Identity.Models
{
	public class CapitalRequestsAccessModel
	{
		public string AccessProfileName { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public bool ModuleActivated { get; set; }
		public bool RequestAdmin { get; set; }
		public bool RequestCapital { get; set; }
		public bool RequestCreation { get; set; }
		public bool RequestEngeneering { get; set; }
		public CapitalRequestsAccessModel()
		{

		}
		public CapitalRequestsAccessModel(List<Infrastructure.Data.Entities.Tables.CPL.AccessProfileEntity> accessProfileEntities)
		{
			if(accessProfileEntities == null || accessProfileEntities.Count <= 0)
				return;
			ModuleActivated = false;
			RequestAdmin = false;
			RequestCapital = false;
			RequestCreation = false;
			RequestEngeneering = false;
			foreach(var accessItem in accessProfileEntities)
			{
				ModuleActivated = ModuleActivated || (accessItem?.ModuleActivated ?? false);
				RequestAdmin = RequestAdmin || (accessItem?.RequestAdmin ?? false);
				RequestCapital = RequestCapital || (accessItem?.RequestCapital ?? false);
				RequestCreation = RequestCreation || (accessItem?.RequestCreation ?? false);
				RequestEngeneering = RequestEngeneering || (accessItem?.RequestEngeneering ?? false);
			}
		}
		public CapitalRequestsAccessModel(Infrastructure.Data.Entities.Tables.CPL.AccessProfileEntity entity)
		{
			Id = entity.Id;
			ModuleActivated = entity.ModuleActivated ?? false;
			RequestAdmin = entity.RequestAdmin ?? false;
			RequestCapital = entity.RequestCapital ?? false;
			RequestCreation = entity.RequestCreation ?? false;
			RequestEngeneering = entity.RequestEngeneering ?? false;
		}
		public CapitalRequestsAccessModel(CapitalRequestsAccessModel entity)
		{
			Id = entity?.Id ?? -1;
			ModuleActivated = entity?.ModuleActivated ?? false;
			RequestAdmin = entity?.RequestAdmin ?? false;
			RequestCapital = entity?.RequestCapital ?? false;
			RequestCreation = entity?.RequestCreation ?? false;
			RequestEngeneering = entity?.RequestEngeneering ?? false;
		}

		public Infrastructure.Data.Entities.Tables.CPL.AccessProfileEntity ToDbEntity(int id)
		{
			return new Infrastructure.Data.Entities.Tables.CPL.AccessProfileEntity
			{
				Id = id,
				AccessProfileName = AccessProfileName,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				ModuleActivated = ModuleActivated,
				RequestAdmin = RequestAdmin,
				RequestCapital = RequestCapital,
				RequestCreation = RequestCreation,
				RequestEngeneering = RequestEngeneering
			};
		}
	}
}