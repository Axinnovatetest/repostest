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
	public class DepartmentController: ControllerBase
	{
		private const string MODULE = "FinanceControl";


		//Api Joint dept land

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Department.GetModel>), 200)]
		//public IActionResult GetAllJointedDept()
		//{
		//    try
		//    {
		//        var response = new Core.FinanceControl.Handlers.Budget.Department.GetAllHandler(this.GetCurrentUser()).Handle();

		//        return Ok(response);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        return this.HandleException(e);
		//    }
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Department.GetModel>), 200)]
		//public IActionResult GetJointedDeptByLand(int Land)
		//{
		//    try
		//    {
		//        var response = new Core.FinanceControl.Handlers.Budget.Department.GetByLandHandler(this.GetCurrentUser(), Land).Handle();

		//        return Ok(response);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        return this.HandleException(e);
		//    }
		//}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataDeptJointLandModel>>), 200)]
		public IActionResult GetAllDataDeptJointLand()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAllDataDeptJointLandHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetDeptJointLandModel>>), 200)]
		public IActionResult CreateDeptJointLand(Core.FinanceControl.Models.Budget.GetDeptJointLandModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CreateDeptJointLandHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateDeptJointLand(Core.FinanceControl.Models.Budget.GetDeptJointLandModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.UpdateDeptJointLandHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteDeptJointLand(int data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.DeleteDeptJointLandHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
	}
}