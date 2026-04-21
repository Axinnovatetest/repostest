using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.BaseData.Controllers
{
	[Authorize]
	[Area("BaseData")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ArticleBOMController: ControllerBase
	{
		private const string MODULE = "BaseData";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetArticleCandidates(int id, string searchNummer, int? maxItemsNumber)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.GetCandidateArticlesHandler(
					this.GetCurrentUser(), id, searchNummer, maxItemsNumber)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetArticleROHList()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.GetListROHHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.Bom>>), 200)]
		public IActionResult GetArticleBOM(int id)
		{
			try
			{
				//return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.GetHandler(this.GetCurrentUser(), id)
				//    .Handle());

				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.GetArticleBomHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateBomStatus(Core.BaseData.Models.Article.BillOfMaterial.BOMStatusDetailsModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateBOMStatus");
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.UpdateHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListStatus()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.GetListBomStatusHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.VersionListModel>>), 200)]
		public IActionResult GetValidatedVersionList(int articleId)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.GetValidatedVersionListHandler(this.GetCurrentUser(), articleId)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}

		// POSITIONS -------------------------------------------------
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.BomPosition>>), 200)]
		public IActionResult GetPositionsByArticleId(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.Position.GetByArticleIdHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.BillOfMaterial.BomPosition>), 200)]
		public IActionResult GetPosition(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.Position.GetHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//new BOM Process
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.BillOfMaterial.BomPosition>), 200)]
		public IActionResult GetBOMLIst(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.GetPositionsHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeletePosition(int id)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article DeleteBOMPos");
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.Position.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddPosition([FromForm] Models.Articles.Bom.PositionEdit data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article AddPos");
				var response = new Core.BaseData.Handlers.Article.BillOfMaterial.Position.AddHandler(this.GetCurrentUser(), data.ToBusinessModel())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdatePosition([FromForm] Models.Articles.Bom.PositionEdit data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateBOM");
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.Position.UpdateHandler(this.GetCurrentUser(), data.ToBusinessModel()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult ImportFromXLS([FromForm] Models.Articles.Bom.PositionImportXLSModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article ImportBOM");
				// AllowAnonymous <<<<<<<
				var user = this.GetCurrentUser();
				if(user == null)
				{
					return Ok("Authentication: User not found");
				}

				var file = data.AttachmentFile;
				if(file == null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "File to upload is null.");
					return BadRequest("No file sent.");
				}

				if(file.Length > 0)
				{
					// Save file to temp dir
					var tempFilePath = System.IO.Path.GetTempPath();
					var filePath = System.IO.Path.Combine(tempFilePath, DateTime.Now.ToString("yyyyMMddTHHmmss_") + file.FileName);

					var fileName = System.IO.Path.GetFileName(file.FileName);
					if(!System.IO.Directory.Exists(tempFilePath))
					{
						System.IO.Directory.CreateDirectory(tempFilePath);
					}

					using(var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					return Ok(
						new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.Position.ImportFromXLSHandler(user,
						new Core.BaseData.Models.Article.BillOfMaterial.ImportPositionXLSRequestModel
						{
							ArticleId = data.ArticleId,
							AttachmentFilePath = filePath,
							Overwrite = data.Overwrite ?? false,
							// UpgradeBomVersion = data.UpgradeBomVersion  // - 2022 - 07 - 25 - deprecated
						}).Handle());
				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult DownloadPositions(Core.BaseData.Models.Article.BillOfMaterial.ExportAsXLSModel data)
		{
			try
			{
				var result = new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.Position.ExportAsXLSHandler(this.GetCurrentUser(), data).Handle();
				if(result != null && result.Success)
					return File(result.Body, "application/xlsx", $"BOM-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				else
					return Ok(result);

			} catch(Exception e)
			{
				this.HandleException(e, data);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult DownloadTemplateXLS()
		{
			try
			{
				var result = new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.Position.GetTemplateXLSHandler(this.GetCurrentUser()).Handle();
				if(result != null && result.Success)
					return File(result.Body, "application/xlsx", $"Template-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				else
					return Ok(result);

			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		// POSITIONS ALTERNATIVE *********************
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.BomPositionAlt>>), 200)]
		public IActionResult GetPositionAltsByPositionId(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.PositionAlt.GetByPositionIdHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.BillOfMaterial.BomPositionAlt>), 200)]
		public IActionResult GetPositionAlt(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.PositionAlt.GetHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeletePositionAlt(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.PositionAlt.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddPositionAlt([FromForm] Models.Articles.Bom.PositionAltEdit data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.BillOfMaterial.PositionAlt.AddHandler(this.GetCurrentUser(), data.ToBusinessModel())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//check article exsistance
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<bool>), 200)]
		public IActionResult CheckArticleExsistance(string ArticleNumber)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.BillOfMaterial.CheckArticleExsistanceHandler(this.GetCurrentUser(), ArticleNumber)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdatePositionAlt([FromForm] Models.Articles.Bom.PositionAltEdit data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.PositionAlt.UpdateHandler(this.GetCurrentUser(), data.ToBusinessModel()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GetBOMLog(int ArticleID)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.GetBOMLogHandler(this.GetCurrentUser(), ArticleID)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Attachement
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateBOMAttachement([FromForm] Models.Articles.UpdateAttachmentModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.UpdateBOMAttachementHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//upper part of BOM page
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.BillOfMaterial.BOMStatusDetailsModel>), 200)]
		public IActionResult GetBOMStatusDetails(int ArticleID)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.GetBOMStatusDetails(this.GetCurrentUser(), ArticleID)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.BomPosition>>), 200)]
		public IActionResult HandleBOMChanges([FromBody] Core.BaseData.Models.Article.BillOfMaterial.ImportRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article BOMChanges");
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.ApplyChangesHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.ValidateBomResponseModel>>), 200)]
		public IActionResult ValidateBOM(Core.BaseData.Models.Article.BillOfMaterial.ValidateWPartialDocRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article ValidateBOM");
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.ValidateWithPartialDocHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.ValidateBomResponseWithBcrModel>>), 200)]
		public IActionResult ValidateBOMWithBCR(Core.BaseData.Models.Article.BillOfMaterial.ValidateBomWBcrPartialDocRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article ValidateBOM");
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.ValidateBomWithPartialDocAndBCRHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpgradeFABom(Core.BaseData.Models.Article.BillOfMaterial.UpgradeFABomRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpgradeFABOM");
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.UpgradeFABomHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>), 200)]
		public IActionResult GetArticleList_BOMControl(Core.BaseData.Models.Article.BillOfMaterial.VersionControl.GetArticleListRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.VersionControl.GetArticleListHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>), 200)]
		public IActionResult GetArticleListEng_BOMControl()
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.VersionControl.GetArticleListEngineeringHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GetArticleListEng_BOMControlCount()
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.VersionControl.GetArticleListEngineeringCountHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>), 200)]
		public IActionResult GetArticleListQlt_BOMControl()
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.VersionControl.GetArticleListQualityHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GetArticleListQlt_BOMControlCount()
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.VersionControl.GetArticleListQualityCountHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>), 200)]
		public IActionResult ToggleStatus_BOMControl(Core.BaseData.Models.Article.BillOfMaterial.VersionControl.ToggleStatusRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.VersionControl.ToggleStatusHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleSearchResponseModel>>), 200)]
		public IActionResult Search(Core.BaseData.Models.Article.ArticleSearchModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.BillOfMaterial.SearchArticleHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.BomHistoryResponseModel>>), 200)]
		public IActionResult GetHistory(int articleId)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.BillOfMaterial.GetHistoryHandler(this.GetCurrentUser(), articleId)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		// - 2022-07-13 -  Partial Validation
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.BillOfMaterial.CanPartialValidationModel>), 200)]
		public IActionResult CanPartialValidation(int articleId)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.CanPartialValidationHandler(this.GetCurrentUser(), articleId)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		// - 2022-09-21 
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.BillOfMaterial.UBGParentModel>>), 200)]
		public IActionResult GetUBGParents(int articleId)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.BillOfMaterial.GetUBGParentsHandler(this.GetCurrentUser(), articleId)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		// - 2022-06-20 -  Check Validation
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.BillOfMaterial.CanPartialValidationModel>), 200)]
		public IActionResult IsBomValid(int articleId)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.BillOfMaterial.IsBomValidatedHandler(this.GetCurrentUser(), articleId)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}

		// - 2024-03-04
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ToggleEDrawing(int articleId)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article ToggleEDrawing");
				return Ok(new Core.BaseData.Handlers.Article.BillOfMaterial.ToggleEDrawingHandler(this.GetCurrentUser(), articleId).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, articleId);
			}
		}
	}
}
