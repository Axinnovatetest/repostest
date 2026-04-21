using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Settings.Controllers
{
	[Authorize]
	[Area("Settings")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class AccessProfilesController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Apps.Settings.Models.AccessProfiles.AccessProfileModel>), 200)]
		public IActionResult Delete(int id)
		{
			try
			{
				var response = Core.Apps.Settings.Handlers.AccessProfiles.Delete(id, this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Apps.Settings.Models.AccessProfiles.AccessProfileModel>), 200)]
		public IActionResult Get(int id)
		{
			try
			{
				var response = Core.Apps.Settings.Handlers.AccessProfiles.Get(id, this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Apps.Settings.Models.AccessProfiles.AccessProfileModel>>), 200)]
		public IActionResult GetAllBudget()
		{
			try
			{
				var response = Core.Identity.Handlers.AccessProfile.GetBudget(this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Identity.Models.AccessProfileModel>>), 200)]
		public IActionResult GetAll()
		{
			try
			{
				var response = Core.Identity.Handlers.AccessProfile.Get(this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Identity.Models.AccessProfileMinimalModel>>), 200)]
		public IActionResult GetAllMinimal()
		{
			try
			{
				var response = Core.Identity.Handlers.AccessProfile.GetMinimal(this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult UpdateMinimal(Core.Identity.Models.AccessProfileMinimalModel data)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.AccessProfiles.UpdateMinimal(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult Create(Core.Apps.Settings.Models.AccessProfiles.CreationModel data)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.AccessProfiles.Create(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult Update(Core.Apps.Settings.Models.AccessProfiles.UpdateModel data)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.AccessProfiles.Update(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetKeyValues()
		{
			try
			{
				var response = Core.Apps.Settings.Handlers.AccessProfiles.GetKeyValues(this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetUsers([FromBody] List<int> data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Info, " posted data >> " + string.Join(", ", data));

				var response = Core.Apps.Settings.Handlers.Users.GetByAccessProfiles(data, this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Identity.Models.AccessProfileModel>), 200)]
		public IActionResult GetAccessRights(int? id)
		{
			try
			{
				var response = id.HasValue
					? Core.Identity.Handlers.AccessProfile.Get(id.Value, this.GetCurrentUser())
					: new Psz.Core.Common.Models.ResponseModel<Core.Identity.Models.AccessProfileModel>()
					{
						Body = null
					};

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult UpdateWPL(Core.Apps.Settings.Models.AccessProfiles.UpdateModel data)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.AccessProfiles.UpdateWPL(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult CreateWPL(Core.Apps.Settings.Models.AccessProfiles.CreationModel data)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.AccessProfiles.CreateWPL(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult UpdateBudget(Core.Apps.Settings.Models.AccessProfiles.UpdateModel data)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.AccessProfiles.UpdateBudget(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult CreateBudget(Core.Apps.Settings.Models.AccessProfiles.CreationModel data)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.AccessProfiles.CreateBudget(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//[AllowAnonymous]
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "Settings" })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult Delete(int id)
		//{
		//    try
		//    {
		//        var response = new Core.Apps.Settings.Handlers.AccessProfiles.DeleteProfileHandler(id, this.GetCurrentUser())
		//           .Handle();

		//        return Ok(response);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        return this.HandleException(e);
		//    }
		//}
	}
}