using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Identity.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Psz.Api.Areas.Settings.Controllers
{
	[Authorize]
	[Area("Settings")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ToolsController: ControllerBase
	{
		private UserModel _user;

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		public IActionResult GetTime()
		{
			try
			{
				return Ok(DateTime.Now);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return BadRequest();
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		public IActionResult GetVersion()
		{
			try
			{
				return Ok(Core.Program.Version);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return BadRequest();
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		public async Task<IActionResult> SendEmail([FromForm] Models.Tools.EmailNotificationModel data)
		{
			try
			{
				if(string.IsNullOrWhiteSpace(data?.Title?.Trim()))
					return Ok("Email title cannot be empty");

				if(string.IsNullOrWhiteSpace(data?.Message?.Trim()))
					return Ok("Email message cannot be empty");

				if(string.IsNullOrWhiteSpace(data?.ToEmail?.Trim()))
					return Ok("Email receiver cannot be empty");

				var currentUser = this.GetCurrentUser();
				if(currentUser == null)
					return Ok("User not connected");

				var fileIds = "";
				var files = data?.Files == null || data?.Files.Count <= 0
				? null
				: data?.Files.Select(x => new Core.FinanceControl.Models.Budget.Files.FilesModel
				{
					actionFile = x.FileName, // I know it sucks, but I dont have time to change the DB columns name
					fileDate = DateTime.Now,
					DocumentData = Models.Tools.EmailNotificationModel.getBytes(x),
					DocumentExtension = System.IO.Path.GetExtension(x.FileName) //AttachmentFileExtension,
				})?.ToList();

				if(files != null && files.Count > 0)
				{
					foreach(var fileItem in files)
					{
						fileIds = $"{fileIds}|{Core.Common.Helpers.ImageFileHelper.updateImage(-1, fileItem.DocumentData, fileItem.DocumentExtension)}";
					}
				}

				//
				var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data.OrderId ?? -1);
				Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity currentData = new Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity();
				var __attachments = new List<KeyValuePair<string, Stream>>();
				if(files!=null && files.Count> 0)
				 __attachments.AddRange( files?.Select(x => new KeyValuePair<string, System.IO.Stream>(x.actionFile, new System.IO.MemoryStream(x.DocumentData)))?.ToList());
				if(data?.FaSendChoice == true)
				{
					var reportResult = await new Psz.Core.Apps.Purchase.Handlers.CustomerService.GetReportHandler(
						this.GetCurrentUser(),
						new Core.Apps.Purchase.Models.CustomerService.OrderReportRequestModel
						{
							//LanguageId = 1,
							//OrderId = 1295687,
							//TypeId = 1
							LanguageId = (data?.LanguageId ?? 1),
							OrderId = (data?.OrderId ?? 1),
							TypeId = 1
						}
						).HandleAsync();
					MemoryStream stream = new MemoryStream();
					__attachments.Add(new KeyValuePair<string, Stream>($"AB-{Regex.Replace(orderEntity.Bezug?.Trim(), $"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]", "_")}_{orderEntity.Angebot_Nr}_{DateTime.Now.ToString("dd-MM-yyyy")}.pdf", new MemoryStream(reportResult.Body)));
				}
				
				var toAddresses = new List<string> { data?.ToEmail?.Trim() };
				var ccAddresses = data?.CCEmail?.Split(new char[] { ';', ',', ' ' })?.Select(x => x.Trim())?.ToList();

				if(data != null && data.CCSender.HasValue && data.CCSender.Value && string.IsNullOrWhiteSpace(currentUser.Email.Trim()))
				{
					if(ccAddresses != null && ccAddresses.Count > 0)
						ccAddresses.Add(currentUser.Email.Trim());
					else
						ccAddresses = new List<string> { currentUser.Email.Trim() };
				}
				try
				{
					if(data?.FaSendChoice == true)
					{   // Send email notification with saving logs 
						var sent = await Core.Common.Program.EmailingService.SendEmailSendGridWithStaticTemplate(
						data?.Message,
						data?.Title,
							toAddresses,
							__attachments,
							ccAddresses);

						var ebasui = sent.Item2;
						var logs = new Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity()
							{
								UserId = currentUser.Id,
								Username = currentUser.Username,
								LastUserFullName = currentUser.Name,
								LogObject = "[CTS][Orders][order-responses]",
								LogObjectId = data.OrderId ?? -1,
								LogDescription = $"| Send Ab Email Conformation | to the Order with Id [{(data.OrderId ?? -1)}]",
								SendGridId = ebasui,

							};
						var InsertLogsResult = Infrastructure.Data.Access.Tables.CTS.SendAbEmailConfirmLogs.SendAbEmailConfirmationLogsAccess.Insert(logs);
						}
					else
					{

						// Send email notification without saving logs
						var sent = await Core.Common.Program.EmailingService.SendEmailSendGridWithStaticTemplate(
						data?.Message,
						data?.Title,
							toAddresses,
							__attachments,
							ccAddresses);
					}
					// - 2025-03-14 logs
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
					{
						AngebotNr = orderEntity.Angebot_Nr,
						DateTime = DateTime.Now,
						Id=0,
						LogObject=orderEntity.Typ,
						LogText=$"[{orderEntity.Typ}] [{orderEntity.Angebot_Nr}] sent to customer [{string.Join(", ", toAddresses)}]{(ccAddresses != null && ccAddresses.Count > 0? $" AND CC to [{string.Join(", ", ccAddresses)}]":"")}",
						LogType=$"Email",
						Nr=orderEntity.Nr,
						Origin="CTS",
						PositionNr=0,
						ProjektNr=orderEntity.Angebot_Nr,
						UserId=currentUser.Id,
						Username=currentUser.Name
					});

				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", toAddresses)}]"));
					Infrastructure.Services.Logging.Logger.Log(ex);
				}

				return Ok(Core.Common.Models.ResponseModel<int>.SuccessResponse(1));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return BadRequest();
			}
		}
	}
}