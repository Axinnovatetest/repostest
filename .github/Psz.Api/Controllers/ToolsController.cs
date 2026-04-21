using Infrastructure.Data.Entities.Tables.BSD;
using Infrastructure.Data.Entities.Tables.PRS;
using Infrastructure.Services.Files.Parsing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Psz.Core.Apps.Support.Interfaces;
using Psz.Core.Apps.Support.Models.Logs;
using Psz.Core.Common.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Api.Controllers
{
	[AllowAnonymous]
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ToolsController: ControllerBase
	{
		private const string MODULE = "Tools";
		private ILogParser _logParser;
		private ILogService _logService;
		private IConfiguration _configuration;
		public ToolsController(ILogParser logParser, ILogService logService, IConfiguration configuration)
		{
			_logParser = logParser;
			_logService = logService;
			_configuration = configuration;
		}

		[HttpGet]
		public IActionResult GetFile(int id)
		{
			try
			{
				var file = Psz.Core.Program.FilesManager.GetFile(id);
				if(file != null)
				{
					switch(file?.FileExtension)
					{
						case ".png":
							return new FileContentResult(file.FileBytes, "application/png")
							{
								FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
							};
						case ".jpg":
						case ".jpeg":
							return new FileContentResult(file.FileBytes, "application/jpeg")
							{
								FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
							};
						default:
							return new FileContentResult(file.FileBytes, "application/blob")
							{
								FileDownloadName = $"file-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
							};
					}
				}
				else
					return Ok();
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//GetFileFromMinio
		[HttpGet]
		public async Task<IActionResult> GetFileMinio(int id)
		{
			try
			{
				var file = await Psz.Core.Program.FilesManager.GetFileMinio(this.GetCurrentUser().Id, id);
				if(file != null)
				{
					switch(file?.FileExtension)
					{
						case ".png":
							return new FileContentResult(file.FileBytes, "application/png")
							{
								FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
							};
						case ".jpg":
						case ".jpeg":
							return new FileContentResult(file.FileBytes, "application/jpeg")
							{
								FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
							};
						default:
							return new FileContentResult(file.FileBytes, "application/blob")
							{
								FileDownloadName = $"file-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
							};
					}
				}
				else
					return Ok();
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		public async Task<IActionResult> ReSendFailedFilesFromDisk(int id)
		{
			try
			{
				var file = await Psz.Core.Program.FilesManager.ReSendFailedFilesFromDisk(id);
				if(file != null)
				{
					return Ok(true);
				}
				else
					return NotFound(false);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}

		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Infrastructure.Services.Files.GetFailedFileModel>>), 200)]
		public IActionResult GetAllFailedFiles()
		{
			try
			{
				var response = Infrastructure.Services.Files.MinIOManager.GetAllFailedFiles();
				if(response is not null)
					return Ok(Core.Common.Models.ResponseModel<List<Infrastructure.Services.Files.GetFailedFileModel>>.SuccessResponse(response));
				else
					return Ok(Core.Common.Models.ResponseModel<List<Infrastructure.Services.Files.GetFailedFileModel>>.NotFoundResponse());

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult GetAllFailedFilesCount()
		{
			try
			{
				var response = Infrastructure.Services.Files.FilesJobs.GetFilesErrorsCount();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateEmailStatus(Infrastructure.Services.Email.Models.SetEmailStatus data)
		{
			try
			{
				var response = Infrastructure.Services.Email.SendGridManager.SetEmailStatus(data.MessageId, data.EmailTo, data.EmailFrom);

				if(response > 0)
					return Ok(Core.Common.Models.ResponseModel<int>.SuccessResponse(response));
				else
					return Ok(Core.Common.Models.ResponseModel<int>.FailureResponse("Unable to Update Date"));

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Infrastructure.Services.Email.Models.PSZ_SendGrid_Email_Not_DeliveredMinimalModel>>), 200)]
		public IActionResult GetMinimalEmailStatus()
		{
			try
			{
				var response = Infrastructure.Services.Email.SendGridManager.GetMinimalPSZ_SendGrid_Email_Not_DeliveredByUser(this.GetCurrentUser().Id);
				if(response is not null)
					return Ok(Core.Common.Models.ResponseModel<List<Infrastructure.Services.Email.Models.PSZ_SendGrid_Email_Not_DeliveredMinimalModel>>.SuccessResponse(response));
				else
					return Ok(Core.Common.Models.ResponseModel<List<Infrastructure.Services.Email.Models.PSZ_SendGrid_Email_Not_DeliveredMinimalModel>>.NotFoundResponse());

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Infrastructure.Services.Email.Models.FilteredUndeliveredEmails>>), 200)]
		public IActionResult GetEmailStatus(string filter)
		{
			try
			{
				var currentuser = this.GetCurrentUser();
				var response = Infrastructure.Services.Email.SendGridManager.GetPSZ_SendGrid_Email_Not_DeliveredByUser(currentuser.Id, currentuser.Username, filter);

				if(response is not null)
					return Ok(Core.Common.Models.ResponseModel<List<Infrastructure.Services.Email.Models.FilteredUndeliveredEmails>>.SuccessResponse(response));
				else
					return Ok(Core.Common.Models.ResponseModel<List<Infrastructure.Services.Email.Models.FilteredUndeliveredEmails>>.NotFoundResponse());

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Infrastructure.Services.Email.Models.EmailToModel>>), 200)]
		public IActionResult GetEmailToFiltersByUser()
		{
			try
			{
				var response = Infrastructure.Services.Email.SendGridManager.GetPSZ_SendGrid_Email_To_FiltersByUser(this.GetCurrentUser().Id);

				if(response is not null)
					return Ok(Core.Common.Models.ResponseModel<List<Infrastructure.Services.Email.Models.EmailToModel>>.SuccessResponse(response));
				else
					return Ok(Core.Common.Models.ResponseModel<List<Infrastructure.Services.Email.Models.EmailToModel>>.NotFoundResponse());

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		public IActionResult GetProjectTemplateFile()
		{
			try
			{
				var file = new Core.FinanceControl.Handlers.Budget.Project.GetTemplateFileHandler().Handle()?.Body;

				switch(file?.FileExtension)
				{
					case ".png":
						return new FileContentResult(file.FileData, "application/png")
						{
							FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
						};
					case ".jpg":
					case ".jpeg":
						return new FileContentResult(file.FileData, "application/jpeg")
						{
							FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
						};
					default:
						return new FileContentResult(file.FileData, "application/blob")
						{
							FileDownloadName = $"{file.FileName}"
						};
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		public IActionResult GetBomImportTemplateFile()
		{
			try
			{
				var file = new Core.BaseData.Handlers.Article.BillOfMaterial.GetTemplateFileHandler().Handle()?.Body;

				switch(file?.FileExtension)
				{
					case ".png":
						return new FileContentResult(file.FileData, "application/png")
						{
							FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
						};
					case ".jpg":
					case ".jpeg":
						return new FileContentResult(file.FileData, "application/jpeg")
						{
							FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
						};
					default:
						return new FileContentResult(file.FileData, "application/blob")
						{
							FileDownloadName = $"{file.FileName}"
						};
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation()]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UploadTempFile([FromForm] Models.TempFileRequestModel data)
		{
			try
			{
				return Ok(Core.Models.ResponseModel<int>.SuccessResponse(
					Psz.Core.Common.Helpers.ImageFileHelper.NewTempFile(data.GetBytes(), data.AttachmentFileExtension)));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		public IActionResult GetTempFile(int id)
		{
			try
			{
				var file = Psz.Core.Program.FilesManager.GetTempFile(id);
				if(file != null)
				{
					switch(file?.FileExtension)
					{
						case ".png":
							return new FileContentResult(file.FileBytes, "application/png")
							{
								FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
							};
						case ".jpg":
						case ".jpeg":
							return new FileContentResult(file.FileBytes, "application/jpeg")
							{
								FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
							};
						default:
							return new FileContentResult(file.FileBytes, "application/blob")
							{
								FileDownloadName = $"file-{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.FileExtension}"
							};
					}
				}
				else
					return Ok();
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public IActionResult FrontLog(string data)
		{
			try
			{
				return Ok(data);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Infrastructure.Services.Files.Parsing.LogItem>>), 200)]
		public IActionResult GetApiCalls(bool excludeIds = true, bool file = true, string forwardEmail = "")
		{
			try
			{
				if(!string.IsNullOrEmpty(forwardEmail))
				{
					_logParser.GetExcelDataAsync(excludeIds, forwardEmail);
					return Ok(1);
				}
				else
				{
					if(file)
					{
						return new FileContentResult(_logParser.GetExcelData(excludeIds), "application/xlsx")
						{
							FileDownloadName = $"file-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx"
						};
					}
					else
					{
						return Ok(_logParser.GetLines(excludeIds));
					}
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//I

		#region Go live
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArticle_kundenarticleNumber()
		{
			try
			{
				//return Ok(updateKAN());
				return Ok(/*updateFAs()*/);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		int updateKAN()
		{
			var errorsFile = $@"C:\Projects\ERP\Codes\errors-ohneFa-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss")}.log";
			//var articles = Infrastructure.Data.Access.Joins.BSD.Migration.GetEFForCustomerArticleNumber();
			var articles = Infrastructure.Data.Access.Joins.BSD.Migration.GetEFForCustomerArticleNumber_ohneFA();
			if(articles == null || articles.Count <= 0)
				return 0;

			// - 
			var uniqueKreise = articles.Select(x => x.NummerKreise).Distinct().ToList();
			var updatedArticles = new List<Infrastructure.Data.Access.Joins.BSD.Migration.ArtikelExtendedEntity>();
			var updatedNonArticles = new List<Infrastructure.Data.Access.Joins.BSD.Migration.ArtikelExtendedEntity>();
			foreach(var kreis in uniqueKreise)
			{
				var articleKreise = articles.Where(x => x.NummerKreise == kreis).ToList();
				if(articleKreise.Count > 1)
				{
					if(articleKreise.All(x => x.Bezeichnung1 == articleKreise[0].Bezeichnung1))
					{
						continue;
					}
					else
					{
						var kundenartikel = articleKreise.FirstOrDefault(x => long.TryParse(x.Bezeichnung1, out var k) == true);
						if(kundenartikel != null)
						{
							foreach(var item in articleKreise)
							{
								updatedArticles.Add(new Infrastructure.Data.Access.Joins.BSD.Migration.ArtikelExtendedEntity
								{
									ArtikelNr = item.ArtikelNr,
									CustomerItemNumber = kundenartikel.Bezeichnung1,
									CustomerItemNumberSequence = kundenartikel.CustomerItemNumberSequence,
									ArtikelNummer = kundenartikel.ArtikelNummer
								});
							}
						}
						else
						{
							foreach(var item in articleKreise)
							{
								updatedNonArticles.Add(item);
							}
						}
					}
				}
			}

			// - 
			Infrastructure.Data.Access.Joins.BSD.Migration.EditCustomerItem(updatedArticles);

			// -
			if(updatedNonArticles.Count > 0)
			{
				System.IO.File.WriteAllLines(errorsFile, updatedNonArticles.Select(x => $"{x.ArtikelNummer}|{x.Bezeichnung1}").ToList());
			}
			// - 
			return 0;
		}
		int updateFAs()
		{
			var errorsFile = $@"C:\Projects\ERP\Codes\errors-updateFa-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss")}.log";
			int lagerId = 7;
			var lagerOpenFas = Infrastructure.Data.Access.Joins.BSD.Migration.GetOpenFaByLager(lagerId);
			if(lagerOpenFas == null || lagerOpenFas.Count <= 0)
				return 0;

			// -
			var darticleIds = lagerOpenFas.Select(x => x.Artikel_Nr ?? -999).ToList();
			var lastBomEntities = Infrastructure.Data.Access.Joins.BSD.Migration.GetLastBoms(darticleIds);
			var errors = new List<string>();

			foreach(var faItem in lagerOpenFas)
			{
				var newBom = lastBomEntities?.Where(x => x.Artikel_Nr == faItem.Artikel_Nr)?.ToList();
				if(newBom != null && newBom.Count > 0)
				{
					var fertigungPos = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByIdFertigung(faItem.ID);
					if(fertigungPos != null && fertigungPos.Count > 0)
					{
						if(!sameData(faItem.Anzahl ?? 0, newBom, fertigungPos))
						{
							try
							{
								// - delete & insert - replace
								Infrastructure.Data.Access.Joins.BSD.Migration.replacePositions(faItem.ID, newBom.Select(x => new FertigungPositionenEntity
								{
									Anzahl = faItem.Anzahl * x.Anzahl,
									Artikel_Nr = x.Artikel_Nr_des_Bauteils,
									Buchen = true,
									ID_Fertigung = faItem.ID,
									ID_Fertigung_HL = faItem.ID,
									Lagerort_ID = faItem.Lagerort_id,
									ME_gebucht = false,
									Vorgang_Nr = x.Vorgang_Nr
								}).ToList());
								errors.Add($"{faItem.Fertigungsnummer}||UPDATED.");
							} catch(Exception e)
							{
								errors.Add($"{faItem.Fertigungsnummer}||{e.Message}||{e.StackTrace}");
							}
						}
					}
				}
			}

			// -
			if(errors.Count > 0)
			{
				System.IO.File.WriteAllLines(errorsFile, errors);
			}
			// - 
			return 1;
		}
		bool sameData(int faQty, List<Stucklisten_SnapshotEntity> bom, List<FertigungPositionenEntity> faPos)
		{
			if(bom.Count != faPos.Count)
			{
				return false;
			}
			// - 1st way
			foreach(var bomItem in bom)
			{
				if(faPos.FindIndex(x => x.Artikel_Nr == bomItem.Artikel_Nr_des_Bauteils && x.Anzahl == faQty * bomItem.Anzahl) < 0)
				{
					return false;
				}
			}
			// - 2nd way
			foreach(var posItem in faPos)
			{
				if(bom.FindIndex(x => posItem.Artikel_Nr == x.Artikel_Nr_des_Bauteils && posItem.Anzahl == faQty * x.Anzahl) < 0)
				{
					return false;
				}
			}
			// - all good
			return true;
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult generateDay()
		{
			try
			{
				//return Ok(updateKAN());
				return Ok(Psz.Core.Apps.EDI.Handlers.Order.generateDay());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult generateDes(int lsNr)
		{
			try
			{
				return Ok(Psz.Core.Apps.EDI.Handlers.Order.testGenerate(lsNr));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult updatePosOrders()
		{
			try
			{
				var orderIds = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetOpen(null);
				if(orderIds?.Count > 0)
				{
					var orderPos = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByBestellungen(orderIds);
					if(orderPos?.Count > 0)
					{
						foreach(var id in orderIds)
						{
							var pos = orderPos.Where(x => x.Bestellung_Nr == id)?.ToList();
							if(pos?.Count > 0 && pos.TrueForAll(x => (x.Position ?? 0) == 0))
							{
								int i = 1;
								pos = pos.OrderBy(x => x.sortierung).ThenBy(x => x.Nr).ToList();
								pos.ForEach(x => { x.Position = ((x.Position ?? 0) + i) * 10; i++; });
								Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.UpdatePos(pos);
							}
						}
					}
				}
				return Ok(1);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		// Test Houssem Sendgrid
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public async Task<IActionResult> GetUndelivredMessagesCount()
		{
			try
			{
				var h = await Psz.Core.FinanceControl.Helpers.SendGridEmailJobs.GetUndelivredMessagesCount(this.GetCurrentUser().Id);
				return Ok(h);

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public async Task<IActionResult> UpdateSendgridEmailsNow()
		{
			try
			{
				//GetSendGridLicense
				var result = await Psz.Core.FinanceControl.Helpers.SendGridEmailJobs.UpdateMessagesStatus(Core.FinanceControl.Helpers.SendGridEmailJobs.GetSendGridLicense());

				return Ok(result);

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion Go live

		// 
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public IActionResult resendInvoices()
		{
			try
			{
				var nrs = new List<int>
				{/*1299780, 1299843, 1299903
					1299797  ,1299897/ *,,
					1299814, 1299834, 1299840, 1299857, 1299863, 1299906, 1299883, 1299783
							, 1299820, 1299877, 1299777, 1299794, 1299854, 1299846, 1299894, 1299900, 1299800, 1299837, 1299886, 1299811, 1299860
							, 1299831, 1299880, 1299817, 1299866, 1299874, 1299778, 1299849, 1299826, 1299855, 1299795, 1299889, 1299901, 1299841
							, 1299895, 1299861, 1299838, 1299881, 1299912, 1299812, 1299832, 1299818, 1299869, 1299875, 1299806, 1299892, 1299852
							, 1299898, 1299798, 1299815, 1299792, 1299858, 1299809, 1299835, 1299909, 1299829, 1299878, 1299872, 1299807, 1299853
							, 1299793, 1299893, 1299847, 1299787, 1299887, 1299836, 1299810, 1299867, 1299830, 1299824, 1299873, 1299850, 1299827
							, 1299804, 1299781, 1299844, 1299856, 1299904, 1299890, 1299896, 1299813, 1299864, 1299833, 1299907, 1299884, 1299784
							, 1299913, 1299870, 1299876, 1299821, 1299851, 1299828, 1299805, 1299782, 1299891, 1299799, 1299899, 1299845, 1299885
							, 1299816, 1299785, 1299859, 1299908, 1299879, 1299865, 1299822, 1299871, 1299802, 1299825, 1299779, 1299796, 1299808
							, 1299842, 1299902, 1299848, 1299862, 1299882, 1299888, 1299788, 1299905, 1299911, 1299819, 1299868*/
				};
				var failed = new List<string>();
				var succeeded = new List<string>();
				foreach(var nr in nrs)
				{
					try
					{
						(new Psz.Core.CustomerService.Handlers.E_Rechnung.SendRechnungEmailSubHandler(new Core.Identity.Models.UserModel { Id = -1, Username = "background-system" }, nr)
							.HandleAsync()).Wait();
						succeeded.Add($"{nr}");
					} catch { failed.Add($"{nr}"); }
				}
				if(failed.Count > 0)
				{
					System.IO.File.WriteAllLines($@"C:\inetpub\wwwroot\EITECH\LIVE\API\v1q\Logs\failed{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt", failed);
				}
				if(succeeded.Count > 0)
				{
					System.IO.File.WriteAllLines($@"C:\inetpub\wwwroot\EITECH\LIVE\API\v1q\Logs\succeeded{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt", succeeded);
				}
				return Ok();
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<FeedbackLogResponseModel>), 200)]
		public async Task<IActionResult> SaveLogsToDB(FeedbackLogRequestModel request)
		{
			try
			{
				LogParser logParser = new LogParser(this._configuration["LogFolderPath"], this._configuration.GetSection("ExcludeUserIds").Get<List<int>>(), this._configuration.GetSection("Email").Get<Infrastructure.Services.Email.EmailParamtersModel>());
				var result = await _logParser.SaveFeedbacksLogsToDBAsync([0]);

				if(result.Count > 0)
				{
					return Ok(this._logService.GetLogs(this.GetCurrentUser(), request));
				}
				else
				{
					return Ok(ResponseModel<FeedbackLogResponseModel>.SuccessResponse(new FeedbackLogResponseModel()));
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//GetFileFromMiniosourcWithByteAndExtns
		[HttpGet]
		public async Task<IActionResult> GetFileMinioSource(int id)
		{
			try
			{
				var file = await Psz.Core.Program.FilesManager.GetFileMinio(this.GetCurrentUser().Id, id);
				var extension = "";
				switch(file?.FileExtension)
				{
					case ".png":
						extension = "png";
						break;
					case ".jpg":
					case ".jpeg":
						extension = "jpg";
						break;
					default:
						extension = "";
						break;
				}
				if(file != null)
				{
					return Ok($"data:image/{extension};base64,{Convert.ToBase64String(file.FileBytes)}");
				}
				else
					return Ok();
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

	}
}