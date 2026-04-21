using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.FinanceControl.Controllers
{
	[Authorize]
	[Area("FinanceControl")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class UserAssignController: ControllerBase
	{

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult GetMyAssignUsers()
		{
			var errors = new List<string>();

			try
			{
				var connectedUser = this.GetCurrentUser();
				if(connectedUser == null)
					errors.Add("User not authorized.");

				var usersAssignModel = new List<Core.Apps.Budget.Models.User.UserAssignModel>();

				var userAssignBD = Psz.Core.Apps.Budget.Helpers.User.GetUserAssign(connectedUser.Id);
				if(userAssignBD != null)
				{
					var userAssignDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(userAssignBD);

					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Info, "user assign: " + userAssignDb.Count);
					userAssignDb.ForEach(c => usersAssignModel.Add(new Psz.Core.Apps.Budget.Models.User.UserAssignModel(c)));
				}

				if(errors.Count == 0)
					return Ok(new { response = new Api.Models.Response<List<Psz.Core.Apps.Budget.Models.User.UserAssignModel>>(true, usersAssignModel, errors) });
				else
					return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult Get()
		{
			var errors = new List<string>();

			try
			{
				var connectedUser = this.GetCurrentUser();
				if(connectedUser == null)
					errors.Add("User not authorized.");

				var userAssignModel = new List<Psz.Core.Apps.Budget.Models.User.UserAssignModel>();

				var userAssignDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get();
				if(userAssignDb == null)
					errors.Add("No data found");

				if(errors.Count == 0)
				{
					userAssignDb.ForEach(c => userAssignModel.Add(new Psz.Core.Apps.Budget.Models.User.UserAssignModel(c)));
					return Ok(new { response = new Api.Models.Response<List<Psz.Core.Apps.Budget.Models.User.UserAssignModel>>(true, userAssignModel, errors) });
				}

				return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult Get(int Id)
		{
			var errors = new List<string>();
			try
			{
				var userAssign = new Psz.Core.Apps.Budget.Models.User.UserAssignModel();
				var connectedUser = this.GetCurrentUser();

				if(connectedUser == null)
					errors.Add("User not found. Try again later.");
				else
				{
					var userAssignBD = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(Id);
					if(userAssignBD != null)
						userAssign = new Psz.Core.Apps.Budget.Models.User.UserAssignModel(userAssignBD);
					else
						errors.Add("dept: Item not found.");
				}

				if(errors.Count == 0)
					return Ok(new { response = new Api.Models.Response<Psz.Core.Apps.Budget.Models.User.UserAssignModel>(true, userAssign, errors) });
				else
					return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult Add(Psz.Core.Apps.Budget.Models.User.UserAssignModel model)
		{
			var errors = new List<string>();
			try
			{
				//ToDo: ---
				//var addedId = 0;
				//var connectedUser = this.GetCurrentUser();
				//if (connectedUser != null)
				//{
				//    if (connectedUser.Access == null || !connectedUser.Access.Budget.AssignEditLand)
				//        return Ok(new { response = new Api.Models.Response<string>(true, "", new List<string> { "User not authorized" }) });

				//    var userAssignBD = new Infrastructure.Data.Entities.Tables.COR.UserEntity
				//    {
				//        //CreationTime = DateTime.Now,
				//        //CreationUserId = connectedUser.Id,
				//        //Designation = model.Designation,
				//        Username = model.Username,
				//        //Name = model.Name,

				//    };

				//    //Save to Db 
				//    addedId = Infrastructure.Data.Access.Tables.COR.UserAccess.Insert(userAssignBD);
				//    if (addedId == -1)
				//        errors.Add("Add: database error. Try again later.");
				//    else
				//    {
				//        return Ok(new { response = new Api.Models.Response<string>(true, addedId + "", errors) });
				//    }
				//}
				//else
				//{
				//    errors.Add("User not found");
				//}

				//if (errors.Count == 0)
				//{
				//    Helpers.Log.NewLog(Core.Apps.Budget.Enums.LogEnums.LogType.Budget, $"{connectedUser.Username } Added new Land {model.Username} ", connectedUser.Id);

				//}


				return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//public IActionResult Edit(Psz.Core.Apps.Budget.Models.User.UserAssignModel model)
		//{
		//    var errors = new List<string>();

		//    try
		//    {
		//        //Get the token ==>
		//        var connectedUser = this.GetCurrentUser();
		//        var userAssignBD = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(model.Id);

		//        if (connectedUser == null)
		//            errors.Add("User not found. Try again later.");

		//        if (userAssignBD == null)
		//            errors.Add("UserAssign not found");
		//        //Check the country exists
		//        if (errors.Count == 0)
		//        {
		//            //userAssignBD.LastEditTime = DateTime.Now;
		//            //userAssignBD.LastEditUserId = connectedUser.Id;
		//            userAssignBD.Username = model.Username;
		//            //userAssignBD.Name = model.Name;
		//            //userAssignBD.Designation = model.Designation;
		//            //save to DB
		//            Infrastructure.Data.Access.Tables.COR.UserAccess.Update(userAssignBD);
		//            return Ok(new { response = new Api.Models.Response<string>(true, "", errors) });
		//        }

		//        // errors
		//        return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
		//    }
		//    catch (Exception e)
		//    {
		//        return this.HandleException(e);
		//    }
		//}

		//[HttpGet("{id}")]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//public IActionResult Delete(int Id)
		//{
		//    var errors = new List<string>();

		//    try
		//    {
		//        var connectedUser = this.GetCurrentUser();
		//        if (connectedUser == null)
		//        {
		//            errors.Add("User not found. Try again later.");
		//        }

		//        var userAssignBD = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(Id);
		//        if (userAssignBD == null)
		//        {
		//            errors.Add("UserAssign not found.");
		//        }

		//        if (errors.Count == 0)
		//        {
		//            //countryDb.ArchiveTime = DateTime.Now;
		//            //countryDb.ArchiveUserId = connectedUser.Id;
		//            //countryDb.IsArchived = true;

		//            Infrastructure.Data.Access.Tables.COR.UserAccess.Update(userAssignBD);


		//            Helpers.Log.NewLog(Core.Apps.Budget.Enums.LogEnums.LogType.Budget,
		//                $"Land '{userAssignBD.Username}' edited by '{connectedUser.Username}'",
		//                connectedUser.Id);
		//        }

		//        return Ok(new { response = new Api.Models.Response<string>(false, "Delete: database error. Try again later.", errors) });
		//    }
		//    catch (Exception e)
		//    {
		//        return this.HandleException(e);
		//    }
		//}
	}
}