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
	public class BudgetAllocationController: ControllerBase
	{
		private const string MODULE = "FinanceControl";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Budget.Allocation.Company.UpdateModel>>), 200)]
		public IActionResult CompanyGetByUser(int? year)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.GetForCompanyByuserHandler(this.GetCurrentUser(), year).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CompanyAdd(Psz.Core.FinanceControl.Models.Budget.Allocation.Company.UpdateModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.AddForCompanyHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CompanyEdit(Psz.Core.FinanceControl.Models.Budget.Allocation.Company.UpdateModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.UpdateForCompanyHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CompanyDelete(int data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.DeleteForCompanyHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		// -Supplement
		#region Company Supplement

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Budget.Allocation.Company.SupplementUpdateModel>>), 200)]
		public IActionResult CompanySupplementGet(int data, int? year)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.Supplement.GetForCompanyByUserHandler(this.GetCurrentUser(), data, year ?? DateTime.Today.Year).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CompanySupplementAdd(Psz.Core.FinanceControl.Models.Budget.Allocation.Company.SupplementUpdateModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.Supplement.AddForCompanyHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CompanySupplementDelete(int data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.Supplement.DeleteForCompanyHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion Company Supplement


		#region Department
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Budget.Allocation.Department.UpdateModel>>), 200)]
		public IActionResult DepartmentGetByUser(int? year)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.GetForDepartmentByUserHandler(this.GetCurrentUser(), year).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DepartmentAdd(Psz.Core.FinanceControl.Models.Budget.Allocation.Department.UpdateModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.AddForDepartmentHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DepartmentEdit(Psz.Core.FinanceControl.Models.Budget.Allocation.Department.UpdateModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.UpdateForDepartmentHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DepartmentDelete(int data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.DeleteForDepartmentHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		#endregion Department


		#region User
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Budget.Allocation.User.UpdateModel>>), 200)]
		public IActionResult UserGetByUser(int? year)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.GetForUserHandler(this.GetCurrentUser(), year).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UserAdd(Psz.Core.FinanceControl.Models.Budget.Allocation.User.UpdateModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.AddForUserHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UserEdit(Psz.Core.FinanceControl.Models.Budget.Allocation.User.UpdateModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.UpdateForUserHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UserDelete(int data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.DeleteForUserHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		#endregion Department
		#region freeze/reset
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<bool>), 200)]
		public IActionResult FreezeUserBudget(int userId, int type)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.FreezeBudgetUserHandler(this.GetCurrentUser(), userId, type).Handle());

			} catch(Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult FreezeBudgetDepartement(int id, int type)
		{
			try
			{
				var response = new Psz.Core.FinanceControl.Handlers.Budget.Allocation.FreezeDepartementBudgetHandler(this.GetCurrentUser(), id, type).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult FreezeBudgetCompany(int id, int type)
		{
			try
			{
				var response = new Psz.Core.FinanceControl.Handlers.Budget.Allocation.FreezeBudgetCompanyHandler(this.GetCurrentUser(), id, type).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<bool>), 200)]
		public IActionResult FreezeAllUsers()
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.FreezeAllUsersHandler(this.GetCurrentUser()).Handle());

			} catch(Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<bool>), 200)]
		public IActionResult FreezeAllDepartments()
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.FreezeAllDepartmentsHandler(this.GetCurrentUser()).Handle());

			} catch(Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<bool>), 200)]
		public IActionResult FreezeAllCompanies()
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.FreezeAllCompaniesHandler(this.GetCurrentUser()).Handle());

			} catch(Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<bool>), 200)]
		public IActionResult ResetBudgetDepartment(int depId)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.ResetBudgetDepartmentHandler(this.GetCurrentUser(), depId).Handle());

			} catch(Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<bool>), 200)]
		public IActionResult ResetBudgetCompany(int compId)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.ResetBudgetCompanyHandler(this.GetCurrentUser(), compId).Handle());

			} catch(Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ResetBudgetByUser(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Allocation.ResetUserBudgetAllocationHandler(this.GetCurrentUser(), id).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<bool>), 200)]
		public IActionResult ResetAllUsers()
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Allocation.ResetAllUsersBudgetsHandler(this.GetCurrentUser()).Handle());

			} catch(Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ResetAllDepartements()
		{
			try
			{
				var response = new Psz.Core.FinanceControl.Handlers.Budget.Allocation.ResetAllDepartementsHandler(this.GetCurrentUser()).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ResetAllCompanies()
		{
			try
			{
				var response = new Psz.Core.FinanceControl.Handlers.Budget.Allocation.ResetAllCompaniesHandler(this.GetCurrentUser()).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
	}
}