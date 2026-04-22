using Infrastructure.Data.Entities.Tables._Commun;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Files.FileFactory
{
	public class FilesAndModulesNameRelation
	{
		public static   bool ValidateEntityToUpdateExists(int ModuleId, int Module)
		{
			switch(Module)
			{
				case 1:
					return Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(ModuleId) is null ? false : true;
				case 2:
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(ModuleId) is null ? false : true;
				default:
					throw new NotSupportedException("This Module is not yet added yet !");
			}
		}
		public static ModulesInAttachements GetModuleName(int Module)
		{
			switch(Module)
			{
				case 1:
					return ModulesInAttachements.Suppliers;
				case 2:
					return ModulesInAttachements.Rahmen;
				default:
					throw new NotSupportedException("This Module is not yet added yet !");
			}
		}
		public static List<Infrastructure.Data.Entities.Tables._Commun.__BSD_Attachements_Tracking_LogEntity> GenerateFilesTrackingLogEntity(List<Infrastructure.Data.Entities.Tables._Commun.__BSD_Attachements_TrackingEntity> items,string Operation,int Module,int ModuleId,int UserId)
		{
			var logsEntities = new List<Infrastructure.Data.Entities.Tables._Commun.__BSD_Attachements_Tracking_LogEntity>(items.Count);
			foreach(var item in items)
			{
				logsEntities.Add(new __BSD_Attachements_Tracking_LogEntity()
				{
					FileName = item.FileName,
					Operation = Operation,
					FileId = item.FileId,
					UpdateTime = DateTime.Now,
					Module = Module,
					ModuleId = ModuleId,
					UserId = UserId
				});
			}
			return logsEntities;
		}
	}
	public enum ModulesInAttachements
	{
		Suppliers,Rahmen
	}
}
