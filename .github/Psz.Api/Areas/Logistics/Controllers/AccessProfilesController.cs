using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;


namespace Psz.Api.Areas.Logistics.Controllers
{
	[Authorize]
	[Area("Logistics")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class AccessProfilesController: Controller
	{
		private const string MODULE = "Logistics | Access Profile";


		#region User Profiles
		private const string ACCESS_PROFILE = "ADMIN AccessProfile";
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Core.Logistics.Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.AddHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Administration.AccessProfiles.AccessProfileModel>>), 200)]
		public IActionResult AccessProfileGetAll()
		{
			try
			{
				//var response = 1;
				return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.GetAllAccessProfilesHandler(this.GetCurrentUser())
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
		public IActionResult AccessProfileEditName(Core.Logistics.Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.EditNameHandler(this.GetCurrentUser(), data)
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
				return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.DeleteHandler(this.GetCurrentUser(), id)
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
		public IActionResult AccessProfileEdit(Core.Logistics.Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.EditHandler(this.GetCurrentUser(), data)
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
		public IActionResult AccessProfileAddUsers(Core.Logistics.Models.Administration.AccessProfiles.AddUsersModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.AddUsersHandler(this.GetCurrentUser(), data)
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
				return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.GetUsersHandler(this.GetCurrentUser(), ids)
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
		public IActionResult AccessProfileRemoveUsers(Core.Logistics.Models.Administration.AccessProfiles.AddUsersModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.RemoveUsersHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		private const string ADMIN_USERS = "ADMIN Users";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Administration.Users.GetModel>>), 200)]
		public IActionResult AdminUsersGetAll()
		{
			try
			{
				/*var response = 1;*/
				return Ok(new Core.Logistics.Handlers.Administration.Users.GetAllHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Logistics.Models.Administration.Users.GetModel>), 200)]
		public IActionResult AdminUsersGet(int id)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.Logistics.Handlers.Administration.Users.GetHandler(this.GetCurrentUser(), id)
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
		public IActionResult AccessProfileAddToUser(Core.Logistics.Models.Administration.AccessProfiles.AddToUserModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.AddToUserHandler(this.GetCurrentUser(), data)
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
		public IActionResult AccessProfileEditForUser(Core.Logistics.Models.Administration.AccessProfiles.AddToUserModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.EditForUserHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult Edit(Core.Logistics.Models.Administration.AccessProfiles.AccessProfileAddRequestModel data)
		//{
		//	try
		//	{
		//		return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.EditHandler(this.GetCurrentUser(), data)
		//			.Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult EditName(Core.Logistics.Models.Administration.AccessProfiles.AccessProfileAddRequestModel data)
		//{
		//	try
		//	{
		//		return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.EditNameHandler(this.GetCurrentUser(), data)
		//			.Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult AccessProfileDelete(int id)
		//{
		//	try
		//	{
		//		return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.DeleteHandler(this.GetCurrentUser(), id)
		//			 .Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Administration.AccessProfiles.AccessProfileAddRequestModel>>), 200)]
		//public IActionResult GetAll()
		//{
		//	try
		//	{
		//		return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.GetAllAccessProfilesHandler(this.GetCurrentUser())
		//		 .Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		//public IActionResult GetUsers([FromBody] List<int> ids)
		//{
		//	try
		//	{
		//		return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.GetUsersHandler(this.GetCurrentUser(), ids)
		//			.Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//// - 
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult AddUsers(Core.Logistics.Models.Administration.AccessProfiles.AddUsersRequestModel data)
		//{
		//	try
		//	{
		//		return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.AddUsersHandler(this.GetCurrentUser(), data)
		//			 .Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult RemoveUsers(Core.Logistics.Models.Administration.AccessProfiles.AddUsersRequestModel data)
		//{
		//	try
		//	{
		//		return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.RemoveUsersHandler(this.GetCurrentUser(), data)
		//			.Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult AddToUser(Core.Logistics.Models.Administration.AccessProfiles.AddToUserRequestModel data)
		//{
		//	try
		//	{
		//		return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.AddToUserHandler(this.GetCurrentUser(), data)
		//			.Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult EditForUser(Core.Logistics.Models.Administration.AccessProfiles.AddToUserRequestModel data)
		//{
		//	try
		//	{
		//		return Ok(new Core.Logistics.Handlers.Administration.AccessProfiles.EditForUserHandler(this.GetCurrentUser(), data)
		//			.Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//#endregion User Profiles

		//#region Users
		//private const string ADMIN_USERS = "ADMIN Users";
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Administration.Users.GetResponseModel>>), 200)]
		//public IActionResult UsersGetAll()
		//{
		//	try
		//	{
		//		return Ok(new Core.Logistics.Handlers.Administration.Users.GetAllHandler(this.GetCurrentUser())
		//		   .Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Logistics.Models.Administration.Users.GetResponseModel>), 200)]
		//public IActionResult UsersGet(int id)
		//{
		//	try
		//	{
		//		return Ok(new Core.Logistics.Handlers.Administration.Users.GetHandler(this.GetCurrentUser(), id)
		//			.Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		#endregion Users
	}
}
