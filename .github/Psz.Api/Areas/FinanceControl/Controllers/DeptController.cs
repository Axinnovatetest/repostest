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
	public class DeptController: ControllerBase
	{

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//public IActionResult GetMyDepts()
		//{
		//    var errors = new List<string>();

		//    try
		//    {
		//        var connectedUser = this.GetCurrentUser();
		//        if (connectedUser == null)
		//            errors.Add("User not authorized.");

		//        var deptsModel = new List<Psz.Core.FinanceControl.Models.Budget.Department.GetModel>();

		//        var userDepts = Psz.Core.Apps.Budget.Helpers.User.GetUserDepts(connectedUser.Id);
		//        if (userDepts != null)
		//        {
		//            var deptsDb = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(userDepts?.Cast<long>().ToList());

		//            Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Info, "user depts: " + deptsDb.Count);
		//            deptsDb.ForEach(c => deptsModel.Add(new Psz.Core.FinanceControl.Models.Budget.Department.GetModel(c)));
		//        }

		//        if (errors.Count == 0)
		//            return Ok(new { response = new Api.Models.Response<List<Psz.Core.FinanceControl.Models.Budget.Department.GetModel>>(true, deptsModel, errors) });
		//        else
		//            return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
		//    }
		//    catch (Exception e)
		//    {
		//        return this.HandleException(e);
		//    }
		//}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult GetMyDeptsJointedbyLand(int idLand)
		{
			var errors = new List<string>();

			try
			{
				var connectedUser = this.GetCurrentUser();
				if(connectedUser == null)
					errors.Add("User not authorized.");

				var deptsModel = new List<Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandModel>();

				//var userDepts = Psz.Core.Apps.Budget.Helpers.User.GetUserDepts(connectedUser.Id);
				//if (userDepts != null)
				//{
				//    //var deptsDb = Infrastructure.Data.Access.Tables.FNC.Land_Department_JointAccess.GetDeptJointID(userDepts,idLand);

				//    //deptsDb.ForEach(c => deptsModel.Add(new Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandModel(c)));
				//}

				if(errors.Count == 0)
					return Ok(new { response = new Api.Models.Response<List<Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandModel>>(true, deptsModel, errors) });
				else
					return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{LandId}")]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult GetByDeptJointByLandd(int LandId)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				var deptJointViewModel = new List<Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandModel>();

				if(user == null)
				{
					errors.Add("Authentification failed");
				}
				else
				{
					//var deptJointDb = Infrastructure.Data.Access.Tables.FNC.Land_Department_JointAccess.GetbyLand(LandId);

					//if (deptJointDb == null)
					//{
					//    errors.Add("Error Db Can't get Department");
					//}
					//else
					//{
					//    deptJointDb.ForEach(h => deptJointViewModel.Add(new Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandModel(h)));
					//}
				}

				if(errors.Count == 0)
				{
					return Ok(new { response = new Api.Models.Response<List<Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandModel>>() { Success = true, ResponseBody = deptJointViewModel, Errors = errors } });
				}
				else
				{
					return Ok(new { response = new Api.Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//public IActionResult Get()
		//{
		//    var errors = new List<string>();

		//    try
		//    {
		//        var connectedUser = this.GetCurrentUser();
		//        if (connectedUser == null)
		//            errors.Add("User not authorized.");

		//        var deptsModel = new List<Psz.Core.FinanceControl.Models.Budget.Department.GetModel>();

		//        var deptsDb = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get();
		//        if (deptsDb == null)
		//            errors.Add("No data found");

		//        if (errors.Count == 0)
		//        {
		//            deptsDb.ForEach(c => deptsModel.Add(new Psz.Core.FinanceControl.Models.Budget.Department.GetModel(c)));
		//            return Ok(new { response = new Api.Models.Response<List<Psz.Core.FinanceControl.Models.Budget.Department.GetModel>>(true, deptsModel, errors) });
		//        }

		//        return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
		//    }
		//    catch (Exception e)
		//    {
		//        return this.HandleException(e);
		//    }
		//}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult GetJointed()
		{
			var errors = new List<string>();

			try
			{
				var connectedUser = this.GetCurrentUser();
				if(connectedUser == null)
					errors.Add("User not authorized.");

				var deptsModel = new List<Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandConcatModel>();

				//var deptsDb = Infrastructure.Data.Access.Tables.FNC.Land_Department_JointAccess.GetAllDataJointDeptLand();
				//if (deptsDb == null)
				//    errors.Add("No data found");

				//if (errors.Count == 0)
				//{
				//    deptsDb.ForEach(c => deptsModel.Add(new Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandConcatModel(c)));
				//    return Ok(new { response = new Api.Models.Response<List<Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandConcatModel>>(true, deptsModel, errors) });
				//}

				return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		/*[HttpGet]
        [SwaggerOperation(Tags = new[] { "FinanceControl" })]
        public IActionResult GetJointed()
        {
            var errors = new List<string>();

            try
            {
                var connectedUser = this.GetCurrentUser();
                if (connectedUser == null)
                    errors.Add("User not authorized.");

                var deptsModel = new List<Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandModel>();

                var deptsDb = Infrastructure.Data.Access.Tables.FNC.Land_Department_JointAccess.GetAllDataJointDeptLand();
                if (deptsDb == null)
                    errors.Add("No data found");

                if (errors.Count == 0)
                {
                    deptsDb.ForEach(c => deptsModel.Add(new Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandModel(c)));
                    return Ok(new { response = new Api.Models.Response<List<Psz.Core.FinanceControl.Models.Budget.AllDataDeptJointLandModel>>(true, deptsModel, errors) });
                }

                return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
            }
            catch (Exception e)
            {
                return this.HandleException(e);
            }
        }*/

		//[HttpGet("{id}")]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//public IActionResult Get(int Id)
		//{
		//    var errors = new List<string>();
		//    try
		//    {
		//        var dept = new Psz.Core.FinanceControl.Models.Budget.GetDepartementsModel();
		//        var connectedUser = this.GetCurrentUser();

		//        if (connectedUser == null)
		//            errors.Add("User not found. Try again later.");
		//        else
		//        {
		//            var deptDb = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(Id);
		//            if (deptDb != null)
		//                dept = new Psz.Core.FinanceControl.Models.Budget.GetDepartementsModel(deptDb);
		//            else
		//                errors.Add("dept: Item not found.");
		//        }

		//        if (errors.Count == 0)
		//            return Ok(new { response = new Api.Models.Response<Psz.Core.FinanceControl.Models.Budget.GetDepartementsModel>(true, dept, errors) });
		//        else
		//            return Ok(new { response = new Api.Models.Response<string>(false, "", errors) });
		//    }
		//    catch (Exception e)
		//    {
		//        return this.HandleException(e);
		//    }
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//public IActionResult Add(Psz.Core.FinanceControl.Models.Budget.Department.GetModel model)
		//{
		//    var errors = new List<string>();
		//    try
		//    {


		//        return Ok(Core.Apps.Settings.Handlers.Department.GetAllHandler());
		//    }
		//    catch (Exception e)
		//    {
		//        return this.HandleException(e);
		//    }
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//public IActionResult Edit(Psz.Core.FinanceControl.Models.Budget.GetDepartementsModel model)
		//{
		//    var errors = new List<string>();

		//    try
		//    {
		//        //Get the token ==>
		//        var connectedUser = this.GetCurrentUser();
		//        var deptDb = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(model.ID);

		//        if (connectedUser == null)
		//            errors.Add("User not found. Try again later.");

		//        if (deptDb == null)
		//            errors.Add("Dept not found");
		//        //Check the country exists
		//        if (errors.Count == 0)
		//        {
		//            //deptDb.LastEditTime = DateTime.Now;
		//            //deptDb.LastEditUserId = connectedUser.Id;
		//            deptDb.Departement_name = model.Departement_name;
		//            //deptDb.Designation = model.Designation;
		//            //save to DB
		//            Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Update(deptDb);
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

		//        var deptDb = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(Id);
		//        if (deptDb == null)
		//        {
		//            errors.Add("Dept not found.");
		//        }

		//        if (errors.Count == 0)
		//        {
		//            //countryDb.ArchiveTime = DateTime.Now;
		//            //countryDb.ArchiveUserId = connectedUser.Id;
		//            //countryDb.IsArchived = true;

		//            Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Update(deptDb);


		//            Helpers.Log.NewLog(Core.Apps.Budget.Enums.LogEnums.LogType.Budget,
		//                $"Land '{deptDb.Departement_name}' edited by '{connectedUser.Username}'",
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