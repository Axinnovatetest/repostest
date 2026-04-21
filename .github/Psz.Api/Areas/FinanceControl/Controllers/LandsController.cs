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
	public class LandsController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult GetMyLands()
		{
			var errors = new List<string>();

			try
			{
				var connectedUser = this.GetCurrentUser();
				if(connectedUser == null)
					errors.Add("User not authorized.");

				var landsModel = new List<Psz.Core.FinanceControl.Models.Budget.GetLandsModel>();

				//var userLands = Psz.Core.Apps.Budget.Helpers.User.GetUserLands(connectedUser.Id);
				//if (userLands != null)
				//{
				//    var landsDb = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Get(userLands);

				//    Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Info, "user lands: " + landsDb.Count);
				//    landsDb.ForEach(c => landsModel.Add(new Psz.Core.FinanceControl.Models.Budget.GetLandsModel(c)));
				//}

				if(errors.Count == 0)
					return Ok(new { response = new Api.Models.Response<List<Psz.Core.FinanceControl.Models.Budget.GetLandsModel>>(true, landsModel, errors) });
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

				var landsModel = new List<Psz.Core.FinanceControl.Models.Budget.GetLandsModel>();

				//var landsDb = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Get();
				//if (landsDb == null)
				//    errors.Add("No data found");

				//if (errors.Count == 0)
				//{
				//    landsDb.ForEach(c => landsModel.Add(new Psz.Core.FinanceControl.Models.Budget.GetLandsModel(c)));
				//    return Ok(new { response = new Api.Models.Response<List<Psz.Core.FinanceControl.Models.Budget.GetLandsModel>>(true, landsModel, errors) });
				//}

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
				var land = new Psz.Core.FinanceControl.Models.Budget.GetLandsModel();
				var connectedUser = this.GetCurrentUser();

				if(connectedUser == null)
					errors.Add("User not found. Try again later.");
				else
				{
					//var landDb = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Get(Id);
					//if (landDb != null)
					//    land = new Psz.Core.FinanceControl.Models.Budget.GetLandsModel(landDb);
					//else
					//    errors.Add("land: Item not found.");
				}

				if(errors.Count == 0)
					return Ok(new { response = new Api.Models.Response<Psz.Core.FinanceControl.Models.Budget.GetLandsModel>(true, land, errors) });
				else
					return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult Add(Psz.Core.FinanceControl.Models.Budget.GetLandsModel model)
		{
			var errors = new List<string>();
			try
			{
				var addedId = 0;
				var connectedUser = this.GetCurrentUser();
				if(connectedUser != null)
				{
					if(connectedUser.Access == null || !connectedUser.Access.Financial.Budget.AssignEditLand)
						return Ok(new { response = new Api.Models.Response<string>(true, "", new List<string> { "User not authorized" }) });

					var landDb = model.ToBudgetLands();

					//Save to Db 
					//addedId = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Insert(landDb);
					//if (addedId == -1)
					//    errors.Add("Add: database error. Try again later.");
					//else
					//{
					//    return Ok(new { response = new Api.Models.Response<string>(true, addedId+"", errors) });
					//}
				}
				else
				{
					errors.Add("User not found");
				}

				if(errors.Count == 0)
				{
					Helpers.Log.NewLog(Core.Apps.Budget.Enums.LogEnums.LogType.Budget, $"{connectedUser.Username} Added new Land {model.Land_name} ", connectedUser.Id);

				}


				return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult Edit(Psz.Core.FinanceControl.Models.Budget.GetLandsModel model)
		{
			var errors = new List<string>();

			try
			{
				//Get the token ==>
				var connectedUser = this.GetCurrentUser();
				//var landDb = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Get(model.ID);

				//if (connectedUser == null)
				//    errors.Add("User not found. Try again later.");

				//if (landDb == null)
				//    errors.Add("Land not found");
				////Check the country exists
				//if (errors.Count == 0)
				//{
				//    //landDb.LastEditTime = DateTime.Now;
				//    //landDb.LastEditUserId = connectedUser.Id;
				//    landDb.Land_name = model.Land_name;
				//    //landDb.Designation = model.Designation;
				//    //save to DB
				//    Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Update(landDb);
				//    return Ok(new { response = new Api.Models.Response<string>(true, "", errors) });
				//}

				// errors
				return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult Delete(int Id)
		{
			var errors = new List<string>();

			try
			{
				var connectedUser = this.GetCurrentUser();
				if(connectedUser == null)
				{
					errors.Add("User not found. Try again later.");
				}

				//var landDb = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Get(Id);
				//if (landDb == null)
				//{
				//    errors.Add("Land not found.");
				//}

				//if (errors.Count == 0)
				//{
				//    //countryDb.ArchiveTime = DateTime.Now;
				//    //countryDb.ArchiveUserId = connectedUser.Id;
				//    //countryDb.IsArchived = true;

				//    //Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Update(landDb);


				//    Helpers.Log.NewLog(Core.Apps.Budget.Enums.LogEnums.LogType.Budget, 
				//        $"Land '{/*landDb.Land_name*/0}' edited by '{connectedUser.Username}'", 
				//        connectedUser.Id);
				//}

				return Ok(new { response = new Api.Models.Response<string>(false, "Delete: database error. Try again later.", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}