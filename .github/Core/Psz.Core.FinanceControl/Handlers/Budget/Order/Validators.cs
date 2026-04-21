using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	public static class Validators
	{
		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> getByOrderId(int orderId, out List<string> errors, bool isArchived = false, bool isDeleted = false)
		{
			errors = new List<string>();
			List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> results = null;

			try
			{

				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(orderId, isArchived, isDeleted);

				if(orderEntity == null)
					return results;

				results = new List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity>();

				// Add user as validator
				var user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderEntity.IssuerId);
				if(user == null)
				{
					errors.Add($"User [{orderEntity.IssuerId}] not found");
					return results;
				}
				results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
				{
					ID = -1,
					email = user.Email,
					Id_Project = orderId,
					Id_Validator = user.Id,
					Level = (int)Enums.BudgetEnums.ValidationLevels.Draft,
				});

				// Internal project w/o project
				if(orderEntity.ProjectId < 1)
				{
					var departmentEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderEntity.DepartmentId ?? -1);
					var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderEntity.CompanyId ?? -1);
					var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);

					if(departmentEntity != null)
					{
						var u = Infrastructure.Data.Access.Tables.COR.UserAccess.Get((int)(departmentEntity?.HeadUserId ?? -1));
						if(u == null)
						{
							errors.Add($"User [{departmentEntity.HeadUserId}] not found");
							return results;
						}
						results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
						{
							ID = -1,
							email = u.Email,
							Id_Project = orderId,
							Id_Validator = u.Id,
							Level = (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector,
						});
					}
					else
					{
						errors.Add($"Head of department [{orderEntity.DepartmentId ?? -1}] not found");
						return results;
					}

					if(companyEntity != null)
					{
						var u = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(companyEntity.DirectorId ?? -1);
						if(u == null)
						{
							errors.Add($"User [{companyEntity.DirectorId}] not found");
							return results;
						}

						// - Site Director
						results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
						{
							ID = -1,
							email = companyEntity.DirectorEmail,
							Id_Project = orderId,
							Id_Validator = companyEntity.DirectorId ?? -1,
							Level = (int)Enums.BudgetEnums.ValidationLevels.SiteDirector,
						});

						// - Purchase
						results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
						{
							ID = -1,
							email = companyExtensionEntity.PurchaseGroupEmail,
							Id_Project = orderId,
							Id_Validator = companyExtensionEntity.PurchaseProfileId ?? -1,
							Level = (int)Enums.BudgetEnums.ValidationLevels.Purchase,
						});
					}
					else
					{
						errors.Add($"Site director [{orderEntity.CompanyId ?? -1}] not found");
						return results;
					}
				}
				else
				{
					var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderEntity.ProjectId ?? -1);
					if(projectEntity != null)
					{
						// Internal project
						if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.Internal)
						{
							var departmentEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderEntity.DepartmentId ?? -1);
							var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderEntity.CompanyId ?? -1);
							var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);

							if(departmentEntity != null)
							{
								results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
								{
									ID = -1,
									email = departmentEntity.HeadUserEmail,
									Id_Project = orderId,
									Id_Validator = (int)departmentEntity.HeadUserId,
									Level = (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector,
								});
							}

							if(companyEntity != null)
							{
								// - site director
								results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
								{
									ID = -1,
									email = companyEntity.DirectorEmail,
									Id_Project = orderId,
									Id_Validator = companyEntity.DirectorId.HasValue ? companyEntity.DirectorId.Value : -1,
									Level = (int)Enums.BudgetEnums.ValidationLevels.SiteDirector,
								});

								// - Purchase
								results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
								{
									ID = -1,
									email = companyExtensionEntity.PurchaseGroupEmail,
									Id_Project = orderId,
									Id_Validator = companyExtensionEntity.PurchaseProfileId ?? -1,
									Level = (int)Enums.BudgetEnums.ValidationLevels.Purchase,
								});
							}

						}
						else if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.External)// External project
						{
							results.AddRange(Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectId(orderEntity.ProjectId ?? -1));

							var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(projectEntity.CompanyId ?? -1);
							if(companyExtensionEntity == null)
							{
								errors.Add($"Purchase [{orderEntity.CompanyId ?? -1}] not found");
								return results;
							}
							else
							{
								// - Purchase
								results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
								{
									ID = -1,
									email = companyExtensionEntity.PurchaseGroupEmail,
									Id_Project = orderId,
									Id_Validator = companyExtensionEntity.PurchaseProfileId ?? -1,
									Level = results.Count,
								});
							}
						}
					}
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> getByOrderId(
			Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity,
			Infrastructure.Data.Entities.Tables.COR.UserEntity issuer,
			Infrastructure.Data.Entities.Tables.COR.UserEntity departmentHeadOf,
			Infrastructure.Data.Entities.Tables.COR.UserEntity siteDirector,
			Infrastructure.Data.Entities.Tables.STG.DepartmentEntity departmentEntity,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity,
			Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity companyExtensionEntity,
			Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
			Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity projectCompanyExtensionEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> projectValidators,
			out List<string> errors, bool isArchived = false, bool isDeleted = false)
		{
			errors = new List<string>();
			List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> results = null;

			try
			{
				if(orderEntity == null)
					return results;

				var orderId = orderEntity.OrderId;
				results = new List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity>();

				// Add user as validator
				//var user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderEntity.IssuerId) ?? new ;
				if(issuer == null)
				{
					errors.Add($"User [{orderEntity.IssuerId}] not found");
					return results;
				}
				results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
				{
					ID = -1,
					email = issuer.Email,
					Id_Project = orderId,
					Id_Validator = issuer.Id,
					Level = (int)Enums.BudgetEnums.ValidationLevels.Draft,
				});

				// Internal project w/o project
				if(orderEntity.ProjectId < 1)
				{
					// var departmentEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderEntity.DepartmentId ?? -1);
					// var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderEntity.CompanyId ?? -1);
					//var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);

					if(departmentEntity != null)
					{
						//var departmentHeadOf = Infrastructure.Data.Access.Tables.COR.UserAccess.Get((int) (departmentEntity?.HeadUserId ?? -1));
						if(departmentHeadOf == null)
						{
							errors.Add($"User [{departmentEntity.HeadUserId}] not found");
							return results;
						}
						results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
						{
							ID = -1,
							email = departmentHeadOf.Email,
							Id_Project = orderId,
							Id_Validator = departmentHeadOf.Id,
							Level = (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector,
						});
					}
					else
					{
						errors.Add($"Head of department [{orderEntity.DepartmentId ?? -1}] not found");
						return results;
					}

					if(companyEntity != null)
					{
						//var siteDirector = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(companyEntity.DirectorId ?? -1);
						if(siteDirector == null)
						{
							errors.Add($"User [{companyEntity.DirectorId}] not found");
							return results;
						}

						// - Site Director
						results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
						{
							ID = -1,
							email = companyEntity.DirectorEmail,
							Id_Project = orderId,
							Id_Validator = companyEntity.DirectorId ?? -1,
							Level = (int)Enums.BudgetEnums.ValidationLevels.SiteDirector,
						});

						// - Purchase
						results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
						{
							ID = -1,
							email = companyExtensionEntity.PurchaseGroupEmail,
							Id_Project = orderId,
							Id_Validator = companyExtensionEntity.PurchaseProfileId ?? -1,
							Level = (int)Enums.BudgetEnums.ValidationLevels.Purchase,
						});
					}
					else
					{
						errors.Add($"Site director [{orderEntity.CompanyId ?? -1}] not found");
						return results;
					}
				}
				else
				{
					//var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderEntity.ProjectId ?? -1);
					if(projectEntity != null)
					{
						// Internal project
						if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.Internal)
						{
							//var departmentEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderEntity.DepartmentId ?? -1);
							//var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderEntity.CompanyId ?? -1);
							//var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);

							if(departmentEntity != null)
							{
								results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
								{
									ID = -1,
									email = departmentEntity.HeadUserEmail,
									Id_Project = orderId,
									Id_Validator = (int)departmentEntity.HeadUserId,
									Level = (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector,
								});
							}

							if(companyEntity != null)
							{
								results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
								{
									ID = -1,
									email = companyEntity.DirectorEmail,
									Id_Project = orderId,
									Id_Validator = companyEntity.DirectorId.HasValue ? companyEntity.DirectorId.Value : -1,
									Level = (int)Enums.BudgetEnums.ValidationLevels.SiteDirector,
								});

								// - Purchase
								results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
								{
									ID = -1,
									email = companyExtensionEntity.PurchaseGroupEmail,
									Id_Project = orderId,
									Id_Validator = companyExtensionEntity.PurchaseProfileId ?? -1,
									Level = (int)Enums.BudgetEnums.ValidationLevels.Purchase,
								});
							}

						}
						else // External project
						{
							if(projectValidators != null && projectValidators.Count > 0)
								results.AddRange(projectValidators/*Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectId(orderEntity.ProjectId ?? -1)*/);

							//var projectCompanyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(projectEntity.CompanyId ?? -1);
							if(projectCompanyExtensionEntity == null)
							{
								errors.Add($"Purchase [{orderEntity.CompanyId ?? -1}] not found");
								return results;
							}
							else
							{
								// - Purchase
								results.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
								{
									ID = -1,
									email = projectCompanyExtensionEntity.PurchaseGroupEmail,
									Id_Project = orderId,
									Id_Validator = projectCompanyExtensionEntity.PurchaseProfileId ?? -1,
									Level = results.Count,
								});
							}
						}
					}
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return results;
		}
	}
}
