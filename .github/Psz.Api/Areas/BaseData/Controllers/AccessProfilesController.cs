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
	public class AccessProfilesController: ControllerBase
	{
		private const string MODULE = "BaseData | AccessProfiles";

		#region User Profiles
		private const string ACCESS_PROFILE = "ADMIN AccessProfile";
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Core.BaseData.Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.AddHandler(this.GetCurrentUser(), data)
				   .Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEdit(Core.BaseData.Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.EditHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEditName(Core.BaseData.Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.EditNameHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileDelete(int id)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.DeleteHandler(this.GetCurrentUser(), id)
					 .Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Administration.AccessProfiles.AccessProfileModel>>), 200)]
		public IActionResult AccessProfileGetAll()
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.GetAllAccessProfilesHandler(this.GetCurrentUser())
				 .Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult AccessProfileGetUsers([FromBody] List<int> ids)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.GetUsersHandler(this.GetCurrentUser(), ids)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		// - 
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileAddUsers(Core.BaseData.Models.Administration.AccessProfiles.AddUsersModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.AddUsersHandler(this.GetCurrentUser(), data)
					 .Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileRemoveUsers(Core.BaseData.Models.Administration.AccessProfiles.AddUsersModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.RemoveUsersHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileAddToUser(Core.BaseData.Models.Administration.AccessProfiles.AddToUserModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.AddToUserHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEditForUser(Core.BaseData.Models.Administration.AccessProfiles.AddToUserModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.EditForUserHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion User Profiles

		#region Users
		private const string ADMIN_USERS = "ADMIN Users";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Administration.Users.GetModel>>), 200)]
		public IActionResult AdminUsersGetAll()
		{
			try
			{
				/*var response = 1;*/
				return Ok(new Core.BaseData.Handlers.Administration.Users.GetAllHandler(this.GetCurrentUser())
				   .Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Administration.Users.GetModel>), 200)]
		public IActionResult AdminUsersGet(int id)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.Users.GetHandler(this.GetCurrentUser(), id)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion Users

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<KeyValuePair<int, string>>), 200)]
		public IActionResult GetLagerList()
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.GetLagerListHandler(this.GetCurrentUser())
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult AccessProfileGetLager([FromBody] List<int> ids)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.GetAccessProfileLagerHandler(this.GetCurrentUser(), ids)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileAddLager(Core.BaseData.Models.Administration.AccessProfiles.AddLagerModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.AddLagerHandler(this.GetCurrentUser(), data)
					 .Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileRemoveLager(Core.BaseData.Models.Administration.AccessProfiles.AddLagerModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.BaseData.Handlers.Administration.AccessProfiles.RemoveLagerHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
	}
}