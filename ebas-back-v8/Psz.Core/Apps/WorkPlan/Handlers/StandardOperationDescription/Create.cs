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
				List<string> errors = new List<string>();
				var standardOperationDescription = new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity()
				{
					Code=createModel.Code,
					Description = createModel.Description,
					Creation_Date = DateTime.Now, // createModel.CreationTime,
					Creation_User_Id = createModel.CreationUserId,
					StdOperationId = createModel.StandardOperationId,
					LotPiece = createModel.LotPiece,
					MachineToolInsert = createModel.MachineToolInsert,
					ManuelMachinel = createModel.ManuelMachinel,
					Operation_Value_Adding = createModel.Operation_Value_Adding,
					ReationSetup = createModel.ReationSetup,
					RelationOperationSetup = createModel.RelationOperationSetup,
					Remark = createModel.Remark,
					Remark2 = createModel.Remark2,
					SecondsPerSubOperation = createModel.SecondsPerSubOperation,
					Setup = createModel.Setup,
					TechnologieArea = createModel.TechnologieArea,
					ValueAdding = createModel.ValueAdding
				};

				// - same code
				if(Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.GetByCode(createModel.StandardOperationId, createModel.Code)!=null)
				{
					errors.Add($"Suboperation with code [{createModel.Code}] already exists");
				}
				// - same name
				if(Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.GetByDescription(createModel.StandardOperationId, createModel.Description) != null)
				{
					errors.Add($"Suboperation [{createModel.Description}] already exists");
				}

				if(errors.Count>0)
				{
					return Core.Models.ResponseModel<int>.FailureResponse(errors);
				}

				var insertedId = Infrastructure.Data.Access.Tables. WPL.StandardOperationDescriptionAccess.Insert(standardOperationDescription);
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
