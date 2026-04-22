using Psz.Core.Helpers;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class StandardOperationDescription
	{
		public static Core.Models.ResponseModel<int> Edit(Psz.Core.Apps.WorkPlan.Models.StandardOperationDescription.CreateModel editModel, Core.Identity.Models.UserModel user)
		{
			try
			{
				var standardOperationDescription = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Get(editModel.Id);
				if(standardOperationDescription == null)
				{
					return Core.Models.ResponseModel<int>.FailureResponse("Standard operation description not found");
				}
				standardOperationDescription.Code = editModel.Code;
				standardOperationDescription.Description = editModel.Description;
				standardOperationDescription.Last_Edit_Date = DateTime.Now;
				standardOperationDescription.Last_Edit_User_Id = user.Id;
				standardOperationDescription.LotPiece = editModel.LotPiece;
				standardOperationDescription.MachineToolInsert = editModel.MachineToolInsert;
				standardOperationDescription.ManuelMachinel = editModel.ManuelMachinel;
				standardOperationDescription.Operation_Value_Adding = editModel.Operation_Value_Adding;
				standardOperationDescription.ReationSetup = editModel.ReationSetup;
				standardOperationDescription.RelationOperationSetup = editModel.RelationOperationSetup;
				standardOperationDescription.Remark = editModel.Remark;
				standardOperationDescription.Remark2 = editModel.Remark2;
				standardOperationDescription.SecondsPerSubOperation = editModel.SecondsPerSubOperation;
				standardOperationDescription.Setup = editModel.Setup;
				standardOperationDescription.TechnologieArea = editModel.TechnologieArea;
				standardOperationDescription.ValueAdding = editModel.ValueAdding;


				List<string> errors = new List<string>();
				// - same code
				if(Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.GetByCode(editModel.StandardOperationId, editModel.Code, editModel.Id) != null)
				{
					errors.Add($"Suboperation with code [{editModel.Code}] already exists");
				}
				// - same name
				if(Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.GetByDescription(editModel.StandardOperationId, editModel.Description, editModel.Id) != null)
				{
					errors.Add($"Suboperation [{editModel.Description}] already exists");
				}
				if(errors.Count > 0)
				{
					return Core.Models.ResponseModel<int>.FailureResponse(errors);
				}

				var nbUpdated = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Update(standardOperationDescription);
				if(nbUpdated > 0)
				{
					var languages = Psz.Core.Apps.WorkPlan.Handlers.Helpers.addMissingLanguages(user.Id) ?? new List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity>();
					addNotExisting(editModel, standardOperationDescription, languages, Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.GetByStandardOperation(standardOperationDescription.Id));

					// DE Name
					var langDE = languages?.Find(l => l.Code.ToUpper() == "DE");
					if(langDE != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.UpdateByStandardOperationDescription(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
						{
							Description = string.Empty,
							IdStandardOperationDescription = standardOperationDescription.Id,
							IdLanguage = (int)langDE?.Id,
							Name = editModel.NameDE
						});
					}

					// TN Name
					var langTN = languages?.Find(l => l.Code.ToUpper() == "TN");
					if(langTN != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.UpdateByStandardOperationDescription(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
						{
							Description = string.Empty,
							IdStandardOperationDescription = standardOperationDescription.Id,
							IdLanguage = (int)langTN?.Id,
							Name = editModel.NameTN
						});
					}

					// CZ Name
					var langCZ = languages?.Find(l => l.Code.ToUpper() == "CZ");
					if(langCZ != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.UpdateByStandardOperationDescription(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
						{
							Description = string.Empty,
							IdStandardOperationDescription = standardOperationDescription.Id,
							IdLanguage = (int)langCZ?.Id,
							Name = editModel.NameCZ
						});
					}

					// AL Name
					var langAL = languages?.Find(l => l.Code.ToUpper() == "AL");
					if(langAL != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.UpdateByStandardOperationDescription(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
						{
							Description = string.Empty,
							IdStandardOperationDescription = standardOperationDescription.Id,
							IdLanguage = (int)langAL?.Id,
							Name = editModel.NameAL
						});
					}
				}

				return Core.Models.ResponseModel<int>.SuccessResponse(nbUpdated);
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		private static void addNotExisting(Models.StandardOperationDescription.CreateModel editModel,
			Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity standardOperationDb,
			List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> languages,
			List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity> i18n)
		{
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "DE") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "DE");
				Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
					{
						CodeLanguage = "DE",
						Description = string.Empty,
						IdStandardOperationDescription = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameDE
					});
			}

			// TN
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "TN") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "TN");
				Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
					{
						CodeLanguage = "TN",
						Description = string.Empty,
						IdStandardOperationDescription = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameTN
					});
			}

			// CZ
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "CZ") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "CZ");
				Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
					{
						CodeLanguage = "CZ",
						Description = string.Empty,
						IdStandardOperationDescription = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameCZ
					});
			}

			// AL
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "AL") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "AL");
				Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity
					{
						CodeLanguage = "AL",
						Description = string.Empty,
						IdStandardOperationDescription = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameAL
					});
			}
		}
	}
}
