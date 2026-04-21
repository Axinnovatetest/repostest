using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Helpers
{
	public class Notifications
	{
		public static class Email
		{
			public static void SendEmail(string title, string content, List<string> toAddresses, List<KeyValuePair<string, System.IO.Stream>> attachments,
				Identity.Models.UserModel userEntity, List<int> attachmentIds)
			{
				try
				{
					// Send email notification
					Module.EmailingService.SendEmailAsync(title, content, toAddresses, attachments,
						saveHistory: true, senderEmail: userEntity.Email, senderName: userEntity.Username, senderId: userEntity.Id, senderCC: false, attachmentIds: attachmentIds);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", toAddresses)}]"));
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
			}
			public static void SendProjectCreationNotification(int projectId, Identity.Models.UserModel creationUser, bool isNew = true)
			{
				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(projectId);
				var files = Infrastructure.Data.Access.Tables.FNC.ProjectFileAccess.GetByProjectId(projectId);

				#region //- Handle attachments & Notifications
				{
					var attachments = new List<KeyValuePair<string, System.IO.Stream>>();
					var attachmentIds = new List<int>();
					try
					{
						if(files != null && files.Count > 0)
						{
							var n = 1;
							foreach(var fileItem in files)
							{
								if(fileItem.FileId > 0)
								{
									var data = Psz.Core.Common.Program.FilesManager.GetFile(fileItem.FileId);
									if(data != null)
									{
										attachments.Add(new KeyValuePair<string, System.IO.Stream>($"Attachment{n++}{data.FileExtension}", new System.IO.MemoryStream(data.FileBytes)));
										attachmentIds.Add(fileItem.FileId);
									}
								}
							}
						}
					} catch(Exception e)
					{
						Infrastructure.Services.Logging.Logger.Log(e);
					}

					// - Notifs
					var nextValidatorEmail = new List<string> { };
					try
					{
						var isNewProject = isNew ? "created a new" : "updated the";
						nextValidatorEmail = Infrastructure.Data.Access.Tables.COR.UserAccess.GetGlobalDirectors()?.Where(x => x.Id > 0)?.Select(x => x.Email)?.ToList();
						var emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
					+ $"<br/><span style='font-size:1.15em;'><strong>{creationUser.Name}</strong> has just {isNewProject} external project.</span>";

						emailContent += $"<br/><br/><table>"
							+ $"<tr><td colspan='2' style='font-size:1.15em;border-bottom:1px solid black;min-width:300px;background-color:#ebf2ff;'><strong>Project details &nbsp;</strong></td></tr>"
							+ $"<tr><td style='background-color:#f7f9fc;'><strong>Nr.: &nbsp;</strong></td><td style='background-color:#f7f9fc;'>{projectEntity.Id}</td></tr>"
							+ $"<tr><td><strong>Name: &nbsp;</strong></td><td>{projectEntity.ProjectName}</td></tr>"
							+ $"<tr><td style='background-color:#f7f9fc;'><strong>Customer Nr: &nbsp;</strong></td><td style='background-color:#f7f9fc;'>{projectEntity.CustomerNr}</td></tr>"
							+ $"<tr><td><strong>Customer Name: &nbsp;</strong></td><td>{projectEntity.CustomerName}</td></tr>"
							+ $"<tr><td style='background-color:#f7f9fc;'><strong>PSZ Offer: &nbsp;</strong></td><td style='background-color:#f7f9fc;'>{projectEntity.PSZOffer?.ToString("0.##")}</td></tr>"
							+ $"<tr><td style='background-color:#f7f9fc;'><strong>Internal Budget: &nbsp;</strong></td><td style='background-color:#f7f9fc;'>{projectEntity.ProjectBudget.ToString("0.##")}</td></tr>"
							+ $"<tr><td style='vertical-align: top;border-bottom:1px solid black;'><strong>Description: &nbsp;</strong></td><td style='border-bottom:1px solid black;max-width:1000px;'>{projectEntity.Description}</td></tr>"
							+ $"</table>";

						emailContent += $"<br/><br/>Please, login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/projects/{projectId}'>here</a>"
				   + "<br/><br/>Regards, <br/>IT Department</div>";

						// Send email notification
						Module.EmailingService.SendEmailAsync("[Budget] New External Project", emailContent, nextValidatorEmail, attachments,
						saveHistory: true, senderEmail: creationUser.Email, senderName: creationUser.Username, senderId: creationUser.Id, senderCC: false, attachmentIds: attachmentIds);
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", nextValidatorEmail)}]"));
						Infrastructure.Services.Logging.Logger.Log(ex);
					}
				}
				#endregion
			}

			public static string ExternalProjectTemplate_Create(
				Psz.Core.Identity.Models.UserModel currentUser,
				Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
				bool isCreate = true)
			{
				return "";
			}

			public static string ExternalProjectTemplate_Profit(
				Psz.Core.Identity.Models.UserModel currentUser,
				Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
				List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> extensionEntities,
				List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> extensionArticleEntities,
				decimal orderAmount,
				bool isCreate = true)
			{
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

				content += $"<br/><span style=''>Your project Nr.: <strong>{projectEntity.Id}</strong>, Name: <strong>{projectEntity.ProjectName?.ToUpper()}</strong> goes into </span><span style='color: red;'>NEGATIVE PROFIT</span>.<br/><br/>";

				content += $"Project Details<br/>";
				content += $"<table>";
				content += $"<tr><td> <strong> PSZ Offer</strong>: </td><td></strong>{(projectEntity.PSZOffer ?? 0).ToString("0.##")} {projectEntity.CurrencyName}</td></tr>";
				content += $"<tr><td> <strong> Initial budget</strong>: </td><td><span style='color: green;'><strong>{projectEntity.ProjectBudget.ToString("0.##")} {projectEntity.CurrencyName}</strong></span></td></tr>";
				content += $"<tr><td> <strong> Orders amount</strong>: </td><td></strong> <span style='color: red;'><strong>{extensionArticleEntities?.Select(x => x.TotalCostDefaultCurrency ?? 0)?.Sum().ToString("0.##")} {projectEntity.CurrencyName}</strong></span></td></tr>";
				content += $"<tr><td> <strong> Orders count</strong>: </td><td><strong>{extensionEntities?.Count}</strong></td></tr>";
				content += $"</table><br/><br/>";

				content += $"Last order attempt by <strong>{currentUser.Name}</strong> on <strong>{DateTime.Now.ToString("dd.MM.yyyy HH:mm")}</strong> with an amount of <strong>{orderAmount.ToString("0.##")} {projectEntity.CurrencyName}</strong>.";


				content += $"</span><br/><br/>Please, login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/projects/{projectEntity.Id}'>here</a>";
				content += "<br/><br/>Regards, <br/>IT Department </div>";
				return content;
			}

			public static string ExternalProjectTemplate_StatusChange(
				Psz.Core.Identity.Models.UserModel currentUser,
				Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
				int newStatus)
			{
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

				content += $"<br/><span style=''><strong>{currentUser.Username}</strong> has just changed the status of project <strong>{projectEntity.ProjectName?.ToUpper()}</strong> to <strong>{((Enums.BudgetEnums.ProjectStatuses)newStatus).GetDescription()}</strong>.<br/><br/>";

				content += $"</span><br/><br/>Please, login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/projects/{projectEntity.Id}'>here</a>";
				content += "<br/><br/>Regards, <br/>IT Department </div>";

				return content;
			}
			public static string ExternalProjectTemplate_StatusChange(
				Psz.Core.Identity.Models.UserModel currentUser,
				Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
				string newStatus)
			{
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

				content += $"<br/><span style=''><strong>{currentUser.Username}</strong> has just changed the status of project <strong>{projectEntity.ProjectName?.ToUpper()}</strong> to <strong>{newStatus}</strong>.<br/><br/>";

				content += $"</span><br/><br/>Please, login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/projects/{projectEntity.Id}'>here</a>";
				content += "<br/><br/>Regards, <br/>IT Department </div>";

				return content;
			}

			public static string ExternalProjectTemplate_Approval(
				Psz.Core.Identity.Models.UserModel currentUser,
				Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
				int newStatus, string notes = "")
			{
				var statusVerb = "";
				switch((Enums.BudgetEnums.ProjectApprovalStatuses)newStatus)
				{
					case Enums.BudgetEnums.ProjectApprovalStatuses.Inactive:
						statusVerb = "withdrawn approval from";
						break;
					case Enums.BudgetEnums.ProjectApprovalStatuses.Active:
						statusVerb = "approved";
						break;
					case Enums.BudgetEnums.ProjectApprovalStatuses.Reject:
						statusVerb = "rejected";
						break;
					default:
						break;
				}

				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

				content += $"<br/><span style=''><strong>{currentUser.Username}</strong> has just {statusVerb} your project <strong>{projectEntity.ProjectName?.ToUpper()}</strong>.";
				if(!string.IsNullOrWhiteSpace(notes))
					content += $"<br/><br/><strong>Notes from {currentUser.Name?.ToUpper()}:</strong> {notes}<br/><br/>";

				content += $"</span><br/><br/>Please, login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/projects/{projectEntity.Id}'>here</a>";
				content += "<br/><br/>Regards, <br/>IT Department </div>";

				return content;
			}

			public static string ExternalProjectTemplate_Budget(
				Psz.Core.Identity.Models.UserModel currentUser,
				Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
				List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> extensionEntities,
				List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> extensionArticleEntities)
			{
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

				content += $"<br/><span style=''><strong>{currentUser.Username}</strong> has just updated your project <strong>{projectEntity.ProjectName?.ToUpper()}</strong>, Nr.: <strong>{projectEntity?.Id}</strong>.<br/><br/>";

				content += $"<table>";
				content += $"<tr><td colspan='2' style='font-size:1.15em;border-bottom:1px solid black;min-width:300px;'><strong>Project details &nbsp;</strong></td></tr>";
				content += $"<tr><td> PSZ Offer: </td><td><strong>{(projectEntity.PSZOffer ?? 0).ToString("0.##")} {projectEntity.CurrencyName} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</strong></td></tr>";
				content += $"<tr><td> Internal budget: </td><td><span><strong>{projectEntity.ProjectBudget.ToString("0.##")} {projectEntity.CurrencyName}</strong></span></td></tr>";
				content += $"<tr><td> Orders amount: </td><td></strong> <span><strong>{extensionArticleEntities?.Select(x => x.TotalCostDefaultCurrency ?? 0)?.Sum().ToString("0.##")} {projectEntity.CurrencyName}</strong></span></td></tr>";
				content += $"<tr><td> Orders count: </td><td><strong>{extensionEntities?.Count}</strong></td></tr>";
				content += $"</table><br/>";

				content += $"</span><br/><br/>Please, login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/projects/{projectEntity.Id}'>here</a>";
				content += "<br/><br/>Regards, <br/>IT Department </div>";

				return content;
			}

			public static string PlaceOrder()
			{
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

				//content += $"<br/><span style=''><strong>{currentUser.Username}</strong> has just updated your project <strong>{projectEntity.ProjectName?.ToUpper()}</strong>, Nr.: <strong>{projectEntity?.Id}</strong>.<br/><br/>";

				//content += $"<table>";
				//content += $"<tr><td colspan='2' style='font-size:1.15em;border-bottom:1px solid black;min-width:300px;'><strong>Project details &nbsp;</strong></td></tr>";
				//content += $"<tr><td> PSZ Offer: </td><td><strong>{(projectEntity.PSZOffer ?? 0).ToString("0.##")} {projectEntity.CurrencyName} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</strong></td></tr>";
				//content += $"<tr><td> Internal budget: </td><td><span><strong>{projectEntity.ProjectBudget.ToString("0.##")} {projectEntity.CurrencyName}</strong></span></td></tr>";
				//content += $"<tr><td> Orders amount: </td><td></strong> <span><strong>{extensionArticleEntities?.Select(x => x.TotalCostDefaultCurrency ?? 0)?.Sum().ToString("0.##")} {projectEntity.CurrencyName}</strong></span></td></tr>";
				//content += $"<tr><td> Orders count: </td><td><strong>{extensionEntities?.Count}</strong></td></tr>";
				//content += $"</table><br/>";

				//content += $"</span><br/><br/>Please, login to check it <a href='{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/projects/{projectEntity.Id}'>here</a>";
				content += "<br/><br/>Regards, <br/>IT Department </div>";

				return content;
			}
		}
	}
}
