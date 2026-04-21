using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.BaseData.Interfaces.ROH;
using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.BaseData.Controllers
{
	[Authorize]
	[Area("BaseData")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ArticleConfigurationController: ControllerBase
	{
		private const string MODULE = "BaseData";
		private readonly IRohArtikelnummer _rohArtikelnummer;

		public ArticleConfigurationController(IRohArtikelnummer rohArtikelnummer)
		{
			_rohArtikelnummer = rohArtikelnummer;
		}
		#region CheckStatus
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListCheckStatuses()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.CheckStatus.GetListHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.Quality.CheckStatusModel>>), 200)]
		public IActionResult GetAllCheckStatuses()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.CheckStatus.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Configuration.Quality.CheckStatusModel>), 200)]
		public IActionResult GetCheckStatus(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.CheckStatus.GetHandler(this.GetCurrentUser(), id)
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
		public IActionResult DeleteCheckStatus(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.CheckStatus.DeleteHandler(this.GetCurrentUser(), id)
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
		public IActionResult AddCheckStatus(Core.BaseData.Models.Article.Configuration.Quality.CheckStatusModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Configuration.Quality.CheckStatus.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion

		#region ExternalStatus
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListExternalStatuses()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.ExternalStatus.GetListHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.Quality.ExternalStatusModel>>), 200)]
		public IActionResult GetAllExternalStatuses()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.ExternalStatus.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Configuration.Quality.ExternalStatusModel>), 200)]
		public IActionResult GetExternalStatus(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.ExternalStatus.GetHandler(this.GetCurrentUser(), id)
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
		public IActionResult DeleteExternalStatus(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.ExternalStatus.DeleteHandler(this.GetCurrentUser(), id)
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
		public IActionResult AddExternalStatus(Core.BaseData.Models.Article.Configuration.Quality.ExternalStatusModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Configuration.Quality.ExternalStatus.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion

		#region InternalStatus
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListInternalStatuses()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.InternalStatus.GetListHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.Quality.InternalStatusModel>>), 200)]
		public IActionResult GetAllInternalStatuses()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.InternalStatus.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Configuration.Quality.InternalStatusModel>), 200)]
		public IActionResult GetInternalStatus(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.InternalStatus.GetHandler(this.GetCurrentUser(), id)
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
		public IActionResult DeleteInternalStatus(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.InternalStatus.DeleteHandler(this.GetCurrentUser(), id)
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
		public IActionResult AddInternalStatus(Core.BaseData.Models.Article.Configuration.Quality.InternalStatusModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Configuration.Quality.InternalStatus.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion

		#region MHDTag
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListMHDTags()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.MHDTag.GetListHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.Quality.MHDTagModel>>), 200)]
		public IActionResult GetAllMHDTags()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.MHDTag.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Configuration.Quality.MHDTagModel>), 200)]
		public IActionResult GetMHDTag(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.MHDTag.GetHandler(this.GetCurrentUser(), id)
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
		public IActionResult DeleteMHDTag(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Quality.MHDTag.DeleteHandler(this.GetCurrentUser(), id)
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
		public IActionResult AddMHDTag(Core.BaseData.Models.Article.Configuration.Quality.MHDTagModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Configuration.Quality.MHDTag.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion

		#region Country ISO
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListCountryISOs()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logistics.CountryISO.GetListHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.Logistics.CountryISOModel>>), 200)]
		public IActionResult GetAllCountryISOs()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logistics.CountryISO.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Configuration.Logistics.CountryISOModel>), 200)]
		public IActionResult GetCountryISO(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logistics.CountryISO.GetHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Configuration.Logistics.CountryISOModel>), 200)]
		public IActionResult deleteCountryISO(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logistics.CountryISO.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Configuration.Logistics.CountryISOModel>), 200)]
		public IActionResult AddCountryISO(Core.BaseData.Models.Article.Configuration.Logistics.CountryISOModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logistics.CountryISO.AddHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion

		#region Country Manual
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetListCountries()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logistics.Country.GetListHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.Logistics.CountryModel>>), 200)]
		public IActionResult GetAllCountries()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logistics.Country.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Configuration.Logistics.CountryModel>), 200)]
		public IActionResult GetCountry(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logistics.Country.GetHandler(this.GetCurrentUser(), id)
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
		public IActionResult deleteCountry(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logistics.Country.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Configuration.Logistics.CountryModel>), 200)]
		public IActionResult AddCountry(Core.BaseData.Models.Article.Configuration.Logistics.CountryModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logistics.Country.AddHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Configuration.Logistics.CountryModel>), 200)]
		public IActionResult EditCountry(Core.BaseData.Models.Article.Configuration.Logistics.CountryModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logistics.Country.EditHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		#endregion

		#region Production
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListProductionPlaces()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Production.GetListProductionPlaceHandler(this.GetCurrentUser())
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
		public IActionResult GetProductionNTransfertLagers()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Production.GetProductionNTransfertLagersHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Production.ArticleTeamsResponseModel>>), 200)]
		public IActionResult GetProductionTeams()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Production.GetProductionTeamsHandler(this.GetCurrentUser())
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
		public IActionResult AddProductionTeams(Core.BaseData.Models.Article.Configuration.ArticleProductionTeamRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Production.AddProductionTeamHandler(this.GetCurrentUser(), data)
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
		public IActionResult UpdateProductionTeams(Core.BaseData.Models.Article.Configuration.ArticleProductionTeamRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Production.UpdateProductionTeamHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteProductionTeam(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Production.DeleteProductionTeamHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		// >>>>> Config Logs
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.ObjectLog.ObjectLogModel>), 200)]
		public IActionResult GetFullLog()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Logs.GetFullHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion


		#region // >>>>> Config BOM
		private const string MODULE_BOM = "BaseData | CONFIG | BOM ";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.Bom.ProductionSitesModel>>), 200)]
		public IActionResult GetProductionSites()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Bom.GetProductionSitesHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.Bom.MailUserModel>>), 200)]
		public IActionResult GetAllMailUsers()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Bom.GetAllMailUsersHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.Bom.MailUserModel>>), 200)]
		public IActionResult GetMailUsersBySite(int siteId)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Bom.GetMailUsersBySiteHandler(this.GetCurrentUser(), siteId)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddMailUser(Core.BaseData.Models.Article.Configuration.Bom.MailUserModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Bom.AddMailUserToSiteHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult RemoveMailUser(Core.BaseData.Models.Article.Configuration.Bom.MailUserRemoveModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.Bom.RemoveMailUserFromSiteHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion Config BOM

		#region Config EMail Notifications
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.EmailNotifications.GetUserModel>>), 200)]
		public IActionResult GetNotificationUsers()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.EmailNotifications.GetUserListHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddNotificationUser(Core.BaseData.Models.Article.Configuration.EmailNotifications.AddUserModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.EmailNotifications.AddUserHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult RemoveNotificationUser(Core.BaseData.Models.Article.Configuration.EmailNotifications.UpdateUserModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.EmailNotifications.RemoveUserHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion Config EMail Notifications

		#region Contact AV
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddArticleContactAV(Core.BaseData.Models.Article.Configuration.ArticleContactAVModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleContactAV.AddHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.ArticleContactAVModel>>), 200)]
		public IActionResult GetArticleContactAV()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleContactAV.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArticleContactAV(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleContactAV.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		#endregion Contact AV

		#region Employee AV
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddArticleEmployeeAV(Core.BaseData.Models.Article.Configuration.ArticleContactAVModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleEmployeeAV.AddHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.ArticleContactAVModel>>), 200)]
		public IActionResult GetArticleEmployeeAV()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleEmployeeAV.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArticleEmployeeAV(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleEmployeeAV.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		#endregion Employee AV

		#region Contact CS
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddArticleContactCS(Core.BaseData.Models.Article.Configuration.ArticleContactCSModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleContactCS.AddHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.ArticleContactCSModel>>), 200)]
		public IActionResult GetArticleContactCS()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleContactCS.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArticleContactCS(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleContactCS.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		#endregion Contact CS

		#region Contact Technic
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddArticleContactTechnic(Core.BaseData.Models.Article.Configuration.ArticleContactTechnicModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleContactTechnic.AddHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.ArticleContactTechnicModel>>), 200)]
		public IActionResult GetArticleContactTechnic()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleContactTechnic.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArticleContactTechnic(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleContactTechnic.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		#endregion Contact Technic

		#region Article Sample
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddArticleSample(Core.BaseData.Models.Article.Configuration.ArticleSampleModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleSample.AddHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.ArticleSampleModel>>), 200)]
		public IActionResult GetArticleSample()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleSample.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArticleSample(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleSample.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		#endregion Art. Sample

		#region Project Msg Price
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddArticleProjectmsgPrice(Core.BaseData.Models.Article.Configuration.ArticleProjectmsgPriceModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleProjectmsgPrice.AddHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.ArticleProjectmsgPriceModel>>), 200)]
		public IActionResult GetArticleProjectmsgPrice()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleProjectmsgPrice.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArticleProjectmsgPrice(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleProjectmsgPrice.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		#endregion Projectmsg Price

		#region Standort Master
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddArticleStandortMaster(Core.BaseData.Models.Article.Configuration.ArticleStandortMasterModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleStandortMaster.AddHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.ArticleStandortMasterModel>>), 200)]
		public IActionResult GetArticleStandortMaster()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleStandortMaster.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArticleStandortMaster(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleStandortMaster.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		#endregion StandortMaster

		#region Standort Serie
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddArticleStandortSerie(Core.BaseData.Models.Article.Configuration.ArticleStandortSerieModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleStandortSerie.AddHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Configuration.ArticleStandortSerieModel>>), 200)]
		public IActionResult GetArticleStandortSerie()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleStandortSerie.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_BOM })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArticleStandortSerie(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Configuration.ArticleStandortSerie.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		#endregion StandortSerie

		#region ROH
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<RohArtikelnummerLevel1Model>>), 200)]
		public IActionResult GetRohArtikelnummerLevel1()
		{
			try
			{
				var response = _rohArtikelnummer.GetRohArtikelnummerLevel1(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<RohArtikelnummerLevel2Model>>), 200)]
		public IActionResult GetRohArtikelnummerLevel2(int idLevelOne)
		{
			try
			{
				var response = _rohArtikelnummer.GetRohArtikelnummerLevel2(this.GetCurrentUser(), idLevelOne);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<RohArtikelnummerLevel3Model>>), 200)]
		public IActionResult GetRohArtikelnummerLevel3(int idLevelOne, int idLevelTwo)
		{
			try
			{
				var response = _rohArtikelnummer.GetRohArtikelnummerLevel3(this.GetCurrentUser(), idLevelOne, idLevelTwo);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult AddRohArtikelnummerLevel1(RohArtikelnummerLevel1Model data)
		{
			try
			{
				var response = _rohArtikelnummer.AddRohArtikelnummerLevel1(data, this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult AddRohArtikelnummerLevel2(RohArtikelnummerLevel2Model data)
		{
			try
			{
				var response = _rohArtikelnummer.AddRohArtikelnummerLevel2(data, this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult AddRohArtikelnummerLevel3(RohArtikelnummerLevel3Model data)
		{
			try
			{
				var response = _rohArtikelnummer.AddRohArtikelnummerLevel3(data, this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult UpdateRohArtikelnummerLevel1(RohArtikelnummerLevel1Model data)
		{
			try
			{
				var response = _rohArtikelnummer.UpdateRohArtikelnummerLevel1(data, this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult UpdateRohArtikelnummerLevel2(RohArtikelnummerLevel2Model data)
		{
			try
			{
				var response = _rohArtikelnummer.UpdateRohArtikelnummerLevel2(data, this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult UpdateRohArtikelnummerLevel3(RohArtikelnummerLevel3Model data)
		{
			try
			{
				var response = _rohArtikelnummer.UpdateRohArtikelnummerLevel3(data, this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult DeleteRohArtikelnummerLevel1(int id)
		{
			try
			{
				var response = _rohArtikelnummer.DeleteRohArtikelnummerLevel1(id, this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult DeleteRohArtikelnummerLevel2(int id)
		{
			try
			{
				var response = _rohArtikelnummer.DeleteRohArtikelnummerLevel2(id, this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult DeleteRohArtikelnummerLevel3(int id)
		{
			try
			{
				var response = _rohArtikelnummer.DeleteRohArtikelnummerLevel3(id, this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<ROHArtikelnummerPreviewResponseModel>), 200)]
		public IActionResult GetRohArtikelnummerPreview(RohArtikelnummerPreviewRequestModel data)
		{
			try
			{
				var response = _rohArtikelnummer.GetRohArtikelnummerPreview(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ROHPropertiesModel>>), 200)]
		public IActionResult GetRohArtikelnummerProperties(int idLevelOne)
		{
			try
			{
				var response = _rohArtikelnummer.GetROHPropertiesValues(this.GetCurrentUser(), idLevelOne);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ROHPropertiesModel>>), 200)]
		public IActionResult GetPropLevelTwoOrders(int idLevelOne)
		{
			try
			{
				var response = _rohArtikelnummer.GetPropLevelTwoOrders(this.GetCurrentUser(), idLevelOne);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ROHPropertiesModel>>), 200)]
		public IActionResult UpdatePropLevelTwoOrders(LevelTwoDescriptionOrderUpdateModel data)
		{
			try
			{
				var response = _rohArtikelnummer.UpdatePropLevelTwoOrders(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult AddROHLevelTwoRange(ROHLevelTwoRangesModel data)
		{
			try
			{
				var response = _rohArtikelnummer.AddLevelTwoRangeValue(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult UpdateROHLevelTwoRange(ROHLevelTwoRangesModel data)
		{
			try
			{
				var response = _rohArtikelnummer.UpdateLevelTwoRangeValue(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult DeleteROHLevelTwoRange(int id)
		{
			try
			{
				var response = _rohArtikelnummer.DeleteLevelTwoRangeValue(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ROHLevelTwoRangesModel>>), 200)]
		public IActionResult GetROHLevelTwoRanges(int id)
		{
			try
			{
				var response = _rohArtikelnummer.GetROHLevelTwoRangesById(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion ROH
	}
}