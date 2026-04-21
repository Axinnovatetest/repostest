using Infrastructure.Services.Utils;
using iText.StyledXmlParser.Jsoup.Nodes;
using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<string> AddProject(UserModel user, ProjectAddRequestModel data)
		{
			if(user == null)
				return ResponseModel<string>.AccessDeniedResponse();

			var transaction = new TransactionsManager();
			try
			{
				var pm_managerUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.Header.PMManagerId ?? -1);
				var cs_managerUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.Header.CSManagerId ?? -1);
				var pm_managerFactoryUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.Header.PMManagerFactoryId ?? -1);
				var customerAdress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(data.Header.CustomerNumber ?? -1);
				var cablesEntities = new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
				transaction.beginTransaction();
				var headerEntity = new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity
				{
					CreationTime = System.DateTime.Now,
					CreationUserId = user.Id,
					CustomerNumber = data.Header.CustomerNumber,
					CustomerName = customerAdress.Name1,
					PMManagerUserId = data.Header.PMManagerId,
					PMManagerUsername = pm_managerUser.Name,
					PMManagerFactoryUserId = pm_managerFactoryUser.Id,
					PMManagerFactoryUsername = pm_managerFactoryUser.Name,
					CSManagerUserId = cs_managerUser.Id,
					CSManagerUsername = cs_managerUser.Name,
					Factory = data.Header.Factory,
					OfferNumber = data.Header.OfferNumber,
					ProjectName = data.Header.Name,
					QuantityKS = data.Header.QuantityKS,
					Status = Enums.ProjectManagmentEnums.ProjectStatuses.Offer.GetDescription(),
					StatusId = (int)Enums.ProjectManagmentEnums.ProjectStatuses.Offer,
					TypeId = data.Header.Type,
					Type = ((Enums.ProjectManagmentEnums.ProjectTypes)data.Header.Type).GetDescription(),
					DeliveryDate = data.Header.DeliveryDate,
					CustomerRefrence = data.Header.CustomerRefrence
				};
				var projectId = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.InsertWithTransaction(headerEntity, transaction.connection, transaction.transaction);
				var logs = new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity>();
				var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNrs(data.Cables?.Select(x => x.ArticleId ?? -1).ToList());
				var responsibles = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.Cables?.Select(x => x.ResponsibleId ?? -1).ToList());
				data.Cables?.ForEach(x =>
				{
					var article = articles.FirstOrDefault(a => a.ArtikelNr == x.ArticleId) ?? null;
					var responsible = responsibles.FirstOrDefault(r => r.Id == x.ResponsibleId) ?? null;
					if(article != null)
					{
						cablesEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity
						{
							ArticleId = x.ArticleId,
							ArticleNumber = article.ArtikelNummer,
							ArticleCustomerNumber = article.CustomerItemNumber,
							CreationUserId = user.Id,
							CreationUsername = user.Name,
							ProjectId = projectId,
							ResponsibleUserId = x.ResponsibleId,
							ResponsibleUsername = responsible.Name,
							Status = Enums.ProjectManagmentEnums.TaskStatus.NotStarted.GetDescription(),
							StatusId = (int)Enums.ProjectManagmentEnums.TaskStatus.NotStarted
						});
					}
					logs.Add(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
					{
						LogText = $"Cable [{article.ArtikelNummer}] added",
						ProjectId = projectId,
						LogTime = DateTime.Now,
						UserId = user.Id,
						Username = user.Name
					});
				});
				var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CablesAccess.InsertWithTransaction(cablesEntities, transaction.connection, transaction.transaction);
				logs.Add(
					new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
					{
						LogText = $"Project {headerEntity.ProjectName} created",
						ProjectId = projectId,
						LogTime = DateTime.Now,
						UserId = user.Id,
						Username = user.Name
					});

				Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.InsertWithTransaction(logs, transaction.connection, transaction.transaction);
				if(transaction.commit())
				{
					return ResponseModel<string>.SuccessResponse(projectId.ToString("D3"));
				}
				else
					return ResponseModel<string>.FailureResponse("Transaction Error.");
			} catch(System.Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> EditProject(UserModel user, ProjectHeader data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var transaction = new TransactionsManager();
			try
			{
				transaction.beginTransaction();
				var pm_managerUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.PMManagerId ?? -1);
				var cs_managerUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.CSManagerId ?? -1);
				var pm_managerFactoryUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.PMManagerFactoryId ?? -1);
				var customerAdress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(data.CustomerNumber ?? -1);
				var entity = new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity
				{
					Id = data.Id,
					CreationTime = System.DateTime.Now,
					CreationUserId = user.Id,
					CustomerNumber = data.CustomerNumber,
					CustomerName = customerAdress.Name1,
					PMManagerUserId = data.PMManagerId,
					PMManagerUsername = pm_managerUser.Name,
					PMManagerFactoryUserId = pm_managerFactoryUser.Id,
					PMManagerFactoryUsername = pm_managerFactoryUser.Name,
					CSManagerUserId = cs_managerUser.Id,
					CSManagerUsername = cs_managerUser.Name,
					Factory = data.Factory,
					OfferNumber = data.OfferNumber,
					ProjectName = data.Name,
					QuantityKS = data.QuantityKS,
					Status = ((Enums.ProjectManagmentEnums.ProjectStatuses)data.StatusId).GetDescription(),
					StatusId = data.StatusId,
					TypeId = data.Type,
					Type = ((Enums.ProjectManagmentEnums.ProjectTypes)data.Type).GetDescription(),
					DeliveryDate = data.DeliveryDate,
					CustomerRefrence = data.CustomerRefrence
				};
				var logs = GetProjectLogs(entity, user);
				var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.UpdateWithTransaction(entity, transaction.connection, transaction.transaction);
				Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.InsertWithTransaction(logs, transaction.connection, transaction.transaction);
				// -
				if(transaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(response);
				}
				else
					return ResponseModel<int>.FailureResponse("Transaction Error.");
			} catch(System.Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> DeleteProject(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var cables = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CablesAccess.GetByByProjectAndStatus(id, null);
			if(cables != null && cables.Count > 0)
				return ResponseModel<int>.FailureResponse("Project have active cable(s), deletion not allowed.");
			
			var transaction = new TransactionsManager();
			try
			{
				transaction.beginTransaction();
				var project = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get(id);
				var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.DeleteWithTransaction(id, transaction.connection, transaction.transaction);
				Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
				{
					LogText = $"Project [{project.ProjectName}] deleted",
					LogTime = DateTime.Now,
					ProjectId = id,
					UserId = user.Id,
					Username = user.Name
				}, transaction.connection, transaction.transaction);
				// -
				if(transaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(response);
				}
				else
					return ResponseModel<int>.FailureResponse("Transaction Error.");
			} catch(System.Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<ProjectHeader> GetProjectById(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<ProjectHeader>.AccessDeniedResponse();

			try
			{
				var entity = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get(id);
				var response = new ProjectHeader
				{
					CSManagerId = entity.CSManagerUserId,
					CSManagerName = entity.CSManagerUsername,
					CustomerNumber = entity.CustomerNumber ?? -1,
					CustomerRefrence = entity.CustomerRefrence,
					DeliveryDate = entity.DeliveryDate,
					Factory = entity.Factory,
					Id = entity.Id,
					Name = entity.ProjectName,
					OfferNumber = entity.OfferNumber,
					PMManagerFactoryId = entity.PMManagerFactoryUserId,
					PMManagerFactoryName = entity.PMManagerFactoryUsername,
					PMManagerId = entity.PMManagerUserId,
					PmManagerName = entity.PMManagerUsername,
					QuantityKS = entity.QuantityKS,
					Type = entity.TypeId,
					Status = entity.Status,
					StatusId = entity.StatusId
				};

				return ResponseModel<ProjectHeader>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> DeleteCable(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();


			var transaction = new TransactionsManager();
			try
			{
				transaction.beginTransaction();
				var cable = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CablesAccess.Get(id);
				var project = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get(cable.ProjectId ?? -1);

				var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CablesAccess.DeleteWithTransaction(id, transaction.connection, transaction.transaction);
				Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
				{
					LogText = $"Cable [{cable.ArticleNumber}] deleted",
					LogTime = DateTime.Now,
					ProjectId = project.Id,
					UserId = user.Id,
					Username = user.Name
				}, transaction.connection, transaction.transaction);
				// -
				if(transaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(response);
				}
				else
					return ResponseModel<int>.FailureResponse("Transaction Error.");
			} catch(System.Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> AddProjectCable(UserModel user, ProjectCable data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			var transaction = new TransactionsManager();
			try
			{
				transaction.beginTransaction();
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data.ArticleId ?? -1);
				var responsible = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.ResponsibleId ?? -1);
				var project = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get(data.ProjectId ?? -1);
				var cable = new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity
				{
					ArticleCustomerNumber = article.CustomerItemNumber,
					ArticleId = data.ArticleId,
					ArticleNumber = article.ArtikelNummer,
					CreationUserId = user.Id,
					CreationUsername = user.Username,
					ProjectId = data.ProjectId,
					ResponsibleUserId = data.ResponsibleId,
					ResponsibleUsername = responsible.Name,
					Status = Enums.ProjectManagmentEnums.TaskStatus.NotStarted.GetDescription(),
					StatusId = (int)Enums.ProjectManagmentEnums.TaskStatus.NotStarted,
				};
				var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CablesAccess.InsertWithTransaction(cable, transaction.connection, transaction.transaction);
				Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
				{
					LogText = $"Cable [{cable.ArticleNumber}] added",
					LogTime = DateTime.Now,
					ProjectId = project.Id,
					UserId = user.Id,
					Username = user.Name
				}, transaction.connection, transaction.transaction);
				// -
				if(transaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(response);
				}
				else
					return ResponseModel<int>.FailureResponse("Transaction Error.");
			} catch(System.Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity> GetProjectLogs(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity _new, UserModel user)
		{
			var _old = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get(_new.Id);
			var logTexts = new List<string>();
			var logEntities = new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity>();

			if(_old.CustomerName != _new.CustomerName)
			{
				logTexts.Add($"Customer changed from {_old.CustomerName} to {_new.CustomerName}");
			}
			if(_old.CSManagerUsername != _new.CSManagerUsername)
			{
				logTexts.Add($"CS Manager changed from {_old.CSManagerUsername} to {_new.CSManagerUsername}");
			}
			if(_old.CustomerRefrence != _new.CustomerRefrence)
			{
				logTexts.Add($"Customer Refrence changed from {_old.CustomerRefrence} to {_new.CustomerRefrence}");
			}
			if(_old.DeliveryDate != _new.DeliveryDate)
			{
				logTexts.Add($"Delivery Date changed from {_old.DeliveryDate} to {_new.DeliveryDate}");
			}
			if(_old.Factory != _new.Factory)
			{
				logTexts.Add($"Factory changed from {_old.Factory} to {_new.Factory}");
			}
			if(_old.OfferNumber != _new.OfferNumber)
			{
				logTexts.Add($"Offer Number changed from {_old.OfferNumber} to {_new.OfferNumber}");
			}
			if(_old.PMManagerFactoryUsername != _new.PMManagerFactoryUsername)
			{
				logTexts.Add($"PM Manager Factory changed from {_old.PMManagerFactoryUsername} to {_new.PMManagerFactoryUsername}");
			}
			if(_old.PMManagerUsername != _new.PMManagerUsername)
			{
				logTexts.Add($"PM Manager changed from {_old.PMManagerUsername} to {_new.PMManagerUsername}");
			}
			if(_old.ProjectName != _new.ProjectName)
			{
				logTexts.Add($"Project Name changed from {_old.ProjectName} to {_new.ProjectName}");
			}
			if(_old.QuantityKS != _new.QuantityKS)
			{
				logTexts.Add($"Quantity KS changed from {_old.QuantityKS} to {_new.QuantityKS}");
			}
			if(_old.Type != _new.Type)
			{
				logTexts.Add($"Type changed from {_old.Type} to {_new.Type}");
			}
			if(_old.Status != _new.Status)
			{
				logTexts.Add($"Status changed from {_old.Status} to {_new.Status}");
			}
			if(logTexts != null && logTexts.Count > 0)
			{
				foreach(var item in logTexts)
				{
					logEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
					{
						LogText = item,
						ProjectId = _new.Id,
						LogTime = DateTime.Now,
						UserId = user.Id,
						Username = user.Name
					});
				}
			}
			return logEntities;
		}
	}
}