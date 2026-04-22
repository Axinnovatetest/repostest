using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class StandardOperation
	{
		public static int Create(Psz.Core.Apps.WorkPlan.Models.StandardOperation.CreateModel createModel,
			Core.Identity.Models.UserModel user, out List<string> errors)
		{
			try
			{
				errors = new List<string>();
				var stdOpDb = new Infrastructure.Data.Entities.Tables.WPL.StandardOperationEntity()
				{
					Code= createModel.Code,
					Name = createModel.Name,
					OperationValueAdding = createModel.OperationValueAdding,
					RelationOperationTime = createModel.RelationOperationTime,
					CreationTime = DateTime.Now, //createModel.CreationTime,
					CreationUserId = createModel.CreationUserId,
				};

				// - sameCode 
				if(Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.GetByCode(createModel.Code) != null)
				{
					errors.Add($"Operation with code [{createModel.Code}] already exists.");
				}
				// - sameName
				if(Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.GetByName(createModel.Name) != null)
				{
					errors.Add($"Operation with name [{createModel.Name}] already exists.");
				}

				if(errors.Count>0)
				{
					return -1;
				}

				var insertedId = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Insert(stdOpDb);
				if(insertedId > 0)
				{
					var languages = Infrastructure.Data.Access.Tables.STG.LanguageAccess.Get()
						?? new List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity>();

					// DE Name
					var langDE = languages?.Find(l => l.Code.ToUpper() == "DE");
					if(langDE != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
						{
							CodeLanguage = langDE?.Code,
							Description = "",
							IdStandardOperation = insertedId,
							IdLanguage = (int)langDE?.Id,
							Name = createModel.NameDE
						});
					}

					// TN Name
					var langTN = languages?.Find(l => l.Code.ToUpper() == "TN");
					if(langTN != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
						{
							CodeLanguage = langTN?.Code,
							Description = "",
							IdStandardOperation = insertedId,
							IdLanguage = (int)langTN?.Id,
							Name = createModel.NameTN
						});
					}

					// CZ Name
					var langCZ = languages?.Find(l => l.Code.ToUpper() == "CZ");
					Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
					{
						CodeLanguage = langCZ?.Code,
						Description = "",
						IdStandardOperation = insertedId,
						IdLanguage = (int)langCZ?.Id,
						Name = createModel.NameCZ
					});

					// AL Name
					var langAL = languages?.Find(l => l.Code.ToUpper() == "AL");
					if(langAL != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
						{
							CodeLanguage = langAL?.Code,
							Description = "",
							IdStandardOperation = insertedId,
							IdLanguage = (int)langAL?.Id,
							Name = createModel.NameAL
						});
					}
				}
				Infrastructure.Services.Logging.Logger.Log(createModel.Id + "+" + createModel.NameAL + " - " + createModel.NameDE + " + " + insertedId);
				return insertedId;
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
	}
}
