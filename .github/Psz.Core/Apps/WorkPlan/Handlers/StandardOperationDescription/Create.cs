using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class StandardOperationDescription
	{
		public static Core.Models.ResponseModel<int> Create(Psz.Core.Apps.WorkPlan.Models.StandardOperationDescription.CreateModel createModel, Core.Identity.Models.UserModel user)
		{
			try
			{
				var standardOperationDescription = new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity()
				{
					Description = createModel.Description,
					CreationTime = DateTime.Now, // createModel.CreationTime,
					CreationUserId = createModel.CreationUserId,
					StandardOperationId = createModel.StandardOperationId
				};

				var insertedId = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Insert(standardOperationDescription);
				if(insertedId > 0)
				{
					var languages = Infrastructure.Data.Access.Tables.STG.LanguageAccess.Get()
						?? new List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity>();

					// DE Name
					var langDE = languages?.Find(l => l.Code.ToUpper() == "DE");
					if(langDE != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
						{
							CodeLanguage = langDE?.Code,
							Description = "",
							IdStandardOperationDescription = insertedId,
							IdLanguage = (int)langDE?.Id,
							Name = createModel.NameDE
						});
					}

					// TN Name
					var langTN = languages?.Find(l => l.Code.ToUpper() == "TN");
					if(langTN != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
						{
							CodeLanguage = langTN?.Code,
							Description = "",
							IdStandardOperationDescription = insertedId,
							IdLanguage = (int)langTN?.Id,
							Name = createModel.NameTN
						});
					}

					// CZ Name
					var langCZ = languages?.Find(l => l.Code.ToUpper() == "CZ");
					if(langCZ != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
						{
							CodeLanguage = langCZ?.Code,
							Description = "",
							IdStandardOperationDescription = insertedId,
							IdLanguage = (int)langCZ?.Id,
							Name = createModel.NameCZ
						});
					}

					// AL Name
					var langAL = languages?.Find(l => l.Code.ToUpper() == "AL");
					if(langAL != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
						{
							CodeLanguage = langAL?.Code,
							Description = "",
							IdStandardOperationDescription = insertedId,
							IdLanguage = (int)langAL?.Id,
							Name = createModel.NameAL
						});
					}
				}

				return Core.Models.ResponseModel<int>.SuccessResponse(insertedId);
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
	}
}
